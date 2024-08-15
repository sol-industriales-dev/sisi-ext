using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra.Gestion
{
    public class soportesEviDTO
    {
        public int id { get; set; }
        public int idOrdenDeCambio { get; set; }
        public double alcancesNuevos { get; set; }
        public double modificacionesPorCambio { get; set; }
        public double requerimientosDeCampo { get; set; }
        public double ajusteDeVolumenes { get; set; }
        public double serviciosYSuministros { get; set; }
        public DateTime fechaInicial { get; set; }
        public DateTime fechaFinal { get; set; }
        public double MontoContratoOriginal { get; set; }
        public string MontoContratoOriginalFormatMX { get; set; }
        public double MontoContratoOriginalSuma { get; set; }
        public string alcancesNuevosDescripcion { get; set; }
        public string modificacionesPorCambioDescripcion { get; set; }
        public string requerimientosDeCampoDescripcion { get; set; }
        public string ajusteDeVolumenesDescripcion { get; set; }
        public string serviciosYSuministrosDescripcion { get; set; }
        public string fechaDescripcion { get; set; }
        public int Dias { get; set; }
        public string numeroConLetras { get; set; }
        public double numeroTotalSinMontoInicial { get; set; }
        public string numeroTotalSinMontoInicialFormatMX { get; set; }
        public double numeroTotal { get; set; }
        public string numeroTotalFormatMX { get; set; }
        public double sumaTotalDeOrdenesDeCambioPrevias { get; set; }
        public string sumaTotalDeOrdenesDeCambioPreviasFormatMX { get; set; }
    }
}
