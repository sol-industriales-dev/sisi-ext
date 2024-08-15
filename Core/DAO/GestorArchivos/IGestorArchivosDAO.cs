using Core.DTO.GestorArchivos;
using Core.DTO.Principal.Generales;
using System.Collections.Generic;
using System.Web;

namespace Core.DAO.GestorArchivos
{
    public interface IGestorArchivosDAO
    {

        bool VerificarAccesoPrincipal(int usuarioID);

        DirectorioDTO ObtenerEstructuraDirectorios(int idUsuario);

        Dictionary<string, object> AgregarCarpeta(int padreID, int usuarioID, int empresa);

        List<DirectorioDTO> SubirArchivo(List<HttpPostedFileBase> listaArchivos, int padreID, int usuarioID, int empresa);
        Dictionary<string, object> SubirFolder(DirectorioDTO directorio, List<HttpPostedFileBase> listaArchivos, int padreID, int usuarioID, int empresa);

        string ObtenerRutaArchivo(int id, bool esVersion, int empresa);

        string ObtenerRutaCarpetaComprimida(int folderID, int empresa);

        DirectorioDTO ActualizarArchivo(HttpPostedFileBase archivo, int archivoID, int usuarioID, int empresa);

        List<DirectorioDTO> ObtenerHistorialVersiones(int archivoID);

        string RenombrarArchivo(string nuevoNombre, int archivoID, int empresa);

        bool EliminarArchivo(int id, int empresa);

        EstructuraVistasDTO ObtenerEstructuraPermisos(int usuarioID, int adminID);
        List<ComboDTO> ObtenerUsuariosPorDepartamento(int usuarioID, int departamentoID);
        List<ComboDTO> ObtenerDepartamentos(int usuarioID);
        bool GuardarVistasAccionesUsuario(List<EstructuraVistasDTO> carpetas, int usuarioID, int usuarioAdminID);

    }
}
