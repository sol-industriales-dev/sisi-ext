using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.DocumentosXPagar
{
    public class FiltroContratosDTO
    {
        public string Folio { get; set; }
        public string Descripcion { get; set; }
        public DateTime? Fecha { get; set; }
        public int empresa { get; set; }
        public int financiera { get; set; }
        public bool arrendamiento { get; set; }
    }
}