using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Subcontratistas.Evaluacion
{
    public class InformacionFirmaDigitalDTO
    {
        public int evaluacionId { get; set; }
        public int firmanteId { get; set; }
        public string firmaDigitalBase64 { get; set; }
    }
}
