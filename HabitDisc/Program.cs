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

            // Aplica as preferências salvas (cor, fonte, modo escuro) antes de criar as telas
            Tema.Aplicar(new PreferenciasService().Carregar());

            Application.Run(new MainForm());
        }
    }
}
