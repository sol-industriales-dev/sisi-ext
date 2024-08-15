using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class rptReporteFallaDTO
    {
        public int idReporte { get; set; }
        public int fallaComponente { get; set; }
        public string obra { get; set; }
        public string obraMaq { get; set; }
        public DateTime fecha { get; set; }
        public string frente { get; set; }
        public DateTime fechaParo { get; set; }
        public string noEconomico { get; set; }
        public string descripcionMaq { get; set; }
        public string marcaMaq { get; set; }
        public string modeloMaq { get; set; }
        public string noSerieMaq { get; set; }
        public decimal horometroMaq { get; set; }
        public string descripcionFalla { get; set; }
        public string conjunto { get; set; }
        public string subconjunto { get; set; }
        public string causa { get; set; }
        public DateTime fechaComp { get; set; }
        public decimal horometroComp { get; set; }
        public string numParteComp { get; set; }
        public string destinoCargo { get; set; }
        public string realiza { get; set; }
        public string revisa { get; set; }
        public string realizaFirma { get; set; }
        public string revisaFirma { get; set; }
        public string evidencia { get; set; }
        public string diagnosticosAplicados { get; set; }
        public string tipoReparacion { get; set; }
    }
}