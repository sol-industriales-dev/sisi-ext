using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Alamcen
{
    public class RemanenteDTO
    {
        public decimal cantidadConsumo { get; set; }
        public decimal cantidadTraspaso { get; set; }
        public decimal cantidadSalidas { get; set; }
        public decimal eficiencia { get; set; }
        public decimal inventarioActual { get; set; }
        public int almacen { get; set; }
        public string almacenDesc { get; set; }
        public int numeroMovimiento { get; set; }
        public string cc { get; set; }
        public string ccDesc { get; set; }
        public DateTime fechaCompra { get; set; }
        public string fechaCompraString { get; set; }
        public int insumo { get; set; }
        public string insumoDesc { get; set; }
        public decimal cantidadCompra { get; set; }
        public decimal precio { get; set; }
        public decimal importe { get; set; }
        public int orden_ct { get; set; }
        public decimal in_out { get; set; }
        public decimal diferenciaImporte { get; set; }
        public string solicitanteNombre { get; set; }
        public string autorizaRequisicionNombre { get; set; }
        public string voboCompraNombre { get; set; }
        public string autorizaCompraNombre { get; set; }
    }
}
