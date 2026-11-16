using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using HabitDisc.Helpers;

namespace HabitDisc.Telas
{
    public partial class HomeControl : UserControl
    {
        // Guarda os CheckBox das linhas atuais, para calcular o progresso
        private readonly List<CheckBox> _checks = new List<CheckBox>();
        private double _fracaoProgresso = 0;

        // Hábitos exibidos agora (null até a tela ser carregada pela primeira vez).
        // Ficam guardados para remontar as linhas quando o tema muda, sem ir ao banco.
        private List<HabitoDoDia>? _habitosAtuais;

        public HomeControl()
        {
            InitializeComponent();

            // A lista só rola na vertical: as linhas sempre têm a largura do painel.
            // (Truque do WinForms: com AutoScroll ligado não dá para desligar só a
            // barra horizontal; é preciso desligar, configurar e ligar de novo.)
            flpHabitos.AutoScroll = false;
            flpHabitos.HorizontalScroll.Enabled = false;
            flpHabitos.HorizontalScroll.Visible = false;
            flpHabitos.HorizontalScroll.Maximum = 0;
            flpHabitos.AutoScroll = true;

            AplicarTema();
            Tema.Alterado += Tema_Alterado;
            Disposed += delegate { Tema.Alterado -= Tema_Alterado; };
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

        private void Tema_Alterado(object? sender, EventArgs e)
        {
            AplicarTema();
        }

        private void flpHabitos_ClientSizeChanged(object sender, EventArgs e)
        {
            AjustarLarguraLinhas();
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
        private void AplicarTema()
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
            lblData.Text = FormatarData(DateTime.Now);

            _habitosAtuais = ObterHabitosDoDia();
            MontarLinhas();
        }

        // TODO: trocar pelos dados reais, vindos do CheckinService
        // (hábitos do dia + se já têm check-in hoje + streak atual).
        private List<HabitoDoDia> ObterHabitosDoDia()
        {
            List<HabitoDoDia> lista = new List<HabitoDoDia>();
            lista.Add(new HabitoDoDia { Id = 1, Nome = "Beber 2L de água", Categoria = "Saúde", Streak = 12, Concluido = true, Cor = Color.FromArgb(70, 140, 200) });
            lista.Add(new HabitoDoDia { Id = 2, Nome = "Ler 20 páginas", Categoria = "Aprendizado", Streak = 5, Concluido = false, Cor = Color.FromArgb(150, 110, 190) });
            lista.Add(new HabitoDoDia { Id = 3, Nome = "Treinar", Categoria = "Saúde", Streak = 3, Concluido = true, Cor = Color.FromArgb(70, 140, 200) });
            lista.Add(new HabitoDoDia { Id = 4, Nome = "Revisar as finanças", Categoria = "Finanças", Streak = 0, Concluido = false, Cor = Color.FromArgb(220, 160, 60) });
            lista.Add(new HabitoDoDia { Id = 5, Nome = "Ligar para a família", Categoria = "Relacionamentos", Streak = 1, Concluido = false, Cor = Color.FromArgb(210, 100, 110) });
            lista.Add(new HabitoDoDia { Id = 6, Nome = "Estudar C#", Categoria = "Aprendizado", Streak = 21, Concluido = false, Cor = Color.FromArgb(150, 110, 190) });
            return lista;
        }

        // Recria as linhas a partir de _habitosAtuais (sem ir ao banco)
        private void MontarLinhas()
        {
            List<HabitoDoDia> habitos = _habitosAtuais ?? new List<HabitoDoDia>();

            LimparLinhas();

            flpHabitos.SuspendLayout();
            foreach (HabitoDoDia h in habitos)
            {
                flpHabitos.Controls.Add(CriarLinha(h));
            }
            flpHabitos.ResumeLayout();

            bool vazio = habitos.Count == 0;
            flpHabitos.Visible = !vazio;
            lblVazio.Visible = vazio;

            AjustarLarguraLinhas();
            AtualizarProgresso();
        }

        // Remove E descarta as linhas antigas (RemoveAt sozinho não libera da memória)
        private void LimparLinhas()
        {
            _checks.Clear();
            while (flpHabitos.Controls.Count > 0)
            {
                Control c = flpHabitos.Controls[0];
                flpHabitos.Controls.RemoveAt(0);
                c.Dispose();
            }
        }

        // ---------- Montagem de cada linha ----------

        private Panel CriarLinha(HabitoDoDia h)
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
            faixa.BackColor = h.Cor;

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
            lblDetalhe.Text = h.Categoria + "  ·  " + TextoStreak(h.Streak);

            linha.Controls.Add(faixa);
            linha.Controls.Add(chk);
            linha.Controls.Add(lblNome);
            linha.Controls.Add(lblDetalhe);

            _checks.Add(chk);

            // Clicar em qualquer parte da linha também marca/desmarca
            EventHandler alternar = delegate { chk.Checked = !chk.Checked; };
            linha.Click += alternar;
            lblNome.Click += alternar;
            lblDetalhe.Click += alternar;

            chk.CheckedChanged += delegate { AoAlterarCheckin(chk, lblNome, h); };

            return linha;
        }

        private void AoAlterarCheckin(CheckBox chk, Label lblNome, HabitoDoDia h)
        {
            // Guarda o novo estado, para a linha continuar certa se o tema mudar
            h.Concluido = chk.Checked;

            lblNome.Font = FonteNome(chk.Checked);
            lblNome.ForeColor = CorNome(chk.Checked);

            // TODO: chamar o CheckinService para gravar (ou desmarcar) o check-in
            // do hábito h.Id na data de hoje, e atualizar o streak.

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

        // ---------- Progresso ----------

        private void AtualizarProgresso()
        {
            int total = _checks.Count;
            int feitos = 0;
            foreach (CheckBox c in _checks)
            {
                if (c.Checked) feitos++;
            }

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

        // ---------- Auxiliares ----------

        private void AjustarLarguraLinhas()
        {
            foreach (Control c in flpHabitos.Controls)
            {
                c.Width = flpHabitos.ClientSize.Width - c.Margin.Horizontal;
            }
        }

        private static string TextoStreak(int dias)
        {
            if (dias <= 0) return "sem sequência";
            if (dias == 1) return "1 dia seguido";
            return dias + " dias seguidos";
        }

        private static string FormatarData(DateTime data)
        {
            CultureInfo pt = new CultureInfo("pt-BR");
            string texto = data.ToString("dddd, d 'de' MMMM", pt);
            return char.ToUpper(texto[0], pt) + texto.Substring(1);
        }

        // Dados de uma linha da lista (provisório, enquanto a Home não usa o CheckinService)
        private class HabitoDoDia
        {
            public long Id { get; set; }
            public string Nome { get; set; } = string.Empty;
            public string Categoria { get; set; } = string.Empty;
            public int Streak { get; set; }
            public bool Concluido { get; set; }
            public Color Cor { get; set; }
        }
    }
}
