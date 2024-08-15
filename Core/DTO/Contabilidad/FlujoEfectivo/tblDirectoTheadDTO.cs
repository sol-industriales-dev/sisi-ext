using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.FlujoEfectivo
{
    public class tblDirectoTheadDTO
    {
        public DateTime fecha { get; set; }
        public DateTime fechaCorteMax { get; set; }
        public int noSemana { get; set; }
        public int noSemanaConsulta { get; set; }
        public int noSemanaCorte { get; set; }
        public int noSemanaSiguiente { get; set; }
        public string ac { get; set; }
        public string cc { get; set; }
        public tblDirectoTheadDTO()
        {

        }
    }
}
