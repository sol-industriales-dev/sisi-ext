using Core.DTO.RecursosHumanos.Vacaciones;
using Core.Entity.Principal.Usuarios;
using Core.Entity.RecursosHumanos.Vacaciones;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_BN_Incidencia_det
    {
        public int id { get; set; }
        public int incidenciaID { get; set; }
        [JsonIgnore]
        public tblRH_BN_Incidencia incidencia { get; set; }
        public DateTime fecha { get; set; }
        public int evaluacion_detID { get; set; }
        public int bonoUnico_detID { get; set; }
        public int usuarioID { get; set; }
        [JsonIgnore]
        public tblP_Usuario usuario { get; set; }
        public int id_incidencia { get; set; }
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
        public decimal bonoDM { get; set; }
        public decimal bonoCuadrado { get; set; }
        public string bonoDM_Obs { get; set; }
        public decimal bonoU { get; set; }
        public string bono_Obs { get; set; }
        public int total_Dias { get; set; }
        public decimal totalo_Horas { get; set; }
        public bool estatus { get; set; }
        [NotMapped]
        public int countBonosPersonales { get; set; }
        public string nombre { get; set; }
        public string ape_paterno { get; set; }
        public string ape_materno { get; set; }
        public decimal horas_extras { get; set; }
        public int puesto { get; set; }
        public string puestoDesc { get; set; }
        public int clave_depto { get; set; }
        public string deptoDesc { get; set; }
        [NotMapped]
        public DateTime fechaAlta { get; set; }
        [NotMapped]
        public bool isBaja { get; set; }
        [NotMapped]
        public DateTime fechaBaja { get; set; }
        [NotMapped]
        public int numDiasExtratemporales { get; set; }
        [NotMapped]
        public int numDiasExtratemporalesARestar { get; set; }
        public bool primaDominical { get; set; }
        public int dias_extra_concepto { get; set; }
        [NotMapped]
        public List<VacFechasDTO> lstFechasExtratemporaneas { get; set; }

        #region PERU
        public decimal horas_extra_25 { get; set; }
        public decimal horas_extra_35 { get; set; }
        public decimal horas_extra_60 { get; set; }
        public decimal horas_extra_100 { get; set; }
        public decimal quinta_externa { get; set; }
        public decimal minutos_tardanza { get; set; }
        public decimal horas_lactancia { get; set; }
        public decimal horas_nocturnas { get; set; }
        public decimal subsidio { get; set; }
        public decimal bono_transporte { get; set; }
        #endregion
    }
}
