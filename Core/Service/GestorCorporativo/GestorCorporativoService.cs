using Core.DAO.GestorCorporativo;
using Core.DTO.GestorCorporativo;
using Core.Enum.GestorCorporativo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service.GestorCorporativo
{
    public class GestorCorporativoService : IGestorCorporativoDAO
    {

        #region Inicialización
        public IGestorCorporativoDAO GestorCorporativoDAO { get; set; }

        public GestorCorporativoService(IGestorCorporativoDAO GestorCorporativoDAO)
        {
            this.GestorCorporativoDAO = GestorCorporativoDAO;
        }
        #endregion

        public Dictionary<string, object> VerificarAccesoGestor()
        {
            return GestorCorporativoDAO.VerificarAccesoGestor();
        }

        public DirectorioDTO ObtenerEstructuraDirectorios()
        {
            return GestorCorporativoDAO.ObtenerEstructuraDirectorios();
        }

        public Dictionary<string, object> SubirArchivo(HttpPostedFileBase archivo, long padreID)
        {
            return GestorCorporativoDAO.SubirArchivo(archivo, padreID);
        }

        public Dictionary<string, object> EliminarArchivo(long archivoID)
        {
            return GestorCorporativoDAO.EliminarArchivo(archivoID);
        }

        public Dictionary<string, object> CrearCarpeta(string nombreCarpeta, long padreID)
        {
            return GestorCorporativoDAO.CrearCarpeta(nombreCarpeta, padreID);
        }

        public Dictionary<string, object> CrearCarpetaSesion(GrupoCarpetaEnum grupoCarpeta)
        {
            return GestorCorporativoDAO.CrearCarpetaSesion(grupoCarpeta);
        }

        public Dictionary<string, object> RenombrarArchivo(string nuevoNombre, long archivoID)
        {
            return GestorCorporativoDAO.RenombrarArchivo(nuevoNombre, archivoID);
        }

        public Dictionary<string, object> DescargarArchivo(long archivoID)
        {
            return GestorCorporativoDAO.DescargarArchivo(archivoID);
        }

        public Dictionary<string, object> DescargarCarpeta(long carpetaID)
        {
            return GestorCorporativoDAO.DescargarCarpeta(carpetaID);
        }
    }
}
