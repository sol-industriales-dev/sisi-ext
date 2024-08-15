using Core.DTO.MAZDA;
using Core.Entity.MAZDA;
using Core.Enum.MAZDA;
using Data.Factory.MAZDA;
using Data.Factory.Principal.Archivos;
using Newtonsoft.Json;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Excel;
using Infrastructure.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System.Drawing;
using Core.DAO.MAZDA;
using Newtonsoft.Json.Converters;


namespace SIGOPLAN.Areas.MAZDA.Controllers
{
    public class PlanActividadesController : BaseController
    {
        private MAZDAFactoryServices MAZDAFactoryServices;
        private ArchivoFactoryServices ArchivoFS;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            MAZDAFactoryServices = new MAZDAFactoryServices();
            ArchivoFS = new ArchivoFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        // GET: MAZDA/PlanActividades
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PlanMaestro()
        {
            return View();
        }
        public ActionResult CuadrillasCatalogo()
        {
            return View();
        }
        public ActionResult AreasCatalogo()
        {
            return View();
        }
        public ActionResult SubAreasCatalogo()
        {
            return View();
        }
        public ActionResult ActividadesCatalogo()
        {
            return View();
        }
        public ActionResult EquiposCatalogo()
        {
            return View();
        }
        public ActionResult PersonalCatalogo()
        {
            return View();
        }
        public ActionResult CapturaPlan()
        {
            return View();
        }
        public ActionResult Outsourcing()
        {
            return View();
        }
        public ActionResult RevisionAC()
        {
            return View();
        }
        public ActionResult RevisionCuadrilla()
        {
            return View();
        }
        public ActionResult ReporteActividades()
        {
            return View();
        }

