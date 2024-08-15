using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Requisicion
{
    public class rptRequisicionInfoDTO
    {
        public string folioReq { get; set; }
        public string cc { get; set; }
        public string lab { get; set; }
        public string tipoReq { get; set; }
        public string fechaHoy { get; set; }
        public string fechaReq { get; set; }
        public string estatus { get; set; }
        public string comentarios { get; set; }
        public string solicito { get; set; }
        public string vobo { get; set; }
        public string autorizo { get; set; }
        public string usuarioSolicitaDesc { get; set; }
        public string usuarioSolicitaUso { get; set; }
        public List<rptRequisicionPartidasDTO> partidas { get; set; }
    }
}
