using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_AutorizaConciliacionHorometros
    {
        public int id { get; set; }
        public string folio { get; set; }
        public int autorizaGerenteID { get; set; }
        public int autorizaAdmin { get; set; }
        public int autorizaDirector { get; set; }
        public int estatus { get; set; }
        public string firmaGerente { get; set; }
        public string firmaAdmin { get; set; }
        public int pendienteGerente { get; set; }
        public int pendienteAdmin { get; set; }
        public int pendienteDirector { get; set; }
        public int conciliacionID { get; set; }
        public int autorizando { get; set; }
        public string firmaDirector { get; set; }
        public string comentario { get; set; }
    }

}
