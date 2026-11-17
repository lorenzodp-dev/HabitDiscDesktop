using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HabitDisc.Data;
using HabitDisc.Models;

namespace HabitDisc.Services
{
    // Regras das categorias (UC09):
    // - as predefinidas (criadas pelo script do banco) não podem ser editadas nem excluídas;
    // - uma categoria com hábitos vinculados não pode ser excluída
    //   (o banco também bloqueia, com ON DELETE RESTRICT).
    public class CategoriaService
    {
        // Tamanho da coluna categorias.nome (VARCHAR(50))
        public const int TamanhoMaximoNome = 50;

        // Predefinidas primeiro, depois as do usuário, em ordem alfabética
        public List<Categoria> Listar()
        {
            using HabitDiscContext ctx = new HabitDiscContext();

            return ctx.Categorias
                .AsNoTracking()
                .OrderByDescending(c => c.Predefinida)
                .ThenBy(c => c.Nome)
                .ToList();
        }

        // Quantos hábitos usam cada categoria (id da categoria -> quantidade).
        // Categoria sem hábitos não aparece no dicionário.
        public Dictionary<int, int> ContarHabitosPorCategoria()
        {
            using HabitDiscContext ctx = new HabitDiscContext();

            return ctx.Habitos
                .GroupBy(h => h.IdCategoria)
                .Select(g => new { IdCategoria = g.Key, Quantidade = g.Count() })
                .ToDictionary(x => x.IdCategoria, x => x.Quantidade);
        }

        public void Criar(string nome, string icone, string cor)
        {
            string nomeValido = Validar(nome, icone, cor);

            using HabitDiscContext ctx = new HabitDiscContext();
            VerificarNomeRepetido(ctx, nomeValido, idIgnorado: null);

            ctx.Categorias.Add(new Categoria
            {
                Nome = nomeValido,
                Icone = icone,
                Cor = cor.ToUpperInvariant(),
                Predefinida = false
            });
            ctx.SaveChanges();
        }

        public void Atualizar(int idCategoria, string nome, string icone, string cor)
        {
            string nomeValido = Validar(nome, icone, cor);

            using HabitDiscContext ctx = new HabitDiscContext();
            Categoria categoria = BuscarPersonalizada(ctx, idCategoria, "editadas");
            VerificarNomeRepetido(ctx, nomeValido, idIgnorado: idCategoria);

            categoria.Nome = nomeValido;
            categoria.Icone = icone;
            categoria.Cor = cor.ToUpperInvariant();
            ctx.SaveChanges();
        }

        public void Excluir(int idCategoria)
        {
            using HabitDiscContext ctx = new HabitDiscContext();
            Categoria categoria = BuscarPersonalizada(ctx, idCategoria, "excluídas");

            // Fluxo alternativo do UC09
            if (ctx.Habitos.Any(h => h.IdCategoria == idCategoria))
            {
                throw new RegraNegocioException(
                    "A categoria \"" + categoria.Nome + "\" tem hábitos vinculados e não pode ser excluída.\n\n" +
                    "Mude esses hábitos para outra categoria antes de excluí-la.");
            }

            ctx.Categorias.Remove(categoria);
            ctx.SaveChanges();
        }

        // ---------- Auxiliares ----------

        private static string Validar(string nome, string icone, string cor)
        {
            string nomeValido = Validacao.Nome(nome, TamanhoMaximoNome, "da categoria");
            Validacao.Icone(icone);
            Validacao.Cor(cor);
            return nomeValido;
        }

        // Busca a categoria e garante que ela não é predefinida.
        // "acao" completa a frase: "não podem ser editadas" / "não podem ser excluídas"
        private static Categoria BuscarPersonalizada(HabitDiscContext ctx, int idCategoria, string acao)
        {
            Categoria categoria = ctx.Categorias.FirstOrDefault(c => c.IdCategoria == idCategoria)
                ?? throw new RegraNegocioException("Esta categoria não existe mais. Ela pode ter sido excluída.");

            if (categoria.Predefinida)
            {
                throw new RegraNegocioException("Categorias predefinidas não podem ser " + acao + ".");
            }
            return categoria;
        }

        // A comparação usa a collation do banco (utf8mb4_unicode_ci), que não
        // diferencia maiúsculas de minúsculas: "Saúde" e "saúde" contam como iguais.
        private static void VerificarNomeRepetido(HabitDiscContext ctx, string nome, int? idIgnorado)
        {
            bool existe = ctx.Categorias.Any(c => c.Nome == nome && c.IdCategoria != idIgnorado);
            if (existe)
            {
                throw new RegraNegocioException("Já existe uma categoria chamada \"" + nome + "\".");
            }
        }
    }
}
