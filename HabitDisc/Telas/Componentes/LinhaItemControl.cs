using System;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;
using HabitDisc.Helpers;

namespace HabitDisc.Telas.Componentes
{
    // Uma "linha" de lista: faixa colorida, ícone, título, subtítulo e, à direita,
    // botões de ação (editar, excluir...) ou uma etiqueta (ex.: "Predefinida").
    // Usada nas telas Hábitos, Categorias e Humor, para todas as listas terem o
    // mesmo visual. As cores e fontes são lidas do Tema quando a linha é criada:
    // quando o tema muda, a tela recria as linhas.
    public class LinhaItemControl : Panel
    {
        private readonly Panel _textos = new Panel();
        private readonly Panel _areaIcone = new Panel();
        private readonly FlowLayoutPanel _acoes = new FlowLayoutPanel();
        private readonly Label _lblTitulo = new Label();
        private readonly Label _lblSubtitulo = new Label();
        private readonly IconPictureBox _imagem = new IconPictureBox();
        private readonly ToolTip _dicas = new ToolTip();

        public LinhaItemControl(Color cor, IconChar icone, string titulo, string subtitulo)
        {
            Font fonteTitulo = Tema.Fonte(14f, FontStyle.Bold);
            Font fonteSubtitulo = Tema.Fonte(11f);
            int margem = Tema.Escalar(9);

            // Altura calculada pela fonte, para o texto caber em qualquer tamanho
            Height = margem + (fonteTitulo.Height + 4) + (fonteSubtitulo.Height + 2) + margem;
            BackColor = Tema.Superficie;
            Margin = new Padding(0, 0, 0, 10);

            Panel faixa = new Panel();
            faixa.Dock = DockStyle.Left;
            faixa.Width = 6;
            faixa.BackColor = cor;

            _areaIcone.Dock = DockStyle.Left;
            _areaIcone.Width = Tema.Escalar(60);

            _imagem.IconChar = icone;
            _imagem.IconColor = cor;
            _imagem.IconFont = IconFont.Auto;
            _imagem.IconSize = Tema.Escalar(28);
            _imagem.Size = new Size(Tema.Escalar(28), Tema.Escalar(28));
            _imagem.BackColor = Tema.Superficie;
            _imagem.Location = new Point((_areaIcone.Width - _imagem.Width) / 2, (Height - _imagem.Height) / 2);
            _areaIcone.Controls.Add(_imagem);

            _acoes.Dock = DockStyle.Right;
            _acoes.AutoSize = true;
            _acoes.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _acoes.WrapContents = false;
            _acoes.BackColor = Tema.Superficie;
            _acoes.Padding = new Padding(0, 0, Tema.Escalar(12), 0);

            _textos.Dock = DockStyle.Fill;
            _textos.Padding = new Padding(0, margem, Tema.Escalar(8), 0);

            _lblTitulo.Dock = DockStyle.Top;
            _lblTitulo.Height = fonteTitulo.Height + 4;
            _lblTitulo.AutoEllipsis = true;
            _lblTitulo.Text = titulo;
            _lblTitulo.Font = fonteTitulo;
            _lblTitulo.ForeColor = Tema.Titulo;

            _lblSubtitulo.Dock = DockStyle.Top;
            _lblSubtitulo.Height = fonteSubtitulo.Height + 2;
            _lblSubtitulo.AutoEllipsis = true;
            _lblSubtitulo.Text = subtitulo;
            _lblSubtitulo.Font = fonteSubtitulo;
            _lblSubtitulo.ForeColor = Tema.Texto;

            // No Dock, o último adicionado é encaixado primeiro:
            _textos.Controls.Add(_lblSubtitulo);   // embaixo
            _textos.Controls.Add(_lblTitulo);      // em cima

            // Faixa na ponta esquerda, depois o ícone, as ações à direita
            // e os textos ocupando o meio
            Controls.Add(_textos);
            Controls.Add(_acoes);
            Controls.Add(_areaIcone);
            Controls.Add(faixa);
        }

        // Faz a linha inteira (menos os botões de ação) responder ao clique
        public void PermitirClique(EventHandler aoClicar)
        {
            Cursor = Cursors.Hand;
            foreach (Control parte in new Control[] { this, _textos, _lblTitulo, _lblSubtitulo, _areaIcone, _imagem })
            {
                parte.Click += aoClicar;
            }
        }

        // Botão pequeno de ícone à direita (ex.: lápis para editar)
        public IconButton AdicionarAcao(IconChar icone, string dica, EventHandler aoClicar)
        {
            int tamanho = Tema.Escalar(36);

            IconButton botao = new IconButton();
            botao.IconChar = icone;
            botao.IconColor = Tema.Texto;
            botao.IconFont = IconFont.Auto;
            botao.IconSize = Tema.Escalar(20);
            botao.Size = new Size(tamanho, tamanho);
            botao.FlatStyle = FlatStyle.Flat;
            botao.FlatAppearance.BorderSize = 0;
            botao.FlatAppearance.MouseOverBackColor = Tema.Fundo;
            botao.BackColor = Tema.Superficie;
            botao.UseVisualStyleBackColor = false;
            botao.Cursor = Cursors.Hand;
            // Margem de cima centraliza o botão na altura da linha
            botao.Margin = new Padding(Tema.Escalar(4), (Height - tamanho) / 2, 0, 0);
            botao.Click += aoClicar;
            _dicas.SetToolTip(botao, dica);
            // Botão só com ícone não tem texto: este é o nome lido pelos leitores de tela
            botao.AccessibleName = dica;

            _acoes.Controls.Add(botao);
            return botao;
        }

        // Texto discreto à direita (ex.: "Predefinida")
        public void AdicionarEtiqueta(string texto)
        {
            Label etiqueta = new Label();
            etiqueta.AutoSize = true;
            etiqueta.Text = texto;
            etiqueta.Font = Tema.Fonte(11f);
            etiqueta.ForeColor = Tema.TextoSuave;
            etiqueta.Margin = new Padding(Tema.Escalar(4), (Height - etiqueta.PreferredHeight) / 2, 0, 0);

            _acoes.Controls.Add(etiqueta);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dicas.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
