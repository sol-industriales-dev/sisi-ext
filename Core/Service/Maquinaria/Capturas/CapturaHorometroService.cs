using Core.DAO.Maquinaria.Captura;
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

namespace Core.Service.Maquinaria.Capturas
{
    public class CapturaHorometroService : ICapturaHorometroDAO
    {
        #region Atributos
        private ICapturaHorometroDAO m_CapturaHorometroDAO;
        #endregion
        #region Propiedades
        public ICapturaHorometroDAO CapturaHorometroDAO
        {
            get { return m_CapturaHorometroDAO; }
            set { m_CapturaHorometroDAO = value; }
        }
        #endregion
        #region Constructores
        #endregion
        public CapturaHorometroService(ICapturaHorometroDAO capturaHorometroDAO)
        {
            this.CapturaHorometroDAO = capturaHorometroDAO;
        }
        public List<CapHorometroDTO> getDataTable(string cc, int turno, DateTime fecha, int tipo)
        {
            return CapturaHorometroDAO.getDataTable(cc, turno, fecha, tipo);
        }

        public void Guardar(List<tblM_CapHorometro> obj)
        {
            CapturaHorometroDAO.Guardar(obj);
        }
        public void Guardar(tblM_CapHorometro obj)
        {
            CapturaHorometroDAO.Guardar(obj);
        }
        public List<tblM_CapHorometro> getDataTableByRangeDate(string noeco, DateTime start, DateTime end)
        {
            return CapturaHorometroDAO.getDataTableByRangeDate(noeco, start, end);
        }
        public List<tblM_CapHorometro> getDataTableByRangeDate(DateTime start, DateTime end, List<string> listaEco)
        {
            return CapturaHorometroDAO.getDataTableByRangeDate(start, end, listaEco);
        }

        public string getCentroCostos(string cc)
        {
            return CapturaHorometroDAO.getCentroCostos(cc);
        }
        public tblM_CapHorometro getDatoHorometro(string maquina)
        {
            return CapturaHorometroDAO.getDatoHorometro(maquina);
        }

        public List<CapHorometroDTO> getReporteDiario(tblM_CapHorometro obj)
        {
            return CapturaHorometroDAO.getReporteDiario(obj);
        }

        public List<tblM_CapHorometro> getTableInfoHorometros(string cc, int turno, DateTime fechaInicia, DateTime fechaFinal, string economico, string ccFiltro, int grupo, int modelo, decimal hInicial, decimal hFinal, bool estatus)
        {
            return CapturaHorometroDAO.getTableInfoHorometros(cc, turno, fechaInicia, fechaFinal, economico, ccFiltro, grupo, modelo, hInicial, hFinal, estatus);
        }

        public Dictionary<string, object> GetEconomicosSinHorometros(string areaCuenta, string economico, DateTime fechaInicio)
        {
            return CapturaHorometroDAO.GetEconomicosSinHorometros(areaCuenta, economico, fechaInicio);
        }

        public List<tblM_CapHorometro> getTableByRangeDateTipo(DateTime start, DateTime end, string Grupo)
        {
            return CapturaHorometroDAO.getTableByRangeDateTipo(start, end, Grupo);
        }
        public List<tblM_CapHorometro> getHorasRangoFecha(DateTime start, DateTime end, int tipo)
        {
            return CapturaHorometroDAO.getHorasRangoFecha(start, end, tipo);
        }


        public decimal GetHorometroFinal(string Econ)
        {
            return CapturaHorometroDAO.GetHorometroFinal(Econ);
        }

        public decimal GetHorometroInicial(string Econ)
        {
            return CapturaHorometroDAO.GetHorometroInicial(Econ);
        }

        public tblM_CapHorometro getUltimoHorometro(string Econ)
        {
            return CapturaHorometroDAO.getUltimoHorometro(Econ);
        }

        public decimal getTotalHorometros(string Econ)
        {
            return CapturaHorometroDAO.getTotalHorometros(Econ);
        }
        public List<CCMuestrasDTO> getListaCentrosCostos(DateTime fechaInicia, DateTime fechaFinal, string economico)
        {
            return CapturaHorometroDAO.getListaCentrosCostos(fechaInicia, fechaFinal, economico);
        }

        public List<tblM_CapHorometro> getHorometrosEficiencia(string Econ, DateTime Fecha, string cc)
        {
            return CapturaHorometroDAO.getHorometrosEficiencia(Econ, Fecha, cc);
        }

        public List<tblM_CapHorometro> getHorasSoloRangoFecha(DateTime start, DateTime end)
        {
            return CapturaHorometroDAO.getHorasSoloRangoFecha(start, end);
        }

        public  List<tblM_CapHorometro> getHorometro(string Econ, DateTime Fecha)
        {
            return CapturaHorometroDAO.getHorometro(Econ, Fecha);
        }

        public List<EconomicosHrsDTO> getReporteHorometro(List<string> cc,DateTime fechaInicio, DateTime fechaFin)
        {
            return CapturaHorometroDAO.getReporteHorometro(cc, fechaInicio, fechaFin);
        }

        public MemoryStream exportarArchvio(List<EconomicosHrsDTO> listaEconomicosHrsDTO)
        {
            return CapturaHorometroDAO.exportarArchvio(listaEconomicosHrsDTO);
        }
        public List<ComboDTO> obtenerCentrosCostos()
        {
            return CapturaHorometroDAO.obtenerCentrosCostos();
        }
        public List<cboDTO> cboCentroCostos()
        {
            return CapturaHorometroDAO.cboCentroCostos();
        }
        /* public List<tblM_CapHorometro> getTableInfoHorometros(List<string> cc, DateTime fechaInicia, DateTime fechaFinal)
         {
             return CapturaHorometroDAO.getTableInfoHorometros(cc, fechaInicia, fechaFinal);
         }
         */
        public bool GuardarHorasComponente(List<tblM_CapHorometro> array)
        {
            return CapturaHorometroDAO.GuardarHorasComponente(array);
        }

        public List<tblM_CapHorometro> getHorometrosEconomicos(List<string> Economicos, DateTime min, DateTime max)
        {
            return CapturaHorometroDAO.getHorometrosEconomicos(Economicos, min, max);
        }
        public List<string> getUpdatedStandBy(List<string> lst,int tipo)
        {
            return CapturaHorometroDAO.getUpdatedStandBy(lst,tipo);
        }

        public bool getCorteKubrixAC(string ac)
        {
            return CapturaHorometroDAO.getCorteKubrixAC(ac);
        }
        public bool GuardarCorteKubrixAC(string ac)
        {
            return CapturaHorometroDAO.GuardarCorteKubrixAC(ac);
        }
        public byte[] GetReporteHorometrosKubrix(string ac)
        {
            return CapturaHorometroDAO.GetReporteHorometrosKubrix(ac);
        }
        public List<string> GetCorreoGerenteAdmin(string CC) {
            return CapturaHorometroDAO.GetCorreoGerenteAdmin(CC);
        }

    }
}
