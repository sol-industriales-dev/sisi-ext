using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario.Controles
{
    public class DetalleControlDTO
    {
        public string grupo { get; set; }
        public string tipo { get; set; }
        public string marca { get; set; }
        public string serie { get; set; }
        public string modelo { get; set; }
        public string noPoliza { get; set; }
        public string Economico { get; set; }
        public int TipoCaptura { get; set; }
        public int UsuarioRecibeID { get; set; }
        public string UsuarioRecibeNombre { get; set; }
        public int UsuarioEnviaID { get; set; }
        public string UsuarioEnviaNombre { get; set; }
        public int GrupoID { get; set; }
    }
}
