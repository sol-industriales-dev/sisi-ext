using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.ControlObra;

namespace Core.DTO.ControlObra.Gestion
{
    public class rFacultamientosDTO
    {

        public int id { get; set; }
        public int idEmpleado { get; set; }
        public string nombreEmpleado { get; set; }
        public List<string> lstcc { get; set; }
        public List<string> ccDescripcion { get; set; }
        public PrivilegioOrdenCambioEnum privilegio { get; set; }
        public string privilegioDesc { get; set; }
        public bool estatus { get; set; }

    }
}
