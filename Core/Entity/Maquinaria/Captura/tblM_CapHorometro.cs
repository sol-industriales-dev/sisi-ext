using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria
{
    public class tblM_CapHorometro 
    {
        public int id { get; set; }
        public string CC { get; set; }
        public string Economico { get; set; }
        public decimal HorasTrabajo { get; set; }
        public decimal Horometro { get; set; }
        public decimal HorometroAcumulado { get; set; }
        public decimal Desfase { get; set; }
        //public decimal DesfaseAcumulado { get; set; }
        public DateTime Fecha { get; set; }
        public bool Ritmo { get; set; }
        public int turno { get; set; }
        public DateTime FechaCaptura { get; set ; }
        public string folio { get; set; }
    }
}
