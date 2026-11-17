using System;

namespace HabitDisc.Services
{
    // Erro de regra do sistema (ex.: "Informe o nome do hábito.").
    // A mensagem já é escrita para o usuário: a tela pode mostrá-la direto.
    // Serve para separar "o usuário fez algo não permitido" (aviso amarelo)
    // de "deu um erro técnico", como o banco fora do ar (erro vermelho).
    public class RegraNegocioException : Exception
    {
        public RegraNegocioException(string mensagem) : base(mensagem)
        {
        }
    }
}
