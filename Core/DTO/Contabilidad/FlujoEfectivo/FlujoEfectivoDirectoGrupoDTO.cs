using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.FlujoEfectivo
{
    public class FlujoEfectivoDirectoGrupoDTO
    {
        public int idCpto { get; set; }
        public int anio { get; set; }
        public int noSemana { get; set; }
        public string cc { get; set; }
    }
}
