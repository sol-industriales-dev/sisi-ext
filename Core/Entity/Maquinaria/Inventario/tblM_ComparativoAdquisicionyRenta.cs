using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_ComparativoAdquisicionyRenta
    {
        public int id { get; set; }
        public int idAsignacion { get; set; }
        public int estatus { get; set; }
        public int idMegusta { get; set; }
        public int estatusFinanciera { get; set; }
        public string folioAdquisicion { get; set; }
        public string folioFinanciera { get; set; }
        public string ComentarioGeneral { get; set; }
        public DateTime? fechaDeElaboracion { get; set; }
        public DateTime? fechaDeElaboracionFinanciero { get; set; }
        public string obra { get; set; }
        public string nombreDelEquipo { get; set; }
        public bool compra { get; set; }
        public bool renta { get; set; }
        public bool roc { get; set; }
        public string tipoMoneda { get; set; }
    }
}
