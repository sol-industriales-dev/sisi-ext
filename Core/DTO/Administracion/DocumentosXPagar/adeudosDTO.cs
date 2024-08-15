using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.DocumentosXPagar
{
    public class adeudosDTO
    {
        public string proveedor { get; set; }
        public string contrato { get; set; }
        public string noEconomico { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public decimal tasaInteres { get; set; }
        public decimal valorFinanciado { get; set; }
        public string moneda { get; set; }
        public decimal pagoMensual { get; set; }
        public int plazo { get; set; }
        public string fechaPago { get; set; }
        public decimal pagoRealizados { get; set; }
        public decimal importePagado { get; set; }
        public decimal pagosPendientes { get; set; }
        public decimal saldoPendiente { get; set; }
        public decimal intereses { get; set; }
        public decimal ivaSCapital { get; set; }
        public decimal ivaIntereses { get; set; }
        public decimal saldoCP { get; set; }
        public decimal saldoLP { get; set; }
        public string cargoObra { get; set; }
        public string tipoFinanciamiento { get; set; }
        public decimal saldoPendienteConversionDllsMxn { get; set; }
        public decimal saldoPendienteConversionMxn { get; set; }
        public decimal saldoInsoluto { get; set; }
        public decimal tipoCambioMX { get; set; }

        #region DIVISIONES
        public int divisionID { get; set; }
        public string division { get; set; }
        public string cc { get; set; }
        public string isAdmin { get; set; }
        public List<string> lstCC { get; set; }
        public List<int> lstDivisionID { get; set; }
        public decimal tipoCambio { get; set; }
        #endregion
    }
}
