using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HabitDisc.Data;
using HabitDisc.Models;

namespace HabitDisc.Services
{
    // Regras dos hábitos: listar (Home e tela Hábitos), detalhes (UC03 / UC08),
    // criar (UC01), editar e excluir (UC07).
    // Como no CheckinService, cada método abre e fecha o próprio DbContext.
    public class HabitoService
    {
        // Tamanho da coluna habitos.nome (VARCHAR(100))
        public const int TamanhoMaximoNome = 100;

        // Todos os hábitos, com o status de hoje e o streak atual.
        // Todos os hábitos são diários, então esta também é a lista "do dia" da Home.
        public List<HabitoResumo> Listar()
        {
            DateOnly hoje = CheckinService.Hoje();

            using HabitDiscContext ctx = new HabitDiscContext();

            List<Habito> habitos = ctx.Habitos
                .AsNoTracking()
                .Include(h => h.Categoria)
                .OrderBy(h => h.Nome)
                .ToList();

            // Uma única consulta para os check-ins de todos os hábitos
            // (em vez de uma por hábito), trazendo só as duas colunas necessárias
            var concluidos = ctx.Checkins
                .AsNoTracking()
                .Where(c => c.Concluido && c.Data <= hoje)
                .Select(c => new { c.IdHabito, c.Data })
                .ToList();

            Dictionary<long, List<DateOnly>> datasPorHabito = concluidos
                .GroupBy(c => c.IdHabito)
                .ToDictionary(g => g.Key, g => g.Select(c => c.Data).ToList());

            List<HabitoResumo> lista = new List<HabitoResumo>();

            foreach (Habito h in habitos)
            {
                List<DateOnly> datas;
                if (!datasPorHabito.TryGetValue(h.IdHabito, out datas!))
                {
                    datas = new List<DateOnly>();
                }

                lista.Add(new HabitoResumo
                {
                    Id = h.IdHabito,
                    Nome = h.Nome,
                    Icone = h.Icone,
                    Cor = h.Cor,
                    IdCategoria = h.IdCategoria,
                    Categoria = h.Categoria?.Nome ?? string.Empty,
                    Concluido = datas.Contains(hoje),
                    Streak = CheckinService.CalcularStreakAtual(datas, hoje)
                });
            }

            return lista;
        }

        public HabitoDetalhes ObterDetalhes(long idHabito)
        {
            using HabitDiscContext ctx = new HabitDiscContext();

            Habito habito = ctx.Habitos
                .AsNoTracking()
                .Include(h => h.Categoria)
                .FirstOrDefault(h => h.IdHabito == idHabito)
                ?? throw new RegraNegocioException("Este hábito não existe mais. Ele pode ter sido excluído.");

            List<DateOnly> datas = CheckinService.ObterDatasConcluidas(ctx, idHabito);

            return new HabitoDetalhes
            {
                Id = habito.IdHabito,
                Nome = habito.Nome,
                Icone = habito.Icone,
                Cor = habito.Cor,
                IdCategoria = habito.IdCategoria,
                Categoria = habito.Categoria?.Nome ?? string.Empty,
                CriadoEm = habito.CriadoEm,
                StreakAtual = CheckinService.CalcularStreakAtual(datas, CheckinService.Hoje()),
                Recorde = CheckinService.CalcularRecorde(datas),
                TotalConcluidos = datas.Count,
                DiasConcluidos = new HashSet<DateOnly>(datas)
            };
        }

        // UC01. Nome vazio vira aviso (fluxo alternativo do UC01).
        public void Criar(string nome, string icone, string cor, int idCategoria)
        {
            string nomeValido = Validacao.Nome(nome, TamanhoMaximoNome, "do hábito");
            Validacao.Icone(icone);
            Validacao.Cor(cor);

            using HabitDiscContext ctx = new HabitDiscContext();
            ValidarCategoria(ctx, idCategoria);

            // criado_em é preenchido pelo próprio banco (DEFAULT CURRENT_TIMESTAMP)
            ctx.Habitos.Add(new Habito
            {
                Nome = nomeValido,
                Icone = icone,
                Cor = cor.ToUpperInvariant(),
                IdCategoria = idCategoria
            });
            ctx.SaveChanges();
        }

        // UC07 (editar)
        public void Atualizar(long idHabito, string nome, string icone, string cor, int idCategoria)
        {
            string nomeValido = Validacao.Nome(nome, TamanhoMaximoNome, "do hábito");
            Validacao.Icone(icone);
            Validacao.Cor(cor);

            using HabitDiscContext ctx = new HabitDiscContext();
            ValidarCategoria(ctx, idCategoria);

            Habito habito = ctx.Habitos.FirstOrDefault(h => h.IdHabito == idHabito)
                ?? throw new RegraNegocioException("Este hábito não existe mais. Ele pode ter sido excluído.");

            habito.Nome = nomeValido;
            habito.Icone = icone;
            habito.Cor = cor.ToUpperInvariant();
            habito.IdCategoria = idCategoria;
            ctx.SaveChanges();
        }

        // UC07 (excluir). Os check-ins e lembretes do hábito são apagados pelo
        // próprio banco (ON DELETE CASCADE).
        // ExecuteDelete manda um único DELETE, sem carregar nada na memória: assim o
        // log (RF08) registra só a exclusão do hábito, como descreve o docs (5.2.6).
        public void Excluir(long idHabito)
        {
            using HabitDiscContext ctx = new HabitDiscContext();
            ctx.Habitos.Where(h => h.IdHabito == idHabito).ExecuteDelete();
        }

        private static void ValidarCategoria(HabitDiscContext ctx, int idCategoria)
        {
            if (!ctx.Categorias.Any(c => c.IdCategoria == idCategoria))
            {
                throw new RegraNegocioException("Escolha uma categoria.");
            }
        }
    }
}
