using System;
using Core.Entity.Administrativo.Contabilidad.FlujoEfectivo;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Generico.Fecha;

namespace Core.DTO.Contabilidad.FlujoEfectivo
{
    public class FlujoEfectivoOperativoDTO
    {
        public string descripcion { get; set; }
        public decimal ene { get; set; }
        public decimal feb { get; set; }
        public decimal mar { get; set; }
        public decimal abr { get; set; }
        public decimal may { get; set; }
        public decimal jun { get; set; }
        public decimal jul { get; set; }
        public decimal ago { get; set; }
        public decimal sep { get; set; }
        public decimal oct { get; set; }
        public decimal nov { get; set; }
        public decimal dic { get; set; }
        public decimal mes { get; set; }
        //public decimal mesMas { get; set; }
        public decimal mesPorcentaje { get; set; }
        public decimal acum { get; set; }
        //public decimal acumMas { get; set; }
        public decimal acumPorcentaje { get; set; }
        public string clase { get; set; }
        public int idConcepto { get; set; }
        public int mesActual { get; set; }
        #region Contructores
        public FlujoEfectivoOperativoDTO()
        {

        }
        /// <summary>
        /// Encabezado del grupo
        /// </summary>
        /// <param name="padre"></param>
        public FlujoEfectivoOperativoDTO(tblC_FE_CatConcepto padre)
        {
            descripcion = string.Format("ACTIVIDADES DE {0}", padre.Concepto).ToUpper();
            idConcepto = padre.id;
        }
        /// <summary>
        /// Suamtoria de polizas
        /// </summary>
        /// <param name="padre">Grupo</param>
        /// <param name="lstMovPol">Movimiento polizas</param>
        /// <param name="fecha">Fecha límite</param>
        public FlujoEfectivoOperativoDTO(tblC_FE_CatConcepto padre, List<tblC_FE_MovPol> lstMovPol, DateTime fecha, decimal cobroCliente, decimal cobroClienteMes)
        {
            descripcion = (padre.Concepto ?? string.Empty).ToUpper();
            idConcepto = padre.id;
            ene = lstMovPol.Where(w => w.mes == (int)MesEnum.Enero).Sum(s => s.monto);
            feb = lstMovPol.Where(w => w.mes == (int)MesEnum.Febrero).Sum(s => s.monto);
            mar = lstMovPol.Where(w => w.mes == (int)MesEnum.Marzo).Sum(s => s.monto);
            abr = lstMovPol.Where(w => w.mes == (int)MesEnum.Abril).Sum(s => s.monto);
            may = lstMovPol.Where(w => w.mes == (int)MesEnum.Mayo).Sum(s => s.monto);
            jun = lstMovPol.Where(w => w.mes == (int)MesEnum.Junio).Sum(s => s.monto);
            jul = lstMovPol.Where(w => w.mes == (int)MesEnum.Julio).Sum(s => s.monto);
            ago = lstMovPol.Where(w => w.mes == (int)MesEnum.Agosto).Sum(s => s.monto);
            sep = lstMovPol.Where(w => w.mes == (int)MesEnum.Septiembre).Sum(s => s.monto);
            oct = lstMovPol.Where(w => w.mes == (int)MesEnum.Octubre).Sum(s => s.monto);
            nov = lstMovPol.Where(w => w.mes == (int)MesEnum.Noviembre).Sum(s => s.monto);
            dic = lstMovPol.Where(w => w.mes == (int)MesEnum.Diciembre).Sum(s => s.monto);
            acum = lstMovPol.Sum(s => s.monto);
            acumPorcentaje = DivideA(acum, cobroCliente) * 100;
            mesActual = fecha.Month;
            mes = lstMovPol.Where(w => w.mes == mesActual).Sum(s => s.monto);
            mesPorcentaje = DivideA(mes, cobroClienteMes) * 100;
        }
        /// <summary>
        /// Sumador de conceptos
        /// </summary>
        /// <param name="padre">Grupo</param>
        /// <param name="lstMovPol">Movimientos de poliza</param>
        /// <param name="fecha">fecha límite</param>
        public FlujoEfectivoOperativoDTO(tblC_FE_CatConcepto padre, List<FlujoEfectivoOperativoDTO> lstMovPol, DateTime fecha, decimal cobroCliente, decimal cobroClienteMes)
        {
            descripcion = string.Format("FLUJO DE EFECTIVO GENERADO EN ACTIVIDADES DE {0}", padre.Concepto).ToUpper();
            idConcepto = padre.id;
            ene = lstMovPol.Sum(s => s.ene);
            feb = lstMovPol.Sum(s => s.feb);
            mar = lstMovPol.Sum(s => s.mar);
            abr = lstMovPol.Sum(s => s.abr);
            may = lstMovPol.Sum(s => s.may);
            jun = lstMovPol.Sum(s => s.jun);
            jul = lstMovPol.Sum(s => s.jul);
            ago = lstMovPol.Sum(s => s.ago);
            sep = lstMovPol.Sum(s => s.sep);
            oct = lstMovPol.Sum(s => s.oct);
            nov = lstMovPol.Sum(s => s.nov);
            dic = lstMovPol.Sum(s => s.dic);
            acum = ene + feb + mar + abr + may + jun + jul + ago + sep + oct + nov + dic;
            mesActual = fecha.Month;
            switch (mesActual)
            #region case
            {
                case (int)MesEnum.Enero:
                    mes = ene;
                    break;
                case (int)MesEnum.Febrero:
                    mes = feb;
                    break;
                case (int)MesEnum.Marzo:
                    mes = mar;
                    break;
                case (int)MesEnum.Abril:
                    mes = abr;
                    break;
                case (int)MesEnum.Mayo:
                    mes = may;
                    break;
                case (int)MesEnum.Junio:
                    mes = jun;
                    break;
                case (int)MesEnum.Julio:
                    mes = jul;
                    break;
                case (int)MesEnum.Agosto:
                    mes = ago;
                    break;
                case (int)MesEnum.Septiembre:
                    mes = sep;
                    break;
                case (int)MesEnum.Octubre:
                    mes = oct;
                    break;
                case (int)MesEnum.Noviembre:
                    mes = nov;
                    break;
                case (int)MesEnum.Diciembre:
                    mes = dic;
                    break;
            }
            #endregion
            acumPorcentaje = DivideA(acum, cobroCliente) * 100;
            mesPorcentaje = DivideA(mes, cobroClienteMes) * 100;
        }
        public FlujoEfectivoOperativoDTO(List<tblC_FE_SaldoInicial> lstInicial, List<tblC_FE_MovPol> lstMovPol, DateTime fecha)
        {
            descripcion = "EFECTIVO Y EQUIVALENTE DE EFECTIVO AL INICIO DEL PERIODO";
            idConcepto = -1;
            mesActual = fecha.Month;
            var saldoInicial = lstInicial.Sum(s => s.saldo);
            acum = saldoInicial;
            ene = saldoInicial;
            feb = fecha.Month >= (int)MesEnum.Febrero ? ene + lstMovPol.Where(w => w.mes == (int)MesEnum.Enero).Sum(s => s.monto) : 0;
            mar = fecha.Month >= (int)MesEnum.Marzo ? feb + lstMovPol.Where(w => w.mes == (int)MesEnum.Febrero).Sum(s => s.monto) : 0;
            abr = fecha.Month >= (int)MesEnum.Abril ? mar + lstMovPol.Where(w => w.mes == (int)MesEnum.Marzo).Sum(s => s.monto) : 0;
            may = fecha.Month >= (int)MesEnum.Mayo ? abr + lstMovPol.Where(w => w.mes == (int)MesEnum.Abril).Sum(s => s.monto) : 0;
            jun = fecha.Month >= (int)MesEnum.Junio ? may + lstMovPol.Where(w => w.mes == (int)MesEnum.Mayo).Sum(s => s.monto) : 0;
            jul = fecha.Month >= (int)MesEnum.Julio ? jun + lstMovPol.Where(w => w.mes == (int)MesEnum.Junio).Sum(s => s.monto) : 0;
            ago = fecha.Month >= (int)MesEnum.Agosto ? jul + lstMovPol.Where(w => w.mes == (int)MesEnum.Julio).Sum(s => s.monto) : 0;
            sep = fecha.Month >= (int)MesEnum.Septiembre ? ago + lstMovPol.Where(w => w.mes == (int)MesEnum.Agosto).Sum(s => s.monto) : 0;
            oct = fecha.Month >= (int)MesEnum.Octubre ? sep + lstMovPol.Where(w => w.mes == (int)MesEnum.Septiembre).Sum(s => s.monto) : 0;
            nov = fecha.Month >= (int)MesEnum.Noviembre ? oct + lstMovPol.Where(w => w.mes == (int)MesEnum.Octubre).Sum(s => s.monto) : 0;
            dic = fecha.Month >= (int)MesEnum.Diciembre ? nov + lstMovPol.Where(w => w.mes == (int)MesEnum.Noviembre).Sum(s => s.monto) : 0;
            switch(mesActual)
            #region case
            {
                case (int)MesEnum.Enero:
                    mes = ene;
                    break;
                case (int)MesEnum.Febrero:
                    mes = feb;
                    break;
                case (int)MesEnum.Marzo:
                    mes = mar;
                    break;
                case (int)MesEnum.Abril:
                    mes = abr;
                    break;
                case (int)MesEnum.Mayo:
                    mes = may;
                    break;
                case (int)MesEnum.Junio:
                    mes = jun;
                    break;
                case (int)MesEnum.Julio:
                    mes = jul;
                    break;
                case (int)MesEnum.Agosto:
                    mes = ago;
                    break;
                case (int)MesEnum.Septiembre:
                    mes = sep;
                    break;
                case (int)MesEnum.Octubre:
                    mes = oct;
                    break;
                case (int)MesEnum.Noviembre:
                    mes = nov;
                    break;
                case (int)MesEnum.Diciembre:
                    mes = dic;
                    break;
            }
            #endregion
        }
        decimal DivideA(decimal divisor, decimal dividendo = 0, decimal respuestError = 0)
        {
            try { return decimal.Divide(divisor, dividendo); }
            catch (DivideByZeroException) { return respuestError; }
            catch (Exception) { return respuestError; }
        }
        #endregion
    }
}
