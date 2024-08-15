using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Facturas
{
    public class tblF_EK_Pedidos_Detalle_Partidas
    {
        public int id { get; set; }
        public int cia_sucursal { get; set; }
        public int pedido { get; set; }
        public int partida { get; set; }
        public int fila { get; set; }
        public string columna { get; set; }
        public string texto { get; set; }
        public string tipo_fuente { get; set; }
        public string color { get; set; }
        public decimal tam_letra { get; set; }
        public bool negrita { get; set; }
        public bool subrayado { get; set; }
        public bool cursiva { get; set; }
        public bool centrado { get; set; }
        public bool derecha { get; set; }
        public bool izquierda { get; set; }
        public string rtf { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; } 
    }
}
