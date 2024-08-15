using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Mantenimiento
{
    public class tblM_CatActividadPM_tblM_CatParte
    {
        public int id { get; set; }
        public int idParte { get; set; }
        public int idActividadPM { get; set; }
        public int modeloEquipoID { get; set; }
    }
}
