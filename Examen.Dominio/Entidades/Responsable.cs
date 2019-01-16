using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Dominio.Entidades
{
    [Table("responsables")]
    public class Responsable
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [MaxLength(150, ErrorMessage = "El {0} puede tener hasta {1} caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [EmailAddress(ErrorMessage = "El {0} debe de ser un correo válido")]
        [MaxLength(150, ErrorMessage = "El {0} puede tener hasta {1} caracteres")]
        [Display(Description = "Correo electrónico")]
        public string Email { get; set; }
    }
}
