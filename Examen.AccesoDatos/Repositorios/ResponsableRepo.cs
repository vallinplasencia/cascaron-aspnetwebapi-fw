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
    public class ResponsableRepo : IResponsableRepo
    {
        private readonly AppDbContext db;

        public ResponsableRepo(AppDbContext db)
        {
            this.db = db;
        }

        /// <summary>
        /// Retorna un item segun el id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task<Responsable></returns>

        public async Task<Responsable> GetResponsableAsync(int id)
        {
            return await db.Responsables.FindAsync(id);
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
        public async Task<List<Responsable>> ListarAsync(int pagina = 0, int cantItem = 50, string ordenar = "nombre", string orden = "ASC", string filtro = null)
        {
            IQueryable<Responsable> q;

            switch (ordenar)
            {
                case "email":
                    q = (orden == "asc") ? db.Responsables.OrderBy(c => c.Email) : db.Responsables.OrderByDescending(c => c.Email);
                    break;
                case "nombre":
                default:
                    q = (orden == "asc") ? db.Responsables.OrderBy(c => c.Nombre) : db.Responsables.OrderByDescending(c => c.Nombre);
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
        /// <param name="responsable"></param>
        /// <returns></returns>
        public async Task<Responsable> RemoverAsync(Responsable item)
        {
            db.Responsables.Remove(item);
            return (await db.SaveChangesAsync() > 0) ? item : null;
        }

        /// <summary>
        /// Salva el nuevo item o actualiza uno existente.
        /// </summary>
        /// <param name="nueva">Datos del item a salvar. Si es un nuevo item el id tiene q ser cero</param>
        /// <param name="actual">Item guardado en la bd q se va a catualizar los valores con el de nueva</param>
        /// <returns></returns>
        public async Task<int> SalvarAsync(Responsable nueva, Responsable actual = null)
        {
            if (nueva.Id == 0)
            {
                db.Responsables.Add(nueva);
            }
            else if (actual != null)
            {
                if (actual.Nombre != nueva.Nombre)
                {
                    actual.Nombre = nueva.Nombre;
                }
                if (actual.Email != nueva.Email)
                {
                    actual.Email = nueva.Email;
                }
            }
            else
            {
                return -1;
            }
            return await db.SaveChangesAsync();
        }

        /// <summary>
        /// Retorna el numero total de responsables
        /// </summary>
        /// <returns></returns>
        public int TotalResponsables(string filtro = null)
        {
            if (filtro != null)
            {
                return db.Responsables
                    .Where(c => c.Nombre.Contains(filtro))
                    .Count();
            }
            return db.Responsables.Count();
        }
    }
}
