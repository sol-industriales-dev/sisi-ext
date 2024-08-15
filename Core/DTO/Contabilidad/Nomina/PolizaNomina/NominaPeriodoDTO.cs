using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Nomina.PolizaNomina
{
    public class NominaPeriodoDTO
    {
        public int id { get; set; }
        public int year { get; set; }
        public int periodo { get; set; }
        public string tipoNomina { get; set; }
        public string tipoRaya { get; set; }
        public string cc { get; set; }
        public string descripcionCC { get; set; }
        public int cantidadEmpleados { get; set; }
        public decimal netoPagar { get; set; }
        public decimal netoPagar2 { get; set; }
        public DateTime fechaCaptura { get; set; }
        public string validada { get; set; }
        public bool validadaEstatus { get; set; }
        public bool tienePoliza { get; set; }
        public string poliza { get; set; }
        public string estatusPoliza { get; set; }
        public DateTime? fechaValidacion { get; set; }
    }
}
