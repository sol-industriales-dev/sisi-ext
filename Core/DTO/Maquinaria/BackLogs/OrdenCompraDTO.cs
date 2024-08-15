using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.BackLogs
{
    public class OrdenCompraDTO
    {
        public int id { get; set; }
        public string cc { get; set; }
        public string numRequisicion { get; set; }
        public string numOC { get; set; }
        public int idBackLog { get; set; }
        public string estatus { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCreacionNumOC { get; set; }
        public DateTime fechaModificacionNumOC { get; set; }
        public string numero { get; set; } //SE ALMACENA EL VALOR DEL NUMERO DE COMPRA DE ENKONTROL.
        public string num_requisicion { get; set; } //SE ALMACENA EL VALOR DEL NUMERO DE REQUISICION DE ENKONTROL.
        public int partida { get; set; }
        public int insumo { get; set; }
        public DateTime fecha_entrega { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio { get; set; }
        public decimal importe { get; set; }
        public int moneda { get; set; }
        public decimal tipo_cambio { get; set; }
        public decimal porcent_iva { get; set; }
        public decimal sub_total { get; set; }
        public decimal iva { get; set; }
        public decimal total { get; set; }
        public string comentarios { get; set; }
    }
}
