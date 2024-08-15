using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblAlm_Remanente
    {
        public int id { get; set; }
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
        public decimal cantidadEntrada { get; set; }
        public decimal precio { get; set; }
        public decimal importe { get; set; }
        public int orden_ct { get; set; }
        public decimal in_out { get; set; }
        public decimal diferenciaImporte { get; set; }
        public int solicitante { get; set; }
        public string solicitanteNombre { get; set; }
        public int autorizaRequisicion { get; set; }
        public string autorizaRequisicionNombre { get; set; }
        public int voboCompra { get; set; }
        public string voboCompraNombre { get; set; }
        public int autorizaCompra { get; set; }
        public string autorizaCompraNombre { get; set; }
        public bool determinante { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
