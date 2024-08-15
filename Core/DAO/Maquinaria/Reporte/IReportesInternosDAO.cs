using Core.DTO.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Reporte
{
    public interface IReportesInternosDAO
    {
        

        ICollection<pruebaDto> getPrueba();
    }
}