        #region CUADRILLAS
        public ActionResult GetCuadrillas(int cuadrillaID, string personal)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = MAZDAFactoryServices.getPlanActividadesService().getCuadrillas(cuadrillaID, personal);
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
        public ActionResult GetCuadrilla(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = MAZDAFactoryServices.getPlanActividadesService().getCuadrilla(id);

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
        public void GuardarCuadrilla(string desc, List<UsuarioMAZDADTO> personal)
        {
            try
            {
                MAZDAFactoryServices.getPlanActividadesService().GuardarCuadrilla(desc, personal);
            }
            catch (Exception) { }
        }
        public ActionResult GuardarRevisionCuadrilla()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var evi = new List<tblMAZ_Revision_Cuadrilla_Evidencia>();
                var rev = JsonConvert.DeserializeObject<tblMAZ_Revision_Cuadrilla>(Request.Form["rev"]);
                var ayu = JsonConvert.DeserializeObject<tblMAZ_Revision_Cuadrilla_Ayudantes[]>(Request.Form["ayu[]"]);
                var det = JsonConvert.DeserializeObject<tblMAZ_Revision_Cuadrilla_Detalle[]>(Request.Form["det[]"]);
                if (Request.Files.Count > 0)
                {
                    var ultimoArchivo = MAZDAFactoryServices.getPlanActividadesService().GetUltimoArchivo(2);
                    var ultimaRevisionMasUno = MAZDAFactoryServices.getPlanActividadesService().GetUltimaRevision(2) + 1;

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase archivo = Request.Files[i];
                        string nombre = string.Format("{0:D8}_{1}_{2}", ++ultimoArchivo, ultimaRevisionMasUno, rev.cuadrillaID);
                        var extension = archivo.ContentType.Split('/')[1] != "jpeg" ? archivo.ContentType.Split('/')[1] : "jpg";
                        string ruta = ArchivoFS.getArchivo().getUrlDelServidor(2) + nombre + "." + extension;

                        evi.Add(new tblMAZ_Revision_Cuadrilla_Evidencia()
                        {
                            nombre = nombre,
                            ruta = ruta
                        });
                        SaveArchivo(archivo, ruta);
                    }
                }
                result.Add(SUCCESS, MAZDAFactoryServices.getPlanActividadesService().GuardarRevisionCuadrilla(rev, ayu.ToList(), det.ToList(), evi));
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public void EditarCuadrilla(int id, string desc, List<UsuarioMAZDADTO> personal)
        {
            try
            {
                MAZDAFactoryServices.getPlanActividadesService().EditarCuadrilla(id, desc, personal);
            }
            catch (Exception) { }
        }
        public void RemoveCuadrilla(int id)
        {
            try
            {
                MAZDAFactoryServices.getPlanActividadesService().RemoveCuadrilla(id);
            }
            catch (Exception) { }
        }
        public ActionResult GetCuadrillasList()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = MAZDAFactoryServices.getPlanActividadesService().getCuadrillas(0, "");
                var list = obj.Select(x => new
                {
                    Value = x.id,
                    Text = x.descripcion
                });

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
        public ActionResult GetCuadrillasRevisionList()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = MAZDAFactoryServices.getPlanActividadesService().getCuadrillas(0, "");
                var list = obj.Where(y => y.descripcion != "AIRES ACONDICIONADOS").Select(x => new
                {
                    Value = x.id,
                    Text = x.descripcion
                });

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
        public ActionResult GetRevisionCuadrilla(List<int> arrCuadrillas, List<int> arrPeriodos, List<string> arrAreas, List<string> arrActividades, int revisionID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = MAZDAFactoryServices.getPlanActividadesService().getPlanMaestroOrdenado(arrCuadrillas, arrPeriodos, arrAreas, arrActividades, null);
                var revision = MAZDAFactoryServices.getPlanActividadesService().getRevisionCua(arrCuadrillas.FirstOrDefault(), revisionID);

                result.Add("data", data);
                result.Add("revision", revision);
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

        #region AREAS
        public ActionResult GetAreas(int cuadrillaID, string area)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = MAZDAFactoryServices.getPlanActividadesService().getAreas(cuadrillaID, area);
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
        public ActionResult GetArea(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = MAZDAFactoryServices.getPlanActividadesService().getArea(id);
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
        public ActionResult GuardarArea()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var referencias = new List<tblMAZ_Area_Referencia>();
                var area = JsonConvert.DeserializeObject<tblMAZ_Area>(Request.Form["area"]);

                if (Request.Files.Count > 0)
                {
                    var ultimoArchivoArea = MAZDAFactoryServices.getPlanActividadesService().GetUltimoArchivoArea();

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase archivo = Request.Files[i];
                        string nombre = string.Format("{0:D8}_{1}_{2}", ++ultimoArchivoArea, "REF_AREA", area.descripcion);
                        var extension = archivo.ContentType.Split('/')[1] != "jpeg" ? archivo.ContentType.Split('/')[1] : "jpg";
                        string ruta = ArchivoFS.getArchivo().getUrlDelServidor(2) + nombre + "." + extension;
                        referencias.Add(new tblMAZ_Area_Referencia()
                        {
                            nombre = nombre,
                            ruta = ruta
                        });
                        SaveArchivo(archivo, ruta);
                    }
                }
                result.Add(SUCCESS, MAZDAFactoryServices.getPlanActividadesService().GuardarArea(area, referencias));
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditarReferenciaArea()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var referencias = new List<tblMAZ_Area_Referencia>();
                var areaID = JsonConvert.DeserializeObject<int>(Request.Form["areaID"]);

                var area = MAZDAFactoryServices.getPlanActividadesService().getArea(areaID);

                if (Request.Files.Count > 0)
                {
                    MAZDAFactoryServices.getPlanActividadesService().QuitarReferenciaArea(areaID);

                    var ultimoArchivoArea = MAZDAFactoryServices.getPlanActividadesService().GetUltimoArchivoArea();

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase archivo = Request.Files[i];
                        string nombre = string.Format("{0:D8}_{1}_{2}", ++ultimoArchivoArea, "REF_AREA", area.descripcion);
                        var extension = archivo.ContentType.Split('/')[1] != "jpeg" ? archivo.ContentType.Split('/')[1] : "jpg";
                        string ruta = ArchivoFS.getArchivo().getUrlDelServidor(2) + nombre + "." + extension;
                        referencias.Add(new tblMAZ_Area_Referencia()
                        {
                            nombre = nombre,
                            ruta = ruta
                        });
                        SaveArchivo(archivo, ruta);
                    }
                }
                result.Add(SUCCESS, MAZDAFactoryServices.getPlanActividadesService().GuardarReferenciaArea(areaID, referencias));
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public void EditarArea(int id, string desc, int cuadrillaID)
        {
            try
            {
                MAZDAFactoryServices.getPlanActividadesService().EditarArea(id, desc, cuadrillaID);
            }
            catch (Exception) { }
        }
        public void RemoveArea(int id)
        {
            try
            {
                MAZDAFactoryServices.getPlanActividadesService().RemoveArea(id);
            }
            catch (Exception) { }
        }
        public ActionResult GetAreasList(List<int> cuadrillasID, List<int> periodos)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = MAZDAFactoryServices.getPlanActividadesService().getAreas(0, "");

                var actividadesFiltradasAreasID = MAZDAFactoryServices.getPlanActividadesService().getActividades(0, 0, "", "").Where(x => periodos != null ? periodos.Contains(x.periodo) : true).Select(y => y.areaID).ToList();

                var objFiltrado = obj.Where(x => (cuadrillasID != null ? cuadrillasID.Contains(x.cuadrillaID) : true) && ((actividadesFiltradasAreasID != null && periodos != null) ? actividadesFiltradasAreasID.Contains(x.id) : true)).ToList();

                var list = objFiltrado.GroupBy(w => w.descripcion).Select(x => new
                {
                    Value = x.Select(z => z.id).FirstOrDefault(),
                    Text = x.Select(z => z.descripcion).FirstOrDefault()
                }).OrderBy(y => y.Text).ToList();

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
        #endregion

