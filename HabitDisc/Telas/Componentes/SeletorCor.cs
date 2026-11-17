using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using HabitDisc.Helpers;

namespace HabitDisc.Telas.Componentes
{
    // Amostras de cor para escolher (formulários de hábito e de categoria),
    // mais um botão "Outra..." que abre a janela de cores do Windows.
    // As cores sugeridas vêm de CorHelper.CoresSugeridas.
    public class SeletorCor : FlowLayoutPanel
    {
        private readonly List<Button> _amostras = new List<Button>();
        private readonly Button _btnOutra = new Button();
        private string _selecionada;

        public SeletorCor()
        {
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            WrapContents = true;
            MaximumSize = new Size(Tema.Escalar(440), 0);

            int tamanho = Tema.Escalar(34);

            foreach (string hex in CorHelper.CoresSugeridas)
            {
                Color cor = CorHelper.HexParaCor(hex);

                Button amostra = new Button();
                amostra.Tag = hex;
                amostra.Size = new Size(tamanho, tamanho);
                amostra.BackColor = cor;
                amostra.ForeColor = Color.White;
                amostra.Font = Tema.Fonte(12f, FontStyle.Bold);
                amostra.FlatStyle = FlatStyle.Flat;
                amostra.FlatAppearance.MouseOverBackColor = cor;
                amostra.UseVisualStyleBackColor = false;
                amostra.Cursor = Cursors.Hand;
                amostra.Margin = new Padding(0, 0, 6, 6);
                amostra.Click += delegate { CorSelecionada = hex; };

                _amostras.Add(amostra);
                Controls.Add(amostra);
            }

            _btnOutra.Text = "Outra...";
            _btnOutra.AutoSize = true;
            _btnOutra.MinimumSize = new Size(0, tamanho);
            _btnOutra.Margin = new Padding(0, 0, 6, 6);
            _btnOutra.Click += delegate { EscolherOutra(); };
            Controls.Add(_btnOutra);

            _selecionada = CorHelper.CoresSugeridas[0];
            Pintar();
        }

        // Cor escolhida no formato do banco: "#RRGGBB"
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CorSelecionada
        {
            get
            {
                return _selecionada;
            }
            set
            {
                _selecionada = value.ToUpperInvariant();
                Pintar();
            }
        }

        private void EscolherOutra()
        {
            using ColorDialog dialogo = new ColorDialog();
            dialogo.Color = CorHelper.HexParaCor(_selecionada);
            dialogo.FullOpen = true;

            if (dialogo.ShowDialog(FindForm()) == DialogResult.OK)
            {
                CorSelecionada = CorHelper.CorParaHex(dialogo.Color);
            }
        }

        // A amostra escolhida ganha borda e um ✓
        private void Pintar()
        {
            bool corDaLista = false;

            foreach (Button amostra in _amostras)
            {
                bool selecionada = string.Equals((string)amostra.Tag!, _selecionada, StringComparison.OrdinalIgnoreCase);
                if (selecionada)
                {
                    corDaLista = true;
                }

                amostra.Text = selecionada ? "✓" : string.Empty;
                amostra.FlatAppearance.BorderSize = selecionada ? 3 : 0;
                amostra.FlatAppearance.BorderColor = Tema.Titulo;
            }

            if (corDaLista)
            {
                Estilo.BotaoSecundario(_btnOutra);
            }
            else
            {
                // Cor fora da lista: o próprio botão "Outra..." mostra a cor escolhida
                Color cor = CorHelper.HexParaCor(_selecionada);
                _btnOutra.BackColor = cor;
                _btnOutra.ForeColor = Color.White;
                _btnOutra.FlatAppearance.MouseOverBackColor = cor;
                _btnOutra.FlatAppearance.BorderSize = 3;
                _btnOutra.FlatAppearance.BorderColor = Tema.Titulo;
            }
        }
    }
}
