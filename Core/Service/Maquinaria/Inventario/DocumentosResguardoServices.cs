using Core.DAO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using System.Web;

namespace Core.Service.Maquinaria.Inventario
{
    public class DocumentosResguardoServices : IDocumentosResguardosDAO
    {
        #region Atributos
        private IDocumentosResguardosDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private IDocumentosResguardosDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public DocumentosResguardoServices(IDocumentosResguardosDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores
        public void Guardar(tblM_DocumentosResguardos obj)
        {
            interfazDAO.Guardar(obj);
        }

        //raguilar 11/04/18 
        public tblM_DocumentosResguardos GetObjRutaDocumentobyID(int objIdResguardo)
        {
            return interfazDAO.GetObjRutaDocumentobyID(objIdResguardo);
        }

        public tblM_DocumentosResguardos GetObjRutaDocumentoByIDGeneral(int objIdResguardo)
        {
            return interfazDAO.GetObjRutaDocumentoByIDGeneral(objIdResguardo);
        }

        public void ActualizarArchivosResguardo(int resguardoID, HttpPostedFileBase archivoLicencia, HttpPostedFileBase archivoPoliza, HttpPostedFileBase archivoCurso, string rutaBase)
        {
            interfazDAO.ActualizarArchivosResguardo(resguardoID, archivoLicencia, archivoPoliza, archivoCurso, rutaBase);
        }
    }
}
