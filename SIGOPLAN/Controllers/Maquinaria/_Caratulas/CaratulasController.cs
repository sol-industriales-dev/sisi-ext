
using Core.DAO.Maquinaria;
using Core.DAO.Maquinaria.Caratulas;
using Core.DTO.Maquinaria._Caratulas;
using Core.DTO.Maquinaria.Caratulas;
using Core.Entity.Maquinaria._Caratulas;
using Core.Entity.Maquinaria.Caratulas;
using Core.Entity.Maquinaria.Catalogo;
using Data.Factory.Maquinaria.Caratulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.SqlServer;
using Data.EntityFramework.Generic;
using Core.DTO.Utils.Auth;
using Infrastructure.Utils;
using Core.DTO.Maquinaria.Catalogos;
using Newtonsoft.Json;

namespace SIGOPLAN.Controllers.Maquinaria.Caratulas
{
    public class CaratulasController : BaseController
    {
        ICaratulasDAO CaratulaFactoryServices;
        Dictionary<string, object> result = new Dictionary<string, object>();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CaratulaFactoryServices = new CaratulasFactoryServices().GetCaratula();
        }

        #region CARATULA MATRIZ
        public ActionResult viewCaratulas()
        {
            return View();
        }


        public ActionResult FillAreasCuentas()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cboObra = CaratulaFactoryServices.FillAreasCuentas();
                result.Add(ITEMS, cboObra);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCaratulas()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cboCaratulas = CaratulaFactoryServices.FillCaratulas();
                result.Add(ITEMS, cboCaratulas);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboModelo()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cboModelo = CaratulaFactoryServices.FillCboModelo();
                result.Add(ITEMS, cboModelo);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboGrupo()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cboModelo = CaratulaFactoryServices.FillCboGrupo();
                result.Add(ITEMS, cboModelo);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCaratula()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstCaratula = CaratulaFactoryServices.GetCaratula();
                result.Add("lstCaratula", lstCaratula);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MostrarArchivo(HttpPostedFileBase archivo,decimal tipoCambio )
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstReturnExcel = CaratulaFactoryServices.MostrarArchivo(archivo, tipoCambio);
                result.Add("lstReturnExcel", lstReturnExcel);
                result.Add("conceptosMoneda", CaratulaFactoryServices.conceptosMoneda());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarModelo(tblM_Caratulas parametros)
        {
            var result = new Dictionary<string, object>();
            try
            {

                if (parametros.id == 0)
                {
                    bool esCrear = CaratulaFactoryServices.GuardarModelo(parametros);
                    if (esCrear)
                        result.Add(SUCCESS, true);
                    else
                        result.Add(SUCCESS, false);
                }

            }
            catch (Exception ex)
            {
                result.Add(MESSAGE, ex.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarCaratula(string listaCaratulaStr, decimal tipoCambio, int idTecnico, int idSubdireccionMaquinaria)
        {
            var result = new Dictionary<string, object>();

            try
            {
                List<CaratulaGuardadoDTO> listaCaratula = JsonConvert.DeserializeObject<List<CaratulaGuardadoDTO>>(listaCaratulaStr).ToList();
                bool esCrear = CaratulaFactoryServices.GuardarCaratula(listaCaratula, tipoCambio, idTecnico, idSubdireccionMaquinaria);
                if (esCrear)
                    result.Add(SUCCESS, true);
                else
                    result.Add(SUCCESS, false);
            }
            catch (Exception ex)
            {
                result.Add(MESSAGE, ex.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CAPTURA INDIRACORES
        public ActionResult CapturaIndicadores()
        {
            return View();
        }

        public ActionResult GetIndicadores()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstIndicadores = CaratulaFactoryServices.GetIndicadores();
                result.Add("lstIndicadores", lstIndicadores);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GuardarIndicadores(tblM_IndicadoresCaratula parametros)
        {
            var result = new Dictionary<string, object>();
            try
            {

                if (parametros.id == 0)
                {
                    bool esCrear = CaratulaFactoryServices.GuardarIndicadores(parametros);
                    if (esCrear)
                        result.Add(SUCCESS, true);
                    else
                        result.Add(SUCCESS, false);
                }

            }
            catch (Exception ex)
            {
                result.Add(MESSAGE, ex.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActualizarIndicadoresNuevos(List<tblM_IndicadoresCaratula> lstNuevoIndicadores)
        {
            var result = new Dictionary<string, object>();
            try
            {


                bool esCrear = CaratulaFactoryServices.ActualizarIndicadoresNuevos(lstNuevoIndicadores);
                if (esCrear)
                    result.Add(SUCCESS, true);
                else
                    result.Add(SUCCESS, false);


            }
            catch (Exception ex)
            {
                result.Add(MESSAGE, ex.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region AUTORIZANTES
        public ActionResult AUTORIZANTES()
        {
            return View();
        }
        #endregion


        public ActionResult GetReporteCaratula(int idCaratula)
        {
            var result = new Dictionary<string, object>();

            try
            {

                var crc = CaratulaFactoryServices.GetReporte(idCaratula);
                Session["rptCaratula"] = crc;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListaAutorizantes(int idCaratula)
        {
            var result = CaratulaFactoryServices.ListaAutorizantes(idCaratula);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Autorizar(authDTO caratulas)
        {
            var result = new Dictionary<string, object>();
            int id = getUsuario().id;
            var result2 = CaratulaFactoryServices.ObtenerAutorizante(id);
            if (result2 == true)
            {
                result = CaratulaFactoryServices.Autorizar(caratulas, id);
            }
            else
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "NO TIENES AUTORIZACION PENDIENTE");
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Rechazar(authDTO Rechazar)
        {
            var result = CaratulaFactoryServices.Rechazar(Rechazar);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EnviarCorreoAutorizacion()
        {
            var downloadPDF = (List<Byte[]>)Session["reporteCaratula_Autorizante"];
            result = CaratulaFactoryServices.EnviarCorreo(downloadPDF);
            Session["reporteCaratula_Autorizante"] = null;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EnviarCorreoGuardarCaratula()
        {
            var downloadPDF = (List<Byte[]>)Session["reporteCaratula_Autorizante"];
            result = CaratulaFactoryServices.EnviarCorreoGuardarCaratula(downloadPDF);
            Session["reporteCaratula_Autorizante"] = null;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarCaratulaActiva(List<int> lstTipoHoraDia)
        {
            result = CaratulaFactoryServices.CargarCaratulaActiva(lstTipoHoraDia);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DashBoardCaratulas()
        {
            return View();
        }
        public ActionResult AgrupacionCaratulas()
        {
            return View();
        }


        public ActionResult obtenerComboCaratulras()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cboObra = CaratulaFactoryServices.obtenerComboCaratulras();
                result.Add(ITEMS, cboObra);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult obtenerCC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cboObra = CaratulaFactoryServices.obtenerCC();
                result.Add(ITEMS, cboObra);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult obtenerCaratula(int idCaratula, int idCC, int esHoraDia, int status = 0)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cboObra = CaratulaFactoryServices.obtenerCaratula(idCaratula, idCC, status, esHoraDia);
                result.Add(ITEMS, cboObra);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetReporteCaratulacc(string lstCaratula)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var lst = JsonConvert.DeserializeObject<List<ReporteCaratulaCCDTO>>(lstCaratula);
                Session["rptCaratula"] = lst;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult obtenerHistorialCaratulas(int estatus)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, CaratulaFactoryServices.obtenerHistorialCaratulas(estatus));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public ActionResult obtenerAgrupacionCaratulas()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstAgrupacionCaratulas = CaratulaFactoryServices.obtenerAgrupacionCaratulas();
                result.Add(ITEMS, lstAgrupacionCaratulas);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult obtenerGrupos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cboGrupos = CaratulaFactoryServices.obtenerGrupos();
                result.Add(ITEMS, cboGrupos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult obtenerModelos(int idGrupo, int Editar = 1, int Agrupacion = 0)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cboModelos = CaratulaFactoryServices.obtenerModelos(idGrupo, Editar, Agrupacion);
                result.Add(ITEMS, cboModelos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EliminarAgrupacion(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var eliminarAgrupacion = CaratulaFactoryServices.EliminarAgrupacion(id);
                result.Add(ITEMS, eliminarAgrupacion);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EliminarModeloAgrupacion(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var eliminarModeloAgrupacion = CaratulaFactoryServices.EliminarModeloAgrupacion(id);
                result.Add(ITEMS, eliminarModeloAgrupacion);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarEditar(CaratulaEncDTO parametros)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var GuardarEditarAgrupacion = CaratulaFactoryServices.GuardarEditar(parametros);
                result.Add(ITEMS, GuardarEditarAgrupacion);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerAgrupaciones()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var GuardarEditarAgrupacion = CaratulaFactoryServices.ObtenerAgrupaciones();
                result.Add(ITEMS, GuardarEditarAgrupacion);
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