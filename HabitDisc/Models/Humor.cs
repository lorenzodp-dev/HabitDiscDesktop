using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitDisc.Models
{
    [Table("humores")]
    public class Humor
    {
        [Key]
        [Column("id")]
        public long IdHumor { get; private set; }

        // Coluna DATE do banco (um registro por dia)
        [Column("data")]
        public DateOnly Data { get; set; }

        // Escala de 1 (Muito Ruim) a 5 (Excelente)
        [Range(1, 5)]
        [Column("valor")]
        public int Valor { get; set; }

        [Column("observacao")]
        public string? Observacao { get; set; }
    }
}