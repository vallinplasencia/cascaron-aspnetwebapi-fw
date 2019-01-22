using Examen.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Examen.App.DTOs
{
    /// <summary>
    /// Clase utilizada para enviar datos al FrontEnd. Cuando se va a dar de alta o modificar una ACTIVIDAD.
    /// </summary>
    public class ActividadCamposDto
    {
        /// <summary>
        /// Se rellena este campo para enviarse al FrontEnd cuando se va a modificar una ACTIVIDAD.
        /// </summary>
        public Actividad Actividad { get; set; }

       
        /// <summary>
        /// Siempre se rellena este campo. Contiene los posibles trabajadores a asignarle a la ACTIVIDAD.
        /// </summary>
        public List<Trabajador> Trabajadores { get; set; }
    }
}