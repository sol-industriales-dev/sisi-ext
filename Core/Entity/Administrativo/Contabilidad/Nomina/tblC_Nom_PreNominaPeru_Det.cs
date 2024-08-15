using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_PreNominaPeru_Det
    {
        public int id { get; set; }
        public int prenominaID { get; set; }
        public int clave_empleado { get; set; }
        public string nombre_empleado { get; set; }
        public string puesto { get; set; }
        public decimal basico { get; set; }
        public decimal jornada_semanal { get; set; }
        public decimal horas_extra_60 { get; set; }
        public decimal horas_extra_100 { get; set; }
        public decimal horas_nocturnas { get; set; }
        public decimal descuento_medico { get; set; }
        public decimal feriados { get; set; }
        public decimal subsidios { get; set; }
        public decimal buc { get; set; }
        public decimal bono_altitud { get; set; }
        public decimal indemnizacion { get; set; }
        public decimal dominical { get; set; }
        public decimal bonificacion_extraordinaria { get; set; }
        public decimal bonificacion_alta_especial { get; set; }
        public decimal vacaciones_truncas { get; set; }
        public decimal asignacion_escolar { get; set; }
        public decimal bono_por_altura { get; set; }
        public decimal devolucion_5ta { get; set; }
        public decimal gratificacion_proporcional { get; set; }
        public decimal adelanto_quincena { get; set; }
        public decimal adelanto_gratificacion_semestre { get; set; }
        public decimal total_percepciones { get; set; }

        public decimal AFP_obligatoria { get; set; }
        public decimal AFP_voluntaria { get; set; }
        public decimal AFP_comision { get; set; }
        public decimal AFP_prima { get; set; }
        public decimal conafovicer { get; set; }
        public decimal essalud_vida { get; set; }
        public decimal onp { get; set; }
        public decimal renta_5ta { get; set; }
        public decimal total_deducciones { get; set; }

        public decimal essalud_aportes { get; set; }
        public decimal AFP_aportes { get; set; }
        public decimal total_aportaciones { get; set; }

        public decimal total_pagar { get; set; }
        public decimal total_deposito { get; set; }
        public decimal bono_transporte { get; set; }
        public decimal BAE { get; set; }
        public decimal CTS { get; set; }
        public decimal Utilidades { get; set; }
        public decimal totalIngresos { get; set; }
        public decimal totalEgresos { get; set; }
    }
}