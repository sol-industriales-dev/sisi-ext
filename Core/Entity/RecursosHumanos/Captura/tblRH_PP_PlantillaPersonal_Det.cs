using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_PP_PlantillaPersonal_Det
    {
        public int id { get; set; }
        public int plantillaID { get; set; }
        public virtual tblRH_PP_PlantillaPersonal plantilla { get; set; }
        public int puestoNumero { get; set; }
        public string puesto { get; set; }
        public int departamentoNumero { get; set; }
        public string departamento { get; set; }
        public int tipoNomina { get; set; }
        public int personalNecesario { get; set; }
        public decimal sueldoBase { get; set; }
        public decimal sueldoComplemento { get; set; }
        public decimal sueldoTotal { get; set; }
        public int? plantilla_Puesto_EKID { get; set; }
        public int? tabulador_Puesto_EKID { get; set; }
    }
}
