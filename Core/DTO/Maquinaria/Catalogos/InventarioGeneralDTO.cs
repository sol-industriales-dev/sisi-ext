using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Catalogos
{
    public class inventarioGeneralDTO
    {
        public string Economico { get; set; }

        public string Descripcion { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }

        public string NumeroSerie { get; set; }
        public string Anio { get; set; }
        public string Ubicacion { get; set; }
        public string Redireccion { get; set; }
        public string cc { get; set; }
        public string ccCargoObra { get; set; }
        public string CargoObra { get; set; }
        public string Resgurdante { get; set; }
        public string HorometroAcumulado { get; set; }
        public string Tipo { get; set; }
        public int idEconomico { get; set; }
        public string empresa { get; set; }
        public int empresaID { get; set; }
        public string Estatus { get; set; }
    }
}
