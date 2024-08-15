using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.SeguimientoAcuerdos
{
    public class BitacoraSeguimientoAcuerdosDTO
    {
        public string Departamento { get; set; }
        public string Usuario { get; set; }
        public string Proyecto { get; set; }
        public string Minuta { get; set; }
        public DateTime vFecha { get; set; }
        public string fecha { get; set; }
        public string horaInicio { get; set; }
        public string horaFin { get; set; }
        public string lugar { get; set; }

        public string btnListaAsistencia { get; set; }
        public string btnReporteMinuta { get; set; }

    }
}
