using Core.DTO.Maquinaria.Captura;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Principal.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Captura
{
    public interface IRN_MaquinariaDAO
    {
        // Nueva renta
        Dictionary<string, object> GetInfoReporteTiempoRequeridoVsUtilizado(List<int> idsAreasCuenta, List<int> idsCentrosCosto, DateTime periodoInicio, DateTime periodoFinal);
        Dictionary<string, object> GetMaquinasRentadas(List<int> idAreaCuenta, List<int> idCentroCosto, DateTime periodoDel, DateTime periodoA);
        Dictionary<string, object> GetInfoMaquinaRentada(int idMaquinaRentada);
        Dictionary<string, object> GetAreasCuentaPorUsuario();
        Dictionary<string, object> GetCentrosCostoRentados(List<int> idAreasCuenta);
        Dictionary<string, object> TerminarRentaMaquina(int idRentaMaquina);
        Dictionary<string, object> GetCentrosCosto();
        Dictionary<string, object> GetInformacionCentroCosto(int idCentroCosto);
        Dictionary<string, object> RegistrarRenta(tblM_RN_Maquinaria informacionRenta, string tipoRenta);
        Dictionary<string, object> GetHorometroPorPeriodoYCentroCosto(DateTime periodoInicio, DateTime periodoFinal, string Cc, int horometroInicial = 0, int horometroFinal = 0);
        // Nueva renta fin
    }
}