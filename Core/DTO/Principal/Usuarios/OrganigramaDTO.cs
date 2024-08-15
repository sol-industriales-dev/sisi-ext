using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Principal.Usuarios
{
    public class OrganigramaDTO
    {
        public int usuarioID { get; set; }
        public string usuario { get; set; }
        public int departamentoID { get; set; }
        public string departamento { get; set; }
        public int? puestoID { get; set; }
        public string puesto { get; set; }
        public int? puestoPadreID { get; set; }
        public string puestoPadre { get; set; }
        public int nivel { get; set; }
        public int childsCount { get; set; }
        public List<OrganigramaDTO> childs { get; set; }
    }
}
