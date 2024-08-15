using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Mantenimiento.DTO2._0
{
    public class cboLubricantesDTO
    {
        public int componenteID { get; set; }
        public string descripcion { get; set; }
        public int lubricanteID { get; set; }
        public decimal edadAceite { get; set; }
        public decimal edadSuministro { get; set; }
        public decimal cantidadLitros { get; set; }
        public string nomeclatura { get; set; }
    }
}
