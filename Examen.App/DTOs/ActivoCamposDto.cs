using Examen.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Examen.App.DTOs
{
    /// <summary>
    /// Clase utilizada para enviar datos al FrontEnd. Cuando se va a dar de alta o modificar un ACTIVO.
    /// </summary>
    public class ActivoCamposDto
    {
        /// <summary>
        /// Se rellena este campo para enviarse al FrontEnd cuando se va a modificar un ACTIVO.
        /// </summary>
        public Activo Activo { get; set; }

        /// <summary>
        /// Siempre se rellena este campo. Contiene las posibles categorias a asignarle al ACTIVO.
        /// </summary>
        public List<Categoria> Categorias { get; set; }

        /// <summary>
        /// Siempre se rellena este campo. Contiene los posibles resposables a asignarle al ACTIVO.
        /// </summary>
        public List<Responsable> Responsables { get; set; }
    }
}