        #region ACTIVIDADES
        public ActionResult GetActividades(int cuadrillaID, int periodo, string area, string actividad)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = MAZDAFactoryServices.getPlanActividadesService().getActividades(cuadrillaID, periodo, area, actividad);
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
        public ActionResult GetActividad(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = MAZDAFactoryServices.getPlanActividadesService().getActividad(id);
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
        public void GuardarActividad(string desc, string descripcion, int cuadrillaID, string area, int periodo)
        {
            try
            {
                MAZDAFactoryServices.getPlanActividadesService().GuardarActividad(desc, descripcion, cuadrillaID, area, periodo);
            }
            catch (Exception) { }
        }
        public void EditarActividad(int id, string desc, string descripcion, int cuadrillaID, string area, int periodo)
        {
            try
            {
                MAZDAFactoryServices.getPlanActividadesService().EditarActividad(id, desc, descripcion, cuadrillaID, area, periodo);
            }
            catch (Exception) { }
        }
        public void RemoveActividad(int id)
        {
            try
            {
                MAZDAFactoryServices.getPlanActividadesService().RemoveActividad(id);
            }
            catch (Exception) { }
        }
        public ActionResult GetActividadesList(List<int> cuadrillasID, List<int> periodos, List<string> areas)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = MAZDAFactoryServices.getPlanActividadesService().getActividades(0, 0, "", "");

                var objFiltrado = obj.Where(x => (cuadrillasID != null ? cuadrillasID.Contains(x.cuadrillaID) : true) && (periodos != null ? periodos.Contains(x.periodo) : true) && (areas != null ? areas.Contains(x.area) : true)).ToList();

                var objFiltrado2 = objFiltrado.Where(x => areas != null ? areas.Contains(x.area) : true).ToList();

                var list = objFiltrado.GroupBy(w => w.descripcion).Select(x => new
                {
                    Value = x.Select(z => z.id).FirstOrDefault(),
                    Text = x.Select(z => z.descripcion).FirstOrDefault()
                }).OrderBy(y => y.Text).ToList();

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
        public ActionResult GetActividadesAC(int tipo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = MAZDAFactoryServices.getPlanActividadesService().getActividadesAC();

                var objFiltrado = obj;

                switch (tipo)
                {
                    case 1:
                        objFiltrado = obj.Where(x => x.id <= 12).ToList();
                        break;
                    case 2:
                        objFiltrado = obj.Where(x => x.id > 12).ToList();
                        break;
                }

                result.Add("data", objFiltrado);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllActividadesAC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = MAZDAFactoryServices.getPlanActividadesService().getActividadesAC();



                result.Add("lstCondensador", lst.Where(x => x.id <= 12).ToList());
                result.Add("lstEvaporador", lst.Where(x => x.id > 12).ToList());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllActividadesACRevision()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = MAZDAFactoryServices.getPlanActividadesService().getActividadesAC();

                var revisionID = Int32.Parse(Request.QueryString["revisionID"]);
                var revision = MAZDAFactoryServices.getPlanActividadesService().getRevisionAC(revisionID);

                result.Add("lstCondensador", lst.Where(x => x.id <= 12).ToList());
                result.Add("lstEvaporador", lst.Where(x => x.id > 12).ToList());
                result.Add("revision", revision);

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

        #region EQUIPOS
        public ActionResult GetEquipos(string descripcion, string subArea, int periodo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = MAZDAFactoryServices.getPlanActividadesService().getEquipos(descripcion, subArea, periodo);
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
        public ActionResult GetEquiposCatalogo(List<int> arrCuadrillas, List<int> arrAreas, List<int> arrSubAreas)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = MAZDAFactoryServices.getPlanActividadesService().getEquiposCatalogo(arrCuadrillas, arrAreas, arrSubAreas);
                result.Add("data", obj);
                Session["lstEquiposCatalogo"] = obj;
                Session["lstCuadrillas"] = arrCuadrillas;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEquiposList(List<int> arrCuadrillas, List<int> arrAreas, List<int> arrSubAreas)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = MAZDAFactoryServices.getPlanActividadesService().getEquiposCatalogo(arrCuadrillas, arrAreas, arrSubAreas);
                var list = obj.Select(x => new
                {
                    Value = x.id,
                    Text = x.descripcion,
                    Prefijo = 2
                });

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
        public ActionResult GetEquipoMAZDA(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = MAZDAFactoryServices.getPlanActividadesService().getEquipo(id);
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
        public ActionResult GuardarEquipo()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var referencias = new List<tblMAZ_Equipo_Referencia>();
                var equipo = JsonConvert.DeserializeObject<tblMAZ_Equipo_AC>(Request.Form["equipo"]);

                if (Request.Files.Count > 0)
                {
                    var ultimoArchivoEquipo = MAZDAFactoryServices.getPlanActividadesService().GetUltimoArchivoEquipo();

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase archivo = Request.Files[i];
                        string nombre = string.Format("{0:D8}_{1}_{2}", ++ultimoArchivoEquipo, "REF_EQ", equipo.descripcion);
                        var extension = archivo.ContentType.Split('/')[1] != "jpeg" ? archivo.ContentType.Split('/')[1] : "jpg";
                        string ruta = ArchivoFS.getArchivo().getUrlDelServidor(2) + nombre + "." + extension;
                        referencias.Add(new tblMAZ_Equipo_Referencia()
                        {
                            nombre = nombre,
                            ruta = ruta
                        });
                        SaveArchivo(archivo, ruta);
                    }
                }
                result.Add(SUCCESS, MAZDAFactoryServices.getPlanActividadesService().GuardarEquipo(equipo, referencias));
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditarReferenciaEquipo()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var referencias = new List<tblMAZ_Equipo_Referencia>();
                var equipoID = JsonConvert.DeserializeObject<int>(Request.Form["equipoID"]);

                var equipo = MAZDAFactoryServices.getPlanActividadesService().getEquipo(equipoID);

                if (Request.Files.Count > 0)
                {
                    MAZDAFactoryServices.getPlanActividadesService().QuitarReferenciaEquipo(equipoID);

                    var ultimoArchivoEquipo = MAZDAFactoryServices.getPlanActividadesService().GetUltimoArchivoEquipo();

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase archivo = Request.Files[i];
                        string nombre = string.Format("{0:D8}_{1}_{2}", ++ultimoArchivoEquipo, "REF_EQ", equipo.descripcion);
                        var extension = archivo.ContentType.Split('/')[1] != "jpeg" ? archivo.ContentType.Split('/')[1] : "jpg";
                        string ruta = ArchivoFS.getArchivo().getUrlDelServidor(2) + nombre + "." + extension;
                        referencias.Add(new tblMAZ_Equipo_Referencia()
                        {
                            nombre = nombre,
                            ruta = ruta
                        });
                        SaveArchivo(archivo, ruta);
                    }
                }
                result.Add(SUCCESS, MAZDAFactoryServices.getPlanActividadesService().GuardarReferenciaEquipo(equipoID, referencias));
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public void EditarEquipo(int id, string descripcion, string caracteristicas, string modelo, string tonelaje, int subAreaID, string subArea, int cantidad, bool estatus)
        {
            try
            {
                MAZDAFactoryServices.getPlanActividadesService().EditarEquipo(id, descripcion, caracteristicas, modelo, tonelaje, subAreaID, subArea, cantidad, estatus);
            }
            catch (Exception) { }
        }
        public void RemoveEquipo(int id)
        {
            try
            {
                MAZDAFactoryServices.getPlanActividadesService().RemoveEquipo(id);
            }
            catch (Exception) { }
        }
        public ActionResult GetEquiposAreasList(int cuadrillaID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = MAZDAFactoryServices.getPlanActividadesService().getEquiposAreas(cuadrillaID);
                var list = obj.Select(x => new
                {
                    Value = x.id,
                    Text = x.descripcion,
                    Prefijo = x.tipo
                });

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
        public ActionResult GetEquipoAC(int equipoID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = MAZDAFactoryServices.getPlanActividadesService().getEquipoAC(equipoID);

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
        public ActionResult GetReferencias(List<int> equiposID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<string> archivos = new List<string>();

                archivos = MAZDAFactoryServices.getPlanActividadesService().getReferencias(equiposID);

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
        #endregion

        #region USUARIOS
        public void GuardarUsuario(string nombre, string apellidoPaterno, string apellidoMaterno, string correo, string usuario, int cuadrillaID)
        {
            try
            {
                MAZDAFactoryServices.getPlanActividadesService().GuardarUsuario(nombre, apellidoPaterno, apellidoMaterno, correo, usuario, cuadrillaID);
            }
            catch (Exception) { }
        }
        public void EditarUsuario(int id, string nombre, string apellidoPaterno, string apellidoMaterno, string correo, string usuario, int cuadrillaID)
        {
            try
            {
                MAZDAFactoryServices.getPlanActividadesService().EditarUsuario(id, nombre, apellidoPaterno, apellidoMaterno, correo, usuario, cuadrillaID);
            }
            catch (Exception) { }
        }
        public ActionResult GetUsuariosMAZDA(string usuario, int cuadrillaID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = MAZDAFactoryServices.getPlanActividadesService().getUsuarios(usuario, cuadrillaID);

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
        public ActionResult GetUsuariosList()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = MAZDAFactoryServices.getPlanActividadesService().getUsuarios("", 0);

                var list = obj.Select(x => new
                {
                    Value = x.id,
                    Text = x.nombreCompleto
                }).OrderBy(y => y.Text);

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
        public ActionResult GetUsuarioMAZDA(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = MAZDAFactoryServices.getPlanActividadesService().getUsuario(id);

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
        public void RemoveUsuario(int id)
        {
            try
            {
                MAZDAFactoryServices.getPlanActividadesService().RemoveUsuario(id);
            }
            catch (Exception) { }
        }
        #endregion

        #region SUBAREAS
        public ActionResult GetSubAreasList(List<int> areaID, List<int> cuadrillaID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = MAZDAFactoryServices.getPlanActividadesService().getSubAreas(0, "");
                var objAreas = MAZDAFactoryServices.getPlanActividadesService().getAreas(0, "").Where(x => (cuadrillaID != null ? cuadrillaID.Contains(x.cuadrillaID) : true)).Select(y => y.id).ToList();

                var objFiltrado = obj.Where(x => (cuadrillaID != null ? objAreas.Contains(x.areaID) : areaID != null ? areaID.Contains(x.areaID) : true)).ToList();

                var list = objFiltrado.Select(x => new
                {
                    id = x.id,
                    descripcion = x.descripcion
                }).OrderBy(y => y.descripcion).ToList();

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
        public ActionResult GetSubAreasCatalogo(List<int> arrCuadrillas, List<int> arrAreas)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = MAZDAFactoryServices.getPlanActividadesService().getSubAreasCatalogo(arrCuadrillas, arrAreas);
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
        public ActionResult GuardarSubArea()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var referencias = new List<tblMAZ_Subarea_Referencia>();
                var subArea = JsonConvert.DeserializeObject<tblMAZ_SubArea>(Request.Form["subArea"]);

                if (Request.Files.Count > 0)
                {
                    var ultimoArchivoSubArea = MAZDAFactoryServices.getPlanActividadesService().GetUltimoArchivoSubArea();

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase archivo = Request.Files[i];
                        string nombre = string.Format("{0:D8}_{1}_{2}", ++ultimoArchivoSubArea, "REF_SA", subArea.descripcion);
                        var extension = archivo.ContentType.Split('/')[1] != "jpeg" ? archivo.ContentType.Split('/')[1] : "jpg";
                        string ruta = ArchivoFS.getArchivo().getUrlDelServidor(2) + nombre + "." + extension;
                        referencias.Add(new tblMAZ_Subarea_Referencia()
                        {
                            nombre = nombre,
                            ruta = ruta
                        });
                        SaveArchivo(archivo, ruta);
                    }
                }


                result.Add(SUCCESS, MAZDAFactoryServices.getPlanActividadesService().GuardarSubArea(subArea, referencias));
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSubArea(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = MAZDAFactoryServices.getPlanActividadesService().getSubArea(id);
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
        public void EditarSubArea(int id, string descripcion, int areaID, bool estatus)
        {
            try
            {
                MAZDAFactoryServices.getPlanActividadesService().EditarSubArea(id, descripcion, areaID, estatus);
            }
            catch (Exception) { }
        }
        public void RemoveSubArea(int id)
        {
            try
            {
                MAZDAFactoryServices.getPlanActividadesService().RemoveSubArea(id);
            }
            catch (Exception) { }
        }
        public ActionResult EditarReferenciaSubarea()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var referencias = new List<tblMAZ_Subarea_Referencia>();
                var subareaID = JsonConvert.DeserializeObject<int>(Request.Form["subareaID"]);

                var subarea = MAZDAFactoryServices.getPlanActividadesService().getSubArea(subareaID);

                if (Request.Files.Count > 0)
                {
                    MAZDAFactoryServices.getPlanActividadesService().QuitarReferenciaSubarea(subareaID);

                    var ultimoArchivoSubarea = MAZDAFactoryServices.getPlanActividadesService().GetUltimoArchivoSubArea();

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase archivo = Request.Files[i];
                        string nombre = string.Format("{0:D8}_{1}_{2}", ++ultimoArchivoSubarea, "REF_SA", subarea.descripcion);
                        var extension = archivo.ContentType.Split('/')[1] != "jpeg" ? archivo.ContentType.Split('/')[1] : "jpg";
                        string ruta = ArchivoFS.getArchivo().getUrlDelServidor(2) + nombre + "." + extension;
                        referencias.Add(new tblMAZ_Subarea_Referencia()
                        {
                            nombre = nombre,
                            ruta = ruta
                        });
                        SaveArchivo(archivo, ruta);
                    }
                }
                result.Add(SUCCESS, MAZDAFactoryServices.getPlanActividadesService().GuardarReferenciaSubArea(subareaID, referencias));
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PLAN MAESTRO
        public ActionResult GetPlanMaestro(List<int> arrCuadrillas, List<int> arrPeriodos, List<string> arrAreas, List<string> arrActividades, List<int> arrMeses)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = MAZDAFactoryServices.getPlanActividadesService().getPlanMaestroOrdenado(arrCuadrillas, arrPeriodos, arrAreas, arrActividades, arrMeses);
                Session["lstMaestro"] = data;
                result.Add("data", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                Session["lstMaestro"] = null;
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPeriodosList(List<int> cuadrillasID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = MAZDAFactoryServices.getPlanActividadesService().getActividades(0, 0, "", "");

                var objFiltrado = obj.Where(x => (cuadrillasID != null ? cuadrillasID.Contains(x.cuadrillaID) : true)).ToList();

                var list = objFiltrado.GroupBy(w => w.periodo).Select(x => new
                {
                    Value = x.Select(z => z.periodo).FirstOrDefault(),
                    Text = ((PeriodoEnum)x.Select(z => z.periodo).FirstOrDefault()).ToString(),
                }).OrderBy(y => y.Value).ToList();

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
        public ActionResult GetMesesList(int periodo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                string[] months = new string[] { "ENERO", "FEBRERO", "MARZO", "ABRIL", "MAYO", "JUNIO", "JULIO", "AGOSTO", "SEPTIEMBRE", "OCTUBRE", "NOVIEMBRE", "DICIEMBRE" };
                var contador = 1;

                var list = months.Select(x => new
                {
                    Value = contador++,
                    Text = x,
                }).OrderBy(y => y.Value).ToList();

                switch (periodo)
                {
                    case 1:
                        result.Add(ITEMS, list);
                        result.Add(SUCCESS, true);
                        break;
                    case 2:
                        result.Add(ITEMS, list.Where(x => x.Value == 1 || x.Value == 3 || x.Value == 5 || x.Value == 7 || x.Value == 9 || x.Value == 11));
                        result.Add(SUCCESS, true);
                        break;
                    case 3:
                        result.Add(ITEMS, list.Where(x => x.Value == 3 || x.Value == 6 || x.Value == 9 || x.Value == 12));
                        result.Add(SUCCESS, true);
                        break;
                    case 4:
                        result.Add(ITEMS, list.Where(x => x.Value == 4 || x.Value == 10));
                        result.Add(SUCCESS, true);
                        break;
                    case 5:
                        result.Add(ITEMS, list.Where(x => x.Value == 8));
                        result.Add(SUCCESS, true);
                        break;
                    default:
                        result.Add(ITEMS, list);
                        result.Add(SUCCESS, true);
                        break;
                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDiasMes(int year, int month)
        {
            var result = new Dictionary<string, object>();

            try
            {
                int diasCantidad = DateTime.DaysInMonth(year, month);
                Dictionary<int, string> diasArr = new Dictionary<int, string>();

                for (var i = 0; i < diasCantidad; i++)
                {
                    DateTime dt = new DateTime(year, month, i + 1);
                    diasArr.Add(i + 1, DateTimeFormatInfo.CurrentInfo.GetDayName(dt.DayOfWeek).ToString().ToUpper().Substring(0, 1));
                }

                var list = diasArr.Select(x => new ComboDTO
                {
                    Value = x.Key.ToString(),
                    Text = x.Value,
                }).OrderBy(y => Int32.Parse(y.Value)).ToList();
                Session["lstDias"] = list;
                Session["year"] = year;
                Session["month"] = month;
                result.Add(ITEMS, list);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                Session["lstDias"] = null;
                Session["year"] = 0;
                Session["month"] = 0;
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllDays(int year)
        {


            var result = new Dictionary<string, object>();
            var dias = MAZDAFactoryServices.getPlanActividadesService().getAllDays(DateTime.Now.Year);

            result.Add("diasMes", dias);
            Session["lstDias"] = dias;
            result.Add(SUCCESS, true);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getAllMonths(int year)
        {
            var result = new Dictionary<string, object>();
            var meses = new List<string>();
            Dictionary<int, string> dias = new Dictionary<int, string>();

            try
            {
                DateTimeFormatInfo dtfi = DateTimeFormatInfo.CurrentInfo;

                for (int ctr = 0; ctr <= dtfi.MonthNames.Length - 1; ctr++)
                {
                    if (String.IsNullOrEmpty(dtfi.MonthNames[ctr]))
                        continue;
                    meses.Add(dtfi.MonthNames[ctr].ToUpper());

                    //for (int d = 0; d <= DateTime.DaysInMonth(year, ctr + 1); d++ )
                    //{
                    //    DateTime dt = new DateTime(year,  ctr + 1, d + 1);
                    //    dias.Add(d + 1, DateTimeFormatInfo.CurrentInfo.GetDayName(dt.DayOfWeek).ToString().ToUpper().Substring(0, 1));
                    //}

                    //Console.WriteLine("{0,-10}{1,-15}{2,4}", year,
                    //                  dtfi.MonthNames[ctr],
                    //                  DateTime.DaysInMonth(year, ctr + 1));
                }

                result.Add("Meses", meses);
            }
            catch (Exception e)
            {

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetOrganigramaPersonal()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = MAZDAFactoryServices.getPlanActividadesService().getUsuarios("", 0);



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
        public ActionResult GetPlanMes(int cuadrillaID, int periodo, int mes)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = MAZDAFactoryServices.getPlanActividadesService().getPlanMes(cuadrillaID, periodo, mes);

                var cua = MAZDAFactoryServices.getPlanActividadesService().getCuadrilla(cuadrillaID);

                var periodoString = "";
                switch (periodo)
                {
                    case 1:
                        periodoString = "MENSUAL";
                        break;
                    case 2:
                        periodoString = "BIMESTRAL";
                        break;
                    case 3:
                        periodoString = "TRIMESTRAL";
                        break;
                    case 4:
                        periodoString = "SEMESTRAL";
                        break;
                    case 5:
                        periodoString = "ANUAL";
                        break;
                }

                string[] months = new string[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
                var contador = 1;

                var list = months.Select(x => new
                {
                    Value = contador++,
                    Text = x,
                }).OrderBy(y => y.Value).ToList();

                var month = list.Where(x => x.Value == mes).Select(y => y.Text).FirstOrDefault();

                var info = new { cua = cua.descripcion, per = periodoString, mes = month };

                Session["planMesCuadrilla"] = cua.descripcion;
                Session["planMesPeriodo"] = periodoString;
                Session["planMesMes"] = month;

                Session["planMes"] = obj;
                result.Add("data", obj);
                result.Add("info", info);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPlanMesEquipo(int cuadrillaID, int periodo, List<int> equipoID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = MAZDAFactoryServices.getPlanActividadesService().getPlanMesEquipo(cuadrillaID, periodo, equipoID);

                var cua = MAZDAFactoryServices.getPlanActividadesService().getCuadrilla(cuadrillaID);

                var periodoString = "";
                switch (periodo)
                {
                    case 1:
                        periodoString = "MENSUAL";
                        break;
                    case 2:
                        periodoString = "BIMESTRAL";
                        break;
                    case 3:
                        periodoString = "TRIMESTRAL";
                        break;
                    case 4:
                        periodoString = "SEMESTRAL";
                        break;
                    case 5:
                        periodoString = "ANUAL";
                        break;
                }

                string[] months = new string[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
                var contador = 1;

                var list = months.Select(x => new
                {
                    Value = contador++,
                    Text = x,
                }).OrderBy(y => y.Value).ToList();


                Session["planMesCuadrilla"] = cua.descripcion;
                Session["planMesPeriodo"] = periodoString;
                Session["planPeriodoID"] = periodo;

                Session["planMes"] = obj;
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
        public ActionResult GetPlanMesGeneral(int mes)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var listObj = MAZDAFactoryServices.getPlanActividadesService().getPlanMesGeneral(mes);

                string[] months = new string[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
                var contador = 1;

                var list = months.Select(x => new
                {
                    Value = contador++,
                    Text = x,
                }).OrderBy(y => y.Value).ToList();

                var month = list.Where(x => x.Value == mes).Select(y => y.Text).FirstOrDefault();

                var info = new { mes = month };

                Session["planMesMesGeneral"] = month;
                Session["planMesGeneral"] = listObj;

                result.Add("data", listObj);
                result.Add("info", info);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public void GuardarPlanMes(PlanMesDTO plan)
        {
            try
            {
                MAZDAFactoryServices.getPlanActividadesService().GuardarPlanMes(plan);
            }
            catch (Exception) { }
        }
        public ActionResult GetEquiposAllDays(List<int> arrCuadrillas, List<int> arrAreas, List<int> arrSubAreas, int year)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var equipos = MAZDAFactoryServices.getPlanActividadesService().getEquiposCatalogo(arrCuadrillas, arrAreas, arrSubAreas);
                var dias = MAZDAFactoryServices.getPlanActividadesService().getAllDays(year);

                result.Add("Equipos", equipos);
                result.Add("Dias", dias);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public MemoryStream GenerarPlanExcel()
        {
            var equipos = ((List<EquipoDTO>)Session["lstEquiposCatalogo"]).ToList();
            var dias = ((List<ComboDTO>)Session["lstDias"]).ToList();
            var planMesDetalle = Session["planMes"] != null ? ((List<PlanMesDTO>)Session["planMes"]).ToList() : null;
            int periodoID = Session["planPeriodoID"] != null ? Int32.Parse(Session["planPeriodoID"].ToString()) : 0;
            string area = equipos.Count > 0 ? equipos[0].area.ToString().ToUpper() : "";
            int areaID = equipos.Count > 0 ? equipos[0].areaID : 0;
            int cuadrillaID = equipos.Count > 0 ? equipos[0].cuadrillaID : 0;

            var revision = MAZDAFactoryServices.getPlanActividadesService().getRevisionActividadEquipo(equipos, cuadrillaID, areaID);

            var stream = MAZDAFactoryServices.getPlanActividadesService().GenerarPlanExcel(equipos, dias, planMesDetalle, periodoID, revision);

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Desarrollo Mantenimiento" + area + ".xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                return stream;
            }
            else
            {
                return null;
            }
        }

        public ActionResult getRevisionActividadEquipo(List<EquipoDTO> equipos, int cuadrillaID, int areaID)
        {
            var result = new Dictionary<string, object>();
            var revision = MAZDAFactoryServices.getPlanActividadesService().getRevisionActividadEquipo(equipos, cuadrillaID, areaID);
            result.Add("revision", revision);
            result.Add(SUCCESS, true);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region COMBOBOX
        public ActionResult GetEmpleadoList()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = MAZDAFactoryServices.getPlanActividadesService().GetEmpleadoList();

                var list = obj.Select(y => new ComboDTO
                {
                    Value = y.id.ToString(),
                    Text = string.Format("{0} {1} {2}", y.nombre, y.apellidoPaterno, y.apellidoMaterno)
                });

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
        #endregion

        #region REVISION / EVIDENCIA
        public ActionResult GuardarRevision()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var evi = new List<tblMAZ_Revision_AC_Evidencia>();
                var rev = JsonConvert.DeserializeObject<tblMAZ_Revision_AC>(Request.Form["rev"]);
                var ayu = JsonConvert.DeserializeObject<tblMAZ_Revision_AC_Ayudantes[]>(Request.Form["ayu[]"]);
                var det = JsonConvert.DeserializeObject<tblMAZ_Revision_AC_Detalle[]>(Request.Form["det[]"]);
                if (Request.Files.Count > 0)
                {
                    var ultimoArchivo = MAZDAFactoryServices.getPlanActividadesService().GetUltimoArchivo(1);
                    var ultimaRevisionMasUno = MAZDAFactoryServices.getPlanActividadesService().GetUltimaRevision(1) + 1;

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase archivo = Request.Files[i];
                        string nombre = string.Format("{0:D8}_{1}_{2}", ++ultimoArchivo, ultimaRevisionMasUno, "AC");
                        var extension = archivo.ContentType.Split('/')[1] != "jpeg" ? archivo.ContentType.Split('/')[1] : "jpg";
                        string ruta = ArchivoFS.getArchivo().getUrlDelServidor(2) + nombre + "." + extension;
                        evi.Add(new tblMAZ_Revision_AC_Evidencia()
                        {
                            nombre = nombre,
                            ruta = ruta
                        });
                        SaveArchivo(archivo, ruta);
                    }
                }
                result.Add(SUCCESS, MAZDAFactoryServices.getPlanActividadesService().GuardarRevision(rev, ayu.ToList(), det.ToList(), evi));
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetReporteDiario(string fecha)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = MAZDAFactoryServices.getPlanActividadesService().getReporteDiario(fecha);

                string fechaConDia;

                DateTime fechaDate = new DateTime();
                if (fecha != "")
                {
                    DateTime dateTemp;

                    if (DateTime.TryParse(fecha, out dateTemp))
                    {
                        fechaDate = DateTime.Parse(fecha);

                        fechaConDia = DateTimeFormatInfo.CurrentInfo.GetDayName(fechaDate.DayOfWeek) + " " + fecha;

                        result.Add("fechaConDia", UppercaseFirst(fechaConDia));
                    }
                }

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
        public ActionResult GetEvidencias(int tipoRevision, int revisionID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<string> archivos = new List<string>();

                switch (tipoRevision)
                {
                    case 1:
                        archivos = MAZDAFactoryServices.getPlanActividadesService().getEvidenciasAC(revisionID);
                        break;
                    case 2:
                        archivos = MAZDAFactoryServices.getPlanActividadesService().getEvidenciasCua(revisionID);
                        break;
                    default:
                        return null;
                }

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
        public ActionResult GuardarInfoRevDet()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var format = "dd/MM/yyyy"; // your datetime format
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };

                var evi = new List<tblMAZ_Reporte_Actividades_Evidencia>();
                var info = JsonConvert.DeserializeObject<tblMAZ_Reporte_Actividades>(Request.Form["info"], dateTimeConverter);
                var eq = JsonConvert.DeserializeObject<tblMAZ_Reporte_Actividades_Equipo[]>(Request.Form["eq[]"]);

                var flagPasarEvidencias = false;

                if (Request.Files.Count > 0)
                {
                    var ultimoArchivo = MAZDAFactoryServices.getPlanActividadesService().GetUltimoArchivoReporteAct();
                    var ultimaReporteMasUno = MAZDAFactoryServices.getPlanActividadesService().GetUltimoReporteAct() + 1;

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase archivo = Request.Files[i];
                        string nombre = string.Format("{0}_{1:D8}_{2}", "ReporteAct", ++ultimoArchivo, ultimaReporteMasUno);
                        var extension = archivo.ContentType.Split('/')[1] != "jpeg" ? archivo.ContentType.Split('/')[1] : "jpg";
                        string ruta = ArchivoFS.getArchivo().getUrlDelServidor(2) + nombre + "." + extension;
                        evi.Add(new tblMAZ_Reporte_Actividades_Evidencia()
                        {
                            reporteActividadesID = 0,
                            nombre = nombre,
                            ruta = ruta,
                            estatus = true
                        });
                        SaveArchivo(archivo, ruta);
                    }
                }
                else
                {
                    flagPasarEvidencias = true;
                }

                result.Add(SUCCESS, MAZDAFactoryServices.getPlanActividadesService().GuardarInfoRevDet(info, eq.ToList(), evi, flagPasarEvidencias));
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEvidenciasReporte(int revDetID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<string> archivos = new List<string>();

                archivos = MAZDAFactoryServices.getPlanActividadesService().getEvidenciasReporte(revDetID);

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
        #endregion

        #region UTILIDADADES
        void SaveArchivo(HttpPostedFileBase archivo, string ruta)
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
            // ruta = ruta.Replace("C:\\", "\\REPOSITORIO\\");
            //System.IO.File.WriteAllBytes(@"C:\prueba\123.jpg", data);
            System.IO.File.WriteAllBytes(ruta, data);
        }
        static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);

        }
        #endregion
    }
}