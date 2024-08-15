using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Requisicion
{
    public class RequisicionSeguimientoDTO
    {
        public string cc { get; set; }
        public int requisitor { get; set; }
        public string requisitorDesc { get; set; }
        public int numeroRequisicion { get; set; }
        public string requisicion { get; set; }
        public DateTime? fechaElaboracion { get; set; }
        public string fechaElaboracionDesc { get; set; }
        public DateTime? fechaEntregaCompras { get; set; }
        public string fechaEntregaComprasDesc { get; set; }
        public string tipoRequisicion { get; set; }
        public int idTipoRequisicion { get; set; }
        public string economico { get; set; }
        public string descripcion { get; set; }
        public int? comprador { get; set; }
        public string compradorDesc { get; set; }
        public int? numeroOrdenCompra { get; set; }
        public DateTime? fechaCompra { get; set; }
        public string fechaCompraDesc { get; set; }
        public string ordenCompra { get; set; }
        public string ordenCompraAutorizada { get; set; }
        public DateTime? fechaAutorizacionCompra { get; set; }
        public string fechaAutorizacionCompraDesc { get; set; }
        public int? proveedor { get; set; }
        public string proveedorDesc { get; set; }
        public int tiempoEntregaDias { get; set; }
        public string tiempoEntregaDiasDesc { get; set; }
        public string tiempoEntregaComentarios { get; set; }
        public DateTime? fechaEntrada { get; set; }
        public string fechaEntradaDesc { get; set; }
        public DateTime? fechaAutorizaRe { get; set; }
        public string sugerido { get; set; }
        public int sugeridoNum { get; set; }

        //Campos no utilizados.
        public string prioridad { get; set; }
        public DateTime? fechaPromesa1 { get; set; }
        public string fechaPromesa1Desc { get; set; }
        public DateTime? fechaPromesa2 { get; set; }
        public string fechaPromesa2Desc { get; set; }
        public string comentarios { get; set; }
        public int? almacenEntrada { get; set; }
        public int? numeroEntrada { get; set; }
        public bool colocada { get; set; }
        public DateTime? colocadaFecha { get; set; }
        public string correoProveedor { get; set; }
 
        public bool entregaVencida { get; set; }
        public string colocadaFechaStr { get; set; }
        public bool validadoAlmacen { get; set; }
        public bool validadoCompras { get; set; }
        public int almacen { get; set; }
        public string almacenSurtido { get; set; }
        public decimal? compraCantidadRecibida { get; set; }

        public double nivelReqAutReq { get; set; }
        public double nivelAutReqOC { get; set; }
        public double nivelOCAutOC { get; set; }
        public double nivelAutOCEnt { get; set; }

        public bool consigna { get; set; }
        public bool licitacion { get; set; }
        public bool crc { get; set; }
        public bool convenio { get; set; }

       [NotMapped]
        public string bienes_servicios { get; set; }
       public string proveedorPeru { get; set; }
    }
}
