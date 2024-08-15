using Core.DAO.Maquinaria.KPI;
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

namespace Core.Service.Maquinaria.KPI
{
    public class KPIService : IKPIDAO
    {
        #region Variables y constructor
        private IKPIDAO objDAO { get; set; }

        public KPIService(IKPIDAO interfazDAO)
        {
            objDAO = interfazDAO;
        }
        #endregion

        #region Dashboard
        public Dictionary<string, object> GetInfoFiltro(FiltroDTO filtro)
        {
            return objDAO.GetInfoFiltro(filtro);
        }
        public Dictionary<string, object> GetInfoFiltroCodigo(FiltroDTO filtro, string codigo, int tipo)
        {
            return objDAO.GetInfoFiltroCodigo(filtro, codigo, tipo);
        }
        #endregion

        #region Codigo de Paros

        public Dictionary<string, object> GuardarCodigoParo(tblM_KPI_CodigosParo obj)
        {
            return objDAO.GuardarCodigoParo(obj);
        }
        public Dictionary<string, object> CargarCodigosParo(string codigoParo)
        {
            return objDAO.CargarCodigosParo(codigoParo);
        }
        public List<tblM_KPI_CodigosParo> CodigosParo(string ac)
        {
            return objDAO.CodigosParo(ac);
        }
        #endregion
        public Dictionary<string, object> GetCapturaDiaria(BusqKpiDiariaDTO busq)
        {
            return objDAO.GetCapturaDiaria(busq);
        }
        public List<tblM_KPI_Homologado> CargarCapturaDiaria(BusqKpiDiariaDTO busq)
        {
            return objDAO.CargarCapturaDiaria(busq);
        }
        public List<tblM_CatGrupoMaquinaria> CboGrupoEquipos(string areaCuenta)
        {
            return objDAO.CboGrupoEquipos(areaCuenta);
        }
        public List<tblM_CatModeloEquipo> CboModeloEquipos(int grupoID)
        {
            return objDAO.CboModeloEquipos(grupoID);
        }
        public Dictionary<string, object> saveOrUpdateCapturaDiaria(List<tblM_KPI_Homologado> capturaDiaria , List<tblM_CapHorometro> horometros)
        {
            return objDAO.saveOrUpdateCapturaDiaria(capturaDiaria, horometros);
        }
        public bool GuardarAutorizacion(tblM_KPI_AuthHomologado auth)
        {
            return objDAO.GuardarAutorizacion(auth);
        }
        public List<tblM_KPI_Homologado> CargarCapturaBit(int id)
        {
            return objDAO.CargarCapturaBit(id);
        }
        public List<tblM_KPI_AuthHomologado> CargarAutorizantes(BusqKpiAuthDTO busq)
        {
            return objDAO.CargarAutorizantes(busq);
        }
        public List<authDTO> AuthCargar(int id)
        {
            return objDAO.AuthCargar(id);
        }
        public tblM_KPI_AuthHomologado ConsultaAutorizante(int id)
        {
            return objDAO.ConsultaAutorizante(id);
        }
        public List<tblM_CatMaquina> CargarMaquinas(List<int> ids)
        {
            return objDAO.CargarMaquinas(ids);
        }
        public List<ComboDTO> ComboPeriodo()
        {
            return objDAO.ComboPeriodo();
        }
        public List<ComboDTO> ComboAreaCuenta()
        {
            return objDAO.ComboAreaCuenta();
        }
        public Dictionary<string, object> GuardarSemana(string ac, DateTime fechaInicio, DateTime fechaFinal, int semana)
        {
            return objDAO.GuardarSemana(ac, fechaInicio, fechaFinal, semana);
        }

        public Dictionary<string, object> ValidarConcentrado(string ac, DateTime fechaInicio, DateTime fechaFinal, int semana)
        {
            return objDAO.ValidarConcentrado(ac, fechaInicio, fechaFinal, semana);
        }

        public List<tblM_KPI_Homologado> CargarCapturaBitFechas(DateTime fechaInicio, DateTime fechaFin, string ac)
        {
            return objDAO.CargarCapturaBitFechas(fechaInicio, fechaFin, ac);
        }

        public List<ComboDTO> FillCboCC()
        {
            return objDAO.FillCboCC();
        }
        public List<ComboDTO> FillCboGrupos()
        {
            return objDAO.FillCboGrupos();
        }
        public List<ComboDTO> FillCboGruposEnCaptura(List<string> lstCC)
        {
            return objDAO.FillCboGruposEnCaptura(lstCC);
        }

        public List<ComboDTO> FillCboModelos(List<int> lstGrupoID)
        {
            return objDAO.FillCboModelos(lstGrupoID);
        }
        public List<ComboDTO> FillCboModelosEnCaptura(List<string> lstCC, List<int> lstGrupoID)
        {
            return objDAO.FillCboModelosEnCaptura(lstCC, lstGrupoID);
        }
        public List<ComboDTO> FillCboEconomico(List<string> lstCC, List<int> lstGrupoID, List<int> lstModeloID)
        {
            return objDAO.FillCboEconomico(lstCC, lstGrupoID, lstModeloID);
        }

        public Dictionary<string, object> GetConcentradoKPI(string ac, int grupoID, int modeloID, DateTime fechaInicio, DateTime fechaFin)
        {
            return objDAO.GetConcentradoKPI(ac, grupoID, modeloID, fechaInicio, fechaFin);
        }
        public Dictionary<string, object> getHorasDia(string ac)
        {
            return objDAO.getHorasDia(ac);
        }
        public decimal getHorasDiaDec(string ac)
        {
            return objDAO.getHorasDiaDec(ac);
        }
        public List<tblM_KPI_AuthHomologado> CargarPendientes(BusqKpiAuthDTO busq)
        {
            return objDAO.CargarPendientes(busq);
        }
        public tblM_KPI_KPICapturaBit getHomologadobit(int id)
        {
            return objDAO.getHomologadobit(id);
        }

        #region REPORTE AUTORIZACIONES
        public Dictionary<string, object> GetInfoGraficasPDF(FiltroDTO filtro)
        {
            return objDAO.GetInfoGraficasPDF(filtro);
        }

        public List<string> GetLstCorreosFacultamientos(string ac)
        {
            return objDAO.GetLstCorreosFacultamientos(ac);
        }
        #endregion
    }
}
