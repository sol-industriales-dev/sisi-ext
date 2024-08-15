using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.BackLogs
{
    public class IndicadoresRehabilitacionTMCDTO
    {
        public int id { get; set; }
        public int idInspTMC { get; set; }
        public int idFrente { get; set; }
        public string areaCuenta { get; set; }
        public int idMes { get; set; }
        public int mes { get; set; }
        public int tipoMaquina { get; set; }
        public int idMotivo { get; set; }
        public decimal total { get; set; }
        public string noEconomico { get; set; }
        public string descripcion { get; set; }
        public string modelo { get; set; }
        public decimal horometro { get; set; }
        public decimal ppto { get; set; }
        public string motivo { get; set; }
        public decimal estatus { get; set; }
        public int idEstatus { get; set; }
        public DateTime fechaPromesa { get; set; }
        public DateTime fechaTermino { get; set; }
        public int diasDesface { get; set; }
        public int cantBL { get; set; }
        public decimal pptoReal { get; set; }
        public decimal cumpDePpto { get; set; }
        public decimal porcCump { get; set; }
        public string frenteTrabajo { get; set; }
        public DateTime fechaAsignacion { get; set; }
        public DateTime fechaEntrega { get; set; }
        public int tiempoRehabilitacion { get; set; }
        public int cantBLEjecutados { get; set; }
        public int idSegPpto { get; set; }
        public bool esLiberado { get; set; }
        public int tipoEvidencia { get; set; }

        #region GRAFICA 2
        public string categoriesGrafica2 { get; set; }
        public decimal dataGrafica2 { get; set; }
        #endregion

        #region GRAFICA 3
        public string categoriesGrafica3 { get; set; }
        public decimal dataGrafica3 { get; set; }
        #endregion

        public List<int> lstMeses { get; set; }
    }
}
