using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.MAZDA
{
    public class tblMAZ_Reporte_Actividades
    {
        public int id { get; set; }
        public int revisionTipo { get; set; }
        public int revisionDetalleID { get; set; }
        public string ultMant { get; set; }
        public string sigMant { get; set; }
        public string descripcionActividad { get; set; }
        public int semaforo { get; set; }
        public string reprogramacion { get; set; }
        public DateTime? fechaReprogramacion { get; set; }
        public string estatusInfo { get; set; }
        public bool estatus { get; set; }
    }
}
