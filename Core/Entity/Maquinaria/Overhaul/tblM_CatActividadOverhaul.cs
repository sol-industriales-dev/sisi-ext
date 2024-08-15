using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Overhaul
{
    public class tblM_CatActividadOverhaul
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public decimal horasDuracion { get; set; }
        public int modeloID { get; set; }
        public int dia { get; set; }
        public bool estatus { get; set; }
        public bool reporteEjecutivo { get; set; }
    }
}