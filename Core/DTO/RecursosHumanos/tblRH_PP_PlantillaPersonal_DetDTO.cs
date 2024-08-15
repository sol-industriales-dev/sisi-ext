using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_PP_PlantillaPersonal_DetDTO
    {
        public string id { get; set; }
        public string plantillaID { get; set; }
        public string puestoNumero { get; set; }
        public string puesto { get; set; }
        public string departamentoNumero { get; set; }
        public string departamento { get; set; }
        public string tipoNomina { get; set; }
        public string personalNecesario { get; set; }
        public string sueldoBase { get; set; }
        public string sueldoComplemento { get; set; }
        public string sueldoTotal { get; set; }
    }
}
