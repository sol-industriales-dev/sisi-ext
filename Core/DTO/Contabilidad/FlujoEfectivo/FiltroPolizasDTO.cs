using Core.Enum.Administracion.FlujoEfectivo;
using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.FlujoEfectivo
{
    public class FiltroPolizasDTO
    {
        public tipoDetalleEnum tipo { get; set; }
        public EmpresaEnum empresa { get; set; }
        public List<int> listTM { get; set; }
        public List<int> listTMArr { get; set; }
        public BusqFlujoEfectivoDTO busq { get; set; }
        public BusqFlujoEfectivoDetDTO busqDet { get; set; }
        public List<string> lstCC { get; set; }
        public List<string> lstAC { get; set; }
        public int area { get; set; }
        public int cuenta { get; set; }

    }
}
