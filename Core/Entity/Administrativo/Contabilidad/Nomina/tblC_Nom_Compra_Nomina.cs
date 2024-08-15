using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Administracion.Nomina;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_Compra_Nomina
    {
        public int id { get; set; }
        public int nomina_id { get; set; }
        public int year { get; set; }
        public int periodo { get; set; }
        public int tipoNomina { get; set; }
        public string cc { get; set; }
        public string ccDesc { get; set; }
        public int numeroCompra { get; set; }
        public string tipoRequisicion { get; set; }
        public bool registroActivo { get; set; }
    }
}
