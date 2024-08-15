using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Almacen
{
    public class RequisicionCompraConciliacionDTO
    {
        public string cc { get; set; }
        public int numeroRequisicion { get; set; }
        public int numeroCompra { get; set; }
        public int almacen { get; set; }
        public int remision { get; set; }
    }
}
