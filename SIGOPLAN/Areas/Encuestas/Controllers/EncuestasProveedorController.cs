using Core.DAO.Encuestas;
using Core.DAO.Principal.Usuarios;
using Core.DTO;
using Core.DTO.Encuestas;
using Core.DTO.Encuestas.Proveedores;
using Core.DTO.Encuestas.Proveedores.Reportes;
using Core.Entity.Encuestas;
using Core.Enum.Encuesta.Proveedores;
using Core.Enum.Multiempresa;
using Data.Factory.Encuestas;
using Data.Factory.Principal.Usuarios;
using Infrastructure.DTO;
using Infrastructure.Utils;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Encuestas.Controllers
{
    public class EncuestasProveedorController : BaseController
    {
        private IUsuarioDAO usuarioFS;
        private IEncuestasProveedorDAO encuestaProvFS;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            encuestaProvFS = new EncuestasProveedoresFactoryServices().getEncuestasProveedoresFactoryServices();
            usuarioFS = new UsuarioFactoryServices().getUsuarioService();
            base.OnActionExecuting(filterContext);
        }

        // GET: Encuestas/EncuestasProveedor
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            if (Session["realizarEncuestaTop20PorCompras"] != null && (bool)Session["realizarEncuestaTop20PorCompras"])
            {
                ViewBag.realizarEncuestaTop20PorCompras = true;
            }
            return View();
        }

        public ActionResult Responder()
        {
            return View();
        }

        public ActionResult FillCboCompradores()
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<int> ListaPuestos = new List<int>();
                ListaPuestos.Add(6);
                ListaPuestos.Add(14);

                var idUsuario = getUsuario().id;
                var listaCC = usuarioFS.GetUsuariosByPuesto(ListaPuestos);


                List<int> usuarioEx = new List<int>();

                usuarioEx.Add(2262);
                usuarioEx.Add(2259);
                usuarioEx.Add(3741);
                usuarioEx.Add(3905);
                usuarioEx.Add(3916);
                usuarioEx.Add(1174);
                usuarioEx.Add(2266);

                foreach (var item in listaCC)
                {
                    var existe = usuarioEx.Exists(y => y.Equals(item.id));

                    if (existe)
                        item.estatus = false;
                }
                var usuariosPuesto = listaCC.Where(x => x.estatus != false).ToList();

                var usuariosQueHanRealizadoEncuestas = encuestaProvFS.getUsuariosRealizadoEncuestas();

                foreach (var item in usuariosQueHanRealizadoEncuestas)
                {
                    if (usuariosPuesto.FirstOrDefault(x => x.id == item.id) == null)
                    {
                        usuariosPuesto.Add(item);
                    }
                }

                result.Add(ITEMS, usuariosPuesto.Select(x => new { Value = x.id, Text = x.nombre.ToUpper() + " " + x.apellidoPaterno.ToUpper() + " " + (string.IsNullOrEmpty(x.apellidoMaterno) ? "" : x.apellidoMaterno.Substring(0, 1)).ToUpper() + "." }));
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult getProveedores(string term)
        {
            var items = encuestaProvFS.getNombreProveedores(term);// capturaOTFactoryServices.getCapturaOTFactoryServices().getCatEmpleados(term);

            var filteredItems = items.Select(x => new { id = x.noProveedor, label = x.nomProveedor });

            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult saveEncuestaResult(List<tblEN_ResultadoProveedores> obj, tblEN_ResultadoProveedoresDet objSingle, int encuestaID, int tipoEncuesta)
        {
            var respuesta = encuestaProvFS.saveEncuestaResult(obj, objSingle, tipoEncuesta);
            return Json(respuesta);
        }

        [HttpPost]
        public JsonResult saveEncuestaResultReq(List<tblEN_ResultadoProveedorRequisiciones> obj, tblEN_ResultadoProveedorRequisicionDet objSingle, int tipoEncuesta)
        {
            var respuesta = encuestaProvFS.saveEncuestaResultReq(obj, objSingle, tipoEncuesta);
            return Json(respuesta);
        }

        public ActionResult saveEncuestaResultRequisicion(List<tblEN_ResultadoProveedorRequisiciones> obj, tblEN_ResultadoProveedorRequisicionDet objSingle, int encuestaID, string comentario)
        {
            var result = new Dictionary<string, object>();

            try
            {
                objSingle.evaluadorID = vSesiones.sesionUsuarioDTO.id;

                foreach (var i in obj)
                {
                    i.usuarioRespondioID = vSesiones.sesionUsuarioDTO.id;
                    i.fecha = DateTime.Now;
                    i.tipoEncuesta = encuestaID; //i.tipoEncuesta = 2;
                }
                encuestaProvFS.saveEncuestaResultRequisiciones(obj, objSingle, comentario);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cboEncuestas(int tipoEncuesta)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = encuestaProvFS.getEncuestasByTipo(tipoEncuesta);

                result.Add(ITEMS, res.Where(w => w.estatus).Select(x => new ComboDTO
                {
                    Value = x.id,
                    Text = x.titulo

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

        public JsonResult cboEncuestasConsultas(int tipoEncuesta)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = encuestaProvFS.getEncuestasByTipo(tipoEncuesta);

                result.Add(ITEMS, res.Select(x => new ComboDTO
                {
                    Value = x.id,
                    Text = x.titulo

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

        public ActionResult saveEncuesta(tblEN_EncuestaProveedores encuesta, List<tblEN_PreguntasProveedores> listObj, bool updateInfo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (updateInfo)
                {

                    encuestaProvFS.updateEncuesta(encuesta, listObj);
                }
                else
                {
                    encuesta.creadorID = vSesiones.sesionUsuarioDTO.id;
                    encuesta.fecha = DateTime.Now;
                    encuesta.estatus = true;
                    var dataReturn = encuestaProvFS.saveEncuestasProveedores(encuesta, listObj);

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

        public ActionResult getEncuesta(int encuestaID)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var objEncuesta = encuestaProvFS.getEncuesta(encuestaID);
                var objListProveedores = encuestaProvFS.getPreguntasProveedores(encuestaID);

                result.Add("id", objEncuesta.id);
                result.Add("titulo", objEncuesta.titulo);
                result.Add("descripcion", objEncuesta.descripcion);
                result.Add("tipoEncuesta", objEncuesta.tipoEncuesta);
                result.Add("preguntas", objListProveedores.ToList().Select(x => new { x.encuestaID, x.estatus, x.id, x.orden, x.pregunta, x.tipo, x.ponderacion }));
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
        public JsonResult ResponderEncuestaTop20(int tipoEncuesta, int encuestaID, int numeroProveedor, string numeroProveedorPeru)
        {
            var r = new Respuesta();

            if ((EmpresaEnum)vSesiones.sesionEmpresaActual == EmpresaEnum.Peru)
            {
                r = encuestaProvFS.ResponderEncuestaTop20Peru(tipoEncuesta, encuestaID, numeroProveedorPeru);
            }
            else
            {
                r = encuestaProvFS.ResponderEncuestaTop20(tipoEncuesta, encuestaID, numeroProveedor);
            }
            
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ResponderEncuesta(int encuestaID, int numeroOC, string centrocostos)
        {
            var result = new Dictionary<string, object>();
            List<PreguntasDTO> objListaPreguntas = new List<PreguntasDTO>();
            bool RespuestaEncuesta = false;
            try
            {

                var objEncuesta = encuestaProvFS.getEncuesta(encuestaID);
                var getDatosProveedor = encuestaProvFS.datosProveedor(encuestaID, numeroOC, centrocostos);

                if (!getDatosProveedor.estadoEncuesta)
                {
                    if (getDatosProveedor.numeroOC != 0)
                    {
                        objListaPreguntas = encuestaProvFS.getPreguntasProveedores(encuestaID);
                        result.Add("ExisteOC", true);
                    }
                    else
                    {
                        result.Add("ExisteOC", false);
                    }
                }
                else
                {
                    RespuestaEncuesta = true;
                    result.Add("ExisteOC", true);
                }

                var getDatosEncuestaProveedor = "";

                result.Add("id", objEncuesta.id);
                result.Add("RespuestaEncuesta", RespuestaEncuesta);
                result.Add("getDatosProveedor", getDatosProveedor);
                result.Add("titulo", objEncuesta.titulo);
                result.Add("descripcion", objEncuesta.descripcion);
                result.Add("tipoEncuesta", objEncuesta.tipoEncuesta);
                result.Add("preguntas", objListaPreguntas.ToList().Select(x => new { x.encuestaID, x.estatus, x.id, x.orden, x.pregunta, x.tipo, x.ponderacion }));
                result.Add("evaluador", (getDatosProveedor.encuestaID != 0 ? getDatosEncuestaProveedor : getUsuario().nombre));

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult LoadEncuestasProveedores(int estatus, DateTime fechaInicio, DateTime fechaFin)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var objListProveedores = encuestaProvFS.loadEncuestas(estatus, fechaInicio, fechaFin);

                result.Add("objListProveedores", objListProveedores);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult LoadEncuestasProveedoresOC(int estatus, DateTime fechaInicio, DateTime fechaFin, List<int> compradores)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var objListProveedores = encuestaProvFS.evaluacionesRespondidas(estatus, fechaInicio, fechaFin, compradores);

                result.Add("objListProveedores", objListProveedores.Select(x => new
                {
                    id = x.id,
                    centrocostos = x.centrocostos,
                    FechaOC = x.fechaOC.ToShortDateString(),
                    numeroOC = x.centrocostos + "-" + x.numeroOC,
                    NombreProveedor = x.nombreProveedor,
                    NumProveedor = x.numProveedor,
                    Comentarios = x.comentarios,
                    numOC = x.numeroOC,
                    cc = x.centrocostos,
                    estadoEncuesta = x.estadoEncuesta,
                    ponderacion = x.ponderacion,
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

        public ActionResult LoadEncuestasProveedoresRequisiciones(int estatus, DateTime fechaInicio, DateTime fechaFin, List<int> compradores)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var objListProveedores = encuestaProvFS.loadRequisicionesByFiltros(estatus, fechaInicio, fechaFin, compradores);

                result.Add(SUCCESS, true);
                result.Add("objListProveedores", objListProveedores);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ResponderEncuestaRequisicion(int encuestaID, int noRequisicion, string centrocostos)
        {
            var result = new Dictionary<string, object>();
            List<PreguntasDTO> objListaPreguntas = new List<PreguntasDTO>();
            try
            {
                bool RespuestaEncuesta = false;
                var objEncuesta = encuestaProvFS.getEncuesta(encuestaID);
                // var getDatosProveedor = encuestasProveedoresFactoryServices.datosProveedor(encuestaID, numeroOC, centrocostos);
                var getDatosEncuesta = encuestaProvFS.loadEncuestasByRequisicion(noRequisicion, centrocostos);

                if (!getDatosEncuesta.estatus)
                {
                    objListaPreguntas = encuestaProvFS.getPreguntasProveedores(encuestaID);
                }
                else
                {

                    if (getDatosEncuesta.id != 0)
                    {
                        RespuestaEncuesta = true;
                    }
                    else
                    {
                        objListaPreguntas = encuestaProvFS.getPreguntasProveedores(encuestaID);
                    }

                }

                var getDatosEncuestaProveedor = "";

                result.Add("id", objEncuesta.id);
                result.Add("RespuestaEncuesta", RespuestaEncuesta);
                result.Add("getDatosProveedor", getDatosEncuesta);
                result.Add("titulo", objEncuesta.titulo);
                result.Add("descripcion", objEncuesta.descripcion);
                result.Add("tipoEncuesta", objEncuesta.tipoEncuesta);
                result.Add("preguntas", objListaPreguntas.ToList().Select(x => new { x.encuestaID, x.estatus, x.id, x.orden, x.pregunta, x.tipo, x.ponderacion }));
                result.Add("evaluador", (RespuestaEncuesta ? getDatosEncuestaProveedor : getUsuario().nombre));

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SetGraficasEvaluaciones(DateTime fechainicio, DateTime fechafin, int encuesta, int tipoEncuesta, List<int> compradores)
        {
            if (compradores == null)
            {
                compradores = new List<int>();
            }
            var result = new Dictionary<string, object>();
            List<ResEvaluacionProveedoresDTO> ListResEvaluacionProveedoresDTO = new List<ResEvaluacionProveedoresDTO>();

            //compradores.Add(1174);
            try
            {
                var rawInfo = encuestaProvFS.getGraficaEvaluacionProveedores(fechainicio, fechafin.AddHours(23).AddMinutes(59), encuesta, tipoEncuesta, compradores);
                var rawData1 = rawInfo.ProveedoresList;
                var rawData2 = rawInfo.ProveedoresListOC;
                     
                var lista1 = rawData1 != null ? rawData1.Where(c => compradores.Contains(c.usuarioRespondioID)).GroupBy(x => x.encuestaFolioID).ToList() : null;
                var lista2 = rawData2 != null ? rawData2.Where(c => compradores.Contains(c.usuarioRespondioID)).GroupBy(x => x.encuestaFolioID).ToList() : null;
                //var lista2 = rawData2 != null ? rawData2.Where(c => compradores.Contains(c.usuarioRespondioID)).GroupBy(x => x.Detalle.numProveedor).ToList() : null;

                //tipoEncuesta = encuestaProvFS.getTipoEncuesta(tipoEncuesta);

                var listaKey = encuesta == 1 ? lista1.Select(x => x.Key).ToList() : lista2.Select(x => x.Key).ToList();
                       
                var rawListProveedores = listaKey;

                var GetEncuestas = encuestaProvFS.getProveedoresCalificaciones(rawListProveedores, encuesta);
                var GetEncuestasEstrellas = encuestaProvFS.getProveedoresCalificacionesEstrellas(rawListProveedores, encuesta);

                Session["ComboDtoProveedores"] = encuestaProvFS.ComboDtoProveedores(rawListProveedores, encuesta);

                var ResultListaProveedores = encuestaProvFS.ListaProveedores(rawListProveedores, encuesta);

                //  var rawContestadas = encuestasProveedoresFactoryServices.getGraficaEvaluacionCompradores(fechainicio, fechafin, tipoEncuesta).GroupBy(x => x.encuestaFolioID).ToList();

                List<int> resultGraphBueno = new List<int>();
                List<int> resultGraphRegular = new List<int>();
                List<int> resultGraphMalo = new List<int>();

                var countBuenos = 0;
                var countMalos = 0;
                var countRegular = 0;

                if (encuesta == 1)
                {
                    foreach (var item in lista1)
                    {
                        ResEvaluacionProveedoresDTO objEvaluaciones = new ResEvaluacionProveedoresDTO();
                        GraphEvaluacionProveedoresDTO objGrap = new GraphEvaluacionProveedoresDTO();
                        var objraw = item;
                        var listaPreguntas = item.Select(x => x.pregunta).ToList();

                        var getRespuestasSI = item.Where(x => x.calificacion >= 3);
                        var resultadosRespuestasSI = getRespuestasSI.Select(x => x.preguntaID).ToList();



                        decimal SumaCalificacion = listaPreguntas.Where(x => resultadosRespuestasSI.Contains(x.id)).Sum(r => r.ponderacion);

                        if (SumaCalificacion >= 0.75M && SumaCalificacion <= 1)
                        {
                            countBuenos++;


                        }
                        else if (SumaCalificacion >= 0.45M && SumaCalificacion < 0.75M)
                        {
                            countRegular++;


                        }
                        else
                            if (SumaCalificacion >= 0 && SumaCalificacion < .45M)
                            {
                                countMalos++;

                            }


                        var dataEncuesta = GetEncuestas.FirstOrDefault(x => x.folioID == item.Key);
                        if (objEvaluaciones.folioID != null)
                        {
                            ListResEvaluacionProveedoresDTO.Add(objEvaluaciones);
                        }

                    }
                }
                else
                {
                    foreach (var item in lista2)
                    {
                        ResEvaluacionProveedoresDTO objEvaluaciones = new ResEvaluacionProveedoresDTO();
                        GraphEvaluacionProveedoresDTO objGrap = new GraphEvaluacionProveedoresDTO();
                        var objraw = item;
                        var listaPreguntas = item.Select(x => x.pregunta).ToList();

                        var getRespuestasSI = item.Where(x => x.calificacion >= 3);
                        var resultadosRespuestasSI = getRespuestasSI.Select(x => x.preguntaID).ToList();



                        decimal SumaCalificacion = listaPreguntas.Where(x => resultadosRespuestasSI.Contains(x.id)).Sum(r => r.ponderacion);

                        if (SumaCalificacion >= 0.75M && SumaCalificacion <= 1)
                        {
                            countBuenos++;


                        }
                        else if (SumaCalificacion >= 0.45M && SumaCalificacion <= 0.74M)
                        {
                            countRegular++;


                        }
                        else
                            if (SumaCalificacion >= 0 && SumaCalificacion <= .44M)
                            {
                                countMalos++;

                            }


                        var dataEncuesta = GetEncuestas.FirstOrDefault(x => x.folioID == item.Key);
                        if (objEvaluaciones.folioID != null)
                        {
                            ListResEvaluacionProveedoresDTO.Add(objEvaluaciones);
                        }

                    }
                }

            
                result.Add("ResultListaProveedores", ResultListaProveedores);
                result.Add("dtResultado", GetEncuestas);
                result.Add("dtEncuestasEstrellas", GetEncuestasEstrellas);
                result.Add("dtCompradores", rawInfo.CompradoresList.Where(x => compradores.Contains(x.idComprador)).ToList());
                result.Add("resultGraphBueno", countBuenos);
                result.Add("resultGraphRegular", countRegular);
                result.Add("resultGraphMalo", countMalos);

                result.Add(SUCCESS, true);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetGraficasEvaluacionesEstrellas(DateTime fechaInicio, DateTime fechaFin, int encuesta, int tipoEncuesta, List<int> compradores)
        {
            if (compradores == null)
            {
                compradores = new List<int>();
            }

            var result = new Dictionary<string, object>();

            try
            {
                List<ResEvaluacionProveedoresDTO> ListResEvaluacionProveedoresDTO = new List<ResEvaluacionProveedoresDTO>();
                compradores.Add(1174);

                var rawInfo = encuestaProvFS.getGraficaEvaluacionProveedores(fechaInicio, fechaFin.AddHours(23).AddMinutes(59), encuesta, tipoEncuesta, compradores);
                var rawData1 = rawInfo.ProveedoresList;
                var rawData2 = rawInfo.ProveedoresListOC;

                var lista1 = rawData1 != null ? rawData1.Where(c => compradores.Contains(c.usuarioRespondioID)).GroupBy(x => x.encuestaFolioID).ToList() : null;
                var lista2 = rawData2 != null ? rawData2.Where(c => compradores.Contains(c.usuarioRespondioID)).GroupBy(x => x.encuestaFolioID).ToList() : null;

                var listaKey = encuesta == 1 ? lista1.Select(x => x.Key).ToList() : lista2.Select(x => x.Key).ToList();

                var rawListProveedores = listaKey;

                //var rawData = rawInfo.ProveedoresList.Where(c => compradores.Contains(c.usuarioRespondioID)).GroupBy(x => x.encuestaFolioID).ToList();
                var GetEncuestas = encuestaProvFS.getProveedoresCalificaciones(rawListProveedores, encuesta);
                var GetEncuestasEstrellas = encuestaProvFS.getProveedoresCalificacionesEstrellas(rawListProveedores, encuesta);
                var ResultListaProveedores = encuestaProvFS.ListaProveedoresEstrellas(rawListProveedores, encuesta);

                var countPesimos = 0;
                var countMalos = 0;
                var countRegulares = 0;
                var countAceptables = 0;
                var countEstupendos = 0;

                if (encuesta == 1)
                {
                    foreach (var item in lista1)
                    {
                        ResEvaluacionProveedoresDTO objEvaluaciones = new ResEvaluacionProveedoresDTO();
                        GraphEvaluacionProveedoresDTO objGrap = new GraphEvaluacionProveedoresDTO();

                        var objraw = item;
                        var listaPreguntas = item.Select(x => x.pregunta).ToList();

                        int SumaCalificacion = (int)(item.Select(x => x.calificacion).Sum() / item.Count());

                        switch (SumaCalificacion)
                        {
                            case 1:
                                countPesimos++;
                                break;
                            case 2:
                                countMalos++;
                                break;
                            case 3:
                                countRegulares++;
                                break;
                            case 4:
                                countAceptables++;
                                break;
                            case 5:
                                countEstupendos++;
                                break;
                        }

                        var dataEncuesta = GetEncuestas.FirstOrDefault(x => x.folioID == item.Key);
                        if (objEvaluaciones.folioID != null)
                        {
                            ListResEvaluacionProveedoresDTO.Add(objEvaluaciones);
                        }
                    }
                }
                else
                {
                    foreach (var item in lista2)
                    {
                        ResEvaluacionProveedoresDTO objEvaluaciones = new ResEvaluacionProveedoresDTO();
                        GraphEvaluacionProveedoresDTO objGrap = new GraphEvaluacionProveedoresDTO();

                        var objraw = item;
                        var listaPreguntas = item.Select(x => x.pregunta).ToList();

                        int SumaCalificacion = (int)(item.Select(x => x.calificacion).Sum() / item.Count());

                        switch (SumaCalificacion)
                        {
                            case 1:
                                countPesimos++;
                                break;
                            case 2:
                                countMalos++;
                                break;
                            case 3:
                                countRegulares++;
                                break;
                            case 4:
                                countAceptables++;
                                break;
                            case 5:
                                countEstupendos++;
                                break;
                        }

                        var dataEncuesta = GetEncuestas.FirstOrDefault(x => x.folioID == item.Key);
                        if (objEvaluaciones.folioID != null)
                        {
                            ListResEvaluacionProveedoresDTO.Add(objEvaluaciones);
                        }
                    }
                }

              
                result.Add("resultPesimo", countPesimos);
                result.Add("resultGraphMalo", countMalos);
                result.Add("resultGraphRegular", countRegulares);
                result.Add("resultGraphAceptable", countAceptables);
                result.Add("resultGraphEstupendo", countEstupendos);
                result.Add("dtCompradores", rawInfo.CompradoresList.Where(x => compradores.Contains(x.idComprador)).ToList());
                result.Add("dtEncuestasEstrellas", GetEncuestasEstrellas);
                result.Add("ResultListaProveedores", ResultListaProveedores);
                
                result.Add("preguntas", rawInfo.Preguntas);
                result.Add("calificaciones", rawInfo.Calificaciones);

                result.Add("provMasEvaluados", rawInfo.ProveedoresMasEvaluados);
                result.Add("calificacionesMasEvaluados", rawInfo.CalificacionesMasEvaluados);

                result.Add("provPeorEvaluados", rawInfo.ProveedoresPeorEvaluados);
                result.Add("calificacionesPeorEvaluados", rawInfo.CalificacionesPeorEvaluados);

                result.Add("provBest", rawInfo.ProveedoresBest);
                result.Add("calificacionesBest", rawInfo.CalificacionesBest);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }



            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult setGraficaPreguntasProv(int numProv, int tipoEncuesta, int encuestaID, DateTime fechaIni, DateTime fechaFin, List<int> listaUsuario)
        {
            if (listaUsuario == null)
            {
                listaUsuario = new List<int>();
            }

            return Json(encuestaProvFS.setGraficaPreguntasProv(numProv, tipoEncuesta, encuestaID, fechaIni, fechaFin, listaUsuario));
        }

        public JsonResult setGraficaMensualProv(int numProv, int tipoEncuesta, int encuestaID, List<int> listaUsuario)
        {
            if (listaUsuario == null)
            {
                listaUsuario = new List<int>();
            }

            return Json(encuestaProvFS.setGraficaMensualProv(numProv, tipoEncuesta, encuestaID, listaUsuario));
        }

        public ActionResult loadCboProveedores()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, Session["ComboDtoProveedores"]);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboTipoEncuesta()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cboTipo = encuestaProvFS.comboTipoEncuesta();
                result.Add(ITEMS, cboTipo);
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
        public JsonResult GetProveedoresTop20(int tipoEncuestaId)
        {
            var respuesta = encuestaProvFS.GetProveedoresTop20(vSesiones.sesionUsuarioDTO.id, tipoEncuestaId);
            return Json(new { success = respuesta.Success ? "True" : "False", items = respuesta.Value, message = respuesta.Message });
        }
    }
}