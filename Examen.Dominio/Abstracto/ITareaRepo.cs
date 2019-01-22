using Examen.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Dominio.Abstracto
{
    public interface ITareaRepo
    {
        /// <summary>
        /// Retorna un item segun el id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task<Tarea></returns>
        Task<Tarea> GetTareaAsync(int id);
        Task<int> TareaRealizadaAsync(Tarea tarea);        
    }
}
