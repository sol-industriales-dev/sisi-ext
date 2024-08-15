using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_BN_Incidencias
    {
        public int id { get; set; }
        public int tipo_nomina { get; set; }
        public int clave_depto { get; set; }
        public int puesto { get; set; }
        public string cc { get; set; }
        public int anio { get; set; }
        public int periodo { get; set; }
        public int id_incidencia { get; set; }
        public int id_bonoAdminMonto { get; set; }
        public int idAuth { get; set; } 
        public int stAuth { get; set; }
        public int clave_empleado { get; set; }
        public string estatus { get; set; }
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
        public decimal he_dia1 { get; set; }
        public decimal he_dia2 { get; set; }
        public decimal he_dia3 { get; set; }
        public decimal he_dia4 { get; set; }
        public decimal he_dia5 { get; set; }
        public decimal he_dia6 { get; set; }
        public decimal he_dia7 { get; set; }
        public decimal he_dia8 { get; set; }
        public decimal he_dia9 { get; set; }
        public decimal he_dia10 { get; set; }
        public decimal he_dia11 { get; set; }
        public decimal he_dia12 { get; set; }
        public decimal he_dia13 { get; set; }
        public decimal he_dia14 { get; set; }
        public decimal he_dia15 { get; set; }
        public decimal he_dia16 { get; set; }
        public decimal bono { get; set; }
        public string observaciones { get; set; }
        public int archivo_enviado { get; set; }
        public int dias_extras { get; set; }
        public decimal prima_dominical { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechRegistro { get; set; }

        public int empleado_modifica { get; set; }

        public DateTime fecha_modifica { get; set; }
        public DateTime fechaAuth { get; set; }
        public string observacionesBono { get; set; }
        public int countBonosPersonales { get; set; }
    }
}
