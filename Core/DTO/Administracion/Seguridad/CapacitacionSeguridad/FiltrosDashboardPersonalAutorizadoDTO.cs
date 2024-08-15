using Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class FiltrosDashboardPersonalAutorizadoDTO
    {
        public List<string> listaCCConstruplan { get; set; }
        public List<string> listaCCArrendadora { get; set; }
        public List<tblS_CapacitacionSeguridadListaAutorizacionCC> listaAreas { get; set; }
        public List<int> listaCursosID { get; set; }
    }
}
