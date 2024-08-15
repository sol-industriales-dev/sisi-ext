using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Multiempresa;

namespace Core.DTO.Contabilidad
{
    public class EstadoResultadoEmpresasDTO
    {
        public EmpresaEnum empresa { get; set; }
        public string empresaDesc { get; set; }
        public List<EstadoResultadoDTO> listaDatos { get; set; }
    }
}
