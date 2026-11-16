using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HabitDisc.Helpers
{
    // Deixa a barra de título da janela (título + botões minimizar/maximizar/fechar)
    // clara ou escura.
    //
    // Essa barra não é desenhada pelo WinForms, e sim pelo Windows (DWM). Por isso não
    // existe propriedade no Form para isso: é preciso chamar a API do Windows
    // DwmSetWindowAttribute (P/Invoke).
    // Funciona no Windows 10 (versão 1809 ou mais nova) e no Windows 11 (RNF04).
    // Em versões que não suportam, a chamada só falha em silêncio e a barra fica clara.
    public static class BarraDeTitulo
    {
        // Código do atributo "modo escuro" da barra de título.
        // Windows 10 20H1 em diante e Windows 11 usam 20; versões 1809 a 1909 usavam 19.
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_ANTIGO = 19;

        // Flags do SetWindowPos: não mexe em posição/tamanho, só pede para redesenhar a moldura
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_NOACTIVATE = 0x0010;
        private const uint SWP_FRAMECHANGED = 0x0020;

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int atributo, ref int valor, int tamanho);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hwnd, IntPtr depoisDe, int x, int y, int largura, int altura, uint flags);

        public static void AplicarModoEscuro(Form form, bool escuro)
        {
            // A janela ainda não existe no Windows (antes do HandleCreated): nada a fazer.
            // O MainForm chama este método de novo quando o handle é criado.
            if (!form.IsHandleCreated)
            {
                return;
            }

            int valor = escuro ? 1 : 0;

            // 0 = sucesso. Se a versão nova não for aceita, tenta a antiga.
            int resultado = DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref valor, sizeof(int));
            if (resultado != 0)
            {
                DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE_ANTIGO, ref valor, sizeof(int));
            }

            // Com a janela já aberta, o Windows só repinta a barra quando algo muda nela:
            // isto força o redesenho, para a troca aparecer na hora.
            SetWindowPos(form.Handle, IntPtr.Zero, 0, 0, 0, 0,
                SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE | SWP_FRAMECHANGED);
        }
    }
}
