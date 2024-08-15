using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_CapNominaCC
    {
        public int id { get; set; }
        public DateTime periodoInicial { get; set; }
        public DateTime periodoFinal { get; set; }
        public decimal nominaSemanal { get; set; }
        public string archivo { get; set; }
        public DateTime fechaCaptura { get; set; }
        public bool estatus { get; set; }
        public bool isVerificado { get; set; }
        public string ac { get; set; }

        public string areaCuentaDescripcion { get; set; }
    }
}
