using Core.Entity.Principal.Configuraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.FlujoEfectivo
{
    public class tblC_FE_SaldoInicial : RegistroDTO
    {
        public int id { get; set; }
        public int anio { get; set; }
        public string cc { get; set; }
        public decimal saldo { get; set; }
    }
}
