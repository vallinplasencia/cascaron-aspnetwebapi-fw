using Examen.App.DTOs;
using Examen.App.Util;
using Examen.App.Util.Seguridad;
using Examen.Dominio.Abstracto;
using Examen.Dominio.Entidades;
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
    [RoutePrefix("api/activos")]
    [Authorize(Roles = TiposRole.SuperAdmin + "," + TiposRole.Admin)]
    public class ActivosController : ApiController
    {
        private IActivoRepo repo;
        private readonly ICategoriaRepo repoCategoria;
        private readonly IResponsableRepo repoResponsable;

        public ActivosController(IActivoRepo repo, ICategoriaRepo repoCategoria, IResponsableRepo repoResponsable) : base()
        {
            this.repo = repo;
            this.repoCategoria = repoCategoria;
            this.repoResponsable = repoResponsable;
        }

        // GET: api/Activos
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(List<Activo>))]
        public async Task<IHttpActionResult> GetActivos([FromUri]int _pagina = 1, [FromUri]int _limite = 50,
            [FromUri]string _ordenar = "nombre", [FromUri]string _orden = "asc", [FromUri]string _filtro = null
         )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //_orden = _orden.ToLower();
            string[] camposOrdenar = { "nombre", "unidades", "esPrincipal", "fechaAlta", "fechaBaja", "categoriaNombre", "responsableNombre" };
            string[] ordenValores = { "asc", "desc" };

            if (
                !camposOrdenar.Any(campo => campo == _ordenar)
                || !ordenValores.Any(ordValor => ordValor == _orden)
                || _pagina < 1 || _limite < 1)
            {
                ModelState.AddModelError("error", "Valores incorrectos en la query string");
                return BadRequest(ModelState);
            }
            var activos = await repo.ListarAsync(_pagina - 1, _limite, _ordenar, _orden, _filtro);

            var resp = Request.CreateResponse<List<Activo>>(HttpStatusCode.OK, activos);
            resp.Headers.Add(Urls.HEADER_ACCESS_CONTROL_EXPOSE, Urls.MY_HEADER_TOTAL_COUNT);
            resp.Headers.Add(Urls.MY_HEADER_TOTAL_COUNT, repo.TotalActivos(_filtro).ToString());
            //return resp;
            return ResponseMessage(resp);
        }

        // GET: api/Activos/5
        [HttpGet]
        [Route("{id}", Name = "GetActivo")]
        [ResponseType(typeof(Activo))]
        public async Task<IHttpActionResult> GetActivo(int id)
        {
            Activo activo = await repo.GetActivoAsync(id);
            if (activo == null)
            {
                return NotFound();
            }

            return Ok(activo);
        }

        // PUT: api/Activos/5
        [HttpPut]
        [Route("{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutActivo(int id, Activo activo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != activo.Id)
            {
                ModelState.AddModelError("error", "Los id no coinciden");
                return BadRequest(ModelState);
            }
            var activoActual = await repo.GetActivoAsync(id);

            if (activoActual == null)
            {
                return NotFound();
            }
            int rowAffectadas = await repo.SalvarAsync(activo, activoActual);

            switch (rowAffectadas)
            {
                case 0:
                    ModelState.AddModelError("error", "Los datos enviados son los mismos que los que se encuenetran guardados");
                    return BadRequest(ModelState);
                case -1:
                    return StatusCode(HttpStatusCode.InternalServerError);
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Activos
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(Activo))]
        public async Task<IHttpActionResult> PostActivo(Activo activo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await repo.SalvarAsync(activo) < 1)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
            return CreatedAtRoute("GetActivo", new { id = activo.Id }, activo);
        }

        // DELETE: api/Activos/5
        [HttpDelete]
        [Route("{id}")]
        [ResponseType(typeof(Activo))]
        public async Task<IHttpActionResult> DeleteActivo(int id)
        {
            var activo = await repo.GetActivoAsync(id);

            if (activo == null)
            {
                return NotFound();
            }
            activo = await repo.RemoverAsync(activo);

            if (activo == null)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
            return Ok(activo);
        }


        /********---------OTROS METODO DE ACCION.----------**********/

        /// <summary>
        /// Retorna los posibles valores de los campos que se llenan de la bd para dar de alta a un nuevo activo.
        /// 
        /// Los valores de los parametros del metodo se utilizan cuando se van a cargar 
        /// grandes cantidades de datos para los campos y hay q utilizar paginacion para ello.
        /// 
        /// </summary>
        /// <param name="_paginaResp">Numero de la pagina del paginado de la bd. Generalmente es la primera</param>
        /// <param name="_limiteResp">Cant de registros a retornas</param>
        /// <param name="_ordenarResp">Campo por el q se van a ordenar los resultados</param>
        /// <param name="_ordenResp">Orden asc o desc</param>
        /// <param name="_filtroResp">i se va a aplicar algun criterio de busqueda para los resultados.</param>
        /// <returns></returns>        
        [HttpGet()]
        [Route("activo-campos")]
        [ResponseType(typeof(ActivoCamposDto))]
        public async Task<IHttpActionResult> getCamposDelActivo(
            [FromUri]int _paginaResp = 1,
            [FromUri]int _limiteResp = 50,
            [FromUri]string _ordenarResp = "nombre",
            [FromUri]string _ordenResp = "asc",
            [FromUri]string _filtroResp = null)
        {
            _ordenResp = _ordenResp.ToLower();
            string[] camposOrdenarResp = { "nombre", "email" };
            string[] ordenValores = { "asc", "desc" };

            if (
                !camposOrdenarResp.Any(campo => campo == _ordenarResp)
                || !ordenValores.Any(ordValor => ordValor == _ordenResp)
                || _paginaResp < 1 || _limiteResp < 1)
            {
                ModelState.AddModelError("error", "Valores incorrectos en la query string");
                return BadRequest(ModelState);
            }
            //Response.Headers.Add(Urls.HEADER_TOTAL_COUNT_SEC_RESPONSABLES, repoResponsable.TotalResponsables(_filtroResp).ToString());

            //Categoria lo voy a usar en un select por lo q NO se envian en la request(url) los datos
            //de paginado pues son pocos resultados.
            var categorias = await repoCategoria.ListarAsync(0, -1, "nombre", "asc", null);

            var responsables = await repoResponsable.ListarAsync(_paginaResp - 1, _limiteResp, _ordenarResp, _ordenResp, _filtroResp);

            var resp = Request.CreateResponse<ActivoCamposDto>(HttpStatusCode.OK, new ActivoCamposDto
            {
                Categorias = categorias,
                Responsables = responsables
            });

            resp.Headers.Add(Urls.HEADER_ACCESS_CONTROL_EXPOSE, Urls.MY_HEADER_TOTAL_COUNT_SEC_RESPONSABLES);
            resp.Headers.Add(Urls.MY_HEADER_TOTAL_COUNT_SEC_RESPONSABLES, repoResponsable.TotalResponsables(_filtroResp).ToString());

            return ResponseMessage(resp);
        }



        /// <summary>
        /// Retorna los posibles valores de los campos que se llenan de la bd para modificar activo previamente guardado.
        /// 
        /// Los valores de los parametros del metodo se utilizan cuando se van a cargar 
        /// grandes cantidades de datos para los campos del activo y hay q utilizar paginacion para ello.
        /// 
        /// </summary>
        /// <param name="id">Identificador del item a modificar</param>
        /// <param name="_paginaResp">Numero de la pagina del paginado de la bd. Generalmente es la primera</param>
        /// <param name="_limiteResp">Cant de registros a retornas</param>
        /// <param name="_ordenarResp">Campo por el q se van a ordenar los resultados</param>
        /// <param name="_ordenResp">Orden asc o desc</param>
        /// <param name="_filtroResp">i se va a aplicar algun criterio de busqueda para los resultados.</param>
        /// <returns></returns>
        [HttpGet()]
        [Route("activo-y-campos/{id}")]
        [ResponseType(typeof(ActivoCamposDto))]
        public async Task<IHttpActionResult> GetActivoYCampos(
            [FromUri] int id,
            [FromUri]int _paginaResp = 1,
            [FromUri]int _limiteResp = 50,
            [FromUri]string _ordenarResp = "nombre",
            [FromUri]string _ordenResp = "asc",
            [FromUri]string _filtroResp = null
        )
        {
            var activo = await repo.GetActivoAsync(id);

            if (activo == null)
            {
                return NotFound();
            }
            _ordenResp = _ordenResp.ToLower();
            string[] camposOrdenarResp = { "nombre", "email" };
            string[] ordenValores = { "asc", "desc" };

            if (
                !camposOrdenarResp.Any(campo => campo == _ordenarResp)
                || !ordenValores.Any(ordValor => ordValor == _ordenResp)
                || _paginaResp < 1 || _limiteResp < 1)
            {
                ModelState.AddModelError("error", "Valores incorrectos en la query string");
                return BadRequest(ModelState);
            }
            //Categoria lo voy a usar en un select por lo q NO se envian en la request(url) los datos
            //de paginado pues son pocos resultados.
            var categorias = await repoCategoria.ListarAsync(0, -1, "nombre", "asc", null);

            var responsables = await repoResponsable.ListarAsync(_paginaResp - 1, _limiteResp, _ordenarResp, _ordenResp, _filtroResp);

            var resp = Request.CreateResponse<ActivoCamposDto>(HttpStatusCode.OK, new ActivoCamposDto
            {
                Activo = activo,
                Categorias = categorias,
                Responsables = responsables
            });

            resp.Headers.Add(Urls.HEADER_ACCESS_CONTROL_EXPOSE, Urls.MY_HEADER_TOTAL_COUNT_SEC_RESPONSABLES);
            resp.Headers.Add(Urls.MY_HEADER_TOTAL_COUNT_SEC_RESPONSABLES, repoResponsable.TotalResponsables(_filtroResp).ToString());

            return ResponseMessage(resp);
            //Response.Headers.Add(Urls.HEADER_TOTAL_COUNT_SEC_RESPONSABLES, repoResponsable.TotalResponsables(_filtroResp).ToString());

            //var categorias = await repoCategoria.ListarAsync(0, -1, "nombre", "asc", null);
            //var responsables = await repoResponsable.ListarAsync(_paginaResp - 1, _limiteResp, _ordenarResp, _ordenResp, _filtroResp);

            //return new ActivoCamposDM
            //{
            //    Activo = activo,
            //    Categorias = categorias,
            //    Responsables = responsables
            //};
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
