using Core.DTO;
using Core.DAO.Maquinaria.Catalogos;
using Core.DAO.Maquinaria.Reporte;
using Core.DAO.Principal.Usuarios;
using Core.DTO.Maquinaria.Reporte.Rentabilidad;
using Core.Enum.Maquinaria.Reportes.Rentabilidad;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Reporte;
using Data.Factory.Principal.Usuarios;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Factory.Maquinaria.Captura;
using Core.DAO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Reporte.Analisis;
using Core.DTO.Maquinaria.Reporte.Kubrix;
using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria.Rentabilidad;
using Core.DTO.Maquinaria.Rentabilidad;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using Infrastructure.DTO;
using System.Data.Entity;
using Core.Enum.Multiempresa;
using System.Threading;
using System.Threading.Tasks;

namespace SIGOPLAN.Controllers.Maquinaria.Rentabilidad
{
    public class RentabilidadController : BaseController
    {
        IRentabilidadDAO rentabilidadFS;
        ICapturaHorometroDAO capHorometrosFS;
        IUsuarioDAO usuariosFS;
        ICentroCostosDAO centroCostosFS;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            rentabilidadFS = new RentabilidadFactoryServices().getRentabilidadDAO();
            capHorometrosFS = new CapturaHorometroFactoryServices().getCapturaHorometroServices();
            usuariosFS = new UsuarioFactoryServices().getUsuarioService();
            centroCostosFS = new CentroCostosFactoryServices().getCentroCostosService();
            base.OnActionExecuting(filterContext);
        }
        // GET: Rentabilidad

        //-->
        public ActionResult CorteKubrixCplan()
        {
            return View();
        }
        //<--
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Analisis()
        {
            return View();
        }
        public ActionResult Kubrix()
        {
            return View();
        }
        public ActionResult Corte()
        {
            return View();
        }
        public ActionResult CorteCplan()
        {
            return View();
        }
        public ActionResult CorteCostos()
        {
            return View();
        }
        public ActionResult CorteFlujo()
        {
            return View();
        }
        public ActionResult GuardarArrendadora()
        {
            return View();
        }
        public ActionResult GuardarConstruplan()
        {
            return View();
        }
        public ActionResult ExportarReporte()
        {
            return View();
        }
        public ActionResult KubrixConstruplan()
        {
            return View();
        }
        public ActionResult ControlEstimaciones()
        {
            return View();
        }

