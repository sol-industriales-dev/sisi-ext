using Core.DTO;
using Core.DTO.Encuestas.Proveedores;
using Core.DTO.Encuestas.SubContratista;
using Core.Entity.Encuestas;
using Data.Factory.Encuestas;
using Data.Factory.Maquinaria.Catalogos;
using Infrastructure.DTO;
using Infrastructure.Utils;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SIGOPLAN.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Encuestas.Controllers
{
    public class EncuestasSubContratistasController : BaseController
    {
        private CentroCostosFactoryServices centroCostosFactoryServices;
        private EncuestasSubContratistasFactoryServices encuestasSubContratistasFactoryServices;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            encuestasSubContratistasFactoryServices = new EncuestasSubContratistasFactoryServices();
            centroCostosFactoryServices = new CentroCostosFactoryServices();

            base.OnActionExecuting(filterContext);
        }

        // GET: Encuestas/EncuestasSubContratistas
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            Session["cboSubContratistas"] = null;
            return View();
        }

        public ActionResult Responder()
        {
            return View();
        }

        public ActionResult cboEncuestas(int tipoEncuesta)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = encuestasSubContratistasFactoryServices.getEncuestasSubContratistasFactoryServices().fillCboEncuestas(tipoEncuesta);

                result.Add(ITEMS, res.Select(x => new ComboDTO
                {
                    Value = x.id,
                    Text = x.descripcion

                }).ToList());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public MemoryStream getExcel(DateTime fechaInicio, DateTime fechaFin)
        {
            var result = new Dictionary<string, object>();
            var lstSubContratistas = (List<dataSubContratistaDTO>)Session["lstSubContratistas"];

            result = encuestasSubContratistasFactoryServices.getEncuestasSubContratistasFactoryServices().getExcel(lstSubContratistas);

            if ((bool)result[SUCCESS])
            {
                var stream = (MemoryStream)result[ITEMS];
                Response.Clear();
                Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachement; filename=Reporte.xlsx");
                Response.BinaryWrite(stream.ToArray());
                Response.End();
            }
            return null;
        }

        public ActionResult LoadEncuestasSubContratistas(DateTime fechaInicio, DateTime fechaFin, int encuestaID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var objListSubContratistas = encuestasSubContratistasFactoryServices.getEncuestasSubContratistasFactoryServices().getListaSubContratistas(fechaInicio, fechaFin, encuestaID);

                var lstSubContratistasDTO = objListSubContratistas.Select(x => new dataSubContratistaDTO {
                    nombreSubContratista = x.nombreSubContratista,
                    numProveedor = x.numProveedor,
                    nombreProyecto = x.nombreProyecto,
                    centroCostos = x.centroCostos,
                    centroCostosNombre = x.centroCostosNombre,
                    servicioContrato = x.servicioContrato,
                    convenio = x.convenio,
                    estatus = x.estatus,
                    id = x.id,
                    fechaInicio = Convert.ToDateTime(x.fechaInicio.ToShortDateString()),
                    fechaFin = Convert.ToDateTime(x.fechaFin.ToShortDateString()),
                    Comentarios = "",
                    btn = "",
                    fechaEvaluacion = x.fechaEvaluacion,
                    evaluador = x.evaluador,
                    calificacion = x.calificacion
                }).ToList();

                result.Add("dataDashboard", lstSubContratistasDTO);
                Session["lstSubContratistas"] = lstSubContratistasDTO;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getEncuesta(int encuestaID)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var objEncuesta = encuestasSubContratistasFactoryServices.getEncuestasSubContratistasFactoryServices().getEncuestaID(encuestaID);
                var objListProveedores = encuestasSubContratistasFactoryServices.getEncuestasSubContratistasFactoryServices().getListaPreguntasByIDEncuesta(encuestaID);

                result.Add("id", objEncuesta.id);
                result.Add("titulo", objEncuesta.titulo);
                result.Add("descripcion", objEncuesta.descripcion);
                result.Add("tipoEncuesta", objEncuesta.tipoEncuesta);
                result.Add("preguntas", objListProveedores.ToList().Select(x => new { x.encuestaID, x.estatus, x.id, x.orden, x.pregunta, x.tipo, }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getSubContratistas(string term)
        {
            var items = encuestasSubContratistasFactoryServices.getEncuestasSubContratistasFactoryServices().getNombreSubContratistas(term);// capturaOTFactoryServices.getCapturaOTFactoryServices().getCatEmpleados(term);

            var filteredItems = items.Select(x => new { id = x.numSubContratista, label = x.nombreSubContratista });

            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadSubcontratistaEvaluacion(int contratistaID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var objListSubContratistas = encuestasSubContratistasFactoryServices.getEncuestasSubContratistasFactoryServices().getListaSubContratistasbySubContratista(contratistaID);

                result.Add("dataDashboard", objListSubContratistas.Select(x => new
                {

                    x.nombreSubContratista,
                    x.numProveedor,
                    x.nombreProyecto,
                    x.centroCostos,
                    x.centroCostosNombre,
                    x.servicioContrato,
                    x.convenio,
                    x.estatus,
                    id = x.id[0],
                    Comentarios = "",
                    btn = ""

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

        public ActionResult saveEncuesta(tblEN_EncuestaSubContratista encuesta, List<tblEN_PreguntasSubContratistas> listObj, bool updateInfo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (updateInfo)
                {

                    encuesta.fecha = DateTime.Now;
                    var id = encuestasSubContratistasFactoryServices.getEncuestasSubContratistasFactoryServices().saveEncuestaUpdate(encuesta, listObj);
                }
                else
                {
                    encuesta.creadorID = vSesiones.sesionUsuarioDTO.id;
                    encuesta.fecha = DateTime.Now;
                    encuesta.estatus = true;
                    var dataReturn = encuestasSubContratistasFactoryServices.getEncuestasSubContratistasFactoryServices().saveEncuesta(encuesta, listObj);
                    result.Add("id", dataReturn);
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

        public ActionResult CargarEncuestaResponder(int idEncuesta, int numContrato, string CC, int convenio)
        {
            var result = new Dictionary<string, object>();
            List<PreguntasDTO> objListaPreguntas = new List<PreguntasDTO>();
            try
            {

                var Encuesta = encuestasSubContratistasFactoryServices.getEncuestasSubContratistasFactoryServices().getEncuestaID(idEncuesta);
                var Preguntas = encuestasSubContratistasFactoryServices.getEncuestasSubContratistasFactoryServices().getListaPreguntasByIDEncuesta(idEncuesta);
                var DatosSubContratista = encuestasSubContratistasFactoryServices.getEncuestasSubContratistasFactoryServices().getInfoSubContratista(CC, numContrato, convenio);
                DatosSubContratista.convenio = convenio;
                var CentroCostos = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(DatosSubContratista.centroCostos);

                result.Add("id", Encuesta.id);
                result.Add("CentroCostos", CentroCostos);
                result.Add("getDatosSubcontratista", DatosSubContratista);
                result.Add("titulo", Encuesta.titulo);
                result.Add("descripcion", Encuesta.descripcion);
                result.Add("tipoEncuesta", Encuesta.tipoEncuesta);
                result.Add("preguntas", Preguntas.ToList().Select(x => new { x.encuestaID, x.estatus, x.id, x.orden, x.pregunta, x.tipo, }));
                result.Add("evaluador", (getUsuario().nombre));
                result.Add("convenioID", convenio);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult saveEncuestaResult(List<tblEN_ResultadoSubContratistas> obj, tblEN_ResultadoSubContratistasDet objSingle, int encuestaID, string comentario)
        {
            var result = new Dictionary<string, object>();
            try
            {
                foreach (var i in obj)
                {
                    i.usuarioRespondioID = vSesiones.sesionUsuarioDTO.id;
                    i.fecha = DateTime.Now;
                }

                objSingle.evaluador = getUsuario().id;
                encuestasSubContratistasFactoryServices.getEncuestasSubContratistasFactoryServices().saveEncuestaResult(obj, objSingle, comentario);

                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetSubContratistas()
        {
            var result = new Dictionary<string, object>();
            try
            {

                result.Add(ITEMS, (List<ComboDTO>)Session["cboSubContratistas"]);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGraficaSubContratistaCC(DateTime fechaInicio, DateTime fechaFin, List<string> cc, int subContratista)
        {

            var result = new Dictionary<string, object>();
            try
            {
                // List<GraficaEvaluacionSubContratistaCCDTO> GetGraficaEvaluacionSubContratistaCC(DateTime fechaInicio, DateTime fechaFin, List<string> cc, int subContratista)
                var Data = encuestasSubContratistasFactoryServices.getEncuestasSubContratistasFactoryServices().GetGraficaEvaluacionSubContratistaCC(fechaInicio, fechaFin, cc, subContratista);
                result.Add("dtSubCC", Data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGraficaSubContratistaCCEstrellas(DateTime fechaInicio, DateTime fechaFin, List<string> cc, int subContratista)
        {

            var result = new Dictionary<string, object>();
            try
            {
                var Data = encuestasSubContratistasFactoryServices.getEncuestasSubContratistasFactoryServices().GetGraficaSubContratistaCCEstrellas(fechaInicio, fechaFin, cc, subContratista);
                result.Add("dtSubCC", Data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EstadisticasEvaluacionSubContrtaos(DateTime fechaInicio, DateTime fechaFin, List<string> cc)
        {
            var result = new Dictionary<string, object>();
            //try
            //{

            Session["cboSubContratistas"] = null;
            List<string> listaCondicionados = new List<string>();
            List<decimal> listTotales = new List<decimal>();
            List<string> listaReclasificacion = new List<string>();
            DataTable datatable = encuestasSubContratistasFactoryServices.getEncuestasSubContratistasFactoryServices().GetMatrizEvaluacionSubContratista(fechaInicio, fechaFin, cc);

            Session["datatable"] = datatable;
            var Grafica = encuestasSubContratistasFactoryServices.getEncuestasSubContratistasFactoryServices().GetGraficaEvaluacionSubContratista(fechaInicio, fechaFin, cc);


            for (int i = 0; i < datatable.Columns.Count; i++)
            {
                if (i <= 1)
                {
                    listaCondicionados.Add("CLASIFICACIÓN INICIAL  (CONDICIONADO, SATISFACTORIO, PREFERIDO)");
                    listTotales.Add(0);
                    listaReclasificacion.Add("CONDICIONADO");
                }
                else
                {
                    listaCondicionados.Add("CONDICIONADO");

                    var calificacion = infoReturn(datatable, i);
                    listTotales.Add(calificacion);

                    if (calificacion >= 14 && calificacion <= 21)
                    {
                        listaReclasificacion.Add("PREFERIDO");
                    }
                    else if (calificacion >= 8 && calificacion <= 13)
                    {
                        listaReclasificacion.Add("SATISFACTORIO");
                    }
                    else if (calificacion >= 0 && calificacion <= 7)
                    {
                        listaReclasificacion.Add("CONDICIONADO");
                    }

                }


            }



            result.Add("listTotales", listTotales);
            result.Add("dtset", Grafica);
            result.Add("listaReclasificacion", listaReclasificacion);
            result.Add("listaCondicionados", listaCondicionados);
            result.Add("dtSetGrafica", DataTableToJsonObj(datatable));
            result.Add(SUCCESS, true);
            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EstadisticasEvaluacionSubContratosEstrellas(DateTime fechaInicio, DateTime fechaFin, List<string> cc)
        {
            var result = new Dictionary<string, object>();
            try
            {

                Session["cboSubContratistas"] = null;
            List<string> listaCondicionados = new List<string>();
            List<decimal> listTotales = new List<decimal>();
            List<string> listaReclasificacion = new List<string>();
            DataTable datatable = encuestasSubContratistasFactoryServices.getEncuestasSubContratistasFactoryServices().GetMatrizEvaluacionSubContratistaEstrellas(fechaInicio, fechaFin, cc);

            Session["datatable"] = datatable;
            var Grafica = encuestasSubContratistasFactoryServices.getEncuestasSubContratistasFactoryServices().GetGraficaEvaluacionSubContratistaEstrellas(fechaInicio, fechaFin, cc);


            for (int i = 0; i < datatable.Columns.Count; i++)
            {
                if (i <= 1)
                {
                    listaCondicionados.Add("CLASIFICACIÓN INICIAL  (PÈSIMO, MALO, REGULAR, ACEPTABLE, ESTUPENDO)");
                    listTotales.Add(0);
                    listaReclasificacion.Add("PÈSIMO");
                }
                else
                {

                    listaCondicionados.Add("PÈSIMO");

                    var calificacion = infoReturn(datatable, i);
                    int CALI = (int)infoReturn(datatable, i);
                    listTotales.Add(calificacion);

                    switch (CALI)
                    {
                        case 1:
                            listaReclasificacion.Add("PÈSIMO");
                            break;
                        case 2:
                            listaReclasificacion.Add("MALO");
                            break;
                        case 3:
                            listaReclasificacion.Add("REGULAR");
                            break;
                        case 4:
                            listaReclasificacion.Add("ACEPTABLE");
                            break;
                        case 5:
                            listaReclasificacion.Add("ESTUPENDO");
                            break;
                        default:
                            listaReclasificacion.Add("PÈSIMO");
                            break;

                    }
                }
            }

            result.Add("listTotales", listTotales);
            result.Add("dtset", Grafica);
            result.Add("listaReclasificacion", listaReclasificacion);
            result.Add("listaCondicionados", listaCondicionados);
            result.Add("dtSetGrafica", DataTableToJsonObj(datatable));
            result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private decimal infoReturn(DataTable datatable, int IndiceColumna)
        {
            decimal suma = 0;
            int Count = 0;
            foreach (DataRow dr in datatable.Rows)
            {
                suma += Convert.ToDecimal(dr[IndiceColumna]);

                if (Convert.ToDecimal(dr[IndiceColumna]) != 0)
                {
                    Count++;
                }
            }
            return suma != 0 ? suma / Count : 0;

        }

        public string DataTableToJsonObj(DataTable dt)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt);
            StringBuilder JsonString = new StringBuilder();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        if (j < ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\",");
                        }
                        else if (j == ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == ds.Tables[0].Rows.Count - 1)
                    {
                        JsonString.Append("}");
                    }
                    else
                    {
                        JsonString.Append("},");
                    }
                }
                JsonString.Append("]");
                return JsonString.ToString();
            }
            else
            {
                return null;
            }
        }

        public FileResult getFileDownload()
        {
            //   DataTable datatable = encuestasSubContratistasFactoryServices.getEncuestasSubContratistasFactoryServices().GetMatrizEvaluacionSubContratista(fechaInicio, fechaFin, cc);

            DataTable dt = (DataTable)Session["datatable"];
            WriteExcelWithNPOI(dt, "xlsx");

            return null;
        }

        public MemoryStream WriteExcelWithNPOI(DataTable dt, String extension)
        {

            IWorkbook workbook;

            if (extension == "xlsx")
            {
                workbook = new XSSFWorkbook();
            }
            else if (extension == "xls")
            {
                workbook = new HSSFWorkbook();
            }
            else
            {
                throw new Exception("This format is not supported");
            }

            ISheet sheet1 = workbook.CreateSheet("Sheet 1");

            //make a header row
            IRow row1 = sheet1.CreateRow(0);

            for (int j = 0; j < dt.Columns.Count; j++)
            {

                ICell cell = row1.CreateCell(j);
                String columnName = dt.Columns[j].ToString();
                cell.SetCellValue(columnName);
            }

            //loops through data
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet1.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    ICell cell = row.CreateCell(j);
                    String columnName = dt.Columns[j].ToString();
                    cell.SetCellValue(dt.Rows[i][columnName].ToString());
                }
            }

            using (var exportData = new MemoryStream())
            {
                Response.Clear();
                workbook.Write(exportData);

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "Evaluacion_SubContratistas" + ".xlsx"));
                Response.BinaryWrite(exportData.ToArray());

                Response.End();
                return exportData;
            }


        }
    }
}