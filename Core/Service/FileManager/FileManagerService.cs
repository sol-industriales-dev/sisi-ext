using Core.DAO.FileManager;
using Core.DTO.FileManager;
using Core.DTO.Principal.Generales;
using Core.Entity.FileManager;
using System.Collections.Generic;
using System.Web;


namespace Core.Service.FileManager
{
    public class FileManagerService : IFileManagerDAO
    {
        private IFileManagerDAO fileManagerDAO;

        public FileManagerService(IFileManagerDAO fileManagerDAO)
        {
            this.fileManagerDAO = fileManagerDAO;
        }

        #region FileManager

        public Dictionary<string, object> VerificarAccesoGestor()
        {
            return fileManagerDAO.VerificarAccesoGestor();
        }
        public Dictionary<string, object> VerificarAccesoGestorDeprecado()
        {
            return fileManagerDAO.VerificarAccesoGestorDeprecado();
        }
        public Dictionary<string, object> VerificarAccesoGestorHierarchy()
        {
            return fileManagerDAO.VerificarAccesoGestorHierarchy();
        }
        public Dictionary<string, object> ObtenerTiposArchivos(long archivoID)
        {
            return fileManagerDAO.ObtenerTiposArchivos(archivoID);
        }

        public Dictionary<string, object> ObtenerTodosTiposArchivos()
        {
            return fileManagerDAO.ObtenerTodosTiposArchivos();
        }

        public Dictionary<string, object> ObtenerArchivosActualizables(long padreID)
        {
            return fileManagerDAO.ObtenerArchivosActualizables(padreID);
        }

        public Dictionary<string, object> SubirArchivo(List<HttpPostedFileBase> lstArchivos, long padreID, int tipoArchivoID)
        {
            return fileManagerDAO.SubirArchivo(lstArchivos, padreID, tipoArchivoID);
        }

        public Dictionary<string, object> SubirVariosArchivos(List<ArchivoPorSubirDTO> archivosPorSubir, long padreID)
        {
            return fileManagerDAO.SubirVariosArchivos(archivosPorSubir, padreID);
        }

        public Dictionary<string, object> ActualizarArchivo(HttpPostedFileBase archivo, long archivoID)
        {
            return fileManagerDAO.ActualizarArchivo(archivo, archivoID);
        }

        public DirectorioDTO ObtenerEstructuraDirectorios()
        {
            return fileManagerDAO.ObtenerEstructuraDirectorios();
        }
        public DirectorioDTO ObtenerEstructuraDirectoriosDeprecado()
        {
            return fileManagerDAO.ObtenerEstructuraDirectoriosDeprecado();
        }
        public DirectorioDTO ObtenerEstructuraDirectoriosHierarchy()
        {
            return fileManagerDAO.ObtenerEstructuraDirectoriosHierarchy();
        }
        public DirectorioDTO ObtenerEstructuraDirectoriosChildsHierarchy(long padreID, int obraCerrada)
        {
            return fileManagerDAO.ObtenerEstructuraDirectoriosChildsHierarchy(padreID, obraCerrada);
        }
     
        public DirectorioDTO ObtenerEstructuraCarpeta(long padreID)
        {
            return fileManagerDAO.ObtenerEstructuraCarpeta(padreID);
        }

        public Dictionary<string, object> EliminarArchivo(long archivoID)
        {
            return fileManagerDAO.EliminarArchivo(archivoID);
        }

        public Dictionary<string, object> CrearCarpeta(string nombreCarpeta, long padreID, List<int> listaTiposArchivosID, bool considerarse, string abreviacion)
        {
            return fileManagerDAO.CrearCarpeta(nombreCarpeta, padreID, listaTiposArchivosID, considerarse, abreviacion);
        }

        public Dictionary<string, object> CrearCarpetaContratista(long padreID)
        {
            return fileManagerDAO.CrearCarpetaContratista(padreID);
        }

        public Dictionary<string, object> CrearCarpetaContratistaIndustrial(long padreID)
        {
            return fileManagerDAO.CrearCarpetaContratistaIndustrial(padreID);
        }

        public Dictionary<string, object> CrearCarpetaEstimacion(long padreID)
        {
            return fileManagerDAO.CrearCarpetaEstimacion(padreID);
        }

        public Dictionary<string, object> CrearCarpetaEstimacionIndustrial(long padreID)
        {
            return fileManagerDAO.CrearCarpetaEstimacionIndustrial(padreID);
        }

        public Dictionary<string, object> RenombrarArchivo(string nuevoNombre, long archivoID)
        {
            return fileManagerDAO.RenombrarArchivo(nuevoNombre, archivoID);

        }

        public Dictionary<string, object> DescargarArchivo(long archivoID, bool esVersion)
        {
            return fileManagerDAO.DescargarArchivo(archivoID, esVersion);
        }

        public Dictionary<string, object> DescargarCarpeta(long carpetaID)
        {
            return fileManagerDAO.DescargarCarpeta(carpetaID);
        }

        public Dictionary<string, object> ObtenerHistorialVersiones(long archivoID)
        {
            return fileManagerDAO.ObtenerHistorialVersiones(archivoID);
        }

        public Dictionary<string, object> CargarUsuariosAsignados(long archivoID)
        {
            return fileManagerDAO.CargarUsuariosAsignados(archivoID);
        }

