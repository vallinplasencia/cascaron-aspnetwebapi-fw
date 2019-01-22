using Examen.Dominio.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Dominio.Entidades
{
    [Table("actividades")]
    public class Actividad
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [MaxLength(100, ErrorMessage = "El {0} puede tener hasta {1} caracteres")]
        public string Titulo {get;set;}

        [MaxLength(500, ErrorMessage = "El {0} puede tener hasta {1} caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        public EstadosActividad Estado { get; set; }

        [Required(ErrorMessage = "la fecha de registro es obligatorio")]
        [Display(Description = "Fecha de registro")]
        public DateTime FechaRegistro { get; set; }

        //*****Relaciones****

        //Relacion q representa que Trabajador CREO esta Actividad
        [Required(ErrorMessage = "El {0} es obligatorio")]
        public string CreadaPorId { get; set; }
        [ForeignKey("CreadaPorId")]        
        [Display(Description = "Creador")]
        public Trabajador CreadaPor { get; set; }

        //Relacion q representa a q Trabajador se ASIGNO esta Actividad
        public string AsignadaAId { get; set; }
        [ForeignKey("AsignadaAId")]
        [Display(Description = "Asignada a")]
        public Trabajador AsignadaA { get; set; }

        //Relacion 1-m con Tareas
        public List<Tarea> Tareas { get; set; }


        [Timestamp]
        public byte[] RowVersion { get; set; }
        
    }
}
