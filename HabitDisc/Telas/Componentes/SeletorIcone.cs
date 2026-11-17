using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;
using HabitDisc.Helpers;

namespace HabitDisc.Telas.Componentes
{
    // Grade de ícones para escolher (formulários de hábito e de categoria).
    // Os ícones vêm de IconeHelper.Disponiveis: para oferecer um ícone novo,
    // basta acrescentá-lo lá.
    public class SeletorIcone : FlowLayoutPanel
    {
        private readonly List<IconButton> _botoes = new List<IconButton>();
        private string _selecionado;

        public SeletorIcone()
        {
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            WrapContents = true;
            // Largura máxima: os ícones quebram em linhas a partir daqui
            MaximumSize = new Size(Tema.Escalar(440), 0);

            int tamanho = Tema.Escalar(40);

            foreach (string nome in IconeHelper.Disponiveis)
            {
                IconButton botao = new IconButton();
                botao.Tag = nome;
                botao.IconChar = IconeHelper.ParaIconChar(nome);
                botao.IconFont = IconFont.Auto;
                botao.IconSize = Tema.Escalar(22);
                botao.Size = new Size(tamanho, tamanho);
                botao.FlatStyle = FlatStyle.Flat;
                botao.FlatAppearance.BorderSize = 0;
                botao.UseVisualStyleBackColor = false;
                botao.Cursor = Cursors.Hand;
                botao.Margin = new Padding(0, 0, 4, 4);
                botao.Click += delegate { IconeSelecionado = nome; };

                _botoes.Add(botao);
                Controls.Add(botao);
            }

            _selecionado = IconeHelper.Disponiveis[0];
            Pintar();
        }

        // Nome do ícone escolhido, no formato do banco (ex.: "heart")
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string IconeSelecionado
        {
            get
            {
                return _selecionado;
            }
            set
            {
                // Compara pelo desenho, para aceitar também nomes antigos
                // (ex.: "check-circle" seleciona o mesmo botão de "circle-check")
                IconChar desenho = IconeHelper.ParaIconChar(value);
                IconButton? botao = _botoes.Find(b => b.IconChar == desenho);

                _selecionado = botao != null ? (string)botao.Tag! : IconeHelper.Disponiveis[0];
                Pintar();
            }
        }

        private void Pintar()
        {
            foreach (IconButton botao in _botoes)
            {
                bool selecionado = (string)botao.Tag! == _selecionado;

                botao.BackColor = selecionado ? Tema.Menu : Tema.Superficie;
                botao.IconColor = selecionado ? Tema.TextoSobreMenu : Tema.Texto;
                botao.FlatAppearance.MouseOverBackColor = selecionado ? Tema.Menu : Tema.Fundo;
            }
        }
    }
}
