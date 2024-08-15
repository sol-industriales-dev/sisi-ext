using Core.DAO.Proyecciones;
using Core.DTO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Proyecciones
{
    public class CapturadeObrasDAO : GenericDAO<tblPro_CapturadeObras>, ICapturadeObrasDAO
    {
        public struct stCorrectDate
        {
            public int Escenario { get; set; }
            public int mes { get; set; }
            public int anio { get; set; }
        }
        public tblPro_CapturadeObras GetJsonData(int escenario, int meses, int anio)
        {

            var res = _context.tblPro_CapturadeObras.OrderByDescending(x => x.id);
            var CurrentMonth = res.FirstOrDefault().MesInicio;

            if (CurrentMonth == meses)
            {
                return res.FirstOrDefault();
            }
            else if (meses > CurrentMonth)
            {
                var diff = meses - CurrentMonth;
                if (diff == 1)
                {
                    return res.FirstOrDefault();
                }
                else
                {
                    return res.FirstOrDefault(x => x.MesInicio == meses && x.EjercicioInicial == anio);
                }
            }
            else if (CurrentMonth > meses)
            {
                var result = res.FirstOrDefault(x => x.MesInicio.Equals(meses) && x.EjercicioInicial.Equals(anio));
                return result;
            }
            return null;
        }
        public void GuardarActualizarCapturadeObras(tblPro_CapturadeObras obj)
        {
            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.USUARIO);
            else
                Update(obj, obj.id, (int)BitacoraEnum.CADENAPRODUCTIVA);
        }

        public Dictionary<string, object> getinfoCapturaObras(int Escenario, decimal divisor, int mes, int anio)
        {
            var result = new Dictionary<string, object>();
            //try
            //{

            var correctDate = validDateForData(Escenario, mes, anio);
            Escenario = correctDate.Escenario;
            mes = correctDate.mes;
            anio = correctDate.anio;

            var res = GetJsonData(Escenario, mes, anio);
            var res1 = _context.tblPro_Administracion.FirstOrDefault(x => x.Mes == mes && x.Anio == anio);
            var res2 = _context.tblPro_PagosDiversos.FirstOrDefault(x => x.Mes == mes && x.Anio == anio);
            int id = 0;
            int idRow = 0;
            if (res != null && res1 != null && res2 != null)
            {
                id = res.id;

                List<tblPro_Obras> ObrasObj = JsonConvert.DeserializeObject<List<tblPro_Obras>>(res.CadenaJson);

                AdministracionDTO admonObj = new AdministracionDTO();
                if (res1 != null)
                {
                    admonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<AdministracionDTO>(res1.CadenaJson);
                }
                else
                {
                    string fullMonthName = new DateTime(2015, mes + 1, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                    result.Add("DatoNull", "No se contiene la captura del mes de " + fullMonthName + "Gastos de administracion");
                    return result;
                }

                var EfectivoInversionesTemp = _context.tblPro_SaldosIniciales.Where(x => x.Mes.Equals(mes) && x.Anio.Equals(anio)).OrderByDescending(x => x.id).FirstOrDefault();
                var EfectivoInversionesTempObj = JsonConvert.DeserializeObject<List<EPFSaldoInicialDTO>>(EfectivoInversionesTemp.CadenaJson);
                //Pago a proveedores y acreedores
                var SaldoDiverso = _context.tblPro_PagosDiversos.OrderByDescending(x => x.id).FirstOrDefault(x => x.Mes.Equals(mes) && anio.Equals(anio));
                var SaldoDivObj = JsonConvert.DeserializeObject<PagosDivDTO>(SaldoDiverso.CadenaJson);




                var objId = ObrasObj.OrderByDescending(x => x.id).FirstOrDefault().id;

                idRow = objId;

                var FiltroEscenario = dataEscenarios(ObrasObj, Escenario);

                var ValorizacionObra = FiltroEscenario.Select(x => new
                {
                    Area = x.Area,
                    AreaNombre = x.Descripcion,
                    x.Margen,
                    x.Monto,
                    Fecha1 = (x.Monto * (x.Fecha1 / 100)),
                    Fecha2 = (x.Monto * (x.Fecha2 / 100)),
                    Fecha3 = (x.Monto * (x.Fecha3 / 100)),
                    Fecha4 = (x.Monto * (x.Fecha4 / 100)),
                    Fecha5 = (x.Monto * (x.Fecha5 / 100)),
                    Fecha6 = (x.Monto * (x.Fecha6 / 100)),
                    Fecha7 = (x.Monto * (x.Fecha7 / 100)),
                    Fecha8 = (x.Monto * (x.Fecha8 / 100)),
                    Fecha9 = (x.Monto * (x.Fecha9 / 100)),
                    Fecha10 = (x.Monto * (x.Fecha10 / 100)),
                    Fecha11 = (x.Monto * (x.Fecha11 / 100)),
                    Fecha12 = (x.Monto * (x.Fecha12 / 100)),
                });

                var auxaux2 = ValorizacionObra.Sum(x => x.Monto);
                var auxaux3 = ValorizacionObra.Sum(x => x.Fecha1 + x.Fecha2 + x.Fecha3 + x.Fecha4 + x.Fecha5 + x.Fecha6 + x.Fecha7 + x.Fecha8 + x.Fecha9 + x.Fecha10 + x.Fecha11 + x.Fecha12);

                var FlujodeIngresoGeneral = ValorizacionObra.ToList();

                var moreDebu = ToDataTable(FlujodeIngresoGeneral);



                var ValorizacionFlujo = ValorizacionObra.Select(x => new
                {
                    Area = x.Area,
                    x.Margen,
                    x.Monto,
                    Fecha1 = x.Fecha1 * (1 - (Convert.ToDecimal(x.Margen) / 100)),
                    Fecha2 = x.Fecha2 * (1 - (Convert.ToDecimal(x.Margen) / 100)),
                    Fecha3 = x.Fecha3 * (1 - (Convert.ToDecimal(x.Margen) / 100)),
                    Fecha4 = x.Fecha4 * (1 - (Convert.ToDecimal(x.Margen) / 100)),
                    Fecha5 = x.Fecha5 * (1 - (Convert.ToDecimal(x.Margen) / 100)),
                    Fecha6 = x.Fecha6 * (1 - (Convert.ToDecimal(x.Margen) / 100)),
                    Fecha7 = x.Fecha7 * (1 - (Convert.ToDecimal(x.Margen) / 100)),
                    Fecha8 = x.Fecha8 * (1 - (Convert.ToDecimal(x.Margen) / 100)),
                    Fecha9 = x.Fecha9 * (1 - (Convert.ToDecimal(x.Margen) / 100)),
                    Fecha10 = x.Fecha10 * (1 - (Convert.ToDecimal(x.Margen) / 100)),
                    Fecha11 = x.Fecha11 * (1 - (Convert.ToDecimal(x.Margen) / 100)),
                    Fecha12 = x.Fecha12 * (1 - (Convert.ToDecimal(x.Margen) / 100))
                });

                var FlujodeIngresoGeneral1 = ValorizacionFlujo.ToList();


                var moreDebud3 = ToDataTable(FlujodeIngresoGeneral1);
                List<tblPro_Obras> FlujodeIngresos = new List<tblPro_Obras>();
                List<tblPro_Obras> FlujodeIngresosM = new List<tblPro_Obras>();

                var Areas = ValorizacionObra.Select(x => x.Area).Distinct();

                foreach (var area in Areas)
                {
                    tblPro_Obras FujoIngresos = new tblPro_Obras
                    {
                        Area = area,
                        Concepto = "VENTAS",
                        IdRenglon = 1,
                        AreaNombre = _context.tblPro_CatAreas.FirstOrDefault(x => x.id == area).descripcion,
                        Descripcion = ValorizacionObra.Where(x => x.Area.Equals(area)).Select(y => y.AreaNombre).FirstOrDefault(),
                        Fecha1 = (ValorizacionObra.Where(x => x.Area.Equals(area)).Sum(x => x.Fecha1)) / divisor,
                        Fecha2 = (ValorizacionObra.Where(x => x.Area.Equals(area)).Sum(x => x.Fecha2)) / divisor,
                        Fecha3 = (ValorizacionObra.Where(x => x.Area.Equals(area)).Sum(x => x.Fecha3)) / divisor,
                        Fecha4 = (ValorizacionObra.Where(x => x.Area.Equals(area)).Sum(x => x.Fecha4)) / divisor,
                        Fecha5 = (ValorizacionObra.Where(x => x.Area.Equals(area)).Sum(x => x.Fecha5)) / divisor,
                        Fecha6 = (ValorizacionObra.Where(x => x.Area.Equals(area)).Sum(x => x.Fecha6)) / divisor,
                        Fecha7 = (ValorizacionObra.Where(x => x.Area.Equals(area)).Sum(x => x.Fecha7)) / divisor,
                        Fecha8 = (ValorizacionObra.Where(x => x.Area.Equals(area)).Sum(x => x.Fecha8)) / divisor,
                        Fecha9 = (ValorizacionObra.Where(x => x.Area.Equals(area)).Sum(x => x.Fecha9)) / divisor,
                        Fecha10 = (ValorizacionObra.Where(x => x.Area.Equals(area)).Sum(x => x.Fecha10)) / divisor,
                        Fecha11 = (ValorizacionObra.Where(x => x.Area.Equals(area)).Sum(x => x.Fecha11)) / divisor,
                        Fecha12 = (ValorizacionObra.Where(x => x.Area.Equals(area)).Sum(x => x.Fecha12)) / divisor,
                        Total = (ValorizacionObra.Where(x => x.Area.Equals(area)).
                            //Sum(x => x.Fecha1 + x.Fecha2 + x.Fecha3 + x.Fecha4 + x.Fecha5 + x.Fecha6 + x.Fecha7 + x.Fecha8 + x.Fecha9 + x.Fecha10 + x.Fecha11 + x.Fecha12))
                        Sum(x => x.Monto))
                    };

                    //FujoIngresos.Total = (FujoIngresos.Fecha1 + FujoIngresos.Fecha2 + FujoIngresos.Fecha3 + FujoIngresos.Fecha4 + FujoIngresos.Fecha5 + FujoIngresos.Fecha6 + FujoIngresos.Fecha7 + FujoIngresos.Fecha8 + FujoIngresos.Fecha9 + FujoIngresos.Fecha10 + FujoIngresos.Fecha11 + FujoIngresos.Fecha12);



                    tblPro_Obras FujoIngresosMensual = new tblPro_Obras
                    {
                        Area = area,
                        Concepto = "COSTO DE VENTAS",
                        IdRenglon = 3,
                        AreaNombre = _context.tblPro_CatAreas.FirstOrDefault(x => x.id == area).descripcion,
                        // Area = ValorizacionFlujo.Where(x => x.Area.Equals(area)).Select(y => y.Area).FirstOrDefault(),
                        Fecha1 = (ValorizacionFlujo.Where(x => x.Area.Equals(area)).Sum(x => x.Fecha1)) / divisor,
                        Fecha2 = (ValorizacionFlujo.Where(x => x.Area.Equals(area)).Sum(x => x.Fecha2)) / divisor,
                        Fecha3 = (ValorizacionFlujo.Where(x => x.Area.Equals(area)).Sum(x => x.Fecha3)) / divisor,
                        Fecha4 = (ValorizacionFlujo.Where(x => x.Area.Equals(area)).Sum(x => x.Fecha4)) / divisor,
                        Fecha5 = (ValorizacionFlujo.Where(x => x.Area.Equals(area)).Sum(x => x.Fecha5)) / divisor,
                        Fecha6 = (ValorizacionFlujo.Where(x => x.Area.Equals(area)).Sum(x => x.Fecha6)) / divisor,
                        Fecha7 = (ValorizacionFlujo.Where(x => x.Area.Equals(area)).Sum(x => x.Fecha7)) / divisor,
                        Fecha8 = (ValorizacionFlujo.Where(x => x.Area.Equals(area)).Sum(x => x.Fecha8)) / divisor,
                        Fecha9 = (ValorizacionFlujo.Where(x => x.Area.Equals(area)).Sum(x => x.Fecha9)) / divisor,
                        Fecha10 = (ValorizacionFlujo.Where(x => x.Area.Equals(area)).Sum(x => x.Fecha10)) / divisor,
                        Fecha11 = (ValorizacionFlujo.Where(x => x.Area.Equals(area)).Sum(x => x.Fecha11)) / divisor,
                        Fecha12 = (ValorizacionFlujo.Where(x => x.Area.Equals(area)).Sum(x => x.Fecha12)) / divisor
                    };
                    FujoIngresosMensual.Total = (FujoIngresosMensual.Fecha1 + FujoIngresosMensual.Fecha2 + FujoIngresosMensual.Fecha3 + FujoIngresosMensual.Fecha4 + FujoIngresosMensual.Fecha5 + FujoIngresosMensual.Fecha6 + FujoIngresosMensual.Fecha7 + FujoIngresosMensual.Fecha8 + FujoIngresosMensual.Fecha9 + FujoIngresosMensual.Fecha10 + FujoIngresosMensual.Fecha11 + FujoIngresosMensual.Fecha12);


                    FlujodeIngresosM.Add(FujoIngresosMensual);


                    FlujodeIngresos.Add(FujoIngresos);
                }
                //  var FlujodeIngresoGeneral = FlujodeIngresos.ToList();


                //var moreDebud = ToDataTable(FlujodeIngresoGeneral);
                //FlujodeIngresosM;
                //FlujodeIngresos.OrderBy(x => x.Area).ToList();
                List<int> AreasLista = new List<int>();

                var ObrasMasVentas = FlujodeIngresos.OrderByDescending(x => x.Total).Take(3);

                var SumaTotaldeVentas = FlujodeIngresos.Sum(x => x.Total);
                List<DivisionesVentasDTO> ListaDivisionesVentas = new List<DivisionesVentasDTO>();


                int count = 0;
                foreach (var item in ObrasMasVentas)
                {
                    count += 1;
                    DivisionesVentasDTO Division = new DivisionesVentasDTO();

                    Division.Lugar = count;
                    Division.Obra = item.AreaNombre;
                    Division.Procentaje = item.Total / SumaTotaldeVentas;
                    Division.Venta = item.Total;
                    Division.Area = item.Area;
                    ListaDivisionesVentas.Add(Division);
                }


                var Ventas = FlujodeIngresos.OrderBy(x => x.Area).ToList();
                var CostoVentas = FlujodeIngresosM.OrderBy(x => x.Area).ToList();

                EstadoResultadosDTO VentasNetas = new EstadoResultadosDTO();
                EstadoResultadosDTO CostoVentaTotal = new EstadoResultadosDTO();
                VentasNetas = new EstadoResultadosDTO
                {
                    Concepto = "VENTAS",
                    AreaNombre = "VENTAS NETAS",
                    IdRenglon = 2,
                    Fecha1 = Ventas.Sum(x => x.Fecha1),
                    Fecha2 = Ventas.Sum(x => x.Fecha2),
                    Fecha3 = Ventas.Sum(x => x.Fecha3),
                    Fecha4 = Ventas.Sum(x => x.Fecha4),
                    Fecha5 = Ventas.Sum(x => x.Fecha5),
                    Fecha6 = Ventas.Sum(x => x.Fecha6),
                    Fecha7 = Ventas.Sum(x => x.Fecha7),
                    Fecha8 = Ventas.Sum(x => x.Fecha8),
                    Fecha9 = Ventas.Sum(x => x.Fecha9),
                    Fecha10 = Ventas.Sum(x => x.Fecha10),
                    Fecha11 = Ventas.Sum(x => x.Fecha11),
                    Fecha12 = Ventas.Sum(x => x.Fecha12),

                };
                VentasNetas.Total = VentasNetas.Fecha1 + VentasNetas.Fecha2 + VentasNetas.Fecha3 + VentasNetas.Fecha4 + VentasNetas.Fecha5 + VentasNetas.Fecha6 + VentasNetas.Fecha7 + VentasNetas.Fecha8 + VentasNetas.Fecha9 + VentasNetas.Fecha10 + VentasNetas.Fecha11 + VentasNetas.Fecha12;
                CostoVentaTotal = new EstadoResultadosDTO
                {
                    Concepto = "COSTO DE VENTAS ",
                    AreaNombre = "TOTAL",
                    IdRenglon = 4,
                    Fecha1 = CostoVentas.Sum(x => x.Fecha1),
                    Fecha2 = CostoVentas.Sum(x => x.Fecha2),
                    Fecha3 = CostoVentas.Sum(x => x.Fecha3),
                    Fecha4 = CostoVentas.Sum(x => x.Fecha4),
                    Fecha5 = CostoVentas.Sum(x => x.Fecha5),
                    Fecha6 = CostoVentas.Sum(x => x.Fecha6),
                    Fecha7 = CostoVentas.Sum(x => x.Fecha7),
                    Fecha8 = CostoVentas.Sum(x => x.Fecha8),
                    Fecha9 = CostoVentas.Sum(x => x.Fecha9),
                    Fecha10 = CostoVentas.Sum(x => x.Fecha10),
                    Fecha11 = CostoVentas.Sum(x => x.Fecha11),
                    Fecha12 = CostoVentas.Sum(x => x.Fecha12),
                };
                CostoVentaTotal.Total = CostoVentaTotal.Fecha1 + CostoVentaTotal.Fecha2 + CostoVentaTotal.Fecha3 + CostoVentaTotal.Fecha4 + CostoVentaTotal.Fecha5 + CostoVentaTotal.Fecha6 + CostoVentaTotal.Fecha7 + CostoVentaTotal.Fecha8 + CostoVentaTotal.Fecha9 + CostoVentaTotal.Fecha10 + CostoVentaTotal.Fecha11 + CostoVentaTotal.Fecha12;
                EstadoResultadosDTO ContribucionMarginal = new EstadoResultadosDTO();
                EstadoResultadosDTO TotalGtoOperacion = new EstadoResultadosDTO();
                EstadoResultadosDTO UtilidadOperacion = new EstadoResultadosDTO();
                EstadoResultadosDTO CostoIntegral = new EstadoResultadosDTO();
                EstadoResultadosDTO impuestos = new EstadoResultadosDTO();
                EstadoResultadosDTO UtilidadAntesImp = new EstadoResultadosDTO();
                EstadoResultadosDTO UtilidadNeta = new EstadoResultadosDTO();
                EstadoResultadosDTO UtilidadPromedioBruta = new EstadoResultadosDTO();
                EstadoResultadosDTO SaldoFinalFlujoEfectivo = new EstadoResultadosDTO();




                impuestos.Concepto = "IMPUESTOS";
                impuestos.Fecha1 = 0;
                impuestos.Fecha2 = 0;
                impuestos.Fecha3 = 0;
                impuestos.Fecha4 = 0;
                impuestos.Fecha5 = 0;
                impuestos.Fecha6 = 0;
                impuestos.Fecha7 = 0;
                impuestos.Fecha8 = 0;

                impuestos.Fecha9 = 0;
                impuestos.Fecha10 = 0;
                impuestos.Fecha11 = 0;
                impuestos.Fecha12 = 0;
                impuestos.Total = 0;



                if (VentasNetas != null && CostoVentaTotal != null)
                {

                    var Marginal1 = VentasNetas.Fecha1 - CostoVentaTotal.Fecha1;
                    var Marginal2 = VentasNetas.Fecha2 - CostoVentaTotal.Fecha2;
                    var Marginal3 = VentasNetas.Fecha3 - CostoVentaTotal.Fecha3;
                    var Marginal4 = VentasNetas.Fecha4 - CostoVentaTotal.Fecha4;
                    var Marginal5 = VentasNetas.Fecha5 - CostoVentaTotal.Fecha5;
                    var Marginal6 = VentasNetas.Fecha6 - CostoVentaTotal.Fecha6;
                    var Marginal7 = VentasNetas.Fecha7 - CostoVentaTotal.Fecha7;
                    var Marginal8 = VentasNetas.Fecha8 - CostoVentaTotal.Fecha8;
                    var Marginal9 = VentasNetas.Fecha9 - CostoVentaTotal.Fecha9;
                    var Marginal10 = VentasNetas.Fecha10 - CostoVentaTotal.Fecha10;
                    var Marginal11 = VentasNetas.Fecha11 - CostoVentaTotal.Fecha11;
                    var Marginal12 = VentasNetas.Fecha12 - CostoVentaTotal.Fecha12;
                    var Total = Marginal1 + Marginal2 + Marginal3 + Marginal4 + Marginal5 + Marginal6 + Marginal7 + Marginal8 + Marginal9 + Marginal10 + Marginal11 + Marginal12;

                    ContribucionMarginal.Concepto = "CONTRIBUCION MARGINAL:";
                    ContribucionMarginal.AreaNombre = "";
                    ContribucionMarginal.IdRenglon = 5;
                    ContribucionMarginal.Fecha1 = Marginal1;
                    ContribucionMarginal.Fecha2 = Marginal2;
                    ContribucionMarginal.Fecha3 = Marginal3;
                    ContribucionMarginal.Fecha4 = Marginal4;
                    ContribucionMarginal.Fecha5 = Marginal5;
                    ContribucionMarginal.Fecha6 = Marginal6;
                    ContribucionMarginal.Fecha7 = Marginal7;
                    ContribucionMarginal.Fecha8 = Marginal8;
                    ContribucionMarginal.Fecha9 = Marginal9;
                    ContribucionMarginal.Fecha10 = Marginal10;
                    ContribucionMarginal.Fecha11 = Marginal11;
                    ContribucionMarginal.Fecha12 = Marginal12;
                    ContribucionMarginal.Total = Total;
                }



                TotalGtoOperacion.IdRenglon = 6;
                TotalGtoOperacion.Fecha1 = ((admonObj.ADT1 / 100) * admonObj.PMRGAP) / divisor;
                TotalGtoOperacion.Fecha2 = ((admonObj.ADT2 / 100) * admonObj.PMRGAP) / divisor;
                TotalGtoOperacion.Fecha3 = ((admonObj.ADT3 / 100) * admonObj.PMRGAP) / divisor;
                TotalGtoOperacion.Fecha4 = ((admonObj.ADT4 / 100) * admonObj.PMRGAP) / divisor;
                TotalGtoOperacion.Fecha5 = ((admonObj.ADT5 / 100) * admonObj.PMRGAP) / divisor;
                TotalGtoOperacion.Fecha6 = ((admonObj.ADT6 / 100) * admonObj.PMRGAP) / divisor;
                TotalGtoOperacion.Fecha7 = ((admonObj.ADT7 / 100) * admonObj.PMRGAP) / divisor;
                TotalGtoOperacion.Fecha8 = ((admonObj.ADT8 / 100) * admonObj.PMRGAP) / divisor;
                TotalGtoOperacion.Fecha9 = ((admonObj.ADT9 / 100) * admonObj.PMRGAP) / divisor;
                TotalGtoOperacion.Fecha10 = ((admonObj.ADT10 / 100) * admonObj.PMRGAP) / divisor;
                TotalGtoOperacion.Fecha11 = ((admonObj.ADT11 / 100) * admonObj.PMRGAP) / divisor;
                TotalGtoOperacion.Fecha12 = ((admonObj.ADT12 / 100) * admonObj.PMRGAP) / divisor;


                UtilidadOperacion.Concepto = "UTILIDAD DE OPERACIÓN";
                UtilidadOperacion.AreaNombre = "";
                UtilidadOperacion.IdRenglon = 7;
                UtilidadOperacion.Fecha1 = ContribucionMarginal.Fecha1 - TotalGtoOperacion.Fecha1;
                UtilidadOperacion.Fecha2 = ContribucionMarginal.Fecha2 - TotalGtoOperacion.Fecha2;
                UtilidadOperacion.Fecha3 = ContribucionMarginal.Fecha3 - TotalGtoOperacion.Fecha3;
                UtilidadOperacion.Fecha4 = ContribucionMarginal.Fecha4 - TotalGtoOperacion.Fecha4;
                UtilidadOperacion.Fecha5 = ContribucionMarginal.Fecha5 - TotalGtoOperacion.Fecha5;
                UtilidadOperacion.Fecha6 = ContribucionMarginal.Fecha6 - TotalGtoOperacion.Fecha6;
                UtilidadOperacion.Fecha7 = ContribucionMarginal.Fecha7 - TotalGtoOperacion.Fecha7;
                UtilidadOperacion.Fecha8 = ContribucionMarginal.Fecha8 - TotalGtoOperacion.Fecha8;
                UtilidadOperacion.Fecha9 = ContribucionMarginal.Fecha9 - TotalGtoOperacion.Fecha9;
                UtilidadOperacion.Fecha10 = ContribucionMarginal.Fecha10 - TotalGtoOperacion.Fecha10;
                UtilidadOperacion.Fecha11 = ContribucionMarginal.Fecha11 - TotalGtoOperacion.Fecha11;
                UtilidadOperacion.Fecha12 = ContribucionMarginal.Fecha12 - TotalGtoOperacion.Fecha12;


                CostoIntegral.Concepto = "COSTO INTREGRAL DE FINANCIAMIENTO";

                UtilidadAntesImp.Concepto = "UTILIDAD (Antes de Imptos)";
                UtilidadAntesImp.AreaNombre = "";
                UtilidadAntesImp.IdRenglon = 9;
                UtilidadAntesImp.Fecha1 = UtilidadOperacion.Fecha1;
                UtilidadAntesImp.Fecha2 = UtilidadOperacion.Fecha2;
                UtilidadAntesImp.Fecha3 = UtilidadOperacion.Fecha3;
                UtilidadAntesImp.Fecha4 = UtilidadOperacion.Fecha4;
                UtilidadAntesImp.Fecha5 = UtilidadOperacion.Fecha5;
                UtilidadAntesImp.Fecha6 = UtilidadOperacion.Fecha6;
                UtilidadAntesImp.Fecha7 = UtilidadOperacion.Fecha7;
                UtilidadAntesImp.Fecha8 = UtilidadOperacion.Fecha8;
                UtilidadAntesImp.Fecha9 = UtilidadOperacion.Fecha9;
                UtilidadAntesImp.Fecha10 = UtilidadOperacion.Fecha10;
                UtilidadAntesImp.Fecha11 = UtilidadOperacion.Fecha11;
                UtilidadAntesImp.Fecha12 = UtilidadOperacion.Fecha12;


                UtilidadNeta.Concepto = "UTILIDAD ( Pérdida ) NETA";
                UtilidadNeta.AreaNombre = "";
                UtilidadNeta.IdRenglon = 10;
                UtilidadNeta.Fecha1 = UtilidadAntesImp.Fecha1 - impuestos.Fecha1;
                UtilidadNeta.Fecha2 = UtilidadAntesImp.Fecha2 - impuestos.Fecha2;
                UtilidadNeta.Fecha3 = UtilidadAntesImp.Fecha3 - impuestos.Fecha3;
                UtilidadNeta.Fecha4 = UtilidadAntesImp.Fecha4 - impuestos.Fecha4;
                UtilidadNeta.Fecha5 = UtilidadAntesImp.Fecha5 - impuestos.Fecha5;
                UtilidadNeta.Fecha6 = UtilidadAntesImp.Fecha6 - impuestos.Fecha6;
                UtilidadNeta.Fecha7 = UtilidadAntesImp.Fecha7 - impuestos.Fecha7;
                UtilidadNeta.Fecha8 = UtilidadAntesImp.Fecha8 - impuestos.Fecha8;
                UtilidadNeta.Fecha9 = UtilidadAntesImp.Fecha9 - impuestos.Fecha9;
                UtilidadNeta.Fecha10 = UtilidadAntesImp.Fecha10 - impuestos.Fecha10;
                UtilidadNeta.Fecha11 = UtilidadAntesImp.Fecha11 - impuestos.Fecha11;
                UtilidadNeta.Fecha12 = UtilidadAntesImp.Fecha12 - impuestos.Fecha12;

                UtilidadPromedioBruta.Concepto = "UtilidadBrutaPromedio"; //Solo se utiliza en la principal.
                UtilidadPromedioBruta.Fecha1 = ContribucionMarginal.Fecha1 != 0 ? (ContribucionMarginal.Fecha1 / VentasNetas.Fecha1) * 100 : 0;
                UtilidadPromedioBruta.Fecha2 = ContribucionMarginal.Fecha2 != 0 ? (ContribucionMarginal.Fecha1 / VentasNetas.Fecha2) * 100 : 0;  //(ContribucionMarginal.Fecha2 / VentasNetas.Fecha2) * 100;
                UtilidadPromedioBruta.Fecha3 = ContribucionMarginal.Fecha3 != 0 ? (ContribucionMarginal.Fecha2 / VentasNetas.Fecha3) * 100 : 0;  //(ContribucionMarginal.Fecha3 / VentasNetas.Fecha3) * 100;
                UtilidadPromedioBruta.Fecha4 = ContribucionMarginal.Fecha4 != 0 ? (ContribucionMarginal.Fecha3 / VentasNetas.Fecha4) * 100 : 0; //(ContribucionMarginal.Fecha4 / VentasNetas.Fecha4) * 100;
                UtilidadPromedioBruta.Fecha5 = ContribucionMarginal.Fecha5 != 0 ? (ContribucionMarginal.Fecha4 / VentasNetas.Fecha5) * 100 : 0;// (ContribucionMarginal.Fecha5 / VentasNetas.Fecha5) * 100;
                UtilidadPromedioBruta.Fecha6 = ContribucionMarginal.Fecha6 != 0 ? (ContribucionMarginal.Fecha5 / VentasNetas.Fecha6) * 100 : 0; //(ContribucionMarginal.Fecha6 / VentasNetas.Fecha6) * 100;
                UtilidadPromedioBruta.Fecha7 = ContribucionMarginal.Fecha7 != 0 ? (ContribucionMarginal.Fecha6 / VentasNetas.Fecha7) * 100 : 0; //(ContribucionMarginal.Fecha7 / VentasNetas.Fecha7) * 100;
                UtilidadPromedioBruta.Fecha8 = ContribucionMarginal.Fecha8 != 0 ? (ContribucionMarginal.Fecha7 / VentasNetas.Fecha8) * 100 : 0;// (ContribucionMarginal.Fecha8 / VentasNetas.Fecha8) * 100;
                UtilidadPromedioBruta.Fecha9 = ContribucionMarginal.Fecha9 != 0 ? (ContribucionMarginal.Fecha8 / VentasNetas.Fecha9) * 100 : 0;  //(ContribucionMarginal.Fecha9 / VentasNetas.Fecha9) * 100;
                UtilidadPromedioBruta.Fecha10 = ContribucionMarginal.Fecha10 != 0 ? (ContribucionMarginal.Fecha10 / VentasNetas.Fecha10) * 100 : 0; //(ContribucionMarginal.Fecha10 / VentasNetas.Fecha10) * 100;
                UtilidadPromedioBruta.Fecha11 = ContribucionMarginal.Fecha11 != 0 ? (ContribucionMarginal.Fecha11 / VentasNetas.Fecha11) * 100 : 0; //(ContribucionMarginal.Fecha11 / VentasNetas.Fecha11) * 100;
                UtilidadPromedioBruta.Fecha12 = ContribucionMarginal.Fecha12 != 0 ? (ContribucionMarginal.Fecha12 / VentasNetas.Fecha12) * 100 : 0; //(ContribucionMarginal.Fecha12 / VentasNetas.Fecha12) * 100;


                DetallePorcentajeMESDTO UtilidadBrutaDetalle = new DetallePorcentajeMESDTO();
                

                UtilidadBrutaDetalle.Fecha1 = ContribucionMarginal.Fecha1.ToString("0", CultureInfo.InvariantCulture) + "/" + VentasNetas.Fecha1.ToString("0", CultureInfo.InvariantCulture) + "=" + Math.Round(UtilidadPromedioBruta.Fecha1, 2);
                UtilidadBrutaDetalle.Fecha2 = ContribucionMarginal.Fecha2.ToString("0", CultureInfo.InvariantCulture) + "/" + VentasNetas.Fecha2.ToString("0", CultureInfo.InvariantCulture) + "=" + Math.Round(UtilidadPromedioBruta.Fecha2, 2);
                UtilidadBrutaDetalle.Fecha3 = ContribucionMarginal.Fecha3.ToString("0", CultureInfo.InvariantCulture) + "/" + VentasNetas.Fecha3.ToString("0", CultureInfo.InvariantCulture) + "=" + Math.Round(UtilidadPromedioBruta.Fecha3, 2);
                UtilidadBrutaDetalle.Fecha4 = ContribucionMarginal.Fecha4.ToString("0", CultureInfo.InvariantCulture) + "/" + VentasNetas.Fecha4.ToString("0", CultureInfo.InvariantCulture) + "=" + Math.Round(UtilidadPromedioBruta.Fecha4, 2);
                UtilidadBrutaDetalle.Fecha5 = ContribucionMarginal.Fecha5.ToString("0", CultureInfo.InvariantCulture) + "/" + VentasNetas.Fecha5.ToString("0", CultureInfo.InvariantCulture) + "=" + Math.Round(UtilidadPromedioBruta.Fecha5, 2);
                UtilidadBrutaDetalle.Fecha6 = ContribucionMarginal.Fecha6.ToString("0", CultureInfo.InvariantCulture) + "/" + VentasNetas.Fecha6.ToString("0", CultureInfo.InvariantCulture) + "=" + Math.Round(UtilidadPromedioBruta.Fecha6, 2);
                UtilidadBrutaDetalle.Fecha7 = ContribucionMarginal.Fecha7.ToString("0", CultureInfo.InvariantCulture) + "/" + VentasNetas.Fecha7.ToString("0", CultureInfo.InvariantCulture) + "=" + Math.Round(UtilidadPromedioBruta.Fecha7, 2);
                UtilidadBrutaDetalle.Fecha8 = ContribucionMarginal.Fecha8.ToString("0", CultureInfo.InvariantCulture) + "/" + VentasNetas.Fecha8.ToString("0", CultureInfo.InvariantCulture) + "=" + Math.Round(UtilidadPromedioBruta.Fecha8, 2);
                UtilidadBrutaDetalle.Fecha9 = ContribucionMarginal.Fecha9.ToString("0", CultureInfo.InvariantCulture) + "/" + VentasNetas.Fecha9.ToString("0", CultureInfo.InvariantCulture) + "=" + Math.Round(UtilidadPromedioBruta.Fecha9, 2);
                UtilidadBrutaDetalle.Fecha10 = ContribucionMarginal.Fecha10.ToString("0", CultureInfo.InvariantCulture) + "/" + VentasNetas.Fecha10.ToString("0", CultureInfo.InvariantCulture) + "=" + Math.Round(UtilidadPromedioBruta.Fecha10, 2);
                UtilidadBrutaDetalle.Fecha11 = ContribucionMarginal.Fecha11.ToString("0", CultureInfo.InvariantCulture) + "/" + VentasNetas.Fecha11.ToString("0", CultureInfo.InvariantCulture) + "=" + Math.Round(UtilidadPromedioBruta.Fecha11, 2);
                UtilidadBrutaDetalle.Fecha12 = ContribucionMarginal.Fecha12.ToString("0", CultureInfo.InvariantCulture) + "/" + VentasNetas.Fecha12.ToString("0", CultureInfo.InvariantCulture) + "=" + Math.Round(UtilidadPromedioBruta.Fecha12, 2);






                EstadoResultadosDTO IngresosVentas = new EstadoResultadosDTO();
                IngresosVentas.Concepto = "Ingresos de Operación";
                IngresosVentas.IdRenglon = 1;
                IngresosVentas.AreaNombre = "Ingresos por Ventas";
                IngresosVentas.Fecha1 = VentasNetas.Fecha1;
                IngresosVentas.Fecha2 = VentasNetas.Fecha2;
                IngresosVentas.Fecha3 = VentasNetas.Fecha3;
                IngresosVentas.Fecha4 = VentasNetas.Fecha4;
                IngresosVentas.Fecha5 = VentasNetas.Fecha5;
                IngresosVentas.Fecha6 = VentasNetas.Fecha6;
                IngresosVentas.Fecha7 = VentasNetas.Fecha7;
                IngresosVentas.Fecha8 = VentasNetas.Fecha8;
                IngresosVentas.Fecha9 = VentasNetas.Fecha9;
                IngresosVentas.Fecha10 = VentasNetas.Fecha10;
                IngresosVentas.Fecha11 = VentasNetas.Fecha11;
                IngresosVentas.Fecha12 = VentasNetas.Fecha12;
                IngresosVentas.Total = VentasNetas.Total;

                EstadoResultadosDTO IngresosVentasMaq = new EstadoResultadosDTO();
                IngresosVentasMaq.Concepto = "Ingresos de Operación";
                IngresosVentas.IdRenglon = 2;
                IngresosVentasMaq.AreaNombre = "Ingresos por Venta de Maquinaria";
                IngresosVentasMaq.IdRenglon = 8;
                IngresosVentasMaq.Fecha1 = 0;
                IngresosVentasMaq.Fecha2 = 0;
                IngresosVentasMaq.Fecha3 = 0;
                IngresosVentasMaq.Fecha4 = 0;
                IngresosVentasMaq.Fecha5 = 0;
                IngresosVentasMaq.Fecha6 = 0;
                IngresosVentasMaq.Fecha7 = 0;
                IngresosVentasMaq.Fecha8 = 0;
                IngresosVentasMaq.Fecha9 = 0;
                IngresosVentasMaq.Fecha10 = 0;
                IngresosVentasMaq.Fecha11 = 0;
                IngresosVentasMaq.Fecha12 = 0;
                IngresosVentasMaq.Total = 0;

                MesDTO ASPSaldoAmortizar = new MesDTO();
                ASPSaldoAmortizar.Mes1 = 0.5M;


                MesDTO ASPAplicarCxC = new MesDTO();
                ASPSaldoAmortizar.Mes1 = 0;
                /*Aqui le estoy moviendo*/
                MesDTO AcredorDiverso = new MesDTO();
                AcredorDiverso.Mes1 = 0;

                var AmortizaAcreedores = SaldoDivObj.ImporteAmortAcreedores;
                var AmortizaAbono = SaldoDivObj.ImporteAmortAbono;
                EstadoResultadosDTO ProveedorAcreedor = new EstadoResultadosDTO
                {
                    Concepto = "Costos y Gtos de Operación",
                    AreaNombre = "Pago a Proveedores y Acreedores",
                    IdRenglon = 6,
                    Fecha1 = (AmortizaAcreedores.Mes1 + AmortizaAbono.Mes1) / divisor,
                    Fecha2 = (AmortizaAcreedores.Mes2 + AmortizaAbono.Mes2) / divisor,
                    Fecha3 = (AmortizaAcreedores.Mes3 + AmortizaAbono.Mes3) / divisor,
                    Fecha4 = (AmortizaAcreedores.Mes4 + AmortizaAbono.Mes4) / divisor,
                    Fecha5 = (AmortizaAcreedores.Mes5 + AmortizaAbono.Mes5) / divisor,
                    Fecha6 = (AmortizaAcreedores.Mes6 + AmortizaAbono.Mes6) / divisor,
                    Fecha7 = (AmortizaAcreedores.Mes7 + AmortizaAbono.Mes7) / divisor,
                    Fecha8 = (AmortizaAcreedores.Mes8 + AmortizaAbono.Mes8) / divisor,
                    Fecha9 = (AmortizaAcreedores.Mes9 + AmortizaAbono.Mes9) / divisor,
                    Fecha10 = (AmortizaAcreedores.Mes10 + AmortizaAbono.Mes10) / divisor,
                    Fecha11 = (AmortizaAcreedores.Mes11 + AmortizaAbono.Mes11) / divisor,
                    Fecha12 = (AmortizaAcreedores.Mes12 + AmortizaAbono.Mes12) / divisor,
                    //Total = (AmortizaAcreedores.MesT + AmortizaAbono.Mes1) / divisor,
                };



                ProveedorAcreedor.Total = ProveedorAcreedor.Fecha1 + ProveedorAcreedor.Fecha2 + ProveedorAcreedor.Fecha3 + ProveedorAcreedor.Fecha4 + ProveedorAcreedor.Fecha5 + ProveedorAcreedor.Fecha6 + ProveedorAcreedor.Fecha7 + ProveedorAcreedor.Fecha8 + ProveedorAcreedor.Fecha9 + ProveedorAcreedor.Fecha10 + ProveedorAcreedor.Fecha11 + ProveedorAcreedor.Fecha12;

                CostoVentaTotal.Concepto = "Costos y Gtos de Operación";
                TotalGtoOperacion.Concepto = "GASTOS DE OPERACIÓN";
                ProveedorAcreedor.Concepto = "Costos y Gtos de Operación";
                CostoVentaTotal.AreaNombre = "Costos de Ventas";
                TotalGtoOperacion.AreaNombre = "ADMINISTRACIÓN CENTRAL";
                ProveedorAcreedor.AreaNombre = "Pago a Proveedores y Acreedores";
                EstadoResultadosDTO CostoGastoOperacion = new EstadoResultadosDTO();
                CostoGastoOperacion.Concepto = "Costos y Gtos de Operación";
                CostoGastoOperacion.IdRenglon = 3;
                CostoGastoOperacion.AreaNombre = "Gastos de administración";
                CostoGastoOperacion.Fecha1 = TotalGtoOperacion.Fecha1; //CostoVentaTotal.Fecha1 + TotalGtoOperacion.Fecha1 + ProveedorAcreedor.Fecha1; //Costo de venta + gasto administrativo + Pago proveedor acreedor
                CostoGastoOperacion.Fecha2 = TotalGtoOperacion.Fecha2;///CostoVentaTotal.Fecha2 + TotalGtoOperacion.Fecha2 + ProveedorAcreedor.Fecha2;
                CostoGastoOperacion.Fecha3 = TotalGtoOperacion.Fecha3; ///CostoVentaTotal.Fecha3 + TotalGtoOperacion.Fecha3 + ProveedorAcreedor.Fecha3;
                CostoGastoOperacion.Fecha4 = TotalGtoOperacion.Fecha4;//CostoVentaTotal.Fecha4 + TotalGtoOperacion.Fecha4 + ProveedorAcreedor.Fecha4;
                CostoGastoOperacion.Fecha5 = TotalGtoOperacion.Fecha5;//CostoVentaTotal.Fecha5 + TotalGtoOperacion.Fecha5 + ProveedorAcreedor.Fecha5;
                CostoGastoOperacion.Fecha6 = TotalGtoOperacion.Fecha6;//CostoVentaTotal.Fecha6 + TotalGtoOperacion.Fecha6 + ProveedorAcreedor.Fecha6;
                CostoGastoOperacion.Fecha7 = TotalGtoOperacion.Fecha7;//CostoVentaTotal.Fecha7 + TotalGtoOperacion.Fecha7 + ProveedorAcreedor.Fecha7;
                CostoGastoOperacion.Fecha8 = TotalGtoOperacion.Fecha8;//CostoVentaTotal.Fecha8 + TotalGtoOperacion.Fecha8 + ProveedorAcreedor.Fecha8;
                CostoGastoOperacion.Fecha9 = TotalGtoOperacion.Fecha9;//CostoVentaTotal.Fecha9 + TotalGtoOperacion.Fecha9 + ProveedorAcreedor.Fecha9;
                CostoGastoOperacion.Fecha10 = TotalGtoOperacion.Fecha10;//CostoVentaTotal.Fecha10 + TotalGtoOperacion.Fecha10 + ProveedorAcreedor.Fecha10;
                CostoGastoOperacion.Fecha11 = TotalGtoOperacion.Fecha11;//CostoVentaTotal.Fecha11 + TotalGtoOperacion.Fecha11 + ProveedorAcreedor.Fecha11;
                CostoGastoOperacion.Fecha12 = TotalGtoOperacion.Fecha12; //CostoVentaTotal.Fecha12 + TotalGtoOperacion.Fecha12 + ProveedorAcreedor.Fecha12;
                CostoGastoOperacion.Total = CostoGastoOperacion.Fecha1 + CostoGastoOperacion.Fecha2 + CostoGastoOperacion.Fecha3 + CostoGastoOperacion.Fecha4 + CostoGastoOperacion.Fecha5 + CostoGastoOperacion.Fecha6 + CostoGastoOperacion.Fecha7 + CostoGastoOperacion.Fecha8 + CostoGastoOperacion.Fecha9 + CostoGastoOperacion.Fecha10 + CostoGastoOperacion.Fecha11 + CostoGastoOperacion.Fecha12;

                TotalGtoOperacion.Total = CostoGastoOperacion.Total;
                UtilidadOperacion.Total = ContribucionMarginal.Total - TotalGtoOperacion.Total;
                UtilidadAntesImp.Total = UtilidadOperacion.Total;
                UtilidadNeta.Total = UtilidadAntesImp.Total - impuestos.Total;

                EstadoResultadosDTO TotalGtosOperacionR1 = new EstadoResultadosDTO();
                TotalGtosOperacionR1.Concepto = "TOTAL GTOS. OPERACIÓN";
                TotalGtosOperacionR1.AreaNombre = "";
                TotalGtosOperacionR1.Fecha1 = CostoGastoOperacion.Fecha1;
                TotalGtosOperacionR1.Fecha2 = CostoGastoOperacion.Fecha2;
                TotalGtosOperacionR1.Fecha3 = CostoGastoOperacion.Fecha3;
                TotalGtosOperacionR1.Fecha4 = CostoGastoOperacion.Fecha4;
                TotalGtosOperacionR1.Fecha5 = CostoGastoOperacion.Fecha5;
                TotalGtosOperacionR1.Fecha6 = CostoGastoOperacion.Fecha6;
                TotalGtosOperacionR1.Fecha7 = CostoGastoOperacion.Fecha7;
                TotalGtosOperacionR1.Fecha8 = CostoGastoOperacion.Fecha8;
                TotalGtosOperacionR1.Fecha9 = CostoGastoOperacion.Fecha9;
                TotalGtosOperacionR1.Fecha10 = CostoGastoOperacion.Fecha10;
                TotalGtosOperacionR1.Fecha11 = CostoGastoOperacion.Fecha11;
                TotalGtosOperacionR1.Fecha12 = CostoGastoOperacion.Fecha12;
                TotalGtosOperacionR1.Total = CostoGastoOperacion.Total;


                EstadoResultadosDTO CostoGastoOperacionTotal = new EstadoResultadosDTO();
                CostoGastoOperacionTotal.Concepto = "Costos y Gtos de Operación";
                CostoGastoOperacionTotal.AreaNombre = "";
                CostoGastoOperacionTotal.Fecha1 = ProveedorAcreedor.Fecha1 + CostoVentaTotal.Fecha1 + CostoGastoOperacion.Fecha1;
                CostoGastoOperacionTotal.Fecha2 = ProveedorAcreedor.Fecha2 + CostoVentaTotal.Fecha2 + CostoGastoOperacion.Fecha2;
                CostoGastoOperacionTotal.Fecha3 = ProveedorAcreedor.Fecha3 + CostoVentaTotal.Fecha3 + CostoGastoOperacion.Fecha3;
                CostoGastoOperacionTotal.Fecha4 = ProveedorAcreedor.Fecha4 + CostoVentaTotal.Fecha4 + CostoGastoOperacion.Fecha4;
                CostoGastoOperacionTotal.Fecha5 = ProveedorAcreedor.Fecha5 + CostoVentaTotal.Fecha5 + CostoGastoOperacion.Fecha5;
                CostoGastoOperacionTotal.Fecha6 = ProveedorAcreedor.Fecha6 + CostoVentaTotal.Fecha6 + CostoGastoOperacion.Fecha6;
                CostoGastoOperacionTotal.Fecha7 = ProveedorAcreedor.Fecha7 + CostoVentaTotal.Fecha7 + CostoGastoOperacion.Fecha7;
                CostoGastoOperacionTotal.Fecha8 = ProveedorAcreedor.Fecha8 + CostoVentaTotal.Fecha8 + CostoGastoOperacion.Fecha8;
                CostoGastoOperacionTotal.Fecha9 = ProveedorAcreedor.Fecha9 + CostoVentaTotal.Fecha9 + CostoGastoOperacion.Fecha9;
                CostoGastoOperacionTotal.Fecha10 = ProveedorAcreedor.Fecha10 + CostoVentaTotal.Fecha10 + CostoGastoOperacion.Fecha10;
                CostoGastoOperacionTotal.Fecha11 = ProveedorAcreedor.Fecha11 + CostoVentaTotal.Fecha11 + CostoGastoOperacion.Fecha11;
                CostoGastoOperacionTotal.Fecha12 = ProveedorAcreedor.Fecha12 + CostoVentaTotal.Fecha12 + CostoGastoOperacion.Fecha12;
                CostoGastoOperacionTotal.Total = CostoGastoOperacionTotal.Fecha1 + CostoGastoOperacionTotal.Fecha2 + CostoGastoOperacionTotal.Fecha3 + CostoGastoOperacionTotal.Fecha4 + CostoGastoOperacionTotal.Fecha5 + CostoGastoOperacionTotal.Fecha6 + CostoGastoOperacionTotal.Fecha7 + CostoGastoOperacionTotal.Fecha8 + CostoGastoOperacionTotal.Fecha9 + CostoGastoOperacionTotal.Fecha10 + CostoGastoOperacionTotal.Fecha11 + CostoGastoOperacionTotal.Fecha12;

                //Costos de Venta
                EstadoResultadosDTO CostoDeVenta = new EstadoResultadosDTO();
                CostoDeVenta.Concepto = "COSTO DE VENTAS";
                CostoDeVenta.AreaNombre = "COSTO DE VENTAS";
                CostoDeVenta.IdRenglon = 4;
                CostoDeVenta.Fecha1 = CostoVentaTotal.Fecha1;
                CostoDeVenta.Fecha2 = CostoVentaTotal.Fecha2;
                CostoDeVenta.Fecha3 = CostoVentaTotal.Fecha3;
                CostoDeVenta.Fecha4 = CostoVentaTotal.Fecha4;
                CostoDeVenta.Fecha5 = CostoVentaTotal.Fecha5;
                CostoDeVenta.Fecha6 = CostoVentaTotal.Fecha6;
                CostoDeVenta.Fecha7 = CostoVentaTotal.Fecha7;
                CostoDeVenta.Fecha8 = CostoVentaTotal.Fecha8;
                CostoDeVenta.Fecha9 = CostoVentaTotal.Fecha9;
                CostoDeVenta.Fecha10 = CostoVentaTotal.Fecha10;
                CostoDeVenta.Fecha11 = CostoVentaTotal.Fecha11;
                CostoDeVenta.Fecha12 = CostoVentaTotal.Fecha12;
                CostoDeVenta.Total = CostoVentaTotal.Total;

                EstadoResultadosDTO FlujoOperacion = new EstadoResultadosDTO();

                FlujoOperacion.Concepto = "Flujo de Operación";
                FlujoOperacion.AreaNombre = "";
                FlujoOperacion.IdRenglon = 7;
                FlujoOperacion.Fecha1 = IngresosVentas.Fecha1 - CostoGastoOperacionTotal.Fecha1;
                FlujoOperacion.Fecha2 = IngresosVentas.Fecha2 - CostoGastoOperacionTotal.Fecha2;
                FlujoOperacion.Fecha3 = IngresosVentas.Fecha3 - CostoGastoOperacionTotal.Fecha3;
                FlujoOperacion.Fecha4 = IngresosVentas.Fecha4 - CostoGastoOperacionTotal.Fecha4;
                FlujoOperacion.Fecha5 = IngresosVentas.Fecha5 - CostoGastoOperacionTotal.Fecha5;
                FlujoOperacion.Fecha6 = IngresosVentas.Fecha6 - CostoGastoOperacionTotal.Fecha6;
                FlujoOperacion.Fecha7 = IngresosVentas.Fecha7 - CostoGastoOperacionTotal.Fecha7;
                FlujoOperacion.Fecha8 = IngresosVentas.Fecha8 - CostoGastoOperacionTotal.Fecha8;
                FlujoOperacion.Fecha9 = IngresosVentas.Fecha9 - CostoGastoOperacionTotal.Fecha9;
                FlujoOperacion.Fecha10 = IngresosVentas.Fecha10 - CostoGastoOperacionTotal.Fecha10;
                FlujoOperacion.Fecha11 = IngresosVentas.Fecha11 - CostoGastoOperacionTotal.Fecha11;
                FlujoOperacion.Fecha12 = IngresosVentas.Fecha12 - CostoGastoOperacionTotal.Fecha12;
                FlujoOperacion.Total = IngresosVentas.Total - CostoGastoOperacionTotal.Total;


                EstadoResultadosDTO InversionesFisicas = new EstadoResultadosDTO();
                InversionesFisicas.Concepto = "Flujo de Operación";
                InversionesFisicas.AreaNombre = "Inversiones Física";
                InversionesFisicas.IdRenglon = 8;
                InversionesFisicas.Fecha1 = 0;
                InversionesFisicas.Fecha2 = 0;
                InversionesFisicas.Fecha3 = 0;
                InversionesFisicas.Fecha4 = 0;
                InversionesFisicas.Fecha5 = 0;
                InversionesFisicas.Fecha6 = 0;
                InversionesFisicas.Fecha7 = 0;
                InversionesFisicas.Fecha8 = 0;
                InversionesFisicas.Fecha9 = 0;
                InversionesFisicas.Fecha10 = 0;
                InversionesFisicas.Fecha11 = 0;
                InversionesFisicas.Fecha12 = 0;
                InversionesFisicas.Total = 0;

                EstadoResultadosDTO FlujoDespuesInversiones = new EstadoResultadosDTO();
                FlujoDespuesInversiones.Concepto = "Flujo despues de Inversiones";
                FlujoDespuesInversiones.AreaNombre = "";
                FlujoDespuesInversiones.IdRenglon = 9;
                FlujoDespuesInversiones.Fecha1 = FlujoOperacion.Fecha1 - InversionesFisicas.Fecha1;
                FlujoDespuesInversiones.Fecha2 = FlujoOperacion.Fecha2 - InversionesFisicas.Fecha2;
                FlujoDespuesInversiones.Fecha3 = FlujoOperacion.Fecha3 - InversionesFisicas.Fecha3;
                FlujoDespuesInversiones.Fecha4 = FlujoOperacion.Fecha4 - InversionesFisicas.Fecha4;
                FlujoDespuesInversiones.Fecha5 = FlujoOperacion.Fecha5 - InversionesFisicas.Fecha5;
                FlujoDespuesInversiones.Fecha6 = FlujoOperacion.Fecha6 - InversionesFisicas.Fecha6;
                FlujoDespuesInversiones.Fecha7 = FlujoOperacion.Fecha7 - InversionesFisicas.Fecha7;
                FlujoDespuesInversiones.Fecha8 = FlujoOperacion.Fecha8 - InversionesFisicas.Fecha8;
                FlujoDespuesInversiones.Fecha9 = FlujoOperacion.Fecha9 - InversionesFisicas.Fecha9;
                FlujoDespuesInversiones.Fecha10 = FlujoOperacion.Fecha10 - InversionesFisicas.Fecha10;
                FlujoDespuesInversiones.Fecha11 = FlujoOperacion.Fecha11 - InversionesFisicas.Fecha11;
                FlujoDespuesInversiones.Fecha12 = FlujoOperacion.Fecha12 - InversionesFisicas.Fecha12;
                FlujoDespuesInversiones.Total = FlujoOperacion.Total - InversionesFisicas.Total;

                EstadoResultadosDTO InteresesGastoDeuda = new EstadoResultadosDTO();
                InteresesGastoDeuda.Concepto = "Flujo despues de Inversiones";
                InteresesGastoDeuda.AreaNombre = "Intereses y Gastos de la Deuda";
                InteresesGastoDeuda.IdRenglon = 10;
                InteresesGastoDeuda.Fecha1 = 0;
                InteresesGastoDeuda.Fecha2 = 0;
                InteresesGastoDeuda.Fecha3 = 0;
                InteresesGastoDeuda.Fecha4 = 0;
                InteresesGastoDeuda.Fecha5 = 0;
                InteresesGastoDeuda.Fecha6 = 0;
                InteresesGastoDeuda.Fecha7 = 0;
                InteresesGastoDeuda.Fecha8 = 0;
                InteresesGastoDeuda.Fecha9 = 0;
                InteresesGastoDeuda.Fecha10 = 0;
                InteresesGastoDeuda.Fecha11 = 0;
                InteresesGastoDeuda.Fecha12 = 0;
                InteresesGastoDeuda.Total = 0;

                EstadoResultadosDTO FlujoDespuesIntereses = new EstadoResultadosDTO();
                FlujoDespuesIntereses.Concepto = "Flujo despues de Intereses";
                FlujoDespuesIntereses.AreaNombre = "";
                FlujoDespuesIntereses.IdRenglon = 11;
                FlujoDespuesIntereses.Fecha1 = FlujoDespuesInversiones.Fecha1 - InteresesGastoDeuda.Fecha1;
                FlujoDespuesIntereses.Fecha2 = FlujoDespuesInversiones.Fecha2 - InteresesGastoDeuda.Fecha2;
                FlujoDespuesIntereses.Fecha3 = FlujoDespuesInversiones.Fecha3 - InteresesGastoDeuda.Fecha3;
                FlujoDespuesIntereses.Fecha4 = FlujoDespuesInversiones.Fecha4 - InteresesGastoDeuda.Fecha4;
                FlujoDespuesIntereses.Fecha5 = FlujoDespuesInversiones.Fecha5 - InteresesGastoDeuda.Fecha5;
                FlujoDespuesIntereses.Fecha6 = FlujoDespuesInversiones.Fecha6 - InteresesGastoDeuda.Fecha6;
                FlujoDespuesIntereses.Fecha7 = FlujoDespuesInversiones.Fecha7 - InteresesGastoDeuda.Fecha7;
                FlujoDespuesIntereses.Fecha8 = FlujoDespuesInversiones.Fecha8 - InteresesGastoDeuda.Fecha8;
                FlujoDespuesIntereses.Fecha9 = FlujoDespuesInversiones.Fecha9 - InteresesGastoDeuda.Fecha9;
                FlujoDespuesIntereses.Fecha10 = FlujoDespuesInversiones.Fecha10 - InteresesGastoDeuda.Fecha10;
                FlujoDespuesIntereses.Fecha11 = FlujoDespuesInversiones.Fecha11 - InteresesGastoDeuda.Fecha11;
                FlujoDespuesIntereses.Fecha12 = FlujoDespuesInversiones.Fecha12 - InteresesGastoDeuda.Fecha12;
                FlujoDespuesIntereses.Total = FlujoDespuesInversiones.Total - InteresesGastoDeuda.Total;



                var AmortizacionCapital = SaldoDivObj.AmortCapitalCompania;
                //var AmortizacionCapital = 0; //SaldoDivObj.Where(x => x.CONCEPTO.Equals(34)).FirstOrDefault();
                var GastosDiferidos = SaldoDivObj.GastosDiferidos;
                //var GastosDiferidos = 0; //SaldoDivObj.Where(x => x.CONCEPTO.Equals(47)).FirstOrDefault();
                //cambiar cuando el modulo de pagos diversos funcione
                var PasgosEstraordinarios = SaldoDivObj.PagExt;
                if (Escenario.Equals(4))
                {
                    var NocheBuena = ValorizacionObra.Where(x => x.AreaNombre.Contains("MINADO NOCHE BUENA")).FirstOrDefault();
                    var Colorada = ValorizacionObra.Where(x => x.AreaNombre.Contains("MINADO MINA LA COLORADA")).FirstOrDefault();
                    if (NocheBuena != null && Colorada != null)
                    {
                        PasgosEstraordinarios.Mes1 += (NocheBuena.Fecha1 * (decimal)0.35) + Colorada.Fecha1 * (decimal)0.35;
                        PasgosEstraordinarios.Mes2 += (NocheBuena.Fecha2 * (decimal)0.35) + Colorada.Fecha2 * (decimal)0.35;
                        PasgosEstraordinarios.Mes3 += (NocheBuena.Fecha3 * (decimal)0.35) + Colorada.Fecha3 * (decimal)0.35;
                        PasgosEstraordinarios.Mes4 += (NocheBuena.Fecha4 * (decimal)0.35) + Colorada.Fecha4 * (decimal)0.35;
                        PasgosEstraordinarios.Mes5 += (NocheBuena.Fecha5 * (decimal)0.35) + Colorada.Fecha5 * (decimal)0.35;
                        PasgosEstraordinarios.Mes6 += (NocheBuena.Fecha6 * (decimal)0.35) + Colorada.Fecha6 * (decimal)0.35;
                        PasgosEstraordinarios.Mes7 += (NocheBuena.Fecha7 * (decimal)0.35) + Colorada.Fecha7 * (decimal)0.35;
                        PasgosEstraordinarios.Mes8 += (NocheBuena.Fecha8 * (decimal)0.35) + Colorada.Fecha8 * (decimal)0.35;
                        PasgosEstraordinarios.Mes9 += (NocheBuena.Fecha9 * (decimal)0.35) + Colorada.Fecha9 * (decimal)0.35;
                        PasgosEstraordinarios.Mes10 += (NocheBuena.Fecha10 * (decimal)0.35) + Colorada.Fecha10 * (decimal)0.35;
                        PasgosEstraordinarios.Mes11 += (NocheBuena.Fecha11 * (decimal)0.35) + Colorada.Fecha11 * (decimal)0.35;
                        PasgosEstraordinarios.Mes12 += (NocheBuena.Fecha12 * (decimal)0.35) + Colorada.Fecha12 * (decimal)0.35;
                    }
                }
                EstadoResultadosDTO PagosDiversos = new EstadoResultadosDTO();
                PagosDiversos.Concepto = "Flujo despues de Intereses";
                PagosDiversos.AreaNombre = "Pagos Diversos";
                PagosDiversos.IdRenglon = 12;
                PagosDiversos.Fecha1 = (Convert.ToDecimal(PasgosEstraordinarios.Mes1) + AmortizacionCapital.Mes1 + GastosDiferidos.Mes1) / divisor;
                PagosDiversos.Fecha2 = (Convert.ToDecimal(PasgosEstraordinarios.Mes2) + AmortizacionCapital.Mes2 + GastosDiferidos.Mes2) / divisor;
                PagosDiversos.Fecha3 = (Convert.ToDecimal(PasgosEstraordinarios.Mes3) + AmortizacionCapital.Mes3 + GastosDiferidos.Mes3) / divisor;
                PagosDiversos.Fecha4 = (Convert.ToDecimal(PasgosEstraordinarios.Mes4) + AmortizacionCapital.Mes4 + GastosDiferidos.Mes4) / divisor;
                PagosDiversos.Fecha5 = (Convert.ToDecimal(PasgosEstraordinarios.Mes5) + AmortizacionCapital.Mes5 + GastosDiferidos.Mes5) / divisor;
                PagosDiversos.Fecha6 = (Convert.ToDecimal(PasgosEstraordinarios.Mes6) + AmortizacionCapital.Mes6 + GastosDiferidos.Mes6) / divisor;
                PagosDiversos.Fecha7 = (Convert.ToDecimal(PasgosEstraordinarios.Mes7) + AmortizacionCapital.Mes7 + GastosDiferidos.Mes7) / divisor;
                PagosDiversos.Fecha8 = (Convert.ToDecimal(PasgosEstraordinarios.Mes8) + AmortizacionCapital.Mes8 + GastosDiferidos.Mes8) / divisor;
                PagosDiversos.Fecha9 = (Convert.ToDecimal(PasgosEstraordinarios.Mes9) + AmortizacionCapital.Mes9 + GastosDiferidos.Mes9) / divisor;
                PagosDiversos.Fecha10 = (Convert.ToDecimal(PasgosEstraordinarios.Mes10) + AmortizacionCapital.Mes10 + GastosDiferidos.Mes10) / divisor;
                PagosDiversos.Fecha11 = (Convert.ToDecimal(PasgosEstraordinarios.Mes11) + AmortizacionCapital.Mes11 + GastosDiferidos.Mes11) / divisor;
                PagosDiversos.Fecha12 = (Convert.ToDecimal(PasgosEstraordinarios.Mes12) + AmortizacionCapital.Mes12 + GastosDiferidos.Mes12) / divisor;

                PagosDiversos.Total = PagosDiversos.Fecha1 + PagosDiversos.Fecha2 + PagosDiversos.Fecha3 + PagosDiversos.Fecha4 + PagosDiversos.Fecha5 + PagosDiversos.Fecha6 + PagosDiversos.Fecha7 + PagosDiversos.Fecha8 + PagosDiversos.Fecha9 + PagosDiversos.Fecha10 + PagosDiversos.Fecha11 + PagosDiversos.Fecha12;

                var cobroDiversos = _context.tblPro_CobrosDiversos.Where(x => x.Mes.Equals(mes) && x.Anio.Equals(anio)).OrderByDescending(x => x.id).FirstOrDefault();
                var objCobroDiv = JsonConvert.DeserializeObject<CobrosDivDTO>(cobroDiversos.CadenaJson);

                EstadoResultadosDTO RCyCD = new EstadoResultadosDTO();
                //('COBROS DIV'!E16+'COBROS DIV'!E25)/MENU!$D$10
                RCyCD.Concepto = "Flujo despues de Intereses";
                RCyCD.AreaNombre = "Recup. Cartera y Cobros Diversos";
                RCyCD.IdRenglon = 13;
                RCyCD.Fecha1 = (Convert.ToDecimal(objCobroDiv.ln2ImporteAmortizar1.Mes1) + Convert.ToDecimal(objCobroDiv.ln5CxCImporteAmortizar.Mes1)) / divisor;
                RCyCD.Fecha2 = (Convert.ToDecimal(objCobroDiv.ln2ImporteAmortizar1.Mes2) + Convert.ToDecimal(objCobroDiv.ln5CxCImporteAmortizar.Mes2)) / divisor;
                RCyCD.Fecha3 = (Convert.ToDecimal(objCobroDiv.ln2ImporteAmortizar1.Mes3) + Convert.ToDecimal(objCobroDiv.ln5CxCImporteAmortizar.Mes3)) / divisor;
                RCyCD.Fecha4 = (Convert.ToDecimal(objCobroDiv.ln2ImporteAmortizar1.Mes4) + Convert.ToDecimal(objCobroDiv.ln5CxCImporteAmortizar.Mes4)) / divisor;
                RCyCD.Fecha5 = (Convert.ToDecimal(objCobroDiv.ln2ImporteAmortizar1.Mes5) + Convert.ToDecimal(objCobroDiv.ln5CxCImporteAmortizar.Mes5)) / divisor;
                RCyCD.Fecha6 = (Convert.ToDecimal(objCobroDiv.ln2ImporteAmortizar1.Mes6) + Convert.ToDecimal(objCobroDiv.ln5CxCImporteAmortizar.Mes6)) / divisor;
                RCyCD.Fecha7 = (Convert.ToDecimal(objCobroDiv.ln2ImporteAmortizar1.Mes7) + Convert.ToDecimal(objCobroDiv.ln5CxCImporteAmortizar.Mes7)) / divisor;
                RCyCD.Fecha8 = (Convert.ToDecimal(objCobroDiv.ln2ImporteAmortizar1.Mes8) + Convert.ToDecimal(objCobroDiv.ln5CxCImporteAmortizar.Mes8)) / divisor;
                RCyCD.Fecha9 = (Convert.ToDecimal(objCobroDiv.ln2ImporteAmortizar1.Mes9) + Convert.ToDecimal(objCobroDiv.ln5CxCImporteAmortizar.Mes9)) / divisor;
                RCyCD.Fecha10 = (Convert.ToDecimal(objCobroDiv.ln2ImporteAmortizar1.Mes10) + Convert.ToDecimal(objCobroDiv.ln5CxCImporteAmortizar.Mes10)) / divisor;
                RCyCD.Fecha11 = (Convert.ToDecimal(objCobroDiv.ln2ImporteAmortizar1.Mes11) + Convert.ToDecimal(objCobroDiv.ln5CxCImporteAmortizar.Mes11)) / divisor;
                RCyCD.Fecha12 = (Convert.ToDecimal(objCobroDiv.ln2ImporteAmortizar1.Mes12) + Convert.ToDecimal(objCobroDiv.ln5CxCImporteAmortizar.Mes12)) / divisor;
                RCyCD.Total = RCyCD.Fecha1 + RCyCD.Fecha2 + RCyCD.Fecha3 + RCyCD.Fecha4 + RCyCD.Fecha5 + RCyCD.Fecha6 + RCyCD.Fecha7 + RCyCD.Fecha8 + RCyCD.Fecha9 + RCyCD.Fecha10 + RCyCD.Fecha11 + RCyCD.Fecha12;

                EstadoResultadosDTO DeCaja = new EstadoResultadosDTO();
                DeCaja.Concepto = "(Déficit) o Superávit de Caja ";
                DeCaja.AreaNombre = "";
                DeCaja.IdRenglon = 14;
                DeCaja.Fecha1 = FlujoDespuesIntereses.Fecha1 - PagosDiversos.Fecha1 + RCyCD.Fecha1; //Costo de venta + gasto administrativo + Pago proveedor acreedor
                DeCaja.Fecha2 = FlujoDespuesIntereses.Fecha2 - PagosDiversos.Fecha2 + RCyCD.Fecha2;
                DeCaja.Fecha3 = FlujoDespuesIntereses.Fecha3 - PagosDiversos.Fecha3 + RCyCD.Fecha3;
                DeCaja.Fecha4 = FlujoDespuesIntereses.Fecha4 - PagosDiversos.Fecha4 + RCyCD.Fecha4;
                DeCaja.Fecha5 = FlujoDespuesIntereses.Fecha5 - PagosDiversos.Fecha5 + RCyCD.Fecha5;
                DeCaja.Fecha6 = FlujoDespuesIntereses.Fecha6 - PagosDiversos.Fecha6 + RCyCD.Fecha6;
                DeCaja.Fecha7 = FlujoDespuesIntereses.Fecha7 - PagosDiversos.Fecha7 + RCyCD.Fecha7;
                DeCaja.Fecha8 = FlujoDespuesIntereses.Fecha8 - PagosDiversos.Fecha8 + RCyCD.Fecha8;
                DeCaja.Fecha9 = FlujoDespuesIntereses.Fecha9 - PagosDiversos.Fecha9 + RCyCD.Fecha9;
                DeCaja.Fecha10 = FlujoDespuesIntereses.Fecha10 - PagosDiversos.Fecha10 + RCyCD.Fecha10;
                DeCaja.Fecha11 = FlujoDespuesIntereses.Fecha11 - PagosDiversos.Fecha11 + RCyCD.Fecha11;
                DeCaja.Fecha12 = FlujoDespuesIntereses.Fecha12 - PagosDiversos.Fecha12 + RCyCD.Fecha12;
                DeCaja.Total = FlujoDespuesIntereses.Total - PagosDiversos.Total + RCyCD.Total;


                //('COBROS DIV'!E29)/MENU!$D$10
                EstadoResultadosDTO AportacionesCapital = new EstadoResultadosDTO();
                AportacionesCapital.Concepto = "Aportaciones de Capital";
                AportacionesCapital.AreaNombre = "";
                AportacionesCapital.IdRenglon = 15;
                AportacionesCapital.Fecha1 = 0;
                AportacionesCapital.Fecha2 = 0;
                AportacionesCapital.Fecha3 = 0;
                AportacionesCapital.Fecha4 = 0;
                AportacionesCapital.Fecha5 = 0;
                AportacionesCapital.Fecha6 = 0;
                AportacionesCapital.Fecha7 = 0;
                AportacionesCapital.Fecha8 = 0;
                AportacionesCapital.Fecha9 = 0;
                AportacionesCapital.Fecha10 = 0;
                AportacionesCapital.Fecha11 = 0;
                AportacionesCapital.Fecha12 = 0;
                AportacionesCapital.Total = 0;

                EstadoResultadosDTO CreditosBancarios = new EstadoResultadosDTO();
                CreditosBancarios.Concepto = "Disposición de Créditos";
                CreditosBancarios.AreaNombre = "Créditos Bancarios MN";
                CreditosBancarios.IdRenglon = 16;
                CreditosBancarios.Fecha1 = 0;
                CreditosBancarios.Fecha2 = 0;
                CreditosBancarios.Fecha3 = 0;
                CreditosBancarios.Fecha4 = 0;
                CreditosBancarios.Fecha5 = 0;
                CreditosBancarios.Fecha6 = 0;
                CreditosBancarios.Fecha7 = 0;
                CreditosBancarios.Fecha8 = 0;
                CreditosBancarios.Fecha9 = 0;
                CreditosBancarios.Fecha10 = 0;
                CreditosBancarios.Fecha11 = 0;
                CreditosBancarios.Fecha12 = 0;
                CreditosBancarios.Total = 0;

                EstadoResultadosDTO Reservas = new EstadoResultadosDTO();
                Reservas.Concepto = "RESERVAS";
                Reservas.AreaNombre = "";
                Reservas.IdRenglon = 17;
                Reservas.Fecha1 = 0;


                decimal EfectivoInversionesAjustado = EfectivoInversionesTempObj.Where(x => x.Concepto.Contains("Efectivo e Inversiones Temporales")).Select(x => x.Saldo).FirstOrDefault();
                //decimal InicialFinal = EfectivoInversionesTempObj.Where(x => x.Concepto.Contains("")).Select(x => x.Saldo).FirstOrDefault();
                decimal InicialFinal = 0; //E43



                EstadoResultadosDTO SaldoInicial = new EstadoResultadosDTO();
                SaldoInicial.Concepto = "Saldo Inicial";
                SaldoInicial.IdRenglon = 20;
                SaldoInicial.AreaNombre = "";

                SaldoFinalFlujoEfectivo.Concepto = "Saldo Final"; // FujugoEfectivo
                SaldoFinalFlujoEfectivo.AreaNombre = ""; // FujugoEfectivo
                SaldoFinalFlujoEfectivo.IdRenglon = 21;

                SaldoInicial.Fecha1 = (EfectivoInversionesAjustado - InicialFinal) / divisor;
                SaldoFinalFlujoEfectivo.Fecha1 = DeCaja.Fecha1 + AportacionesCapital.Fecha1 - CreditosBancarios.Fecha1 + SaldoInicial.Fecha1;

                SaldoInicial.Fecha2 = SaldoFinalFlujoEfectivo.Fecha1;
                SaldoFinalFlujoEfectivo.Fecha2 = DeCaja.Fecha2 + AportacionesCapital.Fecha2 - CreditosBancarios.Fecha2 + SaldoInicial.Fecha2;

                SaldoInicial.Fecha3 = SaldoFinalFlujoEfectivo.Fecha2;
                SaldoFinalFlujoEfectivo.Fecha3 = DeCaja.Fecha3 + AportacionesCapital.Fecha3 - CreditosBancarios.Fecha3 + SaldoInicial.Fecha3;
                SaldoInicial.Fecha4 = SaldoFinalFlujoEfectivo.Fecha3;
                SaldoFinalFlujoEfectivo.Fecha4 = DeCaja.Fecha4 + AportacionesCapital.Fecha4 - CreditosBancarios.Fecha4 + SaldoInicial.Fecha4;
                SaldoInicial.Fecha5 = SaldoFinalFlujoEfectivo.Fecha4;
                SaldoFinalFlujoEfectivo.Fecha5 = DeCaja.Fecha5 + AportacionesCapital.Fecha5 - CreditosBancarios.Fecha5 + SaldoInicial.Fecha5;
                SaldoInicial.Fecha6 = SaldoFinalFlujoEfectivo.Fecha5;
                SaldoFinalFlujoEfectivo.Fecha6 = DeCaja.Fecha6 + AportacionesCapital.Fecha6 - CreditosBancarios.Fecha6 + SaldoInicial.Fecha6;
                SaldoInicial.Fecha7 = SaldoFinalFlujoEfectivo.Fecha6;
                SaldoFinalFlujoEfectivo.Fecha7 = DeCaja.Fecha7 + AportacionesCapital.Fecha7 - CreditosBancarios.Fecha7 + SaldoInicial.Fecha7;
                SaldoInicial.Fecha8 = SaldoFinalFlujoEfectivo.Fecha7;
                SaldoFinalFlujoEfectivo.Fecha8 = DeCaja.Fecha8 + AportacionesCapital.Fecha8 - CreditosBancarios.Fecha8 + SaldoInicial.Fecha8;
                SaldoInicial.Fecha9 = SaldoFinalFlujoEfectivo.Fecha8;
                SaldoFinalFlujoEfectivo.Fecha9 = DeCaja.Fecha9 + AportacionesCapital.Fecha9 - CreditosBancarios.Fecha9 + SaldoInicial.Fecha9;
                SaldoInicial.Fecha10 = SaldoFinalFlujoEfectivo.Fecha9;
                SaldoFinalFlujoEfectivo.Fecha10 = DeCaja.Fecha10 + AportacionesCapital.Fecha10 - CreditosBancarios.Fecha10 + SaldoInicial.Fecha10;
                SaldoInicial.Fecha11 = SaldoFinalFlujoEfectivo.Fecha10;
                SaldoFinalFlujoEfectivo.Fecha11 = DeCaja.Fecha11 + AportacionesCapital.Fecha11 - CreditosBancarios.Fecha11 + SaldoInicial.Fecha11;
                SaldoInicial.Fecha12 = SaldoFinalFlujoEfectivo.Fecha11;
                SaldoFinalFlujoEfectivo.Fecha12 = DeCaja.Fecha12 + AportacionesCapital.Fecha12 - CreditosBancarios.Fecha12 + SaldoInicial.Fecha12;
                SaldoInicial.Fecha12 = SaldoFinalFlujoEfectivo.Fecha11;
                SaldoInicial.Total = SaldoInicial.Fecha1;


                SaldoFinalFlujoEfectivo.Total = DeCaja.Total + AportacionesCapital.Total - CreditosBancarios.Total + SaldoInicial.Total;


                //SaldoFinalFlujoEfectivo.Fecha1 = 74071;
                //SaldoFinalFlujoEfectivo.Fecha2 = 176346;
                //SaldoFinalFlujoEfectivo.Fecha3 = 223683;
                //SaldoFinalFlujoEfectivo.Fecha4 = 235985;
                //SaldoFinalFlujoEfectivo.Fecha5 = 239544;
                //SaldoFinalFlujoEfectivo.Fecha6 = 242993;
                //SaldoFinalFlujoEfectivo.Fecha7 = 245479;
                //SaldoFinalFlujoEfectivo.Fecha8 = 239714;
                //SaldoFinalFlujoEfectivo.Fecha9 = 243693;
                //SaldoFinalFlujoEfectivo.Fecha10 = 249036;
                //SaldoFinalFlujoEfectivo.Fecha11 = 253019;
                //SaldoFinalFlujoEfectivo.Fecha12 = 256853;

                //Reporte3
                EstadoResultadosDTO EfectivoInversiones = new EstadoResultadosDTO();
                EfectivoInversiones.Concepto = "ACTIVO CIRCULANTE";
                EfectivoInversiones.AreaNombre = "Efectivo e Inversiones temporales";
                EfectivoInversiones.IdRenglon = 1;
                EfectivoInversiones.Fecha1 = SaldoFinalFlujoEfectivo.Fecha1;
                EfectivoInversiones.Fecha2 = SaldoFinalFlujoEfectivo.Fecha2;
                EfectivoInversiones.Fecha3 = SaldoFinalFlujoEfectivo.Fecha3;
                EfectivoInversiones.Fecha4 = SaldoFinalFlujoEfectivo.Fecha4;
                EfectivoInversiones.Fecha5 = SaldoFinalFlujoEfectivo.Fecha5;
                EfectivoInversiones.Fecha6 = SaldoFinalFlujoEfectivo.Fecha6;
                EfectivoInversiones.Fecha7 = SaldoFinalFlujoEfectivo.Fecha7;
                EfectivoInversiones.Fecha8 = SaldoFinalFlujoEfectivo.Fecha8;
                EfectivoInversiones.Fecha9 = SaldoFinalFlujoEfectivo.Fecha9;
                EfectivoInversiones.Fecha10 = SaldoFinalFlujoEfectivo.Fecha10;
                EfectivoInversiones.Fecha11 = SaldoFinalFlujoEfectivo.Fecha11;
                EfectivoInversiones.Fecha12 = SaldoFinalFlujoEfectivo.Fecha12;

                EstadoResultadosDTO CuentasPorCobrar = new EstadoResultadosDTO();
                CuentasPorCobrar.Concepto = "ACTIVO CIRCULANTE";
                CuentasPorCobrar.AreaNombre = "CUENTAS POR COBRAR";




                var ClientesIniciales = EfectivoInversionesTempObj.Where(x => x.Concepto.Contains("Clientes")).Select(x => x.Saldo).ToList();

                EstadoResultadosDTO Clientes = new EstadoResultadosDTO();
                Clientes.Concepto = "CUENTAS POR COBRAR";
                Clientes.AreaNombre = "Clientes";
                Clientes.IdRenglon = 2;
                Clientes.Fecha1 = ((Convert.ToDecimal(ClientesIniciales[0]) - Convert.ToDecimal(objCobroDiv.ln2ImporteAmortizar1.Mes1)) / divisor);
                Clientes.Fecha2 = Clientes.Fecha1 - (Convert.ToDecimal(objCobroDiv.ln2ImporteAmortizar1.Mes2) / divisor);
                Clientes.Fecha3 = Clientes.Fecha2 - (Convert.ToDecimal(objCobroDiv.ln2ImporteAmortizar1.Mes3) / divisor);
                Clientes.Fecha4 = Clientes.Fecha3 - (Convert.ToDecimal(objCobroDiv.ln2ImporteAmortizar1.Mes4) / divisor);
                Clientes.Fecha5 = Clientes.Fecha4 - (Convert.ToDecimal(objCobroDiv.ln2ImporteAmortizar1.Mes5) / divisor);
                Clientes.Fecha6 = Clientes.Fecha5 - (Convert.ToDecimal(objCobroDiv.ln2ImporteAmortizar1.Mes6) / divisor);
                Clientes.Fecha7 = Clientes.Fecha6 - (Convert.ToDecimal(objCobroDiv.ln2ImporteAmortizar1.Mes7) / divisor);
                Clientes.Fecha8 = Clientes.Fecha7 - (Convert.ToDecimal(objCobroDiv.ln2ImporteAmortizar1.Mes8) / divisor);
                Clientes.Fecha9 = Clientes.Fecha8 - (Convert.ToDecimal(objCobroDiv.ln2ImporteAmortizar1.Mes9) / divisor);
                Clientes.Fecha10 = Clientes.Fecha9 - (Convert.ToDecimal(objCobroDiv.ln2ImporteAmortizar1.Mes10) / divisor);
                Clientes.Fecha11 = Clientes.Fecha10 - (Convert.ToDecimal(objCobroDiv.ln2ImporteAmortizar1.Mes11) / divisor);
                Clientes.Fecha12 = Clientes.Fecha11 - (Convert.ToDecimal(objCobroDiv.ln2ImporteAmortizar1.Mes12) / divisor);


                var OtrosDeudoresIniciales = EfectivoInversionesTempObj.Where(x => x.Concepto.Contains("Otros Deudores")).Select(x => x.Saldo).ToList();
                EstadoResultadosDTO OtrosDeudores = new EstadoResultadosDTO();
                OtrosDeudores.Concepto = "CUENTAS POR COBRAR";
                OtrosDeudores.AreaNombre = "Otros Deudores";
                OtrosDeudores.IdRenglon = 3;
                OtrosDeudores.Fecha1 = Convert.ToDecimal(OtrosDeudoresIniciales[0]) / divisor;
                OtrosDeudores.Fecha2 = OtrosDeudores.Fecha1;
                OtrosDeudores.Fecha3 = OtrosDeudores.Fecha2;
                OtrosDeudores.Fecha4 = OtrosDeudores.Fecha3;
                OtrosDeudores.Fecha5 = OtrosDeudores.Fecha4;
                OtrosDeudores.Fecha6 = OtrosDeudores.Fecha5;
                OtrosDeudores.Fecha7 = OtrosDeudores.Fecha6;
                OtrosDeudores.Fecha8 = OtrosDeudores.Fecha7;
                OtrosDeudores.Fecha9 = OtrosDeudores.Fecha8;
                OtrosDeudores.Fecha10 = OtrosDeudores.Fecha9;
                OtrosDeudores.Fecha11 = OtrosDeudores.Fecha10;
                OtrosDeudores.Fecha12 = OtrosDeudores.Fecha11;

                EstadoResultadosDTO SumaCuentasPorCobrar = new EstadoResultadosDTO();
                SumaCuentasPorCobrar.Concepto = "CUENTAS POR COBRAR";
                SumaCuentasPorCobrar.AreaNombre = "Suma cuentas por cobrar";
                SumaCuentasPorCobrar.IdRenglon = 4;
                SumaCuentasPorCobrar.Fecha1 = Clientes.Fecha1 + OtrosDeudores.Fecha1;
                SumaCuentasPorCobrar.Fecha2 = Clientes.Fecha2 + OtrosDeudores.Fecha2;
                SumaCuentasPorCobrar.Fecha3 = Clientes.Fecha3 + OtrosDeudores.Fecha3;
                SumaCuentasPorCobrar.Fecha4 = Clientes.Fecha4 + OtrosDeudores.Fecha4;
                SumaCuentasPorCobrar.Fecha5 = Clientes.Fecha5 + OtrosDeudores.Fecha5;
                SumaCuentasPorCobrar.Fecha6 = Clientes.Fecha6 + OtrosDeudores.Fecha6;
                SumaCuentasPorCobrar.Fecha7 = Clientes.Fecha7 + OtrosDeudores.Fecha7;
                SumaCuentasPorCobrar.Fecha8 = Clientes.Fecha8 + OtrosDeudores.Fecha8;
                SumaCuentasPorCobrar.Fecha9 = Clientes.Fecha9 + OtrosDeudores.Fecha9;
                SumaCuentasPorCobrar.Fecha10 = Clientes.Fecha10 + OtrosDeudores.Fecha10;
                SumaCuentasPorCobrar.Fecha11 = Clientes.Fecha11 + OtrosDeudores.Fecha11;
                SumaCuentasPorCobrar.Fecha12 = Clientes.Fecha12 + OtrosDeudores.Fecha12;

                var Inventario = EfectivoInversionesTempObj.Where(x => x.Concepto.Contains("INVENTARIOS")).Select(x => x.Saldo).ToList();

                EstadoResultadosDTO Inventarios = new EstadoResultadosDTO();
                Inventarios.Concepto = "INVENTARIOS";
                Inventarios.AreaNombre = "";
                Inventarios.IdRenglon = 5;
                Inventarios.Fecha1 = Convert.ToDecimal(Inventario[0]) / divisor;
                Inventarios.Fecha2 = Inventarios.Fecha1;
                Inventarios.Fecha3 = Inventarios.Fecha2;
                Inventarios.Fecha4 = Inventarios.Fecha3;
                Inventarios.Fecha5 = Inventarios.Fecha4;
                Inventarios.Fecha6 = Inventarios.Fecha5;
                Inventarios.Fecha7 = Inventarios.Fecha6;
                Inventarios.Fecha8 = Inventarios.Fecha7;
                Inventarios.Fecha9 = Inventarios.Fecha8;
                Inventarios.Fecha10 = Inventarios.Fecha9;
                Inventarios.Fecha11 = Inventarios.Fecha10;
                Inventarios.Fecha12 = Inventarios.Fecha11;

                var otroActivoInicial = EfectivoInversionesTempObj.Where(x => x.Concepto.Contains("OTROS ACTIVOS")).Select(x => x.Saldo).ToList();

                EstadoResultadosDTO OtrosActivos = new EstadoResultadosDTO();
                OtrosActivos.Concepto = "OTROS ACTIVOS";
                OtrosActivos.AreaNombre = "";
                OtrosActivos.IdRenglon = 6;
                OtrosActivos.Fecha1 = Convert.ToDecimal(otroActivoInicial[0]) / divisor;
                OtrosActivos.Fecha2 = OtrosActivos.Fecha1;
                OtrosActivos.Fecha3 = OtrosActivos.Fecha2;
                OtrosActivos.Fecha4 = OtrosActivos.Fecha3;
                OtrosActivos.Fecha5 = OtrosActivos.Fecha4;
                OtrosActivos.Fecha6 = OtrosActivos.Fecha5;
                OtrosActivos.Fecha7 = OtrosActivos.Fecha6;
                OtrosActivos.Fecha8 = OtrosActivos.Fecha7;
                OtrosActivos.Fecha9 = OtrosActivos.Fecha8;
                OtrosActivos.Fecha10 = OtrosActivos.Fecha9;
                OtrosActivos.Fecha11 = OtrosActivos.Fecha10;
                OtrosActivos.Fecha12 = OtrosActivos.Fecha11;

                EstadoResultadosDTO SumaActivoCirculante = new EstadoResultadosDTO();
                SumaActivoCirculante.Concepto = "Suma el Activo Circulante";
                SumaActivoCirculante.AreaNombre = "";
                SumaActivoCirculante.IdRenglon = 7;
                SumaActivoCirculante.Fecha1 = EfectivoInversiones.Fecha1 + SumaCuentasPorCobrar.Fecha1 + Inventarios.Fecha1 + OtrosActivos.Fecha1;
                SumaActivoCirculante.Fecha2 = EfectivoInversiones.Fecha2 + SumaCuentasPorCobrar.Fecha2 + Inventarios.Fecha2 + OtrosActivos.Fecha2;
                SumaActivoCirculante.Fecha3 = EfectivoInversiones.Fecha3 + SumaCuentasPorCobrar.Fecha3 + Inventarios.Fecha3 + OtrosActivos.Fecha3;
                SumaActivoCirculante.Fecha4 = EfectivoInversiones.Fecha4 + SumaCuentasPorCobrar.Fecha4 + Inventarios.Fecha4 + OtrosActivos.Fecha4;
                SumaActivoCirculante.Fecha5 = EfectivoInversiones.Fecha5 + SumaCuentasPorCobrar.Fecha5 + Inventarios.Fecha5 + OtrosActivos.Fecha5;
                SumaActivoCirculante.Fecha6 = EfectivoInversiones.Fecha6 + SumaCuentasPorCobrar.Fecha6 + Inventarios.Fecha6 + OtrosActivos.Fecha6;
                SumaActivoCirculante.Fecha7 = EfectivoInversiones.Fecha7 + SumaCuentasPorCobrar.Fecha7 + Inventarios.Fecha7 + OtrosActivos.Fecha7;
                SumaActivoCirculante.Fecha8 = EfectivoInversiones.Fecha8 + SumaCuentasPorCobrar.Fecha8 + Inventarios.Fecha8 + OtrosActivos.Fecha8;
                SumaActivoCirculante.Fecha9 = EfectivoInversiones.Fecha9 + SumaCuentasPorCobrar.Fecha9 + Inventarios.Fecha9 + OtrosActivos.Fecha9;
                SumaActivoCirculante.Fecha10 = EfectivoInversiones.Fecha10 + SumaCuentasPorCobrar.Fecha10 + Inventarios.Fecha10 + OtrosActivos.Fecha10;
                SumaActivoCirculante.Fecha11 = EfectivoInversiones.Fecha11 + SumaCuentasPorCobrar.Fecha11 + Inventarios.Fecha11 + OtrosActivos.Fecha11;
                SumaActivoCirculante.Fecha12 = EfectivoInversiones.Fecha12 + SumaCuentasPorCobrar.Fecha12 + Inventarios.Fecha12 + OtrosActivos.Fecha12;

                var activoNoCirculanteInicial = EfectivoInversionesTempObj.Where(x => x.Concepto.Contains("ACTIVO NO CIRCULANTE")).Select(x => x.Saldo).ToList();

                EstadoResultadosDTO ActivoNoCirculante = new EstadoResultadosDTO();
                ActivoNoCirculante.Concepto = "ACTIVO NO CIRCULANTE";
                ActivoNoCirculante.AreaNombre = "";
                ActivoNoCirculante.IdRenglon = 8;
                ActivoNoCirculante.Fecha1 = Convert.ToDecimal(activoNoCirculanteInicial[0]) / divisor;
                ActivoNoCirculante.Fecha2 = ActivoNoCirculante.Fecha1;
                ActivoNoCirculante.Fecha3 = ActivoNoCirculante.Fecha2;
                ActivoNoCirculante.Fecha4 = ActivoNoCirculante.Fecha3;
                ActivoNoCirculante.Fecha5 = ActivoNoCirculante.Fecha4;
                ActivoNoCirculante.Fecha6 = ActivoNoCirculante.Fecha5;
                ActivoNoCirculante.Fecha7 = ActivoNoCirculante.Fecha6;
                ActivoNoCirculante.Fecha8 = ActivoNoCirculante.Fecha7;
                ActivoNoCirculante.Fecha9 = ActivoNoCirculante.Fecha8;
                ActivoNoCirculante.Fecha10 = ActivoNoCirculante.Fecha9;
                ActivoNoCirculante.Fecha11 = ActivoNoCirculante.Fecha10;
                ActivoNoCirculante.Fecha12 = ActivoNoCirculante.Fecha11;

                var ActivoFijo = _context.tblPro_ActivoFijo.Where(x => x.Mes.Equals(mes) && x.Anio.Equals(anio)).OrderByDescending(x => x.id).FirstOrDefault();
                var ActivoFijoObj = JsonConvert.DeserializeObject<List<ActivoFijoDTO>>(EfectivoInversionesTemp.CadenaJson);


                var ActivoExistente = EfectivoInversionesTempObj.Where(x => x.Concepto.Contains("Activo ya Existente")).Select(x => x.Saldo).ToList();
                var ActivoNeto = EfectivoInversionesTempObj.Where(x => x.Concepto.Contains("Activo Nuevo")).Select(x => x.Saldo).ToList();
                var ActivoFijoTotal = ActivoFijoObj.Where(x => x.Concepto.Contains("Suma el Activo Total")).FirstOrDefault();
                decimal maquinaria = 0; //=('SALDOS INIC'!M16+'SALDOS INIC'!M17+'A. FIJO'!G48-'$ MAQ'!F170-'$ MAQ'!G170)/MENU!$D$10


                EstadoResultadosDTO ActivoFijoNeto = new EstadoResultadosDTO();
                ActivoFijoNeto.Concepto = "ACTIVO FIJO-NETO";
                ActivoFijoNeto.AreaNombre = "";
                ActivoFijoNeto.IdRenglon = 9;
                ActivoFijoNeto.Fecha1 = (Convert.ToDecimal(ActivoExistente[0]) + Convert.ToDecimal(ActivoNeto[0]) + Convert.ToDecimal(ActivoFijoTotal.Fecha1) - maquinaria - maquinaria) / divisor;
                ActivoFijoNeto.Fecha2 = ActivoFijoNeto.Fecha1 + (Convert.ToDecimal(ActivoFijoTotal.Fecha2) - maquinaria - maquinaria);
                ActivoFijoNeto.Fecha3 = ActivoFijoNeto.Fecha2 + (Convert.ToDecimal(ActivoFijoTotal.Fecha3) - maquinaria - maquinaria);
                ActivoFijoNeto.Fecha4 = ActivoFijoNeto.Fecha3 + (Convert.ToDecimal(ActivoFijoTotal.Fecha4) - maquinaria - maquinaria);
                ActivoFijoNeto.Fecha5 = ActivoFijoNeto.Fecha4 + (Convert.ToDecimal(ActivoFijoTotal.Fecha5) - maquinaria - maquinaria);
                ActivoFijoNeto.Fecha6 = ActivoFijoNeto.Fecha5 + (Convert.ToDecimal(ActivoFijoTotal.Fecha6) - maquinaria - maquinaria);
                ActivoFijoNeto.Fecha7 = ActivoFijoNeto.Fecha6 + (Convert.ToDecimal(ActivoFijoTotal.Fecha7) - maquinaria - maquinaria);
                ActivoFijoNeto.Fecha8 = ActivoFijoNeto.Fecha7 + (Convert.ToDecimal(ActivoFijoTotal.Fecha8) - maquinaria - maquinaria);
                ActivoFijoNeto.Fecha9 = ActivoFijoNeto.Fecha8 + (Convert.ToDecimal(ActivoFijoTotal.Fecha9) - maquinaria - maquinaria);
                ActivoFijoNeto.Fecha10 = ActivoFijoNeto.Fecha9 + (Convert.ToDecimal(ActivoFijoTotal.Fecha10) - maquinaria - maquinaria);
                ActivoFijoNeto.Fecha11 = ActivoFijoNeto.Fecha10 + (Convert.ToDecimal(ActivoFijoTotal.Fecha11) - maquinaria - maquinaria);
                ActivoFijoNeto.Fecha12 = ActivoFijoNeto.Fecha11 + (Convert.ToDecimal(ActivoFijoTotal.Fecha12) - maquinaria - maquinaria);

                var ActivoDiferidoM18 = EfectivoInversionesTempObj.Where(x => x.Concepto.Contains("ACTIVO DIFERIDO")).Select(x => x.Saldo).ToList();
                var JPagosDiveros = _context.tblPro_PagosDiversos.OrderByDescending(x => x.id).FirstOrDefault(x => x.Mes.Equals(mes) && anio.Equals(anio));
                var PagosDiverosObj = Newtonsoft.Json.JsonConvert.DeserializeObject<PagosDivDTO>(JPagosDiveros.CadenaJson);

                EstadoResultadosDTO ActivoDiferido = new EstadoResultadosDTO();
                ActivoDiferido.Concepto = "ACTIVO DIFERIDO";
                ActivoDiferido.AreaNombre = "";
                ActivoDiferido.IdRenglon = 10;
                ActivoDiferido.Fecha1 = (Convert.ToDecimal(ActivoDiferidoM18[0]) + PagosDiverosObj.GastosDiferidos.Mes1) / divisor;
                ActivoDiferido.Fecha2 = ActivoDiferido.Fecha1 + (PagosDiverosObj.GastosDiferidos.Mes2 / divisor);
                ActivoDiferido.Fecha3 = ActivoDiferido.Fecha2 + (PagosDiverosObj.GastosDiferidos.Mes3 / divisor);
                ActivoDiferido.Fecha4 = ActivoDiferido.Fecha3 + (PagosDiverosObj.GastosDiferidos.Mes4 / divisor);
                ActivoDiferido.Fecha5 = ActivoDiferido.Fecha4 + (PagosDiverosObj.GastosDiferidos.Mes5 / divisor);
                ActivoDiferido.Fecha6 = ActivoDiferido.Fecha5 + (PagosDiverosObj.GastosDiferidos.Mes6 / divisor);
                ActivoDiferido.Fecha7 = ActivoDiferido.Fecha6 + (PagosDiverosObj.GastosDiferidos.Mes7 / divisor);
                ActivoDiferido.Fecha8 = ActivoDiferido.Fecha7 + (PagosDiverosObj.GastosDiferidos.Mes8 / divisor);
                ActivoDiferido.Fecha9 = ActivoDiferido.Fecha8 + (PagosDiverosObj.GastosDiferidos.Mes9 / divisor);
                ActivoDiferido.Fecha10 = ActivoDiferido.Fecha9 + (PagosDiverosObj.GastosDiferidos.Mes10 / divisor);
                ActivoDiferido.Fecha11 = ActivoDiferido.Fecha10 + (PagosDiverosObj.GastosDiferidos.Mes11 / divisor);
                ActivoDiferido.Fecha12 = ActivoDiferido.Fecha11 + (PagosDiverosObj.GastosDiferidos.Mes12 / divisor);


                EstadoResultadosDTO SumaActivoTotal = new EstadoResultadosDTO();
                SumaActivoTotal.Concepto = "ACTIVO DIFERIDO";
                SumaActivoTotal.AreaNombre = "Suma el activo total";
                SumaActivoTotal.IdRenglon = 11;
                SumaActivoTotal.Fecha1 = SumaActivoCirculante.Fecha1 + ActivoNoCirculante.Fecha1 + ActivoFijoNeto.Fecha1 + ActivoDiferido.Fecha1;
                SumaActivoTotal.Fecha2 = SumaActivoCirculante.Fecha2 + ActivoNoCirculante.Fecha2 + ActivoFijoNeto.Fecha2 + ActivoDiferido.Fecha2;
                SumaActivoTotal.Fecha3 = SumaActivoCirculante.Fecha3 + ActivoNoCirculante.Fecha3 + ActivoFijoNeto.Fecha3 + ActivoDiferido.Fecha3;
                SumaActivoTotal.Fecha4 = SumaActivoCirculante.Fecha4 + ActivoNoCirculante.Fecha4 + ActivoFijoNeto.Fecha4 + ActivoDiferido.Fecha4;
                SumaActivoTotal.Fecha5 = SumaActivoCirculante.Fecha5 + ActivoNoCirculante.Fecha5 + ActivoFijoNeto.Fecha5 + ActivoDiferido.Fecha5;
                SumaActivoTotal.Fecha6 = SumaActivoCirculante.Fecha6 + ActivoNoCirculante.Fecha6 + ActivoFijoNeto.Fecha6 + ActivoDiferido.Fecha6;
                SumaActivoTotal.Fecha7 = SumaActivoCirculante.Fecha7 + ActivoNoCirculante.Fecha7 + ActivoFijoNeto.Fecha7 + ActivoDiferido.Fecha7;
                SumaActivoTotal.Fecha8 = SumaActivoCirculante.Fecha8 + ActivoNoCirculante.Fecha8 + ActivoFijoNeto.Fecha8 + ActivoDiferido.Fecha8;
                SumaActivoTotal.Fecha9 = SumaActivoCirculante.Fecha9 + ActivoNoCirculante.Fecha9 + ActivoFijoNeto.Fecha9 + ActivoDiferido.Fecha9;
                SumaActivoTotal.Fecha10 = SumaActivoCirculante.Fecha10 + ActivoNoCirculante.Fecha10 + ActivoFijoNeto.Fecha10 + ActivoDiferido.Fecha10;
                SumaActivoTotal.Fecha11 = SumaActivoCirculante.Fecha11 + ActivoNoCirculante.Fecha11 + ActivoFijoNeto.Fecha11 + ActivoDiferido.Fecha11;
                SumaActivoTotal.Fecha12 = SumaActivoCirculante.Fecha12 + ActivoNoCirculante.Fecha12 + ActivoFijoNeto.Fecha12 + ActivoDiferido.Fecha12;


                var DocumentosInteresPorPagarInicial = EfectivoInversionesTempObj.Where(x => x.Concepto.Contains("Documentos e Intereses por Pagar CP")).Select(x => x.Saldo).ToList();

                EstadoResultadosDTO DocumentosInteresPorPagar = new EstadoResultadosDTO();
                DocumentosInteresPorPagar.Concepto = "PASIVO";
                DocumentosInteresPorPagar.AreaNombre = "Documentos e Intereses por Pagar";
                DocumentosInteresPorPagar.IdRenglon = 12;
                DocumentosInteresPorPagar.Fecha1 = (Convert.ToDecimal(DocumentosInteresPorPagarInicial[0]) - PasgosEstraordinarios.Mes1) / divisor;
                DocumentosInteresPorPagar.Fecha2 = DocumentosInteresPorPagar.Fecha1 - (PasgosEstraordinarios.Mes2 / divisor);
                DocumentosInteresPorPagar.Fecha3 = DocumentosInteresPorPagar.Fecha2 - (PasgosEstraordinarios.Mes3 / divisor);
                DocumentosInteresPorPagar.Fecha4 = DocumentosInteresPorPagar.Fecha3 - (PasgosEstraordinarios.Mes4 / divisor);
                DocumentosInteresPorPagar.Fecha5 = DocumentosInteresPorPagar.Fecha4 - (PasgosEstraordinarios.Mes5 / divisor);
                DocumentosInteresPorPagar.Fecha6 = DocumentosInteresPorPagar.Fecha5 - (PasgosEstraordinarios.Mes6 / divisor);
                DocumentosInteresPorPagar.Fecha7 = DocumentosInteresPorPagar.Fecha6 - (PasgosEstraordinarios.Mes7 / divisor);
                DocumentosInteresPorPagar.Fecha8 = DocumentosInteresPorPagar.Fecha7 - (PasgosEstraordinarios.Mes8 / divisor);
                DocumentosInteresPorPagar.Fecha9 = DocumentosInteresPorPagar.Fecha8 - (PasgosEstraordinarios.Mes9 / divisor);
                DocumentosInteresPorPagar.Fecha10 = DocumentosInteresPorPagar.Fecha9 - (PasgosEstraordinarios.Mes10 / divisor);
                DocumentosInteresPorPagar.Fecha11 = DocumentosInteresPorPagar.Fecha10 - (PasgosEstraordinarios.Mes11 / divisor);
                DocumentosInteresPorPagar.Fecha12 = DocumentosInteresPorPagar.Fecha11 - (PasgosEstraordinarios.Mes12 / divisor);

                var ProveedoresContratistasInicial = EfectivoInversionesTempObj.Where(x => x.Concepto.Contains("Proveedores y Contratistas")).Select(x => x.Saldo).ToList();

                EstadoResultadosDTO ProveedoresContratistas = new EstadoResultadosDTO();
                ProveedoresContratistas.Concepto = "PASIVO";
                ProveedoresContratistas.AreaNombre = "Proveedores y contratistas";
                ProveedoresContratistas.IdRenglon = 13;
                ProveedoresContratistas.Fecha1 = (Convert.ToDecimal(ProveedoresContratistasInicial[0]) - PagosDiverosObj.ImporteAmortAbono.Mes1) / divisor;
                ProveedoresContratistas.Fecha2 = ProveedoresContratistas.Fecha1 - (PagosDiverosObj.ImporteAmortAbono.Mes2 / divisor);
                ProveedoresContratistas.Fecha3 = ProveedoresContratistas.Fecha2 - (PagosDiverosObj.ImporteAmortAbono.Mes3 / divisor);
                ProveedoresContratistas.Fecha4 = ProveedoresContratistas.Fecha3 - (PagosDiverosObj.ImporteAmortAbono.Mes4 / divisor);
                ProveedoresContratistas.Fecha5 = ProveedoresContratistas.Fecha4 - (PagosDiverosObj.ImporteAmortAbono.Mes5 / divisor);
                ProveedoresContratistas.Fecha6 = ProveedoresContratistas.Fecha5 - (PagosDiverosObj.ImporteAmortAbono.Mes6 / divisor);
                ProveedoresContratistas.Fecha7 = ProveedoresContratistas.Fecha6 - (PagosDiverosObj.ImporteAmortAbono.Mes7 / divisor);
                ProveedoresContratistas.Fecha8 = ProveedoresContratistas.Fecha7 - (PagosDiverosObj.ImporteAmortAbono.Mes8 / divisor);
                ProveedoresContratistas.Fecha9 = ProveedoresContratistas.Fecha8 - (PagosDiverosObj.ImporteAmortAbono.Mes9 / divisor);
                ProveedoresContratistas.Fecha10 = ProveedoresContratistas.Fecha9 - (PagosDiverosObj.ImporteAmortAbono.Mes10 / divisor);
                ProveedoresContratistas.Fecha11 = ProveedoresContratistas.Fecha10 - (PagosDiverosObj.ImporteAmortAbono.Mes11 / divisor);
                ProveedoresContratistas.Fecha12 = ProveedoresContratistas.Fecha11 - (PagosDiverosObj.ImporteAmortAbono.Mes12 / divisor);

                var ImpuestosDerechoPorPagarInicial = EfectivoInversionesTempObj.Where(x => x.Concepto.Contains("Impuestos y Derechos por Pagar")).Select(x => x.Saldo).ToList();

                EstadoResultadosDTO ImpuestosDerechoPorPagar = new EstadoResultadosDTO();
                ImpuestosDerechoPorPagar.Concepto = "PASIVO";
                ImpuestosDerechoPorPagar.AreaNombre = "Impuestos y Derechos por Pagar";
                ImpuestosDerechoPorPagar.IdRenglon = 14;
                ImpuestosDerechoPorPagar.Fecha1 = Convert.ToDecimal(ImpuestosDerechoPorPagarInicial[0]) / divisor;
                ImpuestosDerechoPorPagar.Fecha2 = ImpuestosDerechoPorPagar.Fecha1;
                ImpuestosDerechoPorPagar.Fecha3 = ImpuestosDerechoPorPagar.Fecha2;
                ImpuestosDerechoPorPagar.Fecha4 = ImpuestosDerechoPorPagar.Fecha3;
                ImpuestosDerechoPorPagar.Fecha5 = ImpuestosDerechoPorPagar.Fecha4;
                ImpuestosDerechoPorPagar.Fecha6 = ImpuestosDerechoPorPagar.Fecha5;
                ImpuestosDerechoPorPagar.Fecha7 = ImpuestosDerechoPorPagar.Fecha6;
                ImpuestosDerechoPorPagar.Fecha8 = ImpuestosDerechoPorPagar.Fecha7;
                ImpuestosDerechoPorPagar.Fecha9 = ImpuestosDerechoPorPagar.Fecha8;
                ImpuestosDerechoPorPagar.Fecha10 = ImpuestosDerechoPorPagar.Fecha9;
                ImpuestosDerechoPorPagar.Fecha11 = ImpuestosDerechoPorPagar.Fecha10;
                ImpuestosDerechoPorPagar.Fecha12 = ImpuestosDerechoPorPagar.Fecha11;

                var GastosAcomuladosInicial = EfectivoInversionesTempObj.Where(x => x.Concepto.Contains("Otros Gastos Acumulados por Pagar")).Select(x => x.Saldo).ToList();

                EstadoResultadosDTO GastosAcomulados = new EstadoResultadosDTO();
                GastosAcomulados.Concepto = "PASIVO";
                GastosAcomulados.AreaNombre = "Otros Gastos Acomulados por Pagar";
                GastosAcomulados.IdRenglon = 15;
                GastosAcomulados.Fecha1 = Convert.ToDecimal(GastosAcomuladosInicial[0]) / divisor;
                GastosAcomulados.Fecha2 = GastosAcomulados.Fecha1;
                GastosAcomulados.Fecha3 = GastosAcomulados.Fecha2;
                GastosAcomulados.Fecha4 = GastosAcomulados.Fecha3;
                GastosAcomulados.Fecha5 = GastosAcomulados.Fecha4;
                GastosAcomulados.Fecha6 = GastosAcomulados.Fecha5;
                GastosAcomulados.Fecha7 = GastosAcomulados.Fecha6;
                GastosAcomulados.Fecha8 = GastosAcomulados.Fecha7;
                GastosAcomulados.Fecha9 = GastosAcomulados.Fecha8;
                GastosAcomulados.Fecha10 = GastosAcomulados.Fecha9;
                GastosAcomulados.Fecha11 = GastosAcomulados.Fecha10;
                GastosAcomulados.Fecha12 = GastosAcomulados.Fecha11;

                var AcreedoresDiversosInicial = EfectivoInversionesTempObj.Where(x => x.Concepto.Contains("Acreedores Diversos")).Select(x => x.Saldo).ToList();

                EstadoResultadosDTO AcreedoresDiversos = new EstadoResultadosDTO();
                AcreedoresDiversos.Concepto = "PASIVO";
                AcreedoresDiversos.AreaNombre = "Acreedores Diversos";
                AcreedoresDiversos.IdRenglon = 16;
                AcreedoresDiversos.Fecha1 = (Convert.ToDecimal(AcreedoresDiversosInicial[0]) - PagosDiverosObj.ImporteAmortAcreedores.Mes1) / divisor;
                AcreedoresDiversos.Fecha2 = AcreedoresDiversos.Fecha1 - (PagosDiverosObj.ImporteAmortAcreedores.Mes2 / divisor);
                AcreedoresDiversos.Fecha3 = AcreedoresDiversos.Fecha2 - (PagosDiverosObj.ImporteAmortAcreedores.Mes3 / divisor);
                AcreedoresDiversos.Fecha4 = AcreedoresDiversos.Fecha3 - (PagosDiverosObj.ImporteAmortAcreedores.Mes4 / divisor);
                AcreedoresDiversos.Fecha5 = AcreedoresDiversos.Fecha4 - (PagosDiverosObj.ImporteAmortAcreedores.Mes5 / divisor);
                AcreedoresDiversos.Fecha6 = AcreedoresDiversos.Fecha5 - (PagosDiverosObj.ImporteAmortAcreedores.Mes6 / divisor);
                AcreedoresDiversos.Fecha7 = AcreedoresDiversos.Fecha6 - (PagosDiverosObj.ImporteAmortAcreedores.Mes7 / divisor);
                AcreedoresDiversos.Fecha8 = AcreedoresDiversos.Fecha7 - (PagosDiverosObj.ImporteAmortAcreedores.Mes8 / divisor);
                AcreedoresDiversos.Fecha9 = AcreedoresDiversos.Fecha8 - (PagosDiverosObj.ImporteAmortAcreedores.Mes9 / divisor);
                AcreedoresDiversos.Fecha10 = AcreedoresDiversos.Fecha9 - (PagosDiverosObj.ImporteAmortAcreedores.Mes10 / divisor);
                AcreedoresDiversos.Fecha11 = AcreedoresDiversos.Fecha10 - (PagosDiverosObj.ImporteAmortAcreedores.Mes11 / divisor);
                AcreedoresDiversos.Fecha12 = AcreedoresDiversos.Fecha11 - (PagosDiverosObj.ImporteAmortAcreedores.Mes12 / divisor);

                EstadoResultadosDTO SumaPasivosCP = new EstadoResultadosDTO();
                SumaPasivosCP.Concepto = "Suma Pasivo C. P.";
                SumaPasivosCP.AreaNombre = "";
                SumaActivoTotal.IdRenglon = 17;
                SumaPasivosCP.Fecha1 = DocumentosInteresPorPagar.Fecha1 + ProveedoresContratistas.Fecha1 + ImpuestosDerechoPorPagar.Fecha1 + GastosAcomulados.Fecha1 + AcreedoresDiversos.Fecha1;
                SumaPasivosCP.Fecha2 = DocumentosInteresPorPagar.Fecha2 + ProveedoresContratistas.Fecha2 + ImpuestosDerechoPorPagar.Fecha2 + GastosAcomulados.Fecha2 + AcreedoresDiversos.Fecha2;
                SumaPasivosCP.Fecha3 = DocumentosInteresPorPagar.Fecha3 + ProveedoresContratistas.Fecha3 + ImpuestosDerechoPorPagar.Fecha3 + GastosAcomulados.Fecha3 + AcreedoresDiversos.Fecha3;
                SumaPasivosCP.Fecha4 = DocumentosInteresPorPagar.Fecha4 + ProveedoresContratistas.Fecha4 + ImpuestosDerechoPorPagar.Fecha4 + GastosAcomulados.Fecha4 + AcreedoresDiversos.Fecha4;
                SumaPasivosCP.Fecha5 = DocumentosInteresPorPagar.Fecha5 + ProveedoresContratistas.Fecha5 + ImpuestosDerechoPorPagar.Fecha5 + GastosAcomulados.Fecha5 + AcreedoresDiversos.Fecha5;
                SumaPasivosCP.Fecha6 = DocumentosInteresPorPagar.Fecha6 + ProveedoresContratistas.Fecha6 + ImpuestosDerechoPorPagar.Fecha6 + GastosAcomulados.Fecha6 + AcreedoresDiversos.Fecha6;
                SumaPasivosCP.Fecha7 = DocumentosInteresPorPagar.Fecha7 + ProveedoresContratistas.Fecha7 + ImpuestosDerechoPorPagar.Fecha7 + GastosAcomulados.Fecha7 + AcreedoresDiversos.Fecha7;
                SumaPasivosCP.Fecha8 = DocumentosInteresPorPagar.Fecha8 + ProveedoresContratistas.Fecha8 + ImpuestosDerechoPorPagar.Fecha8 + GastosAcomulados.Fecha8 + AcreedoresDiversos.Fecha8;
                SumaPasivosCP.Fecha9 = DocumentosInteresPorPagar.Fecha9 + ProveedoresContratistas.Fecha9 + ImpuestosDerechoPorPagar.Fecha9 + GastosAcomulados.Fecha9 + AcreedoresDiversos.Fecha9;
                SumaPasivosCP.Fecha10 = DocumentosInteresPorPagar.Fecha10 + ProveedoresContratistas.Fecha10 + ImpuestosDerechoPorPagar.Fecha10 + GastosAcomulados.Fecha10 + AcreedoresDiversos.Fecha10;
                SumaPasivosCP.Fecha11 = DocumentosInteresPorPagar.Fecha11 + ProveedoresContratistas.Fecha11 + ImpuestosDerechoPorPagar.Fecha11 + GastosAcomulados.Fecha11 + AcreedoresDiversos.Fecha11;
                SumaPasivosCP.Fecha12 = DocumentosInteresPorPagar.Fecha12 + ProveedoresContratistas.Fecha12 + ImpuestosDerechoPorPagar.Fecha12 + GastosAcomulados.Fecha12 + AcreedoresDiversos.Fecha12;

                decimal blanco = 0; //Concepto en blanco, sin  referencia

                EstadoResultadosDTO SumaPasivosTotal = new EstadoResultadosDTO();
                SumaPasivosTotal.Concepto = "Suma Pasivo Total";
                SumaPasivosTotal.AreaNombre = "";
                SumaPasivosTotal.IdRenglon = 18;
                SumaPasivosTotal.Fecha1 = SumaPasivosCP.Fecha1 + blanco;
                SumaPasivosTotal.Fecha2 = SumaPasivosCP.Fecha2 + blanco;
                SumaPasivosTotal.Fecha3 = SumaPasivosCP.Fecha3 + blanco;
                SumaPasivosTotal.Fecha4 = SumaPasivosCP.Fecha4 + blanco;
                SumaPasivosTotal.Fecha5 = SumaPasivosCP.Fecha5 + blanco;
                SumaPasivosTotal.Fecha6 = SumaPasivosCP.Fecha6 + blanco;
                SumaPasivosTotal.Fecha7 = SumaPasivosCP.Fecha7 + blanco;
                SumaPasivosTotal.Fecha8 = SumaPasivosCP.Fecha8 + blanco;
                SumaPasivosTotal.Fecha9 = SumaPasivosCP.Fecha9 + blanco;
                SumaPasivosTotal.Fecha10 = SumaPasivosCP.Fecha10 + blanco;
                SumaPasivosTotal.Fecha11 = SumaPasivosCP.Fecha11 + blanco;
                SumaPasivosTotal.Fecha12 = SumaPasivosCP.Fecha12 + blanco;

                var CapitalSocialInicial = EfectivoInversionesTempObj.Where(x => x.Concepto.Contains("Capital Social")).Select(x => x.Saldo).ToList();

                EstadoResultadosDTO CapitalSocial = new EstadoResultadosDTO();
                CapitalSocial.Concepto = "CAPITAL CONTABLE";
                CapitalSocial.AreaNombre = "Capital Social";
                CapitalSocial.IdRenglon = 19;
                CapitalSocial.Fecha1 = Convert.ToDecimal(CapitalSocialInicial[0]) / divisor;
                CapitalSocial.Fecha2 = CapitalSocial.Fecha1;
                CapitalSocial.Fecha3 = CapitalSocial.Fecha2;
                CapitalSocial.Fecha4 = CapitalSocial.Fecha3;
                CapitalSocial.Fecha5 = CapitalSocial.Fecha4;
                CapitalSocial.Fecha6 = CapitalSocial.Fecha5;
                CapitalSocial.Fecha7 = CapitalSocial.Fecha6;
                CapitalSocial.Fecha8 = CapitalSocial.Fecha7;
                CapitalSocial.Fecha9 = CapitalSocial.Fecha8;
                CapitalSocial.Fecha10 = CapitalSocial.Fecha9;
                CapitalSocial.Fecha11 = CapitalSocial.Fecha10;
                CapitalSocial.Fecha12 = CapitalSocial.Fecha11;

                var AportFuturoInicial = EfectivoInversionesTempObj.Where(x => x.Concepto.Contains("Aport. Futuros Aum. Capital")).Select(x => x.Saldo).ToList();

                EstadoResultadosDTO AportFuturo = new EstadoResultadosDTO();
                AportFuturo.Concepto = "CAPITAL CONTABLE";
                AportFuturo.AreaNombre = "Aport. Futuro Aum. Capital";
                AportFuturo.IdRenglon = 20;
                AportFuturo.Fecha1 = (Convert.ToDecimal(AportFuturoInicial[0]) + objCobroDiv.ln6AporteCapital.Mes1) / divisor;
                AportFuturo.Fecha2 = (AportFuturo.Fecha1 + (Convert.ToDecimal(objCobroDiv.ln6AporteCapital.Mes2)) / divisor);
                AportFuturo.Fecha3 = (AportFuturo.Fecha2 + (Convert.ToDecimal(objCobroDiv.ln6AporteCapital.Mes3)) / divisor);
                AportFuturo.Fecha4 = (AportFuturo.Fecha3 + (Convert.ToDecimal(objCobroDiv.ln6AporteCapital.Mes4)) / divisor);
                AportFuturo.Fecha5 = (AportFuturo.Fecha4 + (Convert.ToDecimal(objCobroDiv.ln6AporteCapital.Mes5)) / divisor);
                AportFuturo.Fecha6 = (AportFuturo.Fecha5 + (Convert.ToDecimal(objCobroDiv.ln6AporteCapital.Mes6)) / divisor);
                AportFuturo.Fecha7 = (AportFuturo.Fecha6 + (Convert.ToDecimal(objCobroDiv.ln6AporteCapital.Mes7)) / divisor);
                AportFuturo.Fecha8 = (AportFuturo.Fecha7 + (Convert.ToDecimal(objCobroDiv.ln6AporteCapital.Mes8)) / divisor);
                AportFuturo.Fecha9 = (AportFuturo.Fecha8 + (Convert.ToDecimal(objCobroDiv.ln6AporteCapital.Mes9)) / divisor);
                AportFuturo.Fecha10 = (AportFuturo.Fecha9 + (Convert.ToDecimal(objCobroDiv.ln6AporteCapital.Mes10)) / divisor);
                AportFuturo.Fecha11 = (AportFuturo.Fecha10 + (Convert.ToDecimal(objCobroDiv.ln6AporteCapital.Mes11)) / divisor);
                AportFuturo.Fecha12 = (AportFuturo.Fecha11 + (Convert.ToDecimal(objCobroDiv.ln6AporteCapital.Mes12)) / divisor);


                var ResultadoAcomuladoInicial = EfectivoInversionesTempObj.Where(x => x.Concepto.Contains("Resultado Acum. Ejercicios Anteriores")).Select(x => x.Saldo).ToList();

                EstadoResultadosDTO ResultadoAcomulado = new EstadoResultadosDTO();
                ResultadoAcomulado.Concepto = "CAPITAL CONTABLE";
                ResultadoAcomulado.AreaNombre = "Resultado Acom. Resultados Anteriores";
                ResultadoAcomulado.IdRenglon = 21;
                ResultadoAcomulado.Fecha1 = Convert.ToDecimal(ResultadoAcomuladoInicial[0]) / divisor;
                ResultadoAcomulado.Fecha2 = ResultadoAcomulado.Fecha1;
                ResultadoAcomulado.Fecha3 = ResultadoAcomulado.Fecha2;
                ResultadoAcomulado.Fecha4 = ResultadoAcomulado.Fecha3;
                ResultadoAcomulado.Fecha5 = ResultadoAcomulado.Fecha4;
                ResultadoAcomulado.Fecha6 = ResultadoAcomulado.Fecha5;
                ResultadoAcomulado.Fecha7 = ResultadoAcomulado.Fecha6;
                ResultadoAcomulado.Fecha8 = ResultadoAcomulado.Fecha7;
                ResultadoAcomulado.Fecha9 = ResultadoAcomulado.Fecha8;
                ResultadoAcomulado.Fecha10 = ResultadoAcomulado.Fecha9;
                ResultadoAcomulado.Fecha11 = ResultadoAcomulado.Fecha10;
                ResultadoAcomulado.Fecha12 = ResultadoAcomulado.Fecha11;

                var ExcesoEnActualizacionInicial = EfectivoInversionesTempObj.Where(x => x.Concepto.Contains("Exceso (Insuficiencia) en Actualización")).Select(x => x.Saldo).ToList();

                EstadoResultadosDTO ExcesoEnActualizacion = new EstadoResultadosDTO();
                ExcesoEnActualizacion.Concepto = "CAPITAL CONTABLE";
                ExcesoEnActualizacion.AreaNombre = "Exceso (insuficiendia) en Actualización";
                ExcesoEnActualizacion.IdRenglon = 22;
                ExcesoEnActualizacion.Fecha1 = Convert.ToDecimal(ExcesoEnActualizacionInicial[0]) / divisor;
                ExcesoEnActualizacion.Fecha2 = ExcesoEnActualizacion.Fecha1;
                ExcesoEnActualizacion.Fecha3 = ExcesoEnActualizacion.Fecha2;
                ExcesoEnActualizacion.Fecha4 = ExcesoEnActualizacion.Fecha3;
                ExcesoEnActualizacion.Fecha5 = ExcesoEnActualizacion.Fecha4;
                ExcesoEnActualizacion.Fecha6 = ExcesoEnActualizacion.Fecha5;
                ExcesoEnActualizacion.Fecha7 = ExcesoEnActualizacion.Fecha6;
                ExcesoEnActualizacion.Fecha8 = ExcesoEnActualizacion.Fecha7;
                ExcesoEnActualizacion.Fecha9 = ExcesoEnActualizacion.Fecha8;
                ExcesoEnActualizacion.Fecha10 = ExcesoEnActualizacion.Fecha9;
                ExcesoEnActualizacion.Fecha11 = ExcesoEnActualizacion.Fecha10;
                ExcesoEnActualizacion.Fecha12 = ExcesoEnActualizacion.Fecha11;

                EstadoResultadosDTO ResultadoEjercicio = new EstadoResultadosDTO();
                ResultadoEjercicio.Concepto = "CAPITAL CONTABLE";
                ResultadoEjercicio.AreaNombre = "Resultado del Ejercicio";
                ResultadoEjercicio.IdRenglon = 23;
                ResultadoEjercicio.Fecha1 = UtilidadNeta.Fecha1;
                ResultadoEjercicio.Fecha2 = ResultadoEjercicio.Fecha1 + UtilidadNeta.Fecha2;
                ResultadoEjercicio.Fecha3 = ResultadoEjercicio.Fecha2 + UtilidadNeta.Fecha3;
                ResultadoEjercicio.Fecha4 = ResultadoEjercicio.Fecha3 + UtilidadNeta.Fecha4;
                ResultadoEjercicio.Fecha5 = ResultadoEjercicio.Fecha4 + UtilidadNeta.Fecha5;
                ResultadoEjercicio.Fecha6 = ResultadoEjercicio.Fecha5 + UtilidadNeta.Fecha6;
                ResultadoEjercicio.Fecha7 = ResultadoEjercicio.Fecha6 + UtilidadNeta.Fecha7;
                ResultadoEjercicio.Fecha8 = ResultadoEjercicio.Fecha7 + UtilidadNeta.Fecha8;
                ResultadoEjercicio.Fecha9 = ResultadoEjercicio.Fecha8 + UtilidadNeta.Fecha9;
                ResultadoEjercicio.Fecha10 = ResultadoEjercicio.Fecha9 + UtilidadNeta.Fecha10;
                ResultadoEjercicio.Fecha11 = ResultadoEjercicio.Fecha10 + UtilidadNeta.Fecha11;
                ResultadoEjercicio.Fecha12 = ResultadoEjercicio.Fecha11 + UtilidadNeta.Fecha12;

                EstadoResultadosDTO SumaCapitalContable = new EstadoResultadosDTO();
                SumaCapitalContable.Concepto = "Suma el Capital Contable";
                SumaCapitalContable.AreaNombre = "";
                SumaCapitalContable.IdRenglon = 24;
                SumaCapitalContable.Fecha1 = CapitalSocial.Fecha1 + AportFuturo.Fecha1 + ResultadoAcomulado.Fecha1 + ExcesoEnActualizacion.Fecha1 + ResultadoEjercicio.Fecha1;
                SumaCapitalContable.Fecha2 = CapitalSocial.Fecha2 + AportFuturo.Fecha2 + ResultadoAcomulado.Fecha2 + ExcesoEnActualizacion.Fecha2 + ResultadoEjercicio.Fecha2;
                SumaCapitalContable.Fecha3 = CapitalSocial.Fecha3 + AportFuturo.Fecha3 + ResultadoAcomulado.Fecha3 + ExcesoEnActualizacion.Fecha3 + ResultadoEjercicio.Fecha3;
                SumaCapitalContable.Fecha4 = CapitalSocial.Fecha4 + AportFuturo.Fecha4 + ResultadoAcomulado.Fecha4 + ExcesoEnActualizacion.Fecha4 + ResultadoEjercicio.Fecha4;
                SumaCapitalContable.Fecha5 = CapitalSocial.Fecha5 + AportFuturo.Fecha5 + ResultadoAcomulado.Fecha5 + ExcesoEnActualizacion.Fecha5 + ResultadoEjercicio.Fecha5;
                SumaCapitalContable.Fecha6 = CapitalSocial.Fecha6 + AportFuturo.Fecha6 + ResultadoAcomulado.Fecha6 + ExcesoEnActualizacion.Fecha6 + ResultadoEjercicio.Fecha6;
                SumaCapitalContable.Fecha7 = CapitalSocial.Fecha7 + AportFuturo.Fecha7 + ResultadoAcomulado.Fecha7 + ExcesoEnActualizacion.Fecha7 + ResultadoEjercicio.Fecha7;
                SumaCapitalContable.Fecha8 = CapitalSocial.Fecha8 + AportFuturo.Fecha8 + ResultadoAcomulado.Fecha8 + ExcesoEnActualizacion.Fecha8 + ResultadoEjercicio.Fecha8;
                SumaCapitalContable.Fecha9 = CapitalSocial.Fecha9 + AportFuturo.Fecha9 + ResultadoAcomulado.Fecha9 + ExcesoEnActualizacion.Fecha9 + ResultadoEjercicio.Fecha9;
                SumaCapitalContable.Fecha10 = CapitalSocial.Fecha10 + AportFuturo.Fecha10 + ResultadoAcomulado.Fecha10 + ExcesoEnActualizacion.Fecha10 + ResultadoEjercicio.Fecha10;
                SumaCapitalContable.Fecha11 = CapitalSocial.Fecha11 + AportFuturo.Fecha11 + ResultadoAcomulado.Fecha11 + ExcesoEnActualizacion.Fecha11 + ResultadoEjercicio.Fecha11;
                SumaCapitalContable.Fecha12 = CapitalSocial.Fecha12 + AportFuturo.Fecha12 + ResultadoAcomulado.Fecha12 + ExcesoEnActualizacion.Fecha12 + ResultadoEjercicio.Fecha12;


                EstadoResultadosDTO SumaPasivoCapital = new EstadoResultadosDTO();
                SumaPasivoCapital.Concepto = "Suma el Pasivo y el Capital";
                SumaPasivoCapital.AreaNombre = "";
                SumaPasivoCapital.IdRenglon = 25;
                SumaPasivoCapital.Fecha1 = SumaCapitalContable.Fecha1 + SumaPasivosTotal.Fecha1;
                SumaPasivoCapital.Fecha2 = SumaCapitalContable.Fecha2 + SumaPasivosTotal.Fecha2;
                SumaPasivoCapital.Fecha3 = SumaCapitalContable.Fecha3 + SumaPasivosTotal.Fecha3;
                SumaPasivoCapital.Fecha4 = SumaCapitalContable.Fecha4 + SumaPasivosTotal.Fecha4;
                SumaPasivoCapital.Fecha5 = SumaCapitalContable.Fecha5 + SumaPasivosTotal.Fecha5;
                SumaPasivoCapital.Fecha6 = SumaCapitalContable.Fecha6 + SumaPasivosTotal.Fecha6;
                SumaPasivoCapital.Fecha7 = SumaCapitalContable.Fecha7 + SumaPasivosTotal.Fecha7;
                SumaPasivoCapital.Fecha8 = SumaCapitalContable.Fecha8 + SumaPasivosTotal.Fecha8;
                SumaPasivoCapital.Fecha9 = SumaCapitalContable.Fecha9 + SumaPasivosTotal.Fecha9;
                SumaPasivoCapital.Fecha10 = SumaCapitalContable.Fecha10 + SumaPasivosTotal.Fecha10;
                SumaPasivoCapital.Fecha11 = SumaCapitalContable.Fecha11 + SumaPasivosTotal.Fecha11;
                SumaPasivoCapital.Fecha12 = SumaCapitalContable.Fecha12 + SumaPasivosTotal.Fecha12;

                EstadoResultadosDTO Cuadre = new EstadoResultadosDTO();
                Cuadre.Concepto = "CUADRE";
                Cuadre.AreaNombre = "";
                Cuadre.Fecha1 = SumaActivoTotal.Fecha1 - SumaPasivoCapital.Fecha1;
                Cuadre.Fecha2 = SumaActivoTotal.Fecha2 - SumaPasivoCapital.Fecha2;
                Cuadre.Fecha3 = SumaActivoTotal.Fecha3 - SumaPasivoCapital.Fecha3;
                Cuadre.Fecha4 = SumaActivoTotal.Fecha4 - SumaPasivoCapital.Fecha4;
                Cuadre.Fecha5 = SumaActivoTotal.Fecha5 - SumaPasivoCapital.Fecha5;
                Cuadre.Fecha6 = SumaActivoTotal.Fecha6 - SumaPasivoCapital.Fecha6;
                Cuadre.Fecha7 = SumaActivoTotal.Fecha7 - SumaPasivoCapital.Fecha7;
                Cuadre.Fecha8 = SumaActivoTotal.Fecha8 - SumaPasivoCapital.Fecha8;
                Cuadre.Fecha9 = SumaActivoTotal.Fecha9 - SumaPasivoCapital.Fecha9;
                Cuadre.Fecha10 = SumaActivoTotal.Fecha10 - SumaPasivoCapital.Fecha10;
                Cuadre.Fecha11 = SumaActivoTotal.Fecha11 - SumaPasivoCapital.Fecha11;
                Cuadre.Fecha12 = SumaActivoTotal.Fecha12 - SumaPasivoCapital.Fecha12;







                //Reporte1
                result.Add("ContribucionMarginal", ContribucionMarginal);
                //Tabla 1 VAlorizacion de Flujo Ingreso Mensual
                result.Add("ValorizacionObra", ValorizacionObra.ToList());
                //Tabla2 Valorizacion de flujo de costo directo mensual.
                result.Add("ValorizacionFlujo", ValorizacionFlujo.ToList());
                //Tabla 3 Flujo de ingresos mensual(valorizacion )
                result.Add("FlujodeIngresos", FlujodeIngresos.OrderBy(x => x.Area).ToList());
                //Tabla 4 Flujo de ingresos mensual (CostoDirecto)
                result.Add("FlujodeIngresosM", FlujodeIngresosM.OrderBy(x => x.Area).ToList());

                result.Add("FlujodeIngresoGeneral", FlujodeIngresoGeneral);
                result.Add("UtilidadBrutaDetalle", UtilidadBrutaDetalle);

                result.Add("UtilidadNeta", UtilidadNeta);   //Resultado Mensual CifrasPrincipales.
                result.Add("VentasNetas", VentasNetas); // Ventas Mensuales CifrasPrincipales.
                result.Add("UtilidadPromedioBruta", UtilidadPromedioBruta);  // % Utilidad Bruta Promedio
                result.Add("SaldoFinalFlujoEfectivo", SaldoFinalFlujoEfectivo); // Flujo de Efectivo


                result.Add("UtilidadOperacion", UtilidadOperacion);
                result.Add("CostoIntegral", CostoIntegral);
                result.Add("ListaDivisionesVentas", ListaDivisionesVentas);
                result.Add("TotalGtosOperacionR1", TotalGtosOperacionR1);
                //Reporte1UtilidadNeta 
                result.Add("impuestos", impuestos);
                result.Add("UtilidadAntesImp", UtilidadAntesImp);
                //  result.Add("UtilidadOperacion", UtilidadOperacion);
                result.Add("CostoDeVenta", CostoDeVenta);




                //Reporte2
                result.Add("IngresosVentas", IngresosVentas);
                result.Add("IngresosVentasMaq", IngresosVentasMaq);

                result.Add("CostoGastoOperacionTotal", CostoGastoOperacionTotal);
                result.Add("CostoVentaTotal", CostoVentaTotal);
                result.Add("CostoGastoOperacion", CostoGastoOperacion);
                result.Add("TotalGtoOperacion", TotalGtoOperacion);
                result.Add("ProveedorAcreedor", ProveedorAcreedor);



                result.Add("FlujoOperacion", FlujoOperacion);
                result.Add("InversionesFisicas", InversionesFisicas);

                result.Add("FlujoDespuesInversiones", FlujoDespuesInversiones);
                result.Add("InteresesGastoDeuda", InteresesGastoDeuda);

                result.Add("FlujoDespuesIntereses", FlujoDespuesIntereses);
                result.Add("PagosDiversos", PagosDiversos);
                result.Add("RCyCD", RCyCD);

                result.Add("DeCaja", DeCaja);

                result.Add("AportacionesCapital", AportacionesCapital);

                result.Add("CreditosBancarios", CreditosBancarios);

                result.Add("Reservas", Reservas);

                result.Add("SaldoInicial", SaldoInicial);


                //Reporte3
                //Activo Circulante
                result.Add("EfectivoInversiones", EfectivoInversiones);
                result.Add("Clientes", Clientes);
                result.Add("OtrosDeudores", OtrosDeudores);
                result.Add("SumaCuentasPorCobrar", SumaCuentasPorCobrar);
                result.Add("Inventarios", Inventarios);
                result.Add("OtrosActivos", OtrosActivos);
                result.Add("SumaActivoCirculante", SumaActivoCirculante);
                //Activo no circulante
                result.Add("ActivoNoCirculante", ActivoNoCirculante);
                //Activo fijo-neto
                result.Add("ActivoFijoNeto", ActivoFijoNeto);
                //Activo Diferido
                result.Add("ActivoDiferido", ActivoDiferido);
                //suma activo total
                result.Add("SumaActivoTotal", SumaActivoTotal);
                //Pasivo
                result.Add("DocumentosInteresPorPagar", DocumentosInteresPorPagar);
                result.Add("ProveedoresContratistas", ProveedoresContratistas);
                result.Add("ImpuestosDerechoPorPagar", ImpuestosDerechoPorPagar);
                result.Add("GastosAcomulados", GastosAcomulados);
                result.Add("AcreedoresDiversos", AcreedoresDiversos);
                result.Add("SumaPasivosCP", SumaPasivosCP);
                result.Add("SumaPasivosTotal", SumaPasivosTotal);
                //Capital contable
                result.Add("CapitalSocial", CapitalSocial);
                result.Add("AportFuturo", AportFuturo);
                result.Add("ResultadoAcomulado", ResultadoAcomulado);
                result.Add("ResultadoEjercicio", ResultadoEjercicio);
                result.Add("ExcesoEnActualizacion", ExcesoEnActualizacion);
                result.Add("SumaCapitalContable", SumaCapitalContable);
                result.Add("SumaPasivoCapital", SumaPasivoCapital);

                result.Add("Cuadre", Cuadre);
                result.Add("ultimoMes", mes);
                result.Add("ultimoAnio", anio);
            }
            //}
            //catch (Exception e)
            //{
            //    // result.Add(MESSAGE, e.Message);
            //    //    //result.Add(SUCCESS, false);
            //}
            return result;
        }
        public stCorrectDate validDateForData(int Escenario, int mes, int anio)
        {
            var result = new stCorrectDate();
            bool flag = true;
            while (true)
            {
                var res = GetJsonData(Escenario, mes, anio);
                var res1 = _context.tblPro_Administracion.FirstOrDefault(x => x.Mes == mes && x.Anio == anio);
                var res2 = _context.tblPro_PagosDiversos.FirstOrDefault(x => x.Mes == mes && x.Anio == anio);
                if (res1 != null && res2 != null && res != null)
                {
                    result.Escenario = Escenario;
                    result.mes = mes;
                    result.anio = anio;
                    return result;
                }
                else
                {
                    if (mes == 0)
                    {
                        result.Escenario = Escenario;
                        result.mes = 11;
                        result.anio = anio - 1;
                        return result;
                    }
                    else
                    {
                        mes = mes - 1;
                    }

                }
            }

        }

        public List<tblPro_Obras> dataEscenarios(List<tblPro_Obras> listas, int escenario)
        {
            var objEscenario = _context.tblPro_CatEscenarios.FirstOrDefault(x => x.id == escenario);
            if (objEscenario != null)
            {
                var ListaObjEscenarios = _context.tblPro_CatEscenarios.Where(x => x.nivel <= objEscenario.nivel && x.ordenID >= objEscenario.ordenID).ToList();

                var listaDescripcionEscenarios = ListaObjEscenarios.Select(x => x.descripcion).ToList();
                var data = listas.Where(x => listaDescripcionEscenarios.Contains(x.Escenario)).ToList();
                return data;
            }
            else
            {
                return listas;
            }
        }

        public int getUltimoMesCapturado()
        {
            var valor = _context.tblPro_CapturadeObras.OrderByDescending(x => x.id).FirstOrDefault().MesInicio;
            return valor;
        }

        public List<tblPro_CapturadeObras> FillCboObra()
        {
            return _context.tblPro_CapturadeObras.ToList();
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public tblPro_CapturadeObras GetJsonDataID(int idData)
        {
            return _context.tblPro_CapturadeObras.FirstOrDefault(x => x.id == idData);
        }
    }
}