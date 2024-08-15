using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Facultamiento
{
    public class AutorizanteFaDTO
    {
        public int AutorizanteID { get; set; }
        public bool EsAutorizante { get; set; }
        public string Nombre { get; set; }
        public int? UsuarioID { get; set; }
        public bool? Autorizado { get; set; }
        public int Orden { get; set; }
        public int PaqueteFaID { get; set; }
        public string Firma { get; set; }
    }
}
