using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_Uniformes
    {
        public int id { get; set; }
        public int clave_empleado { get; set; }
        public DateTime fechaEntrega { get; set; }
        public string calzado { get; set; }
        public string camisa { get; set; }
        public string pantalon { get; set; }
        public string overol { get; set; }
        public string uniforme_dama { get; set; }
        public string otros { get; set; }
        public string comentarios { get; set; }
        public bool entrego_calzado { get; set; }
        public bool entrego_camisa { get; set; }
        public bool entrego_pantalon { get; set; }
        public bool entrego_overol { get; set; }
        public bool entrego_uniforme_dama { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
    }
}