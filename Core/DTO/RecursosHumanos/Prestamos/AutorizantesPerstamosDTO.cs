using Core.Enum.RecursosHumanos.Prestamos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Prestamos
{
    public class AutorizantesPerstamosDTO
    {
        public int idUsuario { get; set; }
        public string  nombreCompleto { get; set; }
        public string descPuesto { get; set; }
        public EstatusAutorizacionPrestamosEnum descEstatus { get; set; }
        public string cveEmpleado { get; set; }
    }
}
