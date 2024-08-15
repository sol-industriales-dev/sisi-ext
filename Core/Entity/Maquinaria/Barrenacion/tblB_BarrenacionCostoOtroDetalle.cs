using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Core.Entity.Maquinaria.Barrenacion
{
    public class tblB_BarrenacionCostoOtroDetalle
    {
        public int id { get; set; }
        public string  conceptoOtro { get; set; }
        public decimal precioUnitarioOtro { get; set; }
        public decimal  cantidadOtro { get; set; }
        public decimal? totalOtro { get; set; }
        public int  idBarrenacionCosto { get; set; }
        public bool activa { get; set; }
        public DateTime  fechaCreacion { get; set; }
        public int usuarioCreadorID { get; set; }
        [JsonIgnore]
        public virtual tblB_BarrenacionCosto BarrenacionCosto { get; set; }
    }
}