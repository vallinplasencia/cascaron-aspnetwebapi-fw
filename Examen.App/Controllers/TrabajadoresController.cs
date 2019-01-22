using Examen.App.Util;
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
    [RoutePrefix("api/trabajadores")]
    public class TrabajadoresController : ApiController
    {
        private ITrabajadorRepo repo;

        public TrabajadoresController(ITrabajadorRepo repo)
        {
            this.repo = repo;
        }

        // GET: api/Trabajadores
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(List<Trabajador>))]
        public async Task<IHttpActionResult> GetTrabajadores(
            [FromUri]int _pagina = 1,
            [FromUri]int _limite = 50,
            [FromUri]string _ordenar = "nombre",
            [FromUri]string _orden = "asc",
            [FromUri]string _filtro = null
         )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //_orden = _orden.ToLower();
            string[] camposOrdenar = { "nombre", "valor" };
            string[] ordenValores = { "asc", "desc" };

            if (
                !camposOrdenar.Any(campo => campo == _ordenar)
                || !ordenValores.Any(ordValor => ordValor == _orden)
                || _pagina < 1 || _limite < 1)
            {
                ModelState.AddModelError("error", "Valores incorrectos en la query string");
                return BadRequest(ModelState);
            }
            var trabajadores = await repo.ListarAsync(_pagina - 1, _limite, _ordenar, _orden, _filtro);

            //Access-Control-Expose-Headers: Access-Control-Allow-Origin
            //Access-Control-Expose-Headers   Access-Control-Allow-Origin
            var resp = Request.CreateResponse<List<Trabajador>>(HttpStatusCode.OK, trabajadores);
            resp.Headers.Add(Urls.HEADER_ACCESS_CONTROL_EXPOSE, Urls.MY_HEADER_TOTAL_COUNT);
            resp.Headers.Add(Urls.MY_HEADER_TOTAL_COUNT, repo.TotalTrabajadores(_filtro).ToString());
            //return resp;
            return ResponseMessage(resp);
        }

        // GET: api/Trabajadores/5
        [HttpGet]
        [Route("{id}", Name = "GetTrabajador")]
        [ResponseType(typeof(Trabajador))]
        public async Task<IHttpActionResult> GetTrabajador(string id)
        {
            Trabajador trabajador = await repo.GetTrabajadorAsync(id);
            if (trabajador == null)
            {
                return NotFound();
            }

            return Ok(trabajador);
        }

        // PUT: api/Trabajadores/5
        [HttpPut]
        [Route("{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTrabajador(string id, Trabajador trabajador)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != trabajador.UserId)
            {
                ModelState.AddModelError("error", "Los id no coinciden");
                return BadRequest(ModelState);
            }
            var trabajadorActual = await repo.GetTrabajadorAsync(id);

            if (trabajadorActual == null)
            {
                return NotFound();
            }
            int rowAffectadas = await repo.SalvarAsync(trabajador, trabajadorActual);

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

        // POST: api/Trabajadores
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(Trabajador))]
        public async Task<IHttpActionResult> PostTrabajador(Trabajador trabajador)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await repo.SalvarAsync(trabajador) < 1)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
            //return CreatedAtAction("GetTrabajador", new { id = trabajador.Id }, trabajador);
            return CreatedAtRoute("GetTrabajador", new { id = trabajador.UserId }, trabajador);

        }

        // DELETE: api/Trabajadores/5
        [HttpDelete]
        [Route("{id}")]
        [ResponseType(typeof(Trabajador))]
        public async Task<IHttpActionResult> DeleteTrabajador(string id)
        {
            var trabajador = await repo.GetTrabajadorAsync(id);

            if (trabajador == null)
            {
                return NotFound();
            }
            trabajador = await repo.RemoverAsync(trabajador);

            if (trabajador == null)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
            return Ok(trabajador);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
