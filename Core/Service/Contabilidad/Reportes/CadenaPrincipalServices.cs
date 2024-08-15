using Core.DAO.Contabilidad.Reportes;
using Core.Entity.Administrativo.Contabilidad;
using Core.Enum.Administracion.CadenaProductiva;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Contabilidad.Reportes
{
    public class CadenaPrincipalServices : ICadenaPrincipalDAO
    {
        #region Atributos
        private ICadenaPrincipalDAO m_cadenaPrincipalDAO;
        #endregion
        #region Propiedades
        public ICadenaPrincipalDAO CadenaPrincipalDAO
        {
            get { return m_cadenaPrincipalDAO; }
            set { m_cadenaPrincipalDAO = value; }
        }
        #endregion
        #region Constructores
        public CadenaPrincipalServices(ICadenaPrincipalDAO cadenaPrincipalDAO)
        {
            this.CadenaPrincipalDAO = cadenaPrincipalDAO;
        }
        #endregion
        public tblC_CadenaPrincipal Guardar(tblC_CadenaPrincipal obj)
        {
            return this.CadenaPrincipalDAO.Guardar(obj);
        }
        public List<tblC_CadenaPrincipal> GetDocumentosGuardados()
        {
            return this.CadenaPrincipalDAO.GetDocumentosGuardados();
        }
        public List<tblC_CadenaPrincipal> GetDocumentosAplicados()
        {
            return this.CadenaPrincipalDAO.GetDocumentosAplicados();
        }
        public List<tblC_CadenaPrincipal> GetAllDocumentos()
        {
            return this.CadenaPrincipalDAO.GetAllDocumentos();
        }
        public tblC_CadenaPrincipal GetDocumento(int id)
        {
            return this.CadenaPrincipalDAO.GetDocumento(id);
        }
        public bool Eliminar(tblC_CadenaPrincipal obj)
        {
            return this.CadenaPrincipalDAO.Eliminar(obj);
        }
        public bool TienePermisoVoBo()
        {
            return this.CadenaPrincipalDAO.TienePermisoVoBo();
        }
        public Dictionary<string, object> AsignarVoBoCadena(int cadenaID)
        {
            return this.CadenaPrincipalDAO.AsignarVoBoCadena(cadenaID);
        }

        public EstadoAutorizacionCadenaEnum ObtenerEstadoAutorizacionCadena(int cadenaID)
        {
            return this.CadenaPrincipalDAO.ObtenerEstadoAutorizacionCadena(cadenaID);
        }

        public Dictionary<string, object> GetDocumentosPorAutorizar()
        {
            return this.CadenaPrincipalDAO.GetDocumentosPorAutorizar();
        }


        public Dictionary<string, object> AutorizarDocumentos(List<int> idsDocumentosPorAutorizar)
        {
            return this.CadenaPrincipalDAO.AutorizarDocumentos(idsDocumentosPorAutorizar);
        }

        public Dictionary<string, object> RechazarDocumento(int cadenaID, string comentarioRechazo)
        {
            return this.CadenaPrincipalDAO.RechazarDocumento(cadenaID, comentarioRechazo);
        }

        public Dictionary<string, object> ObtenerFacturasCadena(int cadenaID)
        {
            return this.CadenaPrincipalDAO.ObtenerFacturasCadena(cadenaID);
        }


        public Dictionary<string, object> ObtenerRutaPDFFactura(string numProveedor, string factura, string cc)
        {
            return this.CadenaPrincipalDAO.ObtenerRutaPDFFactura(numProveedor, factura, cc);
        }

        public Dictionary<string, object> ObtenerRutaXMLFactura(string numProveedor, string factura, string cc)
        {
            return this.CadenaPrincipalDAO.ObtenerRutaXMLFactura(numProveedor, factura, cc);
        }


        public string ObtenerFirmaAutorizacionCadena(int cadenaID, int tipo)
        {
            return this.CadenaPrincipalDAO.ObtenerFirmaAutorizacionCadena(cadenaID, tipo);
        }
    }
}
