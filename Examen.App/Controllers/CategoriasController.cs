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
    //[Authorize(Roles = TiposRole.SuperAdmin + "," + TiposRole.Admin + "," + TiposRole.Usuario)]
    [RoutePrefix("api/categorias")]
    public class CategoriasController : ApiController
    {
        private ICategoriaRepo repo;

        public CategoriasController(ICategoriaRepo repo)
        {
            this.repo = repo;
        }

        // GET: api/Categorias
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(List<Categoria>))]
        public async Task<IHttpActionResult> GetCategorias(
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
            var categorias = await repo.ListarAsync(_pagina - 1, _limite, _ordenar, _orden, _filtro);

            //Access-Control-Expose-Headers: Access-Control-Allow-Origin
            //Access-Control-Expose-Headers   Access-Control-Allow-Origin
            var resp = Request.CreateResponse<List<Categoria>>(HttpStatusCode.OK, categorias);
            resp.Headers.Add(Urls.HEADER_ACCESS_CONTROL_EXPOSE, Urls.MY_HEADER_TOTAL_COUNT);
            resp.Headers.Add(Urls.MY_HEADER_TOTAL_COUNT, repo.TotalCategorias(_filtro).ToString());
            //return resp;
            return ResponseMessage(resp);
        }

        // GET: api/Categorias/5
        [HttpGet]
        [Route("{id}", Name = "GetCategoria")]
        [ResponseType(typeof(Categoria))]
        public async Task<IHttpActionResult> GetCategoria(int id)
        {
            Categoria categoria = await repo.GetCategoriaAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            return Ok(categoria);
        }

        // PUT: api/Categorias/5
        [HttpPut]
        [Route("{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCategoria(int id, Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != categoria.Id)
            {
                ModelState.AddModelError("error", "Los id no coinciden");
                return BadRequest(ModelState);
            }
            var categoriaActual = await repo.GetCategoriaAsync(id);

            if (categoriaActual == null)
            {
                return NotFound();
            }
            int rowAffectadas = await repo.SalvarAsync(categoria, categoriaActual);

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

        // POST: api/Categorias
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(Categoria))]
        public async Task<IHttpActionResult> PostCategoria(Categoria categoria)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await repo.SalvarAsync(categoria) < 1)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
            //return CreatedAtAction("GetCategoria", new { id = categoria.Id }, categoria);
            return CreatedAtRoute("GetCategoria", new { id = categoria.Id }, categoria);

        }

        // DELETE: api/Categorias/5
        [HttpDelete]
        [Route("{id}")]
        [ResponseType(typeof(Categoria))]
        public async Task<IHttpActionResult> DeleteCategoria(int id)
        {
            var categoria = await repo.GetCategoriaAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }
            categoria = await repo.RemoverAsync(categoria);

            if (categoria == null)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
            return Ok(categoria);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
