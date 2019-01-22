using Examen.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Dominio.Abstracto
{
    public interface IMiActividadRepo
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
        /// <param name="idUserLogeado">Id del usuario logueado</param>
        /// <returns>Task<List<Actividad>></returns>
        Task<List<Actividad>> ListarAsignadasAsync(int pagina = 0, int cantItem = 50, string ordenar = "nombre", string orden = "ASC", string filtro = null, string idUserLogeado = null);

        /// <summary>
        /// Retorna un item segun el id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idUserLogeado">Id del usuario logueado</param>
        /// <returns>Task<Actividad></returns>
        Task<Actividad> GetActividadAsignadaAsync(int id, string idUserLogeado = null);

        /// <summary>
        /// Salva el nuevo item o actualiza uno existente.
        /// </summary>
        /// <param name="nueva">Datos del item a salvar. Si es un nuevo item el id tiene q ser cero</param>
        /// <param name="actual">Item guardado en la bd q se va a catualizar los valores con el de nuevo item</param>
        /// <param name="idUserLogeado">Id del usuario logueado</param>
        /// <returns>Task<int></returns>
        Task<int> SalvarAsignadaAsync(Actividad nueva, Actividad actual = null, string idUserLogeado = null);

        /// <summary>
        /// Elimina el item pasado por parametro
        /// </summary>
        /// <param name="item">Item a eliminar</param>
        /// <param name="idUserLogeado">Id del usuario logueado</param>
        /// <returns>Task<Item></Item> Item eliminado</returns>
        Task<Actividad> RemoverAsignadaAsync(Actividad item, string idUserLogeado = null);

        /// <summary>
        /// Retorna el numero total de item
        /// </summary>
        /// <param name="idUserLogeado">Id del usuario logueado</param>
        /// <returns></returns>
        int TotalActividadesAsignadas(string filtro = null, string idUserLogeado = null);


        /// <summary>
        /// Retorna el listado de todas las tareas que se me han asignado en todas las actividades y
        /// NO la he realizado segun la pagina, la cantidad de item a mostrar asi como el 
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
        /// <returns>Task<List<Tarea>></returns>
        Task<List<Tarea>> MisNotificacionesAsync(int pagina = 0, int cantItem = 50, string ordenar = "nombre", string orden = "ASC", string filtro = null, string idUserLogeado = null);


        /// <summary>
        /// Retorna el numero total de notificaciones
        /// </summary>
        /// <param name="idUserLogeado">Id del usuario logueado</param>
        /// <returns></returns>
        int TotalMisNotificaciones(string filtro = null, string idUserLogeado = null);

    }
}
