using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace HabitDisc.Telas
{
    public partial class HomeControl : UserControl
    {
        // Cores e fontes usadas nas linhas criadas por código
        private static readonly Color CorTitulo = Color.FromArgb(35, 77, 51);
        private static readonly Color CorTextoSuave = Color.FromArgb(140, 140, 140);
        private static readonly Color CorDetalhe = Color.FromArgb(100, 100, 100);

        private static readonly Font FonteNome = new Font("Newsreader", 14f, FontStyle.Bold);
        private static readonly Font FonteNomeRiscada = new Font("Newsreader", 14f, FontStyle.Bold | FontStyle.Strikeout);
        private static readonly Font FonteDetalhe = new Font("Newsreader", 11f, FontStyle.Regular);

        // Guarda os CheckBox das linhas atuais, para calcular o progresso
        private readonly List<CheckBox> _checks = new List<CheckBox>();
        private double _fracaoProgresso = 0;

        public HomeControl()
        {
            InitializeComponent();
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

        private void flpHabitos_ClientSizeChanged(object sender, EventArgs e)
        {
            AjustarLarguraLinhas();
        }

        private void pnlBarraFundo_Resize(object sender, EventArgs e)
        {
            AtualizarBarra();
        }

        // ---------- Carregamento ----------

        private void AtualizarTela()
        {
            lblData.Text = FormatarData(DateTime.Now);

            LimparLinhas();

            List<HabitoDoDia> habitos = ObterHabitosDoDia();

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

        // TODO: trocar pelos dados reais, vindos de um Service
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
            Panel linha = new Panel();
            linha.Height = 64;
            linha.Width = flpHabitos.ClientSize.Width;
            linha.BackColor = Color.White;
            linha.Margin = new Padding(0, 0, 0, 10);
            linha.Cursor = Cursors.Hand;
            linha.Tag = h.Id;

            Panel faixa = new Panel();
            faixa.Dock = DockStyle.Left;
            faixa.Width = 6;
            faixa.BackColor = h.Cor;

            CheckBox chk = new CheckBox();
            chk.AutoSize = false;
            chk.Size = new Size(24, 24);
            chk.Location = new Point(26, 20);
            chk.Cursor = Cursors.Hand;
            chk.Checked = h.Concluido;

            Label lblNome = new Label();
            lblNome.AutoSize = false;
            lblNome.AutoEllipsis = true;
            lblNome.Location = new Point(66, 9);
            lblNome.Size = new Size(linha.Width - 66 - 16, 26);
            lblNome.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblNome.Text = h.Nome;
            lblNome.Font = h.Concluido ? FonteNomeRiscada : FonteNome;
            lblNome.ForeColor = h.Concluido ? CorTextoSuave : CorTitulo;

            Label lblDetalhe = new Label();
            lblDetalhe.AutoSize = false;
            lblDetalhe.AutoEllipsis = true;
            lblDetalhe.Location = new Point(66, 36);
            lblDetalhe.Size = new Size(linha.Width - 66 - 16, 20);
            lblDetalhe.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblDetalhe.Font = FonteDetalhe;
            lblDetalhe.ForeColor = CorDetalhe;
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

            chk.CheckedChanged += delegate { AoAlterarCheckin(chk, lblNome, h.Id); };

            return linha;
        }

        private void AoAlterarCheckin(CheckBox chk, Label lblNome, long idHabito)
        {
            lblNome.Font = chk.Checked ? FonteNomeRiscada : FonteNome;
            lblNome.ForeColor = chk.Checked ? CorTextoSuave : CorTitulo;

            // TODO: chamar o Service para gravar (ou desmarcar) o check-in
            // do hábito idHabito na data de hoje, e recalcular o streak.

            AtualizarProgresso();
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

        // Dados de uma linha da lista (provisório, enquanto não há Service)
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