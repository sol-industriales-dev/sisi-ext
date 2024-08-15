using Core.DAO.Proyecciones;
using Core.DTO.Principal.Generales;
using Core.DTO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using Data.Factory.Principal.Usuarios;
using Data.Factory.Proyecciones;
using Excel;
using Infrastructure.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Core.Enum.Administracion.Proyecciones;
using Core.Enum;

namespace SIGOPLAN.Controllers
{
    public class ProyeccionesController : BaseController
    {

        #region Factory
        TerminacionObraFactoryServices terminacionObraFactoryServices;
        ComentariosObraFactoryServices comentariosObraFactoryServices;
        CxCFactoryServices cxCFactoryServices;
        ObraFactoryServices obraFactoryServices;
        CatAreaFactoryServices catAreaFactoryServices;
        CapturadeObrasFactoryServices capturadeObrasFactoryServices;
        ActivoFijoFactoryService activoFijoFactoryService;
        SaldosInicialesFactoryServices saldosInicialesFactoryServices;
        CatResponsableFactoryServices catResponsableFactoryServices;
        UsuarioFactoryServices usuarioFactoryServices;
        PremisasFactoryServices premisasFactoryServices;
        AdministracionFactoryService administracionFactoryService;
        PagosDiversosFactoryService pagosDiversosFactoryService;
        CobrosDiversosFactoryService cobrosDiversosFactoryService;
        EscenariosFactoryServices escenariosFactoryServices;
        CapCifrasPrincipalesFactoryServices capCifrasPrincipalesFactoryServices;

        #endregion
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            capCifrasPrincipalesFactoryServices = new CapCifrasPrincipalesFactoryServices();
            escenariosFactoryServices = new EscenariosFactoryServices();
            terminacionObraFactoryServices = new TerminacionObraFactoryServices();
            comentariosObraFactoryServices = new ComentariosObraFactoryServices();
            usuarioFactoryServices = new UsuarioFactoryServices();
            catResponsableFactoryServices = new CatResponsableFactoryServices();
            saldosInicialesFactoryServices = new SaldosInicialesFactoryServices();
            catAreaFactoryServices = new CatAreaFactoryServices();
            obraFactoryServices = new ObraFactoryServices();
            capturadeObrasFactoryServices = new CapturadeObrasFactoryServices();
            activoFijoFactoryService = new ActivoFijoFactoryService();
            cxCFactoryServices = new CxCFactoryServices();
            premisasFactoryServices = new PremisasFactoryServices();
            administracionFactoryService = new AdministracionFactoryService();
            pagosDiversosFactoryService = new PagosDiversosFactoryService();
            cobrosDiversosFactoryService = new CobrosDiversosFactoryService();
            base.OnActionExecuting(filterContext);
        }


        public ActionResult LoadCargaArchivos()
        {
            return PartialView("_CargarArchivos");
        }
        public ActionResult CifrasPrincipales()
        {
            return View();
        }
        // GET: Proyecciones
        public ActionResult PantallaPrincipal()
        {
            return View();
        }
        public ActionResult CxC()
        {
            return View();
        }
        public ActionResult CapturaDeObras()
        {
            return View();
        }
        public ActionResult GastosAdministacionyVentas()
        {
            //   return View();
            return PartialView();
        }
        public ActionResult EstadoPosicionFinanciera()
        {
            return View();
        }

        public ActionResult IngresosDiversos()
        {
            return View();
        }

        public ActionResult Premisas()
        {
            return View();
        }
        public ActionResult PagosDiversos()
        {
            return View();
        }
        public ActionResult ActivoFijo()
        {
            return View();
        }
        public ActionResult AltaCifrasPrincipales()
        {
            return View();
        }
        public ActionResult CapturaEscenarios()
        {
            return View();
        }

