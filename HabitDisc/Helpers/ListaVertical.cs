using System.Windows.Forms;

namespace HabitDisc.Helpers
{
    // Prepara um FlowLayoutPanel para mostrar "linhas" uma embaixo da outra
    // (hábitos, categorias, histórico de humor), sempre com a largura do painel
    // e rolando só na vertical.
    public static class ListaVertical
    {
        public static void Configurar(FlowLayoutPanel lista)
        {
            lista.FlowDirection = FlowDirection.TopDown;
            lista.WrapContents = false;

            // Truque do WinForms: com AutoScroll ligado não dá para desligar só a
            // barra horizontal; é preciso desligar, configurar e ligar de novo.
            lista.AutoScroll = false;
            lista.HorizontalScroll.Enabled = false;
            lista.HorizontalScroll.Visible = false;
            lista.HorizontalScroll.Maximum = 0;
            lista.AutoScroll = true;

            // A largura muda quando a janela muda de tamanho e quando a barra
            // vertical aparece ou some
            lista.ClientSizeChanged += delegate { AjustarLarguras(lista); };
            lista.ControlAdded += (sender, e) =>
            {
                if (e.Control != null)
                {
                    e.Control.Width = lista.ClientSize.Width - e.Control.Margin.Horizontal;
                }
            };
        }

        public static void AjustarLarguras(FlowLayoutPanel lista)
        {
            foreach (Control c in lista.Controls)
            {
                c.Width = lista.ClientSize.Width - c.Margin.Horizontal;
            }
        }

        // Remove E descarta as linhas antigas (RemoveAt sozinho não libera da memória)
        public static void Limpar(FlowLayoutPanel lista)
        {
            while (lista.Controls.Count > 0)
            {
                Control c = lista.Controls[0];
                lista.Controls.RemoveAt(0);
                c.Dispose();
            }
        }
    }
}
