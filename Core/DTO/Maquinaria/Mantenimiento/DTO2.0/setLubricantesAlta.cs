using Core.Entity.Maquinaria.Mantenimiento2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Mantenimiento.DTO2._0
{
    public class setLubricantesAlta
    {
        public string Icon { get; set; }
        public int componenteID { get; set; }
        public string componenteDesc { get; set; }
        public decimal vidaRest { get; set; }
        public decimal horServ { get; set; }
        public int actID { get; set; }
        public List<cboLubricantesDTO> lubricantes { get; set; }
        public bool pruebaLub { get; set; }
        public decimal vidaLubricante { get; set; }
        public bool estatus { get; set; }
        public decimal vidaActual { get; set; }

    }
}
