using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblCom_OrdenCompraDet
    {
        public int id { get; set; }
        public int idOrdenCompra { get; set; }
        public string cc { get; set; }
        public int numero { get; set; }
        public int partida { get; set; }
        public int insumo { get; set; }
        public DateTime? fecha_entrega { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio { get; set; }
        public decimal importe { get; set; }
        public decimal ajuste_cant { get; set; }
        public decimal ajuste_imp { get; set; }
        public int num_requisicion { get; set; }
        public int part_requisicion { get; set; }
        public decimal cant_recibida { get; set; }
        public decimal imp_recibido { get; set; }
        public DateTime? fecha_recibido { get; set; }
        public decimal cant_canc { get; set; }
        public decimal imp_canc { get; set; }
        public decimal acum_ant { get; set; }
        public decimal max_orig { get; set; }
        public decimal max_ppto { get; set; }
        public int area { get; set; }
        public int cuenta { get; set; }
        public decimal porcent_iva { get; set; }
        public decimal iva { get; set; }
        public string partidaDescripcion { get; set; }
        public string noEconomico { get; set; }
        public bool estatusRegistro { get; set; }
        public bool exento_iva { get; set; }
    }
}
