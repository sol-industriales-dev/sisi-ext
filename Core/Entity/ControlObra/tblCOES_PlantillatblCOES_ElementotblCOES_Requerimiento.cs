using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.ControlObra;

namespace Core.Entity.ControlObra
{
    public class tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento
    {
        public int id { get; set; }
        public int relacionPlantillaElemento_id { get; set; }
        public int requerimiento_id { get; set; }
        public TipoRequerimientoEnum tipo { get; set; }
        public int usuarioCreacion_id { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificacion_id { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
