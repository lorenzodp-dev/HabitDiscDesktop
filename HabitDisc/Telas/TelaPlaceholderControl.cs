using System;
using System.Drawing;
using System.Windows.Forms;
using HabitDisc.Helpers;

namespace HabitDisc.Telas
{
    // Tela provisória: ocupa o lugar das seções que ainda não foram criadas.
    // Quando a tela real existir, basta trocá-la no MainForm e apagar este arquivo.
    public class TelaPlaceholderControl : UserControl
    {
        private readonly Label _lbl = new Label();

        public TelaPlaceholderControl(string titulo)
        {
            _lbl.Dock = DockStyle.Fill;
            _lbl.Text = titulo + "\r\n(em construção)";
            _lbl.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(_lbl);

            AplicarTema();
            Tema.Alterado += Tema_Alterado;
            Disposed += delegate { Tema.Alterado -= Tema_Alterado; };
        }

        private void Tema_Alterado(object? sender, EventArgs e)
        {
            AplicarTema();
        }

        private void AplicarTema()
        {
            BackColor = Tema.Fundo;
            _lbl.Font = Tema.Fonte(20f);
            _lbl.ForeColor = Tema.Titulo;
        }
    }
}
