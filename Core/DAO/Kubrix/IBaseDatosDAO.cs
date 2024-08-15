using Core.DTO.Kubrix;
using Core.Entity.Kubrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Kubrix
{
    public interface IBaseDatosDAO
    {
        List<VencimientoDTO> lstVencimiento();
        List<SalContCCDTO> lstSalContCC(int anio);
        List<object> getInfoMaquinaria(DateTime fechaInicio, DateTime fechaFin);
        List<object> getInfoCapturaMaquinaria(DateTime fechaInicio, DateTime fechaFin, string cc);
        void CapturarMaq(List<CapturaMaqDTO> arr);
        List<tblK_CatAvance> lstArchivos();
    }
}
