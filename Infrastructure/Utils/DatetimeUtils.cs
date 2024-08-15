using Core.DTO.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utils
{
    public static class DatetimeUtils
    {
        /// <summary>
        /// Devuelve el número de la semana del año según la fecha
        /// </summary>
        /// <param name="time">Fecha a consultar</param>
        /// <returns>Número de semana del año</returns>
        public static int noSemana(this DateTime time)
        {
            var df1 = DateTimeFormatInfo.CurrentInfo;
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, df1.CalendarWeekRule, df1.FirstDayOfWeek);
        }
        /// <summary>
        /// Devuelve el número de la semana del año según la fecha
        /// </summary>
        /// <param name="time">Fecha a consultar</param>
        /// /// <param name="inicio">Inicio de semanar</param>
        /// <returns>Número de semana del año</returns>
        public static int noSemana(this DateTime time, DayOfWeek inicio)
        {
            var df1 = DateTimeFormatInfo.CurrentInfo;
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, df1.CalendarWeekRule, inicio);
        }
        /// <summary>
        /// Siguiente día de la semana
        /// </summary>
        /// <param name="from">Fecha a consultar</param>
        /// <param name="dayOfWeek">Dia que busca</param>
        /// <returns></returns>
        public static DateTime Siguiente(this DateTime from, DayOfWeek dayOfWeek)
        {
            int start = (int)from.DayOfWeek;
            int target = (int)dayOfWeek;
            if (target <= start)
                target += 7;
            return from.AddDays(target - start);
        }
        public static DateTime primerDiaSemana(int anio, int noSemana)
        {
            DateTime enero1 = new DateTime(anio, 1, 1);
            int daysOffset = DayOfWeek.Monday - enero1.DayOfWeek;
            var df1 = DateTimeFormatInfo.CurrentInfo;
            // Use first Thursday in January to get first week of the year as
            // it will never be in Week 52/53
            DateTime firstThursday = enero1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int primerSemana = cal.GetWeekOfYear(firstThursday, df1.CalendarWeekRule, df1.FirstDayOfWeek);

            var weekNum = noSemana;
            // As we're adding days to a date in Week 1,
            // we need to subtract 1 in order to get the right date for week #1
            //if(primerSemana == 1)
            //{
            //    weekNum -= 1;
            //}

            // Using the first Thursday as starting week ensures that we are starting in the right year
            // then we add number of weeks multiplied with days
            var result = firstThursday.AddDays(weekNum * 7);

            // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
            return result.AddDays(-3);
        }

        /// <summary>
        /// Recibe 2 parametros referenciados a los cuales se le asignara
        /// el periodo de la semana actual siendo martes el ultimo día de la semana
        /// y miercoles el primer día.
        /// </summary>
        /// <param name="fechaInicio">Fecha referenciada a la cual se le asignara la fecha de inicio de la semana (miercoles)</param>
        /// <param name="fechaFin">Fecha referenciada a la cual se le asignara la fecha de corte de la semana (martes)</param>
        public static void PeriodoSemanaCorte_Martes(ref DateTime fechaInicio, ref DateTime fechaFin)
        {
            fechaFin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 11, 59, 59);
            fechaInicio = new DateTime(2020, 1, 1);
            while (fechaFin.DayOfWeek != DayOfWeek.Tuesday)
            {
                fechaFin = fechaFin.AddDays(1);
            }
            fechaInicio = fechaFin.AddDays(-6);
        }

        /// <summary>
        /// Regresa la fecha de corte (martes) de la fecha recibida
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static DateTime UltimoDiaSemanaCorte_Martes(DateTime fecha)
        {
            fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 11, 59, 59);
            while (fecha.DayOfWeek != DayOfWeek.Tuesday)
            {
                fecha = fecha.AddDays(1);
            }
            return fecha;
        }

        /// <summary>
        /// Regresa una lista indicando los días del mes solicitado; por ejemplo de la fecha (01/09/2020) y día (martes)
        /// regresa {1, 8, 15, 22, 29}
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="dia"></param>
        /// <returns></returns>
        public static List<int> DiasEspecificosDelMes(DateTime fecha, DayOfWeek dia)
        {
            var dias = new List<int>();

            int año = fecha.Year;
            int mes = fecha.Month;
            int diasDelMes = DateTime.DaysInMonth(año, mes);
            DateTime comienzoDelMes = new DateTime(año, mes, 1);
            for (int i = 0; i < diasDelMes; i++)
            {
                if (comienzoDelMes.AddDays(i).DayOfWeek == dia)
                {
                    dias.Add(i + 1);
                }
            }

            return dias;
        }

        /// <summary>
        /// Regresa la cantidad de meses de diferencia entre dos fechas
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public static int MesesDiferencia(DateTime fechaInicio, DateTime fechaFin)
        {
            return Math.Abs(((fechaFin.Year - fechaInicio.Year) * 12) + (fechaFin.Month - fechaInicio.Month));
        }

        public static int DiasDiferencia(DateTime fechaInicio, DateTime fechaFin)
        {
            var diasEntreMeses = Math.Abs(((fechaFin - fechaInicio).Days + 1));
            return Decimal.ToInt32(Math.Abs(diasEntreMeses));
        }

        public static List<InfoPeriodoDTO> SemanasEntreFechas(DateTime fechaInicial, DateTime fechaFin)
        {
            var diaInicial = fechaInicial;
            var numSemanas = Math.Ceiling((fechaFin - fechaInicial).TotalDays / 7);

            var semanas = new List<InfoPeriodoDTO>();

            for (int i = 1; i <= (int)numSemanas; i++)
            {
                var infoSemana = new InfoPeriodoDTO();

                infoSemana.numPeriodo = i;
                infoSemana.fechaInicio = diaInicial;
                infoSemana.fechaFin = diaInicial.AddDays(6);

                semanas.Add(infoSemana);

                diaInicial = infoSemana.fechaInicio.AddDays(7);
            }

            if (numSemanas - (int)numSemanas > 0)
            {
                var infoSemana = new InfoPeriodoDTO();

                infoSemana.numPeriodo = (int)numSemanas + 1;
                infoSemana.fechaInicio = diaInicial;
                infoSemana.fechaFin = diaInicial.AddDays(6);

                semanas.Add(infoSemana);
            }

            return semanas;
        }

        public static List<InfoPeriodoDTO> MesesEntreFechas(DateTime fechaInicial, DateTime fechaFin)
        {
            var diaInicial = fechaInicial;
            var numMeses = Math.Ceiling((fechaFin - fechaInicial).TotalDays / 30);

            var meses = new List<InfoPeriodoDTO>();

            for (int i = 1; i <= (int)numMeses; i++)
            {
                var infoMes = new InfoPeriodoDTO();
                var inicio = new DateTime(fechaInicial.Year, i, 1);
                var fin = inicio.AddMonths(1).AddDays(-1);
                infoMes.numPeriodo = i;
                infoMes.fechaInicio = inicio;
                infoMes.fechaFin = fin;

                meses.Add(infoMes);

                diaInicial = infoMes.fechaInicio.AddDays(30);
            }

            if (numMeses - (int)numMeses > 0)
            {
                var infoMes = new InfoPeriodoDTO();

                infoMes.numPeriodo = (int)numMeses + 1;
                infoMes.fechaInicio = diaInicial;
                infoMes.fechaFin = diaInicial.AddDays(29);

                meses.Add(infoMes);
            }

            return meses;
        }
    }
}
