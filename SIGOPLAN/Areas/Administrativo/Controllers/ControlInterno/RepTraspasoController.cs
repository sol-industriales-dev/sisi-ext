using Core.DTO;
using Core.DTO.Administracion.ControlInterno;
using Core.DTO.Almacen;
using Core.DTO.Principal.Generales;
using Core.Enum.Multiempresa;
using Core.Service.Administracion.ControlInterno.Reporte;
using Data.Factory.Administracion.ControlInterno.Almacen;
using Data.Factory.Administracion.ControlInterno.Reporte;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.ControlInterno
{
    public class RepTraspasoController : BaseController
    {
        InsumoFactoryServices insumoFactoryServices = new InsumoFactoryServices();
        RepTraspasoFactoryServices RepTraspasoFactoryService = new RepTraspasoFactoryServices();

        // GET: Administrativo/RepTraspaso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ConsultaInsumos()
        {
            return View();
        }

        public ActionResult ConsultaInsumosPeru()
        {
            return View();
        }

        public ActionResult getInsumos(string term, int TipoInsumo, int GrupoInsumo, int Sistema)
        {
            var items = insumoFactoryServices.getRepTraspasoServices().getInsumo(term, TipoInsumo, GrupoInsumo, Sistema);

            var filteredItems = items.Select(x => new { id = x.insumo, label = x.descripcion });

            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }


        //GetListaInsumos
        public ActionResult GetListaInsumosBusqueda(string term, int TipoInsumo, int GrupoInsumo, int Sistema)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var dataSet = insumoFactoryServices.getRepTraspasoServices().getListaInsumos(term, TipoInsumo, GrupoInsumo, Sistema);
                result.Add("dataSet", dataSet.Select(x => new
                {
                    insumo = "<span data-insumo='" + x.insumo + "' data-descripcion='" + x.descripcion + "' class='clsInsumo' style='color:blue;cursor:pointer;'>" + x.insumo + "</span>",
                    descripcion = x.descripcion
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
        public ActionResult GetListaInsumos(string term, int TipoInsumo, int GrupoInsumo, int Sistema)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var dataSet = insumoFactoryServices.getRepTraspasoServices().getListaInsumos(term, TipoInsumo, GrupoInsumo, Sistema);
                result.Add("dataSet", dataSet.Select(x => new
                {
                    insumo = x.insumo,
                    descripcion = x.descripcion

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

        public ActionResult fillCboGrupoInsumos(int tipo, int sistema)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbodata = insumoFactoryServices.getRepTraspasoServices().fillGrupoInsumos(tipo, sistema);
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

        public ActionResult fillCboTipoInsumos(int sistema)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbodata = insumoFactoryServices.getRepTraspasoServices().fillTipoInsumos(sistema);
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

        public ActionResult CargarInsumo(int insumo, int sistema)
        {
            var result = new Dictionary<string, object>();
            try
            {
                int anio = DateTime.Now.Year;

                var Data = insumoFactoryServices.getRepTraspasoServices().getInsumo(insumo, anio, sistema);

                var DataConstruplan = Data.Where(x => x.locacion == 1).ToList();
                var DataArrendadora = Data.Where(x => x.locacion == 2).ToList();
                var GrupoConstruplan = Data.Where(x => x.locacion == 1).GroupBy(x => new { x.almacen }).ToList();
                var GrupoArrendadora = Data.Where(x => x.locacion == 2).GroupBy(x => new { x.almacen }).ToList();

                result.Add("TotalConstruplan", DataConstruplan.Sum(x => x.cantidadInsumos));
                result.Add("TotalArrendadora", DataArrendadora.Sum(x => x.cantidadInsumos));

                List<ComboDTO> dataResultConstruplan = new List<ComboDTO>();
                List<ComboDTO> dataResultArrendadora = new List<ComboDTO>();

                foreach (var item in GrupoConstruplan)
                {
                    ComboDTO dto = new ComboDTO();
                    dto.Value = DataConstruplan.Where(x => x.locacion == 1 && x.almacen == item.Key.almacen).Sum(r => r.cantidadInsumos).ToString();
                    dto.Text = item.Key.almacen.PadLeft(3, '0');

                    if (dto.Value != "0")
                    {
                        dataResultConstruplan.Add(dto);
                    }

                }

                foreach (var item in GrupoArrendadora)
                {
                    ComboDTO dto = new ComboDTO();
                    dto.Value = DataArrendadora.Where(x => x.locacion == 2 && x.almacen == item.Key.almacen).Sum(r => r.cantidadInsumos).ToString();
                    dto.Text = item.Key.almacen.PadLeft(3, '0');

                    if (dto.Value != "0")
                    {
                        dataResultArrendadora.Add(dto);
                    }
                }

                Session["rptInsumosArrendadora"] = dataResultArrendadora.Select(x => new rptInsumosDTO
                {
                    almacen = x.Text,
                    cantidad = Convert.ToDecimal(x.Value)

                }).ToList();
                Session["rptInsumosConstruplan"] = dataResultConstruplan.Select(x => new rptInsumosDTO
                {
                    almacen = x.Text,
                    cantidad = Convert.ToDecimal(x.Value)

                }).ToList();

                result.Add("dataArrendadora", dataResultArrendadora.Select(x => new
                {
                    almacen = x.Text,
                    cantidadInsumos = x.Value

                }));
                result.Add("dataConstruplan", dataResultConstruplan.Select(x => new
                {
                    almacen = x.Text,
                    cantidadInsumos = x.Value

                }));

                result.Add("tituloConstruplan", DataConstruplan.Count > 0 ? DataConstruplan.FirstOrDefault().desInsumoConstruplan : String.Empty);
                result.Add("tituloArrendadora", DataArrendadora.Count > 0 ? DataArrendadora.FirstOrDefault().desInsumoArrendadora : String.Empty);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, "Ocurrió un error al buscar el insumo.");
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CargarInsumoMultiple(List<int> insumos, int sistema)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var dataResultConstruplan = new List<InsumoDTO>();
                var dataResultArrendadora = new List<InsumoDTO>();

                var Data = insumoFactoryServices.getRepTraspasoServices().getInsumoMultiple(insumos, 0, sistema, "");

                var DataConstruplan = Data.Where(x => x.locacion == 1).ToList();
                var DataArrendadora = Data.Where(x => x.locacion == 2).ToList();
                var GrupoConstruplan = DataConstruplan.GroupBy(x => new { x.numInsumoConstruplan, x.almacen }).ToList();
                var GrupoArrendadora = DataArrendadora.GroupBy(x => new { x.numInsumoArrendadora, x.almacen }).ToList();

                foreach (var item in GrupoConstruplan)
                {
                    InsumoDTO dto = new InsumoDTO();

                    if ((EmpresaEnum)vSesiones.sesionEmpresaActual == EmpresaEnum.Peru)
                    {
                        dto.insumoNumero = item.First().numInsumoConstruplan.ToString().PadLeft(11, '0');
                        dto.almacenNumero = item.First().almacen.PadLeft(2, '0');
                    }
                    else
                    {
                        dto.insumoNumero = item.First().numInsumoConstruplan.ToString();
                        dto.almacenNumero = item.First().almacen.PadLeft(3, '0');
                    }
                    
                    dto.insumoDescripcion = item.First().desInsumoConstruplan;
                    dto.insumoCantidad = item.Sum(x => x.cantidadInsumos);
                    dto.almacenNombre = dto.almacenNumero + " - " + item.First().almacenNombre;

                    if (dto.insumoCantidad > 0)
                    {
                        dataResultConstruplan.Add(dto);
                    }
                }

                foreach (var item in GrupoArrendadora)
                {
                    InsumoDTO dto = new InsumoDTO();
                    dto.insumoNumero = item.First().numInsumoArrendadora.ToString();
                    dto.insumoDescripcion = item.First().desInsumoArrendadora;
                    dto.insumoCantidad = item.Sum(x => x.cantidadInsumos);
                    dto.almacenNumero = item.First().almacen.PadLeft(3, '0');
                    dto.almacenNombre = dto.almacenNumero + " - " + item.First().almacenNombre;
                    if (dto.insumoCantidad > 0)
                    {
                        dataResultArrendadora.Add(dto);
                    }
                }

                Session["rptInsumosConstruplan"] = dataResultConstruplan.Select(x => new rptInsumosDTO
                {
                    insumo = x.insumoNumero,
                    descripcion = x.insumoDescripcion,
                    cantidad = x.insumoCantidad,
                    almacen = x.almacenNombre
                }).ToList();
                Session["rptInsumosArrendadora"] = dataResultArrendadora.Select(x => new rptInsumosDTO
                {
                    insumo = x.insumoNumero,
                    descripcion = x.insumoDescripcion,
                    cantidad = x.insumoCantidad,
                    almacen = x.almacenNombre
                }).ToList();
                result.Add("dataConstruplan", dataResultConstruplan);
                result.Add("dataArrendadora", dataResultArrendadora);


                result.Add("TotalConstruplan", dataResultConstruplan.Sum(x => x.insumoCantidad));
                result.Add("TotalArrendadora", dataResultArrendadora.Sum(x => x.insumoCantidad));

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult fnLoadTable(string cc, string folio, string almacen, DateTime fechaIni, DateTime fechaFin)
        {
            var result = new Dictionary<string, object>();
            var abierto = RepTraspasoFactoryService.getRepTraspasoServices().getLstMovAbiertos(cc, folio, almacen, fechaIni, fechaFin);
            var cerrado = RepTraspasoFactoryService.getRepTraspasoServices().getLstMovCerrados(cc, folio, almacen, fechaIni, fechaFin);

            if (abierto.Count.Equals(0))
            {
                result.Add("abiertoSUCCESS", false);
            }
            else
            {
                result.Add("abierto", abierto);
                result.Add("abiertoSUCCESS", true);
            }
            
            if (cerrado.Count.Equals(0))
            {
                result.Add("cerradoSUCCESS", false);
            }
            else
            {
                result.Add("cerrado", cerrado);
                result.Add("cerradoSUCCESS", true);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}