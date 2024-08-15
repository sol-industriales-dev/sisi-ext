using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos
{
    public class IncidenciasEmpDetDTO
    {
        public string nombre { get; set; }
        public string ape_paterno { get; set; }
        public string ape_materno { get; set; }
        public int tipo_nomina { get; set; }
        public int clave_depto { get; set; }
        public int puesto { get; set; }
        public string descripcion { get; set; }
        public string cc { get; set; }
        public int anio { get; set; }
        public int periodo { get; set; }
        public int id_incidencia { get; set; }
        public int clave_empleado { get; set; }
        public string estatus { get; set; }
        public string estatusDesc { get; set; }
        public int empleado_modifica { get; set; }
        public string nombreEmpMod { get; set; }
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
        public decimal bonoMensual { get; set; }
        public decimal bonoUnico { get; set; }
        public string observaciones { get; set; }
        public int archivo_enviado { get; set; }
        public int dias_extras { get; set; }
        public int dias_extra_concepto { get; set; }
        public decimal horas_extras { get; set; }
        public decimal prima_dominical { get; set; }
        public string observacionesBono { get; set; }
        public int countBonosPersonales { get; set; }
        public IncidenciasEmpDetDTO()
        {

        }
        public IncidenciasEmpDetDTO(tblRH_BN_Incidencias obj)
        {
            id_incidencia = obj.id_incidencia;
            clave_empleado = obj.clave_empleado;
            bono = obj.bono;
            observaciones = obj.observaciones;
            archivo_enviado = obj.archivo_enviado;
            dias_extras = obj.dias_extras;
            prima_dominical = obj.prima_dominical;
            dia1 = obj.dia1;
            dia2 = obj.dia2;
            dia3 = obj.dia3;
            dia4 = obj.dia4;
            dia5 = obj.dia5;
            dia6 = obj.dia6;
            dia7 = obj.dia7;
            dia8 = obj.dia8;
            dia9 = obj.dia9;
            dia10 = obj.dia10;
            dia11 = obj.dia11;
            dia12 = obj.dia12;
            dia13 = obj.dia13;
            dia14 = obj.dia14;
            dia15 = obj.dia15;
            dia16 = obj.dia16;
            he_dia1 = obj.he_dia1;
            he_dia2 = obj.he_dia2;
            he_dia3 = obj.he_dia3;
            he_dia4 = obj.he_dia4;
            he_dia5 = obj.he_dia5;
            he_dia6 = obj.he_dia6;
            he_dia7 = obj.he_dia7;
            he_dia8 = obj.he_dia8;
            he_dia9 = obj.he_dia9;
            he_dia10 = obj.he_dia10;
            he_dia11 = obj.he_dia11;
            he_dia12 = obj.he_dia12;
            he_dia13 = obj.he_dia13;
            he_dia14 = obj.he_dia14;
            he_dia15 = obj.he_dia15;
            he_dia16 = obj.he_dia16;
            observacionesBono = obj.observacionesBono;
            countBonosPersonales = obj.countBonosPersonales;
            clave_depto = obj.clave_depto;
        }
    }
}
