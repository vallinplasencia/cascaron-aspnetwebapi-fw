using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Dominio.Entidades
{
    [Table("activos")]
    public class Activo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El {0} del activo es obligatorio")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "El {0} debe de tener entre {1} y {2} caracteres")]
        public string Nombre { get; set; }

        [Range(1, 100, ErrorMessage = "La cantidad de {0} debe de estar entre {1} y {2}")]
        public int Unidades { get; set; }

        public bool EsPrincipal { get; set; }

        [Required(ErrorMessage = "La fecha de alta es obligatoria")]
        [Display(Description = "Fecha de Alta")]
        public DateTime FechaAlta { get; set; }

        [Display(Description = "Fecha de Baja")]
        public DateTime? FechaBaja { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }



        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }

        public int? ResponsableId { get; set; }
        [ForeignKey("ResponsableId")]
        public Responsable Responsable { get; set; }
    }
}
