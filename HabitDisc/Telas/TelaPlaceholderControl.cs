using System.Drawing;
using System.Windows.Forms;

namespace HabitDisc.Telas
{
    // Tela provisória: ocupa o lugar das seções que ainda não foram criadas.
    // Quando a tela real existir, basta trocá-la no MainForm e apagar este arquivo.
    public class TelaPlaceholderControl : UserControl
    {
        public TelaPlaceholderControl(string titulo)
        {
            BackColor = Color.FromArgb(246, 244, 236);

            Label lbl = new Label();
            lbl.Dock = DockStyle.Fill;
            lbl.Text = titulo + "\r\n(em construção)";
            lbl.Font = new Font("Newsreader", 20f, FontStyle.Regular);
            lbl.ForeColor = Color.FromArgb(35, 77, 51);
            lbl.TextAlign = ContentAlignment.MiddleCenter;

            Controls.Add(lbl);
        }
    }
}