using Core.DTO;
using Core.DTO.Maquinaria.Catalogos;
using Core.DTO.Maquinaria.Reporte;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Excel;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using Core.Enum.Maquinaria;
using Core.Enum.Multiempresa;
using Data.Factory.Maquinaria.Captura;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Inventario;
using Data.Factory.Principal.Archivos;
using Infrastructure.Utils;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Catalogos.Maquinaria
{
    public class CatInventarioController : BaseController
    {
        #region Factory
        DocumentosMaquinariaFactoryServices documentosMaquinariaFactoryServices;
        AsignacionEquiposFactoryServices asignacionEquiposFactoryServices;
        CentroCostosFactoryServices centroCostosFactoryServices;
        MaquinaFactoryServices maquinaFactoryServices;
        TipoBajaFactoryServices tipoBajaFactoryServices;
        HistInventarioFactoryServices histInventarioFactoryServices;
        CapturaHorometroFactoryServices horometroFactoryServices;
        ArchivoFactoryServices ArchivoFS;
        #endregion

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            documentosMaquinariaFactoryServices = new DocumentosMaquinariaFactoryServices();
            asignacionEquiposFactoryServices = new AsignacionEquiposFactoryServices();
            histInventarioFactoryServices = new HistInventarioFactoryServices();
            centroCostosFactoryServices = new CentroCostosFactoryServices();
            maquinaFactoryServices = new MaquinaFactoryServices();
            tipoBajaFactoryServices = new TipoBajaFactoryServices();
            horometroFactoryServices = new CapturaHorometroFactoryServices();
            ArchivoFS = new ArchivoFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        // GET: CatInventario
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BajaMaquinaria()
        {
            return View();
        }
        public ActionResult BajaActivoFijo()
        {
            return View();
        }

        public ActionResult HistorialAsignacion()
        {
            return View();
        }

        public ActionResult FillCboEconomicos(List<string> ccs, int grupo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var listaEconomicos = maquinaFactoryServices.getMaquinaServices().GetAllMaquinas().Where(x => (ccs != null ? ccs.Contains(x.centro_costos) : x.id == x.id) && (grupo != 0 ? grupo == x.grupoMaquinariaID : x.id == x.id)).ToList();

                result.Add(ITEMS, listaEconomicos.Select(x => new ComboDTO { Value = x.id.ToString(), Text = x.noEconomico }).OrderBy(x => x.Text));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEconomicosMayor(List<string> ccs, int grupo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var listaEconomicos = maquinaFactoryServices.getMaquinaServices().GetAllMaquinas().Where(x => x.grupoMaquinaria.tipoEquipoID == 1 && (ccs != null ? ccs.Contains(x.centro_costos) : x.id == x.id) && (grupo != 0 ? grupo == x.grupoMaquinariaID : x.id == x.id)).ToList();

                result.Add(ITEMS, listaEconomicos.Select(x => new ComboDTO { Value = x.id.ToString(), Text = x.noEconomico }).OrderBy(x => x.Text));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetHistorialMaqinaria(string EconomicoID)
        {
            var result = new Dictionary<string, object>();
            //try
            //{
            //var HistoricoMaquinaria = asignacionEquiposFactoryServices.getAsignacionEquiposFactoryServices().GetHistorialMaquina(EconomicoID).OrderBy(x => x.FechaEntrega);

            //if (HistoricoMaquinaria.Count() == 0)
            //{
            //    string EconomicoIDP = maquinaFactoryServices.getMaquinaServices().GetMaquinaByNoEconomico(EconomicoID).id.ToString();

            //    var HistoricoMaquinaria2 = asignacionEquiposFactoryServices.getAsignacionEquiposFactoryServices().getHistorialEconomicos(EconomicoIDP);
            //    result.Add("HistoricoMaquinaria", HistoricoMaquinaria2.Select(x => new
            //    {
            //        id = x.id,
            //        Centro_Costos = x.Centro_Costos,
            //        FechaEntrega = x.FechaEntrega,
            //        FechaLiberacion = x.FechaLiberacion,
            //        totalHoras = x.totalHoras

            //    }));
            //}
            //else
            //{
            //    result.Add("HistoricoMaquinaria", HistoricoMaquinaria.Select(x => new
            //    {
            //        id = x.id,
            //        Centro_Costos = x.Centro_Costos,
            //        FechaEntrega = x.FechaEntrega,
            //        FechaLiberacion = x.FechaLiberacion,
            //        totalHoras = x.totalHoras

            //    }));
            //}

            string EconomicoIDP = maquinaFactoryServices.getMaquinaServices().GetMaquinaByNoEconomico(EconomicoID).id.ToString();

            var HistoricoMaquinaria2 = asignacionEquiposFactoryServices.getAsignacionEquiposFactoryServices().getHistorialEconomicos(EconomicoIDP);
            result.Add("HistoricoMaquinaria", HistoricoMaquinaria2);
            result.Add(SUCCESS, true);
            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public ActionResult LoadTablaAnexosMaquinaria(List<string> ccs, int grupo, int Economico, int tipo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var listaEconomicos = maquinaFactoryServices.getMaquinaServices().GetListaAnexos(ccs, grupo, Economico, tipo);

                result.Add("DataSend", listaEconomicos);
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
        public ActionResult SubirArchivoSingle()
        {
            var result = new Dictionary<string, object>();
            try
            {
                HttpPostedFileBase file1 = Request.Files["fSingleAnexo"];
                var pTipoArchivo = JsonConvert.DeserializeObject<int>(Request.Form["pTipoArchivo"]);
                var pEconomico = JsonConvert.DeserializeObject<int>(Request.Form["pNoEconomicoID"]);
                string FileName = "";
                string ruta = "";
                bool pathExist = false;
                tblM_DocumentosMaquinaria objSave = new tblM_DocumentosMaquinaria();
                DateTime fecha = DateTime.Now;
                string f = fecha.ToString("ddMMyyyy") + fecha.Hour + "" + fecha.Minute;
                FileName = file1.FileName;

                ruta = ArchivoFS.getArchivo().getUrlDelServidor(3) + f + FileName;
                pathExist = GuardarDocumentos(file1, ruta);

                if (pathExist)
                {

                    objSave.nombreArchivo = FileName;
                    objSave.nombreRuta = ruta;
                    objSave.economicoID = pEconomico;
                    objSave.fechaCarga = DateTime.Now;
                    objSave.id = 0;
                    objSave.tipoArchivo = pTipoArchivo;
                    objSave.usuarioSubeArchivo = getUsuario().id;
                    documentosMaquinariaFactoryServices.getDocumentosMaquinariaFactoryServices().Guardar(objSave);

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

        [HttpPost]
        public ActionResult ActualizarArchivoEconomico()
        {
            var result = new Dictionary<string, object>();
            try
            {
                HttpPostedFileBase file1 = Request.Files["fSingleAnexo"];
                var pTipoArchivo = JsonConvert.DeserializeObject<int>(Request.Form["pTipoArchivo"]);
                var pEconomico = JsonConvert.DeserializeObject<int>(Request.Form["pNoEconomicoID"]);
                var idDocumento = JsonConvert.DeserializeObject<int>(Request.Form["idDocumento"]);

                #region Guardar Archivo.
                DateTime fechaActual = DateTime.Now;

                string FileName = file1.FileName;
                string ruta = ArchivoFS.getArchivo().getUrlDelServidor(3) + (fechaActual.ToString("ddMMyyyy") + fechaActual.Hour + "" + fechaActual.Minute) + FileName;

#if DEBUG
                ruta = ruta.Replace("\\\\REPOSITORIO", "C:");
#endif

                bool archivoGuardado = GuardarDocumentos(file1, ruta);
                #endregion

                #region Actualizar información.
                if (archivoGuardado)
                {
                    tblM_DocumentosMaquinaria objSave = new tblM_DocumentosMaquinaria();

                    objSave.nombreArchivo = FileName;
                    objSave.nombreRuta = ruta;
                    objSave.economicoID = pEconomico;
                    objSave.fechaCarga = DateTime.Now;
                    objSave.id = 0;
                    objSave.tipoArchivo = pTipoArchivo;
                    objSave.usuarioSubeArchivo = getUsuario().id;

                    documentosMaquinariaFactoryServices.getDocumentosMaquinariaFactoryServices().actualizarArchivoEconomico(objSave, idDocumento);
                }
                #endregion

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
        public ActionResult EnviarInformacion()
        {
            var result = new Dictionary<string, object>();
            try
            {
                HttpPostedFileBase file1 = Request.Files["fFactura"];
                HttpPostedFileBase file2 = Request.Files["fPedimento"];
                HttpPostedFileBase file3 = Request.Files["fPoliza"];
                HttpPostedFileBase file4 = Request.Files["fTarjetaCirculacion"];
                HttpPostedFileBase file5 = Request.Files["fPermisoCarga"];
                HttpPostedFileBase file6 = Request.Files["fCertificacion"];

                HttpPostedFileBase file7 = Request.Files["flCuadroComparativo"];
                HttpPostedFileBase file8 = Request.Files["flContratos"];
                HttpPostedFileBase file9 = Request.Files["flAnsul"];


                var pNoEconomicoID = JsonConvert.DeserializeObject<int>(Request.Form["pNoEconomicoID"]);
                var pTipoFactura = JsonConvert.DeserializeObject<int>(Request.Form["pTipoFactura"]);
                var pTipoPedimento = JsonConvert.DeserializeObject<int>(Request.Form["pTipoPedimento"]);
                var pTipoPoliza = JsonConvert.DeserializeObject<int>(Request.Form["pTipoPoliza"]);
                var pTipoTCirculacion = JsonConvert.DeserializeObject<int>(Request.Form["pTipoTCirculacion"]);
                var pTipoPermisoCarga = JsonConvert.DeserializeObject<int>(Request.Form["pTipoPermisoCarga"]);
                var pTipoCertificacion = JsonConvert.DeserializeObject<int>(Request.Form["pTipoCertificacion"]);
                var pTipoCuadrpoComparativo = JsonConvert.DeserializeObject<int>(Request.Form["pTipoCuadrpoComparativo"]);
                var pTipoContratos = JsonConvert.DeserializeObject<int>(Request.Form["pTipoContratos"]);
                var pTipoAnsul = JsonConvert.DeserializeObject<int>(Request.Form["pTipoAnsul"]);


                if (file1 != null)
                {
                    GuardarArchivos(file1, pNoEconomicoID, pTipoFactura);
                    result.Add("FacturaOK", true);
                }
                else
                {
                    result.Add("FacturaOK", false);
                }

                if (file2 != null)
                {
                    GuardarArchivos(file2, pNoEconomicoID, pTipoPedimento);
                    result.Add("PedimentoOK", true);
                }
                else
                {
                    result.Add("PedimentoOK", false);
                }
                if (file3 != null)
                {
                    GuardarArchivos(file3, pNoEconomicoID, pTipoPoliza);
                    result.Add("PolizaOK", true);
                }
                else
                {
                    result.Add("PolizaOK", false);
                }
                if (file4 != null)
                {
                    GuardarArchivos(file4, pNoEconomicoID, pTipoTCirculacion);
                    result.Add("TCirculacionOK", true);
                }
                else
                {
                    result.Add("TCirculacionOK", false);
                }
                if (file5 != null)
                {
                    GuardarArchivos(file5, pNoEconomicoID, pTipoPermisoCarga);
                    result.Add("PermisosCargoOK", true);
                }
                else
                {
                    result.Add("PermisosCargoOK", false);
                }
                if (file6 != null)
                {
                    GuardarArchivos(file6, pNoEconomicoID, pTipoCertificacion);
                    result.Add("CertificadoOK", true);
                }
                else
                {
                    result.Add("CertificadoOK", false);
                }
                if (file7 != null)
                {
                    GuardarArchivos(file7, pNoEconomicoID, pTipoCuadrpoComparativo);
                    result.Add("CuadroComparativoOK", true);
                }
                else
                {
                    result.Add("CuadroComparativoOK", false);
                }
                if (file8 != null)
                {
                    GuardarArchivos(file8, pNoEconomicoID, pTipoContratos);
                    result.Add("ContratosOK", true);
                }
                else
                {
                    result.Add("ContratosOK", false);
                }
                if (file9 != null)
                {
                    GuardarArchivos(file9, pNoEconomicoID, pTipoContratos);
                    result.Add("AnsulOK", true);
                }
                else
                {
                    result.Add("AnsulOK", false);
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

        private void GuardarArchivos(HttpPostedFileBase file, int pNoEconomicoID, int pTipoFactura)
        {

            string FileName = "";
            string ruta = "";
            bool pathExist = false;
            tblM_DocumentosMaquinaria objSave = new tblM_DocumentosMaquinaria();
            DateTime fecha = DateTime.Now;
            string f = fecha.ToString("ddMMyyyy") + fecha.Hour + "" + fecha.Minute;
            FileName = file.FileName;

            ruta = ArchivoFS.getArchivo().getUrlDelServidor(3) + f + FileName;
            pathExist = GuardarDocumentos(file, ruta);

            if (pathExist)
            {

                objSave.nombreArchivo = FileName;
                objSave.nombreRuta = ruta;
                objSave.economicoID = pNoEconomicoID;
                objSave.fechaCarga = DateTime.Now;
                objSave.id = 0;
                objSave.tipoArchivo = pTipoFactura;
                objSave.usuarioSubeArchivo = getUsuario().id;
                documentosMaquinariaFactoryServices.getDocumentosMaquinariaFactoryServices().Guardar(objSave);

            }
        }

        public FileResult getFileDownload()
        {
            try
            {
                int id = Convert.ToInt32(Request.QueryString["id"]);
                var Archivo = documentosMaquinariaFactoryServices.getDocumentosMaquinariaFactoryServices().getDocumentosByID(id);
                var ruta = Archivo.nombreRuta;

#if DEBUG
                ruta = ruta.Replace("\\\\REPOSITORIO", "C:");
#else
                ruta = Archivo.nombreRuta.Replace("C:\\", "\\\\REPOSITORIO\\");
#endif

                return File(ruta, "multipart/form-data", Archivo.nombreArchivo);
            }
            catch (Exception)
            {

                return null;
            }

        }
        public ActionResult getFileRuta(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var Archivo = documentosMaquinariaFactoryServices.getDocumentosMaquinariaFactoryServices().getDocumentosByID(id);
                var esSuccess = Archivo.nombreRuta.Length > 0;
                if (esSuccess)
                {
                    var ruta = Archivo.nombreRuta;
#if DEBUG
                    ruta = ruta.Replace("\\\\REPOSITORIO", "C:");
#else
                    ruta = Archivo.nombreRuta.Replace("C:\\", "\\\\REPOSITORIO\\");
#endif

                    result.Add("ruta", ruta);
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        private bool GuardarDocumentos(HttpPostedFileBase archivo, string ruta)
        {
            bool result = false;
            byte[] data;
            using (Stream inputStream = archivo.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                data = memoryStream.ToArray();
            }

#if DEBUG
            ruta = ruta.Replace("\\\\REPOSITORIO", "C:");
#else
            ruta = ruta.Replace("C:\\", "\\REPOSITORIO\\");
#endif

            System.IO.File.WriteAllBytes(ruta, data);
            result = System.IO.File.Exists(ruta);
            return result;
        }

        public ActionResult FillGridInventario(MaquinaFiltrosDTO obj)
        {

            var result = new Dictionary<string, object>();
            //try
            //{

            try
            {
                if (obj.ListCC != null)
                {
                    if (obj.ListCC.Count != null)
                    {
                        if (obj.ListCC.Contains("997"))
                        {
                            obj.ListCC.Add("1010");
                            obj.ListCC.Add("1015");
                        }
                    }
                }
            }
            catch (Exception)
            {


            }

            var GetListaInventario = maquinaFactoryServices.getMaquinaServices().GetInventarioMaquinaria(obj);
            Session["GetListaInventario"] = GetListaInventario;
            Session["GetHistorialActual"] = GetListaInventario;

            result.Add("current", 1);
            result.Add("rowCount", 1);
            result.Add("rows", GetListaInventario.OrderBy(x => x.Economico));

            result.Add(SUCCESS, true);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboCC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = centroCostosFactoryServices.getCentroCostosService().getListaCC();
                if (vSesiones.sesionEmpresaActual == 6) 
                {
                    foreach (var item in list) 
                    {
                        item.Value = item.Prefijo;
                        item.Text = item.Prefijo + " " + item.Text.Split('-')[1];
                    }
                }
                result.Add(ITEMS, list.OrderBy(x => x.Value));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboSemanas()
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<FechasDTO> ListaFechas = new List<FechasDTO>();
                List<FechasDTO> ListaFechas2 = new List<FechasDTO>();
                DateTime fecha2 = new DateTime(DateTime.Now.Year - 1, 01, 01);
                ListaFechas2 = GetFechas(fecha2);


                DateTime fecha = new DateTime(DateTime.Now.Year, 01, 01);
                ListaFechas.AddRange(ListaFechas2);
                ListaFechas.AddRange(GetFechas(fecha));
                result.Add(ITEMS, ListaFechas);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public List<FechasDTO> GetFechas(DateTime fecha)
        {

            List<FechasDTO> ListaFechas = new List<FechasDTO>();
            DateTime FechaInicio = new DateTime();
            DateTime FechaFin = new DateTime();

            for (int i = 1; i <= 53; i++)
            {
                if (i == 1)
                {
                    var diaSemana = (int)fecha.DayOfWeek;
                    FechaInicio = fecha.AddDays(-(int)fecha.DayOfWeek - 4);
                    int diasViernes = ((int)DayOfWeek.Tuesday - (int)fecha.DayOfWeek + 7) % 7;
                    FechaFin = FechaInicio.AddDays(6);

                    ListaFechas.Add(new FechasDTO
                    {
                        Value = i,
                        Text = FechaInicio.ToShortDateString() + " - " + FechaFin.ToShortDateString()
                    });

                }
                else
                {
                    var TempFecha = FechaFin.AddDays(1);

                    FechaInicio = TempFecha;
                    FechaFin = TempFecha.AddDays(6);

                    ListaFechas.Add(new FechasDTO
                    {
                        Value = i,
                        Text = FechaInicio.ToShortDateString() + " - " + FechaFin.ToShortDateString()
                    });
                }

            }

            return ListaFechas;

        }
        public ActionResult GetInfoHistorial(int idFecha)
        {
            var result = new Dictionary<string, object>();
            try
            {
                DateTime fecha = DateTime.Now;
                DateTime FechaSend = new DateTime(fecha.Year, 01, 01);
                var Data = GetFechas(FechaSend).FirstOrDefault(x => x.Value == idFecha);

                if (Data != null)
                {
                    var ArraySplit = Data.Text.Split('-');
                    FechaSend = Convert.ToDateTime(ArraySplit[1]);

                }
                var JsonString = histInventarioFactoryServices.getHistInventarioFactoryServices().GetInfoHistorial(FechaSend);

                var GetListaInventario = JsonConvert.DeserializeObject<List<inventarioGeneralDTO>>(JsonString);

                Session["GetHistorialActual"] = GetListaInventario;

                DateTime fechaInicio = FechaSend;
                Session["GetFechaSemana"] = Data.Text;

                result.Add("current", 1);
                result.Add("rowCount", 1);
                result.Add("rows", GetListaInventario.OrderBy(x => x.Economico));

                //   result.Add(ITEMS, list.OrderBy(x => x.Value));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public FileResult ExportData()
        {
            var semana = Session["GetFechaSemana"];

            ExcelUtilities excel = new ExcelUtilities();
            List<excelSheetDTO> Sheets = new List<excelSheetDTO>();

            excelSheetDTO sheetInventario = new excelSheetDTO();
            sheetInventario.name = "INVENTARIO";
            List<excelRowDTO> excelRowsDTOInventario = new List<excelRowDTO>();
            excelRowsDTOInventario.Add(new excelRowDTO
            {
                cells = new List<excelCellDTO>{
                            new excelCellDTO{ text="NO ECONOMICO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="TIPO MAQUINARIA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="GRUPO MAQUINARIA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="MARCA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="MODELO MAQUINA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="SERIE", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="AÑO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="HOROMETRO ACUMULADO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="UBICACIÓN", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="REDIRECCIONAMIENTO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},     
                            new excelCellDTO{ text="CON CARGO A LA OBRA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="PROPIEDAD EMPRESA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                        }
            });
            List<inventarioGeneralDTO> listaContestadas = (List<inventarioGeneralDTO>)Session["GetHistorialActual"];

            foreach (var i in listaContestadas)
            {
                excelRowsDTOInventario.Add(new excelRowDTO
                {
                    cells = new List<excelCellDTO>{
                                    new excelCellDTO{ text=""+i.Economico, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.Tipo, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.Descripcion, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.Marca, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.Modelo, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.NumeroSerie, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.Anio, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.HorometroAcumulado, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.Ubicacion, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.Redireccion, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},     
                                    new excelCellDTO{ text=""+i.CargoObra, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.empresa, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                }
                });
            }

            sheetInventario.Sheet = excelRowsDTOInventario;
            Sheets.Add(sheetInventario);


            excel.CreateExcelFile(this, Sheets, "INVENTARIO GENERAL -" + semana);
            // GlobalUtils.sendEmailAdjuntoInMemory("","",);

            //var nombre =excel.CreateExcelFile(this, Sheets, "CONSTRUCCIONES PLANIFICADAS - ");


            //return File(nombre, "multipart/form-data");

            return null;
        }



        public ActionResult EnviarInventario(int obj)
        {

            var result = new Dictionary<string, object>();
            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {

                    var mMayor = package.Workbook.Worksheets.Add("Inventario General");
                    //   var mMenor = package.Workbook.Worksheets.Add("Maquinaria Menor");
                    //   var mTransporte = package.Workbook.Worksheets.Add("Equipo Transporte");

                    //Format all cells
                    ExcelRange cols = mMayor.Cells["A:M"];
                    cols.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cols.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                    List<inventarioGeneralDTO> listaContestadas = (List<inventarioGeneralDTO>)Session["GetHistorialActual"];

                    var ListaEconomicos = listaContestadas.ToArray();


                    var ListaSerie = ListaEconomicos.Select(x => x.NumeroSerie).ToArray();
                    mMayor.Cells["A1"].LoadFromCollection(ListaEconomicos.Select(x => x.Economico).ToList());
                    mMayor.Cells["B1"].LoadFromCollection(ListaEconomicos.Select(x => x.Tipo).ToList());
                    mMayor.Cells["C1"].LoadFromCollection(ListaEconomicos.Select(x => x.Descripcion).ToList());
                    mMayor.Cells["D1"].LoadFromCollection(ListaEconomicos.Select(x => x.NumeroSerie).ToList());
                    mMayor.Cells["E1"].LoadFromCollection(ListaEconomicos.Select(x => x.Marca).ToList());
                    mMayor.Cells["F1"].LoadFromCollection(ListaEconomicos.Select(x => x.Modelo).ToList());

                    mMayor.Cells["G1"].LoadFromCollection(ListaEconomicos.Select(x => x.Anio).ToList());
                    mMayor.Cells["H1"].LoadFromCollection(ListaEconomicos.Select(x => x.HorometroAcumulado).ToList());
                    mMayor.Cells["I1"].LoadFromCollection(ListaEconomicos.Select(x => x.Ubicacion).ToList());
                    mMayor.Cells["J1"].LoadFromCollection(ListaEconomicos.Select(x => x.Redireccion).ToList());
                    mMayor.Cells["K1"].LoadFromCollection(ListaEconomicos.Select(x => x.CargoObra).ToList());
                    mMayor.Cells["L1"].LoadFromCollection(ListaEconomicos.Select(x => x.empresa).ToList());
                    mMayor.Cells["M1"].LoadFromCollection(ListaEconomicos.Select(x => x.Estatus).ToList());

                    mMayor.InsertRow(1, 1);
                    //Write the headers and style them
                    mMayor.Cells["A1"].Value = "Economico";
                    mMayor.Cells["B1"].Value = "Tipo";
                    mMayor.Cells["C1"].Value = "Grupo";
                    mMayor.Cells["D1"].Value = "Serie";
                    mMayor.Cells["E1"].Value = "Marca";
                    mMayor.Cells["F1"].Value = "Modelo";
                    mMayor.Cells["G1"].Value = "Año";
                    mMayor.Cells["H1"].Value = "Horometro Acum";
                    mMayor.Cells["I1"].Value = "Ubicación";
                    mMayor.Cells["J1"].Value = "Redireccionamiento";
                    mMayor.Cells["K1"].Value = "Cargo Obra";
                    mMayor.Cells["L1"].Value = "Propiedad empresa";
                    mMayor.Cells["M1"].Value = "Estatus";
                    mMayor.View.FreezePanes(2, 1);

                    using (var rng = mMayor.Cells["A1:M1"])
                    {
                        rng.Style.Font.Bold = true;
                        rng.Style.Font.Color.SetColor(Color.White);
                        rng.Style.WrapText = true;
                        rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rng.Style.Fill.BackgroundColor.SetColor(0, 220, 118, 51);
                    }


                    mMayor.Cells[mMayor.Dimension.Address].AutoFitColumns();
                    package.Compression = CompressionLevel.BestSpeed;

                    List<byte[]> lista = new List<byte[]>();
                    using (var exportData = new MemoryStream())
                    {
                        package.SaveAs(exportData);
                        lista.Add(exportData.ToArray());
                    }

                    /***CORTE INVENTARIO - DIAS MARTES***/
                    var corte = new tblM_CorteInventarioMaq();
                    var corteDetalle = new List<tblM_CorteInventarioMaq_Detalle>();

                    corte.Estatus = true;
                    corte.FechaCorte = DatetimeUtils.UltimoDiaSemanaCorte_Martes(DateTime.Now);
                    corte.FechaCreacion = DateTime.Now;

                    var tiposMaquinaria = listaContestadas.Select(m => m.Tipo).Distinct().Count() > 1 ? null : listaContestadas.Select(m => m.Tipo).Distinct();
                    int? tipoMaquina = null;
                    if (tiposMaquinaria != null && tiposMaquinaria.Count() == 1)
                    {
                        switch (tiposMaquinaria.First().ToUpper())
                        {
                            case "MAYOR":
                                tipoMaquina = (int)TipoMaquinaEnum.Mayor;
                                break;
                            case "MENOR":
                                tipoMaquina = (int)TipoMaquinaEnum.Menor;
                                break;
                            case "TRANSPORTE":
                                tipoMaquina = (int)TipoMaquinaEnum.Transporte;
                                break;
                        }
                    }
                    corte.IdTipoMaquina = tipoMaquina;
                    corte.IdUsuarioCorte = vSesiones.sesionUsuarioDTO.id;

                    var diasMartes = DatetimeUtils.DiasEspecificosDelMes(corte.FechaCorte, DayOfWeek.Tuesday);
                    var diasMiercoles = DatetimeUtils.DiasEspecificosDelMes(corte.FechaCorte, DayOfWeek.Wednesday);

                    if (diasMartes.Count == 5 && corte.FechaCorte.Day == diasMartes.First() /*&& diasMiercoles.Count == 5*/)
                    {
                        corte.Bloqueado = false;
                        corte.BloqueadoConstruplan = false;
                    }
                    else
                    {
                        if ((EmpresaEnum)vSesiones.sesionEmpresaActual == EmpresaEnum.Peru)
                        {

                        }
                        else
                        {
                            corte.Bloqueado = true;
                            corte.BloqueadoConstruplan = true;
                        }
                    }

                    foreach (var item in listaContestadas)
                    {
                        var detalle = new tblM_CorteInventarioMaq_Detalle();

                        detalle.Economico = item.Economico;
                        detalle.Descripcion = item.Descripcion;
                        detalle.Marca = item.Marca;
                        detalle.Modelo = item.Modelo;
                        detalle.NumeroSerie = item.NumeroSerie;
                        detalle.Año = item.Anio;
                        detalle.Ubicacion = item.Ubicacion;
                        detalle.Redireccion = item.Redireccion;
                        detalle.cc = item.cc;
                        detalle.ccCargoObra = item.ccCargoObra;
                        detalle.CargoObra = item.CargoObra;
                        detalle.Resguardante = item.Resgurdante;
                        detalle.HorometroAcumulado = item.HorometroAcumulado;
                        detalle.Tipo = item.Tipo;
                        detalle.IdEconomico = item.idEconomico;
                        detalle.Estatus = true;
                        detalle.Empresa = item.empresa;
                        detalle.EstatusDatosDiarios = item.Estatus;
                        corteDetalle.Add(detalle);
                    }

                    corte.DetalleInv = corteDetalle;

                    if (corte.IdTipoMaquina != null)
                    {
                        result.Add(SUCCESS, false);
                        result.Add(MESSAGE, "La consulta debe de ser sin seleccionar el tipo de maquinaria");
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    var guardadoCorrecto = maquinaFactoryServices.getMaquinaServices().guardarCorteInventario(corte);

                    if (!guardadoCorrecto)
                    {
                        result.Add(SUCCESS, false);
                        result.Add(MESSAGE, "Ya se envió el corte");
                        //result.Add(MESSAGE, "Ya se envió el corte de tipo: " + tiposMaquinaria != null && tiposMaquinaria.Count() == 1 ? tiposMaquinaria.First().ToUpper() : "MAYOR, MENOR Y TRANSPORTE");
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    /***FIN CORTE INVENTARIO - DIAS MARTES***/

                    var correos = maquinaFactoryServices.getMaquinaServices().getListaCorreosInventario(obj);

                    //List<string> correos = new List<string>();
                    //correos.Add("angel.devora@construplan.com.mx");

                    var fechaSemanaRaw = GetFechas(DateTime.Now).FirstOrDefault();

                    var fechaRawInicio = fechaSemanaRaw.Text.ToString().Split('-')[0];

                    DateTime fechaInicio = Convert.ToDateTime(fechaSemanaRaw.Text.ToString().Split('-')[0]);
                    DateTime fechaFinal = Convert.ToDateTime(fechaSemanaRaw.Text.ToString().Split('-')[1]);

                    string cuerpo = "Buenas tardes." +

                    "Se envía inventario general del " + fechaInicio.Day.ToString().PadLeft(2, '0') + " de " + fechaInicio.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")) + " al " + fechaFinal.Day.ToString().PadLeft(2, '0') + " de " + fechaFinal.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")) + " " + fechaFinal.ToString("yyyy") +

                   " Saludos.";

                    //List<string> co = new List<string>();
                    //co.Add("adan.gonzalez@construplan.com.mx");
                    correos.Add("e.encinas@construplan.com.mx");
#if DEBUG
                    correos = new List<string> { "martin.zayas@construplan.com.mx" };
#endif
                    var empresaActual = vSesiones.sesionEmpresaActual;

                    switch (empresaActual) 
                    {
                        case 2:
                            result.Add(SUCCESS, GlobalUtils.sendEmailAdjuntoInMemory2("[SIGOPLAN ARRENDADORA] Reporte de inventario general de maquinaria y equipo del " + fechaInicio.Day.ToString().PadLeft(2, '0') + " de " + fechaInicio.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")) + " al " + fechaFinal.Day.ToString().PadLeft(2, '0') + " de " + fechaFinal.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")) + " " + fechaFinal.ToString("yyyy"), cuerpo, correos, lista));
                            break;
                        case 6:
                            result.Add(SUCCESS, GlobalUtils.sendEmailAdjuntoInMemory2("[SIGOPLAN PERÚ] Reporte de inventario general de maquinaria y equipo del " + fechaInicio.Day.ToString().PadLeft(2, '0') + " de " + fechaInicio.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")) + " al " + fechaFinal.Day.ToString().PadLeft(2, '0') + " de " + fechaFinal.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")) + " " + fechaFinal.ToString("yyyy"), cuerpo, correos, lista));
                            break;
                        default:
                            result.Add(SUCCESS, GlobalUtils.sendEmailAdjuntoInMemory2("Reporte de inventario general de maquinaria y equipo del " + fechaInicio.Day.ToString().PadLeft(2, '0') + " de " + fechaInicio.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")) + " al " + fechaFinal.Day.ToString().PadLeft(2, '0') + " de " + fechaFinal.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")) + " " + fechaFinal.ToString("yyyy"), cuerpo, correos, lista));
                            break;
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

        [HttpGet]
        public JsonResult CorteInventarioEnviado(int? tipo)
        {
            var r = new Dictionary<string, object>();

            try
            {
                r.Add("Corte", maquinaFactoryServices.getMaquinaServices().CorteInventarioEnviado(tipo));
                r.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                r.Add(MESSAGE, ex.Message);
                r.Add(SUCCESS, false);
            }

            return Json(r, JsonRequestBehavior.AllowGet);
        }


        //        public FileResult EnviarInventario()
        //        {


        //            var semana = " DEL 11 DE JULIO AL 17 DE JULIO DEL 2018";///Session["GetFechaSemana"];

        //            //  maquinaFactoryServices.getMaquinaServices().GetInventarioMaquinaria(obj);

        //            ExcelUtilities excel = new ExcelUtilities();
        //            List<excelSheetDTO> Sheets = new List<excelSheetDTO>();

        //            excelSheetDTO sheetInventario = new excelSheetDTO();
        //            sheetInventario.name = "INVENTARIO";
        //            List<excelRowDTO> excelRowsDTOInventario = new List<excelRowDTO>();

        //            excelRowsDTOInventario.Add(new excelRowDTO
        //                    {
        //                        cells = new List<excelCellDTO>{
        //                            new excelCellDTO{ text="NO ECONOMICO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
        //                            new excelCellDTO{ text="TIPO MAQUINARIA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
        //                            new excelCellDTO{ text="GRUPO MAQUINARIA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
        //                            new excelCellDTO{ text="MARCA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
        //                            new excelCellDTO{ text="MODELO MAQUINA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
        //                            new excelCellDTO{ text="SERIE", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
        //                            new excelCellDTO{ text="AÑO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
        //                            new excelCellDTO{ text="HOROMETRO ACUMULADO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
        //                            new excelCellDTO{ text="UBICACIÓN", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
        //                            new excelCellDTO{ text="REDIRECCIONAMIENTO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},     
        //                            new excelCellDTO{ text="CON CARGO A LA OBRA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
        //                        }
        //                    });
        //            List<inventarioGeneralDTO> listaContestadas = (List<inventarioGeneralDTO>)Session["GetHistorialActual"];

        //            foreach (var i in listaContestadas)
        //            {
        //                excelRowsDTOInventario.Add(new excelRowDTO
        //                 {
        //                     cells = new List<excelCellDTO>{
        //                                    new excelCellDTO{ text=""+i.Economico, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
        //                                    new excelCellDTO{ text=""+i.Tipo, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
        //                                    new excelCellDTO{ text=""+i.Descripcion, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
        //                                    new excelCellDTO{ text=""+i.Marca, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
        //                                    new excelCellDTO{ text=""+i.Modelo, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
        //                                    new excelCellDTO{ text=""+i.NumeroSerie, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
        //                                    new excelCellDTO{ text=""+i.Anio, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
        //                                    new excelCellDTO{ text=""+i.HorometroAcumulado, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
        //                                    new excelCellDTO{ text=""+i.Ubicacion, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
        //                                    new excelCellDTO{ text=""+i.Redireccion, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},     
        //                                    new excelCellDTO{ text=""+i.CargoObra, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
        //                                }
        //                 });
        //            }

        //            sheetInventario.Sheet = excelRowsDTOInventario;
        //            Sheets.Add(sheetInventario);


        //           excel.CreateExcelFile(this, Sheets, "INVENTARIO GENERAL -" + semana);

        //            string cuerpo = @"Buenas tardes.
        // 
        //Se envía inventario general del 11 de julio al 17 de julio del 2018.
        // 
        //Saludos.
        //";



        //            List<byte[]> lista = new List<byte[]>();

        //            byte[] bytes = excel.CreateExcelFile(this, Sheets, "INVENTARIO GENERAL -" + semana).ToArray();



        //            lista.Add(bytes);

        //            // string correos = @"'l.cecco' <lcecco@construplan.com.mx>, 'j.samaniego' <j.samaniego@construplan.com.mx>, 'arnulfo.islas' <aislas@construplan.com.mx>, 'jvo consultores' <jvo_consultores@yahoo.com>, 'a sanchez' <a.sanchez@construplan.com.mx>, 'y.olivas' <y.olivas@construplan.com.mx>, 'a.higuera' <a.higuera@construplan.com.mx>, 'm.navarrete' <m.navarrete@construplan.com.mx>, 'Ricardo Navarro' <navarro@construplan.com.mx>, 'j gonzalez' <j.gonzalez@construplan.com.mx>, 'alonso martinez' <alonso.martinez@construplan.com.mx>, 'Hugo Verdugo' <hugo.verdugo@construplan.com.mx>, 'gerardohernandez' <gerardohernandez@construplan.com.mx>, 'f.artalejo' <f.artalejo@construplan.com.mx>, 'Ing Edmundo Fraijo' <e.fraijo@construplan.com.mx>, 'franciscoreina' <franciscoreina@construplan.com.mx>, 'adriana.reina' <adriana.reina@construplan.com.mx>, 'b.valenzuela' <b.valenzuela@construplan.com.mx>, 'ricardo.perez' <ricardo.perez@construplan.com.mx>, 'anabel.camacho' <anabel.camacho@construplan.com.mx>, 'ricardo.campos' <ricardo.campos@construplan.com.mx>, 'Martha San Martin' <martha.sanmartin@construplan.com.mx>, 'j.nevarez' <j.nevarez@construplan.com.mx>, 'jpgonzalez' <jpgonzalez@construplan.com.mx>, 'g.reina' <g.reina@construplan.com.mx>, 'Victor Esquer' <victor.esquer@construplan.com.mx>, 'r.romo' <r.romo@construplan.com.mx>, 'antonio monteverde' <antonio.monteverde@construplan.com.mx>, 'ramses daniel' <ramses.daniel@construplan.com.mx>, 'carlos ladriere' <carlos.ladriere@construplan.com.mx>, 'efrain.contreras' <efrain.contreras@construplan.com.mx>, 'Cesar Vega' <Cesar.vega@construplan.com.mx>, 'javier.elias' <javier.elias@construplan.com.mx>, 'jose gaytan' <jose.gaytan@construplan.com.mx>, 'tadeo gracia' <tadeo.gracia@construplan.com.mx>, 'a.samaniego' <a.samaniego@construplan.com.mx>, 'CP OMAR RAMIREZ' <omar.ramirez@construplan.com.mx>, 'nereyda.garcia' <nereyda.garcia@construplan.com.mx>, 'agustin.ontiveros' <agustin.ontiveros@construplan.com.mx>, 'j.burgos' <j.burgos@construplan.com.mx>, 'r.garcia' <r.garcia@construplan.com.mx>, 'Ing. José Alfredo Tovar' <jose.tovar@construplan.com.mx>, 'd.laborin' <d.laborin@construplan.com.mx>, 'jesus bravo' <jesus.bravo@construplan.com.mx>, 'Roberto Arrona' <roberto.arrona@construplan.com.mx>, 'pamela bernal' <pamela.bernal@construplan.com.mx>, 'morales' <arturo.morales@construplan.com.mx>, 'fernando martinez' <fernando.martinez@construplan.com.mx>, 'luis.gracia' <luis.gracia@construplan.com.mx>, 'maritza.montano' <maritza.montano@construplan.com.mx>, 'Escalante' <alfonso.escalante@construplan.com.mx>, 'juan cecco' <juan.cecco@construplan.com.mx>, 'jimenez' <jesus.jimenez@construplan.com.mx>, 'manolo.anton' <manolo.anton@construplan.com.mx>, 'Gerardo Martinez Ayala' <gerardo.martinez@construplan.com.mx>, 'c.coronado' <c.coronado@construplan.com.mx>, 'frias' <eliu.frias@construplan.com.mx>, 'e.flores' <e.flores@construplan.com.mx>, 'martin acosta' <martin.acosta@construplan.com.mx>, 'Luis Mario Rodríguez Zuluaga' <luismario.rodriguez@construplan.com.mx>, 'luis.fortino' <luis.fortino@construplan.com.mx>, 'jaaciel delreal' <jaaciel.delreal@construplan.com.mx>, 'francisco.alcaraz' <francisco.alcaraz@construplan.com.mx>, 'granillo' <susy.granillo@construplan.com.mx>, 'isaac.moreno' <isaac.moreno@construplan.com.mx>, 'ramon grijalva' <ramon.grijalva@construplan.com.mx>, 'williams.rios' <williams.rios@construplan.com.mx>, 'jose buenrostro' <jose.buenrostro@construplan.com.mx>, 'roman' <oscar.roman@construplan.com.mx>, 'jesus.salas' <jesus.salas@construplan.com.mx>, 'Luis Carcamo' <luis.carcamo@construplan.com.mx>, 'control.componentes' <control.componentes@construplan.com.mx>, 'javier.marquez' <javier.marquez@construplan.com.mx>, 'ana.vidal' <ana.vidal@construplan.com.mx>, 'ernesto salas' <ernesto.salas@construplan.com.mx>, 'Ruth Vargas Sesteaga' <ruth.sesteagac@gmail.com>, 'carlos.luna' <carlos.luna@construplan.com.mx>, 'amparano' <karen.amparano@construplan.com.mx>, 'sergio.felix' <sergio.felix@construplan.com.mx>, 'ROGELIO PACHECO' <rogelio.pacheco@construplan.com.mx>, 'lorenzo' <anahitis.lorenzo@construplan.com.mx>, 'Martha Mancilla' <martha.mancilla@construplan.com.mx>, 'm.espinoza' <m.espinoza@construplan.com.mx>, 'patiodemaquinaria' <patiodemaquinaria@construplan.com.mx>, 'Silver Ochoa' <silver.ochoa@construplan.com.mx>, 'Raul Hernandez' <raul.hernandez@construplan.com.mx>, 'karen loya' <karen.loya@construplan.com.mx>, 'jesus.cruz' <jesus.cruz@construplan.com.mx>, 'david' <david.bojorquez@construplan.com.mx>, 'cheno' <martha.cheno@construplan.com.mx>, 'walter peralta' <walter.peralta@construplan.com.mx>, 'Diego Ruiz' <diego.ruiz@construplan.com.mx>, 'jose.acosta' <jose.acosta@construplan.com.mx>, 'javier.otero' <javier.otero@construplan.com.mx>, 'alan.gonzalez' <alan.gonzalez@construplan.com.mx>, 'luis.figueroa' <luis.figueroa@construplan.com.mx>, 'patricia.cruz' <patricia.cruz@construplan.com.mx>, 'francisco salazar' <francisco.salazar@construplan.com.mx>, 'alejandro.lugo' <alejandro.lugo@construplan.com.mx>, 'carlos.sainz' <carlos.sainz@construplan.com.mx>, 'jose.yocupicio' <jose.yocupicio@construplan.com.mx>, 'ityaan.garcia' <ityaan.garcia@construplan.com.mx>, 'jorge.vazquez' <jorge.vazquez@construplan.com.mx>, 'Julio Flores' <julio.flores@construplan.com.mx>, 'stephanie.preciado' <stephanie.preciado@construplan.com.mx>, 'marcos.garcia' <marcos.garcia@construplan.com.mx>, 'karla.torres' <karla.torres@construplan.com.mx>, 'alejandro.ruiz' <alejandro.ruiz@construplan.com.mx>, 'jesus.garay' <jesus.garay@construplan.com.mx>, 'jesus.rodriguez' <jesus.rodriguez@construplan.com.mx>, 'ruben ibarra' <ruben.ibarra@construplan.com.mx>'";
        //            // string correos = "jesus.matus@construplan.com.mx";

        //            //    GlobalUtils.sendEmailAdjuntoInMemory2("Reporte de inventario general de maquinaria y equipo del 11 de JULIO al 17 de JULIO del 2018", cuerpo, correos, lista);



        //            //var nombre =excel.CreateExcelFile(this, Sheets, "CONSTRUCCIONES PLANIFICADAS - ");


        //            //return File(nombre, "multipart/form-data");

        //            return null;
        //        }
        #region BajaMaquinaria
        public ActionResult cargarMaquinaria(DateTime inicio, DateTime fin, int tipo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstBajaMaquina = maquinaFactoryServices.getMaquinaServices().FillGridMaquina(inicio, fin, tipo, 0)
                    .Select(x => new BajaMaquinaDTO
                    {
                        Economico = x.noEconomico,
                        Tipo = x.grupoMaquinaria.tipoEquipo.descripcion,
                        Descripcion = x.grupoMaquinaria.descripcion,
                        Marca = x.marca.descripcion,
                        Modelo = x.modeloEquipo.descripcion,
                        NumeroSerie = x.noSerie,
                        Anio = x.anio.ToString(),
                        CentroCostos = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(x.centro_costos),
                        Redireccion = x.estatus != 1 ? "Stand BY" : string.Empty,
                        CargoObra = x.estatus != 1 ? "MAQUINARIA NO ASIGNADA A OBRA" : centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(x.centro_costos),
                        Motivo = x.TipoBajaID == null || x.TipoBajaID == 0 ? "NO ASIGNADO" : tipoBajaFactoryServices.getTipoBajaService().FillCboTipoBaja().Where(m => m.id == x.TipoBajaID).FirstOrDefault().Motivo
                    })
                    .ToList();
                Session["lstBajaMaquina"] = lstBajaMaquina;
                result.Add("lstBajaMaquina", lstBajaMaquina);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        string asignarMotivo(int? id)
        {
            return id == null ? "NO ASIGNADO" : tipoBajaFactoryServices.getTipoBajaService().FillCboTipoBaja().Where(x => x.id == id).FirstOrDefault().Motivo;
        }
        public ActionResult cargarMaquinariaActivoFijo(DateTime inicio, DateTime fin, int tipo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstBajaMaquina = maquinaFactoryServices.getMaquinaServices().FillGridMaquina(inicio, fin, tipo, 0).ToList();
                var lstHorometros = horometroFactoryServices.getCapturaHorometroServices().getHorasSoloRangoFecha(inicio, fin);
                var lstRelacionBaja = new List<RepBajaMaquinaria>();
                foreach (var item in lstBajaMaquina)
                {
                    var horoeco = horometroFactoryServices.getCapturaHorometroServices().GetHorometroFinal(item.noEconomico);


                    var obj = new RepBajaMaquinaria()
                    {
                        Economico = item.noEconomico,
                        Descripcion = item.grupoMaquinaria.tipoEquipo.descripcion,
                        Horometro = horoeco,// horoeco.Where(x => x.Economico.Equals(item.noEconomico)).Count() == 0 ? 0 : horoeco.Where(x => x.Economico.Equals(item.noEconomico)).OrderByDescending(x => x.id).FirstOrDefault().Horometro,
                        Promedio = horoeco,//horoeco.Where(x => x.Economico.Equals(item.noEconomico)).Count() == 0 ? 0 : decimal.Round(horoeco.Where(x => x.Economico.Equals(item.noEconomico)).Average(x => x.Horometro), 2),
                        NoAsignado = item.TipoBajaID == 0 || item.TipoBajaID == null ? true : false,
                        VentaInterna = item.TipoBajaID == 1 && item.TipoBajaID != null ? true : false,
                        VentaExterna = item.TipoBajaID == 2 && item.TipoBajaID != null ? true : false,
                        TerminoVida = item.TipoBajaID == 3 && item.TipoBajaID != null ? true : false,
                        Siniestro = item.TipoBajaID == 4 && item.TipoBajaID != null ? true : false,
                        Robo = item.TipoBajaID == 5 && item.TipoBajaID != null ? true : false,
                        GrupoID = item.grupoMaquinaria.tipoEquipoID
                    };
                    lstRelacionBaja.Add(obj);
                }
                lstRelacionBaja.GroupBy(x => x.Descripcion).ToList();
                var lstContador = lstRelacionBaja.Select(x => new RepRelacionBajaDTO
                {
                    NoAsignado = lstRelacionBaja.Where(y => y.NoAsignado).Count(),
                    VentaInterna = lstRelacionBaja.Where(y => y.VentaInterna).Count(),
                    VentaExterna = lstRelacionBaja.Where(y => y.VentaExterna).Count(),
                    TerminoVida = lstRelacionBaja.Where(y => y.TerminoVida).Count(),
                    Siniestro = lstRelacionBaja.Where(y => y.Siniestro).Count(),
                    Robo = lstRelacionBaja.Where(y => y.Robo).Count(),
                }).FirstOrDefault();
                decimal totalContador = lstContador.NoAsignado + lstContador.VentaInterna + lstContador.VentaExterna + lstContador.TerminoVida + lstContador.Siniestro + lstContador.Robo;
                var lstRelativo = lstRelacionBaja.Select(x => new RepPorcentajeBaja
                {
                    NoAsignado = string.Format("{0:P2}", lstContador.NoAsignado / totalContador),
                    VentaInterna = string.Format("{0:P2}", lstContador.VentaInterna / totalContador),
                    VentaExterna = string.Format("{0:P2}", lstContador.VentaExterna / totalContador),
                    TerminoVida = string.Format("{0:P2}", lstContador.TerminoVida / totalContador),
                    Siniestro = string.Format("{0:P2}", lstContador.Siniestro / totalContador),
                    Robo = string.Format("{0:P2}", lstContador.Robo / totalContador),
                }).FirstOrDefault();
                Session["lstBajaMaquina"] = lstRelacionBaja;
                Session["lstContador"] = lstContador;
                Session["lstRelativo"] = lstRelativo;
                Session["fecha"] = string.Format("Del {0:dd/MM/yyyy} Al {1:dd/MM/yyyy}", inicio, fin);
                result.Add("lstBajaMaquina", lstRelacionBaja);
                result.Add("lstContador", lstContador);
                result.Add("lstRelativo", lstRelativo);
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


        public ActionResult VerificarPermisoEliminarDocumentoEconomico()
        {
            return Json(maquinaFactoryServices.getMaquinaServices().verificarPermisoEliminarDocumentoEconomico(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarArchivoEconomico(int idDocumento)
        {
            return Json(documentosMaquinariaFactoryServices.getDocumentosMaquinariaFactoryServices().eliminarArchivoEconomico(idDocumento), JsonRequestBehavior.AllowGet);
        }
    }
}