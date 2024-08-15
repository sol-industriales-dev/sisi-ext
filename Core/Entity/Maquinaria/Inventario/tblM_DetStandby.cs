using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_DetStandby
    {
        public int id { get; set; }
        public int noEconomicoID { get; set; }
        public decimal HorometroInicial { get; set; }
        public decimal HorometroFinal { get; set; }
        public DateTime DiaParo { get; set; }
        public int TipoConsideracion { get; set; }
        public DateTime FechaCaptura { get; set; }
        public int StandByID { get; set; }
        public bool estatus { get; set; }
        public DateTime FechaFinStandby { get; set; }

        public virtual tblM_CapStandBy StandBy { get; set; }

    }
}
