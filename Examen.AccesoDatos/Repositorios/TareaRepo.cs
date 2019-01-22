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
    public class TareaRepo : ITareaRepo
    {
        private readonly AppDbContext db;

        public TareaRepo(AppDbContext db)
        {
            this.db = db;
        }
        /// <summary>
        /// Retorna un item segun el id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task<Tarea></returns>

        public async Task<Tarea> GetTareaAsync(int id)
        {
            return await db.Tareas.Include(t=>t.Actividad).Include(t2=>t2.Actividad.Tareas).FirstOrDefaultAsync(t=>t.Id==id);
        }

        public async Task<int> TareaRealizadaAsync(Tarea tarea)
        {
            if (tarea != null)
            {
                tarea.Realizada = true;
                if (tarea.Actividad.Tareas.All(t => t.Realizada))
                {
                    tarea.Actividad.Estado = Dominio.Util.EstadosActividad.Cumplida;
                }                
            }
            else
            {
                return -1;
            }
            return await db.SaveChangesAsync();
        }
    }
}
