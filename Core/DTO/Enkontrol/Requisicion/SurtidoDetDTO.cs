using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Requisicion
{
    public class SurtidoDetDTO
    {
        public int almacenID { get; set; }
        public string almacenDesc { get; set; }
        public decimal aSurtir { get; set; }

        public string cc { get; set; }
        public string ccDesc { get; set; }
        public int numero { get; set; }
        public DateTime fecha { get; set; }
        public int insumo { get; set; }
        public string insumoDesc { get; set; }
        public int almacenOrigenID { get; set; }
        public string almacenOrigenDesc { get; set; }
        public int almacenDestinoID { get; set; }
        public string almacenDestinoDesc { get; set; }
        public decimal cantidad { get; set; }
        public decimal cantidadAutorizar { get; set; }
        public string estadoSurtido { get; set; }
        public bool estatus { get; set; }

        public int ordenTraspaso { get; set; }
        public string comentarios { get; set; }
        public string ccDestino { get; set; }
        public string ccDestinoDesc { get; set; }
        public int numeroDestino { get; set; }
        public decimal precio { get; set; }
        public decimal importe { get; set; }

        public decimal solicitado { get; set; }
        public decimal salidaConsumo { get; set; }
        public int numeroReq { get; set; }

        public bool traspasoSinOrigen { get; set; }
        public int traspasoID { get; set; }
        public string transporte { get; set; }

        public bool checkBoxRechazado { get; set; }

        public string area_alm { get; set; }
        public string lado_alm { get; set; }
        public string estante_alm { get; set; }
        public string nivel_alm { get; set; }

        public int area { get; set; }
        public int cuenta { get; set; }

        public int? folio_traspaso { get; set; }
        public int partida { get; set; }

        public List<UbicacionDetalleDTO> listUbicacionMovimiento { get; set; }
    }
}
