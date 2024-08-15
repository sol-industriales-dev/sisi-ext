using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Kubrix
{
    public class tblK_CapturaMaq
    {
        public int id { get; set; }
        public string ccObra { get; set; }
        public DateTime fecha { get; set; }
        public string economico { get; set; }
        public int turno { get; set; }
        public decimal horoInicial { get; set; }
        public decimal horoFinal { get; set; }
        public int paroClima { get; set; }
        public decimal hrsMtto { get; set; }
        public decimal horasTrab { get; set; }
        public decimal horasProg { get; set; }
        public decimal horasEfectivas { get; set; }
        public decimal eficiencia { get; set; }
        public decimal consumo { get; set; }
        public string grupoEquipo { get; set; }
        public decimal rendTeorico { get; set; }
        public decimal rendReal { get; set; }
        public decimal rendimiento { get; set; }
    }
}
