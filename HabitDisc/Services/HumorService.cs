using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HabitDisc.Data;
using HabitDisc.Models;

namespace HabitDisc.Services
{
    // Registro de humor (UC05): um por dia (humores.data é UNIQUE), valor de 1 a 5
    // e observação opcional. Se já registrou hoje, salvar de novo atualiza o registro
    // (fluxo alternativo do UC05).
    public class HumorService
    {
        // A coluna é TEXT; o limite é só para a observação não virar um texto enorme
        public const int TamanhoMaximoObservacao = 2000;

        public Humor? ObterDeHoje()
        {
            DateOnly hoje = CheckinService.Hoje();

            using HabitDiscContext ctx = new HabitDiscContext();
            return ctx.Humores.AsNoTracking().FirstOrDefault(h => h.Data == hoje);
        }

        public void SalvarHoje(int valor, string? observacao)
        {
            // O EF Core não verifica o [Range(1, 5)] do Model sozinho: a regra fica aqui
            if (valor < 1 || valor > 5)
            {
                throw new RegraNegocioException("Escolha como você está se sentindo (de 1 a 5).");
            }

            string? texto = string.IsNullOrWhiteSpace(observacao) ? null : observacao.Trim();
            if (texto != null && texto.Length > TamanhoMaximoObservacao)
            {
                throw new RegraNegocioException(
                    "A observação pode ter no máximo " + TamanhoMaximoObservacao + " caracteres.");
            }

            DateOnly hoje = CheckinService.Hoje();

            using HabitDiscContext ctx = new HabitDiscContext();
            Humor? humor = ctx.Humores.FirstOrDefault(h => h.Data == hoje);

            if (humor == null)
            {
                ctx.Humores.Add(new Humor { Data = hoje, Valor = valor, Observacao = texto });
            }
            else
            {
                humor.Valor = valor;
                humor.Observacao = texto;
            }
            ctx.SaveChanges();
        }

        // Os registros mais recentes primeiro
        public List<Humor> ListarRecentes(int quantidade)
        {
            using HabitDiscContext ctx = new HabitDiscContext();

            return ctx.Humores
                .AsNoTracking()
                .OrderByDescending(h => h.Data)
                .Take(quantidade)
                .ToList();
        }
    }
}
