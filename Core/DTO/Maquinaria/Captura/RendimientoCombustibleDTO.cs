using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
   public class RendimientoCombustibleDTO
   {
       public string Economico { get; set; }
       public string Descripcion { get; set; }
       public string Marca { get; set; }
       public string Modelo { get; set; }
       public decimal HInicial { get; set; }
       public decimal Hfinal { get; set; }
       public decimal HTrabajadas { get; set; }
       public decimal ConsumoLTS { get; set; }
       public string RendimientoLTS { get; set; }
       public int RendimientoTeorico { get; set; }
       public int Capacidad { get; set; }
       public string Observaciones { get; set; }
       public string bajo { get; set; }
       public string medio { get; set; }
       public string alto { get; set; }

    }
}
