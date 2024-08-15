using Core.DTO.Contabilidad;
using Core.DTO.Contabilidad.FlujoEfectivo;
using Core.DTO.Contabilidad.Propuesta;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Contabilidad;
using Core.Entity.Administrativo.Contabilidad.FlujoEfectivo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.Reportes
{
    public interface ICadenaProductivaDAO
    {

        bool enviarCorreo();
        bool enviarCorreoPropuesta();
        ICollection<VencimientoDTO> getInforVencimiento(string nomprov, int tipoFactura);

        ICollection<ProveedorDTO> getProveedores(int idProveedor, int tipoCambio);

        ICollection<DocumentoNegociableDTO> GetDocumentoNegociable(int factura, int numProveedor);
        decimal GetAbono(string factura, string numProv, decimal saldo, out decimal diff);
        void updateEstatus(bool estatus, int id);
        void Guardar(tblC_Anticipo obj);
        void Guardar(tblC_Linea obj);
       
        void Guardar(tblC_FacturaParcial obj);
        void Guardar(tblC_CadenaProductiva array);
        tblC_Anticipo getObjAnticipo(int id);
        List<tblC_Anticipo> getLstAnticipo(string numProveedor);
        List<tblC_Anticipo> getLstAnticipo(int moneda);
        List<tblC_Anticipo> getLstAnticipo(BusqConcentradoDTO busq);
        List<tblC_Anticipo> getLstAnticipo(List<string> lstCc);
        List<tblC_CadenaProductiva> getLstCadenasPagadas(List<string> lstCC);
        List<tblC_FacturaParcial> GetParcialPorPrincipal(int idPrincial);
        List<tblC_CadenaProductiva> GetDocumentoPorPrincipal(int idPrincial);
        List<tblC_CadenaProductiva> getLstCadenasPagadas(BusqConcentradoDTO busq);
        List<tblC_CadenaProductiva> GetDocumentosGuardados();
        List<tblC_CadenaProductiva> GetDocumentosGuardados(int idPrincial);
        List<tblC_CadenaProductiva> GetDocumentosAplicados();
        List<tblC_CadenaProductiva> GetDocumentosAplicados(int idPrincial);
        List<tblC_CadenaProductiva> GetDocumentosAplicados(DateTime inicio, DateTime fin);
        List<tblC_CadenaProductiva> GetAllDocumentos();

        List<tblC_CadenaProductiva> getDocumentoGuardado(int id);
        decimal GetTipoCambioRegistro(string factura, string numProv);
        void setDocumentoGuardado(int id, string Factoraje, DateTime FechaEmision, DateTime FechaVencimiento, int? IF, string Banco);

        ICollection<ProveedorDTO> ListaPRoveedores();
        string getCCVencimiento(string numpro, string factura);
        void SetPago();
        tblC_FechaPago getUltimaFechaPago();

        #region ResumenSemanal
        List<tblC_CadenaProductiva> lstCompletaCadenaProductiva();
        List<tblC_Linea> lstLinea();
        bool GetPago(int factura, int proveedor, DateTime fecha);
        #endregion
        #region Intereses Nafin
        List<tblC_InteresesNafin> getlstInteresesNafin(DateTime fecha);
        List<tblC_InteresesNafinDetalle> getlstInteresesNafinDetalle(BusqConcentradoDTO busq);
        List<tblC_InteresesNafinDetalle> getlstInteresesNafinDetalle(List<string> lstCc);
        bool guardarInteresesNafin(List<tblC_InteresesNafin> lst);
        #endregion
        void Eliminar(tblC_CadenaProductiva obj);
        void Eliminar(tblC_FacturaParcial obj);
        void ReasignaBanco();
        tblC_CadenaProductiva getMonto(string factura, string numProveedor);
        List<string> lstObraCerradas();
        List<CcDTO> lstObra(int division);
        List<CcDTO> lstObra();
        List<CcDTO> lstObraAC();
        List<CcDTO> lstObraActivas();
        void asignaDolar();
        decimal getDolarDelDia(DateTime dia);
        List< ComboDTO> FillComboProv();
        #region ProyeccionCierre
        List<tblC_FED_DetProyeccionCierre> getLstCadenaProductiva(List<string> lstCC);
        List<tblC_FED_DetProyeccionCierre> getLstMoviminetoProveedor(List<string> lstCC);
        List<tblC_FED_DetProyeccionCierre> getLstMoviminetoArrendadora(List<string> lstCC);
        #endregion
        #region ProyeccionCierreArrendadora
        List<tblC_FED_DetProyeccionCierre> getLstMoviminetoProveedorArrendadora(BusqProyeccionCierreDTO busq);
        List<tblC_FED_DetProyeccionCierre> getLstMoviminetoArrendadora(BusqProyeccionCierreDTO busq);
        #endregion
    }
}
