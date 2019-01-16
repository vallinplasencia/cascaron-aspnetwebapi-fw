using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Examen.App.Util
{
    public class Urls
    { /*******************----------MIS HEADERS------------*******************/

        /// <summary>
        /// Total de item q hay en un listado.
        /// </summary>
        public static readonly string MY_HEADER_TOTAL_COUNT = "x-total-count";



        /// <summary>
        /// Total de items en un listar pero este listar va a ser un listar SECUNDARIO
        /// es decir se va a utilizar dentro de un formulario para seleccionarce como
        /// campo de algun item q se este guardando.
        /// </summary>
        public static readonly string MY_HEADER_TOTAL_COUNT_SEC_RESPONSABLES = "x-total-count-responsables";



        /*******************----------HEADERS OFICIALES------------*******************/

        /// <summary>
        /// Header oficial para exponer cabeceras
        /// </summary>
        public static readonly string HEADER_ACCESS_CONTROL_EXPOSE = "Access-Control-Expose-Headers";
    }
}