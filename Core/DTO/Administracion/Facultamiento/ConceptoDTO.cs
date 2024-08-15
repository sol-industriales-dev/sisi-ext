using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Facultamiento
{
    public class ConceptoDTO
    {
        public int ID { get; set; }
        public string Concepto { get; set; }
        public bool EsAutorizante { get; set; }
        public int PlantillaID { get; set; }
    }
}
