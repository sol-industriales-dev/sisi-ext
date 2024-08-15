using Core.DTO.Maquinaria.Overhaul;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Overhaul;
using Core.Entity.Principal.Alertas;
using Core.Enum.Principal.Alertas;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Overhaul;
using Data.Factory.Principal.Alertas;
using Data.Factory.Principal.Archivos;
using Infrastructure.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Core.Enum.Maquinaria;
using Core.Enum.Maquinaria.Overhaul;
using Data.DAO.Principal.Usuarios;
using Core.Entity.Principal.Usuarios;
using Core.DTO.Principal.Usuarios;
using Core.DTO;
using System.Windows.Forms;
using Data.Factory.Maquinaria.Captura;
using System.Data;
using Data.Factory.Administracion.ControlInterno.Almacen;
using Core.Enum;
using Core.DTO.Principal.Generales;
using System.Drawing;
using Core.DTO.Maquinaria.Catalogos;
using Reportes.Reports.Overhaul;
using Data.Factory.Principal.Usuarios;
using System.Net;
using System.Collections.Specialized;
using System.Text;
using Data.EntityFramework.Context;
using Core.Entity.Principal.Multiempresa;

namespace SIGOPLAN.Controllers.Maquinaria.OverHaul
{
    public class OverhaulController : BaseController
    {

        #region Factory
        AlertaFactoryServices alertaFactoryServices;
        ArchivosNotasCreditoFactoryServices archivosNotasCreditoFactoryServices;
        NotaCreditoFactoryServices notaCreditoFactoryServices;
        LocacionesComponentesFactoryServices locacionesComponentesServices;
        AdministracionComponentesFactoryServices administracionComponentesServices;
        MarcasComponentesFactoryServices marcasComponentesServices;
        RemocionComponenteFactoryServices remocionComponenteServices;
        CapturaOTFactoryServices capturaOTServices;
        ComponenteFactoryServices componenteServices;
        InsumoFactoryServices insumoFactoryServices;
        PlaneacionOverhaulFactoryServices planeacionOverhaulFactoryServices;
        AdministracionServiciosFactoryServices administracionServiciosFactoryServices;
        TallerOverhaulFactoryServices tallerOverhaulFactoryServices;
        CapturaHorometroFactoryServices capturaHorometroFactoryServices;
        PresupuestoOverhaulFactoryServices presupuestoOverhaulFactoryServices;
        UsuarioFactoryServices usuarioFactoryServices;

        MaquinaFactoryServices maquinaFactoryServices;
        ArchivoFactoryServices archivofs;
        ModeloEquipoFactoryServices modelofs;
        #endregion

        tblM_CatMaquina objCatMaquina;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            alertaFactoryServices = new AlertaFactoryServices();
            archivosNotasCreditoFactoryServices = new ArchivosNotasCreditoFactoryServices();
            maquinaFactoryServices = new MaquinaFactoryServices();
            notaCreditoFactoryServices = new NotaCreditoFactoryServices();
            locacionesComponentesServices = new LocacionesComponentesFactoryServices();
            administracionComponentesServices = new AdministracionComponentesFactoryServices();
            marcasComponentesServices = new MarcasComponentesFactoryServices();
            remocionComponenteServices = new RemocionComponenteFactoryServices();
            capturaOTServices = new CapturaOTFactoryServices();
            componenteServices = new ComponenteFactoryServices();
            insumoFactoryServices = new InsumoFactoryServices();
            planeacionOverhaulFactoryServices = new PlaneacionOverhaulFactoryServices();
            administracionServiciosFactoryServices = new AdministracionServiciosFactoryServices();
            tallerOverhaulFactoryServices = new TallerOverhaulFactoryServices();
            capturaHorometroFactoryServices = new CapturaHorometroFactoryServices();
            presupuestoOverhaulFactoryServices = new PresupuestoOverhaulFactoryServices();
            usuarioFactoryServices = new UsuarioFactoryServices();
            modelofs = new ModeloEquipoFactoryServices();

            archivofs = new ArchivoFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        // GET: Overhaul
        public ActionResult NotasCredito()
        {
            if (base.getAction("Autorizacion"))
            {
                ViewBag.Autoriza = true;
            }
            else
            {
                ViewBag.Autoriza = false;
            }

            return View();
        }

        public ActionResult ReporteFallaVista()
        {
            return View();
        }

        public ActionResult Locaciones()
        {
            return View();
        }

        public ActionResult AdministracionComponentes()
        {
            return View();
        }

        public ActionResult PlaneacionOverhaulNuevo()
        {
            return View();
        }

        public ActionResult PlaneacionOverhaulTerminado()
        {
            return View();
        }

        public ActionResult PlaneacionOverhaul()
        {
            return View();
        }

        public ActionResult ReportesPlaneacion()
        {
            return View();
        }

        public ActionResult ReportesPresupuesto()
        {
            return View();
        }

        public ActionResult Marcas()
        {
            return View();
        }

        public ActionResult Remocion()
        {
            return View();
        }

        public ActionResult _RemocionPartial(int id)
        {
            Session["idComponenteRemocion"] = id;
            return PartialView();
        }

        public ActionResult ReportesRemocion()
        {
            return View();
        }

        public ActionResult ReportesAdministradorComponentes()
        {
            var usuarioID = getUsuario().id;
            int tipoUsuario = tipoUsuarioOverhaul(usuarioID);
            ViewBag.tipoUsuarioOverhaul = tipoUsuario;
            return View();
        }

        public ActionResult ControlServicios()
        {
            return View();
        }

        public ActionResult TallerOverhaul()
        {
            return View();
        }

        public ActionResult ReportesTaller()
        {
            return View();
        }

        public ActionResult Presupuesto()
        {
            return View();
        }

        public ActionResult Presupuestos()
        {
            return View();
        }

        public ActionResult Historial()
        {
            return View();
        }

        public ActionResult CatActTaller()
        {
            return View();
        }

        //public ActionResult 

