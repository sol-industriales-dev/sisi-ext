using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra.Gestion
{
    public class SoportesEvidenciaDTO
    {
        public int id { get; set; }
        public int idOrdenDeCambio { get; set; }
        public decimal alcancesNuevos { get; set; }
        public decimal modificacionesPorCambio { get; set; }
        public decimal requerimientosDeCampo { get; set; }
        public decimal ajusteDeVolumenes { get; set; }
        public decimal serviciosYSuministros { get; set; }
        public DateTime fechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public decimal MontoContratoOriginal { get; set; }
        public decimal MontoContratoOriginalSuma { get; set; }
        public string alcancesNuevosDescripcion { get; set; }
        public string modificacionesPorCambioDescripcion { get; set; }
        public string requerimientosDeCampoDescripcion { get; set; }
        public string ajusteDeVolumenesDescripcion { get; set; }
        public string serviciosYSuministrosDescripcion { get; set; }
        public string fechaDescripcion { get; set; }
        public int Dias { get; set; }
        public string numeroConLetras { get; set; }
        public double numeroTotal { get; set; }
        public double numeroTotalSinMontoInicial { get; set; }
        public double sumaTotalDeOrdenesDeCambioPrevias { get; set; }

        public string AlcancesNuevosArchivos { get; set; }
        public string modificacionArchvios { get; set; }
        public string requerimientosArchivos { get; set; }
        public string ajusteDeVolumenesArchivos { get; set; }
        public string serviciosYSuministrosArchivos { get; set; }
    }
}
