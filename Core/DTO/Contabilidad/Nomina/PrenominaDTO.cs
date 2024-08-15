using Core.Entity.Administrativo.Contabilidad.Nomina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos
{
    public class PrenominaDTO
    {
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public tblC_Nom_Nomina prenomina { get; set; }
        public List<tblC_Nom_PreNomina_Det> detalle { get; set; }
        public List<tblC_Nom_PreNomina_Aut> autorizadores { get; set; }

        #region BOLETA NOMINA PERU (REPORTE)
        // ENCABEZADO
        public int clave_empleado { get; set; }
        public string nombre_empleado { get; set; }
        public int tipoNomina { get; set; }
        public string cc { get; set; }
        public string descripcion { get; set; }
        public string puesto { get; set; }
        public DateTime fecha_nac { get; set; }
        public DateTime fecha_alta { get; set; }
        public string fechaNacimiento { get; set; }
        public string fechaAlta { get; set; }
        public string periodoPago { get; set; }
        public string area { get; set; }
        public string fechaPago { get; set; }
        public int clave_banco { get; set; }
        public string desc_banco { get; set; }
        public string num_cta_pago { get; set; }
        public string num_cta_fondo_aho { get; set; }

        // INGRESOS
        public decimal jornada_semanal { get; set; }
        public decimal horas_extra_60 { get; set; }
        public decimal horas_extra_100 { get; set; }
        public decimal horas_nocturnas { get; set; }
        public decimal feriados { get; set; }
        public decimal subsidios { get; set; }
        public decimal buc { get; set; }
        public decimal bono_altitud { get; set; }
        public decimal indemnizacion { get; set; }
        public decimal CTS { get; set; }
        public decimal Utilidades { get; set; }
        public decimal dominical { get; set; }
        public decimal gratificacion_proporcional { get; set; }
        public decimal bonificacion_extraordinaria { get; set; }
        public decimal bonificacion_alta_especial { get; set; }
        public decimal vacaciones_truncas { get; set; }
        public decimal bono_transporte { get; set; }
        public decimal asignacion_escolar { get; set; }
        public decimal BAE { get; set; }
        public decimal bono_por_altura { get; set; }
        public decimal devolucion_5ta { get; set; }
        public decimal totalIngresos { get; set; }

        // DESCUENTOS
        public decimal descuento_medico { get; set; }
        public decimal AFP_obligatoria { get; set; }
        public decimal AFP_voluntaria { get; set; }
        public decimal AFP_comision { get; set; }
        public decimal AFP_prima { get; set; }
        public decimal conafovicer { get; set; }
        public decimal essalud_vida { get; set; }
        public decimal onp { get; set; }
        public decimal renta_5ta { get; set; }
        public decimal totalDeducciones { get; set; }

        // APORTACIONES
        public decimal essalud_aportes { get; set; }
        public decimal AFP_aportes { get; set; }
        public decimal totalAportaciones { get; set; }

        public decimal netoPagar { get; set; }
        public string cuentaImporteAbonado { get; set; }
        #endregion
    }
}
