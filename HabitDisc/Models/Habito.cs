using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitDisc.Models
{
    [Table("habitos")]
    public class Habito
    {
        [Key]
        [Column("id")]
        public long IdHabito { get; private set; }

        [Required]
        [MaxLength(100)]
        [Column("nome")]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("icone")]
        public string Icone { get; set; } = string.Empty;

        // Cor em hexadecimal, ex.: "#468CC8" (use o CorHelper para converter)
        [Required]
        [MaxLength(7)]
        [Column("cor")]
        public string Cor { get; set; } = string.Empty;

        [Column("id_categoria")]
        public int IdCategoria { get; set; }

        // Preenchido pelo próprio banco (DEFAULT CURRENT_TIMESTAMP)
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("criado_em")]
        public DateTime CriadoEm { get; private set; }

        // Navegação
        [ForeignKey(nameof(IdCategoria))]
        public Categoria? Categoria { get; set; }

        public ICollection<Checkin> Checkins { get; set; } = new List<Checkin>();

        public ICollection<Lembrete> Lembretes { get; set; } = new List<Lembrete>();
    }
}