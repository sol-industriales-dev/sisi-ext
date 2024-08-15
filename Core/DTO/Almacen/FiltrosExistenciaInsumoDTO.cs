using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Almacen
{
    public class FiltrosExistenciaInsumoDTO
    {
        public string cc { get; set; }
        public int almacen { get; set; }
        public int insumo { get; set; }
        public string area_alm { get; set; }
        public string lado_alm { get; set; }
        public string estante_alm { get; set; }
        public string nivel_alm { get; set; }
    }
}
