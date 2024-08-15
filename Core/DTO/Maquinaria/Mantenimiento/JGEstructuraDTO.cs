using Core.Entity.Maquinaria.Mantenimiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Mantenimiento
{
    public class JGEstructuraDTO
    {

        public tblM_ComponenteMantenimiento idComponente { get; set; }
        public List<string> componente { get; set; }
        public List<lstAceiteDTO> AceiteVin { get; set; }
        public List<string> Icon { get; set; }
    }
}
