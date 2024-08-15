using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Barrenacion
{
    public class tblB_BarrenacionCosto
    {
        public int id { get; set; }
        public decimal  manoObra { get; set; }
        public decimal  costoRenta { get; set; }
        public decimal  diesel { get; set; }
        public decimal  totalCosto { get; set; }
        public bool  activa { get; set; }
        public DateTime  fechaCreacion { get; set; }
        public int usuarioCreadorID { get; set; }    
    }
}