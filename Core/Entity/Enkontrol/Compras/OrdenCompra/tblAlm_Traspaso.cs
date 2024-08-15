using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblAlm_Traspaso
    {
        public int id { get; set; }
        public string ccOrigen { get; set; }
        public int almacenOrigen { get; set; }
        public string ccDestino { get; set; }
        public int almacenDestino { get; set; }
        public int insumo { get; set; }
        public decimal cantidadTraspasar { get; set; }
        public decimal cantidadCancelada { get; set; }
        public DateTime fecha { get; set; }
        public bool autorizado { get; set; }
        public bool rechazado { get; set; }
        public int folioInterno { get; set; }
        public string comentarios { get; set; }
        public string comentariosGestion { get; set; }
        public string estado { get; set; }
        public string area_alm { get; set; }
        public string lado_alm { get; set; }
        public string estante_alm { get; set; }
        public string nivel_alm { get; set; }
        public bool estatusRegistro { get; set; }
    }
}
