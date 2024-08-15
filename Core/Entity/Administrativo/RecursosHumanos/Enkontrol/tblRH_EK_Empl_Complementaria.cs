using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Empl_Complementaria
    {
        public int id { get; set; }
        public int clave_empleado { get; set; }
        public string calzado { get; set; }
        public string camisa { get; set; }
        public string pantalon { get; set; }
        public string overol { get; set; }
        public DateTime? fecha_entrega { get; set; }
        public string entrego_calzado { get; set; }
        public string entrego_camisa { get; set; }
        public string entrego_pantalon { get; set; }
        public string entregro_overol { get; set; }
        public string comentarios { get; set; }
        public string uniforme_dama { get; set; }
        public string entrego_uniforme_dama { get; set; }
        public string otros { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int? idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
    }   
}
