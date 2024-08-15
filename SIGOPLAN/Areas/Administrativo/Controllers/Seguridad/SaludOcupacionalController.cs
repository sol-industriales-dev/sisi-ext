
using Core.DAO.Administracion.Seguridad.SaludOcupacional;
using Core.DTO.Administracion.Seguridad.SaludOcupacional;
using Core.Entity.Administrativo.Seguridad.SaludOcupacional;
using Data.Factory.Administracion.Seguridad.SaludOcupacional;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Seguridad
{
    public class SaludOcupacionalController : BaseController
    {
        #region CONSTRUCTOR
        private ISaludOcupacionalDAO saludOcupacionalService;
        Dictionary<string, object> dicResultado = new Dictionary<string, object>();
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            saludOcupacionalService = new SaludOcupacionalFactoryService().getSaludOcupacionalService();
            dicResultado.Clear();
            base.OnActionExecuting(filterContext);
        }
        #endregion

        #region VIEWS
        public ActionResult HistorialClinico()
        {
            return View();
        }

        public ActionResult AtencionMedica()
        {
            return View();
        }

        public ActionResult HistorialEmpleado()
        {
            return View();
        }

        public ActionResult Medicos()
        {
            return View();
        }
        #endregion

        #region Atención Médica
        public ActionResult CargarInformacionEmpleado(int claveEmpleado)
        {
            return Json(saludOcupacionalService.CargarInformacionEmpleado(claveEmpleado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevaAtencionMedica(HttpPostedFileBase archivoST7, HttpPostedFileBase archivoST2)
        {
            var atencionMedica = JsonConvert.DeserializeObject<tblS_SO_AtencionMedica>(Request.Form["atencionMedica"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            var revision = JsonConvert.DeserializeObject<tblS_SO_AtencionMedica_Revision>(Request.Form["revision"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });

            return Json(saludOcupacionalService.GuardarNuevaAtencionMedica(atencionMedica, revision, archivoST7, archivoST2), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevaRevision(HttpPostedFileBase archivoST7, HttpPostedFileBase archivoST2)
        {
            var revision = JsonConvert.DeserializeObject<tblS_SO_AtencionMedica_Revision>(Request.Form["revision"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });

            return Json(saludOcupacionalService.GuardarNuevaRevision(revision, archivoST7, archivoST2), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarArchivosST7ST2(HttpPostedFileBase archivoST7, HttpPostedFileBase archivoST2)
        {
            var atencionMedica_id = Convert.ToInt32(Request.Form["atencionMedica_id"]);

            return Json(saludOcupacionalService.GuardarArchivosST7ST2(atencionMedica_id, archivoST7, archivoST2), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarAtencionesMedicas(int claveEmpleado)
        {
            return Json(saludOcupacionalService.CargarAtencionesMedicas(claveEmpleado), JsonRequestBehavior.AllowGet);
        }
        public ActionResult EliminarAtencionMedica(int idAtencion)
        {
            return Json(saludOcupacionalService.EliminarAtencionMedica(idAtencion), JsonRequestBehavior.AllowGet);
        }        

        public ActionResult CargarAtencionMedicaDetalle(int atencionMedica_id)
        {
            return Json(saludOcupacionalService.CargarAtencionMedicaDetalle(atencionMedica_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DescargarArchivoST7(int atencionMedica_id)
        {
            var resultadoTupla = saludOcupacionalService.DescargarArchivoST7(atencionMedica_id);

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

        public ActionResult DescargarArchivoST2(int atencionMedica_id)
        {
            var resultadoTupla = saludOcupacionalService.DescargarArchivoST2(atencionMedica_id);

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

        public ActionResult CargarHistorialEmpleado(int claveEmpleado)
        {
            return Json(saludOcupacionalService.CargarHistorialEmpleado(claveEmpleado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerarCertificadoSesion(CertificadoDTO empleado)
        {

            try
            {

                var emp = saludOcupacionalService.GetReportDataCertificado();

                if(emp["empleado"] != null)
                {
                    var empObj = emp["empleado"] as medicoDTO;

                    empleado.fecha = DateTime.Now.ToString("dd/MM/yyyy");
                    empleado.medicoNombre = empObj.nombre;
                    empleado.medicoCedula = empObj.cedulaProfesional;

                    Session["certificadoEmpleado"] = empleado;

                    dicResultado.Add(SUCCESS, true);
                }
                else
                {
                    dicResultado.Add(SUCCESS, false);
                    dicResultado.Add(MESSAGE, "El usuario actual no es un medico.");
                }

                
            }catch(Exception e)
            {
                dicResultado.Add(SUCCESS, false);
            }

            return Json(dicResultado, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Medicos

        public ActionResult GetMedicos(medicoDTO _objFiltroDTO)
        {
            return Json(saludOcupacionalService.GetMedicos(_objFiltroDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarMedicos(medicoDTO _objFiltroDTO)
        {
            return Json(saludOcupacionalService.CrearEditarMedicos(_objFiltroDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarMedico(int _idMedico)
        {
            return Json(saludOcupacionalService.EliminarMedico(_idMedico), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region HISTORIAL CLINICO
        public ActionResult CEHistorialClinico(List<HttpPostedFileBase> lstArchivosDatosPersonales, List<HttpPostedFileBase> lstArchivosEspirometria, List<HttpPostedFileBase> lstArchivosAudiometria,List<HttpPostedFileBase> lstArchivosElectrocardiograma,
                                               List<HttpPostedFileBase> lstArchivosRadiografias, List<HttpPostedFileBase> lstArchivosLaboratorio, List<HttpPostedFileBase> lstArchivosDocumentosAdjuntos)
        {
            HistorialClinicoDTO objHC = JsonConvert.DeserializeObject<HistorialClinicoDTO>(Request.Form["objHC"]);
            return Json(saludOcupacionalService.CEHistorialClinico(objHC, lstArchivosDatosPersonales, lstArchivosEspirometria, lstArchivosAudiometria, lstArchivosElectrocardiograma, lstArchivosRadiografias, lstArchivosLaboratorio, 
                                                                   lstArchivosDocumentosAdjuntos), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUltimoFolioHistorialClinico()
        {
            return Json(saludOcupacionalService.GetUltimoFolioHistorialClinico(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarHistorialClinico(int idHistorialClinico)
        {
            return Json(saludOcupacionalService.GetDatosActualizarHistorialClinico(idHistorialClinico), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetHistorialesClinicos()
        {
            return Json(saludOcupacionalService.GetHistorialesClinicos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarHistorialClinico(int idHistorialClinico)
        {
            return Json(saludOcupacionalService.EliminarHistorialClinico(idHistorialClinico), JsonRequestBehavior.AllowGet);
        }

        #region OBSERVACIÓN MEDICO CP
        public ActionResult GetObservacionMedicoInternoCP(int idHC)
        {
            return Json(saludOcupacionalService.GetObservacionMedicoInternoCP(idHC), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarObservacionMedicoInternoCP(List<HttpPostedFileBase> lstArchivos)
        {
            HistorialClinicoDTO objDTO = JsonConvert.DeserializeObject<HistorialClinicoDTO>(Request.Form["obj"]);
            return Json(saludOcupacionalService.EditarObservacionMedicoInternoCP(lstArchivos, objDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion 

        #region CARGAR DOCUMENTO FIRMADO MEDICO EXTERNO
        public ActionResult CargarDocumentoFirmadoMedicoExterno(List<HttpPostedFileBase> lstArchivos)
        {
            HistorialClinicoDTO objDTO = JsonConvert.DeserializeObject<HistorialClinicoDTO>(Request.Form["obj"]);
            return Json(saludOcupacionalService.CargarDocumentoFirmadoMedicoExterno(lstArchivos, objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRutaArchivo(int idHC, int tipoArchivo)
        {
            return Json(saludOcupacionalService.GetRutaArchivo(idHC, tipoArchivo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult VerificarExisteDocumento(int idHC, int tipoArchivo)
        {
            return Json(saludOcupacionalService.VerificarExisteDocumento(idHC, tipoArchivo), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region FUNCIONES GENERALES
        public ActionResult DescargarArchivo(int idHC, int tipoArchivo)
        {
            var resultadoTupla = saludOcupacionalService.DescargarArchivo(idHC, tipoArchivo);

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

        public ActionResult FillCboEscolaridades()
        {
            return Json(saludOcupacionalService.FillCboEscolaridades(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEmpresas()
        {
            return Json(saludOcupacionalService.FillCboEmpresas(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCC()
        {
            return Json(saludOcupacionalService.FillCboCC(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEstadoCivil()
        {
            return Json(saludOcupacionalService.FillCboEstadoCivil(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboTipoSanguineo()
        {
            return Json(saludOcupacionalService.FillCboTipoSanguineo(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEdadPaciente(DateTime fechaNac)
        {
            return Json(saludOcupacionalService.GetEdadPaciente(fechaNac), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboMarcasCovid19()
        {
            return Json(saludOcupacionalService.FillCboMarcasCovid19(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboUsuariosMedicos()
        {
            return Json(saludOcupacionalService.FillCboUsuariosMedicos(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CrytalReports

        public ActionResult GetReportDataHistorialClinico(int idHistorialClinico)
        {

            return Json(saludOcupacionalService.GetReportDataHistorialClinico(idHistorialClinico), JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetReportDataCertificado()
        {
            return Json(saludOcupacionalService.GetReportDataCertificado(), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}