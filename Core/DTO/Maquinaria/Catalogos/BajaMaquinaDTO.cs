using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Catalogos
{
    public class BajaMaquinaDTO
    {
        public string Economico { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string NumeroSerie { get; set; }
        public string Anio { get; set; }
        public string CentroCostos { get; set; }
        public string Redireccion { get; set; }
        public string CargoObra { get; set; }
        public string Motivo { get; set; }
    }
}
