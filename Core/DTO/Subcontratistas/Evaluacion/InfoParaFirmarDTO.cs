using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Subcontratistas.Evaluacion
{
    public class InfoParaFirmarDTO
    {
        public int firmanteId { get; set; }
        public string puestoDelFirmante { get; set; }
        public string nombreCompletoFirmante { get; set; }
        public int evaluacionId { get; set; }
    }
}
