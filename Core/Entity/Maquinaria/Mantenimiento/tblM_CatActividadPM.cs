using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Mantenimiento
{
    public class tblM_CatActividadPM
    {
        public int id { get; set; }
        public string descripcionActividad { get; set; }
        public bool estado { get; set; }
        public int idCatTipoActividad { get; set; }
    }
}
