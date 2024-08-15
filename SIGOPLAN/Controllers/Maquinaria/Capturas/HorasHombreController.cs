using Core.DTO.Maquinaria.Captura.OT.rptConcentradoHH;
using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria.Captura;
using Data.Factory.Maquinaria.Captura.HorasHombre;
using Data.Factory.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Capturas
{
    public class HorasHombreController : BaseController
    {

        UsuarioFactoryServices usuarioFactoryServices;
        CapHorasHombreFactoryServices capHorasHombreFactoryServices;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            usuarioFactoryServices = new UsuarioFactoryServices();
            capHorasHombreFactoryServices = new CapHorasHombreFactoryServices();
            base.OnActionExecuting(filterContext);
        }



        // GET: HorasHombre
        public ActionResult CapHorasHombre()
        {
            return View();
        }

        public ActionResult searchEmpleado(string term, int puesto)
        {
            var items = capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().getNombreEmpleado(term, puesto);

            var filteredItems = items.Select(x => new { id = x.Value, label = x.Text });

            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        public ActionResult searchNumEmpleado(string term, int puesto,int numEmpleado)
        {
            var items = capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().searchNumEmpleado(term, puesto, numEmpleado);

            var filteredItems = (new { id = items.Value, label = items.Text ,prefijo = items.Prefijo, idPuesto = items.Id });

            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }


        public ActionResult fillCboPuestos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbodata = capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().fillCboPuestos();
                result.Add(ITEMS, cbodata);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult fillCboCC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioID = getUsuario().id;

                var listaCCusuario = usuarioFactoryServices.getUsuarioService().getCCsUsuario(usuarioID).Select(x => x.cc);

                var cbodata = capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().fillCboCC().Where(x => listaCCusuario.Contains(x.Value)).ToList();
                result.Add(ITEMS, cbodata.Select(x => new ComboDTO { Text = x.Value + "-" + x.Text, Value = x.Value }).OrderBy(x => x.Value).ToList());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult FillComboDepartamentos(string cc)
        {
            return Json(capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().FillComboDepartamentos(cc), JsonRequestBehavior.AllowGet);
        }

        public ActionResult loadCargaHorasHombre(string cc, int clave_depto, DateTime fecha, int turno, int puesto, int empleado)
        {
            var result = new Dictionary<string, object>();
            try
            {

                List<int> lista = new List<int>();

                var rawData = capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().loadTblHorasHombre(cc, clave_depto, fecha, turno, getUsuario().id, puesto, empleado).ToList();

                foreach (var item in capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().getSubCategorias())
                {
                    result.Add(item.Key, item.Value);
                }

                //var rawDataCbo = capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().getSubCategorias(lista).ToList();

                //result.Add("catTrabajosInstalacionesLista", rawDataCbo.GroupBy(x => new { x.categoriaID, x.id, x.descripcion }).Where(x => x.Key.categoriaID == 2).Select(y => new
                //{
                //    Value = y.Key.id,
                //    Text = y.Key.descripcion,
                //    categoria = y.Key.categoriaID

                //}));
                //result.Add("catLimpiezaLista", rawDataCbo.GroupBy(x => new { x.categoriaID, x.id, x.descripcion }).Where(x => x.Key.categoriaID == 3).Select(y => new
                //{
                //    Value = y.Key.id,
                //    Text = y.Key.descripcion,
                //    categoria = y.Key.categoriaID

                //}));
                //result.Add("catConsultaInformacionLista", rawDataCbo.GroupBy(x => new { x.categoriaID, x.id, x.descripcion }).Where(x => x.Key.categoriaID == 4).Select(y => new
                //{
                //    Value = y.Key.id,
                //    Text = y.Key.descripcion,
                //    categoria = y.Key.categoriaID

                //}));

                //result.Add("catTiempoDescansoLista", rawDataCbo.GroupBy(x => new { x.categoriaID, x.id, x.descripcion }).Where(x => x.Key.categoriaID == 5).Select(y => new
                //{
                //    Value = y.Key.id,
                //    Text = y.Key.descripcion,
                //    categoria = y.Key.categoriaID

                //}));
                //result.Add("catCursosCapacitacionesLista", rawDataCbo.GroupBy(x => new { x.categoriaID, x.id, x.descripcion }).Where(x => x.Key.categoriaID == 6).Select(y => new
                //{
                //    Value = y.Key.id,
                //    Text = y.Key.descripcion,
                //    categoria = y.Key.categoriaID

                //}));
                //result.Add("catMonitoreoDiario", rawDataCbo.GroupBy(x => new { x.categoriaID, x.id, x.descripcion }).Where(x => x.Key.categoriaID == 7).Select(y => new
                //{
                //    Value = y.Key.id,
                //    Text = y.Key.descripcion,
                //    categoria = y.Key.categoriaID

                //}));

                result.Add("dtSet", rawData);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult saveorUpdateInformacion(List<tblM_CapHorasHombre> obj, string cc, int turno, DateTime fechaCaptura)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<tblM_CapHorasHombre> objList = new List<tblM_CapHorasHombre>();
                foreach (var item in obj)
                {

                    if (item.tiempo != 0 && item.subCategoria != 0)
                    {
                        item.centroCostos = cc;
                        item.fechaCaptura = fechaCaptura;
                        item.usuarioCapturaID = getUsuario().id;
                        item.turno = turno;
                        objList.Add(item);
                    }

                    /* if (item.valueConsultaInformacion != 0 && item.subCatConsultaInformacion != 0)
                     {
                         tblM_CapHorasHombre ObjCapHorasHombre = new tblM_CapHorasHombre();
                         ObjCapHorasHombre.id = item.idcatConsultaInformacion;
                         if (ObjCapHorasHombre.id == 0)
                         {
                             ObjCapHorasHombre.categoriaTrabajo = item.catConsultaInformacion;
                             ObjCapHorasHombre.centroCostos = cc;
                             ObjCapHorasHombre.fechaCaptura = fechaCaptura;
                             ObjCapHorasHombre.nombreEmpleado = item.empleado;
                             ObjCapHorasHombre.numEmpleado = item.personalID;
                             ObjCapHorasHombre.subCategoria = item.subCatConsultaInformacion;
                             ObjCapHorasHombre.tiempo = item.valueConsultaInformacion;
                             ObjCapHorasHombre.turno = turno;
                             ObjCapHorasHombre.usuarioCapturaID = getUsuario().id;
                             ObjCapHorasHombre.puestoID = item.puestoID;

                             objList.Add(ObjCapHorasHombre);
                         }
                     }
                     if (item.subCatCursosCapacitaciones != 0 && item.valueCursosCapacitaciones != 0)
                     {
                         tblM_CapHorasHombre ObjCapHorasHombre = new tblM_CapHorasHombre();
                         ObjCapHorasHombre.id = item.idcatCursosCapacitaciones;
                         if (ObjCapHorasHombre.id == 0)
                         {
                             ObjCapHorasHombre.categoriaTrabajo = item.catCursosCapacitaciones;
                             ObjCapHorasHombre.centroCostos = cc;
                             ObjCapHorasHombre.fechaCaptura = fechaCaptura;
                             ObjCapHorasHombre.nombreEmpleado = item.empleado;
                             ObjCapHorasHombre.numEmpleado = item.personalID;
                             ObjCapHorasHombre.subCategoria = item.subCatCursosCapacitaciones;
                             ObjCapHorasHombre.tiempo = item.valueCursosCapacitaciones;
                             ObjCapHorasHombre.turno = turno;
                             ObjCapHorasHombre.usuarioCapturaID = getUsuario().id;
                             ObjCapHorasHombre.puestoID = item.puestoID;
                             objList.Add(ObjCapHorasHombre);
                         }
                     }

                     if (item.subCatLimpieza != 0 && item.valueLimpieza != 0)
                     {
                         tblM_CapHorasHombre ObjCapHorasHombre = new tblM_CapHorasHombre();
                         ObjCapHorasHombre.id = item.idcatLimpieza;
                         if (ObjCapHorasHombre.id == 0)
                         {
                             ObjCapHorasHombre.categoriaTrabajo = item.catLimpieza;
                             ObjCapHorasHombre.centroCostos = cc;
                             ObjCapHorasHombre.fechaCaptura = fechaCaptura;
                             ObjCapHorasHombre.nombreEmpleado = item.empleado;
                             ObjCapHorasHombre.numEmpleado = item.personalID;
                             ObjCapHorasHombre.subCategoria = item.subCatLimpieza;
                             ObjCapHorasHombre.tiempo = item.valueLimpieza;
                             ObjCapHorasHombre.turno = turno;
                             ObjCapHorasHombre.usuarioCapturaID = getUsuario().id;
                             ObjCapHorasHombre.puestoID = item.puestoID;
                             objList.Add(ObjCapHorasHombre);
                         }
                     }
                     if (item.valueTiempoDescanso != 0 && item.subCatTiempoDescanso != 0)
                     {
                         tblM_CapHorasHombre ObjCapHorasHombre = new tblM_CapHorasHombre();
                         ObjCapHorasHombre.id = item.idcatTiempoDescanso;
                         if (ObjCapHorasHombre.id == 0)
                         {
                             ObjCapHorasHombre.categoriaTrabajo = item.catTiempoDescanso;
                             ObjCapHorasHombre.centroCostos = cc;
                             ObjCapHorasHombre.fechaCaptura = fechaCaptura;
                             ObjCapHorasHombre.nombreEmpleado = item.empleado;
                             ObjCapHorasHombre.numEmpleado = item.personalID;
                             ObjCapHorasHombre.subCategoria = item.subCatTiempoDescanso;
                             ObjCapHorasHombre.tiempo = item.valueTiempoDescanso;
                             ObjCapHorasHombre.turno = turno;
                             ObjCapHorasHombre.usuarioCapturaID = getUsuario().id;
                             ObjCapHorasHombre.puestoID = item.puestoID;
                             objList.Add(ObjCapHorasHombre);
                         }
                     }
                     if (item.subCatTrabajosInstalaciones != 0 && item.valueTrabajosInstalaciones != 0)
                     {

                         tblM_CapHorasHombre ObjCapHorasHombre = new tblM_CapHorasHombre();
                         ObjCapHorasHombre.id = item.idcatTrabajosInstalaciones;
                         if (ObjCapHorasHombre.id == 0)
                         {
                             ObjCapHorasHombre.categoriaTrabajo = item.catTrabajosInstalaciones;
                             ObjCapHorasHombre.centroCostos = cc;
                             ObjCapHorasHombre.fechaCaptura = fechaCaptura;
                             ObjCapHorasHombre.nombreEmpleado = item.empleado;
                             ObjCapHorasHombre.numEmpleado = item.personalID;
                             ObjCapHorasHombre.subCategoria = item.subCatTrabajosInstalaciones;
                             ObjCapHorasHombre.tiempo = item.valueTrabajosInstalaciones;
                             ObjCapHorasHombre.turno = turno;
                             ObjCapHorasHombre.usuarioCapturaID = getUsuario().id;
                             ObjCapHorasHombre.puestoID = item.puestoID;
                             objList.Add(ObjCapHorasHombre);
                         }

                     }

                     if (item.subCatMonitoreoDiario != 0 && item.valueMonitoreoDiario != 0)
                     {

                         tblM_CapHorasHombre ObjCapHorasHombre = new tblM_CapHorasHombre();
                         ObjCapHorasHombre.id = item.idcatMonitoreoDiario;
                         if (ObjCapHorasHombre.id == 0)
                         {
                             ObjCapHorasHombre.categoriaTrabajo = item.catMonitoreoDiario;
                             ObjCapHorasHombre.centroCostos = cc;
                             ObjCapHorasHombre.fechaCaptura = fechaCaptura;
                             ObjCapHorasHombre.nombreEmpleado = item.empleado;
                             ObjCapHorasHombre.numEmpleado = item.personalID;
                             ObjCapHorasHombre.subCategoria = item.subCatMonitoreoDiario;
                             ObjCapHorasHombre.tiempo = item.valueMonitoreoDiario;
                             ObjCapHorasHombre.turno = turno;
                             ObjCapHorasHombre.usuarioCapturaID = getUsuario().id;
                             ObjCapHorasHombre.puestoID = item.puestoID;
                             objList.Add(ObjCapHorasHombre);
                         }

                     }*/

                }

                capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().guardarInformacion(objList);

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