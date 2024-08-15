using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.EstadoFinanciero
{
    public class FiltroSaldoDTO
    {
        public EmpresaEnum empresa { get; set; }
        public int year { get; set; }
        public int mes { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string cc { get; set; }
        public List<string> listaCC { get; set; }
        public List<string> listaAC { get; set; }

        public int area { get; set; }
        public int cuenta { get; set; }
        public string areaCuenta { get; set; }
    }
}
