using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HabitDisc.Helpers;
using HabitDisc.Services;

namespace HabitDisc.Telas
{
    // Tela inicial (UC02): os hábitos do dia, o check-in de cada um e o
    // progresso do dia. Os dados vêm do banco pelos Services.
    public partial class HomeControl : TelaBase
    {
        private readonly HabitoService _habitoService = new HabitoService();
        private readonly CheckinService _checkinService = new CheckinService();

        private double _fracaoProgresso = 0;

        // Hábitos exibidos agora (null até a tela ser carregada pela primeira vez).
        // Ficam guardados para remontar as linhas quando o tema muda, sem ir ao banco.
        private List<HabitoResumo>? _habitosAtuais;

        // true enquanto um check-in que falhou é desfeito na tela
        // (evita que o CheckedChanged tente gravar de novo)
        private bool _desfazendo;

        public HomeControl()
        {
            InitializeComponent();
            ListaVertical.Configurar(flpHabitos);
            AplicarTema();
        }

        // ---------- Eventos ----------

        // A tela se atualiza sozinha sempre que ficar visível
        // (o MainForm só precisa mostrá-la)
        private void HomeControl_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible && !DesignMode)
            {
                AtualizarTela();
            }
        }

        private void pnlBarraFundo_Resize(object sender, EventArgs e)
        {
            AtualizarBarra();
        }

        // ---------- Tema ----------

        // Pinta os controles fixos com as cores/fontes atuais.
        // As alturas são calculadas pela altura real da fonte (Font.Height), e não
        // fixas em pixels: assim o texto cabe em qualquer tamanho de fonte e também
        // quando o Windows está com escala de tela maior que 100%.
        protected override void AplicarTema()
        {
            BackColor = Tema.Fundo;

            // Topo: título e data, um embaixo do outro
            lblTitulo.Font = Tema.Fonte(28f, FontStyle.Bold);
            lblTitulo.ForeColor = Tema.Titulo;
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Size = new Size(Tema.Escalar(400), lblTitulo.Font.Height + 6);

            lblData.Font = Tema.Fonte(13f);
            lblData.ForeColor = Tema.Texto;
            lblData.Location = new Point(2, lblTitulo.Bottom);
            lblData.Size = new Size(Tema.Escalar(500), lblData.Font.Height + 4);

            pnlTopo.Height = lblData.Bottom + Tema.Escalar(10);

            // Cartão de progresso: texto e percentual na mesma linha, barra embaixo
            pnlProgresso.BackColor = Tema.Menu;

            lblProgresso.Font = Tema.Fonte(15f, FontStyle.Bold);
            lblProgresso.ForeColor = Tema.TextoSobreMenu;
            lblProgresso.Location = new Point(22, Tema.Escalar(16));
            lblProgresso.Height = lblProgresso.Font.Height + 6;
            // Se não couber, termina com "..." em vez de cortar a frase no meio
            lblProgresso.AutoEllipsis = true;

            lblPercentual.Font = Tema.Fonte(22f, FontStyle.Bold);
            lblPercentual.ForeColor = Tema.Destaque;
            // Largura suficiente para o maior texto possível ("100%")
            int larguraPercentual = TextRenderer.MeasureText("100%", lblPercentual.Font).Width + 8;
            int alturaPercentual = lblPercentual.Font.Height + 4;
            lblPercentual.Size = new Size(larguraPercentual, alturaPercentual);
            lblPercentual.Location = new Point(
                pnlProgresso.ClientSize.Width - 22 - larguraPercentual,
                lblProgresso.Top + (lblProgresso.Height - alturaPercentual) / 2);

            // O texto vai só até o percentual: como o fundo dele é transparente,
            // se passasse por cima, apagaria o "33%". Ancorado dos dois lados,
            // acompanha a largura do painel.
            lblProgresso.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblProgresso.Width = lblPercentual.Left - lblProgresso.Left - 8;

            pnlBarraFundo.BackColor = Tema.MenuSelecionado;
            pnlBarraFundo.Top = Math.Max(lblProgresso.Bottom, lblPercentual.Bottom) + Tema.Escalar(8);
            pnlBarraFundo.Height = Tema.Escalar(14);
            pnlBarraPreenchida.BackColor = Tema.Destaque;

            pnlProgresso.Height = pnlBarraFundo.Bottom + Tema.Escalar(22);

            lblHabitosHoje.Font = Tema.Fonte(16f, FontStyle.Bold);
            lblHabitosHoje.ForeColor = Tema.Titulo;
            lblHabitosHoje.Height = lblHabitosHoje.Font.Height + Tema.Escalar(30);

            lblVazio.Font = Tema.Fonte(13f);
            lblVazio.ForeColor = Tema.TextoSuave;

            // As linhas são criadas por código: remonta com as novas cores/fontes
            if (_habitosAtuais != null)
            {
                MontarLinhas();
            }
        }

        // ---------- Carregamento ----------

        private void AtualizarTela()
        {
            lblData.Text = Textos.DataPorExtenso(DateTime.Now);

            // Todos os hábitos são diários: a lista do dia é a lista de todos
            _habitosAtuais = _habitoService.Listar();
            MontarLinhas();
        }

        // Recria as linhas a partir de _habitosAtuais (sem ir ao banco)
        private void MontarLinhas()
        {
            List<HabitoResumo> habitos = _habitosAtuais ?? new List<HabitoResumo>();

            ListaVertical.Limpar(flpHabitos);

            flpHabitos.SuspendLayout();
            foreach (HabitoResumo h in habitos)
            {
                flpHabitos.Controls.Add(CriarLinha(h));
            }
            flpHabitos.ResumeLayout();

            bool vazio = habitos.Count == 0;
            flpHabitos.Visible = !vazio;
            lblVazio.Visible = vazio;

            AtualizarProgresso();
        }

        // ---------- Montagem de cada linha ----------

        private Panel CriarLinha(HabitoResumo h)
        {
            Font fonteNome = FonteNome(h.Concluido);
            Font fonteDetalhe = Tema.Fonte(11f);

            // Alturas calculadas pela fonte, para o texto caber em qualquer tamanho
            int margem = Tema.Escalar(9);
            int alturaNome = fonteNome.Height + 4;
            int alturaDetalhe = fonteDetalhe.Height + 2;
            int recuoTexto = Tema.Escalar(66);

            Panel linha = new Panel();
            linha.Height = margem + alturaNome + alturaDetalhe + margem;
            linha.Width = flpHabitos.ClientSize.Width;
            linha.BackColor = Tema.Superficie;
            linha.Margin = new Padding(0, 0, 0, 10);
            linha.Cursor = Cursors.Hand;
            linha.Tag = h.Id;

            Panel faixa = new Panel();
            faixa.Dock = DockStyle.Left;
            faixa.Width = 6;
            faixa.BackColor = CorHelper.HexParaCor(h.Cor);

            CheckBox chk = new CheckBox();
            chk.AutoSize = false;
            chk.Size = new Size(Tema.Escalar(24), Tema.Escalar(24));
            chk.Location = new Point(Tema.Escalar(26), (linha.Height - chk.Height) / 2);
            chk.Cursor = Cursors.Hand;
            chk.Checked = h.Concluido;

            Label lblNome = new Label();
            lblNome.AutoSize = false;
            lblNome.AutoEllipsis = true;
            lblNome.Location = new Point(recuoTexto, margem);
            lblNome.Size = new Size(linha.Width - recuoTexto - 16, alturaNome);
            lblNome.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblNome.Text = h.Nome;
            lblNome.Font = fonteNome;
            lblNome.ForeColor = CorNome(h.Concluido);

            Label lblDetalhe = new Label();
            lblDetalhe.AutoSize = false;
            lblDetalhe.AutoEllipsis = true;
            lblDetalhe.Location = new Point(recuoTexto, margem + alturaNome);
            lblDetalhe.Size = new Size(linha.Width - recuoTexto - 16, alturaDetalhe);
            lblDetalhe.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblDetalhe.Font = fonteDetalhe;
            lblDetalhe.ForeColor = Tema.Texto;
            lblDetalhe.Text = TextoDetalhe(h);

            linha.Controls.Add(faixa);
            linha.Controls.Add(chk);
            linha.Controls.Add(lblNome);
            linha.Controls.Add(lblDetalhe);

            // Clicar em qualquer parte da linha também marca/desmarca (RNF02: 1 clique)
            EventHandler alternar = delegate { chk.Checked = !chk.Checked; };
            linha.Click += alternar;
            lblNome.Click += alternar;
            lblDetalhe.Click += alternar;

            chk.CheckedChanged += delegate { AoAlterarCheckin(chk, lblNome, lblDetalhe, h); };

            return linha;
        }

        private void AoAlterarCheckin(CheckBox chk, Label lblNome, Label lblDetalhe, HabitoResumo h)
        {
            if (_desfazendo)
            {
                return;
            }

            try
            {
                // Grava (ou desmarca) o check-in de hoje e recebe o streak atualizado
                h.Streak = _checkinService.DefinirCheckin(h.Id, chk.Checked);
            }
            catch (Exception ex)
            {
                // Não gravou (ex.: banco fora do ar): volta a caixinha como estava,
                // para a tela não mostrar algo que não está salvo
                _desfazendo = true;
                chk.Checked = !chk.Checked;
                _desfazendo = false;

                Mensagem.ErroInesperado(ex);
                return;
            }

            h.Concluido = chk.Checked;

            lblNome.Font = FonteNome(h.Concluido);
            lblNome.ForeColor = CorNome(h.Concluido);
            lblDetalhe.Text = TextoDetalhe(h);

            AtualizarProgresso();
        }

        // Concluído: nome riscado e apagado
        private static Font FonteNome(bool concluido)
        {
            return concluido
                ? Tema.Fonte(14f, FontStyle.Bold | FontStyle.Strikeout)
                : Tema.Fonte(14f, FontStyle.Bold);
        }

        private static Color CorNome(bool concluido)
        {
            return concluido ? Tema.TextoSuave : Tema.Titulo;
        }

        private static string TextoDetalhe(HabitoResumo h)
        {
            return h.Categoria + "  ·  " + Textos.Streak(h.Streak);
        }

        // ---------- Progresso ----------

        private void AtualizarProgresso()
        {
            List<HabitoResumo> habitos = _habitosAtuais ?? new List<HabitoResumo>();
            int total = habitos.Count;
            int feitos = habitos.Count(h => h.Concluido);

            if (total == 0)
            {
                lblProgresso.Text = "Nenhum hábito para hoje";
                lblPercentual.Text = "0%";
                _fracaoProgresso = 0;
            }
            else
            {
                string sufixo = total == 1 ? "hábito concluído" : "hábitos concluídos";
                lblProgresso.Text = feitos + " de " + total + " " + sufixo;
                lblPercentual.Text = (feitos * 100 / total) + "%";
                _fracaoProgresso = (double)feitos / total;
            }

            AtualizarBarra();
        }

        private void AtualizarBarra()
        {
            pnlBarraPreenchida.Width = (int)(pnlBarraFundo.Width * _fracaoProgresso);
        }
    }
}
