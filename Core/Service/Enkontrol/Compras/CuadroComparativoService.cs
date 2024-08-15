using Core.DAO.Enkontrol.Compras;
using Core.DTO;
using Core.DTO.Enkontrol.OrdenCompra;
using Core.DTO.Enkontrol.OrdenCompra.CuadroComparativo;
using Core.DTO.Principal.Generales;
using Core.Entity.Enkontrol.Compras.OrdenCompra.CuadroComparativo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Enkontrol.Compras
{
    public class CuadroComparativoService : ICuadroComparativoDAO
    {
        #region Atributos
        public ICuadroComparativoDAO e_ccDAO;
        #endregion
        #region Propiedades
        public ICuadroComparativoDAO CcDAO {
            get { return e_ccDAO; }
            set { e_ccDAO = value; }
        }
        #endregion
        #region Constructor
        public CuadroComparativoService(ICuadroComparativoDAO ccDAO)
        {
            CcDAO = ccDAO;
        }
        #endregion

        #region Dashboard
        public Dictionary<string, object> ConsultaDashboard(DateTime fechaInicio, DateTime fechaFin, List<int> proveedores, List<int> compradores)
        {
            return CcDAO.ConsultaDashboard(fechaInicio, fechaFin, proveedores, compradores);
        }
        #endregion

        public Respuesta darVoBoProvNoOptimo(List<CheckProvNoOptimoDTO> idNoOptimo)
        {
            return CcDAO.darVoBoProvNoOptimo(idNoOptimo);
        }

        public bool GuardarConfiabilidad(CuadroComparativoDTO cuadro)
        {
            return CcDAO.GuardarConfiabilidad(cuadro);
        }

        public List<tblCom_CC_Calificacion> CalificarConfiabilidad(CuadroComparativoDTO cuadro)
        {
            return CcDAO.CalificarConfiabilidad(cuadro);
        }

        public Dictionary<string, object> CalificarConfiabilidad(CuadroComparativoReporteDTO reporte, int numero)
        {
            return CcDAO.CalificarConfiabilidad(reporte, numero);
        }

        public Dictionary<string, object> FillCboProveedores()
        {
            return CcDAO.FillCboProveedores();
        }

        public Dictionary<string, object> FillCboCompradores()
        {
            return CcDAO.FillCboCompradores();
        }
        public Dictionary<string, object> GetDetallesProveedoresOptimosVsNoOptimos(DateTime fechaInicio, DateTime fechaFin, List<int> proveedores, List<int> compradores)
        {
            return CcDAO.GetDetallesProveedoresOptimosVsNoOptimos(fechaInicio, fechaFin, proveedores, compradores);
        }

        public Dictionary<string, object> GetDetallesTop10ProvNoOptimos(DateTime fechaInicio, DateTime fechaFin, List<int> proveedores, List<int> compradores)
        {
            return CcDAO.GetDetallesTop10ProvNoOptimos(fechaInicio, fechaFin, proveedores, compradores);
        }

        public Dictionary<string, object> GetDetallesTop10ProvOptimos(DateTime fechaInicio, DateTime fechaFin, List<int> proveedores, List<int> compradores)
        {
            return CcDAO.GetDetallesTop10ProvOptimos(fechaInicio, fechaFin, proveedores, compradores);
        }

        public Dictionary<string, object> GetDetallesCalificaciones(DateTime fechaInicio, DateTime fechaFin, List<int> proveedores, List<int> compradores)
        {
            return CcDAO.GetDetallesCalificaciones(fechaInicio, fechaFin, proveedores, compradores);
        }
    }
}
