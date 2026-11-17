using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;
using HabitDisc.Helpers;
using HabitDisc.Models;
using HabitDisc.Services;
using HabitDisc.Telas.Componentes;

namespace HabitDisc.Telas
{
    // Tela Humor (UC05): registro opcional do humor do dia, de 1 a 5, com uma
    // observação, e o histórico dos últimos registros.
    // Se já registrou hoje, pode alterar (fluxo alternativo do UC05).
    public partial class HumorControl : TelaBase
    {
        private const int QuantidadeNoHistorico = 30;

        // Escala do docs (5.2.4): índice 0 = valor 1 (Muito ruim) ... índice 4 = valor 5 (Excelente)
        private static readonly string[] Nomes = { "Muito ruim", "Ruim", "Neutro", "Bom", "Excelente" };
        private static readonly IconChar[] Icones =
        {
            IconChar.FaceSadTear, IconChar.FaceFrown, IconChar.FaceMeh, IconChar.FaceSmile, IconChar.FaceLaughBeam
        };
        private static readonly Color[] Cores =
        {
            Color.FromArgb(205, 90, 90), Color.FromArgb(225, 140, 70), Color.FromArgb(205, 170, 50),
            Color.FromArgb(115, 180, 95), Color.FromArgb(55, 155, 95)
        };

        private readonly HumorService _service = new HumorService();
        private readonly List<IconButton> _opcoes = new List<IconButton>();

        private int? _valorSelecionado;
        private bool _jaRegistrouHoje;
        private List<Humor>? _historico;

        public HumorControl()
        {
            InitializeComponent();

            txtObservacao.MaxLength = HumorService.TamanhoMaximoObservacao;
            CriarOpcoes();
            ListaVertical.Configurar(flpHistorico);

            AplicarTema();
        }

        // ---------- Eventos ----------

        private void HumorControl_VisibleChanged(object? sender, EventArgs e)
        {
            if (Visible && !DesignMode)
            {
                Carregar();
            }
        }

        private void btnSalvar_Click(object? sender, EventArgs e)
        {
            if (_valorSelecionado == null)
            {
                Mensagem.Aviso("Escolha como você está se sentindo hoje.");
                return;
            }

            try
            {
                _service.SalvarHoje(_valorSelecionado.Value, txtObservacao.Text);
            }
            catch (RegraNegocioException ex)
            {
                Mensagem.Aviso(ex.Message);
                return;
            }

            Carregar();
            lblStatus.Text = "Humor registrado!";
            PosicionarAcoes();
        }

        // ---------- Carregamento ----------

        private void Carregar()
        {
            Humor? hoje = _service.ObterDeHoje();

            _jaRegistrouHoje = hoje != null;
            _valorSelecionado = hoje?.Valor;
            txtObservacao.Text = hoje?.Observacao ?? string.Empty;
            lblStatus.Text = _jaRegistrouHoje
                ? "Você já registrou hoje. Pode alterar e atualizar."
                : string.Empty;

            _historico = _service.ListarRecentes(QuantidadeNoHistorico);

            PintarOpcoes();
            PosicionarAcoes();
            MontarHistorico();
        }

        // Os 5 botões de humor (rosto + nome)
        private void CriarOpcoes()
        {
            for (int i = 0; i < Nomes.Length; i++)
            {
                int valor = i + 1;

                IconButton botao = new IconButton();
                botao.Text = Nomes[i];
                botao.IconChar = Icones[i];
                botao.IconFont = IconFont.Auto;
                botao.TextImageRelation = TextImageRelation.ImageAboveText;
                botao.ImageAlign = ContentAlignment.BottomCenter;
                botao.TextAlign = ContentAlignment.TopCenter;
                botao.FlatStyle = FlatStyle.Flat;
                botao.FlatAppearance.BorderSize = 0;
                botao.UseVisualStyleBackColor = false;
                botao.Cursor = Cursors.Hand;
                botao.Click += delegate { Selecionar(valor); };

                _opcoes.Add(botao);
                flpOpcoes.Controls.Add(botao);
            }
        }

