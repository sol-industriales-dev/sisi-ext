using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class StandByDepDTO
    {
        public int Tipo { get; set; }
        public string Descripcion { get; set; }
        public List<dynamic> Datos { get; set; }
    }
}