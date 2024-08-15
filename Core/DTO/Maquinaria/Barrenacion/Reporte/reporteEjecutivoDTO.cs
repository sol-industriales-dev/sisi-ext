using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Barrenacion.Reporte
{
    public class reporteEjecutivoDTO
    {
        public string centroCostos { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public string equipos { get; set; }
        public decimal rentaEquipos { get; set; }
        public decimal combustibles { get; set; }
        public decimal aceroDesgaste { get; set; }
        public decimal operador { get; set; }
        public decimal costosMetroLineal { get; set; }
        public decimal costoTonelada { get; set; }
        public decimal aceroDesgasteHr { get; set; }
        public decimal aceroDesgasteMl { get; set; }
        public decimal velocidadBarrenacion { get; set; }
        public decimal disponibilidad { get; set; }
        public decimal suma { get; set; }

        public decimal barraHR { get; set; }
        public decimal barraSegundaHR { get; set; } 
        public decimal brocaHR { get; set; } 
        public decimal martilloHR { get; set; }

        public decimal barraML { get; set; }
        public decimal barraSegundaML { get; set; }
        public decimal brocaML { get; set; }
        public decimal martilloML { get; set; }
        public decimal tipocambio { get; set; }
        public decimal rentaEquiposDLLS { get; set; }
        public decimal combustibleDLLS { get; set; }
        public decimal aceroDesgasteDLLS { get; set; }
        public decimal operadorDLLS { get; set; }
        public decimal sumaDLLS { get; set; }
        public decimal costoMetroLinealDLLS { get; set; }
        public decimal costoToneladaDLLS { get; set; }
        public decimal utilizacion { get; set; }
        public decimal precioOtros { get; set; }
        public decimal precioOtrosDLLS { get; set; }

        public decimal precioCombustible { get; set; }
        public decimal precioCombustibleDLLS { get; set; }
        public decimal CostoM3 { get; set; }
        public decimal CostoM3DLLS { get; set; }
        public decimal LitrosCombustible { get; set; }
        public decimal costoPromedioOperador { get; set; }
        public decimal horasTotales { get; set; }
        public decimal costoBarra { get; set; }
        public decimal costoBroca { get; set; }
        public decimal costoMartillo { get; set; }
        public decimal costoZanco { get; set; }

    }
}
