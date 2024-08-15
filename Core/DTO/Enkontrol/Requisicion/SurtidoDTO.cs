using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Requisicion
{
    public class SurtidoDTO
    {
        public int partida { get; set; }
        public int insumo { get; set; }
        public string insumoDesc { get; set; }
        public decimal cantidad { get; set; }
        public decimal cantidadCapturada { get; set; }
        public List<SurtidoDetDTO> nuevaCaptura { get; set; }
        public bool quitar { get; set; }
        public int almacenOrigen { get; set; }
        public string almacenOrigenDesc { get; set; }
        public int almacenDestino { get; set; }
        public string almacenDestinoDesc { get; set; }
        public string tipoSurtido { get; set; }
        public string comentarioSurtidoQuitar { get; set; }
    }
}
