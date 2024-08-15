using Core.DTO.Contabilidad;
using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Principal.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Captura
{
    public interface IMaquinariaRentada
    {
        tblM_MaquinariaRentada SaveMaquinaRentada(tblM_MaquinariaRentada objMaquina);
        tblM_MaquinariaRentada UpdateMaquinaRentada(tblM_MaquinariaRentada objMaquina);
        List<tblM_MaquinariaRentada> getMaquinariaRentada(List<string> ccs);
        List<tblM_MaquinariaRentada> getMaquinariaRentada(tblM_MaquinariaRentada ccs);
        List<tblM_MaquinariaRentada> getMaquinariaRentada(List<string> ccs, string NoEconomico, DateTime PeriodoInicio, DateTime PeriodoFin);
        Dictionary<string, object> getRptProvisionalInfo(int cc, DateTime fechaCorte, decimal TC, bool TodoReporte);
        List<tblM_MaquinariaRentada> getMaquinariaRentadaPorId(int id);
        List<tblM_MaquinariaRentada> getCboMaquinariaFiltro(string obj);
        List<ProveedorDTO> FillCboProveedor();
        List<tblM_MaquinariaRentada> getMaquinariaRentadaPorFacturacion(List<string> ccs, DateTime PeriodoInicio, DateTime PeriodoFin);
        bool getProveedorMoneda(int idProveedor);
        void guardarExcel();
    }
}
