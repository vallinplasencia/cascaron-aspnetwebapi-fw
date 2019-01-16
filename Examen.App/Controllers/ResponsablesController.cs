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
    [RoutePrefix("api/responsables")]
    [Authorize(Roles = TiposRole.SuperAdmin + "," + TiposRole.Admin)]
    public class ResponsablesController : ApiController
    {
        private IResponsableRepo repo;

        public ResponsablesController(IResponsableRepo repo) : base()
        {
            this.repo = repo;
        }

        // GET: api/Responsables
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(List<Responsable>))]
        public async Task<IHttpActionResult> GetResponsables(
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
            string[] camposOrdenar = { "nombre", "email" };
            string[] ordenValores = { "asc", "desc" };

            if (
                !camposOrdenar.Any(campo => campo == _ordenar)
                || !ordenValores.Any(ordValor => ordValor == _orden)
                || _pagina < 1 || _limite < 1)
            {
                ModelState.AddModelError("error", "Valores incorrectos en la query string");
                return BadRequest(ModelState);
            }
            var responsables = await repo.ListarAsync(_pagina - 1, _limite, _ordenar, _orden, _filtro);

            var resp = Request.CreateResponse<List<Responsable>>(HttpStatusCode.OK, responsables);
            resp.Headers.Add(Urls.HEADER_ACCESS_CONTROL_EXPOSE, Urls.MY_HEADER_TOTAL_COUNT);
            resp.Headers.Add(Urls.MY_HEADER_TOTAL_COUNT, repo.TotalResponsables(_filtro).ToString());
            //return resp;
            return ResponseMessage(resp);
        }

        // GET: api/Responsables/5
        [HttpGet]
        [Route("{id}", Name = "GetResponsable")]
        [ResponseType(typeof(Responsable))]
        public async Task<IHttpActionResult> GetResponsable(int id)
        {
            Responsable responsable = await repo.GetResponsableAsync(id);
            if (responsable == null)
            {
                return NotFound();
            }

            return Ok(responsable);
        }

        // PUT: api/Responsables/5
        [HttpPut]
        [Route("{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutResponsable(int id, Responsable responsable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != responsable.Id)
            {
                ModelState.AddModelError("error", "Los id no coinciden");
                return BadRequest(ModelState);
            }
            var responsableActual = await repo.GetResponsableAsync(id);

            if (responsableActual == null)
            {
                return NotFound();
            }
            int rowAffectadas = await repo.SalvarAsync(responsable, responsableActual);

            switch (rowAffectadas)
            {
                case 0:
                    ModelState.AddModelError("error", "Los datos enviados son los mismos que los que se encuenetran guardados");
                    return BadRequest(ModelState);
                case -1:
                    return StatusCode(HttpStatusCode.InternalServerError);
            }
            return StatusCode(HttpStatusCode.NoContent);


            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //if (id != responsable.Id)
            //{
            //    return BadRequest();
            //}

            //db.Entry(responsable).State = EntityState.Modified;

            //try
            //{
            //    await db.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!ResponsableExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Responsables
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(Responsable))]
        public async Task<IHttpActionResult> PostResponsable(Responsable responsable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await repo.SalvarAsync(responsable) < 1)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
            //return CreatedAtAction("GetResponsable", new { id = responsable.Id }, responsable);
            return CreatedAtRoute("GetResponsable", new { id = responsable.Id }, responsable);

        }

        // DELETE: api/Responsables/5
        [HttpDelete]
        [Route("{id}")]
        [ResponseType(typeof(Responsable))]
        public async Task<IHttpActionResult> DeleteResponsable(int id)
        {
            var responsable = await repo.GetResponsableAsync(id);

            if (responsable == null)
            {
                return NotFound();
            }
            responsable = await repo.RemoverAsync(responsable);

            if (responsable == null)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
            return Ok(responsable);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }
}
