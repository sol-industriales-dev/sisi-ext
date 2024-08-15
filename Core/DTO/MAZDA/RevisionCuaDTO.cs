using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.MAZDA
{
    public class RevisionCuaDTO
    {
        public int id { get; set; }
        public int cuadrillaID { get; set; }
        public string cuadrilla { get; set; }
        public int mes { get; set; }
        public string mesDesc { get; set; }
        public int tecnicoID { get; set; }
        public string tecnico { get; set; }
        public int ayudantesID { get; set; }
        public string ayudantes { get; set; }
        public string observaciones { get; set; }
        public DateTime fechaCaptura { get; set; }
        public List<RevisionCua_DetalleDTO> detalle { get; set; }
    }
}
