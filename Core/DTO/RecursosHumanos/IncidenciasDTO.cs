using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos
{
    public class IncidenciasDTO
    {
        public int clave_empleado { get; set; }
        public string Nombre { get; set; }
        public string cc { get; set; }
        public string ccNombre { get; set; }
        public int anio { get; set; }
        public int periodo { get; set; }
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
        public int dia12{ get; set; }
        public int dia13 { get; set; }
        public int dia14 { get; set; }
        public int dia15 { get; set; }
        public int dia16 { get; set; }
        public int nomina { get; set; }
        public DateTime fecha_inicial { get; set; }
        public DateTime fecha_final { get; set; }
        public DateTime fecha { get; set; }
        public int tipo_nomina { get; set; }
        public string strTipoNomina { get; set; }
    }
}