        private void Selecionar(int valor)
        {
            _valorSelecionado = valor;
            lblStatus.Text = string.Empty;
            PintarOpcoes();
        }

        // O selecionado fica com o fundo na cor do humor
        private void PintarOpcoes()
        {
            for (int i = 0; i < _opcoes.Count; i++)
            {
                IconButton botao = _opcoes[i];
                bool selecionado = _valorSelecionado == i + 1;

                botao.Font = Tema.Fonte(10f, FontStyle.Bold);
                botao.IconSize = Tema.Escalar(36);
                // Estreitos o bastante para os 5 caberem numa linha mesmo com fonte Grande
                botao.Size = new Size(Tema.Escalar(80), Tema.Escalar(88));
                botao.Margin = new Padding(0, 0, Tema.Escalar(8), Tema.Escalar(8));

                botao.BackColor = selecionado ? Cores[i] : Tema.Superficie;
                botao.ForeColor = selecionado ? Color.White : Tema.Texto;
                botao.IconColor = selecionado ? Color.White : Cores[i];
                botao.FlatAppearance.MouseOverBackColor = selecionado ? Cores[i] : Tema.Fundo;
            }
        }

        private void MontarHistorico()
        {
            List<Humor> registros = _historico ?? new List<Humor>();

            ListaVertical.Limpar(flpHistorico);

            flpHistorico.SuspendLayout();
            foreach (Humor humor in registros)
            {
                int i = Math.Clamp(humor.Valor, 1, 5) - 1;

                string subtitulo = Nomes[i];
                if (humor.Observacao != null)
                {
                    subtitulo += "  ·  " + humor.Observacao.ReplaceLineEndings(" ");
                }

                flpHistorico.Controls.Add(new LinhaItemControl(
                    Cores[i], Icones[i], Textos.DataPorExtenso(humor.Data), subtitulo));
            }
            flpHistorico.ResumeLayout();

            bool vazio = registros.Count == 0;
            flpHistorico.Visible = !vazio;
            lblHistoricoVazio.Visible = vazio;
        }

        // Texto do botão muda se já registrou hoje; a mensagem fica ao lado dele
        private void PosicionarAcoes()
        {
            btnSalvar.Text = _jaRegistrouHoje ? "Atualizar registro" : "Registrar humor";
            btnSalvar.Location = new Point(0, Tema.Escalar(12));

            // A mensagem vai do botão até a borda; se não couber, termina com "..."
            lblStatus.AutoSize = false;
            lblStatus.AutoEllipsis = true;
            lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblStatus.Location = new Point(
                btnSalvar.Right + Tema.Escalar(14),
                btnSalvar.Top + (btnSalvar.Height - lblStatus.PreferredHeight) / 2);
            lblStatus.Size = new Size(
                Math.Max(0, pnlAcoes.ClientSize.Width - lblStatus.Left),
                lblStatus.PreferredHeight);

            pnlAcoes.Height = btnSalvar.Bottom + Tema.Escalar(12);
        }

        // ---------- Tema ----------

        protected override void AplicarTema()
        {
            BackColor = Tema.Fundo;

            Estilo.Titulo(lblTitulo);
            lblTitulo.Height = lblTitulo.Font.Height + Tema.Escalar(12);

            Estilo.Secao(lblPergunta);
            lblPergunta.Height = lblPergunta.Font.Height + Tema.Escalar(12);

            PintarOpcoes();

            Estilo.CaixaTexto(txtObservacao);
            txtObservacao.Height = txtObservacao.Font.Height * 3 + 8;

            Estilo.BotaoPrimario(btnSalvar);
            Estilo.TextoSuave(lblStatus);
            PosicionarAcoes();

            Estilo.Secao(lblHistorico);
            lblHistorico.Height = lblHistorico.Font.Height + Tema.Escalar(28);

            lblHistoricoVazio.Font = Tema.Fonte(13f);
            lblHistoricoVazio.ForeColor = Tema.TextoSuave;

            if (_historico != null)
            {
                MontarHistorico();
            }
        }
    }
}
