using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.MAZDA
{
    public class RevisionACDTO
    {
        public int id { get; set; }
        public int equipoID { get; set; }
        public string equipo { get; set; }
        public string tonelaje { get; set; }
        public int areaID { get; set; }
        public string area { get; set; }
        public string periodo { get; set; }
        public int tecnicoID { get; set; }
        public string tecnico { get; set; }
        public int ayudantesID { get; set; }
        public string ayudantes { get; set; }
        public string observaciones { get; set; }
        public DateTime fechaCaptura { get; set; }
        public List<RevisionAC_DetalleDTO> detalle { get; set; }
    }
}
