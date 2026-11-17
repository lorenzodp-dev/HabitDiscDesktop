using HabitDisc.Forms;
using HabitDisc.Helpers;
using HabitDisc.Services;

namespace HabitDisc
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Erros não tratados nas telas viram uma mensagem, em vez de fechar o app.
            // Precisa vir antes de criar qualquer janela.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (sender, e) =>
            {
                if (e.Exception is RegraNegocioException regra)
                {
                    Mensagem.Aviso(regra.Message);       // regra do sistema: aviso
                }
                else
                {
                    Mensagem.ErroInesperado(e.Exception); // ex.: MySQL desligado
                }
            };

            // Aplica as preferências salvas (cor, fonte, modo escuro) antes de criar as telas
            Tema.Aplicar(new PreferenciasService().Carregar());

            Application.Run(new MainForm());
        }
    }
}
