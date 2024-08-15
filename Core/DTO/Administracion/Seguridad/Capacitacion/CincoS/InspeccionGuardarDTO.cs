using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion.CincoS
{
    public class InspeccionGuardarDTO
    {
        public string inspeccion { get; set; }
        public int subAreaId { get; set; }
        public List<int> cincoS { get; set; }
    }
}
