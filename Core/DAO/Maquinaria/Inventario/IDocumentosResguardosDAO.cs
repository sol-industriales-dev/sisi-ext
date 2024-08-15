using Core.Entity.Maquinaria.Inventario;
using System.Web;

namespace Core.DAO.Maquinaria.Inventario
{
    public interface IDocumentosResguardosDAO
    {
        void Guardar(tblM_DocumentosResguardos obj);
        tblM_DocumentosResguardos GetObjRutaDocumentobyID(int objIdResguardo);

        tblM_DocumentosResguardos GetObjRutaDocumentoByIDGeneral(int objIdResguardo);

        void ActualizarArchivosResguardo(int resguardoID, HttpPostedFileBase archivoLicencia, HttpPostedFileBase archivoPoliza, HttpPostedFileBase archivoCurso, string rutaBase);
    }
}
