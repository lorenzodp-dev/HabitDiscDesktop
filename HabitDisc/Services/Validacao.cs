using System.Text.RegularExpressions;

namespace HabitDisc.Services
{
    // Validações repetidas entre os Services (hábitos e categorias).
    // Cada uma lança RegraNegocioException com uma mensagem pronta para o usuário.
    // "internal": só os Services usam; as telas não precisam saber que ela existe.
    internal static class Validacao
    {
        // Devolve o nome sem espaços nas pontas. "doQue" completa a frase:
        // "Informe o nome do hábito." / "Informe o nome da categoria."
        public static string Nome(string? valor, int tamanhoMaximo, string doQue)
        {
            string nome = (valor ?? string.Empty).Trim();

            if (nome.Length == 0)
            {
                throw new RegraNegocioException("Informe o nome " + doQue + ".");
            }
            if (nome.Length > tamanhoMaximo)
            {
                throw new RegraNegocioException(
                    "O nome " + doQue + " pode ter no máximo " + tamanhoMaximo + " caracteres.");
            }
            return nome;
        }

        public static void Icone(string? icone)
        {
            if (string.IsNullOrWhiteSpace(icone))
            {
                throw new RegraNegocioException("Escolha um ícone.");
            }
        }

        // Formato aceito pela coluna "cor" (VARCHAR(7)): "#RRGGBB"
        public static void Cor(string? cor)
        {
            if (cor == null || !Regex.IsMatch(cor, "^#[0-9A-Fa-f]{6}$"))
            {
                throw new RegraNegocioException("Escolha uma cor.");
            }
        }
    }
}
