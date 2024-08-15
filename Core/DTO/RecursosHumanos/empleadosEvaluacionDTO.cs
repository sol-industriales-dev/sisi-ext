using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos
{
    public class empleadosEvaluacionDTO
    {
        public List<tblRH_BN_Evaluacion_Det> validos { get; set; }
        public List<tblRH_BN_Evaluacion_Det> novalidos { get; set; }
        public int estatus { get; set; }
        public int evaluacionID { get; set; }
    }
}
