using System;
using System.Drawing;

namespace HabitDisc.Helpers
{
    // Converte entre o texto hexadecimal guardado no banco (ex.: "#468CC8")
    // e o tipo Color usado pelos controles do Windows Forms.
    public static class CorHelper
    {
        // Cores oferecidas na escolha de cor de hábitos e categorias.
        // As 5 primeiras são as das categorias predefinidas (script.sql).
        public static readonly IReadOnlyList<string> CoresSugeridas = new List<string>
        {
            "#468CC8", "#5FAE7A", "#966EBE", "#D2646E", "#DCA03C",
            "#3AAFA9", "#E07B39", "#C25B9E", "#8C6E4E", "#7A8B99",
        };

        // Banco -> tela. Se o texto for vazio ou inválido, devolve cinza
        // em vez de quebrar a tela.
        public static Color HexParaCor(string? hex)
        {
            if (string.IsNullOrWhiteSpace(hex))
            {
                return Color.Gray;
            }

            try
            {
                return ColorTranslator.FromHtml(hex);
            }
            catch (Exception)
            {
                return Color.Gray;
            }
        }

        // Tela -> banco. Sempre devolve "#RRGGBB".
        // Não use ColorTranslator.ToHtml aqui: ele devolve o NOME da cor
        // quando ela tem um (ex.: "Red"), e a coluna espera o formato HEX.
        public static string CorParaHex(Color cor)
        {
            return "#" + cor.R.ToString("X2") + cor.G.ToString("X2") + cor.B.ToString("X2");
        }
    }
}