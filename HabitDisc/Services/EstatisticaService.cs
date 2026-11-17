using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HabitDisc.Data;

namespace HabitDisc.Services
{
    // Estatísticas (UC04 / RF04). Nada é gravado: tudo é calculado a partir de
    // "habitos" (o que existe e desde quando) e "checkins" (o que foi feito).
    //
    // Taxa de conclusão = check-ins concluídos / check-ins esperados, onde
    // "esperados" = para cada hábito, os dias do período desde o dia em que ele
    // foi criado, contando só os dias que já passaram (até hoje).
    // Assim um hábito criado na quarta não "perde" segunda e terça, e uma semana
    // em andamento não parece ruim só porque ainda não acabou.
    public class EstatisticaService
    {
        // UC04: pré-condição de "ao menos uma semana de check-ins registrados"
        public const int DiasMinimosDeDados = 7;

        public ResumoPeriodo Calcular(long? idHabito, DateOnly inicio, DateOnly fim)
        {
            DateOnly hoje = CheckinService.Hoje();

            using HabitDiscContext ctx = new HabitDiscContext();

            var habitos = ctx.Habitos
                .AsNoTracking()
                .Where(h => idHabito == null || h.IdHabito == idHabito)
                .Select(h => new { h.IdHabito, h.CriadoEm })
                .ToList();

            var feitos = ctx.Checkins
                .AsNoTracking()
                .Where(c => c.Concluido && c.Data >= inicio && c.Data <= fim)
                .Where(c => idHabito == null || c.IdHabito == idHabito)
                .Select(c => new { c.IdHabito, c.Data })
                .ToList();

            // Para perguntar rápido "o hábito X foi feito no dia Y?"
            HashSet<(long, DateOnly)> concluidos = new HashSet<(long, DateOnly)>(
                feitos.Select(f => (f.IdHabito, f.Data)));

            ResumoPeriodo resumo = new ResumoPeriodo();

            for (DateOnly dia = inicio; dia <= fim; dia = dia.AddDays(1))
            {
                int esperadosNoDia = 0;
                int feitosNoDia = 0;

                // Dias futuros não contam: não havia nada a fazer ainda
                if (dia <= hoje)
                {
                    foreach (var h in habitos)
                    {
                        if (dia >= DateOnly.FromDateTime(h.CriadoEm))
                        {
                            esperadosNoDia++;
                            if (concluidos.Contains((h.IdHabito, dia)))
                            {
                                feitosNoDia++;
                            }
                        }
                    }
                }

                resumo.Esperados += esperadosNoDia;
                resumo.Feitos += feitosNoDia;
                resumo.Dias.Add(new PontoDia
                {
                    Data = dia,
                    Taxa = esperadosNoDia == 0 ? null : (double)feitosNoDia / esperadosNoDia
                });
            }

            return resumo;
        }

        // Fluxo alternativo do UC04: o primeiro check-in precisa ter pelo menos 7 dias
        public bool TemDadosSuficientes(long? idHabito)
        {
            DateOnly hoje = CheckinService.Hoje();

            using HabitDiscContext ctx = new HabitDiscContext();

            DateOnly? primeiro = ctx.Checkins
                .Where(c => c.Concluido)
                .Where(c => idHabito == null || c.IdHabito == idHabito)
                .Select(c => (DateOnly?)c.Data)
                .Min();

            return primeiro != null && primeiro.Value <= hoje.AddDays(-(DiasMinimosDeDados - 1));
        }

        // ---------- Períodos ----------

        // Semana de segunda a domingo que contém a data
        public static (DateOnly Inicio, DateOnly Fim) Semana(DateOnly referencia)
        {
            // DayOfWeek: domingo = 0, segunda = 1... Quantos dias voltar até a segunda:
            int diasDesdeSegunda = ((int)referencia.DayOfWeek + 6) % 7;
            DateOnly segunda = referencia.AddDays(-diasDesdeSegunda);
            return (segunda, segunda.AddDays(6));
        }

        // Mês inteiro que contém a data
        public static (DateOnly Inicio, DateOnly Fim) Mes(DateOnly referencia)
        {
            DateOnly primeiro = new DateOnly(referencia.Year, referencia.Month, 1);
            return (primeiro, primeiro.AddMonths(1).AddDays(-1));
        }
    }

    // Resultado de um período (semana ou mês)
    public class ResumoPeriodo
    {
        public int Feitos { get; set; }
        public int Esperados { get; set; }

        // Nulo quando não havia nada esperado no período
        public double? Taxa => Esperados == 0 ? null : (double)Feitos / Esperados;

        // Um ponto por dia, para o gráfico
        public List<PontoDia> Dias { get; } = new List<PontoDia>();
    }

    public class PontoDia
    {
        public DateOnly Data { get; set; }

        // De 0 a 1; nulo = dia futuro ou sem hábito existente naquele dia
        public double? Taxa { get; set; }
    }
}
