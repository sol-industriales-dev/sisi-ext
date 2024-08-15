using Core.DTO.Captura;
using Core.DTO.Maquinaria.SOS;
using Core.DTO.Reportes;
using Core.Entity.Maquinaria;
using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Maquinaria.Captura;
using Core.DTO.Principal.Generales;

namespace Core.DAO.Maquinaria.Captura
{
    public interface ICapturaHorometroDAO
    {
        List<CapHorometroDTO> getDataTable(string cc, int turno, DateTime fecha, int tipo);
        List<tblM_CapHorometro> getTableByRangeDateTipo(DateTime start, DateTime end, string Grupo);

        List<tblM_CapHorometro> getHorasRangoFecha(DateTime start, DateTime end, int tipo);

        void Guardar(List<tblM_CapHorometro> obj);
        List<tblM_CapHorometro> getDataTableByRangeDate(string noeco, DateTime start, DateTime end);
        List<tblM_CapHorometro> getDataTableByRangeDate(DateTime start, DateTime end, List<string> listaEco);
        string getCentroCostos(string cc);
        tblM_CapHorometro getDatoHorometro(string maquina);
        void Guardar(tblM_CapHorometro obj);
        List<CapHorometroDTO> getReporteDiario(tblM_CapHorometro obj);

        List<tblM_CapHorometro> getTableInfoHorometros(string cc, int turno, DateTime fechaInicia, DateTime fechaFinal, string economico, string ccFiltro, int grupo, int modelo, decimal hInicial, decimal hFinal, bool estatus);

        Dictionary<string, object> GetEconomicosSinHorometros(string areaCuenta, string economico, DateTime fechaInicio);

        decimal GetHorometroFinal(string Econ);
        decimal GetHorometroInicial(string Econ);

        tblM_CapHorometro getUltimoHorometro(string Econ);

        decimal getTotalHorometros(string Econ);

        List<CCMuestrasDTO> getListaCentrosCostos(DateTime fechaInicia, DateTime fechaFinal, string economico);
        List<tblM_CapHorometro> getHorometrosEficiencia(string Econ, DateTime Fecha, string cc);

        List<tblM_CapHorometro> getHorasSoloRangoFecha(DateTime start, DateTime end);

        List<tblM_CapHorometro> getHorometro(string Econ, DateTime Fecha);

        List<EconomicosHrsDTO> getReporteHorometro(List<string> cc, DateTime fechaInicio, DateTime fechaFin);

        MemoryStream exportarArchvio(List<EconomicosHrsDTO> listaEconomicosHrsDTO);
        List<ComboDTO> obtenerCentrosCostos();
        /// <summary>
        /// Consutla cc o ac dependiendo de la empresa
        /// </summary>
        /// <returns>Lista de cc o ac</returns>
        List<cboDTO> cboCentroCostos();
        //List<tblM_CapHorometro> getTableInfoHorometros(string cc, DateTime fechaInicia, DateTime fechaFinal);
        //   List<tblM_CapHorometro> getTableInfoHorometros(List<string> cc, DateTime fechaInicia, DateTime fechaFinal);

        bool GuardarHorasComponente(List<tblM_CapHorometro> array);
        List<tblM_CapHorometro> getHorometrosEconomicos(List<string> Economicos, DateTime min, DateTime max);
        List<string> getUpdatedStandBy(List<string> lst,int tipo);

        bool getCorteKubrixAC(string ac);
        bool GuardarCorteKubrixAC(string ac);
        byte[] GetReporteHorometrosKubrix(string ac);

        List<string> GetCorreoGerenteAdmin(string CC);
    }


}
