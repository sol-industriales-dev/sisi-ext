using Core.DTO;
using Core.DTO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
using Data.DAO.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Principal.Archivos;
using Infrastructure.DTO;
using Infrastructure.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Catalogos.Maquinaria
{
    public class CatModeloEquipoController : BaseController
    {

        #region Factory

        subConjuntoModeloFactoryServices subConjuntoModeloFactoryServices;
        ModeloEquipoFactoryServices modeloEquipoFactoryServices;
        ArchivosModelosFactoryServices archivosModelosFactoryServices;
        ArchivoFactoryServices archivofs;
        #endregion

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            subConjuntoModeloFactoryServices = new subConjuntoModeloFactoryServices();
            archivosModelosFactoryServices = new ArchivosModelosFactoryServices();
            modeloEquipoFactoryServices = new ModeloEquipoFactoryServices();
            archivofs = new ArchivoFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        // GET: Modelo
        public ActionResult Index()
        {
            var usuarioDTO = vSesiones.sesionUsuarioDTO;

            if (usuarioDTO != null)
            {
                ViewBag.pagina = "catalogo";
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }
        public ActionResult VerDocumentosModelos()
        {
            return View();
        }

        public ActionResult verSubConjuntos(int modeloID)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var Data = subConjuntoModeloFactoryServices.getsubConjuntoModeloFactoryServices().getDataSubConjuntoModelo(modeloID);




                /*                result.Add("current", 1);
                                result.Add("rowCount", 1);
                                result.Add("total", listResult.Count());
                                result.Add("rows", listResult);*/

                result.Add("dataSubConjunto", Data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillGrid_ModeloEquipo(tblM_CatModeloEquipo modeloEquipo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var listResult = modeloEquipoFactoryServices.getModeloEquipoService().FillGridModeloEquipo(modeloEquipo, "").
                             Select(x => new
                             {
                                 id = x.id,
                                 idMarca = x.marcaEquipoID,
                                 marca = x.marcaDesc,
                                 descripcion = x.descripcion,
                                 nomCorto = x.nomCorto,
                                 noComponente = x.noComponente,
                                 idGrupo = x.idGrupo == null ? 0 : x.idGrupo,
                                 estatus = (x.estatus == true) ? "ACTIVO" : "INACTIVO",
                                 hasArchivos = string.IsNullOrEmpty(x.Ruta) ? true : false
                             });

                result.Add("current", 1);
                result.Add("rowCount", 1);
                result.Add("total", listResult.Count());
                result.Add("rows", listResult);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillGridVistaDocumentosModelo(tblM_CatModeloEquipo modeloEquipo, string grupoDesc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var listResult = modeloEquipoFactoryServices.getModeloEquipoService().FillGridModeloEquipo(modeloEquipo, grupoDesc).Where(y => !string.IsNullOrWhiteSpace(y.Ruta)).
                             Select(x => new
                             {
                                 id = x.id,
                                 idMarca = x.marcaEquipoID,
                                 marca = x.marcaDesc,
                                 descripcion = x.descripcion,
                                 nomCorto = x.nomCorto,
                                 noComponente = x.noComponente,
                                 idGrupo = x.idGrupo == null ? 0 : x.idGrupo,
                                 grupoDesc = x.grupoDesc,
                                 estatus = (x.estatus == true) ? "ACTIVO" : "INACTIVO",
                                 hasArchivos = string.IsNullOrEmpty(x.Ruta) ? true : false
                             }).ToList();

                result.Add("current", 1);
                result.Add("rowCount", 1);
                result.Add("total", listResult.Count());
                result.Add("rows", listResult);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult FillCboMarcaEquipo_ModeloEquipo(bool estatus)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, modeloEquipoFactoryServices.getModeloEquipoService().FillCboMarcaEquipo(estatus).Select(x => new { Value = x.id, Text = x.descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboGrupo_ModeloEquipo(bool estatus)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, modeloEquipoFactoryServices.getModeloEquipoService().fillGrupoMaquinaria(estatus).Select(x => new { Value = x.id, Text = x.descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SaveOrUpdate_ModelosEquipo(tblM_CatModeloEquipo obj, int Actualizacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                modeloEquipoFactoryServices.getModeloEquipoService().Guardar(obj);
                result.Add(MESSAGE, GlobalUtils.getMensaje(Actualizacion));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult FillCboModelo(int obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var dataResult = modeloEquipoFactoryServices.getModeloEquipoService().FillCboModelo(obj).Select(x => new { Value = x.id, Text = x.descripcion });

                result.Add(ITEMS, dataResult);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillMultipleModelo(List<int> lstGrupo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var dataResult = new List<ComboDTO>();
                lstGrupo.ForEach(g => {
                    dataResult.AddRange(modeloEquipoFactoryServices.getModeloEquipoService().FillCboModelo(g).Select(x => new ComboDTO { Value = x.id, Text = x.descripcion }));
                });
                result.Add(ITEMS, dataResult);
                result.Add(SUCCESS, true);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SubirArchivos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = JsonConvert.DeserializeObject<tblM_CatModeloEquipo>(Request.Form["obj"]);
                var listaDocumentos = JsonConvert.DeserializeObject<List<int>>(Request.Form["ArchivosEliminados"]);
                var objetoModal = modeloEquipoFactoryServices.getModeloEquipoService().LoadArchivos(obj.id);
                List<int> listaSubConjuntos = new List<int>();
                List<string> listaNumParte = new List<string>();
                try { listaSubConjuntos = JsonConvert.DeserializeObject<List<int>>(Request.Form["listaSubConjuntos"]); }
                catch (Exception e) {}
                try { listaNumParte = JsonConvert.DeserializeObject<List<string>>(Request.Form["listanumParte"]); }
                catch (Exception e) { }

                if (objetoModal == null)
                {

                    modeloEquipoFactoryServices.getModeloEquipoService().Guardar(obj);
                    modeloEquipoFactoryServices.getModeloEquipoService().GuardarSubconjuntos(listaSubConjuntos, listaNumParte, obj.id);
                }
                string concatenaRuta = "";
                bool tieneArchivos = false;

                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase archivo = Request.Files[i];
                        string extension = Path.GetExtension(archivo.FileName);
                        string nombreOriginal = archivo.FileName;
                        string FileName = DateTime.Now.ToString("ddMMYYYmm") + nombreOriginal;

                        string Ruta = archivofs.getArchivo().getUrlDelServidor(4) + FileName;

                        var archivoExiste = GlobalUtils.SaveArchivo(archivo, Ruta);//notaCreditoFactoryServices.getNotaCredito().SaveArchivo(archivo, Ruta);

                        tblM_ArchivosModelos archivoModelos = new tblM_ArchivosModelos();
                        if (archivoExiste)
                        {
                            archivoModelos.fechaCreacion = DateTime.Now;
                            archivoModelos.id = 0;
                            archivoModelos.nombreArchivo = nombreOriginal;
                            archivoModelos.modeloID = obj.id;
                            archivoModelos.RutaArchivo = Ruta;
                            archivoModelos.usuario = getUsuario().id;
                            obj.Ruta = "T";
                            archivosModelosFactoryServices.getArchivoModelosFactoryServices().GuardarArchivos(archivoModelos);
                        }
                    }
                }
                //foreach (var item in listaDocumentos)
                //{
                //    var archivoobj = archivosModelosFactoryServices.getArchivoModelosFactoryServices().getlistaByID(item);
                //}
                modeloEquipoFactoryServices.getModeloEquipoService().Guardar(obj);
                modeloEquipoFactoryServices.getModeloEquipoService().GuardarSubconjuntos(listaSubConjuntos, listaNumParte, obj.id);
                result.Add("idControl", obj.id);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArchivosModificacion(int obj)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var archivos = archivosModelosFactoryServices.getArchivoModelosFactoryServices().getlistaByModelo(obj);

                List<ModeloArchivoDTO> ListaArchivosGrid = new List<ModeloArchivoDTO>();

                ListaArchivosGrid = archivos.Select(x => new ModeloArchivoDTO
                {
                    id = x.id,
                    nombre = x.nombreArchivo,
                    FechaCreacion = x.fechaCreacion.ToShortDateString(),
                    ruta = x.RutaArchivo

                }).ToList();


                result.Add("ListaArchivosGrid", ListaArchivosGrid.Select(x => new { modeloId = x.id, Nombre = x.nombre, descarga = x.ruta }));
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

            var Archivo = archivosModelosFactoryServices.getArchivoModelosFactoryServices().getlistaByID(id);

            return File(Archivo.RutaArchivo, "multipart/form-data", Archivo.nombreArchivo);
        }

        public ActionResult FillCboConjunto() 
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, subConjuntoModeloFactoryServices.getsubConjuntoModeloFactoryServices().FillCboConjunto().Select(x => new { Value = x.id, Text = x.descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboSubConjunto(int idConjunto) {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, subConjuntoModeloFactoryServices.getsubConjuntoModeloFactoryServices().FillCboSubConjunto(idConjunto).Select(x => new { Value = x.id, Text = x.descripcion }).OrderBy(x => x.Text));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillGridSubConjunto(int idModelo) 
        {
            var result = new Dictionary<string, object>();
            try
            {
                var subconjuntos = subConjuntoModeloFactoryServices.getsubConjuntoModeloFactoryServices().FillGridSubConjunto(idModelo)
                .Select
                (x => new { 
                    subconjuntoID = x.subconjuntoID,
                    conjunto = x.subconjunto.conjunto.descripcion,
                    subconjunto = x.subconjunto.descripcion,
                    numParte = x.numParte
                }).ToList();
                result.Add("subconjuntos", subconjuntos);
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