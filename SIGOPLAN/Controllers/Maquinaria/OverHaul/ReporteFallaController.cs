﻿using Core.Entity.Maquinaria.Overhaul;
using Data.Factory.Maquinaria.Captura;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Principal.Alertas;
using Data.Factory.Maquinaria.Overhaul;
using Data.Factory.Administracion.ControlInterno.Almacen;
using Data.Factory.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.IO;
using Core.DTO.Maquinaria.Catalogos;
using Infrastructure.Utils;
using Core.DTO.Maquinaria.Overhaul;
using System.Globalization;
using Data.Factory.Principal.Archivos;
using Core.Enum.Maquinaria.Overhaul;
using Core.DAO.Maquinaria.Catalogos;
using Core.DAO.Principal.Alertas;
using Core.DAO.Maquinaria.Captura;
using Core.DAO.Maquinaria.Overhaul;
using Core.DAO.Administracion.ControlInterno.Almacen;
using Core.DAO.Principal.Usuarios;
using Core.DAO.Principal.Archivos;
using Core.Entity.Maquinaria.Catalogo;
using System.Drawing;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Newtonsoft.Json.Converters;
using Core.DTO.Maquinaria.Mantenimiento.Correctivo.ReporteFalla;

namespace SIGOPLAN.Controllers.Maquinaria.OverHaul
{
    public class ReporteFallaController : BaseController
    {
        IMaquinaDAO maquinaFS;
        ICapturaHorometroDAO horometroFs;
        ICentroCostosDAO centroCostosFS;
        IReporteFallaOverhaulDAO rptFallaOverhaulFS;
        IRemocionComponenteDAO remocionComponenteFS;
        IinsumosDAO insumoFs;
        IAdministracionComponentesDAO adminComponentesFs;
        IComponenteDAO componenteFS;
        IUsuarioDAO usuarioFS;
        IDirArchivosDAO archivofs;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            maquinaFS = new MaquinaFactoryServices().getMaquinaServices();
            horometroFs = new CapturaHorometroFactoryServices().getCapturaHorometroServices();
            centroCostosFS = new CentroCostosFactoryServices().getCentroCostosService();
            rptFallaOverhaulFS = new ReporteFallaOverhaulFactoryServices().getReporteFallaOverhaulFactoryServices();
            remocionComponenteFS = new RemocionComponenteFactoryServices().getRemocionComponenteFactoryServices();
            insumoFs = new InsumoFactoryServices().getRepTraspasoServices();
            adminComponentesFs = new AdministracionComponentesFactoryServices().getAdministracionComponentesFactoryServices();
            componenteFS = new ComponenteFactoryServices().getComponenteService();
            usuarioFS = new UsuarioFactoryServices().getUsuarioService();
            archivofs = new ArchivoFactoryServices().getArchivo();
            base.OnActionExecuting(filterContext);
        }

        public ActionResult AdministradorReporteFalla()
        {
            return View();
        }

        public ActionResult fillCboEconomicos(string obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, maquinaFS.fillCboNoEconomicos(obj));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarReporteFallaComponente()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var json = Request.RequestContext.HttpContext.Request.Params["reportes"];

