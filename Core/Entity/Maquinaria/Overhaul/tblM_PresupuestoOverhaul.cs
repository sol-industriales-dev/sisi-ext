using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Overhaul
{
    public class tblM_PresupuestoOverhaul
    {
        public int id { get; set; }
        public int modelo { get; set; }
        public int anio { get; set; }
        public int calendarioID { get; set; }
        public int estado { get; set; }
        public string JsonObras { get; set; }
        public DateTime fecha { get; set; }
        public bool cerrado { get; set; }
    }
}
