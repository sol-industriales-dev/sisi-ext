using Core.DAO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Captura;
using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Capturas
{
    public class RN_MaquinariaServices : IRN_MaquinariaDAO
    {
        private IRN_MaquinariaDAO m_IRN_Maquinaria;

        public IRN_MaquinariaDAO MaquinariaRentada
        {
            get { return m_IRN_Maquinaria; }
            set { m_IRN_Maquinaria = value; }
        }

        public RN_MaquinariaServices(IRN_MaquinariaDAO maquinariaRentada)
        {
            this.MaquinariaRentada = maquinariaRentada;
        }

        // Nueva renta
        public Dictionary<string, object> GetInfoReporteTiempoRequeridoVsUtilizado(List<int> idsAreasCuenta, List<int> idsCentrosCosto, DateTime periodoInicio, DateTime periodoFinal)
        {
            return MaquinariaRentada.GetInfoReporteTiempoRequeridoVsUtilizado(idsAreasCuenta, idsCentrosCosto, periodoInicio, periodoFinal);
        }

        public Dictionary<string, object> GetAreasCuentaPorUsuario()
        {
            return MaquinariaRentada.GetAreasCuentaPorUsuario();
        }

        public Dictionary<string, object> GetCentrosCostoRentados(List<int> idAreasCuenta)
        {
            return MaquinariaRentada.GetCentrosCostoRentados(idAreasCuenta);
        }

        public Dictionary<string, object> GetCentrosCosto()
        {
            return MaquinariaRentada.GetCentrosCosto();
        }

        public Dictionary<string, object> GetInformacionCentroCosto(int idCentroCosto)
        {
            return MaquinariaRentada.GetInformacionCentroCosto(idCentroCosto);
        }

        public Dictionary<string, object> RegistrarRenta(tblM_RN_Maquinaria informacionRenta, string tipoRenta)
        {
            return MaquinariaRentada.RegistrarRenta(informacionRenta, tipoRenta);
        }

        public Dictionary<string, object> GetMaquinasRentadas(List<int> idAreaCuenta, List<int> idCentroCosto, DateTime periodoDel, DateTime periodoA)
        {
            return MaquinariaRentada.GetMaquinasRentadas(idAreaCuenta, idCentroCosto, periodoDel, periodoA);
        }

        public Dictionary<string, object> GetInfoMaquinaRentada(int idMaquinaRentada)
        {
            return MaquinariaRentada.GetInfoMaquinaRentada(idMaquinaRentada);
        }

        public Dictionary<string, object> TerminarRentaMaquina(int idRentaMaquina)
        {
            return MaquinariaRentada.TerminarRentaMaquina(idRentaMaquina);
        }

        public Dictionary<string, object> GetHorometroPorPeriodoYCentroCosto(DateTime periodoInicio, DateTime periodoFinal, string Cc, int horometroInicial = 0, int horometroFinal = 0)
        {
            return MaquinariaRentada.GetHorometroPorPeriodoYCentroCosto(periodoInicio, periodoFinal, Cc, horometroInicial, horometroFinal);
        }
        // Nueva renta fin
    }
}