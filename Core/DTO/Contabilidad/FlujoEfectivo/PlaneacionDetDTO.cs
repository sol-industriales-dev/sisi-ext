using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.FlujoEfectivo
{
    public class PlaneacionDetDTO
    {
        public string descripcion { get; set; }
        public int id { get; set; }
        public decimal monto { get; set; }
        public int numProv { get; set; }
        public int numcte { get; set; }
        public string ac { get; set; }
        public string cc { get; set; }
        public bool detalle { get; set; }
        public int semana { get; set; }
        public List<int> listIDSigoplan { get; set; }
        public List<string> listFacturas { get; set; }
        public List<int> listCadenasProductivas { get; set; }
        public List<int> listaNomina { get; set; }
        public int tipo { get; set; }
        public int conceptoID { get; set; }

    }
}
