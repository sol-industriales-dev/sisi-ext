using Core.DAO.GestorArchivos;
using Core.DTO.GestorArchivos;
using Core.DTO.Principal.Generales;
using System.Collections.Generic;
using System.Web;

namespace Core.Service.GestorArchivos
{
    public class GestorArchivosService : IGestorArchivosDAO
    {

        private IGestorArchivosDAO directorioDAO;

        public IGestorArchivosDAO DirectorioDAO { get { return directorioDAO; } set { directorioDAO = value; } }

        public GestorArchivosService(IGestorArchivosDAO DirectorioDAO)
        {
            this.DirectorioDAO = DirectorioDAO;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool VerificarAccesoPrincipal(int usuarioID)
        {
            return this.DirectorioDAO.VerificarAccesoPrincipal(usuarioID);
        }

        public DirectorioDTO ObtenerEstructuraDirectorios(int idUsuario)
        {
            return this.DirectorioDAO.ObtenerEstructuraDirectorios(idUsuario);
        }

        public Dictionary<string, object> AgregarCarpeta(int padreID, int usuarioID, int empresa)
        {
            return this.DirectorioDAO.AgregarCarpeta(padreID, usuarioID, empresa);
        }

        public List<DirectorioDTO> SubirArchivo(List<HttpPostedFileBase> listaArchivos, int padreID, int usuarioID, int empresa)
        {
            return this.DirectorioDAO.SubirArchivo(listaArchivos, padreID, usuarioID, empresa);
        }

        public Dictionary<string, object> SubirFolder(DirectorioDTO directorio, List<HttpPostedFileBase> listaArchivos, int padreID, int usuarioID, int empresa)
        {
            return this.DirectorioDAO.SubirFolder(directorio, listaArchivos, padreID, usuarioID, empresa);
        }

        public string ObtenerRutaArchivo(int id, bool esVersion, int empresa)
        {
            return this.DirectorioDAO.ObtenerRutaArchivo(id, esVersion, empresa);
        }

        public string ObtenerRutaCarpetaComprimida(int folderID, int empresa)
        {
            return this.DirectorioDAO.ObtenerRutaCarpetaComprimida(folderID, empresa);
        }

        public DirectorioDTO ActualizarArchivo(HttpPostedFileBase archivo, int archivoID, int usuarioID, int empresa)
        {
            return this.DirectorioDAO.ActualizarArchivo(archivo, archivoID, usuarioID, empresa);
        }

        public List<DirectorioDTO> ObtenerHistorialVersiones(int archivoID)
        {
            return this.DirectorioDAO.ObtenerHistorialVersiones(archivoID);
        }

        public string RenombrarArchivo(string nuevoNombre, int archivoID, int empresa)
        {
            return this.DirectorioDAO.RenombrarArchivo(nuevoNombre, archivoID, empresa);
        }

        public bool EliminarArchivo(int id, int empresa)
        {
            return this.DirectorioDAO.EliminarArchivo(id, empresa);
        }

        public EstructuraVistasDTO ObtenerEstructuraPermisos(int usuarioID, int adminID)
        {
            return this.DirectorioDAO.ObtenerEstructuraPermisos(usuarioID, adminID);
        }

        public List<ComboDTO> ObtenerUsuariosPorDepartamento(int usuarioID, int departamentoID)
        {
            return this.DirectorioDAO.ObtenerUsuariosPorDepartamento(usuarioID, departamentoID);
        }

        public List<ComboDTO> ObtenerDepartamentos(int usuarioID)
        {
            return this.DirectorioDAO.ObtenerDepartamentos(usuarioID);
        }

        public bool GuardarVistasAccionesUsuario(List<EstructuraVistasDTO> carpetas, int usuarioID, int usuarioAdminID)
        {
            return this.DirectorioDAO.GuardarVistasAccionesUsuario(carpetas, usuarioID, usuarioAdminID);
        }

    }
}
