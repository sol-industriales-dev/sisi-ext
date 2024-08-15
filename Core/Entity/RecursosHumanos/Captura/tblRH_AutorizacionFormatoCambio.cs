using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_AutorizacionFormatoCambio
    {
        public int id {get;set;}
        public int Id_FormatoCambio {get;set;}
        public int Clave_Aprobador { get; set; }
        public string Nombre_Aprobador { get; set; }
        public string PuestoAprobador { get; set; }
        public string Responsable { get; set; }
        public bool Estatus { get; set; }
        public string Firma { get; set; }
        public bool Autorizando { get; set; }
        public bool Rechazado { get; set; }
        public int Orden { get; set; }
        public bool tipoAutoriza { get; set; }
        public string comentario { get; set; }
    }
}