                var format = "dd/MM/yyyy"; // your datetime format
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };

                var obj = JsonConvert.DeserializeObject<tblM_ReporteFalla_Componente>(json, dateTimeConverter);
                // var obj = JsonUtils.convertJsonToNetObject<tblM_ReporteFalla_Componente>(Request.Form["reportes"],dateTimeConverter);
                obj.Reporte = preparaParaGuardarReporteFalla(obj.Reporte);
                rptFallaOverhaulFS.EliminarReparacionDesdeComponente(obj);
                var esGuardado = rptFallaOverhaulFS.Guardar(obj);
                result.Add(SUCCESS, esGuardado);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarReporteFallaReparacion()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = JsonUtils.convertJsonToNetObject<tblM_ReporteFalla_Reparacion>(Request.Form["reportes"]);
                obj.Reporte = preparaParaGuardarReporteFalla(obj.Reporte);
                rptFallaOverhaulFS.EliminarComponenteDesdeReparacion(obj);
                var esGuardado = rptFallaOverhaulFS.Guardar(obj);
                result.Add(SUCCESS, esGuardado);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDataEconomico(int obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var Maquinaria = maquinaFS.GetMaquinaByID(obj).FirstOrDefault();
                if (Maquinaria != null)
                {
                    var ultimoHorometro = horometroFs.getUltimoHorometro(Maquinaria.noEconomico);

                    var asignacion = rptFallaOverhaulFS.getProcedenciaMaquina(Maquinaria.id);
                    var procedencia = rptFallaOverhaulFS.getCCNameByAC(asignacion.CCOrigen);

                    if (ultimoHorometro != null)
                    {
                        result.Add("Horometro", ultimoHorometro.Horometro);
                    }
                    var centro_costos_nombre = centroCostosFS.getNombreCCFix(Maquinaria.centro_costos);
                    result.Add("CCname", centro_costos_nombre);
                    result.Add("Descripcion", Maquinaria.grupoMaquinaria.descripcion);
                    result.Add("Marca", Maquinaria.marca.descripcion);
                    result.Add("Modelo", Maquinaria.modeloEquipo.descripcion);
                    result.Add("Serie", Maquinaria.noSerie);
                    result.Add("id", Maquinaria.id);
                    result.Add("modeloID", Maquinaria.modeloEquipoID);
                    result.Add("procedencia", procedencia);
                    result.Add("fechaAlta", asignacion.fechaInicio.ToString("dd/MM/yyy"));
                    result.Add("horometroAlta", asignacion.Horas);
                    result.Add(SUCCESS, true);
                }
                else
                {
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

        public ActionResult CargarReportesFalla(int estatus, string noEconomico, int cc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var maquinas = maquinaFS.GetAllMaquinas().Where(x => noEconomico == "-1" ? true : x.noEconomico.Contains(noEconomico)).ToList();
                var maquinasID = maquinas.Select(x => x.id).ToList();
                var reportes = rptFallaOverhaulFS.cargarReportes(estatus, cc)
                    .Where(x => (noEconomico == "-1" ? true : maquinasID.Contains(x.maquinaID)))
                .Select(x =>
                {
                    var maquina = maquinas.FirstOrDefault(y => y.id == x.maquinaID);
                    return new
                        {
                            id = x.id,
                            noEconomico = "<b>" + (maquina == null ? " " : maquina.noEconomico) + "</b>",
                            cc = remocionComponenteFS.getCC(x.cc),
                            fechaParo = x.fechaParo,
                            fechaReporte = x.fechaReporte,
                            falla = (x.fallaComponente == 0 ? "INSUMO" : "COMPONENTE"),
                            componenteInsumo = x.componenteInsumo,
                            estatus = x.estatus,
                            maquinaID = x.maquinaID,
                            modeloID = (maquina == null ? 0 : maquina.modeloEquipoID)
                        };
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
        public ActionResult EliminarReporteFalla(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                rptFallaOverhaulFS.Eliminar(id);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult aprobarReporteFalla(int idReporte)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var componenteID = rptFallaOverhaulFS.getIdComponenteFromIdReporteFalla(idReporte);
                var esInsumo = rptFallaOverhaulFS.getReporte(idReporte).fallaComponente;

                var esComponente = componenteID > 0;
                if (esInsumo == 1)
                {
                    var componente = componenteFS.getComponenteByID(componenteID);
                    componente.falla = true;
                    componenteFS.Guardar(componente);
                    var tracking = adminComponentesFs.getTrackingByComponente(componenteID);
                    tracking.estatus = 12;
                    adminComponentesFs.Guardar(tracking);
                }
                var reporte = rptFallaOverhaulFS.getReporteByID(idReporte);
                reporte.estatus = 2;
                rptFallaOverhaulFS.Guardar(reporte);

                result.Add(SUCCESS, esComponente);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReporteFalla(int idReporte)
        {
            var result = new Dictionary<string, object>();
            try
            {
                Session["rptReporteFalla"] = null;
                
                var reporte = rptFallaOverhaulFS.getReporte(idReporte);
                var fechaParo = new DateTime();
                var boolFechaParo = DateTime.TryParse(reporte.fechaParo, out fechaParo);

                if (reporte.fallaComponente == 1)
                {
                    var componente = rptFallaOverhaulFS.getRptComoponente(idReporte);
                    if(boolFechaParo) componente.Horas = remocionComponenteFS.GetHrsCicloActualComponente(componente.Componente, fechaParo);
                    Session["rptReporteFalla"] = GetReporteFallaDoc(componente);
                    //componente.Reporte.lstArchivos = new List<tblM_ReporteFalla_Archivo>();
                    result.Add("componente", componente);

                    var Maquinaria = maquinaFS.GetMaquinaByID(reporte.maquinaID).FirstOrDefault();
                    result.Add("maquina", Maquinaria.modeloEquipoID);
                }
                else
                {
                    var reparacion = rptFallaOverhaulFS.getRptReparacion(idReporte);
                    Session["rptReporteFalla"] = GetReporteFallaDoc(reparacion);
                    //reparacion.Reporte.lstArchivos = new List<tblM_ReporteFalla_Archivo>();
                    result.Add("reparacion", reparacion);
                }
                var archivosEvidencia = reporte.lstArchivos.Select(s => new
                {
                    id = s.id,
                    tipo = s.tipo,
                    FechaCreacion = s.fechaRegistro.ToShortDateString(),
                    nombre = s.nombre,
                    ruta = s.ruta,
                    imagen = getBase64FromNombreRuta(s.ruta, s.nombre)
                }).ToList();
                reporte.lstArchivos = new List<tblM_ReporteFalla_Archivo>();
                //reporte.maquina = new tblM_CatMaquina();


                result.Add("archivosEvidencia", archivosEvidencia);
                result.Add("reporte", reporte);
                result.Add("fechaAlta", (reporte.fechaAlta));
                result.Add("fechaParo", reporte.fechaParo);
                result.Add("fechaReporte", reporte.fechaReporte);
                result.Add(SUCCESS, reporte.id > 0);
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
        public ActionResult DescargarArchivoEvidencia(string ruta, string nombreArchivo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("base64", getBase64FromNombreRuta(ruta, nombreArchivo));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboComponentes(int idMaquina, int idSubconjunto)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var items = rptFallaOverhaulFS.fillCboComponentes(idMaquina, idSubconjunto)
                .Select(x => new
                {
                    Value = x.componenteID,
                    Text = x.componente.noComponente
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

        public ActionResult CargarDatosComponente(int idComponente)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var componente = rptFallaOverhaulFS.getComponente(idComponente);
                var tracking = rptFallaOverhaulFS.getTrackingActualComponente(idComponente);

                result.Add("fechaInstalacion", (tracking.fecha ?? default(DateTime)).ToString("dd/MM/yyy"));
                result.Add("horasUso", componente.horaCicloActual);
                result.Add("numParte", componente.numParte);
                result.Add("CCInicialID", componente.centroCostos);
                result.Add("CCInicial", rptFallaOverhaulFS.getCCNameByID(componente.centroCostos ?? default(int)));
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
            System.IO.File.WriteAllBytes(ruta, data);
            return System.IO.File.Exists(ruta);
        }



        public ActionResult FillCboVistoBueno(string centroCostos)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var items = rptFallaOverhaulFS.fillVistoBueno(centroCostos);
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

        public ActionResult FillCboRevisa(string centroCostos)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var items = rptFallaOverhaulFS.fillCboRevisa(centroCostos);
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
        public ActionResult FillCboRptFallaTipoArchivo()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var items = EnumExtensions.ToCombo<rptFallaTipoArchivoEnum>();
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
        tblM_ReporteFalla preparaParaGuardarReporteFalla(tblM_ReporteFalla reporte)
        {
            var i = 0;
            byte[] data;
            var fecha = DateTime.Now;
            var maxFileSize = 1024 * 20000;
            var files = Request.Files.GetMultiple("files[]").ToList();
            var f = fecha.ToString("ddMMyyyy") + fecha.Hour + fecha.Minute;
            var bd = rptFallaOverhaulFS.getReporteByID(reporte.id);
            var lstFormat = new List<string>() { "pdf", "image" };
            
            reporte.lstArchivos.ToList().ForEach(archivo =>
            {
                var file = files[i++];
                var FileName = file.FileName;
#if DEBUG
                //Local
                //var ruta = @"C:\Proyectos\" + f + FileName;
                var ruta = @"C:\Proyectos\SIGOPLANv2\ImgLocalHost\" + f + FileName;
#else
                //Productivo
                var ruta = archivofs.getUrlDelServidor(12) + f + FileName;
#endif

                if (file.ContentLength <= 0)
                    throw new Exception(string.Format("{0} está vacio", FileName));
                if (file.ContentLength > maxFileSize)
                    throw new Exception(string.Format("{0} es muy grande", FileName));
                if (lstFormat.Any(format => format.Contains(file.ContentType)))
                    throw new Exception(string.Format("{0} no tiene el formato correcto", FileName));
                using (Stream inputStream = file.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    data = memoryStream.ToArray();
                    if (lstFormat.Any(formato => formato.Contains(file.ContentType)))
                    {
                        data = GlobalUtils.FixedSize(data, 500, 500);
                    }
                }
                System.IO.File.WriteAllBytes(ruta, data);
                if (System.IO.File.Exists(ruta))
                {
                    archivo.rptFalla = reporte;
                    archivo.nombre = FileName;
                    archivo.ruta = ruta;
                    archivo.fechaRegistro = fecha;
                    archivo.esActivo = true;

                }
            });
            reporte.realiza = getUsuario().id;
            reporte.fechaAlta = fecha.ToString("dd/MMMM/yyyy");
            return reporte;
        }
        rptReporteFallaDTO GetReporteFallaDoc(tblM_ReporteFalla_Componente reporte)
        {
            var dto = GetReporteFallaDoc(reporte.Reporte);
            var componente = componenteFS.getComponenteByID(reporte.Componente);
            dto.fechaComp = reporte.Fecha;
            dto.horometroComp = reporte.Horas;
            dto.numParteComp = reporte.Parte;
            dto.conjunto = componente.conjunto.descripcion;
            dto.subconjunto = componente.subConjunto.descripcion;
            dto.horometroMaq = reporte.Reporte.horometroReporte;
            return dto;
        }
        rptReporteFallaDTO GetReporteFallaDoc(tblM_ReporteFalla_Reparacion reporte)
        {
            var dto = GetReporteFallaDoc(reporte.Reporte);
            var insumo = insumoFs.getInsumo(reporte.Insumo, dto.fechaParo.Year, 2).FirstOrDefault();
            var insumoTipoGrupo = insumoFs.getInsumoTipoGrupoByID(reporte.Insumo);
            dto.conjunto = insumoTipoGrupo.Text;
            dto.subconjunto = insumoTipoGrupo.Value;
            if((dto.destinoCargo == null || dto.destinoCargo == "") && insumo != null) dto.destinoCargo = insumo.desInsumoArrendadora == null ? insumo.desInsumoConstruplan : insumo.desInsumoArrendadora;
            dto.horometroMaq = reporte.Reporte.horometroReporte;
            return dto;
        }
        rptReporteFallaDTO GetReporteFallaDoc(tblM_ReporteFalla reporte)
        {
            var maquina = maquinaFS.GetMaquinaByID(reporte.maquinaID).FirstOrDefault();
            var dto = new rptReporteFallaDTO();
            var provider = CultureInfo.InvariantCulture;
            var usuario = usuarioFS.getPassByID(reporte.realiza);
            dto.idReporte = reporte.id;
            dto.fallaComponente = reporte.fallaComponente;
            dto.obra = reporte.cc;
            dto.fecha = DateTime.ParseExact(reporte.fechaReporte, "dd/MM/yyyy", provider);
            dto.fechaParo = DateTime.ParseExact(reporte.fechaParo, "dd/MM/yyyy", provider);
            dto.noEconomico = maquina.noEconomico;
            dto.descripcionMaq = maquina.descripcion;
            dto.modeloMaq = maquina.modeloEquipo.descripcion;
            dto.noSerieMaq = maquina.noSerie;
            dto.descripcionFalla = reporte.descripcionFalla;
            dto.causa = reporte.causaFalla;
            dto.obraMaq = adminComponentesFs.getDescripcionCC(maquina.EconomicoCC);
            dto.horometroMaq = reporte.horometroReporte;
            dto.marcaMaq = maquina.marca.descripcion;
            dto.destinoCargo = ((EmpresaEnum)reporte.destino.ParseInt()).GetDescription();
            dto.realiza = usuario != null ? string.Format("{0} {1} {2}", usuario.nombre, usuario.apellidoPaterno, usuario.apellidoMaterno) : "";
            dto.realizaFirma = GlobalUtils.CrearFirmaDigital(reporte.id, DocumentosEnum.Reporte_Falla, reporte.realiza);
            dto.frente = "MINADO";
            dto.revisa = " ";
            dto.revisaFirma = " ";
            dto.diagnosticosAplicados = reporte.diagnosticosAplicados;
            dto.tipoReparacion = reporte.tipoReparacion;
            return dto;
        }
        string getBase64FromNombreRuta(string ruta, string nombreArchivo)
        {
            try
            {
                var archivo = System.IO.File.ReadAllBytes(ruta);
                archivo = GlobalUtils.FixedSize(archivo, 672, 785);
                var base64 = Convert.ToBase64String(archivo);
                return "data:" + MimeMapping.GetMimeMapping(nombreArchivo) + ";base64," + base64;
            }
            catch(Exception e){
                return "";
            }
        }

        public ActionResult GetArchivosReporteFalla(int _idReporteFalla)
        {
            return Json(rptFallaOverhaulFS.GetArchivosReporteFalla(_idReporteFalla), JsonRequestBehavior.AllowGet);            
        }

        [HttpPost]
        public ActionResult DescargarArchivoReporteFalla()
        {
            var filtro = JsonUtils.convertJsonToNetObject<int>(Request.Form["filtro"], "es-MX");
            var resultadoTupla = rptFallaOverhaulFS.DescargarArchivoReporteFalla(filtro);

            if (resultadoTupla != null)
            {
                string nombreArchivo = resultadoTupla.Item2;
                string tipo = MimeMapping.GetMimeMapping(nombreArchivo);

                var fileStreamResult = new FileStreamResult(resultadoTupla.Item1, tipo);
                fileStreamResult.FileDownloadName = nombreArchivo;

                return fileStreamResult;
            }
            else
            {
                return View(RUTA_VISTA_ERROR_DESCARGA);
            }
        }
    }
}