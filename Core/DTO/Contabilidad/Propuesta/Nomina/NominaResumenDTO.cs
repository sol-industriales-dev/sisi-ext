using Core.Enum.Administracion.Propuesta.Nomina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Propuesta.Nomina
{
    public class NominaResumenDTO
    {
        public int id { get; set; }
        public tipoNominaPropuestaEnum tipoNomina { get; set; }
        public tipoCuentaNominaEnum tipoCuenta { get; set; }
        public string cc { get; set; }
        public string descripcion { get; set; }
        public decimal nomina { get; set; }
        public decimal iva { get; set; }
        public decimal retencion { get; set; }
        public decimal total { get; set; }
        public decimal noEmpleado { get; set; }
        public int noPracticante { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCaptura { get; set; }
        public string clase { get; set; }
        public DateTime fecha_inicial { get; set; }
        public DateTime fecha_final { get; set; }
        public string descripcionCuenta { get; set; }
        public string descripcionNomina { get; set; }
        public string division { get; set; }
        public NominaResumenDTO()
        {

        }
        public NominaResumenDTO(List<NominaResumenDTO> lstResumen, DateTime fechaInicio, DateTime fechaFin)
        {
            var lstTotal = from resumen in lstResumen
                           where resumen.clase.Contains("totalCuenta")
                           select resumen;
            cc = "ZZZ";
            descripcion = String.Format("TOTAL GENERAL DEL {0:00}-{1:00} DE {2} {3}.", fechaInicio.Day, fechaFin.Day, fechaFin.ToString("MMMMM").ToUpper(), fechaFin.Year);
            nomina = lstTotal.Sum(s => s.nomina);
            iva = lstTotal.Sum(s => s.iva);
            retencion = lstTotal.Sum(s => s.retencion);
            total = lstTotal.Sum(s => s.total);
            noEmpleado = lstTotal.Sum(s => s.noEmpleado);
            noPracticante = lstTotal.Sum(s => s.noPracticante);
            clase = "totalGeneral";
            division = "total";
        }
    }
}
