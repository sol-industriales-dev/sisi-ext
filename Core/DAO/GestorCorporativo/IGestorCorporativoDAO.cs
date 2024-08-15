using Core.DTO.GestorCorporativo;
using Core.Enum.GestorCorporativo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.GestorCorporativo
{
    public interface IGestorCorporativoDAO
    {
        /// <summary>
        /// Verifica si el usuario logueado tiene acceso a algún archivo del gestor corporativo.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> VerificarAccesoGestor();

        /// <summary>
        /// Obtiene el arbol de carpetas y archivos a mostrar en el gestor de archivos.
        /// </summary>
        /// <returns></returns>
        DirectorioDTO ObtenerEstructuraDirectorios();

        /// <summary>
        /// Sube un archivo al gestor archivos (Tanto en bd como en físico en el servidor).
        /// </summary>
        /// <param name="archivo">Archivo a subir.</param>
        /// <param name="padreID">ID del folder al que pertenecerá.</param>
        /// <returns></returns>
        Dictionary<string, object> SubirArchivo(HttpPostedFileBase archivo, long padreID);

        /// <summary>
        /// Elimina la carpeta o archivo seleccionado.
        /// </summary>
        /// <param name="archivoID">Identificador del archvo a eliminar.</param>
        /// <returns></returns>
        Dictionary<string, object> EliminarArchivo(long archivoID);

        /// <summary>
        /// Crea una carpeta con nombre auto-generado.
        /// </summary>
        /// <param name="padreID">Identificador del folder padre en donde se creará la carpeta.</param>
        /// <param name="nombreCarpeta">Nombre de la carpeta por crear.</param>
        /// <returns></returns>
        Dictionary<string, object> CrearCarpeta(string nombreCarpeta, long padreID);

        /// <summary>
        /// Crea una estructura de carpetas dependiendo del grupo de carpeta perteneciente.
        /// </summary>
        /// <param name="grupoCarpeta">Identificador del grupo de carpeta.</param>
        /// <returns></returns>
        Dictionary<string, object> CrearCarpetaSesion(GrupoCarpetaEnum grupoCarpeta);


        /// <summary>
        /// Renombra una carpeta.
        /// </summary>
        /// <param name="nuevoNombre">El nuevo nombre por el que se reemplazará al anterior.</param>
        /// <param name="archivoID">Identificador de la carpeta.</param>
        /// <returns></returns>
        Dictionary<string, object> RenombrarArchivo(string nuevoNombre, long archivoID);

        /// <summary>
        /// Obtiene el archivo que está dentro del ZIP para descargar.
        /// </summary>
        /// <param name="archivoID">Identificador del archivo a descargar.</param>
        /// <returns></returns>
        Dictionary<string, object> DescargarArchivo(long archivoID);

        /// <summary>
        /// Crea un archivo zip de la carpeta seleccionada y devuelve la ruta para descarga.
        /// </summary>
        /// <param name="carpetaID">Identificador de la carepta a descargar.</param>
        /// <returns></returns>
        Dictionary<string, object> DescargarCarpeta(long carpetaID);
    }
}
