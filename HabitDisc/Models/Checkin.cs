using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitDisc.Models
{
    [Table("checkins")]
    public class Checkin
    {
        [Key]
        [Column("id")]
        public long IdCheckin { get; private set; }

        [Column("id_habito")]
        public long IdHabito { get; set; }

        // Coluna DATE do banco
        [Column("data")]
        public DateOnly Data { get; set; }

        [Column("concluido")]
        public bool Concluido { get; set; } = true;

        // Navegação
        [ForeignKey(nameof(IdHabito))]
        public Habito? Habito { get; set; }
    }
}