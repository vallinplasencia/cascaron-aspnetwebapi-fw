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
    public class MiActividadRepo : IMiActividadRepo
    {
        private readonly AppDbContext db;

        public MiActividadRepo(AppDbContext db)
        {
            this.db = db;
        }

        /// <summary>
        /// Retorna un item segun el id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idUserLogeado">Id del usuario logueado</param>
        /// <returns>Task<Actividad></returns>

        public async Task<Actividad> GetActividadAsignadaAsync(int id, string idUserLogeado = null)
        {
            return await db.Actividades
                .Include(act => act.Tareas)
                .Include(act => act.CreadaPor)
                .Include(act => act.AsignadaA)
                .FirstOrDefaultAsync(act => act.Id == id && act.AsignadaAId == idUserLogeado);
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
        /// <param name="idUserLogeado">Id del usuario logueado</param>
        /// <returns></returns>
        public async Task<List<Actividad>> ListarAsignadasAsync(int pagina = 0, int cantItem = 50, string ordenar = "titulo", string orden = "ASC", string filtro = null, string idUserLogeado = null)
        {
            IQueryable<Actividad> q;

            switch (ordenar)
            {
                case "estado":
                    q = (orden == "asc") ? db.Actividades.OrderBy(c => c.Estado) : db.Actividades.OrderByDescending(c => c.Estado);
                    break;
                case "fechaRegistro":
                    q = (orden == "asc") ? db.Actividades.OrderBy(c => c.FechaRegistro) : db.Actividades.OrderByDescending(c => c.FechaRegistro);
                    break;
                case "creadaPorNombre":
                    q = (orden == "asc") ? db.Actividades.OrderBy(c => c.CreadaPor.Nombre) : db.Actividades.OrderByDescending(c => c.CreadaPor.Nombre);
                    break;
                case "asignadaANombre":
                    q = (orden == "asc") ? db.Actividades.OrderBy(c => c.AsignadaA.Nombre) : db.Actividades.OrderByDescending(c => c.AsignadaA.Nombre);
                    break;
                case "titulo":
                default:
                    q = (orden == "asc") ? db.Actividades.OrderBy(c => c.Titulo) : db.Actividades.OrderByDescending(c => c.Titulo);
                    break;
            }
            if (filtro != null)
            {
                filtro = filtro.ToLower();
                q = q.Where(c => c.Titulo.ToLower().Contains(filtro) || c.Descripcion.ToLower().Contains(filtro));
            }
            q = q
                .Include(a => a.CreadaPor)
                .Include(a => a.AsignadaA)
                .Include(a => a.Tareas)
                .Where(a => a.AsignadaAId == idUserLogeado);
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
        /// <param name="activo"></param>
        /// <param name="idUserLogeado">Id del usuario logueado</param>
        /// <returns></returns>
        public async Task<Actividad> RemoverAsignadaAsync(Actividad item, string idUserLogeado = null)
        {
                db.Actividades.Remove(item);
                return (await db.SaveChangesAsync() > 0) ? item : null;
            
        }

        /// <summary>
        /// Salva el nuevo item o actualiza uno existente.
        /// </summary>
        /// <param name="nueva">Datos del item a salvar. Si es un nuevo item el id tiene q ser cero</param>
        /// <param name="actual">Item guardado en la bd q se va a catualizar los valores con el de nueva</param>
        /// <param name="idUserLogeado">Id del usuario logueado</param>
        /// <returns></returns>
        public async Task<int> SalvarAsignadaAsync(Actividad nueva, Actividad actual = null, string idUserLogeado = null)
        {
            if (nueva.Id == 0)
            {
                db.Actividades.Add(nueva);
            }
            else if (actual != null)
            {
                if (actual.Titulo != nueva.Titulo)
                {
                    actual.Titulo = nueva.Titulo;
                }
                if (actual.Descripcion != nueva.Descripcion)
                {
                    actual.Descripcion = nueva.Descripcion;
                }
                if (actual.Estado != nueva.Estado)
                {
                    actual.Estado = nueva.Estado;
                }
                if (actual.FechaRegistro != nueva.FechaRegistro)
                {
                    actual.FechaRegistro = nueva.FechaRegistro;
                }
                if (actual.CreadaPorId != nueva.CreadaPorId)
                {
                    actual.CreadaPorId = nueva.CreadaPorId;
                }
                if (actual.AsignadaAId != nueva.AsignadaAId)
                {
                    actual.AsignadaAId = nueva.AsignadaAId;
                }


                var listRemover = new List<Tarea>();
                foreach (var ta in actual.Tareas)
                {
                    if (!nueva.Tareas.Any(t => t.Id == ta.Id))
                    {
                        listRemover.Add(ta);
                    }
                }
                listRemover.ForEach(tr =>db.Entry(tr).State = EntityState.Deleted);

                //actual.Tareas.RemoveAll(ta => !nueva.Tareas.Any(t => t.Id == ta.Id));
                //actual.Tareas.Remove(actual.Tareas.First());
                //db.SaveChanges();

                //actual.Tareas.ForEach(ta =>
                //{
                //    if (!nueva.Tareas.Any(t => t.Id == ta.Id)) {
                //        db.Entry(ta).State = EntityState.Deleted;
                //    }
                //});

                var tareasNuevasGuardar = new List<Tarea>();
                foreach (var tn in nueva.Tareas)
                {
                    var tareaEditar = actual.Tareas.Find(t => t.Id == tn.Id);

                    if (tareaEditar != null)
                    {
                        if (tareaEditar.Nombre != tn.Nombre)
                        {
                            tareaEditar.Nombre = tn.Nombre;
                        }
                        if (tareaEditar.Porcentaje != tn.Porcentaje)
                        {
                            tareaEditar.Porcentaje = tn.Porcentaje;
                        }
                        if (tareaEditar.Realizada != tn.Realizada)
                        {
                            tareaEditar.Realizada = tn.Realizada;
                        }
                    }
                    else
                    {
                        tareasNuevasGuardar.Add(tn);
                        //actual.Tareas.Add(tn);
                    }
                }
                tareasNuevasGuardar.ForEach(tng => actual.Tareas.Add(tng));
            }
            else
            {
                return -1;
            }
            var x = -1;
            try
            {
             x = await db.SaveChangesAsync();
            }catch(Exception e)
            {
                var r = 44;
                var xx = e;
                var t = r + 2;
            }
            return x;
        }

        /// <summary>
        /// Retorna el numero total de actividades
        /// </summary>
        /// <param name="idUserLogeado">Id del usuario logueado</param>
        /// <returns></returns>
        public int TotalActividadesAsignadas(string filtro = null, string idUserLogeado = null)
        {
            if (filtro != null)
            {
                return db.Actividades
                    //.Where(c=>c.Titulo.ToLower().Contains(filtro) || c.Descripcion.ToLower().Contains(filtro))
                    .Where(c => c.AsignadaAId == idUserLogeado && (c.Titulo.ToLower().Contains(filtro) || c.Descripcion.ToLower().Contains(filtro)))
                    .Count();
            }
            return db.Actividades.Where(c => c.AsignadaAId == idUserLogeado).Count();
        }

    }
}
