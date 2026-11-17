using System;
using System.Collections.Generic;

namespace HabitDisc.Services
{
    // Tudo o que a janela de detalhes de um hábito mostra (UC03 / UC08).
    // Não é uma tabela: é montado pelo HabitoService.ObterDetalhes.
    public class HabitoDetalhes
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Icone { get; set; } = string.Empty;
        public string Cor { get; set; } = string.Empty;
        public int IdCategoria { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public DateTime CriadoEm { get; set; }

        public int StreakAtual { get; set; }
        public int Recorde { get; set; }
        public int TotalConcluidos { get; set; }

        // Datas em que o hábito foi concluído (para o histórico recente)
        public HashSet<DateOnly> DiasConcluidos { get; set; } = new HashSet<DateOnly>();
    }
}
