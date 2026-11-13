using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitDisc.Models
{
    [Table("lembretes")]
    public class Lembrete
    {
        [Key]
        [Column("id")]
        public long IdLembrete { get; private set; }

        [Column("id_habito")]
        public long IdHabito { get; set; }

        // Coluna TIME do banco
        [Column("horario")]
        public TimeOnly Horario { get; set; }

        [Column("ativo")]
        public bool Ativo { get; set; } = true;

        // Navegação
        [ForeignKey(nameof(IdHabito))]
        public Habito? Habito { get; set; }
    }
}