using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Overhaul;

namespace Core.DTO.Maquinaria.Mantenimiento.Correctivo.ReporteFalla
{
    public class ReporteFallaDTO
    {
        public int id { get; set; }
        public int maquinaID { get; set; }
        public string fechaReporte { get; set; }
        public string fechaParo { get; set; }
        public string cc { get; set; }
        public string descripcionFalla { get; set; }
        public int fallaComponente { get; set; }
        public string causaFalla { get; set; }
        public string diagnosticosAplicados { get; set; }
        public string tipoReparacion { get; set; }
        public string reparaciones { get; set; }
        public string destino { get; set; }
        public int realiza { get; set; }
        public string revisa { get; set; }
        public string procedencia { get; set; }
        public string fechaAlta { get; set; }
        public decimal horometroReporte { get; set; }
        public int estatus { get; set; }
        public virtual List<tblM_ReporteFalla_Archivo> lstArchivos { get; set; }

        public string componenteInsumo { get; set; }
    }
}
