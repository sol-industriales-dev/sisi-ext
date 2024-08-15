using Core.DTO.FileManager;
using Core.DTO.Principal.Generales;
using Core.Entity.FileManager;
using System.Collections.Generic;
using System.Web;

namespace Core.DAO.FileManager
{
    public interface IFileManagerDAO
    {

        #region Métodos FileManager

        /// <summary>
        /// Verifica si el usuario logueado tiene acceso a algún archivo del gestor de archivos.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> VerificarAccesoGestor();
        Dictionary<string, object> VerificarAccesoGestorDeprecado();
        Dictionary<string, object> VerificarAccesoGestorHierarchy();
        

        /// <summary>
        /// Obtiene todos los tipos de archivos que se pueden subir al gestor de archivos en una carpeta específica.
        /// </summary>
        /// <returns>Retorna una lista con los tipos de archivos permitidos.</returns>
        Dictionary<string, object> ObtenerTiposArchivos(long archivoID);

        /// <summary>
        /// Obtiene los diferentes tipos de archivos disponibles en el gestor.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> ObtenerTodosTiposArchivos();

        /// <summary>
        /// Obtiene una lista de archivos existentes en el folder indicado que pueden ser actualizados por el usuario según sus permisos.
        /// </summary>
        /// <param name="padreID"></param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerArchivosActualizables(long padreID);

        /// <summary>
        /// Sube un archivo al gestor archivos (Tanto en bd como en físico en el servidor).
        /// </summary>
        /// <param name="archivo">Archivo a subir.</param>
        /// <param name="padreID">ID del folder al que pertenecerá.</param>
        /// <returns></returns>
        Dictionary<string, object> SubirArchivo(List<HttpPostedFileBase> lstArchivo, long padreID, int tipoArchivoID);

        /// <summary>
        /// Sube varios archivos a la vez al FileManager. Indica que tipo de archivo es cada uno y si es versión nueva de alguno existente.
        /// </summary>
        /// <param name="archivosPorSubir">Lista de archivos a subir.</param>
        /// <returns></returns>
        Dictionary<string, object> SubirVariosArchivos(List<ArchivoPorSubirDTO> archivosPorSubir,long padreID);

        /// <summary>
        /// Obtiene las carpetas iniciales para mostrar en el gestor documental.
        /// </summary>
        /// <returns></returns>
        DirectorioDTO ObtenerEstructuraDirectorios();
        DirectorioDTO ObtenerEstructuraDirectoriosDeprecado();
        DirectorioDTO ObtenerEstructuraDirectoriosHierarchy();
        DirectorioDTO ObtenerEstructuraDirectoriosChildsHierarchy(long padreID, int obraCerrada);

        /// <summary>
        /// Obtiene los archivos y carpetas contenidos en una carpeta específica.
        /// </summary>
        /// <param name="padreID">Identificador de la carpeta.</param>
        /// <returns></returns>
        DirectorioDTO ObtenerEstructuraCarpeta(long padreID);

        /// <summary>
        /// Actualiza un archivo.
        /// </summary>
        /// <param name="archivo">Nueva versión del archivo.</param>
        /// <param name="archivoID">Identificador del archivo.</param>
        /// <returns></returns>
        Dictionary<string, object> ActualizarArchivo(HttpPostedFileBase archivo, long archivoID);

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
        /// <param name="listaTiposArchivosID">Lista de los tipos de archivos que tendrá la carpeta.</param>
        /// <returns></returns>
        Dictionary<string, object> CrearCarpeta(string nombreCarpeta, long padreID, List<int> listaTiposArchivosID, bool considerarse, string abreviacion);

        /// <summary>
        /// Crea una carpeta para un contratista con una lista de archivos permitidos por default.
        /// </summary>
        /// <param name="padreID">Identificador de la carpeta padre.</param>
        /// <returns></returns>
        Dictionary<string, object> CrearCarpetaContratista(long padreID);

        /// <summary>
        /// Crea una carpeta para un contratista con una estructura predefinida para la división industrial.
        /// </summary>
        /// <param name="padreID"></param>
        /// <returns></returns>
        Dictionary<string, object> CrearCarpetaContratistaIndustrial(long padreID);

