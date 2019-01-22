using Examen.Dominio.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Examen.App.Models.BindingModels
{
    /// <summary>
    /// Contiene los datos q se envian del frontend al darle de alta a una actividad
    /// </summary>
    public class ActividadNuevaBM
    {
        [Required(ErrorMessage = "El {0} es obligatorio")]
        [MaxLength(100, ErrorMessage = "El {0} puede tener hasta {1} caracteres")]
        public string Titulo { get; set; }

        [MaxLength(500, ErrorMessage = "El {0} puede tener hasta {1} caracteres")]
        public string Descripcion { get; set; }
        
        [Required(ErrorMessage = "Debe asignarle la actividad a un atrabajador")]
        public string TrabajadorId { get; set; }

        public TareaNuevaBM[] Tareas { get; set; }
    }


    /// <summary>
    /// Contiene los datos q se envian del frontend al EDITAR a una actividad
    /// </summary>
    public class ActividadEditarBM
    {
        [Required(ErrorMessage = "El {0} es obligatorio")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [MaxLength(100, ErrorMessage = "El {0} puede tener hasta {1} caracteres")]
        public string Titulo { get; set; }

        [MaxLength(500, ErrorMessage = "El {0} puede tener hasta {1} caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        public EstadosActividad Estado { get; set; }

        [Required(ErrorMessage = "la fecha de registro es obligatorio")]
        [Display(Description = "Fecha de registro")]
        public DateTime FechaRegistro { get; set; }

        [Required(ErrorMessage = "Debe asignarle la actividad a un atrabajador")]
        public string TrabajadorId { get; set; }

        public TareaEditarBM[] Tareas { get; set; }
    }
}