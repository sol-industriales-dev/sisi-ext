using Core.DTO;
using Core.DTO.SeguimientoAcuerdos;
using Core.Entity.SeguimientoAcuerdos;
using Data.Factory.Principal.Usuarios;
using Data.Factory.SeguimientoAcuerdos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.SeguimientoAcuerdos
{
    public class SeguimientoAcuerdosController : BaseController
    {
        // GET: SeguimientoAcuerdos
        #region Factory
        #endregion
        private UsuarioFactoryServices usuarioFactoryServices;
        private SeguimientoAcuerdosFactoryServices seguimientoAcuerdosFactoryServices;
        private OrganigramaFactoryServices organigramaFactoryServices;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            usuarioFactoryServices = new UsuarioFactoryServices();
            seguimientoAcuerdosFactoryServices = new SeguimientoAcuerdosFactoryServices();
            organigramaFactoryServices = new OrganigramaFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult Acuerdo()
        {
            return View();
        }
        public ActionResult ReporteActividades()
        {
            return View();
        }
        public ActionResult ReporteMinutasPendientes()
        {
            return View();
        }
        public ActionResult ReporteEstadisticoMinutas()
        {
            return View();
        }
        public ActionResult BitacoraMinutas()
        {
            return View();
        }
        public ActionResult getListParticipantes(string term, int minuta)
        {
            var items = usuarioFactoryServices.getUsuarioService().ListUsersByNameAndMinute(term, minuta);
            var filteredItems = items.Select(x => new { id = x.id, label = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        public ActionResult guardarMinuta(tblSA_Minuta obj, bool nuevaVersion)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var id = 0;
                if (obj.id == 0)
                {
                    id = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().guardarMinuta(obj, nuevaVersion);
                    var p = new tblSA_Participante();
                    p.minutaID = id;
                    p.participanteID = vSesiones.sesionUsuarioDTO.id;
                    p.participante = vSesiones.sesionUsuarioDTO.nombre;
                    seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().guardarParticipante(p);
                }
                else
                {
                    id = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().guardarMinuta(obj, nuevaVersion);
                }
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
        [ValidateInput(false)]
        public ActionResult guardarDescripcion(tblSA_Minuta obj, bool nuevaVersion)
        {
            var result = new Dictionary<string, object>();
            try
            {

                seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().guardarDescripcion(obj, nuevaVersion);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult guardarActividad(tblSA_Actividades obj)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var id = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().guardarActividad(obj);
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
        [HttpPost]
        public ActionResult guardarComentario()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = JsonConvert.DeserializeObject<tblSA_ComentariosDTO>(Request.Form["obj"]);
                HttpPostedFileBase file = Request.Files["fupAdjunto"];
                var objS = new tblSA_Comentarios();
                objS.id = obj.id;
                objS.actividadID = obj.actividadID;
                objS.comentario = obj.comentario;
                //objS.usuarioNombre = obj.usuarioNombre;
                objS.usuarioNombre = vSesiones.sesionUsuarioDTO.nombre;
                objS.usuarioID = obj.usuarioID;
                objS.fecha = Convert.ToDateTime(obj.fecha);
                objS.tipo = obj.tipo;
                objS.adjuntoNombre = obj.adjuntoNombre;
                var data = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().guardarComentario(objS, file);
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
        public ActionResult guardarParticipante(tblSA_Participante obj)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var id = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().guardarParticipante(obj);
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
        public ActionResult eliminarParticipante(tblSA_Participante obj)
        {
            var result = new Dictionary<string, object>();
            try
            {

                seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().eliminarParticipante(obj);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getMinuta(int id, int userID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().getMinuta(id, 0);
                result.Add("obj", obj);
                result.Add("owner", (obj.creadorID == userID ? true : false));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult getMinutaForVersion(int id, int userID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().getMinutaForVersion(id);
                result.Add("obj", obj);
                result.Add("owner", (obj.creadorID == userID ? true : false));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult getMinutas()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().getMinutas(vSesiones.sesionUsuarioDTO.id);
                result.Add(ITEMS, obj.Select(x => new { Value = x.id, Text = x.proyecto }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getDashboard(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var minutas = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().getAllMinutas(id);
                var actividades = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().getAllActividades(id);
                result.Add("minutas", minutas);
                result.Add("actividades", actividades);
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
            //return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getParticipantes(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().getParticipantes(id);
                result.Add(ITEMS, obj.OrderBy(x => x.participante).Select(x => new { Value = x.participanteID, Text = x.participante }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult updateAvanceActividad(int id, int columna)
        {
            var result = new Dictionary<string, object>();
            try
            {
                seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().avanceActividad(id, columna);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getOrganigrama(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lista = organigramaFactoryServices.getOrganigramaService().getByUserID(id);
                result.Add("obj", lista);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult validarPromoverAvanceActividad(int actividadID, bool desdeMinuta = false)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var o = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().validarPromoverAvanceActividad(actividadID, vSesiones.sesionUsuarioDTO.id, desdeMinuta);
                result.Add(SUCCESS, o);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult esResponsableACtividad(int id, int u)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var o = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().esResponsableACtividad(id, u);
                result.Add("esResponsable", o);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult promoverAvanceActividad(tblSA_PromoverActividad obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().promoverAvanceActividad(obj);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult promocionAvanceActividad(int promoverID, int accion, string observacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().promocionAvanceActividad(promoverID, accion, observacion);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getAllActividadesAPromover()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().getAllActividadesAPromover(vSesiones.sesionUsuarioDTO.id);
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
        public ActionResult getListInteresados(string term, int actividad)
        {
            var items = usuarioFactoryServices.getUsuarioService().ListUsersByNameAndActivity(term, actividad);
            var filteredItems = items.Select(x => new { id = x.id, label = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        public ActionResult guardarInteresado(tblSA_Interesados obj)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var id = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().guardarInteresado(obj);
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
        public ActionResult guardarInteresados(List<tblSA_Interesados> obj)
        {
            var result = new Dictionary<string, object>();
            try
            {

                seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().guardarInteresados(obj);

                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult eliminarInteresado(tblSA_Interesados obj)
        {
            var result = new Dictionary<string, object>();
            try
            {

                seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().eliminarInteresado(obj);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getInteresadosPorActividad(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().getInteresadosPorActividad(id);
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

        public ActionResult getListResponsables(string term, int actividad)
        {
            var items = usuarioFactoryServices.getUsuarioService().ListResponsablesByNameAndActivity(term, actividad);
            var filteredItems = items.Select(x => new { id = x.id, label = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        public ActionResult guardarResponsable(tblSA_Responsables obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuario = usuarioFactoryServices.getUsuarioService().ListUsersById(obj.usuarioID);
                string nombreText = "";
                if (usuario.Count() > 0)
                {
                    if (usuario.FirstOrDefault() != null)
                    {

                        var temp = usuario.FirstOrDefault();
                        nombreText = temp.nombre + " " + temp.apellidoPaterno + " " + temp.apellidoMaterno;
                    }
                }
                obj.usuarioText = nombreText;

                var id = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().guardarResponsable(obj);
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
        public ActionResult guardarResponsables(List<tblSA_Responsables> obj)
        {
            var result = new Dictionary<string, object>();
            try
            {

                for (int i = 0; i < obj.Count; i++)
                {

                    var usuario = usuarioFactoryServices.getUsuarioService().ListUsersById(obj[i].usuarioID);
                    string nombreText = "";
                    if (usuario.Count > 0)
                    {
                        if (usuario.FirstOrDefault() != null)
                        {

                            var temp = usuario.FirstOrDefault();
                            nombreText = temp.nombre + " " + temp.apellidoPaterno + " " + temp.apellidoMaterno;
                        }



                    }
                    obj[i].usuarioText = nombreText;
                }

                seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().guardarResponsables(obj);

                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult eliminarResponsable(tblSA_Responsables obj)
        {
            var result = new Dictionary<string, object>();
            try
            {

                seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().eliminarResponsable(obj);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getResponsablesPorActividad(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().getResponsablesPorActividad(id);
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

        public FileResult getComentarioArchivoAdjunto()
        {
            int id = int.Parse(Request.QueryString["id"]);
            var o = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().getComentarioByID(id);
            return File(o.adjunto, "multipart/form-data", o.adjuntoNombre);
        }
        public ActionResult enviarCorreos(int minutaID, List<int> usuarios)
        {
            var result = new Dictionary<string, object>();

            try
            {
                List<Byte[]> downloadPDF = null;

                if (Session["downloadPDFMinuta"] != null)
                {
                    downloadPDF = (List<Byte[]>)Session["downloadPDFMinuta"];
                }
                else
                {
                    downloadPDF = (List<Byte[]>)Session["downloadPDF"];
                }
                
                var obj = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().enviarCorreos(minutaID, usuarios, downloadPDF);

                if (obj.Count() == 0)
                {
                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add("obj", string.Join("<br/>", obj.ToList()));
                    result.Add(SUCCESS, false);
                }

                Session["downloadPDF"] = null;
                Session["downloadPDFMinuta"] = null;
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboParticipantes(int minutaID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().FillComboParticipantes(minutaID);
                result.Add(ITEMS, obj.Select(x => new { Value = x.id, Text = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno }).OrderBy(x => x.Text));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboUsuarios(int minutaID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().FillComboUsuarios(minutaID);
                result.Add(ITEMS, obj.Select(x => new { Value = x.id, Text = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno }).OrderBy(x => x.Text));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getRevisaList(string term)
        {

            var items = usuarioFactoryServices.getUsuarioService().getListUsuarioByName(term);
            var filteredItems = items.Select(x => new { id = x.id, label = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno });

            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        public void setRespo()
        {
            seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().setDataCustom();
        }
        public ActionResult getActivitiesCount(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {

                int cantidad = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().getActivitiesCount(id);
                result.Add("cantidad", cantidad);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getReporteActividades(string Departamentos, DateTime FechaInicio, DateTime FechaFin, int Estatus)
        {
            var result = new Dictionary<string, object>();
            //try
            //{
            List<int> DepartamentosID = string.IsNullOrEmpty(Departamentos) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(Departamentos);
            var data = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().getReporteActividades(DepartamentosID, FechaInicio, FechaFin, Estatus);

            result.Add("dataMain", data);
            result.Add(SUCCESS, true);
            //}
            //catch (Exception e)
            //{
            //    result.Add(SUCCESS, false);
            //    result.Add(MESSAGE, e.Message);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getReporteMinutasPendientes(string Departamentos, DateTime FechaInicio, DateTime FechaFin)
        {
            var result = new Dictionary<string, object>();
            //try
            //{
            List<int> DepartamentosID = string.IsNullOrEmpty(Departamentos) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(Departamentos);
            var data = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().getReporteMinutasPendientes(DepartamentosID, FechaInicio, FechaFin);

            result.Add("dataMain", data);
            result.Add(SUCCESS, true);
            //}
            //catch (Exception e)
            //{
            //    result.Add(SUCCESS, false);
            //    result.Add(MESSAGE, e.Message);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getReporteEstadisticoMinutas(string Departamentos, DateTime FechaInicio, DateTime FechaFin)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<int> DepartamentosID = string.IsNullOrEmpty(Departamentos) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(Departamentos);
                var data = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().getReporteEstadisticoMinutas(DepartamentosID, FechaInicio, FechaFin);

                result.Add("dataMain", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, e.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getBitacoraMinutas(string Departamentos, DateTime FechaInicio, DateTime FechaFin)
        {
            var result = new Dictionary<string, object>();
            //try
            //{
            List<int> DepartamentosID = string.IsNullOrEmpty(Departamentos) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(Departamentos);
            var data = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().getBitacoraMinutas(DepartamentosID, FechaInicio, FechaFin);

            result.Add("dataMain", data);
            result.Add(SUCCESS, true);
            //}
            //catch (Exception e)
            //{
            //    result.Add(SUCCESS, false);
            //    result.Add(MESSAGE, e.Message);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboDepartamentos()
        {
            var result = new Dictionary<string, object>();
            try
            {

                var data = seguimientoAcuerdosFactoryServices.getSeguimientoAcuerdosService().getDepartamentos().Select(x => new { Value = x.id, Text = x.descripcion }).OrderBy(x => x.Value);

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

    }
}
