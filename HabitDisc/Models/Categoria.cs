using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitDisc.Models
{
    [Table("categorias")]
    public class Categoria
    {
        [Key]
        [Column("id")]
        public int IdCategoria { get; private set; }

        [Required]
        [MaxLength(50)]
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

        // true = categoria predefinida do sistema; false = criada pelo usuário
        [Column("predefinida")]
        public bool Predefinida { get; set; }

        // Navegação: hábitos que usam esta categoria
        public ICollection<Habito> Habitos { get; set; } = new List<Habito>();
    }
}