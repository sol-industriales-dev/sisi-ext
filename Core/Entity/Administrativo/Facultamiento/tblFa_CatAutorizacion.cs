using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Facultamiento
{
    public class tblFa_CatAutorizacion
    {
        public int id { get; set; }
        public int idMonto { get; set; }
        public int idTitulo { get; set; }
        public int idTipoAutorizacion { get; set; }
        public int renglon { get; set; }
        public int cve { get; set; }
        public string nombre { get; set; }
        public string descPuesto { get; set; }
        public bool Autorizado { get; set; }
    }
}
