using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_BN_Incidencia_det_Peru
    {
        public int id { get; set; }
        public int incidenciaID { get; set; }
        public int incidencia_detID { get; set; }
        public int clave_empleado { get; set; }
        public int dia1 { get; set; }
        public int dia2 { get; set; }
        public int dia3 { get; set; }
        public int dia4 { get; set; }
        public int dia5 { get; set; }
        public int dia6 { get; set; }
        public int dia7 { get; set; }
        public int dia8 { get; set; }
        public int dia9 { get; set; }
        public int dia10 { get; set; }
        public int dia11 { get; set; }
        public int dia12 { get; set; }
        public int dia13 { get; set; }
        public int dia14 { get; set; }
        public int dia15 { get; set; }
        public int dia16 { get; set; }
        public string archivoEvidencia { get; set; }
        public int usuarioCreacion { get; set; }
        public int usuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
