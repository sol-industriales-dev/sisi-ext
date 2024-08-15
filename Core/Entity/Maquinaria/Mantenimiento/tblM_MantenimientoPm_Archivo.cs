using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Mantenimiento
{
    public class tblM_MantenimientoPm_Archivo
    {
        public int id { get; set; }
        public int idMantenimiento { get; set; }
        public int idActividad { get; set; }
        public string nombre { get; set; }
        public string ruta { get; set; }
        public bool estatus { get; set; }
    }
}
