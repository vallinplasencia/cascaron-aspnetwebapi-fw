using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Dominio.Entidades
{
    [Table("trabajadores")]
    public class Trabajador
    {
        [Key]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [Required(ErrorMessage ="El {0} es obligatorio")]
        [MaxLength(100, ErrorMessage ="El {0} puede tener hasta {1} caracteres")]
        public string Nombre { get; set; }

        //Cuando el trabajador ya un jefe
        public Trabajador Jefe { get; set; }
        

        //Relacion 1-m. Actividades CREADAS por este trabajador
        [InverseProperty("CreadaPor")]
        public List<Actividad> ActividadesCreadas { get; set; }

        //Relacion 1-m. Actividades ASIGNADAS a este trabajador
        [InverseProperty("AsignadaA")]
        public List<Actividad> ActividadesAsignadas { get; set; }


        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
