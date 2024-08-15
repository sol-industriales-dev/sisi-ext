using Core.DAO.Maquinaria.Captura;
using Core.DAO.Maquinaria.Catalogos;
using Core.DAO.Maquinaria.Reporte;
using Core.DTO.Maquinaria.Reporte;
using Core.Enum.Maquinaria.Reportes;
using Data.DAO.Maquinaria.Reporte;
using Data.Factory.Maquinaria.Captura;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Reporte;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Inventario
{
    public class AltaBajaEquiposController : BaseController
    {
        #region Factory
        private IMaquinaDAO maquinaFS;
        private IParos parosFS;
        private ICapturaHorometroDAO horometroFS;
        private ICentroCostosDAO centroCostosFS;
        private IRepComparativaTiposDAO comparativaFS;
        private IdocumentosMaquinariaDAO docMaquinariaFS;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            maquinaFS = new MaquinaFactoryServices().getMaquinaServices();
            parosFS = new ParosFactoryServices().getParosServices();
            horometroFS = new CapturaHorometroFactoryServices().getCapturaHorometroServices();
            centroCostosFS = new CentroCostosFactoryServices().getCentroCostosService();
            comparativaFS = new RepComparativaTiposFactoryServices().getComparativoTiposService();
            docMaquinariaFS = new DocumentosMaquinariaFactoryServices().getDocumentosMaquinariaFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        #endregion
        // GET: AltaBajaEquipos
        public ActionResult FichaTecnicaAView()
        {
            return View();
        }
        public ActionResult BuscarFicha(string obj)
        {
            var objMaquina = new List<object>();
            RepGastosMaquinaDAO repGastosMaquina = new RepGastosMaquinaDAO();
            var maquina = maquinaFS.GetMaquinaByNoEconomico(obj);
            if(maquina != null)
            {
                var CostoHora = 0;
                var paro = parosFS.getParosMaquina(maquina.id).First();
                var horometro = horometroFS.getUltimoHorometro(maquina.noEconomico);
                var CC = centroCostosFS.getNombreCCFix(maquina.centro_costos);
                var RES = repGastosMaquina.FillInfoGastosMaquinaria(maquina.noEconomico);
                var saldoinicial = "";
                if(RES.Count > 0)
                {
                    var gastos = RES.Select(x => new RepGastosMaquinaInfoDTO { depreciacion = Convert.ToDecimal(x.depreciacion).ToString("C2"), descripcion = x.descripcion, fechaAdquisicion = x.fechaAdquisicion, marca = x.marca, modelo = x.modelo, saldoinicial = Convert.ToDecimal(x.saldoinicial).ToString("C2") }).FirstOrDefault();
                    saldoinicial = gastos.saldoinicial;
                }
                var costoOverHaul = repGastosMaquina.valorXoverhaul(obj, maquina.fechaAdquisicion, DateTime.Now);
                var costoOverHaulAplicado = repGastosMaquina.valorXoverhaulAplicadoByMaquina(maquina.noEconomico);
                var GetDatosDocumentos = docMaquinariaFS.getByEconomico(maquina.id);
                objMaquina.Add(new
                {
                    noEconomico = maquina.noEconomico,
                    descripcion = maquina.descripcion,
                    marca = maquina.marca.descripcion,
                    modelo = maquina.modeloEquipo.descripcion,
                    noSerie = maquina.noSerie,
                    anio = maquina.anio,
                    fechaCompra = maquina.fechaAdquisicion.ToString("dd/MM/yyyy"),
                    horometroInicio = maquina.horometroAdquisicion,
                    horometroActual = horometro != null ? horometro.Horometro : 0,
                    fechaParo = paro.id > 0 ? paro.fecha_paro.ToString("dd/MM/yyyy") : "",
                    detParo = paro.id > 0 ? paro.descripcion : "",
                    ubicacion = maquina.centro_costos + " - " + CC,
                    costoAdquisicion = saldoinicial,
                    costoOverHaul = Convert.ToDecimal(costoOverHaul).ToString("C2"),
                    costoOverHaulAplicado = Convert.ToDecimal(costoOverHaulAplicado).ToString("C2"),
                    CostoHora = Convert.ToDecimal(CostoHora).ToString("C2"),
                    factura = GetDatosDocumentos.factura,
                    pedimento = GetDatosDocumentos.pedimento,
                    poliza = GetDatosDocumentos.poliza,
                    tarjetaCirculacion = GetDatosDocumentos.tarjetaCirculacion,
                    permisoCarga = GetDatosDocumentos.permisoCarga,
                    certificacion = GetDatosDocumentos.certificacion,
                    facturaID = GetDatosDocumentos.facturaID,
                    pedimentoID = GetDatosDocumentos.pedimentoID,
                    polizaID = GetDatosDocumentos.polizaID,
                    tarjetaCirculacionID = GetDatosDocumentos.tarjetaCirculacionID,
                    permisoCargaID = GetDatosDocumentos.permisoCargaID,
                    certificacionID = GetDatosDocumentos.certificacionID
                });
            }
            else
            {
                objMaquina.Add(new
                {
                    noEconomico = "",
                    descripcion = "",
                    marca = "",
                    modelo = "",
                    noSerie = "",
                    anio = "",
                    fechaCompra = "",
                    horometroInicio = "",
                    horometroActual = "",
                    fechaParo = "",
                    detParo = "",
                    ubicacion = "",
                    costoAdquisicion = "",
                    costoOverHaul = "",
                    costoOverHaulAplicado = "",
                    factura = false,
                    pedimento = false,
                    poliza = false,
                    tarjetaCirculacion = false,
                    permisoCarga = false,
                    certificacion = false

                });
            }
            Session["cr" + ReportesEnum.MRepFichaTecnica] = JsonConvert.SerializeObject(objMaquina);
            return Json(objMaquina.First(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult EconomicoDesripcion(string term)
        {
            var items = maquinaFS.EconomicoDesripcion(term);
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetFiltros(int obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var Maquinaria = maquinaFS.GetMaquina(obj);
                result.Add("Tipo", Maquinaria.grupoMaquinaria.tipoEquipoID);
                result.Add("Grupo", Maquinaria.grupoMaquinariaID);
                result.Add("EconomicoID", Maquinaria.id);
                result.Add("noEconomico", Maquinaria.noEconomico);
                result.Add(SUCCESS, true);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}