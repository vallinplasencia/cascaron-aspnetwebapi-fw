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
    public class TrabajadorRepo : ITrabajadorRepo
    {
        private readonly AppDbContext db;

        public TrabajadorRepo(AppDbContext db)
        {
            this.db = db;
        }

        /// <summary>
        /// Retorna un item segun el id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task<Trabajador></returns>

        public async Task<Trabajador> GetTrabajadorAsync(string id)
        {
            return await db.Trabajadores.FindAsync(id);
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
        public async Task<List<Trabajador>> ListarAsync(int pagina = 0, int cantItem = 50, string ordenar = "nombre", string orden = "ASC", string filtro = null)
        {
            IQueryable<Trabajador> q;

            switch (ordenar)
            {
                case "nombre":
                default:
                    q = (orden == "asc") ? db.Trabajadores.OrderBy(c => c.Nombre) : db.Trabajadores.OrderByDescending(c => c.Nombre);
                    break;
            }
            if (filtro != null)
            {
                filtro = filtro.ToLower();
                q = q.Where(c => c.Nombre.ToLower().Contains(filtro));
            }
            //Si cantidad de item >= 1 y pagina >=0 paginado de la bd sino RECUPERO todos los registros.
            if (cantItem >= 1 && pagina >= 0)
            {
                q = q.Skip(pagina * cantItem)
                .Take(cantItem);
            }
            return await q.ToListAsync();
        }

        /// <summary>
        /// Elimina el item pasado por parametro
        /// </summary>
        /// <param name="trabajador"></param>
        /// <returns></returns>
        public async Task<Trabajador> RemoverAsync(Trabajador item)
        {
            db.Trabajadores.Remove(item);
            return (await db.SaveChangesAsync() > 0) ? item : null;
        }

        /// <summary>
        /// Salva el nuevo item o actualiza uno existente.
        /// </summary>
        /// <param name="nueva">Datos del item a salvar. Si es un nuevo item el id tiene q ser cero</param>
        /// <param name="actual">Item guardado en la bd q se va a catualizar los valores con el de nueva</param>
        /// <returns></returns>
        public async Task<int> SalvarAsync(Trabajador nueva, Trabajador actual = null)
        {
            if (string.IsNullOrEmpty(nueva.UserId))
            {
                db.Trabajadores.Add(nueva);
            }
            else if (actual != null)
            {
                if (actual.Nombre != nueva.Nombre)
                {
                    actual.Nombre = nueva.Nombre;
                }
            }
            else
            {
                return -1;
            }
            return await db.SaveChangesAsync();
        }

        /// <summary>
        /// Retorna el numero total de trabajadores
        /// </summary>
        /// <returns></returns>
        public int TotalTrabajadores(string filtro = null)
        {
            if (filtro != null)
            {
                return db.Trabajadores
                    .Where(c => c.Nombre.Contains(filtro))
                    .Count();
            }
            return db.Trabajadores.Count();
        }
    }
}
