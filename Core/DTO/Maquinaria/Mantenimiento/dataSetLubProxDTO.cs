using Core.DTO.Maquinaria.Mantenimiento.DTO2._0;
using Core.Entity.Maquinaria.Mantenimiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Mantenimiento
{
    public class dataSetLubProxDTO
    {
        public componenteMantenimiento componenteMantenimiento { get; set; }
        public List<lstAceiteDTO> aceiteDTO { get; set; }
        public string icono { get; set; }
        public string descripcion { get; set; }
    }
}
