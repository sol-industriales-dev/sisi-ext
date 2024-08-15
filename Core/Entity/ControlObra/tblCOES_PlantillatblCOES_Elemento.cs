using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra
{
    public class tblCOES_PlantillatblCOES_Elemento
    {
        public int id { get; set; }
        public int plantilla_id { get; set; }
        public int elemento_id { get; set; }
        public bool critico { get; set; }
        public decimal ponderacion { get; set; }
        public int usuarioCreacion_id { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificacion_id { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
