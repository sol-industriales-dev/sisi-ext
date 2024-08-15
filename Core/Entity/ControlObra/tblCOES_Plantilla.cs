using Core.Enum.ControlObra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra
{
    public class tblCOES_Plantilla
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public int colaborador_id { get; set; }
        public DateTime fecha { get; set; }
        public TipoPlantillaEnum tipo { get; set; }
        public bool plantillaBase { get; set; }
        public int usuarioCreacion_id { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificacion_id { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
