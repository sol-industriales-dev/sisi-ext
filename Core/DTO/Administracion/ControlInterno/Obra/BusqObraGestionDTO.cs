using Core.Enum.Administracion.ControlInterno.Obra;
using Core.Enum.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.ControlInterno.Obra
{
    public class BusqObraGestionDTO
    {
        public List<tipoCatalogoEnum> lstTipo { get; set; }
        public List<authEstadoEnum> lstAuth { get; set; }
        public List<string> lstCC { get; set; }
        public List<string> lstAC { get; set; }
        public void verificaBusq()
        {
            if(lstTipo == null)
            {
                lstTipo = new List<tipoCatalogoEnum>();
            }
            if(lstAuth == null)
            {
                lstAuth = new List<authEstadoEnum>();
            }
            if(lstAC == null)
            {
                lstAC = new List<string>();
            }
            if(lstCC == null)
            {
                lstCC = new List<string>();
            }
        }
    }
}
