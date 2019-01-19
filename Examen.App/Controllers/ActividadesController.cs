using Examen.App.Util.Seguridad;
using Examen.Dominio.Abstracto;
using Examen.Dominio.Entidades;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Examen.App.Controllers
{
    [RoutePrefix("api/actividades")]
    [Authorize(Roles = TiposRole.SuperAdmin + "," + TiposRole.Admin)]
    public class ActividadesController : ApiController
    {
       
    }
}