        public ActionResult fillCboEscenariosPadre()
        {
            var result = new Dictionary<string, object>();
            try
            {

                var Res = escenariosFactoryServices.getEscenariosFactoryServices().GetListaEscenariosPrincipales();
                result.Add(ITEMS, Res.Select(x => new { Value = x.id, Text = x.descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult fillCboEscenarios(int tipo)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var Res = escenariosFactoryServices.getEscenariosFactoryServices().GetListaEscenarios();
                List<ComboDTO> cboSend = new List<ComboDTO>();
                foreach (var item in Res.OrderBy(x => x.nivel).ThenBy(x => x.ordenID))
                {
                    ComboDTO dato = new ComboDTO();

                    if (tipo == 1)
                    {
                        dato.Value = item.id.ToString();
                    }
                    else
                    {
                        dato.Value = item.descripcion;
                    }

                    string espacio = "";
                    if (item.PadreID != 0)
                    {
                        espacio = "&nbsp;&nbsp;&nbsp;";
                    }

                    dato.Text = espacio + item.descripcion;
                    dato.Prefijo = item.descripcion;
                    cboSend.Add(dato);
                }

                result.Add(ITEMS, cboSend);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult fillTblEscenarios(int escenario, string descripcion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var Res = escenariosFactoryServices.getEscenariosFactoryServices().GetListEscenariosTable(escenario, descripcion);
                List<CatEscenariosDTO> lts = new List<CatEscenariosDTO>();

                foreach (var item in Res)
                {
                    CatEscenariosDTO catEscenariosDTO = new CatEscenariosDTO();

                    catEscenariosDTO.id = item.id;
                    if (item.PadreID != 0)
                    {
                        catEscenariosDTO.Padre = escenariosFactoryServices.getEscenariosFactoryServices().CatEscenarioByID(item.PadreID).descripcion;
                        catEscenariosDTO.Hijo = item.descripcion;
                    }
                    else
                    {
                        catEscenariosDTO.Padre = item.descripcion;
                    }

                    catEscenariosDTO.estatus = item.estatus;

                    lts.Add(catEscenariosDTO);
                }
                result.Add("tblEscenariosData", lts);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult loadDataEscenario(int idObj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var Res = escenariosFactoryServices.getEscenariosFactoryServices().CatEscenarioByID(idObj);

                if (Res != null)
                {
                    result.Add(SUCCESS, true);
                    result.Add("obj", Res);
                }
                else
                {
                    result.Add(SUCCESS, false);
                }

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult guardarEscenario(tblPro_CatEscenarios obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                int ContadorNext = 0;
                if (obj.id == 0)
                {
                    if (obj.PadreID != 0)
                    {
                        var lstEscenariosHijo = escenariosFactoryServices.getEscenariosFactoryServices().GetListaEscenarios().Where(x => x.nivel == obj.PadreID).ToList();

                        ContadorNext = lstEscenariosHijo.Count + 1;

                        obj.ordenID = ContadorNext;
                        obj.nivel = obj.PadreID;
                    }
                    else
                    {
                        obj.ordenID = 1;

                    }
                }
                else
                {
                    var objTemp = escenariosFactoryServices.getEscenariosFactoryServices().CatEscenarioByID(obj.id);

                    obj.nivel = objTemp.nivel;
                    obj.ordenID = objTemp.ordenID;
                    obj.estatus = objTemp.estatus;
                }
                escenariosFactoryServices.getEscenariosFactoryServices().Guardar(obj);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult setTipoDocumento(int id, int mes, int anio, int idGlobal)
        {
            var result = new Dictionary<string, object>();
            try
            {
                Session["tipoDocumento"] = "";
                Session["idGlobal"] = "";
                Session["tipoDocumento"] = id;
                Session["FechaMes"] = mes;
                Session["FechaAnio"] = anio;
                Session["idGlobal"] = idGlobal;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarAchivoExcel(HttpPostedFileBase files1)
        {
            string fName = "";
            DataSet dtsExcel = null;
            int tipoDocumento = Convert.ToInt32(Session["tipoDocumento"]);
            var result = new Dictionary<string, object>();
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {

                        result.Add(MESSAGE, "El archvivo se importó correctamente");
                        result.Add(SUCCESS, true);
                    }
                    byte[] bytesExcel = null;
                    using (var binaryReader = new BinaryReader(file.InputStream))
                    {
                        bytesExcel = binaryReader.ReadBytes(file.ContentLength);
                    }
                    Stream msExcel = new MemoryStream(bytesExcel);
                    string extension = Path.GetExtension(file.FileName);

                    if (extension == ".xls" || extension == ".XLS")
                    {
                        try
                        {
                            IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(msExcel);

                            excelReader.IsFirstRowAsColumnNames = true;
                            dtsExcel = excelReader.AsDataSet();
                            excelReader.Close();
                        }
                        catch (Exception e)
                        {
                            result.Add(MESSAGE, "El archvivo se importó correctamente");

                        }
                    }
                    else
                    {
                        if (extension == ".xlsx" || extension == ".XLSX")
                        {


                            try
                            {
                                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(msExcel);

                                excelReader.IsFirstRowAsColumnNames = true;
                                dtsExcel = excelReader.AsDataSet();
                                excelReader.Close();
                            }
                            catch (Exception)
                            {

                                // context.Response.Write("errTipoArchivo");
                            }
                        }
                        else
                        {
                            // context.Response.Write("errTipoArchivo");
                            break;
                        }
                    }

                    try
                    {
                        for (int i = dtsExcel.Tables[0].Rows.Count - 1; i >= 0; i--)
                        {
                            if (string.IsNullOrEmpty(dtsExcel.Tables[0].Rows[i][0].ToString()))
                                dtsExcel.Tables[0].Rows[i].Delete();
                        }
                        dtsExcel.Tables[0].AcceptChanges();

                        if (dtsExcel.Tables[0].Rows.Count > 0)
                        {
                            switch (tipoDocumento)
                            {
                                case 1:
                                    {
                                        int idCount = 1;
                                        var ArchivoCxC = (IList<AchivoCxCDTO>)SetEntidad(dtsExcel).ToObject<IList<AchivoCxCDTO>>();

                                        var objeto = ArchivoCxC.Select(x => new CxCDTO
                                        {
                                            id = idCount++,
                                            Obra = x.Obra,
                                            Probabilidad = x.Probabilidad,
                                            CostoYaAplicado = x.Costo,
                                            ImporteCxC = x.Importe,
                                            MESV1 = x.Mes1,
                                            MESV2 = x.Mes2,
                                            MESV3 = x.Mes3,
                                            MESV4 = x.Mes4,
                                            MESV5 = x.Mes5,
                                            MESV6 = x.Mes6,
                                            MESV7 = x.Mes7,
                                            MESV8 = x.Mes8,
                                            MESV9 = x.Mes9,
                                            MESV10 = x.Mes10,
                                            MESV11 = x.Mes11,
                                            MESV12 = x.Mes12,
                                            MESP1 = OperacionCxCMulti(x.Importe, x.Mes1),
                                            MESP2 = OperacionCxCMulti(x.Importe, x.Mes2),
                                            MESP3 = OperacionCxCMulti(x.Importe, x.Mes3),
                                            MESP4 = OperacionCxCMulti(x.Importe, x.Mes4),
                                            MESP5 = OperacionCxCMulti(x.Importe, x.Mes5),
                                            MESP6 = OperacionCxCMulti(x.Importe, x.Mes6),
                                            MESP7 = OperacionCxCMulti(x.Importe, x.Mes7),
                                            MESP8 = OperacionCxCMulti(x.Importe, x.Mes8),
                                            MESP9 = OperacionCxCMulti(x.Importe, x.Mes9),
                                            MESP10 = OperacionCxCMulti(x.Importe, x.Mes10),
                                            MESP11 = OperacionCxCMulti(x.Importe, x.Mes11),
                                            MESP12 = OperacionCxCMulti(x.Importe, x.Mes12),
                                            TotalRecuperacion = OperacionCxCTotal(x),
                                            CxCMargen = OperacionCxCMulti(x.Importe, x.Costo),
                                            CostoXAplicar = OperacionCxCTotal(x) - OperacionCxCMulti(x.Importe, x.Costo),
                                            MES1AC = 0,
                                            MES2AC = 0,
                                            MES3AC = 0,
                                            MES4AC = 0,
                                            TotalAC = 0
                                        });

                                        tblPro_CxC objEntidad = new tblPro_CxC();
                                        var jsonSerialiser = new JavaScriptSerializer();
                                        var json = jsonSerialiser.Serialize(objeto);

                                        int id = Convert.ToInt32(Session["idGlobal"]);
                                        objEntidad.id = id;
                                        objEntidad.CadenaJson = json.ToString();
                                        int mes = Convert.ToInt32(Session["FechaMes"]);
                                        int anio = Convert.ToInt32(Session["FechaAnio"]);

                                        objEntidad.Mes = mes;
                                        objEntidad.Anio = anio;
                                        objEntidad.Estatus = true;
                                        cxCFactoryServices.GetCxC().Guardar(objEntidad);

                                        break;
                                    }
                                case 2:
                                    {
                                        int idCount = 1;
                                        var ArchivoCxC = (IList<AchivoPDDTO>)SetEntidad(dtsExcel).ToObject<IList<AchivoPDDTO>>();

                                        var objeto = ArchivoCxC.Select(x => new MesDTO
                                        {
                                            Concepto = x.Concepto,
                                            Mes1 = x.Mes1,
                                            Mes2 = x.Mes2,
                                            Mes3 = x.Mes3,
                                            Mes4 = x.Mes4,
                                            Mes5 = x.Mes5,
                                            Mes6 = x.Mes6,
                                            Mes7 = x.Mes7,
                                            Mes8 = x.Mes8,
                                            Mes9 = x.Mes9,
                                            Mes10 = x.Mes10,
                                            Mes11 = x.Mes11,
                                            Mes12 = x.Mes12,
                                            MesT = x.Mes1 + x.Mes2 + x.Mes3 + x.Mes4 + x.Mes5 + x.Mes6 + x.Mes7 + x.Mes8 + x.Mes9 + x.Mes10 + x.Mes11 + x.Mes12
                                        });

                                        tblPro_PagosDiversos objEntidad = new tblPro_PagosDiversos();

                                        var ot = new PagosDivDTO();
                                        ot.DesgloseVariosPagos = new List<MesDTO>();
                                        ot.DesgloseVariosPagos.AddRange(objeto);
                                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(ot);
                                        objEntidad.CadenaJson = json;
                                        int mes = Convert.ToInt32(Session["FechaMes"]);
                                        int anio = Convert.ToInt32(Session["FechaAnio"]);

                                        objEntidad.Mes = mes;
                                        objEntidad.Anio = anio;
                                        objEntidad.Estatus = true;
                                        pagosDiversosFactoryService.GetPagosDiversos().GuardarFormExcel(objEntidad);

                                        break;
                                    }
                                default:
                                    break;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                        result.Add(MESSAGE, error);
                    }
                }

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        private int OperacionCxCTotal(AchivoCxCDTO x)
        {
            decimal result = OperacionCxCMulti(x.Importe, x.Mes1) +
                             OperacionCxCMulti(x.Importe, x.Mes2) +
                             OperacionCxCMulti(x.Importe, x.Mes3) +
                             OperacionCxCMulti(x.Importe, x.Mes4) +
                             OperacionCxCMulti(x.Importe, x.Mes5) +
                             OperacionCxCMulti(x.Importe, x.Mes6) +
                             OperacionCxCMulti(x.Importe, x.Mes7) +
                             OperacionCxCMulti(x.Importe, x.Mes8) +
                             OperacionCxCMulti(x.Importe, x.Mes9) +
                             OperacionCxCMulti(x.Importe, x.Mes10) +
                             OperacionCxCMulti(x.Importe, x.Mes11) +
                             OperacionCxCMulti(x.Importe, x.Mes12);

            return Convert.ToInt32(Math.Round(result));
        }

        private int OperacionCxCMulti(decimal importe, decimal probabilidad)
        {
            var temp = (probabilidad / 100) * importe;

            int result = Convert.ToInt32(Math.Round(temp));

            return result;
        }

        private static dynamic SetEntidad(DataSet datosExcel)
        {
            string jsonResult = "";

            foreach (DataRow dtRow in datosExcel.Tables[0].Rows)
            {
                if (dtRow["Mes1"].ToString() == "")
                {
                    dtRow["Mes1"] = 0;
                }
                if (dtRow["Mes2"].ToString() == "")
                {
                    dtRow["Mes2"] = 0;
                }
                if (dtRow["Mes3"].ToString() == "")
                {
                    dtRow["Mes3"] = 0;
                }
                if (dtRow["Mes4"].ToString() == "")
                {
                    dtRow["Mes4"] = 0;
                }
                if (dtRow["Mes5"].ToString() == "")
                {
                    dtRow["Mes5"] = 0;
                }
                if (dtRow["Mes6"].ToString() == "")
                {
                    dtRow["Mes6"] = 0;
                }
                if (dtRow["Mes7"].ToString() == "")
                {
                    dtRow["Mes7"] = 0;
                }
                if (dtRow["Mes8"].ToString() == "")
                {
                    dtRow["Mes8"] = 0;
                }
                if (dtRow["Mes9"].ToString() == "")
                {
                    dtRow["Mes9"] = 0;
                }
                if (dtRow["Mes10"].ToString() == "")
                {
                    dtRow["Mes10"] = 0;
                }
                if (dtRow["Mes10"].ToString() == "")
                {
                    dtRow["Mes10"] = 0;
                }
                if (dtRow["Mes11"].ToString() == "")
                {
                    dtRow["Mes11"] = 0;
                }
                if (dtRow["Mes12"].ToString() == "")
                {
                    dtRow["Mes12"] = 0;
                }
            }

            if (datosExcel.Tables[0].Rows.Count > 0)
            {

                jsonResult = JsonConvert.SerializeObject(datosExcel.Tables[0]);
                return JsonConvert.DeserializeObject<dynamic>(jsonResult);
            }
            return null;
        }

        public ActionResult LoadObra(int Escenario)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = obraFactoryServices.getObraServices().getObras(Escenario);

                result.Add(SUCCESS, true);
                result.Add("GetData", res.Select(x => new
                {
                    Area = x.Area,
                    Codigo = x.Codigo,
                    Descripcion = x.Descripcion,
                    Probabilidad = "<div class='input-group'>" +
                                     "<input type='number' class='form-control tbProbabilidad' value=" + (100 * x.Probabilidad).ToString("N0") + " max='100'>" +
                                        "<span class='input-group-addon'>%</span>" +
                                    "</div>",
                    Margen = "<div class='input-group'>" +
                                    "<input type='number' class='form-control tbMargen' value=" + (100 * x.Margen).ToString("N0") + " max='100'>" +
                                   "<span class='input-group-addon'>%</span>" +
                               "</div>",
                    Monto = "<div class='input-group'>" +
                                 "<span class='input-group-addon'>$</span>" +
                            "<input type='Text' class='form-control tbMonto' value=" + String.Format("{0:n}", x.Monto) + ">" +

                           "</div>",
                    MES1 = "<div class='input-group'>" +
                                   "<input type='number' class='form-control mes' value=" + (100 * x.Fecha1).ToString("N0") + " max='100'>" +
                               "<span class='input-group-addon'>%</span>" +
                           "</div>",
                    MES2 = "<div class='input-group'>" +
                                  "<input type='number' class='form-control mes' value=" + (100 * x.Fecha2).ToString("N0") + " max='100'>" +
                              "<span class='input-group-addon'>%</span>" +
                          "</div>",
                    MES3 = "<div class='input-group'>" +
                                   "<input type='number' class='form-control mes' value=" + (100 * x.Fecha3).ToString("N0") + " max='100'>" +
                               "<span class='input-group-addon'>%</span>" +
                           "</div>",
                    MES4 = "<div class='input-group'>" +
                                  "<input type='number' class='form-control mes' value=" + (100 * x.Fecha4).ToString("N0") + " max='100'>" +
                              "<span class='input-group-addon'>%</span>" +
                          "</div>",
                    MES5 = "<div class='input-group'>" +
                           "<input type='number' class='form-control mes' value=" + (100 * x.Fecha5).ToString("N0") + " max='100'>" +
                           "<span class='input-group-addon'>%</span>" +
                           "</div>",
                    MES6 = "<div class='input-group'>" +
                                  "<input type='number' class='form-control mes' value=" + (100 * x.Fecha6).ToString("N0") + " max='100'>" +
                              "<span class='input-group-addon'>%</span>" +
                          "</div>",
                    MES7 = "<div class='input-group'>" +
                                  "<input type='number' class='form-control mes' value=" + (100 * x.Fecha7).ToString("N0") + " max='100'>" +
                              "<span class='input-group-addon'>%</span>" +
                          "</div>",
                    MES8 = "<div class='input-group'>" +
                                   "<input type='number' class='form-control mes' value=" + (100 * x.Fecha8).ToString("N0") + " max='100'>" +
                               "<span class='input-group-addon'>%</span>" +
                           "</div>",
                    MES9 = "<div class='input-group'>" +
                                  "<input type='number' class='form-control mes' value=" + (100 * x.Fecha9).ToString("N0") + " max='100'>" +
                              "<span class='input-group-addon'>%</span>" +
                          "</div>",
                    MES10 = "<div class='input-group'>" +
                                  "<input type='number' class='form-control mes' value=" + (100 * x.Fecha10).ToString("N0") + " max='100'>" +
                              "<span class='input-group-addon'>%</span>" +
                          "</div>",
                    MES11 = "<div class='input-group'>" +
                                   "<input type='number' class='form-control mes' value=" + (100 * x.Fecha11).ToString("N0") + " max='100'>" +
                               "<span class='input-group-addon'>%</span>" +
                           "</div>",
                    MES12 = "<div class='input-group'>" +
                                 "<input type='number' class='form-control mes' value=" + (100 * x.Fecha12).ToString("N0") + " max='100'>" +
                                 "<span class='input-group-addon'>%</span>" +
                             "</div>",
                    Total = "<label class='lblTotal'>" + ((x.Fecha1 + x.Fecha2 + x.Fecha3 + x.Fecha4 + x.Fecha5 + x.Fecha6 + x.Fecha7 + x.Fecha8 + x.Fecha9 + x.Fecha10 + x.Fecha11 + x.Fecha12) * 100).ToString("N0") + "%</label>"


                }));
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFillTableCxC(int escenario, int meses, int anio)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = cxCFactoryServices.GetCxC().GetJsonData(escenario, meses, anio);
                int id = 0;
                int idRow = 0;
                int estatus = 0;
                if (res != null)
                {
                    id = res.id;

                    var obj = JsonConvert.DeserializeObject<List<CxCDTO>>(res.CadenaJson);

                    var diff = meses - cxCFactoryServices.GetCxC().getUltimoMesCapturado();

                    if (diff == 0 || diff == 1)
                    {
                        estatus = 1;
                    }
                    else
                    {
                        estatus = 2;
                    }

                    if (meses != res.Mes)
                    {
                        id = 0;
                    }

                    int idCount = 1;
                    result.Add("GetData", obj.Select(x => new
                    {
                        accion = "<div> <button data-idRow='" + (idCount) + "' class='btn btn-warning editRow' type='button' style='margin: 2px;'> " +
                                "<span class='glyphicon glyphicon-edit'></span></button>" +
                                "<button class='btn btn-danger removeRow'  data-idRow='" + (idRow) + "' type='button'> " +
                                "<span class='glyphicon glyphicon-remove'></span></button> " +
                                "<button class='btn btn-primary vistaRowPor hide' data-idrow='" + idRow + "' type='button'> " +
                                "<span class='glyphicon glyphicon-eye-open'></span></button>" +
                                " </div>",
                        id = idCount++,
                        Obra = x.Obra,
                        Probabilidad = x.Probabilidad,
                        CostoYaAplicado = x.CostoYaAplicado,
                        ImporteCxC = x.ImporteCxC,
                        MESV1 = x.MESV1,
                        MESV2 = x.MESV2,
                        MESV3 = x.MESV3,
                        MESV4 = x.MESV4,
                        MESV5 = x.MESV5,
                        MESV6 = x.MESV6,
                        MESV7 = x.MESV7,
                        MESV8 = x.MESV8,
                        MESV9 = x.MESV9,
                        MESV10 = x.MESV10,
                        MESV11 = x.MESV11,
                        MESV12 = x.MESV12,
                        MESP1 = x.MESP1,
                        MESP2 = x.MESP2,
                        MESP3 = x.MESP3,
                        MESP4 = x.MESP4,
                        MESP5 = x.MESP5,
                        MESP6 = x.MESP6,
                        MESP7 = x.MESP7,
                        MESP8 = x.MESP8,
                        MESP9 = x.MESP9,
                        MESP10 = x.MESP10,
                        MESP11 = x.MESP11,
                        MESP12 = x.MESP12,
                        TotalRecuperacion = x.TotalRecuperacion,
                        CxCMargen = x.CxCMargen,
                        CostoXAplicar = x.CostoXAplicar,
                        MES1PORAC = x.MES1PORAC,
                        MES2PORAC = x.MES2PORAC,
                        MES3PORAC = x.MES3PORAC,
                        MES4PORAC = x.MES4PORAC,
                        MES5PORAC = x.MES5PORAC,
                        MES6PORAC = x.MES6PORAC,
                        MES7PORAC = x.MES7PORAC,
                        MES8PORAC = x.MES8PORAC,
                        MES9PORAC = x.MES9PORAC,
                        MES10PORAC = x.MES10PORAC,
                        MES11PORAC = x.MES11PORAC,
                        MES12PORAC = x.MES12PORAC,
                        MES1AC = x.MES1AC,
                        MES2AC = x.MES2AC,
                        MES3AC = x.MES3AC,
                        MES4AC = x.MES4AC,
                        MES5AC = x.MES5AC,
                        MES6AC = x.MES6AC,
                        MES7AC = x.MES7AC,
                        MES8AC = x.MES8AC,
                        MES9AC = x.MES9AC,
                        MES10AC = x.MES10AC,
                        MES11AC = x.MES11AC,
                        MES12AC = x.MES12AC,
                        TotalAC = x.TotalAC,

                    }));
                }
                result.Add("idRow", idRow);
                result.Add("id", id);

                result.Add("EstadoRegreso", estatus);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFillTableEPF(int escenario, int meses, int anio)
        {
            var result = new Dictionary<string, object>();
            try
            {
                int estatus = 0;
                var res = saldosInicialesFactoryServices.GetSaldosIniciales().GetJsonData(meses, anio, 0);
                int id = 0;
                int idRow = 0;
                if (res != null)
                {
                    id = res.id;
                    estatus = 1;
                    var obj = JsonConvert.DeserializeObject<List<EPFSaldoInicialDTO>>(res.CadenaJson);

                    result.Add("GetData", obj.Select(x => new
                    {
                        Concepto = x.Concepto,
                        Inicial = x.Inicial,
                        Grupo = x.Grupo,
                        D1 = x.D1,
                        D2 = x.D2,
                        D3 = x.D3,
                        H1 = x.H1,
                        H2 = x.H2,
                        H3 = x.H3,
                        Saldo = x.Saldo
                    }));

                    var diff = meses - saldosInicialesFactoryServices.GetSaldosIniciales().getUltimoMesCapturado();
                    if (diff == 0 || diff == 1)
                    {
                        estatus = 1;
                    }
                    else
                    {
                        estatus = 2;
                    }

                    if (meses != res.Mes)
                    {
                        id = 0;
                    }
                    result.Add("idRow", idRow);
                    result.Add("id", id);


                }

                result.Add("EstadoRegreso", estatus);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private string GetInput(string Classe, object valor, int tipo)
        {
            switch (tipo)
            {
                case 1: // Porcentajes 
                    {
                        return "<div class='input-group'>" +
                                "<input type='number' class='form-control " + Classe + "' value='" + valor + "' max='100'>" +
                            "<span class='input-group-addon'>%</span>" +
                        "</div>";
                    }
                case 2://Totales E importes
                    {
                        return "<div class='input-group'>" +
                             "<span class='input-group-addon'>$</span>" +
                                  "<input type='text' class='form-control " + Classe + " DecimalSet' value='" + valor + "'>" +
                          "</div>";
                    }
                case 3://Descripciones
                    {
                        return "<div class='input-group'>" +

                                  "<input type='text' class='form-control " + Classe + "' value='" + valor + "' >" +
                            "<span class='input-group-addon hide'>" +
                            "<button class='btn-primary btn-xs form-control CEditBtn' style='height: 20px;'><span class='glyphicon glyphicon-pencil'></span></button>" +
                           " </span>" +
                          "</div>";
                    }
            }
            return "";
        }

        public ActionResult GuardarNuevoRegistro(tblPro_Obras obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                obj.Tipo = 1;
                obj.Codigo = "0";
                obraFactoryServices.getObraServices().GuardarRegistros(obj);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevaArea(string obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                tblPro_CatAreas obj1 = new tblPro_CatAreas();

                obj1.descripcion = obj;
                catAreaFactoryServices.getAreasServices().Guardar(obj1);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCboAreas()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = catAreaFactoryServices.getAreasServices().FillCboArea();
                result.Add(ITEMS, res.Select(x => new { Value = x.id, Text = x.descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCboPrioridades()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var enums = Enum.GetNames(typeof(PrioridadObraEnum)).Length;
                var items = new List<ComboDTO>();
                for (int i = 1; i <= enums; i++) 
                {
                    var aux = new ComboDTO();
                    aux.Value = i.ToString();
                    aux.Text = EnumHelper.GetDescription((PrioridadObraEnum)i);
                    items.Add(aux);
                }
                result.Add(ITEMS, items);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCboObras()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = capturadeObrasFactoryServices.GetCapturaObras().FillCboObra();
                var obj = JsonConvert.DeserializeObject<List<tblPro_Obras>>(res[0].CadenaJson);

                result.Add(ITEMS, obj.Select(x => new { Value = x.id, Text = x.Descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarTablaDatos(List<tblPro_Obras> obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                obraFactoryServices.getObraServices().GuardarActualizarRegistroMensual(obj);
                // var res = catAreaFactoryServices.getAreasServices().FillCboArea();
                //result.Add(ITEMS, res.Select(x => new { Value = x.id, Text = x.descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFillTable(int escenario, int meses, int anio)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = capturadeObrasFactoryServices.GetCapturaObras().GetJsonData(escenario, meses, anio);
                int id = 0;
                int idRow = 0;
                int estatus = 0;
                if (res != null)
                {
                    id = res.id;

                    var obj = JsonConvert.DeserializeObject<List<tblPro_Obras>>(res.CadenaJson);
                    var objId = obj.OrderByDescending(x => x.id).FirstOrDefault().id;


                    var SendData = capturadeObrasFactoryServices.GetCapturaObras().dataEscenarios(obj, escenario);
                    idRow = 1;

                    var diff = meses - capturadeObrasFactoryServices.GetCapturaObras().getUltimoMesCapturado();
                    if (diff == 0 || diff == 1)
                    {
                        estatus = 1;
                    }
                    else
                    {
                        estatus = 2;
                    }

                    if (meses != res.MesInicio)
                    {
                        id = 0;
                    }
                    var responsables = catResponsableFactoryServices.getCatResponsable().fillCboResponsables();

                    result.Add("GetData", obj.Where(x => x.estatus != 1).Select(x => new
                    {
                        id = idRow++,
                        accion = "<div> <button data-idRow='" + (idRow) + "' class='btn btn-sm btn-warning editRow ' type='button'> " +
                                 "<span class='glyphicon glyphicon-edit'></span></button>" +
                                 "<button class='btn btn-sm btn-danger removeRow'  data-idRow='" + (idRow) + "' type='button'> " +
                                 "<span class='glyphicon glyphicon-remove'></span></button> " +
                                 "<button class='btn btn-sm  btn-primary vistaRowPor' data-idrow='" + idRow + "' type='button'> " +
                                 "<span class='glyphicon glyphicon-eye-open'></span></button>" +
                                   "<button class='btn btn-sm btn-primary msgRow' data-idrow='" + idRow + "' type='button'> " +
                                 "<span class='glyphicon glyphicon-envelope'></span></button>" +
                                 (x.Escenario == "A" ? "<button class='btn btn-sm btn-primary finishRow' data-idrow='" + idRow + "' type='button'> " +
                                 "<span class='glyphicon glyphicon-ok-sign'></span></button>" : "")
                                 +
                                " </div>",
                        Escenario = x.Escenario,
                        Area = x.Area,
                        Prioridad = x.Prioridad == null ? 4 : x.Prioridad,
                        Codigo = x.Codigo,
                        Descripcion = x.Descripcion,
                        Probabilidad = x.Probabilidad,
                        Margen = x.Margen,
                        Monto = x.Monto,

                        //Probabilidad = "<div class='input-group'>" +
                        //                 "<input type='number' class='form-control tbProbabilidad' value=" + x.Probabilidad.ToString("N0") + " max='100'>" +
                        //                    "<span class='input-group-addon'>%</span>" +
                        //                "</div>",
                        //Margen = "<div class='input-group'>" +
                        //                "<input type='number' class='form-control tbMargen' value=" + x.Margen.ToString("N0") + " max='100'>" +
                        //               "<span class='input-group-addon'>%</span>" +
                        //           "</div>",
                        //Monto = "<div class='input-group'>" +
                        //             "<span class='input-group-addon'>$</span>" +
                        //        "<input type='Text' class='form-control tbMonto' value=" + String.Format("{0:n}", x.Monto) + ">" +

                        //       "</div>",
                        ////MES1 = "<div class='input-group'>" +
                        //               "<input type='number' class='form-control mes' value=" + x.Fecha1.ToString("N0") + " max='100'>" +
                        //           "<span class='input-group-addon'>%</span>" +
                        //       "</div>",
                        //MES2 = "<div class='input-group'>" +
                        //              "<input type='number' class='form-control mes' value=" + x.Fecha2.ToString("N0") + " max='100'>" +
                        //          "<span class='input-group-addon'>%</span>" +
                        //      "</div>",
                        //MES3 = "<div class='input-group'>" +
                        //               "<input type='number' class='form-control mes' value=" + x.Fecha3.ToString("N0") + " max='100'>" +
                        //           "<span class='input-group-addon'>%</span>" +
                        //       "</div>",
                        //MES4 = "<div class='input-group'>" +
                        //              "<input type='number' class='form-control mes' value=" + x.Fecha4.ToString("N0") + " max='100'>" +
                        //          "<span class='input-group-addon'>%</span>" +
                        //      "</div>",
                        //MES5 = "<div class='input-group'>" +
                        //       "<input type='number' class='form-control mes' value=" + x.Fecha5.ToString("N0") + " max='100'>" +
                        //       "<span class='input-group-addon'>%</span>" +
                        //       "</div>",
                        //MES6 = "<div class='input-group'>" +
                        //              "<input type='number' class='form-control mes' value=" + x.Fecha6.ToString("N0") + " max='100'>" +
                        //          "<span class='input-group-addon'>%</span>" +
                        //      "</div>",
                        //MES7 = "<div class='input-group'>" +
                        //              "<input type='number' class='form-control mes' value=" + x.Fecha7.ToString("N0") + " max='100'>" +
                        //          "<span class='input-group-addon'>%</span>" +
                        //      "</div>",
                        //MES8 = "<div class='input-group'>" +
                        //               "<input type='number' class='form-control mes' value=" + x.Fecha8.ToString("N0") + " max='100'>" +
                        //           "<span class='input-group-addon'>%</span>" +
                        //       "</div>",
                        //MES9 = "<div class='input-group'>" +
                        //              "<input type='number' class='form-control mes' value=" + x.Fecha9.ToString("N0") + " max='100'>" +
                        //          "<span class='input-group-addon'>%</span>" +
                        //      "</div>",
                        //MES10 = "<div class='input-group'>" +
                        //              "<input type='number' class='form-control mes' value=" + x.Fecha10.ToString("N0") + " max='100'>" +
                        //          "<span class='input-group-addon'>%</span>" +
                        //      "</div>",
                        //MES11 = "<div class='input-group'>" +
                        //               "<input type='number' class='form-control mes' value=" + x.Fecha11.ToString("N0") + " max='100'>" +
                        //           "<span class='input-group-addon'>%</span>" +
                        //       "</div>",
                        //MES12 = "<div class='input-group'>" +
                        //             "<input type='number' class='form-control mes' value=" + x.Fecha12.ToString("N0") + " max='100'>" +
                        //             "<span class='input-group-addon'>%</span>" +
                        //         "</div>",
                        //Total = "<label class='lblTotal' data-idRow='" + (idRow++) + "'>" + (x.Fecha1 + x.Fecha2 + x.Fecha3 + x.Fecha4 + x.Fecha5 + x.Fecha6 + x.Fecha7 + x.Fecha8 + x.Fecha9 + x.Fecha10 + x.Fecha11 + x.Fecha12).ToString("N0") + "%</label>"


                        MES1 = x.Fecha1.ToString("N0"),
                        MES2 = x.Fecha2.ToString("N0"),
                        MES3 = x.Fecha3.ToString("N0"),
                        MES4 = x.Fecha4.ToString("N0"),
                        MES5 = x.Fecha5.ToString("N0"),
                        MES6 = x.Fecha6.ToString("N0"),
                        MES7 = x.Fecha7.ToString("N0"),
                        MES8 = x.Fecha8.ToString("N0"),
                        MES9 = x.Fecha9.ToString("N0"),
                        MES10 = x.Fecha10.ToString("N0"),
                        MES11 = x.Fecha11.ToString("N0"),
                        MES12 = x.Fecha12.ToString("N0"),
                        Total = (x.Fecha1 + x.Fecha2 + x.Fecha3 + x.Fecha4 + x.Fecha5 + x.Fecha6 + x.Fecha7 + x.Fecha8 + x.Fecha9 + x.Fecha10 + x.Fecha11 + x.Fecha12).ToString("N0") ///"<label class='lblTotal' data-idRow= + (idRow++) + >" + + "%</label>"
                        ,
                        descripcionObra = catAreaFactoryServices.getAreasServices().FillCboArea().FirstOrDefault(y => y.id.Equals(x.Area)).descripcion,
                        descripcionEmpleado = (x.Codigo != "0" ? GetColor(x.Codigo, responsables) : "0"),
                        banderaFinanciamiento = x.banderaFinanciamiento,
                        porcentaje = x.porcentaje,
                        Comentario = x.Comentario,
                        Abreviatura = GetAbreviatura(x.Codigo, responsables),
                        CentroCostos = string.IsNullOrEmpty(x.CentroCostos) ? "NA" : x.CentroCostos
                    }));
                }
                result.Add("idRow", idRow);
                result.Add("id", id);
                result.Add("EstadoRegreso", estatus);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInfoTerminacionObra(int id, int idRow)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var resultado = terminacionObraFactoryServices.getTerminacionObraFactoryServices().GetObrasCerradasByID(id, idRow);

                result.Add("DatosTerminacionObra", resultado);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetInfoTerminacionObra(tblPro_CierreObra obj)
        {
            var result = new Dictionary<string, object>();
            try
            {

                terminacionObraFactoryServices.getTerminacionObraFactoryServices().Guardar(obj);

                var res = capturadeObrasFactoryServices.GetCapturaObras().GetJsonDataID(obj.capturadeObrasID);


                if (res != null)
                {
                    var objJson = JsonConvert.DeserializeObject<List<tblPro_Obras>>(res.CadenaJson);

                    foreach (var item in objJson)
                    {
                        if (item.id == obj.registroID)
                        {
                            item.estatus = 1;
                        }

                    }

                    tblPro_CapturadeObras objEntidad = new tblPro_CapturadeObras();
                    var jsonSerialiser = new JavaScriptSerializer();
                    var json = jsonSerialiser.Serialize(objJson);
                    objEntidad.id = res.id;
                    objEntidad.CadenaJson = json.ToString();
                    objEntidad.EjercicioInicial = res.EjercicioInicial;
                    objEntidad.Escenario = res.Escenario;
                    objEntidad.MesInicio = res.MesInicio;


                    capturadeObrasFactoryServices.GetCapturaObras().GuardarActualizarCapturadeObras(objEntidad);
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

        public ActionResult SetComentariosObra(int id, int idRow)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var resultado = comentariosObraFactoryServices.getComentariosObraFactoryServices().GetListaComentarios(id, idRow);

                //result.Add("comentarios", resultado);


                result.Add("comentarios", resultado.Select(x => new
                {
                    id = x.id,
                    capturadeObrasID = x.capturadeObrasID,
                    registroID = x.registroID,
                    fecha = x.fecha.ToShortDateString(),
                    comentario = x.comentario,
                    usuarioNombre = x.usuarioNombre,
                    usuarioID = x.usuarioID,
                    estatusComentario = x.estatusComentario,
                    adjuntoNombre = x.adjuntoNombre

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

        public ActionResult GuardarComentario(tblPro_ComentariosObras obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuario = usuarioFactoryServices.getUsuarioService().ListUsersById(getUsuario().id).FirstOrDefault();
                obj.usuarioID = 0;
                obj.usuarioNombre = "";
                obj.fecha = DateTime.Now;

                if (usuario != null)
                {
                    obj.usuarioID = usuario.id;
                    obj.usuarioNombre = usuario.nombre + " " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno;
                }

                comentariosObraFactoryServices.getComentariosObraFactoryServices().GuardarComentario(obj);

                var data = comentariosObraFactoryServices.getComentariosObraFactoryServices().GetListaComentarios(obj.capturadeObrasID, obj.registroID);
                result.Add("data", data.Select(x => new
                {
                    id = x.id,
                    capturadeObrasID = x.capturadeObrasID,
                    registroID = x.registroID,
                    fecha = x.fecha.ToShortDateString(),
                    comentario = x.comentario,
                    usuarioNombre = x.usuarioNombre,
                    usuarioID = x.usuarioID,
                    estatusComentario = x.estatusComentario,


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

        private string GetAbreviatura(string codigo, List<tblPro_CatResponsables> responsable)
        {
            if (codigo != null)
            {
                var responsableDescr = responsable.Where(x => x.Color.ToString().Equals(codigo));
                if (responsable != null)
                {
                    return responsableDescr.FirstOrDefault().Abreviatura;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        private string GetColor(string codigo, List<tblPro_CatResponsables> responsable)
        {
            if (codigo != null)
            {
                var responsableDescr = responsable.Where(x => x.Color.ToString().Equals(codigo));
                if (responsable != null)
                {
                    return responsableDescr.FirstOrDefault().Descripcion;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }

        }

        public ActionResult GuardarInfoTabla(List<tblPro_Obras> obj, int obj1, int obj2, int obj3, int id)
        {
            var result = new Dictionary<string, object>();
            try
            {

                tblPro_CapturadeObras objEntidad = new tblPro_CapturadeObras();
                var jsonSerialiser = new JavaScriptSerializer();
                var json = jsonSerialiser.Serialize(obj);
                objEntidad.id = id;
                objEntidad.CadenaJson = json.ToString();

                objEntidad.Escenario = obj1;
                objEntidad.MesInicio = obj2;
                objEntidad.EjercicioInicial = obj3;

                capturadeObrasFactoryServices.GetCapturaObras().GuardarActualizarCapturadeObras(objEntidad);

                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarInfoEPF(List<EPFSaldoInicialDTO> obj, int mes, int anio, int id)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var diff = mes - saldosInicialesFactoryServices.GetSaldosIniciales().getUltimoMesCapturado();
                if (diff == 0 || diff == 1)
                {
                    tblPro_SaldosIniciales objEntidad = new tblPro_SaldosIniciales();
                    var jsonSerialiser = new JavaScriptSerializer();
                    var json = jsonSerialiser.Serialize(obj);
                    objEntidad.id = id;
                    objEntidad.CadenaJson = json.ToString();

                    objEntidad.Mes = mes;
                    objEntidad.Anio = anio;
                    objEntidad.Estatus = true;
                    saldosInicialesFactoryServices.GetSaldosIniciales().Guardar(objEntidad);

                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add(SUCCESS, false);
                    result.Add("Alerta", "Atención: El mes que desea actualizar no es el siguiente al último registro");

                }

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarInfoCxC(List<CxCDTO> obj, int mes, int anio, int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                tblPro_CxC objEntidad = new tblPro_CxC();
                var jsonSerialiser = new JavaScriptSerializer();
                var json = jsonSerialiser.Serialize(obj);
                objEntidad.id = id;
                objEntidad.CadenaJson = json.ToString();

                objEntidad.Mes = mes;
                objEntidad.Anio = anio;
                objEntidad.Estatus = true;
                cxCFactoryServices.GetCxC().Guardar(objEntidad);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarActivoFijo(tblPro_ActivoFijo obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var diff = obj.Mes - activoFijoFactoryService.GetActivoFijo().getUltimoMesCapturado(obj.Mes);
                if (diff == 0 || diff == 1)
                {
                    activoFijoFactoryService.GetActivoFijo().GuardarActualizarActivoFijo(obj);
                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add(SUCCESS, false);
                    result.Add("Alerta", "Atención: El mes que desea actualizar no es el siguiente al último registro");

                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFillTableAF(FiltrosGeneralDTO objFiltro)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var OGT = new ParametrosDTO();
                var res = activoFijoFactoryService.GetActivoFijo().GetJsonData(objFiltro);

                var obj = JsonConvert.DeserializeObject<List<ActivoFijoDTO>>(res.CadenaJson);
                int id = 0;
                int idRow = 0;
                int estatus = 0;
                if (!res.Equals(null))
                {
                    result.Add("GetData", obj.Select(x => new
                    {
                        id = x.id,
                        Concepto = x.Concepto,
                        Area = x.Area,
                        NumObra = x.Obra,
                        Fecha1 = "<div class='input-group'>" +
                                       "<input type='text' class='form-control mes fecha1' value=" + x.Fecha1 + ">" +
                               "</div>",
                        Fecha2 = "<div class='input-group'>" +
                                      "<input type='text' class='form-control mes fecha2' value=" + x.Fecha2 + ">" +
                              "</div>",
                        Fecha3 = "<div class='input-group'>" +
                                       "<input type='text' class='form-control mes fecha3' value=" + x.Fecha3 + ">" +
                               "</div>",
                        Fecha4 = "<div class='input-group'>" +
                                      "<input type='text' class='form-control mes fecha4' value=" + x.Fecha4 + ">" +
                              "</div>",
                        Fecha5 = "<div class='input-group'>" +
                               "<input type='text' class='form-control mes fecha5' value=" + x.Fecha5 + ">" +
                               "</div>",
                        Fecha6 = "<div class='input-group'>" +
                                      "<input type='text' class='form-control mes fecha6' value=" + x.Fecha6 + ">" +
                              "</div>",
                        Fecha7 = "<div class='input-group'>" +
                                      "<input type='text' class='form-control mes fecha7' value=" + x.Fecha7 + ">" +
                              "</div>",
                        Fecha8 = "<div class='input-group'>" +
                                       "<input type='text' class='form-control mes fecha8' value=" + x.Fecha8 + ">" +
                               "</div>",
                        Fecha9 = "<div class='input-group'>" +
                                      "<input type='text' class='form-control mes fecha9' value=" + x.Fecha9 + ">" +
                              "</div>",
                        Fecha10 = "<div class='input-group'>" +
                                      "<input type='text' class='form-control mes fecha10' value=" + x.Fecha10 + ">" +
                              "</div>",
                        Fecha11 = "<div class='input-group'>" +
                                       "<input type='text' class='form-control mes fecha11' value=" + x.Fecha11 + ">" +
                               "</div>",
                        Fecha12 = "<div class='input-group'>" +
                                     "<input type='text' class='form-control mes fecha12' value=" + x.Fecha12 + ">" +
                                 "</div>",
                        Total = "<label class='lblTotal'>" + (decValor(x.Fecha1) + decValor(x.Fecha2) + decValor(x.Fecha3) + decValor(x.Fecha4) + decValor(x.Fecha5) + decValor(x.Fecha6) + decValor(x.Fecha7) + decValor(x.Fecha8) + decValor(x.Fecha9) + decValor(x.Fecha10) + decValor(x.Fecha11) + decValor(x.Fecha12)).ToString("C") + "</label>"
                    }));
                    result.Add("GetTotal", obj.Select(x => new
                    {
                        id = 0,
                        Concepto = "",
                        Area = "",
                        NumObra = "Total",
                        Fecha1 = "<label class='lblTotalFecha1'>" + obj.Sum(y => decValor(y.Fecha1)).ToString("C") + "</label>",
                        Fecha2 = "<label class='lblTotalFecha2'>" + obj.Sum(y => decValor(y.Fecha2)).ToString("C") + "</label>",
                        Fecha3 = "<label class='lblTotalFecha3'>" + obj.Sum(y => decValor(y.Fecha3)).ToString("C") + "</label>",
                        Fecha4 = "<label class='lblTotalFecha4'>" + obj.Sum(y => decValor(y.Fecha4)).ToString("C") + "</label>",
                        Fecha5 = "<label class='lblTotalFecha5'>" + obj.Sum(y => decValor(y.Fecha5)).ToString("C") + "</label>",
                        Fecha6 = "<label class='lblTotalFecha6'>" + obj.Sum(y => decValor(y.Fecha6)).ToString("C") + "</label>",
                        Fecha7 = "<label class='lblTotalFecha7'>" + obj.Sum(y => decValor(y.Fecha7)).ToString("C") + "</label>",
                        Fecha8 = "<label class='lblTotalFecha8'>" + obj.Sum(y => decValor(y.Fecha8)).ToString("C") + "</label>",
                        Fecha9 = "<label class='lblTotalFecha9'>" + obj.Sum(y => decValor(y.Fecha9)).ToString("C") + "</label>",
                        Fecha10 = "<label class='lblTotalFecha10'>" + obj.Sum(y => decValor(y.Fecha10)).ToString("C") + "</label>",
                        Fecha11 = "<label class='lblTotalFecha11'>" + obj.Sum(y => decValor(y.Fecha11)).ToString("C") + "</label>",
                        Fecha12 = "<label class='lblTotalFecha12'>" + obj.Sum(y => decValor(y.Fecha12)).ToString("C") + "</label>",
                        Total = "<label class='lblTotal'>" + (decValor(x.Fecha1) + decValor(x.Fecha2) + decValor(x.Fecha3) + decValor(x.Fecha4) + decValor(x.Fecha5) + decValor(x.Fecha6) + decValor(x.Fecha7) + decValor(x.Fecha8) + decValor(x.Fecha9) + decValor(x.Fecha10) + decValor(x.Fecha11) + decValor(x.Fecha12)).ToString("C") + "</label>"
                    }));

                    var diff = objFiltro.mes - activoFijoFactoryService.GetActivoFijo().getUltimoMesCapturado(objFiltro.mes);
                    if (diff == 0 || diff == 1)
                    {
                        estatus = 1;
                    }
                    else
                    {
                        estatus = 2;
                    }

                    if (objFiltro.mes == res.Mes)
                    {
                        id = res.id;
                    }

                }

                result.Add("EstadoRegreso", estatus);
                result.Add("idRow", idRow);
                result.Add("id", id);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarResponsable(tblPro_CatResponsables obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                catResponsableFactoryServices.getCatResponsable().Guardar(obj);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInfoUsuarios(string term)
        {
            List<int> usuarios = new List<int>();
            var items = usuarioFactoryServices.getUsuarioService().ListUsuariosAutoComplete(term, 0);

            var filteredItems = items.Select(x => new { id = x.id, label = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetResponsables()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = catResponsableFactoryServices.getCatResponsable().fillCboResponsables();
                result.Add(ITEMS, res.Select(x => new { Value = x.Color, Text = x.Descripcion, Prefijo = x.Abreviatura }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarPremisas(tblPro_Premisas obj, PremisasDTO data)
        {
            var result = new Dictionary<string, object>();
            try
            {
                obj.CadenaJson = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                premisasFactoryServices.GetPremisas().GuardarActualizarPremisas(obj);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFillTableP(FiltrosGeneralDTO objFiltro)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = premisasFactoryServices.GetPremisas().GetJsonData(objFiltro);
                var obj = JsonConvert.DeserializeObject<PremisasDTO>(res.CadenaJson);
                int id = 0;
                int idRow = 0;
                if (!res.Equals(null))
                {
                    result.Add("TablaPremisas", obj);
                }
                result.Add("idRow", idRow);
                result.Add("id", id);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        decimal decValor(string strValor)
        {
            return !strValor.Equals(null) ? decimal.Parse(strValor) : 0;
        }

        public ActionResult GetFillTableAdministracion(FiltrosGeneralDTO objFiltro)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = administracionFactoryService.GetAdministracion().GetJsonData(objFiltro);
                if (data.CadenaJson == null)
                {
                    result.Add("obj", new AdministracionDTO());
                }
                else
                {
                    var objData = (AdministracionDTO)Newtonsoft.Json.JsonConvert.DeserializeObject<AdministracionDTO>(data.CadenaJson);
                    var premisas = premisasFactoryServices.GetPremisas().GetJsonData(objFiltro);
                    var objPremisas = (PremisasDTO)Newtonsoft.Json.JsonConvert.DeserializeObject<PremisasDTO>(premisas.CadenaJson);
                    var mes = 11 - objFiltro.mes;
                    objData.ad01_Mes1 = 0;
                    objData.ad01_Mes2 = 0;
                    objData.ad01_Mes3 = 0;
                    objData.ad01_Mes4 = 0;
                    objData.ad01_Mes5 = 0;
                    objData.ad01_Mes6 = 0;
                    objData.ad01_Mes7 = 0;
                    objData.ad01_Mes8 = 0;
                    objData.ad01_Mes9 = 0;
                    objData.ad01_Mes10 = 0;
                    objData.ad01_Mes11 = 0;
                    objData.ad01_Mes12 = 0;
                    switch (mes)
                    {
                        case 0: objData.ad01_Mes1 = objPremisas.Ln12.Mes3; break;
                        case 1: objData.ad01_Mes2 = objPremisas.Ln12.Mes3; break;
                        case 2: objData.ad01_Mes3 = objPremisas.Ln12.Mes3; break;
                        case 3: objData.ad01_Mes4 = objPremisas.Ln12.Mes3; break;
                        case 4: objData.ad01_Mes5 = objPremisas.Ln12.Mes3; break;
                        case 5: objData.ad01_Mes6 = objPremisas.Ln12.Mes3; break;
                        case 6: objData.ad01_Mes7 = objPremisas.Ln12.Mes3; break;
                        case 7: objData.ad01_Mes8 = objPremisas.Ln12.Mes3; break;
                        case 8: objData.ad01_Mes9 = objPremisas.Ln12.Mes3; break;
                        case 9: objData.ad01_Mes10 = objPremisas.Ln12.Mes3; break;
                        case 10: objData.ad01_Mes11 = objPremisas.Ln12.Mes3; break;
                        case 11: objData.ad01_Mes12 = objPremisas.Ln12.Mes3; break;
                        default:
                            break;
                    }

                    administracionFactoryService.GetAdministracion().GuardarActualizarAdministracion(objFiltro, objData);
                    result.Add("obj", objData);
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

        public ActionResult GuardarAdministracion(FiltrosGeneralDTO objFiltro, AdministracionDTO obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                administracionFactoryService.GetAdministracion().GuardarActualizarAdministracion(objFiltro, obj);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTablaFlujoIngresoGeneral(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var ListFlujodeIngresoGeneral = (List<tblPro_Obras>)Session["ListadoFlujoIngresoGeneral"];
                var resultado = ListFlujodeIngresoGeneral.Where(x => x.Area.Equals(id)).ToList();
                tblPro_Obras objTotal = new tblPro_Obras();
                objTotal.AreaNombre = "Totales";
                objTotal.Fecha1 = resultado.Sum(x => x.Fecha1);
                objTotal.Fecha2 = resultado.Sum(x => x.Fecha2);
                objTotal.Fecha3 = resultado.Sum(x => x.Fecha3);
                objTotal.Fecha4 = resultado.Sum(x => x.Fecha4);
                objTotal.Fecha5 = resultado.Sum(x => x.Fecha5);
                objTotal.Fecha6 = resultado.Sum(x => x.Fecha6);
                objTotal.Fecha7 = resultado.Sum(x => x.Fecha7);
                objTotal.Fecha8 = resultado.Sum(x => x.Fecha8);
                objTotal.Fecha9 = resultado.Sum(x => x.Fecha9);
                objTotal.Fecha10 = resultado.Sum(x => x.Fecha10);
                objTotal.Fecha11 = resultado.Sum(x => x.Fecha11);
                objTotal.Fecha12 = resultado.Sum(x => x.Fecha12);
                objTotal.Monto = resultado.Sum(x => x.Fecha1 + x.Fecha2 + x.Fecha3 + x.Fecha4 + x.Fecha5 + x.Fecha6 + x.Fecha7 + x.Fecha8 + x.Fecha9 + x.Fecha10 + x.Fecha11 + x.Fecha12);


                resultado.Add(objTotal);

                foreach (var item in resultado)
                {
                    item.Total = (item.Monto / objTotal.Monto) * 100;


                }

                result.Add("FujoIngresoGeneral", resultado.OrderByDescending(x => x.AreaNombre.Equals("Totales")).ThenByDescending(x => x.Monto));
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CapturaObraSetData(FiltrosGeneralDTO parametros)
        {
            var result = new Dictionary<string, object>();
            //try
            //{
            var res = capturadeObrasFactoryServices.GetCapturaObras().getinfoCapturaObras(parametros.escenario, parametros.divisor, parametros.mes, parametros.anio);

            EstadoResultadosDTO UtilidadNeta = new EstadoResultadosDTO();
            EstadoResultadosDTO VentasNetas = new EstadoResultadosDTO();
            if (res.Count > 0)
            {
                UtilidadNeta = (EstadoResultadosDTO)res["UtilidadNeta"];
                VentasNetas = (EstadoResultadosDTO)res["VentasNetas"];
            }

            EstadoResultadosDTO ResultadoMensualAcumulado = new EstadoResultadosDTO();
            EstadoResultadosDTO VentaAcumulada = new EstadoResultadosDTO();
            EstadoResultadosDTO PorcentajeVentasResultado = new EstadoResultadosDTO();

            DetallePorcentajeMESDTO DetallePorcentaje = new DetallePorcentajeMESDTO();



            ResultadoMensualAcumulado.Fecha1 = UtilidadNeta.Fecha1;
            ResultadoMensualAcumulado.Fecha2 = ResultadoMensualAcumulado.Fecha1 + UtilidadNeta.Fecha2;
            ResultadoMensualAcumulado.Fecha3 = ResultadoMensualAcumulado.Fecha2 + UtilidadNeta.Fecha3;
            ResultadoMensualAcumulado.Fecha4 = ResultadoMensualAcumulado.Fecha3 + UtilidadNeta.Fecha4;
            ResultadoMensualAcumulado.Fecha5 = ResultadoMensualAcumulado.Fecha4 + UtilidadNeta.Fecha5;
            ResultadoMensualAcumulado.Fecha6 = ResultadoMensualAcumulado.Fecha5 + UtilidadNeta.Fecha6;
            ResultadoMensualAcumulado.Fecha7 = ResultadoMensualAcumulado.Fecha6 + UtilidadNeta.Fecha7;
            ResultadoMensualAcumulado.Fecha8 = ResultadoMensualAcumulado.Fecha7 + UtilidadNeta.Fecha8;
            ResultadoMensualAcumulado.Fecha9 = ResultadoMensualAcumulado.Fecha8 + UtilidadNeta.Fecha9;
            ResultadoMensualAcumulado.Fecha10 = ResultadoMensualAcumulado.Fecha9 + UtilidadNeta.Fecha10;
            ResultadoMensualAcumulado.Fecha11 = ResultadoMensualAcumulado.Fecha10 + UtilidadNeta.Fecha11;
            ResultadoMensualAcumulado.Fecha12 = ResultadoMensualAcumulado.Fecha11 + UtilidadNeta.Fecha12;



            VentaAcumulada.Fecha1 = VentasNetas.Fecha1;
            VentaAcumulada.Fecha2 = VentaAcumulada.Fecha1 + VentasNetas.Fecha2;
            VentaAcumulada.Fecha3 = VentaAcumulada.Fecha2 + VentasNetas.Fecha3;
            VentaAcumulada.Fecha4 = VentaAcumulada.Fecha3 + VentasNetas.Fecha4;
            VentaAcumulada.Fecha5 = VentaAcumulada.Fecha4 + VentasNetas.Fecha5;
            VentaAcumulada.Fecha6 = VentaAcumulada.Fecha5 + VentasNetas.Fecha6;
            VentaAcumulada.Fecha7 = VentaAcumulada.Fecha6 + VentasNetas.Fecha7;
            VentaAcumulada.Fecha8 = VentaAcumulada.Fecha7 + VentasNetas.Fecha8;
            VentaAcumulada.Fecha9 = VentaAcumulada.Fecha8 + VentasNetas.Fecha9;
            VentaAcumulada.Fecha10 = VentaAcumulada.Fecha9 + VentasNetas.Fecha10;
            VentaAcumulada.Fecha11 = VentaAcumulada.Fecha10 + VentasNetas.Fecha11;
            VentaAcumulada.Fecha12 = VentaAcumulada.Fecha11 + VentasNetas.Fecha12;


            if (res.Count > 0)
            {
                PorcentajeVentasResultado.Fecha1 = VentaAcumulada.Fecha1 != 0 ? ResultadoMensualAcumulado.Fecha1 / VentaAcumulada.Fecha1 : 0;
                PorcentajeVentasResultado.Fecha2 = VentaAcumulada.Fecha2 != 0 ? ResultadoMensualAcumulado.Fecha2 / VentaAcumulada.Fecha2 : 0;
                PorcentajeVentasResultado.Fecha3 = VentaAcumulada.Fecha3 != 0 ? ResultadoMensualAcumulado.Fecha3 / VentaAcumulada.Fecha3 : 0;
                PorcentajeVentasResultado.Fecha4 = VentaAcumulada.Fecha4 != 0 ? ResultadoMensualAcumulado.Fecha4 / VentaAcumulada.Fecha4 : 0;
                PorcentajeVentasResultado.Fecha5 = VentaAcumulada.Fecha5 != 0 ? ResultadoMensualAcumulado.Fecha5 / VentaAcumulada.Fecha5 : 0;
                PorcentajeVentasResultado.Fecha6 = VentaAcumulada.Fecha6 != 0 ? ResultadoMensualAcumulado.Fecha6 / VentaAcumulada.Fecha6 : 0;
                PorcentajeVentasResultado.Fecha7 = VentaAcumulada.Fecha7 != 0 ? ResultadoMensualAcumulado.Fecha7 / VentaAcumulada.Fecha7 : 0;
                PorcentajeVentasResultado.Fecha8 = VentaAcumulada.Fecha8 != 0 ? ResultadoMensualAcumulado.Fecha8 / VentaAcumulada.Fecha8 : 0;
                PorcentajeVentasResultado.Fecha9 = VentaAcumulada.Fecha9 != 0 ? ResultadoMensualAcumulado.Fecha9 / VentaAcumulada.Fecha9 : 0;
                PorcentajeVentasResultado.Fecha10 = VentaAcumulada.Fecha10 != 0 ? ResultadoMensualAcumulado.Fecha10 / VentaAcumulada.Fecha10 : 0;
                PorcentajeVentasResultado.Fecha11 = VentaAcumulada.Fecha11 != 0 ? ResultadoMensualAcumulado.Fecha11 / VentaAcumulada.Fecha11 : 0;
                PorcentajeVentasResultado.Fecha12 = VentaAcumulada.Fecha12 != 0 ? ResultadoMensualAcumulado.Fecha12 / VentaAcumulada.Fecha12 : 0;
            }

            DetallePorcentaje.Fecha1 = ResultadoMensualAcumulado.Fecha1.ToString("0", CultureInfo.InvariantCulture) + "/" + VentaAcumulada.Fecha1.ToString("0", CultureInfo.InvariantCulture) + "=" + Math.Round(PorcentajeVentasResultado.Fecha1 * 100, 2);
            DetallePorcentaje.Fecha2 = ResultadoMensualAcumulado.Fecha2.ToString("0", CultureInfo.InvariantCulture) + "/" + VentaAcumulada.Fecha2.ToString("0", CultureInfo.InvariantCulture) + "=" + Math.Round(PorcentajeVentasResultado.Fecha2 * 100, 2);
            DetallePorcentaje.Fecha3 = ResultadoMensualAcumulado.Fecha3.ToString("0", CultureInfo.InvariantCulture) + "/" + VentaAcumulada.Fecha3.ToString("0", CultureInfo.InvariantCulture) + "=" + Math.Round(PorcentajeVentasResultado.Fecha3 * 100, 2);
            DetallePorcentaje.Fecha4 = ResultadoMensualAcumulado.Fecha4.ToString("0", CultureInfo.InvariantCulture) + "/" + VentaAcumulada.Fecha4.ToString("0", CultureInfo.InvariantCulture) + "=" + Math.Round(PorcentajeVentasResultado.Fecha4 * 100, 2);
            DetallePorcentaje.Fecha5 = ResultadoMensualAcumulado.Fecha5.ToString("0", CultureInfo.InvariantCulture) + "/" + VentaAcumulada.Fecha5.ToString("0", CultureInfo.InvariantCulture) + "=" + Math.Round(PorcentajeVentasResultado.Fecha5 * 100, 2);
            DetallePorcentaje.Fecha6 = ResultadoMensualAcumulado.Fecha6.ToString("0", CultureInfo.InvariantCulture) + "/" + VentaAcumulada.Fecha6.ToString("0", CultureInfo.InvariantCulture) + "=" + Math.Round(PorcentajeVentasResultado.Fecha6 * 100, 2);
            DetallePorcentaje.Fecha7 = ResultadoMensualAcumulado.Fecha7.ToString("0", CultureInfo.InvariantCulture) + "/" + VentaAcumulada.Fecha7.ToString("0", CultureInfo.InvariantCulture) + "=" + Math.Round(PorcentajeVentasResultado.Fecha7 * 100, 2);
            DetallePorcentaje.Fecha8 = ResultadoMensualAcumulado.Fecha8.ToString("0", CultureInfo.InvariantCulture) + "/" + VentaAcumulada.Fecha8.ToString("0", CultureInfo.InvariantCulture) + "=" + Math.Round(PorcentajeVentasResultado.Fecha8 * 100, 2);
            DetallePorcentaje.Fecha9 = ResultadoMensualAcumulado.Fecha9.ToString("0", CultureInfo.InvariantCulture) + "/" + VentaAcumulada.Fecha9.ToString("0", CultureInfo.InvariantCulture) + "=" + Math.Round(PorcentajeVentasResultado.Fecha9 * 100, 2);
            DetallePorcentaje.Fecha10 = ResultadoMensualAcumulado.Fecha10.ToString("0", CultureInfo.InvariantCulture) + "/" + VentaAcumulada.Fecha10.ToString("0", CultureInfo.InvariantCulture) + "=" + Math.Round(PorcentajeVentasResultado.Fecha10 * 100, 2);
            DetallePorcentaje.Fecha11 = ResultadoMensualAcumulado.Fecha11.ToString("0", CultureInfo.InvariantCulture) + "/" + VentaAcumulada.Fecha11.ToString("0", CultureInfo.InvariantCulture) + "=" + Math.Round(PorcentajeVentasResultado.Fecha11 * 100, 2);
            DetallePorcentaje.Fecha12 = ResultadoMensualAcumulado.Fecha12.ToString("0", CultureInfo.InvariantCulture) + "/" + VentaAcumulada.Fecha12.ToString("0", CultureInfo.InvariantCulture) + "=" + Math.Round(PorcentajeVentasResultado.Fecha12 * 100, 2);




            var FlujodeIngresoGeneral = Newtonsoft.Json.JsonConvert.SerializeObject(res.FirstOrDefault(x => x.Key.Equals("FlujodeIngresoGeneral")).Value);
            var ListFlujodeIngresoGeneral = Newtonsoft.Json.JsonConvert.DeserializeObject<List<tblPro_Obras>>(FlujodeIngresoGeneral);
            Session["ListadoFlujoIngresoGeneral"] = ListFlujodeIngresoGeneral.ToList();

            result = res;
            result.Add("DetallePorcentaje", DetallePorcentaje);
            result.Add("VentaAcumulada", VentaAcumulada);
            result.Add("ResultadoMensualAcumulado", ResultadoMensualAcumulado);
            result.Add("PorcentajeVentasResultado", PorcentajeVentasResultado);
            result.Add("ultimoMes2", (int)res["ultimoMes"]);
            result.Add("ultimoAnio2", (int)res["ultimoAnio"]);

            var resTabla = capCifrasPrincipalesFactoryServices.getCapCifrasPrincipalesFactoryServices().getOBJCifrasPrincipales(parametros.mes, parametros.anio, parametros.escenario.ToString(), 0);
            CapCifrasPrincipalesDTO cifrasPrincipalesTable = new CapCifrasPrincipalesDTO();

            if (resTabla != null)
            {


                cifrasPrincipalesTable.UtilidadPlaneadaAnioActual = regresarVacio(resTabla.UtilidadPlaneadaAnioActual);//.ToString("C0");
                cifrasPrincipalesTable.UtilidadPlaneadaAnioAnterior = regresarVacio(resTabla.UtilidadPlaneadaAnioAnterior);//.ToString("C0");
                cifrasPrincipalesTable.UtilidadPlaneadaMesActual = regresarVacio(resTabla.UtilidadPlaneadaMesActual);//.ToString("C0");
                cifrasPrincipalesTable.UtilidadRealAnioActual = regresarVacio(resTabla.UtilidadRealAnioActual);//.ToString("C0");
                cifrasPrincipalesTable.UtilidadRealAnioAnterior = regresarVacio(resTabla.UtilidadRealAnioAnterior);//.ToString("C0");
                cifrasPrincipalesTable.UtilidadRealMesActual = regresarVacio(resTabla.UtilidadRealMesActual);//.ToString("C0");
                cifrasPrincipalesTable.VentaProyectadaAlAnio = regresarVacio(resTabla.VentaProyectadaAlAnio);//.ToString("C0");
                cifrasPrincipalesTable.VentaProyectadaAnioAnterior = regresarVacio(resTabla.VentaProyectadaAnioAnterior);//.ToString("C0");
                cifrasPrincipalesTable.VentaProyectadaMesActual = regresarVacio(resTabla.VentaProyectadaMesActual);//.ToString("C0");
                cifrasPrincipalesTable.VentaRealAnioAnterior = regresarVacio(resTabla.VentaRealAnioAnterior);//.ToString("C0");
                cifrasPrincipalesTable.VentaRealMesActual = regresarVacio(resTabla.VentaRealMesActual);//.ToString("C0");
                cifrasPrincipalesTable.VentaRealProyectdaAlAnio = regresarVacio(resTabla.VentaRealProyectdaAlAnio);//.ToString("C0");

                var porCumpliminetoAnioAnterior = (resTabla.VentaRealAnioAnterior / resTabla.VentaProyectadaAnioAnterior) * 100;
                var porCumplimientoMesActual = (resTabla.VentaRealMesActual / resTabla.VentaProyectadaMesActual) * 100;
                var porCumplimientoAnioActual = (resTabla.VentaRealProyectdaAlAnio / resTabla.VentaProyectadaAlAnio) * 100;

                cifrasPrincipalesTable.porCumpliminetoAnioAnterior = porCumpliminetoAnioAnterior.ToString("#.##") + "%";
                cifrasPrincipalesTable.porCumplimientoMesActual = porCumplimientoMesActual.ToString("#.##") + "%";
                cifrasPrincipalesTable.porCumplimientoAnioActual = porCumplimientoAnioActual.ToString("#.##") + "%";

                var porUtilidadPlaneadaAnioAnterior = (resTabla.UtilidadPlaneadaAnioAnterior / resTabla.VentaProyectadaAnioAnterior) * 100;
                var porUtilidadPlaneadaMesActual = (resTabla.UtilidadPlaneadaMesActual / resTabla.VentaProyectadaMesActual) * 100;
                var porUtilidadPlenadaAnioActual = (resTabla.UtilidadPlaneadaAnioActual / resTabla.VentaProyectadaAlAnio) * 100;

                cifrasPrincipalesTable.porUtilidadPlaneadaAnioAnterior = porUtilidadPlaneadaAnioAnterior.ToString("#.##") + "%";
                cifrasPrincipalesTable.porUtilidadPlaneadaMesActual = porUtilidadPlaneadaMesActual.ToString("#.##") + "%";
                cifrasPrincipalesTable.porUtilidadPlenadaAnioActual = porUtilidadPlenadaAnioActual.ToString("#.##") + "%";

                var porUtilidadRealAnioAnterior = resTabla.VentaRealAnioAnterior != 0 ? (resTabla.UtilidadRealAnioAnterior / resTabla.VentaRealAnioAnterior) * 100 : 0;
                var porUtilidadRealMesActual = resTabla.VentaRealMesActual != 0 ? (resTabla.UtilidadRealMesActual / resTabla.VentaRealMesActual) * 100 : 0;
                var porUtilidadRealAnioActual = resTabla.VentaRealProyectdaAlAnio != 0 ? (resTabla.UtilidadRealAnioActual / resTabla.VentaRealProyectdaAlAnio) * 100 : 0;

                cifrasPrincipalesTable.porUtilidadRealAnioAnterior = porUtilidadRealAnioAnterior.ToString("#.##") + "%";
                cifrasPrincipalesTable.porUtilidadRealMesActual = porUtilidadRealMesActual.ToString("#.##") + "%";
                cifrasPrincipalesTable.porUtilidadRealAnioActual = porUtilidadRealAnioActual.ToString("#.##") + "%";

            }


            result.Add("DataResultTabla", cifrasPrincipalesTable);

            result.Add(SUCCESS, true);

            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private string porcentajes(decimal dato1, decimal dato2)
        {
            if (dato1 >= 0)
            {

            }

            return "---";
        }

        private string regresarVacio(decimal dato)
        {
            if (dato != 0)
            {
                return dato.ToString("C0");
            }
            else
            {
                return "---";
            }
        }

        public ActionResult GetFillTablePagosDiv(FiltrosGeneralDTO objFiltro)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = pagosDiversosFactoryService.GetPagosDiversos().GetJsonData(objFiltro);
                if (data == null)
                {
                    result.Add("obj", new PagosDivDTO());
                    if (objFiltro.mes == DateTime.Now.Month - 1)
                    {
                        result.Add("loadFile", true);
                        result.Add(SUCCESS, false);
                    }
                    else
                    {
                        result.Add("loadFile", false);
                        result.Add(SUCCESS, false);
                    }
                }
                else
                {
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<PagosDivDTO>(data.CadenaJson);
                    result.Add("obj", r);
                    result.Add("loadFile", false);
                    result.Add(SUCCESS, true);
                }

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarPagosDiv(FiltrosGeneralDTO objFiltro, PagosDivDTO obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                pagosDiversosFactoryService.GetPagosDiversos().GuardarActualizarPagosDiversos(objFiltro, obj);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getPDLN4(MesDTO ln2, MesDTO ln4, FiltrosGeneralDTO objFiltro, int? col)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var r = pagosDiversosFactoryService.GetPagosDiversos().getLN4(ln2, ln4, objFiltro, col);
                result.Add("r", r);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getPDLN6(MesDTO ln5, MesDTO ln6, FiltrosGeneralDTO objFiltro, int? col)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var r = pagosDiversosFactoryService.GetPagosDiversos().getLN6(ln5, ln6, objFiltro, col);
                result.Add("r", r);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getPDLN7(decimal valor, FiltrosGeneralDTO objFiltro, int? col)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var r = pagosDiversosFactoryService.GetPagosDiversos().getLN7(valor, objFiltro, col);
                result.Add("r", r);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getPDLN13(MesDTO ln12, MesDTO ln13, FiltrosGeneralDTO objFiltro, int? col)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var r = pagosDiversosFactoryService.GetPagosDiversos().getLN13(ln12, ln13, objFiltro, col);
                result.Add("r", r);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFillTableCobrosDiv(FiltrosGeneralDTO objFiltro)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = cobrosDiversosFactoryService.GetCobrosDiversos().GetJsonData(objFiltro);
                if (data.CadenaJson == null)
                {
                    var temp = new CobrosDivDTO();
                    temp.ln1CliPorcentajeSaldoAmortizar = new MesDTO();
                    temp.ln2ImporteAmortizar1 = new MesDTO();
                    temp.ln3ImporteAmortizar2 = new MesDTO();
                    temp.ln4CxCPorcentajeSaldoAmortizar = new MesDTO();
                    temp.ln5CxCImporteAmortizar = new MesDTO();
                    temp.ln6AporteCapital = new MesDTO();
                    result.Add("obj", temp);
                }
                else
                {
                    result.Add("obj", Newtonsoft.Json.JsonConvert.DeserializeObject<CobrosDivDTO>(data.CadenaJson));
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

        public ActionResult GuardarCobrosDiv(FiltrosGeneralDTO objFiltro, CobrosDivDTO obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                cobrosDiversosFactoryService.GetCobrosDiversos().GuardarActualizarCobrosDiversos(objFiltro, obj);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult LoadInfoCifrasPples(int mes, int anio, string escenarios)
        {

            var result = new Dictionary<string, object>();
            try
            {

                var res = capCifrasPrincipalesFactoryServices.getCapCifrasPrincipalesFactoryServices().getOBJCifrasPrincipales(mes, anio, escenarios, 1);

                result.Add("DataResult", res);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GuardarCapCifrasPrincipales(tblPro_CapCifrasPrincipales obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                capCifrasPrincipalesFactoryServices.getCapCifrasPrincipalesFactoryServices().Guardar(obj);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult guardarComentarioArchivo()
        {
            var result = new Dictionary<string, object>();
            try
            {

                var obj = JsonConvert.DeserializeObject<tblPro_ComentariosObras>(Request.Form["obj"]);
                HttpPostedFileBase file = Request.Files["fupAdjunto"];

                var usuario = usuarioFactoryServices.getUsuarioService().ListUsersById(getUsuario().id).FirstOrDefault();
                obj.usuarioID = 0;
                obj.usuarioNombre = "";
                obj.fecha = DateTime.Now;

                if (usuario != null)
                {
                    obj.usuarioID = usuario.id;
                    obj.usuarioNombre = usuario.nombre + " " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno;
                }

                comentariosObraFactoryServices.getComentariosObraFactoryServices().GuardarComentarioArchivo(obj, file);

                var data = comentariosObraFactoryServices.getComentariosObraFactoryServices().GetListaComentarios(obj.capturadeObrasID, obj.registroID);

                result.Add("obj", data.Select(x => new
                {
                    id = x.id,
                    capturadeObrasID = x.capturadeObrasID,
                    registroID = x.registroID,
                    fecha = x.fecha.ToShortDateString(),
                    comentario = x.comentario,
                    usuarioNombre = x.usuarioNombre,
                    usuarioID = x.usuarioID,
                    estatusComentario = x.estatusComentario,
                    adjuntoNombre = x.adjuntoNombre

                }).ToList());
                result.Add(SUCCESS, true);

                /*
                  var result = new Dictionary<string, object>();
            try
            {
                var usuario = usuarioFactoryServices.getUsuarioService().ListUsersById(getUsuario().id).FirstOrDefault();
                obj.usuarioID = 0;
                obj.usuarioNombre = "";
                obj.fecha = DateTime.Now;

                if (usuario != null)
                {
                    obj.usuarioID = usuario.id;
                    obj.usuarioNombre = usuario.nombre + " " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno;
                }

                comentariosObraFactoryServices.getComentariosObraFactoryServices().GuardarComentario(obj);

               
                result.Add("data", data.Select(x => new
                {
                    id = x.id,
                    capturadeObrasID = x.capturadeObrasID,
                    registroID = x.registroID,
                    fecha = x.fecha.ToShortDateString(),
                    comentario = x.comentario,
                    usuarioNombre = x.usuarioNombre,
                    usuarioID = x.usuarioID,
                    estatusComentario = x.estatusComentario

                }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
                 
                 
                 
                 */

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public FileResult getComentarioArchivoAdjunto()
        {
            int id = int.Parse(Request.QueryString["id"]);
            var o = comentariosObraFactoryServices.getComentariosObraFactoryServices().GetComentarioById(id);
            return File(o.adjunto, "multipart/form-data", o.adjuntoNombre);
        }

        public ActionResult fillCboEscenariosConfiguraciones(int mes, int anio)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var Res = capCifrasPrincipalesFactoryServices.getCapCifrasPrincipalesFactoryServices().getEscenariosConfiguraciones(mes, anio);
                Res.Sort();
                var Res2 = escenariosFactoryServices.getEscenariosFactoryServices().GetListaEscenarios();

                List<ComboDTO> cboSend = new List<ComboDTO>();
                ComboDTO inicial = new ComboDTO();
                inicial.Value = "0";
                inicial.Text = "Ninguno Seleccionado";
                inicial.Prefijo = "Ninguno seleccionado";
                cboSend.Add(inicial);

                ComboDTO nuevo = new ComboDTO();
                nuevo.Value = "Nuevo";
                nuevo.Text = "Nuevo registro";
                nuevo.Prefijo = "Nuevo registro";
                cboSend.Add(nuevo);

                foreach (var item in Res)
                {
                    ComboDTO dato = new ComboDTO();

                    dato.Value = item;
                    dato.Text = "";
                    var aux = item.Split(',');

                    for (int i = 0; i < aux.Length; i++)
                    {
                        dato.Text += Res2.FirstOrDefault(x => x.id == Convert.ToInt32(aux[i])).descripcion;
                        if (i != aux.Length - 1) { dato.Text += ", "; }
                    }
                    dato.Prefijo = item;
                    cboSend.Add(dato);
                }
                result.Add(ITEMS, cboSend);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BorrarEscenariosCapCifrasPrincipales(int mes, int anio, string escenarios)
        {
            var result = new Dictionary<string, object>();
            try
            {
                capCifrasPrincipalesFactoryServices.getCapCifrasPrincipalesFactoryServices().BorrarEscenarios(mes, anio, escenarios);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}