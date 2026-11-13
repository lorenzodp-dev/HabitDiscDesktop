using System;
using System.Drawing;

namespace HabitDisc.Helpers
{
    // Converte entre o texto hexadecimal guardado no banco (ex.: "#468CC8")
    // e o tipo Color usado pelos controles do Windows Forms.
    public static class CorHelper
    {
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