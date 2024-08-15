using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class AutorizacionResguardoDTO
    {
        public string NombreEmpleado { get; set; }
        public string Puesto { get; set; }
        public string Obra { get; set; }
        public string NoEconomico { get; set; }
        public string Modelo { get; set; }
        public string Descripcion { get; set; }
        public string Marca { get; set; }
        public string Serie { get; set; }
        public string noPlaca { get; set; }
        public decimal KM { get; set; }
        public string Encierro { get; set; }
        public string Comentario { get; set; }
        public string FechaResguardo { get; set; }

    }
}
