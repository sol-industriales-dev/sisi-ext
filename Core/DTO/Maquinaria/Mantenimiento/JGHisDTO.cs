using Core.Entity.Maquinaria.Mantenimiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Mantenimiento
{
    public class JGHisDTO
    {

        public int idhis { get; set; }
        public decimal vidaA { get; set; }

        public List<string> componente { get; set; }
        public List<lstAceiteDTO> AceiteVin { get; set; }
        public bool prueba { get; set; }

        public decimal hrsAplico { get; set; }
        public bool aplico { get; set; }
        public int idComp { get; set; }
    }
}
