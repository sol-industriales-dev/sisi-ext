using Core.Enum.RecursosHumanos.Tabuladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Tabuladores
{
    public class ReportesTabuladoresDTO
    {
        public int id { get; set; }
        public int FK_LineaNegocio { get; set; }
        public int? año { get; set; }
        public EstatusGestionAutorizacionEnum estatus { get; set; }
        public string comentarioRechazo { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int? FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }

        public string descLN { get; set; }
        public bool esFirmar { get; set; }
        public string lineaNegociosCC { get; set; }

        public List<string> lstDescCC { get; set; }
        public List<string> lstDescLineaNegocio { get; set; }
        

    }
}
