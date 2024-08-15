using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class RecorridoDTO
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int area { get; set; }
        public int empresa { get; set; }
        public DateTime fecha { get; set; }
        public string fechaString { get; set; }
        public int realizador { get; set; }
        public string realizadorDesc { get; set; }
        public int division { get; set; }
        public bool estatus { get; set; }
        public List<RecorridoHallazgoDTO> listaHallazgos { get; set; }
        public List<tblS_CapacitacionSeguridadDNRecorridoAreas> listaAreas { get; set; }
    }
}
