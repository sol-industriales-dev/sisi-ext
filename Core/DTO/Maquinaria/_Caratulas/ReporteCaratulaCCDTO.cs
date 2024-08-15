using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria._Caratulas
{
    public class ReporteCaratulaCCDTO
    {

        public string grupo { get; set; }
        public string modelo { get; set; }
        public string depreciacion { get; set; }
        public string inversion { get; set; }
        public string seguro { get; set; }
        public string filtros { get; set; }
        public string correctivo { get; set; }
        public string auxiliar { get; set; }
        public string manoObra { get; set; }
        public string indirectos { get; set; }
        public string depreciacionOverhaul { get; set; }
        public string aceite { get; set; }
        public string carilleria { get; set; }
        public string ansul { get; set; }
        public string utilidad { get; set; }
        public string costoArrendadora { get; set; }
        public decimal tipoCambio { get; set; }
        public string tipoMoneda { get; set; }
        public string tipoHoraDia { get; set; }
    }
}
