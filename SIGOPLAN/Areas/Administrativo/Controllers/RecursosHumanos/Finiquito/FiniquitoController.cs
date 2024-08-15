using Core.DTO;
using Core.DTO.Principal.Usuarios;
using Core.DTO.RecursosHumanos;
using Core.Entity.RecursosHumanos.Captura;
using Data.Factory.RecursosHumanos.Captura;
using Infrastructure.DTO;
using Infrastructure.Utils;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.RecursosHumanos.Finiquito
{
    public class FiniquitoController : BaseController
    {
        FiniquitoFactoryService finiquitoFactoryServices;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            finiquitoFactoryServices = new FiniquitoFactoryService();
            base.OnActionExecuting(filterContext);
        }

        // GET: Administrativo/Finiquito
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult CapturaFiniquito()
        {
            return View();
        }

        public ActionResult GestionFiniquito()
        {
            return View();
        }

        public ActionResult ConceptosFiniquito()
        {
            return View();
        }

        public ActionResult getUsuarioNombre()
        {
            var result = new Dictionary<string, object>();

            var userID = vSesiones.sesionUsuarioDTO.id;
            var user = vSesiones.sesionUsuarioDTO.nombre.Trim();
            var puestoID = vSesiones.sesionUsuarioDTO.puestoID;
            var puesto = vSesiones.sesionUsuarioDTO.puesto.descripcion.Trim();

            var data = new
            {
                userID = userID,
                user = user,
                puestoID = puestoID,
                puesto = puesto
            };

            result.Add("data", data);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getFiniquitos(int clave, string nombre, string cc, int aut)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = finiquitoFactoryServices.getFiniquitoService().getFiniquitos(clave, nombre, cc, aut);

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

        public ActionResult getEmpleado(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var objEmpleado = finiquitoFactoryServices.getFiniquitoService().getEmpleadoForId(id);
                objEmpleado.fechaAlta = DateTime.Parse(objEmpleado.fechaAlta.ToString("dd/MM/yyyy"));

                var diasTrans = 0;

                if (objEmpleado.fechaBaja != null)
                {
                    objEmpleado.fechaBaja = DateTime.Parse(objEmpleado.fechaBaja.Value.ToString("dd/MM/yyyy"));

                    var year = objEmpleado.fechaBaja.Value.Year;
                    DateTime firstDay = new DateTime(year, 1, 1);

                    diasTrans = objEmpleado.fechaBaja.Value.Subtract(firstDay).Days + 1;
                }
                else
                {
                    var year = DateTime.Now.Year;
                    DateTime firstDay = new DateTime(year, 1, 1);

                    diasTrans = DateTime.Now.Subtract(firstDay).Days + 1;
                }
                result.Add(ITEMS, objEmpleado);
                result.Add("diasTrans", diasTrans);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboConcepto()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = finiquitoFactoryServices.getFiniquitoService().getListaConceptos();
                result.Add(ITEMS, list.OrderByDescending(x => x.Text));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboEmpleados()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = finiquitoFactoryServices.getFiniquitoService().getListaConceptos();
                result.Add(ITEMS, list.OrderBy(x => x.Value));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarFiniquito(tblRH_Finiquito general, List<tblRH_FiniquitoDetalle> detalle, string fechaBaja)
        {
            var result = new Dictionary<string, object>();

            try
            {
                DateTime temp;
                if (DateTime.TryParse(fechaBaja, out temp))
                {
                    general.fechaBaja = DateTime.Parse(fechaBaja);
                }

                var finiquito = finiquitoFactoryServices.getFiniquitoService().GuardarFiniquito(general, detalle);
                var data = new FiniquitoDTO
                {
                    id = finiquito.id,
                    claveEmpleado = finiquito.claveEmpleado,
                    nombre = finiquito.nombre,
                    ape_paterno = finiquito.ape_paterno,
                    ape_materno = finiquito.ape_materno,
                    fechaAlta = finiquito.fechaAlta,
                    fechaBaja = finiquito.fechaBaja,
                    puestoID = finiquito.puestoID,
                    puesto = finiquito.puesto,
                    tipoNominaID = finiquito.tipoNominaID,
                    tipoNomina = finiquito.tipoNomina,
                    ccID = finiquito.ccID,
                    cc = finiquito.cc,
                    salarioBase = finiquito.salarioBase,
                    complemento = finiquito.complemento,
                    bono = finiquito.bono,
                    formuloID = finiquito.formuloID,
                    formuloNombre = finiquito.formuloNombre,
                    voboID = finiquito.voboID,
                    voboNombre = finiquito.voboNombre,
                    autorizoID = finiquito.autorizoID,
                    autorizoNombre = finiquito.autorizoNombre,
                    total = finiquito.total,
                    autorizado = finiquito.autorizado
                };

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

        public void EnviarCorreos(int empleadoClave, int usuarioID)
        {
            if (usuarioID != 0)
            {
                var downloadPDF = (List<Byte[]>)Session["downloadPDF"];

                try
                {
                    var infoEmpleado = finiquitoFactoryServices.getFiniquitoService().getEmpleadoForId(empleadoClave);
                    var c = finiquitoFactoryServices.getFiniquitoService().getUsuario(usuarioID);
                    var subject = "Autorización de Finiquito para empleado: " + infoEmpleado.nombre + " " + infoEmpleado.ape_paterno + " " + infoEmpleado.ape_materno;

                    #region imagen
                    //var body = @"<img src='data:image/png;base64,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'/><br/><br/>";
                    #endregion

                    var body = @"Estimado(a): " + c.nombre.ToUpper() + " " + c.apellidoPaterno.ToUpper() + " " + c.apellidoMaterno.ToUpper() + "<br/>Se requiere su autorización para el finiquito del empleado: " + infoEmpleado.nombre + " " + infoEmpleado.ape_paterno + " " + infoEmpleado.ape_materno + " .<br/>" + "<br/><br/>";
                    body += @" ";
                    List<string> contactos = new List<string>();

                    contactos.Add(c.correo);

                    var tipoFormato = "Reporte.pdf";
                    GlobalUtils.sendEmailAdjuntoInMemory2(subject, body, contactos, downloadPDF, tipoFormato);
                    Session["downloadPDF"] = null;
                }
                catch (Exception e)
                {
                }
            }
        }

        public ActionResult getEmpleados(string term)
        {
            var items = finiquitoFactoryServices.getFiniquitoService().getEmpleadosTodos(term);
            var filteredItems = items.Select(x => new { id = x.clave_empleado, label = x.Nombre });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutorizaFiniquito(int aut, int finiquitoID)
        {


            var result = new Dictionary<string, object>();

            try
            {
                var finiquito = finiquitoFactoryServices.getFiniquitoService().AutorizaFiniquito(aut, finiquitoID);
                var data = new FiniquitoDTO
                {
                    id = finiquito.id,
                    claveEmpleado = finiquito.claveEmpleado,
                    nombre = finiquito.nombre,
                    ape_paterno = finiquito.ape_paterno,
                    ape_materno = finiquito.ape_materno,
                    fechaAlta = finiquito.fechaAlta,
                    fechaBaja = finiquito.fechaBaja,
                    puestoID = finiquito.puestoID,
                    puesto = finiquito.puesto,
                    tipoNominaID = finiquito.tipoNominaID,
                    tipoNomina = finiquito.tipoNomina,
                    ccID = finiquito.ccID,
                    cc = finiquito.cc,
                    salarioBase = finiquito.salarioBase,
                    complemento = finiquito.complemento,
                    bono = finiquito.bono,
                    formuloID = finiquito.formuloID,
                    formuloNombre = finiquito.formuloNombre,
                    voboID = finiquito.voboID,
                    voboNombre = finiquito.voboNombre,
                    autorizoID = finiquito.autorizoID,
                    autorizoNombre = finiquito.autorizoNombre,
                    total = finiquito.total,
                    autorizado = finiquito.autorizado
                };

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

        public ActionResult GetDetalleFin(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = finiquitoFactoryServices.getFiniquitoService().GetDetalleFin(id);

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

        public ActionResult GetAutorizaciones(int finiquitoID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = finiquitoFactoryServices.getFiniquitoService().GetAutorizaciones(finiquitoID);

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

        public ActionResult GetConceptos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = finiquitoFactoryServices.getFiniquitoService().getConceptos();

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

        public ActionResult FillComboOperador()
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> list = new List<ComboDTO>();

                list.Add(new ComboDTO
                {
                    Value = 1,
                    Text = "(+)"
                });

                list.Add(new ComboDTO
                {
                    Value = 0,
                    Text = "(-)"
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

        public ActionResult GetDetalleConcepto(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = finiquitoFactoryServices.getFiniquitoService().GetDetalleConcepto(id);

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

        public void GuardarConcepto(string concepto, string detalle, bool operador)
        {
            finiquitoFactoryServices.getFiniquitoService().GuardarConcepto(concepto, detalle, operador);
        }

        public void UpdateConcepto(int id, string concepto, string detalle, bool operador)
        {
            finiquitoFactoryServices.getFiniquitoService().UpdateConcepto(id, concepto, detalle, operador);
        }

        public void RemoveConcepto(int id)
        {
            finiquitoFactoryServices.getFiniquitoService().RemoveConcepto(id);
        }

        public ActionResult CheckDatesDiferencia(string ingreso, string egreso)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var ingresoDate = DateTime.Parse(ingreso);
                var egresoDate = DateTime.Parse(egreso);

                var diferencia = egresoDate.Subtract(ingresoDate).Days;

                if (diferencia > 0)
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

        public ActionResult EgresoChange(string ingreso, string egreso)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var ingresoDate = DateTime.Parse(ingreso);
                var egresoDate = DateTime.Parse(egreso);

                var diasTrans = 0;
                var year = egresoDate.Year;
                DateTime firstDay = new DateTime(year, 1, 1);
                if (ingresoDate > firstDay)
                {
                    diasTrans = egresoDate.Subtract(ingresoDate).Days + 1;
                }
                else {
                    diasTrans = egresoDate.Subtract(firstDay).Days + 1;
                }

                decimal anios = 0;
                decimal diasTotales = egresoDate.Subtract(ingresoDate).Days;
                anios = (diasTotales / 365);

                result.Add("diasTrans", diasTrans);
                result.Add("anios", anios);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrimaAntiguedad(string ingreso, string egreso)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var resultado = finiquitoFactoryServices.getFiniquitoService().GetPrimaAntiguedad(ingreso, egreso);

                result.Add("data", resultado);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboVacacionesPeriodos(string ingreso, string egreso, decimal anios)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var ingresoDate = DateTime.Parse(ingreso);
                var egresoDate = DateTime.Parse(egreso);

                var aniosFloor = Math.Ceiling(anios);

                List<ComboDTO> lista = new List<ComboDTO>();

                for (int i = 0; i < aniosFloor; i++)
                {
                    var j = i + 1;
                    var diasVac = 0;
                    if (j >= 1)
                    {
                        diasVac = 6;
                    }
                    if (j >= 2)
                    {
                        diasVac = 8;
                    }
                    if (j >= 3)
                    {
                        diasVac = 10;
                    }
                    if (j >= 4)
                    {
                        diasVac = 12;
                    }
                    if (j >= 5 && j < 10)
                    {
                        diasVac = 14;
                    }
                    if (j >= 10 && j < 15)
                    {
                        diasVac = 16;
                    }
                    if (j >= 15 && j < 20)
                    {
                        diasVac = 18;
                    }
                    if (j >= 20)
                    {
                        diasVac = 20;
                    }

                    lista.Add(new ComboDTO
                    {
                        Value = i + 1,
                        Text = (ingresoDate.Year + i).ToString() + " - " + (ingresoDate.Year + (i + 1)).ToString() + ". " + diasVac + " días."
                    });
                }

                result.Add(ITEMS, lista);
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