        /// <summary>
        /// Crea una carpeta para una estimación con una lista de archivos permitidos por default.
        /// </summary>
        /// <param name="padreID">Identificador de la carpeta padre.</param>
        /// <returns></returns>
        Dictionary<string, object> CrearCarpetaEstimacion(long padreID);

        /// <summary>
        /// Crea una carpeta para una estimación con una estructura de archivos predefinida de la división industrial.
        /// </summary>
        /// <param name="padreID"></param>
        /// <returns></returns>
        Dictionary<string, object> CrearCarpetaEstimacionIndustrial(long padreID);

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
        /// <param name="esVersion">Indica si se descarga la versión activa o una específica.</param>
        /// <returns></returns>
        Dictionary<string, object> DescargarArchivo(long archivoID, bool esVersion);

        /// <summary>
        /// Obtiene una lista de las diferentes versiones del archivo seleccionado.
        /// </summary>
        /// <param name="archivoID">Identificador del archivo.</param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerHistorialVersiones(long archivoID);

        /// <summary>
        /// Crea un archivo zip de la carpeta seleccionada y devuelve la ruta para descarga.
        /// </summary>
        /// <param name="carpetaID">Identificador de la carepta a descargar.</param>
        /// <returns></returns>
        Dictionary<string, object> DescargarCarpeta(long carpetaID);

        /// <summary>
        /// Obtiene a todos los usuarios que tengan acceso a un archivo en específico.
        /// </summary>
        /// <param name="archivoID">Identificador del archivo.</param>
        /// <returns></returns>
        Dictionary<string, object> CargarUsuariosAsignados(long archivoID);

        /// <summary>
        /// Guarda los cambios hechos sobre la asignación de permisos de un archivo.
        /// </summary>
        /// <param name="listaUsuarios">Lista de usuarios con acceso al archivo y sus permisos.</param>
        /// <param name="archivoID">Identificador del archivo.</param>
        /// <returns></returns>
        Dictionary<string, object> GuardarCambiosPermisos(List<PermisosDTO> listaUsuarios, long archivoID);

        /// <summary>
        /// Crea las carpetas de meses para cada obra de seguridad correspondiente.
        /// </summary>
        /// <param name="carpetaObraArchivoID"></param>
        /// <returns></returns>
        string CrearMesesSeguridad(int carpetaObraArchivoID);

        /// <summary>
        /// Obtiene todas las subdivisiones pertenecientes a una división específica.
        /// </summary>
        /// <param name="divisionID">Identificador de la división por el cual se filtrarán las subdivisiones.</param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerSubdivisiones(int divisionID);

        /// <summary>
        /// Crea una nueva carpeta de tipo subdivisión.
        /// </summary>
        /// <param name="subdivisionID"></param>
        /// <param name="padreID"></param>
        /// <returns></returns>
        Dictionary<string, object> CrearSubdivision(int subdivisionID, long padreID);

        /// <summary>
        /// Crea un conjunto de carpetas predefinidas para una obra de la división industrial desde el FileManager.
        /// </summary>
        /// <param name="subdivisionID"></param>
        /// <param name="ccID"></param>
        /// <returns></returns>
        Dictionary<string, object> CrearEstructuraObraSubdivision(int subdivisionID, int ccID);

        /// <summary>
        /// Crea un conjunto de carpetas predefinidas para un proyecto de la división industrial desde el FileManager.
        /// </summary>
        /// <param name="subdivisionID"></param>
        /// <param name="nombre"></param>
        /// <param name="abreviacion"></param>
        /// <returns></returns>
        Dictionary<string, object> CrearEstructuraObraSubdivisionPorNombre(int subdivisionID, string nombre, string abreviacion);


        /// <summary>
        /// Lee los datos de un archivo dentro de un zip, y los guarda en sesión para ser cargados por el visor.
        /// </summary>
        /// <param name="archivoID"></param>
        /// <returns></returns>
        Dictionary<string, object> CargarDatosArchivoVisor(long archivoID);
        #endregion

        #region Métodos Permisos

        /// <summary>
        /// Obtiene una lista de empleados con un nombre parecido al fragmento proporcionado.
        /// </summary>
        /// <param name="term">Fragmento de nombre para buscar</param>
        /// <returns></returns>
        List<AutocompleteDTO> ObtenerUsuariosAutocompletado(string term);

