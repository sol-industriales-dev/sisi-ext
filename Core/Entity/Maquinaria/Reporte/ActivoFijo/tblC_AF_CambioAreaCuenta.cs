using Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo
{
    public class tblC_AF_CambioAreaCuenta : InfoRegistroDTO
    {
        public int id { get; set; }
        public int maquinaId { get; set; }
        public string acAnterior { get; set; }
        public string acNuevo { get; set; }
    }
}
