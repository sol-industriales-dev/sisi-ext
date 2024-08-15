using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.DocumentosXPagar.Reporte_Adeudo
{
    public class DetalleAdeudo
    {

        public int idInstitucion { get; set; }
        public string descripcionInstitucion { get; set; }
        public string contrato { get; set; }
        public string noEconomico { get; set; }
        public string fechaInicio { get; set; }
        public string fechaTerminacion { get; set; }
        public string tasaIntereses { get; set; }
        public decimal valorFinanciado { get; set; }
        public string moneda { get; set; }
        public string pagoMensual { get; set; }
        public int plazo { get; set; }
        public string FechasPago { get; set; }
        public int pagosRealizados { get; set; }
        public decimal importePagado { get; set; }
        public int pagosPendientes { get; set; }
        public int saldoPendiente { get; set; }
    }
}
