using Core.DTO.Maquinaria.Captura.KPI;
using Core.Entity.Maquinaria.Catalogo;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Auth;
using Core.Entity.Maquinaria.KPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Maquinaria.KPI.Dashboard;
using Core.DTO;
using Core.Entity.Maquinaria;
using Core.DTO.Maquinaria.KPI;

namespace Core.DAO.Maquinaria.KPI
{
    public interface IKPIDAO
    {
        //DASHBOARD
        Dictionary<string, object> GetInfoFiltro(FiltroDTO filtro);
        Dictionary<string, object> GetInfoFiltroCodigo(FiltroDTO filtro, string codigo, int tipo);
        //DASHBOARD FIN

        Dictionary<string, object> GuardarCodigoParo(tblM_KPI_CodigosParo obj);
        Dictionary<string, object> CargarCodigosParo(string codigoParo);
        List<tblM_KPI_CodigosParo> CodigosParo(string ac);
        List<tblM_KPI_Homologado> CargarCapturaDiaria(BusqKpiDiariaDTO busq);
        bool GuardarAutorizacion(tblM_KPI_AuthHomologado auth);
        List<tblM_KPI_Homologado> CargarCapturaBit(int id);
        List<tblM_KPI_AuthHomologado> CargarAutorizantes(BusqKpiAuthDTO busq);
        List<tblM_CatMaquina> CargarMaquinas(List<int> ids);
        List<authDTO> AuthCargar(int id);
        tblM_KPI_AuthHomologado ConsultaAutorizante(int id);
        List<ComboDTO> ComboPeriodo();
        List<ComboDTO> ComboAreaCuenta();
        List<tblM_CatModeloEquipo> CboModeloEquipos(int grupoID);
        List<tblM_CatGrupoMaquinaria> CboGrupoEquipos(string areaCuenta);
        Dictionary<string, object> GetCapturaDiaria(BusqKpiDiariaDTO busq);
        Dictionary<string, object> saveOrUpdateCapturaDiaria(List<tblM_KPI_Homologado> capturaDiaria, List<tblM_CapHorometro> horometros);
        Dictionary<string, object> GuardarSemana(string ac, DateTime fechaInicio, DateTime fechaFinal, int semana);
        Dictionary<string, object> ValidarConcentrado(string ac, DateTime fechaInicio, DateTime fechaFinal, int semana);
        List<tblM_KPI_Homologado> CargarCapturaBitFechas(DateTime fechaInicio, DateTime fechaFin, string ac);
        List<ComboDTO> FillCboCC();
        List<ComboDTO> FillCboGrupos();
        List<ComboDTO> FillCboGruposEnCaptura(List<string> lstCC);
        List<ComboDTO> FillCboModelos(List<int> lstGrupoID);
        List<ComboDTO> FillCboModelosEnCaptura(List<string> lstCC, List<int> lstGrupoID);
        List<ComboDTO> FillCboEconomico(List<string> lstCC, List<int> lstGrupoID, List<int> lstModeloID);
        Dictionary<string, object> GetConcentradoKPI(string ac, int grupoID, int modeloID, DateTime fechaInicio, DateTime fechaFin);
        Dictionary<string, object> getHorasDia(string ac);
        decimal getHorasDiaDec(string ac);
        
        List<tblM_KPI_AuthHomologado> CargarPendientes(BusqKpiAuthDTO busq);
        tblM_KPI_KPICapturaBit getHomologadobit(int id);

        #region REPORTE AUTORIZACIONES
        Dictionary<string, object> GetInfoGraficasPDF(FiltroDTO filtro);
        List<string> GetLstCorreosFacultamientos(string ac);
        #endregion
    }
}