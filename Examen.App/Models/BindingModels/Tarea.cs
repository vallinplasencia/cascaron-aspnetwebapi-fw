using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Examen.App.Models.BindingModels
{
    public class TareaNuevaBM
    {
        [Required(ErrorMessage = "El {0} es obligatorio")]
        [MaxLength(100, ErrorMessage = "El {0} puede tener hasta {1} caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [Range(1, 100, ErrorMessage = "El {0} debe de estar entre {1} y {2}")]
        public int Porcentaje { get; set; }
    }

    public class TareaEditarBM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [MaxLength(100, ErrorMessage = "El {0} puede tener hasta {1} caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [Range(1, 100, ErrorMessage = "El {0} debe de estar entre {1} y {2}")]
        public int Porcentaje { get; set; }

        public bool Realizada { get; set; }
    }
}