using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Catalogo;
using Core.DTO.Maquinaria.Overhaul;
using Core.DTO.Principal.Generales;

namespace Core.DAO.Maquinaria.Overhaul
{
    public interface IPresupuestoOverhaulDAO
    {
        List<PresupuestoOverhaulDTO> CargarTblPresupuesto(List<string> obras, int modeloID, int anio, List<tblM_CatMaquina> maquinas, tblM_PresupuestoOverhaul objPresupuesto);
        List<tblM_CatMaquina> GetMaquinasPresupuestar(List<string> obras, int modeloID, int anio);
        List<DetallePresupuestoDTO> GetDetallePresupuesto(List<string> obra, int anio, int vidas, int modelo, int subConjunto, int presupuestoID, bool esServicio);
        List<ComboDTO> fillCboAnioPresupuesto();
        int GuardarCostoPresupuesto(int componenteID, int maquinaID, decimal costo, decimal costoSugerido, int eventoID, int modelo, int anio, int presupuestoID, int vidas, bool esServicio);
        List<PresupuestoPorObraDTO> CargarTblAvance(List<string> obras, int modeloID, int anio, int estatus);
        List<DetallePresupuestoOverhaul> CargarTblAvanceDetalle(int idDetalle);
        List<PresupuestoPorObraDTO> CargarAvanceGeneral(List<string> obras, int modeloID, int anio, int estatus);
        List<tblM_PresupuestoOverhaul> CargarTblAutorizacion(List<string> obras, int modeloID, int anio);
        List<DetallePresupuestoDTO> GetDetallePresAuto(List<string> obras, int modelo, int anio);
        tblM_PresupuestoOverhaul GetPresupuesto(int modeloID, int anio);
        List<tblM_PresupuestoOverhaul> GetPresupuestos(List<int> modelos, int anio);
        tblM_DetallePresupuestoOverhaul GetDetalleByComp(int componenteID, int anio);
        bool AutorizarPresupuesto(int presupuestoID, int modelo, int anio, string obra, int tipo);
        List<tblM_DetallePresupuestoOverhaul> GetDetalleModalAuto(int presupuestoID, string obra);
        decimal GuardarAumentoPresupuesto(decimal aumento, string comentario, int presupuestoID, int componenteID, int tipo);
        string ComentarioAumPresupuesto(int presupuestoID, int componenteID);
        int IniciarPresupuesto(int modelo, int anio);
        bool CerrarPresupuesto(int presupuestoID);

        List<ReporteInversionOverhaulDTO> GetDetallePresupuestos(List<int> presupuestosID, List<string> obras, int anio, List<int> modelos);

        List<ComboDTO> CargarModelosRptInversion();
        List<ComboDTO> CargarObrasRptInversion();
        List<ComboDTO> CargarAnioRptInversion();

        string cargarComponentesAPresupuesto();

        List<tblM_PresupuestoHC> GetPresupuestoHC(List<string> obras);

        List<PresupuestoComponenteDTO> CargarPresupuestoPorComponente(int anio, int modelo, int subconjunto, string noComponente);
    }
}

