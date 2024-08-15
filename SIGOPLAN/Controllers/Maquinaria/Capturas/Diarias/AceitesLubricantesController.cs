using Core.DTO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Captura.aceites;
using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria.Captura;
using Core.Enum.Maquinaria;
using Data.Factory.Maquinaria.Captura;
using Data.Factory.Maquinaria.Catalogos;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Capturas.Diarias
{
    public class AceitesLubricantesController : BaseController
    {

        #region Factory
        MaquinaFactoryServices maquinaFactoryServices;
        AceitesLubricantesFactoryService AceitesFactory;
        MaquinariaAceitesLubricantesFactoryService MaqAceiteFactory;
        CentroCostosFactoryServices centroCostosFactoryServices;
        CapturaHorometroFactoryServices capturaHorometroFactoryServices;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            maquinaFactoryServices = new MaquinaFactoryServices();
            AceitesFactory = new AceitesLubricantesFactoryService();
            MaqAceiteFactory = new MaquinariaAceitesLubricantesFactoryService();
            centroCostosFactoryServices = new CentroCostosFactoryServices();
            capturaHorometroFactoryServices = new CapturaHorometroFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        #endregion
        #region Consumo
        // GET: AceitesLubricantes
        public ActionResult Consumo()
        {
            return View();
        }
        public ActionResult Reporte()
        {
            return View();
        }

        public ActionResult tblConsumoMaqAceiteLubricante(string cc, string consumo, int turno, DateTime fecha, int tipo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<aceitesLubricantesDTO> newListCosas = new List<aceitesLubricantesDTO>();

                var lst = MaqAceiteFactory.getMaquinariaAceitesFactoryServices().GetLstMaqAceiteLubricante(cc, consumo, turno, fecha, tipo);
                if (lst.Count > 0)
                {
                    Session["lstMaq"] = lst;
                    Session["cc"] = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(cc);
                    Session["turno"] = turno == 1 ? "1RA" : turno == 2 ? "2DA" : "3RA";
                    Session["consumo"] = consumo;
                    Session["fecha"] = fecha;
                }

                result.Add("lstMaq", lst);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult tblRepMaqAceiteLubricante(string cc, int turno, DateTime inicio, DateTime fin, string economico)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<RptAceitesLubricantes> rptLst = new List<RptAceitesLubricantes>();
                var lstAceites = AceitesFactory.getAceitesLubricantesFactoryService().GetAllAceitesLubricantes(0, "");
                var lst = MaqAceiteFactory.getMaquinariaAceitesFactoryServices().GetRepMaqAceiteLubricante(cc, turno, inicio, fin, economico);
                if (lst.Count > 0)
                {


                    var ln = lst.GroupBy(x => x.Economico);




                    var horometrosEco = capturaHorometroFactoryServices.getCapturaHorometroServices().getDataTableByRangeDate(inicio, fin, ln.Select(x => x.Key).ToList());

                    foreach (var item in ln)
                    {
                        RptAceitesLubricantes objRpt = new RptAceitesLubricantes();

                        objRpt.noEconomico = item.Key;
                        var motores = item.GroupBy(x => x.MotorId);
                        objRpt.motor = motores.FirstOrDefault().Sum(x => x.MotorVal);
                        if (motores.FirstOrDefault().Key != motores.LastOrDefault().Key) { objRpt.motor2 = motores.LastOrDefault().Sum(x => x.MotorVal); }
                        objRpt.trans = item.Sum(x => x.TransmisionVal);
                        objRpt.hidraulico = item.Sum(x => x.HidraulicoVal);
                        objRpt.diferenciales = item.Sum(x => x.DiferencialVal);
                        objRpt.mandoFinal = item.Sum(x => (x.MDIzqVal + x.MFTIzqVal));
                        objRpt.direccion = item.Sum(x => x.DirVal);
                        objRpt.grasa = item.Sum(x => x.GrasaVal);
                        objRpt.otros1 = item.Sum(x => x.otros1);
                        objRpt.otros2 = item.Sum(x => x.otros2);
                        objRpt.otros3 = item.Sum(x => x.otros3);
                        objRpt.otros4 = item.Sum(x => x.otros4);
                        objRpt.Antifreeze = item.Sum(x => x.Antifreeze);

                        objRpt.motorDes = motores.FirstOrDefault().Key;
                        if (motores.FirstOrDefault().Key != motores.LastOrDefault().Key) { objRpt.motor2Des = motores.LastOrDefault().Key; }
                        objRpt.transDescr = objRpt.trans > 0 ? item.FirstOrDefault(x => x.TransmisionVal > 0).TransmisionID : item.Select(x => x.TransmisionID).FirstOrDefault();
                        objRpt.hidraulicoDesc = objRpt.hidraulico > 0 ? item.FirstOrDefault(x => x.HidraulicoVal > 0).HidraulicoID : item.Select(x => x.HidraulicoID).FirstOrDefault();
                        objRpt.difDesc = objRpt.diferenciales > 0 ? item.FirstOrDefault(x => x.DiferencialVal > 0).DiferencialId : item.Select(x => x.DiferencialId).FirstOrDefault();
                        objRpt.mandoFinalDesc = objRpt.mandoFinal > 0 ? item.Where(x => (x.MDIzqVal + x.MFTIzqVal) > 0).Select(x => (x.MDIzqID == 0 ? x.MFTIzqId : x.MDIzqID)).FirstOrDefault() : item.Select(x => (x.MDIzqID == 0 ? x.MFTIzqId : x.MDIzqID)).FirstOrDefault();
                        objRpt.direccionDesc = objRpt.direccion > 0 ? item.FirstOrDefault(x => x.DirVal > 0).DirId : item.Select(x => x.DirId).FirstOrDefault();
                        objRpt.grasaDesc = objRpt.grasa > 0 ? item.FirstOrDefault(x => x.GrasaVal > 0).GrasaId : item.Select(x => x.GrasaId).FirstOrDefault();
                        objRpt.otro1Desc = objRpt.otros1 > 0 ? item.FirstOrDefault(x => x.otros1 > 0).otroId1 : item.Select(x => x.otroId1).FirstOrDefault();
                        objRpt.otro2Desc = objRpt.otros2 > 0 ? item.FirstOrDefault(x => x.otros2 > 0).otroId2 : item.Select(x => x.otroId2).FirstOrDefault();
                        objRpt.otro3Desc = objRpt.otros3 > 0 ? item.FirstOrDefault(x => x.otros3 > 0).otroId3 : item.Select(x => x.otroId3).FirstOrDefault();
                        objRpt.otro4Desc = objRpt.otros4 > 0 ? item.FirstOrDefault(x => x.otros4 > 0).otroId4 : item.Select(x => x.otroId4).FirstOrDefault();

                        objRpt.horasTrabajadas = horometrosEco.Where(x => x.Economico == item.Key).Sum(x => x.HorasTrabajo).ToString();


                        rptLst.Add(objRpt);
                    }

                    Session["lstMaq"] = rptLst;
                    Session["cc"] = cc.Equals("--Seleccione--") ? "TODOS LOS CENTROS DE COSTOS" : centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(cc);
                    Session["turno"] = turno == 1 ? "1RA" : turno == 2 ? "2DA" : turno == 3 ? "3RA" : "TODOS";
                    Session["fecha"] = string.Format("Del {0:dd/MM/yyyy} Al {1:dd/MM/yyyy}", inicio, fin);
                    Session["economico"] = economico;
                }
                //var lstLubricante = lst.Select(x => new
                //{
                //    FECHA = x.Fecha.ToShortDateString(),
                //    ECONOMICO = x.Economico,
                //    HOROMETRO = x.Horometro,
                //    TURNO = x.Turno == 1 ? "1RA" : turno == 2 ? "2DA" : "3RA",
                //    CC = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(x.CC.Replace(" ", string.Empty))

                //}).ToList();


                result.Add("lstMaq", rptLst.ToList().Select(r => new
                {
                    ECONOMICO = r.noEconomico,
                    HOROMETRO = r.horasTrabajadas,
                    Antifreeze = r.Antifreeze,
                    MOTOR = r.motorDes != 0 ? lstAceites.FirstOrDefault(x => x.id == r.motorDes).Descripcion : "N/A",
                    MOTOR2 = r.motor2Des != 0 ? lstAceites.FirstOrDefault(x => x.id == r.motor2Des).Descripcion : "N/A",
                    TRANS = r.transDescr != 0 ? lstAceites.FirstOrDefault(x => x.id == r.transDescr).Descripcion : "N/A",
                    HCO = r.hidraulicoDesc != 0 ? lstAceites.FirstOrDefault(x => x.id == r.hidraulicoDesc).Descripcion : "N/A",
                    DIF = r.difDesc != 0 ? lstAceites.FirstOrDefault(x => x.id == r.difDesc).Descripcion : "N/A",
                    MF = r.mandoFinalDesc != 0 ? lstAceites.FirstOrDefault(x => x.id == r.mandoFinalDesc).Descripcion : "N/A",
                    DIR = r.direccionDesc != 0 ? lstAceites.FirstOrDefault(x => x.id == r.direccionDesc).Descripcion : "N/A",
                    GRASA = r.grasaDesc != 0 ? lstAceites.FirstOrDefault(x => x.id == r.grasaDesc).Descripcion : "N/A",
                    otros2Des = r.otro2Desc != 0 ? lstAceites.FirstOrDefault(x => x.id == r.otro2Desc).Descripcion : "N/A",
                    otros3Des = r.otro3Desc != 0 ? lstAceites.FirstOrDefault(x => x.id == r.otro3Desc).Descripcion : "N/A",
                    otros4Des = r.otro4Desc != 0 ? lstAceites.FirstOrDefault(x => x.id == r.otro4Desc).Descripcion : "N/A",
                    otros1Des = r.otro1Desc != 0 ? lstAceites.FirstOrDefault(x => x.id == r.otro1Desc).Descripcion : "N/A",

                    motorVal = r.motor,
                    motor2Val = r.motor2,
                    transVal = r.trans,
                    hcoVal = r.hidraulico,
                    difVal = r.diferenciales,
                    mfVal = r.mandoFinal,
                    dirVal = r.direccion,
                    GrasaVal = r.grasa,

                    otros1Val = r.otros1,
                    otros2Val = r.otros2,
                    otros3Val = r.otros3,
                    otros4Val = r.otros4

                }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //
        public ActionResult CatidadLubricante(string cc, int turno, DateTime inicio, DateTime fin, string economico)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<RptAceitesLubricantes> lstdedatos = obtenerTabla(cc, turno, inicio, fin, economico);
                var lstAceites = AceitesFactory.getAceitesLubricantesFactoryService().GetAllAceitesLubricantes(0, "");
                

                var lista = lstdedatos.Select(x => x.motorDes).ToList();
                List<insumosDTO> listaCon = lstAceites.Where(x => lista.Contains(x.id)).Select(x=> new insumosDTO{
                
                    insumoID = x.id,
                    descripcionInsumo = x.Descripcion
                } ).ToList();

                var lista2 = lstdedatos.Select(x => x.motor2Des).ToList();
                listaCon.AddRange(lstAceites.Where(x => lista2.Contains(x.id)).Select(x=> new insumosDTO{
                
                    insumoID = x.id,
                    descripcionInsumo = x.Descripcion
                } ).ToList());

                var lista3 = lstdedatos.Select(x => x.transDescr).ToList();
                listaCon.AddRange(lstAceites.Where(x => lista3.Contains(x.id)).Select(x=> new insumosDTO{
                
                    insumoID = x.id,
                    descripcionInsumo = x.Descripcion
                } ).ToList());

                var lista4 = lstdedatos.Select(x => x.hidraulicoDesc).ToList();
                listaCon.AddRange(lstAceites.Where(x => lista4.Contains(x.id)).Select(x=> new insumosDTO{
                
                    insumoID = x.id,
                    descripcionInsumo = x.Descripcion
                } ).ToList());

                var lista5 = lstdedatos.Select(x => x.difDesc).ToList();
                listaCon.AddRange(lstAceites.Where(x => lista5.Contains(x.id)).Select(x=> new insumosDTO{
                
                    insumoID = x.id,
                    descripcionInsumo = x.Descripcion
                } ).ToList());

                var lista6 = lstdedatos.Select(x => x.mandoFinalDesc).ToList();
                listaCon.AddRange(lstAceites.Where(x => lista6.Contains(x.id)).Select(x=> new insumosDTO{
                
                    insumoID = x.id,
                    descripcionInsumo = x.Descripcion
                } ).ToList());

                var lista7 = lstdedatos.Select(x => x.direccionDesc).ToList();
                listaCon.AddRange(lstAceites.Where(x => lista7.Contains(x.id)).Select(x=> new insumosDTO{
                
                    insumoID = x.id,
                    descripcionInsumo = x.Descripcion
                } ).ToList());                

                var lista8 = lstdedatos.Select(x => x.otro2Desc).ToList();
                listaCon.AddRange(lstAceites.Where(x => lista8.Contains(x.id)).Select(x=> new insumosDTO{
                
                    insumoID = x.id,
                    descripcionInsumo = x.Descripcion
                } ).ToList());

                var lista9 = lstdedatos.Select(x => x.otro3Desc).ToList();
                listaCon.AddRange(lstAceites.Where(x => lista9.Contains(x.id)).Select(x=> new insumosDTO{
                
                    insumoID = x.id,
                    descripcionInsumo = x.Descripcion
                } ).ToList());

                var lista10 = lstdedatos.Select(x => x.otro4Desc).ToList();
                listaCon.AddRange(lstAceites.Where(x => lista10.Contains(x.id)).Select(x=> new insumosDTO{
                
                    insumoID = x.id,
                    descripcionInsumo = x.Descripcion
                } ).ToList());

                var lista11 = lstdedatos.Select(x => x.otro1Desc).ToList();
                listaCon.AddRange(lstAceites.Where(x => lista11.Contains(x.id)).Select(x=> new insumosDTO{
                
                    insumoID = x.id,
                    descripcionInsumo = x.Descripcion
                } ).ToList());

                listaCon.Distinct();

                var lst = listaCon.Select(x => x.descripcionInsumo).Distinct().ToList();

                List<EncabezadoTotalesDTO> lstResult = lstdedatos.Select(y =>
                {
                    decimal totalGeneral = 0;      
                    decimal totalx = 0;
                    var lstTotal = listaCon.Select(x=> { 
                        decimal total = 0;
                        if (y.motorDes == x.insumoID)
                        {
                            total += y.motor;
                        }

                        if (y.motor2Des == x.insumoID)
                        {
                            total += y.motor2;
                        }
                        if (y.transDescr == x.insumoID)
                        {
                            total += y.trans;

                        }
                        if (y.hidraulicoDesc == x.insumoID)
                        {
                            total += y.hidraulico;

                        }
                        if (y.difDesc == x.insumoID)
                        {
                            total += y.diferenciales;

                        }
                        if (y.mandoFinalDesc == x.insumoID)
                        {
                            total += y.mandoFinal;

                        }
                        if (y.direccionDesc == x.insumoID)
                        {
                            total += y.direccion;

                        }
                        if (y.otro2Desc == x.insumoID)
                        {
                            total += y.otros2;

                        }
                        if (y.otro3Desc == x.insumoID)
                        {
                            total += y.otros3;
                        }
                        if (y.otro4Desc == x.insumoID)
                        {
                            total += y.otros4;

                        }
                        if (y.otro1Desc == x.insumoID)
                        {
                            total += y.otros1;
                        }
                        
                        return new TotalesDTO {
                            insumo = x.descripcionInsumo,
                            total = total
                        };}).ToList();             

                    totalGeneral = y.motor + y.motor2 + y.trans + y.hidraulico + y.diferenciales + y.mandoFinal + y.direccion + y.otros2 + y.otros3 + y.otros4 + y.otros1;
                    
                    return new EncabezadoTotalesDTO
                    {
                        noEconomico = y.noEconomico,
                        TotalFila = totalGeneral,
                        totalColumna = totalx,
                        lstAceites = lstTotal.GroupBy(x => x.insumo.Trim()).Select(x => new TotalesDTO { 
                            insumo = x.Key,
                            total = x.Sum(z=>z.total)
                        }).ToList()
                    };
                }).ToList();


                lstResult.Add(funcionObtener(lstResult));

                result.Add(ITEMS, lstResult);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public EncabezadoTotalesDTO funcionObtener(List<EncabezadoTotalesDTO> lstResult)
        {
            EncabezadoTotalesDTO objRetur = new EncabezadoTotalesDTO();
            TotalesDTO objAceites = new TotalesDTO();
            objRetur.lstAceites = new List<TotalesDTO>();
            decimal totalFila = 0;
            for (int f = 0; f < lstResult[0].lstAceites.Count(); f++)
            {
                
                objAceites = new TotalesDTO();
                for (int i = 0; i < lstResult.Count(); i++)
                {
                    objRetur.noEconomico = "TOTAL";
                    objAceites.insumo = lstResult[i].lstAceites[f].insumo;
                    objAceites.total += lstResult[i].lstAceites[f].total;
                    totalFila += lstResult[i].lstAceites[f].total;
                    
                    
                }
                objRetur.lstAceites.Add(objAceites);
            }
            objRetur.TotalFila = totalFila;
            return objRetur;
        }


        public List<RptAceitesLubricantes> obtenerTabla(string cc, int turno, DateTime inicio, DateTime fin, string economico)
        {
            var result = new Dictionary<string, object>();
            List<RptAceitesLubricantes> rptLst = new List<RptAceitesLubricantes>();
            var lstAceites = AceitesFactory.getAceitesLubricantesFactoryService().GetAllAceitesLubricantes(0, "");
            var lst = MaqAceiteFactory.getMaquinariaAceitesFactoryServices().GetRepMaqAceiteLubricante(cc, turno, inicio, fin, economico);
            if (lst.Count > 0)
            {


                var ln = lst.GroupBy(x => x.Economico);




                var horometrosEco = capturaHorometroFactoryServices.getCapturaHorometroServices().getDataTableByRangeDate(inicio, fin, ln.Select(x => x.Key).ToList());

                foreach (var item in ln)
                {
                    RptAceitesLubricantes objRpt = new RptAceitesLubricantes();

                    objRpt.noEconomico = item.Key;
                    var motores = item.GroupBy(x => x.MotorId);
                    objRpt.motor = motores.FirstOrDefault().Sum(x => x.MotorVal);
                    if (motores.FirstOrDefault().Key != motores.LastOrDefault().Key) { objRpt.motor2 = motores.LastOrDefault().Sum(x => x.MotorVal); }
                    objRpt.trans = item.Sum(x => x.TransmisionVal);
                    objRpt.hidraulico = item.Sum(x => x.HidraulicoVal);
                    objRpt.diferenciales = item.Sum(x => x.DiferencialVal);
                    objRpt.mandoFinal = item.Sum(x => (x.MDIzqVal + x.MFTIzqVal));
                    objRpt.direccion = item.Sum(x => x.DirVal);
                    objRpt.otros1 = item.Sum(x => x.otros1);
                    objRpt.otros2 = item.Sum(x => x.otros2);
                    objRpt.otros3 = item.Sum(x => x.otros3);
                    objRpt.otros4 = item.Sum(x => x.otros4);
                    objRpt.Antifreeze = item.Sum(x => x.Antifreeze);

                    objRpt.motorDes = motores.FirstOrDefault().Key;
                    if (motores.FirstOrDefault().Key != motores.LastOrDefault().Key) { objRpt.motor2Des = motores.LastOrDefault().Key; }
                    objRpt.transDescr = objRpt.trans > 0 ? item.FirstOrDefault(x => x.TransmisionVal > 0).TransmisionID : item.Select(x => x.TransmisionID).FirstOrDefault();
                    objRpt.hidraulicoDesc = objRpt.hidraulico > 0 ? item.FirstOrDefault(x => x.HidraulicoVal > 0).HidraulicoID : item.Select(x => x.HidraulicoID).FirstOrDefault();
                    objRpt.difDesc = objRpt.diferenciales > 0 ? item.FirstOrDefault(x => x.DiferencialVal > 0).DiferencialId : item.Select(x => x.DiferencialId).FirstOrDefault();
                    objRpt.mandoFinalDesc = objRpt.mandoFinal > 0 ? item.Where(x => (x.MDIzqVal + x.MFTIzqVal) > 0).Select(x => (x.MDIzqID == 0 ? x.MFTIzqId : x.MDIzqID)).FirstOrDefault() : item.Select(x => (x.MDIzqID == 0 ? x.MFTIzqId : x.MDIzqID)).FirstOrDefault();
                    objRpt.direccionDesc = objRpt.direccion > 0 ? item.FirstOrDefault(x => x.DirVal > 0).DirId : item.Select(x => x.DirId).FirstOrDefault();
                    objRpt.otro1Desc = objRpt.otros1 > 0 ? item.FirstOrDefault(x => x.otros1 > 0).otroId1 : item.Select(x => x.otroId1).FirstOrDefault();
                    objRpt.otro2Desc = objRpt.otros2 > 0 ? item.FirstOrDefault(x => x.otros2 > 0).otroId2 : item.Select(x => x.otroId2).FirstOrDefault();
                    objRpt.otro3Desc = objRpt.otros3 > 0 ? item.FirstOrDefault(x => x.otros3 > 0).otroId3 : item.Select(x => x.otroId3).FirstOrDefault();
                    objRpt.otro4Desc = objRpt.otros4 > 0 ? item.FirstOrDefault(x => x.otros4 > 0).otroId4 : item.Select(x => x.otroId4).FirstOrDefault();

                    objRpt.horasTrabajadas = horometrosEco.Where(x => x.Economico == item.Key).Sum(x => x.HorasTrabajo).ToString();


                    rptLst.Add(objRpt);
                }
            }

            return rptLst;
        }

        public ActionResult SaveMaqAceiteLubricante(List<tblM_MaquinariaAceitesLubricantes> lst)
        {
            var result = new Dictionary<string, object>();
            try
            {
                foreach (var obj in lst)
                {
                    MaqAceiteFactory.getMaquinariaAceitesFactoryServices().GuardarMaqAceiteLubricante(obj);
                }
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ListasDatos
        public ActionResult FillDlEconomico()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var item = maquinaFactoryServices.getMaquinaServices().GetAllMaquinas();
                result.Add(ITEMS, item.Select(x => new { Value = x.noEconomico, Text = x.noEconomico }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillDlAceiteLubricante(int tipoComponente, string economico)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var item = AceitesFactory.getAceitesLubricantesFactoryService()
                    .GetAllAceitesLubricantes(tipoComponente, economico)
                    .OrderBy(x => x.Descripcion);

                result.Add(ITEMS, item
                    .Select(x => new { Value = x.id, Text = x.Descripcion })
                    .OrderBy(x => x.Value));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillDlOrquestas()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var item = maquinaFactoryServices.getMaquinaServices().GetAllMaquinas().Where(x => x.grupoMaquinaria.tipoEquipoID == 3);
                var item2 = maquinaFactoryServices.getMaquinaServices().GetAllPipas();
                var combo = item
                    .Where(x => x.noEconomico.Contains("OR"))
                    .Select(x => new ComboDTO { Value = x.noEconomico, Text = x.noEconomico })
                    .ToList();
                var combo2 = item2
                 .Select(x => new ComboDTO { Value = x.noEconomico, Text = x.noEconomico }).OrderBy(x => x.Text)
                 .ToList();
                combo.Insert(0, new ComboDTO() { Value = "TALLER", Text = "TALLER" });
                combo.AddRange(combo2);
                //combo.Add(new ComboDTO() { Value = "TA-72", Text = "TA-72" });
                //combo.Add(new ComboDTO() { Value = "TA-76", Text = "TA-76" });
                //combo.Add(new ComboDTO() { Value = "TA-77", Text = "TA-77" });
                //combo.Add(new ComboDTO() { Value = "CSE-01", Text = "CSE-01" });
                result.Add(ITEMS, combo);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public ActionResult getDetalleLubricantes(string cc, DateTime inicio, DateTime fin, string economico)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = MaqAceiteFactory.getMaquinariaAceitesFactoryServices().GetRepMaqAceiteLubricante(cc, 0, inicio, fin, economico);
                var lstAceites = AceitesFactory.getAceitesLubricantesFactoryService().GetAllAceitesLubricantes(0, "");
                var nLstr = lst.ToList().Select(r => new rptAceitesLubricantesDTO
                {
                    FECHA = r.Fecha.ToShortDateString(),
                    HOROMETRO = r.Horometro.ToString(),
                    MOTOR = r.MotorVal.ToString(), // r.HidraulicoID != 0 ? lstAceites.FirstOrDefault(x => x.id == r.HidraulicoID).Descripcion : "N/A",
                    TRANS = r.TransmisionVal.ToString(),// r.TransmisionID != 0 ? lstAceites.FirstOrDefault(x => x.id == r.TransmisionID).Descripcion : "N/A",
                    HCO = r.HidraulicoVal.ToString(),//r.HidraulicoID != 0 ? lstAceites.FirstOrDefault(x => x.id == r.HidraulicoID).Descripcion : "N/A",
                    DIF = r.DiferencialVal.ToString(),//r.DiferencialId != 0 ? lstAceites.FirstOrDefault(x => x.id == r.DiferencialId).Descripcion : "N/A",
                    MF = r.MFTIzqVal.ToString(),//r.MFTDerId != 0 ? lstAceites.FirstOrDefault(x => x.id == r.MFTDerId).Descripcion : "N/A",
                    DIR = r.DirVal.ToString(),//r.DirId != 0 ? lstAceites.FirstOrDefault(x => x.id == r.DirId).Descripcion : "N/A"
                    GRASA = r.GrasaVal.ToString(),
                    otros1 = r.otros1.ToString(),
                    otros2 = r.otros2.ToString(),
                    otros3 = r.otros3.ToString(),
                    otros4 = r.otros4.ToString(),
                    Antifreeze = r.Antifreeze.ToString(),
                    motorDes = r.MotorId != 0 ? lstAceites.FirstOrDefault(x => x.id == r.MotorId).Descripcion : "N/A",
                    //motorDes2 = r.MotorId != 0 ? lstAceites.LastOrDefault(x => x.id == r.MotorId).Descripcion : "N/A",
                    transDes = r.TransmisionID != 0 ? lstAceites.FirstOrDefault(x => x.id == r.TransmisionID).Descripcion : "N/A",
                    hcoDes = r.HidraulicoID != 0 ? lstAceites.FirstOrDefault(x => x.id == r.HidraulicoID).Descripcion : "N/A",
                    mfDes = r.MFTIzqId != 0 ? lstAceites.FirstOrDefault(x => x.id == r.MFTIzqId).Descripcion : "N/A",
                    dirDes = r.DirId != 0 ? lstAceites.FirstOrDefault(x => x.id == r.DirId).Descripcion : "N/A",
                    grasaDes = r.GrasaId != 0 ? lstAceites.FirstOrDefault(x => x.id == r.GrasaId).Descripcion : "N/A",
                    otros1Des = r.otroId1 != 0 ? lstAceites.FirstOrDefault(x => x.id == r.otroId1).Descripcion : "N/A",
                    otros2Des = r.otroId2 != 0 ? lstAceites.FirstOrDefault(x => x.id == r.otroId2).Descripcion : "N/A",
                    otros3Des = r.otroId3 != 0 ? lstAceites.FirstOrDefault(x => x.id == r.otroId3).Descripcion : "N/A",
                    otros4Des = r.otroId4 != 0 ? lstAceites.FirstOrDefault(x => x.id == r.otroId4).Descripcion : "N/A"
                });
                Session["noEconomicoRepLubricantesDetalle"] = economico;
                result.Add("rptList", nLstr.ToList());

                Session["rptdetallelubicantesDTO"] = nLstr.ToList();
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExistenciaLubricante(string almacen)
        {
            return Json(AceitesFactory.getAceitesLubricantesFactoryService().ExistenciaLubricante(almacen));
        }

        #region Catálogo Lubricantes
        public ActionResult CatLubricantes()
        {
            return View();
        }

        public JsonResult CargarCatalogoLubricantes()
        {
            return Json(AceitesFactory.getAceitesLubricantesFactoryService().CargarCatalogoLubricantes(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetComboSubconjuntos()
        {
            var resultado = new Dictionary<string, object>();

            resultado.Add(ITEMS, GlobalUtils.ParseEnumToCombo<AceiteLubricanteEnum>());
            resultado.Add(SUCCESS, "SUCCESS");

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetComboModelos()
        {
            return Json(AceitesFactory.getAceitesLubricantesFactoryService().GetComboModelos(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GuardarNuevoLubricante(tblM_CatAceitesLubricantes lubricante)
        {
            return Json(AceitesFactory.getAceitesLubricantesFactoryService().GuardarNuevoLubricante(lubricante), JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditarLubricante(tblM_CatAceitesLubricantes lubricante, AceiteLubricanteEnum subConjuntoID_Anterior)
        {
            return Json(AceitesFactory.getAceitesLubricantesFactoryService().EditarLubricante(lubricante, subConjuntoID_Anterior), JsonRequestBehavior.AllowGet);
        }

        public JsonResult EliminarLubricante(tblM_CatAceitesLubricantes lubricante)
        {
            return Json(AceitesFactory.getAceitesLubricantesFactoryService().EliminarLubricante(lubricante), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}