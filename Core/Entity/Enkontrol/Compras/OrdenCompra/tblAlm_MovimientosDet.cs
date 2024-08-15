using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblAlm_MovimientosDet
    {
        public int id { get; set; }
        public int almacen { get; set; }
        public int tipo_mov { get; set; }
        public int partida { get; set; }
        public int insumo { get; set; }
        public string comentarios { get; set; }
        public int area { get; set; }
        public int cuenta { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio { get; set; }
        public decimal importe { get; set; }
        public int? partida_oc { get; set; }
        public int id_resguardo { get; set; }
        public string area_alm { get; set; }
        public string lado_alm { get; set; }
        public string estante_alm { get; set; }
        public string nivel_alm { get; set; }
        public string transporte { get; set; }
        public bool estatusHabilitado { get; set; }
        public string PERU_insumo { get; set; }
        public string noEconomico { get; set; }

        public int numero { get; set; }
        public virtual tblAlm_Movimientos movimiento { get; set; }
    }
}
