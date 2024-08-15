using Core.DAO.Maquinaria.Overhaul;
using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Catalogo;
using Core.DTO.Maquinaria.Overhaul;
using Core.DTO.Principal.Generales;

namespace Core.Service.Maquinaria.Overhaul
{
    public class PresupuestoOverhaulServices : IPresupuestoOverhaulDAO
    {
        #region Atributos
        private IPresupuestoOverhaulDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private IPresupuestoOverhaulDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public PresupuestoOverhaulServices(IPresupuestoOverhaulDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores

        public List<PresupuestoOverhaulDTO> CargarTblPresupuesto(List<string> obras, int modeloID, int anio, List<tblM_CatMaquina> maquinas, tblM_PresupuestoOverhaul objPresupuesto)
        {
            return interfazDAO.CargarTblPresupuesto(obras, modeloID, anio, maquinas, objPresupuesto);
        }
        public List<tblM_CatMaquina> GetMaquinasPresupuestar(List<string> obras, int modeloID, int anio)
        {
            return interfazDAO.GetMaquinasPresupuestar(obras, modeloID, anio);
        }
        public List<DetallePresupuestoDTO> GetDetallePresupuesto(List<string> obras, int anio, int vidas, int modelo, int subConjunto, int presupuestoID, bool esServicio)
        {
            return interfazDAO.GetDetallePresupuesto(obras, anio, vidas, modelo, subConjunto, presupuestoID, esServicio);
        }
        public List<ComboDTO> fillCboAnioPresupuesto()
        {
            return interfazDAO.fillCboAnioPresupuesto();
        }
        public int GuardarCostoPresupuesto(int componenteID, int maquinaID, decimal costo, decimal costoSugerido, int eventoID, int modelo, int anio, int presupuestoID, int vidas, bool esServicio)
        {
            return interfazDAO.GuardarCostoPresupuesto(componenteID, maquinaID, costo, costoSugerido, eventoID, modelo, anio, presupuestoID, vidas, esServicio);
        }
        public List<tblM_PresupuestoOverhaul> CargarTblAutorizacion(List<string> obras, int modeloID, int anio)
        {
            return interfazDAO.CargarTblAutorizacion(obras, modeloID, anio);
        }
        public List<PresupuestoPorObraDTO> CargarTblAvance(List<string> obras, int modeloID, int anio, int estatus)
        {
            return interfazDAO.CargarTblAvance(obras, modeloID, anio, estatus);
        }
        public List<DetallePresupuestoOverhaul> CargarTblAvanceDetalle(int idDetalle)
        {
            return interfazDAO.CargarTblAvanceDetalle(idDetalle);
        }
        public List<PresupuestoPorObraDTO> CargarAvanceGeneral(List<string> obras, int modeloID, int anio, int estatus)
        {
            return interfazDAO.CargarAvanceGeneral(obras, modeloID, anio, estatus);
        }
        public List<DetallePresupuestoDTO> GetDetallePresAuto(List<string> obras, int modelo, int anio)
        {
            return interfazDAO.GetDetallePresAuto(obras, modelo, anio);
        }
        public tblM_PresupuestoOverhaul GetPresupuesto(int modelo, int anio)
        {
            return interfazDAO.GetPresupuesto(modelo, anio);
        }
        public List<tblM_PresupuestoOverhaul> GetPresupuestos(List<int> modelos, int anio)
        {
            return interfazDAO.GetPresupuestos(modelos, anio);
        }
        public tblM_DetallePresupuestoOverhaul GetDetalleByComp(int componenteID, int anio)
        {
            return interfazDAO.GetDetalleByComp(componenteID, anio);
        }
        public bool AutorizarPresupuesto(int presupuestoID, int modelo, int anio, string obra, int tipo)
        {
            return interfazDAO.AutorizarPresupuesto(presupuestoID, modelo, anio, obra, tipo);
        }
        public List<tblM_DetallePresupuestoOverhaul> GetDetalleModalAuto(int presupuestoID, string obra)
        {
            return interfazDAO.GetDetalleModalAuto(presupuestoID, obra);
        }
        public decimal GuardarAumentoPresupuesto(decimal aumento, string comentario, int presupuestoID, int componenteID, int tipo)
        {
            return interfazDAO.GuardarAumentoPresupuesto(aumento, comentario, presupuestoID, componenteID, tipo);
        }
        public string ComentarioAumPresupuesto(int presupuestoID, int componenteID)
        {
            return interfazDAO.ComentarioAumPresupuesto(presupuestoID, componenteID);
        }
        public int IniciarPresupuesto(int modelo, int anio)
        {
            return interfazDAO.IniciarPresupuesto(modelo, anio);
        }
        public bool CerrarPresupuesto(int presupuestoID)
        {
            return interfazDAO.CerrarPresupuesto(presupuestoID);
        }

        public List<ReporteInversionOverhaulDTO> GetDetallePresupuestos(List<int> presupuestosID, List<string> obras, int anio, List<int> modelos)
        {
            return interfazDAO.GetDetallePresupuestos(presupuestosID, obras, anio, modelos);
        }

        public List<ComboDTO> CargarModelosRptInversion()
        {
            return interfazDAO.CargarModelosRptInversion();
        }
        public List<ComboDTO> CargarObrasRptInversion()
        {
            return interfazDAO.CargarObrasRptInversion();
        }
        public List<ComboDTO> CargarAnioRptInversion()
        {
            return interfazDAO.CargarAnioRptInversion();
        }


        public string cargarComponentesAPresupuesto()
        {
            return interfazDAO.cargarComponentesAPresupuesto();
        }
        public List<tblM_PresupuestoHC> GetPresupuestoHC(List<string> obras)
        {
            return interfazDAO.GetPresupuestoHC(obras);
        }
        public List<PresupuestoComponenteDTO> CargarPresupuestoPorComponente(int anio, int modelo, int subconjunto, string noComponente)
        {
            return interfazDAO.CargarPresupuestoPorComponente(anio, modelo, subconjunto, noComponente);
        }
    }
}
