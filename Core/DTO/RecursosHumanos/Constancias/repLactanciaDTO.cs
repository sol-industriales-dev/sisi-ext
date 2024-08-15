using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Constancias
{
    public class repLactanciaDTO
    {
        public int id { get; set; }
        public DateTime? fechaInicioPermiso { get; set; }
        public DateTime? fechaFinPermiso { get; set; }
        public DateTime? fechaInicioInca{ get; set; }
        public DateTime? fechaFinInca { get; set; }
        public string sexo { get; set; }     
        public string nombreCompletoLact { get; set; }
        public string nombrePuestoLact { get; set; }
        public int clave_empleado { get; set; }  
        

        public string entradaLunes { get; set; }
        public string entradaMartes { get; set; }
        public string entradaMiercoles { get; set; }
        public string entradaJueves { get; set; }
        public string entradaViernes { get; set; }
        public string entradaSabado { get; set; }
        public string entradaDomingo { get; set; }

        public string salidaLunes { get; set; }
        public string salidaMartes { get; set; }
        public string salidaMiercoles { get; set; }
        public string salidaJueves { get; set; }
        public string salidaViernes { get; set; }
        public string salidaSabado { get; set; }
        public string salidaDomingo { get; set; }

        public string comidaLunes { get; set; }
        public string comidaMartes { get; set; }
        public string comidaMiercoles { get; set; }
        public string comidaJueves { get; set; }
        public string comidaViernes { get; set; }
        public string comidaSabado { get; set; }
        public string comidaDomingo { get; set; }

        public string entradaLunesL { get; set; }
        public string entradaMartesL { get; set; }
        public string entradaMiercolesL { get; set; }
        public string entradaJuevesL { get; set; }
        public string entradaViernesL { get; set; }
        public string entradaSabadoL { get; set; }
        public string entradaDomingoL { get; set; }

        public string salidaLunesL { get; set; }
        public string salidaMartesL { get; set; }
        public string salidaMiercolesL { get; set; }
        public string salidaJuevesL { get; set; }
        public string salidaViernesL { get; set; }
        public string salidaSabadoL { get; set; }
        public string salidaDomingoL { get; set; }

        public string comidaLunesL { get; set; }
        public string comidaMartesL { get; set; }
        public string comidaMiercolesL { get; set; }
        public string comidaJuevesL { get; set; }
        public string comidaViernesL { get; set; }
        public string comidaSabadoL { get; set; }
        public string comidaDomingoL { get; set; }

        public int? idEmpleado { get; set; }
        public int? idJefe { get; set; }
       
    }
}
