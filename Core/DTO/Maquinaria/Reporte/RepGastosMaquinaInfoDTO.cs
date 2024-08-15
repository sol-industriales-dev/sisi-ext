using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte
{
    public class RepGastosMaquinaInfoDTO
    {
        ///descripcion, marca, modelo, saldoinicial, fecha_adquisicion
        ///

        public string descripcion { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public string saldoinicial { get; set; }
        public string fechaAdquisicion { get; set; }
        public string depreciacion { get; set; }
        
    }
}
