using Core.Entity.Maquinaria.Mantenimiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Mantenimiento.DTO2._0
{
    public class dataSetGridLubProxDTO
    {
        public componenteMantenimiento objComponente { get; set; }
        public string componente { get; set; }
        public List<lstAceiteDTO> Suministros { get; set; }
        public string  TipoPrueba  { get; set; }
        public string VidaUtil { get; set; }
        public string Info { get; set; }
        public string VidaConsumida { get; set; }
        public string VidaRestante { get; set; }
        public string Programar { get; set; }
        public JGHisDTO objHis { get; set; }
        public tblM_BitacoraControlAceiteMantProy proyectado { get; set; }
        public int idComponente { get; set; }
        public int idmantenimiento { get; set; }
    }
}
