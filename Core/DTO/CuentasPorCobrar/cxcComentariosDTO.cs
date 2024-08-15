using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.CuentasPorCobrar
{
    public class cxcComentariosDTO
    {
        public int id { get; set; }
        public string comentario { get; set; }
        public int factura { get; set; }
        public int? clienteID { get; set; }
        public string nomCliente { get; set; }
        public string cc { get; set; }
        public string ccDesc { get; set; }
        public int tipoComentario { get; set; }
        public DateTime? fechaCompromiso { get; set; }
        public string nombreUsuarioCreacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool esActivo { get; set; }

        //EXTRA
        public string descTipoComentario { get; set; }
        public bool esVenceMañana { get; set; }
        public bool esVencePasado { get; set; }

    }
}
