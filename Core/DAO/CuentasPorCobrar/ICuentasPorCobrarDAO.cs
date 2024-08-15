using Core.DTO.Contabilidad.Facturacion;
using Core.DTO.CuentasPorCobrar;
using Core.Entity.CuentasPorCobrar;
using Core.Enum.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.CuentasPorCobrar
{
    public interface ICuentasPorCobrarDAO
    {
        #region GESTION DE COBRANZA
        Dictionary<string, object> GetConvenios(tblCXC_Convenios objFiltro);
        Dictionary<string, object> GetAcuerdoById(int idAcuerdo);
        Dictionary<string, object> RemoveAcuerdoDet(int idAcuerdoDet);
        Dictionary<string, object> CrearEditarConvenios(CuentasPorCobrarDTO objConvenio);
        Dictionary<string, object> EliminarConvenio(int idConvenio);
        Dictionary<string, object> GetInfoFacturaById(string idFactura);
        Dictionary<string, object> GetAcuerdoByFactura(string idFactura);
        Dictionary<string, object> GetFacturasByCliente(int idCliente);
        Dictionary<string, object> GetAutorizantesCC(string cc);
        Dictionary<string, object> FillComboPeriodos();
        Dictionary<string, object> FillComboCC();
        Dictionary<string, object> ActualizarEstatusConvenio(int idConvenio, EstatusConvenioEnum estatus);
        Dictionary<string, object> CrearEditarCorte(DateTime fechaCorte, List<string> lstFacturas);
        Dictionary<string, object> RemoveFactura(string idFactura, string comentarioRemove);
        Dictionary<string, object> CrearEditarComentarios(cxcComentariosDTO objFiltro);
        Dictionary<string, object> GetComentarios(cxcComentariosDTO objFiltro);
        Dictionary<string, object> GetComentariosVencer();
        Dictionary<string, object> FillComboTiposComentarios();
        Dictionary<string, object> GetKardex(List<string> lstFiltroCC);
        Dictionary<string, object> GetKardexDet(string cc);
        Dictionary<string, object> VerificarCXC(DateTime fechaInicial, DateTime fechaFinal);
        Dictionary<string, object> GuardarCXC(List<EstClieFacturaDTO> lstFacturas, DateTime fechaInicial, DateTime fechaFinal);
        Dictionary<string, object> CancelarCXC(DateTime fechaInicial);
        List<string> GetDivisionDetByDivision(int divisionID);
        Dictionary<string, object> GuardarFacturaMod(string factura, DateTime fechaVencimientoOG, DateTime fechaNueva);
        Dictionary<string, object> EliminarFacturaMod(string factura);

        #endregion

        #region FILL COMBO
        Dictionary<string, object> fillComboDivision();
        Dictionary<string, object> FillComboCCGestion(int idDivision);
        #endregion

        #region PERMISOS

        bool esAutorizarCXC();

        #endregion
    }
}
