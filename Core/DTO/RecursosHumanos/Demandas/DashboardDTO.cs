using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Demandas
{
    public class DashboardDTO
    {
        #region FILTROS
        public int filtro_Anio { get; set; }
        public string filtro_CC { get; set; }
        #endregion

        #region GRAFICA: DEMANDAS ACTIVAS POR AÑO DE PRESENTACIÓN
        public int cantDemandasActivas { get; set; }
        public int anioDemanda { get; set; }
        #endregion

        #region GRAFICA: COMPORTAMIENTO DE LAS DEMANDAS SEGUN EL AÑO SELECCIONADO
        public int tdAnio { get; set; }
        public int tdInicioMes_Ene { get; set; }
        public int tdInicioMes_Feb { get; set; }
        public int tdInicioMes_Mar { get; set; }
        public int tdInicioMes_Abr { get; set; }
        public int tdInicioMes_May { get; set; }
        public int tdInicioMes_Jun { get; set; }
        public int tdInicioMes_Jul { get; set; }
        public int tdInicioMes_Ago { get; set; }
        public int tdInicioMes_Sep { get; set; }
        public int tdInicioMes_Oct { get; set; }
        public int tdInicioMes_Nov { get; set; }
        public int tdInicioMes_Dic { get; set; }
        public int tdInicioMes_Total { get; set; }
        public int tdNuevas_Ene { get; set; }
        public int tdNuevas_Feb { get; set; }
        public int tdNuevas_Mar { get; set; }
        public int tdNuevas_Abr { get; set; }
        public int tdNuevas_May { get; set; }
        public int tdNuevas_Jun { get; set; }
        public int tdNuevas_Jul { get; set; }
        public int tdNuevas_Ago { get; set; }
        public int tdNuevas_Sep { get; set; }
        public int tdNuevas_Oct { get; set; }
        public int tdNuevas_Nov { get; set; }
        public int tdNuevas_Dic { get; set; }
        public int tdNuevas_Total { get; set; }
        public int tdCierres_Ene { get; set; }
        public int tdCierres_Feb { get; set; }
        public int tdCierres_Mar { get; set; }
        public int tdCierres_Abr { get; set; }
        public int tdCierres_May { get; set; }
        public int tdCierres_Jun { get; set; }
        public int tdCierres_Jul { get; set; }
        public int tdCierres_Ago { get; set; }
        public int tdCierres_Sep { get; set; }
        public int tdCierres_Oct { get; set; }
        public int tdCierres_Nov { get; set; }
        public int tdCierres_Dic { get; set; }
        public int tdCierres_Total { get; set; }
        public int tdFinDeMes_Ene { get; set; }
        public int tdFinDeMes_Feb { get; set; }
        public int tdFinDeMes_Mar { get; set; }
        public int tdFinDeMes_Abr { get; set; }
        public int tdFinDeMes_May { get; set; }
        public int tdFinDeMes_Jun { get; set; }
        public int tdFinDeMes_Jul { get; set; }
        public int tdFinDeMes_Ago { get; set; }
        public int tdFinDeMes_Sep { get; set; }
        public int tdFinDeMes_Oct { get; set; }
        public int tdFinDeMes_Nov { get; set; }
        public int tdFinDeMes_Dic { get; set; }
        public int tdFinDeMes_Total { get; set; }
        public int tdTotalBajas_Anio { get; set; }
        public int tdTotalBajas_Ene { get; set; }
        public int tdTotalBajas_Feb { get; set; }
        public int tdTotalBajas_Mar { get; set; }
        public int tdTotalBajas_Abr { get; set; }
        public int tdTotalBajas_May { get; set; }
        public int tdTotalBajas_Jun { get; set; }
        public int tdTotalBajas_Jul { get; set; }
        public int tdTotalBajas_Ago { get; set; }
        public int tdTotalBajas_Sep { get; set; }
        public int tdTotalBajas_Oct { get; set; }
        public int tdTotalBajas_Nov { get; set; }
        public int tdTotalBajas_Dic { get; set; }
        public int tdTotalBajas_Total { get; set; }
        public int tdTotalEmpleados_Anio { get; set; }
        public int tdTotalEmpleados_Ene { get; set; }
        public int tdTotalEmpleados_Feb { get; set; }
        public int tdTotalEmpleados_Mar { get; set; }
        public int tdTotalEmpleados_Abr { get; set; }
        public int tdTotalEmpleados_May { get; set; }
        public int tdTotalEmpleados_Jun { get; set; }
        public int tdTotalEmpleados_Jul { get; set; }
        public int tdTotalEmpleados_Ago { get; set; }
        public int tdTotalEmpleados_Sep { get; set; }
        public int tdTotalEmpleados_Oct { get; set; }
        public int tdTotalEmpleados_Nov { get; set; }
        public int tdTotalEmpleados_Dic { get; set; }
        public int tdTotalEmpleados_Total { get; set; }
        #endregion

        #region TABLA: CUANTILLA TOTAL - FINIQUITO 100% 
        public decimal cuantillaTotal { get; set; }
        public decimal finiquitoAl100 { get; set; }
        public decimal diferencia { get; set; }
        public string nombre { get; set; }
        public string puesto { get; set; }
        #endregion
    }
}
