using Examen.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Dominio.Abstracto
{
    public interface IActivoRepo
    {
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
        /// <returns>Task<List<Activo>></returns>
        Task<List<Activo>> ListarAsync(int pagina = 0, int cantItem = 50, string ordenar = "nombre", string orden = "ASC", string filtro = null);

        /// <summary>
        /// Retorna un item segun el id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task<Activo></returns>
        Task<Activo> GetActivoAsync(int id);

        /// <summary>
        /// Salva el nuevo item o actualiza uno existente.
        /// </summary>
        /// <param name="nueva">Datos del item a salvar. Si es un nuevo item el id tiene q ser cero</param>
        /// <param name="actual">Item guardado en la bd q se va a catualizar los valores con el de nuevo item</param>
        /// <returns>Task<int></returns>
        Task<int> SalvarAsync(Activo nueva, Activo actual = null);

        /// <summary>
        /// Elimina el item pasado por parametro
        /// </summary>
        /// <param name="item">Item a eliminar</param>
        /// <returns>Task<Item></Item> Item eliminado</returns>
        Task<Activo> RemoverAsync(Activo item);

        /// <summary>
        /// Retorna el numero total de item
        /// </summary>
        /// <returns></returns>
        int TotalActivos(string filtro = null);
    }
}
