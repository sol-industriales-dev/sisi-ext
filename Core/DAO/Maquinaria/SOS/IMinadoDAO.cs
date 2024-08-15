using Core.DTO.Maquinaria.SOS;
using Core.Entity.Maquinaria.SOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.SOS
{
    public interface IMinadoDAO
    {
        void Guardar(List<MinadoEntity> obj);
        List<CCMuestrasDTO> cboFiltroLugar();
        List<CCMuestrasDTO> cboFiltroModelo(string lugar);
        List<CCMuestrasDTO> cboComponente();
        List<MaquinaDTO> cboFiltroMaquinaria(string lugar);
        List<MuestrasGeneralesDTO> muestrasGenerales(string lugar, DateTime fechaini, DateTime fechafin);
        List<MuestrasElementosDTO> detalleGeneralMuestras(string lugar, DateTime fechaini, DateTime fechafin, string indicador);
        List<indicaroresDTO> detallesMuestras(List<MuestrasElementosDTO> resultado, string elemento);
        List<MuestrasElementosDTO> detalleCompleto(string lugar, string componente, string unitid, string modelo, string elemento, DateTime fechaini, DateTime fechafin);
        //detalleCompleto
        List<MuestrasGeneralesDTO> muestrasGeneralesLists(List<string> lugar, DateTime fechaini, DateTime fechafin);
        List<MuestrasElementosDTO> detalleGeneralMuestrasList(List<string> lugar, DateTime fechaini, DateTime fechafin, string indicador);
        List<MaquinaDTO> cboFiltroMaquinariaXlista(List<string> lugar);
        List<MuestrasElementosDTO> detalleCompletoLista(List<string> lugar, string componente, string unitid, string modelo, string elemento, DateTime fechaini, DateTime fechafin);
    }
}
