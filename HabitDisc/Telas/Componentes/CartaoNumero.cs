using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using HabitDisc.Helpers;

namespace HabitDisc.Telas.Componentes
{
    // Cartão com um número em destaque (ex.: "72%") e dois textos menores embaixo.
    // Usado nas Estatísticas e nos detalhes do hábito.
    public class CartaoNumero : Panel
    {
        private readonly Label _lblValor = new Label();
        private readonly Label _lblRotulo = new Label();
        private readonly Label _lblDetalhe = new Label();
        private readonly int _larguraBase;

        // larguraBase: largura com a fonte Média (cresce/encolhe com a escala)
        public CartaoNumero(int larguraBase)
        {
            _larguraBase = larguraBase;

            foreach (Label lbl in new[] { _lblValor, _lblRotulo, _lblDetalhe })
            {
                lbl.Dock = DockStyle.Top;
                lbl.AutoEllipsis = true;
            }

            // No Dock, o último adicionado fica em cima
            Controls.Add(_lblDetalhe);
            Controls.Add(_lblRotulo);
            Controls.Add(_lblValor);

            AplicarTema();
        }

        // [DesignerSerializationVisibility(Hidden)]: estas propriedades são definidas
        // por código, então o designer do Visual Studio não deve gravá-las no .Designer.cs
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Valor
        {
            get { return _lblValor.Text; }
            set { _lblValor.Text = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Rotulo
        {
            get { return _lblRotulo.Text; }
            set { _lblRotulo.Text = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Detalhe
        {
            get { return _lblDetalhe.Text; }
            set { _lblDetalhe.Text = value; }
        }

        // Para destacar o número (ex.: verde quando melhorou, vermelho quando piorou).
        // AplicarTema() volta para a cor padrão.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color CorValor
        {
            get { return _lblValor.ForeColor; }
            set { _lblValor.ForeColor = value; }
        }

        public void AplicarTema()
        {
            BackColor = Tema.Superficie;
            Padding = new Padding(Tema.Escalar(14), Tema.Escalar(10), Tema.Escalar(10), Tema.Escalar(10));

            _lblValor.Font = Tema.Fonte(20f, FontStyle.Bold);
            _lblValor.ForeColor = Tema.Titulo;
            _lblValor.Height = _lblValor.Font.Height + 4;

            _lblRotulo.Font = Tema.Fonte(11f, FontStyle.Bold);
            _lblRotulo.ForeColor = Tema.Texto;
            _lblRotulo.Height = _lblRotulo.Font.Height + 2;

            _lblDetalhe.Font = Tema.Fonte(11f);
            _lblDetalhe.ForeColor = Tema.TextoSuave;
            _lblDetalhe.Height = _lblDetalhe.Font.Height + 2;

            Size = new Size(
                Tema.Escalar(_larguraBase),
                Padding.Vertical + _lblValor.Height + _lblRotulo.Height + _lblDetalhe.Height);
        }
    }
}
