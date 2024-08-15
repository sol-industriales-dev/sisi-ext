using Core.DAO.Contabilidad.Reportes;
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

namespace Core.Service.Contabilidad.Reportes
{
    public class CadenaProductivaServices : ICadenaProductivaDAO
    {
        #region Atributos
        private ICadenaProductivaDAO m_cadenaProductivaDAO;
        #endregion
        #region Propiedades
        public ICadenaProductivaDAO CadenaProductivaDAO
        {
            get { return m_cadenaProductivaDAO; }
            set { m_cadenaProductivaDAO = value; }
        }
        #endregion
        #region Constructores
        public CadenaProductivaServices(ICadenaProductivaDAO cadenaProductivaDAO)
        {
            this.CadenaProductivaDAO = cadenaProductivaDAO;
        }
        #endregion

        public ICollection<VencimientoDTO> getInforVencimiento(string nomprov, int tipoFactura)
        {
            return this.CadenaProductivaDAO.getInforVencimiento(nomprov, tipoFactura);
        }

        public ICollection<ProveedorDTO> getProveedores(int idProveedor, int tipoCambio)
        {
            return this.CadenaProductivaDAO.getProveedores(idProveedor, tipoCambio);
        }

        public ICollection<DocumentoNegociableDTO> GetDocumentoNegociable(int factura, int numProveedor)
        {

            return this.CadenaProductivaDAO.GetDocumentoNegociable(factura, numProveedor);
        }
        public decimal GetAbono(string factura, string numProv, decimal saldo, out decimal diff)
        {
            return this.CadenaProductivaDAO.GetAbono(factura, numProv, saldo, out diff);
        }
        public void updateEstatus(bool estatus, int id)
        {
            this.CadenaProductivaDAO.updateEstatus(estatus, id);
        }
        public void Guardar(tblC_Anticipo obj)
        {
            this.CadenaProductivaDAO.Guardar(obj);
        }
        public void Guardar(tblC_Linea obj)
        {
            this.CadenaProductivaDAO.Guardar(obj);
        }
        public void Guardar(tblC_FacturaParcial obj)
        {
            this.CadenaProductivaDAO.Guardar(obj);
        }
        public void Guardar(tblC_CadenaProductiva array)
        {
            this.CadenaProductivaDAO.Guardar(array);
        }
        public tblC_Anticipo getObjAnticipo(int id)
        {
            return this.CadenaProductivaDAO.getObjAnticipo(id);
        }
        public List<tblC_Anticipo> getLstAnticipo(string numProveedor)
        {
            return this.CadenaProductivaDAO.getLstAnticipo(numProveedor);
        }
        public List<tblC_Anticipo> getLstAnticipo(int moneda)
        {
            return this.CadenaProductivaDAO.getLstAnticipo(moneda);
        }
        public List<tblC_Anticipo> getLstAnticipo(BusqConcentradoDTO busq)
        {
            return CadenaProductivaDAO.getLstAnticipo(busq);
        }
        public List<tblC_Anticipo> getLstAnticipo(List<string> lstCc)
        {
            return CadenaProductivaDAO.getLstAnticipo(lstCc);
        }
        public List<tblC_FacturaParcial> GetParcialPorPrincipal(int idPrincial)
        {
            return this.CadenaProductivaDAO.GetParcialPorPrincipal(idPrincial);
        }
        public List<tblC_CadenaProductiva> GetDocumentoPorPrincipal(int idPrincial)
        {
            return this.CadenaProductivaDAO.GetDocumentoPorPrincipal(idPrincial);
        }
        /// <summary>
        /// Consulta las cadenas productivas pagadas
        /// </summary>
        /// <param name="busq">Busqueda de concentradp</param>
        /// <returns>Cadenas productivas</returns>
        public List<tblC_CadenaProductiva> getLstCadenasPagadas(BusqConcentradoDTO busq)
        {
            return CadenaProductivaDAO.getLstCadenasPagadas(busq);
        }
        /// <summary>
        /// Consulta las cadenas productivas pagadas
        /// </summary>
        /// <param name="lstCC">centro de costos filtrados</param>
        /// <returns>Cadenas productivas</returns>
        public List<tblC_CadenaProductiva> getLstCadenasPagadas(List<string> lstCC)
        {
            return CadenaProductivaDAO.getLstCadenasPagadas(lstCC);
        }
        public List<tblC_CadenaProductiva> GetDocumentosGuardados()
        {
            return this.CadenaProductivaDAO.GetDocumentosGuardados();
        }
        public List<tblC_CadenaProductiva> GetDocumentosGuardados(int idPrincial)
        {
            return this.CadenaProductivaDAO.GetDocumentosGuardados(idPrincial);
        }
        public List<tblC_CadenaProductiva> GetDocumentosAplicados()
        {
            return this.CadenaProductivaDAO.GetDocumentosAplicados();
        }
        public List<tblC_CadenaProductiva> GetDocumentosAplicados(int idPrincial)
        {
            return this.CadenaProductivaDAO.GetDocumentosAplicados(idPrincial);
        }
        public List<tblC_CadenaProductiva> GetDocumentosAplicados(DateTime inicio, DateTime fin)
        {
            return this.CadenaProductivaDAO.GetDocumentosAplicados(inicio, fin);
        }
        public List<tblC_CadenaProductiva> getDocumentoGuardado(int id)
        {
            return this.CadenaProductivaDAO.getDocumentoGuardado(id);

        }
        public bool enviarCorreo()
        {
            return this.CadenaProductivaDAO.enviarCorreo();
        }
        public bool enviarCorreoPropuesta()
        {
            return this.CadenaProductivaDAO.enviarCorreoPropuesta();
        }
        public List<tblC_CadenaProductiva> GetAllDocumentos()
        {
            return this.CadenaProductivaDAO.GetAllDocumentos();
        }
        public decimal GetTipoCambioRegistro(string factura, string numProv)
        {
            return this.CadenaProductivaDAO.GetTipoCambioRegistro(factura, numProv);
        }
        public void setDocumentoGuardado(int id, string Factoraje, DateTime FechaEmision, DateTime FechaVencimiento, int? IF, string Banco)
        {
            this.CadenaProductivaDAO.setDocumentoGuardado(id, Factoraje, FechaEmision, FechaVencimiento, IF, Banco);

        }
        public ICollection<ProveedorDTO> ListaPRoveedores()
        {
            return this.CadenaProductivaDAO.ListaPRoveedores();
        }

