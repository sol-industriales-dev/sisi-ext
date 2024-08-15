using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.SeguimientoAcuerdos
{
    public class EstadisticoMinutasDTO
    {
        public int departamentoID { get; set; }
        public string departamento { get; set; }
        public int minutasFinalizadas { get; set; }
        public int minutasActividadPendiente { get; set; }
        public int minutasTotal { get; set; }
    }
}
