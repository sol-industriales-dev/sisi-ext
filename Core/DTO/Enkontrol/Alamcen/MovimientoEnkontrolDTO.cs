using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Enkontrol.OrdenCompra;

namespace Core.DTO.Enkontrol.Alamcen
{
    public class MovimientoEnkontrolDTO
    {
        public int almacen { get; set; }
        public string almacenDesc { get; set; }
        public int tipo_mov { get; set; }
        public int numero { get; set; }
        public string cc { get; set; }
        public string ccDesc { get; set; }
        public int orden_ct { get; set; }
        public DateTime? fecha { get; set; }
        public string fechaString { get; set; }
        public decimal total { get; set; }
        public string estatus { get; set; }
        public string transferida { get; set; }
        public int empleado { get; set; }
        public string empleadoDesc { get; set; }
        public string comentarios { get; set; }
        public int? sector_id { get; set; }

        public int? recibioID { get; set; }
        public string recibioDesc { get; set; }
        public int proveedor { get; set; }
        public string proveedorDesc { get; set; }
        
        public List<MovimientoDetalleEnkontrolDTO> detalle { get; set; }

        public bool tieneCompra { get; set; }
        public OrdenCompraDTO compra { get; set; }
        public int numeroRequisicion { get; set; }
    }
}
