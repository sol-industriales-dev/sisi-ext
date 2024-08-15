using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad
{
    public class EstadoResultadoConfiguracionDTO
    {
        public DateTime fecha { get; set; }
        public List<EmpresaEnum> listaEmpresas { get; set; }
        public List<Tuple<string, string, int>> listaColumnas { get; set; }
        public List<Dictionary<string, object>> listaDatos { get; set; }
    }
}
