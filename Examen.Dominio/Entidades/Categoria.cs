using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Dominio.Entidades
{
    [Table("categorias")]
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El {0} de la categoria es obligatorio")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "El {0} debe de tener entre {2} y {1} caracteres")]
        public string Nombre { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "El {0} debe de estar entre {1} y {2}")]
        //[Column(TypeName = "decimal(18, 0)")]
        public decimal Valor { get; set; }
    }
}
