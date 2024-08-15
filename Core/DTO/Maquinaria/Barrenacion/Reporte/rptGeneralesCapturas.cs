using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Barrenacion.Reporte
{
    public class rptGeneralesCapturas
    {
        public string operador { get; set; }
        public DateTime fecha { get; set; }
        public decimal barrenos { get; set; }
        public decimal rehabilitacion { get; set; }
        public decimal metrosLineales { get; set; }
        public decimal metrosLinealesEfectivos { get; set; }
        public decimal bordo { get; set; }
        public decimal espaciamiento { get; set; }
        public decimal densidadMaterial { get; set; }
        public decimal m3 { get; set; }
        public decimal metrolinealHr { get; set; }
        public decimal toneladaHR { get; set; }
        public decimal m3HR { get; set; }
        public int turno { get; set; }
        

    }
}
