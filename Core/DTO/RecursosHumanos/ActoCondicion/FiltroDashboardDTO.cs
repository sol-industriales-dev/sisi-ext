using Core.DTO.Administracion.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.ActoCondicion
{
    public class FiltroDashboardDTO
    {
        public List<string> ccs { get; set; }
        public DateTime fechaInicial { get; set; }
        public DateTime fechaFinal { get; set; }
        public int claveSupervisor { get; set; }
        public int departamentoID { get; set; }
        public int subclasificacion { get; set; }
        public List<MultiSegDTO> arrGrupos { get; set; }
        public int clasificacionID { get; set; }
    }
}
