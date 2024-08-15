using Core.DTO.Administracion.DocumentosXPagar;
using Core.Entity.Administrativo.DocumentosXPagar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.DocumentosXPagar
{
    public class ReporteDTO
    {
        public  List<DesgloseGeneralDTO> detalle { get; set; }
        public tblAF_DxP_Contrato contrato { get; set; }

     
    }
}
