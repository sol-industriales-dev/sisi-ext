using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Reportes
{
    public class tendenciasDTO
    {
        public string empresa { get; set; }
        public string cuenta { get; set; }
        public string sescripcion { get; set; }
        public int tipo { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string descripcion { get; set; }
        public decimal ene { get; set; }
        public decimal feb { get; set; }
        public decimal mar { get; set; }
        public decimal abr { get; set; }
        public decimal may { get; set; }
        public decimal jun { get; set; }
        public decimal jul { get; set; }
        public decimal ago { get; set; }
        public decimal sep { get; set; }
        public decimal oct { get; set; }
        public decimal nov { get; set; }
        public decimal dic { get; set; }
        public decimal total { get; set; }
        public decimal porcentaje { get; set; }
        public decimal variabilidad { get; set; }
        public decimal porcentajeVariabilidad { get; set; }
        public bool es90_10 { get; set; }
    }
}
