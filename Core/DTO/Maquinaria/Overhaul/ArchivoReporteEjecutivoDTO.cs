using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class ArchivoReporteEjecutivoDTO
    {
        public int id { get; set; }
        public byte[] imagen { get; set; }

        public string actividadID { get; set; }
    }
}
