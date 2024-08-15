using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core.DTO.Enkontrol.Alamcen
{
    public class salidasAlmacenDTO
    {
        //public int almacen_id { get; set; }
        public string centroCosto { get; set; }
        public string folioSalida { get; set; }
        public string almacen { get; set; }
        public DateTime fechaSalida { get; set; }
        public DateTime fechaEntrada { get; set; }
        public int partida { get; set; }
        public string insumo { get; set; }
        public string areaCuenta { get; set; }
        public decimal cantidad { get; set; }
        public decimal costoPromedio { get; set; }
        public decimal importe { get; set; }
        public string comentarios { get; set; }
        public string comentariosGenerales { get; set; }
        public string almacenDestino { get; set; }
        public string centroCostoDestino { get; set; }

        public string referencia { get; set; }
        public string remision { get; set; }
        public string ordenCompra { get; set; }
        public string proveedor { get; set; }
        public string direccion { get; set; }
        public string ciudad { get; set; }
        public string telefonos { get; set; }
        public decimal precio { get; set; }
        public string area_alm { get; set; }
        public string lado_alm { get; set; }
        public string estante_alm { get; set; }
        public string nivel_alm { get; set; }
        public string ordenTraspaso { get; set; }
        public string almacenOrigen { get; set; }
        public string centroCostoOrigen { get; set; }
        public string surtio { get; set; }
        public string recibio { get; set; }
        public string numero { get; set; }
        public string PERU_insumo { get; set; }
        public string noEconomico { get; set; }
    }
}
