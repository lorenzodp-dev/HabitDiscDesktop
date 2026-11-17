using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace HabitDisc.Helpers
{
    // Estilos reaproveitados pelas telas: em vez de cada tela repetir as mesmas
    // cores e fontes, ela chama Estilo.Titulo(lbl), Estilo.BotaoPrimario(btn) etc.
    // Tudo lê o Tema atual, então funciona com qualquer paleta, fonte e modo escuro.
    public static class Estilo
    {
        public static void Titulo(Label lbl)
        {
            lbl.Font = Tema.Fonte(28f, FontStyle.Bold);
            lbl.ForeColor = Tema.Titulo;
        }

        public static void Secao(Label lbl)
        {
            lbl.Font = Tema.Fonte(16f, FontStyle.Bold);
            lbl.ForeColor = Tema.Titulo;
        }

        public static void Texto(Control controle)
        {
            controle.Font = Tema.Fonte(13f);
            controle.ForeColor = Tema.Texto;
        }

        public static void TextoSuave(Control controle)
        {
            controle.Font = Tema.Fonte(12f);
            controle.ForeColor = Tema.TextoSuave;
        }

        // Botão da ação principal (ex.: Salvar, Novo hábito): fundo na cor do tema
        public static void BotaoPrimario(Button botao)
        {
            PrepararBotao(botao);
            botao.BackColor = Tema.Menu;
            botao.ForeColor = Tema.TextoSobreMenu;
            botao.FlatAppearance.BorderSize = 0;
            botao.FlatAppearance.MouseOverBackColor = Tema.MenuSelecionado;

            if (botao is IconButton comIcone)
            {
                comIcone.IconColor = Tema.TextoSobreMenu;
            }
        }

        // Botão secundário (ex.: Cancelar): fundo claro com borda
        public static void BotaoSecundario(Button botao)
        {
            PrepararBotao(botao);
            botao.BackColor = Tema.Superficie;
            botao.ForeColor = Tema.Titulo;
            botao.FlatAppearance.BorderSize = 1;
            botao.FlatAppearance.BorderColor = Tema.TextoSuave;
            botao.FlatAppearance.MouseOverBackColor = Tema.Fundo;

            if (botao is IconButton comIcone)
            {
                comIcone.IconColor = Tema.Titulo;
            }
        }

        private static void PrepararBotao(Button botao)
        {
            botao.FlatStyle = FlatStyle.Flat;
            botao.UseVisualStyleBackColor = false;
            botao.Cursor = Cursors.Hand;
            botao.Font = Tema.Fonte(12f, FontStyle.Bold);

            if (botao is IconButton comIcone)
            {
                comIcone.IconSize = Tema.Escalar(20);
            }
        }

        public static void CaixaTexto(TextBox caixa)
        {
            caixa.Font = Tema.Fonte(13f);
            caixa.BackColor = Tema.Superficie;
            caixa.ForeColor = Tema.Titulo;
            caixa.BorderStyle = BorderStyle.FixedSingle;
        }

        public static void Combo(ComboBox combo)
        {
            combo.Font = Tema.Fonte(13f);
            combo.BackColor = Tema.Superficie;
            combo.ForeColor = Tema.Titulo;
            combo.FlatStyle = FlatStyle.Flat;
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        // Cabeçalho das telas: título à esquerda e, opcionalmente, um botão de
        // ação à direita (ex.: "Novo hábito"), centralizado na altura do título.
        public static void Cabecalho(Panel painel, Label titulo, Button? acao = null)
        {
            Titulo(titulo);
            titulo.AutoSize = true;
            titulo.Location = new Point(0, 0);
            painel.Height = titulo.PreferredHeight + Tema.Escalar(16);

            if (acao != null)
            {
                BotaoPrimario(acao);
                acao.AutoSize = false;
                acao.Size = acao.PreferredSize;
                acao.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                acao.Location = new Point(
                    painel.ClientSize.Width - acao.Width,
                    (titulo.PreferredHeight - acao.Height) / 2);
            }
        }
    }
}
