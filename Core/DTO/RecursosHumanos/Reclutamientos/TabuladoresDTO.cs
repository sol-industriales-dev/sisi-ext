using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class TabuladoresDTO
    {
        public int id { get; set; }
        public int idEK { get; set; }
        public string cc { get; set; }
        public DateTime fecha { get; set; }
        public int id_puesto { get; set; }
        public decimal salario_base { get; set; }
        public decimal complemento { get; set; }
        public decimal bono_de_zona { get; set; }
        public decimal bono_trab_especiales { get; set; }
        public decimal bono_por_produccion { get; set; }
        public decimal bono_otros { get; set; }
        public decimal hora_extra { get; set; }
        public string observaciones { get; set; }
        public decimal nomina { get; set; }
        public bool libre { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public int claveEmpleado { get; set; }
        public int banco { get; set; }
        public int num_cta_pago { get; set; }
        public int num_cta_pago_aho { get; set; }
        public string clave_empleado { get; set; }
        public string tabulador { get; set; }
        public int tabulador_anterior { get; set; }
        public DateTime fecha_cambio { get; set; }
        public DateTime? fechaAplicaCambioDate { get; set; }
        public string fechaAplicaCambio { get; set; }
        public DateTime hora { get; set; }
        public decimal suma { get; set; }
        public string fechaRealNomina { get; set; }
        public bool esNuevoTabulador { get; set; }
        public int? motivoCambio { get; set; }
        public int fk_tabulador { get; set; }
        public int fk_tabuladorDet { get; set; }

        #region REPORTE TABULADORES
        public int tab { get; set; }
        public string puesto { get; set; }
        public int tipoNomina { get; set; }
        public string strTipoNomina { get; set; }
        public string salarioBase { get; set; }
        public string bonoZona { get; set; }
        public string totalNominal { get; set; }
        public string totalMensual { get; set; }
        public string complementoReporte { get; set; }
        public int idPuesto { get; set; }
        #endregion
    }
}