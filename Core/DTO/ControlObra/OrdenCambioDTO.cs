using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra
{
    public class OrdenCambioDTO
    {
        public int id { get; set; }
        public DateTime fechaEfectiva { get; set; }
        public string cc { get; set; }
        public int idContrato { get; set; }
        public string NoOrden { get; set; }
        public string numeroContrato { get; set; }
        public decimal montoContrato { get; set; }
        public decimal montoTotalOrdenCambio { get; set; }
    }
}
