using System;
using System.Drawing;
using System.Windows.Forms;
using HabitDisc.Helpers;

namespace HabitDisc.Forms
{
    // Base das janelas de diálogo (novo/editar hábito, nova/editar categoria,
    // detalhes do hábito): tamanho automático, centralizada sobre a janela
    // principal, cores do Tema e barra de título escura no modo escuro.
    //
    // As janelas filhas montam o conteúdo com AdicionarCampo(...) e
    // AdicionarBotao(...), ou colocando controles direto em Conteudo.
    public class FormDialogoBase : Form
    {
        // Área do conteúdo: os controles ficam um embaixo do outro
        protected readonly FlowLayoutPanel Conteudo = new FlowLayoutPanel();

        private readonly FlowLayoutPanel _raiz = new FlowLayoutPanel();
        private readonly FlowLayoutPanel _botoes = new FlowLayoutPanel();

        public FormDialogoBase(string titulo)
        {
            Text = titulo;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Tema.Fundo;

            // A janela cresce sozinha até caber todo o conteúdo
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            int margem = Tema.Escalar(24);
            Padding = new Padding(0, 0, margem, margem);

            _raiz.FlowDirection = FlowDirection.TopDown;
            _raiz.WrapContents = false;
            _raiz.AutoSize = true;
            _raiz.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _raiz.Location = new Point(margem, margem);
            _raiz.Margin = Padding.Empty;

            Conteudo.FlowDirection = FlowDirection.TopDown;
            Conteudo.WrapContents = false;
            Conteudo.AutoSize = true;
            Conteudo.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Conteudo.Margin = Padding.Empty;

            // Botões alinhados à direita, embaixo do conteúdo
            _botoes.FlowDirection = FlowDirection.LeftToRight;
            _botoes.WrapContents = false;
            _botoes.AutoSize = true;
            _botoes.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _botoes.Anchor = AnchorStyles.Right;
            _botoes.Margin = new Padding(0, Tema.Escalar(20), 0, 0);

            _raiz.Controls.Add(Conteudo);
            _raiz.Controls.Add(_botoes);
            Controls.Add(_raiz);
        }

        // Rótulo em negrito + o campo logo embaixo
        protected void AdicionarCampo(string rotulo, Control campo)
        {
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.Text = rotulo;
            lbl.Font = Tema.Fonte(12f, FontStyle.Bold);
            lbl.ForeColor = Tema.Titulo;
            lbl.Margin = new Padding(0, Tema.Escalar(12), 0, Tema.Escalar(4));

            campo.Margin = Padding.Empty;

            Conteudo.Controls.Add(lbl);
            Conteudo.Controls.Add(campo);
        }

        // principal = true: botão colorido (ação principal); false: botão com borda
        protected Button AdicionarBotao(string texto, bool principal, EventHandler? aoClicar = null)
        {
            Button botao = new Button();
            botao.Text = texto;
            botao.AutoSize = true;
            botao.Padding = new Padding(Tema.Escalar(10), Tema.Escalar(4), Tema.Escalar(10), Tema.Escalar(4));
            botao.Margin = new Padding(Tema.Escalar(8), 0, 0, 0);

            if (principal)
            {
                Estilo.BotaoPrimario(botao);
            }
            else
            {
                Estilo.BotaoSecundario(botao);
            }

            if (aoClicar != null)
            {
                botao.Click += aoClicar;
            }

            _botoes.Controls.Add(botao);
            return botao;
        }

        // Botão que só fecha a janela (Cancelar / Fechar), também acionado pelo Esc
        protected Button AdicionarBotaoFechar(string texto, bool principal)
        {
            Button botao = AdicionarBotao(texto, principal);
            botao.DialogResult = DialogResult.Cancel;
            CancelButton = botao;
            return botao;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            BarraDeTitulo.AplicarModoEscuro(this, Tema.Atual.ModoEscuro);
        }
    }
}
