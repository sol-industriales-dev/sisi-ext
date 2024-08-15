using Core.DTO;
using Core.DTO.RecursosHumanos;
using Core.Entity.RecursosHumanos.Reportes;
using Core.Enum.Maquinaria.Reportes;
using Core.Enum.RecursosHumanos;
using Data.Factory.RecursosHumanos.Captura;
using Data.Factory.RecursosHumanos.Reportes;
using Newtonsoft.Json;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.RecursosHumanos.Incidencias
{
    public class IncidenciasController : BaseController
    {

        private IncidenciasFactoryService IncidenciasFactoryServices = new IncidenciasFactoryService();
        private FormatoCambioFactoryService capturaFormatoCambioFactoryServices = new FormatoCambioFactoryService();

        // GET: Administrativo/Incidencias
        public ActionResult Index()
        {

            

            return View();
        }

        public ActionResult CatAnios()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, IncidenciasFactoryServices.getIncidenciasService().CatAnios().Select(x => new { Value = x, Text = x }));
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CatCC()
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add(ITEMS, capturaFormatoCambioFactoryServices.getFormatoCambioService().getCCList().Select(x => new { Value = x.cc, Text =x.cc+"-"+x.descripcion }));
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CatPeriodos(int anio, List<string> cc)
        {
            var result = new Dictionary<string, object>();
            
   

            try
            {
                if (cc != null)
                {
                    var objResult = IncidenciasFactoryServices.getIncidenciasService().CatPeriodo(anio, cc[0]).Select(x => new { Value = x[0] + "" + ("" + x[1] == "-" ? "" : "" + x[1]), Text = x });
                    result.Add(ITEMS, objResult);
                }
                else
                {
                    var objResult = IncidenciasFactoryServices.getIncidenciasService().CatPeriodo(anio, null).Select(x => new { Value = x[0] + "" + ("" + x[1] == "-" ? "" : "" + x[1]), Text = x });
                    result.Add(ITEMS, objResult);
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

        public ActionResult CatDias(string nomina)
        {
            var result = new Dictionary<string, object>();

            int dias = nomina.Equals("Quincenal") || nomina.Equals("-Quincenal") ? 15 : 7;

            List<int> lstDias = new List<int>();

            for (int x = 1; x <= dias; x++)
            {
                lstDias.Add(x);
            }

                try
                {
                    result.Add(ITEMS, lstDias.Select(x => new { Value = x, Text = x }));
                    result.Add(SUCCESS, true);

                }
                catch (Exception e)
                {
                    result.Add(MESSAGE, e.Message);
                    result.Add(SUCCESS, false);
                }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CatIncidencias()
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add(ITEMS, IncidenciasFactoryServices.getIncidenciasService().CatIncidencia().Select(x => new { Value = x.id, Text = x.concepto}));
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Buscar(List<int> idIncidencia, List<string> lstCC, DateTime fecha, DateTime inicio, DateTime fin, int idEmpleado=0, int Anio=0, int Periodo=0, int Dia=0, string nomina="")
        {

            IncidenciasDTO objIncidencia = new IncidenciasDTO();

            objIncidencia.clave_empleado = idEmpleado <= 0 ? 0 : idEmpleado;
            //objIncidencia.anio = inicio.Year == fin.Year ? fin.Year : inicio.Year;
            //objIncidencia.periodo = Periodo > 0 ? Periodo : 0;
            objIncidencia.fecha_inicial = inicio;
            objIncidencia.fecha_final = fin;

            objIncidencia.nomina = nomina.Equals("Quincenal") || nomina.Equals("-Quincenal") ? 4 : 1;

            List<IncidenciasDTO> lst = new List<IncidenciasDTO>();

            if (lstCC != null)
            {
                foreach (var cc in lstCC)
                {
                    objIncidencia.cc = cc;

                    var listFull = IncidenciasFactoryServices.getIncidenciasService().getLstIncidencias2Fechas(objIncidencia);

                    foreach (var item in listFull)
                    {
                        lst.Add(item);
                    }
                }
            }

            else {
                objIncidencia.cc = null;

                var listFull = IncidenciasFactoryServices.getIncidenciasService().getLstIncidencias2Fechas(objIncidencia);

                foreach (var item in listFull)
                {
                    lst.Add(item);
                }
            }

            
            List<CatCantIncidencias> listReturn = new List<CatCantIncidencias>();
            if (lstCC != null)
            {
                foreach (var cc in lstCC)
                {
                    var lstdescc = capturaFormatoCambioFactoryServices.getFormatoCambioService().getCCList().Where(x => x.cc == cc).First();
                    var lisDef = lst.Where(x => x.cc == cc);

                    int intInci = 0;
                    if (idIncidencia != null)
                    {
                        foreach (var inci in idIncidencia)
                        {

                            intInci = DefIncidencia(lisDef, inci);

                            var objListRt = new CatCantIncidencias();

                            objListRt.CC = cc+ " - " +lstdescc.descripcion;
                            objListRt.incidencia = Enum.GetName(typeof(Tipo_Incidencia), inci).Replace("_", " ");
                            objListRt.periodo = lisDef.Count() > 0 ? lisDef.First().periodo.ToString() : "Todos";
                            objListRt.Total = intInci.ToString();

                            listReturn.Add(objListRt);

                        }
                    }

                    else
                    {
                        foreach (int nval in Tipo_Incidencia.GetValues(typeof(Tipo_Incidencia)))
                        {
                            intInci = DefIncidencia(lisDef, nval);

                            var objListRt = new CatCantIncidencias();

                            objListRt.CC = cc + " - " + lstdescc.descripcion;
                            objListRt.incidencia = Enum.GetName(typeof(Tipo_Incidencia), nval).Replace("_", " ");
                            objListRt.periodo = lisDef.Count() > 0 ? lisDef.First().periodo.ToString() : "Todos";
                            objListRt.Total = intInci.ToString();

                            listReturn.Add(objListRt);
                        }
                    }
                }
            }

            else
            {
                var lisDef = lst;

                int intInci = 0;
                if (idIncidencia != null)
                {
                    foreach (var inci in idIncidencia)
                    {

                        intInci = DefIncidencia(lisDef, inci);

                        var objListRt = new CatCantIncidencias();

                        objListRt.CC = "todos";
                        objListRt.incidencia = Enum.GetName(typeof(Tipo_Incidencia), inci).Replace("_", " ");
                        objListRt.periodo = lisDef.Count() > 0 ? lisDef.First().periodo.ToString() : "Todos";
                        objListRt.Total = intInci.ToString();

                        listReturn.Add(objListRt);

                    }
                }

                else
                {
                    foreach (int nval in Tipo_Incidencia.GetValues(typeof(Tipo_Incidencia)))
                    {
                        intInci = DefIncidencia(lisDef, nval);

                        var objListRt = new CatCantIncidencias();

                        objListRt.CC = "todos";
                        objListRt.incidencia = Enum.GetName(typeof(Tipo_Incidencia), nval).Replace("_", " ");
                        objListRt.periodo = lisDef.Count() > 0 ? lisDef.First().periodo.ToString() : "Todos";
                        objListRt.Total = intInci.ToString();

                        listReturn.Add(objListRt);
                    }
                }

            }

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = 500000000;

            var jsonReturn = Json(listReturn, JsonRequestBehavior.AllowGet);
            jsonReturn.MaxJsonLength = 500000000;

            //vSesiones.sesionCurrentReport = JsonConvert.SerializeObject(listReturn);
            Session["cr" + ReportesEnum.RHREPINCIDENCIAS] = JsonConvert.SerializeObject(listReturn);
            return jsonReturn;
        }

        public ActionResult buscarDetalle(string strIncid, string cc, DateTime inicio, DateTime fin, int periodo = 0, int Anio = 0, int empleado = 0, int Dia = 0, string nomina = "")
        {

            List<catDetalleIncidencias> listReturn = new List<catDetalleIncidencias>();

            int intIc = incidenciaInt(strIncid);

            try
            {
                IncidenciasDTO objIncidencia = new IncidenciasDTO();

                objIncidencia.clave_empleado = empleado;
                //objIncidencia.anio = Anio;
                //objIncidencia.periodo = periodo;
                objIncidencia.fecha_inicial = inicio;
                objIncidencia.fecha_final = fin;

                objIncidencia.nomina = nomina.Equals("Quincenal") || nomina.Equals("-Quincenal") ? 4 : 1;

                List<IncidenciasDTO> lst = new List<IncidenciasDTO>();

                objIncidencia.cc = cc;

                var listFull = IncidenciasFactoryServices.getIncidenciasService().getLstIncidencias2Fechas(objIncidencia);

                foreach (var item in listFull)
                {
                    lst.Add(item);
                }

                foreach (var listaCompleta in lst)
                {

                    if (listaCompleta.dia1 == intIc)
                    {
                        listReturn.Add(getobjDetalle(listaCompleta, strIncid,1));
                    }
                    if (listaCompleta.dia2 == intIc)
                    {
                        listReturn.Add(getobjDetalle(listaCompleta, strIncid,2));
                    }
                    if (listaCompleta.dia3 == intIc)
                    {
                        listReturn.Add(getobjDetalle(listaCompleta, strIncid,3));
                    }
                    if (listaCompleta.dia4 == intIc)
                    {
                        listReturn.Add(getobjDetalle(listaCompleta, strIncid,4));
                    }
                    if (listaCompleta.dia5 == intIc)
                    {
                        listReturn.Add(getobjDetalle(listaCompleta, strIncid,5));
                    }
                    if (listaCompleta.dia6 == intIc)
                    {
                        listReturn.Add(getobjDetalle(listaCompleta, strIncid,6));
                    }
                    if (listaCompleta.dia7 == intIc)
                    {
                        listReturn.Add(getobjDetalle(listaCompleta, strIncid,7));
                    }
                    if (listaCompleta.dia8 == intIc)
                    {
                        listReturn.Add(getobjDetalle(listaCompleta, strIncid,8));
                    }
                    if (listaCompleta.dia9 == intIc)
                    {
                        listReturn.Add(getobjDetalle(listaCompleta, strIncid,9));
                    }
                    if (listaCompleta.dia10 == intIc)
                    {
                        listReturn.Add(getobjDetalle(listaCompleta, strIncid,10));
                    }
                    if (listaCompleta.dia11 == intIc)
                    {
                        listReturn.Add(getobjDetalle(listaCompleta, strIncid,11));
                    }
                    if (listaCompleta.dia12 == intIc)
                    {
                        listReturn.Add(getobjDetalle(listaCompleta, strIncid,12));
                    }
                    if (listaCompleta.dia13 == intIc)
                    {
                        listReturn.Add(getobjDetalle(listaCompleta, strIncid,13));
                    }
                    if (listaCompleta.dia14 == intIc)
                    {
                        listReturn.Add(getobjDetalle(listaCompleta, strIncid,14));
                    }
                    if (listaCompleta.dia15 == intIc)
                    {
                        listReturn.Add(getobjDetalle(listaCompleta, strIncid,15));
                    }

                    
                }
            }

            catch
            {
                 listReturn = new List<catDetalleIncidencias>();
            }

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = 500000000;

            var jsonReturn = Json(listReturn, JsonRequestBehavior.AllowGet);
            jsonReturn.MaxJsonLength = 500000000;

            //vSesiones.sesionCurrentReportDetIncidencias = JsonConvert.SerializeObject(listReturn);
            Session["cr" + ReportesEnum.RHREPDETINCIDENCIAS] = JsonConvert.SerializeObject(listReturn);
            return jsonReturn;
        }

        private catDetalleIncidencias getobjDetalle(IncidenciasDTO obj, string incidencia, int dia) {

            catDetalleIncidencias objAdd = new catDetalleIncidencias();
            objAdd.clave = obj.clave_empleado;
            objAdd.Nombre =obj.Nombre;
            objAdd.cc = obj.cc;
            objAdd.descripcion = obj.ccNombre;
            objAdd.incidencia = incidencia;
            objAdd.periodo = obj.periodo;
            objAdd.anio = obj.anio;
            objAdd.dia = dia;
            objAdd.tipo_nomina = obj.tipo_nomina;
            objAdd.strTipoNomina = obj.tipo_nomina == 1 ? "Semanal" : "Quincenal";


            objAdd.fecha = dia == 1 ? obj.fecha_inicial.ToString("dd/MM/yyyy") : obj.fecha_inicial.AddDays(dia-1).ToString("dd/MM/yyyy");
            
            return objAdd;

        }

        private int incidenciaInt(string incidencia)
        {
            int result = 0; 
            for (int x = 1; x <=18 ; x++)
            {
                if(x!=14 && x!=15)
                {
                    bool Igual = Enum.GetName(typeof(Tipo_Incidencia), x).Replace("_", " ").Equals(incidencia);

                    if (Igual)
                    {
                        result = x;
                    }
                }
                
            }

            return result;
            
        }

        private int DefIncidencia(IEnumerable<IncidenciasDTO> inc, int caso)
        {
            int cant = 0;
            foreach (var obj in inc)
            {
                if (obj.dia1 == caso)
                {
                    cant++;
                }
                if (obj.dia2 == caso)
                {
                    cant++;
                }
                if (obj.dia3 == caso)
                {
                    cant++;
                }
                if (obj.dia4 == caso)
                {
                    cant++;
                }
                if (obj.dia5 == caso)
                {
                    cant++;
                }
                if (obj.dia6 == caso)
                {
                    cant++;
                }
                if (obj.dia7 == caso)
                {
                    cant++;
                }
                if (obj.dia8 == caso)
                {
                    cant++;
                }
                if (obj.dia9 == caso)
                {
                    cant++;
                }
                if (obj.dia10 == caso)
                {
                    cant++;
                }
                if (obj.dia11 == caso)
                {
                    cant++;
                }
                if (obj.dia12 == caso)
                {
                    cant++;
                }
                if (obj.dia13 == caso)
                {
                    cant++;
                }
                if (obj.dia14 == caso)
                {
                    cant++;
                }
                if (obj.dia15 == caso)
                {
                    cant++;
                }
            }

            return cant;

        }
    }
}