using Core.DTO;
using Core.DTO.Contabilidad;
using Core.DTO.Encuestas;
using Core.DTO.Principal.Usuarios;
using Core.DTO.Utils.Excel;
using Core.Entity.Encuestas;
using Core.Entity.Principal.Alertas;
using Core.Enum.Principal.Alertas;
using Data.DAO.Principal.Usuarios;
using Data.Factory.Encuestas;
using Data.Factory.Principal.Alertas;
using Data.Factory.Principal.Archivos;
using Data.Factory.Principal.Usuarios;
using Infrastructure.DTO;
using Infrastructure.Utils;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Encuestas.Controllers
{
    public class EncuestaController : BaseController
    {
        private EncuestasFactoryServices encuestasFactoryServices;
        private AlertaFactoryServices alertaFactoryServices;
        UsuarioFactoryServices usuarioFactoryServices = new UsuarioFactoryServices();
        ArchivoFactoryServices archivofs;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            encuestasFactoryServices = new EncuestasFactoryServices();
            alertaFactoryServices = new AlertaFactoryServices();
            archivofs = new ArchivoFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult Crear()
        {
            return View();
        }
        public ActionResult Responder()
        {
            return View();
        }
        public ActionResult Reportes()
        {
            return View();
        }
        public ActionResult Gestion()
        {
            return View();
        }
        public ActionResult GestionTelefonica()
        {
            return View();
        }
        public ActionResult _tarjetaEstrella()
        {
            return PartialView();
        }

        public ActionResult GestionUsuarios()
        {
            return View();
        }
        public ActionResult UsuarioPorEncuesta()
        {
            return View();
        }
        public ActionResult Asignacion()
        {
            return View();
        }
        public ActionResult getPermisos(int idUsuario)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var encuestas = encuestasFactoryServices.getEncuestasService().getEncuestas();
                var permisosUsuario = encuestasFactoryServices.getEncuestasService().getUsuarioPermisosCheck(idUsuario);

                var lstMerge = encuestas.Select(x => new EncuestaCheckUsuarioDTO
                {
                    encuestaID = x.id,
                    usuarioID = idUsuario,
                    titulo = x.titulo,
                    descripcion = x.descripcion,
                    departamentoID = x.departamentoID,
                    departamentoDesc = x.departamento,
                    soloLectura = x.soloLectura,
                    encuestaEditar = x.editar ?? false,
                    encuestaEnviar = x.enviar ?? false,
                    encuestaTelefonica = x.telefonica ?? false,
                    encuestaNotificacion = x.notificacion ?? false,
                    encuestaPapel = x.papel ?? false,
                    consultar = permisosUsuario.FirstOrDefault(y => y.encuestaID == x.id) != null ? permisosUsuario.FirstOrDefault(y => y.encuestaID == x.id).consultar : false,
                    editar = permisosUsuario.FirstOrDefault(y => y.encuestaID == x.id) != null ? permisosUsuario.FirstOrDefault(y => y.encuestaID == x.id).editar : false,
                    enviar = permisosUsuario.FirstOrDefault(y => y.encuestaID == x.id) != null ? permisosUsuario.FirstOrDefault(y => y.encuestaID == x.id).enviar : false,
                    contestaTelefonica = permisosUsuario.FirstOrDefault(y => y.encuestaID == x.id) != null ? permisosUsuario.FirstOrDefault(y => y.encuestaID == x.id).contestaTelefonica : false,
                    recibeNotificacion = permisosUsuario.FirstOrDefault(y => y.encuestaID == x.id) != null ? permisosUsuario.FirstOrDefault(y => y.encuestaID == x.id).recibeNotificacion : false,
                    contestaPapel = permisosUsuario.FirstOrDefault(y => y.encuestaID == x.id) != null ? permisosUsuario.FirstOrDefault(y => y.encuestaID == x.id).contestaPapel : false,
                    crear = permisosUsuario.FirstOrDefault(y => y.encuestaID == x.id) != null ? permisosUsuario.FirstOrDefault(y => y.encuestaID == x.id).crear : false
                }).ToList();

                result.Add("dataSet", lstMerge);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarPermisosCheck(List<EncuestaCheckUsuarioDTO> lstPermisos)
        {
            var result = new Dictionary<string, object>();

            try
            {
                encuestasFactoryServices.getEncuestasService().guardarPermisosCheck(lstPermisos);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckPermisosUsuario(int encuestaID)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var usuarioID = vSesiones.sesionUsuarioDTO.id;

                var permisosUsuario = encuestasFactoryServices.getEncuestasService().getUsuarioPermisosCheckPorEncuesta(usuarioID, encuestaID);

                result.Add("permisos", permisosUsuario);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getRptCerteza2(List<string> listaEncuestas, DateTime fechaInicio, DateTime fechaFinal)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<int> data2 = new List<int>();

                foreach (var item in listaEncuestas)
                {
                    data2.Add(Convert.ToInt32(item));
                }


                Session["rptCerteza"] = encuestasFactoryServices.getEncuestasService().exportData(data2, fechaInicio, fechaFinal);



                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult fillCboClienteInterno()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, ((List<usuariosEncuestasDTO>)Session["ClientesInternosCbo"]).Select(x => new ComboDTO
                {
                    Text = x.Nombre,
                    Value = x.id
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

        public ActionResult getUsuariosEncuestas(int tipoCliente)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var usuarios = usuarioFactoryServices.getUsuarioService().getUsuariosData(tipoCliente);
                Session["ClientesInternosCbo"] = usuarios.Where(x => x.Cliente == "CLIENTE INTERNO").ToList();


                result.Add("dataSet", usuarios);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cboDepartamentos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, encuestasFactoryServices.getEncuestasService().getDepartamentos().Select(x => new { Value = x.id, Text = x.descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult saveEncuesta(tblEN_Encuesta obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                obj.creadorID = vSesiones.sesionUsuarioDTO.id;
                obj.fecha = DateTime.Now;
                obj.estatus = true;
                obj.estatusAutoriza = (int)estatusEnum.PENDIENTE;

                //    obj.departamentoID = vSesiones.sesionUsuarioDTO.departamento.id;

                var id = encuestasFactoryServices.getEncuestasService().saveEncuesta(obj);
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
        public ActionResult getEncuesta(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = encuestasFactoryServices.getEncuestasService().getEncuesta(id);
                result.Add("obj", obj);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult setDepartamentoUsuario()
        {
            var result = new Dictionary<string, object>();
            try
            {

                result.Add("permisoAdmin", base.getAction("crearTiposEncuestas"));
                result.Add("departamentoId", vSesiones.sesionUsuarioDTO.departamento.id);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getEncuestaByID(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = encuestasFactoryServices.getEncuestasService().getEncuestaByID(id);
                result.Add("obj", obj);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult saveEncuestaResult(List<tblEN_Resultado> obj, int encuestaID, int encuestaUsuarioID, string comentario)
        {
            var result = new Dictionary<string, object>();

            try
            {
                string mensajeError = verificarRespuesta(obj);

                if (mensajeError != null)
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, mensajeError);
                    return Json(result);
                }

                foreach (var i in obj)
                {
                    i.usuarioRespondioID = vSesiones.sesionUsuarioDTO.id;
                    i.fecha = DateTime.Now;
                }
                encuestasFactoryServices.getEncuestasService().saveEncuestaResult(obj, encuestaUsuarioID, comentario);

                var correo = new SendEncuestaDTO
                {
                    encuestaID = encuestaUsuarioID,
                    encuestadoID = vSesiones.sesionUsuarioDTO.id
                };

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getEncuestaResult(int id)
        {
            var result = new Dictionary<string, object>();
            //try
            //{

            var obj = encuestasFactoryServices.getEncuestasService().getEncuestaResult(id);
            result.Add("obj", obj);
            result.Add(SUCCESS, true);
            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult setAceptarEncuesta(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {

                encuestasFactoryServices.getEncuestasService().setAceptarEncuesta(id);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult setRechazarEncuesta(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {

                encuestasFactoryServices.getEncuestasService().setRechazarEncuesta(id);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult setAceptarEncuestaUpdate(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {

                encuestasFactoryServices.getEncuestasService().setAceptarEncuestaUpdate(id);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult setRechazarEncuestaUpdate(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {

                encuestasFactoryServices.getEncuestasService().setRechazarEncuestaUpdate(id);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getEncuestaAsignaUsuario()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = encuestasFactoryServices.getEncuestasService().getEncuestaAsignaUsuario();
                var esSuccess = lst.Count > 0;
                if (esSuccess)
                {
                    result.Add("lst", lst);
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
        public ActionResult GuardarEncAsigUsuario(List<tblEN_EncuestaAsignaUsuario> lst)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esSuccess = lst.Count > 0 && lst.All(a => a.usuarioID > 0 && a.encuestaID > 0);
                if (esSuccess)
                {
                    esSuccess = encuestasFactoryServices.getEncuestasService().GuardarEncAsigUsuario(lst);
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
        public ActionResult eliminaEncAsigUsuario(tblEN_EncuestaAsignaUsuario obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esSuccess = obj.encuestaID > 0 && obj.usuarioID > 0;
                if (esSuccess)
                {
                    esSuccess = encuestasFactoryServices.getEncuestasService().eliminaEncAsigUsuario(obj);
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
        public ActionResult getEncuestaPendiente()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = encuestasFactoryServices.getEncuestasService().getEncuestaPendiente();
                result.Add("rows", obj.ToList());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getEncuestaAceptada()
        {
            var result = new Dictionary<string, object>();
            try
            {

                var obj = encuestasFactoryServices.getEncuestasService().getEncuestaAceptada();
                result.Add("rows", obj.ToList());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getEncuestaRechazada()
        {
            var result = new Dictionary<string, object>();
            try
            {

                var obj = encuestasFactoryServices.getEncuestasService().getEncuestaRechazada();
                result.Add("rows", obj.ToList());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getEncuestaValidar(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var obj = encuestasFactoryServices.getEncuestasService().getEncuestaValidar(id);
                result.Add("obj", obj);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getEncuestaValidarUpdate(int id, int idUpdate)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var obj = encuestasFactoryServices.getEncuestasService().getEncuestaValidar(id);
                var objUpdate = encuestasFactoryServices.getEncuestasService().getEncuestaValidarUpdate(idUpdate);
                result.Add("obj", obj);
                result.Add("objUpdate", objUpdate);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillEncuestasByOwner()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = encuestasFactoryServices.getEncuestasService().getEncuestasByOwner(vSesiones.sesionUsuarioDTO.id);
                result.Add(ITEMS, obj.Select(x => new { Value = x.id, Text = x.titulo }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillEncuestasByDepto()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioId = usuarioFactoryServices.getUsuarioService().getViewAction(vSesiones.sesionCurrentView, "Administrar") ? 0 : vSesiones.sesionUsuarioDTO.id;

                var encuestas = encuestasFactoryServices.getEncuestasService().getEncuestasPorPermisosCheck(usuarioId);

                var items = encuestas.Select
                    (m => new
                    {
                        Value = m.id,
                        Text = m.titulo + " <i class='glyphicon glyphicon-alert'></i>",
                        deptId = m.departamentoID,
                        soloLectura = m.soloLectura ? 1 : 0
                    });

                var group = encuestas.Select
                    (m => new
                    {
                        Value = m.departamentoID,
                        Text = m.departamento.descripcion
                    });

                result.Add(ITEMS, items);
                result.Add("Group", group);
                result.Add(SUCCESS, true);

                //var idDepartamento = vSesiones.sesionUsuarioDTO.departamento.id;
                //if (usuarioFactoryServices.getUsuarioService().getViewAction(vSesiones.sesionCurrentView, "Administrar"))
                //{
                //    var obj = encuestasFactoryServices.getEncuestasService().getEncuestasTodosDepto();
                //    result.Add(ITEMS, obj.Select(x => new { Value = x.id, Text = x.titulo + " <i class='glyphicon glyphicon-alert'></i>", deptId = x.departamentoID, soloLectura = (x.soloLectura ? 1 : 0) }));
                //    result.Add("Group", obj.Select(x => new { Value = x.departamentoID, Text = x.departamento.descripcion }));
                //    result.Add(SUCCESS, true);
                //}
                //else
                //{
                //    var obj = encuestasFactoryServices.getEncuestasService().getEncuestasByDepto(idDepartamento);
                //    result.Add(ITEMS, obj.Select(x => new { Value = x.id, Text = x.titulo + " <i class='glyphicon glyphicon-alert'></i>", deptId = x.departamentoID, soloLectura = (x.soloLectura ? 1 : 0) }));
                //    result.Add("Group", obj.Select(x => new { Value = x.departamentoID, Text = x.departamento.descripcion }).FirstOrDefault());
                //    result.Add(SUCCESS, true);
                //}
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillEncuestasPorPermisosCheck()
        {
            var result = new Dictionary<string, object>();
            try
            {

                var usuarioId = usuarioFactoryServices.getUsuarioService().getViewAction(vSesiones.sesionCurrentView, "Administrar") ? 0 : vSesiones.sesionUsuarioDTO.id;

                var encuestas = encuestasFactoryServices.getEncuestasService().getEncuestasPorPermisosCheck(usuarioId);

                var items = encuestas.Select
                    (m => new
                    {
                        Value = m.id,
                        Text = m.titulo + " <i class='glyphicon glyphicon-alert'></i>",
                        deptId = m.departamentoID,
                        soloLectura = m.soloLectura ? 1 : 0
                    });

                var group = encuestas.Select
                    (m => new
                    {
                        Value = m.departamentoID,
                        Text = m.departamento.descripcion
                    });

                result.Add(ITEMS, items);
                result.Add("Group", group);
                result.Add("adminPermisosBotones", usuarioId == 0);
                result.Add(SUCCESS, true);

                //var idDepartamento = vSesiones.sesionUsuarioDTO.departamento.id;

                //if (usuarioFactoryServices.getUsuarioService().getViewAction(vSesiones.sesionCurrentView, "Administrar"))
                //{
                //    var obj = encuestasFactoryServices.getEncuestasService().getEncuestasTodosDepto();

                //    result.Add(ITEMS, obj.Select(x => new { Value = x.id, Text = x.titulo + " <i class='glyphicon glyphicon-alert'></i>", deptId = x.departamentoID, soloLectura = (x.soloLectura ? 1 : 0) }));
                //    result.Add("Group", obj.Select(x => new { Value = x.departamentoID, Text = x.departamento.descripcion }));
                //    result.Add("adminPermisosBotones", true);
                //    result.Add(SUCCESS, true);
                //}
                //else
                //{
                //    var obj = encuestasFactoryServices.getEncuestasService().getEncuestasPorPermisosCheck(idDepartamento);

                //    var items = obj.Select(x => new { Value = x.id, Text = x.titulo + " <i class='glyphicon glyphicon-alert'></i>", deptId = x.departamentoID, soloLectura = (x.soloLectura ? 1 : 0) }).ToList();
                //    var Group = obj.Select(x => new { Value = x.departamentoID, Text = x.departamento.descripcion }).ToList();

                //    result.Add(ITEMS, items);
                //    result.Add("Group", Group);
                //    result.Add("adminPermisosBotones", false);
                //    result.Add(SUCCESS, true);
                //}
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult savePregunta(tblEN_Preguntas obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var id = encuestasFactoryServices.getEncuestasService().savePregunta(obj);
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
        public ActionResult delPregunta(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                encuestasFactoryServices.getEncuestasService().delPregunta(id);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getRptPermisosUsuarios(List<int> encuestasID, List<int> usuariosID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<int> obj = new List<int>();

                var dataSet = encuestasFactoryServices.getEncuestasService().getRptPermisosEncuestas(encuestasID, usuariosID, obj).OrderBy(x => x.usuarioID);
                Session["dataSet"] = dataSet;


                result.Add("dataSet", dataSet);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getEncuestaResults(int id, DateTime fechaInicio, DateTime fechaFin)
        {
            var result = new Dictionary<string, object>();
            //try
            //{
            var temp = encuestasFactoryServices.getEncuestasService().getEncuestaResults(id, fechaInicio, fechaFin);
            Session["EncuestaresultsDTO1"] = temp;
            var temp2 = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumen(id, fechaInicio, fechaFin);
            Session["EncuestaresultsDTO2"] = temp2;
            var temp3 = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumenNumero(id, fechaInicio, fechaFin);
            Session["EncuestaresultsDTO2Numero"] = temp3;
            var temp4 = encuestasFactoryServices.getEncuestasService().getEncuestaResultsNoContestadas(id, fechaInicio, fechaFin);
            Session["EncuestaresultsDTO3"] = temp4;
            var temp5 = encuestasFactoryServices.getEncuestasService().getPreguntas3Estrellas(id, fechaInicio, fechaFin);
            Session["Preguntas3Estrellas"] = temp5;

            result.Add("current", 1);
            result.Add("rowCount", 1);
            result.Add("total", temp.Count());
            result.Add("rows", temp);
            result.Add(SUCCESS, true);
            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public FileResult fnDownloadFileEncuesta()
        {
            try
            {
                int id = int.Parse(Request.QueryString["descargar"]);
                var encuesta = encuestasFactoryServices.getEncuestasService().getEncuestaClienteByID(id);
                var ext = encuesta.rutaArchivo.Split('.').Last();

                return File(encuesta.rutaArchivo, "multipart/form-data", "Encuesta." + ext);
            }
            catch (Exception)
            {

                return null;
            }

        }
        public ActionResult getEncuestaGrafica(int id, DateTime fechaInicio, DateTime fechaFin)
        {
            var result = new Dictionary<string, object>();
            try
            {

                result.Add("obj", encuestasFactoryServices.getEncuestasService().getGraficaByEncuesta(id, fechaInicio, fechaFin));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult sendEncuesta(string asunto, List<SendEncuestaDTO> listaEnviar)
        {
            var result = new Dictionary<string, object>();
            try
            {
                foreach (var i in listaEnviar)
                {
                    encuestasFactoryServices.getEncuestasService().sendEncuesta(asunto, i);
                }

                encuestasFactoryServices.getEncuestasService().enviarCorreoUsuariosAsignados(listaEnviar);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string getLinkEncuesta(int encuestaID, int encuestadoID)
        {
            var link = "";
            try
            {
                var m = new UsuarioDAO();
                var cadena = Encriptacion.encriptar(m.getPassByID(encuestadoID).nombreUsuario) + "@" + m.getPassByID(encuestadoID).contrasena;
                link = "http://sigoplan.construplan.com.mx" + ((vSesiones.sesionEmpresaActual == 1) ? "" : ":8084") + "/Encuestas/Encuesta/Responder/?blob=" + cadena + "&encuesta=" + encuestaID + "&empresa=" + vSesiones.sesionEmpresaActual;
            }
            catch (Exception e)
            {
                link = "ocurrio un problema";
            }
            return link;
        }


        public FileResult getRptCertezaDownload()
        {
            using (var exportData = (MemoryStream)Session["rptCerteza"])
            {

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "RptCerteza" + ".xlsx"));
                Response.BinaryWrite(exportData.ToArray());
                Response.End();
                return null;
            }
        }
        public FileResult getFileDownload()
        {
            var encuesta = Request.QueryString["encuesta"];
            var fechaInicio = Request.QueryString["fechainicio"];
            var fechaFin = Request.QueryString["fechafin"];
            ExcelUtilities excel = new ExcelUtilities();
            List<excelSheetDTO> Sheets = new List<excelSheetDTO>();

            #region CONTESTADAS
            excelSheetDTO sheetContestadas = new excelSheetDTO();
            sheetContestadas.name = "CONTESTADAS";
            List<excelRowDTO> excelRowsDTOContestadas = new List<excelRowDTO>();
            excelRowsDTOContestadas.Add(
                new excelRowDTO
                {
                    cells = new List<excelCellDTO>{
                            new excelCellDTO{ text="ENCUESTA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="USUARIO ENVIO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="USUARIO RESPONDIO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="PROYECTO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},                          
                            new excelCellDTO{ text="FECHA ENVIO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="FECHA RESPONDIO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="ASUNTO ENCUESTA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="COMENTARIO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="CALIFICACION", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="TIPO RESPUESTA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0}
                        }
                }
            );
            List<EncuestaResultsDTO> listaContestadas = (List<EncuestaResultsDTO>)Session["EncuestaresultsDTO1"];
            foreach (var i in listaContestadas)
            {
                excelRowsDTOContestadas.Add(
                    new excelRowDTO
                    {
                        cells = new List<excelCellDTO>{
                                new excelCellDTO{ text=""+i.encuestaNombre, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.usuarioEnvioNombre, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.usuarioResponderNombre, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.Proyecto, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.fecha, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.fechaRespndio, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.asunto, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.comentario, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.calificacionPorcentajePromedio, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.tipoRespuestaDesc, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0}
                            }
                    }
                );
            }
            sheetContestadas.Sheet = excelRowsDTOContestadas;
            Sheets.Add(sheetContestadas);
            #endregion
            #region RESUMEN
            excelSheetDTO sheetResumen = new excelSheetDTO();
            sheetResumen.name = "RESUMEN";
            List<excelRowDTO> excelRowsDTOResumen = new List<excelRowDTO>();
            excelRowsDTOResumen.Add(
                new excelRowDTO
                {
                    cells = new List<excelCellDTO>{
                            new excelCellDTO{ text="PREGUNTA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="ENERO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="FEBRERO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="MARZO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="ABRIL", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="MAYO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="JUNIO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="JULIO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="AGOSTO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="SEPTIEMBRE", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="OCTUBRE", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="NOVIEMBRE", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="DICIEMBRE", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="TOTAL", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0}
                        }
                }
            );
            List<EncuestaResults2DTO> listaResumen = (List<EncuestaResults2DTO>)Session["EncuestaresultsDTO2"];
            foreach (var i in listaResumen)
            {
                excelRowsDTOResumen.Add(
                    new excelRowDTO
                    {
                        cells = new List<excelCellDTO>{
                                new excelCellDTO{ text=""+i.pregunta, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.enero+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.febrero+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.marzo+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.abril+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.mayo+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.junio+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.julio+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.agosto+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.septiembre+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.octubre+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.noviembre+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.diciembre+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.total+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0}
                            }
                    }
                );
            }
            excelRowsDTOResumen.Add(
                new excelRowDTO
                {
                    cells = new List<excelCellDTO>{
                            new excelCellDTO{ text="", autoWidthFit=false, fill=false, border=false,colSpan=0,rowSpan=0}
                        }
                }
            );
            excelRowsDTOResumen.Add(
                new excelRowDTO
                {
                    cells = new List<excelCellDTO>{
                            new excelCellDTO{ text="", autoWidthFit=false, fill=false, border=false,colSpan=0,rowSpan=0}
                        }
                }
            );
            List<EncuestaResults2DTO> listaResumenTotales = (List<EncuestaResults2DTO>)Session["EncuestaresultsDTO2Numero"];
            foreach (var i in listaResumenTotales)
            {
                excelRowsDTOResumen.Add(
                    new excelRowDTO
                    {
                        cells = new List<excelCellDTO>{
                                new excelCellDTO{ text=""+i.pregunta, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.enero, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.febrero, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.marzo, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.abril, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.mayo, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.junio, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.julio, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.agosto, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.septiembre, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.octubre, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.noviembre, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.diciembre, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.total, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0}
                            }
                    }
                );
            }
            sheetResumen.Sheet = excelRowsDTOResumen;
            Sheets.Add(sheetResumen);
            #endregion
            #region NO CONTESTADAS
            excelSheetDTO sheetNoContestadas = new excelSheetDTO();
            sheetNoContestadas.name = "NO CONTESTADAS";
            List<excelRowDTO> excelRowsDTONoContestadas = new List<excelRowDTO>();
            excelRowsDTONoContestadas.Add(
                new excelRowDTO
                {
                    cells = new List<excelCellDTO>{
                            new excelCellDTO{ text="ENCUESTA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="ASUNTO ENCUESTA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="USUARIO ENVIO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="USUARIO RESPONDER", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="PROYECTO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="FECHA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0}
                        }
                }
            );
            List<EncuestaResultsDTO> listaNoContestadas = (List<EncuestaResultsDTO>)Session["EncuestaresultsDTO3"];
            foreach (var i in listaNoContestadas)
            {
                excelRowsDTONoContestadas.Add(
                    new excelRowDTO
                    {
                        cells = new List<excelCellDTO>{
                                new excelCellDTO{ text=""+i.encuestaNombre, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.asunto, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.usuarioEnvioNombre, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.usuarioResponderNombre, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.Proyecto, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.fecha, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0}
                            }
                    }
                );
            }
            sheetNoContestadas.Sheet = excelRowsDTONoContestadas;
            Sheets.Add(sheetNoContestadas);
            #endregion
            #region PREGUNTAS 3 ESTRELLAS
            excelSheetDTO sheet3Estrellas = new excelSheetDTO();
            sheet3Estrellas.name = "PREGUNTAS 3 ESTRELLAS";
            List<excelRowDTO> excelRowsDTO3Estrellas = new List<excelRowDTO>();
            excelRowsDTO3Estrellas.Add(
                new excelRowDTO
                {
                    cells = new List<excelCellDTO>{
                            new excelCellDTO{ text="ENCUESTA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="USUARIO ENVIO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="ASUNTO ENCUESTA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="PREGUNTA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="RESPUESTA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="CALIFICACION", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="USUARIO RESPONDIO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="FECHA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                            new excelCellDTO{ text="PROYECTO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0}
                        }
                }
            );
            List<Preguntas3EstrellasDTO> lista3Estrellas = (List<Preguntas3EstrellasDTO>)Session["Preguntas3Estrellas"];
            foreach (var i in lista3Estrellas)
            {
                excelRowsDTO3Estrellas.Add(
                    new excelRowDTO
                    {
                        cells = new List<excelCellDTO>{
                                new excelCellDTO{ text=""+i.Encuesta, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.usuarioEnvioNombre, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.asunto, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.Pregunta, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.Respuesta, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.Calificación, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.Respondio, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.Fecha, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text=""+i.Proyecto, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0}
                            }
                    }
                );
            }
            sheet3Estrellas.Sheet = excelRowsDTO3Estrellas;
            Sheets.Add(sheet3Estrellas);
            #endregion

            excel.CreateExcelFileEstiloPredefinido(this, Sheets, "CONSTRUCCIONES PLANIFICADAS - " + encuesta);
            return null;
        }
        public MemoryStream getFileTodosDownload()
        {
            var fechaInicio = DateTime.Parse(Request.QueryString["fechainicio"]);
            var fechaFin = DateTime.Parse(Request.QueryString["fechafin"]);
            ExcelUtilities excel = new ExcelUtilities();
            List<excelSheetDTO> Sheets = new List<excelSheetDTO>();

            var TodasEncuestas = encuestasFactoryServices.getEncuestasService().getEncuestasTodosDepto();
            foreach (var item in TodasEncuestas)
            {
                #region RESUMEN
                excelSheetDTO sheetResumen = new excelSheetDTO();
                sheetResumen.name = item.departamento.abreviacion + " " + item.titulo;
                List<excelRowDTO> excelRowsDTOResumen = new List<excelRowDTO>();
                excelRowsDTOResumen.Add(
                    new excelRowDTO
                    {
                        cells = new List<excelCellDTO>{
                                new excelCellDTO{ text="PREGUNTA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text="ENERO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text="FEBRERO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text="MARZO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text="ABRIL", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text="MAYO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text="JUNIO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text="JULIO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text="AGOSTO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text="SEPTIEMBRE", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text="OCTUBRE", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text="NOVIEMBRE", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text="DICIEMBRE", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                new excelCellDTO{ text="TOTAL", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0}
                            }
                    }
                );
                List<EncuestaResults2DTO> listaResumen = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumen(item.id, fechaInicio, fechaFin);
                foreach (var i in listaResumen)
                {
                    excelRowsDTOResumen.Add(
                        new excelRowDTO
                        {
                            cells = new List<excelCellDTO>{
                                    new excelCellDTO{ text=""+i.pregunta, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.enero+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.febrero+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.marzo+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.abril+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.mayo+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.junio+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.julio+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.agosto+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.septiembre+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.octubre+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.noviembre+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.diciembre+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.total+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0}
                                }
                        }
                    );
                }
                excelRowsDTOResumen.Add(
                    new excelRowDTO
                    {
                        cells = new List<excelCellDTO>{
                                new excelCellDTO{ text="", autoWidthFit=false, fill=false, border=false,colSpan=0,rowSpan=0}
                            }
                    }
                );
                excelRowsDTOResumen.Add(
                    new excelRowDTO
                    {
                        cells = new List<excelCellDTO>{
                                new excelCellDTO{ text="", autoWidthFit=false, fill=false, border=false,colSpan=0,rowSpan=0}
                            }
                    }
                );
                List<EncuestaResults2DTO> listaResumenTotales = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumenNumero(item.id, fechaInicio, fechaFin);
                foreach (var i in listaResumenTotales)
                {
                    excelRowsDTOResumen.Add(
                        new excelRowDTO
                        {
                            cells = new List<excelCellDTO>{
                                    new excelCellDTO{ text=""+i.pregunta, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.enero, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.febrero, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.marzo, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.abril, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.mayo, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.junio, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.julio, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.agosto, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.septiembre, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.octubre, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.noviembre, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.diciembre, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=""+i.total, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0}
                                }
                        }
                    );
                }
                sheetResumen.Sheet = excelRowsDTOResumen;
                Sheets.Add(sheetResumen);
                #endregion
            }

            excel.CreateExcelFile(this, Sheets, "CONSTRUCCIONES PLANIFICADAS - TODOS");
            return null;
        }

        private MemoryStream WriteExcelEncuesta(DataTable dt1, DataTable dt2, DataTable dt3, DataTable dt4, String extension, string Encuesta)
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
            #region TAB 1
            ISheet sheet1 = workbook.CreateSheet("CONTESTADAS");
            sheet1.SetColumnWidth(0, 5000);
            sheet1.SetColumnWidth(1, 15000);
            sheet1.SetColumnWidth(2, 15000);
            sheet1.SetColumnWidth(3, 5000);
            sheet1.SetColumnWidth(4, 15000);
            sheet1.SetColumnWidth(5, 20000);
            sheet1.SetColumnWidth(6, 5000);
            //make a header row
            IRow row1 = sheet1.CreateRow(0);

            for (int j = 0; j < dt1.Columns.Count; j++)
            {

                ICell cell = row1.CreateCell(j);
                String columnName = dt1.Columns[j].ToString();
                cell.SetCellValue(columnName);
            }

            //loops through data
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                IRow row = sheet1.CreateRow(i + 1);
                for (int j = 0; j < dt1.Columns.Count; j++)
                {

                    ICell cell = row.CreateCell(j);
                    String columnName = dt1.Columns[j].ToString();
                    cell.SetCellValue(dt1.Rows[i][columnName].ToString());
                }
            }
            #endregion
            #region TAB 2
            ISheet sheet2 = workbook.CreateSheet("RESUMEN");
            sheet2.SetColumnWidth(0, 5000);
            sheet2.SetColumnWidth(1, 5000);
            sheet2.SetColumnWidth(2, 5000);
            sheet2.SetColumnWidth(3, 5000);
            sheet2.SetColumnWidth(4, 5000);
            sheet2.SetColumnWidth(5, 5000);
            sheet2.SetColumnWidth(6, 5000);
            sheet2.SetColumnWidth(7, 5000);
            sheet2.SetColumnWidth(8, 5000);
            sheet2.SetColumnWidth(9, 5000);
            sheet2.SetColumnWidth(10, 5000);
            sheet2.SetColumnWidth(11, 5000);
            sheet2.SetColumnWidth(12, 5000);
            sheet2.SetColumnWidth(13, 5000);
            //make a header row
            IRow row2 = sheet2.CreateRow(0);

            for (int j = 0; j < dt2.Columns.Count; j++)
            {

                ICell cell = row2.CreateCell(j);
                String columnName = dt2.Columns[j].ToString();
                cell.SetCellValue(columnName);
            }

            //loops through data
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                IRow row = sheet2.CreateRow(i + 1);
                for (int j = 0; j < dt2.Columns.Count; j++)
                {

                    ICell cell = row.CreateCell(j);
                    String columnName = dt2.Columns[j].ToString();
                    cell.SetCellValue(dt2.Rows[i][columnName].ToString());
                }
            }
            #endregion
            #region TAB 3
            ISheet sheet3 = workbook.CreateSheet("NO CONTESTADAS");
            sheet3.SetColumnWidth(0, 5000);
            sheet1.SetColumnWidth(1, 15000);
            sheet1.SetColumnWidth(2, 15000);
            sheet3.SetColumnWidth(3, 10000);
            sheet3.SetColumnWidth(4, 5000);
            sheet3.SetColumnWidth(5, 10000);
            //make a header row
            IRow row3 = sheet3.CreateRow(0);

            for (int j = 0; j < dt3.Columns.Count; j++)
            {

                ICell cell = row3.CreateCell(j);
                String columnName = dt3.Columns[j].ToString();
                cell.SetCellValue(columnName);
            }

            //loops through data
            for (int i = 0; i < dt3.Rows.Count; i++)
            {
                IRow row = sheet3.CreateRow(i + 1);
                for (int j = 0; j < dt3.Columns.Count; j++)
                {

                    ICell cell = row.CreateCell(j);
                    String columnName = dt3.Columns[j].ToString();
                    cell.SetCellValue(dt3.Rows[i][columnName].ToString());
                }
            }
            #endregion
            #region TAB 4
            ISheet sheet4 = workbook.CreateSheet("PREGUNTAS 3 ESTRELLAS");
            sheet4.SetColumnWidth(0, 5000);
            sheet4.SetColumnWidth(1, 15000);
            sheet4.SetColumnWidth(2, 15000);
            sheet4.SetColumnWidth(3, 20000);
            sheet4.SetColumnWidth(4, 20000);
            sheet4.SetColumnWidth(5, 5000);
            sheet4.SetColumnWidth(6, 5000);
            sheet4.SetColumnWidth(7, 5000);
            //make a header row
            IRow row4 = sheet4.CreateRow(0);

            for (int j = 0; j < dt4.Columns.Count; j++)
            {

                ICell cell = row4.CreateCell(j);
                String columnName = dt4.Columns[j].ToString();
                cell.SetCellValue(columnName);
            }

            //loops through data
            for (int i = 0; i < dt4.Rows.Count; i++)
            {
                IRow row = sheet4.CreateRow(i + 1);
                for (int j = 0; j < dt4.Columns.Count; j++)
                {

                    ICell cell = row.CreateCell(j);
                    String columnName = dt4.Columns[j].ToString();
                    cell.SetCellValue(dt4.Rows[i][columnName].ToString());
                }
            }
            #endregion
            using (var exportData = new MemoryStream())
            {
                Response.Clear();
                workbook.Write(exportData);
                if (extension == "xlsx") //xlsx file format
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "CONSTRUCCIONES PLANIFICADAS - " + Encuesta + ".xlsx"));
                    Response.BinaryWrite(exportData.ToArray());
                }
                else if (extension == "xls")  //xls file format
                {
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "CONSTRUCCIONES PLANIFICADAS - " + Encuesta + ".xls"));
                    Response.BinaryWrite(exportData.GetBuffer());
                }
                Response.End();
                return exportData;
            }
        }

        public ActionResult validarCantidadEncuestas()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var temp = encuestasFactoryServices.getEncuestasService().validarCantidadEncuestas(vSesiones.sesionUsuarioDTO.departamento.id);
                if (temp)
                {
                    result.Add(SUCCESS, true);
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

        public ActionResult getEncuestasTodosMes()
        {
            var mes = Int32.Parse(Request.QueryString["mes"]);
            var year = Request.QueryString["year"];

            var fechaInicio = DateTime.Parse("01/01/" + year);
            var fechaFin = DateTime.Parse("31/01/" + year);

            switch (mes)
            {
                case 1:
                    fechaInicio = DateTime.Parse("01/01/" + year);
                    fechaFin = DateTime.Parse(DateTime.DaysInMonth(Int32.Parse(year), 1) + "/01/" + year);
                    break;
                case 2:
                    fechaInicio = DateTime.Parse("01/02/" + year);
                    fechaFin = DateTime.Parse(DateTime.DaysInMonth(Int32.Parse(year), 2) + "/02/" + year);
                    break;
                case 3:
                    fechaInicio = DateTime.Parse("01/03/" + year);
                    fechaFin = DateTime.Parse(DateTime.DaysInMonth(Int32.Parse(year), 3) + "/03/" + year);
                    break;
                case 4:
                    fechaInicio = DateTime.Parse("01/04/" + year);
                    fechaFin = DateTime.Parse(DateTime.DaysInMonth(Int32.Parse(year), 4) + "/04/" + year);
                    break;
                case 5:
                    fechaInicio = DateTime.Parse("01/05/" + year);
                    fechaFin = DateTime.Parse(DateTime.DaysInMonth(Int32.Parse(year), 5) + "/05/" + year);
                    break;
                case 6:
                    fechaInicio = DateTime.Parse("01/06/" + year);
                    fechaFin = DateTime.Parse(DateTime.DaysInMonth(Int32.Parse(year), 6) + "/06/" + year);
                    break;
                case 7:
                    fechaInicio = DateTime.Parse("01/07/" + year);
                    fechaFin = DateTime.Parse(DateTime.DaysInMonth(Int32.Parse(year), 7) + "/07/" + year);
                    break;
                case 8:
                    fechaInicio = DateTime.Parse("01/08/" + year);
                    fechaFin = DateTime.Parse(DateTime.DaysInMonth(Int32.Parse(year), 8) + "/08/" + year);
                    break;
                case 9:
                    fechaInicio = DateTime.Parse("01/09/" + year);
                    fechaFin = DateTime.Parse(DateTime.DaysInMonth(Int32.Parse(year), 9) + "/09/" + year);
                    break;
                case 10:
                    fechaInicio = DateTime.Parse("01/10/" + year);
                    fechaFin = DateTime.Parse(DateTime.DaysInMonth(Int32.Parse(year), 10) + "/10/" + year);
                    break;
                case 11:
                    fechaInicio = DateTime.Parse("01/11/" + year);
                    fechaFin = DateTime.Parse(DateTime.DaysInMonth(Int32.Parse(year), 11) + "/11/" + year);
                    break;
                case 12:
                    fechaInicio = DateTime.Parse("01/12/" + year);
                    fechaFin = DateTime.Parse(DateTime.DaysInMonth(Int32.Parse(year), 12) + "/12/" + year);
                    break;
                default:
                    fechaInicio = DateTime.Parse("01/01/" + year);
                    fechaFin = DateTime.Parse("31/01/" + year);
                    break;
            }

            ExcelUtilities excel = new ExcelUtilities();
            List<excelSheetDTO> Sheets = new List<excelSheetDTO>();

            excelSheetDTO sheetResumen = new excelSheetDTO();
            sheetResumen.name = "ENCUESTAS MES TODOS";
            excelSheetDTO sheetResumen2 = new excelSheetDTO();
            sheetResumen2.name = "ENCUESTAS MES TODOS 2";
            List<excelRowDTO> excelRowsDTOResumen = new List<excelRowDTO>();
            List<excelRowDTO> excelRowsDTOResumen2 = new List<excelRowDTO>();

            //var departamentos = FillEncuestasByDepto();
            var encuestas = encuestasFactoryServices.getEncuestasService().getEncuestasTodosDepto().ToList();

            var encuestasInternas = encuestas.Where(x => x.tipo == 1).ToList();
            var encuestasExternas = encuestas.Where(x => x.tipo == 2).ToList();
            var encuestasOtros = encuestas.Where(x => x.tipo == 0).ToList();

            var encuestasFiltro = encuestas.Where(x => x.tipo == 1).ToList();

            foreach (var item in encuestasFiltro)
            {
                List<EncuestaResults2DTO> listaResumen = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumenPorPregunta(item.id, fechaInicio, fechaFin);

                decimal mes1;
                decimal mes2;
                decimal mes3;

                #region Meses
                switch (mes)
                {
                    case 1:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                        break;
                    case 2:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.febrero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.febrero).FirstOrDefault());
                        break;
                    case 3:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.marzo).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.marzo).FirstOrDefault());
                        break;
                    case 4:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.abril).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.abril).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.abril).FirstOrDefault());
                        break;
                    case 5:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.mayo).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.mayo).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.mayo).FirstOrDefault());
                        break;
                    case 6:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.junio).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.junio).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.junio).FirstOrDefault());
                        break;
                    case 7:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.julio).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.julio).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.julio).FirstOrDefault());
                        break;
                    case 8:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.agosto).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.agosto).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.agosto).FirstOrDefault());
                        break;
                    case 9:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.septiembre).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.septiembre).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.septiembre).FirstOrDefault());
                        break;
                    case 10:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.octubre).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.octubre).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.octubre).FirstOrDefault());
                        break;
                    case 11:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.noviembre).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.noviembre).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.noviembre).FirstOrDefault());
                        break;
                    case 12:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.diciembre).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.diciembre).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.diciembre).FirstOrDefault());
                        break;
                    default:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                        break;
                }
                #endregion

                decimal[] cal = new decimal[] { mes1 };
                decimal[] tiem = new decimal[] { mes2 };
                decimal[] aten = new decimal[] { mes3 };

                var muestra = listaResumen.Select(x => x.muestra).LastOrDefault();
                var tresestrellas = listaResumen.Select(x => x.tresestrellasCont).LastOrDefault();
                var tresestrellasPerc = listaResumen.Select(x => x.tresestrellasPerc).LastOrDefault();

                excelRowsDTOResumen.Add(
                    new excelRowDTO
                    {
                        cells = new List<excelCellDTO>{
                                new excelCellDTO{ text = item.departamento.descripcion, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = item.titulo, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = mes1.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = mes2.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = mes3.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = muestra.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false},
                                new excelCellDTO{ text = tresestrellas.ToString()
                                    //+ " (" + tresestrellasPerc + "%)"
                                    , autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false}
                            }
                    }
            );
            }

            var encuestas2 = encuestasFactoryServices.getEncuestasService().getEncuestasTodosDepto().ToList();
            var encuestasFiltro2 = encuestas2.Where(x => x.tipo == 2).ToList();

            foreach (var item in encuestasFiltro2)
            {
                List<EncuestaResults2DTO> listaResumen = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumenPorPregunta(item.id, fechaInicio, fechaFin);

                decimal mes1;
                decimal mes2;
                decimal mes3;

                #region Meses
                switch (mes)
                {
                    case 1:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        break;
                    case 2:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                        break;
                    case 3:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                        break;
                    case 4:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.abril).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.abril).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.abril).FirstOrDefault());
                        break;
                    case 5:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.mayo).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.mayo).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.mayo).FirstOrDefault());
                        break;
                    case 6:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.junio).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.junio).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.junio).FirstOrDefault());
                        break;
                    case 7:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.julio).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.julio).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.julio).FirstOrDefault());
                        break;
                    case 8:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.agosto).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.agosto).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.agosto).FirstOrDefault());
                        break;
                    case 9:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.septiembre).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.septiembre).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.septiembre).FirstOrDefault());
                        break;
                    case 10:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.octubre).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.octubre).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.octubre).FirstOrDefault());
                        break;
                    case 11:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.noviembre).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.noviembre).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.noviembre).FirstOrDefault());
                        break;
                    case 12:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.diciembre).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.diciembre).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.diciembre).FirstOrDefault());
                        break;
                    default:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        break;
                }
                #endregion

                var muestra = listaResumen.Select(x => x.muestra).LastOrDefault();
                var tresestrellas = listaResumen.Select(x => x.tresestrellasCont).LastOrDefault();
                var tresestrellasPerc = listaResumen.Select(x => x.tresestrellasPerc).LastOrDefault();

                decimal[] cal = new decimal[] { mes1 };
                decimal[] tiem = new decimal[] { mes2 };
                decimal[] aten = new decimal[] { mes3 };

                var calidad = (cal.Where(x => x != 0).ToList().Count > 0) ? cal.Where(x => x != 0).Average() : 0;
                var tiempo = (tiem.Where(x => x != 0).ToList().Count > 0) ? tiem.Where(x => x != 0).Average() : 0;
                var atencion = (aten.Where(x => x != 0).ToList().Count > 0) ? aten.Where(x => x != 0).Average() : 0;

                excelRowsDTOResumen2.Add(
                    new excelRowDTO
                    {
                        cells = new List<excelCellDTO>{
                                new excelCellDTO{ text = item.departamento.descripcion, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = item.titulo, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = mes1.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = mes2.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = mes3.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = muestra.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false},
                                new excelCellDTO{ text = tresestrellas.ToString()
                                    //+ " (" + tresestrellasPerc + "%)"
                                    , autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false}
                            }
                    }
            );
            }

            var dataMuestraOrdenada = excelRowsDTOResumen.OrderByDescending(x => Int32.Parse(x.cells.ElementAtOrDefault(5).text)).ToList();
            var dataMuestraOrdenada2 = excelRowsDTOResumen2.OrderByDescending(x => Int32.Parse(x.cells.ElementAtOrDefault(5).text)).ToList();

            sheetResumen.Sheet = dataMuestraOrdenada;
            sheetResumen2.Sheet = dataMuestraOrdenada2;
            Sheets.Add(sheetResumen);
            Sheets.Add(sheetResumen2);
            var ruta = archivofs.getArchivo().getUrlDelServidor(13);
            excel.FillExcelFileEncuestasMes(this, Sheets, "ENCUESTAS RESUMEN MES TODOS", mes, year, ruta);
            return null;
        }

        public ActionResult getEncuestasTodosTri()
        {
            var tri = Int32.Parse(Request.QueryString["trimestre"]);
            var year = Request.QueryString["year"];

            var fechaInicio = DateTime.Parse("01/01/" + year);
            var fechaFin = DateTime.Parse("31/03/" + year);

            switch (tri)
            {
                case 1:
                    fechaInicio = DateTime.Parse("01/01/" + year);
                    fechaFin = DateTime.Parse("31/03/" + year);
                    break;
                case 2:
                    fechaInicio = DateTime.Parse("01/04/" + year);
                    fechaFin = DateTime.Parse("30/06/" + year);
                    break;
                case 3:
                    fechaInicio = DateTime.Parse("01/07/" + year);
                    fechaFin = DateTime.Parse("30/09/" + year);
                    break;
                case 4:
                    fechaInicio = DateTime.Parse("01/10/" + year);
                    fechaFin = DateTime.Parse("31/12/" + year);
                    break;
                default:
                    fechaInicio = DateTime.Parse("01/01/" + year);
                    fechaFin = DateTime.Parse("31/03/" + year);
                    break;
            }


            ExcelUtilities excel = new ExcelUtilities();
            List<excelSheetDTO> Sheets = new List<excelSheetDTO>();

            excelSheetDTO sheetResumen = new excelSheetDTO();
            sheetResumen.name = "ENCUESTAS TRI TODOS";
            excelSheetDTO sheetResumen2 = new excelSheetDTO();
            sheetResumen2.name = "ENCUESTAS TRI TODOS 2";
            List<excelRowDTO> excelRowsDTOResumen = new List<excelRowDTO>();
            List<excelRowDTO> excelRowsDTOResumen2 = new List<excelRowDTO>();

            #region Header
            //excelRowsDTOResumen.Add(
            //        new excelRowDTO
            //        {
            //            cells = new List<excelCellDTO>{
            //                    new excelCellDTO{ text="SISTEMA DE GESTIÓN DE CALIDAD", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0,textAlignLeft=true},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0}
            //                }
            //        }
            //);
            //excelRowsDTOResumen.Add(
            //        new excelRowDTO
            //        {
            //            cells = new List<excelCellDTO>{
            //                    new excelCellDTO{ text="SIGOPLAN: SISTEMA DE ENCUESTAS", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0,textAlignLeft=true},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0}
            //                }
            //        }
            //);
            //excelRowsDTOResumen.Add(
            //        new excelRowDTO
            //        {
            //            cells = new List<excelCellDTO>{
            //                    new excelCellDTO{ text="SATISFACCIÓN DE CLIENTE INTERNO", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0,textAlignLeft=true},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0}
            //                }
            //        }
            //);
            //excelRowsDTOResumen.Add(
            //        new excelRowDTO
            //        {
            //            cells = new List<excelCellDTO>{
            //                    new excelCellDTO{ text="Periodo: Enero-Marzo 2018", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0,textAlignLeft=true},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0}
            //                }
            //        }
            //);

            //excelRowsDTOResumen.Add(
            //        new excelRowDTO
            //        {
            //            cells = new List<excelCellDTO>{
            //                    new excelCellDTO{ text="ÁREA", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=2},
            //                    new excelCellDTO{ text="CALIDAD", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=2},
            //                    new excelCellDTO{ text="TIEMPO", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=2},
            //                    new excelCellDTO{ text="ATENCIÓN", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=2},
            //                    new excelCellDTO{ text="CALIDAD", autoWidthFit=true, fill=true, border=true,colSpan=3,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="TIEMPO", autoWidthFit=true, fill=true, border=true,colSpan=3,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="ATENCIÓN", autoWidthFit=true, fill=true, border=true,colSpan=3,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="MUESTRA", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=2},
            //                    new excelCellDTO{ text="3 Estrellas o Menos", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=2}
            //                }
            //        }
            //);
            //excelRowsDTOResumen.Add(
            //        new excelRowDTO
            //        {
            //            cells = new List<excelCellDTO>{
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=false, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=false, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=false, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="ENERO", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="FEBRERO", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="MARZO", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="ENERO", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="FEBRERO", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="MARZO", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="ENERO", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="FEBRERO", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="MARZO", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0}
            //                }
            //        }
            //);
            #endregion

            var encuestas = encuestasFactoryServices.getEncuestasService().getEncuestasTodosDepto().ToList();
            var encuestasFiltro = encuestas.Where(x => x.tipo == 1).ToList();

            foreach (var item in encuestasFiltro)
            {
                List<EncuestaResults2DTO> listaResumen = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumenPorPregunta(item.id, fechaInicio, fechaFin);

                decimal mes1;
                decimal mes2;
                decimal mes3;
                decimal mes4;
                decimal mes5;
                decimal mes6;
                decimal mes7;
                decimal mes8;
                decimal mes9;

                #region Meses
                switch (tri)
                {
                    case 1:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.febrero).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.marzo).FirstOrDefault());
                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.febrero).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.marzo).FirstOrDefault());
                        break;
                    case 2:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.abril).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.mayo).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.junio).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.abril).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.mayo).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.junio).FirstOrDefault());
                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.abril).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.mayo).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.junio).FirstOrDefault());
                        break;
                    case 3:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.julio).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.agosto).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.septiembre).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.julio).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.agosto).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.septiembre).FirstOrDefault());
                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.julio).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.agosto).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.septiembre).FirstOrDefault());
                        break;
                    case 4:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.octubre).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.noviembre).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.diciembre).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.octubre).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.noviembre).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.diciembre).FirstOrDefault());
                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.octubre).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.noviembre).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.diciembre).FirstOrDefault());
                        break;
                    default:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.febrero).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.marzo).FirstOrDefault());
                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.febrero).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.marzo).FirstOrDefault());
                        break;
                }
                #endregion

                decimal[] cal = new decimal[] { mes1, mes2, mes3 };
                decimal[] tiem = new decimal[] { mes4, mes5, mes6 };
                decimal[] aten = new decimal[] { mes7, mes8, mes9 };

                var muestra = listaResumen.Select(x => x.muestra).LastOrDefault();
                var tresestrellas = listaResumen.Select(x => x.tresestrellasCont).LastOrDefault();
                var tresestrellasPerc = listaResumen.Select(x => x.tresestrellasPerc).LastOrDefault();

                var calidad = (cal.Where(x => x != 0).ToList().Count > 0) ? cal.Where(x => x != 0).Average() : 0;
                var tiempo = (tiem.Where(x => x != 0).ToList().Count > 0) ? tiem.Where(x => x != 0).Average() : 0;
                var atencion = (aten.Where(x => x != 0).ToList().Count > 0) ? aten.Where(x => x != 0).Average() : 0;

                excelRowsDTOResumen.Add(
                    new excelRowDTO
                    {
                        cells = new List<excelCellDTO>{
                                new excelCellDTO{ text = item.departamento.descripcion, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = item.titulo, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = calidad.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = tiempo.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = atencion.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = mes1.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes2.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes3.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes4.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes5.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes6.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes7.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes8.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes9.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = muestra.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false},
                                new excelCellDTO{ text = tresestrellas.ToString() + " (" + tresestrellasPerc + "%)", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false}
                            }
                    }
            );
            }

            #region Header2
            //excelRowsDTOResumen.Add(
            //        new excelRowDTO
            //        {
            //            cells = new List<excelCellDTO>{
            //                    new excelCellDTO{ text="SISTEMA DE GESTIÓN DE CALIDAD", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0,textAlignLeft=true},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0}
            //                }
            //        }
            //);
            //excelRowsDTOResumen.Add(
            //        new excelRowDTO
            //        {
            //            cells = new List<excelCellDTO>{
            //                    new excelCellDTO{ text="SIGOPLAN: SISTEMA DE ENCUESTAS DE SATISFACCIÓN DEL CLIENTE", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0,textAlignLeft=true},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0}
            //                }
            //        }
            //);
            //excelRowsDTOResumen.Add(
            //        new excelRowDTO
            //        {
            //            cells = new List<excelCellDTO>{
            //                    new excelCellDTO{ text="SATISFACCIÓN DEL CLIENTE EXTERNO", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0,textAlignLeft=true},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0}
            //                }
            //        }
            //);
            //excelRowsDTOResumen.Add(
            //        new excelRowDTO
            //        {
            //            cells = new List<excelCellDTO>{
            //                    new excelCellDTO{ text="Periodo: Enero-Marzo 2018", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0,textAlignLeft=true},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0}
            //                }
            //        }
            //);

            //excelRowsDTOResumen.Add(
            //        new excelRowDTO
            //        {
            //            cells = new List<excelCellDTO>{
            //                    new excelCellDTO{ text="ÁREA", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=2},
            //                    new excelCellDTO{ text="CALIDAD", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=2},
            //                    new excelCellDTO{ text="TIEMPO", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=2},
            //                    new excelCellDTO{ text="ATENCIÓN", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=2},
            //                    new excelCellDTO{ text="CALIDAD", autoWidthFit=true, fill=true, border=true,colSpan=3,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="TIEMPO", autoWidthFit=true, fill=true, border=true,colSpan=3,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="ATENCIÓN", autoWidthFit=true, fill=true, border=true,colSpan=3,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="MUESTRA", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=2},
            //                    new excelCellDTO{ text="3 Estrellas o Menos", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=2}
            //                }
            //        }
            //);
            //excelRowsDTOResumen.Add(
            //        new excelRowDTO
            //        {
            //            cells = new List<excelCellDTO>{
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=false, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=false, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=false, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="ENERO", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="FEBRERO", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="MARZO", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="ENERO", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="FEBRERO", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="MARZO", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="ENERO", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="FEBRERO", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="MARZO", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
            //                    new excelCellDTO{ text="", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0}
            //                }
            //        }
            //);
            #endregion

            var encuestas2 = encuestasFactoryServices.getEncuestasService().getEncuestasTodosDepto().ToList();
            var encuestasFiltro2 = encuestas2.Where(x => x.tipo == 2).ToList();

            foreach (var item in encuestasFiltro2)
            {
                List<EncuestaResults2DTO> listaResumen = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumenPorPregunta(item.id, fechaInicio, fechaFin);

                decimal mes1;
                decimal mes2;
                decimal mes3;
                decimal mes4;
                decimal mes5;
                decimal mes6;
                decimal mes7;
                decimal mes8;
                decimal mes9;

                #region Meses
                switch (tri)
                {
                    case 1:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.febrero).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.marzo).FirstOrDefault());
                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.febrero).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.marzo).FirstOrDefault());
                        break;
                    case 2:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.abril).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.mayo).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.junio).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.abril).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.mayo).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.junio).FirstOrDefault());
                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.abril).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.mayo).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.junio).FirstOrDefault());
                        break;
                    case 3:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.julio).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.agosto).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.septiembre).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.julio).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.agosto).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.septiembre).FirstOrDefault());
                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.julio).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.agosto).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.septiembre).FirstOrDefault());
                        break;
                    case 4:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.octubre).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.noviembre).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.diciembre).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.octubre).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.noviembre).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.diciembre).FirstOrDefault());
                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.octubre).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.noviembre).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.diciembre).FirstOrDefault());
                        break;
                    default:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.febrero).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.marzo).FirstOrDefault());
                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.febrero).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.marzo).FirstOrDefault());
                        break;
                }
                #endregion

                var muestra = listaResumen.Select(x => x.muestra).LastOrDefault();
                var tresestrellas = listaResumen.Select(x => x.tresestrellasCont).LastOrDefault();
                var tresestrellasPerc = listaResumen.Select(x => x.tresestrellasPerc).LastOrDefault();

                decimal[] cal = new decimal[] { mes1, mes2, mes3 };
                decimal[] tiem = new decimal[] { mes4, mes5, mes6 };
                decimal[] aten = new decimal[] { mes7, mes8, mes9 };

                var calidad = (cal.Where(x => x != 0).ToList().Count > 0) ? cal.Where(x => x != 0).Average() : 0;
                var tiempo = (tiem.Where(x => x != 0).ToList().Count > 0) ? tiem.Where(x => x != 0).Average() : 0;
                var atencion = (aten.Where(x => x != 0).ToList().Count > 0) ? aten.Where(x => x != 0).Average() : 0;

                excelRowsDTOResumen2.Add(
                    new excelRowDTO
                    {
                        cells = new List<excelCellDTO>{
                                new excelCellDTO{ text = item.departamento.descripcion, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = item.titulo, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = calidad.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = tiempo.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = atencion.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = mes1.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes2.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes3.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes4.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes5.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes6.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes7.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes8.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes9.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = muestra.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false},
                                new excelCellDTO{ text = tresestrellas.ToString() + " (" + tresestrellasPerc + "%)", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false}
                            }
                    }
            );
            }

            #region Codigo Anterior
            //foreach (var item in encuestas)
            //{
            //    var departamentos = encuestasFactoryServices.getEncuestasService().getEncuestasByDepto(item.departamentoID);
            //    List<EncuestaResults2DTO> listaResumen = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumen(item.id, fechaInicio, fechaFin);
            //    foreach (var i in listaResumen)
            //    {
            //        excelRowsDTOResumen.Add(
            //            new excelRowDTO
            //            {
            //                cells = new List<excelCellDTO>{
            //                        new excelCellDTO{ text=""+i.pregunta, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.enero+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.febrero+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.marzo+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.abril+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.mayo+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.junio+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.julio+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.agosto+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.septiembre+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.octubre+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.noviembre+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.diciembre+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.total+"%", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0}
            //                    }
            //            }
            //        );
            //    }
            //    excelRowsDTOResumen.Add(
            //        new excelRowDTO
            //        {
            //            cells = new List<excelCellDTO>{
            //                    new excelCellDTO{ text="", autoWidthFit=false, fill=false, border=false,colSpan=0,rowSpan=0}
            //                }
            //        }
            //    );
            //    excelRowsDTOResumen.Add(
            //        new excelRowDTO
            //        {
            //            cells = new List<excelCellDTO>{
            //                    new excelCellDTO{ text="", autoWidthFit=false, fill=false, border=false,colSpan=0,rowSpan=0}
            //                }
            //        }
            //    );
            //    List<EncuestaResults2DTO> listaResumenTotales = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumenNumero(item.id, fechaInicio, fechaFin);
            //    foreach (var i in listaResumenTotales)
            //    {
            //        excelRowsDTOResumen.Add(
            //            new excelRowDTO
            //            {
            //                cells = new List<excelCellDTO>{
            //                        new excelCellDTO{ text=""+i.pregunta, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.enero, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.febrero, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.marzo, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.abril, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.mayo, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.junio, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.julio, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.agosto, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.septiembre, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.octubre, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.noviembre, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.diciembre, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0},
            //                        new excelCellDTO{ text=""+i.total, autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0}
            //                    }
            //            }
            //        );
            //    }
            //}
            #endregion

            var dataMuestraOrdenada = excelRowsDTOResumen.OrderByDescending(x => Int32.Parse(x.cells.ElementAtOrDefault(14).text)).ToList();
            var dataMuestraOrdenada2 = excelRowsDTOResumen2.OrderByDescending(x => Int32.Parse(x.cells.ElementAtOrDefault(14).text)).ToList();

            sheetResumen.Sheet = dataMuestraOrdenada;
            sheetResumen2.Sheet = dataMuestraOrdenada2;
            Sheets.Add(sheetResumen);
            Sheets.Add(sheetResumen2);
            var ruta = archivofs.getArchivo().getUrlDelServidor(13);
            excel.FillExcelFileEncuestas(this, Sheets, "ENCUESTAS RESUMEN TRI TODOS", tri, year, ruta);
            return null;
        }

        public ActionResult getEncuestasTodosSem()
        {
            var sem = Int32.Parse(Request.QueryString["sem"]);
            var year = Request.QueryString["year"];

            var fechaInicio = DateTime.Parse("01/01/" + year);
            var fechaFin = DateTime.Parse("30/06/" + year);

            switch (sem)
            {
                case 1:
                    fechaInicio = DateTime.Parse("01/01/" + year);
                    fechaFin = DateTime.Parse("30/06/" + year);
                    break;
                case 2:
                    fechaInicio = DateTime.Parse("01/07/" + year);
                    fechaFin = DateTime.Parse("31/12/" + year);
                    break;
                default:
                    fechaInicio = DateTime.Parse("01/01/" + year);
                    fechaFin = DateTime.Parse("30/06/" + year);
                    break;
            }


            ExcelUtilities excel = new ExcelUtilities();
            List<excelSheetDTO> Sheets = new List<excelSheetDTO>();

            excelSheetDTO sheetResumen = new excelSheetDTO();
            sheetResumen.name = "ENCUESTAS SEM TODOS";
            excelSheetDTO sheetResumen2 = new excelSheetDTO();
            sheetResumen2.name = "ENCUESTAS SEM TODOS 2";
            List<excelRowDTO> excelRowsDTOResumen = new List<excelRowDTO>();
            List<excelRowDTO> excelRowsDTOResumen2 = new List<excelRowDTO>();

            var encuestas = encuestasFactoryServices.getEncuestasService().getEncuestasTodosDepto().ToList();
            var encuestasFiltro = encuestas.Where(x => x.tipo == 1).ToList();

            foreach (var item in encuestasFiltro)
            {
                List<EncuestaResults2DTO> listaResumen = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumenPorPregunta(item.id, fechaInicio, fechaFin);

                decimal mes1;
                decimal mes2;
                decimal mes3;
                decimal mes4;
                decimal mes5;
                decimal mes6;
                decimal mes7;
                decimal mes8;
                decimal mes9;
                decimal mes10;
                decimal mes11;
                decimal mes12;
                decimal mes13;
                decimal mes14;
                decimal mes15;
                decimal mes16;
                decimal mes17;
                decimal mes18;

                #region Meses
                switch (sem)
                {
                    case 1:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.abril).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.mayo).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.junio).FirstOrDefault());

                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.febrero).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.marzo).FirstOrDefault());
                        mes10 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.abril).FirstOrDefault());
                        mes11 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.mayo).FirstOrDefault());
                        mes12 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.junio).FirstOrDefault());

                        mes13 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                        mes14 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.febrero).FirstOrDefault());
                        mes15 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.marzo).FirstOrDefault());
                        mes16 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.abril).FirstOrDefault());
                        mes17 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.mayo).FirstOrDefault());
                        mes18 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.junio).FirstOrDefault());

                        break;
                    case 2:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.julio).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.agosto).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.septiembre).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.octubre).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.noviembre).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.diciembre).FirstOrDefault());

                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.julio).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.agosto).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.septiembre).FirstOrDefault());
                        mes10 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.octubre).FirstOrDefault());
                        mes11 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.noviembre).FirstOrDefault());
                        mes12 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.diciembre).FirstOrDefault());

                        mes13 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.julio).FirstOrDefault());
                        mes14 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.agosto).FirstOrDefault());
                        mes15 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.septiembre).FirstOrDefault());
                        mes16 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.octubre).FirstOrDefault());
                        mes17 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.noviembre).FirstOrDefault());
                        mes18 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.diciembre).FirstOrDefault());
                        break;
                    default:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.abril).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.mayo).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.junio).FirstOrDefault());

                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.febrero).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.marzo).FirstOrDefault());
                        mes10 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.abril).FirstOrDefault());
                        mes11 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.mayo).FirstOrDefault());
                        mes12 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.junio).FirstOrDefault());

                        mes13 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                        mes14 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.febrero).FirstOrDefault());
                        mes15 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.marzo).FirstOrDefault());
                        mes16 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.abril).FirstOrDefault());
                        mes17 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.mayo).FirstOrDefault());
                        mes18 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.junio).FirstOrDefault());
                        break;
                }
                #endregion

                decimal[] cal = new decimal[] { mes1, mes2, mes3, mes4, mes5, mes6 };
                decimal[] tiem = new decimal[] { mes7, mes8, mes9, mes10, mes11, mes12 };
                decimal[] aten = new decimal[] { mes13, mes14, mes15, mes16, mes17, mes18 };

                var muestra = listaResumen.Select(x => x.muestra).LastOrDefault();
                var tresestrellas = listaResumen.Select(x => x.tresestrellasCont).LastOrDefault();
                var tresestrellasPerc = listaResumen.Select(x => x.tresestrellasPerc).LastOrDefault();

                var calidad = (cal.Where(x => x != 0).ToList().Count > 0) ? cal.Where(x => x != 0).Average() : 0;
                var tiempo = (tiem.Where(x => x != 0).ToList().Count > 0) ? tiem.Where(x => x != 0).Average() : 0;
                var atencion = (aten.Where(x => x != 0).ToList().Count > 0) ? aten.Where(x => x != 0).Average() : 0;

                excelRowsDTOResumen.Add(
                    new excelRowDTO
                    {
                        cells = new List<excelCellDTO>{
                                new excelCellDTO{ text = item.departamento.descripcion, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = item.titulo, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = calidad.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = tiempo.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = atencion.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = mes1.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes2.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes3.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes4.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes5.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes6.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes7.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes8.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes9.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes10.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes11.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes12.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes13.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes14.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes15.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes16.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes17.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes18.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = muestra.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false},
                                new excelCellDTO{ text = tresestrellas.ToString() + " (" + tresestrellasPerc + "%)", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false}
                            }
                    }
            );
            }

            var encuestas2 = encuestasFactoryServices.getEncuestasService().getEncuestasTodosDepto().ToList();
            var encuestasFiltro2 = encuestas2.Where(x => x.tipo == 2).ToList();

            foreach (var item in encuestasFiltro2)
            {
                List<EncuestaResults2DTO> listaResumen = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumenPorPregunta(item.id, fechaInicio, fechaFin);

                decimal mes1;
                decimal mes2;
                decimal mes3;
                decimal mes4;
                decimal mes5;
                decimal mes6;
                decimal mes7;
                decimal mes8;
                decimal mes9;
                decimal mes10;
                decimal mes11;
                decimal mes12;
                decimal mes13;
                decimal mes14;
                decimal mes15;
                decimal mes16;
                decimal mes17;
                decimal mes18;

                #region Meses
                switch (sem)
                {
                    case 1:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.abril).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.mayo).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.junio).FirstOrDefault());

                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.febrero).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.marzo).FirstOrDefault());
                        mes10 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.abril).FirstOrDefault());
                        mes11 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.mayo).FirstOrDefault());
                        mes12 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.junio).FirstOrDefault());

                        mes13 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                        mes14 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.febrero).FirstOrDefault());
                        mes15 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.marzo).FirstOrDefault());
                        mes16 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.abril).FirstOrDefault());
                        mes17 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.mayo).FirstOrDefault());
                        mes18 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.junio).FirstOrDefault());

                        break;
                    case 2:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.julio).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.agosto).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.septiembre).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.octubre).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.noviembre).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.diciembre).FirstOrDefault());

                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.julio).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.agosto).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.septiembre).FirstOrDefault());
                        mes10 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.octubre).FirstOrDefault());
                        mes11 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.noviembre).FirstOrDefault());
                        mes12 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.diciembre).FirstOrDefault());

                        mes13 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.julio).FirstOrDefault());
                        mes14 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.agosto).FirstOrDefault());
                        mes15 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.septiembre).FirstOrDefault());
                        mes16 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.octubre).FirstOrDefault());
                        mes17 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.noviembre).FirstOrDefault());
                        mes18 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.diciembre).FirstOrDefault());
                        break;
                    default:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.abril).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.mayo).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.junio).FirstOrDefault());

                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.febrero).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.marzo).FirstOrDefault());
                        mes10 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.abril).FirstOrDefault());
                        mes11 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.mayo).FirstOrDefault());
                        mes12 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.junio).FirstOrDefault());

                        mes13 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                        mes14 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.febrero).FirstOrDefault());
                        mes15 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.marzo).FirstOrDefault());
                        mes16 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.abril).FirstOrDefault());
                        mes17 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.mayo).FirstOrDefault());
                        mes18 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.junio).FirstOrDefault());
                        break;
                }
                #endregion

                var muestra = listaResumen.Select(x => x.muestra).LastOrDefault();
                var tresestrellas = listaResumen.Select(x => x.tresestrellasCont).LastOrDefault();
                var tresestrellasPerc = listaResumen.Select(x => x.tresestrellasPerc).LastOrDefault();

                decimal[] cal = new decimal[] { mes1, mes2, mes3, mes4, mes5, mes6 };
                decimal[] tiem = new decimal[] { mes7, mes8, mes9, mes10, mes11, mes12 };
                decimal[] aten = new decimal[] { mes13, mes14, mes15, mes16, mes17, mes18 };

                var calidad = (cal.Where(x => x != 0).ToList().Count > 0) ? cal.Where(x => x != 0).Average() : 0;
                var tiempo = (tiem.Where(x => x != 0).ToList().Count > 0) ? tiem.Where(x => x != 0).Average() : 0;
                var atencion = (aten.Where(x => x != 0).ToList().Count > 0) ? aten.Where(x => x != 0).Average() : 0;

                excelRowsDTOResumen2.Add(
                    new excelRowDTO
                    {
                        cells = new List<excelCellDTO>{
                                new excelCellDTO{ text = item.departamento.descripcion, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = item.titulo, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = calidad.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = tiempo.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = atencion.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = mes1.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes2.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes3.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes4.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes5.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes6.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes7.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes8.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes9.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes10.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes11.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes12.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes13.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes14.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes15.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes16.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes17.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes18.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = muestra.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false},
                                new excelCellDTO{ text = tresestrellas.ToString() + " (" + tresestrellasPerc + "%)", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false}
                            }
                    }
            );
            }

            var dataMuestraOrdenada = excelRowsDTOResumen.OrderByDescending(x => Int32.Parse(x.cells.ElementAtOrDefault(23).text)).ToList();
            var dataMuestraOrdenada2 = excelRowsDTOResumen2.OrderByDescending(x => Int32.Parse(x.cells.ElementAtOrDefault(23).text)).ToList();

            sheetResumen.Sheet = dataMuestraOrdenada;
            sheetResumen2.Sheet = dataMuestraOrdenada2;
            Sheets.Add(sheetResumen);
            Sheets.Add(sheetResumen2);
            var ruta = archivofs.getArchivo().getUrlDelServidor(13);
            excel.FillExcelFileEncuestasSem(this, Sheets, "ENCUESTAS RESUMEN SEM TODOS", sem, year, ruta);
            return null;
        }

        public ActionResult getEncuestasTodosYear()
        {
            var year = Request.QueryString["year"];

            var fechaInicio = DateTime.Parse("01/01/" + year);
            var fechaFin = DateTime.Parse("31/12/" + year);

            ExcelUtilities excel = new ExcelUtilities();
            List<excelSheetDTO> Sheets = new List<excelSheetDTO>();

            excelSheetDTO sheetResumen = new excelSheetDTO();
            sheetResumen.name = "ENCUESTAS AÑO TODOS";
            excelSheetDTO sheetResumen2 = new excelSheetDTO();
            sheetResumen2.name = "ENCUESTAS AÑO TODOS 2";
            List<excelRowDTO> excelRowsDTOResumen = new List<excelRowDTO>();
            List<excelRowDTO> excelRowsDTOResumen2 = new List<excelRowDTO>();

            var encuestas = encuestasFactoryServices.getEncuestasService().getEncuestasTodosDepto().ToList();
            var encuestasFiltro = encuestas.Where(x => x.tipo == 1).ToList();

            foreach (var item in encuestasFiltro)
            {
                List<EncuestaResults2DTO> listaResumen = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumenPorPreguntaYear(item.id, fechaInicio, fechaFin);

                decimal trim1 = listaResumen.Where(x => x.tipo == 2).Select(y => y.trim1).FirstOrDefault();
                decimal trim2 = listaResumen.Where(x => x.tipo == 2).Select(y => y.trim2).FirstOrDefault();
                decimal trim3 = listaResumen.Where(x => x.tipo == 2).Select(y => y.trim3).FirstOrDefault();
                decimal trim4 = listaResumen.Where(x => x.tipo == 2).Select(y => y.trim4).FirstOrDefault();
                decimal trim5 = listaResumen.Where(x => x.tipo == 1).Select(y => y.trim1).FirstOrDefault();
                decimal trim6 = listaResumen.Where(x => x.tipo == 1).Select(y => y.trim2).FirstOrDefault();
                decimal trim7 = listaResumen.Where(x => x.tipo == 1).Select(y => y.trim3).FirstOrDefault();
                decimal trim8 = listaResumen.Where(x => x.tipo == 1).Select(y => y.trim4).FirstOrDefault();
                decimal trim9 = listaResumen.Where(x => x.tipo == 3).Select(y => y.trim1).FirstOrDefault();
                decimal trim10 = listaResumen.Where(x => x.tipo == 3).Select(y => y.trim2).FirstOrDefault();
                decimal trim11 = listaResumen.Where(x => x.tipo == 3).Select(y => y.trim3).FirstOrDefault();
                decimal trim12 = listaResumen.Where(x => x.tipo == 3).Select(y => y.trim4).FirstOrDefault();


                decimal[] cal = new decimal[] { trim1, trim2, trim3, trim4 };
                decimal[] tiem = new decimal[] { trim5, trim6, trim7, trim8 };
                decimal[] aten = new decimal[] { trim9, trim10, trim11, trim12 };

                var muestra = listaResumen.Select(x => x.muestra).LastOrDefault();
                var tresestrellas = listaResumen.Select(x => x.tresestrellasCont).LastOrDefault();
                var tresestrellasPerc = listaResumen.Select(x => x.tresestrellasPerc).LastOrDefault();

                var calidad = (cal.Where(x => x != 0).ToList().Count > 0) ? cal.Where(x => x != 0).Average() : 0;
                var tiempo = (tiem.Where(x => x != 0).ToList().Count > 0) ? tiem.Where(x => x != 0).Average() : 0;
                var atencion = (aten.Where(x => x != 0).ToList().Count > 0) ? aten.Where(x => x != 0).Average() : 0;

                excelRowsDTOResumen.Add(
                    new excelRowDTO
                    {
                        cells = new List<excelCellDTO>{
                                new excelCellDTO{ text = item.departamento.descripcion, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = item.titulo, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = "", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = "", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = "", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = "", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = trim1.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim2.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim3.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim4.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim5.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim6.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim7.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim8.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim9.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim10.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim11.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim12.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = muestra.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false},
                                new excelCellDTO{ text = tresestrellas.ToString()
                                    //+ " (" + tresestrellasPerc + "%)"
                                    , autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false}
                            }
                    }
            );
            }

            var encuestas2 = encuestasFactoryServices.getEncuestasService().getEncuestasTodosDepto().ToList();
            var encuestasFiltro2 = encuestas2.Where(x => x.tipo == 2).ToList();

            foreach (var item in encuestasFiltro2)
            {
                List<EncuestaResults2DTO> listaResumen = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumenPorPreguntaYear(item.id, fechaInicio, fechaFin);

                decimal trim1 = listaResumen.Where(x => x.tipo == 2).Select(y => y.trim1).FirstOrDefault();
                decimal trim2 = listaResumen.Where(x => x.tipo == 2).Select(y => y.trim2).FirstOrDefault();
                decimal trim3 = listaResumen.Where(x => x.tipo == 2).Select(y => y.trim3).FirstOrDefault();
                decimal trim4 = listaResumen.Where(x => x.tipo == 2).Select(y => y.trim4).FirstOrDefault();
                decimal trim5 = listaResumen.Where(x => x.tipo == 1).Select(y => y.trim1).FirstOrDefault();
                decimal trim6 = listaResumen.Where(x => x.tipo == 1).Select(y => y.trim2).FirstOrDefault();
                decimal trim7 = listaResumen.Where(x => x.tipo == 1).Select(y => y.trim3).FirstOrDefault();
                decimal trim8 = listaResumen.Where(x => x.tipo == 1).Select(y => y.trim4).FirstOrDefault();
                decimal trim9 = listaResumen.Where(x => x.tipo == 3).Select(y => y.trim1).FirstOrDefault();
                decimal trim10 = listaResumen.Where(x => x.tipo == 3).Select(y => y.trim2).FirstOrDefault();
                decimal trim11 = listaResumen.Where(x => x.tipo == 3).Select(y => y.trim3).FirstOrDefault();
                decimal trim12 = listaResumen.Where(x => x.tipo == 3).Select(y => y.trim4).FirstOrDefault();

                var muestra = listaResumen.Select(x => x.muestra).LastOrDefault();
                var tresestrellas = listaResumen.Select(x => x.tresestrellasCont).LastOrDefault();
                var tresestrellasPerc = listaResumen.Select(x => x.tresestrellasPerc).LastOrDefault();

                decimal[] cal = new decimal[] { trim1, trim2, trim3, trim4 };
                decimal[] tiem = new decimal[] { trim5, trim6, trim7, trim8 };
                decimal[] aten = new decimal[] { trim9, trim10, trim11, trim12 };

                var calidad = (cal.Where(x => x != 0).ToList().Count > 0) ? cal.Where(x => x != 0).Average() : 0;
                var tiempo = (tiem.Where(x => x != 0).ToList().Count > 0) ? tiem.Where(x => x != 0).Average() : 0;
                var atencion = (aten.Where(x => x != 0).ToList().Count > 0) ? aten.Where(x => x != 0).Average() : 0;

                excelRowsDTOResumen2.Add(
                    new excelRowDTO
                    {
                        cells = new List<excelCellDTO>{
                                new excelCellDTO{ text = item.departamento.descripcion, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = item.titulo, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = "", autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = "", autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = "", autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = "", autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = trim1.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim2.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim3.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim4.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim5.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim6.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim7.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim8.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim9.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim10.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim11.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim12.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = muestra.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false},
                                new excelCellDTO{ text = tresestrellas.ToString()
                                    //+ " (" + tresestrellasPerc + "%)"
                                    , autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false}
                            }
                    }
            );
            }

            var dataMuestraOrdenada = excelRowsDTOResumen.OrderByDescending(x => Int32.Parse(x.cells.ElementAtOrDefault(18).text)).ToList();
            var dataMuestraOrdenada2 = excelRowsDTOResumen2.OrderByDescending(x => Int32.Parse(x.cells.ElementAtOrDefault(18).text)).ToList();

            sheetResumen.Sheet = dataMuestraOrdenada;
            sheetResumen2.Sheet = dataMuestraOrdenada2;
            Sheets.Add(sheetResumen);
            Sheets.Add(sheetResumen2);
            var ruta = archivofs.getArchivo().getUrlDelServidor(13);
            excel.FillExcelFileEncuestasYear(this, Sheets, "ENCUESTAS RESUMEN AÑO TODOS", year, ruta);
            return null;
        }

        public ActionResult getEncuestaIndividualMes()
        {
            var departamentoID = Int32.Parse(Request.QueryString["departamentoID"]);
            var departamento = Request.QueryString["departamento"];

            var mes = Int32.Parse(Request.QueryString["mes"]);
            var year = Request.QueryString["year"];

            var fechaInicio = DateTime.Parse("01/01/" + year);
            var fechaFin = DateTime.Parse("31/01/" + year);

            switch (mes)
            {
                case 1:
                    fechaInicio = DateTime.Parse("01/01/" + year);
                    fechaFin = DateTime.Parse(DateTime.DaysInMonth(Int32.Parse(year), 1) + "/01/" + year);
                    break;
                case 2:
                    fechaInicio = DateTime.Parse("01/02/" + year);
                    fechaFin = DateTime.Parse(DateTime.DaysInMonth(Int32.Parse(year), 2) + "/02/" + year);
                    break;
                case 3:
                    fechaInicio = DateTime.Parse("01/03/" + year);
                    fechaFin = DateTime.Parse(DateTime.DaysInMonth(Int32.Parse(year), 3) + "/03/" + year);
                    break;
                case 4:
                    fechaInicio = DateTime.Parse("01/04/" + year);
                    fechaFin = DateTime.Parse(DateTime.DaysInMonth(Int32.Parse(year), 4) + "/04/" + year);
                    break;
                case 5:
                    fechaInicio = DateTime.Parse("01/05/" + year);
                    fechaFin = DateTime.Parse(DateTime.DaysInMonth(Int32.Parse(year), 5) + "/05/" + year);
                    break;
                case 6:
                    fechaInicio = DateTime.Parse("01/06/" + year);
                    fechaFin = DateTime.Parse(DateTime.DaysInMonth(Int32.Parse(year), 6) + "/06/" + year);
                    break;
                case 7:
                    fechaInicio = DateTime.Parse("01/07/" + year);
                    fechaFin = DateTime.Parse(DateTime.DaysInMonth(Int32.Parse(year), 7) + "/07/" + year);
                    break;
                case 8:
                    fechaInicio = DateTime.Parse("01/08/" + year);
                    fechaFin = DateTime.Parse(DateTime.DaysInMonth(Int32.Parse(year), 8) + "/08/" + year);
                    break;
                case 9:
                    fechaInicio = DateTime.Parse("01/09/" + year);
                    fechaFin = DateTime.Parse(DateTime.DaysInMonth(Int32.Parse(year), 9) + "/09/" + year);
                    break;
                case 10:
                    fechaInicio = DateTime.Parse("01/10/" + year);
                    fechaFin = DateTime.Parse(DateTime.DaysInMonth(Int32.Parse(year), 10) + "/10/" + year);
                    break;
                case 11:
                    fechaInicio = DateTime.Parse("01/11/" + year);
                    fechaFin = DateTime.Parse(DateTime.DaysInMonth(Int32.Parse(year), 11) + "/11/" + year);
                    break;
                case 12:
                    fechaInicio = DateTime.Parse("01/12/" + year);
                    fechaFin = DateTime.Parse(DateTime.DaysInMonth(Int32.Parse(year), 12) + "/12/" + year);
                    break;
                default:
                    fechaInicio = DateTime.Parse("01/01/" + year);
                    fechaFin = DateTime.Parse("31/01/" + year);
                    break;
            }

            ExcelUtilities excel = new ExcelUtilities();
            List<excelSheetDTO> Sheets = new List<excelSheetDTO>();

            excelSheetDTO sheetResumen = new excelSheetDTO();
            sheetResumen.name = "ENCUESTAS MES DEPTO";
            excelSheetDTO sheetResumen2 = new excelSheetDTO();
            sheetResumen2.name = "ENCUESTAS MES DEPTO 2";
            List<excelRowDTO> excelRowsDTOResumen = new List<excelRowDTO>();
            List<excelRowDTO> excelRowsDTOResumen2 = new List<excelRowDTO>();

            var departamentos = encuestasFactoryServices.getEncuestasService().getDepartamentos().ToList();
            var departamentosFiltro = departamentos.Where(x => x.id != 16 && x.id != 1 && x.id != 30).ToList();

            foreach (var item in departamentosFiltro)
            {
                List<EncuestaResults2DTO> listaResumen = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumenPorDepartamento(1, departamentoID, item.id, fechaInicio, fechaFin);
                sheetResumen.name = listaResumen.Select(x => x.encuesta).LastOrDefault();
                decimal mes1;
                decimal mes2;
                decimal mes3;

                #region Meses
                switch (mes)
                {
                    case 1:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                        break;
                    case 2:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.febrero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.febrero).FirstOrDefault());
                        break;
                    case 3:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.marzo).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.marzo).FirstOrDefault());
                        break;
                    case 4:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.abril).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.abril).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.abril).FirstOrDefault());
                        break;
                    case 5:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.mayo).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.mayo).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.mayo).FirstOrDefault());
                        break;
                    case 6:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.junio).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.junio).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.junio).FirstOrDefault());
                        break;
                    case 7:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.julio).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.julio).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.julio).FirstOrDefault());
                        break;
                    case 8:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.agosto).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.agosto).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.agosto).FirstOrDefault());
                        break;
                    case 9:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.septiembre).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.septiembre).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.septiembre).FirstOrDefault());
                        break;
                    case 10:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.octubre).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.octubre).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.octubre).FirstOrDefault());
                        break;
                    case 11:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.noviembre).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.noviembre).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.noviembre).FirstOrDefault());
                        break;
                    case 12:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.diciembre).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.diciembre).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.diciembre).FirstOrDefault());
                        break;
                    default:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                        break;
                }
                #endregion

                decimal[] cal = new decimal[] { mes1 };
                decimal[] tiem = new decimal[] { mes2 };
                decimal[] aten = new decimal[] { mes3 };

                var muestra = listaResumen.Select(x => x.muestra).LastOrDefault();
                var tresestrellas = listaResumen.Select(x => x.tresestrellasCont).LastOrDefault();
                var tresestrellasPerc = listaResumen.Select(x => x.tresestrellasPerc).LastOrDefault();
                var encuesta = listaResumen.Select(x => x.encuesta).LastOrDefault();

                excelRowsDTOResumen.Add(
                    new excelRowDTO
                    {
                        cells = new List<excelCellDTO>{
                                new excelCellDTO{ text = item.descripcion, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = mes1.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = mes2.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = mes3.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = muestra.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false},
                                new excelCellDTO{ text = tresestrellas.ToString()
                                    //+ " (" + tresestrellasPerc + "%)"
                                    , autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false}
                            }
                    }
            );
            }

            var departamento2 = encuestasFactoryServices.getEncuestasService().getDepartamentos().ToList();
            var departamentoFiltro2 = departamento2.Where(x => x.id == 5 || x.id == 7).ToList();

            //Empresas a las que se les enviaron encuestas para responder
            var empresas = encuestasFactoryServices.getEncuestasService().getClienteEmpresas(fechaInicio, fechaFin).Select(x => x.empresa).Distinct().ToList();

            foreach (var item in empresas)
            {
                List<EncuestaResults2DTO> listaResumen = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumenPorEmpresa(2, departamentoID, fechaInicio, fechaFin, item);
                sheetResumen2.name = listaResumen.Select(x => x.encuesta).LastOrDefault();
                decimal mes1;
                decimal mes2;
                decimal mes3;

                #region Meses
                switch (mes)
                {
                    case 1:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                        break;
                    case 2:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.febrero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.febrero).FirstOrDefault());
                        break;
                    case 3:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.marzo).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.marzo).FirstOrDefault());
                        break;
                    case 4:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.abril).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.abril).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.abril).FirstOrDefault());
                        break;
                    case 5:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.mayo).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.mayo).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.mayo).FirstOrDefault());
                        break;
                    case 6:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.junio).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.junio).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.junio).FirstOrDefault());
                        break;
                    case 7:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.julio).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.julio).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.julio).FirstOrDefault());
                        break;
                    case 8:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.agosto).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.agosto).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.agosto).FirstOrDefault());
                        break;
                    case 9:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.septiembre).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.septiembre).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.septiembre).FirstOrDefault());
                        break;
                    case 10:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.octubre).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.octubre).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.octubre).FirstOrDefault());
                        break;
                    case 11:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.noviembre).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.noviembre).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.noviembre).FirstOrDefault());
                        break;
                    case 12:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.diciembre).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.diciembre).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.diciembre).FirstOrDefault());
                        break;
                    default:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                        break;
                }
                #endregion

                var muestra = listaResumen.Select(x => x.muestra).LastOrDefault();
                var tresestrellas = listaResumen.Select(x => x.tresestrellasCont).LastOrDefault();
                var tresestrellasPerc = listaResumen.Select(x => x.tresestrellasPerc).LastOrDefault();

                decimal[] cal = new decimal[] { mes1 };
                decimal[] tiem = new decimal[] { mes2 };
                decimal[] aten = new decimal[] { mes3 };

                var calidad = (cal.Where(x => x != 0).ToList().Count > 0) ? cal.Where(x => x != 0).Average() : 0;
                var tiempo = (tiem.Where(x => x != 0).ToList().Count > 0) ? tiem.Where(x => x != 0).Average() : 0;
                var atencion = (aten.Where(x => x != 0).ToList().Count > 0) ? aten.Where(x => x != 0).Average() : 0;

                excelRowsDTOResumen2.Add(
                    new excelRowDTO
                    {
                        cells = new List<excelCellDTO>{
                                new excelCellDTO{ text = item, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = mes1.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = mes2.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = mes3.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = muestra.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false},
                                new excelCellDTO{ text = tresestrellas.ToString()
                                    //+ " (" + tresestrellasPerc + "%)"
                                    , autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false}
                            }
                    }
            );
            }

            var dataMuestraOrdenada = excelRowsDTOResumen.OrderByDescending(x => Int32.Parse(x.cells.ElementAtOrDefault(4).text)).ToList();
            var dataMuestraOrdenada2 = excelRowsDTOResumen2.OrderByDescending(x => Int32.Parse(x.cells.ElementAtOrDefault(4).text)).ToList();

            sheetResumen.Sheet = dataMuestraOrdenada;
            sheetResumen2.Sheet = dataMuestraOrdenada2;
            Sheets.Add(sheetResumen);
            Sheets.Add(sheetResumen2);

            var encuestas = encuestasFactoryServices.getEncuestasService().getEncuestasTodosDepto().ToList();

            var encuestasInternas = encuestas.Where(x => x.tipo == 1 && x.departamentoID == departamentoID).ToList();
            var encuestasExternas = encuestas.Where(x => x.tipo == 2 && x.departamentoID == departamentoID).ToList();
            var encuestasOtros = encuestas.Where(x => x.tipo == 0 && x.departamentoID == departamentoID).ToList();

            var hojasOtros = encuestasOtros.Count();

            foreach (var encuestaOtro in encuestasOtros)
            {
                excelSheetDTO sheetResumenOtro = new excelSheetDTO();
                sheetResumenOtro.name = "ENCUESTAS MES DEPTO OTRO ";
                List<excelRowDTO> excelRowsDTOResumenOtro = new List<excelRowDTO>();

                var departamentosOtros = encuestasFactoryServices.getEncuestasService().getDepartamentos().ToList();
                var departamentosOtrosFiltro = departamentosOtros.Where(x => x.id != 16 && x.id != 1 && x.id != 30).ToList();

                foreach (var item in departamentosOtrosFiltro)
                {
                    List<EncuestaResults2DTO> listaResumen = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumenPorDepartamento(0, departamentoID, item.id, fechaInicio, fechaFin);
                    sheetResumenOtro.name = listaResumen.Select(x => x.encuesta).LastOrDefault();
                    decimal mes1;
                    decimal mes2;
                    decimal mes3;

                    #region Meses
                    switch (mes)
                    {
                        case 1:
                            mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                            mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                            mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                            break;
                        case 2:
                            mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                            mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.febrero).FirstOrDefault());
                            mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.febrero).FirstOrDefault());
                            break;
                        case 3:
                            mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                            mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.marzo).FirstOrDefault());
                            mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.marzo).FirstOrDefault());
                            break;
                        case 4:
                            mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.abril).FirstOrDefault());
                            mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.abril).FirstOrDefault());
                            mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.abril).FirstOrDefault());
                            break;
                        case 5:
                            mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.mayo).FirstOrDefault());
                            mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.mayo).FirstOrDefault());
                            mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.mayo).FirstOrDefault());
                            break;
                        case 6:
                            mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.junio).FirstOrDefault());
                            mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.junio).FirstOrDefault());
                            mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.junio).FirstOrDefault());
                            break;
                        case 7:
                            mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.julio).FirstOrDefault());
                            mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.julio).FirstOrDefault());
                            mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.julio).FirstOrDefault());
                            break;
                        case 8:
                            mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.agosto).FirstOrDefault());
                            mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.agosto).FirstOrDefault());
                            mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.agosto).FirstOrDefault());
                            break;
                        case 9:
                            mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.septiembre).FirstOrDefault());
                            mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.septiembre).FirstOrDefault());
                            mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.septiembre).FirstOrDefault());
                            break;
                        case 10:
                            mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.octubre).FirstOrDefault());
                            mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.octubre).FirstOrDefault());
                            mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.octubre).FirstOrDefault());
                            break;
                        case 11:
                            mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.noviembre).FirstOrDefault());
                            mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.noviembre).FirstOrDefault());
                            mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.noviembre).FirstOrDefault());
                            break;
                        case 12:
                            mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.diciembre).FirstOrDefault());
                            mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.diciembre).FirstOrDefault());
                            mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.diciembre).FirstOrDefault());
                            break;
                        default:
                            mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                            mes2 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                            mes3 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                            break;
                    }
                    #endregion

                    var muestra = listaResumen.Select(x => x.muestra).LastOrDefault();
                    var tresestrellas = listaResumen.Select(x => x.tresestrellasCont).LastOrDefault();
                    var tresestrellasPerc = listaResumen.Select(x => x.tresestrellasPerc).LastOrDefault();

                    excelRowsDTOResumenOtro.Add(
                        new excelRowDTO
                        {
                            cells = new List<excelCellDTO>{
                                new excelCellDTO{ text = item.descripcion, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = mes1.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = mes2.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = mes3.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = muestra.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false},
                                new excelCellDTO{ text = tresestrellas.ToString()
                                    //+ " (" + tresestrellasPerc + "%)"
                                    , autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false}
                            }
                        }
                );
                }
                var dataMuestraOrdenadaOtro = excelRowsDTOResumenOtro.OrderByDescending(x => Int32.Parse(x.cells.ElementAtOrDefault(4).text)).ToList();
                sheetResumenOtro.Sheet = dataMuestraOrdenadaOtro;
                Sheets.Add(sheetResumenOtro);
            }
            var ruta = archivofs.getArchivo().getUrlDelServidor(13);
            excel.FillExcelFileEncuestaIndividualMes(this, Sheets, "ENCUESTAS RESUMEN MES DEPTO", mes, year, departamento, ruta);
            return null;
        }

        public ActionResult getEncuestaIndividualTri()
        {
            var departamentoID = Int32.Parse(Request.QueryString["departamentoID"]);
            var departamento = Request.QueryString["departamento"];

            var tri = Int32.Parse(Request.QueryString["trimestre"]);
            var year = Request.QueryString["year"];

            var fechaInicio = DateTime.Parse("01/01/" + year);
            var fechaFin = DateTime.Parse("31/03/" + year);

            switch (tri)
            {
                case 1:
                    fechaInicio = DateTime.Parse("01/01/" + year);
                    fechaFin = DateTime.Parse("31/03/" + year);
                    break;
                case 2:
                    fechaInicio = DateTime.Parse("01/04/" + year);
                    fechaFin = DateTime.Parse("30/06/" + year);
                    break;
                case 3:
                    fechaInicio = DateTime.Parse("01/07/" + year);
                    fechaFin = DateTime.Parse("30/09/" + year);
                    break;
                case 4:
                    fechaInicio = DateTime.Parse("01/10/" + year);
                    fechaFin = DateTime.Parse("31/12/" + year);
                    break;
                default:
                    fechaInicio = DateTime.Parse("01/01/" + year);
                    fechaFin = DateTime.Parse("31/03/" + year);
                    break;
            }


            ExcelUtilities excel = new ExcelUtilities();
            List<excelSheetDTO> Sheets = new List<excelSheetDTO>();

            excelSheetDTO sheetResumen = new excelSheetDTO();
            sheetResumen.name = "ENCUESTA TRI DEPTO";
            excelSheetDTO sheetResumen2 = new excelSheetDTO();
            sheetResumen2.name = "ENCUESTA TRI DEPTO 2";
            List<excelRowDTO> excelRowsDTOResumen = new List<excelRowDTO>();
            List<excelRowDTO> excelRowsDTOResumen2 = new List<excelRowDTO>();

            var departamentos = encuestasFactoryServices.getEncuestasService().getDepartamentos().ToList();
            var departamentosFiltro = departamentos.Where(x => x.id != 16 && x.id != 1 && x.id != 30).ToList();

            foreach (var item in departamentosFiltro)
            {
                List<EncuestaResults2DTO> listaResumen = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumenPorDepartamento(1, departamentoID, item.id, fechaInicio, fechaFin);
                sheetResumen.name = listaResumen.Select(x => x.encuesta).LastOrDefault();
                decimal mes1;
                decimal mes2;
                decimal mes3;
                decimal mes4;
                decimal mes5;
                decimal mes6;
                decimal mes7;
                decimal mes8;
                decimal mes9;

                #region Meses
                switch (tri)
                {
                    case 1:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.febrero).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.marzo).FirstOrDefault());
                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.febrero).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.marzo).FirstOrDefault());
                        break;
                    case 2:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.abril).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.mayo).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.junio).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.abril).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.mayo).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.junio).FirstOrDefault());
                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.abril).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.mayo).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.junio).FirstOrDefault());
                        break;
                    case 3:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.julio).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.agosto).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.septiembre).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.julio).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.agosto).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.septiembre).FirstOrDefault());
                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.julio).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.agosto).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.septiembre).FirstOrDefault());
                        break;
                    case 4:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.octubre).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.noviembre).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.diciembre).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.octubre).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.noviembre).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.diciembre).FirstOrDefault());
                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.octubre).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.noviembre).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.diciembre).FirstOrDefault());
                        break;
                    default:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.febrero).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.marzo).FirstOrDefault());
                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.febrero).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.marzo).FirstOrDefault());
                        break;
                }
                #endregion

                decimal[] cal = new decimal[] { mes1, mes2, mes3 };
                decimal[] tiem = new decimal[] { mes4, mes5, mes6 };
                decimal[] aten = new decimal[] { mes7, mes8, mes9 };

                var muestra = listaResumen.Select(x => x.muestra).LastOrDefault();
                var tresestrellas = listaResumen.Select(x => x.tresestrellasCont).LastOrDefault();
                var tresestrellasPerc = listaResumen.Select(x => x.tresestrellasPerc).LastOrDefault();

                var calidad = (cal.Where(x => x != 0).ToList().Count > 0) ? cal.Where(x => x != 0).Average() : 0;
                var tiempo = (tiem.Where(x => x != 0).ToList().Count > 0) ? tiem.Where(x => x != 0).Average() : 0;
                var atencion = (aten.Where(x => x != 0).ToList().Count > 0) ? aten.Where(x => x != 0).Average() : 0;

                excelRowsDTOResumen.Add(
                    new excelRowDTO
                    {
                        cells = new List<excelCellDTO>{
                                new excelCellDTO{ text = item.descripcion, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = calidad.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = tiempo.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = atencion.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = mes1.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes2.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes3.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes4.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes5.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes6.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes7.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes8.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes9.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = muestra.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false},
                                new excelCellDTO{ text = tresestrellas.ToString() + " (" + tresestrellasPerc + "%)", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false}
                            }
                    }
            );
            }

            var departamento2 = encuestasFactoryServices.getEncuestasService().getDepartamentos().ToList();
            var departamentoFiltro2 = departamento2.Where(x => x.id == 5 || x.id == 7).ToList();

            //Empresas a las que se les enviaron encuestas para responder
            var empresas = encuestasFactoryServices.getEncuestasService().getClienteEmpresas(fechaInicio, fechaFin).Select(x => x.empresa).Distinct().ToList();

            foreach (var item in empresas)
            {
                List<EncuestaResults2DTO> listaResumen = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumenPorEmpresa(2, departamentoID, fechaInicio, fechaFin, item);
                sheetResumen2.name = listaResumen.Select(x => x.encuesta).LastOrDefault();
                decimal mes1;
                decimal mes2;
                decimal mes3;
                decimal mes4;
                decimal mes5;
                decimal mes6;
                decimal mes7;
                decimal mes8;
                decimal mes9;

                #region Meses
                switch (tri)
                {
                    case 1:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.febrero).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.marzo).FirstOrDefault());
                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.febrero).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.marzo).FirstOrDefault());
                        break;
                    case 2:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.abril).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.mayo).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.junio).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.abril).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.mayo).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.junio).FirstOrDefault());
                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.abril).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.mayo).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.junio).FirstOrDefault());
                        break;
                    case 3:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.julio).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.agosto).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.septiembre).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.julio).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.agosto).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.septiembre).FirstOrDefault());
                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.julio).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.agosto).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.septiembre).FirstOrDefault());
                        break;
                    case 4:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.octubre).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.noviembre).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.diciembre).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.octubre).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.noviembre).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.diciembre).FirstOrDefault());
                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.octubre).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.noviembre).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.diciembre).FirstOrDefault());
                        break;
                    default:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.febrero).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.marzo).FirstOrDefault());
                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.febrero).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.marzo).FirstOrDefault());
                        break;
                }
                #endregion

                var muestra = listaResumen.Select(x => x.muestra).LastOrDefault();
                var tresestrellas = listaResumen.Select(x => x.tresestrellasCont).LastOrDefault();
                var tresestrellasPerc = listaResumen.Select(x => x.tresestrellasPerc).LastOrDefault();

                decimal[] cal = new decimal[] { mes1, mes2, mes3 };
                decimal[] tiem = new decimal[] { mes4, mes5, mes6 };
                decimal[] aten = new decimal[] { mes7, mes8, mes9 };

                var calidad = (cal.Where(x => x != 0).ToList().Count > 0) ? cal.Where(x => x != 0).Average() : 0;
                var tiempo = (tiem.Where(x => x != 0).ToList().Count > 0) ? tiem.Where(x => x != 0).Average() : 0;
                var atencion = (aten.Where(x => x != 0).ToList().Count > 0) ? aten.Where(x => x != 0).Average() : 0;

                excelRowsDTOResumen2.Add(
                    new excelRowDTO
                    {
                        cells = new List<excelCellDTO>{
                                new excelCellDTO{ text = item, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = calidad.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = tiempo.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = atencion.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = mes1.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes2.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes3.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes4.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes5.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes6.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes7.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes8.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes9.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = muestra.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false},
                                new excelCellDTO{ text = tresestrellas.ToString() + " (" + tresestrellasPerc + "%)", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false}
                            }
                    }
            );
            }

            var dataMuestraOrdenada = excelRowsDTOResumen.OrderByDescending(x => Int32.Parse(x.cells.ElementAtOrDefault(13).text)).ToList();
            var dataMuestraOrdenada2 = excelRowsDTOResumen2.OrderByDescending(x => Int32.Parse(x.cells.ElementAtOrDefault(13).text)).ToList();

            sheetResumen.Sheet = dataMuestraOrdenada;
            sheetResumen2.Sheet = dataMuestraOrdenada2;
            Sheets.Add(sheetResumen);
            Sheets.Add(sheetResumen2);

            var encuestas = encuestasFactoryServices.getEncuestasService().getEncuestasTodosDepto().ToList();

            var encuestasInternas = encuestas.Where(x => x.tipo == 1 && x.departamentoID == departamentoID).ToList();
            var encuestasExternas = encuestas.Where(x => x.tipo == 2 && x.departamentoID == departamentoID).ToList();
            var encuestasOtros = encuestas.Where(x => x.tipo == 0 && x.departamentoID == departamentoID).ToList();

            var hojasOtros = encuestasOtros.Count();

            foreach (var encuestaOtro in encuestasOtros)
            {
                excelSheetDTO sheetResumenOtro = new excelSheetDTO();
                sheetResumenOtro.name = "ENCUESTAS TRI DEPTO OTRO ";
                List<excelRowDTO> excelRowsDTOResumenOtro = new List<excelRowDTO>();

                var departamentosOtros = encuestasFactoryServices.getEncuestasService().getDepartamentos().ToList();
                var departamentosOtrosFiltro = departamentosOtros.Where(x => x.id != 16 && x.id != 1 && x.id != 30).ToList();

                foreach (var item in departamentosOtrosFiltro)
                {
                    List<EncuestaResults2DTO> listaResumen = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumenPorDepartamento(0, departamentoID, item.id, fechaInicio, fechaFin);
                    sheetResumenOtro.name = listaResumen.Select(x => x.encuesta).LastOrDefault();
                    decimal mes1;
                    decimal mes2;
                    decimal mes3;
                    decimal mes4;
                    decimal mes5;
                    decimal mes6;
                    decimal mes7;
                    decimal mes8;
                    decimal mes9;

                    #region Meses
                    switch (tri)
                    {
                        case 1:
                            mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                            mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                            mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                            mes4 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                            mes5 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.febrero).FirstOrDefault());
                            mes6 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.marzo).FirstOrDefault());
                            mes7 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                            mes8 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.febrero).FirstOrDefault());
                            mes9 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.marzo).FirstOrDefault());
                            break;
                        case 2:
                            mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.abril).FirstOrDefault());
                            mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.mayo).FirstOrDefault());
                            mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.junio).FirstOrDefault());
                            mes4 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.abril).FirstOrDefault());
                            mes5 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.mayo).FirstOrDefault());
                            mes6 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.junio).FirstOrDefault());
                            mes7 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.abril).FirstOrDefault());
                            mes8 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.mayo).FirstOrDefault());
                            mes9 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.junio).FirstOrDefault());
                            break;
                        case 3:
                            mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.julio).FirstOrDefault());
                            mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.agosto).FirstOrDefault());
                            mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.septiembre).FirstOrDefault());
                            mes4 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.julio).FirstOrDefault());
                            mes5 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.agosto).FirstOrDefault());
                            mes6 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.septiembre).FirstOrDefault());
                            mes7 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.julio).FirstOrDefault());
                            mes8 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.agosto).FirstOrDefault());
                            mes9 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.septiembre).FirstOrDefault());
                            break;
                        case 4:
                            mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.octubre).FirstOrDefault());
                            mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.noviembre).FirstOrDefault());
                            mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.diciembre).FirstOrDefault());
                            mes4 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.octubre).FirstOrDefault());
                            mes5 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.noviembre).FirstOrDefault());
                            mes6 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.diciembre).FirstOrDefault());
                            mes7 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.octubre).FirstOrDefault());
                            mes8 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.noviembre).FirstOrDefault());
                            mes9 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.diciembre).FirstOrDefault());
                            break;
                        default:
                            mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                            mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                            mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                            mes4 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                            mes5 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.febrero).FirstOrDefault());
                            mes6 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.marzo).FirstOrDefault());
                            mes7 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                            mes8 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.febrero).FirstOrDefault());
                            mes9 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.marzo).FirstOrDefault());
                            break;
                    }
                    #endregion

                    decimal[] cal = new decimal[] { mes1, mes2, mes3 };
                    decimal[] tiem = new decimal[] { mes4, mes5, mes6 };
                    decimal[] aten = new decimal[] { mes7, mes8, mes9 };

                    var muestra = listaResumen.Select(x => x.muestra).LastOrDefault();
                    var tresestrellas = listaResumen.Select(x => x.tresestrellasCont).LastOrDefault();
                    var tresestrellasPerc = listaResumen.Select(x => x.tresestrellasPerc).LastOrDefault();

                    var calidad = (cal.Where(x => x != 0).ToList().Count > 0) ? cal.Where(x => x != 0).Average() : 0;
                    var tiempo = (tiem.Where(x => x != 0).ToList().Count > 0) ? tiem.Where(x => x != 0).Average() : 0;
                    var atencion = (aten.Where(x => x != 0).ToList().Count > 0) ? aten.Where(x => x != 0).Average() : 0;

                    excelRowsDTOResumenOtro.Add(
                        new excelRowDTO
                        {
                            cells = new List<excelCellDTO>{
                                new excelCellDTO{ text = item.descripcion, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = calidad.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = tiempo.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = atencion.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = mes1.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes2.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes3.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes4.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes5.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes6.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes7.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes8.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes9.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = muestra.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false},
                                new excelCellDTO{ text = tresestrellas.ToString() + " (" + tresestrellasPerc + "%)", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false}
                            }
                        }
                );
                }
                var dataMuestraOrdenadaOtro = excelRowsDTOResumenOtro.OrderByDescending(x => Int32.Parse(x.cells.ElementAtOrDefault(13).text)).ToList();
                sheetResumenOtro.Sheet = dataMuestraOrdenadaOtro;
                Sheets.Add(sheetResumenOtro);
            }
            var ruta = archivofs.getArchivo().getUrlDelServidor(13);
            excel.FillExcelFileEncuestaIndividualTri(this, Sheets, "ENCUESTA RESUMEN TRI DEPTO", tri, year, departamento, ruta);
            return null;
        }

        public ActionResult getEncuestaIndividualSem()
        {
            var departamentoID = Int32.Parse(Request.QueryString["departamentoID"]);
            var departamento = Request.QueryString["departamento"];

            var sem = Int32.Parse(Request.QueryString["sem"]);
            var year = Request.QueryString["year"];

            var fechaInicio = DateTime.Parse("01/01/" + year);
            var fechaFin = DateTime.Parse("30/06/" + year);

            switch (sem)
            {
                case 1:
                    fechaInicio = DateTime.Parse("01/01/" + year);
                    fechaFin = DateTime.Parse("30/06/" + year);
                    break;
                case 2:
                    fechaInicio = DateTime.Parse("01/07/" + year);
                    fechaFin = DateTime.Parse("31/12/" + year);
                    break;
                default:
                    fechaInicio = DateTime.Parse("01/01/" + year);
                    fechaFin = DateTime.Parse("30/06/" + year);
                    break;
            }


            ExcelUtilities excel = new ExcelUtilities();
            List<excelSheetDTO> Sheets = new List<excelSheetDTO>();

            excelSheetDTO sheetResumen = new excelSheetDTO();
            sheetResumen.name = "ENCUESTAS SEM DEPTO";
            excelSheetDTO sheetResumen2 = new excelSheetDTO();
            sheetResumen2.name = "ENCUESTAS SEM DEPTO 2";
            List<excelRowDTO> excelRowsDTOResumen = new List<excelRowDTO>();
            List<excelRowDTO> excelRowsDTOResumen2 = new List<excelRowDTO>();

            var departamentos = encuestasFactoryServices.getEncuestasService().getDepartamentos().ToList();
            var departamentosFiltro = departamentos.Where(x => x.id != 16 && x.id != 1 && x.id != 30).ToList();

            foreach (var item in departamentosFiltro)
            {
                List<EncuestaResults2DTO> listaResumen = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumenPorDepartamento(1, departamentoID, item.id, fechaInicio, fechaFin);
                sheetResumen.name = listaResumen.Select(x => x.encuesta).LastOrDefault();
                decimal mes1;
                decimal mes2;
                decimal mes3;
                decimal mes4;
                decimal mes5;
                decimal mes6;
                decimal mes7;
                decimal mes8;
                decimal mes9;
                decimal mes10;
                decimal mes11;
                decimal mes12;
                decimal mes13;
                decimal mes14;
                decimal mes15;
                decimal mes16;
                decimal mes17;
                decimal mes18;

                #region Meses
                switch (sem)
                {
                    case 1:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.abril).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.mayo).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.junio).FirstOrDefault());

                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.febrero).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.marzo).FirstOrDefault());
                        mes10 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.abril).FirstOrDefault());
                        mes11 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.mayo).FirstOrDefault());
                        mes12 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.junio).FirstOrDefault());

                        mes13 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                        mes14 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.febrero).FirstOrDefault());
                        mes15 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.marzo).FirstOrDefault());
                        mes16 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.abril).FirstOrDefault());
                        mes17 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.mayo).FirstOrDefault());
                        mes18 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.junio).FirstOrDefault());

                        break;
                    case 2:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.julio).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.agosto).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.septiembre).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.octubre).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.noviembre).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.diciembre).FirstOrDefault());

                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.julio).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.agosto).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.septiembre).FirstOrDefault());
                        mes10 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.octubre).FirstOrDefault());
                        mes11 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.noviembre).FirstOrDefault());
                        mes12 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.diciembre).FirstOrDefault());

                        mes13 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.julio).FirstOrDefault());
                        mes14 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.agosto).FirstOrDefault());
                        mes15 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.septiembre).FirstOrDefault());
                        mes16 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.octubre).FirstOrDefault());
                        mes17 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.noviembre).FirstOrDefault());
                        mes18 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.diciembre).FirstOrDefault());
                        break;
                    default:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.abril).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.mayo).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.junio).FirstOrDefault());

                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.febrero).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.marzo).FirstOrDefault());
                        mes10 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.abril).FirstOrDefault());
                        mes11 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.mayo).FirstOrDefault());
                        mes12 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.junio).FirstOrDefault());

                        mes13 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                        mes14 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.febrero).FirstOrDefault());
                        mes15 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.marzo).FirstOrDefault());
                        mes16 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.abril).FirstOrDefault());
                        mes17 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.mayo).FirstOrDefault());
                        mes18 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.junio).FirstOrDefault());
                        break;
                }
                #endregion

                decimal[] cal = new decimal[] { mes1, mes2, mes3, mes4, mes5, mes6 };
                decimal[] tiem = new decimal[] { mes7, mes8, mes9, mes10, mes11, mes12 };
                decimal[] aten = new decimal[] { mes13, mes14, mes15, mes16, mes17, mes18 };

                var muestra = listaResumen.Select(x => x.muestra).LastOrDefault();
                var tresestrellas = listaResumen.Select(x => x.tresestrellasCont).LastOrDefault();
                var tresestrellasPerc = listaResumen.Select(x => x.tresestrellasPerc).LastOrDefault();

                var calidad = (cal.Where(x => x != 0).ToList().Count > 0) ? cal.Where(x => x != 0).Average() : 0;
                var tiempo = (tiem.Where(x => x != 0).ToList().Count > 0) ? tiem.Where(x => x != 0).Average() : 0;
                var atencion = (aten.Where(x => x != 0).ToList().Count > 0) ? aten.Where(x => x != 0).Average() : 0;

                excelRowsDTOResumen.Add(
                    new excelRowDTO
                    {
                        cells = new List<excelCellDTO>{
                                new excelCellDTO{ text = item.descripcion, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = calidad.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = tiempo.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = atencion.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = mes1.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes2.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes3.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes4.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes5.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes6.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes7.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes8.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes9.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes10.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes11.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes12.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes13.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes14.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes15.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes16.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes17.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes18.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = muestra.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false},
                                new excelCellDTO{ text = tresestrellas.ToString() + " (" + tresestrellasPerc + "%)", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false}
                            }
                    }
            );
            }

            var departamento2 = encuestasFactoryServices.getEncuestasService().getDepartamentos().ToList();
            var departamentoFiltro2 = departamento2.Where(x => x.id == 5 || x.id == 7).ToList();

            //Empresas a las que se les enviaron encuestas para responder
            var empresas = encuestasFactoryServices.getEncuestasService().getClienteEmpresas(fechaInicio, fechaFin).Select(x => x.empresa).Distinct().ToList();

            foreach (var item in empresas)
            {
                List<EncuestaResults2DTO> listaResumen = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumenPorEmpresa(2, departamentoID, fechaInicio, fechaFin, item);
                sheetResumen2.name = listaResumen.Select(x => x.encuesta).LastOrDefault();
                decimal mes1;
                decimal mes2;
                decimal mes3;
                decimal mes4;
                decimal mes5;
                decimal mes6;
                decimal mes7;
                decimal mes8;
                decimal mes9;
                decimal mes10;
                decimal mes11;
                decimal mes12;
                decimal mes13;
                decimal mes14;
                decimal mes15;
                decimal mes16;
                decimal mes17;
                decimal mes18;

                #region Meses
                switch (sem)
                {
                    case 1:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.abril).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.mayo).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.junio).FirstOrDefault());

                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.febrero).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.marzo).FirstOrDefault());
                        mes10 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.abril).FirstOrDefault());
                        mes11 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.mayo).FirstOrDefault());
                        mes12 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.junio).FirstOrDefault());

                        mes13 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                        mes14 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.febrero).FirstOrDefault());
                        mes15 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.marzo).FirstOrDefault());
                        mes16 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.abril).FirstOrDefault());
                        mes17 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.mayo).FirstOrDefault());
                        mes18 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.junio).FirstOrDefault());

                        break;
                    case 2:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.julio).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.agosto).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.septiembre).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.octubre).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.noviembre).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.diciembre).FirstOrDefault());

                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.julio).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.agosto).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.septiembre).FirstOrDefault());
                        mes10 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.octubre).FirstOrDefault());
                        mes11 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.noviembre).FirstOrDefault());
                        mes12 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.diciembre).FirstOrDefault());

                        mes13 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.julio).FirstOrDefault());
                        mes14 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.agosto).FirstOrDefault());
                        mes15 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.septiembre).FirstOrDefault());
                        mes16 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.octubre).FirstOrDefault());
                        mes17 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.noviembre).FirstOrDefault());
                        mes18 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.diciembre).FirstOrDefault());
                        break;
                    default:
                        mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                        mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                        mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                        mes4 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.abril).FirstOrDefault());
                        mes5 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.mayo).FirstOrDefault());
                        mes6 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.junio).FirstOrDefault());

                        mes7 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                        mes8 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.febrero).FirstOrDefault());
                        mes9 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.marzo).FirstOrDefault());
                        mes10 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.abril).FirstOrDefault());
                        mes11 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.mayo).FirstOrDefault());
                        mes12 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.junio).FirstOrDefault());

                        mes13 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                        mes14 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.febrero).FirstOrDefault());
                        mes15 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.marzo).FirstOrDefault());
                        mes16 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.abril).FirstOrDefault());
                        mes17 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.mayo).FirstOrDefault());
                        mes18 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.junio).FirstOrDefault());
                        break;
                }
                #endregion

                var muestra = listaResumen.Select(x => x.muestra).LastOrDefault();
                var tresestrellas = listaResumen.Select(x => x.tresestrellasCont).LastOrDefault();
                var tresestrellasPerc = listaResumen.Select(x => x.tresestrellasPerc).LastOrDefault();

                decimal[] cal = new decimal[] { mes1, mes2, mes3, mes4, mes5, mes6 };
                decimal[] tiem = new decimal[] { mes7, mes8, mes9, mes10, mes11, mes12 };
                decimal[] aten = new decimal[] { mes13, mes14, mes15, mes16, mes17, mes18 };

                var calidad = (cal.Where(x => x != 0).ToList().Count > 0) ? cal.Where(x => x != 0).Average() : 0;
                var tiempo = (tiem.Where(x => x != 0).ToList().Count > 0) ? tiem.Where(x => x != 0).Average() : 0;
                var atencion = (aten.Where(x => x != 0).ToList().Count > 0) ? aten.Where(x => x != 0).Average() : 0;

                excelRowsDTOResumen2.Add(
                    new excelRowDTO
                    {
                        cells = new List<excelCellDTO>{
                                new excelCellDTO{ text = item, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = calidad.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = tiempo.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = atencion.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = mes1.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes2.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes3.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes4.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes5.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes6.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes7.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes8.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes9.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes10.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes11.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes12.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes13.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes14.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes15.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes16.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes17.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes18.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = muestra.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false},
                                new excelCellDTO{ text = tresestrellas.ToString() + " (" + tresestrellasPerc + "%)", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false}
                            }
                    }
            );
            }

            var dataMuestraOrdenada = excelRowsDTOResumen.OrderByDescending(x => Int32.Parse(x.cells.ElementAtOrDefault(22).text)).ToList();
            var dataMuestraOrdenada2 = excelRowsDTOResumen2.OrderByDescending(x => Int32.Parse(x.cells.ElementAtOrDefault(22).text)).ToList();

            sheetResumen.Sheet = dataMuestraOrdenada;
            sheetResumen2.Sheet = dataMuestraOrdenada2;
            Sheets.Add(sheetResumen);
            Sheets.Add(sheetResumen2);

            var encuestas = encuestasFactoryServices.getEncuestasService().getEncuestasTodosDepto().ToList();

            var encuestasInternas = encuestas.Where(x => x.tipo == 1 && x.departamentoID == departamentoID).ToList();
            var encuestasExternas = encuestas.Where(x => x.tipo == 2 && x.departamentoID == departamentoID).ToList();
            var encuestasOtros = encuestas.Where(x => x.tipo == 0 && x.departamentoID == departamentoID).ToList();

            var hojasOtros = encuestasOtros.Count();

            foreach (var encuestaOtro in encuestasOtros)
            {
                excelSheetDTO sheetResumenOtro = new excelSheetDTO();
                sheetResumenOtro.name = "ENCUESTAS SEM DEPTO OTRO ";
                List<excelRowDTO> excelRowsDTOResumenOtro = new List<excelRowDTO>();

                var departamentosOtros = encuestasFactoryServices.getEncuestasService().getDepartamentos().ToList();
                var departamentosOtrosFiltro = departamentosOtros.Where(x => x.id != 16 && x.id != 1 && x.id != 30).ToList();

                foreach (var item in departamentosOtrosFiltro)
                {
                    List<EncuestaResults2DTO> listaResumen = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumenPorDepartamento(0, departamentoID, item.id, fechaInicio, fechaFin);
                    sheetResumenOtro.name = listaResumen.Select(x => x.encuesta).LastOrDefault();
                    decimal mes1;
                    decimal mes2;
                    decimal mes3;
                    decimal mes4;
                    decimal mes5;
                    decimal mes6;
                    decimal mes7;
                    decimal mes8;
                    decimal mes9;
                    decimal mes10;
                    decimal mes11;
                    decimal mes12;
                    decimal mes13;
                    decimal mes14;
                    decimal mes15;
                    decimal mes16;
                    decimal mes17;
                    decimal mes18;

                    #region Meses
                    switch (sem)
                    {
                        case 1:
                            mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                            mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                            mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                            mes4 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.abril).FirstOrDefault());
                            mes5 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.mayo).FirstOrDefault());
                            mes6 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.junio).FirstOrDefault());

                            mes7 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                            mes8 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.febrero).FirstOrDefault());
                            mes9 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.marzo).FirstOrDefault());
                            mes10 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.abril).FirstOrDefault());
                            mes11 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.mayo).FirstOrDefault());
                            mes12 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.junio).FirstOrDefault());

                            mes13 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                            mes14 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.febrero).FirstOrDefault());
                            mes15 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.marzo).FirstOrDefault());
                            mes16 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.abril).FirstOrDefault());
                            mes17 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.mayo).FirstOrDefault());
                            mes18 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.junio).FirstOrDefault());

                            break;
                        case 2:
                            mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.julio).FirstOrDefault());
                            mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.agosto).FirstOrDefault());
                            mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.septiembre).FirstOrDefault());
                            mes4 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.octubre).FirstOrDefault());
                            mes5 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.noviembre).FirstOrDefault());
                            mes6 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.diciembre).FirstOrDefault());

                            mes7 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.julio).FirstOrDefault());
                            mes8 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.agosto).FirstOrDefault());
                            mes9 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.septiembre).FirstOrDefault());
                            mes10 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.octubre).FirstOrDefault());
                            mes11 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.noviembre).FirstOrDefault());
                            mes12 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.diciembre).FirstOrDefault());

                            mes13 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.julio).FirstOrDefault());
                            mes14 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.agosto).FirstOrDefault());
                            mes15 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.septiembre).FirstOrDefault());
                            mes16 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.octubre).FirstOrDefault());
                            mes17 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.noviembre).FirstOrDefault());
                            mes18 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.diciembre).FirstOrDefault());
                            break;
                        default:
                            mes1 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.enero).FirstOrDefault());
                            mes2 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.febrero).FirstOrDefault());
                            mes3 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.marzo).FirstOrDefault());
                            mes4 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.abril).FirstOrDefault());
                            mes5 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.mayo).FirstOrDefault());
                            mes6 = Math.Round(listaResumen.Where(x => x.tipo == 2).Select(y => y.junio).FirstOrDefault());

                            mes7 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.enero).FirstOrDefault());
                            mes8 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.febrero).FirstOrDefault());
                            mes9 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.marzo).FirstOrDefault());
                            mes10 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.abril).FirstOrDefault());
                            mes11 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.mayo).FirstOrDefault());
                            mes12 = Math.Round(listaResumen.Where(x => x.tipo == 1).Select(y => y.junio).FirstOrDefault());

                            mes13 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.enero).FirstOrDefault());
                            mes14 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.febrero).FirstOrDefault());
                            mes15 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.marzo).FirstOrDefault());
                            mes16 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.abril).FirstOrDefault());
                            mes17 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.mayo).FirstOrDefault());
                            mes18 = Math.Round(listaResumen.Where(x => x.tipo == 3).Select(y => y.junio).FirstOrDefault());
                            break;
                    }
                    #endregion

                    var muestra = listaResumen.Select(x => x.muestra).LastOrDefault();
                    var tresestrellas = listaResumen.Select(x => x.tresestrellasCont).LastOrDefault();
                    var tresestrellasPerc = listaResumen.Select(x => x.tresestrellasPerc).LastOrDefault();

                    decimal[] cal = new decimal[] { mes1, mes2, mes3, mes4, mes5, mes6 };
                    decimal[] tiem = new decimal[] { mes7, mes8, mes9, mes10, mes11, mes12 };
                    decimal[] aten = new decimal[] { mes13, mes14, mes15, mes16, mes17, mes18 };

                    var calidad = (cal.Where(x => x != 0).ToList().Count > 0) ? cal.Where(x => x != 0).Average() : 0;
                    var tiempo = (tiem.Where(x => x != 0).ToList().Count > 0) ? tiem.Where(x => x != 0).Average() : 0;
                    var atencion = (aten.Where(x => x != 0).ToList().Count > 0) ? aten.Where(x => x != 0).Average() : 0;

                    excelRowsDTOResumenOtro.Add(
                        new excelRowDTO
                        {
                            cells = new List<excelCellDTO>{
                                new excelCellDTO{ text = item.descripcion, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = calidad.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = tiempo.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = atencion.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = mes1.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes2.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes3.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes4.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes5.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes6.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes7.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes8.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes9.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes10.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes11.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes12.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes13.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes14.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes15.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes16.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes17.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = mes18.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = muestra.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false},
                                new excelCellDTO{ text = tresestrellas.ToString() + " (" + tresestrellasPerc + "%)", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false}
                            }
                        }
                );
                }
                var dataMuestraOrdenadaOtro = excelRowsDTOResumenOtro.OrderByDescending(x => Int32.Parse(x.cells.ElementAtOrDefault(22).text)).ToList();
                sheetResumenOtro.Sheet = dataMuestraOrdenadaOtro;
                Sheets.Add(sheetResumenOtro);
            }
            var ruta = archivofs.getArchivo().getUrlDelServidor(13);
            excel.FillExcelFileEncuestaIndividualSem(this, Sheets, "ENCUESTAS RESUMEN SEM DEPTO", sem, year, departamento, ruta);
            return null;
        }

        public ActionResult getEncuestaIndividualYear()
        {
            var departamentoID = Int32.Parse(Request.QueryString["departamentoID"]);
            var departamento = Request.QueryString["departamento"];

            var year = Request.QueryString["year"];

            var fechaInicio = DateTime.Parse("01/01/" + year);
            var fechaFin = DateTime.Parse("31/12/" + year);

            ExcelUtilities excel = new ExcelUtilities();
            List<excelSheetDTO> Sheets = new List<excelSheetDTO>();

            excelSheetDTO sheetResumen = new excelSheetDTO();
            sheetResumen.name = "ENCUESTA AÑO DEPTO";
            excelSheetDTO sheetResumen2 = new excelSheetDTO();
            sheetResumen2.name = "ENCUESTA AÑO DEPTO 2";
            List<excelRowDTO> excelRowsDTOResumen = new List<excelRowDTO>();
            List<excelRowDTO> excelRowsDTOResumen2 = new List<excelRowDTO>();

            var departamentos = encuestasFactoryServices.getEncuestasService().getDepartamentos().ToList();
            var departamentosFiltro = departamentos.Where(x => x.id != 16 && x.id != 1 && x.id != 30).ToList();

            foreach (var item in departamentosFiltro)
            {
                List<EncuestaResults2DTO> listaResumen = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumenPorDepartamentoYear(1, departamentoID, item.id, fechaInicio, fechaFin);
                sheetResumen.name = listaResumen.Select(x => x.encuesta).LastOrDefault();
                decimal trim1 = listaResumen.Where(x => x.tipo == 2).Select(y => y.trim1).FirstOrDefault();
                decimal trim2 = listaResumen.Where(x => x.tipo == 2).Select(y => y.trim2).FirstOrDefault();
                decimal trim3 = listaResumen.Where(x => x.tipo == 2).Select(y => y.trim3).FirstOrDefault();
                decimal trim4 = listaResumen.Where(x => x.tipo == 2).Select(y => y.trim4).FirstOrDefault();
                decimal trim5 = listaResumen.Where(x => x.tipo == 1).Select(y => y.trim1).FirstOrDefault();
                decimal trim6 = listaResumen.Where(x => x.tipo == 1).Select(y => y.trim2).FirstOrDefault();
                decimal trim7 = listaResumen.Where(x => x.tipo == 1).Select(y => y.trim3).FirstOrDefault();
                decimal trim8 = listaResumen.Where(x => x.tipo == 1).Select(y => y.trim4).FirstOrDefault();
                decimal trim9 = listaResumen.Where(x => x.tipo == 3).Select(y => y.trim1).FirstOrDefault();
                decimal trim10 = listaResumen.Where(x => x.tipo == 3).Select(y => y.trim2).FirstOrDefault();
                decimal trim11 = listaResumen.Where(x => x.tipo == 3).Select(y => y.trim3).FirstOrDefault();
                decimal trim12 = listaResumen.Where(x => x.tipo == 3).Select(y => y.trim4).FirstOrDefault();


                decimal[] cal = new decimal[] { trim1, trim2, trim3, trim4 };
                decimal[] tiem = new decimal[] { trim5, trim6, trim7, trim8 };
                decimal[] aten = new decimal[] { trim9, trim10, trim11, trim12 };

                var muestra = listaResumen.Select(x => x.muestra).LastOrDefault();
                var tresestrellas = listaResumen.Select(x => x.tresestrellasCont).LastOrDefault();
                var tresestrellasPerc = listaResumen.Select(x => x.tresestrellasPerc).LastOrDefault();

                var calidad = (cal.Where(x => x != 0).ToList().Count > 0) ? cal.Where(x => x != 0).Average() : 0;
                var tiempo = (tiem.Where(x => x != 0).ToList().Count > 0) ? tiem.Where(x => x != 0).Average() : 0;
                var atencion = (aten.Where(x => x != 0).ToList().Count > 0) ? aten.Where(x => x != 0).Average() : 0;

                excelRowsDTOResumen.Add(
                    new excelRowDTO
                    {
                        cells = new List<excelCellDTO>{
                                new excelCellDTO{ text = item.descripcion, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = calidad.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = tiempo.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = atencion.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = "", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = trim1.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim2.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim3.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim4.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim5.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim6.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim7.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim8.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim9.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim10.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim11.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim12.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = muestra.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false},
                                new excelCellDTO{ text = tresestrellas.ToString()
                                    //+ " (" + tresestrellasPerc + "%)"
                                    , autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false}
                            }
                    }
            );
            }

            var departamento2 = encuestasFactoryServices.getEncuestasService().getDepartamentos().ToList();
            var departamentoFiltro2 = departamento2.Where(x => x.id == 5 || x.id == 7).ToList();

            //Empresas a las que se les enviaron encuestas para responder
            var empresas = encuestasFactoryServices.getEncuestasService().getClienteEmpresas(fechaInicio, fechaFin).Select(x => x.empresa).Distinct().ToList();

            foreach (var item in empresas)
            {
                List<EncuestaResults2DTO> listaResumen = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumenPorEmpresaYear(2, departamentoID, fechaInicio, fechaFin, item);
                sheetResumen2.name = listaResumen.Select(x => x.encuesta).LastOrDefault();
                decimal trim1 = listaResumen.Where(x => x.tipo == 2).Select(y => y.trim1).FirstOrDefault();
                decimal trim2 = listaResumen.Where(x => x.tipo == 2).Select(y => y.trim2).FirstOrDefault();
                decimal trim3 = listaResumen.Where(x => x.tipo == 2).Select(y => y.trim3).FirstOrDefault();
                decimal trim4 = listaResumen.Where(x => x.tipo == 2).Select(y => y.trim4).FirstOrDefault();
                decimal trim5 = listaResumen.Where(x => x.tipo == 1).Select(y => y.trim1).FirstOrDefault();
                decimal trim6 = listaResumen.Where(x => x.tipo == 1).Select(y => y.trim2).FirstOrDefault();
                decimal trim7 = listaResumen.Where(x => x.tipo == 1).Select(y => y.trim3).FirstOrDefault();
                decimal trim8 = listaResumen.Where(x => x.tipo == 1).Select(y => y.trim4).FirstOrDefault();
                decimal trim9 = listaResumen.Where(x => x.tipo == 3).Select(y => y.trim1).FirstOrDefault();
                decimal trim10 = listaResumen.Where(x => x.tipo == 3).Select(y => y.trim2).FirstOrDefault();
                decimal trim11 = listaResumen.Where(x => x.tipo == 3).Select(y => y.trim3).FirstOrDefault();
                decimal trim12 = listaResumen.Where(x => x.tipo == 3).Select(y => y.trim4).FirstOrDefault();


                decimal[] cal = new decimal[] { trim1, trim2, trim3, trim4 };
                decimal[] tiem = new decimal[] { trim5, trim6, trim7, trim8 };
                decimal[] aten = new decimal[] { trim9, trim10, trim11, trim12 };

                var muestra = listaResumen.Select(x => x.muestra).LastOrDefault();
                var tresestrellas = listaResumen.Select(x => x.tresestrellasCont).LastOrDefault();
                var tresestrellasPerc = listaResumen.Select(x => x.tresestrellasPerc).LastOrDefault();

                var calidad = (cal.Where(x => x != 0).ToList().Count > 0) ? cal.Where(x => x != 0).Average() : 0;
                var tiempo = (tiem.Where(x => x != 0).ToList().Count > 0) ? tiem.Where(x => x != 0).Average() : 0;
                var atencion = (aten.Where(x => x != 0).ToList().Count > 0) ? aten.Where(x => x != 0).Average() : 0;

                excelRowsDTOResumen2.Add(
                    new excelRowDTO
                    {
                        cells = new List<excelCellDTO>{
                                new excelCellDTO{ text = item, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = calidad.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = tiempo.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = atencion.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = "", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = trim1.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim2.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim3.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim4.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim5.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim6.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim7.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim8.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim9.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim10.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim11.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim12.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = muestra.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false},
                                new excelCellDTO{ text = tresestrellas.ToString()
                                    //+ " (" + tresestrellasPerc + "%)"
                                    , autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false}
                            }
                    }
            );
            }

            var dataMuestraOrdenada = excelRowsDTOResumen.OrderByDescending(x => Int32.Parse(x.cells.ElementAtOrDefault(17).text)).ToList();
            var dataMuestraOrdenada2 = excelRowsDTOResumen2.OrderByDescending(x => Int32.Parse(x.cells.ElementAtOrDefault(17).text)).ToList();

            sheetResumen.Sheet = dataMuestraOrdenada;
            sheetResumen2.Sheet = dataMuestraOrdenada2;
            Sheets.Add(sheetResumen);
            Sheets.Add(sheetResumen2);

            var encuestas = encuestasFactoryServices.getEncuestasService().getEncuestasTodosDepto().ToList();

            var encuestasInternas = encuestas.Where(x => x.tipo == 1 && x.departamentoID == departamentoID).ToList();
            var encuestasExternas = encuestas.Where(x => x.tipo == 2 && x.departamentoID == departamentoID).ToList();
            var encuestasOtros = encuestas.Where(x => x.tipo == 0 && x.departamentoID == departamentoID).ToList();

            var hojasOtros = encuestasOtros.Count();

            foreach (var encuestaOtro in encuestasOtros)
            {
                excelSheetDTO sheetResumenOtro = new excelSheetDTO();
                sheetResumenOtro.name = "ENCUESTAS AÑO DEPTO OTRO ";
                List<excelRowDTO> excelRowsDTOResumenOtro = new List<excelRowDTO>();

                var departamentosOtros = encuestasFactoryServices.getEncuestasService().getDepartamentos().ToList();
                var departamentosOtrosFiltro = departamentosOtros.Where(x => x.id != 16 && x.id != 1 && x.id != 30).ToList();

                foreach (var item in departamentosOtrosFiltro)
                {
                    List<EncuestaResults2DTO> listaResumen = encuestasFactoryServices.getEncuestasService().getEncuestaResultsResumenPorDepartamentoYear(0, departamentoID, item.id, fechaInicio, fechaFin);
                    sheetResumenOtro.name = listaResumen.Select(x => x.encuesta).LastOrDefault();
                    decimal trim1 = listaResumen.Where(x => x.tipo == 2).Select(y => y.trim1).FirstOrDefault();
                    decimal trim2 = listaResumen.Where(x => x.tipo == 2).Select(y => y.trim2).FirstOrDefault();
                    decimal trim3 = listaResumen.Where(x => x.tipo == 2).Select(y => y.trim3).FirstOrDefault();
                    decimal trim4 = listaResumen.Where(x => x.tipo == 2).Select(y => y.trim4).FirstOrDefault();
                    decimal trim5 = listaResumen.Where(x => x.tipo == 1).Select(y => y.trim1).FirstOrDefault();
                    decimal trim6 = listaResumen.Where(x => x.tipo == 1).Select(y => y.trim2).FirstOrDefault();
                    decimal trim7 = listaResumen.Where(x => x.tipo == 1).Select(y => y.trim3).FirstOrDefault();
                    decimal trim8 = listaResumen.Where(x => x.tipo == 1).Select(y => y.trim4).FirstOrDefault();
                    decimal trim9 = listaResumen.Where(x => x.tipo == 3).Select(y => y.trim1).FirstOrDefault();
                    decimal trim10 = listaResumen.Where(x => x.tipo == 3).Select(y => y.trim2).FirstOrDefault();
                    decimal trim11 = listaResumen.Where(x => x.tipo == 3).Select(y => y.trim3).FirstOrDefault();
                    decimal trim12 = listaResumen.Where(x => x.tipo == 3).Select(y => y.trim4).FirstOrDefault();


                    decimal[] cal = new decimal[] { trim1, trim2, trim3, trim4 };
                    decimal[] tiem = new decimal[] { trim5, trim6, trim7, trim8 };
                    decimal[] aten = new decimal[] { trim9, trim10, trim11, trim12 };

                    var muestra = listaResumen.Select(x => x.muestra).LastOrDefault();
                    var tresestrellas = listaResumen.Select(x => x.tresestrellasCont).LastOrDefault();
                    var tresestrellasPerc = listaResumen.Select(x => x.tresestrellasPerc).LastOrDefault();

                    var calidad = (cal.Where(x => x != 0).ToList().Count > 0) ? cal.Where(x => x != 0).Average() : 0;
                    var tiempo = (tiem.Where(x => x != 0).ToList().Count > 0) ? tiem.Where(x => x != 0).Average() : 0;
                    var atencion = (aten.Where(x => x != 0).ToList().Count > 0) ? aten.Where(x => x != 0).Average() : 0;

                    excelRowsDTOResumenOtro.Add(
                        new excelRowDTO
                        {
                            cells = new List<excelCellDTO>{
                                new excelCellDTO{ text = item.descripcion, autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true},
                                new excelCellDTO{ text = calidad.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = tiempo.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = atencion.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = "", autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=true, formatType=1},
                                new excelCellDTO{ text = trim1.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim2.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim3.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim4.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim5.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim6.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim7.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim8.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim9.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim10.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim11.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = trim12.ToString(), autoWidthFit=true, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false, formatType=1},
                                new excelCellDTO{ text = muestra.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false},
                                new excelCellDTO{ text = tresestrellas.ToString()
                                    //+ " (" + tresestrellasPerc + "%)"
                                    , autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0,textAlignLeft=false}
                            }
                        }
                );
                }
                var dataMuestraOrdenadaOtro = excelRowsDTOResumenOtro.OrderByDescending(x => Int32.Parse(x.cells.ElementAtOrDefault(17).text)).ToList();
                sheetResumenOtro.Sheet = dataMuestraOrdenadaOtro;
                Sheets.Add(sheetResumenOtro);
            }
            var ruta = archivofs.getArchivo().getUrlDelServidor(13);
            excel.FillExcelFileEncuestaIndividualYear(this, Sheets, "ENCUESTA RESUMEN AÑO DEPTO", year, departamento, ruta);
            return null;
        }

        public ActionResult getEncuestasTodasByDepto()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var id = vSesiones.sesionUsuarioDTO.departamento.id;
                var obj = encuestasFactoryServices.getEncuestasService().getEncuestasTodasByDepto(id);
                result.Add("obj", obj);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getEncuestasTodasByDeptoConEncuestaID(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (id != 0)
                {
                    var obj = encuestasFactoryServices.getEncuestasService().getEncuestasTodasByDeptoConEncuestaID(id);
                    result.Add("obj", obj);
                    result.Add(SUCCESS, true);
                }
                else
                {
                    var depID = vSesiones.sesionUsuarioDTO.departamento.id;
                    var obj = encuestasFactoryServices.getEncuestasService().getEncuestasTodasByDepto(depID);
                    result.Add("obj", obj);
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

        public ActionResult getEncuestas(string titulo, string descripcion, string departamento)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = encuestasFactoryServices.getEncuestasService().getEncuestas();
                var list = data.Where(x => x.titulo.ToUpper().Contains(titulo.ToUpper()) && x.descripcion.ToUpper().Contains(descripcion.ToUpper()) && x.departamento.ToUpper().Contains(departamento.ToUpper())).ToList();

                result.Add("data", list);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getDepartamentosList(string term)
        {
            var data = encuestasFactoryServices.getEncuestasService().getDepartamentos();

            var list = data.Where(y => y.descripcion.ToUpper().Contains(term.ToUpper())).Select(x => new
            {
                id = x.id,
                label = x.descripcion
            }).ToList().Take(10);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public void UpdateTelefonica(int encuestaID, bool telefonica)
        {
            try
            {
                encuestasFactoryServices.getEncuestasService().UpdateTelefonica(encuestaID, telefonica);
            }
            catch (Exception)
            {

            }
        }

        public void UpdateNotificacion(int encuestaID, bool notificacion)
        {
            try
            {
                encuestasFactoryServices.getEncuestasService().UpdateNotificacion(encuestaID, notificacion);
            }
            catch (Exception)
            {

            }
        }

        public void UpdatePapel(int encuestaID, bool papel)
        {
            try
            {
                encuestasFactoryServices.getEncuestasService().UpdatePapel(encuestaID, papel);
            }
            catch (Exception)
            {

            }
        }

        public ActionResult FillEmpresas()
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = encuestasFactoryServices.getEncuestasService().getEmpresas();

                var list = data.Select(x => new ComboDTO
                {
                    Value = 0,
                    Text = x
                }).ToList();

                result.Add(ITEMS, list);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillClientes(string empresa)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = encuestasFactoryServices.getEncuestasService().getClientes(empresa);

                var list = data.Select(x => new ComboDTO
                {
                    Value = x.id,
                    Text = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno
                }).OrderBy(x => x.Text).ToList();

                result.Add(ITEMS, list);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getClientes(string term)
        {
            var empresa = Request.QueryString["empresa"];

            var data = encuestasFactoryServices.getEncuestasService().getClientes(empresa);

            var termUpper = term.ToUpper().Replace(" ", "");

            var list = data.Where(y => (y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno).Replace(" ", "").ToUpper().Contains(termUpper)).Select(x => new
            {
                id = x.id,
                label = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno
            }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SubirArchivoEvidencia()
        {
            var result = new Dictionary<string, object>();
            HttpPostedFileBase file1 = Request.Files["fArchivoEvidencia"];
            string FileName = "";
            string ruta = "";
            bool pathExist = false;
            DateTime fecha = DateTime.Now;
            try
            {
                string f = fecha.ToString("ddMMyyyy") + fecha.Hour + "" + fecha.Minute;


                if (file1 != null && file1.ContentLength > 0)
                {
                    FileName = file1.FileName;
                    string ruta2 = "";
                    ruta = archivofs.getArchivo().getUrlDelServidor(13) + f + FileName;
                    pathExist = GlobalUtils.SaveArchivo(file1, ruta);
                    if (pathExist)
                    {
                        //ruta2 = ruta.Replace("C:\\", "\\REPOSITORIO\\");

                        result.Add("ruta", ruta);
                        result.Add(SUCCESS, false);
                    }

                    else
                    {
                        result.Add("ruta", "");
                        result.Add(SUCCESS, true);
                    }


                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, true);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getEncuestaTelefonica(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = encuestasFactoryServices.getEncuestasService().getEncuestaTelefonica(id);
                result.Add("obj", obj);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private string verificarRespuesta(List<tblEN_Resultado> obj)
        {
            string mensajeError = null;

            for (int r = 0; r < obj.Count; r++)
			{
                if (obj[r].calificacion < 1 || obj[r].calificacion > 5)
                {
                    mensajeError = "Favor de calificar la pregunta número: " + (r + 1);
                    break;
                }
                if (obj[r].calificacion <= 3 && string.IsNullOrEmpty(obj[r].respuesta))
                {
                    mensajeError = "Favor de especificar un motivo para la respuesta con calificación menor o igual a 3 estrellas en la pregunta número: " + (r + 1);
                    break;
                }
			}

            return mensajeError;
        }

        public ActionResult saveEncuestaTelefonica(List<tblEN_Resultado> obj, int encuestaID, string comentario, string asunto, UsuarioDTO nuevoCliente)
        {
            var result = new Dictionary<string, object>();

            try
            {
                string mensajeError = verificarRespuesta(obj);

                if (mensajeError != null)
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, mensajeError);
                    return Json(result);
                }

                if (nuevoCliente.nombre != null && nuevoCliente.empresa != null && nuevoCliente.correo != null)
                {
                    var nuevo = encuestasFactoryServices.getEncuestasService().nuevoCliente(nuevoCliente);

                    foreach (var i in obj)
                    {
                        i.usuarioRespondioID = nuevo.id;
                    }
                }

                foreach (var i in obj)
                {
                    i.fecha = DateTime.Now;
                }

                var usuarioRespondioID = obj.Select(x => x.usuarioRespondioID).FirstOrDefault();

                var nuevoEncuestaUsuario = encuestasFactoryServices.getEncuestasService().nuevoEncuestaUsuario(encuestaID, usuarioRespondioID, asunto, vSesiones.sesionUsuarioDTO.id);

                foreach (var i in obj)
                {
                    i.encuestaUsuarioID = nuevoEncuestaUsuario.id;
                }

                encuestasFactoryServices.getEncuestasService().saveEncuestaResult(obj, nuevoEncuestaUsuario.id, comentario);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult saveEncuestaPapel(List<tblEN_Resultado> obj, int encuestaID, string comentario, string asunto, UsuarioDTO nuevoCliente, string rutaArchivo)
        {
            var result = new Dictionary<string, object>();

            try
            {
                string mensajeError = verificarRespuesta(obj);

                if (mensajeError != null)
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, mensajeError);
                    return Json(result);
                }

                if (nuevoCliente.nombre != null && nuevoCliente.empresa != null && nuevoCliente.correo != null)
                {
                    var nuevo = encuestasFactoryServices.getEncuestasService().nuevoCliente(nuevoCliente);

                    foreach (var i in obj)
                    {
                        i.usuarioRespondioID = nuevo.id;
                    }
                }

                foreach (var i in obj)
                {
                    i.fecha = DateTime.Now;
                }

                var usuarioRespondioID = obj.Select(x => x.usuarioRespondioID).FirstOrDefault();

                var nuevoEncuestaUsuario = encuestasFactoryServices.getEncuestasService().nuevoEncuestaUsuarioPapel(encuestaID, usuarioRespondioID, asunto, vSesiones.sesionUsuarioDTO.id, rutaArchivo);

                foreach (var i in obj)
                {
                    i.encuestaUsuarioID = nuevoEncuestaUsuario.id;
                }

                encuestasFactoryServices.getEncuestasService().saveEncuestaResult(obj, nuevoEncuestaUsuario.id, comentario);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getUsuariosCheck(int encuestaID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = encuestasFactoryServices.getEncuestasService().getUsuariosCheck(encuestaID);

                result.Add("obj", obj);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult checkUsuario(string nombre)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = encuestasFactoryServices.getEncuestasService().checkUsuario(nombre);

                if (obj != null)
                {
                    result.Add("obj", obj);
                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add("obj", null);
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

        public ActionResult GuardarUsuariosCheck(int encuestaID, List<EncuestaCheckUsuarioDTO> usuarios)
        {
            var result = new Dictionary<string, object>();
            try
            {
                encuestasFactoryServices.getEncuestasService().GuardarUsuariosCheck(encuestaID, usuarios);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult checkEncuestaTelefonica(int encuestaID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var respuesta = encuestasFactoryServices.getEncuestasService().checkEncuestaTelefonica(encuestaID);

                result.Add("telefonica", respuesta);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getEncuestaCheck(int encuestaID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = encuestasFactoryServices.getEncuestasService().getEncuestaCheck(encuestaID);

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

        public ActionResult checkTelefonica(int encuestaID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = encuestasFactoryServices.getEncuestasService().checkTelefonica(encuestaID);

                result.Add("flag", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getUsuarios(string term)
        {
            var items = encuestasFactoryServices.getEncuestasService().getUsuarios(term);
            var filteredItems = items.Select(x => new
            {
                id = x.id,
                label = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno,
                puestoDescripcion = x.puesto.descripcion,
                correo = x.correo
            });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getEstrellas()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = encuestasFactoryServices.getEncuestasService().getEstrellas();

                result.Add("data", obj);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult setCrearEncuesta(int empID, int crearID, bool crear)
        {
            var result = new Dictionary<string, object>();
            try
            {
                encuestasFactoryServices.getEncuestasService().setCrearEncuesta(empID, crearID, crear);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
