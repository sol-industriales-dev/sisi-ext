using Core.DAO.CuentasPorCobrar;
using Core.DTO.Contabilidad.Facturacion;
using Core.DTO.CuentasPorCobrar;
using Core.Entity.CuentasPorCobrar;
using Core.Enum.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.CuentasPorCobrar
{
    public class CuentasPorCobrarService : ICuentasPorCobrarDAO
    {
        public ICuentasPorCobrarDAO cxcInterfaz { get; set; }

        public CuentasPorCobrarService(ICuentasPorCobrarDAO cxcInterfaz)
        {
            this.cxcInterfaz = cxcInterfaz;
        }

        #region GESTION DE COBRANZA
        public Dictionary<string, object> GetConvenios(tblCXC_Convenios objFiltro)
        {
            return cxcInterfaz.GetConvenios(objFiltro);
        }
        public Dictionary<string, object> GetAcuerdoById(int idAcuerdo)
        {
            return cxcInterfaz.GetAcuerdoById(idAcuerdo);
        }
        public Dictionary<string, object> GetAcuerdoByFactura(string idFactura)
        {
            return cxcInterfaz.GetAcuerdoByFactura(idFactura);
        }

        public Dictionary<string, object> RemoveAcuerdoDet(int idAcuerdoDet)
        {
            return cxcInterfaz.RemoveAcuerdoDet(idAcuerdoDet);
        }

        public Dictionary<string, object> CrearEditarConvenios(CuentasPorCobrarDTO objConvenio)
        {
            return cxcInterfaz.CrearEditarConvenios(objConvenio);
        }
        public Dictionary<string, object> EliminarConvenio(int idConvenio)
        {
            return cxcInterfaz.EliminarConvenio(idConvenio);
        }

        public Dictionary<string, object> GetInfoFacturaById(string idFactura)
        {
            return cxcInterfaz.GetInfoFacturaById(idFactura);
        }

        public Dictionary<string, object> GetFacturasByCliente(int idCliente)
        {
            return cxcInterfaz.GetFacturasByCliente(idCliente);
        }

        public Dictionary<string,object> GetAutorizantesCC(string cc)
        {
            return cxcInterfaz.GetAutorizantesCC(cc);
        }

        public Dictionary<string, object> FillComboPeriodos()
        {
            return cxcInterfaz.FillComboPeriodos();
        }

        public Dictionary<string, object> FillComboCC()
        {
            return cxcInterfaz.FillComboCC();
        }

        public Dictionary<string, object> ActualizarEstatusConvenio(int idConvenio, EstatusConvenioEnum estatus)
        {
            return cxcInterfaz.ActualizarEstatusConvenio(idConvenio, estatus);
        }

        public Dictionary<string, object> CrearEditarCorte(DateTime fechaCorte, List<string> lstFacturas)
        {
            return cxcInterfaz.CrearEditarCorte(fechaCorte, lstFacturas);
        }

        public Dictionary<string, object> RemoveFactura(string idFactura, string comentarioRemove)
        {
            return cxcInterfaz.RemoveFactura(idFactura, comentarioRemove);
        }

        public Dictionary<string, object> CrearEditarComentarios(cxcComentariosDTO objFiltro)
        {
            return cxcInterfaz.CrearEditarComentarios(objFiltro);
        }

        public Dictionary<string,object> GetComentariosVencer()
        {
            return cxcInterfaz.GetComentariosVencer();
        }

        public Dictionary<string, object> GetComentarios(cxcComentariosDTO objFiltro)
        {
            return cxcInterfaz.GetComentarios(objFiltro);
        }

        public Dictionary<string, object> FillComboTiposComentarios()
        {
            return cxcInterfaz.FillComboTiposComentarios();
        }

        public Dictionary<string, object> GetKardex(List<string> lstFiltroCC)
        {
            return cxcInterfaz.GetKardex(lstFiltroCC);
        }

        public Dictionary<string, object> GetKardexDet(string cc)
        {
            return cxcInterfaz.GetKardexDet(cc);
        }

        public Dictionary<string, object> VerificarCXC(DateTime fechaInicial, DateTime fechaFinal)
        {
            return cxcInterfaz.VerificarCXC(fechaInicial,fechaFinal);
        }

        public Dictionary<string, object> GuardarCXC(List<EstClieFacturaDTO> lstFacturas, DateTime fechaInicial, DateTime fechaFinal)
        {
            return cxcInterfaz.GuardarCXC(lstFacturas, fechaInicial, fechaFinal);
        }

        public Dictionary<string, object> CancelarCXC(DateTime fechaInicial)
        {
            return cxcInterfaz.CancelarCXC(fechaInicial);
        }

        public List<string> GetDivisionDetByDivision(int divisionID)
        {
            return cxcInterfaz.GetDivisionDetByDivision(divisionID);
        }

        public Dictionary<string, object> GuardarFacturaMod(string factura, DateTime fechaVencimientoOG, DateTime fechaNueva)
        {
            return cxcInterfaz.GuardarFacturaMod(factura, fechaVencimientoOG, fechaNueva);
        }

        public Dictionary<string, object> EliminarFacturaMod(string factura)
        {
            return cxcInterfaz.EliminarFacturaMod(factura);
        }
        #endregion

        #region FILL COMBO
        public Dictionary<string, object> fillComboDivision()
        {
            return cxcInterfaz.fillComboDivision();
        }

        public Dictionary<string, object> FillComboCCGestion(int idDivision)
        {
            return cxcInterfaz.FillComboCCGestion(idDivision);
        }

        #endregion

        #region PERMISOS
        public bool esAutorizarCXC()
        {
            return cxcInterfaz.esAutorizarCXC();
        }

        #endregion
    }
}