        public string getCCVencimiento(string numpro, string factura)
        {
            return this.CadenaProductivaDAO.getCCVencimiento(numpro, factura);
        }
        public void SetPago()
        {
            this.CadenaProductivaDAO.SetPago();
        }
        public tblC_FechaPago getUltimaFechaPago()
        {
            return this.CadenaProductivaDAO.getUltimaFechaPago();
        }
        #region ResumenSemanal
        public List<tblC_CadenaProductiva> lstCompletaCadenaProductiva()
        {
            return this.CadenaProductivaDAO.lstCompletaCadenaProductiva();
        }
        public List<tblC_Linea> lstLinea()
        {
            return this.CadenaProductivaDAO.lstLinea();
        }
        public bool GetPago(int factura, int proveedor, DateTime fecha)
        {
            return this.CadenaProductivaDAO.GetPago(factura, proveedor, fecha);
        }
        #endregion
        #region Intereses Nafin
        /// <summary>
        /// Consulta de intereses bancarios de las cadenas productivas vencida
        /// </summary>
        /// <param name="fecha">Fecha de la semana</param>
        /// <returns>Lista de intereses</returns>
        public List<tblC_InteresesNafin> getlstInteresesNafin(DateTime fecha)
        {
            return CadenaProductivaDAO.getlstInteresesNafin(fecha);
        }
        /// <summary>
        /// Consulta de los detalles de intereses bancarios de las cadenas productivas vencida
        /// </summary>
        /// <param name="fecha">Fecha de la semana</param>
        /// <returns>Lista de detalles de intereses</returns>
        public List<tblC_InteresesNafinDetalle> getlstInteresesNafinDetalle(BusqConcentradoDTO busq)
        {
            return CadenaProductivaDAO.getlstInteresesNafinDetalle(busq);
        }
        public List<tblC_InteresesNafinDetalle> getlstInteresesNafinDetalle(List<string> lstCc)
        {
            return CadenaProductivaDAO.getlstInteresesNafinDetalle(lstCc);
        }
        /// <summary>
        /// Guarda o actualiza los intereses de nafin
        /// </summary>
        /// <param name="lst">Intereses a guardar o actualizar </param>
        /// <returns>guardado realizado</returns>
        public bool guardarInteresesNafin(List<tblC_InteresesNafin> lst)
        {
            return CadenaProductivaDAO.guardarInteresesNafin(lst);
        }
        #endregion
        public void Eliminar(tblC_CadenaProductiva obj)
        {
            this.CadenaProductivaDAO.Eliminar(obj);
        }
        public void Eliminar(tblC_FacturaParcial obj)
        {
            this.CadenaProductivaDAO.Eliminar(obj);
        }
        public void ReasignaBanco()
        {
            this.CadenaProductivaDAO.ReasignaBanco();
        }
        public List<string> lstObraCerradas()
        {
            return this.CadenaProductivaDAO.lstObraCerradas();
        }
        public List<CcDTO> lstObra(int division)
        {
            return this.CadenaProductivaDAO.lstObra(division);
        }
        public List<CcDTO> lstObra()
        {
            return this.CadenaProductivaDAO.lstObra();
        }
        public List<CcDTO> lstObraAC()
        {
            return this.CadenaProductivaDAO.lstObraAC();
        }
        public List<CcDTO> lstObraActivas()
        {
            return CadenaProductivaDAO.lstObraActivas();
        }
        public tblC_CadenaProductiva getMonto(string factura, string numProveedor)
        {
            return this.CadenaProductivaDAO.getMonto(factura, numProveedor);
        }
        public void asignaDolar()
        {
            this.CadenaProductivaDAO.asignaDolar();
        }
        /// <summary>
        /// Consulta de dolar al día anterior
        /// </summary>
        /// <param name="dia">fecha de consulta</param>
        /// <returns>tipo de cambio del dolar</returns>
        public decimal getDolarDelDia(DateTime dia)
        {
            return this.CadenaProductivaDAO.getDolarDelDia(dia);
        }
        public List<ComboDTO> FillComboProv()
        {
            return this.CadenaProductivaDAO.FillComboProv();
        }
        #region ProyeccionCierre
        public List<tblC_FED_DetProyeccionCierre> getLstCadenaProductiva(List<string> lstCC)
        {
            return CadenaProductivaDAO.getLstCadenaProductiva(lstCC);
        }
        public List<tblC_FED_DetProyeccionCierre> getLstMoviminetoProveedor(List<string> lstCC)
        {
            return CadenaProductivaDAO.getLstMoviminetoProveedor(lstCC);
        }
        public List<tblC_FED_DetProyeccionCierre> getLstMoviminetoArrendadora(List<string> lstCC)
        {
            return CadenaProductivaDAO.getLstMoviminetoArrendadora(lstCC);
        }
        #endregion
        #region ProyeccionCierreArrendadora
        public List<tblC_FED_DetProyeccionCierre> getLstMoviminetoProveedorArrendadora(BusqProyeccionCierreDTO busq)
        {
            return CadenaProductivaDAO.getLstMoviminetoProveedorArrendadora(busq);
        }
        public List<tblC_FED_DetProyeccionCierre> getLstMoviminetoArrendadora(BusqProyeccionCierreDTO busq)
        {
            return CadenaProductivaDAO.getLstMoviminetoArrendadora(busq);
        }
        #endregion
    }
}
