using Core.DAO.Maquinaria.Captura;
using Core.DTO.Contabilidad;
using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Principal.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Capturas
{
    public class MaquinariaRentadaService : IMaquinariaRentada
    {
        

        private IMaquinariaRentada m_IMaquinariaRentada;

        

        public IMaquinariaRentada MaquinariaRentada
        {
            get { return m_IMaquinariaRentada; }
            set { m_IMaquinariaRentada = value; }
        }

        public MaquinariaRentadaService(IMaquinariaRentada maquinariaRentada)
        {
            this.MaquinariaRentada = maquinariaRentada;
        }

        public tblM_MaquinariaRentada SaveMaquinaRentada(tblM_MaquinariaRentada objMaquina)
        {
            return MaquinariaRentada.SaveMaquinaRentada(objMaquina);
        }
        public tblM_MaquinariaRentada UpdateMaquinaRentada(tblM_MaquinariaRentada objMaquina)
        {
            return MaquinariaRentada.UpdateMaquinaRentada(objMaquina);
        }

        public List<tblM_MaquinariaRentada> getMaquinariaRentada(List<string> ccs)
        {
            return MaquinariaRentada.getMaquinariaRentada(ccs);
        }
        public List<tblM_MaquinariaRentada> getMaquinariaRentada(tblM_MaquinariaRentada ccs)
        {
            return MaquinariaRentada.getMaquinariaRentada(ccs);
        }

        public List<tblM_MaquinariaRentada> getMaquinariaRentada(List<string> ccs, string NoEconomico, DateTime PeriodoInicio, DateTime PeriodoFin)
        {
            return MaquinariaRentada.getMaquinariaRentada(ccs, NoEconomico, PeriodoInicio, PeriodoFin);
        }
        public Dictionary<string, object> getRptProvisionalInfo(int cc, DateTime fechaCorte, decimal TC, bool TodoReporte)
        {
            return MaquinariaRentada.getRptProvisionalInfo(cc, fechaCorte, TC, TodoReporte);
        }

        public List<tblM_MaquinariaRentada> getMaquinariaRentadaPorId(int id)
        {
            return MaquinariaRentada.getMaquinariaRentadaPorId(id);
        }

        public List<tblM_MaquinariaRentada> getCboMaquinariaFiltro(string obj)
        {
            return MaquinariaRentada.getCboMaquinariaFiltro(obj);
        }

        public List<ProveedorDTO> FillCboProveedor()
        {
            return MaquinariaRentada.FillCboProveedor();
        }

        public bool getProveedorMoneda(int idProveedor)
        {
            return MaquinariaRentada.getProveedorMoneda(idProveedor);
        }

        public void guardarExcel()
        {
            MaquinariaRentada.guardarExcel();
        }

        public List<tblM_MaquinariaRentada> getMaquinariaRentadaPorFacturacion(List<string> ccs, DateTime PeriodoInicio, DateTime PeriodoFin)
        {
            return MaquinariaRentada.getMaquinariaRentadaPorFacturacion(ccs, PeriodoInicio, PeriodoFin);
        }
    }
}
