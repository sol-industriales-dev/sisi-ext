using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Constancias
{
    public class repGuarderiaDTO
    {

        public string descripcionCCGuard { get; set; }
        public string guarderia { get; set; }
        public string director { get; set; }
        public string nombreCompletoGuard { get; set; }
        public string regPatron { get; set; }
        public string nombrePatron { get; set; }
        public string imss { get; set; }
        public string rfc { get; set; }
        public string curp { get; set; }
        public string fechaIngreso { get; set; }
        public string nombrePuestoGuard { get; set; }
        public string nombreHijo { get; set; }        
        public string vacaciones { get; set; }
        public string tipoNomina { get; set; }
        public string horaEntrada { get; set; }
        public string horaSalida { get; set; }
        public string horaComida { get; set; }
        public string horaEntradaS { get; set; }
        public string horaSalidaS { get; set; }
        public string horaEntradaD { get; set; }
        public string horaSalidaD { get; set; }
        public string diasTrab { get; set; }
        public string diasDesc { get; set; }
        public int? idEmpleado { get; set; }
        public int? idJefe { get; set; }
        public string cc { get; set; }

    }
}
