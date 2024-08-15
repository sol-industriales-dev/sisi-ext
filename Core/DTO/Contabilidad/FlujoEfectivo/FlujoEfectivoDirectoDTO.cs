using Core.Entity.Administrativo.Contabilidad.FlujoEfectivo;
using Core.Enum.Administracion.Propuesta;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.FlujoEfectivo
{
    public class FlujoEfectivoDirectoDTO
    {
        private string p1;
        private object p2;
        private DateTime fecha;

        public string clase { get; set; }
        public int idCpto { get; set; }
        public string descripcion { get; set; }
        public decimal flujoTotalProyecto { get; set; }
        public decimal flujoTotalProyectoAnterior { get; set; }
        public decimal consulta { get; set; }
        public decimal planeacionConsulta { get; set; }
        public decimal planeacion { get; set; }
        public decimal recientePlaneacion1 { get; set; }
        public decimal recientePlaneacion2 { get; set; }
        #region Constructores
        public FlujoEfectivoDirectoDTO()
        {

        }
        public FlujoEfectivoDirectoDTO(string desc)
        {
            descripcion = desc.ToUpper();
        }
        public FlujoEfectivoDirectoDTO(tblC_FED_CatConcepto concepto, List<tblC_FED_CapPlaneacion> lstPlan)
        {
            var buscado = lstPlan[0];
            var corteMax = lstPlan[1];
            var planSig = lstPlan[2];
            descripcion = string.Format("({0}) {1}", concepto.operador.Trim(), concepto.Concepto.ToUpper()); 
            idCpto = concepto.id;
            clase = GetDescriptionEnum(tipoPropuestaEnum.Saldo);
            flujoTotalProyecto = corteMax.flujoTotal;
            planeacion = planSig.planeado;
            consulta = buscado.corte;
            planeacionConsulta = buscado.planeado;
            recientePlaneacion1 = corteMax.planeado;
            recientePlaneacion2 = corteMax.corte;
        }
        public FlujoEfectivoDirectoDTO(tblC_FED_CatConcepto concepto, List<tblC_FED_CapPlaneacion> lstPlanBuscando, List<tblC_FED_CapPlaneacion> lstPlanCorteMax, List<tblC_FED_CapPlaneacion> lstPlanSig)
        {
            descripcion = string.Format("({0}) {1}", concepto.operador.Trim(), concepto.Concepto.ToUpper());
            idCpto = concepto.id;
            clase = GetDescriptionEnum(tipoPropuestaEnum.Saldo);
            flujoTotalProyecto = lstPlanCorteMax.Sum(x => x.flujoTotal);
            planeacion = lstPlanSig.Sum(x => x.planeado);
            consulta = lstPlanBuscando.Sum(x => x.corte);
            planeacionConsulta = lstPlanBuscando.Sum(x => x.planeado);
            recientePlaneacion1 = lstPlanCorteMax.Sum(x => x.planeado);
            recientePlaneacion2 = lstPlanCorteMax.Sum(x => x.corte);
        }
        public FlujoEfectivoDirectoDTO(tblC_FED_CatConcepto concepto, IEnumerable<tblC_FED_CapPlaneacion> lstPlanBuscando, IEnumerable<tblC_FED_CapPlaneacion> lstPlanCorteMax, IEnumerable<tblC_FED_CapPlaneacion> lstPlanSig)
        {
            descripcion = string.Format("({0}) {1}", concepto.operador.Trim(), concepto.Concepto.ToUpper());
            idCpto = concepto.id;
            clase = GetDescriptionEnum(tipoPropuestaEnum.Saldo);
            flujoTotalProyecto = lstPlanCorteMax.Sum(x => x.flujoTotal);
            planeacion = lstPlanSig.Sum(x => x.planeado);
            consulta = lstPlanBuscando.Sum(x => x.corte);
            planeacionConsulta = lstPlanBuscando.Sum(x => x.planeado);
            recientePlaneacion1 = lstPlanCorteMax.Sum(x => x.planeado);
            recientePlaneacion2 = lstPlanCorteMax.Sum(x => x.corte);
        }
        #endregion
        string GetDescriptionEnum(tipoPropuestaEnum eLement)
        {
            Type type = eLement.GetType();
            MemberInfo[] memberInfo = type.GetMember(eLement.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] atributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (atributes != null && atributes.Length > 0)
                {
                    return ((DescriptionAttribute)atributes[0]).Description;
                }
            }
            return eLement.ToString();
        }
    }
}