        /// <summary>
        /// Obtiene todas las divisiones de la empresa.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> ObtenerDivisiones();

        /// <summary>
        /// Obtiene todas las subdivisiones de la empresa.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> ObtenerSubdivisiones();

        /// <summary>
        /// Obtiene una lista de todas las obras que aún no tienen carpetas creadas dentro de la estructura de archivos.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> ObtenerObras();

        /// <summary>
        /// Obtiene una lista de las obras pertenecientes según las divisiones indicadas (según se definió en la estructura de carpetas del gestor de archivos).
        /// </summary>
        /// <param name="listaDivisionesIDs">Lista de divisiones por filtrar</param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerObrasPorDivision(List<int> listaDivisionesIDs);

        /// <summary>
        /// Obtiene el árbol de carpetas inicial.
        /// </summary>
        /// <param name="usuarioID">Identificador del usuario.</param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerEstructuraPermisos(int usuarioID);

        /// <summary>
        /// Obtiene la información contenida de una carpeta para la vista de permisos.
        /// </summary>
        /// <param name="usuarioID"></param>
        /// <returns></returns>
        List<EstructuraVistasDTO> ObtenerEstructuraCarpetaPermisos(int usuarioID, long folderID);

        /// <summary>
        /// Guarda los cambios hechos sobre los permisos en los archivos seleccionados.
        /// </summary>
        /// <param name="usuarioID">Identificador del usuario al que se le guardarán los cambios.</param>
        /// <param name="archivos">Lista de archivos y permisos a guardar.</param>
        /// <returns></returns>
        Dictionary<string, object> GuardarPermisos(int usuarioID, List<EstructuraVistasDTO> archivos);

        /// <summary>
        /// Crea una estructura fija de carpetas de la obra seleccionada.
        /// </summary>
        /// <param name="divisionID">Identificador de la división.</param>
        /// <param name="ccID">Identificador de la obra.</param>
        /// <returns></returns>
        Dictionary<string, object> CrearEstructuraObra(int divisionID, int ccID);

        /// <summary>
        /// Guarda permisos especiales de un usuario.
        /// </summary>
        /// <param name="permisosEspeciales">Lista de permisos especiales a guardar para el usuario.</param>
        /// <returns></returns>
        Dictionary<string, object> GuardarPermisosEspeciales(List<tblFM_PermisoEspecial> permisosEspeciales);

        /// <summary>
        /// Elimina el permiso especial de algún usuario.
        /// </summary>
        /// <param name="permisoID"></param>
        /// <returns></returns>
        Dictionary<string, object> EliminarPermisoEspecial(int permisoID);

        /// <summary>
        /// Obtiene a todos los usuarios que tengan algún permiso especial.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> ObtenerUsuariosPermisosEspeciales();
        #endregion

        #region MétodosEnvio
        /// <summary>
        /// Obtiene todos los envíos pendientes al Gestor Documental.
        /// </summary>
        /// <returns></returns>
        List<tblFM_EnvioDocumento> CargarTblEnvios(int tipoDocumento, string descripcion, int usuarioID);

        /// <summary>
        /// Guarda los archivos correspondientes al envío pendiente.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> SubirArchivoAuto(List<System.Byte[]> archivo, long padreID, int tipoArchivoID, int envioID);

        /// <summary>
        /// Obtiene el envío con el ID proporcionado.
        /// </summary>
        /// <returns></returns>
        tblFM_EnvioDocumento GetEnvioByID(int id);

        /// <summary>
        /// Elimina el registro de Documentos Pendientes
        /// </summary>
        /// <returns></returns>
        bool EliminarEnvioDoc(int index); 
        #endregion

        #region DUPLICAR CARPETAS
        Dictionary<string, object> FillCboCarpetasBases();

        Dictionary<string, object> GenerarCarpetaDuplicado(CarpetaBaseDTO objParamsDTO);
        #endregion

        #region CERRAR OBRAS
        Dictionary<string, object> FillCboCarpetasObras();

        Dictionary<string, object> CerrarObra(int idArchivo);
        #endregion

        #region GENERAL
        Dictionary<string, object> GetPermisoAcciones();
        #endregion

        Dictionary<string, object> copiadoBase();
    }
}
