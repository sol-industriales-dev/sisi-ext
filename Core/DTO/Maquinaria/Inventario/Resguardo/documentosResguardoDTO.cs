using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario.Resguardo
{
    public class documentosResguardoDTO
    {
        public int idDocumento { get; set; }
        public int tipoDocumento { get; set; }
        public bool existe { get; set; }
    }
}
