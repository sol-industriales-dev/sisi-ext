using Core.DTO.Maquinaria.Reporte.RepAnalisisUtilizacion;
using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Reporte
{
    public interface IRepAnalisisUtilizacionDAO
    {
        List<AnalisisDTO> getAnalisis(BusqAnalisiDTO busq);
        List<RepAnalisisDTO> getRepAnalisisUtilizacion(BusqAnalisiDTO busq);
        List<ComboDTO> cboAC();
    }
}
