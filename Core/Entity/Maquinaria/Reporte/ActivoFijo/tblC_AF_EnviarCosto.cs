using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Catalogo;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo
{
    public class tblC_AF_EnviarCosto
    {
        public int id { get; set; }
        public int idEconomico { get; set; }
        public string cc { get; set; }
        public int? area { get; set; }
        public int? cuenta { get; set; }
        public int mesesMaximoDepreciacion { get; set; }
        public decimal porcentajeDepreciacion { get; set; }
        public int mesesDepreciados { get; set; }
        public int mesesFaltantes { get; set; }
        public int semanasUltimoMesDep { get; set; }
        public decimal depActual { get; set; }
        public decimal depFaltante { get; set; }
        public string descripcion { get; set; }
        public string polizaBaja { get; set; }
        public string polizaCosto { get; set; }
        public string polizaAlta { get; set; }
        public decimal monto { get; set; }
        public DateTime fechaAlta { get; set; }
        public DateTime fechaInicioDep { get; set; }
        public bool enviaACosto { get; set; }
        public bool estatus { get; set; }

        [NotMapped]
        public string numEconomico { get; set; }
        [NotMapped]
        public string year { get; set; }
        [NotMapped]
        public string mes { get; set; }
        [NotMapped]
        public string status { get; set; }
        [NotMapped]
        public string poliza { get; set; }
        [NotMapped]
        public DateTime? fechaPolizaCosto { get; set; }

        [ForeignKey("idEconomico")]
        public virtual tblM_CatMaquina economico { get; set; }
    }
}
