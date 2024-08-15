using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.SeguimientoAcuerdos
{
    public class tblSA_MinutaDTO
    {
        public int id { get; set; }
        public string proyecto { get; set; }
        public string titulo { get; set; }
        public string lugar { get; set; }
        public string fecha { get; set; }
        public string horaInicio { get; set; }
        public string horaFin { get; set; }
        public string descripcion { get; set; }
        public int creadorID { get; set; }
        public virtual List<tblSA_ActividadesDTO> actividades { get; set; }
        public virtual List<tblSA_ParticipanteDTO> participantes { get; set; }
        public string fechaInicio { get; set; }
        public string fechaCompromiso { get; set; }
        public bool ver { get; set; }
    }
}
