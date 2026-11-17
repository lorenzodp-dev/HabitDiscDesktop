using System;
using System.Globalization;

namespace HabitDisc.Helpers
{
    // Textos formatados usados em várias telas (datas, porcentagens, streak),
    // para todas escreverem do mesmo jeito.
    public static class Textos
    {
        private static readonly CultureInfo Pt = new CultureInfo("pt-BR");

        public static string Streak(int dias)
        {
            if (dias <= 0) return "sem sequência";
            if (dias == 1) return "1 dia seguido";
            return dias + " dias seguidos";
        }

        public static string Dias(int dias)
        {
            return dias == 1 ? "1 dia" : dias + " dias";
        }

        // "Segunda-feira, 16 de novembro"
        public static string DataPorExtenso(DateTime data)
        {
            return Capitalizar(data.ToString("dddd, d 'de' MMMM", Pt));
        }

        public static string DataPorExtenso(DateOnly data)
        {
            return DataPorExtenso(data.ToDateTime(TimeOnly.MinValue));
        }

        // "16/11/2026"
        public static string DataCurta(DateTime data)
        {
            return data.ToString("dd/MM/yyyy", Pt);
        }

        // "9 a 15 de novembro" ou "27 de outubro a 2 de novembro"
        public static string Intervalo(DateOnly inicio, DateOnly fim)
        {
            if (inicio.Month == fim.Month)
            {
                return inicio.Day + " a " + fim.ToString("d 'de' MMMM", Pt);
            }
            return inicio.ToString("d 'de' MMMM", Pt) + " a " + fim.ToString("d 'de' MMMM", Pt);
        }

        // "Novembro de 2026"
        public static string MesAno(DateOnly data)
        {
            return Capitalizar(data.ToString("MMMM 'de' yyyy", Pt));
        }

        // "seg", "ter"...
        public static string DiaSemanaCurto(DateOnly data)
        {
            return data.ToString("ddd", Pt).TrimEnd('.');
        }

        // 0.72 -> "72%"; nulo -> "—" (não havia nada a fazer)
        public static string Porcentagem(double? fracao)
        {
            if (fracao == null) return "—";
            return Math.Round(fracao.Value * 100) + "%";
        }

        private static string Capitalizar(string texto)
        {
            if (texto.Length == 0) return texto;
            return char.ToUpper(texto[0], Pt) + texto.Substring(1);
        }
    }
}