        public ActionResult KubrixCorteArrendadora()
        {
            return View();
        }
        public ActionResult KubrixCorteConstruplan()
        {
            return View();
        }
        public ActionResult getLstRentabilidad(BusqRentabilidadDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = rentabilidadFS.getLstRentabilidad(busq);
                var esSuccess = lst.Count > 0;
                if (esSuccess)
                {
                    object listaTabla = null;

                    switch (busq.tipoReporte)
                    {
                        case Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoReporteEnum.Costos:
                            listaTabla = ObtenerListaCostos(lst);
                            break;
                        case Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoReporteEnum.Ingresos:
                            lst.ForEach(x => { x.importe = x.importe * -1; });
                            listaTabla = ObtenerListaIngresos(lst);
                            break;
                        case Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoReporteEnum.Rentabilidad:
                            listaTabla = obtenerListaUtilidad(lst, busq.min, busq.max);
                            break;
                        case Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoReporteEnum.CostoHora:
                            listaTabla = obtenerListaCostoHora(lst, busq.min, busq.max);
                            break;
                        case Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoReporteEnum.PresupuestoMaquina:
                            listaTabla = obtenerListaPptoMaq(lst, busq.min, busq.max);
                            break;
                        default:
                            break;
                    }
                    result.Add("lst", listaTabla);
                }

                result.Add(SUCCESS, esSuccess);

                if (esSuccess == false)
                {
                    result.Add(MESSAGE, "No se encontraron registros con los filtros seleccionados.");
                }
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult getLstRentabilidadDetalle(BusqRentabilidadDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = rentabilidadFS.getLstRentabilidadDetalle(busq);
                var esSuccess = lst.Count > 0;
                if (esSuccess)
                {
                    object listaTabla = null;

                    switch (busq.tipoReporte)
                    {
                        case Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoReporteEnum.Costos:
                            listaTabla = ObtenerListaCostos(lst);
                            break;
                        case Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoReporteEnum.Ingresos:
                            lst.ForEach(x => { x.importe = x.importe * -1; });
                            listaTabla = ObtenerListaIngresos(lst);
                            break;
                        case Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoReporteEnum.Rentabilidad:
                            listaTabla = obtenerListaUtilidad(lst, busq.min, busq.max);
                            break;
                        case Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoReporteEnum.CostoHora:
                            listaTabla = obtenerListaCostoHora(lst, busq.min, busq.max);
                            break;
                        case Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoReporteEnum.PresupuestoMaquina:
                            listaTabla = obtenerListaPptoMaq(lst, busq.min, busq.max);
                            break;
                        default:
                            break;
                    }
                    result.Add("lst", listaTabla);
                }

                result.Add(SUCCESS, esSuccess);

                if (esSuccess == false)
                {
                    result.Add(MESSAGE, "No se encontraron registros con los filtros seleccionados.");
                }
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        #region combobox
        public ActionResult cboObra()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioID = getUsuario().id;
                var cbo = rentabilidadFS.getListaCCByUsuario(usuarioID, 1);
                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboObraKubrix(int divisionID = -1, int responsableID = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioID = getUsuario().id;
                var responsable = rentabilidadFS.checkResponsable(-1, usuarioID);
                var cbo = new List<Core.DTO.Principal.Generales.ComboDTO>();
                if (divisionID == -1 && responsableID == -1 && !responsable) cbo.Add(new Core.DTO.Principal.Generales.ComboDTO { Value = "S/A", Text = "SIN AREA CUENTA", Prefijo = "-1" });
                var auxCbo = rentabilidadFS.getListaCCByUsuario(usuarioID, 0).ToList();
                var detallesDiv = rentabilidadFS.getACDivision(divisionID);
                var detallesResp = rentabilidadFS.getACResponsable(responsableID);
                if (divisionID != -1) auxCbo = auxCbo.Where(x => detallesDiv.Contains(x.Value)).ToList();
                if (responsableID != -1) auxCbo = auxCbo.Where(x => detallesResp.Contains(x.Value)).ToList();
                cbo.AddRange(auxCbo);

                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cboObraEstimados(int divisionID = -1, int responsableID = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioID = getUsuario().id;
                var responsable = rentabilidadFS.checkResponsable(-1, usuarioID);
                var cbo = new List<Core.DTO.Principal.Generales.ComboDTO>();
                if (divisionID == -1 && responsableID == -1 && !responsable) cbo.Add(new Core.DTO.Principal.Generales.ComboDTO { Value = "S/A", Text = "SIN AREA CUENTA", Prefijo = "-1" });
                var auxCbo = rentabilidadFS.getListaCCByUsuario(usuarioID, 0).ToList();
                var detallesDiv = rentabilidadFS.getACDivision(divisionID);
                var detallesResp = rentabilidadFS.getACResponsable(responsableID);
                if (divisionID != -1) auxCbo = auxCbo.Where(x => detallesDiv.Contains(x.Value)).ToList();
                if (responsableID != -1) auxCbo = auxCbo.Where(x => detallesResp.Contains(x.Value)).ToList();
                foreach (var item in auxCbo) 
                {
                    item.Value = item.Text.Split('-')[0].Split(' ')[0];
                }
                cbo.AddRange(auxCbo);

                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboTipo()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = rentabilidadFS.cboTipo();
                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboGrupo(BusqRentabilidadDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = rentabilidadFS.cboGrupo(busq);
                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboModelo(BusqRentabilidadDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = rentabilidadFS.cboModelo(busq);
                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboMaquina(BusqRentabilidadDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = rentabilidadFS.cboMaquina(busq);
                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public ActionResult cboTipoReporte()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = EnumExtensions.ToCombo<TipoReporteEnum>();
                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult cboAreaCuenta()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = rentabilidadFS.getComboAreaCuenta();
                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        private List<RentabilidadIngresosDTO> ObtenerListaIngresos(List<RentabilidadDTO> listaRentabilidad)
        {
            var listaIngresos = new List<RentabilidadIngresosDTO>();
            listaRentabilidad = listaRentabilidad.Where(x => x.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.Abono
                || x.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.AbonoRojo).ToList();

            foreach (var elemento in listaRentabilidad.GroupBy(x => x.noEco))
            {
                var auxElemento = elemento.Where(x => x.tipoInsumo != "4000-3").ToList();
                var ingreso = new RentabilidadIngresosDTO
                {
                    noEconomico = elemento.Key,
                    modelo = rentabilidadFS.ObtenerModeloEconomico(elemento.Key)
                };

                ingreso = AsignarImportesIngresos(ingreso, elemento);

                ingreso.total = auxElemento.Sum(x => x.importe) * (-1);

                ingreso.detalles = elemento.ToList();

                listaIngresos.Add(ingreso);
            }

            return listaIngresos;
        }

        private RentabilidadIngresosDTO AsignarImportesIngresos(RentabilidadIngresosDTO costo, IGrouping<string, RentabilidadDTO> importes)
        {
            foreach (var item in importes.GroupBy(x => new { x.tipoInsumo, x.cta }))
            {
                var totalImporteTipo = item.Sum(x => x.importe);
                var dto = item.FirstOrDefault();

                if (esRentaEquipos(dto))
                {
                    costo.rentaEquipos += (totalImporteTipo * (-1));
                }
                if (esCobroFlete(dto))
                {
                    costo.cobroFletes += (totalImporteTipo * (-1));
                }
                if (esCobroDanos(dto))
                {
                    costo.cobroDanioEquipos += (totalImporteTipo * (-1));
                }
                //if (esTraspasoMM(dto) || esServicios(dto) || esServiciosAdministrativo(dto))
                //{
                //    costo.ingresoVenta = totalImporteTipo *(-1));
                //}
                if (esReparacionNeumaticos(dto))
                {
                    costo.reparacionNeumaticos += (totalImporteTipo * (-1));
                }
                if (esReservaOverhaul(dto))
                {
                    costo.reservaOverhaul += (totalImporteTipo * (-1));
                }
                if (esMttoEquipos(dto))
                {
                    costo.mttoEquipos += (totalImporteTipo * (-1));
                }
                if (esLentoMovimiento(dto))
                {
                    costo.lentoMovimiento += (totalImporteTipo * (-1));
                }

            }
            return costo;
        }

        private RentabilidadUtilidadDTO AsignarImportesUtilidad(RentabilidadUtilidadDTO costo, IGrouping<string, RentabilidadDTO> importes)
        {
            foreach (var item in importes.GroupBy(x => x.tipo_mov))
            {
                var totalImporteTipo = item.Sum(x => x.importe);
                var dto = item.FirstOrDefault();

                if (esAbono(dto))
                {
                    costo.abono += (totalImporteTipo * -1);
                }
                if (esCargo(dto))
                {
                    costo.cargo += totalImporteTipo;
                }
            }
            return costo;
        }

        private List<RentabilidadCostosDTO> ObtenerListaCostos(List<RentabilidadDTO> listaRentabilidad)
        {
            var listaCostos = new List<RentabilidadCostosDTO>();
            listaRentabilidad = listaRentabilidad.Where(x => x.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.Cargo
                || x.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.CargoRojo).ToList();
            foreach (var elemento in listaRentabilidad.GroupBy(x => x.noEco))
            {
                var costo = new RentabilidadCostosDTO
                {
                    noEconomico = elemento.Key,
                    modelo = rentabilidadFS.ObtenerModeloEconomico(elemento.Key)
                };

                costo = AsignarImportesCostos(costo, elemento);

                costo.total = elemento.Sum(x => x.importe);

                costo.detalles = elemento.ToList();

                var prueba = costo.detalles.Where(x => x.insumo_Desc == "TORNO");
                listaCostos.Add(costo);
            }

            return listaCostos;
        }

        private RentabilidadCostosDTO AsignarImportesCostos(RentabilidadCostosDTO costo, IGrouping<string, RentabilidadDTO> importes)
        {
            foreach (var item in importes.GroupBy(x => new { x.tipoInsumo, x.cta }))
            {
                var totalImporteTipo = item.Sum(x => x.importe);
                var dto = item.FirstOrDefault();

                if (esMaterialesLubricacion(dto))
                {
                    costo.materialesLubricacion = totalImporteTipo;
                }
                if (esServiciosAdministrativo(dto))
                {
                    costo.serviciosAdministrativos = totalImporteTipo;
                }
                if (esHerramientas(dto))
                {
                    costo.herramientas = totalImporteTipo;
                }
                if (esSubcontratos(dto))
                {
                    costo.subcontratos = totalImporteTipo;
                }
                if (esTalleresExternos(dto))
                {
                    costo.talleresExternos = totalImporteTipo;
                }
                if (esRentaMaquinaria(dto))
                {
                    costo.rentaMaquinaria = totalImporteTipo;
                }
                if (esRefacciones(dto))
                {
                    costo.refacciones = totalImporteTipo;
                }
                if (esServicios(dto))
                {
                    costo.servicios = totalImporteTipo;
                }
                if (esCombustibles(dto))
                {
                    costo.combustibles = totalImporteTipo;
                }
                if (esFletes(dto))
                {
                    costo.fletes = totalImporteTipo;
                }
                if (esTraspasoMM(dto))
                {
                    costo.traspasoMM = totalImporteTipo;
                }
                if (esIntereses(dto))
                {
                    costo.intereses = totalImporteTipo;
                }
            }
            return costo;
        }

        private List<RentabilidadUtilidadDTO> obtenerListaUtilidad(List<RentabilidadDTO> listaRentabilidad, DateTime min, DateTime max)
        {
            var listaUtilidad = new List<RentabilidadUtilidadDTO>();
            foreach (var elemento in listaRentabilidad.GroupBy(x => x.noEco))
            {
                var utilidad = new RentabilidadUtilidadDTO
                {
                    noEconomico = elemento.Key,
                    modelo = rentabilidadFS.ObtenerModeloEconomico(elemento.Key)
                };
                utilidad = AsignarImportesUtilidad(utilidad, elemento);
                utilidad.total = elemento.Sum(x => x.importe) * (-1);
                utilidad.detalles = elemento.ToList();
                utilidad.detalles.ForEach(x =>
                {
                    if (x.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.Abono
                        || x.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.AbonoRojo)
                        x.importe = x.importe * -1;
                });
                listaUtilidad.Add(utilidad);
            }
            return listaUtilidad;
        }
        private List<RentabilidadCostoHoraDTO> obtenerListaCostoHora(List<RentabilidadDTO> listaRentabilidad, DateTime min, DateTime max)
        {
            listaRentabilidad = listaRentabilidad.Where(x => x.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.Cargo
                || x.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.CargoRojo).ToList();
            var horometros = capHorometrosFS.getHorometrosEconomicos(listaRentabilidad.Select(x => x.noEco).Distinct().ToList(), min, max);
            var lstCostoHora = listaRentabilidad.GroupBy(g => g.noEco).Select(s =>
            {
                var horometroActual = horometros.Where(x => x.Economico == s.Key);
                var horasFinal = horometroActual != null ? horometroActual.Sum(x => x.HorasTrabajo) : 0;
                return new RentabilidadCostoHoraDTO()
                {
                    noEconomico = s.Key,
                    modelo = rentabilidadFS.ObtenerModeloEconomico(s.Key),
                    horasTrabajadas = horasFinal,
                    materialesLubricacion = s.Where(w => esMaterialesLubricacion(w)).Sum(m => m.importe),
                    refacciones = s.Where(w => esRefacciones(w)).Sum(m => m.importe),
                    herramientas = s.Where(w => esHerramientas(w)).Sum(m => m.importe),
                    combustibles = s.Where(w => esCombustibles(w)).Sum(m => m.importe),
                    talleresExternos = s.Where(w => esTalleresExternos(w)).Sum(m => m.importe),
                    servicios = s.Where(w => esServicios(w)).Sum(m => m.importe),
                    subcontratos = s.Where(w => esSubcontratos(w)).Sum(m => m.importe),
                    fletes = s.Where(w => esFletes(w)).Sum(m => m.importe),
                    traspasoMM = s.Where(w => esTraspasoMM(w)).Sum(m => m.importe),
                    rentaMaquinaria = s.Where(w => esRentaMaquinaria(w)).Sum(m => m.importe),
                    serviciosAdministrativos = s.Where(w => esServiciosAdministrativo(w)).Sum(m => m.importe),
                    intereses = s.Where(w => esIntereses(w)).Sum(m => m.importe),
                    total = s.Sum(m => m.importe),
                    totalCostoHorario = s.Sum(m => m.importe) / (horasFinal > 0 ? horasFinal : 1),
                    detalles = s.ToList()
                };
            }).ToList();
            return lstCostoHora;
        }
        private List<RentabilidadPptoMaqDTO> obtenerListaPptoMaq(List<RentabilidadDTO> listaRentabilidad, DateTime min, DateTime max)
        {
            var horometros = capHorometrosFS.getHorometrosEconomicos(listaRentabilidad.Select(x => x.noEco).Distinct().ToList(), min, max);
            var lstCostoHora = listaRentabilidad.GroupBy(g => g.noEco).Select(s =>
            {
                var horometroActual = horometros.Where(x => x.Economico == s.Key);
                var horasFinal = horometroActual != null ? horometroActual.Sum(x => x.HorasTrabajo) : 0;
                var costoHorario = rentabilidadFS.ObtenerObraCostoHorario(s.Key);
                var bolsaPresupuesto = horasFinal * costoHorario;
                return new RentabilidadPptoMaqDTO()
                {
                    noEconomico = s.Key,
                    modelo = rentabilidadFS.ObtenerModeloEconomico(s.Key),
                    costoHorario = costoHorario,
                    horasTrabajadas = horasFinal,
                    bolsaPresupuesto = bolsaPresupuesto,
                    total = s.Sum(m => m.importe),
                    diferencia = bolsaPresupuesto - s.Sum(m => m.importe),
                    detalles = s.ToList()
                };
            }).ToList();
            return lstCostoHora;
        }
        bool esMaterialesLubricacion(RentabilidadDTO importes)
        {
            return importes.tipoInsumo == "5000-1";
        }
        bool esRefacciones(RentabilidadDTO importes)
        {
            return importes.tipoInsumo == "5000-2";
        }
        bool esHerramientas(RentabilidadDTO importes)
        {
            return importes.tipoInsumo == "5000-3";
        }
        bool esCombustibles(RentabilidadDTO importes)
        {
            return importes.tipoInsumo == "5000-4";
        }
        bool esTalleresExternos(RentabilidadDTO importes)
        {
            return importes.tipoInsumo == "5000-5";
        }
        bool esServicios(RentabilidadDTO importes)
        {
            return importes.tipoInsumo == "5000-6";
        }
        bool esServiciosAdministrativo(RentabilidadDTO importes)
        {
            return importes.tipoInsumo == "5000-7";
        }
        bool esSubcontratos(RentabilidadDTO importes)
        {
            return importes.tipoInsumo == "5000-8";
        }
        bool esFletes(RentabilidadDTO importes)
        {
            return importes.tipoInsumo == "5000-9";
        }
        bool esTraspasoMM(RentabilidadDTO importes)
        {
            return importes.tipoInsumo == "5000-10";
        }
        bool esRentaMaquinaria(RentabilidadDTO importes)
        {
            return importes.tipoInsumo == "5000-11";
        }
        bool esIntereses(RentabilidadDTO importes)
        {
            return importes.tipoInsumo == "5900-3";
        }
        bool esRentaEquipos(RentabilidadDTO importes)
        {
            return importes.tipoInsumo == "4000-1";
        }
        bool esReservaOverhaul(RentabilidadDTO importes)
        {
            return importes.tipoInsumo == "4000-2";
        }
        bool esMttoEquipos(RentabilidadDTO importes)
        {
            return importes.tipoInsumo == "4000-3";
        }
        bool esCobroDanos(RentabilidadDTO importes)
        {
            return importes.tipoInsumo == "4000-4";
        }
        bool esReparacionNeumaticos(RentabilidadDTO importes)
        {
            return importes.tipoInsumo == "4000-5";
        }
        bool esCobroFlete(RentabilidadDTO importes)
        {
            return importes.tipoInsumo == "4000-6";
        }
        bool esLentoMovimiento(RentabilidadDTO importes)
        {
            return importes.tipoInsumo == "4000-7";
        }
        bool esCargo(RentabilidadDTO importes)
        {
            return
                importes.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.Cargo
                || importes.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.CargoRojo;
        }
        bool esAbono(RentabilidadDTO importes)
        {
            return
                importes.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.Abono
                || importes.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.AbonoRojo;
        }

        public ActionResult getLstAnalisis(List<RentabilidadDTO> lista, DateTime fecha, bool ejercicioActual, int tipo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                //var lst = rentabilidadFS.getLstAnalisis(busq);
                var esSuccess = lista.Count > 0;
                if (esSuccess)
                {
                    object listaTabla = null;
                    listaTabla = obtenerListaAnalisis(lista, fecha, tipo);
                    result.Add("lst", listaTabla);
                }

                result.Add(SUCCESS, esSuccess);
                result.Add("fecha", fecha);

                if (esSuccess == false)
                {
                    result.Add(MESSAGE, "No se encontraron registros con los filtros seleccionados.");
                }
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        private List<AnalisisUtilidadDTO> obtenerListaAnalisis(List<RentabilidadDTO> listaRentabilidad, DateTime fecha, int tipo)
        {
            var listaUtilidad = new List<AnalisisUtilidadDTO>();
            listaRentabilidad = listaRentabilidad.Where(x => x != null && x.tipo == tipo).ToList();
            //--Ingresos Contabilizados--//
            var elemento = listaRentabilidad.Where(x => /*(x.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.Abono 
                || x.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.AbonoRojo) &&*/ x.cta == 4000).ToList();
            var utilidad = new AnalisisUtilidadDTO
            {
                tipo_mov = 1,
                descripcion = "Ingresos Contabilizados"
            };
            utilidad = AsignarImportesAnalisis(utilidad, elemento, fecha);
            utilidad.actual = utilidad.actual * (-1);
            utilidad.semana2 = utilidad.semana2 * (-1);
            utilidad.semana3 = utilidad.semana3 * (-1);
            utilidad.semana4 = utilidad.semana4 * (-1);
            utilidad.semana5 = utilidad.semana5 * (-1);
            utilidad.cfc = utilidad.cfc * (-1);
            utilidad.cf = utilidad.cf * (-1);
            utilidad.mc = utilidad.mc * (-1);
            utilidad.pr = utilidad.pr * (-1);
            utilidad.tc = utilidad.tc * (-1);
            utilidad.car = utilidad.car * (-1);
            utilidad.ex = utilidad.ex * (-1);
            utilidad.hdt = utilidad.hdt * (-1);
            utilidad.otros = utilidad.otros * (-1);
            var auxUtilidad = utilidad;
            elemento.ForEach(x => x.importe = x.importe * -1);
            utilidad.detalles = elemento.ToList();
            utilidad.detalles.ForEach(x => x.importe = x.importe * (-1));
            listaUtilidad.Add(utilidad);
            //--Ingresos con Estimación--//
            elemento = listaRentabilidad.Where(x => /*(x.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.Abono 
                || x.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.AbonoRojo) &&*/ x.cta == 6000).ToList();

            utilidad = new AnalisisUtilidadDTO
            {
                tipo_mov = 2,
                descripcion = "Ingresos con Estimación"
            };
            utilidad = AsignarImportesAnalisis(utilidad, elemento, fecha);
            utilidad.detalles = elemento.ToList();
            utilidad.detalles.ForEach(x => x.importe = x.importe * (-1));
            listaUtilidad.Add(utilidad);
            //--Ingresos pendientes por generar--//
            elemento = listaRentabilidad.Where(x => /*(x.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.Abono 
                || x.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.AbonoRojo) &&*/ x.cta == 6000).ToList();
            utilidad = new AnalisisUtilidadDTO
            {
                tipo_mov = 3,
                descripcion = "Ingresos Pendientes por Generar"
            };
            utilidad = AsignarImportesAnalisis(utilidad, elemento, fecha);
            utilidad.detalles = elemento.ToList();
            utilidad.detalles.ForEach(x => x.importe = x.importe * (-1));
            listaUtilidad.Add(utilidad);
            //--Total Ingresos--//           
            utilidad = new AnalisisUtilidadDTO
            {
                tipo_mov = 4,
                descripcion = "Total Ingresos"
            };
            var subtotal = listaUtilidad.Where(x => x.tipo_mov <= 3).ToList();
            utilidad.actual = subtotal.Sum(x => x.actual);
            utilidad.semana2 = subtotal.Sum(x => x.semana2);
            utilidad.semana3 = subtotal.Sum(x => x.semana3);
            utilidad.semana4 = subtotal.Sum(x => x.semana4);
            utilidad.semana5 = subtotal.Sum(x => x.semana5);
            utilidad.cfc = subtotal.Sum(x => x.cfc);
            utilidad.cf = subtotal.Sum(x => x.cf);
            utilidad.mc = subtotal.Sum(x => x.mc);
            utilidad.pr = subtotal.Sum(x => x.pr);
            utilidad.tc = subtotal.Sum(x => x.tc);
            utilidad.car = subtotal.Sum(x => x.car);
            utilidad.ex = subtotal.Sum(x => x.ex);
            utilidad.hdt = subtotal.Sum(x => x.hdt);
            utilidad.otros = subtotal.Sum(x => x.otros);
            listaUtilidad.Add(utilidad);
            //--Costo Total--//
            elemento = listaRentabilidad.Where(x => /*(x.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.Cargo 
                || x.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.CargoRojo) &&*/ x.cta == 5000).ToList();
            utilidad = new AnalisisUtilidadDTO
            {
                tipo_mov = 5,
                descripcion = "Costo Total"
            };
            utilidad = AsignarImportesAnalisis(utilidad, elemento, fecha);
            utilidad.detalles = elemento.ToList();
            listaUtilidad.Add(utilidad);
            //--Utilidad Bruta--//
            utilidad = new AnalisisUtilidadDTO
            {
                tipo_mov = 6,
                descripcion = "Utilidad Bruta"
            };
            utilidad.actual = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).actual - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).actual;
            utilidad.semana2 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).semana2 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).semana2;
            utilidad.semana3 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).semana3 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).semana3;
            utilidad.semana4 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).semana4 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).semana4;
            utilidad.semana5 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).semana5 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).semana5;
            utilidad.cfc = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).cfc - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).cfc;
            utilidad.cf = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).cf - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).cf;
            utilidad.mc = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).mc - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).mc;
            utilidad.pr = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).pr - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).pr;
            utilidad.tc = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).tc - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).tc;
            utilidad.car = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).car - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).car;
            utilidad.ex = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).ex - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).ex;
            utilidad.hdt = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).hdt - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).hdt;
            utilidad.otros = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).otros - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).otros;
            listaUtilidad.Add(utilidad);
            //--Gastos de Operación--//
            elemento = listaRentabilidad.Where(x => /*(x.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.Cargo
                || x.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.CargoRojo) &&*/ x.cta == 5280).ToList();
            utilidad = new AnalisisUtilidadDTO
            {
                tipo_mov = 7,
                descripcion = "Gastos de Operación"
            };
            utilidad = AsignarImportesAnalisis(utilidad, elemento, fecha);
            utilidad.detalles = elemento.ToList();
            listaUtilidad.Add(utilidad);
            //--Resultado Antes Financieros--//
            utilidad = new AnalisisUtilidadDTO
            {
                tipo_mov = 8,
                descripcion = "Resultado Antes Financieros"
            };
            utilidad.actual = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).actual - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).actual;
            utilidad.semana2 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).semana2 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).semana2;
            utilidad.semana3 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).semana3 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).semana3;
            utilidad.semana4 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).semana4 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).semana4;
            utilidad.semana5 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).semana5 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).semana5;
            utilidad.cfc = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).cfc - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).cfc;
            utilidad.cf = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).cf - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).cf;
            utilidad.mc = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).mc - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).mc;
            utilidad.pr = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).pr - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).pr;
            utilidad.tc = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).tc - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).tc;
            utilidad.car = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).car - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).car;
            utilidad.ex = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).ex - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).ex;
            utilidad.hdt = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).hdt - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).hdt;
            utilidad.otros = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).otros - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).otros;
            listaUtilidad.Add(utilidad);
            //--Gastos de Financieros--//
            elemento = listaRentabilidad.Where(x => (x.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.Cargo
                || x.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.CargoRojo) && x.cta == 5900).ToList();
            utilidad = new AnalisisUtilidadDTO
            {
                tipo_mov = 9,
                descripcion = "Gastos Financieros"
            };
            utilidad = AsignarImportesAnalisis(utilidad, elemento, fecha);
            utilidad.detalles = elemento.ToList();
            listaUtilidad.Add(utilidad);
            //--Resultado con Financieros--//
            utilidad = new AnalisisUtilidadDTO
            {
                tipo_mov = 10,
                descripcion = "Resultado con Financieros"
            };
            utilidad.actual = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).actual - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).actual;
            utilidad.semana2 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).semana2 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).semana2;
            utilidad.semana3 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).semana3 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).semana3;
            utilidad.semana4 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).semana4 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).semana4;
            utilidad.semana5 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).semana5 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).semana5;
            utilidad.cfc = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).cfc - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).cfc;
            utilidad.cf = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).cf - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).cf;
            utilidad.mc = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).mc - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).mc;
            utilidad.pr = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).pr - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).pr;
            utilidad.tc = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).tc - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).tc;
            utilidad.car = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).car - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).car;
            utilidad.ex = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).ex - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).ex;
            utilidad.hdt = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).hdt - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).hdt;
            utilidad.otros = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).otros - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).otros;
            listaUtilidad.Add(utilidad);
            //--Otros Ingresos--//
            elemento = listaRentabilidad.Where(x => /*(x.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.Abono
                || x.tipo_mov == (int)Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoMovPolizaEnum.AbonoRojo) &&*/ x.cta == 4901).ToList();
            utilidad = new AnalisisUtilidadDTO
            {
                tipo_mov = 11,
                descripcion = "Otros Ingresos"
            };
            utilidad = AsignarImportesAnalisis(utilidad, elemento, fecha);
            utilidad.actual = utilidad.actual * (-1);
            utilidad.semana2 = utilidad.semana2 * (-1);
            utilidad.semana3 = utilidad.semana3 * (-1);
            utilidad.semana4 = utilidad.semana4 * (-1);
            utilidad.semana5 = utilidad.semana5 * (-1);
            utilidad.cfc = utilidad.cfc * (-1);
            utilidad.cf = utilidad.cf * (-1);
            utilidad.mc = utilidad.mc * (-1);
            utilidad.pr = utilidad.pr * (-1);
            utilidad.tc = utilidad.tc * (-1);
            utilidad.car = utilidad.car * (-1);
            utilidad.ex = utilidad.ex * (-1);
            utilidad.hdt = utilidad.hdt * (-1);
            utilidad.otros = utilidad.otros * (-1);
            elemento.ForEach(x => x.importe = x.importe * -1);
            utilidad.detalles = elemento.ToList();
            utilidad.detalles.ForEach(x => x.importe = x.importe * (-1));
            listaUtilidad.Add(utilidad);
            //--Resultado Neto--//
            elemento = listaRentabilidad;
            utilidad = new AnalisisUtilidadDTO
            {
                tipo_mov = 12,
                descripcion = "Resultado Neto"
            };
            utilidad.actual = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).actual + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).actual;
            utilidad.semana2 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).semana2 + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).semana2;
            utilidad.semana3 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).semana3 + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).semana3;
            utilidad.semana4 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).semana4 + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).semana4;
            utilidad.semana5 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).semana5 + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).semana5;
            utilidad.cfc = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).cfc - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).cfc;
            utilidad.cf = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).cf - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).cf;
            utilidad.mc = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).mc - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).mc;
            utilidad.pr = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).pr - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).pr;
            utilidad.tc = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).tc - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).tc;
            utilidad.car = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).car - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).car;
            utilidad.ex = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).ex - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).ex;
            utilidad.hdt = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).hdt - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).hdt;
            utilidad.otros = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).otros - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).otros;
            var auxNeto = utilidad;
            listaUtilidad.Add(utilidad);
            //--% de Margen--//
            utilidad = new AnalisisUtilidadDTO
            {
                tipo_mov = 13,
                descripcion = "% de Margen",
                actual = auxUtilidad.actual != 0 ? (auxNeto.actual / auxUtilidad.actual) * 100 : 0,
                semana2 = auxUtilidad.semana2 != 0 ? (auxNeto.semana2 / auxUtilidad.semana2) * 100 : 0,
                semana3 = auxUtilidad.semana3 != 0 ? (auxNeto.semana3 / auxUtilidad.semana3) * 100 : 0,
                semana4 = auxUtilidad.semana4 != 0 ? (auxNeto.semana4 / auxUtilidad.semana4) * 100 : 0,
                semana5 = auxUtilidad.semana5 != 0 ? (auxNeto.semana5 / auxUtilidad.semana5) * 100 : 0,
                cfc = auxUtilidad.cfc != 0 ? (auxNeto.cfc / auxUtilidad.cfc) * 100 : 0,
                cf = auxUtilidad.cf != 0 ? (auxNeto.cf / auxUtilidad.cf) * 100 : 0,
                mc = auxUtilidad.mc != 0 ? (auxNeto.mc / auxUtilidad.mc) * 100 : 0,
                pr = auxUtilidad.pr != 0 ? (auxNeto.pr / auxUtilidad.pr) * 100 : 0,
                tc = auxUtilidad.tc != 0 ? (auxNeto.tc / auxUtilidad.tc) * 100 : 0,
                car = auxUtilidad.car != 0 ? (auxNeto.car / auxUtilidad.car) * 100 : 0,
                ex = auxUtilidad.ex != 0 ? (auxNeto.ex / auxUtilidad.ex) * 100 : 0,
                hdt = auxUtilidad.hdt != 0 ? (auxNeto.hdt / auxUtilidad.hdt) * 100 : 0,
                otros = auxUtilidad.otros != 0 ? (auxNeto.otros / auxUtilidad.otros) * 100 : 0,
            };
            listaUtilidad.Add(utilidad);
            return listaUtilidad.OrderBy(x => x.tipo_mov).ToList();
        }

        private AnalisisUtilidadDTO AsignarImportesAnalisis(AnalisisUtilidadDTO costo, IEnumerable<RentabilidadDTO> importes, DateTime fecha)
        {
            var actual = importes.Where(x => x.fecha <= fecha).ToList();

            if (actual.Count() > 0)
            {
                costo.actual = actual.Sum(x => x.importe);
            }
            var semana2 = importes.Where(x => x.fecha <= fecha.AddDays(-7)).ToList();
            if (semana2.Count() > 0)
            {
                costo.semana2 = semana2.Sum(x => x.importe);
            }
            var semana3 = importes.Where(x => x.fecha <= fecha.AddDays(-14)).ToList();
            if (semana3.Count() > 0)
            {
                costo.semana3 = semana3.Sum(x => x.importe);
            }
            var semana4 = importes.Where(x => x.fecha <= fecha.AddDays(-21)).ToList();
            if (semana4.Count() > 0)
            {
                costo.semana4 = semana4.Sum(x => x.importe);
            }
            var semana5 = importes.Where(x => x.fecha <= fecha.AddDays(-28)).ToList();
            if (semana5.Count() > 0)
            {
                costo.semana5 = semana5.Sum(x => x.importe);
            }
            //80-20
            var cfc = importes.Where(x => x.noEco.Contains("CFC-") && x.fecha <= fecha).ToList();
            if (cfc.Count() > 0)
            {
                costo.cfc = cfc.Sum(x => x.importe);
            }
            var cf = importes.Where(x => x.noEco.Contains("CF-") && x.fecha <= fecha).ToList();
            if (cf.Count() > 0)
            {
                costo.cf = cf.Sum(x => x.importe);
            }
            var mc = importes.Where(x => x.noEco.Contains("MC-") && x.fecha <= fecha).ToList();
            if (mc.Count() > 0)
            {
                costo.mc = mc.Sum(x => x.importe);
            }
            var pr = importes.Where(x => x.noEco.Contains("PR-") && x.fecha <= fecha).ToList();
            if (pr.Count() > 0)
            {
                costo.pr = pr.Sum(x => x.importe);
            }
            var tc = importes.Where(x => x.noEco.Contains("TC-") && x.fecha <= fecha).ToList();
            if (tc.Count() > 0)
            {
                costo.tc = tc.Sum(x => x.importe);
            }
            var car = importes.Where(x => x.noEco.Contains("CAR-") && x.fecha <= fecha).ToList();
            if (car.Count() > 0)
            {
                costo.car = car.Sum(x => x.importe);
            }
            var ex = importes.Where(x => x.noEco.Contains("EX-") && x.fecha <= fecha).ToList();
            if (ex.Count() > 0)
            {
                costo.ex = ex.Sum(x => x.importe);
            }
            var hdt = importes.Where(x => x.noEco.Contains("HDT-") && x.fecha <= fecha).ToList();
            if (hdt.Count() > 0)
            {
                costo.hdt = hdt.Sum(x => x.importe);
            }
            var otros = importes.Where(x => !x.noEco.Contains("CFC-") && !x.noEco.Contains("CF-") && !x.noEco.Contains("MC-") && !x.noEco.Contains("PR-") && !x.noEco.Contains("TC-") && !x.noEco.Contains("CAR-") && !x.noEco.Contains("EX-") && !x.noEco.Contains("HDT-") && x.fecha <= fecha).ToList();
            if (otros.Count() > 0)
            {
                costo.otros = otros.Sum(x => x.importe);
            }

            return costo;
        }


        #region Kubrix
        public ActionResult getLstKubrix(BusqKubrixDTO busq, int empresa = 1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioID = getUsuario().id;
                List<RentabilidadDTO> listaTabla = new List<RentabilidadDTO>();
                if (empresa == 0)
                {
                    listaTabla = rentabilidadFS.getLstKubrixConstruplan(busq);
                }
                else
                {
                    listaTabla = rentabilidadFS.getLstKubrix(busq);
                }
                var porEstimacion = rentabilidadFS.getLstKubrixIngresosEstimacion(busq, false);
                var porGenerar = rentabilidadFS.getLstKubrixIngresosPendientesGenerar(busq, usuarioID, false);
                listaTabla.AddRange(porEstimacion);
                listaTabla.AddRange(porGenerar);
                listaTabla = AsignarTipoMaquinaria(listaTabla);
                var esSuccess = listaTabla.Count > 0;
                if (esSuccess)
                {
                    //var listaTabla = obtenerListaKubrix(lst, busq);
                    //var CXP = rentabilidadFS.getLstCXP(busq);
                    result.Add("lst", listaTabla);
                    var administrativoCentral = listaTabla.Where(x => x.tipo == 6 && x.noEco != null).Select(x => x.noEco).Distinct().ToList();
                    var administrativoProyectos = listaTabla.Where(x => x.tipo == 9 && x.noEco != null).Select(x => x.noEco).Distinct().ToList();
                    var otros = listaTabla.Where(x => x.tipo == 7 && x.noEco != null).Select(x => x.noEco).Distinct().ToList();
                    result.Add("administrativoCentral", administrativoCentral);
                    result.Add("administrativoProyectos", administrativoProyectos);
                    result.Add("otros", otros);
                    //result.Add("CXP", CXP);
                }

                result.Add(SUCCESS, esSuccess);

                if (esSuccess == false)
                {
                    result.Add(MESSAGE, "No se encontraron registros con los filtros seleccionados.");
                }
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult AccesoBotonCorte()
        {
            var result = new Dictionary<string, object>();
            int idUsuario = getUsuario().id;
            if (idUsuario == 13 || idUsuario == 6032) { result.Add("permiso", true); }
            else
            {
                var permiso = false;
                if (Enum.IsDefined(typeof(PermisoGuardadoCorteEnum), idUsuario))
                {
                    var NUM = (PermisoGuardadoCorteEnum)idUsuario;
                    permiso = true;
                }
                result.Add("permiso", permiso);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return json;
        }


        //private List<RentabilidadKubrikDTO> obtenerListaKubrix(List<RentabilidadDTO> listaRentabilidad, BusqKubrixDTO busq)
        //{
        //    var listaUtilidad = new List<RentabilidadKubrikDTO>();
        //    listaRentabilidad = AsignarTipoMaquinaria(listaRentabilidad);
        //    var usuarioID = getUsuario().id;
        //    foreach (var item in listaRentabilidad) { if (item.noEco == null) item.noEco = item.cc; }

        //    //--Ingresos Contabilizados--//
        //    var elemento = listaRentabilidad.Where(x => x.cta == 4000).ToList();
        //    var utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Ingresos Contabilizados", 1, true, busq.tipoIntervalo);
        //    listaUtilidad.Add(utilidad);
        //    //--Ingresos con Estimación--//
        //    elemento = rentabilidadFS.getLstKubrixIngresosEstimacion(busq, false);
        //    elemento.AddRange(listaRentabilidad.Where(x => x.cta == 1));
        //    elemento = AsignarTipoMaquinaria(elemento);
        //    utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Ingresos con Estimación", 2, false, busq.tipoIntervalo);
        //    listaUtilidad.Add(utilidad);
        //    //--Ingresos Pendientes por Generar--//
        //    elemento = rentabilidadFS.getLstKubrixIngresosPendientesGenerar(busq, usuarioID, false);
        //    elemento = AsignarTipoMaquinaria(elemento);
        //    utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Ingresos Pendientes por Generar", 3, false, busq.tipoIntervalo);
        //    listaUtilidad.Add(utilidad);
        //    //--Subtotal--//
        //    var subtotal = listaUtilidad.Where(x => x.tipo_mov <= 3).ToList();
        //    utilidad = new RentabilidadKubrikDTO
        //    {
        //        tipo_mov = 4,
        //        descripcion = "Total Ingresos",
        //        mayor = subtotal.Sum(x => x.mayor),
        //        menor = subtotal.Sum(x => x.menor),
        //        transporteConstruplan = subtotal.Sum(x => x.transporteConstruplan),
        //        transporteArrendadora = subtotal.Sum(x => x.transporteArrendadora),
        //        administrativoCentral = subtotal.Sum(x => x.administrativoCentral),
        //        administrativoProyectos = subtotal.Sum(x => x.administrativoProyectos),
        //        fletes = subtotal.Sum(x => x.fletes),
        //        neumaticos = subtotal.Sum(x => x.neumaticos),
        //        otros = subtotal.Sum(x => x.otros),
        //        total = subtotal.Sum(x => x.total),
        //        actual = subtotal.Sum(x => x.actual),
        //        semana2 = subtotal.Sum(x => x.semana2),
        //        semana3 = subtotal.Sum(x => x.semana3),
        //        semana4 = subtotal.Sum(x => x.semana4),
        //        semana5 = subtotal.Sum(x => x.semana5),
        //    };
        //    var auxUtilidad = utilidad;
        //    listaUtilidad.Add(utilidad);

        //    //--Costo Total--//
        //    elemento = listaRentabilidad.Where(x => x.cta == 5000 && x.tipoInsumo != "5000-10").ToList();
        //    utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Costo Total", 5, false, busq.tipoIntervalo);
        //    listaUtilidad.Add(utilidad);
        //    //--Depreciación--//
        //    elemento = listaRentabilidad.Where(x => x.cta == 5000 && x.tipoInsumo == "5000-10").ToList();
        //    utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Depreciación", 6, false, busq.tipoIntervalo);
        //    listaUtilidad.Add(utilidad);
        //    //--Utilidad Bruta--//           
        //    utilidad = new RentabilidadKubrikDTO
        //    {
        //        tipo_mov = 7,
        //        descripcion = "Utilidad Bruta",
        //        mayor = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).mayor - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).mayor - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).mayor,
        //        menor = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).menor - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).menor - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).menor,
        //        transporteConstruplan = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).transporteConstruplan - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).transporteConstruplan - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).transporteConstruplan,
        //        transporteArrendadora = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).transporteArrendadora - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).transporteArrendadora - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).transporteArrendadora,
        //        administrativoCentral = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).administrativoCentral - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).administrativoCentral - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).administrativoCentral,
        //        administrativoProyectos = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).administrativoProyectos - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).administrativoProyectos - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).administrativoProyectos,
        //        fletes = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).fletes - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).fletes - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).fletes,
        //        neumaticos = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).neumaticos - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).neumaticos - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).neumaticos,
        //        otros = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).otros - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).otros - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).otros,
        //        total = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).total - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).total - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).total,
        //        actual = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).actual - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).actual - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).actual,
        //        semana2 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).semana2 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).semana2 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).semana2,
        //        semana3 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).semana3 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).semana3 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).semana3,
        //        semana4 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).semana4 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).semana4 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).semana4,
        //        semana5 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).semana5 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).semana5 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).semana5
        //    };
        //    listaUtilidad.Add(utilidad);
        //    //--Gastos de Operación--//
        //    elemento = listaRentabilidad.Where(x => x.cta == 5280).ToList();
        //    utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Gastos de Operación", 8, false, busq.tipoIntervalo);
        //    listaUtilidad.Add(utilidad);
        //    //--Gastos Antes de Finacieros--//           
        //    utilidad = new RentabilidadKubrikDTO
        //    {
        //        tipo_mov = 9,
        //        descripcion = "Resultado Antes Finacieros",
        //        mayor = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).mayor - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).mayor,
        //        menor = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).menor - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).menor,
        //        transporteConstruplan = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).transporteConstruplan - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).transporteConstruplan,
        //        transporteArrendadora = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).transporteArrendadora - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).transporteArrendadora,
        //        administrativoCentral = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).administrativoCentral - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).administrativoCentral,
        //        administrativoProyectos = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).administrativoProyectos - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).administrativoProyectos,
        //        fletes = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).fletes - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).fletes,
        //        neumaticos = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).neumaticos - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).neumaticos,
        //        otros = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).otros - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).otros,
        //        total = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).total - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).total,
        //        actual = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).actual - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).actual,
        //        semana2 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).semana2 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).semana2,
        //        semana3 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).semana3 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).semana3,
        //        semana4 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).semana4 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).semana4,
        //        semana5 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).semana5 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).semana5
        //    };
        //    listaUtilidad.Add(utilidad);
        //    //--Gastos de Financieros--//
        //    elemento = listaRentabilidad.Where(x => x.cta == 5900 || x.cta == 4900).ToList();
        //    utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Gastos y Productos Financieros", 10, true, busq.tipoIntervalo);
        //    listaUtilidad.Add(utilidad);
        //    //--Resultado con Financieros--//           
        //    utilidad = new RentabilidadKubrikDTO
        //    {
        //        tipo_mov = 11,
        //        descripcion = "Resultado con Financieros",
        //        mayor = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).mayor + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).mayor,
        //        menor = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).menor + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).menor,
        //        transporteConstruplan = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).transporteConstruplan + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).transporteConstruplan,
        //        transporteArrendadora = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).transporteArrendadora + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).transporteArrendadora,
        //        administrativoCentral = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).administrativoCentral + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).administrativoCentral,
        //        administrativoProyectos = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).administrativoProyectos + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).administrativoProyectos,
        //        fletes = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).fletes + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).fletes,
        //        neumaticos = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).neumaticos + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).neumaticos,
        //        otros = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).otros + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).otros,
        //        total = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).total + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).total,
        //        actual = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).actual + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).actual,
        //        semana2 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).semana2 + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).semana2,
        //        semana3 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).semana3 + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).semana3,
        //        semana4 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).semana4 + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).semana4,
        //        semana5 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).semana5 + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).semana5
        //    };
        //    listaUtilidad.Add(utilidad);
        //    //--Otros Ingresos--//
        //    elemento = listaRentabilidad.Where(x => x.cta == 4901 || x.cta == 5901).ToList();
        //    utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Otros Ingresos", 12, true, busq.tipoIntervalo);
        //    listaUtilidad.Add(utilidad);
        //    //--Resultado Neto--//           
        //    utilidad = new RentabilidadKubrikDTO
        //    {
        //        tipo_mov = 13,
        //        descripcion = "Resultado Neto",
        //        mayor = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).mayor + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).mayor,
        //        menor = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).menor + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).menor,
        //        transporteConstruplan = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).transporteConstruplan + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).transporteConstruplan,
        //        transporteArrendadora = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).transporteArrendadora + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).transporteArrendadora,
        //        administrativoCentral = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).administrativoCentral + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).administrativoCentral,
        //        administrativoProyectos = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).administrativoProyectos + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).administrativoProyectos,
        //        fletes = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).fletes + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).fletes,
        //        neumaticos = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).neumaticos + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).neumaticos,
        //        otros = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).otros + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).otros,
        //        total = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).total + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).total,
        //        actual = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).actual + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).actual,
        //        semana2 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).semana2 + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).semana2,
        //        semana3 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).semana3 + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).semana3,
        //        semana4 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).semana4 + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).semana4,
        //        semana5 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).semana5 + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).semana5,
        //    };
        //    var auxNeto = utilidad;
        //    listaUtilidad.Add(utilidad);
        //    //--% de Margen--//
        //    utilidad = new RentabilidadKubrikDTO
        //    {
        //        tipo_mov = 14,
        //        descripcion = "% de Margen",
        //        mayor = auxUtilidad.mayor != 0 ? (auxNeto.mayor / auxUtilidad.mayor) * 100 : 0,
        //        menor = auxUtilidad.menor != 0 ? (auxNeto.menor / auxUtilidad.menor) * 100 : 0,
        //        transporteConstruplan = auxUtilidad.transporteConstruplan != 0 ? (auxNeto.transporteConstruplan / auxUtilidad.transporteConstruplan) * 100 : 0,
        //        transporteArrendadora = auxUtilidad.transporteArrendadora != 0 ? (auxNeto.transporteArrendadora / auxUtilidad.transporteArrendadora) * 100 : 0,
        //        administrativoCentral = auxUtilidad.administrativoCentral != 0 ? (auxNeto.administrativoCentral / auxUtilidad.administrativoCentral) * 100 : 0,
        //        administrativoProyectos = auxUtilidad.administrativoProyectos != 0 ? (auxNeto.administrativoProyectos / auxUtilidad.administrativoProyectos) * 100 : 0,
        //        fletes = auxUtilidad.fletes != 0 ? (auxNeto.fletes / auxUtilidad.fletes) * 100 : 0,
        //        neumaticos = auxUtilidad.neumaticos != 0 ? (auxNeto.neumaticos / auxUtilidad.neumaticos) * 100 : 0,
        //        otros = auxUtilidad.otros != 0 ? (auxNeto.otros / auxUtilidad.otros) * 100 : 0,
        //        total = auxUtilidad.total != 0 ? (auxNeto.total / auxUtilidad.total) * 100 : 0,
        //        actual = auxUtilidad.actual != 0 ? (auxNeto.actual / auxUtilidad.actual) * 100 : 0,
        //        semana2 = auxUtilidad.semana2 != 0 ? (auxNeto.semana2 / auxUtilidad.semana2) * 100 : 0,
        //        semana3 = auxUtilidad.semana3 != 0 ? (auxNeto.semana3 / auxUtilidad.semana3) * 100 : 0,
        //        semana4 = auxUtilidad.semana4 != 0 ? (auxNeto.semana4 / auxUtilidad.semana4) * 100 : 0,
        //        semana5 = auxUtilidad.semana5 != 0 ? (auxNeto.semana5 / auxUtilidad.semana5) * 100 : 0,
        //    };
        //    listaUtilidad.Add(utilidad);
        //    return listaUtilidad.OrderBy(x => x.tipo_mov).ToList();
        //}

        //private RentabilidadKubrikDTO AsignacionDetalles(RentabilidadKubrikDTO utilidad, List<RentabilidadDTO> elemento, int tipoMov, bool cambioSigno)
        //{
        //    utilidad.mayor = cambioSigno ? utilidad.mayor * (-1) : utilidad.mayor;
        //    utilidad.menor = cambioSigno ? utilidad.menor * (-1) : utilidad.menor;
        //    utilidad.transporteConstruplan = cambioSigno ? utilidad.transporteConstruplan * (-1) : utilidad.transporteConstruplan;
        //    utilidad.transporteArrendadora = cambioSigno ? utilidad.transporteArrendadora * (-1) : utilidad.transporteArrendadora;
        //    utilidad.administrativoCentral = cambioSigno ? utilidad.administrativoCentral * (-1) : utilidad.administrativoCentral;
        //    utilidad.administrativoProyectos = cambioSigno ? utilidad.administrativoProyectos * (-1) : utilidad.administrativoProyectos;
        //    utilidad.fletes = cambioSigno ? utilidad.fletes * (-1) : utilidad.fletes;
        //    utilidad.neumaticos = cambioSigno ? utilidad.neumaticos * (-1) : utilidad.neumaticos;
        //    utilidad.otros = cambioSigno ? utilidad.otros * (-1) : utilidad.otros;
        //    utilidad.total = cambioSigno ? utilidad.total * (-1) : utilidad.total;
        //    utilidad.actual = cambioSigno ? utilidad.actual * (-1) : utilidad.actual;
        //    utilidad.semana2 = cambioSigno ? utilidad.semana2 * (-1) : utilidad.semana2;
        //    utilidad.semana3 = cambioSigno ? utilidad.semana3 * (-1) : utilidad.semana3;
        //    utilidad.semana4 = cambioSigno ? utilidad.semana4 * (-1) : utilidad.semana4;
        //    utilidad.semana5 = cambioSigno ? utilidad.semana5 * (-1) : utilidad.semana5;
        //    elemento.ForEach(x => x.tipo_mov = tipoMov);
        //    utilidad.detalles = elemento.ToList();
        //    //utilidad.detalles.ForEach(x => x.importe = cambioSigno ? x.importe * (-1) : x.importe);
        //    foreach (var item in utilidad.detalles)
        //    {
        //        if (cambioSigno)
        //        {
        //            item.importe = item.importe * (-1);
        //        }
        //    }

        //    return utilidad;
        //}

        //private RentabilidadKubrikDTO AsignarImportesKubrix(IEnumerable<RentabilidadDTO> importes, DateTime fecha, string descripcion, int tipoMov, bool cambioSigno, int tipoIntervalo)
        //{
        //    RentabilidadKubrikDTO costo = new RentabilidadKubrikDTO();
        //    costo.descripcion = descripcion;
        //    costo.tipo_mov = tipoMov;

        //    var mayor = importes.Where(x => x.tipo == 1).ToList();
        //    if (mayor.Count() > 0) { costo.mayor = cambioSigno ? mayor.Sum(x => x.importe) * (-1) : mayor.Sum(x => x.importe); }

        //    var menor = importes.Where(x => x.tipo == 2).ToList();
        //    if (menor.Count() > 0) { costo.menor = cambioSigno ? menor.Sum(x => x.importe) * (-1) : menor.Sum(x => x.importe); }

        //    var transporteConstruplan = importes.Where(x => x.tipo == 3).ToList();
        //    if (transporteConstruplan.Count() > 0) { costo.transporteConstruplan = cambioSigno ? transporteConstruplan.Sum(x => x.importe) * (-1) : transporteConstruplan.Sum(x => x.importe); }

        //    var transporteArrendadora = importes.Where(x => x.tipo == 8).ToList();
        //    if (transporteArrendadora.Count() > 0) { costo.transporteArrendadora = cambioSigno ? transporteArrendadora.Sum(x => x.importe) * (-1) : transporteArrendadora.Sum(x => x.importe); }

        //    var administrativoCentral = importes.Where(x => x.tipo == 6).ToList();
        //    if (administrativoCentral.Count() > 0) { costo.administrativoCentral = cambioSigno ? administrativoCentral.Sum(x => x.importe) * (-1) : administrativoCentral.Sum(x => x.importe); }

        //    var administrativoProyectos = importes.Where(x => x.tipo == 9).ToList();
        //    if (administrativoProyectos.Count() > 0) { costo.administrativoProyectos = cambioSigno ? administrativoProyectos.Sum(x => x.importe) * (-1) : administrativoProyectos.Sum(x => x.importe); }

        //    var fletes = importes.Where(x => x.tipo == 4).ToList();
        //    if (fletes.Count() > 0) { costo.fletes = cambioSigno ? fletes.Sum(x => x.importe) * (-1) : fletes.Sum(x => x.importe); }

        //    var neumaticos = importes.Where(x => x.tipo == 5).ToList();
        //    if (neumaticos.Count() > 0) { costo.neumaticos = cambioSigno ? neumaticos.Sum(x => x.importe) * (-1) : neumaticos.Sum(x => x.importe); }

        //    var otros = importes.Where(x => x.tipo == 7).ToList();
        //    if (otros.Count() > 0) { costo.otros = cambioSigno ? otros.Sum(x => x.importe) * (-1) : otros.Sum(x => x.importe); }

        //    costo.total = cambioSigno ? importes.Sum(x => x.importe) * (-1) : importes.Sum(x => x.importe);

        //    //Intervalos
        //    var actual = importes.Where(x => x.fecha <= fecha).ToList();
        //    if (actual.Count() > 0) { costo.actual = cambioSigno ? actual.Sum(x => x.importe) * (-1) : actual.Sum(x => x.importe); }

        //    if (tipoIntervalo == 0)
        //    {
        //        var semana2 = importes.Where(x => x.fecha <= fecha.AddDays(-7)).ToList();
        //        if (semana2.Count() > 0) { costo.semana2 = cambioSigno ? semana2.Sum(x => x.importe) * (-1) : semana2.Sum(x => x.importe); }

        //        var semana3 = importes.Where(x => x.fecha <= fecha.AddDays(-14)).ToList();
        //        if (semana3.Count() > 0) { costo.semana3 = cambioSigno ? semana3.Sum(x => x.importe) * (-1) : semana3.Sum(x => x.importe); }

        //        var semana4 = importes.Where(x => x.fecha <= fecha.AddDays(-21)).ToList();
        //        if (semana4.Count() > 0) { costo.semana4 = cambioSigno ? semana4.Sum(x => x.importe) * (-1) : semana4.Sum(x => x.importe); }

        //        var semana5 = importes.Where(x => x.fecha <= fecha.AddDays(-28)).ToList();
        //        if (semana5.Count() > 0) { costo.semana5 = cambioSigno ? semana5.Sum(x => x.importe) * (-1) : semana5.Sum(x => x.importe); }
        //    }
        //    else 
        //    {
        //        var fechaInicioMes = new DateTime(fecha.Year, fecha.Month, 1);
        //        var semana2 = importes.Where(x => x.fecha < fechaInicioMes).ToList();
        //        if (semana2.Count() > 0) { costo.semana2 = cambioSigno ? semana2.Sum(x => x.importe) * (-1) : semana2.Sum(x => x.importe); }

        //        var semana3 = importes.Where(x => x.fecha < fechaInicioMes.AddMonths(-1)).ToList();
        //        if (semana3.Count() > 0) { costo.semana3 = cambioSigno ? semana3.Sum(x => x.importe) * (-1) : semana3.Sum(x => x.importe); }

        //        var semana4 = importes.Where(x => x.fecha < fechaInicioMes.AddMonths(-2)).ToList();
        //        if (semana4.Count() > 0) { costo.semana4 = cambioSigno ? semana4.Sum(x => x.importe) * (-1) : semana4.Sum(x => x.importe); }

        //        var semana5 = importes.Where(x => x.fecha < fechaInicioMes.AddMonths(-3)).ToList();
        //        if (semana5.Count() > 0) { costo.semana5 = cambioSigno ? semana5.Sum(x => x.importe) * (-1) : semana5.Sum(x => x.importe); }

        //    }
        //    costo.detalles = importes.ToList();
        //    foreach (var item in costo.detalles)
        //    {
        //        if (cambioSigno)
        //        {
        //            item.importe = item.importe * (-1);
        //        }
        //        item.tipo_mov = tipoMov;
        //    }

        //    //if (cambioSigno) {

        //    //} costo.detalles.ForEach(x => x.importe = x.importe * (-1));
        //    //costo.detalles.ForEach(x => x.tipo_mov = tipoMov);

        //    return costo;
        //}

        private List<RentabilidadDTO> AsignarTipoMaquinaria(List<RentabilidadDTO> listaRentabilidad)
        {
            var auxFletes = rentabilidadFS.getFletesActivos();

            var relacionEcoTipo = rentabilidadFS.getRelacionEcoTipo(listaRentabilidad.Select(x => x.noEco).ToList());
            var mayor = relacionEcoTipo.Where(x => x.Item1 == 1).Select(x => x.Item2).ToList();
            var menor = relacionEcoTipo.Where(x => x.Item1 == 2).Select(x => x.Item2).ToList();
            var transporteConstruplan = relacionEcoTipo.Where(x => x.Item1 == 3).Select(x => x.Item2).ToList();
            var transporteArrendadora = relacionEcoTipo.Where(x => x.Item1 == 8).Select(x => x.Item2).ToList();
            List<string> auxAdminCentral = (new string[] { "ADMINISTRACIÓN CENTRAL ARRENDADORA  ", "ADMINISTRACION CENTRAL ARRENDADORA", "ALMACEN Y TALLER EN PLANTA DE ASFALTO", "TALLER  HERMOSILLO NOMINA", "ALM Y TALLER EN PLANTA ASFALTO NOMINA", "GASTO TMC Y PATIO DE MAQ", "COMPRA HERRAMIENTA Y EQUIPO MENOR TMC" }).ToList();

            List<string> auxAdminProyectos = (new string[] { "ADQUISICION DE COMBUSTIBLES","MINADO LA COLORADA NOMINA","MINADO NOCHEBUENA II NOMINA","PROYECTO LA YAQUI NOMINA","PRESA DE JALES HERRADURA III NOMINA",
                "PATIOS NOCHE BUENA VII NOMINA","HERRADURA XIII NOMINA","PRESA SAN JULIAN ETAPA II NOMINA","MINADO SAN AGUSTIN NOMINA","PAVIMENTACION QUINTERO ARCE NOMINA","PLANTA DE ASFALTO NOMINA","CAPUFE HERMOSILLO-SANTA ANA NOMINA",
                "TALLER OVERHAUL EQ CONSTRUCCION NOMINA","CAPUFE - SANTANA NOMINA","PERSONAL EN DESARROLLO NÓMINA","TALLER CAPUFE TECATE NOMINA","TALLER DE REPARACION DE LLANTAS OTR NOMI","REHABILITACION COLECTOR 60\" NOMINA",
                "REHABILITACION COLECTOR DE 36” SAHUARO N","CAMINO CERRO PELON NOMINA","TERRACERIAS DESALADORA EMPALME NOMINA","MINADO CERRO PELON NOMINA","CONSERVACION CARRET CABOR-SONOYTA NOMINA","CONSTRUCCION DEL ACUEDUCTO DE 22\" NOMINA",
                "DEPOSITO DE JALES V NOMINA","NOMINA MINADO NOCHEBUENA III","NOMINA  EDIFICIO DE PROCESOS","NOMINA PAVIMENTACION ESTAC CENTRO","NOMINA REHABILITACION DE COLECTOR 60” ET","NOMINA PATIO 4B LA COLORADA",
                "NOMINA ESTACION DE COMPRESION PITIQUITO","NOMINA RECARPETEO BLVD SOLIDARIDAD","DEPRESIACIONES VARIAS","CONTROL DE MAQUINARIA","COMPRA COMPONENTES HERRADURA","COMPRA COMPONENTES LA COLORADA","COMPONENTES EQ CONSTRUCCION",
                "COMPONENTES SAN AGUSTIN","COMPRA DE HERAMIENTA Y EQUIPO MENOR","GASTOS TALLER MINADO LA COLORADA","GASTOS TALLER MINADO NOCHEBUENA II","GASTOS TALLER PROYECTO LA YAQUI","GASTOS TALL PRESA DE JALES HERRADURA III",
                "GASTOS TALLER PRESA SAN JULIAN ETAPA II","GASTOS TALLER PATIOS NOCHEBUENA VII","GASTOS TALLER HERRADURA XIII","GASTOS TALLER SAN AGUSTIN","CARRILERIA","COMPRA Y REPARACION DE CUCHARONES Y CAJA","GASTOS TALLER QUINTERO ARCE",
                "GASTOS TALLER VARIAS CALLES","GASTOS TALLER CAPUFE HERMOSILLO-SANTANA","GASTOS TALLER PLANTA DE ASFALTO","GASTOS TALLER PRESA DE AGUA SAN JULIAN","GASTOS TALLER CAPUFE-SANTA","GASTOS TALLER CAPUFE TECATE",
                "MINA SUBTERRANEA SAN JULIAN","GASTOS TALLER REHABILITACION DE COLECTOR","GASTOS TALLER MINADO LA HERRADURA","GASTOS TALLER CAMINO CERRO PELON","GASTOS TALLER TERRACERIAS DESALADORA EMP","GASTOS TALLER RENTA DE MAQUINARIA MULATO",
                "GASTOS TALLER MINADO CERRO PELON","GASTOS TALLER CONSERVA CARRET CAB-SONOYT","COMPRA DE COMPONENTES CERRO PELÓN","GASTOS TALLER CONSTRUCCION DEL ACUEDUCTO","GASTOS TALLER DEPOSITO DE JALES V","CRC Construplan",
                "GASTOS TALLER MMINADO NOCHEBUENA III","GASTOS TALLER EDIFICIO DE PROCESOS","GASTOS TALLER PAVIMENTACION ESTAC CENTRO","Gastos CRC Construplan","GASTOS TALLER GM SILAO MISCELANEOS ENSAM","GASTOS TALLER REHABILITACION DE COLECTOR",
                "Gastos Taller Overhaul Construccion","Compra Herramienta Overhaul Construcción","GASTOS TALLER PATIO 4B LA COLORADA","GASTOS TALLER ESTACION DE COMPRESION PIT","GASTOS TALLER ESTACION DE COMPRESION PITIQUITO","GASTOS TALLER AMPLIACION OFICINAS CENTRALES",
                "GASTOS TALLER RECARPETEO BLVD SOLIDARIDAD","GASTOS TALLER RECARPETEO BLVD SOLIDARIDA","GASTOS TALLER DEPOSITO DE JALES SECOS VI","GASTOS TALLER COMERCIALIZACION AUTOMOTRIZ","MAQUINARIA Y EQUIPO EN RENTA PURA" }).ToList();

            foreach (var item in listaRentabilidad)
            {
                if (mayor.Contains(item.noEco) && !auxFletes.Contains(item.noEco)) item.tipo = 1;
                else
                {
                    if (menor.Contains(item.noEco) && !auxFletes.Contains(item.noEco)) item.tipo = 2;
                    else
                    {
                        if (transporteConstruplan.Contains(item.noEco) && !auxFletes.Contains(item.noEco)) item.tipo = 3;
                        else
                        {
                            if (transporteArrendadora.Contains(item.noEco) && !auxFletes.Contains(item.noEco)) item.tipo = 8;
                            else
                            {
                                if (item.noEco == "FLETES DE MAQUINARIA Y EQUIPO" || auxFletes.Contains(item.noEco)) item.tipo = 4;
                                else
                                {
                                    if (item.noEco == "TALLER DE REPARACION DE LLANTAS OTR") item.tipo = 5;
                                    else
                                    {
                                        if (auxAdminCentral.Contains(item.noEco)) item.tipo = 6;
                                        else
                                        {
                                            if (auxAdminProyectos.Contains(item.noEco)) item.tipo = 9;
                                            else item.tipo = 7;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return listaRentabilidad;
        }

        public ActionResult getLstKubrixDetalle(BusqKubrixDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = rentabilidadFS.getLstKubrixDetalle(busq);
                var esSuccess = lst.Count > 0;
                if (esSuccess)
                {
                    if (busq.cta != 0 && busq.cta > 1 && busq.cta < 5000) { lst.ForEach(x => x.importe = x.importe * (-1)); }
                    result.Add("lst", lst);
                }
                result.Add(SUCCESS, esSuccess);
                if (esSuccess == false)
                {
                    result.Add(MESSAGE, "No se encontraron registros con los filtros seleccionados.");
                }
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult fillComboMaquinaria(List<int> modeloID, int grupoID = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = rentabilidadFS.fillComboMaquinaria(grupoID, modeloID).Select(x => new Core.DTO.Principal.Generales.ComboDTO { Value = x.id.ToString(), Text = x.noEconomico });
                result.Add(ITEMS, lst);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult fillComboGrupo()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = rentabilidadFS.FillGrupoEquipo().Select(x => new Core.DTO.Principal.Generales.ComboDTO { Value = x.id.ToString(), Text = x.descripcion });
                result.Add(ITEMS, lst);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult fillComboModelo(int grupoID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = rentabilidadFS.FillModeloEquipo(grupoID).Select(x => new Core.DTO.Principal.Generales.ComboDTO { Value = x.id.ToString(), Text = x.descripcion });
                result.Add(ITEMS, lst);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult fillComboDivision()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = rentabilidadFS.getDivisiones().Select(x => new Core.DTO.Principal.Generales.ComboDTO { Value = x.id.ToString(), Text = x.division });
                result.Add(ITEMS, lst);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return json;
        }

        public ActionResult fillComboResponsable()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioID = getUsuario().id;
                var lst = rentabilidadFS.getResponsabilesAC(usuarioID)
                    .Select(x => new Core.DTO.Principal.Generales.ComboDTO { Value = x.usuarioResponsableID.ToString(), Text = x.usuarioResponsable.nombre + " " + x.usuarioResponsable.apellidoPaterno + " " + x.usuarioResponsable.apellidoMaterno });
                result.Add(ITEMS, lst);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return json;
        }

        public ActionResult checkResponsable(int responsableID = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioID = getUsuario().id;
                var responsable = rentabilidadFS.checkResponsable(usuarioID, responsableID);
                result.Add(SUCCESS, true);
                result.Add("responsable", responsable);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return json;
        }

        public ActionResult getACDivision(int divisionID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var detallesDiv = rentabilidadFS.getACDivision(divisionID);
                result.Add(SUCCESS, true);
                result.Add("detallesDiv", detallesDiv);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return json;
        }

        public ActionResult getACResponsable(int responsbaleID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var detallesResp = rentabilidadFS.getACResponsable(responsbaleID);
                result.Add("detallesResp", detallesResp);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return json;
        }

        public ActionResult getLstCC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioID = getUsuario().id;
                var lstCC = rentabilidadFS.getListaCCByUsuario(usuarioID, 0).Select(x => new
                {
                    areaCuenta = x.Value,
                    descripcion = x.Text,
                    guardado = x.Prefijo == "1"
                });
                result.Add("lstCC", lstCC);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return json;
        }

        public ActionResult guardarLstCC(List<string> listaCC)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = rentabilidadFS.guardarLstCC(listaCC);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return json;
        }


        #endregion

        #region Corte

        public ActionResult GuardarCorte(int tipo, int empresa)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuario = getUsuario().id;
                int corteID = 0;
                switch (empresa)
                {
                    case 1:
                        System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                        var correo = new Infrastructure.DTO.CorreoDTO();

                        corteID = rentabilidadFS.guardarCorteConstruplan(usuario, tipo);
                        #region Correos Jesus Villegas
                        correo = new Infrastructure.DTO.CorreoDTO();

                        #region Archivos Cplan
                        if (corteID > 0)
                        {
                            var bytesSaldosCplan = rentabilidadFS.ExcelSaldosCplan(DateTime.Today.Year, EnkontrolEnum.CplanProd);
                            Stream streamSaldosCplan = new MemoryStream(bytesSaldosCplan[0]);
                            System.Net.Mail.Attachment saldosCplan = new System.Net.Mail.Attachment(streamSaldosCplan, ct);
                            saldosCplan.ContentDisposition.FileName = "saldos.xlsx";
                            correo.archivos.Add(saldosCplan);

                            //var bytesSaldosCplanAnterior = rentabilidadFS.ExcelSaldosCplan((DateTime.Today.Year - 1), EnkontrolEnum.CplanProd);
                            Stream streamSaldosCplanAnterior = new MemoryStream(bytesSaldosCplan[1]);
                            System.Net.Mail.Attachment saldosCplanAnterior = new System.Net.Mail.Attachment(streamSaldosCplanAnterior, ct);
                            saldosCplanAnterior.ContentDisposition.FileName = "saldos" + (DateTime.Today.Year - 1).ToString() + ".xlsx";
                            correo.archivos.Add(saldosCplanAnterior);

                            var bytesClientes = rentabilidadFS.ExcelClientesCplan(EnkontrolEnum.CplanProd);
                            Stream streamClientes = new MemoryStream(bytesClientes);
                            System.Net.Mail.Attachment clientes = new System.Net.Mail.Attachment(streamClientes, ct);
                            clientes.ContentDisposition.FileName = "clientes.xlsx";
                            correo.archivos.Add(clientes);

                            var bytesVencimientos = rentabilidadFS.ExcelVencimientosCplan(EnkontrolEnum.CplanProd);
                            Stream streamVencimiento = new MemoryStream(bytesVencimientos);
                            System.Net.Mail.Attachment vencimientos = new System.Net.Mail.Attachment(streamVencimiento, ct);
                            vencimientos.ContentDisposition.FileName = "vencimientos.xlsx";
                            correo.archivos.Add(vencimientos);
                        #endregion

                            #region Archivos Colombia
                            //var bytesSaldosColombia = rentabilidadFS.ExcelSaldosCplan(DateTime.Today.Year, EnkontrolEnum.ColombiaProductivo);
                            //Stream streamSaldosColombia = new MemoryStream(bytesSaldosColombia[0]);
                            //System.Net.Mail.Attachment saldosColombia = new System.Net.Mail.Attachment(streamSaldosColombia, ct);
                            //saldosColombia.ContentDisposition.FileName = "saldoscolombia.xlsx";
                            //correo.archivos.Add(saldosColombia);

                            //// var bytesSaldosColombiaAnterior = rentabilidadFS.ExcelSaldosCplan((DateTime.Today.Year - 1), EnkontrolEnum.ColombiaProductivo);
                            //Stream streamSaldosColombiaAnterior = new MemoryStream(bytesSaldosColombia[1]);
                            //System.Net.Mail.Attachment saldosColombiaAnterior = new System.Net.Mail.Attachment(streamSaldosColombiaAnterior, ct);
                            //saldosColombiaAnterior.ContentDisposition.FileName = "saldoscolombia" + (DateTime.Today.Year - 1).ToString() + ".xlsx";
                            //correo.archivos.Add(saldosColombiaAnterior);
                            #endregion

                            #region Archivos EICI
                            var bytesSaldosCplanEici = rentabilidadFS.ExcelSaldosCplan(DateTime.Today.Year, EnkontrolEnum.CplanEici);
                            Stream streamSaldosCplanEici = new MemoryStream(bytesSaldosCplanEici[0]);
                            System.Net.Mail.Attachment saldosCplanEici = new System.Net.Mail.Attachment(streamSaldosCplanEici, ct);
                            saldosCplanEici.ContentDisposition.FileName = "saldosconstruplaneici.xlsx";
                            correo.archivos.Add(saldosCplanEici);

                            //var bytesSaldosCplanEiciAnterior = rentabilidadFS.ExcelSaldosCplan((DateTime.Today.Year - 1), EnkontrolEnum.CplanEici);
                            Stream streamSaldosCplanEiciAnterior = new MemoryStream(bytesSaldosCplanEici[1]);
                            System.Net.Mail.Attachment saldosCplanEiciAnterior = new System.Net.Mail.Attachment(streamSaldosCplanEiciAnterior, ct);
                            saldosCplanEiciAnterior.ContentDisposition.FileName = "saldosconstruplaneici" + (DateTime.Today.Year - 1).ToString() + ".xlsx";
                            correo.archivos.Add(saldosCplanEiciAnterior);

                            var bytesClientesEici = rentabilidadFS.ExcelClientesCplan(EnkontrolEnum.CplanEici);
                            Stream streamClientesEici = new MemoryStream(bytesClientesEici);
                            System.Net.Mail.Attachment clientesEici = new System.Net.Mail.Attachment(streamClientesEici, ct);
                            clientesEici.ContentDisposition.FileName = "clientesconstruplaneici.xlsx";
                            correo.archivos.Add(clientesEici);
                            #endregion

                            #region Archivos Cplan Virtual
                            var bytesSaldosCplanVirtual = rentabilidadFS.ExcelSaldosCplanVirtual(DateTime.Today.Year);
                            Stream streamSaldosCplanVirtual = new MemoryStream(bytesSaldosCplanVirtual);
                            System.Net.Mail.Attachment saldosCplanVirtual = new System.Net.Mail.Attachment(streamSaldosCplanVirtual, ct);
                            saldosCplanVirtual.ContentDisposition.FileName = "saldosconstruplanvirtual.xlsx";
                            correo.archivos.Add(saldosCplanVirtual);
                            #endregion

                            #region Archivos Cplan Integradora
                            var bytesSaldosCplanIntegradora = rentabilidadFS.ExcelSaldosCplan(DateTime.Today.Year, EnkontrolEnum.CplanIntegradora);
                            Stream streamSaldosCplanIntegradora = new MemoryStream(bytesSaldosCplanIntegradora[0]);
                            System.Net.Mail.Attachment saldosCplanIntegradora = new System.Net.Mail.Attachment(streamSaldosCplanIntegradora, ct);
                            saldosCplanIntegradora.ContentDisposition.FileName = "saldosconstruplanintegradora.xlsx";
                            correo.archivos.Add(saldosCplanIntegradora);

                            //var bytesSaldosCplanIntegradora = rentabilidadFS.ExcelSaldosCplan(DateTime.Today.Year, EnkontrolEnum.CplanIntegradora);
                            Stream streamSaldosCplanIntegradoraAnterior = new MemoryStream(bytesSaldosCplanIntegradora[1]);
                            System.Net.Mail.Attachment saldosCplanIntegradoraAnterior = new System.Net.Mail.Attachment(streamSaldosCplanIntegradoraAnterior, ct);
                            saldosCplanIntegradoraAnterior.ContentDisposition.FileName = "saldosconstruplanintegradoraanterior.xlsx";
                            correo.archivos.Add(saldosCplanIntegradoraAnterior);

                            var bytesClientesIntegradora = rentabilidadFS.ExcelClientesCplan(EnkontrolEnum.CplanIntegradora);
                            Stream streamClientesIntegradora = new MemoryStream(bytesClientesIntegradora);
                            System.Net.Mail.Attachment clientesIntegradora = new System.Net.Mail.Attachment(streamClientesIntegradora, ct);
                            clientesIntegradora.ContentDisposition.FileName = "clientesconstruplanintegradora.xlsx";
                            correo.archivos.Add(clientesIntegradora);

                            var bytesVencimientosIntegradora = rentabilidadFS.ExcelVencimientosCplan(EnkontrolEnum.CplanIntegradora);
                            Stream streamVencimientoIntegradora = new MemoryStream(bytesVencimientosIntegradora);
                            System.Net.Mail.Attachment vencimientosIntegradora = new System.Net.Mail.Attachment(streamVencimientoIntegradora, ct);
                            vencimientosIntegradora.ContentDisposition.FileName = "vencimientosconstruplanintegradora.xlsx";
                            correo.archivos.Add(vencimientosIntegradora);
                            #endregion

                            correo.asunto = "Archivos Autogenerados Construplan";
                            correo.correos.Add("jvo_consultores@yahoo.com");
                            correo.correos.Add("f.artalejo@construplan.com.mx");
                            correo.correos.Add("francisco.salazar@construplan.com.mx");
                            correo.correos.Add("laura.olivas@construplan.com.mx");
                            correo.correos.Add("rene.olea@construplan.com.mx");

                            correo.cuerpo = "Se envian documentos Excel autogenerados";
                            correo.Enviar();
                        #endregion
                        }
                        break;
                    case 2:
                        corteID = rentabilidadFS.guardarCorteArrendadora(usuario, tipo);
                        break;
                }
                if (corteID > 0)
                {
                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, "Ocurrió un error al guardar el corte.");
                }
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult EnviarCorreoSemanalCplan(int tipo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                DateTime fecha = DateTime.Today.AddDays(1);
                if (tipo == 0)
                {
                    while (fecha.DayOfWeek != DayOfWeek.Wednesday)
                        fecha = fecha.AddDays(-1);
                }
                else
                {
                    if (tipo == 10)
                    {
                        while (fecha.DayOfWeek != DayOfWeek.Tuesday)
                            fecha = fecha.AddDays(-1);
                    }
                    else
                    {
                        fecha = new DateTime(fecha.Year, fecha.Month, 1);
                        fecha = fecha.AddDays(-1);
                    }
                }
                var corte = rentabilidadFS.getCortesPorFecha(fecha, tipo).FirstOrDefault();
                List<string> areasCuenta = new List<string> { "10-1","10-2","10-3","10-6","10-8","10-10","11-1","11-2","2-1","3-1","10-12","1-9","5-4","1-10","11-9","10-25","10-24",
                /*"10-27",*/"7-14","5-5","7-15","1-12","10-34","10-35","5-6","10-36","5-7","17-1","10-39","9-32","10-37","10-38","18-1","11-15",
                "19-1","19-2","19-3", "10-40", "20-1", "10-41", "11-18", /*"11-19",*/ "3-2", "10-42", "11-22", "11-23", "7-16", "11-24", "20-3", "C13", "1-13", "7-17", "20-4", "18-2",
                "10-45", "10-46", "11-25", "10-47", "11-27", "15-6", "8-9", "10-45", "2-3", "11-26", "10-44", "50-1", "51-1", "11-28", "52-1" }; //10-30,10-28,1-11,6-17,6-19,5-8

                if (corte != null)
                {
                    System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    var correo = new Infrastructure.DTO.CorreoDTO();

                    List<DetallesSemanalDTO> detallesSemana = rentabilidadFS.GetMovimientosNuevos(corte.id, areasCuenta);

                    correo = new Infrastructure.DTO.CorreoDTO();
                    foreach (var item in areasCuenta)
                    {
                        string ccDesc = rentabilidadFS.GetCCByAC(item);
                        byte[] bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                        Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                        System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                        ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                        correo.archivos.Add(ReporteEntradas);
                    }
                    correo.asunto = "Reportes Costo Semanal por CC";
                    correo.correos.Add("rene.olea@construplan.com.mx");
                    correo.correos.Add("francisco.salazar@construplan.com.mx");
                    correo.correos.Add("f.artalejo@construplan.com.mx");
                    correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                    correo.Enviar();
                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, "No existe corte para la fecha indicada.");
                }
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult EnviarCorreoSemanal(int tipo, int empresa)
        {
            var result = new Dictionary<string, object>();
            DateTime fecha = DateTime.Today.AddDays(1);
            if (tipo == 0)
            {
                while (fecha.DayOfWeek != DayOfWeek.Wednesday)
                    fecha = fecha.AddDays(-1);
            }
            else
            {
                if (tipo == 10)
                {
                    while (fecha.DayOfWeek != DayOfWeek.Tuesday)
                        fecha = fecha.AddDays(-1);
                }
                else
                {
                    fecha = new DateTime(fecha.Year, fecha.Month, 1);
                    fecha = fecha.AddDays(-1);
                }
            }
            var corte = rentabilidadFS.getCortesPorFecha(fecha, tipo).FirstOrDefault();
            if (empresa == 1)
            {
                try
                {
                    List<string> areasCuenta = new List<string> { "10-1","10-2","10-3","10-6",/*"10-8",*/"10-10","11-1","11-2","2-1","3-1","10-12","1-9","5-4","1-10","11-9","10-25","10-24",
                "7-14","5-5","7-15","1-12","10-34","10-35","5-6","10-36",/*"5-7",*//*"17-1",*//*"10-39",*/"9-32","10-37","10-38","18-1","11-15",
                "19-1","19-2","19-3", /*"10-40",*/ "20-1", /*"10-41",*/ "11-18", /*"11-19",*/ "3-2", "10-42", "11-22", "11-23", "7-16", "11-24", "20-3", "C13", "1-13", "7-17", /*"20-4", */"18-2",
                "10-45", "10-46", "11-25", "10-47", "11-27", "8-11", "8-9", "8-10", "10-45", "2-3", "11-26", "10-44", "50-1", "51-1", "11-28", "10-48", "11-29", "10-49", "11-30", "11-32", "50-2", 
                "E01", "E02", "11-34", "52-1", "11-36", "11-35", "11-37", "11-33", "1-14", "50-3", "10-52", "10-51", "10-50", "50-4", "10-53", "11-38", "11-39", "11-40" };
                    List<string> auxAreasCuenta = new List<string>();
                    if (corte != null)
                    {
                        System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                        var correo = new Infrastructure.DTO.CorreoDTO();

                        List<DetallesSemanalDTO> detallesSemana = rentabilidadFS.GetMovimientosNuevos(corte.id, areasCuenta);

                        #region Correo Prueba
                        //auxAreasCuenta = new List<string> { "10-10" };
                        //correo = new Infrastructure.DTO.CorreoDTO();
                        //foreach (var item in auxAreasCuenta)
                        //{
                        //    string ccDesc = rentabilidadFS.GetCCByAC(item);
                        //    byte[] bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                        //    Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                        //    System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                        //    ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                        //    correo.archivos.Add(ReporteEntradas);
                        //}
                        //correo.asunto = "Reportes Costo Semanal por CC";
                        //correo.correos.Add("rene.olea@construplan.com.mx");
                        //correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        //correo.Enviar();
                        #endregion

                        auxAreasCuenta = new List<string> { "10-3", "10-38"/*, "10-41"*/ };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {

                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("jose.jarero@construplan.com.mx");
                        correo.correos.Add("laura.lara@construplan.com.mx");
                        //********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "3-1" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("raul.hernandez@construplan.com.mx");
                        correo.correos.Add("a.samaniego@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "2-1" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("raul.hernandez@construplan.com.mx");
                        correo.correos.Add("a.samaniego@construplan.com.mx");
                        correo.correos.Add("scarlette.chavez@construplan.com.mx");
                        correo.correos.Add("filiberto.bours@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();



                        auxAreasCuenta = new List<string> { "1-10" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        ////********** Correos Responsables ************//
                        correo.correos.Add("luis.leon@construplan.com.mx");
                        correo.correos.Add("a.samaniego@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "5-6" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("jesus.ruiz@construplan.com.mx");
                        correo.correos.Add("mauricio.torres@construplan.com.mx");
                        correo.correos.Add("juan.espinoza@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "5-5" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        ////********** Correos Responsables ************//
                        correo.correos.Add("esteban.rios@construplan.com.mx");

                        correo.correos.Add("ramon.borbon@construplan.com.mx");
                        correo.correos.Add("tadeo.gracia@construplan.com.mx");
                        correo.correos.Add("mauricio.torres@construplan.com.mx");
                        correo.correos.Add("isaac.moreno@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "9-32" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("r.romo@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "19-1" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("francisco.salazarcarreon@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "10-10" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        ////********** Correos Responsables ************//
                        //correo.correos.Add("d.laborin@construplan.com.mx");
                        correo.correos.Add("ulises.olguin@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "11-1" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("jesus.bravo@construplan.com.mx");
                        correo.correos.Add("ulises.olguin@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        //correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        //correo.Enviar();

                        auxAreasCuenta = new List<string> { "10-34" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("ruben.garcia@construplan.com.mx");
                        correo.correos.Add("ulises.olguin@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "11-9" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("marco.lira@construplan.com.mx");
                        correo.correos.Add("ulises.olguin@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "11-2" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("ivan.felix@construplan.com.mx");
                        correo.correos.Add("ulises.olguin@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "10-12" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        ////********** Correos Responsables ************//
                        correo.correos.Add("christian.marquez@construplan.com.mx");
                        correo.correos.Add("ulises.olguin@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "18-1", "18-2" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        ////********** Correos Responsables ************//
                        correo.correos.Add("paul.lara@construplan.com.mx");
                        correo.correos.Add("ivan.romero@construplan.com.mx");
                        correo.correos.Add("juan.cecco@construplan.com.mx");
                        correo.correos.Add("christian.gonzalez@construplan.com.mx");
                        correo.correos.Add("mario.diaz@construplan.com.mx");
                        correo.correos.Add("e.fraijo@construplan.com.mx");
                        correo.correos.Add("agustin.ontiveros@construplan.com.mx");
                        correo.correos.Add("jose.yanez@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "10-2", "10-24", "10-37" };
                        correo = new Infrastructure.DTO.CorreoDTO();
                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            byte[] bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("fernanda.diaz@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "3-2" };
                        correo = new Infrastructure.DTO.CorreoDTO();
                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            byte[] bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("raul.hernandez@construplan.com.mx");
                        correo.correos.Add("a.samaniego@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "11-23" };
                        correo = new Infrastructure.DTO.CorreoDTO();
                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            byte[] bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("rodrigo.hernandez@construplan.com.mx");
                        correo.correos.Add("efrain.silva@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "11-22" };
                        correo = new Infrastructure.DTO.CorreoDTO();
                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            byte[] bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        ////correo.correos.Add("marco.quirino@construplan.com.mx");
                        correo.correos.Add("efrain.silva@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "11-24" };
                        correo = new Infrastructure.DTO.CorreoDTO();
                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            byte[] bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("david.lozano@construplan.com.mx");
                        correo.correos.Add("efrain.silva@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "10-25", "10-35" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("fernanda.diaz@construplan.com.mx");
                        correo.correos.Add("laura.lara@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "C13" };
                        correo = new Infrastructure.DTO.CorreoDTO();
                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            byte[] bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = "C13 " + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("fernanda.diaz@construplan.com.mx");
                        //********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "1-9", "1-12", "1-13" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("erick.oropeza@construplan.com.mx");
                        correo.correos.Add("a.samaniego@construplan.com.mx");
                        correo.correos.Add("yanin.verdugo@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "7-17" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("agustin.ontiveros@construplan.com.mx");

                        correo.correos.Add("victor.melendrez@construplan.com.mx");
                        correo.correos.Add("alejandro.lugo@construplan.com.mx");
                        correo.correos.Add("jose.yanez@construplan.com.mx");

                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "10-46" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("fernanda.diaz@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "10-47" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("alejandro.hodgers@construplan.com.mx");
                        correo.correos.Add("jose.jarero@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "11-25" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        ////correo.correos.Add("marco.quirino@construplan.com.mx");
                        correo.correos.Add("efrain.silva@construplan.com.mx");
                        correo.correos.Add("david.lozano@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "11-27" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("jesus.bravo@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "8-11" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("irving.ruiz@construplan.com.mx");
                        correo.correos.Add("gerardo.alvarez@construplan.com.mx");
                        correo.correos.Add("francisco.salazarcarreon@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "8-9" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("juan.espinoza@construplan.com.mx");
                        correo.correos.Add("a.aguilar@construplan.com.mx");
                        correo.correos.Add("mauricio.torres@construplan.com.mx");
                        correo.correos.Add("r.romo@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");
                        //correo.correos.Add("alejandro.lugo@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "8-10" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("alejandro.cota@construplan.com.mx");

                        //********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "11-26" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("fernanda.diaz@construplan.com.mx");

                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "10-45" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("jose.jarero@construplan.com.mx");
                        correo.correos.Add("fernando.matias@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "2-3" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("b.valenzuela@construplan.com.mx");
                        correo.correos.Add("valeria.gomez@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "10-44" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("fernanda.diaz@construplan.com.mx");
                        correo.correos.Add("fernando.matias@construplan.com.mx");

                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "50-1" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("israel.jacquez@construplan.com.mx");
                        correo.correos.Add("r.romo@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "51-1" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("raul.hernandez@construplan.com.mx");
                        correo.correos.Add("a.samaniego@construplan.com.mx");
                        correo.correos.Add("ricardo.campos@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "11-28" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("efrain.silva@construplan.com.mx");
                        correo.correos.Add("hector.escobedo@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "11-29" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        ////correo.correos.Add("marco.quirino@construplan.com.mx");
                        correo.correos.Add("efrain.silva@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "10-48" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("gerardo.rodriguez@construplan.com.mx");
                        correo.correos.Add("eliseo.gonzales@construplan.com.mx");
                        correo.correos.Add("jose.jarero@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "10-49" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("jose.jarero@construplan.com.mx");
                        correo.correos.Add("jorge.godinez@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "11-30" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("ruben.garcia@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "11-32" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("rodrigo.hernandez@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "50-2" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("r.romo@construplan.com.mx");
                        correo.correos.Add("angel.paez@construplan.com.mx");
                        correo.correos.Add("alejandro.lugo@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "E01" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = "E01";
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("ricardo.campos@construplan.com.mx");
                        correo.correos.Add("miguel.canturin@construplan.com.pe");
                        correo.correos.Add("rafael.rios@construplan.com.pe");
                        correo.correos.Add("jose.sono@construplan.com.pe");
                        correo.correos.Add("jose.leyva@construplan.com.pe");
                        correo.correos.Add("wilber.atencio@construplan.com.pe");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "E02" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = "E02";
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("ricardo.campos@construplan.com.mx");
                        correo.correos.Add("miguel.canturin@construplan.com.pe");
                        correo.correos.Add("rafael.rios@construplan.com.pe");
                        correo.correos.Add("jose.sono@construplan.com.pe");
                        correo.correos.Add("jose.leyva@construplan.com.pe");
                        correo.correos.Add("wilber.atencio@construplan.com.pe");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "11-34" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("ruben.garcia@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "52-1" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("ricardo.leal@construplan.com.mx");
                        correo.correos.Add("david.rada@construplan.com.mx");
                        correo.correos.Add("angie.prada@construplan.com.mx");
                        correo.correos.Add("maritza.gerena@construplan.com.mx");
                        correo.correos.Add("mauricio.bohorquez@construplan.com.mx");
                        correo.correos.Add("alexandra.gomez@construplan.com.mx");
                        correo.correos.Add("aura.canas@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "11-36" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("marco.lira@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "11-35" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("david.lozano@construplan.com.mx");
                        correo.correos.Add("efrain.silva@construplan.com.mx");
                        //correo.correos.Add("d.laborin@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "11-37" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("rodrigo.hernandez@construplan.com.mx");
                        correo.correos.Add("efrain.silva@construplan.com.mx");
                        //correo.correos.Add("d.laborin@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "11-33" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("jesus.bravo@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "1-14" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("luis.leon@construplan.com.mx");
                        correo.correos.Add("a.samaniego@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();


                        auxAreasCuenta = new List<string> { "50-3" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("francisco.salazarcarreon@construplan.com.mx");
                        correo.correos.Add("isaac.moreno@construplan.com.mx");
                        correo.correos.Add("irving.ruiz@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "10-52" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("ruben.garcia@construplan.com.mx");
                        correo.correos.Add("efrain.silva@construplan.com.mx");
                        //correo.correos.Add("d.laborin@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "10-51" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("jesus.bravo@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "10-50" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("fernanda.diaz@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "50-4" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("fernando.denton@construplan.com.mx");
                        correo.correos.Add("juan.sanchez@construplan.com.mx");
                        correo.correos.Add("mauricio.torres@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "10-53" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("e.flores@construplan.com.mx");
                        correo.correos.Add("jorge.godinez@construplan.com.mx");
                        correo.correos.Add("jose.jarero@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "11-38" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("jesus.bravo@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        auxAreasCuenta = new List<string> { "11-39" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("efrain.silva@construplan.com.mx");
                        correo.correos.Add("hector.escobedo@construplan.com.mx");
                        correo.correos.Add("rodrigo.hernandez@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();


                        auxAreasCuenta = new List<string> { "11-40" };
                        correo = new Infrastructure.DTO.CorreoDTO();

                        foreach (var item in auxAreasCuenta)
                        {
                            string ccDesc = rentabilidadFS.GetCCByAC(item);
                            var bytesReporteEntradas = rentabilidadFS.ExcelEntradasSemanas(detallesSemana.FirstOrDefault(x => x.cc == item));
                            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
                            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
                            ReporteEntradas.ContentDisposition.FileName = ccDesc + "<>" + fecha.ToString("dd-MM-yyyy") + ".xlsx";
                            correo.archivos.Add(ReporteEntradas);
                        }
                        correo.asunto = "Reportes Costo Semanal por CC";
                        //********** Correos Responsables ************//
                        correo.correos.Add("efrain.silva@construplan.com.mx");
                        correo.correos.Add("hector.escobedo@construplan.com.mx");
                        ////********** Correos Default ************//
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("karla.duarte@construplan.com.mx");

                        correo.cuerpo = "En envían reportes en hoja de cálculo relacionados a los costos semanales por centro de costo";
                        correo.Enviar();

                        result.Add(SUCCESS, true);

                    }
                    else
                    {
                        result.Add(SUCCESS, false);
                        result.Add(MESSAGE, "No existe corte para la fecha indicada.");
                    }
                }
                catch (Exception)
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
                }
            }
            else if (empresa == 2)
            {
                try
                {
                    if (corte != null)
                    {
                        System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                        var correo = new Infrastructure.DTO.CorreoDTO();

                        var bytesSemanal = rentabilidadFS.ExcelSemanalKubrix(corte.id);
                        Stream streamSemanal = new MemoryStream(bytesSemanal);
                        System.Net.Mail.Attachment semanal = new System.Net.Mail.Attachment(streamSemanal, ct);
                        semanal.ContentDisposition.FileName = "semanal.xlsx";
                        correo.archivos.Add(semanal);

                        var bytesSubcuentas = rentabilidadFS.ExcelPorSubcuentaKubrix(corte.id);
                        Stream streamSubcuentas = new MemoryStream(bytesSubcuentas);
                        System.Net.Mail.Attachment subcuentas = new System.Net.Mail.Attachment(streamSubcuentas, ct);
                        subcuentas.ContentDisposition.FileName = "subcuentas.xlsx";
                        correo.archivos.Add(subcuentas);

                        correo.asunto = "Archivos Autogenerados Arrendadora";
                        correo.correos.Add("f.artalejo@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("rene.olea@construplan.com.mx");

                        correo.cuerpo = "Se envian documentos Excel autogenerados";
                        correo.Enviar();
                    }

                    result.Add(SUCCESS, true);
                }
                catch (Exception)
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
                }
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult GuardarCorteEstimados()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuario = getUsuario().id;
                int corteID = 0;
                var empresa = vSesiones.sesionEmpresaActual;
                switch (empresa)
                {
                    case 1:

                        break;
                    case 2:
                        corteID = rentabilidadFS.GuardarEstimadosArrendadora();
                        break;
                }
                if (corteID > 0)
                {
                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, "Ocurrió un error al guardar el corte.");
                }
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }


        public ActionResult getLstFechasCortes(int tipoCorte = 0)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstFechas = rentabilidadFS.getLstFechasCortes(tipoCorte);
                result.Add(SUCCESS, true);
                result.Add("fechas", lstFechas);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult getHorasCorte(DateTime fecha, int tipoCorte)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = rentabilidadFS.getCortesPorFecha(fecha, tipoCorte).Select(x => new Core.DTO.Principal.Generales.ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.fecha.Day.ToString() + " " + char.ToUpper(x.fecha.ToString("MMMM")[0]) + x.fecha.ToString("MMMM").Substring(1) + " " + x.fecha.ToString("yyyy HH:ss")
                });
                result.Add(ITEMS, lst);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return json;
        }

        public JsonResult getLstKubrixCorteDet(int corteID, int tipo, int columna, int renglon, List<int> modelos, string economico, DateTime fechaInicio, DateTime fechaFin, List<string> areaCuenta, string divisionCol, string areaCuentaCol, string economicoCol, int tipoCorte = 0, bool reporteCostos = false)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<CorteDTO> lst1 = new List<CorteDTO>();
                List<CorteDTO> lst2 = new List<CorteDTO>();
                List<CorteDTO> lst2Detallada = new List<CorteDTO>();
                List<CorteDTO> lst3 = new List<CorteDTO>();
                List<CorteDTO> lst4 = new List<CorteDTO>();
                List<CorteDTO> lst5 = new List<CorteDTO>();

                var auxFechaEspecial = new DateTime(2020, 2, 29);

                tblM_KBCorte corte = rentabilidadFS.getCorteByID(corteID);

                var fechaCorteActual = corte.fechaCorte;
                if (tipo == 3) for (int i = 1; i < columna; i++)
                    {
                        switch (tipoCorte)
                        {
                            case 0:
                                fechaCorteActual = fechaCorteActual.AddDays(-7);
                                break;
                            case 1:
                                fechaCorteActual = fechaCorteActual.AddMonths(-1);
                                break;
                        }

                    }
                var auxFechaFin = fechaCorteActual;
                fechaFin = fechaCorteActual;

                if (reporteCostos)
                {
                    switch (renglon)
                    {
                        case 0: renglon = 4; break;
                        case 1: renglon = 5; break;
                        case 2: renglon = 6; break;
                        case 3: renglon = 8; break;
                        case 4: renglon = 10; break;
                        case 5: renglon = 12; break;
                        case 6: renglon = 13; break;
                    }
                }

                var usuarioID = getUsuario().id;

                switch (tipoCorte)
                {
                    case 0:
                        //corte semana 2
                        lst2 = new List<CorteDTO>();
                        auxFechaFin = auxFechaFin.AddDays(-7);
                        corte = rentabilidadFS.getCortesPorFecha(auxFechaFin, 0).OrderByDescending(x => x.fecha).FirstOrDefault();
                        if (corte != null)
                        {
                            lst2 = rentabilidadFS.getLstKubrixCorteDet(corte.id, tipo, columna, renglon, modelos, economico, fechaInicio, auxFechaFin, areaCuenta, 2, usuarioID, divisionCol, areaCuentaCol, economicoCol, reporteCostos);
                            lst2Detallada = rentabilidadFS.getLstKubrixCorteAnterior(corte.id, tipo, columna, renglon, modelos, economico, fechaInicio, auxFechaFin, areaCuenta, usuarioID, divisionCol, areaCuentaCol, economicoCol, reporteCostos);
                        }
                        //corte semana 3
                        lst3 = new List<CorteDTO>();
                        auxFechaFin = auxFechaFin.AddDays(-7);
                        corte = rentabilidadFS.getCortesPorFecha(auxFechaFin, 0).OrderByDescending(x => x.fecha).FirstOrDefault();
                        if (corte != null) { lst3 = rentabilidadFS.getLstKubrixCorteDet(corte.id, tipo, columna, renglon, modelos, economico, fechaInicio, auxFechaFin, areaCuenta, 3, usuarioID, divisionCol, areaCuentaCol, economicoCol, reporteCostos); }
                        //corte semana 4
                        lst4 = new List<CorteDTO>();
                        auxFechaFin = auxFechaFin.AddDays(-7);
                        corte = rentabilidadFS.getCortesPorFecha(auxFechaFin, 0).OrderByDescending(x => x.fecha).FirstOrDefault();
                        if (corte != null) { lst4 = rentabilidadFS.getLstKubrixCorteDet(corte.id, tipo, columna, renglon, modelos, economico, fechaInicio, auxFechaFin, areaCuenta, 4, usuarioID, divisionCol, areaCuentaCol, economicoCol, reporteCostos); }
                        //corte semana 5
                        lst5 = new List<CorteDTO>();
                        auxFechaFin = auxFechaFin.AddDays(-7);
                        corte = rentabilidadFS.getCortesPorFecha(auxFechaFin, 0).OrderByDescending(x => x.fecha).FirstOrDefault();
                        if (corte != null) { lst5 = rentabilidadFS.getLstKubrixCorteDet(corte.id, tipo, columna, renglon, modelos, economico, fechaInicio, auxFechaFin, areaCuenta, 5, usuarioID, divisionCol, areaCuentaCol, economicoCol, reporteCostos); }
                        //corte actual
                        corte = rentabilidadFS.getCortesPorFecha(fechaCorteActual, 0).OrderByDescending(x => x.fecha).FirstOrDefault();
                        lst1 = rentabilidadFS.getLstKubrixCorteActualDet(corte.id, tipo, columna, renglon, modelos, economico, fechaInicio, fechaFin, areaCuenta, 1, usuarioID, lst2Detallada, divisionCol, areaCuentaCol, economicoCol, reporteCostos);
                        break;
                    case 1:
                        //corte semana 2
                        lst2 = new List<CorteDTO>();
                        auxFechaFin = new DateTime(auxFechaFin.Year, auxFechaFin.Month, 1).AddDays(-1);
                        corte = rentabilidadFS.getCortesPorMes(auxFechaFin).OrderByDescending(x => x.fecha).FirstOrDefault();
                        if (corte != null)
                        {
                            lst2 = rentabilidadFS.getLstKubrixCorteDet(corte.id, tipo, columna, renglon, modelos, economico, fechaInicio, auxFechaFin, areaCuenta, 2, usuarioID, divisionCol, areaCuentaCol, economicoCol, reporteCostos);
                            lst2Detallada = rentabilidadFS.getLstKubrixCorteAnterior(corte.id, tipo, columna, renglon, modelos, economico, fechaInicio, auxFechaFin, areaCuenta, usuarioID, divisionCol, areaCuentaCol, economicoCol, reporteCostos);
                        }
                        //corte semana 3
                        lst3 = new List<CorteDTO>();
                        auxFechaFin = new DateTime(auxFechaFin.Year, auxFechaFin.Month, 1).AddDays(-1);
                        corte = rentabilidadFS.getCortesPorMes(auxFechaFin).OrderByDescending(x => x.fecha).FirstOrDefault();
                        if (corte != null) { lst3 = rentabilidadFS.getLstKubrixCorteDet(corte.id, tipo, columna, renglon, modelos, economico, fechaInicio, auxFechaFin, areaCuenta, 3, usuarioID, divisionCol, areaCuentaCol, economicoCol, reporteCostos); }
                        //corte semana 4
                        lst4 = new List<CorteDTO>();
                        auxFechaFin = new DateTime(auxFechaFin.Year, auxFechaFin.Month, 1).AddDays(-1);
                        corte = rentabilidadFS.getCortesPorMes(auxFechaFin).OrderByDescending(x => x.fecha).FirstOrDefault();
                        if (corte != null) { lst4 = rentabilidadFS.getLstKubrixCorteDet(corte.id, tipo, columna, renglon, modelos, economico, fechaInicio, auxFechaFin, areaCuenta, 4, usuarioID, divisionCol, areaCuentaCol, economicoCol, reporteCostos); }
                        //corte semana 5
                        lst5 = new List<CorteDTO>();
                        auxFechaFin = new DateTime(auxFechaFin.Year, auxFechaFin.Month, 1).AddDays(-1);
                        corte = rentabilidadFS.getCortesPorMes(auxFechaFin).OrderByDescending(x => x.fecha).FirstOrDefault();
                        if (corte != null) { lst5 = rentabilidadFS.getLstKubrixCorteDet(corte.id, tipo, columna, renglon, modelos, economico, fechaInicio, auxFechaFin, areaCuenta, 5, usuarioID, divisionCol, areaCuentaCol, economicoCol, reporteCostos); }
                        //corte actual
                        corte = rentabilidadFS.getCortesPorMes(fechaCorteActual).OrderByDescending(x => x.fecha).FirstOrDefault();
                        lst1 = rentabilidadFS.getLstKubrixCorteActualDet(corte.id, tipo, columna, renglon, modelos, economico, fechaInicio, fechaFin, areaCuenta, 1, usuarioID, lst2Detallada, divisionCol, areaCuentaCol, economicoCol, reporteCostos);
                        break;
                }
                //
                var esSuccess = lst1.Count > 0 || lst2.Count() > 0 || lst3.Count() > 0 || lst4.Count() > 0 || lst5.Count() > 0;
                if (esSuccess)
                {
                    List<CorteDTO> detalles = new List<CorteDTO>();
                    List<tblM_KBDivisionDetalle> divisiones = rentabilidadFS.getDivisionesDetalle();
                    List<tblM_KBCatCuenta> cuentasDesc = rentabilidadFS.getCuentasDescripcion();
                    var total = lst1.Sum(x => x.monto);

                    detalles.AddRange(lst1);
                    detalles.AddRange(lst2);
                    detalles.AddRange(lst3);
                    detalles.AddRange(lst4);
                    detalles.AddRange(lst5);
                    var data = detalles.Where(x => x.cuenta != null).GroupBy(x => new { x.cc, x.cuenta, x.areaCuenta, x.concepto, x.tipoEquipo, x.poliza, x.semana, x.referencia, x.empresa, x.tipoMov }).Select(x =>
                    {
                        //var cuentas = x.Key.cuenta.Split('-');
                        //var tipoInsumoDesc = cuentasDesc.FirstOrDefault(y => y.cuenta == (cuentas[0] + "-" + cuentas[1] + "-0"));
                        //var grupoInsumoDesc = cuentasDesc.FirstOrDefault(y => y.cuenta == x.Key.cuenta);
                        var divisionDetalle = vSesiones.sesionEmpresaActual == 1 ?
                        divisiones.FirstOrDefault(y => y.ac != null && (y.ac.cc.Trim() + " " + y.ac.descripcion.Trim()) == x.Key.areaCuenta.Trim())
                        : divisiones.FirstOrDefault(y => y.ac != null && (y.ac.areaCuenta.Trim() + " " + y.ac.descripcion.Trim()) == x.Key.areaCuenta.Trim());
                        //var areaCuenta = x.areaCuenta.Split('-');
                        return new CorteDetDTO()
                        {
                            //id = x.id,
                            cc = x.Key.cc,
                            //cta = Int32.Parse(cuentas[0]),
                            //tipoInsumo = cuentas[0] + "-" + cuentas[1],
                            //tipoInsumo_Desc = tipoInsumoDesc == null ? "" : tipoInsumoDesc.descripcion,
                            cuenta = x.Key.cuenta.Contains("4000-4-") ? "5000-4-2" : x.Key.cuenta,
                            //grupoInsumo_Desc = grupoInsumoDesc == null ? "" : grupoInsumoDesc.descripcion,
                            areaCuenta = x.Key.areaCuenta,
                            //insumo_Desc = x.Key.concepto,
                            monto = x.Sum(y => y.monto),
                            //fecha = x.fechapol,
                            tipoEquipo = x.Key.tipoEquipo,
                            //poliza = x.Key.poliza,
                            semana = x.Key.semana,
                            //corteID = x.corteID,
                            referencia = x.Key.referencia,
                            empresa = x.Key.empresa,
                            division = divisionDetalle == null ? getDivisionEnkontrol(x.Key.areaCuenta) : divisionDetalle.division.division,
                            //linea = x.linea,
                            tipoMov = x.Key.tipoMov
                        };
                    }).ToList();
                    result.Add("detalles", data);
                    result.Add("fecha", fechaFin);
                    result.Add("usuarioID", getUsuario().id);
                }

                result.Add(SUCCESS, esSuccess);

                if (esSuccess == false)
                {
                    result.Add(MESSAGE, "No se encontraron registros con los filtros seleccionados.");
                }
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            JsonResult json = new JsonResult();
            json.MaxJsonLength = Int32.MaxValue;
            json.Data = result;
            return json;
        }

        public JsonResult getCuentasDesc()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cuentas = rentabilidadFS.getCuentasDesc();
                result.Add("cuentas", cuentas);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }

            JsonResult json = new JsonResult();
            json.Data = result;
            return json;
        }

        public JsonResult getLstKubrixCorteCostoEstimado(int corteID, int tipoGuardado)
        {
            var result = new Dictionary<string, object>();
            //try
            //{
            List<CorteDTO> lst1 = new List<CorteDTO>();
            var corte = rentabilidadFS.getCorteByID(corteID);
            lst1 = rentabilidadFS.getLstKubrixCorteCostoEstimado(corteID, new DateTime(corte.fechaCorte.Year, 1, 1), corte.fechaCorte, tipoGuardado);

            //
            var esSuccess = lst1.Count > 0;
            if (esSuccess)
            {
                List<tblM_KBDivisionDetalle> divisiones = rentabilidadFS.getDivisionesDetalle();
                List<tblM_KBCatCuenta> cuentasDesc = rentabilidadFS.getCuentasDescripcion();
                var data = lst1.Select(x =>
                {
                    var cuentas = x.cuenta.Split('-');
                    var tipoInsumoDesc = cuentasDesc.FirstOrDefault(y => y.cuenta == (cuentas[0] + "-" + cuentas[1] + "-0"));
                    var grupoInsumoDesc = cuentasDesc.FirstOrDefault(y => y.cuenta == x.cuenta);
                    var divisionDetalle = vSesiones.sesionEmpresaActual == 1 ?
                    divisiones.FirstOrDefault(y => y.ac != null && (y.ac.cc.Trim() + " " + y.ac.descripcion.Trim()) == x.areaCuenta.Trim())
                    : divisiones.FirstOrDefault(y => y.ac != null && (y.ac.areaCuenta.Trim() + " " + y.ac.descripcion.Trim()) == x.areaCuenta.Trim());
                    //var areaCuenta = x.areaCuenta.Split('-');
                    return new RentabilidadDTO()
                    {
                        id = x.id,
                        noEco = x.cc,
                        cta = Int32.Parse(cuentas[0]),
                        tipoInsumo = cuentas[0] + "-" + cuentas[1],
                        tipoInsumo_Desc = tipoInsumoDesc == null ? "" : tipoInsumoDesc.descripcion,
                        grupoInsumo = x.cuenta,
                        grupoInsumo_Desc = grupoInsumoDesc == null ? "" : grupoInsumoDesc.descripcion,
                        areaCuenta = x.areaCuenta,
                        insumo_Desc = x.concepto,
                        //tipo_mov = tipoMov,
                        //importe = negativo ? (x.monto * (-1)) : x.monto,
                        importe = x.monto,
                        fecha = x.fechapol,
                        tipo = x.tipoEquipo,
                        poliza = x.poliza,
                        cc = x.cc,
                        semana = x.semana,
                        corteID = x.corteID,
                        referencia = x.referencia,
                        empresa = x.empresa,
                        division = divisionDetalle == null ? getDivisionEnkontrol(x.areaCuenta) : divisionDetalle.division.division,
                        linea = x.linea,
                        tipoMov = x.tipoMov
                    };
                });
                result.Add("detalles", data);
                result.Add("usuarioID", getUsuario().id);
            }

            result.Add(SUCCESS, esSuccess);

            if (esSuccess == false)
            {
                result.Add(MESSAGE, "No se encontraron registros con los filtros seleccionados.");
            }
            //}
            //catch (Exception e)
            //{
            //    result.Add(SUCCESS, false);
            //    result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            //}
            JsonResult json = new JsonResult();
            json.MaxJsonLength = Int32.MaxValue;
            json.Data = result;
            return json;
        }

        public JsonResult getLstKubrixCorteDetTabla(int corteID, int tipo, int columna, int renglon, List<int> modelos, string economico,
            DateTime fechaFin, List<string> areaCuenta, string divisionCol, string areaCuentaCol, string economicoCol,
            string subcuentaFiltro, string subsubcuentaFiltro, string divisionFiltro, string areaCuentaFiltro, string conciliacionFiltro,
            string economicoFiltro, bool semanal, int acumulado, int empresa = 1, int tipoCorte = 0, bool reporteCostos = false)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<CorteDTO> lst1 = new List<CorteDTO>();
                List<CorteDTO> lst2 = new List<CorteDTO>();

                //var auxFechaEspecial = new DateTime(2020, 2, 29);

                tblM_KBCorte corte = rentabilidadFS.getCorteByID(corteID);
                tblM_KBCorte corteAnt = new tblM_KBCorte();
                var cortesAnt = rentabilidadFS.getCortesAnt(corte.fechaCorte, tipoCorte);
                if (tipo == 3 && columna > 0)
                {
                    corte = cortesAnt.Skip(columna - 1).FirstOrDefault();
                    corteAnt = cortesAnt.Skip(columna).FirstOrDefault();
                }
                else
                {
                    corteAnt = cortesAnt.FirstOrDefault();
                }
                var fechaCorteActual = corte.fechaCorte;

                //-> Calculo de fecha inicio de búsqueda en base a si se busca el acumulado o el año actual
                var fechaInicio = new DateTime(1900, 1, 1);
                switch (acumulado)
                {
                    case 1:
                        fechaInicio = new DateTime(fechaCorteActual.Year, 1, 1);
                        break;
                    case 2:
                        fechaInicio = new DateTime(2020, 1, 1);
                        break;
                }
                var usuarioID = getUsuario().id;




                //if (tipo == 3) for (int i = 1; i < columna; i++) { 

                //    fechaCorteActual = fechaCorteActual.AddDays(-7); 
                //}
                var auxFechaFin = fechaCorteActual;
                fechaFin = fechaCorteActual;

                if (reporteCostos)
                {
                    switch (renglon)
                    {
                        case 0: renglon = 4; break;
                        case 1: renglon = 5; break;
                        case 2: renglon = 6; break;
                        case 3: renglon = 8; break;
                        case 4: renglon = 10; break;
                        case 5: renglon = 12; break;
                        case 6: renglon = 13; break;
                    }
                }
                lst2 = new List<CorteDTO>();
                if (corteAnt != null)
                {
                    if (vSesiones.sesionEmpresaActual == 1)
                    {
                        lst2 = rentabilidadFS.getLstKubrixCorteDetCompletoCplan(corteAnt.id, tipo, columna, renglon,
                            modelos, economico, fechaInicio, auxFechaFin, areaCuenta, 2, usuarioID, divisionCol, areaCuentaCol,
                            economicoCol, subcuentaFiltro, subsubcuentaFiltro, divisionFiltro, areaCuentaFiltro, conciliacionFiltro,
                            economicoFiltro, empresa, reporteCostos, acumulado);
                    }
                    else
                    {
                        lst2 = rentabilidadFS.getLstKubrixCorteDetCompleto(corteAnt.id, tipo, columna, renglon,
                            modelos, economico, fechaInicio, auxFechaFin, areaCuenta, 2, usuarioID, divisionCol, areaCuentaCol,
                            economicoCol, subcuentaFiltro, subsubcuentaFiltro, divisionFiltro, areaCuentaFiltro, conciliacionFiltro,
                            economicoFiltro, empresa, reporteCostos);
                    }

                }
                if (vSesiones.sesionEmpresaActual == 1)
                {
                    lst1 = rentabilidadFS.getLstKubrixCorteDetCompletoCplan(corte.id, tipo, columna, renglon, modelos, economico, fechaInicio,
                        fechaFin, areaCuenta, 1, usuarioID, divisionCol, areaCuentaCol, economicoCol, subcuentaFiltro, subsubcuentaFiltro,
                        divisionFiltro, areaCuentaFiltro, conciliacionFiltro, economicoFiltro, empresa, reporteCostos, acumulado);
                }
                else
                {
                    lst1 = rentabilidadFS.getLstKubrixCorteDetCompleto(corte.id, tipo, columna, renglon, modelos, economico, fechaInicio,
                        fechaFin, areaCuenta, 1, usuarioID, divisionCol, areaCuentaCol, economicoCol, subcuentaFiltro, subsubcuentaFiltro,
                        divisionFiltro, areaCuentaFiltro, conciliacionFiltro, economicoFiltro, empresa, reporteCostos);
                }

                //
                var esSuccess = lst1.Count > 0;
                if (esSuccess)
                {
                    List<tblM_KBDivisionDetalle> divisiones = rentabilidadFS.getDivisionesDetalle();
                    List<tblM_KBCatCuenta> cuentasDesc = rentabilidadFS.getCuentasDescripcion();
                    if (semanal)
                    {
                        var auxDetallesEliminados = lst2.GroupJoin(lst1, x => new { x.poliza, x.linea, x.monto }, y => new { y.poliza, y.linea, y.monto }, (x, y) => new { x, y }).Where(e => e.y.Count() < 1)
                            .Select(z => z.x).ToList();

                        var auxDetallesActual = lst1.GroupJoin(lst2, x => new { x.poliza, x.linea, x.monto }, y => new { y.poliza, y.linea, y.monto }, (x, y) => new { x, y }).Where(e => e.y.Count() < 1)
                            .Select(z => z.x).ToList();



                        //foreach (var item in lst2)
                        //{
                        //    var existe = lst1.FirstOrDefault(x => x.linea == item.linea && x.poliza == item.poliza && x.monto == item.monto);
                        //    if (existe == null) auxDetallesEliminadosRaw.Add(item);
                        //}

                        //foreach (var item in lst1)
                        //{
                        //    var existe = lst2.FirstOrDefault(x => x.linea == item.linea && x.poliza == item.poliza && x.monto == item.monto);
                        //    if (existe == null) auxDetallesActualRaw.Add(item);
                        //}
                        lst1 = auxDetallesActual;


                        auxDetallesEliminados.ForEach(x => x.tipoMov = 2);
                        auxDetallesEliminados.ForEach(x => x.monto = x.monto * (-1));
                        auxDetallesEliminados.ForEach(x => x.concepto = "**REGISTRO ELIMINADO** " + x.concepto);

                        lst1.AddRange(auxDetallesEliminados);
                    }

                    var data = lst1.Select(x =>
                    {
                        var cuentas = x.cuenta.Split('-');
                        var tipoInsumoDesc = cuentasDesc.FirstOrDefault(y => y.cuenta == (cuentas[0] + "-" + cuentas[1] + "-0"));
                        var grupoInsumoDesc = cuentasDesc.FirstOrDefault(y => y.cuenta == x.cuenta);
                        var divisionDetalle = vSesiones.sesionEmpresaActual == 1 ?
                        divisiones.FirstOrDefault(y => y.ac != null && (y.ac.cc.Trim() + " " + y.ac.descripcion.Trim()) == x.areaCuenta.Trim())
                        : divisiones.FirstOrDefault(y => y.ac != null && (y.ac.areaCuenta.Trim() + " " + y.ac.descripcion.Trim()) == x.areaCuenta.Trim());
                        //var areaCuenta = x.areaCuenta.Split('-');
                        return new RentabilidadDTO()
                        {
                            id = x.id,
                            noEco = x.cc,
                            cta = Int32.Parse(cuentas[0]),
                            tipoInsumo = cuentas[0] + "-" + cuentas[1],
                            tipoInsumo_Desc = tipoInsumoDesc == null ? "" : tipoInsumoDesc.descripcion,
                            grupoInsumo = x.cuenta,
                            grupoInsumo_Desc = grupoInsumoDesc == null ? "" : grupoInsumoDesc.descripcion,
                            areaCuenta = x.areaCuenta,
                            insumo_Desc = x.concepto,
                            //tipo_mov = tipoMov,
                            //importe = negativo ? (x.monto * (-1)) : x.monto,
                            importe = x.monto,
                            fecha = x.fechapol,
                            tipo = x.tipoEquipo,
                            poliza = x.poliza,
                            cc = x.cc,
                            semana = x.semana,
                            corteID = x.corteID,
                            referencia = x.referencia,
                            empresa = x.empresa,
                            division = divisionDetalle == null ? getDivisionEnkontrol(x.areaCuenta) : divisionDetalle.division.division,
                            linea = x.linea,
                            tipoMov = x.tipoMov
                        };
                    });
                    result.Add("detalles", data);
                    result.Add("fecha", fechaFin);
                    result.Add("usuarioID", getUsuario().id);
                }

                result.Add(SUCCESS, esSuccess);

                if (esSuccess == false)
                {
                    result.Add(MESSAGE, "No se encontraron registros con los filtros seleccionados.");
                }
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            JsonResult json = new JsonResult();
            json.MaxJsonLength = Int32.MaxValue;
            json.Data = result;
            return json;
        }
        //--> Carga Ppal de tabla Kubrix
        public JsonResult getLstKubrixCorte(int corteID, List<int> modelos, string economico, DateTime fechaFin, List<string> areaCuenta, int acumulado, int tipoCorte = 0, bool reporteCostos = false)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            //try
            //{
            List<CortePpalDTO> detalles = new List<CortePpalDTO>();
            List<DateTime> fechas = new List<DateTime>();
            List<int> cortes = new List<int>();
            var corte = rentabilidadFS.getCorteByID(corteID);
            var auxFechaFin = corte.fechaCorte;
            //-> Calculo de fecha inicio de búsqueda en base a si se busca el acumulado o el año actual
            var fechaInicio = new DateTime(1900, 1, 1);
            switch (acumulado)
            {
                case 1:
                    fechaInicio = new DateTime(auxFechaFin.Year, 1, 1);
                    break;
                case 2:
                    fechaInicio = new DateTime(2020, 1, 1);
                    break;
            }

            fechaFin = auxFechaFin;
            var usuarioID = getUsuario().id;
            cortes.Add(corte.id);
            fechas.Add(auxFechaFin);
            //--> Carga de cortes para las 5 semanas anteriores
            var cortesAnt = rentabilidadFS.getCortesAnt(auxFechaFin, tipoCorte);
            var cortesAntID = cortesAnt.Select(x => x.id).ToList();
            cortes.AddRange(cortesAntID);
            var cortesAntFechas = cortesAnt.Select(x => x.fechaCorte).ToList();
            fechas.AddRange(cortesAntFechas);
            //--> Si no existen 6 cortes anteriores a la fecha de búsqueda se rellenan los espacios con ids y fechas default
            if (cortes.Count() < 6)
            {
                for (int i = cortes.Count(); i < 6; i++)
                {
                    cortes.Add(-1);
                    fechas.Add(default(DateTime));
                }
            }
            cortes.Reverse();
            fechas.Reverse();
            //->Carga de detalles de los cortes 
            detalles = rentabilidadFS.getLstKubrixCortes(cortes, areaCuenta, modelos, economico, fechaInicio, fechas, usuarioID, reporteCostos);
            //-->Carga de Cortes de años anteriores a 2018
            int anioCortesAnteriores = 2018;
            if (corte != null) anioCortesAnteriores = corte.fechaCorte.Year - 2;
            List<CortePpalDTO> detallesCortesAnteriores = new List<CortePpalDTO>();
            detallesCortesAnteriores = null;
            detallesCortesAnteriores = rentabilidadFS.getLstKubrixCortesAnteriores(anioCortesAnteriores, areaCuenta, modelos, economico, usuarioID, reporteCostos);
            var esSuccess = detalles.Count() > 0;
            if (esSuccess)
            {
                List<tblM_KBDivisionDetalle> divisiones = rentabilidadFS.getDivisionesDetalle();

                if (vSesiones.sesionEmpresaActual == 2)
                {
                    var data = detalles.AsEnumerable().GroupBy(x => new { x.cc, x.cuenta, x.areaCuenta, x.tipoEquipo, x.semana, x.tipoMov, x.referencia, x.empresa }).Select(x =>
                    {
                        var divisionDetalle = vSesiones.sesionEmpresaActual == 1 ?
                        divisiones.FirstOrDefault(y => y.ac != null && (y.ac.cc.Trim() + " " + y.ac.descripcion.Trim()) == x.Key.areaCuenta.Trim())
                        : divisiones.FirstOrDefault(y => y.ac != null && (y.ac.areaCuenta.Trim() + " " + y.ac.descripcion.Trim()) == x.Key.areaCuenta.Trim());
                        return new CorteDetDTO()
                        {
                            cc = x.Key.cc,
                            cuenta = x.Key.cuenta,
                            areaCuenta = x.Key.areaCuenta,
                            monto = x.Sum(y => y.monto),
                            tipoEquipo = x.Key.tipoEquipo,
                            semana = x.Key.semana,
                            referencia = x.Key.referencia,
                            empresa = x.Key.empresa,
                            division = divisionDetalle == null ? getDivisionEnkontrol(x.Key.areaCuenta) : divisionDetalle.division.division,
                            tipoMov = x.Key.tipoMov
                        };
                    }).ToList();


                    var dataAnterior = detallesCortesAnteriores.AsEnumerable().GroupBy(x => new { x.cc, x.cuenta, x.areaCuenta, x.tipoEquipo, x.referencia, x.empresa }).Select(x =>
                    {
                        var divisionDetalle = vSesiones.sesionEmpresaActual == 1 ?
                        divisiones.FirstOrDefault(y => y.ac != null && (y.ac.cc.Trim() + " " + y.ac.descripcion.Trim()) == x.Key.areaCuenta.Trim())
                        : divisiones.FirstOrDefault(y => y.ac != null && (y.ac.areaCuenta.Trim() + " " + y.ac.descripcion.Trim()) == x.Key.areaCuenta.Trim());
                        return new CorteDetDTO()
                        {
                            cc = x.Key.cc,
                            cuenta = x.Key.cuenta,
                            areaCuenta = x.Key.areaCuenta,
                            monto = x.Sum(y => y.monto),
                            tipoEquipo = x.Key.tipoEquipo,
                            semana = 6,
                            referencia = x.Key.referencia,
                            empresa = x.Key.empresa,
                            division = divisionDetalle == null ? getDivisionEnkontrol(x.Key.areaCuenta) : divisionDetalle.division.division,
                            tipoMov = 0
                        };
                    }).ToList();
                    data.AddRange(dataAnterior);

                    result.Add("lst", data);
                }

                else
                {
                    var data = detalles.AsEnumerable().GroupBy(x => new { x.cuenta, x.areaCuenta, x.semana, x.tipoMov, x.acumulado }).Select(x =>
                    {
                        var divisionDetalle = vSesiones.sesionEmpresaActual == 1 ?
                        divisiones.FirstOrDefault(y => y.ac != null && y.ac.cc.ToUpper().Trim() == x.Key.areaCuenta.Split(' ')[0].ToUpper().Trim())
                        : divisiones.FirstOrDefault(y => y.ac != null && (y.ac.areaCuenta.ToUpper().Trim() + " " + y.ac.descripcion.ToUpper().Trim()) == x.Key.areaCuenta.ToUpper().Trim());
                        var data2 = new
                        {
                            cuenta = x.Key.cuenta.Contains("4000-4-") ? "5000-4-2" : x.Key.cuenta,
                            cuentaUltimo = x.Key.cuenta.Contains("4000-4-") ? "2" : x.Key.cuenta.Split('-')[2],
                            areaCuenta = x.Key.areaCuenta,
                            monto = x.Sum(y => y.monto),
                            semana = x.Key.semana,
                            division = divisionDetalle == null ? getDivisionEnkontrol(x.Key.areaCuenta) : divisionDetalle.division.division,
                            tipoMov = x.Key.tipoMov,
                            acumulado = x.Key.acumulado,
                        };
                        return data2;
                    }).ToList();


                    var dataAnterior = detallesCortesAnteriores.AsEnumerable().GroupBy(x => new { x.cuenta, x.areaCuenta, x.acumulado }).Select(x =>
                    {
                        var divisionDetalle = vSesiones.sesionEmpresaActual == 1 ?
                        divisiones.FirstOrDefault(y => y.ac != null && y.ac.cc.ToUpper().Trim() == x.Key.areaCuenta.Split(' ')[0].ToUpper().Trim())
                        : divisiones.FirstOrDefault(y => y.ac != null && (y.ac.areaCuenta.ToUpper().Trim() + " " + y.ac.descripcion.ToUpper().Trim()) == x.Key.areaCuenta.ToUpper().Trim());
                        return new
                        {
                            cuenta = x.Key.cuenta.Contains("4000-4-") ? "5000-4-2" : x.Key.cuenta,
                            cuentaUltimo = x.Key.cuenta.Contains("4000-4-") ? "2" : x.Key.cuenta.Split('-')[2],
                            areaCuenta = x.Key.areaCuenta,
                            monto = x.Sum(y => y.monto),
                            semana = 6,
                            division = divisionDetalle == null ? getDivisionEnkontrol(x.Key.areaCuenta) : divisionDetalle.division.division,
                            tipoMov = 0,
                            acumulado = x.Key.acumulado,
                        };
                    }).ToList();



                    data.AddRange(dataAnterior);
                    List<string> lstDivisiones = new List<string>();
                    lstDivisiones.Add("ADMINISTRACION");
                    lstDivisiones.Add("ADMINISTRACION ARRENDADORA");
                    lstDivisiones.Add("SIN DIVISION");
                    lstDivisiones.Add("FLETES");
                    var lstDevuelta = data.Where(r => !lstDivisiones.Contains(r.division)).OrderBy(y => y.cuentaUltimo).ToList();

                    var obtenerLstArrendadora = rentabilidadFS.obtenerCortesArrendadora(corteID, fechaFin);
                    var lstCC = rentabilidadFS.obtenerCentrosCostos();
                    var formato = obtenerLstArrendadora.AsEnumerable().Select(x =>
                    {
                        var divisionDetalle = divisiones.FirstOrDefault(y => y.ac != null && (y.ac.areaCuenta.ToUpper().Trim()) == x.areaCuenta.ToUpper().Trim());
                        return new
                        {
                            centroCostos = lstCC.Where(r => r.areaCuenta == x.areaCuenta).FirstOrDefault() == null ? "SINCC" : lstCC.Where(r => r.areaCuenta == x.areaCuenta).Select(y => y.cc + " " + y.descripcion).FirstOrDefault().Trim(),
                            areaCuenta = x.areaCuenta,
                            monto = x.montoAcumulado,
                            semana = x.semana,
                            division = divisionDetalle == null ? getDivision(x.areaCuenta) : divisionDetalle.division.division.Trim(),
                            cuenta = x.cuenta,
                            tipoMov = 0,
                        };
                    }).ToList();
                    //var oblst = formato.Select(n => new CorteDetDTO
                    //{
                    //    division = n.division,
                    //    monto = n.monto * (n.cuenta != "1-1-0" && n.cuenta != "1-2-1" && n.cuenta != "1-2-2" && n.cuenta != "1-2-3" && n.cuenta != "1-3-1" && n.cuenta != "1-3-2" ? (-1) : (1)),
                    //    semana = n.semana,
                    //    areaCuenta = n.areaCuenta,
                    //    cuenta = n.cuenta,
                    //}).ToList();
                    #region NO SIRVE


                    //decimal sumatoria = 0;
                    //CorteDetDTO obj = new CorteDetDTO();
                    //foreach (var item in oblst)
                    //{
                    //    sumatoria += item.monto;
                    //}
                    //obj.monto = sumatoria;
                    //obj.division = "CONSTRUPLAN";
                    //oblst.Add(obj);
                    #endregion
                    lstDivisiones.Add("LLANTAS OTR");
                    var lstFinal = formato.Where(r => !lstDivisiones.Contains(r.division)).ToList();


                    result.Add("lstDetalle", lstFinal);
                    result.Add("lst", lstDevuelta);
                }

                //List<CorteDetDTO> dataAnterior = new List<CorteDetDTO>();
                //dataAnterior = null;

                //result.Add("lst", data);
                //result.Add("lstAnterior", dataAnterior);
                result.Add("fecha", fechaFin);
                //data = null;
                //dataAnterior = null;
                //result.Add("administrativoCentral", administrativoCentral);
                //result.Add("administrativoProyectos", administrativoProyectos);
                //result.Add("otros", otros);
                //listaTabla = null;
                BusqKubrixDTO busq = new BusqKubrixDTO();
                busq.fechaFin = fechaFin;
                //var CXP = rentabilidadFS.getLstCXP(busq);
                //result.Add("CXP", CXP);
                //var CXC = rentabilidadFS.getLstCXC(busq);
                //result.Add("CXC", CXC);
                result.Add("usuarioID", getUsuario().id);

            }

            result.Add(SUCCESS, esSuccess);

            if (esSuccess == false)
            {
                result.Add(MESSAGE, "No se encontraron registros con los filtros seleccionados.");
            }
            //}
            //catch (Exception e)
            //{
            //    result.Add(SUCCESS, false);
            //    result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            //}

            var json = Json(result, JsonRequestBehavior.AllowGet);
            //JsonResult json = new JsonResult();
            json.MaxJsonLength = Int32.MaxValue;
            //json.Data = result;

            return json;
        }
        private string getDivision(string areaCuentaDesc)
        {
            var areaCuenta = areaCuentaDesc.Split(' ')[0];
            List<string> administracion = (new string[] { "0-0" }).ToList();
            List<string> administracionArr = (new string[] { "9-30", "14-1", "14-2", "15-1", "16-1", "16-2", "16-3", "9-13", "994", "14-1" }).ToList();
            if (administracion.Contains(areaCuenta)) return "ADMINISTRACION";
            if (administracionArr.Contains(areaCuenta)) return "ADMINISTRACION ARRENDADORA";
            return "SIN DIVISION";
        }
        public ActionResult getEconomicoEstatus(List<string> economicos)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var maquinasEstatus = rentabilidadFS.getEconomicoEstatus(economicos);
                result.Add("maquinasEstatus", maquinasEstatus);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return json;
        }

        private List<RentabilidadKubrikDTO> obtenerListaKubrixCorte(List<CortePpalDTO> listaCorte/*, List<CortePpalDTO> listaSemana2, List<CortePpalDTO> listaSemana3
            , List<CortePpalDTO> listaSemana4, List<CortePpalDTO> listaSemana5*/, DateTime fecha, bool reporteCostos = false, int corteID = -1, int corte2ID = -1, int corte3ID = -1, int corte4ID = -1, int corte5ID = -1)
        {
            var listaUtilidad = new List<RentabilidadKubrikDTO>();
            List<tblM_KBCatCuenta> cuentasDesc = rentabilidadFS.getCuentasDescripcion();
            List<CortePpalDTO> elemento = null;
            List<CortePpalDTO> elemento2 = null;
            List<CortePpalDTO> elemento3 = null;
            List<CortePpalDTO> elemento4 = null;
            List<CortePpalDTO> elemento5 = null;

            RentabilidadKubrikDTO utilidad = new RentabilidadKubrikDTO();
            RentabilidadKubrikDTO auxUtilidad = new RentabilidadKubrikDTO();
            if (!reporteCostos)
            {
                //--Ingresos Contabilizados--//
                //elemento = listaCorte.Where(x => x.cuenta.Contains("4000-")).ToList();
                //elemento2 = listaSemana2.Where(x => x.cuenta.Contains("4000-")).ToList();
                //elemento3 = listaSemana3.Where(x => x.cuenta.Contains("4000-")).ToList();
                //elemento4 = listaSemana4.Where(x => x.cuenta.Contains("4000-")).ToList();
                //elemento5 = listaSemana5.Where(x => x.cuenta.Contains("4000-")).ToList();

                elemento = listaCorte.Where(x => x.cuenta.Contains("4000-") && (x.semana == 1 || x.semana == 6)).ToList();
                elemento2 = listaCorte.Where(x => x.cuenta.Contains("4000-") && (x.semana == 2 || x.semana == 6)).ToList();
                elemento3 = listaCorte.Where(x => x.cuenta.Contains("4000-") && (x.semana == 3 || x.semana == 6)).ToList();
                elemento4 = listaCorte.Where(x => x.cuenta.Contains("4000-") && (x.semana == 4 || x.semana == 6)).ToList();
                elemento5 = listaCorte.Where(x => x.cuenta.Contains("4000-") && (x.semana == 5 || x.semana == 6)).ToList();

                utilidad = GetSeparadoresKubrixCorte(elemento, elemento2, elemento3, elemento4, elemento5, fecha, 1, "Ingresos Contabilizados", false);
                auxUtilidad = utilidad;
                utilidad.detalles = GetTipoMovCorte(elemento, 1, false, cuentasDesc, 1, corteID);
                utilidad.detalles.AddRange(GetTipoMovCorte(elemento2, 1, false, cuentasDesc, 2, corte2ID));
                utilidad.detalles.AddRange(GetTipoMovCorte(elemento3, 1, false, cuentasDesc, 3, corte3ID));
                utilidad.detalles.AddRange(GetTipoMovCorte(elemento4, 1, false, cuentasDesc, 4, corte4ID));
                utilidad.detalles.AddRange(GetTipoMovCorte(elemento5, 1, false, cuentasDesc, 5, corte5ID));
                listaUtilidad.Add(utilidad);
                //--Ingresos con Estimación--//
                elemento = listaCorte.Where(x => (x.cuenta == "1-1-0" || x.cuenta == "1-3-1" || x.cuenta == "1-3-2") && (x.semana == 1 || x.semana == 6)).ToList();
                elemento2 = listaCorte.Where(x => (x.cuenta == "1-1-0" || x.cuenta == "1-3-1" || x.cuenta == "1-3-2") && (x.semana == 2 || x.semana == 6)).ToList();
                elemento3 = listaCorte.Where(x => (x.cuenta == "1-1-0" || x.cuenta == "1-3-1" || x.cuenta == "1-3-2") && (x.semana == 3 || x.semana == 6)).ToList();
                elemento4 = listaCorte.Where(x => (x.cuenta == "1-1-0" || x.cuenta == "1-3-1" || x.cuenta == "1-3-2") && (x.semana == 4 || x.semana == 6)).ToList();
                elemento5 = listaCorte.Where(x => (x.cuenta == "1-1-0" || x.cuenta == "1-3-1" || x.cuenta == "1-3-2") && (x.semana == 5 || x.semana == 6)).ToList();

                utilidad = GetSeparadoresKubrixCorte(elemento, elemento2, elemento3, elemento4, elemento5, fecha, 2, "Ingresos con Estimación", false);
                utilidad.detalles = GetTipoMovCorte(elemento, 2, false, cuentasDesc, 1, corteID).ToList();
                utilidad.detalles.AddRange(GetTipoMovCorte(elemento2, 2, false, cuentasDesc, 2, corte2ID));
                utilidad.detalles.AddRange(GetTipoMovCorte(elemento3, 2, false, cuentasDesc, 3, corte3ID));
                utilidad.detalles.AddRange(GetTipoMovCorte(elemento4, 2, false, cuentasDesc, 4, corte4ID));
                utilidad.detalles.AddRange(GetTipoMovCorte(elemento5, 2, false, cuentasDesc, 5, corte5ID));
                listaUtilidad.Add(utilidad);
                //--Ingresos Pendientes por Generar--//
                elemento = listaCorte.Where(x => (x.cuenta == "1-2-1" || x.cuenta == "1-2-2" || x.cuenta == "1-2-3") && (x.semana == 1 || x.semana == 6)).ToList();
                elemento2 = listaCorte.Where(x => (x.cuenta == "1-2-1" || x.cuenta == "1-2-2" || x.cuenta == "1-2-3") && (x.semana == 2 || x.semana == 6)).ToList();
                elemento3 = listaCorte.Where(x => (x.cuenta == "1-2-1" || x.cuenta == "1-2-2" || x.cuenta == "1-2-3") && (x.semana == 3 || x.semana == 6)).ToList();
                elemento4 = listaCorte.Where(x => (x.cuenta == "1-2-1" || x.cuenta == "1-2-2" || x.cuenta == "1-2-3") && (x.semana == 4 || x.semana == 6)).ToList();
                elemento5 = listaCorte.Where(x => (x.cuenta == "1-2-1" || x.cuenta == "1-2-2" || x.cuenta == "1-2-3") && (x.semana == 5 || x.semana == 6)).ToList();

                var sumatoria = elemento.Sum(x => x.monto);
                utilidad = GetSeparadoresKubrixCorte(elemento, elemento2, elemento3, elemento4, elemento5, fecha, 3, "Ingresos Pendientes por Generar", false);
                utilidad.detalles = GetTipoMovCorte(elemento, 3, false, cuentasDesc, 1, corteID).ToList();
                utilidad.detalles.AddRange(GetTipoMovCorte(elemento2, 3, false, cuentasDesc, 2, corte2ID));
                utilidad.detalles.AddRange(GetTipoMovCorte(elemento3, 3, false, cuentasDesc, 3, corte3ID));
                utilidad.detalles.AddRange(GetTipoMovCorte(elemento4, 3, false, cuentasDesc, 4, corte4ID));
                utilidad.detalles.AddRange(GetTipoMovCorte(elemento5, 3, false, cuentasDesc, 5, corte5ID));
                listaUtilidad.Add(utilidad);
                //--Subtotal--//
                var subtotal = listaUtilidad.Where(x => x.tipo_mov <= 3);
                utilidad = new RentabilidadKubrikDTO
                {
                    tipo_mov = 4,
                    descripcion = "Total Ingresos",
                    mayor = subtotal.Sum(x => x.mayor),
                    menor = subtotal.Sum(x => x.menor),
                    transporteConstruplan = subtotal.Sum(x => x.transporteConstruplan),
                    transporteArrendadora = subtotal.Sum(x => x.transporteArrendadora),
                    administrativoCentral = subtotal.Sum(x => x.administrativoCentral),
                    administrativoProyectos = subtotal.Sum(x => x.administrativoProyectos),
                    fletes = subtotal.Sum(x => x.fletes),
                    neumaticos = subtotal.Sum(x => x.neumaticos),
                    otros = subtotal.Sum(x => x.otros),
                    total = subtotal.Sum(x => x.total),
                    actual = subtotal.Sum(x => x.actual),
                    semana2 = subtotal.Sum(x => x.semana2),
                    semana3 = subtotal.Sum(x => x.semana3),
                    semana4 = subtotal.Sum(x => x.semana4),
                    semana5 = subtotal.Sum(x => x.semana5),
                };
                listaUtilidad.Add(utilidad);
            }
            //--Costo Total--//
            elemento = listaCorte.Where(x => x.cuenta.Contains("5000-") && !x.cuenta.Contains("5000-10") && (x.semana == 1 || x.semana == 6)).ToList();
            elemento2 = listaCorte.Where(x => x.cuenta.Contains("5000-") && !x.cuenta.Contains("5000-10") && (x.semana == 2 || x.semana == 6)).ToList();
            elemento3 = listaCorte.Where(x => x.cuenta.Contains("5000-") && !x.cuenta.Contains("5000-10") && (x.semana == 3 || x.semana == 6)).ToList();
            elemento4 = listaCorte.Where(x => x.cuenta.Contains("5000-") && !x.cuenta.Contains("5000-10") && (x.semana == 4 || x.semana == 6)).ToList();
            elemento5 = listaCorte.Where(x => x.cuenta.Contains("5000-") && !x.cuenta.Contains("5000-10") && (x.semana == 5 || x.semana == 6)).ToList();

            utilidad = GetSeparadoresKubrixCorte(elemento, elemento2, elemento3, elemento4, elemento5, fecha, 5, "Costo Total", false);
            utilidad.detalles = GetTipoMovCorte(elemento, 5, false, cuentasDesc, 1, corteID).ToList();
            utilidad.detalles.AddRange(GetTipoMovCorte(elemento2, 5, false, cuentasDesc, 2, corte2ID));
            utilidad.detalles.AddRange(GetTipoMovCorte(elemento3, 5, false, cuentasDesc, 3, corte3ID));
            utilidad.detalles.AddRange(GetTipoMovCorte(elemento4, 5, false, cuentasDesc, 4, corte4ID));
            utilidad.detalles.AddRange(GetTipoMovCorte(elemento5, 5, false, cuentasDesc, 5, corte5ID));
            listaUtilidad.Add(utilidad);
            //--Depreciación--//
            elemento = listaCorte.Where(x => x.cuenta.Contains("5000-10") && (x.semana == 1 || x.semana == 6)).ToList();
            elemento2 = listaCorte.Where(x => x.cuenta.Contains("5000-10") && (x.semana == 2 || x.semana == 6)).ToList();
            elemento3 = listaCorte.Where(x => x.cuenta.Contains("5000-10") && (x.semana == 3 || x.semana == 6)).ToList();
            elemento4 = listaCorte.Where(x => x.cuenta.Contains("5000-10") && (x.semana == 4 || x.semana == 6)).ToList();
            elemento5 = listaCorte.Where(x => x.cuenta.Contains("5000-10") && (x.semana == 5 || x.semana == 6)).ToList();

            utilidad = GetSeparadoresKubrixCorte(elemento, elemento2, elemento3, elemento4, elemento5, fecha, 6, "Depreciación", false);
            utilidad.detalles = GetTipoMovCorte(elemento, 6, false, cuentasDesc, 1, corteID).ToList();
            utilidad.detalles.AddRange(GetTipoMovCorte(elemento2, 6, false, cuentasDesc, 2, corte2ID));
            utilidad.detalles.AddRange(GetTipoMovCorte(elemento3, 6, false, cuentasDesc, 3, corte3ID));
            utilidad.detalles.AddRange(GetTipoMovCorte(elemento4, 6, false, cuentasDesc, 4, corte4ID));
            utilidad.detalles.AddRange(GetTipoMovCorte(elemento5, 6, false, cuentasDesc, 5, corte5ID));
            listaUtilidad.Add(utilidad);
            //--Costo Estimado--//
            elemento = listaCorte.Where(x => x.cuenta == "1-4-0" && (x.semana == 1 || x.semana == 6)).ToList();
            elemento2 = listaCorte.Where(x => x.cuenta == "1-4-0" && (x.semana == 2 || x.semana == 6)).ToList();
            elemento3 = listaCorte.Where(x => x.cuenta == "1-4-0" && (x.semana == 3 || x.semana == 6)).ToList();
            elemento4 = listaCorte.Where(x => x.cuenta == "1-4-0" && (x.semana == 4 || x.semana == 6)).ToList();
            elemento5 = listaCorte.Where(x => x.cuenta == "1-4-0" && (x.semana == 5 || x.semana == 6)).ToList();

            utilidad = GetSeparadoresKubrixCorte(elemento, elemento2, elemento3, elemento4, elemento5, fecha, 15, "Costos Estimados", false);
            utilidad.detalles = GetTipoMovCorte(elemento, 15, false, cuentasDesc, 1, corteID).ToList();
            utilidad.detalles.AddRange(GetTipoMovCorte(elemento2, 15, false, cuentasDesc, 2, corte2ID));
            utilidad.detalles.AddRange(GetTipoMovCorte(elemento3, 15, false, cuentasDesc, 3, corte3ID));
            utilidad.detalles.AddRange(GetTipoMovCorte(elemento4, 15, false, cuentasDesc, 4, corte4ID));
            utilidad.detalles.AddRange(GetTipoMovCorte(elemento5, 15, false, cuentasDesc, 5, corte5ID));
            listaUtilidad.Add(utilidad);
            if (!reporteCostos)
            {
                //--Utilidad Bruta--//           
                utilidad = new RentabilidadKubrikDTO
                {
                    tipo_mov = 7,
                    descripcion = "Utilidad Bruta",
                    mayor = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).mayor - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).mayor - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).mayor,
                    menor = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).menor - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).menor - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).menor,
                    transporteConstruplan = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).transporteConstruplan - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).transporteConstruplan - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).transporteConstruplan,
                    transporteArrendadora = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).transporteArrendadora - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).transporteArrendadora - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).transporteArrendadora,
                    administrativoCentral = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).administrativoCentral - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).administrativoCentral - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).administrativoCentral,
                    administrativoProyectos = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).administrativoProyectos - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).administrativoProyectos - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).administrativoProyectos,
                    fletes = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).fletes - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).fletes - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).fletes,
                    neumaticos = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).neumaticos - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).neumaticos - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).neumaticos,
                    otros = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).otros - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).otros - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).otros,
                    total = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).total - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).total - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).total,
                    actual = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).actual - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).actual - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).actual,
                    semana2 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).semana2 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).semana2 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).semana2,
                    semana3 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).semana3 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).semana3 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).semana3,
                    semana4 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).semana4 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).semana4 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).semana4,
                    semana5 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 4).semana5 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 5).semana5 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 6).semana5,
                };
                listaUtilidad.Add(utilidad);
            }
            //--Gastos de Operación--//
            elemento = listaCorte.Where(x => x.cuenta.Contains("5280-") && (x.semana == 1 || x.semana == 6)).ToList();
            elemento2 = listaCorte.Where(x => x.cuenta.Contains("5280-") && (x.semana == 2 || x.semana == 6)).ToList();
            elemento3 = listaCorte.Where(x => x.cuenta.Contains("5280-") && (x.semana == 3 || x.semana == 6)).ToList();
            elemento4 = listaCorte.Where(x => x.cuenta.Contains("5280-") && (x.semana == 4 || x.semana == 6)).ToList();
            elemento5 = listaCorte.Where(x => x.cuenta.Contains("5280-") && (x.semana == 5 || x.semana == 6)).ToList();

            utilidad = GetSeparadoresKubrixCorte(elemento, elemento2, elemento3, elemento4, elemento5, fecha, 8, "Gastos de Operación", false);
            utilidad.detalles = GetTipoMovCorte(elemento, 8, false, cuentasDesc, 1, corteID).ToList();
            utilidad.detalles.AddRange(GetTipoMovCorte(elemento2, 8, false, cuentasDesc, 2, corte2ID));
            utilidad.detalles.AddRange(GetTipoMovCorte(elemento3, 8, false, cuentasDesc, 3, corte3ID));
            utilidad.detalles.AddRange(GetTipoMovCorte(elemento4, 8, false, cuentasDesc, 4, corte4ID));
            utilidad.detalles.AddRange(GetTipoMovCorte(elemento5, 8, false, cuentasDesc, 5, corte5ID));
            listaUtilidad.Add(utilidad);
            if (!reporteCostos)
            {
                //--Gastos Antes de Finacieros--//           
                utilidad = new RentabilidadKubrikDTO
                {
                    tipo_mov = 9,
                    descripcion = "Resultado Antes Finacieros",
                    mayor = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).mayor - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).mayor,
                    menor = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).menor - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).menor,
                    transporteConstruplan = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).transporteConstruplan - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).transporteConstruplan,
                    transporteArrendadora = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).transporteArrendadora - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).transporteArrendadora,
                    administrativoCentral = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).administrativoCentral - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).administrativoCentral,
                    administrativoProyectos = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).administrativoProyectos - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).administrativoProyectos,
                    fletes = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).fletes - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).fletes,
                    neumaticos = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).neumaticos - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).neumaticos,
                    otros = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).otros - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).otros,
                    total = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).total - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).total,
                    actual = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).actual - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).actual,
                    semana2 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).semana2 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).semana2,
                    semana3 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).semana3 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).semana3,
                    semana4 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).semana4 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).semana4,
                    semana5 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 7).semana5 - listaUtilidad.FirstOrDefault(x => x.tipo_mov == 8).semana5
                };
                listaUtilidad.Add(utilidad);
            }
            //--Gastos Financieros--//
            elemento = listaCorte.Where(x => (reporteCostos ? x.cuenta.Contains("5900-") : x.cuenta.Contains("5900-") || x.cuenta.Contains("4900-")) && (x.semana == 1 || x.semana == 6)).ToList();
            elemento2 = listaCorte.Where(x => (reporteCostos ? x.cuenta.Contains("5900-") : x.cuenta.Contains("5900-") || x.cuenta.Contains("4900-")) && (x.semana == 2 || x.semana == 6)).ToList();
            elemento3 = listaCorte.Where(x => (reporteCostos ? x.cuenta.Contains("5900-") : x.cuenta.Contains("5900-") || x.cuenta.Contains("4900-")) && (x.semana == 3 || x.semana == 6)).ToList();
            elemento4 = listaCorte.Where(x => (reporteCostos ? x.cuenta.Contains("5900-") : x.cuenta.Contains("5900-") || x.cuenta.Contains("4900-")) && (x.semana == 4 || x.semana == 6)).ToList();
            elemento5 = listaCorte.Where(x => (reporteCostos ? x.cuenta.Contains("5900-") : x.cuenta.Contains("5900-") || x.cuenta.Contains("4900-")) && (x.semana == 5 || x.semana == 6)).ToList();

            utilidad = GetSeparadoresKubrixCorte(elemento, elemento2, elemento3, elemento4, elemento5, fecha, 10, "Gastos y Productos Financieros", false);
            utilidad.detalles = GetTipoMovCorte(elemento, 10, false, cuentasDesc, 1, corteID).ToList();
            utilidad.detalles.AddRange(GetTipoMovCorte(elemento2, 10, false, cuentasDesc, 2, corte2ID));
            utilidad.detalles.AddRange(GetTipoMovCorte(elemento3, 10, false, cuentasDesc, 3, corte3ID));
            utilidad.detalles.AddRange(GetTipoMovCorte(elemento4, 10, false, cuentasDesc, 4, corte4ID));
            utilidad.detalles.AddRange(GetTipoMovCorte(elemento5, 10, false, cuentasDesc, 5, corte5ID));
            listaUtilidad.Add(utilidad);
            if (!reporteCostos)
            {
                //--Resultado con Financieros--//           
                utilidad = new RentabilidadKubrikDTO
                {
                    tipo_mov = 11,
                    descripcion = "Resultado con Financieros",
                    mayor = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).mayor + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).mayor,
                    menor = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).menor + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).menor,
                    transporteConstruplan = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).transporteConstruplan + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).transporteConstruplan,
                    transporteArrendadora = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).transporteArrendadora + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).transporteArrendadora,
                    administrativoCentral = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).administrativoCentral + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).administrativoCentral,
                    administrativoProyectos = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).administrativoProyectos + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).administrativoProyectos,
                    fletes = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).fletes + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).fletes,
                    neumaticos = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).neumaticos + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).neumaticos,
                    otros = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).otros + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).otros,
                    total = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).total + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).total,
                    actual = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).actual + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).actual,
                    semana2 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).semana2 + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).semana2,
                    semana3 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).semana3 + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).semana3,
                    semana4 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).semana4 + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).semana4,
                    semana5 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 9).semana5 + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 10).semana5,
                };
                listaUtilidad.Add(utilidad);
            }
            //--Otros Ingresos--//
            elemento = listaCorte.Where(x => (reporteCostos ? x.cuenta.Contains("5901-") : x.cuenta.Contains("4901-") || x.cuenta.Contains("5901-")) && (x.semana == 1 || x.semana == 6)).ToList();
            elemento2 = listaCorte.Where(x => (reporteCostos ? x.cuenta.Contains("5901-") : x.cuenta.Contains("4901-") || x.cuenta.Contains("5901-")) && (x.semana == 2 || x.semana == 6)).ToList();
            elemento3 = listaCorte.Where(x => (reporteCostos ? x.cuenta.Contains("5901-") : x.cuenta.Contains("4901-") || x.cuenta.Contains("5901-")) && (x.semana == 3 || x.semana == 6)).ToList();
            elemento4 = listaCorte.Where(x => (reporteCostos ? x.cuenta.Contains("5901-") : x.cuenta.Contains("4901-") || x.cuenta.Contains("5901-")) && (x.semana == 4 || x.semana == 6)).ToList();
            elemento5 = listaCorte.Where(x => (reporteCostos ? x.cuenta.Contains("5901-") : x.cuenta.Contains("4901-") || x.cuenta.Contains("5901-")) && (x.semana == 5 || x.semana == 6)).ToList();

            utilidad = GetSeparadoresKubrixCorte(elemento, elemento2, elemento3, elemento4, elemento5, fecha, 12, "Otros Ingresos", false);
            utilidad.detalles = GetTipoMovCorte(elemento, 12, false, cuentasDesc, 1, corteID).ToList();
            utilidad.detalles.AddRange(GetTipoMovCorte(elemento2, 12, false, cuentasDesc, 2, corte2ID));
            utilidad.detalles.AddRange(GetTipoMovCorte(elemento3, 12, false, cuentasDesc, 3, corte3ID));
            utilidad.detalles.AddRange(GetTipoMovCorte(elemento4, 12, false, cuentasDesc, 4, corte4ID));
            utilidad.detalles.AddRange(GetTipoMovCorte(elemento5, 12, false, cuentasDesc, 5, corte5ID));
            listaUtilidad.Add(utilidad);
            //--Resultado Neto--//      
            if (!reporteCostos)
            {
                utilidad = new RentabilidadKubrikDTO
                {
                    tipo_mov = 13,
                    descripcion = "Resultado Neto",
                    mayor = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).mayor + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).mayor,
                    menor = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).menor + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).menor,
                    transporteConstruplan = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).transporteConstruplan + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).transporteConstruplan,
                    transporteArrendadora = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).transporteArrendadora + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).transporteArrendadora,
                    administrativoCentral = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).administrativoCentral + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).administrativoCentral,
                    administrativoProyectos = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).administrativoProyectos + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).administrativoProyectos,
                    fletes = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).fletes + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).fletes,
                    neumaticos = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).neumaticos + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).neumaticos,
                    otros = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).otros + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).otros,
                    total = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).total + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).total,
                    actual = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).actual + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).actual,
                    semana2 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).semana2 + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).semana2,
                    semana3 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).semana3 + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).semana3,
                    semana4 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).semana4 + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).semana4,
                    semana5 = listaUtilidad.FirstOrDefault(x => x.tipo_mov == 11).semana5 + listaUtilidad.FirstOrDefault(x => x.tipo_mov == 12).semana5,
                };
                listaUtilidad.Add(utilidad);
            }
            else
            {
                utilidad = new RentabilidadKubrikDTO();

                {
                    utilidad.tipo_mov = 13;
                    utilidad.descripcion = "Resultado Neto";
                    foreach (var item in listaUtilidad) utilidad.mayor += item.mayor;
                    foreach (var item in listaUtilidad) utilidad.menor += item.menor;
                    foreach (var item in listaUtilidad) utilidad.transporteConstruplan += item.transporteConstruplan;
                    foreach (var item in listaUtilidad) utilidad.transporteArrendadora += item.transporteArrendadora;
                    foreach (var item in listaUtilidad) utilidad.administrativoCentral += item.administrativoCentral;
                    foreach (var item in listaUtilidad) utilidad.administrativoProyectos += item.administrativoProyectos;
                    foreach (var item in listaUtilidad) utilidad.fletes += item.fletes;
                    foreach (var item in listaUtilidad) utilidad.neumaticos += item.neumaticos;
                    foreach (var item in listaUtilidad) utilidad.otros += item.otros;
                    foreach (var item in listaUtilidad) utilidad.total += item.total;
                    foreach (var item in listaUtilidad) utilidad.actual += item.actual;
                    foreach (var item in listaUtilidad) utilidad.semana2 += item.semana2;
                    foreach (var item in listaUtilidad) utilidad.semana3 += item.semana3;
                    foreach (var item in listaUtilidad) utilidad.semana4 += item.semana4;
                    foreach (var item in listaUtilidad) utilidad.semana5 += item.semana5;
                };
                listaUtilidad.Add(utilidad);
            }
            var auxNeto = utilidad;
            if (!reporteCostos)
            {
                //--% de Margen--//
                utilidad = new RentabilidadKubrikDTO
                {
                    tipo_mov = 14,
                    descripcion = "% de Margen",
                    mayor = auxUtilidad.mayor != 0 ? (auxNeto.mayor / auxUtilidad.mayor) * 100 : 0,
                    menor = auxUtilidad.menor != 0 ? (auxNeto.menor / auxUtilidad.menor) * 100 : 0,
                    transporteConstruplan = auxUtilidad.transporteConstruplan != 0 ? (auxNeto.transporteConstruplan / auxUtilidad.transporteConstruplan) * 100 : 0,
                    transporteArrendadora = auxUtilidad.transporteArrendadora != 0 ? (auxNeto.transporteArrendadora / auxUtilidad.transporteArrendadora) * 100 : 0,
                    administrativoCentral = auxUtilidad.administrativoCentral != 0 ? (auxNeto.administrativoCentral / auxUtilidad.administrativoCentral) * 100 : 0,
                    administrativoProyectos = auxUtilidad.administrativoProyectos != 0 ? (auxNeto.administrativoProyectos / auxUtilidad.administrativoProyectos) * 100 : 0,
                    fletes = auxUtilidad.fletes != 0 ? (auxNeto.fletes / auxUtilidad.fletes) * 100 : 0,
                    neumaticos = auxUtilidad.neumaticos != 0 ? (auxNeto.neumaticos / auxUtilidad.neumaticos) * 100 : 0,
                    otros = auxUtilidad.otros != 0 ? (auxNeto.otros / auxUtilidad.otros) * 100 : 0,
                    total = auxUtilidad.total != 0 ? (auxNeto.total / auxUtilidad.total) * 100 : 0,
                    actual = auxUtilidad.actual != 0 ? (auxNeto.actual / auxUtilidad.actual) * 100 : 0,
                    semana2 = auxUtilidad.semana2 != 0 ? (auxNeto.semana2 / auxUtilidad.semana2) * 100 : 0,
                    semana3 = auxUtilidad.semana3 != 0 ? (auxNeto.semana3 / auxUtilidad.semana3) * 100 : 0,
                    semana4 = auxUtilidad.semana4 != 0 ? (auxNeto.semana4 / auxUtilidad.semana4) * 100 : 0,
                    semana5 = auxUtilidad.semana5 != 0 ? (auxNeto.semana5 / auxUtilidad.semana5) * 100 : 0,
                };
                listaUtilidad.Add(utilidad);
            }
            return listaUtilidad.OrderBy(x => x.tipo_mov).ToList();
        }

        private RentabilidadKubrikDTO GetSeparadoresKubrixCorte(List<CortePpalDTO> elemento, List<CortePpalDTO> elemento2, List<CortePpalDTO> elemento3, List<CortePpalDTO> elemento4, List<CortePpalDTO> elemento5, DateTime fecha, int tipoMov, string tipoMovDesc, bool negativo)
        {
            decimal auxMayorSuma = 0;
            decimal auxMenorSuma = 0;
            decimal auxTransporteConstruplanSuma = 0;
            decimal auxFletesSuma = 0;
            decimal auxNeumaticosSuma = 0;
            decimal auxAdministrativoCentralSuma = 0;
            decimal auxOtrosSuma = 0;
            decimal auxTransporteArrendadoraSuma = 0;
            decimal auxAdministrativoProyectosSuma = 0;
            decimal auxActualSuma = 0;
            decimal auxSemana2Suma = 0;
            decimal auxSemana3Suma = 0;
            decimal auxSemana4Suma = 0;
            decimal auxSemana5Suma = 0;

            var auxMayor = elemento.Where(x => x.tipoEquipo == 1).Select(x => x.monto).AsEnumerable();
            var auxMenor = elemento.Where(x => x.tipoEquipo == 2).Select(x => x.monto).AsEnumerable();
            var auxTransporteConstruplan = elemento.Where(x => x.tipoEquipo == 3).Select(x => x.monto).AsEnumerable();
            var auxFletes = elemento.Where(x => x.tipoEquipo == 4).Select(x => x.monto).AsEnumerable();
            var auxNeumaticos = elemento.Where(x => x.tipoEquipo == 5).Select(x => x.monto).AsEnumerable();
            var auxAdministrativoCentral = elemento.Where(x => x.tipoEquipo == 6).Select(x => x.monto).AsEnumerable();
            var auxOtros = elemento.Where(x => x.tipoEquipo == 7).Select(x => x.monto).AsEnumerable();
            var auxTransporteArrendadora = elemento.Where(x => x.tipoEquipo == 8).Select(x => x.monto).AsEnumerable();
            var auxAdministrativoProyectos = elemento.Where(x => x.tipoEquipo == 9).Select(x => x.monto).AsEnumerable();

            var auxActual = elemento.Where(x => x.semana == 1).Select(x => x.monto).AsEnumerable();
            var auxSemana2 = elemento2.Where(x => x.semana == 2).Select(x => x.monto).AsEnumerable();
            var auxSemana3 = elemento3.Where(x => x.semana == 3).Select(x => x.monto).AsEnumerable();
            var auxSemana4 = elemento4.Where(x => x.semana == 4).Select(x => x.monto).AsEnumerable();
            var auxSemana5 = elemento5.Where(x => x.semana == 5).Select(x => x.monto).AsEnumerable();

            auxMayorSuma = auxMayor.Sum() * (negativo ? (-1) : 1);
            auxMenorSuma = auxMenor.Sum() * (negativo ? (-1) : 1);
            auxTransporteConstruplanSuma = auxTransporteConstruplan.Sum() * (negativo ? (-1) : 1);
            auxFletesSuma = auxFletes.Sum() * (negativo ? (-1) : 1);
            auxNeumaticosSuma = auxNeumaticos.Sum() * (negativo ? (-1) : 1);
            auxAdministrativoCentralSuma = auxAdministrativoCentral.Sum() * (negativo ? (-1) : 1);
            auxOtrosSuma = auxOtros.Sum() * (negativo ? (-1) : 1);
            auxTransporteArrendadoraSuma = auxTransporteArrendadora.Sum() * (negativo ? (-1) : 1);
            auxAdministrativoProyectosSuma = auxAdministrativoProyectos.Sum() * (negativo ? (-1) : 1);
            auxActualSuma = auxActual.Sum() * (negativo ? (-1) : 1);
            auxSemana2Suma = auxSemana2.Sum() * (negativo ? (-1) : 1);
            auxSemana3Suma = auxSemana3.Sum() * (negativo ? (-1) : 1);
            auxSemana4Suma = auxSemana4.Sum() * (negativo ? (-1) : 1);
            auxSemana5Suma = auxSemana5.Sum() * (negativo ? (-1) : 1);


            var data = new RentabilidadKubrikDTO
            {
                tipo_mov = tipoMov,
                descripcion = tipoMovDesc,
                mayor = auxMayorSuma, //auxMayor.Any() ? (auxMayor.Sum() * (negativo ? (-1) : 1)) : 0, //  (negativo ? (elemento.Where(x => x.tipoEquipo == 1).Sum(x => x.monto)) * (-1) : elemento.Where(x => x.tipoEquipo == 1).Sum(x => x.monto)) : 0,
                menor = auxMenorSuma, // auxMenor.Any() ? (auxMenor.Sum() * (negativo ? (-1) : 1)) : 0, //(negativo ? (elemento.Where(x => x.tipoEquipo == 2).Sum(x => x.monto)) * (-1) : elemento.Where(x => x.tipoEquipo == 2).Sum(x => x.monto)) : 0,
                transporteConstruplan = auxTransporteConstruplanSuma, //auxTransporteConstruplan.Any() ? (auxTransporteConstruplan.Sum() * (negativo ? (-1) : 1)) : 0, //(negativo ? (elemento.Where(x => x.tipoEquipo == 3).Sum(x => x.monto)) * (-1) : elemento.Where(x => x.tipoEquipo == 3).Sum(x => x.monto)) : 0,
                transporteArrendadora = auxTransporteArrendadoraSuma, //auxTransporteArrendadora.Any() ? (auxTransporteArrendadora.Sum() * (negativo ? (-1) : 1)) : 0,//(negativo ? (elemento.Where(x => x.tipoEquipo == 8).Sum(x => x.monto)) * (-1) : elemento.Where(x => x.tipoEquipo == 8).Sum(x => x.monto)) : 0,
                administrativoCentral = auxAdministrativoCentralSuma, //auxAdministrativoCentral.Any() ? (auxAdministrativoCentral.Sum() * (negativo ? (-1) : 1)) : 0,//(negativo ? (elemento.Where(x => x.tipoEquipo == 6).Sum(x => x.monto)) * (-1) : elemento.Where(x => x.tipoEquipo == 6).Sum(x => x.monto)) : 0,
                administrativoProyectos = auxAdministrativoProyectosSuma, //auxAdministrativoProyectos.Any() ? (auxAdministrativoProyectos.Sum() * (negativo ? (-1) : 1)) : 0,//(negativo ? (elemento.Where(x => x.tipoEquipo == 9).Sum(x => x.monto)) * (-1) : elemento.Where(x => x.tipoEquipo == 9).Sum(x => x.monto)) : 0,
                fletes = auxFletesSuma, //auxFletes.Any() ? (auxFletes.Sum() * (negativo ? (-1) : 1)) : 0,//(negativo ? (elemento.Where(x => x.tipoEquipo == 4).Sum(x => x.monto)) * (-1) : elemento.Where(x => x.tipoEquipo == 4).Sum(x => x.monto)) : 0,
                neumaticos = auxNeumaticosSuma, //auxNeumaticos.Any() ? (auxNeumaticos.Sum() * (negativo ? (-1) : 1)) : 0,//(negativo ? (elemento.Where(x => x.tipoEquipo == 5).Sum(x => x.monto)) * (-1) : elemento.Where(x => x.tipoEquipo == 5).Sum(x => x.monto)) : 0,
                otros = auxOtrosSuma, //auxOtros.Any() ? (auxOtros.Sum() * (negativo ? (-1) : 1)) : 0,//(negativo ? (elemento.Where(x => x.tipoEquipo == 7).Sum(x => x.monto)) * (-1) : elemento.Where(x => x.tipoEquipo == 7).Sum(x => x.monto)) : 0,
                total = auxActualSuma, //elemento.Any() ? (elemento.Sum(x => x.monto) * (negativo ? (-1) : 1)) : 0,//(negativo ? (elemento.Sum(x => x.monto)) * (-1) : elemento.Sum(x => x.monto)) : 0,
                actual = auxActualSuma, //auxActual.Any() ? (auxActual.Sum() * (negativo ? (-1) : 1)) : 0,//(negativo ? (elemento.Where(x => x.fechapol.Date <= fecha).Sum(x => x.monto)) * (-1) : elemento.Where(x => x.fechapol.Date <= fecha).Sum(x => x.monto)) : 0,
                semana2 = auxSemana2Suma, //auxSemana2.Any() ? (auxSemana2.Sum() * (negativo ? (-1) : 1)) : 0,//(negativo ? auxSemana2.Sum(x => x.monto) * (-1) : auxSemana3.Sum(x => x.monto)) : 0,
                semana3 = auxSemana3Suma, //auxSemana3.Any() ? (auxSemana3.Sum() * (negativo ? (-1) : 1)) : 0,//(negativo ? auxSemana3.Sum(x => x.monto) * (-1) :
                //elemento3.Where(x => (fecha.AddDays(-14).Date == new DateTime(2020, 2, 26) ? x.fechapol.Date <= new DateTime(2020, 2, 29) : x.fechapol.Date <= fecha.AddDays(-14))).Sum(x => x.monto)) : 0,
                semana4 = auxSemana4Suma, //auxSemana4.Any() ? (auxSemana4.Sum() * (negativo ? (-1) : 1)) : 0,//(negativo ? (elemento4.Where(x => (fecha.AddDays(-21).Date == new DateTime(2020, 2, 26) ? x.fechapol.Date <= new DateTime(2020, 2, 29) : x.fechapol.Date <= fecha.AddDays(-21))).Sum(x => x.monto)) * (-1) :
                //elemento4.Where(x => (fecha.AddDays(-21).Date == new DateTime(2020, 2, 26) ? x.fechapol.Date <= new DateTime(2020, 2, 29) : x.fechapol.Date <= fecha.AddDays(-21))).Sum(x => x.monto)) : 0,
                semana5 = auxSemana5Suma, //auxSemana5.Any() ? (auxSemana5.Sum() * (negativo ? (-1) : 1)) : 0,//(negativo ? (elemento5.Where(x => (fecha.AddDays(-28).Date == new DateTime(2020, 2, 26) ? x.fechapol.Date <= new DateTime(2020, 2, 29) : x.fechapol.Date <= fecha.AddDays(-28))).Sum(x => x.monto)) * (-1) :
                //elemento5.Where(x => (fecha.AddDays(-28).Date == new DateTime(2020, 2, 26) ? x.fechapol.Date <= new DateTime(2020, 2, 29) : x.fechapol.Date <= fecha.AddDays(-28))).Sum(x => x.monto)) : 0,
                //semana3 = negativo ? (elemento3.Where(x => x.fechapol.Date <= fecha.AddDays(-14)).Sum(x => x.monto)) * (-1) : elemento3.Where(x => x.fechapol.Date <= fecha.AddDays(-14)).Sum(x => x.monto),
                //semana4 = negativo ? (elemento4.Where(x => x.fechapol.Date <= fecha.AddDays(-21)).Sum(x => x.monto)) * (-1) : elemento4.Where(x => x.fechapol.Date <= fecha.AddDays(-21)).Sum(x => x.monto),
                //semana5 = negativo ? (elemento5.Where(x => x.fechapol.Date <= fecha.AddDays(-28)).Sum(x => x.monto)) * (-1) : elemento5.Where(x => x.fechapol.Date <= fecha.AddDays(-28)).Sum(x => x.monto),
            };

            return data;
        }

        private List<CorteDetDTO> GetTipoMovCorte(List<CortePpalDTO> elemento, int tipoMov, bool negativo, List<tblM_KBCatCuenta> cuentasDesc, int semana, int corteID)
        {
            List<CorteDetDTO> data;
            List<tblM_KBDivisionDetalle> divisiones = rentabilidadFS.getDivisionesDetalle();
            data = elemento.AsEnumerable().GroupBy(x => new { x.cc, x.cuenta, x.areaCuenta, x.tipoEquipo, x.semana, x.tipoMov, x.referencia, x.empresa }).Select(x =>
            {
                //var cuentas = x.cuenta.Split('-');
                //var tipoInsumoDesc = cuentasDesc.FirstOrDefault(y => y.cuenta == (cuentas[0] + "-" + cuentas[1] + "-0"));
                //var grupoInsumoDesc = cuentasDesc.FirstOrDefault(y => y.cuenta == x.cuenta);
                var divisionDetalle = vSesiones.sesionEmpresaActual == 1 ?
                divisiones.FirstOrDefault(y => y.ac != null && (y.ac.cc.Trim() + " " + y.ac.descripcion.Trim()) == x.Key.areaCuenta.Trim())
                : divisiones.FirstOrDefault(y => y.ac != null && (y.ac.areaCuenta.Trim() + " " + y.ac.descripcion.Trim()) == x.Key.areaCuenta.Trim());
                //var areaCuenta = x.areaCuenta.Split('-');
                return new CorteDetDTO()
                {
                    //id = x.id,
                    //noEco = x.cc,
                    //cta = Int32.Parse(cuentas[0]),
                    ////hrsTrabajadas = ,
                    //tipoInsumo = cuentas[0] + "-" + cuentas[1],
                    //tipoInsumo_Desc = tipoInsumoDesc == null ? "" : tipoInsumoDesc.descripcion,
                    //grupoInsumo = x.cuenta,
                    //grupoInsumo_Desc = grupoInsumoDesc == null ? "" : grupoInsumoDesc.descripcion,
                    ////insumo = ,
                    //areaCuenta = x.areaCuenta,
                    //insumo_Desc = x.concepto,
                    //tipo_mov = tipoMov,
                    //importe = negativo ? (x.monto * (-1)) : x.monto,
                    //fecha = x.fechapol,
                    //tipo = x.tipoEquipo,
                    //poliza = x.poliza,
                    //cc = x.cc,
                    ////area = Int32.Parse(areaCuenta[0]),
                    ////cuenta = Int32.Parse(areaCuenta[1]),
                    //semana = semana,
                    //corteID = x.corteID,
                    //referencia = x.referencia,
                    //empresa = x.empresa,
                    //division = divisionDetalle == null ? getDivisionEnkontrol(x.areaCuenta) : divisionDetalle.division.division,
                    //linea = x.linea

                    //id = x.id,
                    cc = x.Key.cc,
                    //cta = Int32.Parse(cuentas[0]),
                    //tipoInsumo = cuentas[0] + "-" + cuentas[1],
                    //tipoInsumo_Desc = tipoInsumoDesc == null ? "" : tipoInsumoDesc.descripcion,
                    cuenta = x.Key.cuenta.Contains("4000-4-") ? "5000-4-2" : x.Key.cuenta,
                    //grupoInsumo_Desc = grupoInsumoDesc == null ? "" : grupoInsumoDesc.descripcion,
                    areaCuenta = x.Key.areaCuenta,
                    //insumo_Desc = x.Key.concepto,
                    monto = x.Sum(y => y.monto),
                    //fecha = x.fechapol,
                    tipoEquipo = x.Key.tipoEquipo,
                    //poliza = x.Key.poliza,
                    semana = x.Key.semana,
                    //corteID = x.corteID,
                    referencia = x.Key.referencia,
                    empresa = x.Key.empresa,
                    division = divisionDetalle == null ? getDivisionEnkontrol(x.Key.areaCuenta) : divisionDetalle.division.division,
                    //linea = x.linea,
                    tipoMov = x.Key.tipoMov
                };
            }).ToList();
            return data;
        }

        private string getDivisionEnkontrol(string areaCuentaDesc)
        {
            var areaCuenta = areaCuentaDesc.Split(' ')[0];
            List<string> administracion = (new string[] { "003", "D01", "0", "A05", "026", "524", "979", "990" }).ToList();
            List<string> administracionArr = (new string[] { "9-30", "14-1", "14-2", "15-1", "16-1", "16-2", "16-3", "988", "994", "997" }).ToList();
            if (administracion.Contains(areaCuenta)) return "ADMINISTRACION";
            if (administracionArr.Contains(areaCuenta)) return "ADMINISTRACION ARRENDADORA";
            return "SIN DIVISION";
        }

        //public ActionResult getLstKubrixDetalleCorte(int corteID, List<int> modelos, string economico, DateTime fechaInicio, DateTime fechaFin, string cuenta, int tipo, int tipoEquipoMayor, List<string> areaCuenta)
        //{
        //    var result = new Dictionary<string, object>();
        //    try
        //    {
        //        var lst = rentabilidadFS.getLstKubrixDetalleCorte(corteID, areaCuenta, modelos, economico, fechaInicio, fechaFin, cuenta, tipo, tipoEquipoMayor);
        //        var esSuccess = lst.Count > 0;
        //        if (esSuccess)
        //        {
        //            if (cuenta.Substring(0, 1) == "4") { lst.ForEach(x => x.importe = x.importe * (-1)); }
        //            result.Add("lst", lst);
        //        }
        //        result.Add(SUCCESS, esSuccess);
        //        if (esSuccess == false)
        //        {
        //            result.Add(MESSAGE, "No se encontraron registros con los filtros seleccionados.");
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        result.Add(SUCCESS, false);
        //        result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
        //    }
        //    var json = Json(result, JsonRequestBehavior.AllowGet);
        //    json.MaxJsonLength = int.MaxValue;
        //    return json;
        //}

        public ActionResult GuardarLineaCorte(int id, int corteID, string concepto, decimal monto, string cc, string ac, DateTime fecha, string conciliacion, int empresa, int tipoGuardado)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = rentabilidadFS.GuardadoLineaCorte(id, corteID, concepto, monto, cc, ac, fecha, conciliacion, empresa, tipoGuardado);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return json;
        }

        public ActionResult EliminarLineaCorte(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = rentabilidadFS.EliminarLineaCorte(id);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return json;
        }

        public ActionResult getGrupoMaquinas()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = rentabilidadFS.getGrupoMaquinas();
                result.Add("maquinas", lst);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return json;
        }

        public ActionResult CheckCostoEstimadoCerrado(int corteID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cerrado = rentabilidadFS.CheckCostoEstimadoCerrado(corteID);
                result.Add("cerrado", cerrado);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return json;
        }

        public ActionResult CerrarCostoEst(int corteID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = rentabilidadFS.CerrarCostoEst(corteID);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return json;
        }

        #endregion

        #region Balanza

        public ActionResult CargarBalanza(int tipo, int empresa)
        {
            var result = new Dictionary<string, object>();
            try
            {
                DateTime fechaCorte = DateTime.Today.AddDays(1);
                //DateTime fechaCorte = DateTime.Today.AddMonths(-1);
                switch (tipo)
                {
                    case 0:
                        while (fechaCorte.DayOfWeek != DayOfWeek.Wednesday)
                            fechaCorte = fechaCorte.AddDays(-1);
                        break;
                    case 1:
                        fechaCorte = new DateTime(fechaCorte.Year, fechaCorte.Month, 1);
                        fechaCorte = fechaCorte.AddDays(-1);
                        break;
                }

                var balanza = rentabilidadFS.getBalanza(fechaCorte, empresa, tipo);
                result.Add(SUCCESS, true);
                result.Add("balanza", balanza);
                result.Add("fechaCorte", fechaCorte.ToString("dd/MM/yyyy"));
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        #endregion

        #region Catalógo Divisiones
        public ViewResult CatalogoDivisiones()
        {
            return View();
        }
        public ActionResult SaveOrUpdateCatalogoDivisiones(tblM_KBDivision objDivision)
        {
            return Json(rentabilidadFS.SaveOrUpdateDivision(objDivision), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInfoDivision(int areaCuenta, bool estatus)
        {
            return Json(rentabilidadFS.GetInfoDivision(0, true), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getDivisionByID(int divisionID)
        {
            return Json(rentabilidadFS.getDivisionByID(divisionID), JsonRequestBehavior.AllowGet);
        }


        public ActionResult bajaDivision(int divisionID)
        {
            return Json(rentabilidadFS.bajaDivision(divisionID), JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region Catalógo Fletes
        public ViewResult CatalogoFletes()
        {
            return View();
        }
        public ActionResult SaveOrUpdateFlete(tblM_KBFletes nuevo)
        {
            return Json(rentabilidadFS.SaveOrUpdateFlete(nuevo), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInfoFletes()
        {
            return Json(rentabilidadFS.GetInfoFletes(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult CboEconomico()
        {
            return Json(rentabilidadFS.CboEconomico(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Administacion centro de costos por usuario.
        public ViewResult CatalogoAdministracionUsuarios()
        {
            return View();
        }
        public ActionResult SaveOrUpdateAdministacionUsuarios(tblM_KBUsuarioResponsable nuevoUsuario)
        {
            return Json(rentabilidadFS.SaveOrUpdateAdministacionUsuarios(nuevoUsuario), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInfoAdministracionUsuarios(int estatus)
        {
            return Json(rentabilidadFS.GetInfoAdministracionUsuarios(estatus), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getReponsableByID(int id)
        {
            return Json(rentabilidadFS.getReponsableByID(id), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getListaUsuarios()
        {
            return Json(rentabilidadFS.getListaUsuarios(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult bajaUsuario(int id)
        {
            return Json(rentabilidadFS.bajaUsuario(id), JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Reportes autogenerados

        public MemoryStream CrearExcelSaldosCplanKubrix(int corteID)
        {
            var corte = rentabilidadFS.getCorteByID(corteID);
            var stream = rentabilidadFS.crearExcelSaldosCplan(corte.fechaCorte);

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "saldos.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                return stream;
            }
            else
            {
                return null;
            }
        }

        public MemoryStream CrearExcelSaldosColombiaKubrix(int corteID)
        {
            var corte = rentabilidadFS.getCorteByID(corteID);
            var stream = rentabilidadFS.crearExcelSaldosColombia(corte.fechaCorte);

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "saldoscolombia.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                return stream;
            }
            else
            {
                return null;
            }
        }

        public MemoryStream CrearExcelSaldosCplanEiciKubrix(int corteID)
        {
            var corte = rentabilidadFS.getCorteByID(corteID);
            var stream = rentabilidadFS.crearExcelSaldosCplanEici(corte.fechaCorte);

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "saldosconstruplaneici.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                return stream;
            }
            else
            {
                return null;
            }
        }

        public MemoryStream CrearExcelClientesCplanKubrix(int corteID)
        {
            var corte = rentabilidadFS.getCorteByID(corteID);
            var stream = rentabilidadFS.crearExcelClientesCplan(corte.fechaCorte, 0);

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "clientes.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                return stream;
            }
            else
            {
                return null;
            }
        }

        public MemoryStream CrearExcelClientesCplanEiciKubrix(int corteID)
        {
            var corte = rentabilidadFS.getCorteByID(corteID);
            var stream = rentabilidadFS.crearExcelClientesCplan(corte.fechaCorte, 1);

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "clientes.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                return stream;
            }
            else
            {
                return null;
            }
        }

        public MemoryStream CrearExcelVencimientosCplanKubrix(int corteID)
        {
            var corte = rentabilidadFS.getCorteByID(corteID);
            var stream = rentabilidadFS.crearExcelVencimientosCplan(corte.fechaCorte);

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "vencimientos.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                return stream;
            }
            else
            {
                return null;
            }
        }
        public MemoryStream CrearExcelSaldosCplanIntegradoraKubrix(int corteID)
        {
            var corte = rentabilidadFS.getCorteByID(corteID);
            var stream = rentabilidadFS.crearExcelSaldosCplanIntegradora(corte.fechaCorte);

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "vencimientos.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                return stream;
            }
            else
            {
                return null;
            }
        }


        public MemoryStream crearExcelSemanalKubrix(int corteID, int tipo)
        {
            var lstAreaCuenta = new List<string>();
            var stream = rentabilidadFS.crearExcelSemanalKubrix(corteID, tipo, lstAreaCuenta);

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Reporte_Kubrix_Semanal.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                return stream;
            }
            else
            {
                return null;
            }
        }

        public MemoryStream crearExcelPorSubcuentaKubrix(int corteID, int tipo)
        {
            var lstAreaCuenta = new List<string>();
            var stream = rentabilidadFS.crearExcelPorSubcuentaKubrix(corteID, tipo, lstAreaCuenta);

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Reporte_Kubrix_Subcuenta.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                return stream;
            }
            else
            {
                return null;
            }
        }

        #endregion


        public ActionResult cboCentroCostosUsuarios(int divisionID = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioID = getUsuario().id;

                var listaCCusuario = usuariosFS.getCCsByUsuario(usuarioID).OrderBy(x => x.area).ThenBy(x => x.cuenta).ToList();

                List<string> ACDivision = new List<string>();
                if (divisionID != -1)
                {
                    ACDivision = rentabilidadFS.getACDivision(divisionID);
                    listaCCusuario = listaCCusuario.Where(x => ACDivision.Contains(x.areaCuenta)).ToList();
                }

                var listaCentroCostosActuales = centroCostosFS.getListaCC();
                if (base.getAction("AllCC"))
                {

                    result.Add(ITEMS, listaCentroCostosActuales);
                }
                else
                {
                    List<Core.DTO.Principal.Generales.ComboDTO> Resultado = new List<Core.DTO.Principal.Generales.ComboDTO>();
                    if (Core.DTO.vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual==3 || vSesiones.sesionEmpresaActual==6)
                    {
                        Resultado = listaCCusuario.Select(y => new Core.DTO.Principal.Generales.ComboDTO
                        {
                            Value = y.cc,
                            Text = y.cc + " - " + y.descripcion,
                            Prefijo = y.abreviacion

                        }).ToList();
                    }
                    else
                    {
                        Resultado = listaCCusuario.Select(y => new Core.DTO.Principal.Generales.ComboDTO
                        {
                            Value = y.areaCuenta,
                            Text = y.areaCuenta + " - " + y.descripcion,
                            Prefijo = y.abreviacion

                        }).ToList();
                    }
                    result.Add(ITEMS, Resultado);
                }

                //result.Add(ITEMS, maquinaFactoryServices.getMaquinaServices().getCboMaquinaria(obj).Select(x => new { Value = x.noEconomico, Text = x.noEconomico }).OrderBy(x => x.Text));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region subirExcelEstimados
        [HttpPost]
        public ActionResult GuardarEstimados()
        {
            var result = new Dictionary<string, object>();
            try
            {
                byte[] data;
                int exito = 0;
                HttpPostedFileBase file = Request.Files["archivoEstimados"];
                int corteID = Int32.Parse(Request.Form["corteID"]);

                if (file != null && file.ContentLength > 0)
                {
                    using (Stream inputStream = file.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        data = memoryStream.ToArray();
                    }
                    exito = rentabilidadFS.CargarExcelEstimados(data, corteID);
                }
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GuardarCorteFlujo()
        {
            var result = new Dictionary<string, object>();
            try
            {
                int corteID = 0;
                var usuario = getUsuario().id;
                if (vSesiones.sesionEmpresaActual == 1)
                {
                    corteID = rentabilidadFS.guardarCorteConstruplan(usuario, 10);
                }
                else if (vSesiones.sesionEmpresaActual == 2)
                {
                    corteID = rentabilidadFS.guardarCorteArrendadora(usuario, 10);
                }
                result.Add("corteID", corteID);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            JsonResult json = new JsonResult();
            json.MaxJsonLength = Int32.MaxValue;
            json.Data = result;
            return json;
        }

        #endregion




        public ActionResult obtenerDapper(int corteID, DateTime fechaFin)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = rentabilidadFS.obtenerCortesArrendadora(corteID, fechaFin);
                result.Add("exito", exito);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return json;
        }
        #region Stored Procedures Kubrix
        public JsonResult getLstKubrixCorteArrendadora(int corteID, List<int> modelos, string economico, List<string> areaCuenta, int acumulado, int tipoCorte = 0, bool reporteCostos = false)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                var corte = rentabilidadFS.getCorteByID(corteID);
                DateTime fechaFin = corte.fechaCorte;
                var data = rentabilidadFS.getLstKubrixCortesArrendadora(corteID, areaCuenta, modelos, economico, fechaFin, reporteCostos, acumulado);
                result.Add("lst", data);
                result.Add("fecha", fechaFin);
                result.Add("usuarioID", getUsuario().id);
                result.Add(SUCCESS, true);
            }
            catch (Exception e) 
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = Int32.MaxValue;
            return json;
        }

        public JsonResult getLstKubrixCorteArrendadoraDetalle(
            int corteID, List<int> modelos, string economico, List<string> areaCuenta, int acumulado, int tipoCorte, bool reporteCostos, 
            int tipoTabla, int columna, int renglon, string divisionTabla, string areaCuentaTabla, string economicoTabla
        )
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                //--> Cargar corte
                var corte = rentabilidadFS.getCorteByID(corteID);
                var semana = 1;
                int concepto = 0;
                bool aplicaSinDivision = false;
                List<string> areasCuentaEnDivision = new List<string>();
                //--> Si la tabla a cargar es de tipo economico el concepto es la columna
                if (tipoTabla == 5 || tipoTabla == 6) concepto = columna;
                else concepto = renglon;
                //--> Si la tabla a cargar es semanal, el corte referenciado cambia
                if (tipoTabla == 3) { 
                    semana = columna;
                    if (semana > 1)
                    {
                        var corteAnterior = rentabilidadFS.getCortesAnt(corte.fechaCorte, corte.tipo).OrderByDescending(x => x.fechaCorte).Skip(semana - 2).FirstOrDefault();
                        if (corteAnterior != null) { corte = corteAnterior; }
                    }
                }

                DateTime fechaFin = corte.fechaCorte;

                if (areaCuentaTabla != null && areaCuentaTabla != "" && areaCuentaTabla != divisionTabla) 
                {
                    areaCuenta = new List<string>();
                    areaCuenta.Add(areaCuentaTabla.Split(' ')[0]);
                }

                else if (divisionTabla != null && divisionTabla != "")
                {
                    var auxDivision = rentabilidadFS.getDivisionByNombre(divisionTabla);
                    if (auxDivision != null) { areasCuentaEnDivision = auxDivision.divisionDetalle.Where(x => x.estatus).Select(x => x.ac).Select(x => x.areaCuenta).ToList(); }
                    else if(divisionTabla == "SIN DIVISION") { aplicaSinDivision = true; }
                    
                    if (areaCuenta == null || areaCuenta.Count() == 0) { areaCuenta = areasCuentaEnDivision; }
                    else {
                        if (areasCuentaEnDivision != null && areasCuentaEnDivision.Count() > 0) areaCuenta = areaCuenta.Where(x => areasCuentaEnDivision.Contains(x)).ToList(); 
                    }
                }

                if (economicoTabla != null && economicoTabla != "") { economico = economicoTabla; }

                var data = rentabilidadFS.getLstKubrixCortesArrendadoraDetalle(corte.id, areaCuenta, modelos, economico, fechaFin, reporteCostos, acumulado, concepto, semana, aplicaSinDivision, tipoTabla == 6 ? true : false);
                result.Add("lst", data);
                result.Add("fecha", fechaFin);
                result.Add("usuarioID", getUsuario().id);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = Int32.MaxValue;
            return json;
        }

        public JsonResult getLstKubrixCorteConstruplan(int corteID, List<int> modelos, string economico, List<string> areaCuenta, int acumulado, int tipoCorte = 0, bool reporteCostos = false)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                var corte = rentabilidadFS.getCorteByID(corteID);
                DateTime fechaFin = corte.fechaCorte;
                var data = rentabilidadFS.getLstKubrixCortesConstruplan(corteID, areaCuenta, modelos, economico, fechaFin, reporteCostos, acumulado);

                List<tblM_KBDivisionDetalle> divisiones = rentabilidadFS.getDivisionesDetalle();
                List<string> lstDivisiones = new List<string>();
                lstDivisiones.Add("ADMINISTRACION");
                lstDivisiones.Add("ADMINISTRACION ARRENDADORA");
                lstDivisiones.Add("SIN DIVISION");
                lstDivisiones.Add("FLETES");
                var lstDevuelta = data.Where(r => !lstDivisiones.Contains(r.division)).ToList();

                var ultimoCorteArrend = rentabilidadFS.getCorteByIDArrendadora();
                //var obtenerLstArrendadora = rentabilidadFS.getLstKubrixCortesArrendadoraCentrosCostos(ultimoCorteArrend.id, areaCuenta, modelos, economico, fechaFin, reporteCostos, acumulado);
                var lstCC = rentabilidadFS.obtenerCentrosCostos();
                //var formato = obtenerLstArrendadora.AsEnumerable().Select(x =>
                //{
                //    var divisionDetalle = divisiones.FirstOrDefault(y => y.ac != null && (y.ac.areaCuenta.ToUpper().Trim()) == x.areaCuenta.ToUpper().Trim());
                //    return new
                //    {
                //        centroCostos = lstCC.Where(r => r.areaCuenta == x.areaCuenta).FirstOrDefault() == null ? "SINCC" : lstCC.Where(r => r.areaCuenta == x.areaCuenta).Select(y => y.cc + " " + y.descripcion).FirstOrDefault().Trim(),
                //        areaCuenta = x.areaCuenta,
                //        monto = x.montoActual,
                //        semana = x.semana,
                //        division = divisionDetalle == null ? getDivision(x.areaCuenta) : divisionDetalle.division.division.Trim(),
                //        cuenta = x.cuenta,
                //        tipoMov = 0,
                //    };
                //}).ToList();

                //var lstFinal = formato.Where(r => !lstDivisiones.Contains(r.division)).ToList();
                //result.Add("lstDetalle", obtenerLstArrendadora);

                result.Add("lst", lstDevuelta);
                result.Add("fecha", fechaFin);
                result.Add("usuarioID", getUsuario().id);

                BusqKubrixDTO busq = new BusqKubrixDTO();
                busq.fechaFin = fechaFin;
                //var CXP = rentabilidadFS.getLstCXP(busq);
                //result.Add("CXP", CXP);
                //var CXC = rentabilidadFS.getLstCXC(busq);
                //result.Add("CXC", CXC);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = Int32.MaxValue;
            return json;
        }

        public JsonResult getCXC(DateTime fecha, List<string> areaCuenta)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                BusqKubrixDTO busq = new BusqKubrixDTO();
                busq.fechaFin = fecha;
                busq.ccEnkontrol = areaCuenta;
                var CXC = rentabilidadFS.getLstCXC(busq);
                result.Add("CXC", CXC);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = Int32.MaxValue;
            return json;
        }

        #endregion
        public ActionResult fillComboCCConstruplan(int divisionID = -1, int responsableID = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioID = getUsuario().id;
                var responsable = rentabilidadFS.checkResponsable(-1, usuarioID);
                var cbo = new List<Core.DTO.Principal.Generales.ComboDTO>();
                if (divisionID == -1 && responsableID == -1 && !responsable) cbo.Add(new Core.DTO.Principal.Generales.ComboDTO { Value = "S/A", Text = "SIN AREA CUENTA", Prefijo = "-1" });
                var auxCbo = rentabilidadFS.getListaCCConstruplan(usuarioID).ToList();
                var detallesDiv = rentabilidadFS.getACDivision(divisionID);
                var detallesResp = rentabilidadFS.getACResponsable(responsableID);
                if (divisionID != -1) auxCbo = auxCbo.Where(x => detallesDiv.Contains(x.Value)).ToList();
                if (responsableID != -1) auxCbo = auxCbo.Where(x => detallesResp.Contains(x.Value)).ToList();
                cbo.AddRange(auxCbo);

                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #region Nuevas Funciones para Kubrix Cplan 

        public ActionResult fillComboFechasConstruplan(int tipoCorte)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = new List<Core.DTO.Principal.Generales.ComboDTO>();
                cbo = rentabilidadFS.getListaFechasConstruplan(tipoCorte);
                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cargarInformacionNivel1(int corteID, List<string> listaCC)
        {
            var usuarioID = getUsuario().id;
            var json = Json(rentabilidadFS.cargarInformacionNivel1(corteID, listaCC, usuarioID), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = Int32.MaxValue;
            return json;
        }

        #endregion

    }
}