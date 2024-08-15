using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Entity.Cursos;
using SIGOPLAN.Controllers;
using Data.Factory.Cursos;
using Data.DAO.Principal.Usuarios;
using Core.DTO;
namespace SIGOPLAN.Areas.Curso.Controllers
{
    public class CursoController : BaseController
    {
        //raguilar instancias
        private CursosFactoryServices objCursosFactoryService = new CursosFactoryServices();
        int idCurso;
        //private AditivaDeductivaFactoryService objAditivaDeductivaFactoryServices = new AditivaDeductivaFactoryService();
        // GET: Curso/Curso
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GestorCursos()
        {
            return View();
        }
        public ActionResult AsignacionCurso()
        {
            return View();
        }
        public ActionResult CreacionExamen()
        {
            return View();
        }
        public ActionResult CursoExamen()
        {
            return View();
        }
        //int idModulo;
        [ValidateInput(false)]
        public ActionResult saveCurso(tblCU_Curso objCurso, List<tblCU_Modulo> lstobjModulo, List<tblCU_ModuloDet> lstobjModuloDet, bool NuevoCurso, List<tblCU_Modulo> arraylstEliminaMod, List<tblCU_ModuloDet> arraylstEliminaModDet)
        {
            var BanderaModicicacion = false;
            var result = new Dictionary<string, object>();
            objCurso.usuarioCap = getUsuario().id;
            objCurso.nomUsuarioCap = getUsuario().nombre;
            objCurso.fechaCaptura = DateTime.Now;
            int idMod = 0;
            try
            {
                if (objCurso.id == 0)
                {
                    idCurso = objCursosFactoryService.getCursosService().GuardarCurso(objCurso).id;
                    objCurso.folio = objCurso.usuarioCap + "-" + idCurso;
                    objCursosFactoryService.getCursosService().GuardarCurso(objCurso);
                    bool bandera = false;
                    int moduloIdTmp = 0;
                    //raguilar guardado
                    if (NuevoCurso)
                    {
                        foreach (var modulo in lstobjModulo)
                        {
                            modulo.estado = false;
                            modulo.idCurso = idCurso;

                            if (lstobjModuloDet != null)
                            {
                                foreach (var modulodet in lstobjModuloDet)
                                {
                                    if (modulodet.idModulo == modulo.id && modulodet.estado == false)
                                    {
                                        if (bandera == false)
                                        {
                                            bandera = true;
                                            moduloIdTmp = modulo.id;
                                            modulo.id = 0;
                                            idMod = objCursosFactoryService.getCursosService().GuardarModulo(modulo).id;
                                        }
                                        if (modulodet.estado == false)
                                        {
                                            modulodet.idModulo = idMod;
                                            objCursosFactoryService.getCursosService().GuardarModuloDet(modulodet);
                                            modulo.id = moduloIdTmp;

                                        }
                                    }
                                }
                                if (bandera == false)
                                {
                                    modulo.id = 0;
                                    objCursosFactoryService.getCursosService().GuardarModulo(modulo);
                                }
                                bandera = false;
                            }
                            else
                            {
                                if (bandera == false)
                                {
                                    modulo.id = 0;
                                    objCursosFactoryService.getCursosService().GuardarModulo(modulo);
                                }
                                bandera = false;

                            }
                        }
                    }
                    result.Add("folio", objCurso.folio);
                }
                else
                {
                    BanderaModicicacion = true;
                    idCurso = objCursosFactoryService.getCursosService().GuardarCurso(objCurso).id;
                    if (arraylstEliminaMod != null)
                    {
                        foreach (var modEliminar in arraylstEliminaMod)
                        {
                            objCursosFactoryService.getCursosService().EliminaModulo(modEliminar.id);
                            objCursosFactoryService.getCursosService().EliminaModuloDet(modEliminar.id);
                        }
                    }

                    if (arraylstEliminaModDet != null)
                    {
                        foreach (var modEliminarDet in arraylstEliminaModDet)
                        {
                            objCursosFactoryService.getCursosService().EliminaModuloDetbyId(modEliminarDet.id);
                        }
                    }

                    if (lstobjModulo != null)
                    {
                        foreach (var objmodedit in lstobjModulo)
                        {
                            objmodedit.idCurso = idCurso;
                            if (objmodedit.estado == true)//si existe el modulo y el detalle ""
                            {
                                idMod = objCursosFactoryService.getCursosService().GuardarModulo(objmodedit).id;//guarda el modulo
                                foreach (var objmoddetedit in lstobjModuloDet)
                                {
                                    if (objmoddetedit.idModulo == idMod && objmoddetedit.estado == true)
                                    {
                                        objCursosFactoryService.getCursosService().GuardarModuloDet(objmoddetedit);//guard detalle
                                    }
                                    else if (objmoddetedit.idModulo == idMod && objmoddetedit.estado == false)
                                    {
                                        bool tempExiste = objCursosFactoryService.getCursosService().ComparaPagina(objmoddetedit.pagina, objmodedit.id);//guard detalle
                                        if (tempExiste == false)
                                        {
                                            objCursosFactoryService.getCursosService().GuardarModuloDet(objmoddetedit);//guard detalle    
                                        }
                                    }
                                }
                            }
                        }
                        bool flaglocalmod = false;
                        int idtmpMod = 0;
                        foreach (var objmodedit in lstobjModulo)
                        {
                            if (objmodedit.estado == false)//si no existe el modulo ni el detalle
                            {
                                if (flaglocalmod == false)
                                {
                                    idtmpMod = objmodedit.id;
                                    flaglocalmod = true;
                                    objmodedit.id = 0;
                                    idMod = objCursosFactoryService.getCursosService().GuardarModulo(objmodedit).id;
                                }
                                foreach (var objmoddetedit in lstobjModuloDet)
                                {
                                    if (objmoddetedit.idModulo == idtmpMod && objmoddetedit.estado == false)
                                    {
                                        objmoddetedit.idModulo = idMod;
                                        objmoddetedit.id = 0;
                                        objCursosFactoryService.getCursosService().GuardarModuloDet(objmoddetedit);
                                    }
                                }
                                flaglocalmod = false;
                            }
                        }
                    }
                    var temp = objCursosFactoryService.getCursosService().GetListCursos(objCurso.id, "", "", 1).First();
                    result.Add("modificacion", BanderaModicicacion);
                    result.Add("folio", temp.folio);
                    //result.Add("folio", temp.folio);
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
        //obtener listado de cursos 9/1/18 raguilar
        public ActionResult GetListCursos()
        {
            List<tblCU_Curso> lstobjCurso = new List<tblCU_Curso>();
            lstobjCurso = objCursosFactoryService.getCursosService().GetListCursos(0, "", "", 1).ToList();
            lstobjCurso = ObtencionEditables(lstobjCurso);
            return Json(lstobjCurso, JsonRequestBehavior.AllowGet);

        }
        //habilitado btn editar seung los permisos
        public List<tblCU_Curso> ObtencionEditables(List<tblCU_Curso> lstobjCurso)
        {
            int usuarioLog = getUsuario().id;
            List<tblCU_Curso> listaMostrar = new List<tblCU_Curso>();
            foreach (var objCurso in lstobjCurso)
            {
                //var ud = new UsuarioDAO();
                if (objCurso.usuarioCap == usuarioLog)//solo el aprobador  y el capturista peuden visualizar el listado
                {
                    //var isEditarTodo = ud.getViewAction(vSesiones.sesionCurrentView, "EditarTodoAditiva");
                    //if (isEditarTodo) tabla vista permisos vista se dan de alta los usuarios //sistemas vistas acciones
                    //{
                    objCurso.editable = true;
                    //}

                    listaMostrar.Add(objCurso);
                }
            }
            return listaMostrar;
        }
        //obtener por id el curso a editar
        public ActionResult ObtenerCursobyId(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<tblCU_Modulo> lstobjModulo = new List<tblCU_Modulo>();
                List<tblCU_ModuloDet> lstobjModuloDet = new List<tblCU_ModuloDet>();
                lstobjModulo = objCursosFactoryService.getCursosService().getModuloid(id);
                foreach (var Modulo in lstobjModulo)
                {
                    //lstobjModuloDet = objCursosFactoryService.getCursosService().getModuloDetid(Modulo.id);
                    lstobjModuloDet.AddRange(objCursosFactoryService.getCursosService().getModuloDetid(Modulo.id));
                }
                //liberar peso en consulta
                //foreach (var ModuloDet in lstobjModuloDet)
                //{
                //    ModuloDet.contenido = "";
                //}

                //result.Add("objCurso", objCursosFactoryService.getCursosService().getFiltroCurso(id, "", ""));
                result.Add("objCurso", objCursosFactoryService.getCursosService().GetListCursos(id, "", "", 1));
                result.Add("objModulo", lstobjModulo);
                result.Add("objModuloDet", lstobjModuloDet);
                //lstObjAditivaDeductiva.Sort((p, q) => string.Compare(p.puesto, q.puesto));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //obtener paginas por el id del modulo
        public ActionResult ObtenerPagbyId(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<tblCU_ModuloDet> lstobjModuloDet = new List<tblCU_ModuloDet>();
                lstobjModuloDet.AddRange(objCursosFactoryService.getCursosService().getModuloDetid(id));
                //lstobjModuloDet.Sort((p, q) => string.Compare(p.pagina, q.pagina));
                result.Add("objModuloDet", lstobjModuloDet);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //carga curso 12/1/18 14:53
        //raguilar cargando centro de costos
        public ActionResult FillComboCurso()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = objCursosFactoryService.getCursosService().FillComboCurso();
                result.Add(ITEMS, list.OrderBy(x => x.Text));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //creacion de examen 15/01/18
        int idExamen = 0;
        public ActionResult guardarexamen(tblCU_Examen objExamen, List<tblCU_ExamenPregunta> lstobjExamenPregunta, List<tblCU_ExamenRespuesta> lstobjExamenRespuesta, bool NuevoExamen)
        {
            var result = new Dictionary<string, object>();
            objExamen.usuarioCap = getUsuario().id;
            objExamen.nomUsuarioCap = getUsuario().nombre;
            objExamen.fechaCaptura = DateTime.Now;
            try
            {
                if (objExamen.id == 0)//agregar nuevo curso
                {
                    idExamen = objCursosFactoryService.getCursosService().GuardarExamen(objExamen).id;
                    objExamen.folio = objExamen.usuarioCap + "-" + idExamen;
                    objCursosFactoryService.getCursosService().GuardarExamen(objExamen);
                    //objFormatoADitivaDTO.objAditivaDeductiva.folio = objFormatoADitivaDTO.objAditivaDeductiva.cCid + "-" + objFormatoADitivaDTO.objAditivaDeductiva.id;
                    //objFormatoADitivaDTO.objAditivaDeductiva = objAditivaDeductivaFactoryServices.getAditivaDeductivaService().GuardarAditivaDeduc(objFormatoADitivaDTO.objAditivaDeductiva);


                    //raguilar guardado



                }
                else//actualizacion
                {


                }

                bool bandera = false;
                int idPreg = 0;
                int preguntaIdTmp = 0;
                if (NuevoExamen)
                {
                    foreach (var pregunta in lstobjExamenPregunta)//recorrimiento del modulo
                    {
                        pregunta.idExamen = idExamen;
                        foreach (var respuesta in lstobjExamenRespuesta)//recorrimiento del modulo
                        {
                            if (respuesta.idPregunta == pregunta.id)
                            {
                                if (bandera == false)
                                {
                                    bandera = true;
                                    preguntaIdTmp = pregunta.id;
                                    pregunta.id = 0;
                                    idPreg = objCursosFactoryService.getCursosService().GuardarPregunta(pregunta).id;
                                }
                                respuesta.idPregunta = idPreg;
                                objCursosFactoryService.getCursosService().GuardarRespuesta(respuesta);
                                pregunta.id = preguntaIdTmp;
                            }
                        }
                        bandera = false;
                    }
                }
                result.Add("folio", objExamen.folio);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //eliminado desde gestor de curso
        public ActionResult EliminaGestor(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var tmp = objCursosFactoryService.getCursosService().EliminaCurso(id);
                result.Add("Elimina", tmp);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //gestor busqueda filtro
        public ActionResult filtroCurso(int id, string folio, string nombre, int combo)
        {
            List<tblCU_Curso> lstobjCurso = new List<tblCU_Curso>();
            lstobjCurso = objCursosFactoryService.getCursosService().GetListCursos(id, folio, nombre, combo).ToList();
            lstobjCurso = ObtencionEditables(lstobjCurso);
            return Json(lstobjCurso, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetNombreCurso(int IdCurso)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("nombreCurso", objCursosFactoryService.getCursosService().GetListCursos(IdCurso, "", "", 0).First().nombreCurso);
            }
            catch (Exception)
            {
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //GetDeptos
        //var deps = _context.tblP_Departamento.ToList();
        //raguilar visualizacion de departamentos
        public ActionResult GetListDeptos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("departamentos", objCursosFactoryService.getCursosService().GetListDeptos().ToList());
            }
            catch (Exception)
            {
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //AsignarCurso 14/02/18
        public ActionResult AsignarCurso(List<tblCU_Asignacion> lstAsignacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                objCursosFactoryService.getCursosService().AsignarCurso(lstAsignacion);
            }
            catch (Exception)
            {
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}