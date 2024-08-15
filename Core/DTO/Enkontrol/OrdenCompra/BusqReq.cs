using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class BusqReq
    {
        public List<string> lstCc { get; set; }
        public DateTime ini { get; set; }
        public DateTime fin { get; set; }
        public int min { get; set; }
        public int max { get; set; }
        public int lab { get; set; }
        public int numComp { get; set; }
        public string nomComp { get; set; }
        public DateTime fechaOC { get; set; }

        public string cc { get; set; }
        public int numeroRequisicion { get; set; }
    }
}
