using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Overhaul
{
    public class tblM_trackServicioOverhaul
    {
        public int id { get; set; }
        public int servicioID { get; set; }
        public int tipoServicioID { get; set; }
        public DateTime fecha { get; set; }
        public decimal horasCiclo { get; set; }
        public decimal target { get; set; }
        public string archivos { get; set; }

        public virtual tblM_CatServicioOverhaul servicio { get; set; }
    }
}