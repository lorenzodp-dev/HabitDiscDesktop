using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HabitDisc.Models;

namespace HabitDisc.Data
{
    // DbContext do HabitDisc: é a ponte entre as classes de Models e o banco MySQL.
    // Herda de DbContext (do EF Core) e acrescenta as especificações do nosso sistema.
    public class HabitDiscContext : DbContext
    {
        // Cada DbSet representa uma tabela do banco
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Habito> Habitos { get; set; }
        public DbSet<Checkin> Checkins { get; set; }
        public DbSet<Humor> Humores { get; set; }
        public DbSet<Lembrete> Lembretes { get; set; }

        // Lidos uma única vez e reaproveitados por todos os contextos:
        // ler o arquivo e detectar a versão do MySQL (AutoDetect abre uma conexão)
        // a cada "new HabitDiscContext()" deixaria cada operação mais lenta.
        private static string? _conexao;
        private static ServerVersion? _versaoServidor;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);

            if (_conexao == null)
            {
                // A string de conexão fica no appsettings.json (RNF03), e não no código
                IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

                string? conexao = config.GetConnectionString("HabitDisc");

                if (string.IsNullOrWhiteSpace(conexao))
                {
                    throw new InvalidOperationException(
                        "String de conexão 'HabitDisc' não encontrada no appsettings.json.");
                }

                _conexao = conexao;
            }

            _versaoServidor ??= ServerVersion.AutoDetect(_conexao);

            options.UseMySql(_conexao, _versaoServidor);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Categoria com hábitos vinculados NÃO pode ser excluída (UC09).
            // O banco já bloqueia (ON DELETE RESTRICT); aqui dizemos o mesmo ao EF,
            // para ele não tentar apagar os hábitos em cascata.
            modelBuilder.Entity<Habito>()
                .HasOne(h => h.Categoria)
                .WithMany(c => c.Habitos)
                .HasForeignKey(h => h.IdCategoria)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}