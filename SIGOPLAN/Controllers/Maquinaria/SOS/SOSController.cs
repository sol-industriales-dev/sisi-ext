using Core.DTO.Maquinaria.SOS;
using Core.Entity.Maquinaria.SOS;
using Core.Enum.Maquinaria;
using Data.Factory.Maquinaria.SOS;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.SOS
{
    public class SOSController : BaseController
    {
        #region Atributos
        private readonly string SUCCESS = "success";
        private readonly string MESSAGE = "message";
        private const string PAGE = "page";
        private const string TOTAL_PAGE = "total";
        private const string ROWS = "rows";
        private const string ITEMS = "items";
        #endregion

        #region Factory
        MinadoFactoryServices minadoFactoryServices;
        #endregion
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            minadoFactoryServices = new MinadoFactoryServices();
            base.OnActionExecuting(filterContext);
        }

        // GET: SOS
        public ActionResult Index(HttpPostedFileBase files1)
        {
            string fName = "";
            var result = new Dictionary<string, object>();
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];

                    BinaryReader b = new BinaryReader(file.InputStream);
                    byte[] binData = b.ReadBytes(file.ContentLength);
                    //Save file content goes here
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        // "C://Proyectos//Iconos SIGOPLAN//Minado la Colorada.csv"
                        var entidad = (List<MinadoEntity>)CSVtoEntity.ConvertCSVTABtoDataTable(binData).ToObject<IList<MinadoEntity>>();
                        minadoFactoryServices.getMinadoService().Guardar(entidad);

                        result.Add(MESSAGE, "El archvivo se importó correctamente");
                        result.Add(SUCCESS, true);
                    }
                }

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult CargarArchivos()
        {
            return View();
        }
        public ActionResult ReporteMuestras()
        {
            return View();
        }

        public ActionResult cboLugares()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, minadoFactoryServices.getMinadoService().cboFiltroLugar().Select(x => new { Value = x.folio, Text = x.descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboComponente()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, minadoFactoryServices.getMinadoService().cboComponente().Select(x => new { Value = x.folio, Text = x.descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboModelo(string lugar)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, minadoFactoryServices.getMinadoService().cboFiltroModelo(lugar).Select(x => new { Value = x.folio, Text = x.descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboMaquina(string lugar)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, minadoFactoryServices.getMinadoService().cboFiltroMaquinaria(lugar).Select(x => new { Value = x.descripcion, Text = x.economico }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cboMaquinaMultiple(List<string> lugar)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, minadoFactoryServices.getMinadoService().cboFiltroMaquinariaXlista(lugar).Select(x => new { Value = x.descripcion, Text = x.economico }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult cargarGraficaReporteMuestras(List<string> Lugar, string Componente, string Unitid, string Modelo, string Elemento, string FechaInicio, string FechaFin)
        {
            CultureInfo c = new CultureInfo("en-us", true);
            var result = new Dictionary<string, object>();
            try
            {
                DateTime fechaInicio = Convert.ToDateTime(FechaInicio);
                DateTime fechaFin = Convert.ToDateTime(FechaFin);


                List<MuestrasElementosDTO> resultado = minadoFactoryServices.getMinadoService().detalleCompletoLista(Lugar, Componente, Unitid, Modelo, Elemento, fechaInicio, fechaFin);
                if (Elemento != "TODO")
                {
                    var res = minadoFactoryServices.getMinadoService().detallesMuestras(resultado, Elemento);
                    result.Add("grafica", res);
                    var table = resultado.Select(x => new
                                                {
                                                    Equipo = x.name,
                                                    AL = x.al,
                                                    cautionAL = 3.5,
                                                    alertaAL = 7,
                                                    CU = x.cu,
                                                    cautionCU = 7.5,
                                                    Descripcion = x.description,
                                                    alertaCU = 11,
                                                    FE = x.fe,
                                                    cautionFE = 25,
                                                    alertaFE = 40,
                                                    SI = x.si,
                                                    cautionSI = 4,
                                                    alertaSI = 10,
                                                    fecha = x.fecha.ToShortDateString()
                                                });
                    result.Add("tabla", table);
                }
                else
                {
                    var res = resultado.OrderBy(x => x.name).Select(x => new { al = x.al, cu = x.cu, si = x.si, fe = x.fe, name = x.name, fecha = x.fecha.ToShortDateString(), description = x.description });
                    var table = resultado.OrderBy(x => x.name).Select(x => new { al = x.al, cu = x.cu, si = x.si, fe = x.fe, Equipo = x.name, fecha = x.fecha.ToShortDateString(), description = x.description });
                    result.Add("grafica", res);
                    result.Add("tabla", table);

                }
                //  result.Add("grafica", resultado);

                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ReporteMuestrasDetalle()
        {
            return View();
        }
        public ActionResult cargarGraficaMuestras(List<string> Lugar, string FechaInicio, string FechaFin)
        {
            CultureInfo c = new CultureInfo("en-us", true);
            var result = new Dictionary<string, object>();
            try
            {
                DateTime fechaInicio = Convert.ToDateTime(FechaInicio);
                DateTime fechaFin = Convert.ToDateTime(FechaFin);
                //  var res = minadoFactoryServices.getMinadoService().muestrasGenerales(Lugar, fechaInicio, fechaFin);

                var res = minadoFactoryServices.getMinadoService().muestrasGeneralesLists(Lugar, fechaInicio, fechaFin);

                result.Add(SUCCESS, true);
                result.Add("grafica", res);


            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }
        public ActionResult cargarDetalleMuestraGeneral(List<string> Lugar, string FechaInicio, string FechaFin, string TipoAlerta)
        {
            CultureInfo c = new CultureInfo("en-us", true);
            int cont = 0;
            var result = new Dictionary<string, object>();

            var al = new List<indicaroresMuestraGeneralDTO>();
            var cu = new List<indicaroresMuestraGeneralDTO>();
            var fe = new List<indicaroresMuestraGeneralDTO>();
            var si = new List<indicaroresMuestraGeneralDTO>();
            try
            {
                DateTime fechaInicio = Convert.ToDateTime(FechaInicio);
                DateTime fechaFin = Convert.ToDateTime(FechaFin);
                //   var lista = minadoFactoryServices.getMinadoService().detalleGeneralMuestras(Lugar, fechaInicio, fechaFin, TipoAlerta);

                var lista = minadoFactoryServices.getMinadoService().detalleGeneralMuestrasList(Lugar, fechaInicio, fechaFin, TipoAlerta);


                if (TipoAlerta == "normal")
                {
                    al = lista.Where(x => Convert.ToInt16(x.al) < 3.5 && !x.al.Equals("0")).GroupBy(x => x.name).Select(x =>
                            new indicaroresMuestraGeneralDTO
                            {
                                id = cont++,
                                elemento = x.Sum(y => Convert.ToInt32(y.al)).ToString(),
                                maquina = x.First().name,
                                children = x.GroupBy(y => y.description).Select(y =>
                                    new indicaroresMuestraGeneralDTO
                                    {
                                        id = cont++,
                                        elemento = y.Sum(z => Convert.ToInt32(z.al)).ToString(),
                                        maquina = y.First().description,
                                        children = new List<indicaroresMuestraGeneralDTO>()
                                    }).ToList()
                            }).ToList();
                    //-----------------------------------------------------------------------------------------------------------------
                    cu = lista.Where(x => Convert.ToInt16(x.cu) < 7.5 && !x.cu.Equals("0")).GroupBy(x => x.name).Select(x =>
                            new indicaroresMuestraGeneralDTO
                            {
                                id = cont++,
                                elemento = x.Sum(y => Convert.ToInt32(y.cu)).ToString(),
                                maquina = x.First().name,
                                children = x.GroupBy(y => y.description).Select(y =>
                                    new indicaroresMuestraGeneralDTO
                                    {
                                        id = cont++,
                                        elemento = y.Sum(z => Convert.ToInt32(z.cu)).ToString(),
                                        maquina = y.First().description,
                                        children = new List<indicaroresMuestraGeneralDTO>()
                                    }).ToList()
                            }).ToList();
                    //-----------------------------------------------------------------------------------------------------------------
                    fe = lista.Where(x => Convert.ToInt16(x.fe) < 25 && !x.fe.Equals("0")).GroupBy(x => x.name).Select(x =>
                            new indicaroresMuestraGeneralDTO
                            {
                                id = cont++,
                                elemento = x.Sum(y => Convert.ToInt32(y.fe)).ToString(),
                                maquina = x.First().name,
                                children = x.GroupBy(y => y.description).Select(y =>
                                    new indicaroresMuestraGeneralDTO
                                    {
                                        id = cont++,
                                        elemento = y.Sum(z => Convert.ToInt32(z.fe)).ToString(),
                                        maquina = y.First().description,
                                        children = new List<indicaroresMuestraGeneralDTO>()
                                    }).ToList()
                            }).ToList();
                    //-----------------------------------------------------------------------------------------------------------------
                    si = lista.Where(x => Convert.ToInt16(x.si) < 4 && !x.si.Equals("0")).GroupBy(x => x.name).Select(x =>
                            new indicaroresMuestraGeneralDTO
                            {
                                id = cont++,
                                elemento = x.Sum(y => Convert.ToInt32(y.si)).ToString(),
                                maquina = x.First().name,
                                children = x.GroupBy(y => y.description).Select(y =>
                                    new indicaroresMuestraGeneralDTO
                                    {
                                        id = cont++,
                                        elemento = y.Sum(z => Convert.ToInt32(z.si)).ToString(),
                                        maquina = y.First().description,
                                        children = new List<indicaroresMuestraGeneralDTO>()
                                    }).ToList()
                            }).ToList();
                }
                if (TipoAlerta == "alerta")
                {
                    al = lista.Where(x => Convert.ToInt16(x.al) >= 7 && !x.al.Equals("0")).GroupBy(x => x.name).Select(x =>
                            new indicaroresMuestraGeneralDTO
                            {
                                id = cont++,
                                elemento = x.Sum(y => Convert.ToInt32(y.al)).ToString(),
                                maquina = x.First().name,
                                children = x.GroupBy(y => y.description).Select(y =>
                                    new indicaroresMuestraGeneralDTO
                                    {
                                        id = cont++,
                                        elemento = y.Sum(z => Convert.ToInt32(z.al)).ToString(),
                                        maquina = y.First().description,
                                        children = new List<indicaroresMuestraGeneralDTO>()
                                    }).ToList()
                            }).ToList();
                    //-----------------------------------------------------------------------------------------------------------------
                    cu = lista.Where(x => Convert.ToInt16(x.cu) >= 11 && !x.cu.Equals("0")).GroupBy(x => x.name).Select(x =>
                            new indicaroresMuestraGeneralDTO
                            {
                                id = cont++,
                                elemento = x.Sum(y => Convert.ToInt32(y.cu)).ToString(),
                                maquina = x.First().name,
                                children = x.GroupBy(y => y.description).Select(y =>
                                    new indicaroresMuestraGeneralDTO
                                    {
                                        id = cont++,
                                        elemento = y.Sum(z => Convert.ToInt32(z.cu)).ToString(),
                                        maquina = y.First().description,
                                        children = new List<indicaroresMuestraGeneralDTO>()
                                    }).ToList()
                            }).ToList();
                    //-----------------------------------------------------------------------------------------------------------------
                    fe = lista.Where(x => Convert.ToInt16(x.fe) >= 40 && !x.fe.Equals("0")).GroupBy(x => x.name).Select(x =>
                            new indicaroresMuestraGeneralDTO
                            {
                                id = cont++,
                                elemento = x.Sum(y => Convert.ToInt32(y.fe)).ToString(),
                                maquina = x.First().name,
                                children = x.GroupBy(y => y.description).Select(y =>
                                    new indicaroresMuestraGeneralDTO
                                    {
                                        id = cont++,
                                        elemento = y.Sum(z => Convert.ToInt32(z.fe)).ToString(),
                                        maquina = y.First().description,
                                        children = new List<indicaroresMuestraGeneralDTO>()
                                    }).ToList()
                            }).ToList();
                    //-----------------------------------------------------------------------------------------------------------------
                    si = lista.Where(x => Convert.ToInt16(x.si) >= 10 && !x.si.Equals("0")).GroupBy(x => x.name).Select(x =>
                            new indicaroresMuestraGeneralDTO
                            {
                                id = cont++,
                                elemento = x.Sum(y => Convert.ToInt32(y.si)).ToString(),
                                maquina = x.First().name,
                                children = x.GroupBy(y => y.description).Select(y =>
                                    new indicaroresMuestraGeneralDTO
                                    {
                                        id = cont++,
                                        elemento = y.Sum(z => Convert.ToInt32(z.si)).ToString(),
                                        maquina = y.First().description,
                                        children = new List<indicaroresMuestraGeneralDTO>()
                                    }).ToList()
                            }).ToList();
                }
                if (TipoAlerta == "caution")
                {
                    al = lista.Where(x => Convert.ToInt16(x.al) < 7 && !x.al.Equals("0")).GroupBy(x => x.name).Select(x =>
                            new indicaroresMuestraGeneralDTO
                            {
                                id = cont++,
                                elemento = x.Sum(y => Convert.ToInt32(y.al)).ToString(),
                                maquina = x.First().name,
                                children = x.GroupBy(y => y.description).Select(y =>
                                    new indicaroresMuestraGeneralDTO
                                    {
                                        id = cont++,
                                        elemento = y.Sum(z => Convert.ToInt32(z.al)).ToString(),
                                        maquina = y.First().description,
                                        children = new List<indicaroresMuestraGeneralDTO>()
                                    }).ToList()
                            }).ToList();
                    //-----------------------------------------------------------------------------------------------------------------
                    cu = lista.Where(x => Convert.ToInt16(x.cu) < 11 && !x.cu.Equals("0")).GroupBy(x => x.name).Select(x =>
                            new indicaroresMuestraGeneralDTO
                            {
                                id = cont++,
                                elemento = x.Sum(y => Convert.ToInt32(y.cu)).ToString(),
                                maquina = x.First().name,
                                children = x.GroupBy(y => y.description).Select(y =>
                                    new indicaroresMuestraGeneralDTO
                                    {
                                        id = cont++,
                                        elemento = y.Sum(z => Convert.ToInt32(z.cu)).ToString(),
                                        maquina = y.First().description,
                                        children = new List<indicaroresMuestraGeneralDTO>()
                                    }).ToList()
                            }).ToList();
                    //-----------------------------------------------------------------------------------------------------------------
                    fe = lista.Where(x => Convert.ToInt16(x.fe) < 40 && !x.fe.Equals("0")).GroupBy(x => x.name).Select(x =>
                            new indicaroresMuestraGeneralDTO
                            {
                                id = cont++,
                                elemento = x.Sum(y => Convert.ToInt32(y.fe)).ToString(),
                                maquina = x.First().name,
                                children = x.GroupBy(y => y.description).Select(y =>
                                    new indicaroresMuestraGeneralDTO
                                    {
                                        id = cont++,
                                        elemento = y.Sum(z => Convert.ToInt32(z.fe)).ToString(),
                                        maquina = y.First().description,
                                        children = new List<indicaroresMuestraGeneralDTO>()
                                    }).ToList()
                            }).ToList();
                    //-----------------------------------------------------------------------------------------------------------------
                    si = lista.Where(x => Convert.ToInt16(x.si) < 10 && !x.si.Equals("0")).GroupBy(x => x.name).Select(x =>
                            new indicaroresMuestraGeneralDTO
                            {
                                id = cont++,
                                elemento = x.Sum(y => Convert.ToInt32(y.si)).ToString(),
                                maquina = x.First().name,
                                children = x.GroupBy(y => y.description).Select(y =>
                                    new indicaroresMuestraGeneralDTO
                                    {
                                        id = cont++,
                                        elemento = y.Sum(z => Convert.ToInt32(z.si)).ToString(),
                                        maquina = y.First().description,
                                        children = new List<indicaroresMuestraGeneralDTO>()
                                    }).ToList()
                            }).ToList();
                }

                result.Add("al", al);
                result.Add("cu", cu);
                result.Add("fe", fe);
                result.Add("si", si);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCbo_Elementos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, GlobalUtils.ParseEnumToCombo<ElementosMuestrasEnum>());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cargarGraficaTotal(string Lugar, string Componente, string Unitid, string Modelo, string Elemento, string FechaInicio, string FechaFin)
        {
            CultureInfo c = new CultureInfo("en-us", true);
            var result = new Dictionary<string, object>();
            try
            {
                DateTime fechaInicio = Convert.ToDateTime(FechaInicio);
                DateTime fechaFin = Convert.ToDateTime(FechaFin);
                List<MuestrasElementosDTO> resultado = minadoFactoryServices.getMinadoService().detalleCompleto(Lugar, Componente, Unitid, Modelo, Elemento, fechaInicio, fechaFin);
                if (Elemento != "todo")
                {
                    var res = minadoFactoryServices.getMinadoService().detallesMuestras(resultado, Elemento);
                    result.Add("grafica", res);
                }
                else
                {

                }
                //  result.Add("grafica", resultado);
                var table = resultado.Select(x => new
                {
                    Equipo = x.name,
                    AL = x.al,
                    cautionAL = 3.5,
                    alertaAL = 7,
                    CU = x.cu,
                    cautionCU = 7.5,
                    alertaCU = 11,
                    FE = x.fe,
                    cautionFE = 25,
                    alertaFE = 40,
                    SI = x.si,
                    cautionSI = 4,
                    alertaSI = 10
                });
                result.Add("tabla", table);
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