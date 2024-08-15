using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    //raguilar 23/11/17
    public class tblRH_AutorizacionAditivaDeductiva
    {
        public int id { get; set; }
        public int id_AditivaDeductiva { get; set; }
        public int clave_Aprobador { get; set; }
        public string nombre_Aprobador { get; set; }
        public string responsable { get; set; }
        public string puestoAprobador { get; set; }
        public bool estatus { get; set; }
        public string firma { get; set; }
        public bool autorizando { get; set; }
        public bool rechazado { get; set; }
        public int orden { get; set; }
        public bool tipoAutoriza { get; set; }
        public string fechafirma { get; set; }
        public string comentario { get; set; }
    }
}
