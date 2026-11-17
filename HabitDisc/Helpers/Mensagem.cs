using System;
using System.Windows.Forms;

namespace HabitDisc.Helpers
{
    // Caixas de mensagem padronizadas: mesmo título e mesmos ícones em todo o app.
    public static class Mensagem
    {
        private const string Titulo = "HabitDisc";

        public static void Aviso(string texto)
        {
            MessageBox.Show(texto, Titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void Erro(string texto)
        {
            MessageBox.Show(texto, Titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Pergunta Sim/Não. O "Não" já vem selecionado, para ninguém excluir algo
        // por engano apertando Enter.
        public static bool Confirmar(string texto)
        {
            DialogResult resposta = MessageBox.Show(texto, Titulo, MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            return resposta == DialogResult.Yes;
        }

        // Erro que não é regra do sistema: quase sempre o banco fora do ar
        public static void ErroInesperado(Exception ex)
        {
            Erro("Não foi possível concluir a operação.\n\n" +
                 "Verifique se o MySQL está ligado e se o appsettings.json está configurado.\n\n" +
                 "Detalhe técnico: " + ex.GetBaseException().Message);
        }
    }
}
