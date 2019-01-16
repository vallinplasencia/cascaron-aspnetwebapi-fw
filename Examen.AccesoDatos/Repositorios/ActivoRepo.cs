using Examen.AccesoDatos.Context;
using Examen.Dominio.Abstracto;
using Examen.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.AccesoDatos.Repositorios
{
    public class ActivoRepo : IActivoRepo
    {
        private readonly AppDbContext db;

        public ActivoRepo(AppDbContext db)
        {
            this.db = db;
        }

        /// <summary>
        /// Retorna un item segun el id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task<Activo></returns>

        public async Task<Activo> GetActivoAsync(int id)
        {
            return await db.Activos
                .Include(a => a.Categoria)
                .Include(a => a.Responsable)
                .FirstOrDefaultAsync(a => a.Id == id);
            //return await db.Activos.Include(a => a.Categoria).Include(a => a.Responsable).FindAsync(id);
            //return await db.Activos.FindAsync(id);
        }

        /// <summary>
        /// Retorna el listado de los item segun la pagina y la cantidad de item a mostrar asi como el 
        /// campo a ordenar y el orden de los elementos.
        /// Si pagina es menor q cero se retornan todos los registros.
        /// Si cantItem es menor q uno se retornan todos los registros
        /// </summary>
        /// <param name="pagina"></param>
        /// <param name="cantItem"></param>
        /// <param name="ordenar"></param>
        /// <param name="orden"></param>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public async Task<List<Activo>> ListarAsync(int pagina = 0, int cantItem = 50, string ordenar = "nombre", string orden = "ASC", string filtro = null)
        {
            IQueryable<Activo> q;

            switch (ordenar)
            {
                case "unidades":
                    q = (orden == "asc") ? db.Activos.OrderBy(c => c.Unidades) : db.Activos.OrderByDescending(c => c.Unidades);
                    break;
                case "esPrincipal":
                    q = (orden == "asc") ? db.Activos.OrderBy(c => c.EsPrincipal) : db.Activos.OrderByDescending(c => c.EsPrincipal);
                    break;
                case "fechaAlta":
                    q = (orden == "asc") ? db.Activos.OrderBy(c => c.FechaAlta) : db.Activos.OrderByDescending(c => c.FechaAlta);
                    break;
                case "fechaBaja":
                    q = (orden == "asc") ? db.Activos.OrderBy(c => c.FechaBaja) : db.Activos.OrderByDescending(c => c.FechaBaja);
                    break;

                case "responsableNombre":
                    q = (orden == "asc") ? db.Activos.OrderBy(c => c.Responsable.Nombre) : db.Activos.OrderByDescending(c => c.Responsable.Nombre);
                    break;

                case "nombre":
                default:
                    q = (orden == "asc") ? db.Activos.OrderBy(c => c.Nombre) : db.Activos.OrderByDescending(c => c.Nombre);
                    break;
            }
            if (filtro != null)
            {
                filtro = filtro.ToLower();
                q = q.Where(c => c.Nombre.ToLower().Contains(filtro) || c.Categoria.Nombre.ToLower().Contains(filtro));
            }
            //Si cantidad de item >= 1 y pagina >=0 paginado de la bd sino RECUPERO todos los registros.
            if (cantItem >= 1 && pagina >= 0)
            {
                q = q.Skip(pagina * cantItem)
                .Take(cantItem);
            }
            return await q
                .Include(a => a.Categoria)
                .Include(a => a.Responsable)
                .ToListAsync();
        }

        /// <summary>
        /// Elimina el item pasado por parametro
        /// </summary>
        /// <param name="activo"></param>
        /// <returns></returns>
        public async Task<Activo> RemoverAsync(Activo item)
        {
            db.Activos.Remove(item);
            return (await db.SaveChangesAsync() > 0) ? item : null;
        }

        /// <summary>
        /// Salva el nuevo item o actualiza uno existente.
        /// </summary>
        /// <param name="nueva">Datos del item a salvar. Si es un nuevo item el id tiene q ser cero</param>
        /// <param name="actual">Item guardado en la bd q se va a catualizar los valores con el de nueva</param>
        /// <returns></returns>
        public async Task<int> SalvarAsync(Activo nueva, Activo actual = null)
        {
            if (nueva.Id == 0)
            {
                db.Activos.Add(nueva);
            }
            else if (actual != null)
            {
                if (actual.Nombre != nueva.Nombre)
                {
                    actual.Nombre = nueva.Nombre;
                }
                if (actual.Unidades != nueva.Unidades)
                {
                    actual.Unidades = nueva.Unidades;
                }
                if (actual.EsPrincipal != nueva.EsPrincipal)
                {
                    actual.EsPrincipal = nueva.EsPrincipal;
                }
                if (actual.FechaAlta != nueva.FechaAlta)
                {
                    actual.FechaAlta = nueva.FechaAlta;
                }
                if (actual.FechaBaja != nueva.FechaBaja)
                {
                    actual.FechaBaja = nueva.FechaBaja;
                }
                if (actual.CategoriaId != nueva.CategoriaId)
                {
                    actual.CategoriaId= nueva.CategoriaId;
                }
                if (actual.ResponsableId != nueva.ResponsableId)
                {
                    actual.ResponsableId = nueva.ResponsableId;
                }
            }
            else
            {
                return -1;
            }
            return await db.SaveChangesAsync();
        }

        /// <summary>
        /// Retorna el numero total de activos
        /// </summary>
        /// <returns></returns>
        public int TotalActivos(string filtro = null)
        {
            if (filtro != null)
            {
                return db.Activos
                    .Where(c => c.Nombre.ToLower().Contains(filtro) || c.Categoria.Nombre.ToLower().Contains(filtro))
                    .Count();
            }
            return db.Activos.Count();
        }
    }
}
