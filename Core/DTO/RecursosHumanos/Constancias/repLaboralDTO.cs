using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Constancias
{
    public class repLaboralDTO
    {
        public string tituloLab { get; set; }
        public string nombreCompletoLab { get; set; }
        public string numeroPatronal { get; set; }       
        public string nombreRegPatronal { get; set; }
        public string nombrePuestoLab { get; set; }
        public string proyectoCCLab { get; set; }
        public string fechaAltaLab { get; set; }
        public string fechaBajaLab { get; set; }
        public string numeroSeguroLab { get; set; }
        public string tipoNominaLab { get; set; }        
        public string curpLab { get; set; }
        public string rfcLab { get; set; }
        public string baseNeto { get; set; }
        public string valorLetraBase { get; set; }
        public string mensualNetoLab { get; set; }
        public string valorLetraLab { get; set; }
        public string sueldoBaseLab { get; set; }
        public string complementoLab { get; set; }
        public string mostrarSueldoLab { get; set; }
        public string contratable { get; set; }
        public int? idEmpleado { get; set; }
        public int? idJefe { get; set; }
        public string cc { get; set; }
        public string status { get; set; }
        
       
        //public string direccionPatronal { get; set; }
    }
}
