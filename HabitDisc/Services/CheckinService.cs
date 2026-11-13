using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HabitDisc.Data;
using HabitDisc.Models;

namespace HabitDisc.Services
{
    // Regras de check-in (UC02) e cálculo do streak (UC03 / UC08).
    //
    // Cada método abre o próprio DbContext e o descarta ao terminar (vida curta).
    // Assim nada fica carregado entre uma operação e outra, e ao excluir um hábito
    // quem apaga os check-ins é sempre o ON DELETE CASCADE do banco (ver docs 5.2.6).
    //
    // O streak NÃO é gravado (docs 5.1): é calculado a partir do histórico
    // sempre que alguém precisa dele.
    public class CheckinService
    {
        // ---------- Home (UC02) ----------

        // Todos os hábitos (todos são diários), com o status de hoje e o streak atual.
        public List<HabitoDoDia> ObterHabitosDoDia()
        {
            DateOnly hoje = Hoje();

            using HabitDiscContext ctx = new HabitDiscContext();

            List<Habito> habitos = ctx.Habitos
                .AsNoTracking()
                .Include(h => h.Categoria)
                .OrderBy(h => h.Nome)
                .ToList();

            // Uma única consulta para todos os hábitos (em vez de uma por hábito).
            // Traz só as duas colunas necessárias.
            var concluidos = ctx.Checkins
                .AsNoTracking()
                .Where(c => c.Concluido && c.Data <= hoje)
                .Select(c => new { c.IdHabito, c.Data })
                .ToList();

            Dictionary<long, List<DateOnly>> datasPorHabito = concluidos
                .GroupBy(c => c.IdHabito)
                .ToDictionary(g => g.Key, g => g.Select(c => c.Data).ToList());

            List<HabitoDoDia> lista = new List<HabitoDoDia>();

            foreach (Habito h in habitos)
            {
                List<DateOnly> datas;
                if (!datasPorHabito.TryGetValue(h.IdHabito, out datas!))
                {
                    datas = new List<DateOnly>();
                }

                lista.Add(new HabitoDoDia
                {
                    Id = h.IdHabito,
                    Nome = h.Nome,
                    Icone = h.Icone,
                    Cor = h.Cor,
                    Categoria = h.Categoria?.Nome ?? string.Empty,
                    Concluido = datas.Contains(hoje),
                    Streak = CalcularStreakAtual(datas, hoje)
                });
            }

            return lista;
        }

        // Marca (concluido = true) ou desmarca (false) o check-in de HOJE.
        // Desmarcar não apaga a linha: só muda "concluido" (docs 5.2.3),
        // e o trigger de UPDATE registra a mudança no log (RF08).
        // Devolve o streak atualizado, para a tela mostrar na hora.
        public int DefinirCheckin(long idHabito, bool concluido)
        {
            DateOnly hoje = Hoje();

            using HabitDiscContext ctx = new HabitDiscContext();

            // (id_habito, data) é único: no máximo uma linha por hábito por dia
            Checkin? checkin = ctx.Checkins
                .FirstOrDefault(c => c.IdHabito == idHabito && c.Data == hoje);

            if (checkin == null)
            {
                // Sem linha hoje e pediram para desmarcar: não há o que fazer
                if (concluido)
                {
                    ctx.Checkins.Add(new Checkin
                    {
                        IdHabito = idHabito,
                        Data = hoje,
                        Concluido = true
                    });
                    ctx.SaveChanges();
                }
            }
            else if (checkin.Concluido != concluido)
            {
                checkin.Concluido = concluido;
                ctx.SaveChanges();
            }

            return CalcularStreakAtual(ObterDatasConcluidas(ctx, idHabito), hoje);
        }

        // ---------- Detalhes do hábito (UC03 / UC08) ----------

        // Streak atual e melhor recorde de um hábito
        public (int Atual, int Recorde) ObterStreak(long idHabito)
        {
            using HabitDiscContext ctx = new HabitDiscContext();

            List<DateOnly> datas = ObterDatasConcluidas(ctx, idHabito);

            return (CalcularStreakAtual(datas, Hoje()), CalcularRecorde(datas));
        }

        // ---------- Cálculos (não acessam o banco) ----------

        // Dias seguidos terminando hoje.
        // Se ainda não fez hoje, o streak não "quebra" até o dia acabar:
        // a contagem começa por ontem. Se também não fez ontem, o streak é 0 (UC03).
        public static int CalcularStreakAtual(IEnumerable<DateOnly> datasConcluidas, DateOnly hoje)
        {
            HashSet<DateOnly> datas = new HashSet<DateOnly>(datasConcluidas);

            DateOnly dia = datas.Contains(hoje) ? hoje : hoje.AddDays(-1);

            int streak = 0;
            while (datas.Contains(dia))
            {
                streak++;
                dia = dia.AddDays(-1);
            }

            return streak;
        }

        // Maior sequência de dias seguidos em todo o histórico
        public static int CalcularRecorde(IEnumerable<DateOnly> datasConcluidas)
        {
            int melhor = 0;
            int atual = 0;
            DateOnly? anterior = null;

            foreach (DateOnly d in datasConcluidas.Distinct().OrderBy(d => d))
            {
                if (anterior != null && d == anterior.Value.AddDays(1))
                {
                    atual++;
                }
                else
                {
                    atual = 1;
                }

                melhor = Math.Max(melhor, atual);
                anterior = d;
            }

            return melhor;
        }

        // ---------- Auxiliares ----------

        // O índice único (id_habito, data) atende exatamente esta consulta (RNF05)
        private static List<DateOnly> ObterDatasConcluidas(HabitDiscContext ctx, long idHabito)
        {
            DateOnly hoje = Hoje();

            return ctx.Checkins
                .AsNoTracking()
                .Where(c => c.IdHabito == idHabito && c.Concluido && c.Data <= hoje)
                .Select(c => c.Data)
                .ToList();
        }

        // "Hoje" vem do relógio do Windows na hora do cálculo; nada é guardado
        private static DateOnly Hoje()
        {
            return DateOnly.FromDateTime(DateTime.Today);
        }
    }
}