        public ActionResult NuevaNotaCredito(tblM_CapNotaCredito obj, bool Tipo, string ComentarioRechazo, string cc)
        {
            var result = new Dictionary<string, object>();
            try
            {

                if (Tipo)
                {
                    HttpPostedFileBase archivo = Request.Files.Count > 0 ? Request.Files[0] : null;

                    if (archivo == null || ComentarioRechazo == null || ComentarioRechazo == "")
                    {
                        throw new Exception("Debe capturar todos los campos.");
                    }

                    //obj = JsonConvert.DeserializeObject<tblM_CapNotaCredito>(Request.Form["obj"]);
                    Tipo = Convert.ToBoolean(Request.Form["Tipo"]);
                    ComentarioRechazo = Request.Form["ComentarioRechazo"].ToString();
                    cc = Request.Form["cc"].ToString();
                    var notaId = Convert.ToInt32(Request.Form["notaId"]);
                    var estado = Convert.ToInt32(Request.Form["estado"]);

                    var estatus = estado;
                    obj = notaCreditoFactoryServices.getNotaCredito().getNotaCreditoById(notaId);
                    obj.cc = cc;
                    obj.Estado = estatus;
                    var objS = new tblM_ComentariosNotaCredito();
                    objS.id = 0;
                    objS.notaCreditoID = obj.id;
                    objS.comentario = ComentarioRechazo;
                    objS.usuarioNombre = getUsuario().nombre; //obj.usuarioNombre; //obj.usuarioNombre;
                    objS.usuarioID = getUsuario().id; // obj.usuarioID;
                    objS.fecha = DateTime.Now;
                    objS.factura = "";
                    if (archivo != null)
                    {
                        var extension = Path.GetExtension(archivo.FileName);
                        var nombreOriginal = archivo.FileName;
                        var FileName = DateTime.Now.ToString("ddMMyyyymmffff_") + nombreOriginal;

                        ArchivoFactoryServices archivofs = new ArchivoFactoryServices();
#if DEBUG
                        string ruta = @"C:\\Proyectos\\SIGOPLAN\\MAQUINARIA\\OVERHAUL\\NOTASCREDITO\\EVIDENCIA_RECHAZO\\";
#else
                    string ruta = archivofs.getArchivo().getUrlDelServidor(6) + "EVIDENCIA_RECHAZO\\";
#endif
                        GlobalUtils.VerificarExisteCarpeta(ruta, true);

                        ruta += FileName;

                        var archivoExiste = GlobalUtils.SaveArchivo(archivo, ruta);


                        objS.nombreEvidencia = FileName;
                    }
                    notaCreditoFactoryServices.getNotaCredito().guardarComentario(objS);
                }

                var idUsuario = base.getUsuario().id;
                obj.idUsuarioModifico = idUsuario;
                obj.FechaCaptura = DateTime.Now;


                notaCreditoFactoryServices.getNotaCredito().Guardar(obj);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditNotaCredito(tblM_CapNotaCredito obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var objNotaCredito = notaCreditoFactoryServices.getNotaCredito().getNotaCreditoById(obj.id);
                var jsonSerialiser = new JavaScriptSerializer();
                obj.Estado = objNotaCredito.Estado;
                obj.FechaCaptura = objNotaCredito.FechaCaptura;
                obj.idUsuarioModifico = getUsuario().id;

                var json = jsonSerialiser.Serialize(obj);
                objNotaCredito.CadenaModifica = json;
                objNotaCredito.EstatusModifica = 1;

                notaCreditoFactoryServices.getNotaCredito().Guardar(objNotaCredito);

                tblP_Alerta objAlerta = new tblP_Alerta();

                objAlerta.msj = "MODIFICACION DE NOTA DE CREDITO";
                objAlerta.sistemaID = 1;
                objAlerta.tipoAlerta = (int)AlertasEnum.REDIRECCION;
                objAlerta.url = "/OT/EditNotaCredito/";
                objAlerta.objID = obj.id;
                objAlerta.userEnviaID = getUsuario().id;
                objAlerta.userRecibeID = 6;

                alertaFactoryServices.getAlertaService().saveAlerta(objAlerta);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AutorizaNotaMod(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = notaCreditoFactoryServices.getNotaCredito().getNotaCreditoById(id);

                obj.CadenaModifica = null;
                obj.EstatusModifica = 0;

                notaCreditoFactoryServices.getNotaCredito().Guardar(obj);


                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public FileResult getFileDownload()
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);

            var archivo = archivosNotasCreditoFactoryServices.getArchivosNotasCreditoFactoryServices().getlistaByID(id);

            var nombre = archivo.nombreArchivo;
            var Ruta = archivo.rutaArchivo;

            return File(Ruta, "multipart/form-data", nombre);
        }
        public ActionResult LoadTableNotasCredito(FiltrosNotasCredito objFiltro)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var resultado = notaCreditoFactoryServices.getNotaCredito().getFillGridNotasCredito(objFiltro).Select(x => new
                {
                    id = x.id,
                    cc = x.cc,
                    Generador = x.Generador,
                    OC = x.OC,
                    Factura = x.factura ?? "",
                    idEquipo = x.idEconomico != 0 ? getMaquinaria(x.idEconomico) : 0,
                    Equipo = x.idEconomico != 0 ? objCatMaquina.noEconomico : "",
                    Modelo = x.idEconomico != 0 ? objCatMaquina.modeloEquipo.descripcion : "",
                    Almacen = x.noAlmacen,
                    SerieEquipo = x.idEconomico != 0 ? objCatMaquina.noSerie : "",
                    SerieComponente = x.SerieComponente,
                    Descripcion = x.Descripcion,
                    Fecha = x.Fecha,
                    CausaRemosion = x.CausaRemosion == 1 ? "Programada" : "falla",
                    HorometroEquipo = x.HorometroEconomico.ToString("0,0", CultureInfo.InvariantCulture),
                    HoraComponente = x.HorometroComponente.ToString("0,0", CultureInfo.InvariantCulture),
                    MontoPesos = x.MontoPesos > 0 ? x.MontoPesos.ToString("C2") : "$0",
                    MontoDLL = x.MontoDLL > 0 ? x.MontoDLL.ToString("C2") : "$0",
                    AbonoDLL = x.AbonoDLL > 0 ? x.AbonoDLL.ToString("C2") : "$0",
                    ClaveCredito = x.ClaveCredito,
                    Estado = x.Estado,
                    Acciones = getSelectores(x.id, x.Estado, x.EstatusModifica),
                    EstatusModifica = x.EstatusModifica,
                    TipoNC = x.TipoNC == 1 ? "Nota de credito" : x.TipoNC == 2 ? "Casco Reman" : "",
                    MontoTotalOC = x.montoTotalOC > 0 ? x.montoTotalOC.ToString("C2") : "$0",
                    Diferencia = (x.montoTotalOC - x.AbonoDLL).ToString("C2")
                    //Diferencia = (x.MontoPesos - x.montoTotalOC) > 0 ? (x.MontoPesos - x.montoTotalOC).ToString("C2") : "$0"
                }).OrderByDescending(x => x.EstatusModifica).ToList();

                result.Add("tblNotaCredito", resultado);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        private string getSelectores(int id, int estado, int estadoM)
        {
            string archivo = "";

            switch (estado)
            {
                case 1:


                    if (getAction("VerArchivos"))
                    {
                        archivo += "<button class='btn btn-sm btn-info btnListaArchivos' type='button' data-id='" + id + "' style='margin-right: 2px;'> " +
                                 "<i class='glyphicon glyphicon-comment' title='Archivos'></i></button>";
                    }

                    if (getAction("AddComentarios"))
                    {
                        archivo += "<button class='btn btn-sm btn-info btnAddComentariosPendientes' type='button' data-id='" + id + "' style='margin-right: 2px;'> " +
                              "<i class='glyphicon glyphicon-comment' title='Comentarios'></i></button>";
                    }
                    if (base.getAction("SubirAchivos"))
                    {
                        archivo += "<button class='btn btn-sm btn-info btnListaArchivos' type='button' data-id='" + id + "' style='margin-right: 2px;'> " +
                               "<i class='glyphicon glyphicon-list-alt' title='Subir archivos'></i></button>";
                    }

                    return "<div> <div class='btn-group' role='group' aria-label='Basic example'>" +
                             "<button class='btn btn-sm btn-success setAutorizacion' title='Abonar' type='button' style='margin-right: 2px;' data-id='" + id + "'> " +
                             "<span class='glyphicon glyphicon-ok'></span></button>" +
                             "<button class='btn btn-sm btn-danger denegado' title='Rechazar' type='button' data-id='" + id + "'> " +
                             "<i class='fas fa-ban'></i></button> " +
                             archivo +
                             " </div></div>";
                case 2:
                    {
                        var HasFactura = notaCreditoFactoryServices.getNotaCredito().getComentarios(id, 1);
                        var nc = notaCreditoFactoryServices.getNotaCredito().getNotaCreditoById(id);
                        var numFacturas = HasFactura.Where(x => x.factura != "").Count();
                        string ColorBtn = "btn-info";
                        string btnColor2 = "btn-warning";
                        if (numFacturas > 0)
                        {
                            ColorBtn = "btn-success";
                        }

                        if (base.getAction("SubirAchivos"))
                        {
                            archivo = "<button class='btn btn-sm btn-info btnListaArchivos' type='button' data-id='" + id + "' data-oc='" + (string.IsNullOrEmpty(nc.OC) ? "no" : nc.OC) + "' data-factura='" + (string.IsNullOrEmpty(nc.factura) ? "no" : nc.factura) + "'> " +
                                   "<i class='glyphicon glyphicon-list-alt' title='Subir archivos'></i></button>";
                        }
                        if (base.getAction("Autorizacion") && estadoM == 1)
                        {
                            btnColor2 = "bgColorAlerta";

                        }
                        archivo += "<button class='btn btn-sm " + btnColor2 + " btnEditar'  title='Actualizar' type='button' data-id='" + id + "'> " +
                                                "<i class='glyphicon glyphicon-edit' aria-hidden='true'></i></button>";
                        return "<div><div class='btn-group' role='group' aria-label='Basic example'><button class='btn btn-sm " + ColorBtn + " CbtnComentario' title='Comentarios' type='button' data-id='" + id + "'> " +
                                     "<i class='glyphicon glyphicon-comment'></i></button> " + archivo + "</div></div>";
                    }
                case 3: return "<div><div class='btn-group' role='group' aria-label='Basic example'><button class='btn btn-sm btn-info CbtnComentario' type='button' data-id='" + id + "' > " +
                                  "<i class='glyphicon glyphicon-comment' title='Comentarios'></i></button> " +
                                  "<button class='btn btn-sm btn-info btnComentarioRechazo' type='button' data-id='" + id + "' > " +
                                  "<i class='fas fa-comment-slash' title='Comentario rechazo'></i></button></div>";
                case 4:
                    {
                        var HasFactura = notaCreditoFactoryServices.getNotaCredito().getComentarios(id, 1);

                        var numFacturas = HasFactura.Where(x => x.factura != "").Count();
                        string ColorBtn = "btn-info";
                        string btnColor2 = "btn-warning";
                        if (numFacturas > 0)
                        {
                            ColorBtn = "btn-success";
                        }

                        if (base.getAction("SubirAchivos"))
                        {
                            archivo = "<button class='btn btn-sm btn-info btnListaArchivos' type='button' data-id='" + id + "'> " +
                                   "<i class='glyphicon glyphicon-list-alt' title='Subir archivos'></i></button>";
                        }
                        if (base.getAction("Autorizacion") && estadoM == 1)
                        {
                            btnColor2 = "bgColorAlerta";

                        }
                        archivo += "<button class='btn  btn-sm " + btnColor2 + " btnEditar'  title='Actualizar' type='button' data-id='" + id + "'> " +
                                                "<i class='glyphicon glyphicon-edit' aria-hidden='true'></i></button>";
                        return "<div><div class='btn-group' role='group' aria-label='Basic example'><button class='btn  btn-sm " + ColorBtn + " CbtnComentario' title='Comentarios' type='button' data-id='" + id + "'> " +
                                     "<i class='glyphicon glyphicon-comment'></i></button> " + archivo + "</div></div>";
                    }
                default:
                    return "";
            }

        }
        private int getMaquinaria(int id)
        {
            objCatMaquina = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(id).FirstOrDefault();


            return id;
        }

        public MemoryStream DescargarEvidencia(int id, string nombreEvidencia)
        {
            string ruta;
#if DEBUG

            ArchivoFactoryServices archivofs = new ArchivoFactoryServices();
            ruta = @"C:\\Proyectos\\SIGOPLAN\\MAQUINARIA\\OVERHAUL\\NOTASCREDITO\\EVIDENCIA_RECHAZO\\" + nombreEvidencia;
#else
            ruta = archivofs.getArchivo().getUrlDelServidor(6) + "EVIDENCIA_RECHAZO\\" + nombreEvidencia;
#endif

            var stream = GlobalUtils.GetFileAsStream(ruta);
            var fileBytes = GlobalUtils.ConvertFileToByte(stream);
            Response.Clear();
            var mime = MimeMapping.GetMimeMapping(nombreEvidencia);
            Response.ContentType = mime;
            Response.AddHeader("Content-Disposition", "attachement; filename=" + nombreEvidencia);
            Response.BinaryWrite(fileBytes);
            Response.End();
            return null;
        }

        public ActionResult cboModalEconomico()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, maquinaFactoryServices.getMaquinaServices().getCboMaquinaria("0").Select(x => new { Value = x.id, Text = x.noEconomico }).OrderBy(x => x.Text));
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadInfoMaquinaria(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var DatosEconomico = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(id).Select(x => new
                {
                    Modelo = x.modeloEquipo.descripcion,
                    Serie = x.noSerie
                }).FirstOrDefault();

                result.Add("DatosEconomico", DatosEconomico);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDataNotaCredito(int obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var tblM_CapNotaCredito = notaCreditoFactoryServices.getNotaCredito().getNotaCreditoById(obj);

                result.Add("Fecha", tblM_CapNotaCredito.Fecha.ToShortDateString());

                if (tblM_CapNotaCredito.fechaCasco != null)
                    result.Add("fechaCasco", ((DateTime)tblM_CapNotaCredito.fechaCasco).ToShortDateString());
                result.Add("tblM_CapNotaCredito", tblM_CapNotaCredito);
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
        public ActionResult GuardarAutorizacion()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var idNotaCredito = JsonConvert.DeserializeObject<int>(Request.Form["idNotaCredito"].ToString());
                var AbonoDLL = JsonConvert.DeserializeObject<decimal>(Request.Form["AbonoDLL"].ToString());
                var ClaveCredito = JsonConvert.DeserializeObject<string>(Request.Form["ClaveCredito"].ToString());
                var OC = JsonConvert.DeserializeObject<string>(Request.Form["OC"].ToString());
                var cc = JsonConvert.DeserializeObject<string>(Request.Form["cc"].ToString());

                if (Request.Files.Count == 0)
                {
                    throw new Exception("Debe anexar un archivo.");
                }

                var tblM_capNotaCredito = notaCreditoFactoryServices.getNotaCredito().getNotaCreditoById(idNotaCredito);
                tblM_capNotaCredito.cc = cc;
                if (string.IsNullOrEmpty(tblM_capNotaCredito.OC))
                {
                    tblM_capNotaCredito.OC = OC;
                }

                if (!string.IsNullOrEmpty(tblM_capNotaCredito.OC) || !string.IsNullOrEmpty(tblM_capNotaCredito.Generador))
                {

                    var EconomicoObj = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(tblM_capNotaCredito.idEconomico).FirstOrDefault();
                    tblM_capNotaCredito.AbonoDLL = AbonoDLL;

                    if (tblM_capNotaCredito.TipoNC == 2)
                    {
                        var tipoCambio = notaCreditoFactoryServices.getNotaCredito().TipoCambio(tblM_capNotaCredito.Fecha.ToString("yyyy-MM-dd"));
                        tblM_capNotaCredito.MontoPesos = (AbonoDLL * tipoCambio);
                        tblM_capNotaCredito.MontoDLL = AbonoDLL;
                    }
                    tblM_capNotaCredito.ClaveCredito = ClaveCredito;
                    tblM_capNotaCredito.Estado = 2;
                    tblM_capNotaCredito.FechaCaptura = DateTime.Now;
                    tblM_capNotaCredito.CadenaModifica = "";
                    tblM_capNotaCredito.EstatusModifica = 0;
                    notaCreditoFactoryServices.getNotaCredito().Guardar(tblM_capNotaCredito);

                    List<string> ListaAdjuntos = new List<string>();
                    if (Request.Files.Count > 0)
                    {
                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            HttpPostedFileBase archivo = Request.Files[i];
                            string extension = Path.GetExtension(archivo.FileName);
                            string nombreOriginal = archivo.FileName;
                            string FileName = tblM_capNotaCredito.id + nombreOriginal;

                            string Ruta = archivofs.getArchivo().getUrlDelServidor(6) + FileName;

                            var archivoExiste = notaCreditoFactoryServices.getNotaCredito().SaveArchivo(archivo, Ruta);


                            tblM_ArchivosNotasCredito archivosNotasCredito = new tblM_ArchivosNotasCredito();
                            if (archivoExiste)
                            {
                                archivosNotasCredito.FechaSubida = DateTime.Now;
                                archivosNotasCredito.id = 0;
                                archivosNotasCredito.nombreArchivo = nombreOriginal;
                                archivosNotasCredito.NotaCreditoID = tblM_capNotaCredito.id;
                                archivosNotasCredito.rutaArchivo = Ruta;
                                archivosNotasCredito.tipoArchivo = 1;
                                archivosNotasCredito.usuario = getUsuario().id;

                                archivosNotasCreditoFactoryServices.getArchivosNotasCreditoFactoryServices().Guardar(archivosNotasCredito);
                                ListaAdjuntos.Add(Ruta);

                            }
                        }
                    }

                    var listArchivosNota = archivosNotasCreditoFactoryServices.getArchivosNotasCreditoFactoryServices().getlistaArchivosAdjuntos(tblM_capNotaCredito.id);

                    foreach (string Ruta in listArchivosNota)
                    {
                        ListaAdjuntos.Add(Ruta);
                    }

                    var today = DateTime.Today;
                    var month = new DateTime(today.Year, today.Month == 12 ? today.Month : today.Month + 1, 1);
                    var first = month.AddMonths(-1);
                    var last = month.AddDays(-1);

                    List<string> CadenaCorreos = new List<string>();

                    var data = notaCreditoFactoryServices.getNotaCredito().GetNotasCreditoRpt(first, last, (int)tblM_capNotaCredito.TipoNC, 2, cc, "").Select(
                                x => new
                                {
                                    Generador = x.Generador,
                                    OC = x.OC,
                                    SerieComponente = x.SerieComponente,
                                    Descripcion = x.Descripcion,
                                    Fecha = x.Fecha.ToShortDateString(),
                                    CausaRemosion = x.CausaRemosion == 1 ? "Programada" : "Falla",
                                    HorometroEquipo = x.HorometroEconomico,
                                    HorometroComponente = x.HorometroComponente,
                                    MontoPesos = x.MontoPesos,
                                    MontoDLL = x.MontoDLL,
                                    AbonoDLL = x.AbonoDLL,
                                    NoCredito = x.ClaveCredito,
                                    Comentario = x.Estado == 1 ? "Proceso" : x.Estado == 2 ? "Abonado" : "Denegado",
                                    GrupoMes = x.FechaCaptura.Month,
                                    DescripcionMes = x.FechaCaptura.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper(),
                                    Anio = x.FechaCaptura.Year,
                                    fechaCierre = x.FechaCaptura.ToShortDateString()

                                }).ToList();

                    // CadenaCorreos.Add("jesus.matus@construplan.com.mx");
                    //CadenaCorreos.Add("alma.pelaez@construplan.com.mx");
                    CadenaCorreos.Add("notas.credito@construplan.com.mx");
                    var data4 = notaCreditoFactoryServices.getNotaCredito().GetNotasCreditoRpt(first, last, (int)tblM_capNotaCredito.TipoNC, 4, cc, "").ToList();
                    var TotalAcumulado = data.Sum(x => x.AbonoDLL).ToString("C2");
                    var TotalAcumulado4 = data4.Sum(x => x.AbonoDLL).ToString("C2");
                    var TotalAcumuladoTotal = (data.Sum(x => x.AbonoDLL) + data4.Sum(x => x.AbonoDLL)).ToString("C2");
                    var FechaMES = DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper();
                    var CountCasos = data.Count();
                    var CountCasos4 = data4.Count();
                    var CountCasosTotal = CountCasos + CountCasos4;
                    var Año = DateTime.Now.Year;

                    DateTime InicioAño = new DateTime(Año, 1, 1);
                    DateTime FinAnio = new DateTime(Año, 12, 31);
                    var acumyear = notaCreditoFactoryServices.getNotaCredito().GetNotasCreditoRpt(InicioAño, FinAnio, (int)tblM_capNotaCredito.TipoNC, 2, cc, "");
                    var TotalAñodlls = acumyear.Sum(x => x.AbonoDLL).ToString("C2");
                    var acumyear4 = notaCreditoFactoryServices.getNotaCredito().GetNotasCreditoRpt(InicioAño, FinAnio, (int)tblM_capNotaCredito.TipoNC, 4, cc, "");
                    var TotalAñodlls4 = acumyear4.Sum(x => x.AbonoDLL).ToString("C2");
                    var TotalAñodllsTotal = (acumyear.Sum(x => x.AbonoDLL) + acumyear4.Sum(x => x.AbonoDLL)).ToString("C2");
                    var FechaNotaCredito = tblM_capNotaCredito.Fecha.ToString("dd-MM-yyyy");
                    string mensaje = @"<html xmlns:v='urn:schemas-microsoft-com:vml' xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:word' xmlns:x='urn:schemas-microsoft-com:office:excel' xmlns:m='http://schemas.microsoft.com/office/2004/12/omml' xmlns='http://www.w3.org/TR/REC-html40'>

<head>
    <meta charset='UTF-8'>
    <style>
    <!--
    /* Font Definitions */
    
    @font-face {
        font-family: Helvetica;
        panose-1: 2 11 6 4 2 2 2 2 2 4;
    }
    
    @font-face {
        font-family: 'Cambria Math';
        panose-1: 2 4 5 3 5 4 6 3 2 4;
    }
    
    @font-face {
        font-family: Calibri;
        panose-1: 2 15 5 2 2 2 4 3 2 4;
    }
    /* Style Definitions */
    
    p.MsoNormal,
    li.MsoNormal,
    div.MsoNormal {
        margin: 0cm;
        margin-bottom: .0001pt;
        font-size: 11.0pt;
        font-family: 'Calibri', sans-serif;
        mso-fareast-language: EN-US;
    }
    
    a:link,
    span.MsoHyperlink {
        mso-style-priority: 99;
        color: #0563C1;
        text-decoration: underline;
    }
    
    a:visited,
    span.MsoHyperlinkFollowed {
        mso-style-priority: 99;
        color: #954F72;
        text-decoration: underline;
    }
    
    span.EstiloCorreo17 {
        mso-style-type: personal-compose;
        font-family: 'Calibri', sans-serif;
        color: windowtext;
    }
    
    .MsoChpDefault {
        mso-style-type: export-only;
        mso-fareast-language: EN-US;
    }
    
    @page WordSection1 {
        size: 612.0pt 792.0pt;
        margin: 70.85pt 3.0cm 70.85pt 3.0cm;
    }
    
    div.WordSection1 {
        page: WordSection1;
    }
    
    -->
    </style>
    <!--[if gte mso 9]><xml>
<o:shapedefaults v:ext='edit' spidmax='1026' />
</xml><![endif]-->
    <!--[if gte mso 9]><xml>
<o:shapelayout v:ext='edit'>
<o:idmap v:ext='edit' data='1' />
</o:shapelayout></xml><![endif]-->
</head>

<body lang=ES-MX link='#0563C1' vlink='#954F72'>
    <div class=WordSection1>
        <p class=MsoNormal>
            <o:p>Buen Día estimado usuario</o:p>
        </p>
        <p class=MsoNormal>
            <o:p>&nbsp;</o:p>
        </p>
                <p class=MsoNormal>Se cerró " + (tblM_capNotaCredito.TipoNC == 1 ? ("una Nota de Credito") : ("un Casco Reman")) + " por la cantidad de <b>" + tblM_capNotaCredito.AbonoDLL + @"</b>  dlls.  Durante el mes de <b>" + FechaMES + "</b> hay un total acumulado de <b>" + TotalAcumuladoTotal + @"</b> dlls con <b>" + CountCasosTotal + " casos</b> componiendose de (Abonado: " + TotalAcumulado + " con " + CountCasos + " + Aplicado: " + TotalAcumulado4 + " con " + CountCasos4 + @" casos)
            <o:p></o:p>
        </p>
        <p class=MsoNormal>
            <o:p>&nbsp;</o:p>
        </p>
                <p class=MsoNormal>En lo que va del año <b>" + Año + @"  </b> se tiene un total de <b>" + TotalAñodllsTotal + @" dlls componiendose de (Abonado: " + TotalAñodlls + " + Aplicado: " + TotalAñodlls4 + @") <o:p></o:p></b></p>
        <p class=MsoNormal><b><o:p>&nbsp;</o:p></b></p>
        <p class=MsoNormal>
            <o:p>&nbsp;</o:p>
        </p>
        <table class=MsoTableGrid border=1 cellspacing=0 cellpadding=0 style='margin-left:.3pt;border-collapse:collapse;border:none'>
            <tbody>
                <tr style='height:53.6pt'>
                    <td width=119 style='width:97.4pt;border:solid windowtext 1.0pt;background:#ED7D31;padding:0cm 5.4pt 0cm 5.4pt;height:53.55pt'>
                        <p class=MsoNormal align=center style='text-align:center'><b><span style='font-size:12.0pt;font-family:'Arial',sans-serif'>GENERADOR<o:p></o:p></span></b></p>
                    </td>
                    <td width=119 style='width:97.4pt;border:solid windowtext 1.0pt;border-top:none;padding:0cm 5.4pt 0cm 5.4pt;height:53.6pt'>
                        <p class=MsoNormal align=center style='text-align:center'><span style='font-size:10.0pt;font-family:' Arial ',sans-serif;mso-fareast-language:ES-MX'>" + tblM_capNotaCredito.Generador != null ? tblM_capNotaCredito.Generador : "" + @"<o:p></o:p></span></p>
                    </td>
                </tr>
                <tr>
                    <td width=60 style='width:71.55pt;border:solid windowtext 1.0pt;border-left:none;background:#ED7D31;padding:0cm 5.4pt 0cm 5.4pt;height:53.55pt'>
                        <p class=MsoNormal align=center style='text-align:center'><b><span style='font-size:12.0pt;font-family:'Arial',sans-serif'>OC<o:p></o:p></span></b></p>
                    </td>
                    <td width=60 style='width:71.55pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:53.6pt'>
                        <p class=MsoNormal align=center style='text-align:center'><span style='font-size:10.0pt;font-family:' Arial ',sans-serif;mso-fareast-language:ES-MX'>" + tblM_capNotaCredito.OC + @"<o:p></o:p></span></p>
                    </td>
                </tr>
                <tr>
                    <td width=76 style='width:63.7pt;border:solid windowtext 1.0pt;border-left:none;background:#ED7D31;padding:0cm 5.4pt 0cm 5.4pt;height:53.55pt'>
                        <p class=MsoNormal align=center style='text-align:center'><b><span style='font-size:12.0pt;font-family:'Arial',sans-serif'>EQUIPO<o:p></o:p></span></b></p>
                    </td>
                    <td width=76 style='width:63.7pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:53.6pt'>
                        <p class=MsoNormal align=center style='text-align:center'><span style='font-size:10.0pt;font-family:' Arial ',sans-serif;mso-fareast-language:ES-MX'>" + EconomicoObj != null ? EconomicoObj.grupoMaquinaria.descripcion : "" + @"<o:p></o:p></span></p>
                    </td>
                </tr>
                <tr>
                                    <td width=84 style='width:70.8pt;border:solid windowtext 1.0pt;border-left:none;background:#ED7D31;padding:0cm 5.4pt 0cm 5.4pt;height:53.55pt'>
                        <p class=MsoNormal align=center style='text-align:center'><b><span style='font-size:12.0pt;font-family:'Arial',sans-serif'>MODELO<o:p></o:p></span></b></p>
                    </td>
                <td width=84 style='width:70.8pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:53.6pt'>
                    <p class=MsoNormal align=center style='text-align:center'><span style='font-size:10.0pt;font-family:' Arial ',sans-serif;mso-fareast-language:ES-MX'>" + EconomicoObj != null ? EconomicoObj.modeloEquipo.descripcion : "" + @"<o:p></o:p></span></p>
                </td>
                </tr>
                <tr>
                 <td width=159 style='width:120.45pt;border:solid windowtext 1.0pt;border-left:none;background:#ED7D31;padding:0cm 5.4pt 0cm 5.4pt;height:53.55pt'>
                        <p class=MsoNormal align=center style='text-align:center'><b><span style='font-size:12.0pt;font-family:'Arial',sans-serif'>SERIE DEL EQUIPO<o:p></o:p></span></b></p>
                    </td>
                <td width=159 style='width:120.45pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:53.6pt'>
                    <p class=MsoNormal align=center style='text-align:center'><span style='font-size:10.5pt;font-family:' Helvetica ',sans-serif;color:#333333;background:#F9F9F9'>" + EconomicoObj != null ? EconomicoObj.noSerie : "" + @"</span><span style='font-size:10.0pt;font-family:' Arial ',sans-serif;mso-fareast-language:ES-MX'><o:p></o:p></span></p>
                </td>
                </tr>
                <tr>
                   <td width=131 style='width:127.25pt;border:solid windowtext 1.0pt;border-left:none;background:#ED7D31;padding:0cm 5.4pt 0cm 5.4pt;height:53.55pt'>
                        <p class=MsoNormal align=center style='text-align:center'><b><span style='font-size:12.0pt;font-family:'Arial',sans-serif'>SERIE DEL COMPONENTE<o:p></o:p></span></b></p>
                    </td>
                <td width=131 style='width:127.25pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:53.6pt'>
                    <p class=MsoNormal align=center style='text-align:center'><span style='font-size:10.0pt;font-family:' Arial ',sans-serif;mso-fareast-language:ES-MX'>" + tblM_capNotaCredito.SerieComponente != null ? tblM_capNotaCredito.SerieComponente : "" + @"<o:p></o:p></span></p>
                </td></tr>
                <tr>
					<td width=127 style='width:113.15pt;border:solid windowtext 1.0pt;border-left:none;background:#ED7D31;padding:0cm 5.4pt 0cm 5.4pt;height:53.55pt'>
                        <p class=MsoNormal align=center style='text-align:center'><b><span style='font-size:12.0pt;font-family:'Arial',sans-serif'>DESCRIPCION<o:p></o:p></span></b></p>
                    </td>
                <td width=127 style='width:113.15pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:53.6pt'>
                    <p class=MsoNormal align=center style='text-align:center'><span style='font-size:10.0pt;font-family:' Arial ',sans-serif;mso-fareast-language:ES-MX'>" + tblM_capNotaCredito.Descripcion != null ? tblM_capNotaCredito.Descripcion : "" + @" <o:p></o:p></span></p>
                </td>
                </tr>
                <tr>
                 <td width=78 style='width:63.8pt;border:solid windowtext 1.0pt;border-left:none;background:#ED7D31;padding:0cm 5.4pt 0cm 5.4pt;height:53.55pt'>
                        <p class=MsoNormal align=center style='text-align:center'><b><span style='font-size:12.0pt;font-family:'Arial',sans-serif'>FECHA<o:p></o:p></span></b></p>
                    </td>
                <td width=78 style='width:63.8pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:53.6pt'>
                    <p class=MsoNormal align=center style='text-align:center'><span style='font-size:10.0pt;font-family:' Arial ',sans-serif;mso-fareast-language:ES-MX'>" + FechaNotaCredito + @"<o:p></o:p></span></p>
                </td></tr>
                <tr>
                <td width=109 style='width:92.0pt;border:solid windowtext 1.0pt;border-left:none;background:#ED7D31;padding:0cm 5.4pt 0cm 5.4pt;height:53.55pt'>
                        <p class=MsoNormal align=center style='text-align:center'><b><span style='font-size:12.0pt;font-family:'Arial',sans-serif'>CAUSA DE REMOCIÓN<o:p></o:p></span></b></p>
                    </td>
                <td width=109 style='width:92.0pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:53.6pt'>
                    <p class=MsoNormal align=center style='text-align:center'><span style='font-size:10.0pt;font-family:' Arial ',sans-serif;mso-fareast-language:ES-MX'>" + (tblM_capNotaCredito.CausaRemosion == 1 ? "Programada" : "Falla") + @"<o:p></o:p></span></p>
                </td>
                </tr>
                <tr>
                <td width=121 style='width:88.7pt;border:solid windowtext 1.0pt;border-left:none;background:#ED7D31;padding:0cm 5.4pt 0cm 5.4pt;height:53.55pt'>
                        <p class=MsoNormal align=center style='text-align:center'><b><span style='font-size:12.0pt;font-family:'Arial',sans-serif'>HOROMETRO DEL EQUIPO<o:p></o:p></span></b></p>
                    </td>
                <td width=121 style='width:88.7pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:53.6pt'>
                    <p class=MsoNormal align=center style='text-align:center'><span style='font-size:10.0pt;font-family:' Arial ',sans-serif;mso-fareast-language:ES-MX'>" + tblM_capNotaCredito.HorometroEconomico.ToString("0,0", CultureInfo.InvariantCulture) + @"<o:p></o:p></span></p>
                </td>
                </tr>
                <tr>
                  <td width=131 style='width:94.55pt;border:solid windowtext 1.0pt;border-left:none;background:#ED7D31;padding:0cm 5.4pt 0cm 5.4pt;height:53.55pt'>
                        <p class=MsoNormal align=center style='text-align:center'><b><span style='font-size:12.0pt;font-family:'Arial',sans-serif'>HOROMETRO COMPONENTE<o:p></o:p></span></b></p>
                    </td>
                <td width=131 style='width:94.55pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:53.6pt'>
                    <p class=MsoNormal align=center style='text-align:center'><span style='font-size:10.0pt;font-family:' Arial ',sans-serif;mso-fareast-language:ES-MX'>" + tblM_capNotaCredito.HorometroComponente.ToString("0,0", CultureInfo.InvariantCulture) + @"<o:p></o:p></span></p>
                </td>
                </tr>
                <tr>
                                   <td width=108 style='width:122.9pt;border:solid windowtext 1.0pt;border-left:none;background:#ED7D31;padding:0cm 5.4pt 0cm 5.4pt;height:53.55pt'>
                        <p class=MsoNormal align=center style='text-align:center'><b><span style='font-size:12.0pt;font-family:'Arial',sans-serif'>MONTO TOTAL DEL GENRADOR EN PESOS<o:p></o:p></span></b></p>
                    </td>
                <td width=108 style='width:122.9pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:53.6pt'>
                    <p class=MsoNormal align=center style='text-align:center'><span style='font-size:10.0pt;font-family:' Arial ',sans-serif;mso-fareast-language:ES-MX'> " + tblM_capNotaCredito.MontoPesos.ToString("C2") + @" <o:p></o:p></span></p>
                </td>
                </tr>
                <tr>
                                    <td width=119 style='width:99.2pt;border:solid windowtext 1.0pt;border-left:none;background:#ED7D31;padding:0cm 5.4pt 0cm 5.4pt;height:53.55pt'>
                        <p class=MsoNormal align=center style='text-align:center'><b><span style='font-size:12.0pt;font-family:'Arial',sans-serif'>MONTO TOTAL DEL GENERADOR EN dlls<o:p></o:p></span></b></p>
                    </td>
                <td width=119 style='width:99.2pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:53.6pt'>
                    <p class=MsoNormal align=center style='text-align:center'><span style='font-size:10.0pt;font-family:' Arial ',sans-serif;mso-fareast-language:ES-MX'> " + tblM_capNotaCredito.MontoDLL.ToString("C2") + @" <o:p></o:p></span></p>
                </td>
                </tr>
                <tr>
                 <td width=96 style='width:3.0cm;border:solid windowtext 1.0pt;border-left:none;background:#ED7D31;padding:0cm 5.4pt 0cm 5.4pt;height:53.55pt'>
                        <p class=MsoNormal align=center style='text-align:center'><b><span style='font-size:12.0pt;font-family:'Arial',sans-serif'>ABONADO EN<o:p></o:p></span></b></p>
                        <p class=MsoNormal align=center style='text-align:center'><b><span style='font-size:12.0pt;font-family:'Arial',sans-serif'>dlls<o:p></o:p></span></b></p>
                    </td>
                <td width=96 style='width:3.0cm;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:53.6pt'>
                    <p class=MsoNormal align=center style='text-align:center'><span style='font-size:10.0pt;font-family:' Arial ',sans-serif;mso-fareast-language:ES-MX'> " + tblM_capNotaCredito.AbonoDLL.ToString("C2") + @" <o:p></o:p></span></p>
                </td>
                </tr>
                <tr>
                <td width=67 style='width:92.15pt;border:solid windowtext 1.0pt;border-left:none;background:#ED7D31;padding:0cm 5.4pt 0cm 5.4pt;height:53.55pt'>
                        <p class=MsoNormal align=center style='text-align:center'><b><span style='font-size:12.0pt;font-family:'Arial',sans-serif'># De credito<o:p></o:p></span></b></p>
                    </td>
                <td width=67 style='width:92.15pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:53.6pt'>
                    <p class=MsoNormal align=center style='text-align:center'><span style='font-size:10.0pt;font-family:' Arial ',sans-serif;mso-fareast-language:ES-MX'> " + tblM_capNotaCredito.ClaveCredito + @" <o:p></o:p></span></p>
                </td>
                </tr>
                <tr>
                <td width=112 style='width:3.0cm;border:solid windowtext 1.0pt;border-left:none;background:#ED7D31;padding:0cm 5.4pt 0cm 5.4pt;height:53.55pt'>
                        <p class=MsoNormal align=center style='text-align:center'><b><span style='font-size:12.0pt;font-family:'Arial',sans-serif'>Comentarios<o:p></o:p></span></b></p>
                    </td>
                <td width=112 style='width:3.0cm;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:53.6pt'>
                    <p class=MsoNormal align=center style='text-align:center'><span style='font-size:10.0pt;font-family:' Arial ',sans-serif;mso-fareast-language:ES-MX'>" + (tblM_capNotaCredito.Estado == 1 ? "Proceso" : tblM_capNotaCredito.Estado == 2 ? "Abonado" : "Denegado") + @"<o:p></o:p></span></p>
                </td>
                </tr>

            </tbody>
        </table>
        <p class=MsoNormal>
            <o:p>&nbsp;</o:p>
        </p>
  <p class=MsoNormal>
            <o:p>&nbsp;</o:p>
        </p>
          <p class=MsoNormal>
            <o:p>Saludos.</o:p>
        </p>
          <p class=MsoNormal>
            <o:p> <b>No puede enviar respuesta a esta direccion de correo, es una alerta de sistema sigoplan. Para observaciones, aclaraciones o dudas contactar area de TI Construplan.</b></o:p>
        </p>
    </div>
</body>
</html>";
                    GlobalUtils.sendEmailAdjunto("Aviso Nota de Credito", mensaje, CadenaCorreos, ListaAdjuntos);
                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add(MESSAGE, "Se encontraron datos faltantes.");
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
        [HttpPost]
        public ActionResult SubirNuevoArchivo()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var idNotaCredito = JsonConvert.DeserializeObject<int>(Request.Form["idNotaCredito"].ToString());
                var TipoArchivo = JsonConvert.DeserializeObject<int>(Request.Form["TipoArchivo"].ToString());
                HttpPostedFileBase file = Request.Files["fupAdjunto"];
                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase archivo = Request.Files[i];
                        string extension = Path.GetExtension(archivo.FileName);
                        string nombreOriginal = archivo.FileName;
                        string FileName = nombreOriginal + idNotaCredito + "." + extension;

                        string Ruta = archivofs.getArchivo().getUrlDelServidor(6) + FileName;

                        var archivoExiste = notaCreditoFactoryServices.getNotaCredito().SaveArchivo(archivo, Ruta);

                        tblM_ArchivosNotasCredito archivosNotasCredito = new tblM_ArchivosNotasCredito();
                        if (archivoExiste)
                        {
                            archivosNotasCredito.FechaSubida = DateTime.Now;
                            archivosNotasCredito.id = 0;
                            archivosNotasCredito.nombreArchivo = nombreOriginal;
                            archivosNotasCredito.NotaCreditoID = idNotaCredito;
                            archivosNotasCredito.rutaArchivo = Ruta;
                            archivosNotasCredito.tipoArchivo = TipoArchivo;
                            archivosNotasCredito.usuario = getUsuario().id;

                            archivosNotasCreditoFactoryServices.getArchivosNotasCreditoFactoryServices().Guardar(archivosNotasCredito);
                        }
                    }
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
        public ActionResult getListaArchivos(int obj/*idNotaCredito*/)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = archivosNotasCreditoFactoryServices.getArchivosNotasCreditoFactoryServices().getlistaByNota(obj);
                var notaCredito = notaCreditoFactoryServices.getNotaCredito().getNotaCreditoById(obj);

                var res = data.Select(x => new
                                        {
                                            id = x.id,
                                            noCredito = notaCredito.ClaveCredito,
                                            nombArchivo = x.nombreArchivo,
                                            Fecha = x.FechaSubida.ToShortDateString()
                                        }
                    );

                result.Add("ListaArchivos", res);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult guardarComentario(tblM_ComentariosNotaCreditoDTO obj, int tipoComentario)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var objS = new tblM_ComentariosNotaCredito();
                objS.id = obj.id;
                objS.notaCreditoID = obj.notaCreditoID;
                objS.comentario = obj.comentario;
                objS.usuarioNombre = getUsuario().nombre; //obj.usuarioNombre; //obj.usuarioNombre;
                objS.usuarioID = getUsuario().id; // obj.usuarioID;
                objS.fecha = DateTime.Now;
                objS.factura = obj.factura;
                objS.tipoComentario = tipoComentario;

                notaCreditoFactoryServices.getNotaCredito().guardarComentario(objS);

                var data = notaCreditoFactoryServices.getNotaCredito().getComentarios(objS.notaCreditoID, tipoComentario);

                result.Add("obj", data);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult setFactura(int id, string oc, string factura)
        {
            var result = new Dictionary<string, object>();
            try
            {

                notaCreditoFactoryServices.getNotaCredito().setFactura(id, oc, factura);

                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetComentarios(int id, int TipoComentarios)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var data = notaCreditoFactoryServices.getNotaCredito().getComentarios(id, TipoComentarios);

                result.Add("obj", data);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetComentariosRechazo(int id, int TipoComentarios)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = notaCreditoFactoryServices.getNotaCredito().getComentarios(id, TipoComentarios);

                result.Add("obj", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendCorreoArchivos(int obj, string oc, string factura)
        {
            var result = new Dictionary<string, object>();
            try
            {


                List<string> listaRutaArchivos = new List<string>();
                var archivo = archivosNotasCreditoFactoryServices.getArchivosNotasCreditoFactoryServices().getlistaByNota(obj);
                var nc = notaCreditoFactoryServices.getNotaCredito().getNotaCreditoById(obj);
                var tipo = nc.TipoNC == 1 ? "Nota de credito aplicada" : nc.TipoNC == 2 ? "Casco reman aplicado" : "Nota de credito aplicada";
                string Asunto = tipo + " OC: " + nc.OC + " y Folio Generador: " + nc.Generador;
                string Contenido = "Se adjuntan archivos de la nota credito con OC: " + nc.OC;
                List<string> CorreoEnvio = new List<string>();
                CorreoEnvio.Add("notas.credito@construplan.com.mx");
                //CorreoEnvio.Add("angel.devora@construplan.com.mx");
                foreach (var item in archivo)
                {
                    var ruta = item.rutaArchivo;
                    listaRutaArchivos.Add(ruta);
                }
                notaCreditoFactoryServices.getNotaCredito().setFactura(obj, oc, factura);
                GlobalUtils.sendEmailAdjunto(Asunto, Contenido, CorreoEnvio, listaRutaArchivos);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /*-Generales Overhaul*/

        public ActionResult GetComponentesEquipo(int noEconomicoID)
        {
            var result = new Dictionary<string, object>();
            try
            {

                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CargarLocaciones(bool estatus, string descripcion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var locaciones = locacionesComponentesServices.getLocacionesComponentesFactoryServices().getLocaciones(estatus, descripcion)
                .Select(x => new
                {
                    id = x.id,
                    tipoLocacion = x.tipoLocacion,
                    descripcion = x.descripcion,
                    estatus = x.estatus,
                    estatusTexto = x.estatus ? "ACTIVO" : "INACTIVO",
                    areaCuenta = x.areaCuenta
                }).ToList();
                result.Add("rows", locaciones);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCorreosLocacionOverhaul(int idLocacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var auxLocacionesID = new List<int>();
                auxLocacionesID.Add(idLocacion);
                var locaciones = locacionesComponentesServices.getLocacionesComponentesFactoryServices().GetCorreosLocacionesOverhaul(auxLocacionesID).ToList();
                result.Add("correos", locaciones);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult FillCbo_CentroCostos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var items = locacionesComponentesServices.getLocacionesComponentesFactoryServices().getCentrosCostos().Select(x => new { Value = x.areaCuenta, Text = x.areaCuenta + " - " + x.descripcion }).OrderBy(x => x.Text);
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

        #region Locaciones

        public ActionResult AltaUpdateLocacion(tblM_CatLocacionesComponentes locacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                locacionesComponentesServices.getLocacionesComponentesFactoryServices().Guardar(locacion);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BajaLocacion(int idLocacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                locacionesComponentesServices.getLocacionesComponentesFactoryServices().eliminarLocacion(idLocacion);
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

        #region Administracion de Componentes

        public ActionResult CargarMaquinaria(int grupo, int modelo, string economicoBusqueda, string descripcionComponente, string obra, string noComponente)
        {
            var result = new Dictionary<string, object>();
            economicoBusqueda = economicoBusqueda.ToUpper();
            descripcionComponente = descripcionComponente.ToUpper();
            try
            {
                var auxMaquinas = administracionComponentesServices.getAdministracionComponentesFactoryServices().getMaquinas(grupo, modelo, economicoBusqueda, descripcionComponente, obra, noComponente);

                var componentesIDs = auxMaquinas.SelectMany(x => x.componente).Distinct().Select(x => x.id).ToList();

                List<horometrosComponentesDTO> horometrosComponentes = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHrsCicloActualComponentes(componentesIDs);

                var maquinas = auxMaquinas
                .Select
                (x =>
                {
                    decimal minResta = decimal.MaxValue;
                    return new AdminComponentesDTO
                    {

                        id = x.idLocacion,
                        cc = x.cc,
                        CCName = getCCName(x.cc).Trim().ToUpper(),
                        economico = x.descripcionLocacion,
                        locacion = x.idLocacion,
                        listaComponentes = x.componente.Distinct()
                        .Select
                        (z =>
                        {
                            var horometroComp = horometrosComponentes.FirstOrDefault(y => y.componenteID == z.id);
                            if (horometroComp != null && minResta > (z.cicloVidaHoras - horometroComp.horometroActual)) minResta = (z.cicloVidaHoras - horometroComp.horometroActual);
                            return new lstCompDTO
                            {
                                descripcion = z.subConjunto.descripcion + " " + (z.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)z.posicionID).ToUpper() : ""),
                                nombreCorto = z.subConjunto.prefijo,
                                noComponente = z.noComponente,
                                restaEstatus = z.falla == true ? decimal.MinValue : (horometroComp == null ? z.cicloVidaHoras : z.cicloVidaHoras - horometroComp.horometroActual),
                                falla = z.falla ?? false,
                                horaCicloActual = horometroComp == null ? 0 : horometroComp.horometroActual
                            };
                        }).OrderBy(y => y.descripcion).OrderBy(y => y.restaEstatus).OrderByDescending(y => y.falla).ToList(),
                        listaComponentesid = x.componente.Select(y => y.id).Distinct().ToList(),
                        //listaComponentesid = x.componente.GroupBy(y => y.id).Select(z => z.Key).ToList(),
                        minRestaEstatus =
                        x.componente.Select(z => new { resta = z.falla == true ? decimal.MinValue : minResta }).OrderBy(z => z.resta).FirstOrDefault().resta
                    };
                }
                ).Where(x => x.listaComponentes.Count() > 0).OrderBy(x => x.minRestaEstatus).ToList();

                result.Add("maquinas", maquinas);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarMaquinariaLocaciones(int locacionBusqueda, string descripcionComponente, int estatus, string obra)
        {
            var result = new Dictionary<string, object>();
            //locacionBusqueda = locacionBusqueda.ToUpper();
            descripcionComponente = descripcionComponente.ToUpper();
            try
            {
                var auxLocaciones = administracionComponentesServices.getAdministracionComponentesFactoryServices().getMaquinasLocaciones(estatus)
                    .Where(x => (locacionBusqueda == 0 ? true : x.idLocacion == locacionBusqueda));

                var componentesIDs = auxLocaciones.SelectMany(x => x.componente).Distinct().Select(x => x.id).ToList();

                List<horometrosComponentesDTO> horometrosComponentes = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHrsCicloActualComponentes(componentesIDs);
                var locaciones = auxLocaciones
                .Select
                (x => new
                {
                    id = x.idLocacion,
                    locacion = x.descripcionLocacion,
                    listaComponentes = x.componente.Where(y => (descripcionComponente.Trim() == "" ? true : (y.subConjunto.descripcion.Trim().ToUpper() == descripcionComponente.Trim())) /*&& y.estatus == true*/).GroupBy(y => y.id)
                    .Select
                    (z =>
                    {
                        var horometroComp = horometrosComponentes.FirstOrDefault(y => y.componenteID == z.FirstOrDefault().id);
                        //if (horometroComp != null && minResta > (z.cicloVidaHoras - horometroComp.horometroActual)) minResta = (z.cicloVidaHoras - horometroComp.horometroActual);
                        return new lstCompDTO
                        {
                            descripcion = z.FirstOrDefault().subConjunto.descripcion + " " + (z.FirstOrDefault().posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)z.FirstOrDefault().posicionID).ToUpper() : ""),
                            nombreCorto = z.FirstOrDefault().subConjunto.prefijo,
                            noComponente = z.FirstOrDefault().noComponente,
                            restaEstatus = z.FirstOrDefault().cicloVidaHoras - (horometroComp == null ? 0 : horometroComp.horometroActual)
                        };
                    }).OrderBy(y => y.descripcion).OrderBy(y => y.restaEstatus).ToList(),
                    listaComponentesid = x.componente.GroupBy(y => y.id).Select(z => z.Key).ToList(),
                    minRestaEstatus = x.componente
                    .Where(y => (descripcionComponente.Trim() == "" ? true : (y.subConjunto.descripcion.Trim().ToUpper() == descripcionComponente.Trim())) /*&& y.estatus == true*/).Select
                    (z => new { resta = (z.cicloVidaHoras - z.horaCicloActual) }).OrderBy(z => z.resta).FirstOrDefault()
                }
                ).Where(x => x.listaComponentes.Count() > 0).OrderBy(x => x.minRestaEstatus.resta).ToList();

                result.Add("locaciones", locaciones);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarMaquinariaCRC(List<int> locacionBusqueda, string descripcionComponente, int estatus, string obra, string noComponente)
        {
            var result = new Dictionary<string, object>();
            DateTime fechaActual = new DateTime();
            fechaActual = DateTime.Now;
            descripcionComponente = descripcionComponente.ToUpper();
            try
            {
                noComponente = noComponente.Trim().ToUpper();
                var auxLocaciones = administracionComponentesServices.getAdministracionComponentesFactoryServices().getMaquinasLocaciones(estatus)
                    .Where(x => locacionBusqueda == null ? false : locacionBusqueda.Contains(x.idLocacion));
                var componentesIDs = auxLocaciones.SelectMany(x => x.componente).Distinct().Select(x => x.id).ToList();
                List<horometrosComponentesDTO> horometrosComponentes = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHrsCicloActualComponentes(componentesIDs);

                var locaciones = auxLocaciones.Select
                (x => new
                {
                    id = x.idLocacion,
                    locacion = x.descripcionLocacion,
                    listaComponentes = x.componente.Where(y => y.noComponente.Contains(noComponente) && (descripcionComponente.Trim() == "" ? true : (y.subConjunto.descripcion.Trim().ToUpper() == descripcionComponente.Trim())) /*&& y.estatus == true*/).GroupBy(y => y.id)
                    .Select
                    (z =>
                    {
                        var horometroComp = horometrosComponentes.FirstOrDefault(y => y.componenteID == z.FirstOrDefault().id);
                        return new lstCompDTO
                            {
                                descripcion = z.FirstOrDefault().subConjunto.descripcion + " " + (z.FirstOrDefault().posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)z.FirstOrDefault().posicionID).ToUpper() : ""),
                                nombreCorto = z.FirstOrDefault().subConjunto.prefijo,
                                noComponente = z.FirstOrDefault().noComponente,
                                //restaEstatus = z.FirstOrDefault().cicloVidaHoras - z.FirstOrDefault().horaCicloActual
                                fecha = z.FirstOrDefault().fecha ?? default(DateTime),
                                diasEnLocacion = Math.Floor(1 + (fechaActual - (z.FirstOrDefault().fecha ?? default(DateTime))).TotalDays),
                                horaCicloActual = horometroComp == null ? 0 : horometroComp.horometroActual
                            };
                    }).OrderBy(y => y.descripcion).OrderBy(y => y.fecha).ToList(),
                    listaComponentesid = x.componente.GroupBy(y => y.id).Select(z => z.Key).ToList(),
                    minFechaEstatus = x.componente
                    .Where(y => (descripcionComponente.Trim() == "" ? true : (y.subConjunto.descripcion.Trim().ToUpper() == descripcionComponente.Trim())) /*&& y.estatus == true*/)
                    .Select(z => new { fechaInstalacion = z.fecha })
                    .OrderBy(z => z.fechaInstalacion).FirstOrDefault()
                }
                ).Where(x => x.listaComponentes.Count() > 0).OrderBy(x => x.minFechaEstatus.fechaInstalacion).ToList();

                result.Add("locaciones", locaciones);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarFechasCRC(int idTrack, DateTime fecha, int estatus, bool intercambio, string datosExtra)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuario = getUsuario().id;
                administracionComponentesServices.getAdministracionComponentesFactoryServices().GuardarFechasCRC(idTrack, fecha, estatus, intercambio, datosExtra, usuario);
                //if (estatus == 4)
                //{
                //    tblP_Alerta objAlerta = new tblP_Alerta();

                //    objAlerta.msj = "NUEVA COTIZACIÓN";
                //    objAlerta.sistemaID = 1;
                //    objAlerta.tipoAlerta = (int)AlertasEnum.REDIRECCION;
                //    objAlerta.url = "/Overhaul/AdministracionComponentes/";
                //    objAlerta.objID = idTrack;
                //    objAlerta.userEnviaID = 13;
                //    objAlerta.userRecibeID = 6032;
                //    objAlerta.moduloID = 110;

                //    alertaFactoryServices.getAlertaService().saveAlerta(objAlerta);
                //}
                //if (estatus == 5 || estatus == 9)
                //{
                //    alertaFactoryServices.getAlertaService().updateAlertaByModulo(idTrack, 110);

                //    tblP_Alerta objAlerta = new tblP_Alerta();
                //    objAlerta.msj = "COTIZACIÓN APROBADA";
                //    objAlerta.sistemaID = 1;
                //    objAlerta.tipoAlerta = (int)AlertasEnum.REDIRECCION;
                //    objAlerta.url = "/Overhaul/AdministracionComponentes/";
                //    objAlerta.objID = idTrack;
                //    objAlerta.userEnviaID = 13;
                //    objAlerta.userRecibeID = 6032;
                //    objAlerta.moduloID = 110;
                //    alertaFactoryServices.getAlertaService().saveAlerta(objAlerta);
                //}
                //if (estatus == 6)
                //{
                //    alertaFactoryServices.getAlertaService().updateAlertaByModulo(idTrack, 110);
                //}
                if (estatus == 9)
                {

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

        private static double DiasHabilesDiferencia(DateTime inicio, DateTime fin)
        {
            double diasHabiles =
                1 + ((fin - inicio).TotalDays * 5 -
                (inicio.DayOfWeek - fin.DayOfWeek) * 2) / 7;

            if (fin.DayOfWeek == DayOfWeek.Saturday) diasHabiles--;
            if (inicio.DayOfWeek == DayOfWeek.Sunday) diasHabiles--;

            return diasHabiles;
        }

        public ActionResult CargarComponentesAlmacen(string noComponente, List<int> idLocacion, string descripcionComponente, int estatus, int grupoId = 0, int modeloId = 0)
        {
            var result = new Dictionary<string, object>();
            try
            {
                noComponente = noComponente.Trim().ToUpper();
                tblM_trackComponentes trackingAnterior = new tblM_trackComponentes();
                var fechas = new FechasTrackingComponenteCRC();
                var fechasTrackAnterior = new FechasTrackingComponenteCRC();
                var auxComponentes = administracionComponentesServices.getAdministracionComponentesFactoryServices().getComponentesAlmacenInactivos(noComponente, idLocacion, descripcionComponente, estatus, grupoId, modeloId).ToList();

                var componentesIDs = auxComponentes.Select(x => x.componenteID).Distinct().ToList();

                List<horometrosComponentesDTO> horometrosComponentes = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHrsCicloActualComponentes(componentesIDs);


                var trackAnteriores = administracionComponentesServices.getAdministracionComponentesFactoryServices().getFacturaTrackAnterior(componentesIDs);
                var facturas = trackAnteriores.Select(x => x.Text).ToList();
                List<FechaFacturaEnkontrolDTO> fechasFacturas = new List<FechaFacturaEnkontrolDTO>();
                if (facturas.Count() > 0) fechasFacturas = administracionComponentesServices.getAdministracionComponentesFactoryServices().getFechaFacturaEnkontrol(facturas);

                var componentes = auxComponentes.Select
                    (x =>
                    {
                        DateTime fechaEntradaFactura = new DateTime();
                        if (x.JsonFechasCRC != null) { fechas = JsonConvert.DeserializeObject<FechasTrackingComponenteCRC>(x.JsonFechasCRC); }
                        var trackAnterior = trackAnteriores.FirstOrDefault(y => y.Value == x.componenteID.ToString());
                        if (trackAnterior != null)
                        {
                            var factura = fechasFacturas.FirstOrDefault(y => y.factura == trackAnterior.Text);
                            if (factura != null)
                            {
                                fechaEntradaFactura = factura.fecha;
                            }
                        }
                        var horometroComp = horometrosComponentes.FirstOrDefault(y => y.componenteID == x.componenteID);
                        var descripcionModelo = administracionComponentesServices.getAdministracionComponentesFactoryServices().descripcionModelo(x.componente.modeloEquipoID ?? 0);
                        return new
                        {
                            id = x.componenteID,
                            noComponente = x.componente.noComponente,
                            locacionID = x.locacionID,
                            locacion = x.locacion,
                            fecha = (x.fecha ?? default(DateTime)).ToString("dd/MM/yy"),
                            fechaRaw = x.fecha,
                            modeloId = descripcionModelo,
                            dias = (fechas == null || fechas.entradaAlmacen == null) ? -1 :
                                Math.Floor((DateTime.Now - (fechas.entradaAlmacen ?? default(DateTime))).TotalDays),
                            subconjunto = x.componente.subConjunto.descripcion + " " + (x.componente.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)x.componente.posicionID).ToUpper() : ""),
                            horasCicloActual = horometroComp == null ? 0 : horometroComp.horometroActual,
                            reporteDesecho = administracionComponentesServices.getAdministracionComponentesFactoryServices().GetReporteDesechoID(x.componenteID),
                            entrada = (x.JsonFechasCRC == null || fechas == null || fechas.entradaAlmacen == null) ?
                                "<button type='button' class='btn btn-primary entrada' data-index='" + x.id + "' data-date='" + (x.fecha ?? default(DateTime)).ToString("MM/dd/yyyy") +
                                "'><span class='glyphicon glyphicon-calendar'></span></button>"
                                : (fechas.entradaAlmacen ?? default(DateTime)).ToString("dd/MM/yy"),
                            proveedor = x.componente.proveedor == null ? "" : x.componente.proveedor.descripcion,
                            ordenCompra = x.componente.ordenCompra,
                            fechaEntradaFactura = fechaEntradaFactura > new DateTime() ? fechaEntradaFactura.ToString("dd/MM/yy") : "--"
                        };
                    }
                ).ToList().OrderByDescending(x => x.dias).ThenBy(x => x.fechaRaw);
                int idUsuario = getUsuario().id;
                int tipoUsuario = tipoUsuarioOverhaul(idUsuario);
                result.Add("componentes", componentes);
                result.Add("tipoUsuario", tipoUsuario);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region REPORTE ALMACEN
        public ActionResult CargarReporteAlmacen(string noComponente, List<int> idLocacion, string descripcionComponente, int estatus, int grupoId = 0, int modeloId = 0)
        {
            var result = new Dictionary<string, object>();
            try
            {
                noComponente = noComponente.Trim().ToUpper();
                tblM_trackComponentes trackingAnterior = new tblM_trackComponentes();
                var fechas = new FechasTrackingComponenteCRC();
                var fechasTrackAnterior = new FechasTrackingComponenteCRC();
                var auxComponentes = administracionComponentesServices.getAdministracionComponentesFactoryServices().getComponentesAlmacenInactivos(noComponente, idLocacion, descripcionComponente, estatus, grupoId, modeloId).ToList();

                var componentesIDs = auxComponentes.Select(x => x.componenteID).Distinct().ToList();

                List<horometrosComponentesDTO> horometrosComponentes = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHrsCicloActualComponentes(componentesIDs);


                var trackAnteriores = administracionComponentesServices.getAdministracionComponentesFactoryServices().getFacturaTrackAnterior(componentesIDs);
                var facturas = trackAnteriores.Select(x => x.Text).ToList();
                List<FechaFacturaEnkontrolDTO> fechasFacturas = new List<FechaFacturaEnkontrolDTO>();
                if (facturas.Count() > 0) fechasFacturas = administracionComponentesServices.getAdministracionComponentesFactoryServices().getFechaFacturaEnkontrol(facturas);

                var componentes = auxComponentes.Select
                    (x =>
                    {
                        DateTime fechaEntradaFactura = new DateTime();
                        if (x.JsonFechasCRC != null) { fechas = JsonConvert.DeserializeObject<FechasTrackingComponenteCRC>(x.JsonFechasCRC); }
                        var trackAnterior = trackAnteriores.FirstOrDefault(y => y.Value == x.componenteID.ToString());
                        if (trackAnterior != null)
                        {
                            var factura = fechasFacturas.FirstOrDefault(y => y.factura == trackAnterior.Text);
                            if (factura != null)
                            {
                                fechaEntradaFactura = factura.fecha;
                            }
                        }                
                        var horometroComp = horometrosComponentes.FirstOrDefault(y => y.componenteID == x.componenteID);
                        var descripcionModelo = administracionComponentesServices.getAdministracionComponentesFactoryServices().descripcionModelo(x.componente.modeloEquipoID ?? 0);                        
                        return new
                        {
                            id = x.componenteID,                          
                            fecha = (x.fecha ?? default(DateTime)).ToString("dd/MM/yy"),
                            fechaRaw = x.fecha,
                            entrada = (x.JsonFechasCRC == null || fechas == null || fechas.entradaAlmacen == null) ? "--" : (fechas.entradaAlmacen ?? default(DateTime)).ToString("dd/MM/yy"),
                            dias = (fechas == null || fechas.entradaAlmacen == null) ? 0 : Math.Floor((DateTime.Now - (fechas.entradaAlmacen ?? default(DateTime))).TotalDays),
                            subconjunto = x.componente.subConjunto.descripcion + " " + (x.componente.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)x.componente.posicionID).ToUpper() : ""),
                            noComponente = x.componente.noComponente,
                            modeloId = descripcionModelo,
                            locacionID = x.locacionID,
                            locacion = x.locacion,
                            horasCicloActual = horometroComp == null ? 0 : horometroComp.horometroActual,
                            reporteDesecho = administracionComponentesServices.getAdministracionComponentesFactoryServices().GetReporteDesechoID(x.componenteID),
                            proveedor = x.componente.proveedor == null ? "" : x.componente.proveedor.descripcion,
                            ordenCompra = x.componente.ordenCompra,
                            fechaEntradaFactura = fechaEntradaFactura > new DateTime() ? fechaEntradaFactura.ToString("dd/MM/yy") : "--"

                        };
                    }
                ).ToList().OrderByDescending(x => x.dias).ThenBy(x => x.fechaRaw).Select(y => new RptAlmacenComponentesDTO
                {
                    fecha =y.fecha,
                    fechaEntradaFactura = y.fechaEntradaFactura,
                    entrada = y.entrada,
                    dias =Convert.ToInt32(y.dias),
                    subconjunto = y.subconjunto,
                    noComponente= y.noComponente,
                    modeloId = y.modeloId,
                    locacion = y.locacion,
                    horasCicloActual = Convert.ToInt32(y.horasCicloActual),

                }).ToList();

                Session["reporte_almacen"] = componentes;
                
                result.Add("componentes", componentes);
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

        public ActionResult CargarComponentesInactivos(string noComponente, List<int> idLocacion, string descripcionComponente, int estatus, int grupoId, int modeloId)
        {
            var result = new Dictionary<string, object>();
            try
            {
                noComponente = noComponente.Trim().ToUpper();
                var locaciones = administracionComponentesServices.getAdministracionComponentesFactoryServices().getLocaciones();
                var auxComponentes = administracionComponentesServices.getAdministracionComponentesFactoryServices().getComponentesAlmacenInactivos(noComponente, idLocacion, descripcionComponente, estatus, grupoId, modeloId).ToList();

                var componentesIDs = auxComponentes.Select(x => x.componenteID).Distinct().ToList();
                var trackAnteriores = administracionComponentesServices.getAdministracionComponentesFactoryServices().getLocacionTrackAnterior(componentesIDs);

                var componentes = auxComponentes.Select
                    (x =>
                    {
                        var almacen = trackAnteriores.FirstOrDefault(y => y.Value == x.componenteID.ToString());
                        return new
                        {
                            id = x.componenteID,
                            noComponente = x.componente.noComponente,
                            locacionID = x.locacionID,
                            locacion = x.locacion,
                            fecha = (x.fecha ?? default(DateTime)).ToString("dd/MM/yy"),
                            subconjunto = x.componente.subConjunto.descripcion + " " + (x.componente.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)x.componente.posicionID).ToUpper() : ""),
                            proveedor = x.componente.proveedor == null ? "" : x.componente.proveedor.descripcion,
                            ordenCompra = x.componente.ordenCompra,
                            almacen = (almacen != null && almacen.Text != null && locaciones.Contains(almacen.Text)) ? almacen.Text : "--",
                            fechaRaw = (x.fecha ?? default(DateTime)).ToString("MM/dd/yy"),
                        };
                    }
                ).OrderBy(x => x.fecha).ToList();

                result.Add("componentes", componentes);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReactivarComponentesInactivos(int componenteID, int locacionID, DateTime fecha)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = administracionComponentesServices.getAdministracionComponentesFactoryServices().ReactivarComponentesInactivos(componenteID, locacionID, fecha);

                result.Add("exito", exito);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboLocacion(int tipoLocacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, administracionComponentesServices.getAdministracionComponentesFactoryServices().FillCboLocacion(tipoLocacion).Select(x => new { Value = x.id, Text = x.descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboLocacionByListaTipo(List<int> tipoLocaciones)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, administracionComponentesServices.getAdministracionComponentesFactoryServices().FillCboLocacionByListaTipo(tipoLocaciones)
                    .Select(x => new { Value = x.id, Text = x.descripcion, Prefijo = x.tipoLocacion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getSubConjuntos(string term)
        {
            var subConjuntos = administracionComponentesServices.getAdministracionComponentesFactoryServices().getSubConjuntos(term).ToList().Take(10);
            var subConjuntosFiltrados = subConjuntos.Select(x => new { id = x.id, label = x.descripcion });
            return Json(subConjuntosFiltrados, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getNoComponenteReporte(string term, int modeloID, int subconjuntoID)
        {
            var componentes = administracionComponentesServices.getAdministracionComponentesFactoryServices().getNoComponenteReporte(term, modeloID, subconjuntoID).ToList().Take(10);
            var componentesFiltrados = componentes.Select(x => new { id = x.id, label = x.noComponente });
            return Json(componentesFiltrados, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getNoComponente(string term)
        {
            var componentes = administracionComponentesServices.getAdministracionComponentesFactoryServices().getNoComponente(term).ToList().Take(10);
            var componentesFiltrados = componentes.Select(x => new { id = x.id, label = x.noComponente });
            return Json(componentesFiltrados, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getEconomico(string term)
        {
            var economicos = administracionComponentesServices.getAdministracionComponentesFactoryServices().getEconomico(term).ToList().Take(10);
            var economicosFiltrados = economicos.Select(x => new { id = x.id, label = x.noEconomico });
            return Json(economicosFiltrados, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getSubConjuntosPlaneacion(string term)
        {
            var subConjuntos = administracionComponentesServices.getAdministracionComponentesFactoryServices().getSubConjuntos(term).ToList().Take(10);
            var Servicios = administracionServiciosFactoryServices.getAdministracionServiciosFactoryServices().getServiciosOverhaul(term).ToList().Take(10);
            var subConjuntosFiltrados = subConjuntos.Select(x => new { id = x.id, label = x.descripcion, tipo = 0 });
            subConjuntosFiltrados = subConjuntosFiltrados.Concat(Servicios.Select(x => new { id = x.id, label = x.descripcion.ToUpper(), tipo = 1 }));
            return Json(subConjuntosFiltrados, JsonRequestBehavior.AllowGet);
        }

        private string getCCName(string centro_costos)
        {
            string aux = "";
            try
            {
                aux = administracionComponentesServices.getAdministracionComponentesFactoryServices().getDescripcionCC(centro_costos);
                return aux;
            }
            catch (Exception e) { return centro_costos; }
        }

        public ActionResult fillCboModelo(int idGrupo = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, administracionComponentesServices.getAdministracionComponentesFactoryServices().FillCboModeloEquipoGrupo(idGrupo).Select(x => new { Value = x.id, Text = x.descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ModalComponentes(string subconjunto, string componenteBusqueda, int maquinaID, string maquina, int tipoLocacion)
        {
            var result = new Dictionary<string, object>();
            DateTime fechaActual = new DateTime();
            fechaActual = DateTime.Now;
            subconjunto = subconjunto.ToUpper();
            if (componenteBusqueda == null) { componenteBusqueda = ""; }
            componenteBusqueda = componenteBusqueda.ToUpper();
            decimal ritmo = 0;
            ritmo = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().CalculoHrsPromDiarioPub(maquina);
            try
            {
                var auxComponentes = administracionComponentesServices.getAdministracionComponentesFactoryServices().FillModalComponentes(maquinaID, componenteBusqueda).Where(x => x.componente.subConjunto.descripcion.Contains(subconjunto));

                var componentesID = auxComponentes.Select(x => x.componenteID).Distinct().ToList();
                var horometrosActuales = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHrsCicloActualComponentes(componentesID);
                var horometrosAcumulados = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHrsAcumuladasComponentes(componentesID);

                var componentes = auxComponentes.Select(x =>
                {
                    double dias = ritmo > 0 ? decimal.ToDouble(Math.Ceiling((x.componente.cicloVidaHoras - x.componente.horaCicloActual)) / ritmo) : 0;
                    var horometroActual = horometrosActuales.FirstOrDefault(y => y.componenteID == x.componenteID);
                    var horometroAcumulado = horometrosAcumulados.FirstOrDefault(y => y.componenteID == x.componenteID);
                    if (dias < 0) dias = 0;
                    return new
                    {
                        id = x.id,
                        noComponente = x.componente.noComponente,
                        subconjunto = x.componente.subConjunto.descripcion + " " + (x.componente.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)x.componente.posicionID).ToUpper() : ""),
                        fechaRaw = x.fecha ?? default(DateTime),
                        fecha = x.fecha.Value.ToString("dd/MM/yyyy"),
                        fechaProxRemocion = x.componente.falla == true ? DateTime.Today.ToString("dd/MM/yyyy") : (DateTime.Today).AddDays(dias).ToString("dd/MM/yyyy"),
                        horasCicloActual = horometroActual == null ? 0 : horometroActual.horometroActual,
                        cicloVidaHoras = x.componente.cicloVidaHoras,
                        horasAcumuladas = horometroAcumulado == null ? 0 : horometroAcumulado.horometroActual,
                        estatus = x.componente.estatus,
                        maquinariaID = x.locacionID == null ? -1 : x.locacionID,
                        maquina = administracionComponentesServices.getAdministracionComponentesFactoryServices().getLocacionByID(x.locacionID ?? default(int), x.tipoLocacion ?? default(bool)),
                        estatusNumero = x.componente.falla == true ? decimal.MinValue : (x.componente.cicloVidaHoras - (horometroActual == null ? 0 : horometroActual.horometroActual)),
                        idComponente = x.componenteID,
                        descripcion = x.componente.subConjunto.descripcion,
                        numVidasAcumuladas = administracionComponentesServices.getAdministracionComponentesFactoryServices().getVidasAcumuladas(x.componenteID),
                        diasEnLocacion = DiasHabilesDiferencia(x.fecha ?? default(DateTime), fechaActual),
                        falla = x.componente.falla,
                        proveedor = x.componente.proveedor == null ? "" : x.componente.proveedor.descripcion,
                        ordenCompra = x.componente.ordenCompra
                    };
                }
                ).Where(x => maquinaID == -1 ? x.maquinariaID == x.maquinariaID : x.maquinariaID == maquinaID && x.noComponente.Contains(componenteBusqueda)).OrderBy(x => x.estatusNumero).ThenBy(x => x.descripcion)
                .GroupBy(x => x.noComponente, (key, g) => g.OrderBy(e => e.fechaRaw).FirstOrDefault()).ToList();
                if (tipoLocacion == 2) { componentes = componentes.OrderBy(x => x.fechaRaw).ToList(); }
                result.Add("componentes", componentes);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ModalComponentesCRC(int tipoLocacion, string maquina, int estatus = -1, int locacion = -1, int grupoMaquina = -1, int modeloMaquina = -1, string subconjunto = "", string noComponente = "", string clvCotizacion = "")
        {
            var result = new Dictionary<string, object>();

            try
            {
                var fechas = new FechasTrackingComponenteCRC();
                DateTime fechaActual = new DateTime();
                fechaActual = DateTime.Now;
                subconjunto = subconjunto.ToUpper();
                maquina = maquina.ToUpper();
                var armado = new int[] { 6, 7, 8 };
                var componentes = administracionComponentesServices.getAdministracionComponentesFactoryServices().FillModalComponentes(locacion, noComponente);

                var componentesIDs = componentes.Select(x => x.componenteID).Distinct().ToList();
                List<horometrosComponentesDTO> horometrosComponentes = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHrsCicloActualComponentes(componentesIDs);
                List<horometrosComponentesDTO> acumuladosComponentes = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHrsAcumuladasComponentes(componentesIDs);

                var data = componentes.Where(x => x.estatus == 2 || x.estatus > 3)
                .Select(x =>
                {
                    var fechasGuardadas = (x.JsonFechasCRC == null || x.JsonFechasCRC == "") ? fechas : JsonConvert.DeserializeObject<FechasTrackingComponenteCRC>(x.JsonFechasCRC);
                    //var almacenGuardado = (fechasGuardadas.almacen == null || fechasGuardadas.almacen == "") ? "" : fechasGuardadas.almacen;
                    //var stringAlmacen = (almacenGuardado == null || almacenGuardado == "") ? "" : 
                    //administracionComponentesServices.getAdministracionComponentesFactoryServices().getAlmacenLocacionByID(Int32.Parse(almacenGuardado));
                    DateTime fechaRecepcion = new DateTime();
                    var boolParseFecha = DateTime.TryParse(fechasGuardadas.fechaRecepcion, out fechaRecepcion);
                    var horometroComp = horometrosComponentes.FirstOrDefault(y => y.componenteID == x.componenteID);
                    var acumuladoComp = acumuladosComponentes.FirstOrDefault(y => y.componenteID == x.componenteID);
                    return new
                        {
                            id = x.id,
                            estatus = x.estatus,
                            estatusDescripcion = EnumHelper.GetDescription((EstadosTrackingEnum)x.estatus),
                            datosMaquina = administracionComponentesServices.getAdministracionComponentesFactoryServices().getMaquinaModalCRC(x.componenteID),
                            contadorDias = boolParseFecha ? Math.Floor(1 + (fechaActual - fechaRecepcion).TotalDays) : Math.Floor(1 + (fechaActual - (x.fecha ?? default(DateTime))).TotalDays),
                            componenteID = x.componenteID,
                            noComponente = x.componente.noComponente,
                            subConjunto = x.componente.subConjunto.descripcion + " " + (x.componente.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)x.componente.posicionID).ToUpper() : ""),
                            locacionID = x.locacionID == null ? -1 : x.locacionID,
                            locacion = administracionComponentesServices.getAdministracionComponentesFactoryServices().getLocacionByID(x.locacionID ?? default(int), true),
                            fechas = fechasGuardadas,
                            clvCoti = fechasGuardadas.claveCotizacion == null ? "" : fechasGuardadas.claveCotizacion,
                            //almacen = stringAlmacen,
                            horasCiclo = horometroComp == null ? 0 : horometroComp.horometroActual,
                            horasAcumuladas = acumuladoComp == null ? 0 : acumuladoComp.horometroActual,
                            comprador = fechasGuardadas.comprador,
                            proveedor = x.componente.proveedor == null ? "" : x.componente.proveedor.descripcion,
                            ordenCompra = x.componente.ordenCompra,
                            modeloID = x.componente.modeloEquipoID
                        };
                }).Where(x => (locacion == -1 ? true : x.locacionID == locacion) && x.noComponente.Contains(noComponente) && x.subConjunto.Contains(subconjunto) && x.datosMaquina.Value.Contains(maquina)
                    && (estatus != -1 ? (estatus == 6 ? armado.Contains(x.estatus) : x.estatus == estatus) : true) && (grupoMaquina == -1 ? true : grupoMaquina == Int32.Parse(x.datosMaquina.Prefijo))
                    && x.clvCoti.Contains(clvCotizacion) && (modeloMaquina == -1 ? true : x.modeloID == modeloMaquina))
                .OrderByDescending(x => x.estatus).OrderByDescending(x => x.contadorDias).ToList();
                result.Add("componentes", data);
                int idUsuario = getUsuario().id;
                int tipoUsuario = tipoUsuarioOverhaul(idUsuario);
                result.Add("tipoUsuario", tipoUsuario);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RegresarEstadoCRC(int estado, int idTrack)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var tracking = administracionComponentesServices.getAdministracionComponentesFactoryServices().getTrackingByID(idTrack);
                FechasTrackingComponenteCRC fechas = JsonConvert.DeserializeObject<FechasTrackingComponenteCRC>(tracking.JsonFechasCRC);
                switch (estado)
                {
                    case 4:
                        fechas.fechaRecepcion = "";
                        tracking.estatus = 2;
                        break;
                    case 5:
                        fechas.fechaCotizacion = "";
                        fechas.claveCotizacion = "";
                        fechas.costo = "";
                        tracking.costoCRC = 0;
                        tracking.estatus = 4;
                        break;
                    case 6:
                        fechas.fechaAutorizacion = "";
                        tracking.estatus = 5;
                        break;
                    case 7:
                        fechas.fechaRequisicion = "";
                        fechas.folioRequisicion = "";
                        tracking.estatus = 6;
                        break;
                    case 8:
                        fechas.fechaEnvioOC = "";
                        fechas.OC = "";
                        tracking.estatus = 7;
                        break;
                    case 9:
                        fechas.fechaTerminacion = "";
                        tracking.estatus = 8;
                        break;
                    case 10:
                        fechas.fechaRecoleccion = "";
                        tracking.estatus = 9;
                        break;
                }
                tracking.JsonFechasCRC = JsonConvert.SerializeObject(fechas);
                administracionComponentesServices.getAdministracionComponentesFactoryServices().Guardar(tracking);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult fillCboModalEconomico()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var maquinas = administracionComponentesServices.getAdministracionComponentesFactoryServices().getMaquinas(-1, -1, "", "", "", "")
                .Select
                (x => new { Value = x.idLocacion, Text = x.descripcionLocacion }).OrderBy(x => x.Value);
                result.Add(ITEMS, maquinas);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ModalComponentesHistorial(int idComponente)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var historial = administracionComponentesServices.getAdministracionComponentesFactoryServices().FillModalComponentesHistorial(idComponente)
                .Select
                (x => new
                {
                    id = x.id,
                    fecha = x.fecha.Value.ToString("dd/MM/yyyy"),
                    locacion = administracionComponentesServices.getAdministracionComponentesFactoryServices().getLocacionByID(x.locacionID ?? default(int), x.tipoLocacion ?? default(bool)),
                    noComponente = x.componente.subConjunto.descripcion + " " + (x.componente.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)x.componente.posicionID).ToUpper() : ""),
                    reciclado = x.reciclado,
                    horasAcumuladas = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHrsAcumuladasComponente(x.componenteID, x.fecha ?? default(DateTime), true),
                    horasCiclo = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHrsCicloActualComponente(x.componenteID, x.fecha ?? default(DateTime), x.id, true),
                }
                ).ToList();
                result.Add("historial", historial);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboGrupoMaquinaComponentes(int obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, administracionComponentesServices.getAdministracionComponentesFactoryServices().FillCboGrupoMaquinaria(obj));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboObraMaquina()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, administracionComponentesServices.getAdministracionComponentesFactoryServices().FillCboObraMaquina().Select(x => new { Value = x.Value, Text = x.Text.ToUpper(), Prefijo = x.Prefijo }));
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboObraMaquinaID()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, administracionComponentesServices.getAdministracionComponentesFactoryServices().FillCboObraMaquinaID().Select(x => new { Value = x.id, Text = x.descripcion.ToUpper(), Prefijo = x.abreviacion.ToUpper() }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboObraMaquinaIDComboDTO()
        {
            return Json(administracionComponentesServices.getAdministracionComponentesFactoryServices().FillCboObraMaquinaIDComboDTO(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboLocacionYObra()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, administracionComponentesServices.getAdministracionComponentesFactoryServices().FillCboLocacionYObra());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboObraMaquinaAC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, administracionComponentesServices.getAdministracionComponentesFactoryServices().FillCboObraMaquinaID().Select(x => new { Value = x.areaCuenta, Text = x.descripcion.ToUpper() }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // TIPO DE NC
        public ActionResult FillcboFiltroTipo()
        {
            var ud = new UsuarioDAO();
            var isPermisoNC = ud.getViewAction(vSesiones.sesionCurrentView, "NotaCredito");
            var isCascoReman = ud.getViewAction(vSesiones.sesionCurrentView, "CascoReman");

            var result = new Dictionary<string, object>();
            try
            {
                var list = notaCreditoFactoryServices.getNotaCredito().getTiposNotaCredito().Where(x => ((isPermisoNC ? (isCascoReman ? (x.Key == 1 || x.Key == 2) : x.Key == 1) : isCascoReman ? (x.Key == 2) : x.Key == 0))).Select(x => new { Value = x.Key, Text = x.Value }).OrderBy(x => x.Text);
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

        #endregion

        #region Catálogo de Marcas

        public ActionResult AltaUpdateMarca(tblM_CatMarcasComponentes marca)
        {
            var result = new Dictionary<string, object>();
            try
            {
                marcasComponentesServices.getMarcasComponentesFactoryServices().Guardar(marca);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarMarcasComponentes(string descripcion, bool estatus)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var marcas = marcasComponentesServices.getMarcasComponentesFactoryServices().getLocaciones(estatus, descripcion)
                .Select(x => new
                {
                    id = x.id,
                    descripcion = x.descripcion,
                    estatus = x.estatus,
                    estatusTexto = x.estatus ? "ACTIVO" : "INACTIVO"
                }).ToList();
                result.Add("rows", marcas);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BajaMarcaComponente(int idLocacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                marcasComponentesServices.getMarcasComponentesFactoryServices().eliminar(idLocacion);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckTallerRemocion(int componenteID, int maquinaID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().CheckTallerRemocion(componenteID, maquinaID);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cargarDatosRemocionComponente(int idComponente = 0)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (idComponente == 0) idComponente = (int)Session["idComponenteRemocion"];
                var remocion = remocionComponenteServices.getRemocionComponenteFactoryServices().cargarDatosRemocionComponente(idComponente);
                result.Add("remocion", remocion);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cargarCboComponenteInstalado(int idModelo, int idSubconjunto)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var items = remocionComponenteServices.getRemocionComponenteFactoryServices().cargarCboComponenteInstalado(idModelo, idSubconjunto);
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

        public ActionResult cargarCboPersonal(string cc) //AUTOCOMPLETAR
        {
            var result = new Dictionary<string, object>();
            try
            {
                var items = remocionComponenteServices.getRemocionComponenteFactoryServices().getCatEmpleados().ToList().OrderBy(x => x.Text).Select(x => new
                {
                    Value = x.Value,
                    Text = x.Text /*+ " " + remocionComponenteServices.getRemocionComponenteFactoryServices().getDescripcionCC(x.Prefijo)*/,
                    Prefijo = x.Prefijo
                });

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

        public ActionResult getReporteRemocionComponente(int idReporteRemocion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var reporte = remocionComponenteServices.getRemocionComponenteFactoryServices().getReporteRemocionByID(idReporteRemocion);

                var horasCicloInstalado = reporte.fechaInstalacionCInstalado != null ? remocionComponenteServices.getRemocionComponenteFactoryServices().CalcularHrsCicloComponente(reporte.componenteInstaladoID, reporte.fechaInstalacionCInstalado ?? default(DateTime)).ToString() : "0.00";
                var obra = administracionComponentesServices.getAdministracionComponentesFactoryServices().getDescripcionCCByCC(reporte.areaCuenta);
                var componenteRemovido = componenteServices.getComponenteService().getComponenteByID(reporte.componenteRemovidoID);
                var subconjunto = "";
                if (componenteRemovido != null && componenteRemovido.subConjunto != null) { subconjunto = componenteRemovido.subConjunto.descripcion; }
                var horasAcumuladasRemovido = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHrsAcumuladasComponente(reporte.componenteRemovidoID, reporte.fechaRemocion, false).ToString();
                var vidasRemovido = administracionComponentesServices.getAdministracionComponentesFactoryServices().getVidasAcumuladasByFecha(reporte.componenteRemovidoID, reporte.fechaRemocion);

                var trackAnterior = administracionComponentesServices.getAdministracionComponentesFactoryServices().GetTrackUltimaInstalacion(componenteRemovido.id, reporte.fechaRemocion);

                //Session["imgRemovido"] = System.Convert.FromBase64String(reporte.imgComponenteRemovido.Split(',')[1]);
                if (reporte.imgComponenteRemovido != null) { Session["imgRemovido"] = System.Convert.FromBase64String(reporte.imgComponenteRemovido.Split(',')[1]); }
                else
                {

                    string startupPath = AppDomain.CurrentDomain.BaseDirectory;
                    string targetPath = startupPath + "Content\\img\\nodisponible.png";
                    Image newImage = Image.FromFile(targetPath);
                    MemoryStream stream = new MemoryStream();
                    newImage.Save(stream, newImage.RawFormat);
                    byte[] data = stream.ToArray();
                    Session["imgRemovido"] = data;
                }
                if (reporte.imgComponenteInstalado != null) { Session["imgInstalado"] = System.Convert.FromBase64String(reporte.imgComponenteInstalado.Split(',')[1]); }
                else
                {

                    string startupPath = AppDomain.CurrentDomain.BaseDirectory;
                    string targetPath = startupPath + "Content\\img\\nodisponible.png";
                    Image newImage = Image.FromFile(targetPath);
                    MemoryStream stream = new MemoryStream();
                    newImage.Save(stream, newImage.RawFormat);
                    byte[] data = stream.ToArray();
                    Session["imgInstalado"] = data;
                }
                if (reporte.personal == null)
                {
                    List<string> aux = new List<string>();
                    aux.Add("N/A");
                    Session["personal"] = aux;
                }
                else
                {
                    Session["personal"] = reporte.personal.Split(',').ToList();
                }

                Session["comentario"] = reporte.comentario;

                //Session["realiza"] = (getUsuario().nombre + " " + getUsuario().apellidoPaterno + " " + getUsuario().apellidoMaterno).Trim();
                var html = "";
                html = "/Reportes/Vista.aspx?idReporte=150";
                html += "&fecha=" + reporte.fechaRemocion.ToString("dd/MM/yyy");
                html += "&noEconomico=" + (reporte.maquina == null ? "N/A" : reporte.maquina.noEconomico);
                html += "&modelo=" + (reporte.maquina == null ? "N/A" : reporte.maquina.modeloEquipo.descripcion);
                html += "&horasmaquina=" + (reporte.maquina == null ? 0 : reporte.horasMaquina);
                html += "&seriemaquina=" + (reporte.maquina == null ? "N/A" : reporte.maquina.noSerie);
                html += "&descripcion=" + reporte.componenteRemovido.subConjunto.descripcion + " " + (reporte.componenteRemovido.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)reporte.componenteRemovido.posicionID).ToUpper() : "");
                html += "&numparte=" + reporte.componenteRemovido.numParte;
                html += "&nocomponenteremovido=" + reporte.componenteRemovido.noComponente;
                html += "&horasComponenteRemovido=" + reporte.horasComponente;
                html += "&nocomponenteinstalado=" + (reporte.componenteInstalado != null ? reporte.componenteInstalado.noComponente : "");
                html += "&garantia=" + (reporte.garantia == true ? "SI" : "NO");
                html += "&empresaresponsable=" + (reporte.empresaResponsable == 0 ? "CONSTRUPLAN" : administracionComponentesServices.getAdministracionComponentesFactoryServices().getLocacionByID((reporte.empresaResponsable) ?? default(int)));
                html += "&motivo=" + (reporte.motivoRemocionID == 0 ? "VIDA UTIL" : (reporte.motivoRemocionID == 1 ? "FALLA" : "ESTRATEGIA"));
                html += "&firmaRealizo=" + (reporte.estatus > 1 ? GlobalUtils.CrearFirmaDigital(idReporteRemocion, Core.Enum.Principal.DocumentosEnum.Remocion_Overhual, reporte.realiza) : "");
                html += "&firmaAdminOverhaul=" + (reporte.estatus > 3 ? GlobalUtils.CrearFirmaDigital(idReporteRemocion, Core.Enum.Principal.DocumentosEnum.Remocion_Overhual, 3292) : "");
                html += "&realiza=" + reporte.realiza;
                html += "&horasComponenteInstalado=" + horasCicloInstalado;
                html += "&obra=" + obra;
                html += "&fechaInstalacion=" + (reporte.fechaInstalacionCInstalado == null ? "N/A" : (reporte.fechaInstalacionCInstalado ?? default(DateTime)).ToString("dd/MM/yyyy"));
                html += "&subconjunto=" + subconjunto;
                html += "&horasAcumuladasRemovido=" + horasAcumuladasRemovido;
                html += "&vidasRemovido=" + vidasRemovido;
                html += "&fechaInstalacionRemovido=" + (trackAnterior == null ? "N/A" : (trackAnterior.fecha ?? default(DateTime)).ToString("dd/MM/yyyy"));

                result.Add("estatus", reporte.estatus);
                result.Add("html", html);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string getURLReporteRemocion(int idReporteRemocion)
        {
            var result = "";
            try
            {
                var reporte = remocionComponenteServices.getRemocionComponenteFactoryServices().getReporteRemocionByID(idReporteRemocion);

                var horasCicloInstalado = reporte.fechaInstalacionCInstalado != null ? remocionComponenteServices.getRemocionComponenteFactoryServices().CalcularHrsCicloComponente(reporte.componenteInstaladoID, reporte.fechaInstalacionCInstalado ?? default(DateTime)).ToString() : "N/A";
                var obra = administracionComponentesServices.getAdministracionComponentesFactoryServices().getDescripcionCCByCC(reporte.areaCuenta);
                var componenteRemovido = componenteServices.getComponenteService().getComponenteByID(reporte.componenteRemovidoID);
                var subconjunto = "";
                if (componenteRemovido != null && componenteRemovido.subConjunto != null) { subconjunto = componenteRemovido.subConjunto.descripcion; }
                var horasAcumuladasRemovido = reporte.fechaInstalacionCInstalado != null ? remocionComponenteServices.getRemocionComponenteFactoryServices().GetHrsAcumuladasComponente(reporte.componenteRemovidoID, reporte.fechaRemocion, false).ToString() : "N/A";
                var vidasRemovido = administracionComponentesServices.getAdministracionComponentesFactoryServices().getVidasAcumuladasByFecha(reporte.componenteRemovidoID, reporte.fechaRemocion);
                var trackAnterior = administracionComponentesServices.getAdministracionComponentesFactoryServices().GetTrackUltimaInstalacion(componenteRemovido.id, reporte.fechaRemocion);

                //Session["imgRemovido"] = System.Convert.FromBase64String(reporte.imgComponenteRemovido.Split(',')[1]);
                if (reporte.imgComponenteRemovido != null) { Session["imgRemovido"] = System.Convert.FromBase64String(reporte.imgComponenteRemovido.Split(',')[1]); }
                else
                {

                    string startupPath = AppDomain.CurrentDomain.BaseDirectory;
                    string targetPath = startupPath + "Content\\img\\nodisponible.png";
                    Image newImage = Image.FromFile(targetPath);
                    MemoryStream stream = new MemoryStream();
                    newImage.Save(stream, newImage.RawFormat);
                    byte[] data = stream.ToArray();
                    Session["imgRemovido"] = data;
                }
                if (reporte.imgComponenteInstalado != null) { Session["imgInstalado"] = System.Convert.FromBase64String(reporte.imgComponenteInstalado.Split(',')[1]); }
                else
                {

                    string startupPath = AppDomain.CurrentDomain.BaseDirectory;
                    string targetPath = startupPath + "Content\\img\\nodisponible.png";
                    Image newImage = Image.FromFile(targetPath);
                    MemoryStream stream = new MemoryStream();
                    newImage.Save(stream, newImage.RawFormat);
                    byte[] data = stream.ToArray();
                    Session["imgInstalado"] = data;
                }
                if (reporte.personal == null)
                {
                    List<string> aux = new List<string>();
                    aux.Add("N/A");
                    Session["personal"] = aux;
                }
                else
                {
                    Session["personal"] = reporte.personal.Split(',').ToList();
                }

                Session["comentario"] = reporte.comentario;

                //Session["realiza"] = (getUsuario().nombre + " " + getUsuario().apellidoPaterno + " " + getUsuario().apellidoMaterno).Trim();
                var html = "";
                html = "/Reportes/Vista.aspx?idReporte=150";
                html += "&fecha=" + reporte.fechaRemocion.ToString("dd/MM/yyy");
                html += "&noEconomico=" + (reporte.maquina == null ? "N/A" : reporte.maquina.noEconomico);
                html += "&modelo=" + (reporte.maquina == null ? "N/A" : reporte.maquina.modeloEquipo.descripcion);
                html += "&horasmaquina=" + (reporte.maquina == null ? 0 : reporte.horasMaquina);
                html += "&seriemaquina=" + (reporte.maquina == null ? "N/A" : reporte.maquina.noSerie);
                html += "&descripcion=" + reporte.componenteRemovido.subConjunto.descripcion + " " + (reporte.componenteRemovido.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)reporte.componenteRemovido.posicionID).ToUpper() : "");
                html += "&numparte=" + reporte.componenteRemovido.numParte;
                html += "&nocomponenteremovido=" + reporte.componenteRemovido.noComponente;
                html += "&horasComponenteRemovido=" + reporte.horasComponente;
                html += "&nocomponenteinstalado=" + (reporte.componenteInstalado != null ? reporte.componenteInstalado.noComponente : "");
                html += "&garantia=" + (reporte.garantia == true ? "SI" : "NO");
                html += "&empresaresponsable=" + (reporte.empresaResponsable == 0 ? "CONSTRUPLAN" : administracionComponentesServices.getAdministracionComponentesFactoryServices().getLocacionByID((reporte.empresaResponsable) ?? default(int)));
                html += "&motivo=" + (reporte.motivoRemocionID == 0 ? "VIDA UTIL" : (reporte.motivoRemocionID == 1 ? "FALLA" : "ESTRATEGIA"));
                html += "&firmaRealizo=" + (reporte.estatus > 1 ? GlobalUtils.CrearFirmaDigital(idReporteRemocion, Core.Enum.Principal.DocumentosEnum.Remocion_Overhual, reporte.realiza) : "");
                html += "&firmaAdminOverhaul=" + (reporte.estatus > 3 ? GlobalUtils.CrearFirmaDigital(idReporteRemocion, Core.Enum.Principal.DocumentosEnum.Remocion_Overhual, 3292) : "");
                html += "&realiza=" + reporte.realiza;
                html += "&horasComponenteInstalado=" + horasCicloInstalado;
                html += "&obra=" + obra;
                html += "&fechaInstalacion=" + (reporte.fechaInstalacionCInstalado == null ? "N/A" : (reporte.fechaInstalacionCInstalado ?? default(DateTime)).ToString("dd/MM/yyyy"));
                html += "&subconjunto=" + subconjunto;
                html += "&horasAcumuladasRemovido=" + horasAcumuladasRemovido;
                html += "&vidasRemovido=" + vidasRemovido;
                html += "&fechaInstalacionRemovido=" + (trackAnterior == null ? "N/A" : (trackAnterior.fecha ?? default(DateTime)).ToString("dd/MM/yyyy"));

                result = html;
                return html;
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public ActionResult guardarReporte(tblM_ReporteRemocionComponente reporte)
        {
            var result = new Dictionary<string, object>();
            try
            {
                reporte.realiza = getUsuario().id;
                var tipo = 0;
                if (reporte.estatus > 2)
                    tipo = 1;
                remocionComponenteServices.getRemocionComponenteFactoryServices().Guardar(reporte, tipo);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult nombreUsuarioActual()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var nombre = getUsuario().nombreUsuario;
                result.Add("nombre", nombre);
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


        public ActionResult fillCboLocaciones(int idModelo, int tipoLocacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var maquinas = remocionComponenteServices.getRemocionComponenteFactoryServices().getMaquinasByModelo(idModelo, tipoLocacion).Select(x => new { Value = x.Value, Text = x.Text });
                result.Add(ITEMS, maquinas);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarReportesRemocion(int estatus, string descripcionComponente, string noEconomico, int motivoRemocion, DateTime? fechaInicio, DateTime? fechaFinal, List<int> cc, List<int> modelos, string noComponente = "")
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<string> areasCuentaStr = new List<string>();
                if (cc != null)
                {
                    var areasCuenta = administracionComponentesServices.getAdministracionComponentesFactoryServices().getListaCCByID(cc);
                    areasCuentaStr = areasCuenta.Select(x => x.areaCuenta).ToList();
                }

                var reportes = remocionComponenteServices.getRemocionComponenteFactoryServices().cargarReportes(estatus, descripcionComponente, noEconomico, motivoRemocion, fechaInicio, fechaFinal, areasCuentaStr, modelos, noComponente)
                .Select(x => new
                {
                    id = x.id,
                    componenteRemovido = "<b>" + x.componenteRemovido.noComponente + "</b>",
                    subConjunto = x.componenteRemovido.subConjunto.descripcion + " " + (x.componenteRemovido.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)x.componenteRemovido.posicionID).ToUpper() : ""),
                    noComponente = x.componenteRemovido.noComponente,
                    estatus = x.estatus,
                    componenteID = x.componenteRemovidoID,
                    noEconomico = x.maquina == null ? "N/A" : x.maquina.noEconomico,
                    cc = remocionComponenteServices.getRemocionComponenteFactoryServices().getCC(x.areaCuenta).ToUpper(),
                    motivo = x.motivoRemocionID,
                    fecha = x.fechaRemocion.ToString("dd/MM/yyy"),
                    fechaVoBo = x.fechaVoBo == null ? "N/A" : (x.fechaVoBo ?? default(DateTime)).ToString("dd/MM/yyy"),
                    fechaEnvio = x.fechaEnvio == null ? "N/A" : (x.fechaEnvio ?? default(DateTime)).ToString("dd/MM/yyy"),
                    fechaAutorizacion = x.fechaAutorizacion == null ? "N/A" : (x.fechaAutorizacion ?? default(DateTime)).ToString("dd/MM/yyy"),
                    destino = x.destinoID,
                    realiza = x.realiza,
                    horasCiclo = x.horasComponente,
                    target = x.componenteRemovido.cicloVidaHoras,
                    destinoStr = x.destino == null ? "--" : x.destino.descripcion,
                    porcentajeRemocion = x.horasComponente == 0 ? "0%" : decimal.Round((((x.componenteRemovido.cicloVidaHoras - x.horasComponente) * 100) / x.horasComponente), 2).ToString() + " %",
                    costo = x.horasMaquina
                });
                result.Add("reportes", reportes);
                var usuarioID = getUsuario().id;
                int tipoUsuario = tipoUsuarioOverhaul(usuarioID);

                result.Add("tipoUsuario", tipoUsuario);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PermisosBotonesAdminComp()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioID = getUsuario().id;
                int tipoUsuario = tipoUsuarioOverhaul(usuarioID);
                result.Add("tipoUsuario", tipoUsuario);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getEmpleadosRemocion(string term)
        {
            var empleados = remocionComponenteServices.getRemocionComponenteFactoryServices().getEmpleadosRemocion(term).ToList();
            var empleadosFiltrados = empleados.Select(x => new { id = x.Value, label = x.Text });
            return Json(empleadosFiltrados, JsonRequestBehavior.AllowGet);
        }

        public int tipoUsuarioOverhaul(int idUsuario)
        {
            if (idUsuario == 13) { return 7; }
            var descripcion = "visitante";
            if (Enum.IsDefined(typeof(PermisosOverhaulEnum), idUsuario))
            {
                var NUM = (PermisosOverhaulEnum)idUsuario;
                descripcion = NUM.GetDescription();
            }
            int tipoUsuario = 6;
            switch (descripcion)
            {
                case "Control de Componentes":
                    tipoUsuario = 0;
                    break;
                case "Administrador de Overhaul":
                    tipoUsuario = 1;
                    break;
                case "Gerente de Overhaul":
                    tipoUsuario = 2;
                    break;
                case "Facilitador":
                    tipoUsuario = 3;
                    break;
                case "Director de Maquinaria":
                    tipoUsuario = 4;
                    break;
                case "Director de Servicios":
                    tipoUsuario = 7;
                    break;
                case "Gerente de Maquinaria y Equipo de Construccion":
                    tipoUsuario = 8;
                    break;
                case "Jefe de Departamento de Evaluación y Diagnóstico":
                    tipoUsuario = 9;
                    break;
                default:
                    tipoUsuario = 6;
                    break;
            }
            return tipoUsuario;
        }

        public ActionResult EliminarReportesRemocion(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                remocionComponenteServices.getRemocionComponenteFactoryServices().Eliminar(id);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult autorizarReporteRemocion(int idReporte)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var reporte = remocionComponenteServices.getRemocionComponenteFactoryServices().getReporteRemocionByID(idReporte);
                var economico = reporte.maquina.noEconomico;
                var componenteInstalado = componenteServices.getComponenteService().getComponenteByID(reporte.componenteInstaladoID);
                var TrackID = componenteServices.getComponenteService().GuardarTrackingComponente(componenteInstalado, reporte.maquinaID, reporte.fechaInstalacionCInstalado ?? DateTime.Now, 0, false, "", "");
                var horasTrabajadas = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHorasMaquinaPorFecha(economico, DateTime.Now) -
                    remocionComponenteServices.getRemocionComponenteFactoryServices().GetHorasMaquinaPorFecha(economico, reporte.fechaInstalacionCInstalado ?? DateTime.Now);
                if (horasTrabajadas > 0)
                {
                    componenteInstalado.horaCicloActual = componenteInstalado.horaCicloActual + horasTrabajadas;
                    componenteInstalado.horasAcumuladas = componenteInstalado.horasAcumuladas + horasTrabajadas;
                }
                componenteInstalado.trackComponenteID = TrackID;
                componenteServices.getComponenteService().Guardar(componenteInstalado);
                remocionComponenteServices.getRemocionComponenteFactoryServices().aprobarReporte(idReporte);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult cargarFechaInstalacion(int idComponente)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var fecha = remocionComponenteServices.getRemocionComponenteFactoryServices().fechaInstalacion(idComponente);
                result.Add("fecha", fecha);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarArchivoTrackComponentes(int idArchivo, int idComponente)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (idArchivo > -1 && idComponente > 0)
                {
                    var EliminarArchivo = remocionComponenteServices.getRemocionComponenteFactoryServices().EliminarArchivoTrackComponentes(idArchivo, idComponente);

                    if (!EliminarArchivo)
                    {
                        result.Add(MESSAGE, "Ocurrió un error al eliminar el Archivo.");
                        result.Add(SUCCESS, false);
                    }
                    else
                    {
                        result.Add(SUCCESS, true);
                    }
                }
                else
                {
                    result.Add(MESSAGE, "Ocurrió un error al lanzar la petición.");
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


        public ActionResult getReportesRemocionComponenteGrupo(int estatus, string descripcionComponente, string noEconomico, int motivoRemocion, List<int> cc, DateTime? fechaInicio, DateTime? fechaFinal, List<int> modelos, string noComponente = "")
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<string> areasCuentaStr = new List<string>();
                if (cc != null)
                {
                    var areasCuenta = administracionComponentesServices.getAdministracionComponentesFactoryServices().getListaCCByID(cc);
                    areasCuentaStr = areasCuenta.Select(x => x.areaCuenta).ToList();
                }

                var reportes = remocionComponenteServices.getRemocionComponenteFactoryServices().cargarReportes(estatus, descripcionComponente, noEconomico, motivoRemocion, fechaInicio, fechaFinal, areasCuentaStr, modelos, noComponente)
                .ToList();

                Session["bitacoraComponentesRemovidos"] = reportes;

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getReporteHistoricoAlmacen(int anio)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var reportes = administracionComponentesServices.getAdministracionComponentesFactoryServices().CargarReporteValorAlmacen(anio, anio);

                //DataTable datatable1 = new DataTable();
                //datatable1.Columns.Add("almacen", typeof(string));
                //datatable1.Columns.Add("enero", typeof(decimal));
                //datatable1.Columns.Add("febrero", typeof(decimal));
                //datatable1.Columns.Add("marzo", typeof(decimal));
                //datatable1.Columns.Add("abril", typeof(decimal));
                //datatable1.Columns.Add("mayo", typeof(decimal));
                //datatable1.Columns.Add("junio", typeof(decimal));
                //datatable1.Columns.Add("julio", typeof(decimal));
                //datatable1.Columns.Add("agosto", typeof(decimal));
                //datatable1.Columns.Add("septiembre", typeof(decimal));
                //datatable1.Columns.Add("octubre", typeof(decimal));
                //datatable1.Columns.Add("noviembre", typeof(decimal));
                //datatable1.Columns.Add("diciembre", typeof(decimal));
                //foreach (var item in reportes) {
                //    datatable1.Rows.Add(item);
                //}
                //List<string> meses = new List<string>();
                //meses.Add("enero");
                //meses.Add("febrero");
                //meses.Add("marzo");
                //meses.Add("abril");
                //meses.Add("mayo");
                //meses.Add("junio");
                //meses.Add("julio");
                //meses.Add("agosto");
                //meses.Add("septiembre");
                //meses.Add("octubre");
                //meses.Add("noviembre");
                //meses.Add("diciembre");

                Session["rptHistoricoAlmacen"] = reportes;
                Session["anioHistoricoAlmacen"] = anio;

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboInsumos(int tipo, int grupo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var dataSet = insumoFactoryServices.getRepTraspasoServices().getListaInsumos("", tipo, grupo, 2);
                result.Add(ITEMS, dataSet.Select(x => new
                {
                    Value = x.insumo,
                    Text = x.descripcion

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

        public ActionResult verificarReporteRemocion(int idReporte)
        {
            var result = new Dictionary<string, object>();
            try
            {
                remocionComponenteServices.getRemocionComponenteFactoryServices().verificarReporte(idReporte);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult enviarReporteRemocion(int idReporte)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var downloadPDF = (List<Byte[]>)Session["reporteRemocion"];
                Session["reporteRemocion"] = null;

                var reporte = remocionComponenteServices.getRemocionComponenteFactoryServices().getReporteRemocionByID(idReporte);
                var componenteRemovido = componenteServices.getComponenteService().getComponenteByID(reporte.componenteRemovidoID);
                var trackID = componenteServices.getComponenteService().GuardarTrackingComponente(componenteRemovido, reporte.destinoID, reporte.fechaRemocion, reporte.destino.tipoLocacion, false, "", "");
                componenteRemovido.trackComponenteID = trackID;
                componenteRemovido.falla = false;
                componenteServices.getComponenteService().Guardar(componenteRemovido);
                planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().ActualizarComponenteRemovido(reporte.maquinaID, reporte.componenteRemovidoID, reporte.fechaRemocion);
                var exito = remocionComponenteServices.getRemocionComponenteFactoryServices().enviarReporte(idReporte, trackID);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                Session["reporteRemocion"] = null;
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult enviarCorreoReporteRemocion(int idReporte, List<string> correos)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var downloadPDF = (List<Byte[]>)Session["reporteRemocion"];
                Session["reporteRemocion"] = null;
                var reporte = remocionComponenteServices.getRemocionComponenteFactoryServices().getReporteRemocionByID(idReporte);
                List<string> mails = new List<string>();
                var subconjunto = /*reporte.componenteRemovido.subConjunto.descripcion; */
                    reporte.componenteRemovido.subConjunto.descripcion + " " + (reporte.componenteRemovido.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)reporte.componenteRemovido.posicionID).ToUpper() : "");
                var serie = reporte.componenteRemovido.noComponente;
                var motivo = reporte.motivoRemocionID;
                var economico = reporte.maquina.noEconomico;
                var obra = reporte.areaCuenta;
                var obraDescr = administracionComponentesServices.getAdministracionComponentesFactoryServices().getDescripcionCCByCC(obra);
                var motivoDescr = "";
                if (Enum.IsDefined(typeof(MotivoFallaCompEnum), motivo))
                {
                    var descr = (MotivoFallaCompEnum)motivo;
                    motivoDescr = descr.GetDescription();
                }
                mails.AddRange(correos.Distinct());
                var mensaje = @"<html><head>
                    <style>
                        body { color: #000000; font-family: arial, helvetica, sans-serif; font-size: 16px; }
                    </style>
                </head>
                <body>
                    BUEN DÍA.<br><br>
                    SE ENVÍA FORMATO DE REMOCIÓN PARA ATENDER LA REPARACIÓN DEL COMPONENTE ";
                mensaje +=
                    subconjunto.ToUpper() + " / " + serie.ToUpper() + " / " + motivoDescr.ToUpper() + " / " + economico.ToUpper() + " / " + obraDescr.ToUpper();
                mensaje +=
                    @"<br><br>SALUDOS.
                    <br><br>----------------------------------------------------------------------------------------------<br>ÉSTE ES UN MENSAJE AUTOGENERADO POR EL SISTEMA SIGOPLAN.
                </body>  
                </html>";
                var tipoFormato = "REPORTE.pdf";
                GlobalUtils.sendEmailAdjuntoInMemory2("REMOCION /" + subconjunto.ToUpper() + " / " + serie.ToUpper() + " / " + motivoDescr.ToUpper() + " / " + economico.ToUpper() + " / " + obraDescr.ToUpper(), mensaje, mails, downloadPDF, tipoFormato);
                System.Threading.Thread.Sleep(10000);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                Session["reporteRemocion"] = null;
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEstatusComponentesCRC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> items = new List<ComboDTO>();
                ComboDTO init = new ComboDTO();
                init.Value = "2";
                init.Text = "Traslado";
                items.Add(init);
                for (int i = 4; i < 10; i++)
                {
                    var existe = items.FirstOrDefault(x => x.Text == EnumHelper.GetDescription((EstadosTrackingEnum)i));
                    if (existe == null)
                    {
                        ComboDTO aux = new ComboDTO();
                        aux.Value = i.ToString();
                        aux.Text = EnumHelper.GetDescription((EstadosTrackingEnum)i);
                        items.Add(aux);
                    }
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

        [HttpPost]
        public ActionResult enviarCorreoCotizacion(string observaciones, string claveCotizacion, int idTrack, List<string> correos, int tipo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                tblM_trackComponentes tracking = administracionComponentesServices.getAdministracionComponentesFactoryServices().getTrackingByID(idTrack);
                var economico = "";
                var subconjunto = "";
                var modelo = "";
                var obra = "";
                var mensajeTipo = "";
                if (tracking != null)
                {
                    var maquina = administracionComponentesServices.getAdministracionComponentesFactoryServices().getUltimaMaquina(tracking.componenteID);
                    subconjunto = /*tracking.componente.subConjunto.descripcion;*/
                        tracking.componente.subConjunto.descripcion + " " + (tracking.componente.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)tracking.componente.posicionID).ToUpper() : "");
                    if (maquina != null)
                    {
                        economico = maquina.noEconomico;
                        modelo = maquina.modeloEquipo.descripcion;
                        obra = administracionComponentesServices.getAdministracionComponentesFactoryServices().getDescripcionCC(maquina.centro_costos);
                    }
                }
                List<ModeloArchivoDTO> listaArchivos = new List<ModeloArchivoDTO>();
                if (tracking.JsonArchivos != null)
                    listaArchivos = JsonConvert.DeserializeObject<List<ModeloArchivoDTO>>(tracking.JsonArchivos);
                List<string> adjuntos = new List<string>();
                List<string> nombres = new List<string>();
                foreach (var item in listaArchivos)
                {
                    var data = item.ruta;
                    var data2 = item.nombre;
                    adjuntos.Add(data);
                    nombres.Add(data2);
                }
                switch (tipo)
                {
                    case 0:
                        mensajeTipo = " BUEN DÍA.<br> ADELANTE CON LA REPARACIÓN.";
                        break;
                    case 1:
                        mensajeTipo = " BUEN DÍA.<br> SE CANCELA REPARACIÓN.";
                        break;
                }
                List<string> mails = new List<string>();
                mails.AddRange(correos.Distinct());
                var mensaje = @"<html>
                    <head><style>body { color: #000000; font-family: arial, helvetica, sans-serif; font-size: 16px; }</style></head>
                    <body>" + mensajeTipo;
                if (observaciones.Trim() != "") { mensaje += "<br> " + observaciones + "."; }
                mensaje +=
                    @"<br> SALUDOS.
                    <br><br>----------------------------------------------------------------------------------------------<br>ÉSTE ES UN MENSAJE AUTOGENERADO POR EL SISTEMA SIGOPLAN.
                    </body></html>";
                GlobalUtils.sendEmailAdjuntoNombre(("COTIZACIÓN " + claveCotizacion + " - " + economico + " - " + subconjunto + " - " + modelo + " - " + obra), mensaje, mails, adjuntos, nombres);
                System.Threading.Thread.Sleep(10000);
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
        public ActionResult GuardarArchivoCRC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                DateTime fecha = DateTime.Now;
                string FileName = "";
                string ruta = "";
                string f = fecha.ToString("ddMMyyyy") + fecha.Hour + fecha.Minute;
                bool pathExist = false;

                HttpPostedFileBase file = Request.Files["archivoCRC"];
                int idTrack = Int32.Parse(Request.Form["idTrack"]);
                if (file != null && file.ContentLength > 0)
                {
                    FileName = file.FileName;
                    ////Productivo
                    ruta = archivofs.getArchivo().getUrlDelServidor(12) + f + FileName;
                    //Local
                    //ruta = @"C:\Users\raguilar\Documents\Proyecto\SIGOPLAN\MAQUINARIA\OVERHAUL\" + f + FileName;
                    pathExist = SaveArchivo(file, ruta);

                    if (pathExist)
                    {
                        tblM_trackComponentes tracking = administracionComponentesServices.getAdministracionComponentesFactoryServices().getTrackingByID(idTrack);
                        ModeloArchivoDTO auxArchivo = new ModeloArchivoDTO();
                        List<ModeloArchivoDTO> listaArchivos = new List<ModeloArchivoDTO>();
                        if (tracking.JsonArchivos != null && tracking.JsonArchivos != "[]") { listaArchivos = JsonConvert.DeserializeObject<List<ModeloArchivoDTO>>(tracking.JsonArchivos); auxArchivo.id = listaArchivos.Last().id + 1; }


                        auxArchivo.nombre = FileName;
                        auxArchivo.ruta = ruta;
                        auxArchivo.FechaCreacion = f;
                        listaArchivos.Add(auxArchivo);
                        tracking.JsonArchivos = JsonConvert.SerializeObject(listaArchivos);
                        administracionComponentesServices.getAdministracionComponentesFactoryServices().Guardar(tracking);
                    }
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

        public FileResult DescargarArchivoCRC(int idTrack, int idArchivo)
        {
            tblM_trackComponentes tracking = administracionComponentesServices.getAdministracionComponentesFactoryServices().getTrackingByID(idTrack);
            List<ModeloArchivoDTO> listaArchivos = new List<ModeloArchivoDTO>();
            listaArchivos = JsonConvert.DeserializeObject<List<ModeloArchivoDTO>>(tracking.JsonArchivos);
            var archivo = listaArchivos.FirstOrDefault(x => x.id == idArchivo);
            var ruta = archivo.ruta;
            var nombreArchivo = archivo.nombre;
            return File(ruta, MimeMapping.GetMimeMapping(nombreArchivo), nombreArchivo);
        }

        [HttpPost]
        public ActionResult GuardarReporteDesecho()
        {
            var result = new Dictionary<string, object>();
            try
            {
                byte[] data;
                DateTime fecha = DateTime.Now;
                string FileName = "";
                string ruta = "";
                string f = fecha.ToString("ddMMyyyy") + fecha.Hour + fecha.Minute;
                List<HttpPostedFileBase> files = Request.Files.GetMultiple("archivoEvidencia").ToList();
                HttpPostedFileBase archivoSerie = Request.Files["archivoSerie"];
                string obj = Request.Form["reporteDesecho"];
                tblM_ReporteRemocionComponente reporte = JsonConvert.DeserializeObject<tblM_ReporteRemocionComponente>(obj);
                FileName = archivoSerie.FileName;
                //Productivo
                ruta = archivofs.getArchivo().getUrlDelServidor(12) + f + FileName;
                //Local
                //ruta = @"C:\Users\raguilar\Documents\Proyecto\SIGOPLAN\MAQUINARIA\OVERHAUL\" + f + FileName;

                using (Stream inputStream = archivoSerie.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    data = memoryStream.ToArray();
                    data = GlobalUtils.FixedSize(data, 500, 500);
                }
                System.IO.File.WriteAllBytes(ruta, data);
                if (System.IO.File.Exists(ruta)) { reporte.imgComponenteRemovido = ruta; }
                List<ModeloArchivoDTO> listaArchivos = new List<ModeloArchivoDTO>();
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    FileName = file.FileName;
                    //Productivo
                    ruta = archivofs.getArchivo().getUrlDelServidor(12) + f + FileName;
                    //Local
                    //ruta = @"C:\Users\raguilar\Documents\Proyecto\SIGOPLAN\MAQUINARIA\OVERHAUL\" + f + FileName;
                    using (Stream inputStream = file.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        data = memoryStream.ToArray();
                        data = GlobalUtils.FixedSize(data, 500, 500);
                    }
                    System.IO.File.WriteAllBytes(ruta, data);
                    if (System.IO.File.Exists(ruta))
                    {
                        ModeloArchivoDTO auxArchivo = new ModeloArchivoDTO();
                        auxArchivo.nombre = FileName;
                        auxArchivo.ruta = ruta;
                        auxArchivo.FechaCreacion = f;
                        listaArchivos.Add(auxArchivo);
                    }
                }
                reporte.JsonEvidencia = JsonConvert.SerializeObject(listaArchivos);
                var componente = componenteServices.getComponenteService().getComponenteByID(reporte.componenteRemovidoID);
                reporte.horasComponente = componente.horaCicloActual;
                var noEconomico = administracionComponentesServices.getAdministracionComponentesFactoryServices().getUltimoEconomico(reporte.componenteRemovidoID);
                var maquina = maquinaFactoryServices.getMaquinaServices().GetMaquinaByNoEconomico(noEconomico);
                reporte.maquinaID = maquina != null ? maquina.id : -1;
                reporte.horasMaquina = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHorasMaquina(noEconomico);
                reporte.realiza = getUsuario().id;
                remocionComponenteServices.getRemocionComponenteFactoryServices().Guardar(reporte, 0);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AutorizarDesechoAlmacen(int idReporte, int idComponente)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = remocionComponenteServices.getRemocionComponenteFactoryServices().UpdateReporteDesecho(idReporte);
                if (exito)
                {
                    List<int> arrComponentes = new List<int>();
                    arrComponentes.Add(idComponente);
                    administracionComponentesServices.getAdministracionComponentesFactoryServices().cambioAlmacen(arrComponentes, 1013, 3);
                }
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarEntradaAlmacen(int trackingID, DateTime fecha)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = administracionComponentesServices.getAdministracionComponentesFactoryServices().GuardarEntradaAlmacen(trackingID, fecha);
                result.Add("exito", exito);
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
        public ActionResult EnviarCorreoTraspaso()
        {
            var result = new Dictionary<string, object>();
            try
            {
                HttpPostedFileBase archivoSerie = Request.Files["archivoFactura"];
                int idTrack = JsonConvert.DeserializeObject<int>(Request.Form["idTrack"].ToString());
                var correos = JsonConvert.DeserializeObject<List<string>>(Request.Form["correos"].ToString());
                var unidad = Request.Form["unidad"].ToString();
                var placas = Request.Form["placas"].ToString();
                var chofer = Request.Form["chofer"].ToString();
                var intercambio = JsonConvert.DeserializeObject<bool>(Request.Form["intercambio"].ToString());
                var almacen = Request.Form["almacen"].ToString(); ;
                List<byte[]> downloadPDF = new List<byte[]>();
                if (archivoSerie != null)
                {
                    using (Stream inputStream = archivoSerie.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        downloadPDF.Add(memoryStream.ToArray());
                    }
                }
                var fecha = DateTime.Today.ToString("dd/MM/yyyy");
                tblM_trackComponentes tracking = administracionComponentesServices.getAdministracionComponentesFactoryServices().getTrackingByID(idTrack);
                FechasTrackingComponenteCRC fechasCRC = new FechasTrackingComponenteCRC();
                string tblComponentes = "";
                if (tracking != null && tracking.JsonFechasCRC != null)
                {
                    fechasCRC = JsonConvert.DeserializeObject<FechasTrackingComponenteCRC>(tracking.JsonFechasCRC);
                    fecha = DateTime.ParseExact(fechasCRC.fechaRecoleccion, "dd/MM/yy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                    var fechaEntrada = DateTime.ParseExact(fechasCRC.fechaRecepcion, "dd/MM/yy", System.Globalization.CultureInfo.InvariantCulture);
                    var fechaSalida = DateTime.ParseExact(fechasCRC.fechaRecoleccion, "dd/MM/yy", System.Globalization.CultureInfo.InvariantCulture);
                    int diasReparacion = Convert.ToInt32((fechaSalida - fechaEntrada).TotalDays);
                    tblComponentes =
                    @"<br><br><table><thead>
                    <tr>
                        <th>RQ</th>
                        <th>OC</th>
                        <th>Proveedor - Factura</th>
                        <th>Comprador</th>
                        <th>Serie</th>
                        <th>Cotización</th>
                        <th>Descripción</th>
                        <th>CRC Origen</th>
                        <th>Fecha envío</th>
                        <th>Destino</th>
                    </tr></thead>";
                    tblComponentes += "<tr>" +
                        "<td style='white-space: nowrap;'>" + fechasCRC.folioRequisicion + "</td>" +
                        "<td style='white-space: nowrap;'>" + ((fechasCRC.OC == null || fechasCRC.OC == "") ? ((tracking.componente.ordenCompra == null || tracking.componente.ordenCompra == "") ? "" : tracking.componente.ordenCompra) : fechasCRC.OC) + "</td>" +
                        "<td style='white-space: nowrap;'>" + fechasCRC.folioFactura + "</td>" +
                        "<td>" + fechasCRC.comprador + "</td>" +
                        "<td>" + tracking.componente.noComponente + "</td>" +
                        "<td style='white-space: nowrap;'>" + fechasCRC.claveCotizacion + "</td>" +
                        "<td>" + tracking.componente.subConjunto.descripcion + " " + (tracking.componente.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)tracking.componente.posicionID).ToUpper() : "") + "</td>" +
                        "<td>" + tracking.locacion + "</td>" +
                        "<td style='white-space: nowrap;'>" + fechasCRC.fechaRecoleccion + "</td>" +
                        "<td>" + almacen + "</td>" +
                    "</tr>";
                    tblComponentes += @"</table><br><br>";
                }
                var subconjunto = tracking.componente.subConjunto.descripcion + " " + (tracking.componente.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)tracking.componente.posicionID).ToUpper() : "");
                var serie = tracking.componente.noComponente;
                var parteMensaje = almacen + " " + fecha + " " + subconjunto + " " + serie + (intercambio ? " INTERCAMBIO" : "");
                var stringPlacas = "";
                if (placas != "")
                {
                    stringPlacas += " CON LAS PLACAS " + placas + " ";
                }
                List<string> mails = new List<string>();
                mails.AddRange(correos.Distinct());
                var mensaje = @"
                <html>
                    <head>
                        <style>                        
                            table, th, td { border: 1px solid black;  border-collapse: collapse;  } 
                            thead { font-size:15px; background:#303f9f !important; border:none !important; color:#f5f5f5; }
                            th, td {  padding: 5px;} 
                            body { color: #000000; font-family: arial, helvetica, sans-serif; font-size: 16px; }
                            table th:first-child { border-top-left-radius: 5px; }
                            table th:last-child { border-top-right-radius:5px; }
                            table tr:last-child td:first-child { border-bottom-left-radius:5px; }
                            table tr:last-child td:last-child { border-bottom-right-radius:5px; }
                        </style>
                    </head>
                    <body>";
                mensaje +=
                    @"<br> SALUDOS.
                    <br><br>SE ADJUNTA FACTURA DE COMPONENTE ENVIADO A ";
                mensaje += parteMensaje;
                mensaje += tblComponentes;
                mensaje += intercambio ? "" : ("<br>EN LA UNIDAD " + unidad + stringPlacas + " Y EL CHOFER " + chofer);
                mensaje +=
                    @"<br><br>----------------------------------------------------------------------------------------------<br>ÉSTE ES UN MENSAJE AUTOGENERADO POR EL SISTEMA SIGOPLAN.
                    </body></html>";
                var tipoFormato = "Factura.pdf";
                GlobalUtils.sendEmailAdjuntoInMemory2("COMPONENTES ENVIADOS A " + parteMensaje, mensaje, mails, downloadPDF, tipoFormato);
                System.Threading.Thread.Sleep(10000);
                Session["downloadPDF"] = null;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                Session["downloadPDF"] = null;
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cargarGridArchivosCRC(int idTrack)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var JsonArchivosTrack = administracionComponentesServices.getAdministracionComponentesFactoryServices().getTrackingByID(idTrack).JsonArchivos;
                List<ModeloArchivoDTO> archivos = new List<ModeloArchivoDTO>();
                if (JsonArchivosTrack != null)
                {
                    archivos = JsonConvert.DeserializeObject<List<ModeloArchivoDTO>>(JsonArchivosTrack);
                }
                result.Add("archivos", archivos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        static bool SaveArchivo(HttpPostedFileBase archivo, string ruta)
        {
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
            //ruta = ruta.Replace("C:\\", "\\REPOSITORIO\\");
            System.IO.File.WriteAllBytes(ruta, data);
            return System.IO.File.Exists(ruta);
        }

        public ActionResult DeleteArchivoCRC(int idTrack, int idArchivo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                tblM_trackComponentes tracking = administracionComponentesServices.getAdministracionComponentesFactoryServices().getTrackingByID(idTrack);
                List<ModeloArchivoDTO> listaArchivos = new List<ModeloArchivoDTO>();
                listaArchivos = JsonConvert.DeserializeObject<List<ModeloArchivoDTO>>(tracking.JsonArchivos);
                var archivo = listaArchivos.FirstOrDefault(x => x.id == idArchivo);
                var ruta = archivo.ruta;
                System.IO.File.Delete(ruta);
                listaArchivos.Remove(archivo);
                tracking.JsonArchivos = JsonConvert.SerializeObject(listaArchivos);
                administracionComponentesServices.getAdministracionComponentesFactoryServices().Guardar(tracking);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RechazarOC(int idTrack)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var tracking = administracionComponentesServices.getAdministracionComponentesFactoryServices().getTrackingByID(idTrack);
                FechasTrackingComponenteCRC fechas = JsonConvert.DeserializeObject<FechasTrackingComponenteCRC>(tracking.JsonFechasCRC);
                List<ModeloArchivoDTO> listaArchivos = new List<ModeloArchivoDTO>();
                listaArchivos = JsonConvert.DeserializeObject<List<ModeloArchivoDTO>>(tracking.JsonArchivos != null ? tracking.JsonArchivos : "[]");
                fechas.claveCotizacion = "";
                fechas.costo = "";
                fechas.parcial = null;
                tracking.estatus = 4;
                tracking.JsonFechasCRC = JsonConvert.SerializeObject(fechas);
                try
                {
                    foreach (var archivo in listaArchivos)
                    {
                        var ruta = archivo.ruta;
                        System.IO.File.Delete(ruta);
                    }
                }
                catch (Exception e) { }
                listaArchivos.Clear();
                tracking.JsonArchivos = JsonConvert.SerializeObject(listaArchivos);

                administracionComponentesServices.getAdministracionComponentesFactoryServices().Guardar(tracking);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EnviarComponenteAlmacenRechazo(int idTrack, DateTime fecha)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var tracking = administracionComponentesServices.getAdministracionComponentesFactoryServices().getTrackingByID(idTrack);
                FechasTrackingComponenteCRC fechas = JsonConvert.DeserializeObject<FechasTrackingComponenteCRC>(tracking.JsonFechasCRC);
                List<ModeloArchivoDTO> listaArchivos = new List<ModeloArchivoDTO>();
                //listaArchivos = JsonConvert.DeserializeObject<List<ModeloArchivoDTO>>(tracking.JsonArchivos != null ? tracking.JsonArchivos : "[]");                
                //fechas.claveCotizacion = "N/A";
                //fechas.costo = "N/A";
                fechas.parcial = null;
                fechas.fechaAutorizacion = "N/A";
                fechas.fechaRequisicion = "N/A";
                fechas.folioRequisicion = "N/A";
                fechas.fechaEnvioOC = "N/A";
                fechas.OC = "N/A";
                fechas.fechaTerminacion = fecha.ToString("dd/MM/yy");
                tracking.estatus = 9;
                tracking.JsonFechasCRC = JsonConvert.SerializeObject(fechas);
                //try
                //{
                //    foreach (var archivo in listaArchivos)
                //    {
                //        var ruta = archivo.ruta;
                //        System.IO.File.Delete(ruta);
                //    }
                //}
                //catch (Exception e) { }
                //listaArchivos.Clear();
                //tracking.JsonArchivos = JsonConvert.SerializeObject(listaArchivos);

                administracionComponentesServices.getAdministracionComponentesFactoryServices().Guardar(tracking);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CambioAlmacen(List<int> arrComponentes, int idAlmacen, string placas, string chofer, List<string> correos, string unidad)
        {
            var result = new Dictionary<string, object>();
            try
            {
                administracionComponentesServices.getAdministracionComponentesFactoryServices().cambioAlmacen(arrComponentes, idAlmacen, 1);
                enviarCorreoCambioAlmacen(arrComponentes, idAlmacen, placas, chofer, correos, unidad);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CambioAlmacenDesecho(List<int> arrComponentes, int idAlmacen)
        {
            var result = new Dictionary<string, object>();
            try
            {
                administracionComponentesServices.getAdministracionComponentesFactoryServices().cambioAlmacen(arrComponentes, idAlmacen, 3);
                //enviarCorreoCambioAlmacen(arrComponentes, idAlmacen, idMaquina, chofer);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetCorreosOverhaul(List<int> locacionesID)
        {
            if (locacionesID == null) { locacionesID = new List<int>(); }
            var result = new Dictionary<string, object>();
            try
            {
                var usuario = getUsuario();
                List<string> correosPpales = new List<string>();
                if (usuario != null && usuario.correo.Trim() != "") correosPpales.Add(usuario.correo);
                List<string> correosLocacion = new List<string>();
                foreach (CorreosOverhaulEnum correo in Enum.GetValues(typeof(CorreosOverhaulEnum))) { correosPpales.Add(EnumHelper.GetDescription(correo)); }
                if (locacionesID.Count > 0)
                {
                    var correos = locacionesComponentesServices.getLocacionesComponentesFactoryServices().GetCorreosLocacionesOverhaul(locacionesID);
                    foreach (var item in correos) { correosLocacion.Add(item); }
                }
                result.Add("correosPpales", correosPpales);
                result.Add("correosLocacion", correosLocacion);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCorreosOverhaul(List<int> locacionesID, List<int> listaCorreos)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> items = new List<ComboDTO>();
                var usuario = getUsuario();
                List<string> correosPpales = new List<string>();
                if (usuario != null && usuario.correo.Trim() != "") correosPpales.Add(usuario.correo);
                List<string> correosLocacion = new List<string>();
                int j = 0;
                foreach (CorreosOverhaulEnum correo in Enum.GetValues(typeof(CorreosOverhaulEnum)))
                {
                    if (j < listaCorreos.Count() && listaCorreos[j] == 1) correosPpales.Add(EnumHelper.GetDescription(correo));
                    j++;
                }
                if (locacionesID.Count > 0)
                {
                    var correos = locacionesComponentesServices.getLocacionesComponentesFactoryServices().GetCorreosLocacionesOverhaul(locacionesID);
                    foreach (var item in correos) { correosLocacion.Add(item); }
                }
                correosPpales.AddRange(correosLocacion);
                result.Add(ITEMS, correosPpales.Select(x => new ComboDTO { Value = x, Text = x }));
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
        public ActionResult enviarCorreoCambioAlmacen(List<int> arrComponentes, int idAlmacen, string placas, string chofer, List<string> correos, string unidad)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<string> mails = new List<string>();
                //var maquina = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(idMaquina).FirstOrDefault();
                var almacen = administracionComponentesServices.getAdministracionComponentesFactoryServices().getLocacionByID(idAlmacen);
                var componentes = componenteServices.getComponenteService().getComponentesByIDs(arrComponentes);

                var fecha = DateTime.Today.ToString("dd/MM/yyyy");

                var tblComponentes = @"<br><br><table>
                    <tr>
                    <th>RQ</th>
                    <th>OC</th>
                    <th>Proveedor - Factura</th>
                    <th>Comprador</th>
                    <th>Serie</th>
                    <th>Cotización</th>
                    <th>Descripción</th>
                    <th>Almacén origen</th>
                    <th>Fecha de envío</th>
                    <th>Almacén destino</th>
                    </tr>";
                foreach (var item in componentes)
                {
                    var economico = administracionComponentesServices.getAdministracionComponentesFactoryServices().getUltimoEconomico(item.id);
                    var trackingCRC = administracionComponentesServices.getAdministracionComponentesFactoryServices().getUltimoTrackCRC(item.id);
                    var trackingAnterior = administracionComponentesServices.getAdministracionComponentesFactoryServices().getTrackAnterior(item.id);
                    int numeroTrackings = administracionComponentesServices.getAdministracionComponentesFactoryServices().getNumeroTrackings(item.id);
                    if (trackingCRC != null && trackingCRC.JsonFechasCRC != null && trackingCRC.JsonFechasCRC != "")
                    {
                        var fechasCRC = JsonConvert.DeserializeObject<FechasTrackingComponenteCRC>(trackingCRC.JsonFechasCRC);
                        //tabla += "<br>" + item.subConjunto.descripcion + " <strong>" + item.noComponente + "</strong>";
                        tblComponentes += "<tr>" +
                            "<td style='white-space: nowrap;'>" + fechasCRC.folioRequisicion + "</td>" +
                            "<td style='white-space: nowrap;'>" + ((fechasCRC.OC == null || fechasCRC.OC == "") ? ((item.ordenCompra == null || item.ordenCompra == "") ? "" : item.ordenCompra) : fechasCRC.OC) + "</td>" +
                            "<td style='white-space: nowrap;'>" + fechasCRC.folioFactura + "</td>" +
                            "<td>" + fechasCRC.comprador + "</td>" +
                            "<td>" + item.noComponente + "</td>" +
                            "<td style='white-space: nowrap;'>" + fechasCRC.claveCotizacion + "</td>" +
                            //"<td>" + item.subConjunto.descripcion + "</td>" +
                            "<td>" + item.subConjunto.descripcion + " " + (item.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)item.posicionID).ToUpper() : "") + "</td>" +
                            "<td>" + (trackingAnterior != null ? trackingAnterior.locacion : "") + "</td>" +
                            "<td style='white-space: nowrap;'>" + fechasCRC.fechaRecoleccion + "</td>" +
                            "<td>" + almacen + "</td>" +
                        "</tr>";
                    }
                    else
                    {
                        tblComponentes += "<tr>" +
                            "<td>N/A</td>" +
                            "<td>N/A</td>" +
                            "<td>N/A</td>" +
                            "<td>N/A</td>" +
                            "<td>" + item.noComponente + "</td>" +
                            "<td>N/A</td>" +
                            //"<td>" + item.subConjunto.descripcion + "</td>" +
                            "<td>" + item.subConjunto.descripcion + " " + (item.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)item.posicionID).ToUpper() : "") + "</td>" +
                            "<td>" + (trackingAnterior != null ? trackingAnterior.locacion : "") + "</td>" +
                            "<td>N/A</td>" +
                            "<td>" + almacen + "</td>" +
                        "</tr>";
                    }
                }
                tblComponentes += @"</table><br><br>";

                mails.AddRange(correos.Distinct());
                var mensaje = @"<html>
                    <head><style>                        
                        table, th, td { border: 1px solid black;  border-collapse: collapse; } 
                        th, td {  padding: 5px;} 
                        body { color: #000000; font-family: arial, helvetica, sans-serif; font-size: 16px; }</style></head>
                    <body>";
                mensaje += @"<br><br> BUEN DÍA.
                    <br><br>";
                mensaje += "EL DÍA " + fecha + " SE ENVIARON LOS SIGUIENTES COMPONENTES A " + almacen + "<br>";
                mensaje += tblComponentes;
                mensaje += "<br>EN LA UNIDAD " + unidad + " CON EL CHOFER " + chofer;
                mensaje +=
                    @"<br><br>----------------------------------------------------------------------------------------------<br>ÉSTE ES UN MENSAJE AUTOGENERADO POR EL SISTEMA SIGOPLAN.
                    </body></html>";
                GlobalUtils.sendEmail(("COMPONENTES ENVIADOS A " + almacen + " " + DateTime.Now.ToString("dd/MM/yy")), mensaje, mails);
                System.Threading.Thread.Sleep(10000);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboUnidadesIntercambioAlmacen(string term)
        {
            term = term.Trim().ToUpper();
            var maquinas = administracionComponentesServices.getAdministracionComponentesFactoryServices().getMaquinasByCC().Select(x => new { id = x.placas, label = x.noEconomico }).ToList();
            var maquinasFiltradas = maquinas.Where(x => x.label.ToUpper().Contains(term)).ToList().Take(10);
            return Json(maquinasFiltradas, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboChoferesIntercambioAlmacen(string term)
        {
            term = term.Trim().ToUpper();
            var choferesFiltradas = administracionComponentesServices.getAdministracionComponentesFactoryServices().getEmpleadosChoferAlmacen(term)/*.Where(x => x.Text.ToUpper().Contains(term))*/.Select(x => new { Value = x.Value, label = x.Text }).Take(10);
            //var choferesFiltradas = choferes.Where(x => x.label.ToUpper().Contains(term)).ToList().Take(10);
            return Json(choferesFiltradas, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillTxtComprador(string term)
        {
            term = term.Trim().ToUpper();
            var choferesFiltradas = administracionComponentesServices.getAdministracionComponentesFactoryServices().getCompradores(term).Select(x => new { Value = x.Value, label = x.Text });
            return Json(choferesFiltradas, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getUsuarioRealiza()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuario = getUsuario().id;
                result.Add("usuario", usuario);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cargarEventosPlaneacionOverhaul(string obra, string subconjunto, List<int> modeloMaquina, int grupoMaquina = -1, int tipoSubconjunto = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var eventos = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().getEventosOverhaul(grupoMaquina, modeloMaquina, obra, subconjunto.Trim(), tipoSubconjunto).Select(x =>
                {
                    var maquina = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(x.maquinaID).FirstOrDefault().noEconomico;

                    var auxComponentes = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(x.idComponentes).Distinct();
                    return new
                    {
                        id = x.indexCal,
                        //idComponentes = auxComponentes,
                        //componentes = componenteServices.getComponenteService().getComponentesByIDs(JsonConvert.DeserializeObject<List<ComboDTO>>(x.idComponentes).Select(z => z.Text).Select(s => int.Parse(s)).ToList()).Select(y => (y.noComponente + ": " + y.subConjunto.descripcion + " " + (y.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)y.posicionID).ToUpper() : "") + " - " + y.horaCicloActual + "/" + y.cicloVidaHoras)),
                        componentes = auxComponentes,
                        fecha = x.fecha.ToString("yyyy-MM-dd"),
                        maquinaID = x.maquinaID,
                        maquina = maquina,
                        tipo = x.tipo,
                        estatus = x.estatus,
                        ritmo = x.ritmo,
                        indexCalOriginal = x.indexCal,
                        iniciado = false
                    };
                }).ToList();
                result.Add("eventos", eventos);
                result.Add("obra", obra);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarCalendarioOverhaul(tblM_CalendarioPlaneacionOverhaul calendario, List<tblM_CapPlaneacionOverhaul> listaOverhauls)
        {
            var result = new Dictionary<string, object>();
            int index = 0;
            try
            {
                index = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().GuardarCalendario(calendario, listaOverhauls);
                result.Add("index", index);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevoCalendarioOverhaul(tblM_CalendarioPlaneacionOverhaul calendario, List<tblM_CapPlaneacionOverhaul> listaOverhauls)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().GuardarNuevoCalendario(calendario, listaOverhauls);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CargarCalendariosGuardados()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var calendarios = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().CargarCalendariosGuardados().Select(x => new
                {
                    Value = x.id,
                    Text = x.nombre
                });
                result.Add(ITEMS, calendarios);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarCalendariosGuardadosTaller()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var calendarios = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().CargarCalendariosGuardados().Select(x => new
                {
                    Value = x.id,
                    Text = x.nombre
                });
                result.Add(ITEMS, calendarios);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarCalendarios(int anio)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var calendarios = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().CargarCalendarios(anio).Select(x => new
                {
                    Value = x.id,
                    Text = x.nombre
                });
                result.Add(ITEMS, calendarios);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarModelosRptInversion()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var modelos = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().CargarModelosRptInversion();
                result.Add(ITEMS, modelos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CargarObrasRptInversion()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obras = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().CargarObrasRptInversion();
                result.Add(ITEMS, obras);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CargarAnioRptInversion()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var anios = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().CargarAnioRptInversion();
                result.Add(ITEMS, anios);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cargarEventosPlaneacionOverhaulGuardados(int idCalendario = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var calendario = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().getCalendarioByID(idCalendario);
                List<tblM_CapPlaneacionOverhaul> eventos = new List<tblM_CapPlaneacionOverhaul>();
                if (idCalendario != -1) eventos = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().getEventosOverhaulGuardado(idCalendario);

                var data = eventos.Select(x =>
                {
                    var auxComponentes = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(x.idComponentes);
                    var maquina = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(x.maquinaID).FirstOrDefault();
                    return new
                    {
                        id = x.indexCal,
                        idComponentes = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(x.idComponentes),
                        componentes = auxComponentes,
                        fecha = x.fecha.ToString("yyyy-MM-dd"),
                        fechaInicio = x.fechaInicio == null ? "--" : (x.fechaInicio ?? default(DateTime)).ToString("dd/MM/yyyy"),
                        maquinaID = x.maquinaID.ToString(),
                        maquina = maquina != null ? maquina.noEconomico : "",
                        ritmo = x.ritmo,
                        tipo = x.tipo,
                        terminado = x.terminado,
                        indexCalOriginal = x.indexCalOriginal,
                        iniciado = x.fechaInicio != null,
                        tablaID = x.id
                    };
                });
                result.Add("calendario", calendario);
                result.Add("obra", calendario.obraID);
                result.Add("eventos", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cargarEventosPlaneacionOverhaulTerminados(int idCalendario = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var calendario = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().getCalendarioByID(idCalendario);
                List<tblM_CapPlaneacionOverhaul> eventos = new List<tblM_CapPlaneacionOverhaul>();
                if (idCalendario != -1) eventos = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().getEventosOverhaulGuardado(idCalendario);
                var data = eventos.Select(x =>
                {
                    var auxComponentes = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(x.idComponentes);
                    var maquina = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(x.maquinaID).FirstOrDefault();
                    return new
                    {
                        id = x.indexCal,
                        idComponentes = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(x.idComponentes).Where(y => y.Value == "1").ToList(),
                        componentes = auxComponentes.Where(y => y.Value == "1").ToList(),
                        fecha = x.fecha.ToString("yyyy-MM-dd"),
                        fechaInicio = x.fechaInicio == null ? "--" : (x.fechaInicio ?? default(DateTime)).ToString("dd/MM/yyyy"),
                        maquinaID = x.maquinaID.ToString(),
                        maquina = maquina != null ? maquina.noEconomico : "",
                        ritmo = x.ritmo,
                        tipo = x.tipo,
                        terminado = x.terminado,
                        indexCalOriginal = x.indexCalOriginal,
                        iniciado = x.fechaInicio != null,
                    };
                }).Where(x => x.componentes.Count() > 0);
                result.Add("calendario", calendario);
                result.Add("obra", calendario.obraID);
                result.Add("eventos", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarRemocionesVidaUtil(DateTime fechaInicio, DateTime fechaFin, int modelo = -1, int grupo = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var remociones = administracionComponentesServices.getAdministracionComponentesFactoryServices().CargarRemocionesVidaUtil(modelo, grupo, fechaInicio, fechaFin);
                result.Add("remociones", remociones);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarReporteInventario(int grupo = -1, int modelo = -1, int conjunto = -1, int subconjunto = -1, string obra = "")
        {
            var result = new Dictionary<string, object>();
            try
            {
                var inventario = administracionComponentesServices.getAdministracionComponentesFactoryServices().CargarReporteInventario(grupo, modelo, conjunto, subconjunto, obra);
                var locaciones = administracionComponentesServices.getAdministracionComponentesFactoryServices().getLocaciones();
                result.Add("inventario", inventario);
                result.Add("locaciones", locaciones);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarReporteValorAlmacen(int anioInicial = -1, int anioFinal = -1)
        {
            var result = new Dictionary<string, object>();
            if (anioInicial == -1) { anioInicial = DateTime.Today.Year; }
            if (anioFinal == -1) { anioFinal = DateTime.Today.Year; }
            try
            {
                var valorAlmacen = administracionComponentesServices.getAdministracionComponentesFactoryServices().CargarReporteValorAlmacen(anioInicial, anioFinal);

                result.Add("valoralmacen", valorAlmacen);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarReporteMaestro(int idCalendario)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var maestro = administracionComponentesServices.getAdministracionComponentesFactoryServices().CargarReporteMaestro(idCalendario);

                result.Add("maestro", maestro);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDatosDetalleMaestro(int idPlaneacionOH)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var detalles = administracionComponentesServices.getAdministracionComponentesFactoryServices().CargarDatosDetalleMaestro(idPlaneacionOH);

                result.Add("detalles", detalles);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReporteListadoMaestro(int idEvento, DatosLstMaestro datos)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var detalles = administracionComponentesServices.getAdministracionComponentesFactoryServices().CargarDatosDetalleMaestro(idEvento).Select(x =>
                {
                    var componente = componenteServices.getComponenteService().getComponenteByID(Int32.Parse(x.Text));
                    return new
                    {
                        subconjunto = //componente.subConjunto.descripcion, 
                            componente.subConjunto.descripcion + " " + (componente.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)componente.posicionID).ToUpper() : ""),
                        removido = componente.noComponente,
                        instalado = " "
                    };
                });
                Session["rptLstMaestro"] = detalles;
                Session["rptLstMaestroDatos"] = datos;
                var html = "/Reportes/Vista.aspx?idReporte=156&idEvento=" + idEvento;
                result.Add("html", html);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CargarDatosDetalleMaestroPlaneacion(string indexCal)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var detalles = administracionComponentesServices.getAdministracionComponentesFactoryServices().CargarDatosDetalleMaestroPlaneacion(indexCal);
                result.Add("detalles", detalles);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCalendarioReporteMaestro()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var anios = administracionComponentesServices.getAdministracionComponentesFactoryServices().FillCboCalendarioReporteMaestro();
                result.Add(ITEMS, anios);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboAniosValorAlmacen()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var anios = administracionComponentesServices.getAdministracionComponentesFactoryServices().FillCboAniosValorAlmacen();
                result.Add(ITEMS, anios);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region Reporte ComponentList
        public ActionResult FillCboLocacionesComponentList(List<int> modelosID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var locaciones = administracionComponentesServices.getAdministracionComponentesFactoryServices().FillCboLocacionesComponentList(modelosID);
                result.Add(ITEMS, locaciones);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboAlmacenesInventario()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var almacenes = administracionComponentesServices.getAdministracionComponentesFactoryServices().FillCboAlmacenesInventario();
                result.Add(ITEMS, almacenes);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboComponentes()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var componentes = administracionComponentesServices.getAdministracionComponentesFactoryServices().FillCboComponentes();
                result.Add(ITEMS, componentes);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboConjuntos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var conjuntos = administracionComponentesServices.getAdministracionComponentesFactoryServices().FillCboConjuntos();
                result.Add(ITEMS, conjuntos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboSubconjuntos(int conjunto)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var locaciones = administracionComponentesServices.getAdministracionComponentesFactoryServices().FillCboSubconjuntos(conjunto);
                result.Add(ITEMS, locaciones);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarComponentList(List<int> modelo, int locacion = -1, string noComponente = "", int conjunto = -1, int subconjunto = -1, string obra = "")
        {
            var result = new Dictionary<string, object>();
            try
            {
                var ritmoLocaciones = new List<RitmoMaquinaDTO>();
                if (modelo == null) modelo = new List<int>();
                var auxComponentes = administracionComponentesServices.getAdministracionComponentesFactoryServices().CargarComponentList(locacion, noComponente, conjunto, subconjunto, modelo, obra);
                var componentesIDs = auxComponentes.Select(x => x.tracking).Select(x => x.componenteID).Distinct().ToList();

                List<horometrosComponentesDTO> horActualComponentes = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHrsCicloActualComponentes(componentesIDs);
                List<horometrosComponentesDTO> horAcumuladoComponentes = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHrsAcumuladasComponentes(componentesIDs);
                var componentes = auxComponentes
                    .Select(x =>
                    {
                        double dias = x.ritmoMaquina > 0 ? decimal.ToDouble(Math.Ceiling((x.tracking.componente.cicloVidaHoras - x.tracking.componente.horaCicloActual)) / x.ritmoMaquina) : 0;
                        if (dias < 0) dias = 0;
                        var horasActuales = horActualComponentes.FirstOrDefault(y => y.componenteID == x.tracking.componenteID);
                        var horasAcumuladas = horAcumuladoComponentes.FirstOrDefault(y => y.componenteID == x.tracking.componenteID);
                        return new
                        {
                            noComponente = x.tracking.componente.noComponente,
                            noParte = x.tracking.componente.numParte,
                            subconjunto = x.tracking.componente.subConjunto.descripcion + " " + (x.tracking.componente.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)x.tracking.componente.posicionID).ToUpper() : ""),
                            fechaRaw = x.tracking.fecha ?? default(DateTime),
                            fecha = x.tracking.fecha.Value.ToString("dd/MM/yyyy"),
                            fechaProxRemocionRaw = x.tracking.componente.falla == true ? DateTime.Today : (DateTime.Today).AddDays(dias),
                            fechaProxRemocion = x.tracking.estatus == 0 ? (x.tracking.componente.falla == true ? DateTime.Today.ToString("dd/MM/yyyy") : (DateTime.Today).AddDays(dias).ToString("dd/MM/yyyy")) : "--",
                            horasCicloActual = (horasActuales == null ? 0 : horasActuales.horometroActual).ToString("N"),
                            cicloVidaHoras = x.tracking.componente.cicloVidaHoras,
                            horasAcumuladas = (horasAcumuladas == null ? 0 : horasAcumuladas.horometroActual).ToString("N"),
                            locacion = x.tracking.locacion,
                            descripcion = x.tracking.componente.subConjunto.descripcion,
                            numVidasAcumuladas = x.tracking.componente.vidaInicio //administracionComponentesServices.getAdministracionComponentesFactoryServices().getVidasAcumuladas(x.tracking.componenteID)
                        };
                    }).OrderBy(x => x.locacion).ThenBy(x => x.noComponente).ToList();

                result.Add("componentes", componentes);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReporteComponentList(List<int> modelo, int locacion = -1, string noComponente = "", int conjunto = -1, int subconjunto = -1, string obra = "")
        {
            var result = new Dictionary<string, object>();
            try
            {
                var ritmoLocaciones = new List<RitmoMaquinaDTO>();
                if (modelo == null) modelo = new List<int>();
                var auxComponentes = administracionComponentesServices.getAdministracionComponentesFactoryServices().CargarComponentList(locacion, noComponente, conjunto, subconjunto, modelo, obra);
                var componentesIDs = auxComponentes.Select(x => x.tracking).Select(x => x.componenteID).Distinct().ToList();

                List<horometrosComponentesDTO> horActualComponentes = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHrsCicloActualComponentes(componentesIDs);
                List<horometrosComponentesDTO> horAcumuladoComponentes = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHrsAcumuladasComponentes(componentesIDs);

                var componentes = auxComponentes
                    .Select(x =>
                    {
                        double dias = x.ritmoMaquina > 0 ? decimal.ToDouble(Math.Ceiling((x.tracking.componente.cicloVidaHoras - x.tracking.componente.horaCicloActual)) / x.ritmoMaquina) : 0;
                        if (dias < 0) dias = 0;
                        var horasActuales = horActualComponentes.FirstOrDefault(y => y.componenteID == x.tracking.componenteID);
                        var horasAcumuladas = horAcumuladoComponentes.FirstOrDefault(y => y.componenteID == x.tracking.componenteID);
                        return new ReporteComponentListDTO
                        {
                            noComponente = x.tracking.componente.noComponente,
                            subconjunto = x.tracking.componente.subConjunto.descripcion + " " + (x.tracking.componente.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)x.tracking.componente.posicionID).ToUpper() : ""),
                            fecha = x.tracking.fecha.Value.ToString("dd/MM/yyyy"),
                            fechaProxRemocion = x.tracking.estatus == 0 ? (x.tracking.componente.falla == true ? DateTime.Today.ToString("dd/MM/yyyy") : (DateTime.Today).AddDays(dias).ToString("dd/MM/yyyy")) : "--",
                            horaCicloActual = horasActuales == null ? 0 : horasActuales.horometroActual,
                            target = x.tracking.componente.cicloVidaHoras,
                            horasAcumuladas = horasAcumuladas == null ? 0 : horasAcumuladas.horometroActual,
                            locacion = x.tracking.locacion,
                            descripcion = x.tracking.componente.subConjunto.descripcion,
                            vidas = x.tracking.componente.vidaInicio,
                            estatus = x.tracking.estatus
                        };
                    }).OrderBy(x => x.locacion).ThenBy(x => x.noComponente).ToList();

                Session["rptComponentList"] = componentes;
                var html = "/Reportes/Vista.aspx?idReporte=178";

                result.Add("html", html);
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

        #region Reporte Inventario Componentes

        public ActionResult CargarInventarioComponentes(List<int> modelo, DateTime? fechaInicio, DateTime? fechaFin, int locacion = -1, string noComponente = "", int conjunto = -1, int subconjunto = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var ritmoLocaciones = new List<RitmoMaquinaDTO>();
                var componentes = administracionComponentesServices.getAdministracionComponentesFactoryServices().CargarInventario(locacion, noComponente, conjunto, subconjunto, modelo)
                    .Select(x =>
                    {
                        //double dias = x.ritmoMaquina > 0 ? decimal.ToDouble(Math.Ceiling((x.tracking.componente.cicloVidaHoras - x.tracking.componente.horaCicloActual)) / x.ritmoMaquina) : 0;
                        //if (dias < 0) dias = 0;
                        FechasTrackingComponenteCRC fechas = new FechasTrackingComponenteCRC();
                        DateTime fechaEntrada = new DateTime();
                        if (x.JsonFechasCRC != null && x.JsonFechasCRC != "")
                        {
                            fechas = JsonConvert.DeserializeObject<FechasTrackingComponenteCRC>(x.JsonFechasCRC);
                            if (fechas.entradaAlmacen != null) fechaEntrada = fechas.entradaAlmacen ?? default(DateTime);
                        }
                        var vidaActual = administracionComponentesServices.getAdministracionComponentesFactoryServices().GetVidasComponenteTracking(x.id);
                        return new
                        {
                            noComponente = x.componente.noComponente,
                            subconjunto = x.componente.subConjunto.descripcion + " " + (x.componente.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)x.componente.posicionID).ToUpper() : ""),
                            fechaRaw = x.fecha ?? default(DateTime),
                            fecha = x.fecha.Value.ToString("dd/MM/yyyy"),
                            horasCicloActual = x.componente.horaCicloActual,
                            cicloVidaHoras = x.componente.cicloVidaHoras,
                            horasAcumuladas = x.componente.horasAcumuladas,
                            locacion = x.locacion,
                            descripcion = x.componente.subConjunto.descripcion,
                            numVidasAcumuladas = vidaActual + x.componente.vidaInicio,
                            diasAlmacenado = fechaEntrada == default(DateTime) ? -1 : DateTime.Today.Subtract(fechaEntrada).Days
                        };
                    }).OrderBy(x => x.locacion).ThenBy(x => x.noComponente).ToList();

                result.Add("componentes", componentes);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReporteInventarioComponentes(List<int> modelo, DateTime? fechaInicio, DateTime? fechaFin, int locacion = -1, string noComponente = "", int conjunto = -1, int subconjunto = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var ritmoLocaciones = new List<RitmoMaquinaDTO>();
                var componentes = administracionComponentesServices.getAdministracionComponentesFactoryServices().CargarInventario(locacion, noComponente, conjunto, subconjunto, modelo)
                    .Select(x =>
                    {
                        FechasTrackingComponenteCRC fechas = new FechasTrackingComponenteCRC();
                        DateTime fechaEntrada = new DateTime();
                        if (x.JsonFechasCRC != null && x.JsonFechasCRC != "")
                        {
                            fechas = JsonConvert.DeserializeObject<FechasTrackingComponenteCRC>(x.JsonFechasCRC);
                            if (fechas.entradaAlmacen != null) fechaEntrada = fechas.entradaAlmacen ?? default(DateTime);
                        }
                        var vidaActual = administracionComponentesServices.getAdministracionComponentesFactoryServices().GetVidasComponenteTracking(x.id);
                        return new ReporteComponentListDTO
                        {
                            noComponente = x.componente.noComponente,
                            subconjunto = x.componente.subConjunto.descripcion + " " + (x.componente.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)x.componente.posicionID).ToUpper() : ""),
                            fecha = x.fecha.Value.ToString("dd/MM/yyyy"),
                            horaCicloActual = x.componente.horaCicloActual,
                            target = x.componente.cicloVidaHoras,
                            horasAcumuladas = x.componente.horasAcumuladas,
                            locacion = x.locacion,
                            descripcion = x.componente.subConjunto.descripcion,
                            vidas = vidaActual + x.componente.vidaInicio,
                            estatus = x.estatus,
                            diasAlmacenado = fechaEntrada == default(DateTime) ? "--" : (DateTime.Today.Subtract(fechaEntrada).Days).ToString()
                        };
                    }).OrderBy(x => x.locacion).ThenBy(x => x.noComponente).ToList();

                Session["rptInventarioComponentes"] = componentes;
                var html = "/Reportes/Vista.aspx?idReporte=179";

                result.Add("html", html);
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

        #region REPORTE ALMANCEN
        /*public ActionResult GetReporteAlmacen( DateTime fecha, DateTime entrada, DateTime fechaEntradaFactura, int dias, string subconjunto, int modeloId, string locacion, int horasCicloActual)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var auxComponentes = administracionComponentesServices.getAdministracionComponentesFactoryServices().CargarReporteAlmacen(fecha, entrada, fechaEntradaFactura, dias, subconjunto, modeloId, locacion, horasCicloActual).ToList();

                auxComponentes.Select(Y => new RptAlmacenComponentesDTO
                {
                    fecha = Y.fecha
                });


                Session["rptDTO"] = auxComponentes;
             
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }*/
        #endregion

        #region Reporte de componentes en reparacion
        //Consulta de carga
        public ActionResult CargarCompReparacion(int locacion = -1, int grupo = -1, int modelo = -1, int subconjunto = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                //var ritmoLocaciones = new List<RitmoMaquinaDTO>();
                var data = administracionComponentesServices.getAdministracionComponentesFactoryServices().CargarCompReparacion(locacion, grupo, modelo, subconjunto)
                    .Select(x =>
                    {
                        FechasTrackingComponenteCRC fechas = new FechasTrackingComponenteCRC();
                        DateTime fechaEntrada = new DateTime();
                        if (x.JsonFechasCRC != null && x.JsonFechasCRC != "")
                        {
                            fechas = JsonConvert.DeserializeObject<FechasTrackingComponenteCRC>(x.JsonFechasCRC);
                            if (fechas.entradaAlmacen != null) fechaEntrada = fechas.entradaAlmacen ?? default(DateTime);
                        }
                        var noEconomico = administracionComponentesServices.getAdministracionComponentesFactoryServices().getMaquinaModalCRC(x.componenteID).Value;
                        var obra = administracionComponentesServices.getAdministracionComponentesFactoryServices().getCCByEconomico(noEconomico);
                        var cc = "";
                        if (obra != null) { cc = obra.descripcion; }
                        return new rptComponenteReparacionDTO
                        {
                            noComponente = x.componente.noComponente,
                            subconjunto = x.componente.subConjunto.descripcion + " " + (x.componente.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)x.componente.posicionID).ToUpper() : ""),
                            noEconomico = administracionComponentesServices.getAdministracionComponentesFactoryServices().getMaquinaModalCRC(x.componenteID).Value,
                            cc = cc,
                            locacion = x.locacion,
                            cotizacion = fechas.claveCotizacion,
                            costo = fechas.costo == null ? "" : fechas.costo,
                            costoPromedio = ""
                        };
                    }).OrderBy(x => x.locacion).ThenBy(x => x.noComponente).ToList();

                Session["rptComponentesReparcion"] = data;
                result.Add("data", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetReporteCompReparacion()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var html = "/Reportes/Vista.aspx?idReporte=192";
                result.Add("html", html);
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






        // Control de servicios
        #region Control de Servicios

        public ActionResult GuardarTipoServicioOverhaul(tblM_CatTipoServicioOverhaul obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = administracionServiciosFactoryServices.getAdministracionServiciosFactoryServices().GuardarTipo(obj);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarTipoServicios(string nombre, bool estatus, int grupo = -1, int modelo = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var tipoServicios = administracionServiciosFactoryServices.getAdministracionServiciosFactoryServices().CargarTipoServicios(nombre, estatus, grupo, modelo).Select(x => new
                {
                    id = x.id,
                    nombre = x.nombre,
                    grupoMaquinaID = x.grupoMaquinaID,
                    modeloMaquinaID = x.modeloMaquinaID,
                    modeloMaquina = administracionServiciosFactoryServices.getAdministracionServiciosFactoryServices().getModeloByID(x.modeloMaquinaID),
                    descripcion = x.descripcion,
                    planeacion = x.planeacion,
                    estatus = x.estatus ? "ACTIVO" : "INACTIVO"
                });
                result.Add("tipoServicios", tipoServicios);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillEconomicoByModelo(int modeloID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var items = administracionServiciosFactoryServices.getAdministracionServiciosFactoryServices().getMaquinasByModeloID(modeloID).Select(x => new
                {
                    Value = x.id,
                    Text = x.noEconomico
                });
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

        public ActionResult GuardarAsignacionServicioOverhaul(tblM_CatServicioOverhaul obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                obj.fechaAsignacion = DateTime.Now;
                var exito = administracionServiciosFactoryServices.getAdministracionServiciosFactoryServices().Guardar(obj);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeshabilitarServicioOverhaul(int idServicio)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = administracionServiciosFactoryServices.getAdministracionServiciosFactoryServices().DeshabilitarServicioOverhaul(idServicio);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarServiciosActivos(string economico, string servicio, bool estatus, string cc = "", int grupoMaquina = -1, int modeloMaquina = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var servicios = administracionServiciosFactoryServices.getAdministracionServiciosFactoryServices().CargarServiciosActivos(economico, servicio, cc, grupoMaquina, modeloMaquina, estatus).Select(x => new
                {
                    id = x.id,
                    centroCostos = x.centroCostos,
                    economico = x.maquina.noEconomico,
                    maquinaID = x.maquina.id,
                    servicios = x.servicios.OrderBy(z => z.Item2.cicloVidaHoras - z.Item2.horasCicloActual),
                    fecha = x.fecha,
                    CCName = getCCName(x.centroCostos)
                }).ToList();

                result.Add("servicios", servicios);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult guardarModificacionesServicios(decimal cicloVidaHoras, int estatusNuevo, string economico, string servicio, bool estatus, string cc = "", int grupoMaquina = -1, int modeloMaquina = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var servicios = administracionServiciosFactoryServices.getAdministracionServiciosFactoryServices().guardarModificacionesServicios(cicloVidaHoras, estatusNuevo, economico, servicio, cc, grupoMaquina, modeloMaquina, estatus);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarModalServiciosActivos(int idMaquina, string servicio, bool estatus)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var servicios = administracionServiciosFactoryServices.getAdministracionServiciosFactoryServices().CargarModalServiciosActivos(idMaquina, servicio)
                    .Where(x => x.Item2.estatus == estatus).Select(x => new
                {
                    //estatus = x.Item2.estatus,
                    id = x.Item2.id,
                    economico = x.Item2.maquina.noEconomico,
                    maquinaID = x.Item2.maquina.id,
                    nombreServicio = x.Item1.nombre,
                    fecha = x.Item2.fechaAsignacion.ToString("dd/MM/yy"),
                    horasCicloActual = x.Item2.horasCicloActual,
                    cicloVidaHoras = x.Item2.cicloVidaHoras,
                    resta = x.Item2.cicloVidaHoras - x.Item2.horasCicloActual,
                    isPlaneacion = x.Item1.planeacion,
                    fechaUltimoServicio = x.Item2.fechaAplicacion == null ? "N/A" : (x.Item2.fechaAplicacion ?? default(DateTime)).ToString("dd/MM/yy"),
                    estatus = x.Item2.estatus
                }).OrderBy(x => x.resta).ToList();

                result.Add("servicios", servicios);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AplicarServicioOverhaul(int id, int idMaquina, bool isPLaneacion, List<ModeloArchivoDTO> archivos, DateTime fecha)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (fecha == null) fecha = DateTime.Now;
                var exito = administracionServiciosFactoryServices.getAdministracionServiciosFactoryServices().Aplicar(id, idMaquina, isPLaneacion, archivos, fecha);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CargarArchivosEvidenciaServicio(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var archivos = administracionServiciosFactoryServices.getAdministracionServiciosFactoryServices().CargarArchivosEvidencia(id);
                result.Add("archivos", archivos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DesasignarServicioOverhaul(int idServicio)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = administracionServiciosFactoryServices.getAdministracionServiciosFactoryServices().Desasignar(idServicio);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CargarHistorialServiciosActivos(int idServicio)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var servicios = administracionServiciosFactoryServices.getAdministracionServiciosFactoryServices().CargarHistorialServiciosActivos(idServicio).Select(x => new
                {
                    id = x.id,
                    fecha = x.fecha.ToString("dd/MM/yy"),
                    horasCiclo = x.horasCiclo,
                    target = x.target,
                    archivos = x.archivos
                }).ToList();

                result.Add("servicios", servicios);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cargarArchivosHistServ(int idTrack)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var JsonArchivosTrack = administracionServiciosFactoryServices.getAdministracionServiciosFactoryServices().getTrackingByID(idTrack);
                List<ModeloArchivoDTO> archivos = new List<ModeloArchivoDTO>();
                if (JsonArchivosTrack != null && JsonArchivosTrack.archivos != null)
                {
                    archivos = JsonConvert.DeserializeObject<List<ModeloArchivoDTO>>(JsonArchivosTrack.archivos);
                }
                result.Add("archivos", archivos);
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
        public ActionResult GuardarArchivoServHist()
        {
            var result = new Dictionary<string, object>();
            try
            {
                DateTime fecha = DateTime.Now;
                string FileName = "";
                string ruta = "";
                string f = fecha.ToString("ddMMyyyy") + fecha.Hour + fecha.Minute;
                bool pathExist = false;

                HttpPostedFileBase file = Request.Files["archivoCRC"];
                ModeloArchivoDTO auxArchivo = new ModeloArchivoDTO();
                if (file != null && file.ContentLength > 0)
                {
                    FileName = file.FileName;
                    ////Productivo
                    ruta = archivofs.getArchivo().getUrlDelServidor(12) + f + FileName;
                    //Local
                    //ruta = @"C:\Proyectos\" + f + FileName;
                    pathExist = SaveArchivo(file, ruta);

                    if (pathExist)
                    {
                        auxArchivo.nombre = FileName;
                        auxArchivo.ruta = ruta;
                        auxArchivo.FechaCreacion = f;
                        auxArchivo.FechaCreacionSinFormato = f;

                        string dd = string.Empty;
                        string MM = string.Empty;
                        string yyyy = string.Empty; //091020201137

                        dd = auxArchivo.FechaCreacion.Substring(0, 2);
                        MM = auxArchivo.FechaCreacion.Substring(2, 2);
                        yyyy = auxArchivo.FechaCreacion.Substring(4, 4);
                        auxArchivo.FechaCreacion = dd + "/" + MM + "/" + yyyy;
                    }
                }
                result.Add("archivo", auxArchivo);
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
        public ActionResult GuardarArchivoEnTrack()
        {
            var result = new Dictionary<string, object>();
            try
            {
                DateTime fecha = DateTime.Now;
                string FileName = "";
                string ruta = "";
                string f = fecha.ToString("ddMMyyyy") + fecha.Hour + fecha.Minute;
                bool pathExist = false;
                bool guardadoEnTrack = false;

                HttpPostedFileBase file = Request.Files["archivoCRC"];
                ModeloArchivoDTO auxArchivo = new ModeloArchivoDTO();
                List<ModeloArchivoDTO> listaArchivos = new List<ModeloArchivoDTO>();

                int idTrack = Int32.Parse(Request.Form["idTrack"]);
                if (file != null && file.ContentLength > 0)
                {
                    FileName = file.FileName;
                    //PRODUCTIVO
                    ruta = archivofs.getArchivo().getUrlDelServidor(12) + f + FileName;
                    //LOCAL
                    //ruta = @"C:\Proyectos\" + f + FileName;
                    //ruta = @"C:\NUNEZ\PRUEBAS\" + f + FileName;
                    pathExist = SaveArchivo(file, ruta);

                    if (pathExist)
                    {
                        auxArchivo.nombre = FileName;
                        auxArchivo.ruta = ruta;
                        auxArchivo.FechaCreacion = f;
                        auxArchivo.FechaCreacionSinFormato = f;
                        guardadoEnTrack = administracionServiciosFactoryServices.getAdministracionServiciosFactoryServices().ActualizarArchivoTrack(idTrack, auxArchivo);

                        string dd = string.Empty;
                        string MM = string.Empty;
                        string yyyy = string.Empty; //091020201137

                        dd = auxArchivo.FechaCreacion.Substring(0, 2);
                        MM = auxArchivo.FechaCreacion.Substring(2, 2);
                        yyyy = auxArchivo.FechaCreacion.Substring(4, 4);
                        auxArchivo.FechaCreacion = dd + "/" + MM + "/" + yyyy;
                    }
                }
                var track = administracionServiciosFactoryServices.getAdministracionServiciosFactoryServices().getTrackingByID(idTrack);
                listaArchivos = JsonConvert.DeserializeObject<List<ModeloArchivoDTO>>(track.archivos);

                result.Add("listaArchivos", listaArchivos);
                result.Add("guardadoEnTrack", guardadoEnTrack);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarArchivoHistServ(string nombre)
        {
            var result = new Dictionary<string, object>();
            try
            {
                ////Productivo
                var ruta = archivofs.getArchivo().getUrlDelServidor(12) + nombre;
                //Local
                //var ruta = @"C:\Users\raguilar\Documents\Proyecto\SIGOPLAN\MAQUINARIA\OVERHAUL\" + nombre;
                System.IO.File.Delete(ruta);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public FileResult DescargarArchivoHistServ(string fecha, string nombre)
        {
            ////Productivo
            var ruta = archivofs.getArchivo().getUrlDelServidor(12) + fecha + nombre;
            //Local
            //var ruta = @"C:\Proyectos\" + fecha + nombre;

            var archivo = File(ruta, MimeMapping.GetMimeMapping(nombre), nombre);
            return archivo;
        }

        #endregion

        #region Taller Overhaul

        public ActionResult CargarGridTallerEstatus(int idCalendario = -1, int estatus = -1, int tipo = -1 /*string economico = "", string obra = "", int grupo = -1, int modelo = -1, int estatus = 1*/)
        {
            var result = new Dictionary<string, object>();
            try
            {
                #region codigo nuevo
                List<TallerOverhaulDTO> data2 = new List<TallerOverhaulDTO>();
                var auxEventos2 = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().CargarGridTallerEstatus(idCalendario, estatus, tipo);
                List<int> _componentesIDs = new List<int>();
                List<int> _serviciosIds = new List<int>();
                foreach (var evento in auxEventos2.Where(x => x.ComponentePlaneacionDTO != null))
                {
                    var componentes = evento.ComponentePlaneacionDTO;
                    var tipoOverhaulGeneral = auxEventos2.FirstOrDefault(x => x.id == evento.id).tipo;
                    foreach (var componente in componentes)
                    {
                        if (componente.Tipo == 1)
                        {
                            _serviciosIds.Add(componente.componenteID);
                        }
                        else 
                        {
                            if (componente.falla == true && componente.Value == "1" && tipoOverhaulGeneral < 3) { }
                            else _componentesIDs.Add(componente.componenteID);
                        }
                    }
                }

                List<tblM_CatServicioOverhaul> catServiciosOverhaul = administracionServiciosFactoryServices.getAdministracionServiciosFactoryServices().GetServiciosByID(_serviciosIds);
                List<tblM_CatComponente> catComponentes = componenteServices.getComponenteService().getComponentesByIDs(_componentesIDs);
                List<int> maquinasId = auxEventos2.Select(x => x.maquinaID).Distinct().ToList();
                List<tblP_CC> _ccs = new List<tblP_CC>();
                List<dynamic> maquinasInfo = new List<dynamic>();

                using (var _ctx = new MainContext())
                {
                    maquinasInfo.AddRange(_ctx.tblM_CatMaquina.Where(x => maquinasId.Contains(x.id)).Select(x => new { noEconomico = x.noEconomico, id = x.id, centro_costos = x.centro_costos, modelo = x.modeloEquipoID} ));
                    _ccs = _ctx.tblP_CC.ToList();
                }

                foreach (var item in auxEventos2)
                {
                    var maquina = maquinasInfo.FirstOrDefault(x => x.id == item.maquinaID);
                    var cc = "N/A";
                    if (maquina != null)
                    {
                        var _cc = _ccs.FirstOrDefault(x => x.areaCuenta == maquina.centro_costos);
                        if (_cc != null)
                        {
                            cc = _cc.descripcion;
                        }
                        else
                        {
                            cc = maquina.centro_costos;
                        }
                    }

                    var componentes = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(item.idComponentes);
                    if (componentes.Count() > 0)
                    {
                        var componentesIDs = componentes.Where(y => y.Tipo != 1).Select(y => y.componenteID).ToList();
                        var serviciosIDs = componentes.Where(y => y.Tipo == 1).Select(y => y.componenteID).ToList();
                        var componentesSP = catComponentes.Where(x => componentesIDs.Contains(x.id)).ToList();
                        var serviciosSP = catServiciosOverhaul.Where(x => serviciosIDs.Contains(x.id)).ToList();

                        foreach (var componente in componentesSP)
                        {
                            var componenteTaller = componentes.FirstOrDefault(x => x.componenteID == componente.id && x.Tipo == 0);
                            //var horometroComp = horometrosComponentes.FirstOrDefault(y => y.componenteID == componente.id);
                            var horometroComp = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHrsCicloActualComponente(componente.id, item.fecha, 0, false);
                            var auxData = new TallerOverhaulDTO
                            {
                                //CCName = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getCCByMaquinaID(item.maquinaID),
                                CCName = cc,
                                maquinaID = item.maquinaID,
                                //economico = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getEconomicoByID(item.maquinaID),
                                economico = maquina != null ? maquina.noEconomico : "N/A",
                                //modeloID = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getModeloIDByMaquinaID(item.maquinaID),
                                modeloID = maquina != null ? maquina.modelo : -1,
                                tipoOverhaul = item.tipo,
                                estatusOverhaul = item.estatus,
                                fecha = item.fecha.ToString("dd/MM/yy"),
                                fechaInicio = item.fechaInicio == null ? "" : (item.fechaInicio ?? default(DateTime)).ToString("dd/MM/yy"),
                                fechaInicioFix = item.fechaInicio == null ? "" : (item.fechaInicio ?? default(DateTime)).ToString("MM/dd/yy"),
                                fechaFinP = item.fechaFinP == null ? "" : (item.fechaFinP ?? default(DateTime)).ToString("dd/MM/yy"),
                                fechaFin = item.fechaFin == null ? "" : (item.fechaFin ?? default(DateTime)).ToString("dd/MM/yy"),
                                fechaFinFix = item.fechaFin == null ? "" : (item.fechaFin ?? default(DateTime)).ToString("MM/dd/yy"),
                                mes = item.fechaInicio == null ? item.fecha.Month : (item.fechaInicio ?? default(DateTime)).Month,
                                id = item.id,
                                diasProgramados = item.diasDuracionP ?? default(decimal),
                                subconjunto = componente.subConjunto.descripcion + (componente.posicionID > 0 ? (" " + EnumHelper.GetDescription((PosicionesEnum)componente.posicionID).ToUpper()) : ""),
                                componente = componente.noComponente,
                                target = componente.cicloVidaHoras,
                                horasComponente = horometroComp == null ? 0 : horometroComp,
                                falla = componenteTaller == null ? false : componenteTaller.falla
                            };
                            data2.Add(auxData);
                        }
                        foreach (var componente in serviciosSP)
                        {
                            var componenteTaller = componentes.FirstOrDefault(x => x.componenteID == componente.id && x.Tipo == 1);
                            var auxData = new TallerOverhaulDTO
                            {
                                //CCName = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getCCByMaquinaID(item.maquinaID),
                                CCName = cc,
                                maquinaID = item.maquinaID,
                                //economico = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getEconomicoByID(item.maquinaID),
                                economico = maquina != null ? maquina.noEconomico : "N/A",
                                //modeloID = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getModeloIDByMaquinaID(item.maquinaID),
                                modeloID = maquina != null ? maquina.modelo : -1,
                                tipoOverhaul = item.tipo,
                                estatusOverhaul = item.estatus,
                                fecha = item.fecha.ToString("dd/MM/yy"),
                                fechaInicio = item.fechaInicio == null ? "" : (item.fechaInicio ?? default(DateTime)).ToString("dd/MM/yy"),
                                fechaInicioFix = item.fechaInicio == null ? "" : (item.fechaInicio ?? default(DateTime)).ToString("MM/dd/yy"),
                                fechaFinP = item.fechaFinP == null ? "" : (item.fechaFinP ?? default(DateTime)).ToString("dd/MM/yy"),
                                fechaFin = item.fechaFin == null ? "" : (item.fechaFin ?? default(DateTime)).ToString("dd/MM/yy"),
                                fechaFinFix = item.fechaFin == null ? "" : (item.fechaFin ?? default(DateTime)).ToString("MM/dd/yy"),
                                mes = item.fechaInicio == null ? item.fecha.Month : (item.fechaInicio ?? default(DateTime)).Month,
                                id = item.id,
                                diasProgramados = item.diasDuracionP ?? default(decimal),
                                subconjunto = componente.servicio.descripcion,
                                componente = componente.servicio.nombre,
                                target = componente.cicloVidaHoras,
                                horasComponente = componente.horasCicloActual,
                                falla = componenteTaller == null ? false : componenteTaller.falla
                            };
                            data2.Add(auxData);
                        }
                    }
                }

                var calendario = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().getCalendarioByID(idCalendario);
                var obra = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getCCByCalendarioID(idCalendario);
                result.Add("anio", calendario.anio);
                result.Add("data", data2);
                result.Add("obra", obra);
                result.Add(SUCCESS, true);
                #endregion

                #region codigo anterior
                //var auxEventos = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().CargarGridTallerEstatus(/*economico, obra, grupo, modelo*/idCalendario, estatus, tipo);/*.Where(x => x.estatus == estatus)*/
                //List<TallerOverhaulDTO> data = new List<TallerOverhaulDTO>();
                //foreach (var item in auxEventos)
                //{
                //    var componentes = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(item.idComponentes);
                //    if (componentes.Count() > 0)
                //    {
                //        var componentesIDs = componentes.Where(y => y.Tipo == 0).Select(y => y.componenteID).ToList();
                //        var serviciosIDs = componentes.Where(y => y.Tipo == 1).Select(y => y.componenteID).ToList();
                //        var componentesSP = componenteServices.getComponenteService().getComponentesByIDs(componentesIDs);
                //        var serviciosSP = administracionServiciosFactoryServices.getAdministracionServiciosFactoryServices().GetServiciosByID(serviciosIDs);

                //        //List<horometrosComponentesDTO> horometrosComponentes = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHrsCicloActualComponentes(componentesIDs);

                //        foreach (var componente in componentesSP)
                //        {
                //            var componenteTaller = componentes.FirstOrDefault(x => x.componenteID == componente.id && x.Tipo == 0);
                //            //var horometroComp = horometrosComponentes.FirstOrDefault(y => y.componenteID == componente.id);
                //            var horometroComp = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHrsCicloActualComponente(componente.id, item.fecha, 0, false);
                //            var auxData = new TallerOverhaulDTO
                //            {
                //                CCName = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getCCByMaquinaID(item.maquinaID),
                //                maquinaID = item.maquinaID,
                //                economico = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getEconomicoByID(item.maquinaID),
                //                modeloID = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getModeloIDByMaquinaID(item.maquinaID),
                //                tipoOverhaul = item.tipo,
                //                estatusOverhaul = item.estatus,
                //                fecha = item.fecha.ToString("dd/MM/yy"),
                //                fechaInicio = item.fechaInicio == null ? "" : (item.fechaInicio ?? default(DateTime)).ToString("dd/MM/yy"),
                //                fechaInicioFix = item.fechaInicio == null ? "" : (item.fechaInicio ?? default(DateTime)).ToString("MM/dd/yy"),
                //                fechaFinP = item.fechaFinP == null ? "" : (item.fechaFinP ?? default(DateTime)).ToString("dd/MM/yy"),
                //                fechaFin = item.fechaFin == null ? "" : (item.fechaFin ?? default(DateTime)).ToString("dd/MM/yy"),
                //                fechaFinFix = item.fechaFin == null ? "" : (item.fechaFin ?? default(DateTime)).ToString("MM/dd/yy"),
                //                mes = item.fechaInicio == null ? item.fecha.Month : (item.fechaInicio ?? default(DateTime)).Month,
                //                id = item.id,
                //                diasProgramados = item.diasDuracionP ?? default(decimal),
                //                subconjunto = componente.subConjunto.descripcion + (componente.posicionID > 0 ? (" " + EnumHelper.GetDescription((PosicionesEnum)componente.posicionID).ToUpper()) : ""),
                //                componente = componente.noComponente,
                //                target = componente.cicloVidaHoras,
                //                //horasComponente = horometroComp == null ? 0 : horometroComp.horometroActual,
                //                horasComponente = horometroComp == null ? 0 : horometroComp,
                //                falla = componenteTaller == null ? false : componenteTaller.falla
                //            };
                //            data.Add(auxData);
                //        }
                //        foreach (var componente in serviciosSP)
                //        {
                //            var componenteTaller = componentes.FirstOrDefault(x => x.componenteID == componente.id && x.Tipo == 1);
                //            var auxData = new TallerOverhaulDTO
                //            {
                //                CCName = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getCCByMaquinaID(item.maquinaID),
                //                maquinaID = item.maquinaID,
                //                economico = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getEconomicoByID(item.maquinaID),
                //                modeloID = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getModeloIDByMaquinaID(item.maquinaID),
                //                tipoOverhaul = item.tipo,
                //                estatusOverhaul = item.estatus,
                //                fecha = item.fecha.ToString("dd/MM/yy"),
                //                fechaInicio = item.fechaInicio == null ? "" : (item.fechaInicio ?? default(DateTime)).ToString("dd/MM/yy"),
                //                fechaInicioFix = item.fechaInicio == null ? "" : (item.fechaInicio ?? default(DateTime)).ToString("MM/dd/yy"),
                //                fechaFinP = item.fechaFinP == null ? "" : (item.fechaFinP ?? default(DateTime)).ToString("dd/MM/yy"),
                //                fechaFin = item.fechaFin == null ? "" : (item.fechaFin ?? default(DateTime)).ToString("dd/MM/yy"),
                //                fechaFinFix = item.fechaFin == null ? "" : (item.fechaFin ?? default(DateTime)).ToString("MM/dd/yy"),
                //                mes = item.fechaInicio == null ? item.fecha.Month : (item.fechaInicio ?? default(DateTime)).Month,
                //                id = item.id,
                //                diasProgramados = item.diasDuracionP ?? default(decimal),
                //                subconjunto = componente.servicio.descripcion,
                //                componente = componente.servicio.nombre,
                //                target = componente.cicloVidaHoras,
                //                horasComponente = componente.horasCicloActual,
                //                falla = componenteTaller == null ? false : componenteTaller.falla
                //            };
                //            data.Add(auxData);
                //        }
                //    }
                //}

                #region comentado
                //var eventos = auxEventos.Select(x =>
                //{

                //    var componentes = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(x.idComponentes).Where(y => !y.falla);
                //    if (componentes.Count() > 0)
                //    {
                //        var componentesIDs = componentes.Where(y => y.Tipo == 0).Select(y => y.componenteID).ToList();
                //        var serviciosIDs = componentes.Where(y => y.Tipo == 1).Select(y => y.componenteID).ToList();
                //        var componentesSP = componenteServices.getComponenteService().getComponentesByIDs(componentesIDs);
                //        var serviciosSP = administracionServiciosFactoryServices.getAdministracionServiciosFactoryServices().GetServiciosByID(serviciosIDs);
                //        return new
                //        {
                //            CCName = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getCCByMaquinaID(x.maquinaID),
                //            economico = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getEconomicoByID(x.maquinaID),
                //            modeloID = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getModeloIDByMaquinaID(x.maquinaID),
                //            tipoOverhaul = x.tipo,
                //            estatusOverhaul = x.estatus,
                //            fecha = x.fecha.ToString("dd/MM/yy"),
                //            fechaInicio = x.fechaInicio == null ? "" : (x.fechaInicio ?? default(DateTime)).ToString("dd/MM/yy"),
                //            fechaInicioFix = x.fechaInicio == null ? "" : (x.fechaInicio ?? default(DateTime)).ToString("MM/dd/yy"),
                //            fechaFinP = x.fechaFinP == null ? "" : (x.fechaFinP ?? default(DateTime)).ToString("dd/MM/yy"),
                //            fechaFin = x.fechaFin == null ? "" : (x.fechaFin ?? default(DateTime)).ToString("dd/MM/yy"),
                //            fechaFinFix = x.fechaFin == null ? "" : (x.fechaFin ?? default(DateTime)).ToString("MM/dd/yy"),
                //            mes = x.fechaInicio == null ? x.fecha.Month : (x.fechaInicio ?? default(DateTime)).Month,
                //            id = x.id,
                //            diasProgramados = x.diasDuracionP,
                //            componentes = componentesSP.Select(y => y.subConjunto.descripcion + (y.posicionID > 0 ? (" " + EnumHelper.GetDescription((PosicionesEnum)y.posicionID).ToUpper()) : "")).Concat(serviciosSP.Select(y => y.servicio.descripcion)),
                //            componentesID = componentesSP.Select(y => y.noComponente).Concat(serviciosSP.Select(y => y.servicio.nombre)),
                //            componentestarget = componentesSP.Select(y => y.cicloVidaHoras).Concat(serviciosSP.Select(y => y.cicloVidaHoras)),
                //            componentesHoras = componentesSP.Select(y => y.horaCicloActual).Concat(serviciosSP.Select(y => y.horasCicloActual)),
                //        };
                //    }
                //    else
                //    {
                //        return null;
                //    }
                //}).Where(x => x != null).ToList();
                #endregion

                //var calendario = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().getCalendarioByID(idCalendario);
                //var obra = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getCCByCalendarioID(idCalendario);
                //result.Add("anio", calendario.anio);
                //result.Add("data", data);
                //result.Add("obra", obra);
                //result.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarGridModalTallerEstatus(int index)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var eventos = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().CargarGridModalTallerEstatus(index).Select(x => new
                {
                    id = x.id,
                    actividad = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getActividadByID(x.id),
                    fecha = x.fechaInicio == null ? null : (x.fechaInicio ?? default(DateTime)).ToString("dd/MM/yy"),
                    fechaFin = x.fechaFin == null ? null : (x.fechaFin ?? default(DateTime)).ToString("dd/MM/yy"),
                    estatus = x.estatus
                }).ToList();
                result.Add("eventos", eventos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IniciarActividadOverhaul(int actividadID, int eventoID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().IniciarActividadOverhaul(actividadID, eventoID);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FinalizarActividadOverhaul(int actividadID, int eventoID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().FinalizarActividadOverhaul(actividadID, eventoID);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarComentarioActividadOverhaul(int actividadID, int eventoID, int tipo, int numDia)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var comentario = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().CargarComentarioActividad(actividadID, eventoID, tipo, numDia);
                if (comentario != null)
                {
                    result.Add("comentario", comentario.comentario);
                    if (comentario.fecha != null) result.Add("fecha", comentario.fecha.ToString("dd/MM/yy"));
                    else result.Add("fecha", null);
                }
                else
                {
                    result.Add("comentario", null);
                    result.Add("fecha", null);
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


        public ActionResult AgregarOverhaulTaller(int maquinaID, List<int> actividadesID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().AgregarOverhaulTaller(maquinaID, actividadesID);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEconomico_Componente(int idModelo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var economicos = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().FillCboEconomico_Componente(idModelo);
                result.Add(ITEMS, economicos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDatosDiagramaGanttEstatus(int idModelo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var actividades = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().CargarDatosDiagramaGantt(idModelo, "", true).Select(x => new
                {
                    id = x.id,
                    descripcion = x.descripcion.ToUpper(),
                    horasDuracion = x.horasDuracion,
                    modeloID = x.modeloID,
                    dia = x.dia
                });
                result.Add("data", actividades);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarDiagramaGantt(int idEvento, List<ActividadOverhaulDTO> actividades, List<decimal> horasTrabajadas)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var sumaHoras = actividades.Sum(x => x.horasDuracion);
                var exito = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().GuardarDiagramaGantt(idEvento, JsonConvert.SerializeObject(actividades), sumaHoras, horasTrabajadas);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDatosDiagramaGantt(int idModelo, int idEvento)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var descripciones = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().CargarDatosDiagramaGantt(idModelo, "", true);
                var actividades = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().CargarDatosDiagramaGantt(idModelo, idEvento).Select(x =>
                {
                    var auxDescripcion = descripciones.FirstOrDefault(y => y.id == x.idAct);
                    return new
                    {
                        id = x.idAct,
                        actividad = auxDescripcion == null ? "" : auxDescripcion.descripcion.ToUpper(),
                        duracion = x.horasDuracion,
                        modeloID = idModelo,
                        dia = x.numDia
                    };
                }).ToList();
                //var actividadesID = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().CargarActGuardadasDiagramaGantt(idEvento);
                var diasTrabajados = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().CargarDiasTrabajadosDG(idEvento);
                var horasTrabajadas = diasTrabajados.FirstOrDefault(x => x > 0);
                var diaActual = actividades.Max(x => x.dia);
                var restaHoras = actividades.Where(x => x.dia == diaActual).Sum(x => x.duracion);
                if (restaHoras == horasTrabajadas)
                {
                    diaActual++;
                    restaHoras = 0;
                }
                result.Add("data", new { actividades = actividades, diasTrabajados = diasTrabajados, horasTrabajadas = horasTrabajadas, diaActual = diaActual, restaHoras = restaHoras });
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboDiagramaGantt(int idModelo, int idEvento)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var items = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().CargarDatosDiagramaGantt(idModelo, "", true).Select(x => new
                {
                    Value = x.id,
                    Text = x.descripcion.ToUpper(),
                    Prefijo = x.horasDuracion
                }).ToList(); result.Add(ITEMS, items);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarCatActTaller(string descripcion, bool estatus, int idModelo = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var actividades = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().CargarDatosDiagramaGantt(idModelo, descripcion, estatus).Select(x => new
                {

                    id = x.id,
                    modeloID = x.modeloID,
                    descripcion = x.descripcion,
                    hrsDuracion = x.horasDuracion,
                    modelo = administracionServiciosFactoryServices.getAdministracionServiciosFactoryServices().getModeloByID(x.modeloID),
                    reporteEjecutivo = x.reporteEjecutivo
                }).ToList();
                result.Add("actividades", actividades);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarActividadTaller(int actividadID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var actividad = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().CargarActividad(actividadID);
                result.Add("actividad", actividad);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarActividadTaller(int actividadID, string descripcion, int modeloID, bool estatus, bool reporteEjecutivo, decimal horasDuracion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().GuardarActividad(actividadID, descripcion, modeloID, estatus, reporteEjecutivo, horasDuracion);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult IniciarOverhaulTaller(int idEvento, DateTime fechaInicio, int tipoOverhaul)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obra = "";
                var economico = "";
                var evento = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getEventoOHByID(idEvento);
                var exito = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().IniciarOverhaul(idEvento, fechaInicio, tipoOverhaul);
                if (exito && evento != null)
                {
                    var maquina = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(evento.maquinaID).FirstOrDefault();
                    economico = "N/A";
                    obra = "N/A";
                    if (maquina != null)
                    {
                        obra = administracionComponentesServices.getAdministracionComponentesFactoryServices().getDescripcionCC(maquina.centro_costos);
                        economico = maquina.noEconomico;
                    }
                }
                result.Add("exito", exito);
                result.Add("economico", economico);
                result.Add("obra", obra);
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
        public ActionResult EnviarCorreoInicioOH(string economico, string obra, List<string> correos, int tipo, int eventoID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<string> mails = new List<string>();
                var evento = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getEventoOHByID(eventoID);
                var componentes = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(evento.idComponentes);
                var tblComponentes = @"<br><br><table>
                    <tr>
                    <th>SUBCONJUNTO</th>
                    <th>SERIE</th>
                    <th>HORAS CICLO</th>
                    </tr>";
                var componentesIDs = componentes.Select(x => x.componenteID).ToList();
                var horometros = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHrsCicloActualComponentes(componentesIDs);

                foreach (var item in componentes)
                {
                    var auxHorometro = horometros.FirstOrDefault(x => x.componenteID == item.componenteID);
                    tblComponentes += "<tr>" +
                        "<td>" + item.descripcion + "</td>" +
                        "<td>" + item.nombre + "</td>" +
                        "<td>" + (auxHorometro != null ? auxHorometro.horometroActual.ToString() : item.horasCiclo.ToString()) + "</td>" +
                    "</tr>";
                }
                tblComponentes += @"</table><br><br>";
                var fecha = DateTime.Today;
                mails.AddRange(correos.Distinct());
                var mensaje = "";
                if (tipo == 0)
                {
                    var downloadPDF = (List<Byte[]>)Session["reporteDG"];
                    mensaje = @"<html>
                    <head><style>                        
                        table, th, td { border: 1px solid black;  border-collapse: collapse; } 
                        th, td {  padding: 5px;} 
                        body { color: #000000; font-family: arial, helvetica, sans-serif; font-size: 16px; }</style></head>
                    <body>";
                    mensaje += "<br> BUEN DÍA. <br> SE ADJUNTA DIAGRAMA DE GANTT DE OVERHAUL PARA EL EQUIPO " + economico + " EL CUAL INICIA EL " + fecha.ToString("dd/MM/yyyy");
                    mensaje += tblComponentes;
                    mensaje += "<br><br>SALUDOS";
                    mensaje += @"<br><br>----------------------------------------------------------------------------------------------<br>ÉSTE ES UN MENSAJE AUTOGENERADO POR EL SISTEMA SIGOPLAN.
                    </body></html>";
                    GlobalUtils.sendEmailAdjuntoInMemorySend(("INICIO OVERHAUL " + economico + " " + obra), mensaje, mails, downloadPDF, "DIAGRAMA DE GANTT");
                }
                else
                {
                    mensaje = @"<html>
                    <head><style>                        
                        table, th, td { border: 1px solid black;  border-collapse: collapse; } 
                        th, td {  padding: 5px;} 
                        body { color: #000000; font-family: arial, helvetica, sans-serif; font-size: 16px; }</style></head>
                    <body>";
                    mensaje += "<br> BUEN DÍA. <br> SE NOTIFICA EL INICIO DE REPARACIONES PARA EL EQUIPO " + economico + " EN LA FECHA " + fecha.ToString("dd/MM/yyyy");
                    mensaje += tblComponentes;
                    mensaje += "<br><br>SALUDOS";
                    mensaje += @"<br><br>----------------------------------------------------------------------------------------------<br>ÉSTE ES UN MENSAJE AUTOGENERADO POR EL SISTEMA SIGOPLAN.
                    </body></html>";
                    GlobalUtils.sendEmail(("INICIO OVERHAUL " + economico + " " + obra), mensaje, mails);
                }
                System.Threading.Thread.Sleep(10000);
                Session["reporteDG"] = null;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                Session["reporteDG"] = null;
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDatosActividadesTaller(int idEvento)
        {
            var result = new Dictionary<string, object>();
            try
            {
                bool terminado = false;
                var actividades = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().CargarDatosActividadesTaller(idEvento).Select(x =>
                {
                    var catActividad = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().CargarActividad(x.idAct);
                    return new
                    {
                        id = x.id,
                        idAct = x.idAct,
                        descripcion = catActividad == null ? "" : catActividad.descripcion,
                        fechaInicio = x.fechaInicio == null ? null : (x.fechaInicio ?? default(DateTime)).ToString("dd/MM/yy"),
                        fechaFin = x.fechaFin == null ? null : (x.fechaFin ?? default(DateTime)).ToString("dd/MM/yy"),
                        horasDuracion = x.horasDuracion,
                        fechaInicioP = x.fechaInicioP == null ? null : (x.fechaInicioP ?? default(DateTime)).ToString("dd/MM/yy"),
                        fechaInicioPRaw = x.fechaInicioP,
                        fechaFinP = x.fechaFinP == null ? null : (x.fechaFinP ?? default(DateTime)).ToString("dd/MM/yy"),
                        estatus = x.estatus,
                        reporteEjecutivo = catActividad.reporteEjecutivo,
                        numDia = x.numDia
                    };
                });
                var actividadesSinTerminar = actividades.Where(x => x.fechaFin == null).ToList();
                if (actividadesSinTerminar.Count < 1) { terminado = true; }
                result.Add("data", actividades);
                result.Add("terminado", terminado);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IniciarActividadOHTaller(int idEvento, int idActividad, int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().IniciarActividad(idEvento, idActividad, id);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FinalizarActividadOHTaller(int idEvento, int idActividad, int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().FinalizarActividad(idEvento, idActividad, id);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GuardarComentarioActividadOverhaul(int actividadID, int eventoID, string comentario, int tipo, int numDia)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().GuardarComentarioActividad(actividadID, eventoID, comentario, tipo, numDia);
                result.Add("exito", exito);
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
        public ActionResult GuardarArchivoActividad()
        {
            var result = new Dictionary<string, object>();
            try
            {
                DateTime fecha = DateTime.Now;
                string FileName = "";
                string ruta = "";
                string f = fecha.ToString("ddMMyyyy") + fecha.Hour + fecha.Minute + fecha.Second + fecha.Millisecond;
                bool pathExist = false;

                HttpPostedFileBase file = Request.Files["archivoActividad"];
                int idEvento = Int32.Parse(Request.Form["idEvento"]);
                int idActividad = Int32.Parse(Request.Form["idActividad"]);
                int numActividad = Int32.Parse(Request.Form["numActividad"]);
                int tipo = Int32.Parse(Request.Form["tipo"]);
                int numDia = Int32.Parse(Request.Form["numDia"]);
                if (file != null && file.ContentLength > 0)
                {
                    FileName = file.FileName;
                    ////Productivo
                    ruta = archivofs.getArchivo().getUrlDelServidor(12) + f + FileName;
                    //Local
                    //ruta = @"C:\Users\raguilar\Documents\Proyecto\SIGOPLAN\MAQUINARIA\OVERHAUL\" + f + FileName;
                    pathExist = SaveArchivo(file, ruta);

                    if (pathExist)
                    {
                        var evento = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getEventoOHByID(idEvento);
                        tblM_ComentarioActividadOverhaul archivo = new tblM_ComentarioActividadOverhaul();
                        var archivosCount = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getArchivosCount(idActividad);
                        archivo.fecha = fecha;
                        archivo.eventoID = idEvento;
                        archivo.actividadID = idActividad;
                        archivo.comentario = ruta;
                        archivo.tipo = tipo;
                        archivo.numDia = numDia;
                        if (tipo == 2) { archivo.nombreArchivo = "Imagen " + (numActividad + 1).ToString() + "."; }
                        else { archivo.nombreArchivo = FileName; }
                        tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().GuardarArchivoActividad(archivo);
                    }
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

        public ActionResult cargarGridArchivosActividad(int idEvento, int idActividad, int tipo, int numDia)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var archivos = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().CargarArchivosActividad(idEvento, idActividad, tipo, numDia).Select(x => new
                {
                    id = x.id,
                    fecha = x.fecha.ToString("dd/MM/yyyy"),
                    nombre = x.nombreArchivo
                });
                result.Add("data", archivos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public FileResult DescargarArchivoActividad(int idComentario)
        {
            tblM_ComentarioActividadOverhaul archivo = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getComentarioByID(idComentario);
            var ruta = archivo.comentario;
            if (archivo.tipo == 2)
            {
                var arregloRuta = archivo.comentario.Split('.');
                var tipoArchivo = "";
                if (arregloRuta.Count() > 1) { tipoArchivo = arregloRuta.Last(); }
                return File(ruta, MimeMapping.GetMimeMapping(archivo.nombreArchivo), archivo.nombreArchivo + "." + tipoArchivo);
            }
            else
            {
                return File(ruta, MimeMapping.GetMimeMapping(archivo.nombreArchivo), archivo.nombreArchivo);
            }

        }

        public ActionResult DeleteArchivoActividad(int idComentario)
        {
            var result = new Dictionary<string, object>();
            try
            {
                tblM_ComentarioActividadOverhaul archivo = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getComentarioByID(idComentario);
                var ruta = archivo.comentario;
                System.IO.File.Delete(ruta);
                var exito = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().EliminarArchivo(archivo.id);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //MODIFICAR ESTA FUNCION
        public ActionResult TerminarOverhaulTaller(int tipoOverhaul, int idEvento, int estatus, DateTime fechaFin)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().TerminarOverhaul(idEvento, fechaFin, tipoOverhaul, estatus);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEconomicosByObraID(string obra = "-1")
        {
            var result = new Dictionary<string, object>();
            try
            {
                var items = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getEconomicosByObraID(obra).Select(x => new { Value = x.id, Text = x.noEconomico }).OrderBy(x => x.Text);
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

        public ActionResult CargarGridOHFallaTaller(int idMaquina)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var componentes = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().CargarGridOHFallaTaller(idMaquina, false).OrderBy(x => x.descripcion);
                result.Add("data", componentes);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarGridOHNuevoParo(int idMaquina)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var componentes = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().CargarGridOHFallaTaller(idMaquina, true).OrderBy(x => x.descripcion);
                result.Add("data", componentes);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarOHFallaTaller(List<ComponentePlaneacionDTO> componentes, int idMaquina, int mes, int anio, int calendarioID, int tipo)
        {
            DateTime fecha = new DateTime(anio, mes, 1);
            var result = new Dictionary<string, object>();
            try
            {
                var exito = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().GuardarOHFallaTaller(idMaquina, componentes, fecha, calendarioID, tipo);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateOHFallaTaller(int id, List<ComponentePlaneacionDTO> componentes, int calendarioID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().UpdateOHFallaTaller(id, componentes, calendarioID);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult VerificarOHFallaTaller(int idMaquina, int mes, int anio, int calendarioID)
        {
            DateTime fecha = new DateTime(anio, mes, 1);
            var result = new Dictionary<string, object>();
            try
            {
                var paroVerificado = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().VerificarOHFallaTaller(idMaquina, fecha, calendarioID);
                if (paroVerificado != null)
                {
                    var data = new
                    {
                        id = paroVerificado.id,
                        fecha = paroVerificado.fechaInicio == null ? paroVerificado.fecha.ToString("dd/MM/yyyy") : (paroVerificado.fechaInicio ?? default(DateTime)).ToString("dd/MM/yyyy")
                    };
                    result.Add("data", data);
                }
                else
                {
                    result.Add("data", paroVerificado);
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

        public ActionResult GuardarOHParo(List<ComponentePlaneacionDTO> componentes, int idMaquina, DateTime fecha, int calendarioID, string indexCal = "")
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().GuardarOHParoTaller(idMaquina, componentes, fecha, calendarioID, indexCal);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getReporteEjecutivo(int idEvento)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var html = "/Reportes/Vista.aspx?idReporte=154&index=" + idEvento + "&tipo=1";
                result.Add("html", html);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getReporteEjecutivoImprimible(int idEvento)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var html = "/Reportes/Vista.aspx?idReporte=155&index=" + idEvento;
                Session["rptReporteEjecutivoOverhaul"] = null;
                result.Add("html", html);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                Session["rptReporteEjecutivoOverhaul"] = null;
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EnviarLiberacionTaller(int idEvento, int tipoOverhaul, List<string> correos)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var downloadPDF2 = (List<Byte[]>)Session["reporteEjecutivo"];
                List<ModeloArchivoDTO> listaArchivos = new List<ModeloArchivoDTO>();
                List<string> adjuntos = new List<string>();
                List<string> nombres = new List<string>();
                var downloadPDF = (List<Byte[]>)Session["reporteDG"];
                var evento = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().getEventoOHByID(idEvento);
                var tipo = evento.tipo;
                var fechaFin = (evento.fechaFin ?? default(DateTime)).ToString("dd/MM/yyyy");
                var exito = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().TerminarOverhaul(idEvento);
                var maquina = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(evento.maquinaID).FirstOrDefault();
                var obra = maquina != null ? getCCName(maquina.centro_costos) : "";
                var archivosFL = tallerOverhaulFactoryServices.getTallerOverhaulFactoryServices().CargarArchivosActividad(idEvento, -1, 3, -1).ToList();
                int i = 0;
                foreach (var item in archivosFL)
                {
                    i++;
                    adjuntos.Add(item.comentario);
                    nombres.Add("Archivo " + i + ".pdf");
                }
                List<string> mails = new List<string>();
                mails.AddRange(correos.Distinct());
                var mensaje = "";
                var asunto = "";
                if (tipo == 0)
                {
                    mensaje = @"<html>
                    <head><style>body { color: #000000; font-family: arial, helvetica, sans-serif; font-size: 16px; }</style></head>
                    <body>";
                    mensaje +=
                        @"<br> BUEN DÍA.
                           <br> SE NOTIFICA QUE EL EQUIPO ";
                    mensaje += maquina != null ? maquina.noEconomico : "";
                    mensaje += " QUEDÓ LIBERADO EL DIA " + fechaFin + " POR OVERHAUL";
                    mensaje += @"<br><br>SE ADJUNTA:<br><br>";
                    mensaje += @"<ul>
                      <li>REPORTE EJECUTIVO</li>
                      <li>DIAGRAMA DE GANTT</li>
                      <li>PRUEBAS DE LIBERACIÓN Y CALIDAD</li>
                    </ul> <br><br>SALUDOS.
                    <br><br>----------------------------------------------------------------------------------------------<br>ÉSTE ES UN MENSAJE AUTOGENERADO POR EL SISTEMA SIGOPLAN.
                    </body></html>";
                    asunto = "LIBERACIÓN DE OVERHAUL";
                }
                else
                {
                    var componentes = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(evento.idComponentes);
                    var auxMensajeComp = "";
                    foreach (var item in componentes)
                    {
                        auxMensajeComp += "<tr>" +
                            "<td>" + item.descripcion + "</td>" +
                            "<td>" + item.nombre + "</td>" +
                            "<td>" + item.horasCiclo + "</td>" +
                        "</tr>";
                    }
                    mensaje = @"<html>
                    <head><style>
                        table, th, td { border: 1px solid black;  border-collapse: collapse; } 
                        th, td {  padding: 5px;} 
                        body { color: #000000; font-family: arial, helvetica, sans-serif; font-size: 16px; }
                    </style></head>
                    <body>";
                    mensaje +=
                        @"<br> BUEN DÍA.
                           <br> SE NOTIFICA QUE EL EQUIPO ";
                    mensaje += maquina != null ? maquina.noEconomico : "";
                    mensaje += " QUEDÓ LIBERADO EL DIA " + fechaFin + " POR ";
                    switch (tipo)
                    {
                        case 1:
                            mensaje += "CAMBIO DE COMPONENTES CON MOTOR";
                            asunto = "LIBERACIÓN DE CAMBIO DE COMPONENTES CON MOTOR";
                            break;
                        case 2:
                            mensaje += "CAMBIO DE COMPONENTES DESFASADOS";
                            asunto = "LIBERACIÓN DE CAMBIO DE COMPONENTES DESFASADOS";
                            break;
                        default:
                            mensaje += "CAMBIO DE COMPONENTES POR FALLA";
                            asunto = "LIBERACIÓN DE CAMBIO DE COMPONENTES POR FALLA";
                            break;
                    }
                    mensaje += @"<br><br><table>
                      <tr>
                        <th>COMPONENTE</th>
                        <th>SERIE</th>
                        <th>HORAS CICLO</th>
                      </tr>";
                    mensaje += auxMensajeComp;
                    mensaje += @"</table><br><br>SE ADJUNTA:<br><br>";
                    mensaje += @"<ul><li>PRUEBAS DE LIBERACIÓN Y CALIDAD</li></ul> <br><br>SALUDOS.
                    <br><br>----------------------------------------------------------------------------------------------<br>ÉSTE ES UN MENSAJE AUTOGENERADO POR EL SISTEMA SIGOPLAN.
                    </body></html>";
                }
                GlobalUtils.sendEmailAdjuntoLiberacionOH(asunto + " " + maquina.noEconomico + " " + obra, mensaje, mails, downloadPDF, downloadPDF2, adjuntos, nombres);
                System.Threading.Thread.Sleep(10000);
                Session["reporteEjecutivo"] = null;
                Session["reporteDG"] = null;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                Session["reporteEjecutivo"] = null;
                Session["reporteDG"] = null;
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult EnviarReporteAdministradorComponentes(int tipo, List<string> correos)
        {
            var result = new Dictionary<string, object>();
            try
            {
                
                List<ModeloArchivoDTO> listaArchivos = new List<ModeloArchivoDTO>();          
                
                List<string> mails = new List<string>();
                mails.AddRange(correos.Distinct());
                var mensaje = "";
                
                if (tipo == 1)
                {
                    var asunto = "Reporte operativo";
                    var downloadPDF = (List<Byte[]>)Session["reporteTiempos_Reparacion"];
                    mensaje = @"<html>
                    <head><style>
                        table, th, td { border: 1px solid black;  border-collapse: collapse; } 
                        th, td {  padding: 5px;} 
                        body { color: #000000; font-family: arial, helvetica, sans-serif; font-size: 16px; }
                    </style></head>
                    <body>";
                    mensaje +=
                        @"<br> BUEN DÍA.
                           <br> Reporte administracion de componetes ";
                    mensaje += @"</table><br><br>SE ADJUNTA:<br><br>";
                    mensaje += @"<br><br>----------------------------------------------------------------------------------------------<br>ÉSTE ES UN MENSAJE AUTOGENERADO POR EL SISTEMA SIGOPLAN.
                    </body></html>";

                    GlobalUtils.sendEmailAdjuntoInMemorySend(asunto, mensaje, mails, downloadPDF, "reporteOperativo");
                    System.Threading.Thread.Sleep(10000);
                }
                else
                {
                    if (tipo == 2)
                    {
                        var asunto = "Reporte Adminitrativo";

                         var downloadPDF = (List<Byte[]>)Session["ReporteTiemposCRCAdmin"];
                    mensaje = @"<html>
                    <head><style>
                        table, th, td { border: 1px solid black;  border-collapse: collapse; } 
                        th, td {  padding: 5px;} 
                        body { color: #000000; font-family: arial, helvetica, sans-serif; font-size: 16px; }
                    </style></head>
                    <body>";
                    mensaje +=
                        @"<br> BUEN DÍA.
                           <br> Reporte administracion de componetes ";
                    mensaje += @"</table><br><br>SE ADJUNTA:<br><br>";
                    mensaje += @"<br><br>----------------------------------------------------------------------------------------------<br>ÉSTE ES UN MENSAJE AUTOGENERADO POR EL SISTEMA SIGOPLAN.
                    </body></html>";

                    GlobalUtils.sendEmailAdjuntoInMemorySend(asunto, mensaje, mails, downloadPDF, "reporteAdministrativo");
                    System.Threading.Thread.Sleep(10000);
                    }
                }                    
                Session["reporteTiempos_Reparacion"] = null;
                Session["ReporteTiemposCRCAdmin"] = null;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                Session["reporteTiempos_Reparacion"] = null;
                Session["ReporteTiemposCRCAdmin"] = null;
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

       

        #endregion

        #region Presupuestos Overhaul

        public ActionResult CargarTblPresupuesto(List<string> obras, int modeloID, int anio = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (obras != null)
                {
                    var maquinas = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().GetMaquinasPresupuestar(obras, modeloID, anio);
                    var presupuesto = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().GetPresupuesto(modeloID, anio);
                    var componentes = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().CargarTblPresupuesto(obras, modeloID, anio, maquinas, presupuesto);
                    var maquinasComponentes = componentes.SelectMany(x => x.maquinasComponentes).Distinct().ToList();
                    result.Add("data", componentes);
                    result.Add("maquinas", maquinas.Where(x => maquinasComponentes.Contains(x.noEconomico)).Select(x => new { id = x.id, noEconomico = x.noEconomico }).ToList());
                    result.Add("presupuestoID", presupuesto == null ? 0 : presupuesto.id);
                    result.Add("cerrado", presupuesto == null ? false : presupuesto.cerrado);
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


        public ActionResult GetDetallePresupuesto(int modelo, int subConjunto, string obras, int presupuestoID, int vidas = -1, int anio = -1, bool esServicio = false)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<string> lstObras = obras.Split(',').ToList();
                var componentesPresupuesto = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().GetDetallePresupuesto(lstObras, vidas, modelo, anio, subConjunto, presupuestoID, esServicio);
                result.Add("data", componentesPresupuesto);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult fillCboAnioPresupuesto()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().fillCboAnioPresupuesto();
                result.Add(ITEMS, data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarTblAutorizacion(List<string> obras, int anio = -1, int modeloID = 0)
        {
            var result = new Dictionary<string, object>();
            if (obras == null) { obras = new List<string>(); }
            try
            {
                var presupuestos = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().CargarTblAutorizacion(obras, modeloID, anio).Select(x =>
                {
                    var auxModelo = modelofs.getModeloEquipoService().getModeloByID(x.modelo);
                    List<FechasPresupuestoObraDTO> fechas = JsonConvert.DeserializeObject<List<FechasPresupuestoObraDTO>>(x.JsonObras);
                    return new
                    {
                        presupuestoID = x.id,
                        anio = x.anio,
                        calendarioID = x.calendarioID,
                        cerrado = x.cerrado,
                        estado = x.estado,
                        fechaEnvio = fechas.FirstOrDefault().fechaEnvio.ToString("dd/MM/yyyy"),
                        fechaVoBo1 = fechas.FirstOrDefault().fechaVoBo1.ToString("dd/MM/yyyy"),
                        fechaVoBo2 = fechas.FirstOrDefault().fechaVoBo2.ToString("dd/MM/yyyy"),
                        fechaVoBo3 = fechas.FirstOrDefault().fechaVoBo3.ToString("dd/MM/yyyy"),
                        fechaAutorizacion = fechas.FirstOrDefault().fechaAutorizacion.ToString("dd/MM/yyyy"),
                        JsonObras = x.JsonObras,
                        modeloID = x.modelo,
                        modelo = auxModelo == null ? x.modelo.ToString() : auxModelo.descripcion
                    };
                });
                var idUsuario = getUsuario().id;
                var tipoUsuario = tipoUsuarioOverhaul(idUsuario);
                result.Add("data", presupuestos);
                result.Add("tipoUsuario", tipoUsuario);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarCostoPresupuesto(int componenteID, int maquinaID, decimal costo = 0, decimal costoSugerido = 0, int eventoID = -1, int modelo = -1, int anio = -1, int presupuestoID = 0, int vidas = 0, bool esServicio = false)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var presupuesto = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().GuardarCostoPresupuesto(componenteID, maquinaID, costo, costoSugerido, eventoID, modelo, anio, presupuestoID, vidas, esServicio);
                result.Add("presupuestoID", presupuesto);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IniciarPresupuesto(int modelo = -1, int anio = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var presupuesto = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().IniciarPresupuesto(modelo, anio);
                result.Add("presupuestoID", presupuesto);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CerrarPresupuesto(int presupuestoID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().CerrarPresupuesto(presupuestoID);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarAumentoPresupuesto(decimal aumento, string comentario, int presupuestoID, int componenteID, int tipo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var costo = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().GuardarAumentoPresupuesto(aumento, comentario, presupuestoID, componenteID, tipo);
                result.Add("costo", costo);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ComentarioAumPresupuesto(int presupuestoID, int componenteID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var comentario = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().ComentarioAumPresupuesto(presupuestoID, componenteID);
                result.Add("comentario", comentario);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarTblAvance(List<string> obras, int estatus, int anio = -1, int modeloID = 0)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var presupuestos = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().CargarTblAvance(obras, modeloID, anio, estatus);
                result.Add("data", presupuestos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetReporteTblAvance(List<string> obras, int estatus, int anio = -1, int modeloID = 0)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var presupuestos = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().CargarTblAvance(obras, modeloID, anio, estatus);
                result.Add("data", presupuestos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult CargarTblAvanceDetalle(int idDetalle)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var presupuestos = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().CargarTblAvanceDetalle(idDetalle);
                result.Add("data", presupuestos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult CargarAvanceGeneral(List<string> obras, int estatus, int anio = -1, int modeloID = 0)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var presupuestos = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().CargarAvanceGeneral(obras, modeloID, anio, estatus);
                result.Add("data", presupuestos.Where(x => x.estado != -1));
                result.Add("dataAtrasados", presupuestos.Where(x => x.estado == -1));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetDetallePresAuto(int modelo, List<string> obras, int anio = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var componentesPresAuto = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().GetDetallePresAuto(obras, modelo, anio);
                result.Add("data", componentesPresAuto);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutorizarPresupuesto(int presupuestoID = 0, int modelo = 0, int anio = -1, string obra = "", int tipo = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var modeloDescr = administracionServiciosFactoryServices.getAdministracionServiciosFactoryServices().getModeloByID(modelo);
                var obraDescr = administracionComponentesServices.getAdministracionComponentesFactoryServices().getDescripcionCC(obra);
                var exito = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().AutorizarPresupuesto(presupuestoID, modelo, anio, obra, tipo);
                if (exito) { EnviarCorreoAutoPres(modeloDescr, obraDescr.ToUpper(), anio, tipo); }
                result.Add("exito", exito);
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
        public ActionResult EnviarCorreoAutoPres(string modelo, string obra, int anio, int tipo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<string> mails = new List<string>();
                mails.Add(EnumHelper.GetDescription((CorreosOverhaulEnum)1));
                mails.Add(EnumHelper.GetDescription((CorreosOverhaulEnum)2));
                mails.Add(EnumHelper.GetDescription((CorreosOverhaulEnum)3));
                mails.Add(EnumHelper.GetDescription((CorreosOverhaulEnum)4));
                mails.Add(EnumHelper.GetDescription((CorreosOverhaulEnum)6));
                var mensaje = "";
                var auxMensajeComp = "<tr>" +
                    "<td>JOSE PEDRO LOPEZ PROVENCIO</td>" +
                    "<td style='background-color:green'>ENVIO</td>" +
                "</tr>";
                auxMensajeComp += "<tr>" +
                    "<td>LUIS FORTINO CERVANTES LIZARRAGA</td>" +
                    "<td" + (tipo > 0 ? " style='background-color:green'>VoBo" : ">PENDIENTE VoBo") + "</td>" +
                "</tr>";
                auxMensajeComp += "<tr>" +
                    "<td>OSCAR MANUEL ROMAN RUIZ</td>" +
                    "<td" + (tipo > 1 ? " style='background-color:green'>VoBo" : ">PENDIENTE VoBo") + "</td>" +
                "</tr>";
                auxMensajeComp += "<tr>" +
                    "<td>JOSE MANUEL GAYTAN LIZAMA</td>" +
                    "<td" + (tipo > 2 ? " style='background-color:green'>VoBo" : ">PENDIENTE VoBo") + "</td>" +
                "</tr>";
                auxMensajeComp += "<tr>" +
                    "<td>GERARDO REINA CECCO</td>" +
                    "<td" + (tipo > 3 ? " style='background-color:green'>AUTORIZADO" : ">PENDIENTE AUTORIZACIÓN") + "</td>" +
                "</tr>";
                mensaje = @"<html>
                <head><style>
                    table, th, td { border: 1px solid black;  border-collapse: collapse; } 
                    th, td {  padding: 5px;} 
                    body { color: #000000; font-family: arial, helvetica, sans-serif; font-size: 16px; }
                </style></head>
                <body>";
                mensaje +=
                    @"<br> BUEN DÍA.
                        <br> SE GENERÓ EL PRESUPUESTO DE OVERHAUL PARA EL MODELO ";
                mensaje += modelo + " " + anio;
                mensaje += @"<br><br><table>
                    <tr>
                    <th>AUTORIZANTE</th>
                    <th>ESTADO</th>
                    </tr>";
                mensaje += auxMensajeComp;
                mensaje += @"</table><br><br>";
                mensaje += @"<br><br>SALUDOS.
                <br><br>----------------------------------------------------------------------------------------------<br>ÉSTE ES UN MENSAJE AUTOGENERADO POR EL SISTEMA SIGOPLAN.
                </body></html>";

                GlobalUtils.sendEmail("SE GENERÓ EL PRESUPUESTO DE OVERHAUL PARA EL MODELO " + modelo + " " + anio, mensaje, mails);
                System.Threading.Thread.Sleep(5000);
                Session["reporteEjecutivo"] = null;
                Session["reporteDG"] = null;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                Session["reporteEjecutivo"] = null;
                Session["reporteDG"] = null;
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarTblModalAutorizacion(int presupuestoID, string obra, int subconjunto = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var detallePres = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().GetDetalleModalAuto(presupuestoID, obra).Where(x => (subconjunto == -1 ? true : x.subconjuntoID == subconjunto)).Select(x =>
                {
                    return new
                    {
                        noEconomico = x.maquina == null ? "N/A" : x.maquina.noEconomico,
                        noComponente = x.componente == null ? "N/A" : x.componente.noComponente,
                        subconjunto = //x.componente.subConjunto.descripcion,
                            x.componente.subConjunto.descripcion + " " + (x.componente.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)x.componente.posicionID).ToUpper() : ""),
                        horasCiclo = x.horasCiclo,
                        target = x.componente == null ? 0 : x.componente.cicloVidaHoras,
                        costo = x.costoPresupuesto,
                        fechaID = x.fecha,
                        fecha = x.fecha.ToString("dd/MM/yyyy"),
                        causa = x.tipo,
                        costoReal = x.costoReal,
                        programado = x.programado ? "Si" : "No"
                    };
                }).ToList();
                result.Add("data", detallePres);
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

        #region Reportes

        public ActionResult getReporteInventario(int grupo, int modelo, int conjunto, int subconjunto)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var inventario = administracionComponentesServices.getAdministracionComponentesFactoryServices().CargarReporteInventario(grupo, modelo, conjunto, subconjunto, "");
                Session["rptInventarioComponente"] = inventario;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cargarTblInversion(int calendarioID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<string> meses = (new string[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" }).ToList();
                List<string> auxTipoParo = (new string[] { "Overhaul General", "Cambio de Motor", "Componentes Desfasados", "Fallo" }).ToList();

                var calendario = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().getCalendarioByID(calendarioID);

                var calendarios = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().CargarTblInversion(calendarioID).Where(x => x.tipo < 3).Select(x =>
                {
                    var maquina = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(x.maquinaID).FirstOrDefault();
                    var componentes = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(x.idComponentes);
                    var componentesIDs = componentes.Select(y => y.componenteID).ToList();
                    List<horometrosComponentesDTO> horometrosComponentes = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHrsCicloActualComponentes(componentesIDs);

                    List<ReporteInversionOverhaulDTO> data = new List<ReporteInversionOverhaulDTO>();
                    foreach (var item in componentes)
                    {
                        bool existeComponente = false;
                        if (item.Tipo == 0)
                        {
                            var auxComponente = componenteServices.getComponenteService().getComponenteByID(item.componenteID);
                            existeComponente = (auxComponente != null);
                        }
                        else
                        {
                            var auxServicio = administracionServiciosFactoryServices.getAdministracionServiciosFactoryServices().GetServicioByID(item.componenteID);
                            existeComponente = (auxServicio != null);
                        }

                        if (existeComponente)
                        {
                            var horasCiclo = item.horasCiclo;
                            if (horometrosComponentes.FirstOrDefault(y => y.componenteID == item.componenteID) != null && item.Tipo == 0) horasCiclo = horometrosComponentes.FirstOrDefault(y => y.componenteID == item.componenteID).horometroActual;
                            DateTime fechaRemocion = new DateTime();
                            var presupuestoDetalle = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().GetDetalleByComp(item.componenteID, calendario.anio);
                            bool parse = false;
                            if (item.Value == "1") parse = DateTime.TryParse(item.fechaRemocion, out fechaRemocion);
                            ReporteInversionOverhaulDTO auxData = new ReporteInversionOverhaulDTO();
                            auxData.componente = item.nombre;
                            auxData.equipo = maquina == null ? "" : maquina.noEconomico;
                            auxData.erogado = presupuestoDetalle == null ? 0 : presupuestoDetalle.costoReal;
                            auxData.horasComponente = horasCiclo;
                            auxData.numMes = x.fecha.Month;
                            auxData.fechaRemocion = item.Value == "1" && parse ? fechaRemocion.ToString("dd/MM/yyyy") : "--";
                            auxData.mes = meses[auxData.numMes - 1];
                            auxData.presupuesto = presupuestoDetalle == null ? 0 : presupuestoDetalle.costoPresupuesto;
                            auxData.proximoPCR = x.fecha.ToString("dd/MM/yyyy");
                            auxData.ritmo = x.ritmo;
                            auxData.target = item.target;
                            auxData.numTipoParo = x.tipo;
                            auxData.tipoParo = auxTipoParo[x.tipo];
                            auxData.subconjunto = item.descripcion;
                            auxData.paroID = x.id;
                            auxData.paroTerminado = x.terminado;
                            data.Add(auxData);
                        }
                    }
                    return data;
                });
                var detalles = calendarios.SelectMany(x => x).OrderBy(x => x.numMes).ThenBy(x => x.equipo).ToList();
                result.Add("detalles", detalles);
                result.Add("pAutorizado", detalles.Sum(s => s.presupuesto));
                result.Add("pProgramado", detalles.Sum(s => s.presupuesto));
                result.Add("eProgramado", detalles.Where(w => w.numTipoParo < 3).Sum(s => s.erogado));
                result.Add("eNoProgramado", detalles.Where(w => w.numTipoParo == 3).Sum(s => s.erogado));
                result.Add("pTotal", result["pProgramado"]);
                result.Add("eTotal", Convert.ToDecimal(result["eProgramado"]) + Convert.ToDecimal(result["eNoProgramado"]));
                result.Add("bolsa", Convert.ToDecimal(result["pAutorizado"]) - Convert.ToDecimal(result["eTotal"]));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cargarTblInversionFiltrada(List<string> obras, List<int> modelos, int anio)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var presupuestos = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().GetPresupuestos(modelos, anio);
                var presupuestosID = presupuestos.Select(x => x.id).ToList();
                var detalles = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().GetDetallePresupuestos(presupuestosID, obras, anio, modelos);

                if (anio == 2021)
                {
                    var detallesPresupuestoHC = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().GetPresupuestoHC(obras);

                    result.Add("pAutorizado", detallesPresupuestoHC.Sum(s => s.pAutorizado));
                    result.Add("pProgramado", detallesPresupuestoHC.Sum(s => s.pProgramado));
                    result.Add("eProgramado", detallesPresupuestoHC.Sum(s => s.eProgramado));
                    result.Add("eNoProgramado", detallesPresupuestoHC.Sum(s => s.eNoProgramado));
                    result.Add("pTotal", detallesPresupuestoHC.Sum(s => s.pTotal));
                    result.Add("eTotal", detallesPresupuestoHC.Sum(s => s.eTotal));
                    result.Add("bolsa", detallesPresupuestoHC.Sum(s => s.bolsa));
                }
                else 
                {
                    result.Add("pAutorizado", detalles.Sum(s => s.presupuesto));
                    result.Add("pProgramado", detalles.Where(x => x.paroTerminado && x.programado).Sum(s => s.presupuesto));
                    result.Add("eProgramado", detalles.Where(x => x.paroTerminado && x.programado).Sum(s => s.erogado));
                    result.Add("eNoProgramado", detalles.Where(x => x.paroTerminado && !x.programado).Sum(s => s.erogado));
                    result.Add("pTotal", result["pProgramado"]);
                    result.Add("eTotal", Convert.ToDecimal(result["eProgramado"]) + Convert.ToDecimal(result["eNoProgramado"]));
                    result.Add("bolsa", Convert.ToDecimal(result["pAutorizado"]) - Convert.ToDecimal(result["eTotal"]));
                }

                detalles = detalles.OrderBy(x => x.numMes).ThenBy(x => x.equipo).ToList();
                result.Add("detalles", detalles);
                result.Add("pAutorizado", detalles.Sum(s => s.presupuesto));
                result.Add("pProgramado", detalles.Where(x => x.paroTerminado && x.programado).Sum(s => s.presupuesto));
                result.Add("eProgramado", detalles.Where(x => x.paroTerminado && x.programado).Sum(s => s.erogado));
                result.Add("eNoProgramado", detalles.Where(x => x.paroTerminado && !x.programado).Sum(s => s.erogado));
                result.Add("pTotal", result["pProgramado"]);
                result.Add("eTotal", Convert.ToDecimal(result["eProgramado"]) + Convert.ToDecimal(result["eNoProgramado"]));
                result.Add("bolsa", Convert.ToDecimal(result["pAutorizado"]) - Convert.ToDecimal(result["eTotal"]));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cargarTblInversionObra(int modelo = -1, string obra = "", int anio = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<string> meses = (new string[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" }).ToList();
                List<string> auxTipoParo = (new string[] { "Overhaul General", "Cambio de Motor", "Componentes Desfasados", "Fallo" }).ToList();
                List<ReporteInversionOverhaulDTO> detalles = new List<ReporteInversionOverhaulDTO>();
                var calendario = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().getCalendarioByObra(obra, anio);
                foreach (var auxCalendario in calendario)
                {
                    var calendarios = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().CargarTblInversion(auxCalendario.id).Select(x =>
                    {
                        var maquina = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(x.maquinaID).FirstOrDefault();
                        var componentes = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(x.idComponentes);
                        var componentesIDs = componentes.Select(y => y.componenteID).ToList();
                        List<horometrosComponentesDTO> horometrosComponentes = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHrsCicloActualComponentes(componentesIDs);

                        List<ReporteInversionOverhaulDTO> data = new List<ReporteInversionOverhaulDTO>();
                        if (maquina.modeloEquipoID == modelo)
                        {
                            foreach (var item in componentes)
                            {
                                bool existeComponente = false;
                                if (item.Tipo == 0)
                                {
                                    var auxComponente = componenteServices.getComponenteService().getComponenteByID(item.componenteID);
                                    existeComponente = (auxComponente != null);
                                }
                                else
                                {
                                    var auxServicio = administracionServiciosFactoryServices.getAdministracionServiciosFactoryServices().GetServicioByID(item.componenteID);
                                    existeComponente = (auxServicio != null);
                                }

                                if (existeComponente)
                                {
                                    var horasCiclo = item.horasCiclo;
                                    if (horometrosComponentes.FirstOrDefault(y => y.componenteID == item.componenteID) != null && item.Tipo == 0) horasCiclo = horometrosComponentes.FirstOrDefault(y => y.componenteID == item.componenteID).horometroActual;
                                    DateTime fechaRemocion = new DateTime();
                                    var presupuestoDetalle = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().GetDetalleByComp(item.componenteID, auxCalendario.anio);
                                    bool parse = false;
                                    if (item.Value == "1") parse = DateTime.TryParse(item.fechaRemocion, out fechaRemocion);
                                    ReporteInversionOverhaulDTO auxData = new ReporteInversionOverhaulDTO();
                                    auxData.componente = item.nombre;
                                    auxData.equipo = maquina == null ? "" : maquina.noEconomico;
                                    auxData.erogado = presupuestoDetalle == null ? 0 : presupuestoDetalle.costoReal;
                                    auxData.horasComponente = horasCiclo;
                                    auxData.numMes = x.fecha.Month;
                                    auxData.fechaRemocion = item.Value == "1" && parse ? fechaRemocion.ToString("dd/MM/yyyy") : "--";
                                    auxData.mes = meses[auxData.numMes - 1];
                                    auxData.presupuesto = presupuestoDetalle == null ? 0 : presupuestoDetalle.costoPresupuesto;
                                    auxData.proximoPCR = x.fecha.ToString("dd/MM/yyyy");
                                    auxData.ritmo = x.ritmo;
                                    auxData.target = item.target;
                                    auxData.numTipoParo = x.tipo;
                                    auxData.tipoParo = auxTipoParo[x.tipo];
                                    auxData.subconjunto = item.descripcion;
                                    auxData.paroID = x.id;
                                    auxData.paroTerminado = x.terminado;
                                    data.Add(auxData);
                                }
                            }
                        }
                        return data;
                    });
                    var auxDetalles = calendarios.SelectMany(x => x).OrderBy(x => x.numMes).ThenBy(x => x.equipo).ToList();
                    detalles.AddRange(auxDetalles);
                }
                detalles = detalles.OrderBy(x => x.numMes).ThenBy(x => x.equipo).ToList();
                result.Add("detalles", detalles);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cargarTblCalendarioEjec(string obra = "", int anio = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var calendariosEjecutados = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().CargarCalendariosEjec(obra, anio).Select(x => new
                {
                    nombre = x.nombre,
                    id = x.id,
                    fecha = x.fecha.ToString("dd/MM/yyy"),
                    obraID = x.obraID,
                    anio = x.anio,
                    estatus = x.estatus
                });
                result.Add("calendarios", calendariosEjecutados);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReporteInversion(List<string> obras, List<int> modelos, int anio)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var presupuestos = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().GetPresupuestos(modelos, anio);
                var presupuestosID = presupuestos.Select(x => x.id).ToList();
                var detalles = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().GetDetallePresupuestos(presupuestosID, obras, anio, modelos);
                detalles = detalles.OrderBy(x => x.numMes).ThenBy(x => x.equipo).ToList();
                Session["rptProgramaCambioComp"] = detalles;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReporteCalendarioInversion(int calendarioID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<string> meses = (new string[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" }).ToList();
                List<string> auxTipoParo = (new string[] { "Overhaul General", "Cambio de Motor", "Componentes Desfasados", "Fallo" }).ToList();
                var calendario = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().getCalendarioByID(calendarioID);
                var calendarios = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().CargarTblInversion(calendarioID).Where(x => x.tipo < 3).Select(x =>
                {
                    var maquina = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(x.maquinaID).FirstOrDefault();
                    var componentes = JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(x.idComponentes);
                    List<ReporteInversionOverhaulDTO> data = new List<ReporteInversionOverhaulDTO>();
                    foreach (var item in componentes)
                    {
                        DateTime fechaRemocion = new DateTime();
                        var presupuestoDetalle = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().GetDetalleByComp(item.componenteID, calendario.anio);
                        bool parse = false;
                        if (item.Value == "1") parse = DateTime.TryParse(item.fechaRemocion, out fechaRemocion);
                        ReporteInversionOverhaulDTO auxData = new ReporteInversionOverhaulDTO();
                        auxData.componente = item.nombre;
                        auxData.equipo = maquina == null ? "" : maquina.noEconomico;
                        auxData.erogado = presupuestoDetalle == null ? 0 : presupuestoDetalle.costoReal;
                        auxData.horasComponente = item.horasCiclo;
                        auxData.numMes = x.fecha.Month;
                        auxData.fechaRemocion = item.Value == "1" && parse ? fechaRemocion.ToString("dd/MM/yyyy") : "--";
                        auxData.mes = meses[auxData.numMes - 1];
                        auxData.presupuesto = presupuestoDetalle == null ? 0 : presupuestoDetalle.costoPresupuesto;
                        auxData.proximoPCR = x.fecha.ToString("dd/MM/yyyy");
                        auxData.ritmo = x.ritmo;
                        auxData.target = item.target;
                        auxData.numTipoParo = x.tipo;
                        auxData.tipoParo = auxTipoParo[x.tipo];
                        auxData.subconjunto = item.descripcion;
                        auxData.paroID = x.id;
                        auxData.paroTerminado = x.terminado;
                        data.Add(auxData);
                    }
                    return data;
                });
                var detalles = calendarios.SelectMany(x => x).OrderBy(x => x.numMes).ThenBy(x => x.equipo).ToList();
                Session["rptProgramaCambioComp"] = detalles;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetReporteDisponibilidad(string obra, int anio)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var overhauls = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().GetReporteDisponibilidad(new List<string> { obra }, anio).Select(x =>
                {
                    var auxMaquina = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(x.idMaquina).FirstOrDefault();
                    var diferencia = x.fechaInicio.Day - (x.fechaFin.AddDays(1 - x.fechaFin.Day).AddMonths(1).AddDays(-1).Day - x.fechaFin.Day);
                    return new
                    {
                        maquina = auxMaquina.noEconomico,
                        fechaInicio = x.fechaInicio,
                        fechaFin = x.fechaFin,
                        mes = diferencia > 0 ? x.fechaInicio.Month : x.fechaFin.Month,
                        enero = x.disponibilidad[0].disponibilidad,
                        febrero = x.disponibilidad[1].disponibilidad,
                        marzo = x.disponibilidad[2].disponibilidad,
                        abril = x.disponibilidad[3].disponibilidad,
                        mayo = x.disponibilidad[4].disponibilidad,
                        junio = x.disponibilidad[5].disponibilidad,
                        julio = x.disponibilidad[6].disponibilidad,
                        agosto = x.disponibilidad[7].disponibilidad,
                        septiembre = x.disponibilidad[8].disponibilidad,
                        octubre = x.disponibilidad[9].disponibilidad,
                        noviembre = x.disponibilidad[10].disponibilidad,
                        diciembre = x.disponibilidad[11].disponibilidad
                    };
                });
                Session["rptDisponibilidadOverhaul"] = overhauls;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarTblDispOverhaul(string obra, int anio)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var overhauls = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().GetReporteDisponibilidad(new List<string> { obra }, anio).Select(x =>
                {
                    var auxMaquina = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(x.idMaquina).FirstOrDefault();
                    var diferencia = x.fechaInicio.Day - (x.fechaFin.AddDays(1 - x.fechaFin.Day).AddMonths(1).AddDays(-1).Day - x.fechaFin.Day);
                    var mes = diferencia > 0 ? x.fechaInicio.Month - 1 : x.fechaFin.Month - 1;
                    return new
                    {
                        id = x.idMaquina,
                        maquina = auxMaquina.noEconomico,
                        enero = mes == 0 ? (x.fechaInicio.ToString("dd/MM/yyy") + " - " + x.fechaFin.ToString("dd/MM/yyy")) : x.disponibilidad[0].disponibilidad,
                        febrero = mes == 1 ? (x.fechaInicio.ToString("dd/MM/yyy") + " - " + x.fechaFin.ToString("dd/MM/yyy")) : x.disponibilidad[1].disponibilidad,
                        marzo = mes == 2 ? (x.fechaInicio.ToString("dd/MM/yyy") + " - " + x.fechaFin.ToString("dd/MM/yyy")) : x.disponibilidad[2].disponibilidad,
                        abril = mes == 3 ? (x.fechaInicio.ToString("dd/MM/yyy") + " - " + x.fechaFin.ToString("dd/MM/yyy")) : x.disponibilidad[3].disponibilidad,
                        mayo = mes == 4 ? (x.fechaInicio.ToString("dd/MM/yyy") + " - " + x.fechaFin.ToString("dd/MM/yyy")) : x.disponibilidad[4].disponibilidad,
                        junio = mes == 5 ? (x.fechaInicio.ToString("dd/MM/yyy") + " - " + x.fechaFin.ToString("dd/MM/yyy")) : x.disponibilidad[5].disponibilidad,
                        julio = mes == 6 ? (x.fechaInicio.ToString("dd/MM/yyy") + " - " + x.fechaFin.ToString("dd/MM/yyy")) : x.disponibilidad[6].disponibilidad,
                        agosto = mes == 7 ? (x.fechaInicio.ToString("dd/MM/yyy") + " - " + x.fechaFin.ToString("dd/MM/yyy")) : x.disponibilidad[7].disponibilidad,
                        septiembre = mes == 8 ? (x.fechaInicio.ToString("dd/MM/yyy") + " - " + x.fechaFin.ToString("dd/MM/yyy")) : x.disponibilidad[8].disponibilidad,
                        octubre = mes == 9 ? (x.fechaInicio.ToString("dd/MM/yyy") + " - " + x.fechaFin.ToString("dd/MM/yyy")) : x.disponibilidad[9].disponibilidad,
                        noviembre = mes == 10 ? (x.fechaInicio.ToString("dd/MM/yyy") + " - " + x.fechaFin.ToString("dd/MM/yyy")) : x.disponibilidad[10].disponibilidad,
                        diciembre = mes == 11 ? (x.fechaInicio.ToString("dd/MM/yyy") + " - " + x.fechaFin.ToString("dd/MM/yyy")) : x.disponibilidad[11].disponibilidad
                    };
                });
                result.Add("overhauls", overhauls);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReporteCalenEjecOverhaul(string obra, int anio)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var componentes = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().GetReporteCalenEjecOverhaul(obra, anio);
                Session["rptCalenEjecOverhaul"] = componentes;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReportePrecisionOverhaul(DateTime fechaInicio, DateTime fechaFin, int tipo = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var eventos = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().GetReportePrecisionOverhaul(fechaInicio, fechaFin, tipo).Select(x =>
                {

                    var componente = x.idComponentes != null ? JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(x.idComponentes) : new List<ComponentePlaneacionDTO>();
                    var maquina = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(x.maquinaID).FirstOrDefault();
                    var auxTipo = "";
                    switch (x.tipo)
                    {
                        case 0: auxTipo = "OVERHAUL GENERAL"; break;
                        case 1: auxTipo = "CAMBIO DE MOTOR"; break;
                        case 2: auxTipo = "COMPONENTES DESFASADOS"; break;
                        case 3: auxTipo = "FALLO"; break;
                        default: auxTipo = ""; break;
                    }
                    foreach (var item in componente)
                    {
                        item.horasCiclo = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHrsCicloActualComponente(item.componenteID, (x.fechaFin ?? default(DateTime)));
                    }
                    decimal diasReales = (decimal) Math.Floor(((x.fechaFin ?? default(DateTime)) - (x.fechaInicio ?? default(DateTime))).TotalDays);
                    return new
                    {
                        id = x.id,
                        economico = (maquina != null) ? maquina.noEconomico : "",
                        inttipo = Convert.ToString(x.tipo),
                        tipo = html(componente, auxTipo, "Tipo"),
                        lstComponentes = html(componente, auxTipo, "nombre"),
                        HorasCiclo = html(componente, auxTipo, "horasCiclo"),
                        diasDG = x.tipo == 0 ? Convert.ToString(x.diasDuracionP ?? default(decimal)) : "N/A",
                        diasReales = ((x.fechaFin ?? default(DateTime)) - (x.fechaInicio ?? default(DateTime))).TotalDays,
                        precicion = x.tipo == 0 ? (diasReales < (x.diasDuracionP ?? 0) ? "0 %" : ((Math.Abs(1 - (diasReales / (x.diasDuracionP == 0 ? 1 : (x.diasDuracionP ?? 0))))) * 100).ToString("0.##") + " %") : "N/A",
                        fecha = x.fecha,
                        tipoid = x.tipo
                    };
                }).ToList();
                eventos.OrderByDescending(y => y.fecha);
                Session["rptPrecisionOverhaul"] = eventos;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public string html(List<ComponentePlaneacionDTO> objComponente, string auxTipo, string tipo)
        {
            string htmlR = "";
            if (tipo == "Tipo")
            {
                htmlR = "     " + auxTipo;
            }
            foreach (var item in objComponente)
            {
                switch (tipo)
                {
                    case "nombre":
                        htmlR += item.nombre + "<br>";
                        break;
                    case "horasCiclo":
                        htmlR += item.horasCiclo + "<br>";
                        break;
                    case "Tipo":
                        htmlR += "<br>";
                        break;
                    default:
                        break;
                }
            }
            var auxHtmlR = htmlR.Substring(0, htmlR.Length - 4);
            htmlR = "<p align = 'center'>" + auxHtmlR + "</p>";
            return htmlR;
        }
        public ActionResult CargarTblPrecOH(DateTime fechaInicio, DateTime fechaFin, int tipo = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var eventos = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().GetReportePrecisionOverhaul(fechaInicio, fechaFin, tipo).Select(x =>
                {

                    var componente = x.idComponentes != null ? JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(x.idComponentes) : new List<ComponentePlaneacionDTO>();
                    var maquina = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(x.maquinaID).FirstOrDefault();
                    //decimal auxHorometroP = 0;
                    //decimal auxHorometro = 0;
                    //if (maquina != null)
                    //{
                    //    auxHorometroP = capturaHorometroFactoryServices.getCapturaHorometroServices().getHorometro(maquina.noEconomico, x.fecha).FirstOrDefault().Horometro;
                    //    auxHorometro = capturaHorometroFactoryServices.getCapturaHorometroServices().getHorometro(maquina.noEconomico, (x.fechaInicio ?? default(DateTime))).FirstOrDefault().Horometro;
                    //}
                    var auxTipo = "";
                    switch (x.tipo)
                    {
                        case 0: auxTipo = "OVERHAUL GENERAL"; break;
                        case 1: auxTipo = "CAMBIO DE MOTOR"; break;
                        case 2: auxTipo = "COMPONENTES DESFASADOS"; break;
                        case 3: auxTipo = "FALLO"; break;
                        default: auxTipo = ""; break;
                    }
                    foreach (var item in componente)
                    {
                        item.horasCiclo = remocionComponenteServices.getRemocionComponenteFactoryServices().GetHrsCicloActualComponente(item.componenteID, (x.fechaFin ?? default(DateTime)));
                    }
                    decimal diasReales = (decimal)Math.Floor(((x.fechaFin ?? default(DateTime)) - (x.fechaInicio ?? default(DateTime))).TotalDays);
                    return new
                    {
                        id = x.id,
                        economico = (maquina != null) ? maquina.noEconomico : "",
                        tipo = auxTipo,
                        lstComponentes = componente.Select(r => r.nombre),
                        HorasCiclo = componente.Select(r => r.horasCiclo),
                        diasDG = x.tipo == 0 ? Convert.ToString(x.diasDuracionP ?? default(decimal)) : "N/A",
                        diasReales = ((x.fechaFin ?? default(DateTime)) - (x.fechaInicio ?? default(DateTime))).TotalDays,
                        precicion = x.tipo == 0 ? (diasReales < (x.diasDuracionP ?? 0) ? "0 %" : Convert.ToString((Math.Abs(1 - (diasReales / (x.diasDuracionP == 0 ? 1 : (x.diasDuracionP ?? 0))))) * 100) + " %") : "N/A",
                        fecha = x.fecha,
                        tipoid = x.tipo
                    };
                }).ToList();               

                //var lsteventoReturn = eventos.Where(x => x.tipoid == tipo).ToList();
                eventos.OrderByDescending(y => y.fecha);
                result.Add("data", eventos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarReporteReparaciones(string subconjunto, string noComponente, string obra, string economico, string cotizacion, string comprador, List<int> proveedor, bool enProceso = true)
        {
            var result = new Dictionary<string, object>();
            if (proveedor == null) proveedor = new List<int>();
            try
            {
                var crc = administracionComponentesServices.getAdministracionComponentesFactoryServices().GetReporteConjunto(enProceso).Select(x =>
                {
                    var fechas = x.JsonFechasCRC != null ? JsonConvert.DeserializeObject<FechasTrackingComponenteCRC>(x.JsonFechasCRC) : new FechasTrackingComponenteCRC();
                    DateTime fechaT;
                    bool exito = DateTime.TryParseExact(fechas.fechaTerminacion, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaT);
                    var maquina = administracionComponentesServices.getAdministracionComponentesFactoryServices().getUltimoEconomico(x.componenteID);
                    int almacenID = 0;
                    var exitoParse = Int32.TryParse(fechas.almacen, out almacenID);
                    var almacen = exitoParse ? administracionComponentesServices.getAdministracionComponentesFactoryServices().getLocacionByID(almacenID) : "";
                    var CC = administracionComponentesServices.getAdministracionComponentesFactoryServices().getCCByEconomico(maquina);
                    return new ReparacionesPendientesDTO
                    {
                        rq = fechas.folioRequisicion,
                        oc = fechas.OC,
                        noEconomico = maquina,
                        cotizacion = fechas.claveCotizacion,
                        noComponente = x.componente.noComponente,
                        descripcion = //x.componente.subConjunto.descripcion,
                            x.componente.subConjunto.descripcion + " " + (x.componente.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)x.componente.posicionID).ToUpper() : ""),
                        almacen = almacen,
                        proveedor = x.locacion,
                        factura = fechas.folioFactura,
                        obra = CC != null ? CC.descripcion : "",
                        comprador = fechas.comprador,
                        fecha_rq = fechas.fechaRequisicion,
                        fecha_envio = fechas.fechaEnvio,
                        fecha_recepcion = fechas.fechaRecepcion,
                        fecha_terminacion = fechas.fechaTerminacion,
                        fecha = x.fecha ?? default(DateTime),
                        proveedorID = x.locacionID ?? default(int),
                        obraID = CC.areaCuenta
                    };
                }).Where(x => x.descripcion.Contains(subconjunto) && x.noComponente.Contains(noComponente) && x.noEconomico.Contains(economico)
                && (proveedor.Count() == 0 ? true : proveedor.Contains(x.proveedorID)) && (obra == "" ? true : x.obraID == obra)
                && (cotizacion == "" ? true : (x.cotizacion != null && x.cotizacion.Contains(cotizacion))) && (comprador == "" ? true : comprador == x.comprador)).OrderBy(x => x.fecha).ToList();
                result.Add("reparaciones", crc);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetReporteReparaciones(string subconjunto, string noComponente, string obra, string economico, string cotizacion, string comprador, List<int> proveedor, bool enProceso = true)
        {
            var result = new Dictionary<string, object>();
            if (proveedor == null) proveedor = new List<int>();
            CultureInfo provider = CultureInfo.InvariantCulture;
            try
            {
                var crc = administracionComponentesServices.getAdministracionComponentesFactoryServices().GetReporteConjunto(enProceso).Select(x =>
                {
                    var fechas = x.JsonFechasCRC != null ? JsonConvert.DeserializeObject<FechasTrackingComponenteCRC>(x.JsonFechasCRC) : new FechasTrackingComponenteCRC();
                    DateTime fechaT;
                    bool exito = DateTime.TryParseExact(fechas.fechaTerminacion, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaT);
                    var maquina = administracionComponentesServices.getAdministracionComponentesFactoryServices().getUltimoEconomico(x.componenteID);
                    int almacenID = 0;
                    var exitoParse = Int32.TryParse(fechas.almacen, out almacenID);
                    var almacen = exitoParse ? administracionComponentesServices.getAdministracionComponentesFactoryServices().getLocacionByID(almacenID) : "";
                    var CC = administracionComponentesServices.getAdministracionComponentesFactoryServices().getCCByEconomico(maquina);
                    return new ReparacionesPendientesDTO
                    {
                        rq = fechas.folioRequisicion,
                        oc = fechas.OC,
                        noEconomico = maquina,
                        cotizacion = fechas.claveCotizacion,
                        noComponente = x.componente.noComponente,
                        descripcion = //x.componente.subConjunto.descripcion,
                            x.componente.subConjunto.descripcion + " " + (x.componente.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)x.componente.posicionID).ToUpper() : ""),
                        almacen = almacen,
                        proveedor = x.locacion,
                        factura = fechas.folioFactura,
                        obra = CC != null ? CC.descripcion : "",
                        comprador = fechas.comprador,
                        fecha_rq = fechas.fechaRequisicion,
                        fecha_envio = fechas.fechaEnvio,
                        fecha_recepcion = fechas.fechaRecepcion,
                        fecha_terminacion = fechas.fechaTerminacion,
                        fecha = x.fecha ?? default(DateTime),
                        proveedorID = x.locacionID ?? default(int),
                        obraID = CC.areaCuenta
                    };
                }).Where(x => x.descripcion.Contains(subconjunto) && x.noComponente.Contains(noComponente) && x.noEconomico.Contains(economico)
                && (proveedor.Count() == 0 ? true : proveedor.Contains(x.proveedorID)) && (obra == "" ? true : x.obraID == obra)
                && (cotizacion == "" ? true : (x.cotizacion != null && x.cotizacion.Contains(cotizacion))) && (comprador == "" ? true : comprador == x.comprador)).OrderBy(x => x.fecha).ToList();
                Session["rptReparacionesPendientes"] = crc;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarReporteTiemposCRC(DateTime fechaInicio, DateTime fechaFin, string subconjunto, string noComponente, string obra, string economico, string cotizacion, List<int> proveedor, bool enProceso = true)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (proveedor == null) proveedor = new List<int>(); //
                var crc = administracionComponentesServices.getAdministracionComponentesFactoryServices().GetReporteConjunto(enProceso).Where(x => x.fecha >= fechaInicio && x.fecha <= fechaFin && x.componente.subConjunto.descripcion.Contains(subconjunto) && x.componente.noComponente.Contains(noComponente)
                    && (proveedor.Count() == 0 ? true : proveedor.Contains(x.locacionID ?? default(int))) ).Select(x =>
                {
                    var fechas = x.JsonFechasCRC != null && x.JsonFechasCRC != "" ? JsonConvert.DeserializeObject<FechasTrackingComponenteCRC>(x.JsonFechasCRC) : new FechasTrackingComponenteCRC();
                    var siguienteTracking = administracionComponentesServices.getAdministracionComponentesFactoryServices().GetSiguenteTracking(x.id, -1);
                    var siguienteTrackingInstalacion = administracionComponentesServices.getAdministracionComponentesFactoryServices().GetSiguenteTracking(x.id, 0);
                    var siguienteTrackingCRC = administracionComponentesServices.getAdministracionComponentesFactoryServices().GetSiguenteTracking(x.id, 2);
                    DateTime fechaEnvioCRC, fechaRecepcionCRC, fechaCotizacion, fechaAutorizacion, fechaTerminado, fechaRecoleccion;
                    var fechasSiguienteTracking = (siguienteTracking != null && siguienteTracking.JsonFechasCRC != null && siguienteTracking.JsonFechasCRC != "") ? JsonConvert.DeserializeObject<FechasTrackingComponenteCRC>(siguienteTracking.JsonFechasCRC) : new FechasTrackingComponenteCRC();
                    DateTime? fechaEntradaAlmacen = fechasSiguienteTracking.entradaAlmacen;
                    bool exito = DateTime.TryParseExact(fechas.fechaEnvio, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaEnvioCRC);
                    exito = DateTime.TryParseExact(fechas.fechaRecepcion, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaRecepcionCRC);
                    exito = DateTime.TryParseExact(fechas.fechaCotizacion, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaCotizacion);
                    exito = DateTime.TryParseExact(fechas.fechaAutorizacion, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaAutorizacion);
                    exito = DateTime.TryParseExact(fechas.fechaTerminacion, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaTerminado);
                    exito = DateTime.TryParseExact(fechas.fechaRecoleccion, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaRecoleccion);
                    var maquina = administracionComponentesServices.getAdministracionComponentesFactoryServices().getUltimoEconomico(x.componenteID);
                    var siguienteMaquina = siguienteTrackingInstalacion == null ? "N/A" : siguienteTrackingInstalacion.locacion;
                    var CC = administracionComponentesServices.getAdministracionComponentesFactoryServices().getCCByEconomico(maquina);
                    var horasActuales = siguienteTrackingCRC == null ? remocionComponenteServices.getRemocionComponenteFactoryServices().CalcularHrsCicloComponente(x.componenteID, DateTime.Today) :
                        remocionComponenteServices.getRemocionComponenteFactoryServices().CalcularHrsCicloComponente(x.componenteID, siguienteTrackingCRC.fecha ?? DateTime.Today);
                    decimal costoCotizacion = 0;
                    var horasRemocion = remocionComponenteServices.getRemocionComponenteFactoryServices().CalcularHrsCicloComponente(x.componenteID, x.fecha ?? DateTime.Today);
                    bool parseCotizacion = Decimal.TryParse(fechas.costo, out costoCotizacion);
                    return new
                    {
                        descripcion = //x.componente.subConjunto.descripcion,
                            x.componente.subConjunto.descripcion + " " + (x.componente.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)x.componente.posicionID).ToUpper() : ""),
                        noComponente = x.componente.noComponente,
                        noEconomico = maquina,
                        obra = CC != null ? CC.descripcion : "",
                        cotizacion = fechas.claveCotizacion,
                        proveedor = x.locacion,
                        fechaEnvioCRC = fechas.fechaEnvio,
                        trasladoCRC = fechas.fechaEnvio == null ? -1 : (fechas.fechaRecepcion == null ? Math.Ceiling((DateTime.Today - fechaEnvioCRC).TotalDays) : Math.Ceiling((fechaRecepcionCRC - fechaEnvioCRC).TotalDays)),
                        fechaRecepcionCRC = fechas.fechaRecepcion,
                        desarmado = fechas.fechaRecepcion == null ? -1 : (fechas.fechaCotizacion == null ? Math.Ceiling((DateTime.Today - fechaRecepcionCRC).TotalDays) : Math.Ceiling((fechaCotizacion - fechaRecepcionCRC).TotalDays)),
                        fechaCotizacion = fechas.fechaCotizacion,
                        autorizacion = fechas.fechaCotizacion == null ? -1 : (fechas.fechaAutorizacion == null ? Math.Ceiling((DateTime.Today - fechaCotizacion).TotalDays) : Math.Ceiling((fechaAutorizacion - fechaCotizacion).TotalDays)),
                        fechaAutorizacion = fechas.fechaAutorizacion,
                        armado = fechas.fechaAutorizacion == null ? -1 : (fechas.fechaTerminacion == null ? Math.Ceiling((DateTime.Today - fechaAutorizacion).TotalDays) : Math.Ceiling((fechaTerminado - fechaAutorizacion).TotalDays)),
                        fechaTerminado = fechas.fechaTerminacion,
                        recoleccion = fechas.fechaTerminacion == null ? -1 : (fechas.fechaRecoleccion == null ? Math.Ceiling((DateTime.Today - fechaTerminado).TotalDays) : Math.Ceiling((fechaRecoleccion - fechaTerminado).TotalDays)),
                        fechaRecoleccion = fechas.fechaRecoleccion,
                        trasladoAlmacen = fechas.fechaRecoleccion == null ? -1 : (fechaEntradaAlmacen == null ? Math.Ceiling((DateTime.Today - fechaRecoleccion).TotalDays) : Math.Ceiling(((fechaEntradaAlmacen ?? default(DateTime)) - fechaRecoleccion).TotalDays)),
                        fechaEntradaAlmacen = fechaEntradaAlmacen == null ? "" : (fechaEntradaAlmacen ?? default(DateTime)).ToString("dd/MM/yyyy"),
                        diasCRC = fechas.fechaRecepcion == null ? -1 : (fechas.fechaRecoleccion == null ? Math.Ceiling((DateTime.Today - fechaRecepcionCRC).TotalDays) : Math.Ceiling((fechaRecoleccion - fechaRecepcionCRC).TotalDays)),
                        diasProceso = fechas.fechaEnvio == null ? -1 : (fechaEntradaAlmacen == null ? Math.Ceiling((DateTime.Today - fechaEnvioCRC).TotalDays) : Math.Ceiling(((fechaEntradaAlmacen ?? default(DateTime)) - fechaEnvioCRC).TotalDays)),
                        diasReparacion = fechas.fechaRecepcion == null ? -1 : (fechas.fechaTerminacion == null ? Math.Ceiling((DateTime.Today - fechaRecepcionCRC).TotalDays) : Math.Ceiling((fechaTerminado - fechaRecepcionCRC).TotalDays)),
                        proveedorID = x.locacionID ?? default(int),
                        obraID = CC.areaCuenta,
                        estatus = x.estatus,
                        rq = fechas.folioRequisicion,
                        oc = fechas.OC,
                        factura = fechas.folioFactura,
                        costoCotizacion = parseCotizacion ? costoCotizacion : 0,
                        fechaInstalacion = siguienteTrackingInstalacion == null ? "N/A" : (siguienteTrackingInstalacion.fecha ?? DateTime.Today).ToString("dd/MM/yyyy"),
                        equipoInstalacion = siguienteMaquina,
                        horasSiguientes = horasActuales,
                        horasRemocion = horasRemocion,
                        fechaRemocion = (x.fecha ?? DateTime.Today).ToString("dd/MM/yyyy"),
                    };
                }).Where(x => x.noEconomico.Contains(economico) && (obra == "" ? true : x.obraID == obra) && (cotizacion == "" ? true : (x.cotizacion != null && x.cotizacion.Contains(cotizacion)))).OrderByDescending(x => x.estatus).ToList();
                result.Add("tiempos", crc);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }


        public ActionResult GetReporteTiemposCRC(string subconjunto, string noComponente, string obra, string economico, string cotizacion, List<int> proveedor, bool enProceso = true)
        {
            var result = new Dictionary<string, object>();
            if (proveedor == null) proveedor = new List<int>();
            CultureInfo provider = CultureInfo.InvariantCulture;
            try
            {
                var crc = administracionComponentesServices.getAdministracionComponentesFactoryServices().GetReporteConjunto(enProceso).Select(x =>
                {
                    var fechas = x.JsonFechasCRC != null && x.JsonFechasCRC != "" ? JsonConvert.DeserializeObject<FechasTrackingComponenteCRC>(x.JsonFechasCRC) : new FechasTrackingComponenteCRC();
                    var siguenteTracking = administracionComponentesServices.getAdministracionComponentesFactoryServices().GetSiguenteTracking(x.id, -1);
                    DateTime fechaEnvioCRC, fechaRecepcionCRC, fechaCotizacion, fechaAutorizacion, fechaTerminado, fechaRecoleccion;
                    var fechasSiguienteTracking = (siguenteTracking != null && siguenteTracking.JsonFechasCRC != null && siguenteTracking.JsonFechasCRC != "") ? JsonConvert.DeserializeObject<FechasTrackingComponenteCRC>(siguenteTracking.JsonFechasCRC) : new FechasTrackingComponenteCRC();
                    DateTime? fechaEntradaAlmacen = fechasSiguienteTracking.entradaAlmacen;
                    bool exito = DateTime.TryParseExact(fechas.fechaEnvio, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaEnvioCRC);
                    exito = DateTime.TryParseExact(fechas.fechaRecepcion, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaRecepcionCRC);
                    exito = DateTime.TryParseExact(fechas.fechaCotizacion, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaCotizacion);
                    exito = DateTime.TryParseExact(fechas.fechaAutorizacion, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaAutorizacion);
                    exito = DateTime.TryParseExact(fechas.fechaTerminacion, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaTerminado);
                    exito = DateTime.TryParseExact(fechas.fechaRecoleccion, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaRecoleccion);
                    var maquina = administracionComponentesServices.getAdministracionComponentesFactoryServices().getUltimoEconomico(x.componenteID);
                    var CC = administracionComponentesServices.getAdministracionComponentesFactoryServices().getCCByEconomico(maquina);
                    return new TiemposReparacionDTO
                    {
                        descripcion = //x.componente.subConjunto.descripcion,
                            x.componente.subConjunto.descripcion + " " + (x.componente.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)x.componente.posicionID).ToUpper() : ""),
                        noComponente = x.componente.noComponente,
                        noEconomico = maquina,
                        obra = CC != null ? CC.descripcion : "",
                        cotizacion = fechas.claveCotizacion,
                        proveedor = x.locacion,
                        fechaEnvioCRC = fechas.fechaEnvio,
                        trasladoCRC = Convert.ToInt32(fechas.fechaEnvio == null ? -1 : (fechas.fechaRecepcion == null ? Math.Ceiling((DateTime.Today - fechaEnvioCRC).TotalDays) : Math.Ceiling((fechaRecepcionCRC - fechaEnvioCRC).TotalDays))),
                        fechaRecepcionCRC = fechas.fechaRecepcion,
                        desarmado = Convert.ToInt32(fechas.fechaRecepcion == null ? -1 : (fechas.fechaCotizacion == null ? Math.Ceiling((DateTime.Today - fechaRecepcionCRC).TotalDays) : Math.Ceiling((fechaCotizacion - fechaRecepcionCRC).TotalDays))),
                        fechaCotizacion = fechas.fechaCotizacion,
                        autorizacion = fechas.fechaAutorizacion == "N/A" ? -1 : Convert.ToInt32(fechas.fechaCotizacion == null ? -1 : (fechas.fechaAutorizacion == null ? Math.Ceiling((DateTime.Today - fechaCotizacion).TotalDays) : Math.Ceiling((fechaAutorizacion - fechaCotizacion).TotalDays))),
                        fechaAutorizacion = fechas.fechaAutorizacion,
                        armado = fechas.fechaAutorizacion == "N/A" ? -1 : Convert.ToInt32(fechas.fechaAutorizacion == null ? -1 : (fechas.fechaTerminacion == null ? Math.Ceiling((DateTime.Today - fechaAutorizacion).TotalDays) : Math.Ceiling((fechaTerminado - fechaAutorizacion).TotalDays))),
                        fechaTerminado = fechas.fechaTerminacion,
                        recoleccion = Convert.ToInt32(fechas.fechaTerminacion == null ? -1 : (fechas.fechaRecoleccion == null ? Math.Ceiling((DateTime.Today - fechaTerminado).TotalDays) : Math.Ceiling((fechaRecoleccion - fechaTerminado).TotalDays))),
                        fechaRecoleccion = fechas.fechaRecoleccion,
                        trasladoAlmacen = Convert.ToInt32(fechas.fechaRecoleccion == null ? -1 : (fechaEntradaAlmacen == null ? Math.Ceiling((DateTime.Today - fechaRecoleccion).TotalDays) : Math.Ceiling(((fechaEntradaAlmacen ?? default(DateTime)) - fechaRecoleccion).TotalDays))),
                        fechaEntradaAlmacen = fechaEntradaAlmacen == null ? "" : (fechaEntradaAlmacen ?? default(DateTime)).ToString("dd/MM/yy"),
                        diasCRC = Convert.ToInt32(fechas.fechaRecepcion == null ? -1 : (fechas.fechaRecoleccion == null ? Math.Ceiling((DateTime.Today - fechaRecepcionCRC).TotalDays) : Math.Ceiling((fechaRecoleccion - fechaRecepcionCRC).TotalDays))),
                        diasProceso = Convert.ToInt32(fechas.fechaEnvio == null ? -1 : (fechaEntradaAlmacen == null ? Math.Ceiling((DateTime.Today - fechaEnvioCRC).TotalDays) : Math.Ceiling(((fechaEntradaAlmacen ?? default(DateTime)) - fechaEnvioCRC).TotalDays))),
                        diasReparacion = Convert.ToInt32(fechas.fechaRecepcion == null ? -1 : (fechas.fechaTerminacion == null ? Math.Ceiling((DateTime.Today - fechaRecepcionCRC).TotalDays) : Math.Ceiling((fechaTerminado - fechaRecepcionCRC).TotalDays))),
                        proveedorID = x.locacionID ?? default(int),
                        obraID = CC.areaCuenta,
                        estatus = x.estatus,
                        rq = fechas.folioRequisicion,
                        oc = fechas.OC,
                        factura = fechas.folioFactura
                    };
                }).Where(x => x.descripcion.Contains(subconjunto) && x.noComponente.Contains(noComponente) && x.noEconomico.Contains(economico)
                    && (proveedor.Count() == 0 ? true : proveedor.Contains(x.proveedorID)) && (obra == "" ? true : x.obraID == obra) && (cotizacion == "" ? true : (x.cotizacion != null && x.cotizacion.Contains(cotizacion)))).OrderByDescending(x => x.estatus).ToList();
                Session["rptTiemposReparacion"] = crc;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getReporteVidaUtil(DateTime fechaInicio, DateTime fechaFin, int grupo = -1, int modelo = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var vidaUtil = administracionComponentesServices.getAdministracionComponentesFactoryServices().CargarRemocionesVidaUtil(modelo, grupo, fechaInicio, fechaFin).Select(x => new
                {
                    equipo = x.equipo,
                    componente = x.componente,
                    ordenCRC = x.ordenCRC,
                    costo = x.costoPromedio,
                    fecha = x.fecha,
                    serie = x.noComponente,
                    vida = x.vida,
                    horometro = x.horometro,
                    horAcumulado = x.horasAcumuladas,
                    motivo = x.motivo,
                });
                Session["rptVidaUtil"] = vidaUtil;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarReportesDesecho(DateTime fechaInicio, DateTime fechaFinal, string noComponente, int subconjunto = -1, int conjunto = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                //estatus, descripcionComponente, noEconomico, motivoRemocion, fechaInicio, fechaFinal, areasCuentaStr, modelos, noComponente
                var reportes = remocionComponenteServices.getRemocionComponenteFactoryServices().cargarReportes(6, "", "", 5, fechaInicio, fechaFinal, new List<string>(), null, "")
                .Select(x => new
                {
                    id = x.id,
                    subConjunto = //x.componenteRemovido.subConjunto.descripcion,
                        x.componenteRemovido.subConjunto.descripcion + " " + (x.componenteRemovido.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)x.componenteRemovido.posicionID).ToUpper() : ""),
                    noComponente = x.componenteRemovido.noComponente,
                    componenteID = x.componenteRemovidoID,
                    fecha = x.fechaRemocion.ToString("dd/MM/yyy"),
                });
                result.Add("reportes", reportes);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReporteDesecho(int idReporteDesecho)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var reporte = remocionComponenteServices.getRemocionComponenteFactoryServices().getReporteRemocionByID(idReporteDesecho);
                reporteDesechoDTO aux = new reporteDesechoDTO();
                var usuario = usuarioFactoryServices.getUsuarioService().getPassByID(reporte.realiza);
                aux.idReporte = reporte.id;
                aux.fecha = reporte.fechaRemocion.ToString("dd/MM/yyyy");
                aux.horasComponente = reporte.horasComponente;
                aux.horasAcumuladas = reporte.componenteRemovido.horasAcumuladas;
                aux.horasMaquina = reporte.horasMaquina;
                aux.modelo = reporte.maquina == null ? "" : reporte.maquina.modeloEquipo.descripcion;
                aux.motivo = reporte.comentario;
                aux.noEconomico = reporte.maquina == null ? "" : reporte.maquina.noEconomico;
                aux.numParte = reporte.componenteRemovido.numParte == null ? "" : reporte.componenteRemovido.numParte;
                aux.realizo = usuario != null ? usuario.nombre + " " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno : "";
                aux.realizoID = reporte.realiza;
                aux.rutaFotoSerie = reporte.imgComponenteRemovido;
                aux.serieComponente = reporte.componenteRemovido.noComponente;
                aux.serieMaquina = reporte.maquina == null ? "" : reporte.maquina.noSerie;
                aux.subconjunto = //reporte.componenteRemovido.subConjunto.descripcion;
                     reporte.componenteRemovido.subConjunto.descripcion + " " + (reporte.componenteRemovido.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)reporte.componenteRemovido.posicionID).ToUpper() : "");
                aux.evidencia = reporte.JsonEvidencia;
                Session["reporteDesecho"] = aux;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarReporteDesecho(int idReporteDesecho)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = remocionComponenteServices.getRemocionComponenteFactoryServices().EliminarReporteRemocionByID(idReporteDesecho);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //public ActionResult CargarTblPrecOH(DateTime fechaInicio, DateTime fechaFin)
        //{
        //    var result = new Dictionary<string, object>();
        //    try
        //    {
        //        var eventos = planeacionOverhaulFactoryServices.getPlaneacionOverhaulFactoryServices().GetReportePrecisionOverhaul(fechaInicio, fechaFin).ToList();
        //        eventos.OrderByDescending(y => y.fecha);
        //        result.Add("data", eventos);
        //        result.Add(SUCCESS, true);
        //    }
        //    catch (Exception e)
        //    {
        //        result.Add(MESSAGE, e.Message);
        //        result.Add(SUCCESS, false);
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}



        public ActionResult CargarReporteAvanceGeneral(List<string> obras, int estatus, int anio = -1, int modeloID = 0)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var presupuestosAvance = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().CargarTblAvance(obras, modeloID, anio, estatus);
                var presupuestosAvanceGeneral = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().CargarAvanceGeneral(obras, modeloID, anio, estatus);

                Session["reporteAvancePresupuesto"] = presupuestosAvance;
                Session["reporteAvancePresupuestoGeneral"] = presupuestosAvanceGeneral;
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

        #region Historial

        public ActionResult CargarTablaHistorial(string componente, string locacion, DateTime fechaInicio, DateTime fechaFin, int grupo = -1, int subconjunto = -1, int modelo = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var historial = administracionComponentesServices.getAdministracionComponentesFactoryServices().CargarTablaHistorial(componente.Trim(), subconjunto, locacion.Trim(), fechaInicio, fechaFin, grupo, modelo).Select(x =>
                {
                    var JsonFechas = x.JsonFechasCRC != null ? JsonConvert.DeserializeObject<FechasTrackingComponenteCRC>(x.JsonFechasCRC) : null;
                    var vidaActual = administracionComponentesServices.getAdministracionComponentesFactoryServices().GetVidasComponenteTracking(x.id);
                    return new
                    {
                        id = x.id,
                        componente = x.componente.noComponente,
                        componenteID = x.componenteID,
                        locacion = x.locacion,
                        fecha = (x.fecha ?? default(DateTime)).ToString("dd/MM/yyyy"),
                        fechaRaw = x.fecha ?? default(DateTime),
                        vida = vidaActual + x.componente.vidaInicio,
                        cotizacion = JsonFechas != null ? (JsonFechas.claveCotizacion != null ? JsonFechas.claveCotizacion : "") : "",
                        costo = x.costoCRC,
                        parcial = JsonFechas != null ? (JsonFechas.parcial != null ? JsonFechas.parcial : "") : "",
                        requisicion = JsonFechas != null ? (JsonFechas.folioRequisicion != null ? JsonFechas.folioRequisicion : "") : "",
                        oc = JsonFechas != null ? (JsonFechas.OC != null ? JsonFechas.OC : "") : "",
                        factura = JsonFechas != null ? (JsonFechas.folioFactura != null ? JsonFechas.folioFactura : "") : "",
                        tipo = x.tipoLocacion
                    };
                });
                result.Add("historial", historial);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarTracking(int idTracking, string cotizacion, decimal costo, bool parcial, string requisicion, string oc, string factura)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var tracking = administracionComponentesServices.getAdministracionComponentesFactoryServices().getTrackingByID(idTracking);
                if (tracking != null)
                {
                    FechasTrackingComponenteCRC jsonInfo = new FechasTrackingComponenteCRC();
                    if (tracking.JsonFechasCRC != null)
                        jsonInfo = JsonConvert.DeserializeObject<FechasTrackingComponenteCRC>(tracking.JsonFechasCRC);
                    jsonInfo.claveCotizacion = cotizacion;
                    jsonInfo.costo = costo.ToString();
                    jsonInfo.parcial = parcial.ToString().ToLower();
                    jsonInfo.folioRequisicion = requisicion;
                    jsonInfo.OC = oc;
                    jsonInfo.folioFactura = factura;
                    tracking.JsonFechasCRC = JsonConvert.SerializeObject(jsonInfo);
                    tracking.costoCRC = costo;
                    administracionComponentesServices.getAdministracionComponentesFactoryServices().Guardar(tracking);
                    result.Add("exito", true);
                }
                else { result.Add("exito", false); }
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cargarDatosRemocionHistorial(int idComponente, int trackID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var remocion = administracionComponentesServices.getAdministracionComponentesFactoryServices().cargarDatosRemocionHistorial(idComponente, trackID);
                if (remocion.componenteRemovidoID == -1)
                {
                    result.Add(MESSAGE, "No se encontró el Económico");
                    result.Add(SUCCESS, false);
                }
                else
                {
                    result.Add("remocion", remocion);
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

        #endregion

        #region Utils

        public ActionResult GetComponenteByID(int index, int tipo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (tipo == 0)
                {
                    var componente = componenteServices.getComponenteService().getComponenteByID(index);
                    result.Add("componente", componente);
                }
                else
                {
                    var servicio = administracionServiciosFactoryServices.getAdministracionServiciosFactoryServices().GetServicioByID(index);
                    var componente = new
                    {
                        horaCicloActual = servicio.horasCicloActual,
                        cicloVidaHoras = servicio.cicloVidaHoras
                    };
                    result.Add("componente", componente);
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

        [HttpGet]
        public ActionResult getInsumo(string term, bool porDesc)
        {
            return Json(notaCreditoFactoryServices.getNotaCredito().getInsumo(term, porDesc), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getComboAlamcen()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var items = notaCreditoFactoryServices.getNotaCredito().getComboAlamcen();
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


        public ActionResult cargarComponentesAPresupuesto()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().cargarComponentesAPresupuesto();
                result.Add("data", data);
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

        #region Nuevo Presupuesto

        public ActionResult CargarPresupuestoPorComponente(int anio, int modelo = 0, int subconjunto = 0, string noComponente = "")
        {
            var result = new Dictionary<string, object>();
            try
            {
                var detallePres = presupuestoOverhaulFactoryServices.getPresupuestoOverhaulFactoryServices().CargarPresupuestoPorComponente(anio, modelo, subconjunto, noComponente).ToList();
                result.Add("data", detallePres);
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

    }
}