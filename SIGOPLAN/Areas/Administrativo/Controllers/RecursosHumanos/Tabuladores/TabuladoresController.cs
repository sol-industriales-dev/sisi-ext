using Core.DAO.RecursosHumanos.Tabuladores;
using Core.DTO.RecursosHumanos.Tabuladores;
using Core.Enum.RecursosHumanos.Tabuladores;
using Data.Factory.RecursosHumanos.Tabuladores;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.RecursosHumanos.Tabuladores
{
    public class TabuladoresController : BaseController
    {
        #region INIT
        Dictionary<string, object> resultado = new Dictionary<string, object>();
        public ITabuladoresDAO _TabuladorFS;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _TabuladorFS = new TabuladoresFactoryService().GetTabuladoresService();
            base.OnActionExecuting(filterContext);
        }
        #endregion

        #region RETURN VIEWS
        public ActionResult LineaNegocio()
        {
            bool tieneAcceso = _TabuladorFS.VerificarAcceso((int)MenuTabuladoresEnum.LINEA_DE_NEGOCIOS);
            if (tieneAcceso)
                return View();
            else
                return View("ErrorPermisoVista");
        }

        public ActionResult AsignarTabulador()
        {
            bool tieneAcceso = _TabuladorFS.VerificarAcceso((int)MenuTabuladoresEnum.ASIGNACION_DE_TABULADORES);
            if (tieneAcceso)
                return View();
            else
                return View("ErrorPermisoVista");
        }

        public ActionResult PlantillaPersonal()
        {
            bool tieneAcceso = _TabuladorFS.VerificarAcceso((int)MenuTabuladoresEnum.PLANTILLA_DE_PERSONAL);
            if (tieneAcceso)
                return View();
            else
                return View("ErrorPermisoVista");
        }

        public ActionResult GestionTabuladores()
        {
            bool tieneAcceso = _TabuladorFS.VerificarAcceso((int)MenuTabuladoresEnum.GESTION_DE_TABULADORES);
            if (tieneAcceso)
                return View();
            else
                return View("ErrorPermisoVista");
        }

        public ActionResult GestionPlantillasPersonal()
        {
            bool tieneAcceso = _TabuladorFS.VerificarAcceso((int)MenuTabuladoresEnum.GESTION_DE_PLANTILLAS);
            if (tieneAcceso)
                return View();
            else
                return View("ErrorPermisoVista");
        }

        public ActionResult Modificacion()
        {
            bool tieneAcceso = _TabuladorFS.VerificarAcceso((int)MenuTabuladoresEnum.MODIFICACION_TABULADORES);
            if (tieneAcceso)
                return View();
            else
                return View("ErrorPermisoVista");
        }

        public ActionResult GestionModificacion()
        {
            bool tieneAcceso = _TabuladorFS.VerificarAcceso((int)MenuTabuladoresEnum.GESTION_DE_MODIFICACION);
            if (tieneAcceso)
                return View();
            else
                return View("ErrorPermisoVista");
        }

        public ActionResult Reportes()
        {
            bool tieneAcceso = _TabuladorFS.VerificarAcceso((int)MenuTabuladoresEnum.REPORTES);
            if (tieneAcceso)
                return View();
            else
                return View("ErrorPermisoVista");
        }

        public ActionResult GestionReportes()
        {
            bool tieneAcceso = _TabuladorFS.VerificarAcceso((int)MenuTabuladoresEnum.GETSION_REPORTES);
            if (tieneAcceso)
                return View();
            else
                return View("ErrorPermisoVista");
        }

        
        #endregion

        #region LINEA DE NEGOCIOS
        public ActionResult GetLineaNegocios()
        {
            return Json(_TabuladorFS.GetLineaNegocios(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CELineaNegocio(LineaNegocioDTO objFiltroDTO)
        {
            return Json(_TabuladorFS.CELineaNegocio(objFiltroDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarLineaNegocio(LineaNegocioDTO objFiltroDTO)
        {
            return Json(_TabuladorFS.GetDatosActualizarLineaNegocio(objFiltroDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarLineaNegocio(LineaNegocioDTO objFiltroDTO)
        {
            return Json(_TabuladorFS.EliminarLineaNegocio(objFiltroDTO), JsonRequestBehavior.AllowGet); 
        }

        #region LINEA DE NEGOCIOS REL CC
        public ActionResult GetLineaNegociosRelCC(LineaNegocioDetDTO objFiltroDTO)
        {
            return Json(_TabuladorFS.GetLineaNegociosRelCC(objFiltroDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CELineaNegocioRelCC(LineaNegocioDetDTO objFiltroDTO)
        {
            return Json(_TabuladorFS.CELineaNegocioRelCC(objFiltroDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarLineaNegocioRelCC(LineaNegocioDetDTO objFiltroDTO)
        {
            return Json(_TabuladorFS.EliminarLineaNegocioRelCC(objFiltroDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCCDisponibles(LineaNegocioDetDTO objFiltroDTO)
        {
            return Json(_TabuladorFS.FillCboCCDisponibles(objFiltroDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region ASIGNACIÓN TABULADORES
        public ActionResult InitCEAsignacionTabulador()
        {
            return Json(_TabuladorFS.InitCEAsignacionTabulador(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAsignacionTabuladores(TabuladorDetDTO objFiltroDTO)
        {
            return Json(_TabuladorFS.GetAsignacionTabuladores(objFiltroDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearAsignacionTabulador(TabuladorDTO objTabuladorDTO, List<TabuladorDetDTO> lstTabuladoresDTO, List<GestionAutorizanteDTO> lstGestionAutorizantesDTO)
        {
            return Json(_TabuladorFS.CrearAsignacionTabulador(objTabuladorDTO, lstTabuladoresDTO, lstGestionAutorizantesDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarTabuladoresPuesto(TabuladorDTO objFiltroDTO)
        {
            return Json(_TabuladorFS.EliminarTabuladoresPuesto(objFiltroDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTabuladoresExistentes(TabuladorDTO objParamDTO)
        {
            return Json(_TabuladorFS.GetTabuladoresExistentes(objParamDTO), JsonRequestBehavior.AllowGet);
        }

        #region TABULADORES DET
        public ActionResult GetListadoTabuladoresDet(TabuladorDetDTO objFiltroDTO)
        {
            return Json(_TabuladorFS.GetListadoTabuladoresDet(objFiltroDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarTabuladorDet(TabuladorDetDTO objFiltroDTO)
        {
            return Json(_TabuladorFS.GetDatosActualizarTabuladorDet(objFiltroDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActualizarTabuladorDet(TabuladorDetDTO objFiltroDTO)
        {
            return Json(_TabuladorFS.ActualizarTabuladorDet(objFiltroDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarTabuladorDet(TabuladorDetDTO objParamDTO)
        {
            return Json(_TabuladorFS.EliminarTabuladorDet(objParamDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region PLANTILLA DE PERSONAL
        public ActionResult GetTabuladoresAutorizados(TabuladorDTO objParamDTO)
        {
            return Json(_TabuladorFS.GetTabuladoresAutorizados(objParamDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboFiltroCC_PlantillaPersonal(TabuladorDTO objParamDTO)
        {
            return Json(_TabuladorFS.FillCboFiltroCC_PlantillaPersonal(objParamDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearSolicitudPlantilla(TabuladorDTO objParamDTO)
        {
            return Json(_TabuladorFS.CrearSolicitudPlantilla(objParamDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult NotificarPlantilla(string ccPlantilla, bool esAuthCompleta)
        {
            return Json(_TabuladorFS.NotificarPlantilla(ccPlantilla, esAuthCompleta), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboFiltroPuestos_PlantillaPersonal(TabuladorDTO objParamDTO)
        {
            return Json(_TabuladorFS.FillCboFiltroPuestos_PlantillaPersonal(objParamDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboFiltroLineaNegocios_PlantillaPersonal(TabuladorDTO objParamDTO)
        {
            return Json(_TabuladorFS.FillCboFiltroLineaNegocios_PlantillaPersonal(objParamDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region MODIFICACIÓN
        public ActionResult FillCboTipoModificaciones()
        {
            return Json(_TabuladorFS.FillCboTipoModificaciones(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTabuladoresModificacion(TabuladorDTO objParamDTO)
        {
            return Json(_TabuladorFS.GetTabuladoresModificacion(objParamDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboFiltroPuestos_Modificacion(TabuladorDTO objParamDTO)
        {
            return Json(_TabuladorFS.FillCboFiltroPuestos_Modificacion(objParamDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearModificacion(GestionModificacionTabuladorDTO objParamDTO, List<GestionModificacionTabuladorDetDTO> lstParamDTO, List<GestionAutorizanteDTO> lstGestionAutorizantesDTO)
        {
            return Json(_TabuladorFS.CrearModificacion(objParamDTO, lstParamDTO, lstGestionAutorizantesDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTabuladoresEmpleadosActivos(TabuladorDTO objParamDTO)
        {
            resultado = new Dictionary<string, object>();
            resultado = _TabuladorFS.GetTabuladoresEmpleadosActivos(objParamDTO);
            var json = Json(resultado, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult GetTabuladoresPuestos(TabuladorDTO objParamDTO)
        {
            resultado = new Dictionary<string, object>();
            resultado = _TabuladorFS.GetTabuladoresPuestos(objParamDTO);
            var json = Json(resultado, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        #endregion

        #region GESTIÓN TABULADORES
        public ActionResult GetGestionTabuladores(TabuladorDTO objParamDTO)
        {
            return Json(_TabuladorFS.GetGestionTabuladores(objParamDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutorizarRechazarTabulador(TabuladorDTO objParamDTO)
        {
            return Json(_TabuladorFS.AutorizarRechazarTabulador(objParamDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLstAutorizantesTabulador(GestionAutorizanteDTO objParamDTO)
        {
            return Json(_TabuladorFS.GetLstAutorizantesTabulador(objParamDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarComentarioRechazoTabulador(int idTabulador, string comentario)
        {
            return Json(_TabuladorFS.GuardarComentarioRechazoTabulador(idTabulador, comentario), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDetalleRelTabulador(TabuladorDetDTO objParamDTO)
        {
            return Json(_TabuladorFS.GetDetalleRelTabulador(objParamDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GESTIÓN PLANTILLAS PERSONAL
        public ActionResult GetGestionPlantillasPersonal(PlantillaPersonalDTO objParamDTO)
        {
            return Json(_TabuladorFS.GetGestionPlantillasPersonal(objParamDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutorizarRechazarPlantillaPersonal(PlantillaPersonalDTO objParamDTO)
        {
            return Json(_TabuladorFS.AutorizarRechazarPlantillaPersonal(objParamDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLstAutorizantesPlantilla(int idPlantilla)
        {
            return Json(_TabuladorFS.GetLstAutorizantesPlantilla(idPlantilla), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarComentarioRechazoPlantilla(int idPlantilla, string comentario)
        {
            return Json(_TabuladorFS.GuardarComentarioRechazoPlantilla(idPlantilla, comentario), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPlantillaDetalle(int plantilla_id)
        {
            return Json(_TabuladorFS.GetPlantillaDetalle(plantilla_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDetallePlantillaTabuladores(PlantillaPersonalDTO objParamDTO)
        {
            return Json(_TabuladorFS.GetDetallePlantillaTabuladores(objParamDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GESTIÓN MODIFICACIÓN
        public ActionResult GetGestionModificacion(GestionModificacionTabuladorDTO objParamDTO)
        {
            return Json(_TabuladorFS.GetGestionModificacion(objParamDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutorizarRechazarGestionModificacion(GestionModificacionTabuladorDTO objParamDTO)
        {
            return Json(_TabuladorFS.AutorizarRechazarGestionModificacion(objParamDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLstAutorizantesModificacion(int idModificacion)
        {
            return Json(_TabuladorFS.GetLstAutorizantesModificacion(idModificacion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarComentarioRechazoModificacion(int idModificacion, string comentario)
        {
            return Json(_TabuladorFS.GuardarComentarioRechazoModificacion(idModificacion, comentario), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetModificacionDetalle(GestionModificacionTabuladorDetDTO objParamDTO)
        {
            //return Json(_TabuladorFS.GetModificacionDetalle(objParamDTO), JsonRequestBehavior.AllowGet);
            var json = Json(_TabuladorFS.GetModificacionDetalle(objParamDTO), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        //public ActionResult GetParamsPDF_TabuladoresModificacion(TabuladorDTO objParamDTO)
        //{   
        //    resultado = new Dictionary<string, object>();
        //    try
        //    {
        //        #region SE SETEAN LOS PARAMETROS PARA GENERAR PDF
        //        var lstDescCC = _TabuladorFS.GetDescCCs(objParamDTO.lstCC);
        //        var lstDescLineasNegocio = _TabuladorFS.GetDescLineaNegocio(objParamDTO.lstFK_LineaNegocio);
        //        //var lstAutorizadoresDTO = _TabuladorFS.GetAutorizantesReporte(objParamDTO.lstGestionAutorizantesDTO);

        //        objParamDTO.lstDescCC = lstDescCC;
        //        objParamDTO.lstDescLineaNegocio = lstDescLineasNegocio;
        //        //objParamDTO.lstReporteAutorizantesDTO = lstAutorizadoresDTO;

        //        Session["objParamsPDF"] = objParamDTO;
        //        resultado.Add(SUCCESS, true);
        //        #endregion
        //    }
        //    catch (Exception)
        //    {
        //        resultado.Add(SUCCESS, false);
        //        resultado.Add(MESSAGE, "Ocurrió un error al generar el reporte.");
        //    }
        //    return Json(resultado, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region REPORTES
        public ActionResult SendReporteCorreo(TabuladorDTO objParamDTO)
        {
            var json = Json(_TabuladorFS.SendReporteCorreo(objParamDTO), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult GetTabuladoresReporte(TabuladorDTO objParamDTO)
        {
            var json = Json(_TabuladorFS.GetTabuladoresReporte(objParamDTO), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult GetParamsDTO(TabuladorDTO objParamDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LISTADO DE TABULADORES
                Dictionary<string, object> resultadoTabuladoresReporte = _TabuladorFS.GetTabuladoresReporte(objParamDTO);
                List<TabuladorDetDTO> lstTabuladoresDTO = (List<TabuladorDetDTO>)resultadoTabuladoresReporte["lstTabPuestos"];
                Session["lstTabuladoresDTO"] = lstTabuladoresDTO;

                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al generar el reporte.");
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetParamsDTOPdf(TabuladorDTO objParamDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE GUARDAN LOS PARAMETROS
                var lstDescCC = _TabuladorFS.GetDescCCs(objParamDTO.lstCC);
                var lstDescLineasNegocio = _TabuladorFS.GetDescLineaNegocio(objParamDTO.lstFK_LineaNegocio);
                var lstAutorizadoresDTO = _TabuladorFS.GetAutorizantesReporte(objParamDTO.lstGestionAutorizantesDTO);

                objParamDTO.lstDescCC = lstDescCC;
                objParamDTO.lstDescLineaNegocio = lstDescLineasNegocio;
                objParamDTO.lstReporteAutorizantesDTO = lstAutorizadoresDTO;

                Session["objParamDTOPdf"] = objParamDTO;

                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al generar el reporte.");
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public MemoryStream GenerarExcelTabuladores()
        {
            try
            {
                #region GENERACIÓN EXCEL REPORTE
                if (Session["lstTabuladoresDTO"] == null)
                    throw new Exception("Ocurrió un error al generar el reporte.");

                List<TabuladorDetDTO> lstTabuladoresDTO = (List<TabuladorDetDTO>)Session["lstTabuladoresDTO"];
                if (lstTabuladoresDTO == null || lstTabuladoresDTO.Count() <= 0)
                    throw new Exception("Es necesario realizar una búsqueda para poder generar el archivo excel.");

                using (ExcelPackage package = new ExcelPackage())
                {
                    var Valuacion = package.Workbook.Worksheets.Add("Tabuladores");
                    //ExcelRange cols = Valuacion.Cells["A:G"];

                    Valuacion.InsertRow(1, 1);
                    Valuacion.Cells["A1"].Value = "ID";
                    Valuacion.Cells["B1"].Value = "Puesto";
                    Valuacion.Cells["C1"].Value = "AREA/DEPARTAMENTO";
                    Valuacion.Cells["D1"].Value = "SINDICATO";
                    Valuacion.Cells["E1"].Value = "Tipo nómina";
                    Valuacion.Cells["F1"].Value = "Linea de negocio";
                    Valuacion.Cells["G1"].Value = "Categoría";
                    Valuacion.Cells["H1"].Value = "Sueldo base";
                    Valuacion.Cells["I1"].Value = "Complemento";
                    Valuacion.Cells["J1"].Value = "Total nominal";
                    Valuacion.Cells["K1"].Value = "Total mensual";

                    int rowNumber_PuestoDesc = 2;
                    string rowLetter = string.Empty;
                    for (int i = 0; i < lstTabuladoresDTO.Count(); i++)
                    {
                        // ID PUESTO 
                        string rowCol_PuestoID = string.Format("A{0}", rowNumber_PuestoDesc);
                        Valuacion.Cells[rowCol_PuestoID].Value = (!string.IsNullOrEmpty(lstTabuladoresDTO[i].puestoDesc) ? lstTabuladoresDTO[i].idPuesto.ToString() : string.Empty);

                        // PUESTO
                        string rowCol_PuestoDesc = string.Format("B{0}", rowNumber_PuestoDesc);
                        Valuacion.Cells[rowCol_PuestoDesc].Value = (!string.IsNullOrEmpty(lstTabuladoresDTO[i].puestoDesc) ? lstTabuladoresDTO[i].puestoDesc.ToString() : string.Empty);

                        // AREA DEPARTAMENTO
                        string rowCol_AreaDepartamento = string.Format("C{0}", rowNumber_PuestoDesc);
                        Valuacion.Cells[rowCol_AreaDepartamento].Value = !string.IsNullOrEmpty(lstTabuladoresDTO[i].descAreaDepartamento) ? lstTabuladoresDTO[i].descAreaDepartamento.ToString() : string.Empty;

                        // SINDICATO
                        string rowCol_Sindicato = string.Format("D{0}", rowNumber_PuestoDesc);
                        Valuacion.Cells[rowCol_Sindicato].Value = !string.IsNullOrEmpty(lstTabuladoresDTO[i].descSindicato) ? lstTabuladoresDTO[i].descSindicato.ToString() : string.Empty;

                        // TIPO NOMINA
                        string rowCol_TipoNomina = string.Format("E{0}", rowNumber_PuestoDesc);
                        Valuacion.Cells[rowCol_TipoNomina].Value = !string.IsNullOrEmpty(lstTabuladoresDTO[i].tipoNominaDesc) ? lstTabuladoresDTO[i].tipoNominaDesc.ToString() : string.Empty;

                        int rowNumberRelPuesto = rowNumber_PuestoDesc;
                        for (int j = 0; j < lstTabuladoresDTO[i].lstLineasNegocios.Count(); j++)
                        {
                            // LINEA DE NEGOCIOS
                            string rowCol_LineaNegocio = string.Format("F{0}", rowNumberRelPuesto);
                            Valuacion.Cells[rowCol_LineaNegocio].Value = lstTabuladoresDTO[i].lstLineasNegocios[j];

                            // CATEGORIAS
                            string rowCol_Categoria = string.Format("G{0}", rowNumberRelPuesto);
                            Valuacion.Cells[rowCol_Categoria].Value = lstTabuladoresDTO[i].lstCategorias[j];

                            // SUELDO BASE
                            string rowCol_SueldoBase = string.Format("H{0}", rowNumberRelPuesto);
                            Valuacion.Cells[rowCol_SueldoBase].Value = lstTabuladoresDTO[i].lstSueldosBases[j];

                            // COMPLEMENTO
                            string rowCol_Complemento = string.Format("I{0}", rowNumberRelPuesto);
                            Valuacion.Cells[rowCol_Complemento].Value = lstTabuladoresDTO[i].lstComplementos[j];

                            // TOTAL NOMINAL
                            string rowCol_TotalNominal = string.Format("J{0}", rowNumberRelPuesto);
                            Valuacion.Cells[rowCol_TotalNominal].Value = lstTabuladoresDTO[i].lstTotalNominal[j];

                            // TOTAL MENSUAL
                            string rowCol_TotalMensual = string.Format("K{0}", rowNumberRelPuesto);
                            Valuacion.Cells[rowCol_TotalMensual].Value = lstTabuladoresDTO[i].lstSueldoMensual[j];

                            rowNumberRelPuesto++;
                        }

                        rowNumber_PuestoDesc = (rowNumberRelPuesto++);
                    }

                    using (var rng = Valuacion.Cells["A1:K1"])
                    {
                        //rng.Style.Font.Bold = true;
                        //rng.Style.Font.Color.SetColor(Color.Black);
                        //rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        //rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //rng.Style.Fill.BackgroundColor.SetColor(0, 0, 100, 0);
                    }

                    Valuacion.Cells[Valuacion.Dimension.Address].AutoFitColumns();
                    package.Compression = CompressionLevel.BestSpeed;
                    List<byte[]> lista = new List<byte[]>();
                    using (var exportData = new MemoryStream())
                    {
                        this.Response.Clear();
                        package.SaveAs(exportData);
                        lista.Add(exportData.ToArray());
                        this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        this.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", package.Workbook.Worksheets.FirstOrDefault().Name + ".xlsx"));
                        this.Response.BinaryWrite(exportData.ToArray());
                        this.Response.End();
                        return exportData;
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ActionResult FillCboFiltroPuestos_Reportes(TabuladorDTO objParamDTO)
        {
            return Json(_TabuladorFS.FillCboFiltroPuestos_Reportes(objParamDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GESTION REPORTES
        public ActionResult GetGestionReportes(ReportesTabuladoresDTO objParamDTO)
        {
            return Json(_TabuladorFS.GetGestionReportes(objParamDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutorizarRechazarReporte(ReportesTabuladoresDTO objParamDTO)
        {
            return Json(_TabuladorFS.AutorizarRechazarReporte(objParamDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLstAutorizantesReporte(int idReporte)
        {
            return Json(_TabuladorFS.GetLstAutorizantesReporte(idReporte), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarComentarioRechazoReporte(int idReporte, string comentario)
        {
            return Json(_TabuladorFS.GuardarComentarioRechazoReporte(idReporte, comentario), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTabuladoresReporteByCC(TabuladorDTO objParamDTO)
        {
            //return Json(_TabuladorFS.GetTabuladoresReporteByCC(objParamDTO), JsonRequestBehavior.AllowGet);

            var json = Json(_TabuladorFS.GetTabuladoresReporteByCC(objParamDTO), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult GetParametrosReporte(int idReporte)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE GUARDAN LOS PARAMETROS
                var objParamDTO = _TabuladorFS.GetParametrosReporte(idReporte);
                var lstDescCC = _TabuladorFS.GetDescCCs(objParamDTO.lstCC);
                var lstDescLineasNegocio = _TabuladorFS.GetDescLineaNegocio(objParamDTO.lstFK_LineaNegocio);

                var lstAuth = _TabuladorFS.GetLstAutorizantesReporte(idReporte)["items"] as List<GestionAutorizanteDTO>;
                var lstAutorizadoresDTO = _TabuladorFS.GetAutorizantesReporte(lstAuth);

                objParamDTO.lstDescCC = lstDescCC;
                objParamDTO.lstDescLineaNegocio = lstDescLineasNegocio;
                objParamDTO.lstReporteAutorizantesDTO = lstAutorizadoresDTO;

                Session["objParamDTOPdf"] = objParamDTO;

                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al generar el reporte.");
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutorizarReportesMasivo(List<int> lstIdReportes)
        {
            return Json(_TabuladorFS.AutorizarReportesMasivo(lstIdReportes), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ACCESO MENU
        public ActionResult GetAccesosMenu()
        {
            return Json(_TabuladorFS.GetAccesosMenu(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GENERALES
        public ActionResult FillCboPuestos()
        {
            return Json(_TabuladorFS.FillCboPuestos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInformacionPuesto(PuestoDTO objFiltroDTO)
        {
            return Json(_TabuladorFS.GetInformacionPuesto(objFiltroDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCategorias()
        {
            return Json(_TabuladorFS.FillCboCategorias(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEsquemaPagos()
        {
            return Json(_TabuladorFS.FillCboEsquemaPagos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboLineaNegocios(TabuladorDTO objParamDTO)
        {
            return Json(_TabuladorFS.FillCboLineaNegocios(objParamDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboAreasDepartamentos()
        {
            return Json(_TabuladorFS.FillCboAreasDepartamentos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboTipoNomina()
        {
            return Json(_TabuladorFS.FillCboTipoNomina(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboSindicatos()
        {
            return Json(_TabuladorFS.FillCboSindicatos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboNivelMando()
        {
            return Json(_TabuladorFS.FillCboNivelMando(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboUsuarios()
        {
            return Json(_TabuladorFS.FillCboUsuarios(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCC(List<int> lstFK_LineaNegocio)
        {
            return Json(_TabuladorFS.FillCboCC(lstFK_LineaNegocio), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboGestionEstatus()
        {
            return Json(_TabuladorFS.FillCboGestionEstatus(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFechaActual()
        {
            return Json(_TabuladorFS.GetFechaActual(), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}