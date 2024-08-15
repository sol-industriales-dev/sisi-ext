using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core.Entity.ControlObra
{
    public class tblCO_Unidades_Actividad
    {
        public int id { get; set; } 
        //public decimal? costo { get; set; }

        public int actividad_id { get; set; }
        public virtual tblCO_Actividades Actividad { get; set; }

        public int unidad_id { get; set; }
        public virtual tblCO_Unidades unidad { get; set; }

    }
}