        public Dictionary<string, object> GuardarCambiosPermisos(List<PermisosDTO> listaUsuarios, long archivoID)
        {
            return fileManagerDAO.GuardarCambiosPermisos(listaUsuarios, archivoID);

        }

        public string CrearMesesSeguridad(int carpetaObraArchivoID)
        {
            return fileManagerDAO.CrearMesesSeguridad(carpetaObraArchivoID);
        }

        public Dictionary<string, object> ObtenerSubdivisiones(int divisionID)
        {
            return fileManagerDAO.ObtenerSubdivisiones(divisionID);
        }

        public Dictionary<string, object> CrearSubdivision(int subdivisionID, long padreID)
        {
            return fileManagerDAO.CrearSubdivision(subdivisionID, padreID);
        }

        public Dictionary<string, object> CrearEstructuraObraSubdivision(int subdivisionID, int ccID)
        {
            return fileManagerDAO.CrearEstructuraObraSubdivision(subdivisionID, ccID);
        }

        public Dictionary<string, object> CrearEstructuraObraSubdivisionPorNombre(int subdivisionID, string nombre, string abreviacion)
        {
            return fileManagerDAO.CrearEstructuraObraSubdivisionPorNombre(subdivisionID, nombre, abreviacion);
        }

        public Dictionary<string, object> CargarDatosArchivoVisor(long archivoID)
        {
            return fileManagerDAO.CargarDatosArchivoVisor(archivoID);
        }
        #endregion

        #region Permisos

        public List<AutocompleteDTO> ObtenerUsuariosAutocompletado(string term)
        {
            return fileManagerDAO.ObtenerUsuariosAutocompletado(term);
        }

        public Dictionary<string, object> ObtenerDivisiones()
        {
            return fileManagerDAO.ObtenerDivisiones();
        }

        public Dictionary<string, object> ObtenerSubdivisiones()
        {
            return fileManagerDAO.ObtenerSubdivisiones();
        }

        public Dictionary<string, object> ObtenerObras()
        {
            return fileManagerDAO.ObtenerObras();
        }

        public Dictionary<string, object> ObtenerObrasPorDivision(List<int> listaDivisionesIDs)
        {
            return fileManagerDAO.ObtenerObrasPorDivision(listaDivisionesIDs);
        }

        public Dictionary<string, object> ObtenerEstructuraPermisos(int usuarioID)
        {
            return fileManagerDAO.ObtenerEstructuraPermisos(usuarioID);
        }

        public List<EstructuraVistasDTO> ObtenerEstructuraCarpetaPermisos(int userID, long folderID)
        {
            return fileManagerDAO.ObtenerEstructuraCarpetaPermisos(userID, folderID);
        }

        public Dictionary<string, object> GuardarPermisos(int usuarioID, List<EstructuraVistasDTO> archivos)
        {
            return fileManagerDAO.GuardarPermisos(usuarioID, archivos);
        }

        public Dictionary<string, object> CrearEstructuraObra(int divisionID, int ccID)
        {
            return fileManagerDAO.CrearEstructuraObra(divisionID, ccID);
        }

        public Dictionary<string, object> GuardarPermisosEspeciales(List<tblFM_PermisoEspecial> permisosEspeciales)
        {
            return fileManagerDAO.GuardarPermisosEspeciales(permisosEspeciales);
        }

        public Dictionary<string, object> EliminarPermisoEspecial(int permisoID)
        {
            return fileManagerDAO.EliminarPermisoEspecial(permisoID);
        }

        public Dictionary<string, object> ObtenerUsuariosPermisosEspeciales()
        {
            return fileManagerDAO.ObtenerUsuariosPermisosEspeciales();
        }
        #endregion

        #region Envio
        public List<tblFM_EnvioDocumento> CargarTblEnvios(int tipoDocumento, string descripcion, int usuarioID) 
        {
            return fileManagerDAO.CargarTblEnvios(tipoDocumento, descripcion, usuarioID);
        }
        public Dictionary<string, object> SubirArchivoAuto(List<System.Byte[]> archivo, long padreID, int tipoArchivoID, int envioID)
        {
            return fileManagerDAO.SubirArchivoAuto(archivo, padreID, tipoArchivoID, envioID);
        }
        public tblFM_EnvioDocumento GetEnvioByID(int id)
        {
            return fileManagerDAO.GetEnvioByID(id);
        }
        public bool EliminarEnvioDoc(int index)
        {
            return fileManagerDAO.EliminarEnvioDoc(index);
        }
        #endregion

        #region DUPLICAR CARPETAS
        public Dictionary<string, object> FillCboCarpetasBases()
        {
            return fileManagerDAO.FillCboCarpetasBases();
        }

        public Dictionary<string, object> GenerarCarpetaDuplicado(CarpetaBaseDTO objParamsDTO)
        {
            return fileManagerDAO.GenerarCarpetaDuplicado(objParamsDTO);
        }
        #endregion

        #region CERRAR OBRAS
        public Dictionary<string, object> FillCboCarpetasObras()
        {
            return fileManagerDAO.FillCboCarpetasObras();
        }

        public Dictionary<string, object> CerrarObra(int idArchivo)
        {
            return fileManagerDAO.CerrarObra(idArchivo);
        }
        #endregion

        #region GENERAL
        public Dictionary<string, object> GetPermisoAcciones()
        {
            return fileManagerDAO.GetPermisoAcciones();
        }
        #endregion

        public Dictionary<string, object> copiadoBase()
        {
            return fileManagerDAO.copiadoBase();
        }
    }
}
