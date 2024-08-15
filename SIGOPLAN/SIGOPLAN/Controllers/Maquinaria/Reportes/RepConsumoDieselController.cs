using Core.DTO.Maquinaria.Catalogos;
using Core.DTO.Maquinaria.Reporte;
using Core.Enum.Maquinaria.Reportes;
using Data.Factory.Maquinaria.Captura;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Inventario;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Reportes
{
    public class RepConsumoDieselController : BaseController
    {

        private AsignacionEquiposFactoryServices AsignacionEquiposServices = new AsignacionEquiposFactoryServices();
        private MaquinaFactoryServices maquinaFactoryServices = new MaquinaFactoryServices();
        private CapturaCombustibleFactoryServices capturaCombustibleFactoryServices = new CapturaCombustibleFactoryServices();
        private PrecioDieselFactoryServices precioDieselFactoryServices = new PrecioDieselFactoryServices();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getConsumoMaquinas(string ccs)
        {
            ConsumoDieselTotales objResult = new ConsumoDieselTotales();

            List<string> lstCcs = new List<string>();
            lstCcs.Add(ccs);

            List<ConsumoDieselDTO> lstConsumoDiesel = new List<ConsumoDieselDTO>();

            MaquinaFiltrosDTO objBuscar = new MaquinaFiltrosDTO();
            objBuscar.ccId = ccs;

            var lstMaquinasDiesel = capturaCombustibleFactoryServices.getCapturaCombustiblesServices().getConsumoCombustibles(ccs, DateTime.Now, null).Select(x=> x.Economico).Distinct().ToList();
            var ConsumoEnkontrol = capturaCombustibleFactoryServices.getCapturaCombustiblesServices().getTotalEnkontrolConsumoDiesel(ccs);

            foreach (var economico in lstMaquinasDiesel)
            {
                ConsumoDieselDTO objConsumo = new ConsumoDieselDTO();


                objConsumo.economico = economico;
                objConsumo.descripcion = maquinaFactoryServices.getMaquinaServices().GetMaquinaByNoEconomico(economico).descripcion;

                var objCombustible = capturaCombustibleFactoryServices.getCapturaCombustiblesServices().getConsumoCombustibles(ccs, DateTime.Now, economico);

                decimal precioD = precioDieselFactoryServices.getPrecioDieselService().GetPrecioDiesel().precio;
                decimal suma = 0;
                decimal totalPrecio = 0;


                foreach(var objSumar in objCombustible)
                {
                    suma += objSumar.volumne_carga;
                    totalPrecio += objSumar.volumne_carga * precioD;
                }

                objConsumo.consumo = suma;
                objConsumo.importe = totalPrecio;
                objConsumo.importeKontrol = Decimal.Round(ConsumoEnkontrol.Where(x => x.Economico.Equals(economico)).Sum(x => x.IMPORTE), 2);

                lstConsumoDiesel.Add(objConsumo);
                
            }

            decimal totalConsumido = 0;
            foreach (var objcontar in lstConsumoDiesel)
            {
                totalConsumido += objcontar.importe;
                objcontar.descripcion = objcontar.descripcion==null? "BAJA" : objcontar.descripcion;
            }

            Session["cr" + ReportesEnum.MConsumoDiesel] = JsonConvert.SerializeObject(lstConsumoDiesel);

            objResult.lstConsumos = lstConsumoDiesel;
            objResult.totalConsumido = Decimal.Round(totalConsumido,2);
            objResult.totalEnKontrol = Decimal.Round(ConsumoEnkontrol.Sum(x => x.IMPORTE), 2);
            objResult.totalContratistas = Decimal.Round(capturaCombustibleFactoryServices.getCapturaCombustiblesServices().getTotalContratistaConsumoDiesel(ccs) * -1,2);
            objResult.totalProvisionar = Decimal.Round(objResult.totalConsumido - objResult.totalEnKontrol - objResult.totalContratistas, 2);

            Session["crTotalConsumo"] = "$ " + objResult.totalConsumido.ToString("N1", CultureInfo.InvariantCulture);
            Session["crtotalEnKontrol"] = "$ " + objResult.totalEnKontrol.ToString("N1", CultureInfo.InvariantCulture);
            Session["crtotalContratistas"] = "$ " + objResult.totalContratistas.ToString("N1", CultureInfo.InvariantCulture);
            Session["crtotalProvisionar"] = "$ " + objResult.totalProvisionar.ToString("N1", CultureInfo.InvariantCulture);
            Session["crtotalCCs"] = ccs;


            return Json(objResult, JsonRequestBehavior.AllowGet);
        }
    }
}