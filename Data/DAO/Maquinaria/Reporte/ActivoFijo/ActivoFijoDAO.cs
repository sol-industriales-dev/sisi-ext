using Core.DAO.Maquinaria.Reporte.ActivoFijo;
using Core.DTO.Maquinaria.Reporte.ActivoFijo;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using Core.Entity.Maquinaria.Reporte.ActivoFijo;
using Core.Enum.Multiempresa;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Utils;
using System.Data.Odbc;
using Core.DTO;
using System.Data.Entity;
using Core.Entity.Maquinaria.Catalogo;
using Core.DTO.Principal.Generales;
using Core.Enum.Maquinaria;
using Core.Entity.Maquinaria.Inventario.ControlCalidad;
using Core.DTO.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion;
using System.ComponentModel;
using Core.Enum.Maquinaria.Reportes.ActivoFijo;
using Core.DTO.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion.CapturaEnkontrol;
using Data.EntityFramework;
using Core.Entity.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion;
using Core.DTO.Maquinaria.Reporte.ActivoFijo.Generales.Enkontrol;
using Core.Entity.Principal.Multiempresa;
using Core.DTO.Maquinaria.Inventario;
using System.Drawing;
using System.Data.Entity.Migrations;
using Core.DTO.Maquinaria.Reporte.ActivoFijo.Colombia;
using Core.Entity.Maquinaria.Reporte.ActivoFijo.Colombia;
using Core.Enum.Principal.Bitacoras;
using Core.DTO.Maquinaria.Reporte.ActivoFijo.Colombia.RelacionPoliza;
using Data.Factory.Enkontrol.General.CC;
using Core.Enum.Contabilidad.Poliza;
using Core.DTO.Maquinaria.Reporte.ActivoFijo.Colombia.Cedula;
using Core.Enum.Maquinaria.Reportes.ActivoFijo.Colombia;
using Core.Entity.Maquinaria.Reporte.ActivoFijo.Peru;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using Core.DTO.Maquinaria.Reporte.ActivoFijo.Peru;

namespace Data.DAO.Maquinaria.Reporte.ActivoFijo
{
    public enum TipoDelMovimiento
    {
        [DescriptionAttribute("Compra normal")]
        A = 1,
        [DescriptionAttribute("Depreciación en junio 2020")]
        B = 2,
        [DescriptionAttribute("Compra/Venta PIINSA")]
        C = 3,
        [DescriptionAttribute("Compra por financiamiento")]
        F = 4,
        [DescriptionAttribute("Overhaul")]
        O = 5
    }

    public enum TipoResultadoCuentasMaquinaria
    {
        CboxMaquinaria = 1,
        CuentasMaquinaria = 2
    }

    public enum TipoDocumentoMaquinaria
    {
        Factura = 1
    }

    public enum MesesCargoTablaContabilidad
    {
        Enecargos = 1,
        Febcargos = 2,
        Marcargos = 3,
        Abrcargos = 4,
        Maycargos = 5,
        Juncargos = 6,
        Julcargos = 7,
        Agocargos = 8,
        Sepcargos = 9,
        Octcargos = 10,
        Novcargos = 11,
        Diccargos = 12
    }

    public enum MesesAbonosTablaContabilidad
    {
        Eneabonos = 1,
        Febabonos = 2,
        Marabonos = 3,
        Abrabonos = 4,
        Mayabonos = 5,
        Junabonos = 6,
        Julabonos = 7,
        Agoabonos = 8,
        Sepabonos = 9,
        Octabonos = 10,
        Novabonos = 11,
        Dicabonos = 12
    }

    public class ActivoFijoDAO : GenericDAO<tblC_SaldoConciliado>, IActivoFijoDAO
    {
        private bool _productivo = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["enkontrolProductivo"]) == "1";

        private tblC_AF_InfoDepreciacion _InfoCuentaDep = null;
        private List<tblC_AF_InsumosOverhaul> _InsumoOverhaul = null;
        private List<ActivoFijoCorteInventarioDTO> corteInventarioMayo = null;
        private List<ActivoFijoCorteInventarioDTO> corteInventarioEneroJulio = new List<ActivoFijoCorteInventarioDTO>();
        private List<ActivoFijoCorteInventarioDTO> corteInventarioDelMes = new List<ActivoFijoCorteInventarioDTO>();
        private DateTime fechaCortePol = new DateTime();
        private List<ActivoFijoCorteInventarioDTO> primerasSemanas = new List<ActivoFijoCorteInventarioDTO>();

        List<int> polizasExceptions = new List<int>
                    {
                        12041204,
                        12051205,
                        12101210,
                        12111211,
                        12151215,
                        12201220,
                        12251225,
                        12301230,
                        12351235
                    };

        List<DateTime> fechasCorteEneroJulio = new List<DateTime>()
        {
            new DateTime(2023, 1, 10),
            new DateTime(2023, 1, 17),
            new DateTime(2023, 1, 24),
            new DateTime(2023, 1, 31),
            new DateTime(2023, 2, 7),
            new DateTime(2023, 2, 14),
            new DateTime(2023, 2, 21),
            new DateTime(2023, 2, 28),
            new DateTime(2023, 3, 7),
            new DateTime(2023, 3, 14),
            new DateTime(2023, 3, 21),
            new DateTime(2023, 3, 28),
            new DateTime(2023, 4, 4),
            new DateTime(2023, 4, 11),
            new DateTime(2023, 4, 18),
            new DateTime(2023, 4, 25),
            new DateTime(2023, 5, 9),
            new DateTime(2023, 5, 16),
            new DateTime(2023, 5, 23),
            new DateTime(2023, 5, 30),
            new DateTime(2023, 6, 6),
            new DateTime(2023, 6, 13),
            new DateTime(2023, 6, 20),
            new DateTime(2023, 6, 27),
            new DateTime(2023, 7, 4),
            new DateTime(2023, 7, 11),
            new DateTime(2023, 7, 18),
            new DateTime(2023, 7, 25)
        };

        #region cedulaDepreciacion
        public Dictionary<string, object> getDetalleExcel(List<ActivoFijoDetalleCuentaDTO> datosExcel, DateTime fechaHasta, List<CedulaColombiaDTO> colombia)
        {
            var resultado = new Dictionary<string, object>();

            var cuentas = _context.tblC_AF_Cuentas.Where(x => x.Estatus).Select(m => m.Cuenta).ToList();

            //
            int mesActual = fechaHasta.Month;
            int añoActual = fechaHasta.Year;
            List<int> añosAnteriores = new List<int>();
            var fechasAnteriores = new List<DateTime>();

            for (int año = 2018; año < añoActual; año++)
            {
                añosAnteriores.Add(año);
                fechasAnteriores.Add(new DateTime(año, 12, 31));
            }

            if (añosAnteriores.Count == 0)
            {
                añosAnteriores.Add(2017);
                fechasAnteriores.Add(new DateTime(2018 - 1, 12, 31));
            }
            //

            //
            var columnasMonto = new Dictionary<string, int>{
                { "moi", 11 }, { "altas", 12 }, { "componentes", 13 }, { "bajas", 15 }, { "cancelaciones", 17 },
                { "depMensual", 20 }, { "depAnterior", 23 }, { "depActual", 24 }, { "bajaDep", 25 }, { "depAcumulada", 26 },
                { "valorLibros", 27 }, { "semanasOH14_1", 34 }, { "depOH14_1", 35 }
            };
            //

            //var detallesCreados = construirDetalles(fechaHasta, cuentas, false);
            var detallesCreados = new Dictionary<string, object>();
            detallesCreados.Add(SUCCESS, true);

            if ((bool)detallesCreados[SUCCESS])
            {
                //var detalles = (List<ActivoFijoDetalleCuentaDTO>)detallesCreados[ITEMS];
                var detalles = datosExcel;

                //CREACIÓN EXCEL
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    foreach (var cta in cuentas)
                    {
                        var excelDetalles = excel.Workbook.Worksheets.Add(cta.ToString() + "-" + detalles.Where(x => x.Cuenta == cta).Select(x => x.Descripcion).FirstOrDefault());

                        //var header = new List<string> {"Factura", "Poliza", "Subcuenta", "SubSubcuenta", "Fecha", "Fecha inicio depreciación", "Meses de depreciación", "Cc", "Clave", "Descripción", "MOI al 31/12/2018", "Altas", "Overhaul",
                        //                "Fecha cancelación", "Monto cancelación", "Fecha de baja", "Monto baja", "% de la depreciación", "Depreciación mensual", "Meses depreciados año anterior",
                        //                "Meses 2019", "Dep. contable acumulada año anterior", "Dep. contable del ejercicio año actual", "Baja de depreciación año actual",
                        //                "Depreciación contable acumulada al año actual", "Área cuenta", "Faltante", "FechaBajaPol", "FechaCancelacionPol" };

                        var header = new List<string> {"Número", "Fecha", "Fecha inicio depreciación", "Póliza (año-mes-póliza-tp-línea)", "Subcuenta", "SubSubcuenta", "Factura", "CC",
                            "Clave", "Descripción", "MOI al " + fechasAnteriores.OrderBy(o => o).Last().ToShortDateString(), "Altas al " + fechaHasta.ToShortDateString(), "Componentes al " + fechaHasta.ToShortDateString(), "Fecha baja", "Monto baja", "Fecha cancelación", "Monto cancelación",
                            "% depreciación", "Meses totales", "Depreciación mensual", "Meses depreciados al " + fechasAnteriores.OrderBy(o => o).Last().ToShortDateString(), "Meses depreciados " + añoActual,
                            "Dep. contable acumulada al " + fechasAnteriores.OrderBy(o => o).Last().ToShortDateString(), "Dep. contable " + añoActual, "Baja de dep " + añoActual,
                            "Dep. contable acumulada al " + fechaHasta.ToShortDateString(), "Valor en libros", "Área cuenta", "Procesado correctamente", "Alta Sigoplan", "Baja Sigoplan", "Cancelación Sigoplan", "EsOverhaul",
                            "Semanas dep 14-1",
                            "Dep 14-1"
                        };

                        for (int i = 1; i <= header.Count; i++)
                        {
                            excelDetalles.Cells[1, i].Value = header[i - 1];
                        }

                        //excelDetalles.Cells[2, 1].LoadFromCollection<ActivoFijoDetalleDTO>(detalles.Where(x => x.Cuenta == cta).Select(x => x.Detalles).FirstOrDefault());

                        //
                        var cellData = new List<object[]>();
                        int contador = 1;
                        foreach (var item in detalles.Where(x => x.Cuenta == cta).Select(x => x.Detalles).FirstOrDefault())
                        {
                            cellData.Add(new object[]{
                                contador,
                                item.Fecha,
                                item.FechaInicioDepreciacion,
                                item.Poliza,
                                item.Subcuenta,
                                item.SubSubcuenta,
                                item.Factura,
                                item.Cc,
                                item.Clave,
                                item.Descripcion,
                                //item.Tipo,
                                item.MOI,
                                item.Altas,
                                item.Overhaul,
                                //item.SumaAltas,
                                item.FechaBaja,
                                item.MontoBaja,
                                item.FechaCancelacion,
                                item.MontoCancelacion,
                                //item.FacturaBaja,
                                item.PorcentajeDepreciacion,
                                item.MesesMaximoDepreciacion,
                                item.DepreciacionMensual,
                                //item.DepreciacionAnual,
                                //item.DepreciacionDiaria,
                                //item.DepreciacionSemanal,
                                item.MesesDepreciadosAñoAnterior,
                                item.MesesDepreciadosAñoActual,
                                item.DepreciacionAcumuladaAñoAnterior,
                                item.DepreciacionAñoActual,
                                item.BajaDepreciacion,
                                item.DepreciacionContableAcumulada,
                                item.ValorEnLibros,
                                item.AreaCuenta,
                                item.faltante,
                                item.AltaSigoplan,
                                item.BajaSigoplan,
                                item.CancelacionSigoplan,
                                item.EsOverhaul,
                                //item.MesesDepreciadosAñoAnteriorParaDiferencias
                                item.semanasDepreciacionOverhaul14_1,
                                item.depreciacionOverhaul14_1
                            });

                            contador++;
                        }

                        excelDetalles.Cells[2, 1].LoadFromArrays(cellData);
                        //

                        ExcelRange range = excelDetalles.Cells[1, 1, excelDetalles.Dimension.End.Row, excelDetalles.Dimension.End.Column];

                        var cellFont = range.Style.Font;
                        cellFont.SetFromFont(new Font("Arial", 12));

                        ExcelTable tab = excelDetalles.Tables.Add(range, "Tabla" + cta.ToString());

                        excelDetalles.Cells[1, 11, excelDetalles.Dimension.End.Row, 13].Style.Numberformat.Format = "$#,##0.00";
                        excelDetalles.Cells[1, 15, excelDetalles.Dimension.End.Row, 15].Style.Numberformat.Format = "$#,##0.00";
                        excelDetalles.Cells[1, 17, excelDetalles.Dimension.End.Row, 17].Style.Numberformat.Format = "$#,##0.00";
                        excelDetalles.Cells[1, 20, excelDetalles.Dimension.End.Row, 20].Style.Numberformat.Format = "$#,##0.00";
                        excelDetalles.Cells[1, 23, excelDetalles.Dimension.End.Row, 27].Style.Numberformat.Format = "$#,##0.00";
                        excelDetalles.Cells[1, 23, excelDetalles.Dimension.End.Row, 27].Style.Numberformat.Format = "$#,##0.00";
                        excelDetalles.Cells[1, 34, excelDetalles.Dimension.End.Row, 35].Style.Numberformat.Format = "$#,##0.00";

                        excelDetalles.Cells[1, 2, excelDetalles.Dimension.End.Row, 3].Style.Numberformat.Format = "dd-mm-yyyy";
                        excelDetalles.Cells[1, 14, excelDetalles.Dimension.End.Row, 14].Style.Numberformat.Format = "dd-mm-yyyy";
                        excelDetalles.Cells[1, 16, excelDetalles.Dimension.End.Row, 16].Style.Numberformat.Format = "dd-mm-yyyy";

                        int cont = 0;
                        foreach (var item in excelDetalles.Cells["B" + excelDetalles.Dimension.Start.Row + ":" + "B" + excelDetalles.Dimension.End.Row])
                        {
                            if (cont > 0 && (DateTime?)item.Value != null)
                            {
                                item.Formula = "=DATEVALUE(\"" + Convert.ToDateTime(item.Value).ToString("dd/MM/yyyy") +"\")";
                            }
                            cont++;
                        }
                        cont = 0;
                        foreach (var item in excelDetalles.Cells["C" + excelDetalles.Dimension.Start.Row + ":" + "C" + excelDetalles.Dimension.End.Row])
                        {
                            if (cont > 0 && (DateTime?)item.Value != null)
                            {
                                item.Formula = "=DATEVALUE(\"" + Convert.ToDateTime(item.Value).ToString("dd/MM/yyyy") + "\")";
                            }
                            cont++;
                        }
                        cont = 0;
                        foreach (var item in excelDetalles.Cells["N" + excelDetalles.Dimension.Start.Row + ":" + "N" + excelDetalles.Dimension.End.Row])
                        {
                            if (cont > 0 && (DateTime?)item.Value != null)
                            {
                                item.Formula = "=DATEVALUE(\"" + Convert.ToDateTime(item.Value).ToString("dd/MM/yyyy") + "\")";
                            }
                            cont++;
                        }
                        cont = 0;
                        foreach (var item in excelDetalles.Cells["P" + excelDetalles.Dimension.Start.Row + ":" + "P" + excelDetalles.Dimension.End.Row])
                        {
                            if (cont > 0 && (DateTime?)item.Value != null)
                            {
                                item.Formula = "=DATEVALUE(\"" + Convert.ToDateTime(item.Value).ToString("dd/MM/yyyy") + "\")";
                            }
                            cont++;
                        }

                        cont = 0;
                        foreach (var item in excelDetalles.Cells["AC" + excelDetalles.Dimension.Start.Row + ":" + "AC" + excelDetalles.Dimension.End.Row])
                        {
                            if (cont > 0)
                            {
                                if ((bool)item.Value)
                                {
                                    item.Value = "No";
                                    continue;
                                }
                                item.Value = "Si";
                            }
                            cont++;
                        }

                        tab.TableStyle = TableStyles.Medium17;

                                                
                        var letraIni = OfficeOpenXml.ExcelCellAddress.GetColumnLetter(columnasMonto["moi"] - 3);
                        var letraFin = OfficeOpenXml.ExcelCellAddress.GetColumnLetter(columnasMonto["moi"] - 1);
                        excelDetalles.Cells[letraIni + (range.Rows + 2).ToString() + ":" + letraFin + (range.Rows + 2).ToString()].Merge = true;

                        var cellsMerged = excelDetalles.MergedCells[0];
                        excelDetalles.Cells[cellsMerged].Value = "TOTALES";
                        excelDetalles.Cells[cellsMerged].Style.Font.Bold = true;
                        excelDetalles.Cells[cellsMerged].Style.Font.Size = 12;
                        excelDetalles.Cells[cellsMerged].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        excelDetalles.Cells[cellsMerged].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        foreach (var columna in columnasMonto)
                        {
                            var letraColumna = OfficeOpenXml.ExcelCellAddress.GetColumnLetter(columna.Value);
                            excelDetalles.Cells[range.Rows + 2, columna.Value].Formula = "=SUM(" + letraColumna + "2:" + letraColumna + range.Rows + ")";
                            excelDetalles.Cells[range.Rows + 2, columna.Value].Style.Numberformat.Format = "$#,##0.00";
                            excelDetalles.Cells[range.Rows + 2, columna.Value].Style.Font.Bold = true;
                            excelDetalles.Cells[range.Rows + 2, columna.Value].Style.Font.Size = 12;
                        }

                        excelDetalles.Cells[excelDetalles.Dimension.Address].AutoFitColumns();
                    }

                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan && colombia != null)
                    {
                        foreach (var cedula in colombia)
                        {
                            var excelDetalles = excel.Workbook.Worksheets.Add(cedula.saldo.cuenta + " " + cedula.saldo.concepto);

                            var header = new List<string> {
                                "Factura", "# Económico", "Descripción", "Fecha Movimiento", "Fecha Inicio Dep",
                                "Meses de dep", "% dep", "CC", "Póliza", "Cuenta", "MOI", "Alta", "Baja", "Fecha baja",
                                "Dep mensual", "Meses dep anteriores", "Meses dep actual", "Dep anterior", "Dep actual",
                                "Baja dep", "Dep acumulada", "Saldo Libro"
                            };

                            for (int i = 1; i <= header.Count; i++)
                            {
                                excelDetalles.Cells[1, i].Value = header[i - 1];
                            }

                            var cellData = new List<object[]>();
                            foreach (var detalle in cedula.detalle)
                            {
                                cellData.Add(new object[] {
                                    detalle.factura,
                                    detalle.noEconomico,
                                    detalle.descripcion,
                                    detalle.fechaMovimiento,
                                    detalle.fechaInicioDep,
                                    detalle.mesesDepreciacion,
                                    detalle.porcentajeDepreciacion,
                                    detalle.ccCompleto,
                                    detalle.polizaCompleta,
                                    detalle.cuentaCompleta,
                                    detalle.moi,
                                    detalle.alta,
                                    detalle.baja,
                                    detalle.fechaBaja,
                                    detalle.depMensual,
                                    detalle.mesesDepreciadosAnteriormente,
                                    detalle.mesesDepreciadosActualmente,
                                    detalle.depreciacionAnterior,
                                    detalle.depreciacionActual,
                                    detalle.bajaDepreciacion,
                                    detalle.depreciacionAcumulada,
                                    detalle.saldoLibro
                                });
                            }

                            excelDetalles.Cells[2, 1].LoadFromArrays(cellData);

                            ExcelRange range = excelDetalles.Cells[1, 1, excelDetalles.Dimension.End.Row, excelDetalles.Dimension.End.Column];

                            var cellFont = range.Style.Font;
                            cellFont.SetFromFont(new Font("Arial", 12));

                            ExcelTable tab = excelDetalles.Tables.Add(range, "Tabla" + cedula.saldo.cuenta.ToString() + "Colombia");

                            excelDetalles.Cells[1, 11, excelDetalles.Dimension.End.Row, 13].Style.Numberformat.Format = "$#,##0.00";
                            excelDetalles.Cells[1, 15, excelDetalles.Dimension.End.Row, 15].Style.Numberformat.Format = "$#,##0.00";
                            excelDetalles.Cells[1, 18, excelDetalles.Dimension.End.Row, 22].Style.Numberformat.Format = "$#,##0.00";

                            excelDetalles.Cells[1, 4, excelDetalles.Dimension.End.Row, 5].Style.Numberformat.Format = "dd-mm-yyyy";
                            excelDetalles.Cells[1, 14, excelDetalles.Dimension.End.Row, 14].Style.Numberformat.Format = "dd-mm-yyyy";

                            tab.TableStyle = TableStyles.Medium17;

                            excelDetalles.Cells[range.Rows + 2, 1, range.Rows + 2, 10].Merge = true;

                            var cellsMerged = excelDetalles.MergedCells[0];
                            excelDetalles.Cells[cellsMerged].Value = "TOTALES";
                            excelDetalles.Cells[cellsMerged].Style.Font.Bold = true;
                            excelDetalles.Cells[cellsMerged].Style.Font.Size = 12;
                            excelDetalles.Cells[cellsMerged].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            excelDetalles.Cells[cellsMerged].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                            var columnasMontoColombia = new List<int> { 11, 12, 13, 15, 18, 19, 20, 21, 22 };

                            foreach (var columna in columnasMontoColombia)
                            {
                                var letraColumna = OfficeOpenXml.ExcelCellAddress.GetColumnLetter(columna);
                                excelDetalles.Cells[range.Rows + 2, columna].Formula = "=SUM(" + letraColumna + "2:" + letraColumna + range.Rows + ")";
                                excelDetalles.Cells[range.Rows + 2, columna].Style.Numberformat.Format = "$#,##0.00";
                                excelDetalles.Cells[range.Rows + 2, columna].Style.Font.Bold = true;
                                excelDetalles.Cells[range.Rows + 2, columna].Style.Font.Size = 12;
                            }

                            excelDetalles.Cells[excelDetalles.Dimension.Address].AutoFitColumns();
                        }
                    }

                    var bytes = new MemoryStream();

                    using (var stream = new MemoryStream())
                    {
                        excel.SaveAs(stream);
                        bytes = stream;
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, bytes);
                }
            }
            //CREACIÓN EXCEL FIN
            }

            return resultado;
        }

        public Dictionary<string, object> GetDetalleCuenta(DateTime fechaHasta, int cuenta)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var cuentas = new List<int>{ cuenta };

                var detallesCreados = construirDetalles(fechaHasta, cuentas, false);

                if ((bool)detallesCreados[SUCCESS])
                {
                    var detalle = (List<ActivoFijoDetalleCuentaDTO>)detallesCreados[ITEMS];

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, detalle.FirstOrDefault().Detalles);
                }
                else
                {
                    resultado = detallesCreados;
                }
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al consultar la base de datos para obtener las cuentas. " + ex.ToString());
            }

            return resultado;
        }

        public Dictionary<string, object> GetResumen(DateTime fechaHasta)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                if ((EmpresaEnum)vSesiones.sesionEmpresaActual == EmpresaEnum.Peru)
                {
                    using (var ctx = new MainContext(EmpresaEnum.Construplan))
                    {
                        var cuentasActivoPeru = ctx.tblC_AF_CuentaPeru.Where(x => x.estatus && !x.peruMexico && x.tipoCuentaId == (int)AFTipoCuentaEnum.Movimiento).ToList();
                        var cedulaPeru = construirDetallesPeru(fechaHasta, cuentasActivoPeru.Select(x => x.cuenta).ToList());

                        resultado.Add(SUCCESS, true);
                        resultado.Add("peru", cedulaPeru);
                    }
                }
                else if ((EmpresaEnum)vSesiones.sesionEmpresaActual == EmpresaEnum.Colombia)
                {
                    using (var ctx = new MainContext(EmpresaEnum.Construplan))
                    {
                        var cuentasActivoColombia = ctx.tblC_AF_CuentaColombia.Where(x => x.estatus && !x.colombiaMexico && x.tipoCuentaId == (int)AFTipoCuentaEnum.Movimiento).ToList();
                        var cedulaColombia = construirDetallesColombia(fechaHasta, cuentasActivoColombia.Select(x => x.cuenta).ToList());

                        resultado.Add(SUCCESS, true);
                        resultado.Add("colombia", cedulaColombia);
                    }
                }
                else
                {
                    var cuentas = _context.tblC_AF_Cuentas.Where(x => x.Estatus).Select(x => x.Cuenta).ToList();

                    if (cuentas != null && cuentas.Count > 0)
                    {
                        var detallesCreados = construirDetalles(fechaHasta, cuentas, true);

                        if ((bool)detallesCreados[SUCCESS])
                        {
                            var resumen = new ActivoFijoResumenCedulaDTO((List<ActivoFijoSaldosDTO>)detallesCreados["ResumenSaldos"], (List<ActivoFijoDepreciacionDTO>)detallesCreados["ResumenDepreciacion"]);

                            //JUNTA CUENTAS 1204 Y 1230 EN CONSTRUPLAN
                            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                            {
                                var _1204 = resumen.Depreciaciones.FirstOrDefault(w => w.Cuenta == 1204);
                                var _1230 = resumen.Depreciaciones.FirstOrDefault(w => w.Cuenta == 1230);

                                _1204.DepreciacionAcumulada += _1230.DepreciacionAcumulada;
                                _1204.DepreciacionAnterior += _1230.DepreciacionAnterior;

                                resumen.SumaDepreciacion_Registrada -= _1204.DepreciacionRegistrada;

                                resumen.SumaDepreciacion_Diferencia -= _1204.DepreciacionRegistrada;

                                resumen.Depreciaciones.RemoveAll(r => r.Cuenta == 1230);
                            }
                            //

                            resultado.Add(SUCCESS, true);
                            resultado.Add(ITEMS, resumen);
                            resultado.Add("DiferenciasContables", detallesCreados["DiferenciasContables"]);
                            resultado.Add("DiferenciasContablesDep", detallesCreados["DiferenciasContablesDep"]);
                            resultado.Add("Excel", detallesCreados[ITEMS]);
                            var resumenTotalizadores = new ActivoFijoResumenTotalizadoresDTO((List<ActivoFijoDetallesTotalizadoresDTO>)detallesCreados["Totalizadores"]);

                            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                            {
                                var _1204t = resumenTotalizadores.Detalles.FirstOrDefault(f => f.Cuenta == 1204);
                                var _1230t = resumenTotalizadores.Detalles.FirstOrDefault(f => f.Cuenta == 1230);

                                _1204t.DepAcumuladaAnterior += _1230t.DepAcumuladaAnterior;
                                _1204t.DepAñoActual += _1230t.DepAñoActual;
                                _1204t.BajaDep += _1230t.BajaDep;
                                _1204t.DepContableAcumulada += _1230t.DepContableAcumulada;
                                _1204t.DepValLibros += _1230t.DepValLibros;

                                _1204t.DepValLibrosSalCont += _1204t.DepContableAcumuladaSalCont + _1230t.DepValLibrosSalCont;

                                resumenTotalizadores.Detalles.RemoveAll(r => r.Cuenta == 1230);
                                resumenTotalizadores.TotalDepAcumuladaAnteriorSalCont -= _1230t.DepAcumuladaAnteriorSalCont;
                                resumenTotalizadores.TotalDepAñoActualSalCont -= _1230t.DepAñoActualSalCont;
                                resumenTotalizadores.TotalBajaDepSalCont -= _1230t.BajaDepSalCont;
                                resumenTotalizadores.TotalDepContableAcumuladaSalCont -= _1204t.DepContableAcumuladaSalCont;
                                resumenTotalizadores.TotalDepValLibrosSalCont += _1204t.DepContableAcumuladaSalCont;
                            }

                            resultado.Add("Totalizadores", resumenTotalizadores);
                            resultado.Add("FechaHasta", detallesCreados["FechaHasta"]);

                            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                            {
                                var cuentasActivoColombia = _context.tblC_AF_CuentaColombia.Where(w => w.estatus && !w.colombiaMexico && w.tipoCuentaId == (int)AFTipoCuentaEnum.Movimiento).ToList();
                                var cedulaColombia = construirDetallesColombia(fechaHasta, cuentasActivoColombia.Select(m => m.cuenta).ToList());

                                resultado.Add("colombia", cedulaColombia);

                                var cuentasActivoPeru = _context.tblC_AF_CuentaPeru.Where(x => x.estatus && !x.peruMexico && x.tipoCuentaId == (int)AFTipoCuentaEnum.Movimiento).ToList();
                                var cedulaPeru = construirDetallesPeru(fechaHasta, cuentasActivoPeru.Select(x => x.cuenta).ToList());

                                resultado.Add("peru", cedulaPeru);
                            }
                        }
                        else
                        {
                            resultado = detallesCreados;
                        }
                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se encontraron cuentas");
                    }
                }
            }
            catch (Exception ex)
            {
                resultado.Remove(SUCCESS);

                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al consultar la base de datos para obtener las cuentas. " + ex.ToString());
            }

            return resultado;
        }

        private Dictionary<string, object> construirDetalles(DateTime fechaHasta, List<int> cuentas, bool verDetalle, string noEconomico = null)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

            try
            {
                fechaCortePol = fechaHasta;

                int mesActual = fechaHasta.Month;
                int añoActual = fechaHasta.Year;
                List<int> añosAnteriores = new List<int>();

                int añoInicio = 2020;

                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                {
                    añoInicio = 1987;
                }
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                {
                    añoInicio = 2018;
                }

                for (int año = añoInicio; año < añoActual; año++)
                {
                    añosAnteriores.Add(año);
                }

                if (añosAnteriores.Count == 0)
                {
                    añosAnteriores.Add(2017);
                }

                Dictionary<string, object> resultadoConsulta = getInfoCuentas(añoActual, mesActual, cuentas, verDetalle, noEconomico);

                var virtuales = new List<string>{ "1010", "1015", "1018" };

                var catMaqArrendadora = new List<tblM_CatMaquina>();
                var ccArrendadora = new List<tblP_CC>();
                var corteInventario = new List<ActivoFijoCorteInventarioDTO>();
                using (var ctx = new MainContext(EmpresaEnum.Arrendadora))
                {
                    catMaqArrendadora = ctx.tblM_CatMaquina.Where(w => !string.IsNullOrEmpty(w.noEconomico)).ToList();
                    ccArrendadora = ctx.tblP_CC.ToList();

                    var corteInventarioSemanal = ctx.tblM_CorteInventarioMaq.Where(x => x.Estatus && x.FechaCorte <= fechaHasta).OrderByDescending(x => x.FechaCorte).FirstOrDefault();
                    if (corteInventarioSemanal != null)
                    {
                        corteInventario = ctx.tblM_CorteInventarioMaq_Detalle
                            .Where(x => x.IdCorteInvMaq == corteInventarioSemanal.Id && x.Estatus)
                            .Select(x => new ActivoFijoCorteInventarioDTO
                            {
                                economicoId = x.IdEconomico,
                                noEconomico = x.Economico,
                                areaCuenta = virtuales.Contains(x.ccCargoObra) ? "14-1" : x.ccCargoObra,
                                cc = virtuales.Contains(x.cc.Trim()) ? "997" : x.cc.Trim(),
                                obra = virtuales.Contains(x.ccCargoObra) ? "MAQUINARIA NO ASIGNADA A OBRA" : x.CargoObra,
                                fechaCorte = corteInventarioSemanal.FechaCorte
                            }).ToList();
                    }

                    fechaHasta = new DateTime(añoActual, mesActual, DateTime.DaysInMonth(añoActual, mesActual));

                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora && fechaHasta >= new DateTime(2023, 06, 01))
                    {
                        var fechaCorteInventario = new DateTime(2023, 05, 30);
                        corteInventarioMayo = ctx.tblM_CorteInventarioMaq_Detalle.Where(x => x.IdCorteInvMaq == 25132 && x.Estatus)
                            .Select(x => new ActivoFijoCorteInventarioDTO
                            {
                                economicoId = x.IdEconomico,
                                noEconomico = x.Economico,
                                areaCuenta = virtuales.Contains(x.ccCargoObra) ? "14-1" : x.ccCargoObra,
                                cc = virtuales.Contains(x.cc.Trim()) ? "997" : x.cc.Trim(),
                                obra = virtuales.Contains(x.ccCargoObra) ? "MAQUINARIA NO ASIGNADA A OBRA" : x.CargoObra,
                                fechaCorte = fechaCorteInventario
                            }).ToList();

                        corteInventarioEneroJulio = ctx.tblM_CorteInventarioMaq_Detalle
                            .Where(x =>
                                x.Estatus &&
                                x.Corte.Estatus &&
                                fechasCorteEneroJulio.Contains(x.Corte.FechaCorte) &&
                                (virtuales.Contains(x.ccCargoObra) || x.ccCargoObra == "14-1"))
                            .Select(x => new ActivoFijoCorteInventarioDTO
                            {
                                economicoId = x.IdEconomico,
                                noEconomico = x.Economico,
                                areaCuenta = "14-1",
                                cc = "997",
                                obra = "MAQUINARIA NO ASIGNADA A OBRA",
                                fechaCorte = x.Corte.FechaCorte
                            }).ToList();

                        #region AJUSTES EN INVENTARIO
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1393,
                            noEconomico = "TC-52",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 10)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1201,
                            noEconomico = "CFC-05",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 2, 27)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1203,
                            noEconomico = "CFC-07",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 2, 27)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1204,
                            noEconomico = "CFC-08",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 10)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1204,
                            noEconomico = "CFC-08",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 3)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1204,
                            noEconomico = "CFC-08",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 24)
                        });
                        corteInventarioEneroJulio.RemoveAll(x => x.fechaCorte == new DateTime(2023, 1, 31) && x.economicoId == 1204); //CFC-08
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1240,
                            noEconomico = "CFC-44",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 3)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1240,
                            noEconomico = "CFC-44",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 10)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1240,
                            noEconomico = "CFC-44",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 24)
                        });
                        corteInventarioEneroJulio.RemoveAll(x => x.fechaCorte == new DateTime(2023, 3, 21) && x.economicoId == 1240); //CFC-44
                        corteInventarioEneroJulio.RemoveAll(x => x.fechaCorte == new DateTime(2023, 1, 31) && x.economicoId == 1240); //CFC-44
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45184,
                            noEconomico = "CF-84",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 3)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45184,
                            noEconomico = "CF-84",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 10)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1241,
                            noEconomico = "CFC-45",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 3)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1241,
                            noEconomico = "CFC-45",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 10)
                        });
                        corteInventarioEneroJulio.RemoveAll(x => x.fechaCorte == new DateTime(2023, 3, 21) && x.economicoId == 4537); //CFC-50
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 5653,
                            noEconomico = "CFC-54",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 3)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 5653,
                            noEconomico = "CFC-54",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 10)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 5665,
                            noEconomico = "CFC-56",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 3)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 5665,
                            noEconomico = "CFC-56",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 10)
                        });
                        corteInventarioEneroJulio.RemoveAll(x => x.fechaCorte == new DateTime(2023, 3, 31) && x.economicoId == 7853); //CFC-64
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 7853,
                            noEconomico = "CFC-64",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 3)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 7853,
                            noEconomico = "CFC-64",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 10)
                        });
                        corteInventarioEneroJulio.RemoveAll(x => x.fechaCorte == new DateTime(2023, 3, 7) && x.economicoId == 7854); //CFC-65
                        corteInventarioEneroJulio.RemoveAll(x => x.fechaCorte == new DateTime(2023, 1, 31) && x.economicoId == 7854); //CFC-65
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 7854,
                            noEconomico = "CFC-65",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 3)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 7854,
                            noEconomico = "CFC-65",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 10)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 7854,
                            noEconomico = "CFC-65",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 2, 28)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 7905,
                            noEconomico = "CFC-70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 3)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 7905,
                            noEconomico = "CFC-70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 10)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 7905,
                            noEconomico = "CFC-70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 24)
                        });
                        corteInventarioEneroJulio.RemoveAll(x => x.fechaCorte == new DateTime(2023, 1, 31) && x.economicoId == 7911); //CFC-74
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 7911,
                            noEconomico = "CFC-74",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 3)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 7911,
                            noEconomico = "CFC-74",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 10)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 7911,
                            noEconomico = "CFC-74",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 24)
                        });
                        corteInventarioEneroJulio.RemoveAll(x => x.fechaCorte == new DateTime(2023, 1, 31) && x.economicoId == 9993); //CFC-81
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 10008,
                            noEconomico = "CFC-82",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 3)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 10008,
                            noEconomico = "CFC-82",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 10)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 10008,
                            noEconomico = "CFC-82",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 25)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 10007,
                            noEconomico = "CFC-83",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 3)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 10007,
                            noEconomico = "CFC-83",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 10)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 10007,
                            noEconomico = "CFC-83",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 24)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 3)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 10)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 17)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 24)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 2, 7)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 2, 14)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 2, 21)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 2, 28)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 3, 7)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 3, 14)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 3, 21)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 3, 28)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 4, 4)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 4, 11)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 4, 18)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 4, 26)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 5, 9)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 5, 16)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 5, 23)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 5, 30)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 6, 6)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 6, 13)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 6, 20)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 6, 27)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 7, 4)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 7, 11)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 7, 18)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 45239,
                            noEconomico = "CF-R70",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 7, 25)
                        });
                        corteInventarioEneroJulio.RemoveAll(x => x.fechaCorte == new DateTime(2023, 1, 31) && x.economicoId == 1345); //PR-01
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1345,
                            noEconomico = "PR-01",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 3)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1345,
                            noEconomico = "PR-01",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 10)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1345,
                            noEconomico = "PR-01",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 24)
                        });
                        corteInventarioEneroJulio.RemoveAll(x => x.fechaCorte == new DateTime(2023, 1, 31) && x.economicoId == 1348); //PR-04
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1348,
                            noEconomico = "PR-04",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 3)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1348,
                            noEconomico = "PR-04",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 10)
                        });
                        corteInventarioEneroJulio.RemoveAll(x => x.fechaCorte == new DateTime(2023, 2, 7) && x.economicoId == 1349); //PR-05
                        corteInventarioEneroJulio.RemoveAll(x => x.fechaCorte == new DateTime(2023, 1, 31) && x.economicoId == 1349); //PR-05
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1349,
                            noEconomico = "PR-05",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 3)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1349,
                            noEconomico = "PR-05",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 10)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1349,
                            noEconomico = "PR-05",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 17)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1349,
                            noEconomico = "PR-05",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 24)
                        });
                        corteInventarioEneroJulio.RemoveAll(x => x.fechaCorte == new DateTime(2023, 1, 31) && x.economicoId == 1381); //TC-39
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1381,
                            noEconomico = "TC-39",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 3)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1381,
                            noEconomico = "TC-39",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 10)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1381,
                            noEconomico = "TC-39",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 24)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1391,
                            noEconomico = "TC-50",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 3)
                        });
                        corteInventarioEneroJulio.RemoveAll(x => x.fechaCorte == new DateTime(2023, 1, 31) && x.economicoId == 1399); //TC-58
                        corteInventarioEneroJulio.RemoveAll(x => x.fechaCorte == new DateTime(2023, 3, 7) && x.economicoId == 1399); //TC-58
                        corteInventarioEneroJulio.RemoveAll(x => x.fechaCorte == new DateTime(2023, 3, 14) && x.economicoId == 1399); //TC-58
                        corteInventarioEneroJulio.RemoveAll(x => x.fechaCorte == new DateTime(2023, 3, 21) && x.economicoId == 1399); //TC-58
                        corteInventarioEneroJulio.RemoveAll(x => x.fechaCorte == new DateTime(2023, 3, 28) && x.economicoId == 1399); //TC-58
                        corteInventarioEneroJulio.RemoveAll(x => x.fechaCorte == new DateTime(2023, 1, 31) && x.economicoId == 1174); //CF-45
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1174,
                            noEconomico = "CF-45",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 3)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1174,
                            noEconomico = "CF-45",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 10)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1174,
                            noEconomico = "CF-45",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 24)
                        });
                        corteInventarioEneroJulio.RemoveAll(x => x.fechaCorte == new DateTime(2023, 3, 21) && x.economicoId == 7942); //CF-81
                        corteInventarioEneroJulio.RemoveAll(x => x.fechaCorte == new DateTime(2023, 2, 28) && x.economicoId == 1206); //CFC-10
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1206,
                            noEconomico = "CFC-10",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 3)
                        });
                        corteInventarioEneroJulio.Add(new ActivoFijoCorteInventarioDTO
                        {
                            economicoId = 1206,
                            noEconomico = "CFC-10",
                            areaCuenta = "14-1",
                            cc = "997",
                            obra = "MAQUINARIA NO ASIGNADA A OBRA",
                            fechaCorte = new DateTime(2023, 1, 10)
                        });
                        //corteInventarioEneroJulio.RemoveAll(x => x.fechaCorte == new DateTime(2023, 1, 31) && x.economicoId == 1185); //CF-58
                        //corteInventarioEneroJulio.RemoveAll(x => x.fechaCorte == new DateTime(2023, 5, 2) && x.economicoId == 1185); //CF-58
                        #endregion

                        var diasCorteMes = Ultimos4MartesDelMes(fechaHasta);
                        if (diasCorteMes.Count == 5)
                        {
                            diasCorteMes.RemoveAt(0);
                        }

                        List<DateTime> fechasCorteMes = new List<DateTime>();
                        foreach (var item in diasCorteMes)
                        {
                            fechasCorteMes.Add(new DateTime(añoActual, mesActual, item));
                        }

                        corteInventarioDelMes = ctx.tblM_CorteInventarioMaq_Detalle
                            .Where(x =>
                                x.Estatus &&
                                x.Corte.Estatus &&
                                fechasCorteMes.Contains(x.Corte.FechaCorte))
                            .Select(x => new ActivoFijoCorteInventarioDTO
                            {
                                economicoId = x.IdEconomico,
                                noEconomico = x.Economico,
                                areaCuenta = virtuales.Contains(x.ccCargoObra) ? "14-1" : x.ccCargoObra,
                                cc = virtuales.Contains(x.cc.Trim()) ? "997" : x.cc.Trim(),
                                obra = virtuales.Contains(x.ccCargoObra) ? "MAQUINARIA NO ASIGNADA A OBRA" : x.CargoObra,
                                fechaCorte = x.Corte.FechaCorte
                            }).ToList();

                        if (fechaHasta >= new DateTime(2023, 8, 1))
                        {
                            var fechaInicial = new DateTime(2023, 8, 1);
                            var fechaFinal = fechaCortePol;
                            var diferenciaMeses = ((fechaFinal.Year - fechaInicial.Year) * 12) + fechaFinal.Month - fechaInicial.Month + 1;

                            List<DateTime> fechasPrimeraSemana = new List<DateTime>();
                            var fecha = fechaInicial;
                            for (int i = 0; i < diferenciaMeses; i++)
                            {
                                fecha = fecha.AddMonths(i);
                                var diasCorteMartes = Ultimos4MartesDelMes(fecha);
                                if (diasCorteMartes.Count == 5)
                                {
                                    diasCorteMartes.RemoveAt(0);
                                }

                                fechasPrimeraSemana.Add(new DateTime(fecha.Year, fecha.Month, diasCorteMartes[0]));
                            }

                            primerasSemanas = ctx.tblM_CorteInventarioMaq_Detalle
                                .Where(x =>
                                    x.Estatus &&
                                    x.Corte.Estatus &&
                                    fechasPrimeraSemana.Contains(x.Corte.FechaCorte))
                                .Select(x => new ActivoFijoCorteInventarioDTO
                                {
                                    economicoId = x.IdEconomico,
                                    noEconomico = x.Economico,
                                    areaCuenta = virtuales.Contains(x.ccCargoObra) ? "14-1" : x.ccCargoObra,
                                    cc = virtuales.Contains(x.cc.Trim()) ? "997" : x.cc.Trim(),
                                    obra = virtuales.Contains(x.ccCargoObra) ? "MAQUINARIA NO ASIGNADA A OBRA" : x.CargoObra,
                                    //fechaCorte = corteInventarioSemanal.FechaCorte
                                    fechaCorte = x.Corte.FechaCorte
                                }).ToList();

                            foreach (var item in fechasPrimeraSemana)
                            {
                                primerasSemanas.Add(new ActivoFijoCorteInventarioDTO
                                {
                                    economicoId = 45239,
                                    noEconomico = "CF-R70",
                                    areaCuenta = "14-1",
                                    cc = "997",
                                    obra = "MAQUINARIA NO ASIGNADA A OBRA",
                                    fechaCorte = item
                                });

                                //primerasSemanas.Add(new ActivoFijoCorteInventarioDTO
                                //{
                                //    economicoId = 4539,
                                //    noEconomico = "CFC-53",
                                //    areaCuenta = "14-1",
                                //    cc = "997",
                                //    obra = "MAQUINARIA NO ASIGNADA A OBRA",
                                //    fechaCorte = item
                                //});
                            }
                        }
                    }
                }

                var _cambioAC = _context.tblC_AF_CambioAreaCuenta.Where(x => x.esActivo).ToList();

                if ((bool)resultadoConsulta[SUCCESS])
                {
                    List<sc_movpolDTO> lista_sc_movpol = (List<sc_movpolDTO>)resultadoConsulta[ITEMS];

                    //COMENTADO - YA NO SE USA
                    //List<sc_movpolDTO> lista_sc_movpol_copia = new List<sc_movpolDTO>();
                    
                    //foreach (var item in lista_sc_movpol)
                    //{
                    //    var copia_sc_movpol = new sc_movpolDTO();

                    //    copia_sc_movpol.Year = item.Year;
                    //    copia_sc_movpol.Mes = item.Mes;
                    //    copia_sc_movpol.Poliza = item.Poliza;
                    //    copia_sc_movpol.TP = item.TP;
                    //    copia_sc_movpol.Linea = item.Linea;
                    //    copia_sc_movpol.Cta = item.Cta;
                    //    copia_sc_movpol.Scta = item.Scta;
                    //    copia_sc_movpol.Sscta = item.Sscta;
                    //    copia_sc_movpol.Digito = item.Digito;
                    //    copia_sc_movpol.TM = item.TM;
                    //    copia_sc_movpol.Referencia = item.Referencia;
                    //    copia_sc_movpol.Cc = item.Cc;
                    //    copia_sc_movpol.Concepto = item.Concepto;
                    //    copia_sc_movpol.Monto = item.Monto;
                    //    copia_sc_movpol.ITM = item.ITM;
                    //    copia_sc_movpol.FechaPol = item.FechaPol;
                    //    copia_sc_movpol.FechaFactura = item.FechaFactura;
                    //    copia_sc_movpol.FechaCFD = item.FechaCFD;
                    //    copia_sc_movpol.Match = item.Match;
                    //    copia_sc_movpol.PolizaAlta = item.PolizaAlta;
                    //    copia_sc_movpol.AreaCuenta = item.AreaCuenta;
                    //    copia_sc_movpol.Factura = item.Factura;

                    //    lista_sc_movpol_copia.Add(copia_sc_movpol);
                    //}

                    var cuentasMaquinaria = (List<int>)GetCuentas((int)TipoResultadoCuentasMaquinaria.CuentasMaquinaria)[ITEMS];

                    List<ActivoFijoSaldosContablesDTO> lista_sc_salcont = new List<ActivoFijoSaldosContablesDTO>();
                    List<ActivoFijoSaldosContablesDTO> lista_sc_salcontTemp = new List<ActivoFijoSaldosContablesDTO>();
                    List<ActivoFijoSaldosContablesDTO> lista_sc_depCont = new List<ActivoFijoSaldosContablesDTO>();
                    List<sc_movpolDTO> lista_sc_movpolDep = new List<sc_movpolDTO>();
                    List<sc_movpolDTO> lista_sc_movpolDepTemp = new List<sc_movpolDTO>();
                    if (verDetalle)
                    {
                        lista_sc_salcont = (List<ActivoFijoSaldosContablesDTO>)resultadoConsulta["sc_salcont_cc"];
                        if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                        {
                            lista_sc_depCont = (List<ActivoFijoSaldosContablesDTO>)resultadoConsulta["depSalCont"];
                        }
                        lista_sc_movpolDep = (List<sc_movpolDTO>)resultadoConsulta["sc_movpolDep"];

                        foreach (var item in lista_sc_movpolDep)
                        {
                            var copia_sc_movpolDep = new sc_movpolDTO();

                            copia_sc_movpolDep.Year = item.Year;
                            copia_sc_movpolDep.Mes = item.Mes;
                            copia_sc_movpolDep.Poliza = item.Poliza;
                            copia_sc_movpolDep.TP = item.TP;
                            copia_sc_movpolDep.Linea = item.Linea;
                            copia_sc_movpolDep.Cta = item.Cta;
                            copia_sc_movpolDep.Scta = item.Scta;
                            copia_sc_movpolDep.Sscta = item.Sscta;
                            copia_sc_movpolDep.Digito = item.Digito;
                            copia_sc_movpolDep.TM = item.TM;
                            copia_sc_movpolDep.Referencia = item.Referencia;
                            copia_sc_movpolDep.Cc = item.Cc;
                            copia_sc_movpolDep.Concepto = item.Concepto;
                            copia_sc_movpolDep.Monto = item.Monto;
                            copia_sc_movpolDep.ITM = item.ITM;
                            copia_sc_movpolDep.FechaPol = item.FechaPol;
                            copia_sc_movpolDep.FechaFactura = item.FechaFactura;
                            copia_sc_movpolDep.FechaCFD = item.FechaCFD;
                            copia_sc_movpolDep.Match = item.Match;
                            copia_sc_movpolDep.PolizaAlta = item.PolizaAlta;
                            copia_sc_movpolDep.AreaCuenta = item.AreaCuenta;
                            copia_sc_movpolDep.Factura = item.Factura;

                            lista_sc_movpolDepTemp.Add(copia_sc_movpolDep);
                        }

                        foreach (var item in lista_sc_salcont)
                        {
                            var copia_sc_salcont = new ActivoFijoSaldosContablesDTO();

                            copia_sc_salcont.Year = item.Year;
                            copia_sc_salcont.Cta = item.Cta;
                            copia_sc_salcont.Scta = item.Scta;
                            copia_sc_salcont.Sscta = item.Sscta;
                            copia_sc_salcont.Cc = item.Cc;
                            copia_sc_salcont.Enecargos = item.Enecargos;
                            copia_sc_salcont.Eneabonos = item.Eneabonos;
                            copia_sc_salcont.Febcargos = item.Febcargos;
                            copia_sc_salcont.Febabonos = item.Febabonos;
                            copia_sc_salcont.Marcargos = item.Marcargos;
                            copia_sc_salcont.Marabonos = item.Marabonos;
                            copia_sc_salcont.Abrcargos = item.Abrcargos;
                            copia_sc_salcont.Abrabonos = item.Abrabonos;
                            copia_sc_salcont.Maycargos = item.Maycargos;
                            copia_sc_salcont.Mayabonos = item.Mayabonos;
                            copia_sc_salcont.Juncargos = item.Juncargos;
                            copia_sc_salcont.Junabonos = item.Junabonos;
                            copia_sc_salcont.Julcargos = item.Julcargos;
                            copia_sc_salcont.Julabonos = item.Julabonos;
                            copia_sc_salcont.Agocargos = item.Agocargos;
                            copia_sc_salcont.Agoabonos = item.Agoabonos;
                            copia_sc_salcont.Sepcargos = item.Sepcargos;
                            copia_sc_salcont.Sepabonos = item.Sepabonos;
                            copia_sc_salcont.Octcargos = item.Octcargos;
                            copia_sc_salcont.Octabonos = item.Octabonos;
                            copia_sc_salcont.Novcargos = item.Novcargos;
                            copia_sc_salcont.Novabonos = item.Novabonos;
                            copia_sc_salcont.Diccargos = item.Diccargos;
                            copia_sc_salcont.Dicabonos = item.Dicabonos;

                            lista_sc_salcontTemp.Add(copia_sc_salcont);
                        }
                    }

                    Dictionary<string, object> resultadoRelSubcuentas = getRelacionSubcuentas(añosAnteriores, cuentas);

                    if ((bool)resultadoRelSubcuentas[SUCCESS])
                    {
                        List<ActivoFijoDetalleCuentaDTO> activoFijoDetalleCuenta = new List<ActivoFijoDetalleCuentaDTO>();

                        List<tblC_AF_RelSubCuentas> relacionSubcuentas = (List<tblC_AF_RelSubCuentas>)resultadoRelSubcuentas[ITEMS];
                        var detallesTotalizadores = new List<ActivoFijoDetallesTotalizadoresDTO>();
                        var detallesTotalizadoresSalCont = new List<ActivoFijoDetallesTotalizadoresDTO>();

                        var relDepMaqPol = CatMaquinaDep(noEconomico);
                        var relacionDepMaqPol_all = (List<tblM_CatMaquinaDepreciacion>)relDepMaqPol[ITEMS];

                        if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                        {
                            sumaComplementos(lista_sc_movpol, relacionDepMaqPol_all, fechaHasta);
                        }

                        List<ActivoFijoSaldosDTO> saldos = new List<ActivoFijoSaldosDTO>();
                        List<ActivoFijoDepreciacionDTO> depreciaciones = new List<ActivoFijoDepreciacionDTO>();

                        var diferenciasContablesSaldos = new List<ActivoFijoDiferenciasContablesDTO>();
                        var diferenciasContablesDep = new List<ActivoFijoDiferenciasContablesDTO>();

                        _InsumoOverhaul = _context.tblC_AF_InsumosOverhaul.Where(w => w.Estatus).ToList();

                        foreach (int cuenta in cuentas)
                        {
                            _InfoCuentaDep = GetInfoDepB(cuenta);

                            List<ActivoFijoDetalleDTO> registrosCalculados = new List<ActivoFijoDetalleDTO>();
                            ActivoFijoSaldosDTO cuentaSaldos = new ActivoFijoSaldosDTO();
                            ActivoFijoDepreciacionDTO cuentaDepreciacion = new ActivoFijoDepreciacionDTO();

                            int mesesDeDepreciacion = relacionSubcuentas.FirstOrDefault(x => x.Cuenta.Cuenta == cuenta).Cuenta.MesesDeDepreciacion;
                            decimal porcentajeDepreciacion = relacionSubcuentas.FirstOrDefault(x => x.Cuenta.Cuenta == cuenta).Cuenta.PorcentajeDepreciacion;

                            //var areasCuenta = _context.tblP_CC.ToList();
                            //var catMaq = _context.tblM_CatMaquina.ToList();
                            var areasCuenta = ccArrendadora;
                            var catMaq = catMaqArrendadora;

                            cuentaDepreciacion.Cuenta = cuentaSaldos.Cuenta = cuenta;
                            cuentaDepreciacion.Concepto = cuentaSaldos.Concepto = relacionSubcuentas.FirstOrDefault(x => !x.EsCuentaDepreciacion && x.Cuenta.Cuenta == cuenta).Cuenta.Descripcion;

                            #region Cuentas maquinas
                            if (cuentasMaquinaria.Contains(cuenta))
                            {
                                var polizasEspeciales = _context.tblC_AF_PolizaEspecial.Where(x => x.registroActivo).ToList();

                                var maquinasNoInventario = _context.tblM_CatMaquinaDepreciacion_Extras.Where(w => w.Estatus).ToList();


                                List<ActivoFijoDetalleDTO> registrosCalculadosTemp = new List<ActivoFijoDetalleDTO>();
                                List<sc_movpolDTO> sc_movpolDTO_temp = new List<sc_movpolDTO>();

                                //Dictionary<string, object> relDepMaqPol = RelacionDepMaqPolizas(cuenta);
                                if ((bool)relDepMaqPol[SUCCESS])
                                {
                                    var relacionDepMaqPol = relacionDepMaqPol_all.Where(x => x.Poliza.Cuenta == cuenta).ToList();

                                    //
                                    foreach (var item in maquinasNoInventario.Where(w => w.Poliza.TM == 1 && w.Poliza.Cuenta == cuenta))
                                    {
                                        sc_movpolDTO relacion = null;

                                        if (polizasExceptions.Contains(item.Poliza.Poliza))
                                        {
                                            var ccNoEconomico = ccArrendadora.FirstOrDefault(f => f.cc == "997");

                                            relacion = new sc_movpolDTO()
                                            {
                                                Scta = item.Poliza.Subcuenta.Value,
                                                Sscta = item.Poliza.SubSubcuenta.Value,
                                                Concepto = item.Poliza.Concepto,
                                                Monto = item.Poliza.Monto.Value,
                                                Factura = item.Poliza.Factura,
                                                Year = item.Poliza.Año,
                                                Mes = item.Poliza.Mes,
                                                Poliza = item.Poliza.Poliza,
                                                TP = item.Poliza.TP,
                                                Linea = item.Poliza.Linea,
                                                Cc = ccNoEconomico != null && ccNoEconomico.areaCuenta != "1010" && ccNoEconomico.areaCuenta != "1015" && ccNoEconomico.areaCuenta != "1018" ? ccNoEconomico.cc : "997"
                                            };
                                        }
                                        else
                                        {
                                            relacion = lista_sc_movpol.FirstOrDefault
                                            (f =>
                                                item.Poliza.Año == f.Year &&
                                                item.Poliza.Mes == f.Mes &&
                                                item.Poliza.Poliza == f.Poliza &&
                                                item.Poliza.TP == f.TP &&
                                                item.Poliza.Linea == f.Linea
                                            );
                                        }

                                        var recalcular = false;

                                        do
                                        {
                                            recalcular = false;

                                            if (relacion != null)
                                            {
                                                tblP_CC acMatch = null;

                                                acMatch = areasCuenta.FirstOrDefault(x => x.cc == "997");

                                                tblC_AF_InsumosOverhaul insumoMatch = null;
                                                //foreach (var insmOverhaul in insumosOverhaul)
                                                //{
                                                //    if (relacion.Concepto.Contains(insmOverhaul.Descripcion))
                                                //    {
                                                //        insumoMatch = insmOverhaul;
                                                //        break;
                                                //    }
                                                //}

                                                ActivoFijoDetalleDTO registroCalculadoDTO = new ActivoFijoDetalleDTO
                                                {
                                                    Fecha = item.Poliza.FechaMovimiento,
                                                    //Fecha = relacion.FechaPol.ToShortDateString(),
                                                    Cuenta = cuenta,
                                                    Subcuenta = relacion.Scta,
                                                    SubSubcuenta = relacion.Sscta,
                                                    Cc = acMatch.cc,
                                                    Clave = item.IdCatMaquina,
                                                    Descripcion = relacion.Concepto,
                                                    MesesMaximoDepreciacion = insumoMatch != null ? insumoMatch.Meses : item.MesesTotalesDepreciacion.Value,
                                                    //MesesMaximoDepreciacion = infoSubDep != null ? infoSubDep.MesesDepreciacion : item.MesesTotalesDepreciacion.Value,
                                                    //MOI = añosAnteriores.Contains(relacion.Year) ? relacion.Monto : 0.0M,
                                                    MOI = añosAnteriores.Contains(item.Poliza.FechaMovimiento.Year) ? relacion.Monto : 0.0M,
                                                    Altas = item.TipoDelMovimiento != (int)TipoDelMovimiento.O && item.Poliza.FechaMovimiento.Year == añoActual ? relacion.Monto : 0.0M,
                                                    Overhaul = item.TipoDelMovimiento == (int)TipoDelMovimiento.O && item.Poliza.FechaMovimiento.Year == añoActual ? relacion.Monto : 0.0M,
                                                    EsOverhaul = item.TipoDelMovimiento == (int)TipoDelMovimiento.O ? true : false,
                                                    PorcentajeDepreciacion = insumoMatch != null && insumoMatch.Porcentaje != null ? insumoMatch.Porcentaje.Value : item.PorcentajeDepreciacion.Value,
                                                    //PorcentajeDepreciacion = infoSubDep != null ? infoSubDep.PorcentajeDepreciacion : item.PorcentajeDepreciacion.Value,
                                                    FechaInicioDepreciacion = item.FechaInicioDepreciacion.Value,
                                                    Factura = !string.IsNullOrEmpty(item.Poliza.Factura) ? item.Poliza.Factura : !string.IsNullOrEmpty(relacion.Factura) ? relacion.Factura : "",
                                                    Poliza = relacion.Year + "-" + relacion.Mes + "-" + relacion.Poliza + "-" + relacion.TP + "-" + relacion.Linea,
                                                    Area = acMatch != null && acMatch.areaCuenta != "1010" && acMatch.areaCuenta != "1015" && acMatch.areaCuenta != "1018" ? acMatch.area : 14,
                                                    Cuenta_OC = acMatch != null && acMatch.areaCuenta != "1010" && acMatch.areaCuenta != "1015" && acMatch.areaCuenta != "1018" ? acMatch.cuenta : 1,
                                                    AreaCuenta = acMatch != null && acMatch.areaCuenta != "1010" && acMatch.areaCuenta != "1015" && acMatch.areaCuenta != "1018" ? acMatch.areaCuenta + " - " + acMatch.descripcion : 14 + "-" + 1 + " - " + "MAQUINARIA NO ASIGNADA A OBRA",
                                                    AreaCuentaDescripcion = acMatch != null ? acMatch.descripcion : "MAQUINARIA NO ASIGNADA A OBRA",
                                                    AltaSigoplan = true,
                                                    AñoPol_alta = item.Poliza.Año,
                                                    MesPol_alta = item.Poliza.Mes,
                                                    PolPol_alta = item.Poliza.Poliza,
                                                    TpPol_alta = item.Poliza.TP,
                                                    LineaPol_alta = item.Poliza.Linea,
                                                    IdDepMaquina = item.Id,
                                                    EsExtraCatMaqDep = true,
                                                    IdCatMaquina = null,
                                                    TipoMovimiento = item.Movimiento.TipoDelMovimiento,
                                                    TipoActivo = item.Poliza.DescripcionTipoActivo.TipoActivo,
                                                    IdTipoMovimiento = item.Movimiento.Id
                                                };

                                                sc_movpolDTO_temp.Add(relacion);

                                                var b = maquinasNoInventario.FirstOrDefault(x => x.IdPolizaReferenciaAlta == item.IdPoliza);

                                                sc_movpolDTO baja = null;

                                                if (b != null && b.Poliza.FechaMovimiento <= fechaHasta)
                                                {
                                                    baja = lista_sc_movpol.FirstOrDefault(x => x.Year == b.Poliza.Año && x.Mes == b.Poliza.Mes && x.Poliza == b.Poliza.Poliza && x.TP == b.Poliza.TP && x.Linea == b.Poliza.Linea);
                                                    if (baja != null)
                                                    {
                                                        lista_sc_movpol.Remove(baja);
                                                        baja.FechaCFD = b.Poliza.FechaMovimiento;

                                                        //registroCalculadoDTO.FechaBaja = baja.FechaCFD.Value;
                                                        //registroCalculadoDTO.MontoBaja = baja.Monto;
                                                        //registroCalculadoDTO.FechaBajaPol = baja.FechaPol;

                                                        if (baja.TM == 2)
                                                        {
                                                            registroCalculadoDTO.FechaBaja = baja.FechaCFD.Value;
                                                            registroCalculadoDTO.MontoBaja = baja.Monto;
                                                            registroCalculadoDTO.FechaBajaPol = baja.FechaPol;

                                                            registroCalculadoDTO.BajaSigoplan = true;
                                                        }
                                                        if (baja.TM == 3)
                                                        {
                                                            registroCalculadoDTO.FechaCancelacion = baja.FechaCFD.Value;
                                                            registroCalculadoDTO.MontoCancelacion = baja.Monto;
                                                            registroCalculadoDTO.FechaCancelacionPol = baja.FechaPol;

                                                            registroCalculadoDTO.CancelacionSigoplan = true;
                                                        }
                                                    }
                                                }

                                                //
                                                if (b == null)
                                                {
                                                    baja = encontrarBaja(cuenta, registroCalculadoDTO, lista_sc_movpol, relacionSubcuentas, fechaHasta);
                                                    if (baja != null)
                                                    {
                                                        var existePolizaEspecial = polizasEspeciales.FirstOrDefault(x => x.year == baja.Year && x.mes == baja.Mes && x.poliza == baja.Poliza && x.tp == baja.TP && x.comportamientoId == ComportamientoPolizaEnum.NO_RELACIONAR);
                                                        if (existePolizaEspecial != null)
                                                        {
                                                            baja = null;
                                                        }

                                                        var existeBajaEnSigoplan = relacionDepMaqPol.Any(x => x.Poliza.Año == baja.Year && x.Poliza.Mes == baja.Mes && x.Poliza.Poliza == baja.Poliza && x.Poliza.TP == baja.TP && x.Poliza.Linea == baja.Linea);
                                                        var existeBajaEnSigoplan2 = maquinasNoInventario.Any(x => x.Poliza.Año == baja.Year && x.Poliza.Mes == baja.Mes && x.Poliza.Poliza == baja.Poliza && x.Poliza.TP == baja.TP && x.Poliza.Linea == baja.Linea);
                                                        if (existeBajaEnSigoplan || existeBajaEnSigoplan2)
                                                        {
                                                            baja = null;
                                                        }
                                                    }
                                                }
                                                //

                                                if (item.FechaInicioDepreciacion > fechaHasta)
                                                {
                                                    if (añoActual == registroCalculadoDTO.Fecha.Year && registroCalculadoDTO.FechaBaja == null && registroCalculadoDTO.FechaCancelacion == null)
                                                    {
                                                        registroCalculadoDTO.ValorEnLibros = (registroCalculadoDTO.MOI + registroCalculadoDTO.Altas + registroCalculadoDTO.Overhaul) - registroCalculadoDTO.DepreciacionContableAcumulada - registroCalculadoDTO.MontoBaja;
                                                    }

                                                    if (registroCalculadoDTO.FechaBaja != null && añosAnteriores.Contains(registroCalculadoDTO.FechaBaja.Value.Year))
                                                    {

                                                    }
                                                    else
                                                    {
                                                        if (registroCalculadoDTO.FechaBaja != null)
                                                        {
                                                            registroCalculadoDTO.ValorEnLibros = 0.0M;
                                                        }
                                                        else
                                                        {
                                                            registroCalculadoDTO.ValorEnLibros = (registroCalculadoDTO.MOI + registroCalculadoDTO.Altas + registroCalculadoDTO.Overhaul) - registroCalculadoDTO.DepreciacionContableAcumulada - registroCalculadoDTO.MontoBaja;
                                                        }
                                                    }

                                                    registrosCalculadosTemp.Add(registroCalculadoDTO);
                                                    continue;
                                                }

                                                Calculos(baja, lista_sc_movpol, registroCalculadoDTO, cuenta, relacionSubcuentas, registrosCalculados, añosAnteriores, añoActual, mesActual);
                                            }
                                            else
                                            {
                                                //recalcular = true;

                                                //var ccNoEconomico = ccArrendadora.FirstOrDefault(f => f.cc == "997");

                                                //relacion = new sc_movpolDTO()
                                                //{
                                                //    Scta = item.Poliza.Subcuenta.Value,
                                                //    Sscta = item.Poliza.SubSubcuenta.Value,
                                                //    Concepto = item.Poliza.Concepto,
                                                //    Monto = item.Poliza.Monto.Value,
                                                //    Factura = item.Poliza.Factura,
                                                //    Year = item.Poliza.Año,
                                                //    Mes = item.Poliza.Mes,
                                                //    Poliza = item.Poliza.Poliza,
                                                //    TP = item.Poliza.TP,
                                                //    Linea = item.Poliza.Linea,
                                                //    Cc = ccNoEconomico != null && ccNoEconomico.areaCuenta != "1010" && ccNoEconomico.areaCuenta != "1015" && ccNoEconomico.areaCuenta != "1018" ? ccNoEconomico.cc : "997"
                                                //};
                                            }
                                        } while (recalcular && vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora);

                                        //ToDo...
                                    }

                                    foreach (var item in sc_movpolDTO_temp)
                                    {
                                        lista_sc_movpol.Remove(item);
                                    }                                    
                                    //

                                    foreach (var item in relacionDepMaqPol.Where(x => x.Poliza.TM == 1))
                                    {
                                        item.Maquina = catMaq.First(f => f.id == item.IdCatMaquina);

                                        sc_movpolDTO relacion = null;
                                        if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan && polizasExceptions.Contains(item.Poliza.Poliza))
                                        {
                                            //var ccNoEconomico = ccArrendadora.FirstOrDefault(f => f.areaCuenta == item.Maquina.centro_costos && item.Maquina.estatus == 1);
                                            var ccNoEconomico = corteInventario.FirstOrDefault(x => x.economicoId == item.IdCatMaquina && item.Maquina.estatus == 1);

                                            relacion = new sc_movpolDTO()
                                            {
                                                Scta = item.Poliza.Subcuenta.Value,
                                                Sscta = item.Poliza.SubSubcuenta.Value,
                                                Concepto = item.Poliza.Concepto,
                                                Monto = item.Poliza.Monto.Value,
                                                Factura = item.Poliza.Factura,
                                                Year = item.Poliza.Año,
                                                Mes = item.Poliza.Mes,
                                                Poliza = item.Poliza.Poliza,
                                                TP = item.Poliza.TP,
                                                Linea = item.Poliza.Linea,
                                                Cc = ccNoEconomico != null && ccNoEconomico.areaCuenta != "1010" && ccNoEconomico.areaCuenta != "1015" && ccNoEconomico.areaCuenta != "1018" && ccNoEconomico.areaCuenta != "14-1" ? ccNoEconomico.cc : "997"
                                            };
                                        }
                                        if (relacion == null/*vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora*/)
                                        {
                                            relacion = lista_sc_movpol.FirstOrDefault
                                            (x =>
                                                item.Poliza.Año == x.Year && item.Poliza.Mes == x.Mes && item.Poliza.Poliza == x.Poliza &&
                                                item.Poliza.TP == x.TP && item.Poliza.Linea == x.Linea /*&&
                                                relacionSubcuentas.Where(r => !r.EsCuentaDepreciacion && r.Cuenta.Cuenta == cuenta && r.Subcuenta == x.Scta && r.SubSubcuenta == x.Sscta && r.Año == x.Year).Count() > 0 &&
                                                relacionSubcuentas.Where(r => !r.EsCuentaDepreciacion && r.Cuenta.Cuenta == cuenta && r.Excluir && r.Subcuenta == x.Scta && r.SubSubcuenta == x.Sscta && r.Año == x.Year).Count() == 0*/
                                            );
                                        }

                                        var recalcular = false;

                                        do
                                        {
                                            recalcular = false;

                                            if (relacion != null)
                                            {
                                                ActivoFijoCorteInventarioDTO acMatch = null;
                                                if (item.Maquina.estatus == 1)
                                                {
                                                    //acMatch = areasCuenta.FirstOrDefault(x => x.areaCuenta == item.Maquina.centro_costos);
                                                    acMatch = corteInventario.FirstOrDefault(x => x.economicoId == item.IdCatMaquina);
                                                    if (acMatch != null)
                                                    {
                                                        var acCambio = _cambioAC.FirstOrDefault(x => x.acAnterior == acMatch.areaCuenta && x.maquinaId == item.IdCatMaquina);
                                                        if (acCambio != null)
                                                        {
                                                            //acMatch = areasCuenta.FirstOrDefault(x => x.areaCuenta == acCambio.acNuevo);
                                                            acMatch = corteInventario.FirstOrDefault(x => x.areaCuenta == acCambio.acNuevo);
                                                        }
                                                    }
                                                }
                                                if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora)
                                                {
                                                    relacion.Cc = acMatch != null && acMatch.areaCuenta != "1010" && acMatch.areaCuenta != "1015" && acMatch.areaCuenta != "1018" && acMatch.areaCuenta != "14-1" ? acMatch.cc : "997";

                                                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan && acMatch != null && acMatch.cc == "935")
                                                    {
                                                        relacion.Cc = "010";
                                                    }
                                                }

                                                tblC_AF_InsumosOverhaul insumoMatch = null;
                                                //foreach (var insmOverhaul in insumosOverhaul)
                                                //{
                                                //    if (relacion.Concepto.Contains(insmOverhaul.Descripcion))
                                                //    {
                                                //        insumoMatch = insmOverhaul;
                                                //        break;
                                                //    }
                                                //}
                                                ActivoFijoDetalleDTO registroCalculadoDTO = new ActivoFijoDetalleDTO
                                                {
                                                    Fecha = item.Poliza.FechaMovimiento,
                                                    //Fecha = relacion.FechaPol.ToShortDateString(),
                                                    Cuenta = cuenta,
                                                    Subcuenta = relacion.Scta,
                                                    SubSubcuenta = relacion.Sscta,
                                                    Cc = relacion.Cc,
                                                    Clave = item.Maquina.noEconomico,
                                                    Descripcion = relacion.Concepto,
                                                    MesesMaximoDepreciacion = insumoMatch != null ? insumoMatch.Meses : item.MesesTotalesDepreciacion.Value,
                                                    //MesesMaximoDepreciacion = infoSubDep != null ? infoSubDep.MesesDepreciacion : item.MesesTotalesDepreciacion.Value,
                                                    //MOI = añosAnteriores.Contains(relacion.Year) ? relacion.Monto : 0.0M,
                                                    MOI = añosAnteriores.Contains(item.Poliza.FechaMovimiento.Year) ? relacion.Monto : 0.0M,
                                                    Altas = item.TipoDelMovimiento != (int)TipoDelMovimiento.O && item.Poliza.FechaMovimiento.Year == añoActual ? relacion.Monto : 0.0M,
                                                    Overhaul = item.TipoDelMovimiento == (int)TipoDelMovimiento.O && item.Poliza.FechaMovimiento.Year == añoActual ? relacion.Monto : 0.0M,
                                                    EsOverhaul = item.TipoDelMovimiento == (int)TipoDelMovimiento.O ? true : false,
                                                    PorcentajeDepreciacion = insumoMatch != null && insumoMatch.Porcentaje != null ? insumoMatch.Porcentaje.Value : item.PorcentajeDepreciacion.Value,
                                                    //PorcentajeDepreciacion = infoSubDep != null ? infoSubDep.PorcentajeDepreciacion : item.PorcentajeDepreciacion.Value,
                                                    FechaInicioDepreciacion = item.FechaInicioDepreciacion.Value,
                                                    Factura = !string.IsNullOrEmpty(item.Poliza.Factura) ? item.Poliza.Factura : !string.IsNullOrEmpty(relacion.Factura) ? relacion.Factura : "",
                                                    Poliza = relacion.Year + "-" + relacion.Mes + "-" + relacion.Poliza + "-" + relacion.TP + "-" + relacion.Linea,
                                                    //Area = acMatch != null && acMatch.areaCuenta != "1010" && acMatch.areaCuenta != "1015" && acMatch.areaCuenta != "1018" ? acMatch.area : 14,
                                                    Area = acMatch != null && acMatch.areaCuenta != "1010" && acMatch.areaCuenta != "1015" && acMatch.areaCuenta != "1018" ? Convert.ToInt32(acMatch.areaCuenta.Split('-')[0]) : 14,
                                                    //Cuenta_OC = acMatch != null && acMatch.areaCuenta != "1010" && acMatch.areaCuenta != "1015" && acMatch.areaCuenta != "1018" ? acMatch.cuenta : 1,
                                                    Cuenta_OC = acMatch != null && acMatch.areaCuenta != "1010" && acMatch.areaCuenta != "1015" && acMatch.areaCuenta != "1018" ? Convert.ToInt32(acMatch.areaCuenta.Split('-')[1]) : 1,
                                                    //AreaCuenta = acMatch != null && acMatch.areaCuenta != "1010" && acMatch.areaCuenta != "1015" && acMatch.areaCuenta != "1018" ? acMatch.areaCuenta + " - " + acMatch.descripcion : 14 + "-" + 1 + " - " + "MAQUINARIA NO ASIGNADA A OBRA",
                                                    AreaCuenta = acMatch != null && acMatch.areaCuenta != "1010" && acMatch.areaCuenta != "1015" && acMatch.areaCuenta != "1018" ? acMatch.areaCuenta + " - " + acMatch.obra : "14-1 - MAQUINARIA NO ASIGNADA A OBRA",
                                                    //AreaCuentaDescripcion = acMatch != null ? acMatch.descripcion : "MAQUINARIA NO ASIGNADA A OBRA",
                                                    AreaCuentaDescripcion = acMatch != null ? acMatch.obra : "MAQUINARIA NO ASIGNADA A OBRA",
                                                    AltaSigoplan = true,
                                                    AñoPol_alta = item.Poliza.Año,
                                                    MesPol_alta = item.Poliza.Mes,
                                                    PolPol_alta = item.Poliza.Poliza,
                                                    TpPol_alta = item.Poliza.TP,
                                                    LineaPol_alta = item.Poliza.Linea,
                                                    IdDepMaquina = item.Id,
                                                    IdCatMaquina = item.IdCatMaquina,
                                                    TipoMovimiento = item.Movimiento.TipoDelMovimiento,
                                                    TipoActivo = item.Poliza.DescripcionTipoActivo.TipoActivo,
                                                    IdTipoMovimiento = item.Movimiento.Id
                                                };

                                                sc_movpolDTO_temp.Add(relacion);

                                                var b = relacionDepMaqPol.FirstOrDefault(x => x.IdPolizaReferenciaAlta == item.IdPoliza);

                                                //Excepciones
                                                if (fechaHasta >= new DateTime(2021, 10, 01) && vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora && cuenta == 1210)
                                                {
                                                    if (item.IdPoliza == 22126)
                                                    {
                                                        item.Poliza.FechaMovimiento = new DateTime(2021, 10, 31);
                                                    }

                                                    var idPolizasAltasParaDesrelacionarBaja = new List<int>
                                                    {
                                                        17980, // CFC-10
                                                        26081,
                                                        //26064 // CFC-13
                                                    };

                                                    if (idPolizasAltasParaDesrelacionarBaja.Contains(item.IdPoliza) && b != null && b.Id != 37969)
                                                    {
                                                       b = null;
                                                    }
                                                }
                                                //Excepciones fin


                                                sc_movpolDTO baja = null;

                                                if (b != null && b.Poliza.FechaMovimiento <= fechaHasta)
                                                {
                                                    baja = lista_sc_movpol.FirstOrDefault(x => x.Year == b.Poliza.Año && x.Mes == b.Poliza.Mes && x.Poliza == b.Poliza.Poliza && x.TP == b.Poliza.TP && x.Linea == b.Poliza.Linea);

                                                    //PARA BAJA COSTO
                                                    if (b.IdBajaCosto != null && baja != null)
                                                    {
                                                        baja.bajaCosto = true;
                                                        baja.idBajaCosto = b.IdBajaCosto.Value;
                                                        baja.semanas = b.Costo.semanasUltimoMesDep;
                                                    }
                                                    //

                                                    if (baja != null)
                                                    {
                                                        lista_sc_movpol.Remove(baja);
                                                        baja.FechaCFD = b.Poliza.FechaMovimiento;

                                                        //registroCalculadoDTO.FechaBaja = baja.FechaCFD.Value;
                                                        //registroCalculadoDTO.MontoBaja = baja.Monto;
                                                        //registroCalculadoDTO.FechaBajaPol = baja.FechaPol;

                                                        if (baja.TM == 2)
                                                        {
                                                            registroCalculadoDTO.FechaBaja = baja.FechaCFD.Value;
                                                            registroCalculadoDTO.MontoBaja = baja.Monto;
                                                            registroCalculadoDTO.FechaBajaPol = baja.FechaPol;

                                                            registroCalculadoDTO.BajaSigoplan = true;
                                                        }
                                                        if (baja.TM == 3)
                                                        {
                                                            registroCalculadoDTO.FechaCancelacion = baja.FechaCFD.Value;
                                                            registroCalculadoDTO.MontoCancelacion = baja.Monto;
                                                            registroCalculadoDTO.FechaCancelacionPol = baja.FechaPol;

                                                            registroCalculadoDTO.CancelacionSigoplan = true;
                                                        }
                                                    }
                                                }

                                                //
                                                if (b == null)
                                                {
                                                    baja = encontrarBaja(cuenta, registroCalculadoDTO, lista_sc_movpol, relacionSubcuentas, fechaHasta);

                                                    if (baja != null)
                                                    {
                                                        var existePolizaEspecial = polizasEspeciales.FirstOrDefault(x => x.year == baja.Year && x.mes == baja.Mes && x.poliza == baja.Poliza && x.tp == baja.TP && x.comportamientoId == ComportamientoPolizaEnum.NO_RELACIONAR);
                                                        if (existePolizaEspecial != null)
                                                        {
                                                            baja = null;
                                                        }
                                                    }

                                                    if (baja != null && vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora && polizasExceptions.Contains(registroCalculadoDTO.PolPol_alta))
                                                    {
                                                        if (
                                                            cuenta == 1215 &&
                                                            (
                                                                (baja.FechaCFD != null && baja.FechaCFD.Value <= new DateTime(2020, 12, 31)) ||
                                                                (baja.FechaFactura != null && baja.FechaFactura.Value <= new DateTime(2020, 12, 31)) ||
                                                                (baja.FechaPol != null && baja.FechaPol <= new DateTime(2020, 12, 31))
                                                            )
                                                           )
                                                        {
                                                            baja = null;
                                                        }

                                                        if (
                                                            cuenta == 1210 &&
                                                            (
                                                                (baja.FechaCFD != null && baja.FechaCFD.Value <= new DateTime(2020, 12, 31)) ||
                                                                (baja.FechaFactura != null && baja.FechaFactura.Value <= new DateTime(2020, 12, 31)) ||
                                                                (baja.FechaPol != null && baja.FechaPol <= new DateTime(2020, 12, 31))
                                                            )
                                                           )
                                                        {
                                                            baja = null;
                                                        }

                                                        if (
                                                            cuenta == 1211 &&
                                                            (
                                                                (baja.FechaCFD != null && baja.FechaCFD.Value <= new DateTime(2020, 12, 31)) ||
                                                                (baja.FechaFactura != null && baja.FechaFactura.Value <= new DateTime(2020, 12, 31)) ||
                                                                (baja.FechaPol != null && baja.FechaPol <= new DateTime(2020, 12, 31))
                                                            )
                                                           )
                                                        {
                                                            baja = null;
                                                        }
                                                    }
                                                    if (baja != null)
                                                    {
                                                        var existeBajaEnSigoplan = relacionDepMaqPol.Any(x => x.Poliza.Año == baja.Year && x.Poliza.Mes == baja.Mes && x.Poliza.Poliza == baja.Poliza && x.Poliza.TP == baja.TP && x.Poliza.Linea == baja.Linea && x.Poliza.Estatus && x.Estatus);
                                                        if (existeBajaEnSigoplan)
                                                        {
                                                            baja = null;
                                                        }
                                                    }
                                                }
                                                //

                                                if (item.FechaInicioDepreciacion > fechaHasta)
                                                {
                                                    if (añoActual == registroCalculadoDTO.Fecha.Year && registroCalculadoDTO.FechaBaja == null && registroCalculadoDTO.FechaCancelacion == null)
                                                    {
                                                        registroCalculadoDTO.ValorEnLibros = (registroCalculadoDTO.MOI + registroCalculadoDTO.Altas + registroCalculadoDTO.Overhaul) - registroCalculadoDTO.DepreciacionContableAcumulada - registroCalculadoDTO.MontoBaja;
                                                    }

                                                    if (registroCalculadoDTO.FechaBaja != null && añosAnteriores.Contains(registroCalculadoDTO.FechaBaja.Value.Year))
                                                    {

                                                    }
                                                    else
                                                    {
                                                        if (registroCalculadoDTO.FechaBaja != null)
                                                        {
                                                            registroCalculadoDTO.ValorEnLibros = 0.0M;
                                                        }
                                                        else
                                                        {
                                                            registroCalculadoDTO.ValorEnLibros = (registroCalculadoDTO.MOI + registroCalculadoDTO.Altas + registroCalculadoDTO.Overhaul) - registroCalculadoDTO.DepreciacionContableAcumulada - registroCalculadoDTO.MontoBaja;
                                                        }
                                                    }

                                                    registrosCalculadosTemp.Add(registroCalculadoDTO);
                                                    continue;
                                                }

                                                #region depContabilidad
                                                //var regCont = lista_sc_movpolDepTemp.Where
                                                //    (x =>
                                                //        x.Cc == registroCalculadoDTO.Cc &&
                                                //        ((añosAnteriores.Contains(x.Year)) || (añoActual == x.Year && x.Mes <= mesActual)) &&
                                                //        relacionSubcuentas.Where(r => r.EsCuentaDepreciacion && r.Cuenta.Cuenta == cuenta && r.Subcuenta == x.Scta && r.SubSubcuenta == x.Sscta && r.Año == x.Year).Count() > 0
                                                //    ).ToList();

                                                //registroCalculadoDTO.DepreciacionContableAcumuladaContabilidad = regCont.Sum(s => s.Monto) * -1;

                                                //foreach (var rCont in regCont)
                                                //{
                                                //    lista_sc_movpolDepTemp.Remove(rCont);
                                                //}
                                                #endregion

                                                #region SaldoContabilidad
                                                //var regSalCont = lista_sc_salcontTemp.Where
                                                //    (x =>
                                                //        x.Cta == cuenta &&
                                                //        x.Cc == registroCalculadoDTO.Cc &&
                                                //        x.Year <= añoActual &&
                                                //        relacionSubcuentas.Where(r => !r.EsCuentaDepreciacion && r.Cuenta.Cuenta == cuenta && r.Subcuenta == x.Scta && r.SubSubcuenta == x.Sscta && (añosAnteriores.Contains(r.Año) || r.Año == añoActual)).Count() > 0
                                                //    ).ToList();

                                                //var sumSalCont = 0.0M;

                                                //foreach (var añoAnterior in añosAnteriores)
                                                //{
                                                //    sumSalCont += regSalCont.Where(x => x.Year == añoAnterior).Sum
                                                //        (s =>
                                                //            s.Enecargos + s.Eneabonos + s.Febcargos + s.Febabonos + s.Marcargos + s.Marabonos + s.Abrcargos + s.Abrabonos +
                                                //            s.Maycargos + s.Mayabonos + s.Juncargos + s.Junabonos + s.Julcargos + s.Julabonos + s.Agocargos + s.Agoabonos +
                                                //            s.Sepcargos + s.Sepabonos + s.Octcargos + s.Octabonos + s.Novcargos + s.Novabonos + s.Diccargos + s.Dicabonos
                                                //        );
                                                //}

                                                //for (int mes = 1; mes <= mesActual; mes++)
                                                //{
                                                //    var mesCargo = Enum.GetName(typeof(MesesCargoTablaContabilidad), mes);
                                                //    var mesAbono = Enum.GetName(typeof(MesesAbonosTablaContabilidad), mes);

                                                //    sumSalCont += regSalCont.Where(x => x.Year == añoActual).Sum
                                                //        (s =>
                                                //            Convert.ToDecimal(s.GetType().GetProperty(mesCargo).GetValue(s, null)) +
                                                //            Convert.ToDecimal(s.GetType().GetProperty(mesAbono).GetValue(s, null))
                                                //        );
                                                //}

                                                //registroCalculadoDTO.MontoContabilidad = sumSalCont;

                                                //foreach (var rSalCont in regSalCont)
                                                //{
                                                //    lista_sc_salcontTemp.Remove(rSalCont);
                                                //}
                                                #endregion

                                                Calculos(baja, lista_sc_movpol, registroCalculadoDTO, cuenta, relacionSubcuentas, registrosCalculados, añosAnteriores, añoActual, mesActual);
                                            }
                                            else
                                            {
                                                //recalcular = true;

                                                //var ccNoEconomico = ccArrendadora.FirstOrDefault(f => f.areaCuenta == item.Maquina.centro_costos && item.Maquina.estatus == 1);

                                                //relacion = new sc_movpolDTO()
                                                //{
                                                //    Scta = item.Poliza.Subcuenta.Value,
                                                //    Sscta = item.Poliza.SubSubcuenta.Value,
                                                //    Concepto = item.Poliza.Concepto,
                                                //    Monto = item.Poliza.Monto.Value,
                                                //    Factura = item.Poliza.Factura,
                                                //    Year = item.Poliza.Año,
                                                //    Mes = item.Poliza.Mes,
                                                //    Poliza = item.Poliza.Poliza,
                                                //    TP = item.Poliza.TP,
                                                //    Linea = item.Poliza.Linea,
                                                //    Cc = ccNoEconomico != null && ccNoEconomico.areaCuenta != "1010" && ccNoEconomico.areaCuenta != "1015" && ccNoEconomico.areaCuenta != "1018" ? ccNoEconomico.cc : "997"
                                                //};
                                            }
                                        } while (recalcular && vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora);
                                    }

                                    foreach (var item in sc_movpolDTO_temp)
                                    {
                                        lista_sc_movpol.Remove(item);
                                    }

                                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                                    {
                                        lista_sc_movpol.RemoveAll(r => r.Year <= 2020 && r.Cta == cuenta);
                                    }

                                    #region CANCELACIONES SIN ALTAS
                                    foreach (var item in relacionDepMaqPol.Where(x => x.Poliza.TM == 3 && !x.IdPolizaReferenciaAlta.HasValue))
                                    {
                                        item.Maquina = catMaq.First(x => x.id == item.IdCatMaquina);

                                        var polizaSinAlta = lista_sc_movpol
                                            .FirstOrDefault(x =>
                                                x.Year == item.Poliza.Año &&
                                                x.Mes == item.Poliza.Mes &&
                                                x.Poliza == item.Poliza.Poliza &&
                                                x.TP == item.Poliza.TP &&
                                                x.Linea == item.Poliza.Linea);

                                        if (polizaSinAlta != null)
                                        {
                                            var acMatch = corteInventario.Where(x => x.economicoId == item.Maquina.id).Select(x => new tblP_CC
                                            {
                                                cc = x.cc,
                                                area = Convert.ToInt32(x.areaCuenta.Split('-')[0]),
                                                cuenta = Convert.ToInt32(x.areaCuenta.Split('-')[1]),
                                                areaCuenta = x.areaCuenta,
                                                descripcion = x.obra
                                            }).FirstOrDefault();

                                            if (acMatch == null)
                                            {
                                                acMatch = areasCuenta.FirstOrDefault(x => x.areaCuenta == item.Maquina.centro_costos);
                                                if (acMatch != null && (acMatch.areaCuenta == "1010" || acMatch.areaCuenta == "1015" || acMatch.areaCuenta == "1018"))
                                                {
                                                    acMatch = areasCuenta.FirstOrDefault(x => x.cc == "997");
                                                }
                                            }

                                            var detalle = new ActivoFijoDetalleDTO();
                                            detalle.FechaInicioDepreciacion = item.Poliza.FechaMovimiento;
                                            detalle.Cuenta = cuenta;
                                            detalle.Subcuenta = polizaSinAlta.Scta;
                                            detalle.SubSubcuenta = polizaSinAlta.Sscta;
                                            detalle.EsOverhaul = item.TipoDelMovimiento == (int)TipoDelMovimiento.O ? true : false;
                                            detalle.Fecha = item.Poliza.FechaMovimiento;
                                            detalle.FechaBaja = null;
                                            detalle.FechaCancelacion = item.Poliza.FechaMovimiento;
                                            detalle.Cc = polizaSinAlta.Cc;
                                            detalle.Clave = item.Maquina.noEconomico;
                                            detalle.Descripcion = polizaSinAlta.Concepto;
                                            detalle.MesesMaximoDepreciacion = item.MesesTotalesDepreciacion.HasValue ? item.MesesTotalesDepreciacion.Value : 0;
                                            detalle.PorcentajeDepreciacion = item.PorcentajeDepreciacion.HasValue ? item.PorcentajeDepreciacion.Value : 0;
                                            detalle.MontoBaja = 0m;
                                            detalle.MontoCancelacion = polizaSinAlta.Monto;
                                            detalle.Area = acMatch.area;
                                            detalle.Cuenta_OC = acMatch.cuenta;
                                            detalle.AreaCuenta = acMatch.area + "-" + acMatch.cuenta + " - " + acMatch.descripcion;
                                            detalle.faltante = true;
                                            detalle.AltaSigoplan = false;
                                            detalle.CancelacionSigoplan = true;
                                            detalle.FechaBajaPol = item.Poliza.FechaMovimiento;
                                            detalle.FechaCancelacionPol = item.Poliza.FechaMovimiento;
                                            detalle.Poliza = polizaSinAlta.Year + "-" + polizaSinAlta.Mes + "-" + polizaSinAlta.Poliza + "-" + polizaSinAlta.TP + "-" + polizaSinAlta.Linea;
                                            detalle.AñoPol_baja = polizaSinAlta.Year;
                                            detalle.MesPol_baja = polizaSinAlta.Mes;
                                            detalle.PolPol_baja = polizaSinAlta.Poliza;
                                            detalle.TpPol_baja = polizaSinAlta.TP;
                                            detalle.LineaPol_baja = polizaSinAlta.Linea;
                                            detalle.TipoMovimiento = item.Movimiento.TipoDelMovimiento;
                                            detalle.TipoActivo = item.Poliza.DescripcionTipoActivo.TipoActivo;
                                            detalle.IdTipoMovimiento = item.TipoDelMovimiento;
                                            detalle.IdCatMaquina = item.IdCatMaquina;

                                            Calculos(null, lista_sc_movpol, detalle, cuenta, relacionSubcuentas, registrosCalculados, añosAnteriores, añoActual, mesActual);

                                            lista_sc_movpol.Remove(polizaSinAlta);
                                        }
                                    }
                                    #endregion
                                }

                                registrosCalculados.AddRange(registrosCalculadosTemp);
                            }
                            #endregion Cuentas maquinas

                            if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora)
                            {
                                lista_sc_movpol.RemoveAll(x => x.Year < 2020 && x.Cta == cuenta);
                                if (cuenta == 1215)
                                {
                                    lista_sc_movpol.RemoveAll(x => x.Year == 2020 && x.Mes < 11 && x.Cta == cuenta);
                                }
                                if (cuenta == 1210 || cuenta == 1211)
                                {
                                    lista_sc_movpol.RemoveAll(x => x.Year == 2020 && x.Mes < 12 && x.Cta == cuenta);
                                }
                            }

                            #region Cuentas no maquinas
                            if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora && !cuentasMaquinaria.Contains(cuenta))
                            {
                                var noMaquinas = _context.tblC_AF_PolizaAltaBaja_NoMaquina.Where
                                    (w =>
                                        w.ctaPoliza == cuenta &&
                                        w.estatus &&
                                        w.fechaMovimiento <= fechaHasta.Date
                                    ).ToList();

                                foreach (var infoMov in noMaquinas.Where(w => w.tmPoliza == (int)AFTipoMovimientoEnum.Cargo))
                                {
                                    var registro = new ActivoFijoDetalleDTO();

                                    if (infoMov.polizasAjuste.Count > 0)
                                    {
                                        foreach (var infoAjuste in infoMov.polizasAjuste.GroupBy(g => g.tipoPolizaDeAjusteId))
                                        {
                                            switch (infoAjuste.Key)
                                            {
                                                case (int)AFTipoPolizaAjusteEnum.AjusteDeMonto:
                                                    infoMov.montoPoliza = infoMov.montoPoliza + infoAjuste.Sum(s => s.montoPoliza);
                                                    break;
                                                case (int)AFTipoPolizaAjusteEnum.VariasPolizasUnMovimiento:
                                                    //Por el momento mismo comportamiento que AjusteDeMonto
                                                    infoMov.montoPoliza = infoMov.montoPoliza + infoAjuste.Sum(s => s.montoPoliza);
                                                    break;
                                                case (int)AFTipoPolizaAjusteEnum.UnaPolizaVariosMovimientos:
                                                    //Por el momento solo informativo...
                                                    break;
                                                case (int)AFTipoPolizaAjusteEnum.MontoDiferenteEnPoliza:
                                                    //Por el momento solo informativo...
                                                    break;
                                            }
                                        }

                                        if (infoMov.montoPoliza == 0)
                                        {
                                            continue;
                                        }
                                    }

                                    registro.Fecha = infoMov.fechaMovimiento;
                                    registro.Cuenta = cuenta;
                                    registro.Subcuenta = infoMov.sctaPoliza;
                                    registro.SubSubcuenta = infoMov.ssctaPoliza;
                                    registro.Cc = infoMov.cc;
                                    registro.Clave = infoMov.ccDescripcion;
                                    registro.Descripcion = infoMov.conceptoPoliza;
                                    registro.MesesMaximoDepreciacion = infoMov.mesesDepreciacion.Value;
                                    registro.MOI = añosAnteriores.Contains(registro.Fecha.Year) ? infoMov.montoPoliza : 0M;
                                    registro.Altas = registro.Fecha.Year == añoActual ? infoMov.montoPoliza : 0M;
                                    registro.Overhaul = 0M;
                                    registro.EsOverhaul = false;
                                    registro.PorcentajeDepreciacion = infoMov.porcentajeDepreciacion.Value;
                                    registro.FechaInicioDepreciacion = infoMov.inicioDepreciacion.Value;
                                    registro.Factura = infoMov.factura;
                                    registro.Poliza = infoMov.yearPoliza + "-" + infoMov.mesPoliza + "-" + infoMov.tpPoliza + "-" + infoMov.lineaPoliza;
                                    registro.Area = null;
                                    registro.Cuenta_OC = null;
                                    registro.AreaCuenta = null;
                                    registro.AreaCuentaDescripcion = null;
                                    registro.AñoPol_alta = infoMov.yearPoliza;
                                    registro.MesPol_alta = infoMov.mesPoliza;
                                    registro.PolPol_alta = infoMov.polizaPoliza;
                                    registro.TpPol_alta = infoMov.tpPoliza;
                                    registro.LineaPol_alta = infoMov.lineaPoliza;
                                    registro.TipoMovimiento = Enum.GetName(typeof(TipoDelMovimiento), infoMov.tipoMovimientoId.Value);
                                    registro.TipoActivo = "Maquina";
                                    registro.IdTipoMovimiento = infoMov.tipoMovimientoId.Value;
                                    registro.AltaSigoplan = true;
                                    registro.faltante = false;

                                    sc_movpolDTO baja = null;

                                    if (infoMov.polizaBajaId != null)
                                    {
                                        var registroBaja = noMaquinas.FirstOrDefault(f => f.id == infoMov.polizaBajaId);

                                        if (registroBaja != null)
                                        {
                                            switch (registroBaja.tmPoliza)
                                            {
                                                case (int)AFTipoMovimientoEnum.Abono:
                                                    registro.BajaSigoplan = true;
                                                    break;
                                                case (int)AFTipoMovimientoEnum.CargoRojo:
                                                    registro.CancelacionSigoplan = true;
                                                    break;
                                            }

                                            baja = new sc_movpolDTO();

                                            baja.TM = registroBaja.tmPoliza;
                                            baja.Scta = registroBaja.sctaPoliza;
                                            baja.Sscta = registroBaja.ssctaPoliza;
                                            baja.Concepto = registroBaja.conceptoPoliza;
                                            baja.Monto = registroBaja.montoPoliza;
                                            baja.Factura = registroBaja.factura;
                                            baja.Year = registroBaja.yearPoliza;
                                            baja.Mes = registroBaja.mesPoliza;
                                            baja.Poliza = registroBaja.polizaPoliza;
                                            baja.TP = registroBaja.tpPoliza;
                                            baja.Linea = registroBaja.lineaPoliza;
                                            baja.Cc = registroBaja.cc;
                                            baja.FechaPol = registroBaja.fechaMovimiento;
                                        }
                                    }

                                    Calculos(baja, lista_sc_movpol, registro, cuenta, relacionSubcuentas, registrosCalculados, añosAnteriores, añoActual, mesActual);
                                }
                            }
                            else
                            {
                                List<ActivoFijoDetalleDTO> registros = lista_sc_movpol.Where
                                (x =>
                                    x.Cta == cuenta &&
                                    x.TM == 1 &&
                                    relacionSubcuentas.Where(r => !r.EsCuentaDepreciacion && r.Cuenta.Cuenta == cuenta && r.Subcuenta == x.Scta && r.SubSubcuenta == x.Sscta /*&& (añosAnteriores.Contains(r.Año) || r.Año == añoActual)*/).Count() > 0 &&
                                    relacionSubcuentas.Where(r => !r.EsCuentaDepreciacion && r.Cuenta.Cuenta == cuenta && r.Excluir && r.Subcuenta == x.Scta && r.SubSubcuenta == x.Sscta && r.Año == x.Year).Count() == 0
                                ).Select((m, consecutivo) => new ActivoFijoDetalleDTO
                                {
                                    FechaInicioDepreciacion = m.FechaCFD != null && m.FechaCFD.Value.Year >= 2018 ? m.FechaCFD.Value : m.FechaFactura != null ? m.FechaFactura.Value : m.FechaPol,
                                    //Fecha = m.FechaCFD != null && m.FechaCFD.Value.Year >= 2018 ? m.FechaCFD.Value.ToShortDateString() : m.FechaFactura != null ? m.FechaFactura.Value.ToShortDateString() : m.FechaPol.ToShortDateString(),
                                    Fecha = m.FechaPol,
                                    Cuenta = cuenta,
                                    Subcuenta = m.Scta,
                                    SubSubcuenta = m.Sscta,
                                    Cc = m.Cc,
                                    Clave = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? m.AreaCuenta : m.Referencia,
                                    Descripcion = m.Concepto,
                                    MesesMaximoDepreciacion = cuenta != 1210 && cuenta != 1211 && cuenta != 1215 ? mesesDeDepreciacion : relacionSubcuentas.FirstOrDefault(r => !r.EsCuentaDepreciacion && r.Cuenta.Cuenta == m.Cta && r.Subcuenta == m.Scta && r.SubSubcuenta == m.Sscta /*&& r.Año == m.Year*/).MesesMaximoDepreciacion,
                                    MOI = añosAnteriores.Contains(m.Year) ? m.Monto : 0.0M,
                                    Altas = m.Year == añoActual &&
                                            relacionSubcuentas.Where
                                            (r =>
                                                !r.EsCuentaDepreciacion &&
                                                !r.EsOverhaul && r.Subcuenta == m.Scta &&
                                                r.SubSubcuenta == m.Sscta &&
                                                (añosAnteriores.Contains(r.Año) || r.Año == añoActual)).Count() != 0 ? m.Monto : 0.0M,
                                    Overhaul = m.Year == añoActual &&
                                               relacionSubcuentas.Where
                                               (r =>
                                                   !r.EsCuentaDepreciacion &&
                                                   r.EsOverhaul && r.Subcuenta == m.Scta &&
                                                   r.SubSubcuenta == m.Sscta &&
                                                   (añosAnteriores.Contains(r.Año) || r.Año == añoActual)).Count() > 0 ? m.Monto : 0.0M,
                                    EsOverhaul = relacionSubcuentas.Where
                                                 (r =>
                                                    !r.EsCuentaDepreciacion &&
                                                    r.EsOverhaul && r.Subcuenta == m.Scta &&
                                                    r.SubSubcuenta == m.Sscta &&
                                                    (añosAnteriores.Contains(r.Año) || r.Año == añoActual)).Count() > 0 ? true : false,
                                    PorcentajeDepreciacion = cuenta != 1210 && cuenta != 1211 && cuenta != 1215 ? porcentajeDepreciacion : relacionSubcuentas.FirstOrDefault(r => !r.EsCuentaDepreciacion /*&& r.Año == m.Year*/ && r.Subcuenta == m.Scta && r.Cuenta.Cuenta == m.Cta).PorcentajeDepreciacion,
                                    Area = m.Area,
                                    Cuenta_OC = m.Cuenta_OC,
                                    AreaCuenta = m.Area != null && m.Cuenta_OC != null ? m.Area + "-" + m.Cuenta_OC + " - " + m.AreaCuentaDescripcion : "",
                                    AreaCuentaDescripcion = m.AreaCuentaDescripcion,
                                    Factura = !string.IsNullOrEmpty(m.Factura) ? m.Factura : "",
                                    Poliza = m.Year + "-" + m.Mes + "-" + m.Poliza + "-" + m.TP + "-" + m.Linea,
                                    AñoPol_alta = m.Year,
                                    MesPol_alta = m.Mes,
                                    PolPol_alta = m.Poliza,
                                    TpPol_alta = m.TP,
                                    LineaPol_alta = m.Linea
                                }).ToList();

                                foreach (ActivoFijoDetalleDTO registro in registros)
                                {
                                    //foreach (var insmOverhaul in insumosOverhaul)
                                    //{
                                    //    if (registro.Descripcion.Contains(insmOverhaul.Descripcion))
                                    //    {
                                    //        registro.MesesMaximoDepreciacion = insmOverhaul.Meses;
                                    //        registro.PorcentajeDepreciacion = insmOverhaul.Porcentaje != null ? insmOverhaul.Porcentaje.Value : registro.PorcentajeDepreciacion;
                                    //        break;
                                    //    }
                                    //}

                                    if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora && cuentasMaquinaria.Contains(cuenta))
                                    {
                                        var noEconomicoMaq = catMaq.FirstOrDefault(f => f.noEconomico.Contains(registro.Clave));

                                        if (noEconomicoMaq == null)
                                        {
                                            continue;
                                        }
                                    }

                                    registro.TipoMovimiento = registro.EsOverhaul ? "O" : "A";
                                    registro.IdTipoMovimiento = registro.EsOverhaul ? 5 : 1;
                                    registro.TipoActivo = registro.EsOverhaul ? "Componente" : "Maquina";
                                    //EXCEPCIONES
                                    if (cuenta == 1211 && vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                                    {
                                        if (registro.Poliza == "2018-5-3-03-176" || registro.Poliza == "2018-5-3-03-340" || registro.Poliza == "2018-5-3-03-316")
                                        {
                                            registro.FechaInicioDepreciacion = new DateTime(2020, 5, 1);
                                            registro.IdTipoMovimiento = 2;
                                            registro.TipoMovimiento = "B";
                                        }
                                        if (registro.Poliza == "2019-8-999-07-3")
                                        {
                                            registro.FechaInicioDepreciacion = new DateTime(2019, 6, 18);
                                        }
                                    }
                                    //EXCEPCIONES FIN

                                    //var acMatch = areasCuenta.FirstOrDefault(x => x.areaCuenta == item.Maquina.centro_costos);
                                    var catMqMatch = catMaq.FirstOrDefault(x => x.noEconomico == registro.Clave && x.estatus == 1);

                                    tblP_CC acMatch = null;
                                    if (catMqMatch != null)
                                    {
                                        registro.IdCatMaquina = catMqMatch.id;
                                        //acMatch = areasCuenta.FirstOrDefault(x => x.areaCuenta == catMqMatch.centro_costos);
                                        acMatch = corteInventario.Where(x => x.economicoId == catMqMatch.id).Select(x => new tblP_CC
                                        {
                                            cc = x.cc.Trim(),
                                            areaCuenta = x.areaCuenta,
                                            descripcion = x.obra,
                                            area = Convert.ToInt32(x.areaCuenta.Split('-')[0]),
                                            cuenta = Convert.ToInt32(x.areaCuenta.Split('-')[1])
                                        }).FirstOrDefault();

                                        if (acMatch == null)
                                        {
                                            acMatch = areasCuenta.FirstOrDefault(x => x.areaCuenta == catMqMatch.centro_costos);
                                        }

                                        if (acMatch != null)
                                        {
                                            var acCambio = _cambioAC.FirstOrDefault(x => x.acAnterior == acMatch.areaCuenta && x.maquinaId == catMqMatch.id);
                                            if (acCambio != null)
                                            {
                                                acMatch = areasCuenta.FirstOrDefault(x => x.areaCuenta == acCambio.acNuevo);
                                            }
                                        }

                                        if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora)
                                        {
                                            registro.Cc = acMatch != null && acMatch.areaCuenta != "1010" && acMatch.areaCuenta != "1015" && acMatch.areaCuenta != "1018" && acMatch.areaCuenta != "14-1" ? acMatch.cc : "997";
                                        }
                                    }
                                    if (cuenta == 1220 || cuenta == 1225)
                                    {
                                        acMatch = areasCuenta.FirstOrDefault(x => x.area == 9 && x.cuenta == 1);
                                    }
                                    if (cuenta == 1230 && vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                                    {
                                        acMatch = areasCuenta.FirstOrDefault(x => x.area == registro.Area && x.cuenta == registro.Cuenta_OC);
                                    }
                                    if (cuenta == 1230 && vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora)
                                    {
                                        acMatch = areasCuenta.FirstOrDefault(x => x.cc == registro.Cc);
                                    }

                                    registro.Area = acMatch != null && acMatch.areaCuenta != "1010" && acMatch.areaCuenta != "1015" && acMatch.areaCuenta != "1018" ? acMatch.area : 14;
                                    registro.Cuenta_OC = acMatch != null && acMatch.areaCuenta != "1010" && acMatch.areaCuenta != "1015" && acMatch.areaCuenta != "1018" ? acMatch.cuenta : 1;
                                    registro.AreaCuenta = acMatch != null && acMatch.areaCuenta != "1010" && acMatch.areaCuenta != "1015" && acMatch.areaCuenta != "1018" ? acMatch.areaCuenta + " - " + acMatch.descripcion : registro.Area + "-" + registro.Cuenta_OC + " - " + "MAQUINARIA NO ASIGNADA A OBRA";
                                    registro.AreaCuentaDescripcion = acMatch != null && acMatch.areaCuenta != "1010" && acMatch.areaCuenta != "1015" && acMatch.areaCuenta != "1018" ? acMatch.descripcion : "MAQUINARIA NO ASIGNADA A OBRA";

                                    sc_movpolDTO baja = encontrarBaja(cuenta, registro, lista_sc_movpol, relacionSubcuentas, fechaHasta);

                                    //Excepciones
                                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora && baja != null && añoActual >= 2021 && mesActual >= 2 && cuenta == 1220)
                                    {
                                        if (baja.Year == 2021 && baja.Mes == 2 && baja.Poliza == 49 && baja.TP == "03" && baja.Linea == 3)
                                        {//-719.00
                                            lista_sc_movpol.Remove(baja);
                                            baja = null;
                                            continue;
                                        }
                                        if (baja.Year == 2021 && baja.Mes == 2 && baja.Poliza == 49 && baja.TP == "03" && baja.Linea == 1)
                                        {//-2.00
                                            lista_sc_movpol.Remove(baja);
                                            baja = null;
                                            continue;
                                        }
                                        if (baja.Year == 2021 && baja.Mes == 2 && baja.Poliza == 49 && baja.TP == "03" && baja.Linea == 2)
                                        {//-359.50
                                            lista_sc_movpol.Remove(baja);
                                            baja = null;
                                            continue;
                                        }
                                        if (baja.Year == 2021 && baja.Mes == 2 && baja.Poliza == 49 && baja.TP == "03" && baja.Linea == 4)
                                        {//-1.00
                                            lista_sc_movpol.Remove(baja);
                                            baja = null;
                                            continue;
                                        }
                                        if (baja.Year == 2021 && baja.Mes == 2 && baja.Poliza == 49 && baja.TP == "03" && baja.Linea == 5)
                                        {//-1.00
                                            lista_sc_movpol.Remove(baja);
                                            baja = null;
                                            continue;
                                        }
                                    }

                                    Calculos(baja, lista_sc_movpol, registro, cuenta, relacionSubcuentas, registrosCalculados, añosAnteriores, añoActual, mesActual);
                                }

                                List<ActivoFijoDetalleDTO> faltantes = lista_sc_movpol.Where
                                (x =>
                                    x.Cta == cuenta &&
                                    x.TM != 1 &&
                                    relacionSubcuentas.Where
                                        (r =>
                                            !r.EsCuentaDepreciacion &&
                                            r.Cuenta.Cuenta == cuenta &&
                                            r.Subcuenta == x.Scta &&
                                            r.SubSubcuenta == x.Sscta &&
                                            (añosAnteriores.Contains(r.Año) || r.Año == añoActual)
                                        ).Count() > 0 &&
                                    relacionSubcuentas.Where
                                        (r =>
                                            !r.EsCuentaDepreciacion &&
                                            r.Cuenta.Cuenta == cuenta &&
                                            r.Excluir &&
                                            r.Subcuenta == x.Scta &&
                                            r.SubSubcuenta == x.Sscta &&
                                            r.Año == x.Year
                                        ).Count() == 0
                                ).Select((m, consecutivo) => new ActivoFijoDetalleDTO
                                {
                                    FechaInicioDepreciacion = m.FechaCFD != null && m.FechaCFD.Value.Year >= 2018 ? m.FechaCFD.Value : m.FechaFactura != null ? m.FechaFactura.Value : m.FechaPol,
                                    Cuenta = cuenta,
                                    Subcuenta = m.Scta,
                                    SubSubcuenta = m.Sscta,
                                    EsOverhaul = relacionSubcuentas.Where
                                                     (r =>
                                                        !r.EsCuentaDepreciacion &&
                                                        r.EsOverhaul && r.Subcuenta == m.Scta &&
                                                        r.SubSubcuenta == m.Sscta &&
                                                        (añosAnteriores.Contains(r.Año) || r.Año == añoActual)).Count() > 0 ? true : false,
                                    Fecha = m.FechaCFD != null && m.FechaCFD.Value.Year >= 2018 ? m.FechaCFD.Value : m.FechaFactura != null ? m.FechaFactura.Value : m.FechaPol,
                                    FechaBaja = m.TM != 3 ? m.FechaCFD != null && m.FechaCFD.Value.Year >= 2018 ? m.FechaCFD : m.FechaFactura != null ? m.FechaFactura : m.FechaPol : null,
                                    FechaCancelacion = m.TM == 3 ? m.FechaCFD != null && m.FechaCFD.Value.Year >= 2018 ? m.FechaCFD : m.FechaFactura != null ? m.FechaFactura : m.FechaPol : null,
                                    Cc = m.Cc,
                                    Clave = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? m.AreaCuenta : m.Referencia,
                                    Descripcion = m.Concepto,
                                    MesesMaximoDepreciacion = cuenta != 1210 && cuenta != 1211 && cuenta != 1215 ? mesesDeDepreciacion : relacionSubcuentas.FirstOrDefault(r => !r.EsCuentaDepreciacion && r.Cuenta.Cuenta == m.Cta && r.Subcuenta == m.Scta && r.SubSubcuenta == m.Sscta).MesesMaximoDepreciacion,
                                    PorcentajeDepreciacion = cuenta != 1210 && cuenta != 1211 && cuenta != 1215 ? porcentajeDepreciacion : relacionSubcuentas.FirstOrDefault(r => !r.EsCuentaDepreciacion && r.Subcuenta == m.Scta && r.Cuenta.Cuenta == m.Cta).PorcentajeDepreciacion,
                                    MontoBaja = m.TM != 3 ? m.Monto : 0.0M,
                                    MontoCancelacion = m.TM == 3 ? m.Monto : 0.0M,
                                    Area = m.Area,
                                    Cuenta_OC = m.Cuenta_OC,
                                    AreaCuenta = m.Area != null && m.Cuenta_OC != null ? m.Area + "-" + m.Cuenta_OC + " - " + m.AreaCuentaDescripcion : "",
                                    faltante = true,
                                    FechaBajaPol = m.FechaPol,
                                    FechaCancelacionPol = m.FechaPol,
                                    Poliza = m.Year + "-" + m.Mes + "-" + m.Poliza + "-" + m.TP + "-" + m.Linea,
                                    AñoPol_baja = m.Year,
                                    MesPol_baja = m.Mes,
                                    PolPol_baja = m.Poliza,
                                    TpPol_baja = m.TP,
                                    LineaPol_baja = m.Linea
                                }).ToList();

                                foreach (var faltante in faltantes)
                                {
                                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora && cuenta == 1210)
                                    {
                                        if (faltante.Poliza == "2021-8-214-03-1")
                                        {
                                            faltante.PorcentajeDepreciacion = 0;
                                        }

                                        if (faltante.Poliza == "2022-3-79-05-15")
                                        {
                                            faltante.PorcentajeDepreciacion = .6666M;
                                            faltante.MesesMaximoDepreciacion = 18;
                                        }
                                    }
                                    //if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                                    //{
                                    //    if (faltante.Poliza == "2020-9-34-05-3")
                                    //    {
                                    //        faltante.PorcentajeDepreciacion = 0;
                                    //        faltante.MesesMaximoDepreciacion = 0;
                                    //    }
                                    //    if (faltante.Poliza == "2021-2-102-05-35")
                                    //    {
                                    //        faltante.PorcentajeDepreciacion = 0;
                                    //        faltante.MesesMaximoDepreciacion = 0;
                                    //    }
                                    //    if (faltante.Poliza == "2021-2-102-05-39")
                                    //    {
                                    //        faltante.PorcentajeDepreciacion = 0;
                                    //        faltante.MesesMaximoDepreciacion = 0;
                                    //    }
                                    //    if (faltante.Poliza == "2021-2-102-05-40")
                                    //    {
                                    //        faltante.PorcentajeDepreciacion = 0;
                                    //        faltante.MesesMaximoDepreciacion = 0;
                                    //    }
                                    //    if (faltante.Poliza == "2021-2-233-03-7")
                                    //    {
                                    //        faltante.PorcentajeDepreciacion = 0;
                                    //        faltante.MesesMaximoDepreciacion = 0;
                                    //    }
                                    //    if (faltante.Poliza == "2021-2-66-05-125")
                                    //    {
                                    //        faltante.PorcentajeDepreciacion = 0;
                                    //        faltante.MesesMaximoDepreciacion = 0;
                                    //    }
                                    //    if (faltante.Poliza == "2021-2-66-05-126")
                                    //    {
                                    //        faltante.PorcentajeDepreciacion = 0;
                                    //        faltante.MesesMaximoDepreciacion = 0;
                                    //    }
                                    //}
                                    //foreach (var insmOverhaul in insumosOverhaul)
                                    //{
                                    //    if (faltante.Descripcion.Contains(insmOverhaul.Descripcion))
                                    //    {
                                    //        faltante.MesesMaximoDepreciacion = insmOverhaul.Meses;
                                    //        faltante.PorcentajeDepreciacion = insmOverhaul.Porcentaje != null ? insmOverhaul.Porcentaje.Value : faltante.PorcentajeDepreciacion;
                                    //        break;
                                    //    }
                                    //}

                                    if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora && cuentasMaquinaria.Contains(cuenta))
                                    {
                                        var noEconomicoMaq = catMaq.FirstOrDefault(f => f.noEconomico.Contains(faltante.Clave));

                                        if (noEconomicoMaq == null)
                                        {
                                            continue;
                                        }
                                    }

                                    faltante.TipoMovimiento = faltante.EsOverhaul ? "O" : "A";
                                    faltante.TipoActivo = faltante.EsOverhaul ? "Componente" : "Maquina";
                                    faltante.IdTipoMovimiento = faltante.EsOverhaul ? 5 : 1;

                                    var catMqMatch = catMaq.FirstOrDefault(x => x.noEconomico == faltante.Clave && x.estatus == 1);

                                    tblP_CC acMatch = null;
                                    if (catMqMatch != null)
                                    {
                                        faltante.IdCatMaquina = catMqMatch.id;

                                        acMatch = corteInventario.Where(x => x.economicoId == catMqMatch.id).Select(x => new tblP_CC
                                        {
                                            cc = x.cc.Trim(),
                                            areaCuenta = x.areaCuenta,
                                            descripcion = x.obra,
                                            area = Convert.ToInt32(x.areaCuenta.Split('-')[0]),
                                            cuenta = Convert.ToInt32(x.areaCuenta.Split('-')[1])
                                        }).FirstOrDefault();

                                        if (acMatch == null)
                                        {
                                            acMatch = areasCuenta.FirstOrDefault(x => x.areaCuenta == catMqMatch.centro_costos);
                                        }

                                        if (acMatch != null)
                                        {
                                            var acCambio = _cambioAC.FirstOrDefault(x => x.acAnterior == acMatch.areaCuenta && x.maquinaId == catMqMatch.id);
                                            if (acCambio != null)
                                            {
                                                acMatch = areasCuenta.FirstOrDefault(x => x.areaCuenta == acCambio.acNuevo);
                                            }
                                        }

                                        if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora)
                                        {
                                            faltante.Cc = acMatch != null && acMatch.areaCuenta != "1010" && acMatch.areaCuenta != "1015" && acMatch.areaCuenta != "1018" && acMatch.areaCuenta != "14-1" ? acMatch.cc : "997";
                                        }
                                    }

                                    faltante.Area = acMatch != null && acMatch.areaCuenta != "1010" && acMatch.areaCuenta != "1015" && acMatch.areaCuenta != "1018" ? acMatch.area : 14;
                                    faltante.Cuenta_OC = acMatch != null && acMatch.areaCuenta != "1010" && acMatch.areaCuenta != "1015" && acMatch.areaCuenta != "1018" ? acMatch.cuenta : 1;
                                    faltante.AreaCuenta = acMatch != null && acMatch.areaCuenta != "1010" && acMatch.areaCuenta != "1015" && acMatch.areaCuenta != "1018" ? acMatch.areaCuenta + " - " + acMatch.descripcion : faltante.Area + "-" + faltante.Cuenta_OC + " - " + "MAQUINARIA NO ASIGNADA A OBRA";
                                    faltante.AreaCuentaDescripcion = acMatch != null && acMatch.areaCuenta != "1010" && acMatch.areaCuenta != "1015" && acMatch.areaCuenta != "1018" ? acMatch.descripcion : "MAQUINARIA NO ASIGNADA A OBRA";

                                    Calculos(null, lista_sc_movpol, faltante, cuenta, relacionSubcuentas, registrosCalculados, añosAnteriores, añoActual, mesActual);

                                    //registrosCalculados.Add(faltante);
                                }
                            }
                            #endregion

                            //
                            var depEspecial = _context.tblC_AF_DepreciacionEspecial.Where(w => w.ctaSaldo == cuenta && w.estatus).ToList();
                            decimal montoDepEspecialDep = 0M;
                            foreach (var dp in depEspecial)
                            {
                                if (dp.montoDepreciacion.HasValue)
                                {
                                    var registroDepEspecial = new ActivoFijoDetalleDTO();
                                    registroDepEspecial.Fecha = dp.fechaInicio;
                                    registroDepEspecial.Cuenta = dp.ctaSaldo;
                                    registroDepEspecial.Subcuenta = 0;
                                    registroDepEspecial.SubSubcuenta = 0;
                                    registroDepEspecial.Cc = dp.cc;
                                    registroDepEspecial.Clave = "";
                                    registroDepEspecial.Descripcion = dp.descripcion;
                                    registroDepEspecial.MesesMaximoDepreciacion = dp.mesesDepreciacion.Value;
                                    registroDepEspecial.MOI = 0;
                                    registroDepEspecial.Altas = 0;
                                    registroDepEspecial.Overhaul = 0;
                                    registroDepEspecial.EsOverhaul = false;
                                    registroDepEspecial.PorcentajeDepreciacion = dp.porcentajeDepreciacion.Value;
                                    registroDepEspecial.FechaInicioDepreciacion = dp.fechaInicioDepreciacion.Value;
                                    registroDepEspecial.Factura = "";
                                    registroDepEspecial.Poliza = "";
                                    registroDepEspecial.Area = 14;
                                    registroDepEspecial.Cuenta_OC = 1;
                                    registroDepEspecial.AreaCuenta = "14-1 MAQUINARIA NO ASIGNADA A OBRA";
                                    registroDepEspecial.AreaCuentaDescripcion = "MAQUINARIA NO ASIGNADA A OBRA";
                                    registroDepEspecial.TipoMovimiento = Enum.GetName(typeof(TipoDelMovimiento), TipoDelMovimiento.A);
                                    registroDepEspecial.TipoMovimiento = "Maquina";
                                    registroDepEspecial.IdTipoMovimiento = 1;
                                    registroDepEspecial.AltaSigoplan = true;
                                    registroDepEspecial.faltante = false;
                                    registroDepEspecial.DepreciacionMensual = dp.montoDepreciacion.Value;
                                    registroDepEspecial.DepreciacionAcumuladaAñoAnterior = añoActual == dp.fechaInicio.Year ? 0 : dp.monto;
                                    registroDepEspecial.DepreciacionAñoActual = añoActual == dp.fechaInicio.Year ? dp.monto : 0;
                                    registroDepEspecial.DepreciacionContableAcumulada = dp.monto;
                                    registroDepEspecial.ValorEnLibros = Math.Abs(dp.monto);
                                    registroDepEspecial.esDepreciacionEspecialFija = true;

                                    Calculos(null, lista_sc_movpol, registroDepEspecial, cuenta, relacionSubcuentas, registrosCalculados, añosAnteriores, añoActual, mesActual);
                                    montoDepEspecialDep += registroDepEspecial.DepreciacionAñoActual + registroDepEspecial.DepreciacionAcumuladaAñoAnterior;
                                }
                                else
                                {
                                    var dbCalculado = new ActivoFijoDetalleDTO();
                                    dbCalculado.Fecha = dp.fechaInicio;
                                    dbCalculado.FechaInicioDepreciacion = dp.fechaInicio;
                                    dbCalculado.Cuenta = dp.ctaSaldo;
                                    dbCalculado.Subcuenta = dp.sctaSaldo;
                                    dbCalculado.SubSubcuenta = dp.ssctaSaldo;
                                    dbCalculado.Cc = dp.cc ?? "";
                                    dbCalculado.Clave = "";
                                    dbCalculado.Descripcion = dp.descripcion ?? "";
                                    dbCalculado.MesesMaximoDepreciacion = dp.mesesDepreciacion ?? 0;
                                    dbCalculado.MOI = 0;
                                    dbCalculado.Altas = 0;
                                    dbCalculado.Overhaul = 0;
                                    dbCalculado.EsOverhaul = false;
                                    dbCalculado.PorcentajeDepreciacion = dp.porcentajeDepreciacion ?? 0;
                                    dbCalculado.Factura = "";
                                    dbCalculado.Poliza = "";
                                    dbCalculado.Area = null;
                                    dbCalculado.Cuenta_OC = null;
                                    dbCalculado.AreaCuenta = "";
                                    dbCalculado.AreaCuentaDescripcion = "";
                                    dbCalculado.AltaSigoplan = true;
                                    dbCalculado.AñoPol_alta = dp.fechaInicio.Year;
                                    dbCalculado.MesPol_alta = dp.fechaInicio.Month;
                                    dbCalculado.PolPol_alta = 0;
                                    dbCalculado.TpPol_alta = "03";
                                    dbCalculado.LineaPol_alta = 0;
                                    dbCalculado.IdDepMaquina = null;
                                    dbCalculado.IdCatMaquina = null;
                                    dbCalculado.TipoMovimiento = "A";
                                    dbCalculado.TipoActivo = "Maquina";
                                    dbCalculado.IdTipoMovimiento = 1;
                                    dbCalculado.DepreciacionAcumuladaAñoAnterior = añoActual == dp.fechaInicio.Year ? 0 : dp.monto;
                                    dbCalculado.DepreciacionAñoActual = añoActual == dp.fechaInicio.Year ? dp.monto : 0;
                                    dbCalculado.DepreciacionContableAcumulada = dbCalculado.DepreciacionAcumuladaAñoAnterior + dbCalculado.DepreciacionAñoActual;
                                    dbCalculado.ValorEnLibros = Math.Abs(dp.monto);
                                    registrosCalculados.Add(dbCalculado);
                                }
                            }
                            //

                            //SALDOS
                            foreach (var reg in registrosCalculados)
                            {
                                cuentaSaldos.SaldoAnterior += añosAnteriores.Contains(reg.Fecha.Year) ? reg.MOI : 0.0M;
                                cuentaSaldos.SaldoAnterior += reg.FechaCancelacion != null && añosAnteriores.Contains(reg.FechaCancelacion.Value.Year) ? reg.MontoCancelacion : 0.0M;
                                cuentaSaldos.SaldoAnterior += reg.FechaBaja != null && (añosAnteriores.Contains(reg.FechaBaja.Value.Year)) ? reg.MontoBaja : 0.0M;

                                cuentaSaldos.Altas += añoActual == reg.Fecha.Year ? reg.Altas + reg.Overhaul + reg.MontoCancelacion : 0.0M;

                                cuentaSaldos.Bajas += reg.FechaBaja != null && añoActual == reg.FechaBaja.Value.Year ? reg.MontoBaja * -1 : 0.0M;

                                cuentaSaldos.Altas -= 
                                    reg.FechaCancelacion != null &&
                                    añoActual == reg.FechaCancelacion.Value.Year &&
                                    reg.FechaCancelacion.Value.Year != reg.Fecha.Year &&
                                    relacionSubcuentas.Where
                                        (x =>
                                            x.EsOverhaul &&
                                            !x.EsCuentaDepreciacion &&
                                            x.Cuenta.Cuenta == cuenta &&
                                            x.Subcuenta == reg.Subcuenta &&
                                            x.SubSubcuenta == reg.SubSubcuenta
                                        ).Count() > 0 &&
                                    reg.DepreciacionMensual != 0.0M &&
                                    (reg.MesesDepreciadosAñoAnterior != 0 || reg.MesesDepreciadosAñoActual != 0) ? reg.MontoCancelacion * -1 : 0.0M;

                                //cuentaSaldos.Bajas += reg.FechaCancelacion != null && añoActual == reg.FechaCancelacion.Value.Year && relacionSubcuentas.Where(x => x.EsOverhaul && x.Cuenta.Cuenta == cuenta && x.Subcuenta == reg.Subcuenta && x.SubSubcuenta == reg.SubSubcuenta).Count() > 0 ? reg.MontoCancelacion * -1 : 0.0M;

                                //DEPRECIACION
                                cuentaDepreciacion.DepreciacionAnterior += reg.DepreciacionAñoActual;
                                //cuentaDepreciacion.DepreciacionAnterior += reg.DepreciacionAcumuladaAñoAnterior;
                                cuentaDepreciacion.DepreciacionAcumulada += reg.DepreciacionContableAcumulada;
                                //DEPRECIACION FIN
                            }

                            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan && cuenta == 1215)
                            {
                                cuentaSaldos.SaldoAnterior -= 1;
                            }
                            //SALDOS FIN

                            //EXCEPCION
                            //if (cuenta == 1210 && añoActual == 2020)
                            //{
                            //    cuentaSaldos.Bajas += 546998.52M;
                            //}
                            //if (cuenta == 1210 && Math.Round(cuentaDepreciacion.DepreciacionAnterior, 2) == 310228096.79M)
                            //{
                            //    cuentaDepreciacion.DepreciacionAnterior = 310228096.83M;
                            //}
                            //if (cuenta == 1211 && Math.Round(cuentaDepreciacion.DepreciacionAnterior, 2) == 5827325.98M)
                            //{
                            //    cuentaDepreciacion.DepreciacionAnterior = 5827325.96M;
                            //}

                            //if (cuenta == 1210 && Math.Round(cuentaDepreciacion.DepreciacionAcumulada, 2) == 350735872.99M)
                            //{
                            //    cuentaDepreciacion.DepreciacionAcumulada = 350735873.04M;
                            //}
                            //if (cuenta == 1211 && Math.Round(cuentaDepreciacion.DepreciacionAcumulada, 2) == 8308330.35M)
                            //{
                            //    cuentaDepreciacion.DepreciacionAcumulada = 8308330.31M;
                            //}
                            //if (cuenta == 1215 && Math.Round(cuentaDepreciacion.DepreciacionAcumulada, 2) == 16330398.18M)
                            //{
                            //    cuentaDepreciacion.DepreciacionAcumulada = 16330398.35M;
                            //}
                            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                            {
                                if (cuenta == 1210)
                                {
                                    if (añoActual == 2019)
                                    {
                                        cuentaDepreciacion.DepreciacionAcumulada += 0.05M;
                                        cuentaDepreciacion.DepreciacionAnterior += 0.04M;
                                    }
                                    if (añoActual > 2019)
                                    {
                                        cuentaDepreciacion.DepreciacionAnterior += 0.05M;
                                        cuentaDepreciacion.DepreciacionAcumulada += 0.05M;
                                        cuentaDepreciacion.DepreciacionAcumulada += 0.04M;
                                    }
                                }
                                if (cuenta == 1211)
                                {
                                    if (añoActual == 2019)
                                    {
                                        cuentaDepreciacion.DepreciacionAcumulada -= 0.04M;
                                        cuentaDepreciacion.DepreciacionAnterior -= 0.02M;
                                    }
                                    if (añoActual > 2019)
                                    {
                                        cuentaDepreciacion.DepreciacionAnterior -= 0.04M;
                                        cuentaDepreciacion.DepreciacionAcumulada -= 0.04M;
                                        cuentaDepreciacion.DepreciacionAcumulada -= 0.02M;
                                    }
                                }
                                if (cuenta == 1215)
                                {
                                    if (añoActual == 2019)
                                    {
                                        cuentaDepreciacion.DepreciacionAcumulada += 0.17M;
                                        cuentaDepreciacion.DepreciacionAnterior += 1664.14M;
                                    }
                                    if (añoActual > 2019)
                                    {
                                        cuentaDepreciacion.DepreciacionAnterior += 0.17M;
                                        //cuentaDepreciacion.DepreciacionAcumulada += 0.17M;
                                    }
                                }
                            }
                            
                            //EXCEPCION FIN

                            var totalizadoresConstruplan = new ActivoFijoDetallesTotalizadoresDTO();

                            //SALDOS CONTABLES
                            if (verDetalle)
                            {
                                cuentaSaldos.Contabilidad = lista_sc_salcont.Where
                                    (x =>
                                        x.Cta == cuenta &&
                                        añosAnteriores.Contains(x.Year) &&
                                        relacionSubcuentas.Where(r => !r.EsCuentaDepreciacion && r.Cuenta.Cuenta == cuenta && r.Subcuenta == x.Scta && r.SubSubcuenta == x.Sscta && añosAnteriores.Contains(r.Año)).Count() > 0
                                        ).Sum(s =>
                                        s.Enecargos + s.Eneabonos + s.Febcargos + s.Febabonos + s.Marcargos + s.Marabonos + s.Abrcargos + s.Abrabonos +
                                        s.Maycargos + s.Mayabonos + s.Juncargos + s.Junabonos + s.Julcargos + s.Julabonos + s.Agocargos + s.Agoabonos +
                                        s.Sepcargos + s.Sepabonos + s.Octcargos + s.Octabonos + s.Novcargos + s.Novabonos + s.Diccargos + s.Dicabonos
                                    );
                                for (int mes = 1; mes <= mesActual; mes++)
                                {
                                    var mesCargo = Enum.GetName(typeof(MesesCargoTablaContabilidad), mes);
                                    var mesAbono = Enum.GetName(typeof(MesesAbonosTablaContabilidad), mes);

                                    var cargos = 0.0M;
                                    var abonos = 0.0M;

                                    cargos += lista_sc_salcont.Where
                                        (x =>
                                            x.Cta == cuenta &&
                                            x.Year == añoActual &&
                                            relacionSubcuentas.Where(r => !r.EsCuentaDepreciacion && r.Cuenta.Cuenta == cuenta && r.Subcuenta == x.Scta && r.SubSubcuenta == x.Sscta).Count() > 0
                                        ).Sum(s => Convert.ToDecimal(s.GetType().GetProperty(mesCargo).GetValue(s, null)));

                                    abonos += lista_sc_salcont.Where
                                        (x =>
                                            x.Cta == cuenta &&
                                            x.Year == añoActual &&
                                            relacionSubcuentas.Where(r => !r.EsCuentaDepreciacion && r.Cuenta.Cuenta == cuenta && r.Subcuenta == x.Scta && r.SubSubcuenta == x.Sscta).Count() > 0
                                        ).Sum(s => Convert.ToDecimal(s.GetType().GetProperty(mesAbono).GetValue(s, null)));

                                    var sumaMensual = cargos + abonos;

                                    cuentaSaldos.Contabilidad += sumaMensual;
                                }

                                cuentaDepreciacion.DepreciacionRegistrada = lista_sc_movpolDep.Where
                                        (x =>
                                            //relacionSubcuentas.Where(r => r.EsCuentaDepreciacion && r.Cuenta.Cuenta == cuenta && r.Subcuenta == x.Scta && r.SubSubcuenta == x.Sscta && r.Año == x.Year).Count() > 0
                                            relacionSubcuentas.Where(r => r.EsCuentaDepreciacion && r.Cuenta.Cuenta == cuenta && r.Subcuenta == x.Scta && r.SubSubcuenta == x.Sscta && (añosAnteriores.Contains(r.Año) || r.Año == añoActual)).Count() > 0
                                        ).Sum
                                            (y => (y.Monto));

                                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan /*&& !cuentasMaquinaria.Contains(cuenta)*/)
                                {
                                    var infoContabilidad = getInfoContabilidad(añoActual, mesActual, cuenta);

                                    if ((bool)infoContabilidad[SUCCESS])
                                    {
                                        var saldoInicial = (List<ActivoFijoSaldosContablesDTO>)infoContabilidad["saldos_sc_salcont_cc"];
                                        var saldoDelYear = (List<sc_movpolDTO>)infoContabilidad["saldos_sc_movpol"];
                                        var depreciacionInicial = (List<ActivoFijoSaldosContablesDTO>)infoContabilidad["depreciacion_sc_salcont_cc"];
                                        var depreciacionYear = (List<sc_movpolDTO>)infoContabilidad["depreciacion_sc_movpol"];

                                        cuentaSaldos.Contabilidad = saldoInicial.Sum(s => s.SalIni);
                                        cuentaSaldos.Contabilidad += saldoDelYear.Sum(s => s.Monto);

                                        cuentaDepreciacion.DepreciacionRegistrada = depreciacionInicial.Sum(s => s.SalIni);
                                        cuentaDepreciacion.DepreciacionRegistrada += depreciacionYear.Sum(s => s.Monto);
                                        cuentaDepreciacion.DepreciacionRegistrada *= -1;

                                        
                                        totalizadoresConstruplan.Cuenta = cuenta;
                                        totalizadoresConstruplan.Concepto = relacionSubcuentas.FirstOrDefault(x => !x.EsCuentaDepreciacion && x.Cuenta.Cuenta == cuenta).Cuenta.Descripcion;
                                        totalizadoresConstruplan.DepAcumuladaAnterior = registrosCalculados.Sum(s => s.DepreciacionAcumuladaAñoAnterior);
                                        totalizadoresConstruplan.DepAñoActual = registrosCalculados.Sum(s => s.DepreciacionAñoActual);
                                        totalizadoresConstruplan.BajaDep = registrosCalculados.Sum(s => s.BajaDepreciacion);
                                        totalizadoresConstruplan.DepContableAcumulada = registrosCalculados.Sum(s => s.DepreciacionContableAcumulada);
                                        totalizadoresConstruplan.DepValLibros = registrosCalculados.Sum(s => (s.MOI + s.Altas + s.Overhaul + s.MontoBaja + s.MontoCancelacion - s.DepreciacionContableAcumulada)) + depEspecial.Sum(s => s.monto);

                                        if ((int)EmpresaEnum.Construplan == vSesiones.sesionEmpresaActual && añoActual >= 2022 && cuenta == 1210)
                                        {
                                            //totalizadoresConstruplan.DepValLibros += depEspecial.Where(x => x.montoDepreciacion.HasValue).Sum(s => s.montoDepreciacion.Value);
                                            totalizadoresConstruplan.DepValLibros += montoDepEspecialDep;
                                        }

                                        totalizadoresConstruplan.DepAcumuladaAnteriorSalCont = depreciacionInicial.Sum(s => s.SalIni) * -1;
                                        totalizadoresConstruplan.DepAñoActualSalCont = depreciacionYear.Where(w => w.TM == (int)AFTipoMovimientoEnum.Abono || w.TM == (int)AFTipoMovimientoEnum.AbonoRojo).Sum(s => s.Monto) * -1;
                                        totalizadoresConstruplan.BajaDepSalCont = depreciacionYear.Where(w => w.TM == (int)AFTipoMovimientoEnum.Cargo || w.TM == (int)AFTipoMovimientoEnum.CargoRojo).Sum(s => s.Monto);
                                        totalizadoresConstruplan.DepContableAcumuladaSalCont = cuentaDepreciacion.DepreciacionRegistrada;
                                        totalizadoresConstruplan.DepValLibrosSalCont = cuentaSaldos.Contabilidad - cuentaDepreciacion.DepreciacionRegistrada;
                                    }
                                }

                                if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora && cuentasMaquinaria.Contains(cuenta))
                                {
                                    if (cuentasMaquinaria.Contains(cuenta))
                                    {
                                        cuentaDepreciacion.DepreciacionRegistrada = lista_sc_depCont.Where
                                    (w =>
                                        añosAnteriores.Contains(w.Year) &&
                                        relacionSubcuentas.Where
                                            (r =>
                                                r.EsCuentaDepreciacion &&
                                                !r.Excluir &&
                                                r.CuentaDepreciacion.Value == w.Cta &&
                                                r.Subcuenta == w.Scta &&
                                                r.SubSubcuenta == w.Sscta &&
                                                    //r.Año >= añoActual &&
                                                    //r.Año <= añoActual &&
                                                    //r.Año == 2020 &&
                                                r.Cuenta.Cuenta == cuenta
                                            ).Count() > 0
                                    ).Sum(s =>
                                        s.Enecargos + s.Eneabonos + s.Febcargos + s.Febabonos + s.Marcargos + s.Marabonos + s.Abrcargos + s.Abrabonos +
                                        s.Maycargos + s.Mayabonos + s.Juncargos + s.Junabonos + s.Julcargos + s.Julabonos + s.Agocargos + s.Agoabonos +
                                        s.Sepcargos + s.Sepabonos + s.Octcargos + s.Octabonos + s.Novcargos + s.Novabonos + s.Diccargos + s.Dicabonos
                                    );
                                        for (int mes = 1; mes <= mesActual; mes++)
                                        {
                                            var mesCargo = Enum.GetName(typeof(MesesCargoTablaContabilidad), mes);
                                            var mesAbono = Enum.GetName(typeof(MesesAbonosTablaContabilidad), mes);

                                            var cargos = 0.0M;
                                            var abonos = 0.0M;

                                            cargos += lista_sc_depCont.Where
                                                (w =>
                                                    w.Year == añoActual &&
                                                    relacionSubcuentas.Where
                                                    (r =>
                                                        r.EsCuentaDepreciacion &&
                                                        !r.Excluir &&
                                                        r.CuentaDepreciacion.Value == w.Cta &&
                                                        r.Subcuenta == w.Scta &&
                                                        r.SubSubcuenta == w.Sscta &&
                                                            //r.Año >= añoActual &&
                                                            //r.Año <= añoActual &&
                                                            //r.Año == 2020 &&
                                                        r.Año == añoActual &&
                                                        r.Cuenta.Cuenta == cuenta
                                                ).Count() > 0
                                                ).Sum(s => Convert.ToDecimal(s.GetType().GetProperty(mesCargo).GetValue(s, null)));

                                            abonos += lista_sc_depCont.Where
                                                (w =>
                                                    w.Year == añoActual &&
                                                    relacionSubcuentas.Where
                                                    (r =>
                                                        r.EsCuentaDepreciacion &&
                                                        !r.Excluir &&
                                                        r.CuentaDepreciacion.Value == w.Cta &&
                                                        r.Subcuenta == w.Scta &&
                                                        r.SubSubcuenta == w.Sscta &&
                                                            //r.Año >= añoActual &&
                                                            //r.Año <= añoActual &&
                                                            //r.Año == 2020 &&
                                                        r.Año == añoActual &&
                                                        r.Cuenta.Cuenta == cuenta
                                                ).Count() > 0
                                                ).Sum(s => Convert.ToDecimal(s.GetType().GetProperty(mesAbono).GetValue(s, null)));

                                            var sumaMensual = cargos + abonos;

                                            cuentaDepreciacion.DepreciacionRegistrada += sumaMensual;
                                        }
                                    }
                                }

                                cuentaDepreciacion.DepreciacionRegistrada = Math.Abs(cuentaDepreciacion.DepreciacionRegistrada);

                                //
                                //foreach (var dp in depEspecial)
                                //{
                                //    cuentaDepreciacion.DepreciacionRegistrada += dp.monto;
                                //}
                                //

                                ////SALDOS CONTABLES
                                //if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora && cuentasMaquinaria.Contains(cuenta))
                                //{
                                //    

                                    
                                //    //SALDOS CONTABLES FIN
                                //}
                            }

                            saldos.Add(cuentaSaldos);
                            depreciaciones.Add(cuentaDepreciacion);

                            activoFijoDetalleCuenta.Add(new ActivoFijoDetalleCuentaDTO() { Cuenta = cuenta, Descripcion = relacionSubcuentas.FirstOrDefault(x => !x.EsCuentaDepreciacion && x.Cuenta.Cuenta == cuenta).Cuenta.Descripcion, Detalles = registrosCalculados });
                            var afdTotalizadores = new ActivoFijoDetallesTotalizadoresDTO()
                                    {
                                        Cuenta = cuenta,
                                        Concepto = relacionSubcuentas.FirstOrDefault(x => !x.EsCuentaDepreciacion && x.Cuenta.Cuenta == cuenta).Cuenta.Descripcion,
                                        DepAcumuladaAnterior = registrosCalculados.Sum(s => s.DepreciacionAcumuladaAñoAnterior),
                                        DepAñoActual = registrosCalculados.Sum(s => s.DepreciacionAñoActual),
                                        BajaDep = registrosCalculados.Sum(s => s.BajaDepreciacion),
                                        DepContableAcumulada = registrosCalculados.Sum(s => s.DepreciacionContableAcumulada),
                                        DepValLibros = registrosCalculados.Sum(s => (s.MOI + s.Altas + s.Overhaul + s.MontoBaja + s.MontoCancelacion - s.DepreciacionContableAcumulada)),
                                        //DepValLibros = registrosCalculados.Sum(s => s.ValorEnLibros),

                                        DepAcumuladaAnteriorSalCont = lista_sc_movpolDep.Where
                                            (x =>
                                                añosAnteriores.Contains(x.Year) &&
                                                relacionSubcuentas.Where(r => r.EsCuentaDepreciacion && r.Cuenta.Cuenta == cuenta && r.Subcuenta == x.Scta && r.SubSubcuenta == x.Sscta && añosAnteriores.Contains(r.Año)).Count() > 0
                                            ).Sum(s => s.Monto) * -1,
                                        DepAñoActualSalCont = lista_sc_movpolDep.Where
                                            (x =>
                                                x.Year == añoActual &&
                                                (x.TM == 2 || x.TM == 4) &&
                                                relacionSubcuentas.Where(r => r.EsCuentaDepreciacion && r.Cuenta.Cuenta == cuenta && r.Subcuenta == x.Scta && r.SubSubcuenta == x.Sscta && r.Año == x.Year).Count() > 0
                                            ).Sum(s => s.Monto) * -1,
                                        BajaDepSalCont = lista_sc_movpolDep.Where
                                            (x =>
                                                x.Year == añoActual &&
                                                (x.TM == 1 || x.TM == 3) &&
                                                relacionSubcuentas.Where(r => r.EsCuentaDepreciacion && r.Cuenta.Cuenta == cuenta && r.Subcuenta == x.Scta && r.SubSubcuenta == x.Sscta && r.Año == x.Year).Count() > 0
                                            ).Sum(s => s.Monto),
                                        DepContableAcumuladaSalCont = lista_sc_movpolDep.Where
                                            (x =>
                                                relacionSubcuentas.Where(r => r.EsCuentaDepreciacion && r.Cuenta.Cuenta == cuenta && r.Subcuenta == x.Scta && r.SubSubcuenta == x.Sscta && r.Año == x.Year).Count() > 0
                                            ).Sum(s => (s.Monto * -1)),
                                        DepValLibrosSalCont = cuentaSaldos.Contabilidad - cuentaDepreciacion.DepreciacionRegistrada
                                    };

                            //
                            foreach (var dp in depEspecial)
                            {
                                afdTotalizadores.DepAcumuladaAnteriorSalCont += dp.monto;
                                afdTotalizadores.DepContableAcumuladaSalCont += dp.monto;
                            }
                            //

                            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan /*&& !cuentasMaquinaria.Contains(cuenta)*/)
                            {
                                afdTotalizadores = totalizadoresConstruplan;
                                //
                                foreach (var dp in depEspecial)
                                {
                                    afdTotalizadores.DepAcumuladaAnteriorSalCont += dp.monto;
                                    afdTotalizadores.DepContableAcumuladaSalCont += dp.monto;
                                }
                                //
                            }

                            //EXCEPCIONES
                            if (cuenta == 1215 && (añosAnteriores.Contains(2018) && añoActual == 2019) && vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                            {
                                afdTotalizadores.DepAcumuladaAnteriorSalCont += 1664.04M;
                                afdTotalizadores.DepAñoActual += 1664.04M;
                            }

                            if (añoActual == 2019 || añoActual == 2018 && vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                            {
                                if (Math.Abs(afdTotalizadores.DepAcumuladaAnterior - afdTotalizadores.DepAcumuladaAnteriorSalCont) < 1)
                                {
                                    afdTotalizadores.DepAcumuladaAnterior = afdTotalizadores.DepAcumuladaAnteriorSalCont;
                                }
                                if (Math.Abs(afdTotalizadores.DepAñoActual - afdTotalizadores.DepAñoActualSalCont) < 1)
                                {
                                    afdTotalizadores.DepAñoActual = afdTotalizadores.DepAñoActualSalCont;
                                }
                                if (Math.Abs(afdTotalizadores.BajaDep - afdTotalizadores.BajaDepSalCont) < 1)
                                {
                                    afdTotalizadores.BajaDep = afdTotalizadores.BajaDepSalCont;
                                }
                                if (Math.Abs(afdTotalizadores.DepContableAcumulada - afdTotalizadores.DepContableAcumuladaSalCont) < 1)
                                {
                                    afdTotalizadores.DepContableAcumulada = afdTotalizadores.DepContableAcumuladaSalCont;
                                }
                                if (Math.Abs(afdTotalizadores.DepValLibros - afdTotalizadores.DepValLibrosSalCont) < 1)
                                {
                                    afdTotalizadores.DepValLibros = afdTotalizadores.DepValLibrosSalCont;
                                }
                            }

                            if (añoActual > 2019 && vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                            {
                                if (Math.Abs(afdTotalizadores.DepAcumuladaAnterior - afdTotalizadores.DepAcumuladaAnteriorSalCont) < 1)
                                {
                                    afdTotalizadores.DepAcumuladaAnterior = afdTotalizadores.DepAcumuladaAnteriorSalCont;
                                }
                            }

                            //if (cuenta == 1215 && Math.Round(afdTotalizadores.DepAcumuladaAnteriorSalCont, 2) == 4429157.91M)
                            //{
                            //    afdTotalizadores.DepAcumuladaAnteriorSalCont = 4429157.86M;
                            //}
                            //if (cuenta == 1215 && Math.Round(afdTotalizadores.DepAcumuladaAnteriorSalCont, 2) == 4429157.91M)
                            //{
                            //    afdTotalizadores.DepAcumuladaAnteriorSalCont = 4429157.86M;
                            //}
                            //EXCEPCIONES FIN

                            detallesTotalizadores.Add(afdTotalizadores);

                            #region CALCULOS DE DIFERENCIA SALDOS / DEPRECIACION
                            //DIFERENCIAS 13/03/2020
                            var depMesCalculadoBajas = registrosCalculados.Where(x => x.FechaBaja != null).ToList();

                            var resumenMensual = new List<ActivoFijoDifDepMensualDTO>();
                            foreach (var item in registrosCalculados.Where(x => x.faltante == false && x.FechaCancelacion == null))
                            {
                                var activo = new ActivoFijoDifDepMensualDTO();
                                activo.DepMensual = item.DepreciacionMensual;
                                activo.Meses = item.MesesDepreciadosAñoAnterior + item.MesesDepreciadosAñoActual + item.MesesDepreciadosAñoAnteriorParaDiferencias;
                                activo.FechaInicio = item.FechaInicioDepreciacion;
                                activo.FechaBaja = item.FechaBaja;
                                activo.MesesOriginales = activo.Meses;
                                activo.CC = item.Cc;
                                activo.FechaBajaOriginal = activo.FechaBaja;
                                resumenMensual.Add(activo);
                            }

                            var logDepMensualCC = new List<ActivoFijoDifDepMensualCCDTO>();

                            foreach (var años in registrosCalculados.GroupBy(gb => gb.Fecha.Year).OrderBy(o => o.Key))
                            {
                                var mesTope = añosAnteriores.Contains(años.Key) ? 12 : mesActual;
                                for (int mes = 1; mes <= mesTope; mes++)
                                {
                                    var mesCargo = Enum.GetName(typeof(MesesCargoTablaContabilidad), mes);
                                    var mesAbono = Enum.GetName(typeof(MesesAbonosTablaContabilidad), mes);

                                    //DEPRECIACION NUEVA
                                    var depAUtilizar = 0.0M;
                                    var diasMes = DateTime.DaysInMonth(años.Key, mes);
                                    var fecha = new DateTime(años.Key, mes, 1);
                                    foreach (var item in resumenMensual.Where(x => x.FechaInicio < fecha))
                                    {
                                        var logDep = new ActivoFijoDifDepMensualCCDTO();

                                        if (item.Meses > 0)
                                        {
                                            logDep.CC = item.CC;
                                            logDep.DepMensual = item.DepMensual;
                                            logDep.Año = años.Key;
                                            logDep.Mes = mes;

                                            depAUtilizar += item.DepMensual;
                                            item.Meses = item.Meses - 1;
                                        }
                                        else
                                        {
                                            if (item.FechaBaja != null)
                                            {
                                                logDep.CC = item.CC;
                                                logDep.DepMensual = (item.DepMensual * item.MesesOriginales) * -1;
                                                logDep.Año = años.Key;
                                                logDep.Mes = mes;

                                                //depAUtilizar += item.DepMensual;
                                                item.Meses = item.Meses - 1;

                                                depAUtilizar -= item.DepMensual * item.MesesOriginales;
                                                item.FechaBaja = null;
                                            }
                                        }

                                        logDepMensualCC.Add(logDep);
                                    }

                                    if (añoActual == años.Key && mesActual == mes)
                                    {
                                        foreach (var item in resumenMensual.Where(x => x.FechaBaja != null && x.FechaBaja.Value.Year == añoActual && x.FechaBaja.Value.Month == mes))
                                        {
                                            depAUtilizar -= item.DepMensual * item.MesesOriginales;
                                            item.FechaBaja = null;

                                            var logDep = new ActivoFijoDifDepMensualCCDTO();
                                            logDep.CC = item.CC;
                                            logDep.DepMensual = (item.DepMensual * item.MesesOriginales) * -1;
                                            logDep.Año = años.Key;
                                            logDep.Mes = mes;
                                            logDepMensualCC.Add(logDep);
                                        }
                                    }

                                    //var depMesCalculado = años.Where(x => x.FechaInicioDepreciacion.Month == mes && x.FechaCancelacion == null).ToList();
                                    //depMensualActual = 0.0M;
                                    //foreach (var regDep in depMesCalculado)
                                    //{
                                    //    if (añosAnteriores.Contains(regDep.FechaInicioDepreciacion.Year) && regDep.FechaBaja != null && añosAnteriores.Contains(regDep.FechaBaja.Value.Year))
                                    //    {

                                    //    }
                                    //    else
                                    //    {
                                    //        depMensualActual += regDep.DepreciacionMensual;
                                    //    }
                                    //}

                                    //var depAUtilizar = depMensualAnterior;

                                    //foreach (var baja in depMesCalculadoBajas.Where(x => x.FechaBaja.Value.Year == años.Key && x.FechaBaja.Value.Month == mes))
                                    //{
                                    //    depMensualAnterior -= baja.BajaDepreciacion;
                                    //}

                                    //depMensualAnterior += depMensualActual;

                                    var depRegistrada = lista_sc_movpolDep.Where
                                        (x =>
                                            x.Year == años.Key &&
                                            x.Mes == mes &&
                                            relacionSubcuentas.Where
                                                (r =>
                                                    r.EsCuentaDepreciacion && r.Cuenta.Cuenta == cuenta &&
                                                    r.Subcuenta == x.Scta && r.SubSubcuenta == x.Sscta &&
                                                    r.Año == x.Year
                                                ).Count() > 0
                                        ).ToList();
                                    var sumaDepRegistrada = depRegistrada.Sum(s => s.Monto);

                                    if (depAUtilizar != sumaDepRegistrada)
                                    {
                                        var difDep = new ActivoFijoDiferenciasContablesDTO();
                                        var difDepDet = new List<ActivoFijoDiferenciasContablesDetallesDTO>();

                                        difDep.Cuenta = cuenta;
                                        difDep.Año = años.Key;
                                        difDep.Mes = new DateTime(2020, mes, 1).ToString("MMMM");
                                        difDep.MontoMovPol = depAUtilizar;
                                        difDep.MontoSalCont = sumaDepRegistrada * -1;
                                        difDep.Diferencia = depAUtilizar - difDep.MontoSalCont;
                                        difDep.Detalles = new List<ActivoFijoDiferenciasContablesDetallesDTO>();

                                        var ccs = new List<string>();
                                        //var resumenParaCCS = resumenMensual.Where(x => x.FechaInicio < fecha).ToList();
                                        //ccs.AddRange(resumenParaCCS.Select(m => m.CC).ToList());
                                        ccs.AddRange(logDepMensualCC.Select(m => m.CC).ToList());
                                        ccs.AddRange(depRegistrada.Select(m => m.Cc).ToList());

                                        foreach (var cc in ccs.Distinct())
                                        {
                                            var sumaDepCCCalculado = logDepMensualCC.Where(x => x.CC == cc && x.Mes == mes && x.Año == años.Key).Sum(s => s.DepMensual);
                                            var sumaDepCCSalCont = depRegistrada.Where(x => x.Cc == cc).Sum(s => s.Monto);

                                            var difDetalles = new ActivoFijoDiferenciasContablesDetallesDTO();

                                            difDetalles.CC = cc;
                                            difDetalles.MontoMovPol = sumaDepCCCalculado;
                                            difDetalles.MontoSalCont = sumaDepCCSalCont * -1;
                                            difDetalles.Diferencia = sumaDepCCCalculado - difDetalles.MontoSalCont;

                                            if (cuentasMaquinaria.Contains(cuenta))
                                            {
                                                var ccEnMovPol = registrosCalculados.FirstOrDefault(x => x.Cc == cc);
                                                difDetalles.NumEconomico = ccEnMovPol != null ? ccEnMovPol.Clave : "";
                                            }

                                            difDepDet.Add(difDetalles);
                                        }

                                        difDep.Detalles = difDepDet;
                                        diferenciasContablesDep.Add(difDep);
                                    }
                                    //DEPRECIACION NUEVA FIN

                                    //DEPRECIACION
                                    //var depMesCalculado = años.Where(x => Convert.ToDateTime(x.FechaInicioDepreciacion).Month == mes).ToList();
                                    //var sumaDepCalculado = depMesCalculado.Sum(s => s.DepreciacionContableAcumulada);

                                    //var depRegistrada = lista_sc_movpolDep.Where
                                    //    (x =>
                                    //        x.Year == años.Key &&
                                    //        x.Mes == mes &&
                                    //        relacionSubcuentas.Where
                                    //            (r =>
                                    //                r.EsCuentaDepreciacion && r.Cuenta.Cuenta == cuenta &&
                                    //                r.Subcuenta == x.Scta && r.SubSubcuenta == x.Sscta &&
                                    //                r.Año == x.Year
                                    //            ).Count() > 0
                                    //    ).ToList();
                                    //var sumaDepRegistrada = depRegistrada.Sum(s => s.Monto);

                                    //if (sumaDepCalculado != sumaDepRegistrada)
                                    //{
                                    //    var difDep = new ActivoFijoDiferenciasContablesDTO();
                                    //    var difDepDet = new List<ActivoFijoDiferenciasContablesDetallesDTO>();

                                    //    difDep.Cuenta = cuenta;
                                    //    difDep.Año = años.Key;
                                    //    //difDep.Mes = mes;
                                    //    difDep.Mes = new DateTime(2020, mes, 1).ToString("MMMM");
                                    //    difDep.MontoMovPol = sumaDepCalculado;
                                    //    difDep.MontoSalCont = sumaDepRegistrada * -1;
                                    //    difDep.Diferencia = sumaDepCalculado - difDep.MontoSalCont;
                                    //    difDep.Detalles = new List<ActivoFijoDiferenciasContablesDetallesDTO>();

                                    //    var ccs = new List<string>();

                                    //    ccs.AddRange(depMesCalculado.Select(x => x.Cc).ToList());
                                    //    ccs.AddRange(depRegistrada.Select(x => x.Cc).ToList());

                                    //    foreach (var cc in ccs.Distinct())
                                    //    {
                                    //        var sumaDepCCCalculado = depMesCalculado.Where(x => x.Cc == cc).Sum(s => s.DepreciacionContableAcumulada);

                                    //        var sumaDepCCSalCont = depRegistrada.Where
                                    //            (x => x.Cc == cc).Sum(s => s.Monto);

                                    //        if (sumaDepCCCalculado != sumaDepCCSalCont)
                                    //        {
                                    //            var difDetalles = new ActivoFijoDiferenciasContablesDetallesDTO();

                                    //            difDetalles.CC = cc;
                                    //            difDetalles.MontoMovPol = sumaDepCCCalculado;
                                    //            difDetalles.MontoSalCont = sumaDepCCSalCont;
                                    //            difDetalles.Diferencia = sumaDepCCCalculado - difDetalles.MontoSalCont;

                                    //            if (cuentasMaquinaria.Contains(cuenta))
                                    //            {
                                    //                var ccenMovPol = registrosCalculados.FirstOrDefault(x => x.Cc == cc);

                                    //                difDetalles.NumEconomico = ccenMovPol != null ? ccenMovPol.Clave : "";
                                    //            }

                                    //            difDepDet.Add(difDetalles);
                                    //        }
                                    //    }

                                    //    difDep.Detalles = difDepDet;
                                    //    diferenciasContablesDep.Add(difDep);
                                    //}
                                    //DEPRECIACION FIN

                                    //SALDOS
                                    var regMesAltas = años.Where(x => x.Fecha.Month == mes).ToList();

                                    var regMesCancelaciones = registrosCalculados.Where
                                        (x =>
                                            x.FechaCancelacion != null &&
                                            x.FechaCancelacion.Value.Year == años.Key &&
                                            x.FechaCancelacion.Value.Month == mes &&
                                            relacionSubcuentas.Where
                                                (r =>
                                                    !r.EsCuentaDepreciacion &&
                                                    r.Cuenta.Cuenta == cuenta &&
                                                    r.Subcuenta == x.Subcuenta &&
                                                    r.SubSubcuenta == x.SubSubcuenta &&
                                                    ((añosAnteriores.Contains(años.Key)) || (añoActual == años.Key))
                                                ).Count() > 0
                                        ).ToList();

                                    var regMesBajas = registrosCalculados.Where
                                        (x =>
                                            x.FechaBaja != null &&
                                            x.FechaBaja.Value.Year == años.Key &&
                                            x.FechaBaja.Value.Month == mes &&
                                            relacionSubcuentas.Where
                                                (r =>
                                                    !r.EsCuentaDepreciacion &&
                                                    r.Cuenta.Cuenta == cuenta &&
                                                    r.Subcuenta == x.Subcuenta &&
                                                    r.SubSubcuenta == x.SubSubcuenta &&
                                                    ((añosAnteriores.Contains(años.Key)) || (añoActual == años.Key))
                                                ).Count() > 0
                                        ).ToList();

                                    var sumaMesCalculado = regMesAltas.Sum(s => s.MOI + s.Altas + s.Overhaul) + regMesCancelaciones.Sum(s => s.MontoCancelacion) + regMesBajas.Sum(s => s.MontoBaja);

                                    var regMesSalCont = lista_sc_salcont.Where
                                        (x =>
                                            x.Cta == cuenta &&
                                            x.Year == años.Key &&
                                            relacionSubcuentas.Where
                                                (r =>
                                                    !r.EsCuentaDepreciacion &&
                                                    r.Cuenta.Cuenta == cuenta &&
                                                    r.Subcuenta == x.Scta &&
                                                    r.SubSubcuenta == x.Sscta &&
                                                    ((añosAnteriores.Contains(años.Key)) || (añoActual == años.Key))
                                                ).Count() > 0 &&
                                            ((Convert.ToDecimal(x.GetType().GetProperty(mesCargo).GetValue(x, null)) != 0) || (Convert.ToDecimal(x.GetType().GetProperty(mesAbono).GetValue(x, null)) != 0))
                                        ).ToList();

                                    var sumaMesSalCont = regMesSalCont.Sum(s => Convert.ToDecimal(s.GetType().GetProperty(mesCargo).GetValue(s, null)) + Convert.ToDecimal(s.GetType().GetProperty(mesAbono).GetValue(s, null)));

                                    if (sumaMesCalculado != sumaMesSalCont || sumaMesCalculado == sumaMesSalCont) //Se agregó el || para validarlo como true
                                    {
                                        var dif = new ActivoFijoDiferenciasContablesDTO();
                                        var difDet = new List<ActivoFijoDiferenciasContablesDetallesDTO>();

                                        var ccs = new List<string>();
                                        
                                        ccs.AddRange(regMesAltas.Select(x => x.Cc).ToList());
                                        ccs.AddRange(regMesCancelaciones.Select(x => x.Cc).ToList());
                                        ccs.AddRange(regMesBajas.Select(x => x.Cc).ToList());
                                        ccs.AddRange(regMesSalCont.Select(x => x.Cc).ToList());

                                        foreach (var cc in ccs.Distinct())
                                        {
                                            var sumaAltaCC = regMesAltas.Where(x => x.Cc == cc).Sum(s => s.MOI + s.Altas + s.Overhaul);
                                            var sumaCancelacionCC = regMesCancelaciones.Where(x => x.Cc == cc).Sum(s => s.MontoCancelacion);
                                            var sumaBajaCC = regMesBajas.Where(x => x.Cc == cc).Sum(s => s.MontoBaja);

                                            var sumaCCCalculado = sumaAltaCC + sumaCancelacionCC + sumaBajaCC;

                                            var sumaCCSalcont = regMesSalCont.Where
                                                (x =>
                                                    x.Cc == cc
                                                ).Sum
                                                    (s =>
                                                        Convert.ToDecimal(s.GetType().GetProperty(mesCargo).GetValue(s, null)) +
                                                        Convert.ToDecimal(s.GetType().GetProperty(mesAbono).GetValue(s, null))
                                                    );

                                            if (sumaCCCalculado != sumaCCSalcont || sumaCCCalculado == sumaCCSalcont) //Se agregó el || para validar como true
                                            {
                                                var difDetalles = new ActivoFijoDiferenciasContablesDetallesDTO();

                                                difDetalles.CC = cc;
                                                difDetalles.MontoMovPol = sumaCCCalculado;
                                                difDetalles.MontoSalCont = sumaCCSalcont;
                                                difDetalles.Diferencia = sumaCCCalculado - sumaCCSalcont;

                                                if (cuentasMaquinaria.Contains(cuenta))
                                                {
                                                    var ccEnMovPol = registrosCalculados.FirstOrDefault(x => x.Cc == cc);

                                                    difDetalles.NumEconomico = ccEnMovPol != null ? ccEnMovPol.Clave : "";
                                                }

                                                difDet.Add(difDetalles);
                                            }
                                        }

                                        dif.Cuenta = cuenta;
                                        dif.Año = años.Key;
                                        //dif.Mes = mes;
                                        dif.Mes = new DateTime(2020, mes, 1).ToString("MMMM");
                                        dif.MontoMovPol = sumaMesCalculado;
                                        dif.MontoSalCont = sumaMesSalCont;
                                        dif.Diferencia = sumaMesCalculado - sumaMesSalCont;
                                        dif.Detalles = difDet;

                                        diferenciasContablesSaldos.Add(dif);
                                    }
                                    //SALDOS FIN
                                }
                            }
                            #endregion
                        }

                        resultado.Add(SUCCESS, true);
                        resultado.Add(ITEMS, activoFijoDetalleCuenta);
                        resultado.Add("ResumenSaldos", saldos);
                        resultado.Add("ResumenDepreciacion", depreciaciones);
                        resultado.Add("DiferenciasContables", diferenciasContablesSaldos);
                        resultado.Add("DiferenciasContablesDep", diferenciasContablesDep);
                        resultado.Add("Totalizadores", detallesTotalizadores);
                        resultado.Add("FechaHasta", fechaHasta);
                    }
                    else
                    {
                        resultado = resultadoRelSubcuentas;
                    }
                }
                else
                {
                    resultado = resultadoConsulta;
                }
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Error: " + ex.ToString());
            }

            return resultado;
        }

        private Dictionary<string, object> RelacionDepMaqPolizas(int cuenta)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

            try
            {
                List<tblM_CatMaquinaDepreciacion> catMaqDep = _context.tblM_CatMaquinaDepreciacion.Where(x => x.Estatus && x.Poliza.Cuenta == cuenta).ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, catMaqDep);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Error: " + ex.ToString());
            }

            return resultado;
        }

        private Dictionary<string, object> CatMaquinaDep(string noEconomico = null)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                tblM_CatMaquina maquina = null;
                int id = 0;
                if (!string.IsNullOrEmpty(noEconomico) && vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                {
                    maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.noEconomico == noEconomico);
                    if (maquina != null)
                    {
                        id = maquina.id;
                    }
                }
                var catMaqDep = _context.tblM_CatMaquinaDepreciacion.Where(x => x.Estatus && x.Poliza.Estatus && (id != 0 ? x.IdCatMaquina == id : true)).ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, catMaqDep);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Error: " + ex.ToString());
            }

            return resultado;
        }

        private void Calculos(sc_movpolDTO baja, List<sc_movpolDTO> lista_sc_movpol, ActivoFijoDetalleDTO registro, int cuenta, List<tblC_AF_RelSubCuentas> relacionSubcuentas, List<ActivoFijoDetalleDTO> registrosCalculados, List<int> añosAnteriores, int añoActual, int mesActual)
        {
            lista_sc_movpol.Remove(baja);

            if (baja != null && baja.TM == 3 && (registro.Overhaul == 0.0M && relacionSubcuentas.Where(x => x.EsOverhaul && !x.EsCuentaDepreciacion && x.Cuenta.Cuenta == cuenta && x.Subcuenta == registro.Subcuenta && x.SubSubcuenta == registro.SubSubcuenta).Count() == 0))
            {
                registro.FechaCancelacion = baja.FechaCFD != null && baja.FechaCFD.Value.Year >= 2018 ? baja.FechaCFD : baja.FechaFactura != null ? baja.FechaFactura : baja.FechaPol;
                registro.MontoCancelacion = baja.Monto;
                registro.FechaCancelacionPol = baja.FechaPol;
                registro.AñoPol_baja = baja.Year;
                registro.MesPol_baja = baja.Mes;
                registro.PolPol_baja = baja.Poliza;
                registro.TpPol_baja = baja.TP;
                registro.LineaPol_baja = baja.Linea;

                registrosCalculados.Add(registro);

                return;
            }

            sc_movpolDTO notaCredito = lista_sc_movpol.FirstOrDefault
                (x =>
                    x.Cta == cuenta &&
                    x.TM == 3 &&
                    x.Monto < 0.0M &&
                    x.Referencia == registro.Clave &&
                    x.Concepto.Contains("NOTA") &&
                    relacionSubcuentas.Where
                        (r =>
                            !r.EsCuentaDepreciacion &&
                            r.Cuenta.Cuenta == x.Cta &&
                            r.Subcuenta == x.Scta &&
                            r.SubSubcuenta == x.Sscta
                        ).Count() > 0 &&
                    relacionSubcuentas.Where
                        (r =>
                            !r.EsCuentaDepreciacion &&
                            r.Excluir &&
                            r.Subcuenta == x.Scta &&
                            r.SubSubcuenta == x.Sscta &&
                            r.Año == x.Year
                        ).Count() == 0
                );

            lista_sc_movpol.Remove(notaCredito);

            if (notaCredito != null)
            {
                registro.MOI = registro.MOI > 0.0M ? registro.MOI + notaCredito.Monto : 0.0M;
                registro.Altas = registro.Altas > 0.0M ? registro.Altas + notaCredito.Monto : 0.0M;
                registro.Overhaul = registro.Overhaul > 0.0M ? registro.Overhaul + notaCredito.Monto : 0.0M;
            }

            if (baja != null && (baja.TM == 2 || baja.TM == 4))
            {
                registro.FechaBaja = baja.FechaCFD != null ? baja.FechaCFD : baja.FechaFactura != null ? baja.FechaFactura : baja.FechaPol;
                registro.MontoBaja = baja.Monto;
                registro.FechaBajaPol = baja.FechaPol;
                registro.AñoPol_baja = baja.Year;
                registro.MesPol_baja = baja.Mes;
                registro.PolPol_baja = baja.Poliza;
                registro.TpPol_baja = baja.TP;
                registro.LineaPol_baja = baja.Linea;

                if (añosAnteriores.Contains(registro.FechaBaja.Value.Year))
                {
                    //registro.MOI = 0.0M;
                    for (int año = registro.FechaInicioDepreciacion.Year; año <= añosAnteriores.OrderBy(x => x).Last(); año++)
                    {
                        if (año == registro.FechaInicioDepreciacion.Year)
                        {
                            registro.MesesDepreciadosAñoAnteriorParaDiferencias += registro.FechaBaja.Value.Month- registro.FechaInicioDepreciacion.Month;
                        }
                        else
                        {
                            registro.MesesDepreciadosAñoAnteriorParaDiferencias += 12;
                        }
                    }

                    /*CASO ESPECIAL CF-85*/
                    if (cuenta == 1210 && registro.Clave == "CF-85" && registro.Poliza == "2022-11-129-03-3" && registro.FechaBaja.HasValue && registro.MesesDepreciadosAñoAnteriorParaDiferencias >= 9)
                    {
                        registro.MesesDepreciadosAñoAnteriorParaDiferencias -= 1;
                    }
                    /*CASO ESPECIAL CF-85 FIN*/
                }
                else
                {
                    if (añosAnteriores.Contains(registro.FechaInicioDepreciacion.Year))
                    {
                        for (int año = registro.FechaInicioDepreciacion.Year; año <= añosAnteriores.OrderBy(x => x).Last(); año++)
                        {
                            if (año == registro.FechaInicioDepreciacion.Year)
                            {
                                registro.MesesDepreciadosAñoAnterior += 12 - registro.FechaInicioDepreciacion.Month;
                            }
                            else
                            {
                                registro.MesesDepreciadosAñoAnterior += 12;
                            }
                        }
                    }
                    registro.MesesDepreciadosAñoActual = registro.FechaInicioDepreciacion.Year == añoActual ? registro.FechaBaja.Value.Month - registro.FechaInicioDepreciacion.Month : registro.FechaBaja.Value.Month;

                    /*CASO ESPECIAL CF-85*/
                    if (cuenta == 1210 && registro.Clave == "CF-85" && registro.Poliza == "2022-11-129-03-3" && registro.FechaBaja.HasValue && registro.MesesDepreciadosAñoActual >= 8)
                    {
                        registro.MesesDepreciadosAñoActual = 7;
                    }
                    /*CASO ESPECIAL CF-85 FIN*/
                }
            }
            else
            {
                //DEP OVH
                if (baja != null && baja.TM == 3 && (registro.Overhaul != 0.0M || relacionSubcuentas.Where(x => x.EsOverhaul && !x.EsCuentaDepreciacion && x.Cuenta.Cuenta == cuenta && x.Subcuenta == registro.Subcuenta && x.SubSubcuenta == registro.SubSubcuenta).Count() > 0))
                {
                    registro.FechaCancelacion = baja.FechaCFD != null ? baja.FechaCFD : baja.FechaFactura != null ? baja.FechaFactura : baja.FechaPol;
                    registro.MontoCancelacion = baja.Monto;
                    registro.FechaCancelacionPol = baja.FechaPol;
                    registro.AñoPol_baja = baja.Year;
                    registro.MesPol_baja = baja.Mes;
                    registro.PolPol_baja = baja.Poliza;
                    registro.TpPol_baja = baja.TP;
                    registro.LineaPol_baja = baja.Linea;

                    if (añosAnteriores.Contains(registro.FechaCancelacion.Value.Year))
                    {
                        //registro.MOI = 0.0M;
                        for (int año = registro.FechaInicioDepreciacion.Year; año <= añosAnteriores.OrderBy(x => x).Last(); año++)
                        {
                            if (año == registro.FechaInicioDepreciacion.Year)
                            {
                                registro.MesesDepreciadosAñoAnteriorParaDiferencias += registro.FechaCancelacion.Value.Month - registro.FechaInicioDepreciacion.Month;
                            }
                            else
                            {
                                registro.MesesDepreciadosAñoAnteriorParaDiferencias += 12;
                            }
                        }
                    }
                    else
                    {
                        if (añosAnteriores.Contains(registro.FechaInicioDepreciacion.Year))
                        {
                            for (int año = registro.FechaInicioDepreciacion.Year; año <= añosAnteriores.OrderBy(x => x).Last(); año++)
                            {
                                if (año == registro.FechaInicioDepreciacion.Year)
                                {
                                    registro.MesesDepreciadosAñoAnterior += 12 - registro.FechaInicioDepreciacion.Month;
                                }
                                else
                                {
                                    registro.MesesDepreciadosAñoAnterior += 12;
                                }
                            }
                        }
                        registro.MesesDepreciadosAñoActual = registro.FechaInicioDepreciacion.Year == añoActual ? registro.FechaCancelacion.Value.Month - registro.FechaInicioDepreciacion.Month : registro.FechaCancelacion.Value.Month;
                    }
                }
                else
                {
                    //ya estaba
                    if (añosAnteriores.Contains(registro.FechaInicioDepreciacion.Year))
                    {
                        for (int año = registro.FechaInicioDepreciacion.Year; año <= añosAnteriores.OrderBy(x => x).Last(); año++)
                        {
                            if (año == registro.FechaInicioDepreciacion.Year)
                            {
                                registro.MesesDepreciadosAñoAnterior += 12 - registro.FechaInicioDepreciacion.Month;
                            }
                            else
                            {
                                registro.MesesDepreciadosAñoAnterior += 12;
                            }
                        }
                    }
                    registro.MesesDepreciadosAñoActual = registro.FechaInicioDepreciacion.Year == añoActual ? mesActual - registro.FechaInicioDepreciacion.Month : mesActual;
                    //registro.MesesDepreciadosAñoActual = Convert.ToDateTime(registro.FechaInicioDepreciacion).Year == añoActual ? mesActual - Convert.ToDateTime(registro.FechaInicioDepreciacion).Month : Convert.ToDateTime(registro.FechaInicioDepreciacion) >= new DateTime(añoActual, mesActual, 1) ? 0 : mesActual;
                    //ya estaba
                }
                //DEP OVH FIN

                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan && cuenta == 1230 && ((registro.Poliza == "2019-10-03-7") || (registro.Poliza == "2019-10-03-15") || (registro.Poliza == "2019-10-03-10")))
                {
                    if (new DateTime(añoActual, mesActual, 1) > new DateTime(2021, 10, 31))
                    {
                        if (new DateTime(añoActual, mesActual, 1) > new DateTime(2021, 12, 31))
                        {
                            registro.MesesDepreciadosAñoActual = 0;
                            registro.MesesDepreciadosAñoAnterior = 24;
                            añoActual = 2021;
                            mesActual = 10;
                        }
                        else
                        {
                            registro.MesesDepreciadosAñoAnterior = 14;
                            registro.MesesDepreciadosAñoActual = 10;
                            añoActual = 2021;
                            mesActual = 10;
                        }
                        registro.DepreciacionTerminadaPorMeses = true;
                    }
                }
            }

            //Excepciones
            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora && cuenta == 1225 && ((añoActual == 2021 && mesActual >= 2) || (añoActual > 2021)))
            {
                if (registro.AñoPol_alta == 2021 && registro.MesPol_alta == 2 && registro.PolPol_alta == 49 && registro.TpPol_alta == "03" && registro.LineaPol_alta == 6)
                {//2.00
                    if (añoActual > 2021)
                    {
                        registro.MesesDepreciadosAñoAnterior += 22;
                    }
                    else
                    {
                        registro.MesesDepreciadosAñoActual += 22;
                    }
                }
                if (registro.AñoPol_alta == 2021 && registro.MesPol_alta == 2 && registro.PolPol_alta == 49 && registro.TpPol_alta == "03" && registro.LineaPol_alta == 8)
                {//719.00
                    if (añoActual > 2021)
                    {
                        registro.MesesDepreciadosAñoAnterior += 14;
                    }
                    else
                    {
                        registro.MesesDepreciadosAñoActual += 14;
                    }
                }
                if (registro.AñoPol_alta == 2021 && registro.MesPol_alta == 2 && registro.PolPol_alta == 49 && registro.TpPol_alta == "03" && registro.LineaPol_alta == 7)
                {//359.50
                    if (añoActual > 2021)
                    {
                        registro.MesesDepreciadosAñoAnterior += 22;
                    }
                    else
                    {
                        registro.MesesDepreciadosAñoActual += 22;
                    }
                }
                if (registro.AñoPol_alta == 2021 && registro.MesPol_alta == 2 && registro.PolPol_alta == 49 && registro.TpPol_alta == "03" && registro.LineaPol_alta == 9)
                {//1.00
                    if (añoActual > 2021)
                    {
                        registro.MesesDepreciadosAñoAnterior += 11;
                    }
                    else
                    {
                        registro.MesesDepreciadosAñoActual += 11;
                    }
                }
                if (registro.AñoPol_alta == 2021 && registro.MesPol_alta == 2 && registro.PolPol_alta == 49 && registro.TpPol_alta == "03" && registro.LineaPol_alta == 10)
                {//1.00
                    if (añoActual > 2021)
                    {
                        registro.MesesDepreciadosAñoAnterior += 7;
                    }
                    else
                    {
                        registro.MesesDepreciadosAñoActual += 7;
                    }
                }
            }
            //

            //EXCEPCIONES
            if (!registro.faltante)
            {
                var ultimoDiaMes = DateTime.DaysInMonth(añoActual, mesActual);
                if (registro.FechaInicioDepreciacion >= new DateTime(añoActual, mesActual, ultimoDiaMes))
                {
                    if (añoActual == registro.Fecha.Year && registro.FechaBaja == null && registro.FechaCancelacion == null)
                    {
                        registro.ValorEnLibros = (registro.MOI + registro.Altas + registro.Overhaul) - registro.DepreciacionContableAcumulada - registro.MontoBaja;
                    }

                    if (registro.FechaBaja != null && añosAnteriores.Contains(registro.FechaBaja.Value.Year))
                    {

                    }
                    else
                    {
                        if (registro.FechaBaja != null)
                        {
                            registro.ValorEnLibros = 0.0M;
                        }
                        else
                        {
                            registro.ValorEnLibros = (registro.MOI + registro.Altas + registro.Overhaul) - registro.DepreciacionContableAcumulada - registro.MontoBaja;
                        }
                    }

                    registrosCalculados.Add(registro);
                    return;
                }
            }
            //EXCEPCIONES FIN
            tblC_AF_InsumosOverhaul insumoOverhaulInfoNueva = null;
            if (registro.IdTipoMovimiento == (int)TipoDelMovimiento.O && _InsumoOverhaul != null && new DateTime(registro.FechaInicioDepreciacion.Year, registro.FechaInicioDepreciacion.Month, 1) >= new DateTime(2021, 01, 01))
            {
                foreach (var insmOverhaul in _InsumoOverhaul)
                {
                    if (registro.Descripcion.Contains(insmOverhaul.Descripcion))
                    {
                        insumoOverhaulInfoNueva = insmOverhaul;

                        if (registro.Subcuenta == 7 && insumoOverhaulInfoNueva.Porcentaje.Value != 1)
                        {
                            continue;
                        }
                        else if (registro.Subcuenta == 6 && insumoOverhaulInfoNueva.Porcentaje.Value != .6666M)
                        {
                            continue;
                        }
                        else if (registro.Subcuenta == 5 && insumoOverhaulInfoNueva.Porcentaje.Value != .5M)
                        {
                            continue;
                        }

                        if (registro.Poliza == "2023-7-22-05-42")
                        {
                            continue;
                        }
                        registro.PorcentajeDepreciacion = insumoOverhaulInfoNueva.Porcentaje.Value;
                        registro.MesesMaximoDepreciacion = insumoOverhaulInfoNueva.Meses;
                        break;
                    }
                }
            }

            //
            var ListPorNuevaDep = new List<AFInfoDepDTO>();
            var nuevosMesesAnteriores = 0;
            var nuevosMesesAnterioresConNuevaDep = 0;
            var nuevosMesesActuales = 0;
            if (((_InfoCuentaDep != null && registro.IdTipoMovimiento == (int)TipoDelMovimiento.B && vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora) || (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan && ((registro.Poliza == "2019-10-03-7") || (registro.Poliza == "2019-10-03-15") || (registro.Poliza == "2019-10-03-10")))/* || (registro.IdTipoMovimiento == (int)TipoDelMovimiento.O && new DateTime(añoActual, mesActual, 1) >= new DateTime(2021, 02, 01) && insumoOverhaulInfoNueva != null)*/))
            {
                if (registro.IdTipoMovimiento == (int)TipoDelMovimiento.B)
                {
                    registro.MesesMaximoDepreciacion = 26;
                }
                var FechaComienzo = registro.IdTipoMovimiento == (int)TipoDelMovimiento.O ? new DateTime(2021, 01, 01) : _InfoCuentaDep.FechaComienzo.AddMonths(-1);
                var fechaHasta = new DateTime(añoActual, mesActual, 1);
                var mesesDiferencia = ((añoActual - FechaComienzo.Year) * 12) + (mesActual - FechaComienzo.Month);

                //
                if (registro.FechaBaja != null)
                {
                    var bbaja = new DateTime(registro.FechaBaja.Value.Year, registro.FechaBaja.Value.Month, 1);

                    if (fechaHasta >= bbaja)
                    {
                        mesesDiferencia = ((añoActual - FechaComienzo.Year) * 12) + (mesActual - FechaComienzo.Month);
                    }

                    if (bbaja > FechaComienzo)
                    {
                        mesesDiferencia = ((bbaja.Year - FechaComienzo.Year) * 12) + (bbaja.Month - FechaComienzo.Month);
                    }
                    if (añoActual > bbaja.Year)
                    {
                        mesesDiferencia = 0;
                        registro.MesesDepreciadosAñoActual = 0;
                        registro.MesesDepreciadosAñoAnterior = 0;
                    }
                }
                if (registro.FechaCancelacion != null)
                {
                    var bbaja = new DateTime(registro.FechaCancelacion.Value.Year, registro.FechaCancelacion.Value.Month, 1);

                    if (fechaHasta >= bbaja)
                    {
                        mesesDiferencia = ((añoActual - FechaComienzo.Year) * 12) + (mesActual - FechaComienzo.Month);
                    }

                    if (bbaja > FechaComienzo)
                    {
                        mesesDiferencia = ((bbaja.Year - FechaComienzo.Year) * 12) + (bbaja.Month - FechaComienzo.Month);
                    }
                    if (añoActual > bbaja.Year)
                    {
                        mesesDiferencia = 0;
                        registro.MesesDepreciadosAñoActual = 0;
                        registro.MesesDepreciadosAñoAnterior = 0;
                    }
                }
                //
                if (mesesDiferencia > 0)
                {
                    if (
                        (
                            registro.FechaBaja != null &&
                            registro.FechaBaja.Value >= (registro.IdTipoMovimiento == (int)TipoDelMovimiento.O ? new DateTime(2021, 02, 01) : _InfoCuentaDep.FechaComienzo)                            
                        ) ||
                        (
                            registro.FechaCancelacion != null &&
                            registro.FechaCancelacion.Value >= (registro.IdTipoMovimiento == (int)TipoDelMovimiento.O ? new DateTime(2021, 02, 01) : _InfoCuentaDep.FechaComienzo)
                        ) ||
                        (
                            registro.FechaBaja == null &&
                            registro.FechaCancelacion == null
                        )
                       )
                    {
                        if (registro.MesesDepreciadosAñoActual - mesesDiferencia < 0)
                        {
                            if (1==2/*mesesDiferencia > 0 && registro.MesesDepreciadosAñoActual == 0*/)
                            {
                                registro.MesesDepreciadosAñoAnterior = registro.MesesDepreciadosAñoAnterior + registro.MesesDepreciadosAñoAnteriorParaDiferencias;
                                registro.MesesDepreciadosAñoAnteriorParaDiferencias = 0;
                                registro.MesesDepreciadosAñoAnterior = registro.MesesDepreciadosAñoAnterior - mesesDiferencia;
                                nuevosMesesAnteriores = mesesDiferencia;
                            }
                            else
                            {
                                nuevosMesesAnteriores = mesesDiferencia - registro.MesesDepreciadosAñoActual;//14 - 12 = 2
                                nuevosMesesActuales = mesesDiferencia - nuevosMesesAnteriores; //14-2 = 12

                                registro.MesesDepreciadosAñoAnterior = registro.MesesDepreciadosAñoAnterior - nuevosMesesAnteriores;//11 - 2 = 9
                                //registro.MesesDepreciadosAñoActual = registro.MesesDepreciadosAñoActual - nuevosMesesAnteriores;//12 - 12 = 0
                                registro.MesesDepreciadosAñoActual = 0;                                
                            }
                        }
                        else
                        {
                            registro.MesesDepreciadosAñoActual = registro.MesesDepreciadosAñoActual - mesesDiferencia;
                            nuevosMesesAnteriores = 0;
                            nuevosMesesActuales = mesesDiferencia;
                        }
                    }
                }
            }

            //
            if (registro.esDepreciacionEspecialFija)
            {
                if (new DateTime(añoActual, mesActual, 1) > registro.FechaInicioDepreciacion)
                {
                    int _mesesDiferencia = ((registro.FechaInicioDepreciacion.Year - añoActual) * 12) + registro.FechaInicioDepreciacion.Month;
                    int _mesesAnteriores = 0;
                    int _mesesActuales = 0;

                    for (int i = 0; i < _mesesDiferencia; i++)
                    {
                        var periodoDep = registro.FechaInicioDepreciacion.AddMonths(i);
                        if (periodoDep.Year < añoActual)
                        {
                            _mesesAnteriores++;
                        }
                        else
                        {
                            _mesesActuales++;
                        }
                        if (_mesesDiferencia >= registro.MesesMaximoDepreciacion)
                        {
                            registro.MesesDepreciadosAñoAnterior = _mesesAnteriores;
                            registro.MesesDepreciadosAñoActual = _mesesActuales;
                            break;
                        }
                    }

                    registro.DepreciacionAcumuladaAñoAnterior = registro.MesesDepreciadosAñoAnterior * registro.DepreciacionMensual;
                    registro.DepreciacionAñoActual = registro.MesesDepreciadosAñoActual * registro.DepreciacionMensual;
                    registro.DepreciacionContableAcumulada = registro.DepreciacionContableAcumulada + registro.DepreciacionAcumuladaAñoAnterior + registro.DepreciacionAñoActual;
                    registro.ValorEnLibros = registro.DepreciacionContableAcumulada * -1;

                    registrosCalculados.Add(registro);
                }
                else
                {
                    registrosCalculados.Add(registro);
                }
            }
            else
            {
                var mesesAnteriores = 0;
                var mesesActuales = 0;
                for (int deps = 0; deps < (nuevosMesesAnteriores != 0 || nuevosMesesActuales != 0 ? 2 : 1); deps++)
                {
                    if (deps == 1)
                    {
                        registro.PorcentajeDepreciacion = registro.IdTipoMovimiento == (int)TipoDelMovimiento.O ? insumoOverhaulInfoNueva.Porcentaje.Value : _InfoCuentaDep.PorcentajeDepreciacion;
                        registro.MesesMaximoDepreciacion = registro.IdTipoMovimiento == (int)TipoDelMovimiento.O ? insumoOverhaulInfoNueva.Meses : _InfoCuentaDep.MesesDepreciacion;
                        registro.MesesDepreciadosAñoAnterior = nuevosMesesAnteriores;
                        registro.MesesDepreciadosAñoActual = nuevosMesesActuales;

                        //
                        if (registro.IdTipoMovimiento == (int)TipoDelMovimiento.B)
                        {
                            registro.MesesMaximoDepreciacion = 26;
                            if (registro.MesesMaximoDepreciacion < registro.MesesDepreciadosAñoAnterior + mesesAnteriores + registro.MesesDepreciadosAñoActual)
                            {
                                registro.MesesDepreciadosAñoActual = registro.MesesMaximoDepreciacion - registro.MesesDepreciadosAñoAnterior - mesesAnteriores;
                                registro.DepreciacionTerminadaPorMeses = true;
                            }
                        }
                        //
                    }
                    if (nuevosMesesAnteriores != 0 || nuevosMesesActuales != 0)
                    {
                        registro.MesesMaximoDepreciacion = /*registro.IdTipoMovimiento == (int)TipoDelMovimiento.O ? insumoOverhaulInfoNueva.Meses :*/ _InfoCuentaDep.MesesDepreciacion;
                        if (registro.IdTipoMovimiento == (int)TipoDelMovimiento.B)
                        {
                            registro.MesesMaximoDepreciacion = 26;
                        }
                    }
                    registro.DepreciacionMensual = registro.MOI > 0.0M ? (registro.MOI * registro.PorcentajeDepreciacion) / 12
                        : registro.Altas > 0.0M ? (registro.Altas * registro.PorcentajeDepreciacion) / 12
                        : (registro.Overhaul * registro.PorcentajeDepreciacion) / 12;

                    ///////////////////////OVH 1-9, 1-10
                    ActivoFijoCorteInventarioDTO obraEnMayo = null;
                    var tresCuartos = 0m;
                    if (
                        registro.EsOverhaul &&
                        new DateTime(añoActual, mesActual, DateTime.DaysInMonth(añoActual, mesActual)) >= new DateTime(2023, 5, 31) &&
                        corteInventarioMayo != null
                       )
                    {
                        if (registro.IdCatMaquina.HasValue && registro.IdCatMaquina.Value > 0)
                        {
                            obraEnMayo = corteInventarioMayo.FirstOrDefault(x => x.economicoId == registro.IdCatMaquina && (x.areaCuenta == "1-9" || x.areaCuenta == "1-10"));
                            if (obraEnMayo != null)
                            {
                                tresCuartos = (registro.DepreciacionMensual / 4);
                            }
                        }
                    }

                    // PARA FALTANTES
                    if (registro.faltante && registro.EsOverhaul)
                    {
                        registro.DepreciacionMensual = (registro.MontoCancelacion * registro.PorcentajeDepreciacion) / 12;

                        ///////////////////////OVH 1-9, 1-10
                        if (
                            tresCuartos == 0 &&
                            registro.EsOverhaul &&
                            (
                                (registro.Area == 1 && registro.Cuenta_OC == 9) ||
                                (registro.Area == 1 && registro.Cuenta_OC == 10)
                            ) &&
                            new DateTime(añoActual, mesActual, DateTime.DaysInMonth(añoActual, mesActual)) >= new DateTime(2023, 5, 31)
                           )
                        {
                            tresCuartos = (registro.DepreciacionMensual / 4);
                        }
                    }
                    //

                    if (registro.MesesMaximoDepreciacion < (registro.MesesDepreciadosAñoAnterior))
                    {
                        registro.DepreciacionAcumuladaAñoAnterior += registro.DepreciacionMensual * (registro.MesesMaximoDepreciacion - mesesAnteriores);
                        registro.MesesDepreciadosAñoAnterior = registro.MesesMaximoDepreciacion;
                        registro.MesesDepreciadosAñoActual = 0;
                        registro.DepreciacionTerminadaPorMeses = true;

                        ///////////////////////OVH 1-9, 1-10
                        if (
                            tresCuartos != 0 &&
                            registro.FechaInicioDepreciacion < new DateTime(2023, 5, 1) &&
                            registro.FechaInicioDepreciacion.AddMonths(registro.MesesMaximoDepreciacion) >= new DateTime(2023, 05, 1)
                           )
                        {
                            if (registro.MesesDepreciadosAñoActual != 0)
                            {
                                registro.DepreciacionAcumuladaAñoAnterior -= tresCuartos;

                                if (new DateTime(añoActual, mesActual, 1) >= new DateTime(2023, 06, 1))
                                {
                                    registro.DepreciacionAcumuladaAñoAnterior += tresCuartos;

                                    var fecha1 = new DateTime(registro.FechaInicioDepreciacion.Year, registro.FechaInicioDepreciacion.Month, 1);
                                    var fecha2 = fecha1.AddMonths(registro.MesesDepreciadosAñoAnterior + registro.MesesDepreciadosAñoActual + 1);
                                    var fecha3 = new DateTime(fecha2.Year, fecha2.Month, 1);
                                    var fecha4 = new DateTime(añoActual, mesActual, 1);
                                    if (fecha4 == fecha3)
                                    {
                                        registro.esOverhaul14_1Herradura = true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        registro.DepreciacionAcumuladaAñoAnterior += registro.DepreciacionMensual * registro.MesesDepreciadosAñoAnterior;

                        if (registro.MesesMaximoDepreciacion < (registro.MesesDepreciadosAñoAnterior + registro.MesesDepreciadosAñoActual))
                        {
                            //registro.DepreciacionAñoActual = registro.MesesMaximoDepreciacion - registro.MesesDepreciadosAñoAnterior;
                            registro.MesesDepreciadosAñoActual = registro.MesesMaximoDepreciacion - registro.MesesDepreciadosAñoAnterior;
                            registro.DepreciacionAñoActual = registro.DepreciacionMensual * registro.MesesDepreciadosAñoActual;
                            registro.DepreciacionTerminadaPorMeses = true;

                            ///////////////////////OVH 1-9, 1-10
                            if (
                                tresCuartos != 0 &&
                                registro.FechaInicioDepreciacion < new DateTime(2023, 5, 1) &&
                                registro.FechaInicioDepreciacion.AddMonths(registro.MesesMaximoDepreciacion) >= new DateTime(2023, 05, 1)
                               )
                            {
                                if (registro.FechaInicioDepreciacion.AddMonths(registro.MesesDepreciadosAñoAnterior) >= new DateTime(2023, 05, 01))
                                {
                                    registro.DepreciacionAcumuladaAñoAnterior -= tresCuartos;
                                        if (new DateTime(añoActual, mesActual, 1) >= new DateTime(2023, 06, 1))
                                        {
                                            registro.DepreciacionAcumuladaAñoAnterior += tresCuartos;

                                            var fecha1 = new DateTime(registro.FechaInicioDepreciacion.Year, registro.FechaInicioDepreciacion.Month, 1);
                                            var fecha2 = fecha1.AddMonths(registro.MesesDepreciadosAñoAnterior + registro.MesesDepreciadosAñoActual + 1);
                                            var fecha3 = new DateTime(fecha2.Year, fecha2.Month, 1);
                                            var fecha4 = new DateTime(añoActual, mesActual, 1);
                                            if (fecha4 == fecha3)
                                            {
                                                registro.esOverhaul14_1Herradura = true;
                                            }
                                        }
                                }
                                else if (registro.FechaInicioDepreciacion.AddMonths(registro.MesesDepreciadosAñoAnterior + registro.MesesDepreciadosAñoActual) >= new DateTime(2023, 05, 01))
                                {
                                    registro.DepreciacionAñoActual -= tresCuartos;
                                        if (new DateTime(añoActual, mesActual, 1) >= new DateTime(2023, 06, 1))
                                        {
                                            registro.DepreciacionAñoActual += tresCuartos;

                                            var fecha1 = new DateTime(registro.FechaInicioDepreciacion.Year, registro.FechaInicioDepreciacion.Month, 1);
                                            var fecha2 = fecha1.AddMonths(registro.MesesDepreciadosAñoAnterior + registro.MesesDepreciadosAñoActual + 1);
                                            var fecha3 = new DateTime(fecha2.Year, fecha2.Month, 1);
                                            var fecha4 = new DateTime(añoActual, mesActual, 1);
                                            if (fecha4 == fecha3)
                                            {
                                                registro.esOverhaul14_1Herradura = true;
                                            }
                                        }
                                }
                            }
                        }
                        else
                        {
                            registro.DepreciacionAñoActual += registro.DepreciacionMensual * registro.MesesDepreciadosAñoActual;

                            ///////////////////////OVH 1-9, 1-10
                            if (
                                tresCuartos != 0 &&
                                registro.FechaInicioDepreciacion < new DateTime(2023, 5, 1) &&
                                registro.FechaInicioDepreciacion.AddMonths(registro.MesesMaximoDepreciacion) >= new DateTime(2023, 05, 1)
                               )
                            {
                                if (registro.FechaInicioDepreciacion.AddMonths(registro.MesesDepreciadosAñoAnterior) >= new DateTime(2023, 05, 01))
                                {
                                    registro.DepreciacionAcumuladaAñoAnterior -= tresCuartos;
                                }
                                else if (registro.FechaInicioDepreciacion.AddMonths(registro.MesesDepreciadosAñoAnterior + registro.MesesDepreciadosAñoActual) >= new DateTime(2023, 05, 01))
                                {
                                    registro.DepreciacionAñoActual -= tresCuartos;
                                }
                            }
                        }
                    }

                    registro.BajaDepreciacion = baja != null ? registro.DepreciacionAcumuladaAñoAnterior + registro.DepreciacionAñoActual : 0.0M;

                    registro.DepreciacionContableAcumulada = registro.DepreciacionAcumuladaAñoAnterior + registro.DepreciacionAñoActual - registro.BajaDepreciacion;

                    if ((registro.FechaBaja != null && añosAnteriores.Contains(registro.FechaBaja.Value.Year)) || (registro.Overhaul != 0.0M && registro.FechaCancelacion != null))
                    {

                    }
                    else
                    {
                        if (registro.FechaBaja != null)
                        {
                            registro.ValorEnLibros = 0.0M;
                        }
                        else
                        {
                            registro.ValorEnLibros = (registro.MOI + registro.Altas + registro.Overhaul) - registro.DepreciacionContableAcumulada - registro.MontoBaja;
                        }

                        if (registro.FechaCancelacion != null)
                        {
                            registro.ValorEnLibros = 0.0M;
                        }
                        if (registro.faltante && registro.EsOverhaul)
                        {
                            registro.ValorEnLibros = registro.MontoCancelacion - registro.DepreciacionContableAcumulada;
                        }
                    }
                    mesesAnteriores += registro.MesesDepreciadosAñoAnterior;
                    mesesActuales += registro.MesesDepreciadosAñoActual;
                }

                registro.MesesDepreciadosAñoActual = mesesActuales;
                if (mesesAnteriores > registro.MesesMaximoDepreciacion)
                {
                    registro.MesesDepreciadosAñoAnterior = registro.MesesMaximoDepreciacion;
                }
                else
                {
                    registro.MesesDepreciadosAñoAnterior = mesesAnteriores;
                }

                //EL 11-02-2021 JESSICA ME SOLICITO POR TELEFONO QUE LE AGREGARA $29.05 A LA DEPRECIACION DEL INSUMO: DISPENSADOR DE AGUA,
                //PORQUE ELLA EN LUGAR DE COMENZAR A DEPRECIARLO EN DICIEMBRE DE 2020 LO DEPRECIO EN ENERO SUMANDO LA DEP QUE DEBERIA DE TENER EN DICIEMBRE + LA DEP DE ENERO
                //PERO BUENO...
                if (cuenta == 1220 && vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                {
                    if (registro.Poliza == "2020-12-48-05-210")
                    {
                        if (new DateTime(añoActual, mesActual, 1) >= new DateTime(2020, 01, 01))
                        {
                            if (añoActual == 2021)
                            {
                                registro.MesesDepreciadosAñoActual += 1;
                                registro.DepreciacionAñoActual += 29.05M;
                                registro.DepreciacionContableAcumulada += 29.05M;
                                registro.ValorEnLibros -= 29.05M;

                                if (registro.FechaBaja != null)
                                {
                                    registro.BajaDepreciacion += 29.05M;
                                }
                            }
                            if (añoActual > 2021)
                            {
                                if (registro.FechaBaja == null)
                                {
                                    registro.MesesDepreciadosAñoAnterior += 1;
                                    registro.DepreciacionAcumuladaAñoAnterior += 29.05M;
                                    registro.DepreciacionContableAcumulada += 29.05M;
                                    registro.ValorEnLibros -= 29.05M;
                                }
                                else
                                {
                                    if (registro.FechaBaja.Value.Year == añoActual)
                                    {
                                        registro.MesesDepreciadosAñoAnterior += 1;
                                        registro.DepreciacionAcumuladaAñoAnterior += 29.05M;
                                        registro.DepreciacionContableAcumulada += 29.05M;
                                        registro.ValorEnLibros -= 29.05M;
                                        registro.BajaDepreciacion += 29.05M;
                                    }
                                }
                            }
                        }
                    }
                }
                //FIN DE LO QUE NO DEBERIA EXISTIR.

                //RECLASIFICACIONES DE LOS 200(20X) A LOS EQUIPOS, SE PASA SU DEP ACUMULADA A LA ALTA NUEVA
                if (añoActual >= 2021)
                {
                    Dictionary<string, decimal> movimientosCanceladosRec = new Dictionary<string, decimal>
                    {
                    };
                    Dictionary<string, decimal> movimientoNuevoRec = new Dictionary<string, decimal>()
                    {
                    };

                    if (movimientosCanceladosRec.Keys.Contains(registro.Poliza))
                    {
                        registro.DepreciacionAcumuladaAñoAnterior = movimientosCanceladosRec[registro.Poliza];
                        registro.DepreciacionContableAcumulada = movimientosCanceladosRec[registro.Poliza];
                        registro.BajaDepreciacion = 0;
                        registro.ValorEnLibros = 0;
                        registro.MesesMaximoDepreciacion = 24;
                    }

                    if (movimientoNuevoRec.Keys.Contains(registro.Poliza))
                    {
                        registro.DepreciacionAcumuladaAñoAnterior += movimientoNuevoRec[registro.Poliza];
                        registro.DepreciacionContableAcumulada += movimientoNuevoRec[registro.Poliza];
                        registro.MesesMaximoDepreciacion = 24;
                        registro.MesesDepreciadosAñoAnterior = registro.MesesMaximoDepreciacion - registro.MesesDepreciadosAñoActual;
                        if (baja != null)
                        {
                            registro.DepreciacionContableAcumulada = 0;
                            registro.ValorEnLibros = 0M;
                        }
                        else
                        {
                            registro.ValorEnLibros = (registro.MOI + registro.Altas + registro.Overhaul) - (registro.DepreciacionContableAcumulada);
                        }
                    }
                }

                #region AJUSTE DEPRECIACION OVERHAUL EN STANDBY DE ENERO A JULIO 2023
                if (registro.EsOverhaul)
                {
                    var fechaCorteJulio = new DateTime(2023, 7, 1);
                    var fechaCorteActual = new DateTime(añoActual, mesActual, DateTime.DaysInMonth(añoActual, mesActual));

                    var fechaBaja = registro.FechaBaja.HasValue ? registro.FechaBaja.Value : registro.FechaCancelacion.HasValue && !registro.faltante ? registro.FechaCancelacion.Value : (DateTime?)null;
                    fechaBaja = fechaBaja.HasValue ? new DateTime(fechaBaja.Value.Year, fechaBaja.Value.Month, 1) : (DateTime?)null;

                    var fechaInicioLogica = new DateTime(2023, 1, 1);

                    if (
                        fechaCorteActual >= fechaCorteJulio &&
                        (
                            !fechaBaja.HasValue ||
                            (
                                (
                                    fechaBaja.HasValue &&
                                    fechaBaja.Value >= fechaCorteJulio
                                ) ||
                                (
                                    fechaBaja.HasValue &&
                                    registro.FechaCancelacion.HasValue &&
                                    registro.MOI + registro.Overhaul == 0
                                )
                            )
                        )
                       )
                    {
                        var fechaUltimaDepreciacionDelActivo = new DateTime(registro.FechaInicioDepreciacion.Year, registro.FechaInicioDepreciacion.Month, 1)
                            .AddMonths(registro.MesesDepreciadosAñoAnterior + registro.MesesDepreciadosAñoActual);
                        fechaUltimaDepreciacionDelActivo = new DateTime
                            (
                                fechaUltimaDepreciacionDelActivo.Year,
                                fechaUltimaDepreciacionDelActivo.Month,
                                DateTime.DaysInMonth(fechaUltimaDepreciacionDelActivo.Year, fechaUltimaDepreciacionDelActivo.Month));
                        var fechaInicioDepreciacion = new DateTime
                            (
                                registro.FechaInicioDepreciacion.Year,
                                registro.FechaInicioDepreciacion.Month,
                                DateTime.DaysInMonth(registro.FechaInicioDepreciacion.Year, registro.FechaInicioDepreciacion.Month)
                            );
                        if (fechaUltimaDepreciacionDelActivo >= fechaInicioLogica)
                        {
                            var semanasEn14_1 = corteInventarioEneroJulio
                                .Where(x =>
                                    x.noEconomico == registro.Clave &&
                                    x.fechaCorte > fechaInicioDepreciacion &&
                                    x.fechaCorte <= fechaUltimaDepreciacionDelActivo).Count();

                            #region AJUSTE DE SEMANAS POR EQUIPO
                            if (semanasEn14_1 > 0)
                            {
                                switch (registro.Clave)
                                {
                                    case "PR-05":
                                        {
                                            switch (registro.MesesMaximoDepreciacion)
                                            {
                                                case 12:
                                                    semanasEn14_1 -= 1;
                                                    break;
                                                case 18:
                                                    semanasEn14_1 -= 1;
                                                    break;
                                            }
                                        }
                                        break;
                                    case "CFC-70":
                                        {
                                            switch (registro.MesesMaximoDepreciacion)
                                            {
                                                case 24:
                                                    semanasEn14_1 -= 1;
                                                    break;
                                            }
                                        }
                                        break;
                                    case "CFC-65":
                                        {
                                            switch (registro.MesesMaximoDepreciacion)
                                            {
                                                case 18:
                                                    semanasEn14_1 -= 1;
                                                    break;
                                            }
                                        }
                                        break;
                                    case "CFC-64":
                                        {
                                            switch (registro.MesesMaximoDepreciacion)
                                            {
                                                case 12:
                                                    semanasEn14_1 -= 1;
                                                    break;
                                                case 24:
                                                    semanasEn14_1 -= 1;
                                                    break;
                                            }
                                        }
                                        break;
                                    case "CFC-69":
                                        {
                                            switch (registro.MesesMaximoDepreciacion)
                                            {
                                                case 24:
                                                    {
                                                        if (registro.Poliza == "2023-6-231-03-27")
                                                        {
                                                            semanasEn14_1 -= 3;
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                }
                            }
                            #endregion

                            registro.semanasDepreciacionOverhaul14_1 = semanasEn14_1;
                            registro.depreciacionOverhaul14_1 = (registro.DepreciacionMensual / 4) * semanasEn14_1;
                            if (fechaCorteActual.Year > 2023)
                            {
                                registro.DepreciacionAcumuladaAñoAnterior -= registro.depreciacionOverhaul14_1;
                                registro.DepreciacionContableAcumulada -= registro.depreciacionOverhaul14_1;
                            }
                            else
                            {
                                registro.DepreciacionAñoActual -= registro.depreciacionOverhaul14_1;
                                registro.DepreciacionContableAcumulada -= registro.depreciacionOverhaul14_1;
                            }

                            registro.ValorEnLibros += registro.depreciacionOverhaul14_1;
                        }
                    }

                    {
                        var fechaCorteAgosto = new DateTime(2023, 8, 1);
                        if (new DateTime(añoActual, mesActual, 1) >= fechaCorteAgosto)
                        {
                            var obraPrimeraSemanaDelMes = corteInventarioDelMes
                                .Where(x =>
                                    x.noEconomico == registro.Clave
                                ).OrderBy(x => x.fechaCorte).FirstOrDefault();
                            if (obraPrimeraSemanaDelMes != null)
                            {
                                if (obraPrimeraSemanaDelMes.areaCuenta != "14-1" && (registro.Area + "-" + registro.Cuenta_OC) == "14-1")
                                {
                                    var obraAnterior = corteInventarioDelMes
                                        .Where(x =>
                                            x.noEconomico == registro.Clave &&
                                            x.fechaCorte <= fechaCortePol &&
                                            x.areaCuenta != "14-1"
                                        ).OrderByDescending(x => x.fechaCorte).FirstOrDefault();
                                    if (obraAnterior != null)
                                    {
                                        registro.Area = Convert.ToInt32(obraAnterior.areaCuenta.Split('-')[0]);
                                        registro.Cuenta_OC = Convert.ToInt32(obraAnterior.areaCuenta.Split('-')[1]);
                                        registro.AreaCuenta = obraAnterior.areaCuenta + " - " + obraAnterior.obra;
                                        registro.AreaCuentaDescripcion = obraAnterior.obra;
                                    }
                                }
                                if (obraPrimeraSemanaDelMes.areaCuenta == "14-1" && (registro.Area + "-" + registro.Cuenta_OC) != "14-1")
                                {
                                    registro.Area = 14;
                                    registro.Cuenta_OC = 1;
                                    registro.AreaCuenta = "14-1 - MAQUINARIA NO ASIGNADA A OBRA";
                                    registro.AreaCuentaDescripcion = "1MAQUINARIA NO ASIGNADA A OBRA";
                                    registro.esOverhaul14_1 = true;
                                }
                            }
                            if (primerasSemanas != null)
                            {
                                var fechaIniDep = new DateTime(registro.FechaInicioDepreciacion.Year, registro.FechaInicioDepreciacion.Month, 1);
                                var fechaIniDepDiaFinal = new DateTime(fechaIniDep.Year, fechaIniDep.Month, DateTime.DaysInMonth(fechaIniDep.Year, fechaIniDep.Month));

                                if (fechaIniDep.AddMonths(registro.MesesMaximoDepreciacion) >= fechaCorteAgosto)
                                {
                                    if (!fechaBaja.HasValue || (fechaBaja.HasValue && fechaBaja.Value >= fechaCorteAgosto))
                                    {
                                        var mesesAnioAnterior = 0;
                                        var mesesAnioActual = 0;

                                        var mesesDepAntesAgosto = (((fechaCorteAgosto.Year - fechaIniDep.Year) * 12) + fechaCorteAgosto.Month - fechaIniDep.Month) - 1;

                                        if (mesesDepAntesAgosto < 0)
                                        {
                                            var mesesDepDespuestAgosto = (mesesDepAntesAgosto + 1) * -1;

                                            for (int i = 0; i < mesesDepDespuestAgosto; i++)
                                            {
                                                var fechaDespeusAgosto2023 = fechaIniDep.AddMonths(i + 1);
                                                if (fechaDespeusAgosto2023.Year < añoActual)
                                                {
                                                    mesesAnioAnterior += 1;
                                                }
                                                else
                                                {
                                                    mesesAnioActual += 1;
                                                }
                                            }
                                        }

                                        mesesDepAntesAgosto = mesesDepAntesAgosto > 0 ? mesesDepAntesAgosto : 0;

                                        for (int i = 0; i < mesesDepAntesAgosto; i++)
                                        {
                                            var fechaAntesAgosto2023 = fechaIniDep.AddMonths(i + 1);
                                            if (fechaAntesAgosto2023.Year < añoActual)
                                            {
                                                mesesAnioAnterior += 1;
                                            }
                                            else
                                            {
                                                mesesAnioActual += 1;
                                            }
                                        }

                                        if ((mesesAnioActual + mesesAnioAnterior) < registro.MesesMaximoDepreciacion)
                                        {
                                            List<ActivoFijoCorteInventarioDTO> primerasSemanaEquipo = new List<ActivoFijoCorteInventarioDTO>();
                                            if (fechaBaja.HasValue)
                                            {
                                                var fechaBajaUltimoDia = new DateTime(fechaBaja.Value.Year, fechaBaja.Value.Month, DateTime.DaysInMonth(fechaBaja.Value.Year, fechaBaja.Value.Month));
                                                primerasSemanaEquipo = primerasSemanas.Where(x => x.noEconomico == registro.Clave && x.fechaCorte <= fechaBajaUltimoDia && x.fechaCorte > fechaIniDepDiaFinal).OrderBy(x => x.fechaCorte).ToList();
                                            }
                                            else
                                            {
                                                primerasSemanaEquipo = primerasSemanas.Where(x => x.noEconomico == registro.Clave && x.fechaCorte > fechaIniDepDiaFinal).OrderBy(x => x.fechaCorte).ToList();
                                            }

                                            var mesesOH_1_Actual = 0;
                                            var mesesOH_1_Anterior = 0;
                                            var mesesObraActual = 0;
                                            var mesesObraAnterior = 0;
                                            var mesesSinDepreciarDespuesDeVencerActual = 0;
                                            var mesesSinDepreciarDespuesDeVencerAnterior = 0;
                                            foreach (var primeraSemana in primerasSemanaEquipo)
                                            {
                                                if (primeraSemana.areaCuenta != "14-1")
                                                {
                                                    if ((mesesAnioAnterior + mesesAnioActual + mesesObraActual + mesesObraAnterior) < registro.MesesMaximoDepreciacion)
                                                    {
                                                        if (primeraSemana.fechaCorte.Year < añoActual)
                                                        {
                                                            mesesObraAnterior += 1;
                                                        }
                                                        else
                                                        {
                                                            mesesObraActual += 1;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if ((mesesAnioAnterior + mesesAnioActual + mesesObraActual + mesesObraAnterior) < registro.MesesMaximoDepreciacion && ((mesesAnioAnterior + mesesAnioActual + mesesObraActual + mesesObraAnterior) != 0 || registro.DepreciacionAñoActual != 0))
                                                    {
                                                        if (primeraSemana.fechaCorte.Year < añoActual)
                                                        {
                                                            mesesOH_1_Anterior += 1;
                                                        }
                                                        else
                                                        {
                                                            mesesOH_1_Actual += 1;
                                                        }
                                                        

                                                        //registro.semanasDepreciacionOverhaul14_1 += 1;
                                                        //registro.depreciacionOverhaul14_1 += (registro.DepreciacionMensual / 4);
                                                        registro.semanasDepreciacionOverhaul14_1 += 4;
                                                        registro.depreciacionOverhaul14_1 += registro.DepreciacionMensual;
                                                    }
                                                }
                                            }

                                            if ((mesesAnioAnterior + mesesAnioActual + mesesOH_1_Anterior) >= registro.MesesMaximoDepreciacion)
                                            {
                                                mesesSinDepreciarDespuesDeVencerAnterior = (mesesAnioAnterior + mesesAnioActual + mesesOH_1_Anterior) - registro.MesesMaximoDepreciacion;
                                            }
                                            else if ((mesesAnioAnterior + mesesAnioActual + mesesOH_1_Anterior + mesesOH_1_Actual) >= registro.MesesMaximoDepreciacion)
                                            {
                                                mesesSinDepreciarDespuesDeVencerActual = (mesesAnioAnterior + mesesAnioActual + mesesOH_1_Anterior + mesesOH_1_Actual) - registro.MesesMaximoDepreciacion;
                                            }

                                            registro.MesesDepreciadosAñoAnterior = mesesAnioAnterior + mesesObraAnterior;
                                            registro.MesesDepreciadosAñoAnteriorParaDiferencias = registro.MesesDepreciadosAñoAnterior;
                                            registro.MesesDepreciadosAñoActual = mesesAnioActual + mesesObraActual;
                                            registro.DepreciacionAcumuladaAñoAnterior = (registro.DepreciacionAcumuladaAñoAnterior + (registro.DepreciacionMensual * mesesSinDepreciarDespuesDeVencerAnterior)) - (registro.DepreciacionMensual * (mesesOH_1_Anterior + mesesObraAnterior)) + (registro.DepreciacionMensual * mesesObraAnterior);
                                            //registro.DepreciacionAñoActual = (registro.DepreciacionAñoActual + (registro.DepreciacionMensual * mesesSinDepreciarDespuesDeVencerActual)) - (registro.DepreciacionMensual * (mesesOH_1_Actual + mesesObraActual)) + (registro.DepreciacionMensual * mesesObraActual);
                                            registro.DepreciacionAñoActual = ((registro.DepreciacionAñoActual + (registro.DepreciacionMensual * mesesSinDepreciarDespuesDeVencerActual)) - (registro.DepreciacionMensual * mesesOH_1_Actual));
                                            registro.BajaDepreciacion = fechaBaja.HasValue ? registro.DepreciacionAcumuladaAñoAnterior + registro.DepreciacionAñoActual : 0;
                                            registro.DepreciacionContableAcumulada = registro.DepreciacionAcumuladaAñoAnterior + registro.DepreciacionAñoActual - registro.BajaDepreciacion;
                                            registro.ValorEnLibros = (registro.MOI + registro.Altas + registro.Overhaul) - registro.DepreciacionContableAcumulada - registro.MontoBaja;

                                            if (
                                                fechaIniDep.AddMonths(registro.MesesDepreciadosAñoAnterior + registro.MesesDepreciadosAñoActual) >= fechaIniDep.AddMonths(registro.MesesMaximoDepreciacion) &&
                                                new DateTime(fechaCorteActual.Year, fechaCorteActual.Month, 1) > fechaIniDep.AddMonths(registro.MesesDepreciadosAñoAnterior + registro.MesesDepreciadosAñoActual)
                                               )
                                            {
                                                registro.DepreciacionTerminadaPorMeses = true;
                                            }
                                            else
                                            {
                                                registro.DepreciacionTerminadaPorMeses = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                registrosCalculados.Add(registro);

                //PARA COSTO
                if (cuenta == 1210 && vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora && baja != null && baja.bajaCosto)
                {
                    if (registro.FechaBaja != null && registro.FechaBaja.Value.Year == añoActual && registro.DepreciacionAñoActual > 0)
                    {
                        var _depMensual = registro.DepreciacionMensual;
                        var _depSemanal = _depMensual / 4;
                        registro.BajaDepreciacion = registro.DepreciacionAcumuladaAñoAnterior + registro.DepreciacionAñoActual;
                    }

                    if (registro.FechaBaja != null && registro.FechaBaja.Value.Year < añoActual && registro.DepreciacionAñoActual == 0)
                    {
                        var _depMensual = registro.DepreciacionMensual;
                        var _depSemanal = _depMensual / 4;
                        registro.BajaDepreciacion = registro.DepreciacionAcumuladaAñoAnterior + registro.DepreciacionAñoActual;
                    }
                }
            }
        }

        private Dictionary<string, object> getRelacionSubcuentas(List<int> años, List<int> cuentas)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

            try
            {
                List<tblC_AF_RelSubCuentas> relacionSubcuentas = _context.tblC_AF_RelSubCuentas.Where
                    (x =>
                        cuentas.Contains(x.Cuenta.Cuenta) && x.Estatus
                    ).ToList();

                if (relacionSubcuentas != null && relacionSubcuentas.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, relacionSubcuentas);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "No se encontró la relación de subcuentas");
                }
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Error: " + ex.ToString());
            }

            return resultado;
        }

        private sc_movpolDTO encontrarBaja(int cuenta, ActivoFijoDetalleDTO registro, List<sc_movpolDTO> lista_sc_movpol, List<tblC_AF_RelSubCuentas> relacionSubcuentas, DateTime fechaHasta)
        {
            sc_movpolDTO baja = null;

            List<sc_movpolDTO> lista_coincidencias = lista_sc_movpol.Where
                    (x =>
                        x.Cta == cuenta &&
                        (x.TM != 4) &&
                        relacionSubcuentas.Where
                            (r =>
                                !r.EsCuentaDepreciacion &&
                                r.Cuenta.Cuenta == x.Cta &&
                                r.Subcuenta == x.Scta &&
                                r.SubSubcuenta == x.Sscta //&&
                                //r.Año == registro.Fecha.Year
                            ).Count() > 0 &&
                        relacionSubcuentas.Where
                            (r =>
                                !r.EsCuentaDepreciacion &&
                                r.Excluir &&
                                r.Subcuenta == x.Scta &&
                                r.SubSubcuenta == x.Sscta &&
                                r.Año == x.Year
                            ).Count() == 0 &&
                        (
                            (registro.MOI > 0.0M && x.Monto == (registro.MOI * -1)) ||
                            (registro.Altas > 0.0M && x.Monto == (registro.Altas * -1)) ||
                            (registro.Overhaul > 0.0M && x.Monto == (registro.Overhaul * -1))
                        )
                     ).ToList();

            if (lista_coincidencias != null && lista_coincidencias.Count(x => x.TM != 1) > 0)
            {
                if (lista_coincidencias.Where(x => (x.FechaPol.Year == registro.Fecha.Year && x.FechaPol.Month >= registro.Fecha.Month) || (x.FechaPol.Year > registro.Fecha.Year)).Count(x => x.TM != 1) == 1)
                {
                    baja = lista_coincidencias.FirstOrDefault(x => x.TM != 1);
                }
                else
                {
                    baja = lista_coincidencias.FirstOrDefault
                        (x =>
                            (x.FechaPol.Year == registro.Fecha.Year && x.FechaPol.Month >= registro.Fecha.Month) || (x.FechaPol.Year > registro.Fecha.Year) &&
                            x.TM != 1 &&
                            //x.Referencia == registro.Clave &&
                            (
                                (x.FechaCFD != null && registro.Fecha <= x.FechaCFD && x.FechaCFD.Value.Year >= 2018) ||
                                (x.FechaFactura != null && (x.FechaCFD == null || x.FechaCFD != null && x.FechaCFD.Value.Year < 2018)  && registro.Fecha <= x.FechaFactura) ||
                                (registro.Fecha <= x.FechaPol && x.FechaFactura == null && (x.FechaCFD == null || x.FechaCFD != null && x.FechaCFD.Value.Year < 2018))
                            )
                        );
                }
            }

            //Excepcion
            if (baja != null && baja.Year == 2021 && baja.Mes == 2 && baja.Poliza == 76 && baja.TP == "05" && baja.Linea == 9 && baja.Cta == 1210 && vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
            {
                baja = null;
            }
            if (registro.AñoPol_alta == 2020 && registro.MesPol_alta == 2 && registro.PolPol_alta == 49 && registro.TpPol_alta == "05" && registro.LineaPol_alta == 493 &&
                baja != null && baja.Year == 2021 && baja.Mes == 3 && baja.Poliza == 149 && baja.TP == "05" && baja.Linea == 3)
            {
                baja = null;
            }
            if (baja != null && baja.Year == 2021 && baja.Mes == 3 && baja.Poliza == 71 && baja.TP == "05" && baja.Linea == 1 && registro.Cc.ToUpper() != "AIT")
            {
                baja = null;
            }
            if (baja != null && baja.Year == 2022 && baja.Mes == 4 && baja.Poliza == 52 && baja.TP == "05" && baja.Linea == 32)//2022-4-52-05-32
            {
                baja = null;
            }
            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
            {
                if (fechaHasta >= new DateTime(2021, 10, 01))
                {
                    if (baja != null && baja.Year == 2021 && baja.Mes == 9 && baja.Poliza == 90 && baja.TP == "05" && baja.Linea == 3 && baja.Cta == 1210)
                    {
                        baja = null;
                    }
                    if (baja != null && baja.Year == 2021 && baja.Mes == 8 && baja.Poliza == 214 && baja.TP == "03" && baja.Linea == 1 && baja.Cta == 1210)
                    {
                        baja = null;
                    }
                    if (baja != null && baja.Year == 2021 && baja.Mes == 9 && baja.Poliza == 90 && baja.TP == "05" && baja.Linea == 1 && baja.Cta == 1210)
                    {
                        baja = null;
                    }
                }
                if (registro.Poliza == "2022-8-223-03-8" && baja != null && baja.Year == 2022 && baja.Mes == 8 && baja.Poliza == 223 && baja.TP == "03" && baja.Linea == 7 && baja.Cta == 1210)
                {
                    baja = null;
                }
                if (registro.Poliza == "2022-9-74-05-334" && baja != null && baja.Year == 2022 && baja.Mes == 10 && baja.Poliza == 116 && baja.TP == "05" && baja.Linea == 1 && baja.Cta == 1210)
                {
                    baja = null;
                }
            }            

            return baja;
        }

        private void sumaComplementos(List<sc_movpolDTO> lista_sc_movpol, List<tblM_CatMaquinaDepreciacion> catMaqDep, DateTime fechaConsulta)
        {
            List<sc_movpolDTO> complementosEncontrados = new List<sc_movpolDTO>();
            List<sc_movpolDTO> financiamientosEncontrados = new List<sc_movpolDTO>();

            List<sc_movpolDTO> registrosAEliminar = new List<sc_movpolDTO>();

            List<sc_movpolDTO> lista_sc_movpol_auxiliar = new List<sc_movpolDTO>();
            lista_sc_movpol_auxiliar.AddRange(lista_sc_movpol);

            //foreach (var item in catMaqDep)
            //{
            //    var match = lista_sc_movpol.FirstOrDefault(x => x.Cta == item.Poliza.Cuenta && x.Year == item.Poliza.Año && x.Mes == item.Poliza.Mes && x.Poliza == item.Poliza.Poliza && x.TP == item.Poliza.TP && x.Linea == item.Poliza.Linea);
            //    if (match != null)
            //    {
            //        lista_sc_movpol_auxiliar.Remove(match);
            //    }
            //}

            //EXCEPCIONES
            var nuevoRegEx = new sc_movpolDTO()
            {
                Year = 2018,
                Mes = 10,
                Poliza = 343,
                TP = "0A",
                Linea = 99999,
                Cc = "200",
                Referencia = "",
                Cta = 1215,
                Scta = 1,
                Sscta = 1,
                Concepto = "Sobrante - PREMIER AUTOCOUNTRY SA DE CV",
                Digito = 7,
                Monto = 129517.24M,
                TM = 3,
                ITM = 0,
                FechaPol = new DateTime(2018, 10, 26)
            };

            var nuevoRegEx2 = new sc_movpolDTO()
            { //2018-10-343-0A-2
                Year = 2019,
                Mes = 10,
                Poliza = 343,
                TP = "0A",
                Linea = 99998,
                Cc = "200",
                Referencia = "",
                Cta = 1215,
                Scta = 1,
                Sscta = 1,
                TM = 3,
                Concepto = "Sobrante - PREMIER AUTOCOUNTRY SA DE CV",
                Digito = 7,
                Monto = 361395.76M,
                ITM = 0,
                FechaPol = new DateTime(2019, 10, 26)
            };

            var nuevoRegEx3 = new sc_movpolDTO()
            { //2021-3-118-05-537
                Year = 2021,
                Mes = 3,
                Poliza = 118,
                TP = "05",
                Linea = 99997,
                Cc = "E02",
                Referencia = "",
                Cta = 1210,
                Scta = 6,
                Sscta = 1,
                TM = 1,
                Concepto = "INSUMO: 58357409 COMPRESOR HR2.5 (REPA",
                Digito = 7,
                Monto = 221151.73M,
                ITM = 51,
                FechaPol = new DateTime(2021, 3, 19)
            };

            lista_sc_movpol.Add(nuevoRegEx);
            lista_sc_movpol.Add(nuevoRegEx2);
            lista_sc_movpol.Add(nuevoRegEx3);
            //EXCEPCIONES FIN

            //var polizasQueNoSeRelacionaran = _context.tblC_AF_PolizasExcluidasParaCapturaAutomatica.Where(x => x.Estatus).ToList();

            //foreach (var item in polizasQueNoSeRelacionaran)
            //{
            //    lista_sc_movpol.RemoveAll(x => x.Year == item.Año && x.Mes == item.Mes && x.Poliza == item.Poliza && x.TP == item.TipoPoliza && x.Linea == item.Linea);
            //}

            //EXCEPCIONES - QUITAR DIRECTAMENTE
            if (fechaConsulta >= new DateTime(2021, 9, 01))
            {
                lista_sc_movpol.RemoveAll(x => x.Year == 2021 && x.Mes == 8 && x.Poliza == 214 && x.TP == "03" && x.Linea == 17 && x.Cta == 1210); // CFC-54
                lista_sc_movpol.RemoveAll(x => x.Year == 2021 && x.Mes == 9 && x.Poliza == 65 && x.TP == "03" && x.Linea == 1 && x.Cta == 1210); // CFC-54
            }
            if (fechaConsulta >= new DateTime(2021, 10, 01))
            {
                lista_sc_movpol.RemoveAll(x => x.Year == 2021 && x.Mes == 8 && x.Poliza == 214 && x.TP == "03" && x.Linea == 4 && x.Cta == 1210); // CFC-13
                lista_sc_movpol.RemoveAll(x => x.Year == 2021 && x.Mes == 10 && x.Poliza == 239 && x.TP == "03" && x.Linea == 4 && x.Cta == 1210); // CFC-13
                lista_sc_movpol.RemoveAll(x => x.Year == 2021 && x.Mes == 8 && x.Poliza == 35 && x.TP == "05" && x.Linea == 424); // CFC-13
                lista_sc_movpol.RemoveAll(x => x.Year == 2021 && x.Mes == 9 && x.Poliza == 90 && x.TP == "05" && x.Linea == 1); // CFC-13

                //lista_sc_movpol.RemoveAll(x => x.Year == 2021 && x.Mes == 8 && x.Poliza == 208 && x.TP == "05" && x.Linea == 473 && x.Cta == 1210); // CFC-10
                //lista_sc_movpol.RemoveAll(x => x.Year == 2021 && x.Mes == 9 && x.Poliza == 90 && x.TP == "05" && x.Linea == 3 && x.Cta == 1210); // CFC-10

                lista_sc_movpol.RemoveAll(x => x.Year == 2021 && x.Mes == 2 && x.Poliza == 66 && x.TP == "05" && x.Linea == 125 && x.Cta == 1210); // CFC-75
                lista_sc_movpol.RemoveAll(x => x.Year == 2021 && x.Mes == 9 && x.Poliza == 181 && x.TP == "03" && x.Linea == 76 && x.Cta == 1210); // CFC-75
                lista_sc_movpol.RemoveAll(x => x.Year == 2021 && x.Mes == 2 && x.Poliza == 66 && x.TP == "05" && x.Linea == 126 && x.Cta == 1210); // CFC-75
                lista_sc_movpol.RemoveAll(x => x.Year == 2021 && x.Mes == 2 && x.Poliza == 233 && x.TP == "03" && x.Linea == 7 && x.Cta == 1210); // CFC-75
                lista_sc_movpol.RemoveAll(x => x.Year == 2021 && x.Mes == 2 && x.Poliza == 102 && x.TP == "05" && x.Linea == 37 && x.Cta == 1210); // CFC-75
                lista_sc_movpol.RemoveAll(x => x.Year == 2021 && x.Mes == 2 && x.Poliza == 102 && x.TP == "05" && x.Linea == 35 && x.Cta == 1210); // CFC-75
                lista_sc_movpol.RemoveAll(x => x.Year == 2021 && x.Mes == 10 && x.Poliza == 239 && x.TP == "03" && x.Linea == 15 && x.Cta == 1210); // CFC-75
            }
            if (fechaConsulta >= new DateTime(2022, 3, 1))
            {
                lista_sc_movpol.RemoveAll(x => x.Year == 2022 && x.Mes == 3 && x.Poliza == 321 && x.TP == "03" && x.Cta == 1210); // 2022-3-321-03-2 PR-09
            }
            //EXCEPCIONES - QUITAR DIRECTAMENTE FIN

            /*EQUIPO DE OFICINA ARRENDADORA*/
            var fechaEquipoOficina = new DateTime(2023, 7, 1);
            lista_sc_movpol.RemoveAll(x => x.Cta == 1220 && x.FechaPol >= fechaEquipoOficina && x.Monto <= 100);
            /*EQUIPO DE OFICINA ARRENDADORA*/

            /******/
            foreach (var maqDep in catMaqDep.Where(x => x.Poliza.TipoActivo == 3).GroupBy(x => x.IdPolizaReferenciaAlta))
            {
                var maqDepPrincipal = catMaqDep.FirstOrDefault(x => x.IdPoliza == maqDep.Key);
                if (maqDepPrincipal != null)
                {
                    var poliza = lista_sc_movpol
                        .FirstOrDefault(x =>
                            x.Year == maqDepPrincipal.Poliza.Año &&
                            x.Mes == maqDepPrincipal.Poliza.Mes &&
                            x.Poliza == maqDepPrincipal.Poliza.Poliza &&
                            x.TP == maqDepPrincipal.Poliza.TP &&
                            x.Linea == maqDepPrincipal.Poliza.Linea);
                    if (poliza != null)
                    {
                        var polizasUnion = new List<sc_movpolDTO>();
                        foreach (var item in maqDep)
                        {
                            var polizaUnion = lista_sc_movpol.FirstOrDefault(x =>
                                x.Year == item.Poliza.Año &&
                                x.Mes == item.Poliza.Mes &&
                                x.Poliza == item.Poliza.Poliza &&
                                x.TM == item.Poliza.TM &&
                                x.Linea == item.Poliza.Linea);
                            if (polizaUnion != null)
                            {
                                polizasUnion.Add(polizaUnion);
                            }
                            else
                            {
                                throw new Exception("No se encontró la póliza de unión: " + item.Poliza.Año + "-" + item.Poliza.Mes + "-" + item.Poliza.Poliza + "-" + item.Poliza.TM + "-" + item.Poliza.Linea + " : [" + item.Maquina.noEconomico + "]");
                            }
                        }

                        poliza.Monto += polizasUnion.Sum(x => x.Monto);

                        lista_sc_movpol.RemoveAll(x => polizasUnion.Contains(x));
                    }
                    else
                    {
                        //throw new Exception("No se encontró la póliza principal de unión: " + maqDepPrincipal.Poliza.Año + "-" + maqDepPrincipal.Poliza.Mes + "-" + maqDepPrincipal.Poliza.Poliza + "-" + maqDepPrincipal.Poliza.TP + "-" + maqDepPrincipal.Poliza.Linea + " : [" + maqDepPrincipal.Maquina.noEconomico + "]");
                    }
                }
                else
                {
                    //throw new Exception("No se el registro principal de la relación-póliza-unión: " + maqDep.Key);
                }
            }

            foreach (var item in catMaqDep.Where(x => x.Poliza.TipoActivo == 4))
            {
                var poliza = lista_sc_movpol
                    .FirstOrDefault(x =>
                        x.Year == item.Poliza.Año &&
                        x.Mes == item.Poliza.Mes &&
                        x.Poliza == item.Poliza.Poliza &&
                        x.TP == item.Poliza.TP &&
                        x.Linea == item.Poliza.Linea);

                if (poliza != null)
                {
                    lista_sc_movpol.Remove(poliza);
                }
            }
            /******/

            foreach (sc_movpolDTO registro in lista_sc_movpol)
            {
                //EXCEPCIONES
                if (registro.Year == 2019 && registro.Mes == 12 && registro.Poliza == 156 && registro.TP == "07" && registro.Linea == 4 && registro.Cta == 1215)
                {
                    registro.Monto -= 0.01M;
                }
                if (registro.Year == 2018 && registro.Mes == 12 && registro.Poliza == 68 && registro.TP == "03" && registro.Linea == 6 && registro.Cta == 1215)
                {
                    registro.Monto += 0.01M;
                }
                //if (registro.Year == 2018 && registro.Mes == 10 && registro.Poliza == 343 && registro.TP == "0A" && registro.Linea == 2 && registro.Cta == 1215)
                //{
                //    registro.Monto = 809482.76M;
                //}
                if (registro.Year == 2018 && registro.Mes == 12 && registro.Poliza == 1555 && registro.TP == "07" && registro.Linea == 5 && registro.Cta == 1215)
                {
                    registro.Monto = 0.0M;
                }
                if (registro.Year == 2019 && registro.Mes == 2 && registro.Poliza == 90 && registro.TP == "05" && registro.Linea == 543 && registro.Cta == 1210)
                {
                    registro.Monto = 492675.68M;
                }
                if (registro.Year == 2019 && registro.Mes == 2 && registro.Poliza == 99 && registro.TP == "05" && registro.Linea == 449 && registro.Cta == 1210)
                {
                    registro.Monto = 59453.47M;
                }
                //if (registro.Year == 2019 && registro.Mes == 4 && registro.Poliza == 82 && registro.TP == "05" && registro.Linea == 472 && registro.Cta == 1210)
                //{
                //    registro.Monto = 468707.42M;
                //}
                //2021-5-174-03-8
                if (registro.Year == 2021 && registro.Mes == 5 && registro.Poliza == 174 && registro.TP == "03" && registro.Linea == 8 && registro.Cta == 1210)
                {
                    registro.Monto = 468707.42M;
                }
                if (registro.Year == 2019 && registro.Mes == 4 && registro.Poliza == 18 && registro.TP == "05" && registro.Linea == 548 && registro.Cta == 1210)
                {
                    registro.Monto = 109502.93M;
                }
                if (registro.Year == 2018 && registro.Mes == 12 && registro.Poliza == 158 && registro.TP == "03" && (registro.Linea == 3 || registro.Linea == 4) && registro.Cta == 1225)
                {
                    registro.FechaCFD = new DateTime(2018, 11, 01);
                }
                if (registro.Year == 2019 && registro.Mes == 5 && registro.Poliza == 162 && registro.TP == "03" && (registro.Linea == 7) && registro.Cta == 1211)
                {
                    registro.FechaCFD = new DateTime(2019, 5, 01);
                }
                if (registro.Year == 2019 && registro.Mes == 3 && registro.Poliza == 137 && registro.TP == "03" && (registro.Linea == 67) && registro.Cta == 1215)
                {
                    registro.FechaCFD = new DateTime(2019, 4, 01);
                }
                if (registro.Year == 2019 && registro.Mes == 3 && registro.Poliza == 137 && registro.TP == "03" && (registro.Linea == 106) && registro.Cta == 1215)
                {
                    registro.FechaCFD = new DateTime(2019, 4, 01);
                }
                if (registro.Year == 2018 && registro.Mes == 10 && registro.Poliza == 343 && registro.TP == "0A" && registro.Linea == 2 && registro.Cta == 1215)
                {
                    /*registro.Monto = 809482.76M;*/ registro.Monto = 448087; registro.TM = 3;
                }
                if (registro.Year == 2019 && registro.Mes == 9 && registro.Poliza == 28 && registro.TP == "03" && registro.Linea == 19 && registro.Cta == 1215)
                {
                    registro.FechaCFD = new DateTime(2019, 4, 01);
                }
                if (registro.Year == 2018 && registro.Mes == 8 && registro.Poliza == 3 && registro.TP == "0K" && registro.Linea == 3 && registro.Cta == 1215)
                {
                    registro.Cc = "LTE";
                    registro.Sscta = 1;
                }
                //SE MODIFICO MONTO ORIGINAL DE LA ALTA
                if (registro.Year == 2020 && registro.Mes == 2 && registro.Poliza == 49 && registro.TP == "05" && registro.Linea == 485 && registro.Cta == 1210)
                {
                    registro.Monto = 50118.77M;
                }
                if (registro.Year == 2020 && registro.Mes == 2 && registro.Poliza == 49 && registro.TP == "05" && registro.Linea == 487 && registro.Cta == 1210)
                {
                    registro.Monto = 142631.81M;
                }
                //2021-3-118-05-537
                if (registro.Year == 2021 && registro.Mes == 3 && registro.Poliza == 118 && registro.TP == "05" && registro.Linea == 537)
                {
                    registro.Monto = 567792.57M;
                }
                if (registro.Year == 2021 && registro.Mes == 2 && registro.Poliza == 160 && registro.TP == "05" && registro.Linea == 326)
                {
                    registro.Monto = 77333.20M;
                }
                //2021-4-194-03-4
                if (registro.Year == 2021 && registro.Mes == 4 && registro.Poliza == 194 && registro.TP == "03" && registro.Linea == 4)
                {
                    registro.Monto = -221151.73M;
                }
                if (registro.Year == 2022 && registro.Mes == 4 && registro.Poliza == 2234 && registro.TP == "07" && registro.Linea == 3)
                {
                    registro.FechaCFD = new DateTime(2022, 04, 01);
                }
                //EXCEPCIONES FIN

                foreach (sc_movpolDTO registroComplemento in lista_sc_movpol_auxiliar)
                {
                    //EXCEPCIONES
                    if (registro.Year == 2018 && registro.Mes == 12 && registro.Poliza == 2256 && registro.TP == "07" && registro.Linea == 6 && registro.Cta == 1211)
                    {
                        if (registroComplemento.Year == 2018 && registroComplemento.Mes == 12 && (registroComplemento.Poliza == 2257 || registroComplemento.Poliza == 2258) && registroComplemento.TP == "07" && registroComplemento.Linea == 6 && registroComplemento.Cta == 1211)
                        {
                            registro.Monto += registroComplemento.Monto;
                            complementosEncontrados.Add(registroComplemento);
                        }
                    }
                    if (registro.Year == 2018 && registro.Mes == 12 && registro.Poliza == 2257 && registro.TP == "07" && registro.Linea == 4 && registro.Cta == 1211)
                    {
                        if (registroComplemento.Year == 2018 && registroComplemento.Mes == 12 && registroComplemento.TP == "07" &&
                            (
                                (registroComplemento.Poliza == 2257 && registroComplemento.Linea == 3) ||
                                (registroComplemento.Poliza == 2256 && registroComplemento.Linea == 4) ||
                                (registroComplemento.Poliza == 2256 && registroComplemento.Linea == 3)
                            ) &&
                            registroComplemento.Cta == 1211)
                        {
                            registro.Monto += registroComplemento.Monto;
                            complementosEncontrados.Add(registroComplemento);
                        }
                    }
                    if (registro.Year == 2018 && registro.Mes == 12 && registro.Poliza == 2258 && registro.TP == "07" && registro.Linea == 3 && registro.Cta == 1211)
                    {
                        if (registroComplemento.Year == 2018 && registroComplemento.Mes == 12 && registroComplemento.TP == "07" &&
                            (
                                (registroComplemento.Poliza == 2258 && registroComplemento.Linea == 4)
                            ) &&
                            registroComplemento.Cta == 1211)
                        {
                            registro.Monto += registroComplemento.Monto;
                            complementosEncontrados.Add(registroComplemento);
                        }
                    }
                    if (registro.Year == 2019 && registro.Mes == 5 && registro.Poliza == 162 && registro.TP == "03" && registro.Linea == 4 && registro.Cta == 1211)
                    {
                        if (registroComplemento.Year == 2019 && registroComplemento.Mes == 5 && registroComplemento.Poliza == 162 && registroComplemento.TP == "03" &&
                            (
                                (registroComplemento.Linea == 8)
                            )
                           )
                        {
                            registro.FechaCFD = new DateTime(2019, 6, 1);
                            registro.Monto += registroComplemento.Monto;
                            complementosEncontrados.Add(registroComplemento);
                        }
                    }
                    if (registro.Year == 2018 && registro.Mes == 12 && registro.Poliza == 68 && registro.TP == "03" && registro.Linea == 6 && registro.Cta == 1215)
                    {
                        if (registroComplemento.Year == 2018 && registroComplemento.Mes == 12 && registroComplemento.Poliza == 68 && registroComplemento.TP == "03" && registro.Cta == 1215 &&
                            (
                                (registroComplemento.Linea == 8) ||
                                (registroComplemento.Linea == 10) ||
                                (registroComplemento.Linea == 12)
                            )
                           )
                        {
                            registro.Monto += registroComplemento.Monto;
                            complementosEncontrados.Add(registroComplemento);
                        }
                    }
                    if (registro.Year == 2018 && registro.Mes == 12 && registro.Poliza == 1558 && registro.TP == "07" && registro.Linea == 5 && registro.Cta == 1215)
                    {
                        if (registroComplemento.Year == 2018 && registroComplemento.Mes == 12 && registroComplemento.Poliza == 1551 && registroComplemento.TP == "07" && registro.Cta == 1215 &&
                            (
                                (registroComplemento.Linea == 5)
                            )
                           )
                        {
                            registro.Monto += registroComplemento.Monto;
                            complementosEncontrados.Add(registroComplemento);
                        }
                    }
                    if (registro.Year == 2018 && registro.Mes == 12 && registro.Poliza == 2477 && registro.TP == "07" && registro.Linea == 8 && registro.Cta == 1215)
                    {
                        if (registroComplemento.Year == 2018 && registroComplemento.Mes == 12 && registroComplemento.Poliza == 2477 && registroComplemento.TP == "07" && registro.Cta == 1215 &&
                            (
                                (registroComplemento.Linea == 12)
                            )
                           )
                        {
                            registro.Monto += registroComplemento.Monto;
                            complementosEncontrados.Add(registroComplemento);
                        }
                    }
                    if (registro.Year == 2019 && registro.Mes == 11 && registro.Poliza == 2326 && registro.TP == "07" && registro.Linea == 3 && registro.Cta == 1215)
                    {
                        if (registroComplemento.Year == 2019 && registroComplemento.Mes == 11 && registroComplemento.Poliza == 2327 && registroComplemento.TP == "07" && registro.Cta == 1215 &&
                            (
                                (registroComplemento.Linea == 3)
                            )
                           )
                        {
                            registro.Monto += registroComplemento.Monto;
                            complementosEncontrados.Add(registroComplemento);
                        }
                    }
                    if (registro.Year == 2018 && registro.Mes == 12 && registro.Poliza == 59 && registro.TP == "03" && registro.Linea == 1 && registro.Cta == 1210)
                    {
                        if (registroComplemento.Year == 2018 && registroComplemento.Mes == 12 && registroComplemento.Poliza == 59 && registroComplemento.TP == "03" && registro.Cta == 1210 &&
                            (
                                (registroComplemento.Linea == 14) ||
                                (registroComplemento.Linea == 15)
                            )
                           )
                        {
                            registro.Monto += registroComplemento.Monto;
                            complementosEncontrados.Add(registroComplemento);
                        }
                    }
                    if (registro.Year == 2019 && registro.Mes == 2 && registro.Poliza == 51 && registro.TP == "05" && registro.Linea == 3 && registro.Cta == 1210)
                    {
                        if (registroComplemento.Year == 2019 && registroComplemento.Mes == 2 && registroComplemento.Poliza == 51 && registroComplemento.TP == "05" && registro.Cta == 1210 &&
                            (
                                (registroComplemento.Linea == 5)
                            )
                           )
                        {
                            registro.Monto = -944315.53M;
                            complementosEncontrados.Add(registroComplemento);
                        }
                    }
                    if (registro.Year == 2019 && registro.Mes == 2 && registro.Poliza == 2371 && registro.TP == "07" && registro.Linea == 3 && registro.Cta == 1210)
                    {
                        if (registroComplemento.Year == 2019 && registroComplemento.Mes == 3 && registroComplemento.Poliza == 1917 && registroComplemento.TP == "07" && registro.Cta == 1210 &&
                            (
                                (registroComplemento.Linea == 3)
                            )
                           )
                        {
                            registro.Monto += registroComplemento.Monto;
                            complementosEncontrados.Add(registroComplemento);
                        }
                    }
                    //if (registro.Cta == 1210 && registro.Year == 2023 && registro.Mes == 6 && registro.Poliza == 36 && registro.TP == "05" && registro.Linea == 4)
                    //{
                    //    if (registroComplemento.Cta == 1210 && registroComplemento.Year == 2023 && registroComplemento.Mes == 6 && registroComplemento.Poliza == 167 && registroComplemento.TP == "03" && registroComplemento.Linea == 3)
                    //    {
                    //        registro.Monto += registroComplemento.Monto;
                    //        complementosEncontrados.Add(registroComplemento);
                    //    }
                    //}
                    if (fechaConsulta >= new DateTime(2022, 9, 1))
                    {
                        if (registro.Year == 2022 && registro.Mes == 6 && registro.Poliza == 193 && registro.TP == "03" && registro.Linea == 2 && registro.Cta == 1210)
                        {
                            if (registroComplemento.Year == 2022 && registroComplemento.Mes == 9 && registroComplemento.Poliza == 189 && registroComplemento.TP == "03" && registroComplemento.Linea == 13 && registro.Cta == 1210)
                            {
                                registro.Monto += registroComplemento.Monto;
                                registro.FechaCFD = registroComplemento.FechaCFD;
                                registro.FechaFactura = registroComplemento.FechaFactura;
                                registro.FechaPol = registroComplemento.FechaPol;
                                complementosEncontrados.Add(registroComplemento);
                            }
                        }
                    }
                    //if (registro.Year == 2019 && registro.Mes == 9 && registro.Poliza == 68 && registro.TP == "05" && registro.Linea == 498 && registro.Cta == 1210)
                    //{
                    //    if (registroComplemento.Year == 2019 && registroComplemento.Mes == 7 && registroComplemento.Poliza == 10 && registroComplemento.TP == "05" && registroComplemento.Linea == 78 && registroComplemento.Cta == 1210)
                    //    {
                    //        registro.Monto += registroComplemento.Monto;
                    //        complementosEncontrados.Add(registroComplemento);
                    //    }
                    //}
                    if (registro.Cta == 1210 && registro.Year == 2023 && registro.Mes == 1 && registro.Poliza == 195 && registro.TP == "03" && registro.Linea == 10)
                    {
                        if (registroComplemento.Cta == 1210 && registroComplemento.Year == 2023 && registroComplemento.Mes == 1 && registroComplemento.Poliza == 195 && registroComplemento.TP == "03" && registroComplemento.Linea == 11)
                        {
                            registro.Monto += registroComplemento.Monto;
                            complementosEncontrados.Add(registroComplemento);
                        }
                    }
                    if (registroComplemento.Year == 2019 && registroComplemento.Mes == 2 && registroComplemento.Poliza == 51 && registroComplemento.TP == "05" && registroComplemento.Linea == 6 && registroComplemento.Cta == 1210)
                    {
                        complementosEncontrados.Add(registroComplemento);
                    }
                    if (registroComplemento.Year == 2019 && registroComplemento.Mes == 4 && registroComplemento.Poliza == 9 && registroComplemento.TP == "05" && registroComplemento.Linea == 3 && registroComplemento.Cta == 1210)
                    {
                        complementosEncontrados.Add(registroComplemento);
                    }
                    if (registroComplemento.Year == 2019 && registroComplemento.Mes == 4 && registroComplemento.Poliza == 16 && registroComplemento.TP == "05" && registroComplemento.Linea == 1 && registroComplemento.Cta == 1210)
                    {
                        complementosEncontrados.Add(registroComplemento);
                    }
                    if (registroComplemento.Year == 2019 && registroComplemento.Mes == 4 && registroComplemento.Poliza == 4 && registroComplemento.TP == "05" && registroComplemento.Linea == 336 && registroComplemento.Cta == 1210)
                    {
                        complementosEncontrados.Add(registroComplemento);
                    }
                    if (registroComplemento.Year == 2020 && registroComplemento.Mes == 9 && registroComplemento.Poliza == 210 && registroComplemento.TP == "03" && registroComplemento.Linea == 1 && registroComplemento.Cta == 1210)
                    {
                        complementosEncontrados.Add(registroComplemento);
                    }
                    if (registroComplemento.Year == 2020 && registroComplemento.Mes == 9 && registroComplemento.Poliza == 57 && registroComplemento.TP == "05" && registroComplemento.Linea == 11 && registroComplemento.Cta == 1210)
                    {
                        complementosEncontrados.Add(registroComplemento);
                    }
                    if (registroComplemento.Year == 2020 && registroComplemento.Mes == 10 && registroComplemento.Poliza == 217 && registroComplemento.TP == "03" && (registroComplemento.Linea == 19 || registroComplemento.Linea == 20) && registroComplemento.Cta == 1210)
                    {
                        complementosEncontrados.Add(registroComplemento);
                    }
                    if (registroComplemento.Year == 2021 && registroComplemento.Mes == 2 && registroComplemento.Poliza == 76 && registroComplemento.TP == "05" && registroComplemento.Linea == 9 && registroComplemento.Cta == 1210)
                    {
                        complementosEncontrados.Add(registroComplemento);
                    }
                    if (registroComplemento.Year == 2021 && registroComplemento.Mes == 2 && registroComplemento.Poliza == 179 && registroComplemento.TP == "05" && registroComplemento.Linea == 124 && registroComplemento.Cta == 1210)
                    {
                        complementosEncontrados.Add(registroComplemento);
                    }
                    if (registroComplemento.Year == 2021 && registroComplemento.Mes == 2 && registroComplemento.Poliza == 233 && registroComplemento.TP == "03" && registroComplemento.Linea == 11 && registroComplemento.Cta == 1210)
                    {
                        complementosEncontrados.Add(registroComplemento);
                    }
                    if (registroComplemento.Year == 2021 && registroComplemento.Mes == 3 && registroComplemento.Poliza == 60 && registroComplemento.TP == "05" && registroComplemento.Linea == 6 && registroComplemento.Cta == 1220)
                    {
                        complementosEncontrados.Add(registroComplemento);
                    }
                    if (registroComplemento.Year == 2021 && registroComplemento.Mes == 3 && registroComplemento.Poliza == 139 && registroComplemento.TP == "05" && registroComplemento.Linea == 9 && registroComplemento.Cta == 1220)
                    {
                        complementosEncontrados.Add(registroComplemento);
                    }
                    if (registroComplemento.Year == 2021 && registroComplemento.Mes == 3 && registroComplemento.Poliza == 206 && registroComplemento.TP == "05" && registroComplemento.Linea == 11 && registroComplemento.Cta == 1220)
                    {
                        complementosEncontrados.Add(registroComplemento);
                    }
                    if (registroComplemento.Year == 2021 && registroComplemento.Mes == 3 && registroComplemento.Poliza == 280 && registroComplemento.TP == "03" && registroComplemento.Cta == 1220)
                    {
                        complementosEncontrados.Add(registroComplemento);
                    }
                    if (registroComplemento.Year == 2022 && registroComplemento.Mes == 9 && registroComplemento.Poliza == 40 && registroComplemento.TP == "05" && registroComplemento.Cta == 1210 && registroComplemento.Linea == 561)
                    {
                        complementosEncontrados.Add(registroComplemento);
                    }
                    if (registroComplemento.Year == 2022 && registroComplemento.Mes == 9 && registroComplemento.Poliza == 40 && registroComplemento.TP == "05" && registroComplemento.Cta == 1210 && registroComplemento.Linea == 561)
                    {
                        complementosEncontrados.Add(registroComplemento);
                    }
                    if (registroComplemento.Year == 2022 && registroComplemento.Mes == 9 && registroComplemento.Poliza == 189 && registroComplemento.TP == "03" && registroComplemento.Cta == 1210 && registroComplemento.Linea == 7)
                    {
                        complementosEncontrados.Add(registroComplemento);
                    }
                    //if (registroComplemento.Year == 2022 && registroComplemento.Mes == 9 && registroComplemento.Poliza == 189 && registroComplemento.TP == "03" && registroComplemento.Cta == 1210 && registroComplemento.Linea == 9)
                    //{
                    //    complementosEncontrados.Add(registroComplemento);
                    //}
                    if (registroComplemento.Year == 2022 && registroComplemento.Mes == 9 && registroComplemento.Poliza == 18 && registroComplemento.TP == "05" && registroComplemento.Cta == 1210 && registroComplemento.Linea == 7)
                    {
                        complementosEncontrados.Add(registroComplemento);
                    }
                    //SE MODIFICO MONTO ORIGINAL DE LA ALTA Y SE ELIMINO LOS AJUSTES
                    //2021-4-194-03-4
                    //if (registroComplemento.Year == 2021 && registroComplemento.Mes == 4 && registroComplemento.Poliza == 194 && registroComplemento.TP == "03" && registroComplemento.Linea == 4 && registroComplemento.Cta == 1210)
                    //{
                    //    complementosEncontrados.Add(registroComplemento);
                    //}
                    //2021-4-194-03-2
                    if (registroComplemento.Year == 2021 && registroComplemento.Mes == 4 && registroComplemento.Poliza == 194 && registroComplemento.TP == "03" && registroComplemento.Linea == 2 && registroComplemento.Cta == 1210)
                    {
                        complementosEncontrados.Add(registroComplemento);
                    }
                    //2021-4-84-05-3
                    //if (registroComplemento.Year == 2021 && registroComplemento.Mes == 4 && registroComplemento.Poliza == 84 && registroComplemento.TP == "05" && registroComplemento.Linea == 3 && registroComplemento.Cta == 1210)
                    //{
                    //    complementosEncontrados.Add(registroComplemento);
                    //}
                    //2021-4-236-03-2
                    if (registroComplemento.Year == 2021 && registroComplemento.Mes == 4 && registroComplemento.Poliza == 236 && registroComplemento.TP == "03" && registroComplemento.Linea == 2 && registroComplemento.Cta == 1210)
                    {
                        complementosEncontrados.Add(registroComplemento);
                    }
                    //2021-4-236-03-1
                    //if (registroComplemento.Year == 2021 && registroComplemento.Mes == 4 && registroComplemento.Poliza == 236 && registroComplemento.TP == "03" && registroComplemento.Linea == 1 && registroComplemento.Cta == 1210)
                    //{
                    //    complementosEncontrados.Add(registroComplemento);
                    //}
                    //EXCEPCIONES FIN

                    if (
                        1 == 2
                        //registro.Cta == registroComplemento.Cta &&
                        //registro.Scta == registroComplemento.Scta &&
                        //registro.Sscta == registroComplemento.Sscta &&
                        //registro.TM == 2 && registroComplemento.TM == 2 &&
                        //registro.Concepto == registroComplemento.Concepto &&
                        //registro.Concepto.Contains("FINANCIAMIENTO") && registroComplemento.Concepto.Contains("FINANCIAMIENTO") &&
                        //(registro.Monto == registroComplemento.Monto || (financiamientosEncontrados.Count > 0 && financiamientosEncontrados[0].Monto == registroComplemento.Monto)) &&
                        //registro != registroComplemento
                       )
                    {
                        //registro.Monto += registroComplemento.Monto;
                        //financiamientosEncontrados.Add(registroComplemento);
                    }
                    else
                    {
                        if (
                            registro.Cta == registroComplemento.Cta &&
                            registro.TM == 2 && registroComplemento.TM == 2 &&
                            registro.Referencia == registroComplemento.Referencia &&
                            registro.Cc == registroComplemento.Cc &&
                            registro.Mes <= registroComplemento.Mes &&
                            registroComplemento.Concepto.Contains("COMPL") &&
                            registro != registroComplemento
                           )
                        {
                            registro.Monto += registroComplemento.Monto;
                            complementosEncontrados.Add(registroComplemento);
                        }
                        else
                        {
                            if (
                                registro.Cta == registroComplemento.Cta &&
                                registro.Scta == registroComplemento.Scta &&
                                registro.Sscta == registroComplemento.Sscta &&
                                registro.TM == 2 && registroComplemento.TM == 2 &&
                                registro.Cc == registroComplemento.Cc &&
                                registro.Monto < 0 && registroComplemento.Monto < 0 &&
                                (
                                    registroComplemento.Referencia.Contains("AJUSTE") ||
                                    registroComplemento.Concepto.Contains("AJUSTE")
                                )
                               )
                            {
                                registro.Monto += registroComplemento.Monto;
                                complementosEncontrados.Add(registroComplemento);
                            }
                        }
                    }

                    if (
                        registro.Cta == registroComplemento.Cta &&
                        registro.Scta == registroComplemento.Scta &&
                        registro.Sscta == registroComplemento.Sscta &&
                        (registro.TM == 2 && registroComplemento.TM == 4) &&
                        registro.Referencia == registroComplemento.Referencia //&&
                        //(registroComplemento.Year.ToString() + registroComplemento.Mes.ToString() != "20205")
                       )
                    {
                        var match = catMaqDep.FirstOrDefault(x => x.Poliza.Cuenta == registro.Cta && x.Poliza.Año == registro.Year && x.Poliza.Mes == registro.Mes && x.Poliza.Poliza == registro.Poliza && x.Poliza.TP == registro.TP && x.Poliza.Linea == registro.Linea);
                        if (match != null)
                        {
                            continue;
                        }

                        registro.Monto += registroComplemento.Monto;
                        complementosEncontrados.Add(registroComplemento);
                        if (registro.Monto == 0)
                        {
                            complementosEncontrados.Add(registro);
                        }
                    }

                    if (
                        registro.Cta == registroComplemento.Cta &&
                        registro.Monto == registroComplemento.Monto * -1 &&
                        registro.TM == 1 && registroComplemento.TM == 3 &&
                        registro.Year == registroComplemento.Year &&
                        registro.Mes == registroComplemento.Mes &&
                        registro.Poliza == registroComplemento.Poliza &&
                        registro.Referencia == "REC" && registroComplemento.Referencia == "REC" &&
                        registro.Cc == registroComplemento.Cc
                       )
                    {
                        if (
                            catMaqDep
                                .Any(x =>
                                    x.Poliza.Año == registroComplemento.Year &&
                                    x.Poliza.Mes == registroComplemento.Mes &&
                                    x.Poliza.Poliza == registroComplemento.Poliza &&
                                    x.Poliza.TP == registroComplemento.TP &&
                                    x.Poliza.Linea == registroComplemento.Linea)
                            )
                        {

                        }
                        else
                        {
                            //2021-4-236-03-2
                            if ((registroComplemento.Poliza == 295 && registroComplemento.Linea == 1 && registroComplemento.Year == 2021 && registroComplemento.Mes == 3 && registroComplemento.TP == "03")/* ||
                            (registroComplemento.Poliza == 236 && registroComplemento.Linea == 2 && registroComplemento.Year == 2021 && registroComplemento.Mes == 4 && registroComplemento.TP == "03")*/ ||
                                (registroComplemento.Year == 2021 && registroComplemento.Mes == 12 && registroComplemento.Poliza == 281 && registroComplemento.TP == "03" && registroComplemento.Linea == 5) ||
                                (registroComplemento.Year == 2021 && registroComplemento.Mes == 12 && registroComplemento.Poliza == 281 && registroComplemento.TP == "03" && registroComplemento.Linea == 6) ||
                                (registroComplemento.Year == 2021 && registroComplemento.Mes == 12 && registroComplemento.Poliza == 281 && registroComplemento.TP == "03" && registroComplemento.Linea == 7) ||
                                (registroComplemento.Year == 2021 && registroComplemento.Mes == 12 && registroComplemento.Poliza == 281 && registroComplemento.TP == "03" && registroComplemento.Linea == 8) ||
                                (registroComplemento.Year == 2022 && registroComplemento.Mes == 3 && registroComplemento.Poliza == 321 && registroComplemento.TP == "03" && registroComplemento.Linea == 2) || //2022-3-321-03-2
                                (registroComplemento.Year == 2022 && registroComplemento.Mes == 3 && registroComplemento.Poliza == 326 && registroComplemento.TP == "03" && registroComplemento.Linea == 1) || //2022-3-326-03-1
                                (registroComplemento.Year == 2022 && registroComplemento.Mes == 4 && registroComplemento.Poliza == 52 && registroComplemento.TP == "05" && registroComplemento.Linea == 32) ||
                                (registroComplemento.Year == 2022 && registroComplemento.Mes == 8 && registroComplemento.Poliza == 223 && registroComplemento.TP == "03" && registroComplemento.Cta == 1210))  //2022-4-52-05-32
                            {
                                var nada = 0; //EXCEPCION
                            }
                            else
                            {
                                complementosEncontrados.Add(registroComplemento);
                                complementosEncontrados.Add(registro);
                            }
                        }
                    }
                }

                lista_sc_movpol_auxiliar.RemoveAll(x => financiamientosEncontrados.Contains(x));
                lista_sc_movpol_auxiliar.RemoveAll(x => complementosEncontrados.Contains(x));

                registrosAEliminar.AddRange(financiamientosEncontrados);
                registrosAEliminar.AddRange(complementosEncontrados);

                financiamientosEncontrados.RemoveRange(0, financiamientosEncontrados.Count);
                complementosEncontrados.RemoveRange(0, complementosEncontrados.Count);
            }

            lista_sc_movpol.RemoveAll(x => registrosAEliminar.Contains(x));
        }

        private Dictionary<string, object> getAreasCuentaEnkontrol()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                OdbcConsultaDTO odbcAreaCuenta = new OdbcConsultaDTO();

                odbcAreaCuenta.consulta = string.Format(@"SELECT DISTINCT
                                                            (CAST(area  AS varchar(4)) + '-' + CAST(cuenta AS varchar(4))) AS areaCuenta, descripcion, area, cuenta
                                                          FROM
                                                            si_area_cuenta
                                                          ORDER BY area, cuenta");

                List<ActivoFijoAreasCuentaEnkontrolDTO> sc_area_cuenta= _contextEnkontrol.Select<ActivoFijoAreasCuentaEnkontrolDTO>(EnkontrolAmbienteEnum.Prod, odbcAreaCuenta);

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, sc_area_cuenta);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, ex.Message);
            }

            return resultado;
        }

        /// <summary>
        /// Se obtienen los saldos de activos y depreciación para comparar lo calculado por el sistema con contabilidad.
        /// Se obtiene el saldo/depreciación inicial del año desde sc_salcont_cc
        /// Se obtiene el saldo/depreciación del durante el año desde sc_movpol
        /// </summary>
        /// <param name="añoFin">la consulta se realizara hasta el año que se envie</param>
        /// <param name="mesFin">la consulta se realizara hasta el mes del año que se envie</param>
        /// <param name="cuenta">la consutla se realizara para la cuenta que se envie</param>
        /// <returns></returns>
        private Dictionary<string, object> getInfoContabilidad(int añoFin, int mesFin, int cuenta)
        {
            var r = new Dictionary<string, object>();

            //Consultamos conjunto de cta/scta/sscta de las cuentas de activos y sus cuentas de depreciación
            #region Conjunto Cta/Scta/Sscta
            var relacionCuentas = _context.tblC_AF_RelacionesCuentaAño.Where
                (w =>
                    w.Estatus &&
                    (
                        (
                            w.CuentaMovimientoId != null &&
                            w.Cuenta.Cuenta == cuenta &&
                            w.Subcuenta.Cuenta.TipoCuentaId == (int)AFTipoCuentaEnum.Depreciacion
                        ) ||
                        (
                            w.CuentaMovimientoId == null &&
                            w.Subcuenta.Cuenta.TipoCuentaId == (int)AFTipoCuentaEnum.Movimiento &&
                            w.Subcuenta.Cuenta.Cuenta == cuenta
                        )
                    )
                ).ToList();
            #endregion

            //Obtenemos información de Enkontrol
            #region Saldos
            var query_saldos_sc_salcont_cc = new OdbcConsultaDTO();

            query_saldos_sc_salcont_cc.consulta = string.Format
            (
                @"SELECT
                    *
                FROM
                    sc_salcont_cc
                WHERE
                    cta = ? AND
                    year = ? AND
                    scta = 0 AND
                    sscta = 0"
            );

            query_saldos_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
            {
                nombre = "cta",
                tipo = OdbcType.Int,
                valor = cuenta
            });
            query_saldos_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
            {
                nombre = "year",
                tipo = OdbcType.Int,
                valor = añoFin
            });

            var saldos_sc_salcont_cc = _contextEnkontrol.Select<ActivoFijoSaldosContablesDTO>(_productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, query_saldos_sc_salcont_cc);
            r.Add("saldos_sc_salcont_cc", saldos_sc_salcont_cc);

            var where_saldos_sc_movpol = "";

            var relacionCuentasSaldos = relacionCuentas.Where(w => w.Subcuenta.Cuenta.TipoCuentaId == (int)AFTipoCuentaEnum.Movimiento).ToList();

            foreach (var ctas in relacionCuentasSaldos)
            {
                where_saldos_sc_movpol += "(MOV.scta = ? AND MOV.sscta = ?)";

                if (ctas != relacionCuentasSaldos.Last())
                {
                    where_saldos_sc_movpol += " OR ";
                }
            }

            var query_saldos_sc_movpol = new OdbcConsultaDTO();

            query_saldos_sc_movpol.consulta = string.Format
            (
                @"SELECT
                    MOV.year,
                    MOV.mes,
                    MOV.poliza,
                    MOV.tp,
                    MOV.linea,
                    MOV.cta,
                    MOV.scta,
                    MOV.sscta,
                    MOV.tm,
                    MOV.referencia,
                    MOV.cc,
                    MOV.concepto,
                    MOV.monto,
                    POL.fechapol,
                    CC.descripcion
                FROM
                    sc_movpol AS MOV
                INNER JOIN
                    sc_polizas AS POL
                    ON
                        MOV.year = POL.year AND
                        MOV.mes = POL.mes AND
                        MOV.poliza = POL.poliza AND
                        MOV.tp = POL.tp
                INNER JOIN
                    cc AS CC
                    ON
                        MOV.cc = CC.cc
                WHERE
                    MOV.cta = ? AND
                    MOV.year = ? AND
                    MOV.mes <= ? AND
                    (
                        {0}
                    )",
                    where_saldos_sc_movpol
            );

            query_saldos_sc_movpol.parametros.Add(new OdbcParameterDTO
            {
                nombre = "cta",
                tipo = OdbcType.Int,
                valor = cuenta
            });
            query_saldos_sc_movpol.parametros.Add(new OdbcParameterDTO
            {
                nombre = "year",
                tipo = OdbcType.Int,
                valor = añoFin
            });
            query_saldos_sc_movpol.parametros.Add(new OdbcParameterDTO
            {
                nombre = "mes",
                tipo = OdbcType.Int,
                valor = mesFin
            });

            foreach (var ctas in relacionCuentasSaldos)
            {
                query_saldos_sc_movpol.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "scta",
                    tipo = OdbcType.Int,
                    valor = ctas.Subcuenta.Subcuenta
                });
                query_saldos_sc_movpol.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "sscta",
                    tipo = OdbcType.Int,
                    valor = ctas.Subcuenta.SubSubcuenta
                });
            }

            var saldos_sc_movpol = _contextEnkontrol.Select<sc_movpolDTO>(_productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, query_saldos_sc_movpol);
            r.Add("saldos_sc_movpol", saldos_sc_movpol);
            #endregion

            #region Depreciación
            var where_depreciacion_salcont_cc = "";

            var relacionCuentasDepreciacion = relacionCuentas.Where(w => w.Subcuenta.Cuenta.TipoCuentaId == (int)AFTipoCuentaEnum.Depreciacion).ToList();

            if (relacionCuentasDepreciacion.Count == 0)
            {
                r.Add("depreciacion_sc_salcont_cc", new List<ActivoFijoSaldosContablesDTO>());
                r.Add("depreciacion_sc_movpol", new List<sc_movpolDTO>());
            }
            else
            {
                foreach (var ctas in relacionCuentasDepreciacion)
                {
                    where_depreciacion_salcont_cc += "(scta = ? AND sscta = ?)";

                    if (ctas != relacionCuentasDepreciacion.Last())
                    {
                        where_depreciacion_salcont_cc += " OR ";
                    }
                }

                var query_depreciacion_sc_salcont_cc = new OdbcConsultaDTO();

                query_depreciacion_sc_salcont_cc.consulta = string.Format
                (
                    @"SELECT
                        *
                    FROM
                        sc_salcont_cc
                    WHERE
                        cta = ? AND
                        year = ? AND
                        (
                            {0}
                        )",
                        where_depreciacion_salcont_cc
                );

                query_depreciacion_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "cta",
                    tipo = OdbcType.Int,
                    valor = relacionCuentasDepreciacion.First().Subcuenta.Cuenta.Cuenta
                });
                query_depreciacion_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "year",
                    tipo = OdbcType.Int,
                    valor = añoFin
                });

                foreach (var ctas in relacionCuentasDepreciacion)
                {
                    query_depreciacion_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "scta",
                        tipo = OdbcType.Int,
                        valor = ctas.Subcuenta.Subcuenta
                    });
                    query_depreciacion_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "sscta",
                        tipo = OdbcType.Int,
                        valor = ctas.Subcuenta.SubSubcuenta
                    });
                }

                var depreciacion_sc_salcont_cc = _contextEnkontrol.Select<ActivoFijoSaldosContablesDTO>(_productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, query_depreciacion_sc_salcont_cc);
                r.Add("depreciacion_sc_salcont_cc", depreciacion_sc_salcont_cc);

                var where_depreciacion_sc_movpol = "";

                foreach (var ctas in relacionCuentasDepreciacion)
                {
                    where_depreciacion_sc_movpol += "(MOV.scta = ? AND MOV.sscta = ?)";

                    if (ctas != relacionCuentasDepreciacion.Last())
                    {
                        where_depreciacion_sc_movpol += " OR ";
                    }
                }

                var query_depreciacion_sc_movpol = new OdbcConsultaDTO();

                query_depreciacion_sc_movpol.consulta = string.Format
                (
                    @"SELECT
                        MOV.year,
                        MOV.mes,
                        MOV.poliza,
                        MOV.tp,
                        MOV.linea,
                        MOV.cta,
                        MOV.scta,
                        MOV.sscta,
                        MOV.tm,
                        MOV.referencia,
                        MOV.cc,
                        MOV.concepto,
                        MOV.monto,
                        POL.fechapol,
                        CC.descripcion
                    FROM
                        sc_movpol AS MOV
                    INNER JOIN
                        sc_polizas AS POL
                        ON
                            MOV.year = POL.year AND
                            MOV.mes = POL.mes AND
                            MOV.poliza = POL.poliza AND
                            MOV.tp = POL.tp
                    INNER JOIN
                        cc AS CC
                        ON
                            MOV.cc = CC.cc
                    WHERE
                        MOV.cta = ? AND
                        MOV.year = ? AND
                        MOV.mes <= ? AND
                        (
                            {0}
                        )",
                        where_depreciacion_sc_movpol
                );

                query_depreciacion_sc_movpol.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "cta",
                    tipo = OdbcType.Int,
                    valor = relacionCuentasDepreciacion.First().Subcuenta.Cuenta.Cuenta
                });
                query_depreciacion_sc_movpol.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "year",
                    tipo = OdbcType.Int,
                    valor = añoFin
                });
                query_depreciacion_sc_movpol.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "mes",
                    tipo = OdbcType.Int,
                    valor = mesFin
                });

                foreach (var ctas in relacionCuentasDepreciacion)
                {
                    query_depreciacion_sc_movpol.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "scta",
                        tipo = OdbcType.Int,
                        valor = ctas.Subcuenta.Subcuenta
                    });
                    query_depreciacion_sc_movpol.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "sscta",
                        tipo = OdbcType.Int,
                        valor = ctas.Subcuenta.SubSubcuenta
                    });
                }

                var depreciacion_sc_movpol = _contextEnkontrol.Select<sc_movpolDTO>(_productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, query_depreciacion_sc_movpol);
                r.Add("depreciacion_sc_movpol", depreciacion_sc_movpol);
            }
            #endregion

            r.Add(SUCCESS, true);

            return r;
        }

        private List<sc_movpolDTO> getInfoCuentaMovimiento(int añoFin, int mesFin, int cuenta)
        {
            if (añoFin < 2021)
            {
                return new List<sc_movpolDTO>();
            }
            else
            {
                if (añoFin == 2021 && mesFin < 4)
                {
                    return new List<sc_movpolDTO>();
                }
                else
                {
                    var relacionCuenta = _context.tblC_AF_RelacionesCuentaAño.Where
                        (w =>
                            w.Estatus &&
                            w.Año <= añoFin &&
                            w.CuentaMovimientoId == null &&
                            w.Subcuenta.Cuenta.TipoCuentaId == (int)AFTipoCuentaEnum.Movimiento &&
                            w.Subcuenta.Cuenta.Cuenta == cuenta
                        ).ToList();

                    var where_sc_movpol = "";

                    foreach (var relCuenta in relacionCuenta)
                    {
                        where_sc_movpol += "(MOV.scta = ? AND MOV.sscta = ?)";

                        if (relCuenta != relacionCuenta.Last())
                        {
                            where_sc_movpol += " OR ";
                        }
                    }

                    var query_sc_movpol = new OdbcConsultaDTO();

                    query_sc_movpol.consulta = string.Format
                    (
                        @"SELECT
                            MOV.year,
                            MOV.mes,
                            MOV.poliza,
                            MOV.tp,
                            MOV.linea,
                            MOV.cta,
                            MOV.scta,
                            MOV.sscta,
                            MOV.tm,
                            MOV.referencia,
                            MOV.cc,
                            MOV.concepto,
                            MOV.monto,
                            POL.fechapol,
                            CC.descripcion AS ccDescripcion
                        FROM
                            sc_movpol AS MOV
                        INNER JOIN
                            sc_polizas AS POL
                            ON
                                POL.year = MOV.year AND
                                POL.mes = MOV.mes AND
                                POL.poliza = MOV.poliza AND
                                POL.tp = MOV.tp
                        INNER JOIN
                            cc AS CC
                            ON
                                CC.cc = MOV.cc
                        WHERE
                            MOV.year = ? AND
                            MOV.mes <= ? AND
                            MOV.cta = ? AND
                            (
                                {0}
                            )",
                            where_sc_movpol
                    );

                    query_sc_movpol.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "year",
                        tipo = OdbcType.Int,
                        valor = añoFin
                    });
                    query_sc_movpol.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "mes",
                        tipo = OdbcType.Int,
                        valor = mesFin
                    });
                    query_sc_movpol.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "cta",
                        tipo = OdbcType.Int,
                        valor = cuenta
                    });
                    foreach (var relCuenta in relacionCuenta)
                    {
                        query_sc_movpol.parametros.Add(new OdbcParameterDTO
                        {
                            nombre = "scta",
                            tipo = OdbcType.Int,
                            valor = relCuenta.Subcuenta.Subcuenta
                        });
                        query_sc_movpol.parametros.Add(new OdbcParameterDTO
                        {
                            nombre = "sscta",
                            tipo = OdbcType.Int,
                            valor = relCuenta.Subcuenta.SubSubcuenta
                        });
                    }

                    var sc_movpol = _contextEnkontrol.Select<sc_movpolDTO>(_productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, query_sc_movpol);

                    return sc_movpol;
                }
            }
        }

        private Dictionary<string, object> getInfoCuentas(int añoActual, int mes, List<int> cuentas, bool verDetalle, string noEconomico = null)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

            try
            {
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora && !string.IsNullOrEmpty(noEconomico))
                {
                    var queryCC_Economico = new OdbcConsultaDTO();
                    queryCC_Economico.consulta = "SELECT cc FROM cc WHERE descripcion = ?";
                    queryCC_Economico.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "descripcion",
                        tipo = OdbcType.NVarChar,
                        valor = noEconomico
                    });
                    var cc_economico = _contextEnkontrol.Select<dynamic>(vSesiones.sesionAmbienteEnkontrolAdm, queryCC_Economico).FirstOrDefault();
                    if (cc_economico != null)
                    {
                        noEconomico = (string)cc_economico.cc;
                    }
                }


                OdbcConsultaDTO odbcMovPol = new OdbcConsultaDTO();
                OdbcConsultaDTO odbcSalCont = new OdbcConsultaDTO();
                OdbcConsultaDTO odbcDepContFromSalCont = new OdbcConsultaDTO();
                OdbcConsultaDTO odbcDepCont = new OdbcConsultaDTO();

                List<sc_movpolDTO> sc_movpol = null;

                if (1==2/*vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan && añoActual <= 2020 && mes <= 12*/)
                {
                    sc_movpol = new List<sc_movpolDTO>();
                }
                else
                {
                    odbcMovPol.consulta = string.Format(@"SELECT DISTINCT
                                                        FAC.fecha AS fechaFactura, FAC.cfd_fecha AS fechaCFD, CC.descripcion AS AreaCuenta,
                                                        MOV.year, MOV.mes, MOV.Poliza, MOV.tp, MOV.Linea, MOV.Cta, MOV.scta, MOV.sscta, MOV.Digito, MOV.tm,
                                                        MOV.Referencia, MOV.Cc, MOV.Concepto, MOV.Monto, MOV.ITM, POL.fechapol, FAC.factura,
                                                        MOV.area AS Area, MOV.cuenta_oc AS Cuenta_OC,
                                                        (SELECT TOP 1 descripcion FROM si_area_cuenta WHERE area = MOV.area AND cuenta = MOV.cuenta_oc) AS AreaCuentaDescripcion
                                                      FROM
                                                        sc_movpol AS MOV
                                                      INNER JOIN
                                                        sc_polizas AS POL ON MOV.year = POL.year AND MOV.mes = POL.mes AND MOV.poliza = POL.poliza AND MOV.tp = POL.tp
                                                      LEFT JOIN
                                                        sp_movprov AS FAC ON MOV.year = FAC.year AND MOV.mes = FAC.mes AND MOV.poliza = FAC.poliza AND MOV.tp = FAC.tp AND FAC.es_factura = 'S'
                                                      LEFT JOIN CC AS CC ON MOV.cc = CC.cc
                                                      WHERE {2}
                                                        {0} AND MOV.cta IN {1}
                                                      ORDER BY POL.fechapol", vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? "((MOV.year > 2018 AND MOV.year < ?) OR (MOV.year = ? AND MOV.mes <= ? AND ((MOV.year > 2020) OR (MOV.year = 2020 AND MOV.mes in (11, 12)))))" : "((MOV.year >= 2018 AND MOV.year < ?) OR (MOV.year = ? AND MOV.mes <= ?))", cuentas.ToParamInValue(), !string.IsNullOrEmpty(noEconomico) ? "MOV.cc = '" + noEconomico + "' AND " : "");
                                                      //ORDER BY POL.fechapol", vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? añoActual == 2020 ? "(MOV.year BETWEEN ? AND ? AND MOV.mes BETWEEN 11 AND ?) " : "((MOV.year = 2020 AND MOV.mes BETWEEN 11 AND 12) OR (MOV.year BETWEEN 2021 AND ?) OR (MOV.year = ? AND MOV.mes <= ?))" : "((MOV.year >= 2018 AND MOV.year < ?) OR (MOV.year = ? AND MOV.mes <= ?))", cuentas.ToParamInValue());
                    //MOV.cta IN {0} AND ((MOV.year >= 2018 AND MOV.year < ?) OR (MOV.year = ? AND MOV.mes <= ?))", cuentas.ToParamInValue());((MOV.year > 2020) OR (MOV.year = 2020 AND MOV.mes in (11, 12)))

                    if (1 == 2/*vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora*/)
                    {
                        var fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        var fechaTemp = fechaActual.AddMonths(-3);
                        var añoAnterior = fechaActual.Year;
                        var mesAnterior = 1;

                        if (fechaTemp.Year < fechaActual.Year)
                        {
                            añoAnterior = fechaTemp.Year;
                            mesAnterior = fechaTemp.Month;
                        }

                        var fechaConsulta = new DateTime(añoActual, mes, 1);

                        odbcMovPol.parametros.AddRange(cuentas.Select(cta => new OdbcParameterDTO()
                        {
                            nombre = "cta",
                            tipo = OdbcType.Int,
                            valor = cta
                        }).ToList());

                        if (añoAnterior < añoActual)
                        {
                            odbcMovPol.consulta = string.Format(@"
                                                                SELECT DISTINCT
                                                                    CC.descripcion AS AreaCuenta, MOV.year, MOV.mes, MOV.Poliza, MOV.tp, MOV.Linea,
                                                                    MOV.Cta, MOV.scta, MOV.sscta, MOV.Digito, MOV.tm, MOV.Referencia, MOV.Cc, MOV.Concepto,
                                                                    MOV.Monto, MOV.ITM, POL.fechapol, MOV.area AS Area, MOV.cuenta_oc AS Cuenta_OC
                                                                FROM
                                                                    sc_movpol AS MOV
                                                                INNER JOIN
                                                                    sc_polizas AS POL ON MOV.year = POL.year AND MOV.mes = POL.mes AND MOV.poliza = POL.poliza AND MOV.tp = POL.tp
                                                                LEFT JOIN
                                                                    CC AS CC ON MOV.cc = CC.cc
                                                                WHERE
                                                                    MOV.cta IN {0} AND
                                                                    (
                                                                        (MOV.year = ? AND MOV.mes >= ?) &&
                                                                        (MOV.year = ? AND MOV.mes <= ?)
                                                                    )
                                                                ORDER BY POL.fechapol", cuentas.ToParamInValue());
                            odbcMovPol.parametros.Add(new OdbcParameterDTO()
                            {
                                nombre = "year",
                                tipo = System.Data.Odbc.OdbcType.Int,
                                valor = añoAnterior
                            });
                            odbcMovPol.parametros.Add(new OdbcParameterDTO()
                            {
                                nombre = "mes",
                                tipo = System.Data.Odbc.OdbcType.Int,
                                valor = mesAnterior
                            });
                            odbcMovPol.parametros.Add(new OdbcParameterDTO()
                            {
                                nombre = "year",
                                tipo = System.Data.Odbc.OdbcType.Int,
                                valor = añoActual
                            });
                            odbcMovPol.parametros.Add(new OdbcParameterDTO()
                            {
                                nombre = "mes",
                                tipo = System.Data.Odbc.OdbcType.Int,
                                valor = mes
                            });
                        }
                        else
                        {
                            odbcMovPol.consulta = string.Format(@"
                                                                SELECT DISTINCT
                                                                    CC.descripcion AS AreaCuenta, MOV.year, MOV.mes, MOV.Poliza, MOV.tp, MOV.Linea,
                                                                    MOV.Cta, MOV.scta, MOV.sscta, MOV.Digito, MOV.tm, MOV.Referencia, MOV.Cc, MOV.Concepto,
                                                                    MOV.Monto, MOV.ITM, POL.fechapol, MOV.area AS Area, MOV.cuenta_oc AS Cuenta_OC
                                                                FROM
                                                                    sc_movpol AS MOV
                                                                INNER JOIN
                                                                    sc_polizas AS POL ON MOV.year = POL.year AND MOV.mes = POL.mes AND MOV.poliza = POL.poliza AND MOV.tp = POL.tp
                                                                LEFT JOIN
                                                                    CC AS CC ON MOV.cc = CC.cc
                                                                WHERE
                                                                    MOV.cta IN {0} AND
                                                                    (
                                                                        (MOV.year = ? AND MOV.mes <= ?)
                                                                    )
                                                                ORDER BY POL.fechapol", cuentas.ToParamInValue());
                            odbcMovPol.parametros.Add(new OdbcParameterDTO()
                            {
                                nombre = "year",
                                tipo = System.Data.Odbc.OdbcType.Int,
                                valor = añoActual
                            });
                            odbcMovPol.parametros.Add(new OdbcParameterDTO()
                            {
                                nombre = "mes",
                                tipo = System.Data.Odbc.OdbcType.Int,
                                valor = mes
                            });
                        }
                    }
                    else
                    {
                        odbcMovPol.parametros.Add(new OdbcParameterDTO()
                        {
                            nombre = "year",
                            tipo = System.Data.Odbc.OdbcType.Int,
                            valor = añoActual
                        });

                        odbcMovPol.parametros.Add(new OdbcParameterDTO()
                        {
                            nombre = "year",
                            tipo = System.Data.Odbc.OdbcType.Int,
                            valor = añoActual
                        });

                        odbcMovPol.parametros.Add(new OdbcParameterDTO()
                        {
                            nombre = "mes",
                            tipo = System.Data.Odbc.OdbcType.Int,
                            valor = mes
                        });

                        odbcMovPol.parametros.AddRange(cuentas.Select(cta => new OdbcParameterDTO()
                        {
                            nombre = "cta",
                            tipo = OdbcType.Int,
                            valor = cta
                        }).ToList());
                    }

                    sc_movpol = _contextEnkontrol.Select<sc_movpolDTO>(EnkontrolAmbienteEnum.Prod, odbcMovPol);
                }

                if (!verDetalle)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, sc_movpol);
                    return resultado;
                }

                if (sc_movpol != null)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, sc_movpol);

                    switch (vSesiones.sesionEmpresaActual)
                    {
                        case (int)EmpresaEnum.Arrendadora:
                            odbcSalCont.consulta = string.Format
                                (@"SELECT
                                    *
                                   FROM
                                    sc_salcont_cc
                                   WHERE {2}
                                    {0} AND
                                    cta IN {1}",
                                 "(year >= 2018 AND year <= ?)",
                                 cuentas.ToParamInValue(), !string.IsNullOrEmpty(noEconomico) ? " cc = '" + noEconomico + "' AND" : ""
                                );
                            break;
                        case (int)EmpresaEnum.Construplan:
                            odbcSalCont.consulta = string.Format
                                (@"SELECT
                                    *
                                   FROM
                                    sc_salcont_cc
                                   WHERE
                                    year >= 1987 AND
                                    year <= ? AND
                                    cta in {0}",
                                 cuentas.ToParamInValue()
                                );

                            var cuentasDep = _context.tblC_AF_RelSubCuentas.Where(w => cuentas.Contains(w.Cuenta.Cuenta) && w.EsCuentaDepreciacion && !w.Excluir && w.Año <= añoActual && w.Estatus).ToList();
                            odbcDepContFromSalCont.consulta = "SELECT * FROM sc_salcont_cc WHERE (";
                            foreach (var relSubCuenta in cuentasDep)
                            {
                                odbcDepContFromSalCont.consulta += "(cta = ? AND scta = ? AND sscta = ?)";
                                if (relSubCuenta.Id != cuentasDep.Last().Id)
                                {
                                    odbcDepContFromSalCont.consulta += " OR ";
                                }
                                odbcDepContFromSalCont.parametros.Add(new OdbcParameterDTO()
                                {
                                    nombre = "cta",
                                    tipo = OdbcType.Int,
                                    valor = relSubCuenta.CuentaDepreciacion.Value
                                });
                                odbcDepContFromSalCont.parametros.Add(new OdbcParameterDTO()
                                {
                                    nombre = "scta",
                                    tipo = OdbcType.Int,
                                    valor = relSubCuenta.Subcuenta
                                });
                                odbcDepContFromSalCont.parametros.Add(new OdbcParameterDTO()
                                {
                                    nombre = "sscta",
                                    tipo = OdbcType.Int,
                                    valor = relSubCuenta.SubSubcuenta
                                });
                            }
                            odbcDepContFromSalCont.consulta += ")";
                            var depContFromSalCont = _contextEnkontrol.Select<ActivoFijoSaldosContablesDTO>(EnkontrolAmbienteEnum.Prod, odbcDepContFromSalCont);
                            resultado.Add("depSalCont", depContFromSalCont);
                            break;
                    }
                    

                    odbcSalCont.parametros.Add(new OdbcParameterDTO()
                    {
                        nombre = "year",
                        tipo = OdbcType.Int,
                        valor = añoActual
                    });

                    odbcSalCont.parametros.AddRange(cuentas.Select(cta => new OdbcParameterDTO()
                    {
                        nombre = "cta",
                        tipo = OdbcType.Int,
                        valor = cta
                    }).ToList());

                    List<ActivoFijoSaldosContablesDTO> sc_salcont_cc = _contextEnkontrol.Select<ActivoFijoSaldosContablesDTO>(EnkontrolAmbienteEnum.Prod, odbcSalCont);

                    if (sc_salcont_cc != null)
                    {
                        resultado.Add("sc_salcont_cc", sc_salcont_cc);

                        odbcDepCont.consulta = string.Format(@"SELECT DISTINCT
                                                                FAC.fecha AS fechaFactura, FAC.cfd_fecha AS fechaCFD, CC.descripcion AS AreaCuenta,
                                                                MOV.year, MOV.mes, MOV.Poliza, MOV.tp, MOV.Linea, MOV.Cta, MOV.scta, MOV.sscta, MOV.Digito, MOV.tm,
                                                                MOV.Referencia, MOV.Cc, MOV.Concepto, MOV.Monto, MOV.ITM, POL.fechapol, FAC.factura
                                                               FROM
                                                                sc_movpol AS MOV
                                                               INNER JOIN
                                                                sc_polizas AS POL ON MOV.year = POL.year AND MOV.mes = POL.mes AND MOV.poliza = POL.poliza AND MOV.tp = POL.tp
                                                               LEFT JOIN
                                                                sp_movprov AS FAC ON MOV.year = FAC.year AND MOV.mes = FAC.mes AND MOV.poliza = FAC.poliza AND MOV.tp = FAC.tp AND FAC.es_factura = 'S'
                                                               LEFT JOIN CC AS CC ON MOV.cc = CC.cc
                                                               {0}",
                                                                   vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? "WHERE MOV.cta = 1250 AND ((MOV.year >= 2020 AND MOV.year < ?) OR (MOV.year = ? AND MOV.mes <= ?))" : "WHERE MOV.cta = 1250 AND ((MOV.year >= 2018 AND MOV.year < ?) OR (MOV.year = ? AND MOV.mes <= ?))" + (!string.IsNullOrEmpty(noEconomico) ? " AND MOV.cc = '" + noEconomico + "'" : ""));

                        odbcDepCont.parametros.Add(new OdbcParameterDTO()
                        {
                            nombre = "year",
                            tipo = OdbcType.Int,
                            valor = añoActual
                        });

                        odbcDepCont.parametros.Add(new OdbcParameterDTO()
                        {
                            nombre = "year",
                            tipo = OdbcType.Int,
                            valor = añoActual
                        });

                        odbcDepCont.parametros.Add(new OdbcParameterDTO()
                        {
                            nombre = "mes",
                            tipo = OdbcType.Int,
                            valor = mes
                        });

                        List<sc_movpolDTO> sc_movpolDep = _contextEnkontrol.Select<sc_movpolDTO>(EnkontrolAmbienteEnum.Prod, odbcDepCont);

                        if (sc_movpolDep != null)
                        {
                            resultado.Add("sc_movpolDep", sc_movpolDep);
                        }
                        else
                        {
                            resultado[SUCCESS] = false;
                            resultado[MESSAGE] = "No se encontró información en enkontrol de la depreciación contable";
                        }
                    }
                    else
                    {
                        resultado[SUCCESS] = false;
                        resultado[MESSAGE] = "No se encontró información en enkontrol del saldo contable";
                    }
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "No se encontró información en enkontrol");
                }
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al consultar la base de datos de enkontrol. " + ex.ToString());
            }

            return resultado;
        }
        
        public Dictionary<string, object> GetDepreciacionCuenta()
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

            try
            {
                List<ActivoFijoDepreciacionCuentasDTO> DepreciacionCuentas = _context.tblC_AF_Cuentas.Where(x => x.Estatus).Select(m => new ActivoFijoDepreciacionCuentasDTO()
                {
                    Id = m.Id,
                    Cuenta = m.Cuenta,
                    Descripcion = m.Descripcion,
                    MesesDeDepreciacion = m.MesesDeDepreciacion,
                    PorcentajeDepreciacion = m.PorcentajeDepreciacion
                }).ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, DepreciacionCuentas);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Error: " + ex.ToString());
            }
            return resultado;
        }

        public Dictionary<string, object> ModificarDepreciacionCuenta(List<ActivoFijoDepreciacionCuentasDTO> depCuentas)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                foreach (var depCta in depCuentas)
                {
                    tblC_AF_Cuentas cta = _context.tblC_AF_Cuentas.FirstOrDefault(x => x.Id == depCta.Id);
                    cta.PorcentajeDepreciacion = depCta.PorcentajeDepreciacion / 100;
                    cta.MesesDeDepreciacion = depCta.MesesDeDepreciacion;

                    _context.SaveChanges();
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Error: " + ex.ToString());
            }

            return resultado;
        }
        #endregion

        #region Catalogo de depreciación de maquinas
        public Respuesta CalcularEnviosACosto()
        {
            var r = new Respuesta();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                    {
                        var fechaHasta = DateTime.Now;

                        var cuentaMayor = _context.tblC_AF_Cuenta.FirstOrDefault(f => f.Estatus && f.Cuenta == 1210);

                        var lstCuentas = new List<int> { cuentaMayor.Cuenta };

                        var depreciacionArmada = construirDetalles(fechaHasta, lstCuentas, false);

                        if ((bool)depreciacionArmada[SUCCESS])
                        {
                            var depArmada = depreciacionArmada[ITEMS] as List<ActivoFijoDetalleCuentaDTO>;

                            var depArmadaClean = new List<ActivoFijoDetalleDTO>();

                            foreach (var detalle in depArmada.First().Detalles)
                            {
                                if ((detalle.FechaBaja != null && detalle.FechaBaja.Value < new DateTime(fechaHasta.Year, fechaHasta.Month, 1)) || (detalle.FechaCancelacion != null && !detalle.EsOverhaul)) { continue; }
                                if (detalle.MesesDepreciadosAñoAnterior + detalle.MesesDepreciadosAñoActual > detalle.MesesMaximoDepreciacion) { continue; }
                                if (detalle.FechaInicioDepreciacion > new DateTime(fechaHasta.Year, fechaHasta.Month, DateTime.DaysInMonth(fechaHasta.Year, fechaHasta.Month))) { continue; }
                                if (detalle.EsOverhaul && (detalle.MesesDepreciadosAñoAnterior + detalle.MesesDepreciadosAñoActual) == 0) { continue; }
                                if (detalle.DepreciacionTerminadaPorMeses) { continue; }
                                if (!detalle.EsOverhaul) { continue; }
                                if (detalle.ValorEnLibros == 0) { continue; }
                                if (new DateTime(detalle.FechaInicioDepreciacion.Year, detalle.FechaInicioDepreciacion.Month, 1) == new DateTime(fechaHasta.Year, fechaHasta.Month, 1)) { continue; }
                                if (detalle.FechaCancelacion != null && new DateTime(detalle.FechaCancelacion.Value.Year, detalle.FechaCancelacion.Value.Month, 1) < new DateTime(fechaHasta.Year, fechaHasta.Month, 1) && detalle.DepreciacionMensual >= 0) { continue; }
                                depArmadaClean.Add(detalle);
                            }

                            var CalculosGuardadosAnteriormente = _context.tblC_AF_EnviarCosto.Where(w => !w.enviaACosto);

                            foreach (var item in CalculosGuardadosAnteriormente)
                            {
                                item.estatus = false;
                            }
                            _context.SaveChanges();

                            foreach (var depMaquina in depArmadaClean.Where(w => w.EsOverhaul && w.IdCatMaquina != null && w.IdCatMaquina.Value > 0).GroupBy(g => g.IdCatMaquina))
                            {
                                foreach (var insumo in depMaquina)
                                {
                                    if (!string.IsNullOrEmpty(insumo.Descripcion) && insumo.Descripcion.ToUpper().Contains("INSUMO: "))
                                    {
                                        var arreglo_insumo = insumo.Descripcion.Trim().Split(new char[] { ' ' });

                                        if (arreglo_insumo.Length > 0)
                                        {
                                            insumo.Insumo = arreglo_insumo[1];
                                        }
                                    }
                                }

                                var operacionRelacionEquipoInsumo = RelacionEquipoInsumo(depMaquina.Key.Value);

                                var depMaquinaTemporal = depMaquina.Where(w => !string.IsNullOrEmpty(w.Insumo)).ToList();

                                if (operacionRelacionEquipoInsumo.Success)
                                {
                                    var partesMaquina = operacionRelacionEquipoInsumo.Value as List<ActivoFijoRelacionEquipoInsumoDTO>;

                                    foreach (var insumo in depMaquina.Where(w => !string.IsNullOrEmpty(w.Insumo)).GroupBy(g => g.Insumo))
                                    {
                                        var lstInsumosMismoSubconjunto = new List<ActivoFijoDetalleDTO>();

                                        if (partesMaquina != null)
                                        {
                                            var subconjuntoDelInsumo = partesMaquina.FirstOrDefault(w => w.insumo.Contains(insumo.Key));

                                            if (subconjuntoDelInsumo != null)
                                            {
                                                lstInsumosMismoSubconjunto.AddRange(insumo);

                                                var quitarDeTemporal = new List<ActivoFijoDetalleDTO>();
                                                foreach (var otroInsumo in depMaquinaTemporal.GroupBy(g => g.Insumo))
                                                {
                                                    if (otroInsumo.Key != insumo.Key)
                                                    {
                                                        var siHayOtraParteDelMismoSubconjunto = partesMaquina.FirstOrDefault(f => f.insumo.Contains(otroInsumo.Key) && f.subconjunto == subconjuntoDelInsumo.subconjunto);

                                                        if (siHayOtraParteDelMismoSubconjunto != null)
                                                        {
                                                            lstInsumosMismoSubconjunto.AddRange(otroInsumo);
                                                            quitarDeTemporal.AddRange(otroInsumo);
                                                        }
                                                    }
                                                }

                                                foreach (var _itemTemp in quitarDeTemporal)
                                                {
                                                    depMaquinaTemporal.Remove(_itemTemp);
                                                }

                                                if (subconjuntoDelInsumo.maximo < insumo.Count())
                                                {
                                                    var diferencia = insumo.Count() - subconjuntoDelInsumo.maximo;

                                                    var lstCostos = new List<tblC_AF_EnviarCosto>();

                                                    foreach (var _insACosto in lstInsumosMismoSubconjunto.OrderBy(o => o.Fecha))
                                                    {
                                                        if (diferencia == 0)
                                                        {
                                                            break;
                                                        }
                                                        diferencia--;

                                                        var enviarACosto = new tblC_AF_EnviarCosto();

                                                        //var ultimos4MiercolesDelMes = Ultimos4MiercolesDelMes(new DateTime(fechaHasta.Year, fechaHasta.Month, 1));
                                                        var ultimos4MiercolesDelMes = Ultimos4MartesDelMes(new DateTime(fechaHasta.Year, fechaHasta.Month, 1));
                                                        if (ultimos4MiercolesDelMes.Count == 5)
                                                        {
                                                            ultimos4MiercolesDelMes.RemoveAt(0);
                                                        }

                                                        var semanasDepDelMes = 0;
                                                        for (int i = 0; i < ultimos4MiercolesDelMes.Count; i++)
                                                        {
                                                            if (fechaHasta.Day < ultimos4MiercolesDelMes[i])
                                                            {
                                                                semanasDepDelMes = i + 1;
                                                                break;
                                                            }
                                                        }
                                                        if (semanasDepDelMes == 0)
                                                        {
                                                            semanasDepDelMes = 4;
                                                        }

                                                        enviarACosto.mesesMaximoDepreciacion = _insACosto.MesesMaximoDepreciacion;
                                                        enviarACosto.porcentajeDepreciacion = _insACosto.PorcentajeDepreciacion;
                                                        enviarACosto.mesesDepreciados = _insACosto.MesesDepreciadosAñoAnterior + _insACosto.MesesDepreciadosAñoActual;
                                                        enviarACosto.mesesFaltantes = enviarACosto.mesesMaximoDepreciacion - (enviarACosto.mesesDepreciados);
                                                        enviarACosto.semanasUltimoMesDep = semanasDepDelMes;
                                                        enviarACosto.monto = Math.Round(_insACosto.MOI + _insACosto.Altas + _insACosto.Overhaul, 2);
                                                        enviarACosto.depActual = semanasDepDelMes == 4 ? Math.Round(_insACosto.DepreciacionContableAcumulada, 2) : Math.Round((_insACosto.DepreciacionContableAcumulada - _insACosto.DepreciacionMensual) + ((_insACosto.DepreciacionMensual / 4) * semanasDepDelMes), 2);
                                                        enviarACosto.depFaltante = semanasDepDelMes == 4 ? Math.Round(_insACosto.ValorEnLibros, 2) : Math.Round((_insACosto.MOI + _insACosto.Altas + _insACosto.Overhaul) - enviarACosto.depActual, 2);
                                                        enviarACosto.descripcion = _insACosto.Descripcion;
                                                        enviarACosto.enviaACosto = false;
                                                        enviarACosto.fechaAlta = _insACosto.Fecha;
                                                        enviarACosto.fechaInicioDep = _insACosto.FechaInicioDepreciacion;
                                                        enviarACosto.idEconomico = _insACosto.IdCatMaquina.Value;
                                                        enviarACosto.cc = _insACosto.Cc;
                                                        enviarACosto.area = _insACosto.Area.Value;
                                                        enviarACosto.cuenta = _insACosto.Cuenta_OC.Value;
                                                        enviarACosto.polizaAlta = _insACosto.Poliza;
                                                        enviarACosto.polizaBaja = null;
                                                        enviarACosto.polizaCosto = null;
                                                        enviarACosto.estatus = true;

                                                        lstCostos.Add(enviarACosto);
                                                    }

                                                    _context.tblC_AF_EnviarCosto.AddRange(lstCostos);
                                                    _context.SaveChanges();
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    transaction.Rollback();
                                    r.Message = "ERROR AL CONSULTAR LA RELACION DE # DE PARTES EN EL EQUIPO: " + depMaquina.Key.Value + ". " + operacionRelacionEquipoInsumo.Message;
                                    return r;
                                }
                                
                            }

                            transaction.Commit();
                        }
                        else
                        {
                            r.Message = "ERROR AL CONSULTAR LA DEPRECIACIÓN";
                            return r;
                        }
                    }
                    else
                    {
                        //Hacer nada...
                    }

                    r.Success = true;
                    r.Message = "Ok";
                    r.Value = null;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    r.Message += ex.Message;
                }
            }

            return r;
        }

        public Respuesta GenerarPolizaCostoPorInsumo(int idCatMaqDepreciacion)
        {
            var r = new Respuesta();

            try
            {
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                {
                    var fechaHasta = DateTime.Now;

                    var cuentaMayor = _context.tblC_AF_Cuenta.FirstOrDefault(f => f.Estatus && f.Cuenta == 1210);

                    var lstCuentas = new List<int> { cuentaMayor.Cuenta };

                    var depreciacionArmada = construirDetalles(fechaHasta, lstCuentas, false);

                    var catMaqDep = _context.tblM_CatMaquinaDepreciacion.First(f => f.Id == idCatMaqDepreciacion);

                    if ((bool)depreciacionArmada[SUCCESS])
                    {
                        var depArmada = depreciacionArmada[ITEMS] as List<ActivoFijoDetalleCuentaDTO>;

                        var infoDepInsumo = depArmada.First().Detalles.FirstOrDefault
                            (f =>
                                f.AñoPol_alta == catMaqDep.Poliza.Año &&
                                f.MesPol_alta == catMaqDep.Poliza.Mes &&
                                f.PolPol_alta == catMaqDep.Poliza.Poliza &&
                                f.TpPol_alta == catMaqDep.Poliza.TP &&
                                f.LineaPol_alta == catMaqDep.Poliza.Linea
                            );

                        if (infoDepInsumo != null)
                        {
                            var enviarACosto = new tblC_AF_EnviarCosto();

                            //var ultimos4MiercolesDelMes = Ultimos4MiercolesDelMes(new DateTime(fechaHasta.Year, fechaHasta.Month, 1));
                            var ultimos4MiercolesDelMes = Ultimos4MartesDelMes(new DateTime(fechaHasta.Year, fechaHasta.Month, 1));

                            if (ultimos4MiercolesDelMes.Count == 5)
                            {
                                ultimos4MiercolesDelMes.RemoveAt(0);
                            }

                            var semanasDepDelMes = 0;
                            for (int i = 0; i < ultimos4MiercolesDelMes.Count; i++)
                            {
                                if (fechaHasta.Day < ultimos4MiercolesDelMes[i])
                                {
                                    semanasDepDelMes = i + 1;
                                    break;
                                }
                            }
                            if (semanasDepDelMes == 0)
                            {
                                semanasDepDelMes = 4;
                            }

                            enviarACosto.mesesMaximoDepreciacion = infoDepInsumo.MesesMaximoDepreciacion;
                            enviarACosto.porcentajeDepreciacion = infoDepInsumo.PorcentajeDepreciacion;
                            enviarACosto.mesesDepreciados = infoDepInsumo.MesesDepreciadosAñoAnterior + infoDepInsumo.MesesDepreciadosAñoActual;
                            enviarACosto.mesesFaltantes = enviarACosto.mesesMaximoDepreciacion - (enviarACosto.mesesDepreciados);
                            enviarACosto.semanasUltimoMesDep = semanasDepDelMes;
                            enviarACosto.monto = Math.Round(infoDepInsumo.MOI + infoDepInsumo.Altas + infoDepInsumo.Overhaul, 2);
                            enviarACosto.depActual = semanasDepDelMes == 4 ? Math.Round(infoDepInsumo.DepreciacionContableAcumulada, 2) : Math.Round((infoDepInsumo.DepreciacionContableAcumulada - infoDepInsumo.DepreciacionMensual) + ((infoDepInsumo.DepreciacionMensual / 4) * semanasDepDelMes), 2);
                            enviarACosto.depFaltante = semanasDepDelMes == 4 ? Math.Round(infoDepInsumo.ValorEnLibros, 2) : Math.Round((infoDepInsumo.MOI + infoDepInsumo.Altas + infoDepInsumo.Overhaul) - enviarACosto.depActual, 2);
                            enviarACosto.descripcion = infoDepInsumo.Descripcion;
                            enviarACosto.enviaACosto = false;
                            enviarACosto.fechaAlta = infoDepInsumo.Fecha;
                            enviarACosto.fechaInicioDep = infoDepInsumo.FechaInicioDepreciacion;
                            enviarACosto.idEconomico = infoDepInsumo.IdCatMaquina.Value;
                            enviarACosto.cc = infoDepInsumo.Cc;
                            enviarACosto.area = infoDepInsumo.Area.Value;
                            enviarACosto.cuenta = infoDepInsumo.Cuenta_OC.Value;
                            enviarACosto.polizaAlta = infoDepInsumo.Poliza;
                            enviarACosto.polizaBaja = null;
                            enviarACosto.polizaCosto = null;
                            enviarACosto.estatus = true;

                            var cuentaMaquinaria = _context.tblC_AF_Cuenta.First(f => f.Estatus && f.Cuenta == infoDepInsumo.Cuenta);
                            //ABONO
                            var cuentaBaja = _context.tblC_AF_RelacionesCuentaAño.FirstOrDefault
                                (f =>
                                    f.Estatus &&
                                    f.Subcuenta.CuentaId == cuentaMaquinaria.Id &&
                                    f.Subcuenta.Estatus &&
                                    f.Subcuenta.EsMaquinaria &&
                                    f.Subcuenta.EsOverhaul &&
                                    f.Subcuenta.Cuenta.TipoCuentaId == (int)AFTipoCuentaEnum.Movimiento &&
                                    f.Subcuenta.meses != null &&
                                    f.Subcuenta.meses.Value == infoDepInsumo.MesesMaximoDepreciacion &&
                                    f.Año == infoDepInsumo.Fecha.Year
                                );
                            //CARGO
                            var cuentaDep = _context.tblC_AF_RelacionesCuentaAño.FirstOrDefault
                                (f =>
                                    f.Estatus &&
                                    f.Subcuenta.Cuenta.TipoCuentaId == (int)AFTipoCuentaEnum.Depreciacion &&
                                    f.Cuenta.TipoCuentaId == cuentaMaquinaria.Id &&
                                    f.Cuenta.Cuenta == cuentaMaquinaria.Cuenta &&
                                    f.Subcuenta.Estatus &&
                                    f.Subcuenta.EsMaquinaria &&
                                    f.Subcuenta.EsOverhaul &&
                                    f.Subcuenta.meses != null &&
                                    f.Subcuenta.meses == infoDepInsumo.MesesMaximoDepreciacion &&
                                    f.Año == fechaHasta.Year
                                );
                            //CARGO COSTO
                            var cuentaCosto = _context.tblC_AF_RelacionesCuentaAño.FirstOrDefault
                                (f =>
                                    f.Estatus &&
                                    f.Cuenta.Id == cuentaMaquinaria.Id &&
                                    f.Año == fechaHasta.Year &&
                                    f.Subcuenta.Estatus &&
                                    f.Subcuenta.EsMaquinaria &&
                                    f.Subcuenta.EsOverhaul &&
                                    f.Subcuenta.Cuenta.TipoCuentaId == (int)AFTipoCuentaEnum.Costo &&
                                    f.Año == fechaHasta.Year
                                );

                            if (cuentaBaja != null && cuentaDep != null && cuentaCosto != null)
                            {
                                var polizaCaptura = new PolizaCapturaEnkontrolDTO();

                                polizaCaptura.EnviarCosto = enviarACosto;

                                var sc_poliza = new PolizaEnkontrolDTO();

                                sc_poliza.Year = fechaHasta.Year;
                                sc_poliza.Mes = fechaHasta.Month;
                                sc_poliza.Poliza = 1;
                                sc_poliza.TP = "03";
                                sc_poliza.FechaPol = fechaHasta.Date;
                                sc_poliza.Cargos = enviarACosto.depActual + enviarACosto.depFaltante;
                                sc_poliza.Abonos = enviarACosto.monto * -1;
                                sc_poliza.Concepto = cuentaCosto.Subcuenta.ConceptoDepreciacion;
                                sc_poliza.Generada = Convert.ToChar(Enum.GetName(typeof(AFModuloEnkontrolEnum), (int)AFModuloEnkontrolEnum.C));
                                sc_poliza.Status = Convert.ToChar(Enum.GetName(typeof(AFEstatusPolizaEnum), (int)AFEstatusPolizaEnum.C));
                                sc_poliza.Fecha_hora_crea = DateTime.Now;
                                sc_poliza.Usuario_Crea = 1;

                                polizaCaptura.Poliza = sc_poliza;

                                var linea = 0;

                                //ABONO = 0
                                //CARGO = 1
                                //COSTO = 2
                                for (int i = 0; i < 3; i++)
                                {
                                    linea++;

                                    var movimiento = new PolizaDetalleEnkontrolDTO();

                                    movimiento.Year = sc_poliza.Year;
                                    movimiento.Mes = sc_poliza.Mes;
                                    movimiento.Poliza = sc_poliza.Poliza;
                                    movimiento.TP = sc_poliza.TP;
                                    movimiento.Linea = linea;
                                    movimiento.Referencia = infoDepInsumo.Clave;
                                    movimiento.Concepto = "BAJA " + infoDepInsumo.Descripcion;
                                    movimiento.CC = infoDepInsumo.Cc;
                                    movimiento.IClave = 0;
                                    movimiento.ITM = 0;
                                    movimiento.Area = infoDepInsumo.Area.Value;
                                    movimiento.Cuenta_OC = infoDepInsumo.Cuenta_OC.Value;

                                    switch (i)
                                    {
                                        case 0:
                                            movimiento.Cta = cuentaMaquinaria.Cuenta;
                                            movimiento.Scta = cuentaBaja.Subcuenta.Subcuenta;
                                            movimiento.Sscta = cuentaBaja.Subcuenta.SubSubcuenta;
                                            movimiento.Digito = 7;
                                            movimiento.TM = 2;
                                            movimiento.Monto = enviarACosto.monto * -1;
                                            movimiento.EsBaja = true;
                                            break;
                                        case 1:
                                            movimiento.Cta = cuentaDep.Subcuenta.Cuenta.Cuenta;
                                            movimiento.Scta = cuentaDep.Subcuenta.Subcuenta;
                                            movimiento.Sscta = cuentaDep.Subcuenta.SubSubcuenta;
                                            movimiento.Digito = 0;
                                            movimiento.TM = 1;
                                            movimiento.Monto = enviarACosto.depActual;
                                            break;
                                        case 2:
                                            movimiento.Cta = cuentaCosto.Subcuenta.Cuenta.Cuenta;
                                            movimiento.Scta = cuentaCosto.Subcuenta.Subcuenta;
                                            movimiento.Sscta = cuentaCosto.Subcuenta.SubSubcuenta;
                                            movimiento.Digito = 6;
                                            movimiento.TM = 1;
                                            movimiento.Monto = enviarACosto.depFaltante;
                                            break;
                                    }

                                    polizaCaptura.Detalle.Add(movimiento);
                                }

                                r.Success = true;
                                r.Message = "Ok";
                                r.Value = polizaCaptura;
                            }
                            else
                            {
                                r.Message = "NO SE ENCONTRO INFORMACION DE LAS CUENTAS PARA GENERAR LA POLIZA";
                            }
                        }
                        else
                        {
                            r.Message = "NO SE ENCONTRO LA INFORMACION DE DEPRECIACION DEL INSUMO";
                        }
                    }
                    else
                    {
                        r.Message = (string)depreciacionArmada[MESSAGE];
                        return r;
                    }
                }
                else
                {
                    r.Message = "OPCION SOLO DISPONIBLE EN ARRENDADORA";
                }

                r.Success = true;
                r.Message = "Ok";
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }

        public Respuesta GenerarPolizaCosto(List<int> idsEnvioCosto)
        {
            var r = new Respuesta();

            try
            {
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                {
                    var costos = _context.tblC_AF_EnviarCosto.Where(w => idsEnvioCosto.Contains(w.id) && !w.enviaACosto && w.estatus).ToList();

                    if (costos.Count > 0)
                    {
                        var fechaHasta = DateTime.Now;
                        fechaHasta = fechaHasta.AddMonths(-1);
                        for (int i = DateTime.DaysInMonth(fechaHasta.Year, fechaHasta.Month); i > 0; i--)
                        {
                            if (new DateTime(fechaHasta.Year, fechaHasta.Month, i).DayOfWeek == DayOfWeek.Sunday)
                            {
                                continue;
                            }
                            else
                            {
                                fechaHasta = new DateTime(fechaHasta.Year, fechaHasta.Month, i);
                                break;
                            }
                        }

                        var mesesDepInvolucrados = costos.Select(m => m.mesesMaximoDepreciacion).Distinct().ToList();
                        var cuentaMaquinaria = _context.tblC_AF_Cuenta.First(f => f.Estatus && f.Cuenta == 1210);
                        //ABONO
                        var cuentasBaja = _context.tblC_AF_RelacionesCuentaAño.Where
                            (w =>
                                w.Estatus &&
                                w.Subcuenta.CuentaId == cuentaMaquinaria.Id &&
                                w.Subcuenta.Estatus &&
                                w.Subcuenta.EsMaquinaria &&
                                w.Subcuenta.EsOverhaul &&
                                w.Subcuenta.Cuenta.TipoCuentaId == (int)AFTipoCuentaEnum.Movimiento
                            ).ToList();
                        //CARGO
                        var cuentasDep = _context.tblC_AF_RelacionesCuentaAño.Where
                            (w =>
                                w.Estatus &&
                                w.Subcuenta.Cuenta.TipoCuentaId == (int)AFTipoCuentaEnum.Depreciacion &&
                                w.Cuenta.TipoCuentaId == cuentaMaquinaria.Id &&
                                w.Cuenta.Cuenta == cuentaMaquinaria.Cuenta &&
                                w.Subcuenta.Estatus &&
                                w.Subcuenta.EsMaquinaria &&
                                w.Subcuenta.EsOverhaul
                            ).ToList();
                        //CARGO COSTO
                        var cuentaCosto = _context.tblC_AF_RelacionesCuentaAño.FirstOrDefault
                            (w =>
                                w.Estatus &&
                                w.Cuenta.Id == cuentaMaquinaria.Id &&
                                w.Año == fechaHasta.Year &&
                                w.Subcuenta.Estatus &&
                                w.Subcuenta.EsMaquinaria &&
                                w.Subcuenta.EsOverhaul &&
                                w.Subcuenta.Cuenta.TipoCuentaId == (int)AFTipoCuentaEnum.Costo
                            );

                        if (cuentaCosto != null)
                        {
                            var polizaCaptura = new PolizaCapturaEnkontrolDTO();

                            polizaCaptura.idsEnvioCosto = idsEnvioCosto;

                            var sc_poliza = new PolizaEnkontrolDTO();

                            sc_poliza.Year = fechaHasta.Year;
                            sc_poliza.Mes = fechaHasta.Month;
                            sc_poliza.Poliza = 1;
                            sc_poliza.TP = "03";
                            sc_poliza.FechaPol = fechaHasta.Date;
                            sc_poliza.Cargos = costos.Sum(s => s.depActual + s.depFaltante);
                            sc_poliza.Abonos = costos.Sum(s => s.monto) * -1;
                            sc_poliza.Generada = Convert.ToChar(Enum.GetName(typeof(AFModuloEnkontrolEnum), (int)AFModuloEnkontrolEnum.C));
                            sc_poliza.Status = Convert.ToChar(Enum.GetName(typeof(AFEstatusPolizaEnum), (int)AFEstatusPolizaEnum.C));
                            sc_poliza.Fecha_hora_crea = DateTime.Now;
                            sc_poliza.Usuario_Crea = 13;

                            polizaCaptura.Poliza = sc_poliza;

                            var linea = 0;

                            foreach (var costo in costos)
                            {

                                //ABONO = 0
                                //CARGO = 1
                                //COSTO = 2
                                for (int i = 0; i < 3; i++)
                                {
                                    linea++;

                                    var movimiento = new PolizaDetalleEnkontrolDTO();

                                    movimiento.Year = sc_poliza.Year;
                                    movimiento.Mes = sc_poliza.Mes;
                                    movimiento.Poliza = sc_poliza.Poliza;
                                    movimiento.TP = sc_poliza.TP;
                                    movimiento.Linea = linea;
                                    movimiento.Referencia = costo.economico.noEconomico;
                                    movimiento.Concepto = "BAJA " + costo.descripcion;
                                    movimiento.CC = costo.cc;
                                    movimiento.IClave = 0;
                                    movimiento.ITM = 0;
                                    movimiento.Area = costo.area;
                                    movimiento.Cuenta_OC = costo.cuenta;

                                    switch (i)
                                    {
                                        case 0:
                                            movimiento.Cta = cuentaMaquinaria.Cuenta;
                                            movimiento.Scta = cuentasBaja.FirstOrDefault
                                                (f =>
                                                    f.Año == costo.fechaAlta.Year &&
                                                    f.Subcuenta.meses != null &&
                                                    f.Subcuenta.meses.Value == costo.mesesMaximoDepreciacion
                                                ).Subcuenta.Subcuenta;
                                            movimiento.Sscta = cuentasBaja.FirstOrDefault
                                                (f =>
                                                    f.Año == costo.fechaAlta.Year &&
                                                    f.Subcuenta.meses != null &&
                                                    f.Subcuenta.meses.Value == costo.mesesMaximoDepreciacion
                                                ).Subcuenta.SubSubcuenta;
                                            movimiento.Digito = 7;
                                            movimiento.TM = 2;
                                            movimiento.Concepto = "BAJA " + costo.descripcion;
                                            movimiento.Monto = costo.monto * -1;
                                            movimiento.EsBaja = true;
                                            movimiento.IdCosto = costo.id;
                                            break;
                                        case 1:
                                            movimiento.Cta = cuentasDep.FirstOrDefault
                                                (f =>
                                                    f.Año == fechaHasta.Year &&
                                                    f.Subcuenta.meses != null &&
                                                    f.Subcuenta.meses.Value == costo.mesesMaximoDepreciacion
                                                ).Subcuenta.Cuenta.Cuenta;
                                            movimiento.Scta = cuentasDep.FirstOrDefault
                                                (f =>
                                                    f.Año == fechaHasta.Year &&
                                                    f.Subcuenta.meses != null &&
                                                    f.Subcuenta.meses.Value == costo.mesesMaximoDepreciacion
                                                ).Subcuenta.Subcuenta;
                                            movimiento.Sscta = cuentasDep.FirstOrDefault
                                                (f =>
                                                    f.Año == fechaHasta.Year &&
                                                    f.Subcuenta.meses != null &&
                                                    f.Subcuenta.meses.Value == costo.mesesMaximoDepreciacion
                                                ).Subcuenta.SubSubcuenta;
                                            movimiento.Digito = 0;
                                            movimiento.TM = 1;
                                            movimiento.Monto = costo.depActual;
                                            break;
                                        case 2:
                                            movimiento.Cta = cuentaCosto.Subcuenta.Cuenta.Cuenta;
                                            movimiento.Scta = cuentaCosto.Subcuenta.Subcuenta;
                                            movimiento.Sscta = cuentaCosto.Subcuenta.SubSubcuenta;
                                            movimiento.Digito = 6;
                                            movimiento.TM = 1;
                                            movimiento.Monto = costo.depFaltante;
                                            break;
                                    }

                                    polizaCaptura.Detalle.Add(movimiento);
                                }
                            }

                            r.Success = true;
                            r.Message = "Ok";
                            r.Value = polizaCaptura;
                        }
                        else
                        {
                            r.Message = "NO SE ENCONTRO INFORMACION DE LA CUENTA DE COSTO";
                        }
                    }
                    else
                    {
                        r.Message = "NO SE ENCONTRARON DATOS PARA GENERAR POLIZA";
                    }
                }
                else
                {
                    r.Message = "ACCION SOLO DISPONIBLE EN ARRENDADORA";
                }
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }

        public Dictionary<string, object> RegistrarPolizaCosto(PolizaCapturaEnkontrolDTO infoPoliza)
        {
            var r = new Dictionary<string, object>();

            using (var transaction = _context.Database.BeginTransaction())
            {
                using (var connEk = ConexionEnkontrol())
                {
                    using (var transactionEk = connEk.BeginTransaction())
                    {
                        try
                        {
                            var nuevosRegistros = new List<ActivoFijoRegInfoDepDTO>();
                            var esIndividual = false;

                            var numPoliza = 0;

                            OdbcConsultaDTO odbcUltimaPoliza = new OdbcConsultaDTO();

                            odbcUltimaPoliza.consulta = "SELECT poliza FROM sc_polizas WHERE year = ? AND mes = ? AND tp = ?";

                            odbcUltimaPoliza.parametros.Add(new OdbcParameterDTO()
                            {
                                nombre = "year",
                                tipo = OdbcType.Int,
                                valor = infoPoliza.Poliza.Year
                            });

                            odbcUltimaPoliza.parametros.Add(new OdbcParameterDTO()
                            {
                                nombre = "mes",
                                tipo = OdbcType.Int,
                                valor = infoPoliza.Poliza.Mes
                            });

                            odbcUltimaPoliza.parametros.Add(new OdbcParameterDTO()
                            {
                                nombre = "tp",
                                tipo = OdbcType.Char,
                                valor = "03"
                            });

                            List<PolizaDTO> polizas = _contextEnkontrol.Select<PolizaDTO>(EnkontrolAmbienteEnum.Prod, odbcUltimaPoliza);
                            if (polizas.Count > 0)
                            {
                                numPoliza = polizas.OrderBy(o => o.Poliza).Last().Poliza + 1;
                            }
                            else
                            {
                                numPoliza = 1;
                            }

                            var insertPoliza = @"
                                                INSERT INTO
                                                    sc_polizas
                                                        (
                                                            year, mes, poliza, tp, fechapol, cargos, abonos, generada,
                                                            status, error, status_lock, fec_hora_movto, usuario_movto,
                                                            fecha_hora_crea, usuario_crea, status_carga_pol, concepto
                                                        )
                                                    VALUES
                                                        (
                                                            ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?
                                                        )
                                                ";

                            using (var cmd = new OdbcCommand(insertPoliza))
                            {
                                OdbcParameterCollection parameters = cmd.Parameters;
                                parameters.Clear();

                                parameters.Add("@year", OdbcType.Numeric).Value = infoPoliza.Poliza.Year;
                                parameters.Add("@mes", OdbcType.Numeric).Value = infoPoliza.Poliza.Mes;
                                parameters.Add("@poliza", OdbcType.Numeric).Value = numPoliza;
                                parameters.Add("@tp", OdbcType.Char).Value = "03";
                                parameters.Add("@fechapol", OdbcType.Date).Value = infoPoliza.Poliza.FechaPol;
                                parameters.Add("@cargos", OdbcType.Numeric).Value = infoPoliza.Poliza.Cargos;
                                parameters.Add("@abonos", OdbcType.Numeric).Value = infoPoliza.Poliza.Abonos;
                                parameters.Add("@generada", OdbcType.Char).Value = infoPoliza.Poliza.Generada;
                                parameters.Add("@status", OdbcType.Char).Value = infoPoliza.Poliza.Status;
                                parameters.Add("@error", OdbcType.VarChar).Value = string.Empty;
                                parameters.Add("@status_lock", OdbcType.Char).Value = 'N';
                                parameters.Add("@fec_hora_movto", OdbcType.DateTime).Value = infoPoliza.Poliza.Fecha_hora_crea;
                                parameters.Add("@usuario_movto", OdbcType.Char).Value = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? "151" : "290";
                                parameters.Add("@fecha_hora_crea", OdbcType.DateTime).Value = infoPoliza.Poliza.Fecha_hora_crea;
                                parameters.Add("@usuario_crea", OdbcType.Char).Value = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? "151" : "290";
                                parameters.Add("@status_carga_pol", OdbcType.VarChar).Value = DBNull.Value;
                                parameters.Add("@concepto", OdbcType.VarChar).Value = "BAJA DE ACTIVOS OVH " + infoPoliza.Poliza.Year;

                                cmd.Connection = transactionEk.Connection;
                                cmd.Transaction = transactionEk;
                                cmd.ExecuteNonQuery();
                            }

                            foreach (var costo in infoPoliza.Detalle)
                            {
                                var insertMovPol = @"
                                                    INSERT INTO
                                                        sc_movpol 
                                                            (
                                                                year, mes, poliza, tp, linea, cta, scta, sscta, digito, tm, referencia, cc, concepto,
                                                                monto, iclave, itm, st_par, orden_compra, numpro, area, cuenta_oc
                                                            )
                                                        VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
                                                    ";
                                using (var cmd = new OdbcCommand(insertMovPol))
                                {
                                    OdbcParameterCollection parameters = cmd.Parameters;
                                    parameters.Clear();

                                    parameters.Add("@year", OdbcType.Numeric).Value = costo.Year;
                                    parameters.Add("@mes", OdbcType.Numeric).Value = costo.Mes;
                                    parameters.Add("@poliza", OdbcType.Numeric).Value = numPoliza;
                                    parameters.Add("@tp", OdbcType.Char).Value = "03";
                                    parameters.Add("@linea", OdbcType.Numeric).Value = costo.Linea;
                                    parameters.Add("@cta", OdbcType.Numeric).Value = costo.Cta;
                                    parameters.Add("@scta", OdbcType.Numeric).Value = costo.Scta;
                                    parameters.Add("@sscta", OdbcType.Numeric).Value = costo.Sscta;
                                    parameters.Add("@digito", OdbcType.Numeric).Value = costo.Digito;
                                    parameters.Add("@tm", OdbcType.Numeric).Value = costo.TM;
                                    parameters.Add("@referencia", OdbcType.Char).Value = costo.Referencia;
                                    parameters.Add("@cc", OdbcType.Char).Value = costo.CC;
                                    parameters.Add("@concepto", OdbcType.Char).Value = costo.Concepto;
                                    parameters.Add("@monto", OdbcType.Numeric).Value = costo.Monto;
                                    parameters.Add("@iclave", OdbcType.Numeric).Value = costo.IClave;
                                    parameters.Add("@itm", OdbcType.Numeric).Value = costo.ITM;
                                    parameters.Add("@st_par", OdbcType.Char).Value = string.Empty;
                                    parameters.Add("@orden_compra", OdbcType.Numeric).Value = 0;
                                    parameters.Add("@numpro", OdbcType.Numeric).Value = 0;
                                    if (costo.Area != null && vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                                    {
                                        parameters.Add("@area", OdbcType.Numeric).Value = costo.Area.Value;
                                    }
                                    else
                                    {
                                        parameters.Add("@area", OdbcType.Numeric).Value = DBNull.Value;
                                    }
                                    if (costo.Area != null && vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                                    {
                                        parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = costo.Cuenta_OC.Value;
                                    }
                                    else
                                    {
                                        parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = DBNull.Value;
                                    }

                                    cmd.Connection = transactionEk.Connection;
                                    cmd.Transaction = transactionEk;
                                    cmd.ExecuteNonQuery();
                                }

                                if (costo.EsBaja)
                                {
                                    if (costo.IdCosto == 0)
                                    {
                                        if (infoPoliza.EnviarCosto != null)
                                        {
                                            infoPoliza.EnviarCosto.enviaACosto = true;
                                            infoPoliza.EnviarCosto.polizaBaja = infoPoliza.Poliza.Year.ToString() + "-" + infoPoliza.Poliza.Mes + "-" + "-" + numPoliza + "-" + infoPoliza.Poliza.TP + "-" + costo.Linea;
                                            infoPoliza.EnviarCosto.polizaCosto = infoPoliza.Poliza.Year.ToString() + "-" + infoPoliza.Poliza.Mes + "-" + "-" + numPoliza + "-" + infoPoliza.Poliza.TP;

                                            _context.tblC_AF_EnviarCosto.Add(infoPoliza.EnviarCosto);
                                            _context.SaveChanges();

                                            costo.IdCosto = infoPoliza.EnviarCosto.id;

                                            esIndividual = true;
                                        }
                                    }

                                    var EnvioCosto = _context.tblC_AF_EnviarCosto.First(f => f.id == costo.IdCosto);

                                    EnvioCosto.enviaACosto = true;
                                    EnvioCosto.polizaBaja = infoPoliza.Poliza.Year.ToString() + '-' + infoPoliza.Poliza.Mes + '-' + numPoliza + '-' + infoPoliza.Poliza.TP + "-" + costo.Linea;
                                    EnvioCosto.polizaCosto = infoPoliza.Poliza.Year.ToString() + '-' + infoPoliza.Poliza.Mes + '-' + numPoliza + '-' + infoPoliza.Poliza.TP;

                                    _context.SaveChanges();

                                    var _infoPolAlta = EnvioCosto.polizaAlta.Split(new char[] { '-' });

                                    var _anio = Convert.ToInt32(_infoPolAlta[0]);
                                    var _mes = Convert.ToInt32(_infoPolAlta[1]);
                                    var _pol = Convert.ToInt32(_infoPolAlta[2]);
                                    var _tp = _infoPolAlta[3];
                                    var _linea = Convert.ToInt32(_infoPolAlta[4]);

                                    var _anioBaja = infoPoliza.Poliza.Year;
                                    var _mesBaja = infoPoliza.Poliza.Mes;
                                    var _polBaja = numPoliza;
                                    var _tpBaja = infoPoliza.Poliza.TP;
                                    var _lineaBaja = costo.Linea;

                                    var polizaAlta = _context.tblM_CatMaquinaDepreciacion.FirstOrDefault
                                        (f =>
                                            f.Poliza.Año == _anio &&
                                            f.Poliza.Mes == _mes &&
                                            f.Poliza.Poliza == _pol &&
                                            f.Poliza.TP == _tp &&
                                            f.Poliza.Linea == _linea &&
                                            f.Poliza.Estatus
                                        );

                                    if (polizaAlta != null)
                                    {
                                        tblC_AF_PolizaAltaBaja polizaBaja = new tblC_AF_PolizaAltaBaja();
                                        tblM_CatMaquinaDepreciacion catMaqDepBaja = new tblM_CatMaquinaDepreciacion();

                                        polizaBaja.Año = _anioBaja;
                                        polizaBaja.Mes = _mesBaja;
                                        polizaBaja.Poliza = _polBaja;
                                        polizaBaja.TP = _tpBaja;
                                        polizaBaja.Linea = _lineaBaja;
                                        polizaBaja.TM = costo.TM;
                                        polizaBaja.Cuenta = costo.Cta;
                                        polizaBaja.Factura = "";
                                        polizaBaja.FechaMovimiento = infoPoliza.Poliza.FechaPol;
                                        polizaBaja.TipoActivo = 2;
                                        polizaBaja.Estatus = true;
                                        polizaBaja.FechaCreacion = DateTime.Now;
                                        polizaBaja.IdUsuarioCreacion = 13;
                                        polizaBaja.FechaModificacion = polizaBaja.FechaCreacion;
                                        polizaBaja.IdUsuarioModificacion = polizaBaja.IdUsuarioCreacion;

                                        _context.tblC_AF_PolizaAltaBaja.Add(polizaBaja);
                                        _context.SaveChanges();

                                        catMaqDepBaja.IdCatMaquina = EnvioCosto.idEconomico;
                                        catMaqDepBaja.IdPoliza = polizaBaja.Id;
                                        catMaqDepBaja.FechaInicioDepreciacion = null;
                                        catMaqDepBaja.PorcentajeDepreciacion = null;
                                        catMaqDepBaja.MesesTotalesDepreciacion = null;
                                        catMaqDepBaja.TipoDelMovimiento = (int)TipoDelMovimiento.O;
                                        catMaqDepBaja.IdPolizaReferenciaAlta = polizaAlta.IdPoliza;
                                        catMaqDepBaja.Estatus = true;
                                        catMaqDepBaja.FechaCreacion = DateTime.Now;
                                        catMaqDepBaja.IdUsuarioCreacion = 13;
                                        catMaqDepBaja.FechaModificacion = catMaqDepBaja.FechaCreacion;
                                        catMaqDepBaja.IdUsuarioModificacion = catMaqDepBaja.IdUsuarioCreacion;
                                        catMaqDepBaja.CapturaAutomatica = true;
                                        catMaqDepBaja.IdBajaCosto = EnvioCosto.id;

                                        _context.tblM_CatMaquinaDepreciacion.Add(catMaqDepBaja);
                                        _context.SaveChanges();

                                        var nuevoRegistro = new ActivoFijoRegInfoDepDTO();

                                        nuevoRegistro.IdCatMaquinaDepreciacion = catMaqDepBaja.Id;
                                        nuevoRegistro.Año = polizaBaja.Año;
                                        nuevoRegistro.Mes = polizaBaja.Mes;
                                        nuevoRegistro.Poliza = polizaBaja.Poliza;
                                        nuevoRegistro.TP = polizaBaja.TP;
                                        nuevoRegistro.Linea = polizaBaja.Linea;
                                        nuevoRegistro.TM = polizaBaja.TM;
                                        nuevoRegistro.Cuenta = polizaBaja.Cuenta;
                                        nuevoRegistro.Subcuenta = costo.Scta;
                                        nuevoRegistro.SubSubcuenta = costo.Sscta;
                                        nuevoRegistro.Monto = costo.Monto;
                                        nuevoRegistro.Concepto = polizaBaja.Concepto;
                                        nuevoRegistro.Factura = polizaBaja.Factura;
                                        nuevoRegistro.FechaFactura = polizaBaja.FechaMovimiento;
                                        nuevoRegistro.FechaMovimiento = polizaBaja.FechaMovimiento;
                                        nuevoRegistro.TipoActivo = polizaBaja.TipoActivo;
                                        nuevoRegistro.IdCatMaquina = catMaqDepBaja.IdCatMaquina;
                                        nuevoRegistro.FechaInicioDepreciacion = catMaqDepBaja.FechaInicioDepreciacion;
                                        nuevoRegistro.PorcentajeDepreciacion = catMaqDepBaja.PorcentajeDepreciacion;
                                        nuevoRegistro.MesesTotalesDepreciacion = catMaqDepBaja.MesesTotalesDepreciacion;
                                        nuevoRegistro.TipoDelMovimiento = catMaqDepBaja.TipoDelMovimiento;
                                        nuevoRegistro.CapturaPorSistema = true;
                                        nuevoRegistro.PolizaRefAlta = _anio + "-" + _mes + "-" + _pol + "-" + _tp + "-" + _linea;
                                        nuevoRegistro.Concepto = costo.Concepto;
                                        nuevoRegistro.AreaCuenta = costo.Area.Value + "-" + costo.Cuenta_OC.Value;

                                        nuevosRegistros.Add(nuevoRegistro);
                                    }
                                    else
                                    {
                                        r.Add(SUCCESS, false);
                                        r.Add(MESSAGE, "NO SE ENCONTRO LA POLIZA DE ALTA CON LA CUAL RELACIONAR LA BAJA: " + _anio + "-" + _mes + "-" + _pol + "-" + _tp + "-" + _linea);
                                        return r;
                                    }
                                }
                            }

                            transaction.Commit();
                            transactionEk.Commit();

                            var CCs = CCEnkontrol();

                            var reporte = new ReportePolizaDTO();
                            reporte.Poliza = infoPoliza.Poliza.Year + "-" + infoPoliza.Poliza.Mes + "-" + numPoliza + "-" + infoPoliza.Poliza.TP;
                            reporte.EsCC = true;
                            reporte.CCInicial = CCs.First().CC;
                            reporte.CCFinal = CCs.Last().CC;
                            reporte.PolizaInicial = numPoliza;
                            reporte.PolizaFinal = numPoliza;
                            reporte.TipoPolizaInicial = infoPoliza.Poliza.TP;
                            reporte.TipoPolizaFinal = infoPoliza.Poliza.TP;
                            reporte.PeriodoInicial = infoPoliza.Poliza.FechaPol;
                            reporte.PeriodoFinal = infoPoliza.Poliza.FechaPol;
                            reporte.ReporteResumido = false;
                            reporte.PolizaPorHoja = true;
                            reporte.IncluirFirmas = true;
                            reporte.Estatus = Enum.GetName(typeof(AFEstatusPolizaEnum), AFEstatusPolizaEnum.C);
                            reporte.Reviso = "CP. Jessica Galdean";
                            reporte.Autorizo = "CP. Arturo Sánchez";

                            r.Add(SUCCESS, true);
                            r.Add("Value", reporte);
                            r.Add("esIndividual", esIndividual);
                            r.Add("nuevosRegistros", nuevosRegistros);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            transactionEk.Rollback();

                            r.Add(SUCCESS, false);
                            r.Add(MESSAGE, ex.Message);
                        }
                    }
                }
            }

            return r;
        }

        public Dictionary<string, object> RelacionAutomaticaPolizas()
        {
            var r = new Dictionary<string, object>();

            var fechaHasta = DateTime.Now;
            fechaHasta = fechaHasta.AddMonths(-1);
            //var fechaHasta = new DateTime(2021, 02, 28);
            fechaHasta = fechaHasta <= new DateTime(2020, 1, 1) ? new DateTime(2020, 1, 1) : fechaHasta;
            
            //int añoActual = fechaHasta.Year;

            //List<int> añosAnteriores = new List<int>();
            //for (int año = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? 2018 : 2020; año < añoActual; año++) { añosAnteriores.Add(año); }
            //if (añosAnteriores.Count == 0) { añosAnteriores.Add(vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? 2017 : 2019); }

            var getCuentas = GetCuentas((int)TipoResultadoCuentasMaquinaria.CuentasMaquinaria);
            if (!(bool)getCuentas[SUCCESS]) { return getCuentas; }
            var cuentas = (List<int>)getCuentas[ITEMS];

            var obtenerDetalles = construirDetalles(fechaHasta, cuentas, false);

            if ((bool)obtenerDetalles[SUCCESS])
            {
                var detalles = obtenerDetalles[ITEMS] as List<ActivoFijoDetalleCuentaDTO>;

                List<tblM_CatMaquina> catMaquina = null;

                using (var ctx = new MainContext(EmpresaEnum.Arrendadora))
                {
                    using (var transactionArre = ctx.Database.BeginTransaction())
                    {
                        using (var transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                var lstRegistrosConDuplicados = new List<ActivoFijoDetalleDTO>();
                                var lstCostos = new List<tblC_AF_EnviarCosto>();

                                //catMaquina = ctx.tblM_CatMaquina.Where(w => !string.IsNullOrEmpty(w.noEconomico)).ToList();
                                catMaquina = _context.tblM_CatMaquina.Where(w => !string.IsNullOrEmpty(w.noEconomico)).ToList();

                                var exclusiones = _context.tblC_AF_PolizasExcluidasParaCapturaAutomatica.Where(x => x.Estatus).ToList();

                                foreach (var cuenta in detalles)
                                {
                                    #region calculosCosto
                                    if (cuenta.Cuenta == 1210)
                                    {
                                        //QUITAR EXCLUSIONES
                                        var lstTempDet = new List<ActivoFijoDetalleDTO>();
                                        foreach (var item in cuenta.Detalles)
                                        {
                                            if (exclusiones.Any(a => a.Año == item.AñoPol_alta && a.Mes == item.MesPol_alta && a.Poliza == item.PolPol_alta && a.TipoPoliza == item.TpPol_alta && a.Linea == item.LineaPol_alta))
                                            {
                                                continue;
                                            }
                                            lstTempDet.Add(item);
                                        }
                                        cuenta.Detalles = lstTempDet;
                                        //

                                        //PROCESO COSTO
                                        var todoEnviosACostosPendientes = _context.tblC_AF_EnviarCosto.Where(w => w.estatus && !w.enviaACosto).ToList();
                                        foreach (var item in todoEnviosACostosPendientes)
                                        {
                                            item.estatus = false;
                                        }
                                        _context.SaveChanges();

                                        var registrosARelacionar = new List<ActivoFijoDetalleDTO>();

                                        //REGISTROS QUE NO PUEDEN PASAR POR LA VALIDACION DE INSUMO POR NO TENER IDMAQUINA, NO SON OVH, O ALGO ASI
                                        foreach (var registro in cuenta.Detalles.Where(w => !w.EsOverhaul || w.IdCatMaquina == null || (w.IdCatMaquina != null && w.IdCatMaquina == 0)))
                                        {
                                            registrosARelacionar.Add(registro);
                                        }
                                        foreach (var registro in cuenta.Detalles.Where(w => (w.FechaBaja != null || w.FechaCancelacion != null) && w.EsOverhaul))
                                        {
                                            registrosARelacionar.Add(registro);
                                        }
                                        //

                                        //OBTENER INSUMOS
                                        foreach (var item in cuenta.Detalles.Where(w => w.IdCatMaquina != null && w.IdCatMaquina.Value > 0 && w.EsOverhaul))
                                        {
                                            if (!string.IsNullOrEmpty(item.Descripcion) && item.Descripcion.ToUpper().Contains("INSUMO: "))
                                            {
                                                var arreglo_insumo = item.Descripcion.Trim().Split(new char[] { ' ' });

                                                if (arreglo_insumo.Length > 0)
                                                {
                                                    item.Insumo = arreglo_insumo[1];
                                                }
                                            }
                                            else
                                            {
                                                registrosARelacionar.Add(item);
                                            }
                                        }
                                        //
                                        //DETECTAR DUPLICADOS SI EXISTE INSUMO NUEVO
                                        var nuevasPol = cuenta.Detalles.Where
                                            (w =>
                                                w.IdCatMaquina != null &&
                                                w.IdCatMaquina.Value > 0 &&
                                                w.EsOverhaul &&
                                                !string.IsNullOrEmpty(w.Insumo) &&
                                                !w.faltante &&
                                                w.FechaBaja == null &&
                                                w.FechaCancelacion == null &&
                                                !w.AltaSigoplan
                                            ).ToList();

                                        foreach (var gpMaq in cuenta.Detalles.Where
                                                                (w =>
                                                                    w.IdCatMaquina != null &&
                                                                    w.IdCatMaquina.Value > 0 &&
                                                                    w.EsOverhaul &&
                                                                    !string.IsNullOrEmpty(w.Insumo) &&
                                                                    !w.faltante &&
                                                                    w.FechaBaja == null &&
                                                                    w.FechaCancelacion == null &&
                                                                    w.AltaSigoplan
                                                                ).GroupBy(g => g.IdCatMaquina)
                                                )
                                        {
                                            var operacionRelacionEquipoInsumo = RelacionEquipoInsumo(gpMaq.Key.Value);

                                            var depMaquinaTemporal = gpMaq.ToList();

                                            if (operacionRelacionEquipoInsumo.Success)
                                            {
                                                var partesMaquina = operacionRelacionEquipoInsumo.Value as List<ActivoFijoRelacionEquipoInsumoDTO>;

                                                if (partesMaquina.Count > 0)
                                                {
                                                    var lstInsumosRevisados = new List<ActivoFijoRelacionEquipoInsumoDTO>();
                                                    foreach (var gpInsumo in gpMaq.GroupBy(g => g.Insumo))
                                                    {
                                                        var lstInsumosMismoSubconjunto = new List<ActivoFijoDetalleDTO>();

                                                        var subconjuntoDelInsumo = partesMaquina.FirstOrDefault(f => f.insumo.Contains(gpInsumo.Key));

                                                        if (subconjuntoDelInsumo != null)
                                                        {
                                                            if (lstInsumosRevisados.Contains(subconjuntoDelInsumo))
                                                            {
                                                                continue;
                                                            }

                                                            lstInsumosRevisados.Add(subconjuntoDelInsumo);

                                                            lstInsumosMismoSubconjunto.AddRange(gpInsumo);

                                                            var quitarDeTemporal = new List<ActivoFijoDetalleDTO>();
                                                            foreach (var otroInsumo in depMaquinaTemporal.GroupBy(g => g.Insumo))
                                                            {
                                                                if (otroInsumo.Key != gpInsumo.Key)
                                                                {
                                                                    var siHayOtraParteDelMismoSubconjunto = subconjuntoDelInsumo.insumo.Contains(otroInsumo.Key);

                                                                    if (siHayOtraParteDelMismoSubconjunto)
                                                                    {
                                                                        lstInsumosMismoSubconjunto.AddRange(otroInsumo);
                                                                        quitarDeTemporal.AddRange(otroInsumo);
                                                                    }
                                                                }
                                                            }

                                                            foreach (var _itemTemp in quitarDeTemporal)
                                                            {
                                                                depMaquinaTemporal.Remove(_itemTemp);
                                                            }

                                                            //SE BUSCAN SI HAY NUEVO POR RELACIONAR DE ESTE SUBCONJUNTO DE INSUMOS, SI NO HAY SE MANDAN DIRECTO A RELACIONAR
                                                            var coincidenciaConNuevaPol = nuevasPol.Where(w => subconjuntoDelInsumo.insumo.Contains(w.Insumo) && w.IdCatMaquina == gpMaq.Key).ToList();

                                                            foreach (var coincidencia in coincidenciaConNuevaPol)
                                                            {
                                                                coincidencia.NuevoInsumo = true;
                                                            }

                                                            if (coincidenciaConNuevaPol.Count == 0)
                                                            {
                                                                registrosARelacionar.AddRange(lstInsumosMismoSubconjunto);
                                                            }
                                                            else
                                                            {
                                                                lstInsumosMismoSubconjunto.AddRange(coincidenciaConNuevaPol);
                                                                foreach (var item in coincidenciaConNuevaPol)
                                                                {
                                                                    nuevasPol.Remove(item);
                                                                }

                                                                //SE QUITAN LOS QUE YA TIENEN BAJA/CANCELACION
                                                                var lstInsumosConBaja = new List<ActivoFijoDetalleDTO>();
                                                                foreach (var insSubconjunto in lstInsumosMismoSubconjunto)
                                                                {
                                                                    if (insSubconjunto.FechaBaja != null || insSubconjunto.FechaCancelacion != null || insSubconjunto.DepreciacionTerminadaPorMeses)
                                                                    {
                                                                        lstInsumosConBaja.Add(insSubconjunto);
                                                                    }
                                                                }

                                                                foreach (var conBaja in lstInsumosConBaja)
                                                                {
                                                                    lstInsumosMismoSubconjunto.Remove(conBaja);
                                                                    registrosARelacionar.Add(conBaja);
                                                                }
                                                                //

                                                                //SE VERIFICA SI HAY INSUMOS DE MAS
                                                                if (subconjuntoDelInsumo.maximo < lstInsumosMismoSubconjunto.Count)
                                                                {
                                                                    var diferencia = lstInsumosMismoSubconjunto.Count - subconjuntoDelInsumo.maximo;

                                                                    foreach (var _insACosto in lstInsumosMismoSubconjunto.OrderBy(o => o.Fecha))
                                                                    {
                                                                        if (diferencia == 0)
                                                                        {
                                                                            if (_insACosto.NuevoInsumo)
                                                                            {
                                                                                lstRegistrosConDuplicados.Add(_insACosto);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            diferencia--;

                                                                            var enviarACosto = new tblC_AF_EnviarCosto();

                                                                            //var ultimos4MiercolesDelMes = Ultimos4MiercolesDelMes(new DateTime(fechaHasta.Year, fechaHasta.Month, 1));
                                                                            var ultimos4MiercolesDelMes = Ultimos4MartesDelMes(new DateTime(fechaHasta.Year, fechaHasta.Month, 1));
                                                                            if (ultimos4MiercolesDelMes.Count == 5)
                                                                            {
                                                                                ultimos4MiercolesDelMes.RemoveAt(0);
                                                                            }

                                                                            //var semanasDepDelMes = 0;
                                                                            //for (int i = 0; i < ultimos4MiercolesDelMes.Count; i++)
                                                                            //{
                                                                            //    if (fechaHasta.Day < ultimos4MiercolesDelMes[i])
                                                                            //    {
                                                                            //        semanasDepDelMes = i + 1;
                                                                            //        break;
                                                                            //    }
                                                                            //}
                                                                            //if (semanasDepDelMes == 0)
                                                                            //{
                                                                            //    semanasDepDelMes = 4;
                                                                            //}

                                                                            enviarACosto.mesesMaximoDepreciacion = _insACosto.MesesMaximoDepreciacion;
                                                                            enviarACosto.porcentajeDepreciacion = _insACosto.PorcentajeDepreciacion;
                                                                            enviarACosto.mesesDepreciados = _insACosto.MesesDepreciadosAñoAnterior + _insACosto.MesesDepreciadosAñoActual;
                                                                            enviarACosto.mesesFaltantes = enviarACosto.mesesMaximoDepreciacion - (enviarACosto.mesesDepreciados);
                                                                            //enviarACosto.semanasUltimoMesDep = semanasDepDelMes;
                                                                            enviarACosto.semanasUltimoMesDep = 4;
                                                                            enviarACosto.monto = Math.Round(_insACosto.MOI + _insACosto.Altas + _insACosto.Overhaul, 2);
                                                                            //enviarACosto.depActual = semanasDepDelMes == 4 ? Math.Round(_insACosto.DepreciacionContableAcumulada, 2) : Math.Round((_insACosto.DepreciacionContableAcumulada - _insACosto.DepreciacionMensual) + ((_insACosto.DepreciacionMensual / 4) * semanasDepDelMes), 2);
                                                                            enviarACosto.depActual = Math.Round(_insACosto.DepreciacionContableAcumulada, 2);
                                                                            //enviarACosto.depFaltante = semanasDepDelMes == 4 ? Math.Round(_insACosto.ValorEnLibros, 2) : Math.Round((_insACosto.MOI + _insACosto.Altas + _insACosto.Overhaul) - enviarACosto.depActual, 2);
                                                                            enviarACosto.depFaltante = Math.Round(_insACosto.ValorEnLibros, 2);
                                                                            enviarACosto.descripcion = _insACosto.Descripcion;
                                                                            enviarACosto.enviaACosto = false;
                                                                            enviarACosto.fechaAlta = _insACosto.Fecha;
                                                                            enviarACosto.fechaInicioDep = _insACosto.FechaInicioDepreciacion;
                                                                            enviarACosto.idEconomico = _insACosto.IdCatMaquina.Value;
                                                                            enviarACosto.cc = _insACosto.Cc;
                                                                            enviarACosto.area = _insACosto.Area.Value;
                                                                            enviarACosto.cuenta = _insACosto.Cuenta_OC.Value;
                                                                            enviarACosto.polizaAlta = _insACosto.Poliza;
                                                                            enviarACosto.polizaBaja = null;
                                                                            enviarACosto.polizaCosto = null;
                                                                            enviarACosto.estatus = true;

                                                                            lstCostos.Add(enviarACosto);
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    registrosARelacionar.AddRange(lstInsumosMismoSubconjunto);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            registrosARelacionar.AddRange(gpInsumo);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    registrosARelacionar.AddRange(gpMaq);
                                                }
                                            }
                                            else
                                            {
                                                r.Add(SUCCESS, false);
                                                r.Add(MESSAGE, "ERROR AL CONSULTAR LA RELACION DE # DE PARTES EN EL EQUIPO: " + gpMaq.Key.Value + ". " + operacionRelacionEquipoInsumo.Message);
                                                return r;
                                            }
                                        }

                                        registrosARelacionar.AddRange(nuevasPol);
                                        //

                                        _context.tblC_AF_EnviarCosto.AddRange(lstCostos);
                                        _context.SaveChanges();

                                        cuenta.Detalles = registrosARelacionar;
                                    }
                                    #endregion

                                    #region relacionPolizaMaquina
                                    foreach (var item in cuenta.Detalles.Where(x => !x.AltaSigoplan && !x.faltante))
                                    {
                                        if (exclusiones.Any(a => a.Año == item.AñoPol_alta && a.Mes == item.MesPol_alta && a.Poliza == item.PolPol_alta && a.TipoPoliza == item.TpPol_alta && a.Linea == item.LineaPol_alta))
                                        {
                                            continue;
                                        }
                                        var existe = _context.tblC_AF_PolizaAltaBaja.FirstOrDefault(x => x.Estatus && x.Año == item.AñoPol_alta && x.Mes == item.MesPol_alta && x.Poliza == item.PolPol_alta && x.TP == item.TpPol_alta && x.Linea == item.LineaPol_alta && x.Cuenta == cuenta.Cuenta);
                                        if (existe == null)
                                        {
                                            var idMaq = catMaquina.FirstOrDefault(x => x.noEconomico == item.Clave);
                                            if (idMaq != null)
                                            {
                                                var polizaAlta = new tblC_AF_PolizaAltaBaja();
                                                var catMaqDep = new tblM_CatMaquinaDepreciacion();

                                                polizaAlta.Año = item.AñoPol_alta;
                                                polizaAlta.Mes = item.MesPol_alta;
                                                polizaAlta.Poliza = item.PolPol_alta;
                                                polizaAlta.TP = item.TpPol_alta;
                                                polizaAlta.Linea = item.LineaPol_alta;
                                                polizaAlta.TM = 1;
                                                polizaAlta.Cuenta = cuenta.Cuenta;
                                                polizaAlta.Subcuenta = item.Subcuenta;
                                                polizaAlta.SubSubcuenta = item.SubSubcuenta;
                                                polizaAlta.Concepto = item.Descripcion;
                                                polizaAlta.Monto = item.MOI + item.Overhaul + item.Altas;
                                                polizaAlta.Factura = item.Factura;
                                                polizaAlta.FechaMovimiento = item.Fecha;
                                                polizaAlta.TipoActivo = item.Overhaul != 0.0M || item.EsOverhaul ? 2 : 1;
                                                polizaAlta.Estatus = true;
                                                polizaAlta.FechaCreacion = DateTime.Now;
                                                polizaAlta.IdUsuarioCreacion = 13;
                                                polizaAlta.FechaModificacion = polizaAlta.FechaCreacion;
                                                polizaAlta.IdUsuarioModificacion = polizaAlta.IdUsuarioCreacion;

                                                _context.tblC_AF_PolizaAltaBaja.Add(polizaAlta);
                                                _context.SaveChanges();

                                                catMaqDep.IdCatMaquina = idMaq.id;
                                                catMaqDep.IdPoliza = polizaAlta.Id;
                                                catMaqDep.FechaInicioDepreciacion = polizaAlta.FechaMovimiento;
                                                catMaqDep.PorcentajeDepreciacion = item.PorcentajeDepreciacion;
                                                catMaqDep.MesesTotalesDepreciacion = item.MesesMaximoDepreciacion;
                                                catMaqDep.TipoDelMovimiento = item.Overhaul != 0.0M || item.EsOverhaul ? 5 : 1;
                                                catMaqDep.IdPolizaReferenciaAlta = null;
                                                catMaqDep.Estatus = true;
                                                catMaqDep.FechaCreacion = DateTime.Now;
                                                catMaqDep.IdUsuarioCreacion = 13;
                                                catMaqDep.FechaModificacion = catMaqDep.FechaCreacion;
                                                catMaqDep.IdUsuarioModificacion = catMaqDep.IdUsuarioCreacion;
                                                catMaqDep.CapturaAutomatica = true;

                                                _context.tblM_CatMaquinaDepreciacion.Add(catMaqDep);
                                                _context.SaveChanges();

                                                if (item.FechaBaja != null || item.FechaCancelacion != null)
                                                {
                                                    var existeBaja = _context.tblC_AF_PolizaAltaBaja.FirstOrDefault(x => x.Estatus && x.Año == item.AñoPol_baja && x.Mes == item.MesPol_baja && x.Poliza == item.PolPol_baja && x.TP == item.TpPol_baja && x.Linea == item.LineaPol_baja && x.Cuenta == cuenta.Cuenta);

                                                    if (existeBaja == null)
                                                    {
                                                        tblC_AF_PolizaAltaBaja polizaBaja = new tblC_AF_PolizaAltaBaja();
                                                        tblM_CatMaquinaDepreciacion catMaqDepBaja = new tblM_CatMaquinaDepreciacion();

                                                        polizaBaja.Año = item.AñoPol_baja.Value;
                                                        polizaBaja.Mes = item.MesPol_baja.Value;
                                                        polizaBaja.Poliza = item.PolPol_baja.Value;
                                                        polizaBaja.TP = item.TpPol_baja;
                                                        polizaBaja.Linea = item.LineaPol_baja.Value;
                                                        polizaBaja.TM = item.FechaBaja != null ? 2 : 3;
                                                        polizaBaja.Cuenta = cuenta.Cuenta;
                                                        polizaBaja.Factura = item.Factura;
                                                        polizaBaja.FechaMovimiento = item.FechaBaja != null ? item.FechaBaja.Value : item.FechaCancelacion.Value;
                                                        polizaBaja.TipoActivo = polizaAlta.TipoActivo;
                                                        polizaBaja.Estatus = true;
                                                        polizaBaja.FechaCreacion = DateTime.Now;
                                                        polizaBaja.IdUsuarioCreacion = 13;
                                                        polizaBaja.FechaModificacion = polizaBaja.FechaCreacion;
                                                        polizaBaja.IdUsuarioModificacion = polizaBaja.IdUsuarioCreacion;

                                                        _context.tblC_AF_PolizaAltaBaja.Add(polizaBaja);
                                                        _context.SaveChanges();

                                                        catMaqDepBaja.IdCatMaquina = idMaq.id;
                                                        catMaqDepBaja.IdPoliza = polizaBaja.Id;
                                                        catMaqDepBaja.FechaInicioDepreciacion = null;
                                                        catMaqDepBaja.PorcentajeDepreciacion = null;
                                                        catMaqDepBaja.MesesTotalesDepreciacion = null;
                                                        catMaqDepBaja.TipoDelMovimiento = catMaqDep.TipoDelMovimiento;
                                                        catMaqDepBaja.IdPolizaReferenciaAlta = polizaAlta.Id;
                                                        catMaqDepBaja.Estatus = true;
                                                        catMaqDepBaja.FechaCreacion = DateTime.Now;
                                                        catMaqDepBaja.IdUsuarioCreacion = 13;
                                                        catMaqDepBaja.FechaModificacion = catMaqDepBaja.FechaCreacion;
                                                        catMaqDepBaja.IdUsuarioModificacion = catMaqDepBaja.IdUsuarioCreacion;
                                                        catMaqDepBaja.CapturaAutomatica = true;

                                                        _context.tblM_CatMaquinaDepreciacion.Add(catMaqDepBaja);
                                                        _context.SaveChanges();
                                                    }
                                                    else
                                                    {
                                                        //Ya se encuentra registrada esta póliza (baja)
                                                    }
                                                }

                                                idMaq.DepreciacionCapturada = true;
                                                _context.SaveChanges();
                                                //ctx.SaveChanges();
                                            }
                                            else
                                            {
                                                //No hay coincidencia con noEconomico Enkontrol y noEconomico Sigoplan
                                            }
                                        }
                                        else
                                        {
                                            //La póliza (alta) ya se encuentra registrada en sigoplan
                                        }
                                    }
                                    foreach (var item in cuenta.Detalles.Where(x => x.AltaSigoplan && !x.faltante && x.PolPol_alta != 12151215 && x.PolPol_alta != 12101210 && ((!x.BajaSigoplan && x.FechaBaja != null) || (!x.CancelacionSigoplan && x.FechaCancelacion != null))))
                                    {
                                        var existe = _context.tblC_AF_PolizaAltaBaja.FirstOrDefault(x => x.Estatus && x.Año == item.AñoPol_baja && x.Mes == item.MesPol_baja && x.Poliza == item.PolPol_baja && x.TP == item.TpPol_baja && x.Linea == item.LineaPol_baja && x.Cuenta == cuenta.Cuenta);

                                        if (exclusiones.Any(a => a.Año == item.AñoPol_baja && a.Mes == item.MesPol_baja && a.Poliza == item.PolPol_baja && a.TipoPoliza == item.TpPol_baja && a.Linea == item.LineaPol_baja))
                                        {
                                            continue;
                                        }

                                        if (existe == null)
                                        {
                                            var idMaq = catMaquina.FirstOrDefault(x => x.noEconomico == item.Clave);

                                            var buscarAlta = _context.tblM_CatMaquinaDepreciacion.FirstOrDefault(x => x.Estatus && x.Poliza.Año == item.AñoPol_alta && x.Poliza.Mes == item.MesPol_alta && x.Poliza.Poliza == item.PolPol_alta && x.Poliza.TP == item.TpPol_alta && x.Poliza.Linea == item.LineaPol_alta && x.Poliza.Cuenta == cuenta.Cuenta);
                                            if (buscarAlta != null)
                                            {
                                                tblC_AF_PolizaAltaBaja polizaBaja = new tblC_AF_PolizaAltaBaja();
                                                tblM_CatMaquinaDepreciacion catMaqDepBaja = new tblM_CatMaquinaDepreciacion();

                                                polizaBaja.Año = item.AñoPol_baja.Value;
                                                polizaBaja.Mes = item.MesPol_baja.Value;
                                                polizaBaja.Poliza = item.PolPol_baja.Value;
                                                polizaBaja.TP = item.TpPol_baja;
                                                polizaBaja.Linea = item.LineaPol_baja.Value;
                                                polizaBaja.TM = item.FechaBaja != null ? 2 : 3;
                                                polizaBaja.Cuenta = cuenta.Cuenta;
                                                polizaBaja.Factura = item.Factura;
                                                polizaBaja.FechaMovimiento = item.FechaBaja != null ? item.FechaBaja.Value : item.FechaCancelacion.Value;
                                                polizaBaja.TipoActivo = buscarAlta.Poliza.TipoActivo;
                                                polizaBaja.Estatus = true;
                                                polizaBaja.FechaCreacion = DateTime.Now;
                                                polizaBaja.IdUsuarioCreacion = 13;
                                                polizaBaja.FechaModificacion = polizaBaja.FechaCreacion;
                                                polizaBaja.IdUsuarioModificacion = polizaBaja.IdUsuarioCreacion;

                                                _context.tblC_AF_PolizaAltaBaja.Add(polizaBaja);
                                                _context.SaveChanges();

                                                catMaqDepBaja.IdCatMaquina = idMaq.id;
                                                catMaqDepBaja.IdPoliza = polizaBaja.Id;
                                                catMaqDepBaja.FechaInicioDepreciacion = null;
                                                catMaqDepBaja.PorcentajeDepreciacion = null;
                                                catMaqDepBaja.MesesTotalesDepreciacion = null;
                                                catMaqDepBaja.TipoDelMovimiento = buscarAlta.TipoDelMovimiento;
                                                catMaqDepBaja.IdPolizaReferenciaAlta = buscarAlta.IdPoliza;
                                                catMaqDepBaja.Estatus = true;
                                                catMaqDepBaja.FechaCreacion = DateTime.Now;
                                                catMaqDepBaja.IdUsuarioCreacion = 13;
                                                catMaqDepBaja.FechaModificacion = catMaqDepBaja.FechaCreacion;
                                                catMaqDepBaja.IdUsuarioModificacion = catMaqDepBaja.IdUsuarioCreacion;
                                                catMaqDepBaja.CapturaAutomatica = true;

                                                _context.tblM_CatMaquinaDepreciacion.Add(catMaqDepBaja);
                                                _context.SaveChanges();
                                            }
                                        }
                                        else
                                        {
                                            //La póliza de baja ya se encuentra registrada.
                                        }
                                    }
                                    #endregion
                                }


                                if (lstCostos.Count > 0)
                                {
                                    var _insumosConDuplicados = lstRegistrosConDuplicados.Select(m => new
                                    {
                                        Poliza = m.Poliza,
                                        Cuenta = m.Cuenta,
                                        Subcuenta = m.Subcuenta,
                                        SubSubcuenta = m.SubSubcuenta,
                                        Monto = m.MOI + m.Altas + m.Overhaul,
                                        Concepto = m.Descripcion,
                                        aCosto = !m.NuevoInsumo,
                                        numEconomico = m.Clave
                                    });

                                    var polizaGeneradaCostos = GenerarPolizaCosto(lstCostos.Select(m => m.id).ToList());

                                    if (polizaGeneradaCostos.Success)
                                    {
                                        r.Add(SUCCESS, true);
                                        r.Add("seGeneroPolizaCosto", true);
                                        r.Add("polizaGeneradaCostos", polizaGeneradaCostos.Value);
                                        r.Add("lstRegistrosConDuplicados", _insumosConDuplicados);

                                        transaction.Commit();
                                        //transactionArre.Commit();
                                    }
                                    else
                                    {
                                        r.Add(SUCCESS, false);
                                        r.Add(MESSAGE, polizaGeneradaCostos.Message);
                                        transaction.Rollback();
                                    }
                                }
                                else
                                {
                                    r.Add(SUCCESS, true);
                                    r.Add("seGeneroPolizaCosto", false);
                                    transaction.Commit();
                                    //transactionArre.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                transactionArre.Rollback();
                                r.Add(SUCCESS, false);
                                r.Add(MESSAGE, ex.Message);
                            }
                        }
                    }
                }
            }
            else
            {
                r = obtenerDetalles;
            }

            return r;
        }

        private Dictionary<string, object> GetMaquinasEnkontrol()
        {
            var resultado = new Dictionary<string, object>();

            var empresaSesionActual = vSesiones.sesionEmpresaActual;

            try
            {
                var odbcCC = new OdbcConsultaDTO();

                odbcCC.consulta = string.Format(@"SELECT cc, descripcion FROM cc");

                vSesiones.sesionEmpresaActual = (int)EmpresaEnum.Arrendadora;

                var cc = _contextEnkontrol.Select<ActivoFijoCCDTO>(EnkontrolAmbienteEnum.Prod, odbcCC);

                vSesiones.sesionEmpresaActual = empresaSesionActual;

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, cc);
            }
            catch (Exception ex)
            {
                vSesiones.sesionEmpresaActual = empresaSesionActual;

                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Error al obtener el cc desde enkontrol: " + ex.ToString());
            }

            return resultado;
        }

        public Dictionary<string, object> GetMaquinas(int idCuenta, int estatusMaquina, int tipoCaptura)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

            try
            {
                var getMaquinasEnkontrol = GetMaquinasEnkontrol();
                //var areasCuentaEnkontrol = AreaCuentaEnkontrol();
                var cc = new List<ActivoFijoCCDTO>();

                List<tblP_CC> areasCuenta = null;
                List<tblM_DocumentosMaquinaria> documentosMaquina = null;
                List<ActivoFijoCatMaqDTO> catDepMaq = null;
                IQueryable<tblM_CatMaquinaDepreciacion> depMaq = null;

                using (var ctx = new MainContext(EmpresaEnum.Arrendadora))
                {
                    areasCuenta = ctx.tblP_CC.ToList();
                    documentosMaquina = ctx.tblM_DocumentosMaquinaria.Where(x => x.tipoArchivo == (int)TipoDocumentoMaquinaria.Factura).ToList();

                    catDepMaq = ctx.tblM_CatMaquina.Where(x => x.noEconomico != null && x.grupoMaquinaria.tipoEquipoID == idCuenta).Select(m => new ActivoFijoCatMaqDTO
                    {
                        Id = m.id,
                        CC = "",
                        NoEconomico = m.noEconomico,
                        Descripcion = m.descripcion + "(" + m.anio + ", " + m.noSerie + ", " + m.marca.descripcion + ", " + m.modeloEquipo.descripcion + ", " + m.proveedor + ")",
                        AreaCuenta = m.centro_costos,
                        DepreciacionCapturada = m.DepreciacionCapturada,
                        CapturaAutomatica = false,
                        estatus = m.estatus,
                        FechaAdquisicion = m.fechaAdquisicion
                    }).ToList();

                    depMaq = _context.tblM_CatMaquinaDepreciacion.Where(x => x.Estatus && x.IdUsuarioModificacion == 13);

                    if ((bool)getMaquinasEnkontrol[SUCCESS])
                    {
                        cc = (List<ActivoFijoCCDTO>)getMaquinasEnkontrol[ITEMS];

                        var registrosSinCC = new List<ActivoFijoCatMaqDTO>();

                        foreach (var item in catDepMaq)
                        {
                            ActivoFijoCCDTO ccMatch = null;

                            ccMatch = cc.FirstOrDefault(x => x.Descripcion == item.NoEconomico);

                            var docMatch = documentosMaquina.FirstOrDefault(x => x.economicoID == item.Id);
                            var depMaqMatch = depMaq.FirstOrDefault(x => x.Estatus && x.CapturaAutomatica && x.IdCatMaquina == item.Id && x.CapturaAutomatica);

                            if (depMaqMatch != null)
                            {
                                item.CapturaAutomatica = true;
                            }

                            if (ccMatch != null)
                            {
                                item.CC = ccMatch.CC;

                                var acMatch = areasCuenta.FirstOrDefault(x => x.areaCuenta == item.AreaCuenta && item.estatus == 1);

                                if (acMatch != null && acMatch.areaCuenta != "1010" && acMatch.areaCuenta != "1015" && acMatch.areaCuenta != "1018")
                                {
                                    item.AreaCuenta = acMatch.areaCuenta + " - " + acMatch.descripcion;
                                }
                                else
                                {
                                    item.AreaCuenta = "14-1 - MAQUINARIA NO ASIGNADA A OBRA";
                                }

                                if (docMatch != null)
                                {
                                    item.Factura = new AnexoMaquinaDTO
                                    {
                                        id = docMatch.id,
                                        nombre = docMatch.nombreArchivo,
                                        rutaArchivo = docMatch.nombreRuta,
                                        tipo = docMatch.tipoArchivo
                                    };
                                }
                            }
                            else
                            {
                                var acMatch = areasCuenta.FirstOrDefault(x => x.areaCuenta == item.AreaCuenta && item.estatus == 1);
                                if (acMatch != null && acMatch.areaCuenta != "1010" && acMatch.areaCuenta != "1015" && acMatch.areaCuenta != "1018")
                                {
                                    item.AreaCuenta = acMatch.areaCuenta + " - " + acMatch.descripcion;
                                }
                                else
                                {
                                    item.AreaCuenta = "14-1 - MAQUINARIA NO ASIGNADA A OBRA";
                                }
                                //registrosSinCC.Add(item);
                            }

                            if (tipoCaptura == 2) //Automática
                            {
                                if (!item.CapturaAutomatica)
                                {
                                    registrosSinCC.Add(item);
                                }
                            }
                            if (tipoCaptura == 3) //Manual
                            {
                                if (item.CapturaAutomatica)
                                {
                                    registrosSinCC.Add(item);
                                }
                            }

                            if (estatusMaquina == 2) //Dep capturada
                            {
                                if (!item.DepreciacionCapturada)
                                {
                                    registrosSinCC.Add(item);
                                }
                            }
                            if (estatusMaquina == 3) //Dep no capturada
                            {
                                if (item.DepreciacionCapturada)
                                {
                                    registrosSinCC.Add(item);
                                }
                            }

                        }

                        foreach (var item in registrosSinCC)
                        {
                            catDepMaq.Remove(item);
                        }

                        //var catDepMaq = _context.tblM_CatMaquina.Select(c => new ActivoFijoCatMaqDTO
                        //{
                        //    Id = c.id,
                        //    CC = c.noEconomico,
                        //    Descripcion = c.descripcion,
                        //    DepreciacionCapturada = c.DepreciacionCapturada
                        //}).SelectMany(cat => _context.tblM_CatMaquinaDepreciacion.Where(m => m.Estatus).Select(x => new ActivoFijoDepMaqDTO
                        //{
                        //    Id = x.Id,
                        //    IdCatMaquina = x.IdCatMaquina,
                        //    MesesTotalesADepreciar = x.MesesTotalesDepreciacion,
                        //    PorcentajeDepreciacion = x.PorcentajeDepreciacion
                        //}).Where(dep => dep.IdCatMaquina == cat.Id).DefaultIfEmpty(),
                        //    (cat, dep) => new ActivoFijoRelMaqDepDTO
                        //    {
                        //        Maquina = cat,
                        //        Depreciacion = dep
                        //    }
                        //).ToList();

                        resultado.Add(SUCCESS, true);
                        resultado.Add(ITEMS, catDepMaq);
                    }
                    else
                    {
                        resultado = getMaquinasEnkontrol;
                    }
                }
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Error: " + ex.ToString());
            }

            return resultado;
        }

        public Dictionary<string, object> RegistrarDepMaquina(List<ActivoFijoRegInfoDepDTO> depMaquina)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

            using (var ctx = new MainContext(EmpresaEnum.Arrendadora))
            {
                using (var transactionArre = ctx.Database.BeginTransaction())
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (ActivoFijoRegInfoDepDTO infoDep in depMaquina)
                            {
                                if (_context.tblC_AF_PolizaAltaBaja.Any(x => x.Año == infoDep.Año && x.Mes == infoDep.Mes && x.Poliza == infoDep.Poliza && x.TP == infoDep.TP && x.Linea == infoDep.Linea && x.Estatus))
                                {
                                    transaction.Rollback();
                                    transactionArre.Rollback();

                                    resultado.Add(SUCCESS, false);
                                    resultado.Add(MESSAGE, "Error: Ya se encuentra un registro con la poliza " + infoDep.Año + "-" + infoDep.Mes + "-" + infoDep.Poliza + "-" + infoDep.TP + "-" + infoDep.Linea);

                                    return resultado;
                                }

                                tblC_AF_PolizaAltaBaja poliza = new tblC_AF_PolizaAltaBaja();

                                poliza.Año = infoDep.Año;
                                poliza.Mes = infoDep.Mes;
                                poliza.Poliza = infoDep.Poliza;
                                poliza.TP = infoDep.TP;
                                poliza.Linea = infoDep.Linea;
                                poliza.TM = infoDep.TM;
                                poliza.Cuenta = infoDep.Cuenta;
                                poliza.Factura = infoDep.Factura;
                                poliza.FechaMovimiento = infoDep.FechaMovimiento;
                                poliza.TipoActivo = infoDep.TipoActivo;
                                poliza.Estatus = true;
                                poliza.FechaCreacion = DateTime.Now;
                                poliza.IdUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                poliza.FechaModificacion = poliza.FechaCreacion;
                                poliza.IdUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;

                                _context.tblC_AF_PolizaAltaBaja.Add(poliza);
                                _context.SaveChanges();

                                tblM_CatMaquinaDepreciacion catMaqDep = new tblM_CatMaquinaDepreciacion();

                                catMaqDep.IdCatMaquina = infoDep.IdCatMaquina;
                                catMaqDep.IdPoliza = poliza.Id;
                                catMaqDep.FechaInicioDepreciacion = infoDep.FechaInicioDepreciacion;
                                catMaqDep.PorcentajeDepreciacion = infoDep.PorcentajeDepreciacion / 100;
                                catMaqDep.MesesTotalesDepreciacion = infoDep.MesesTotalesDepreciacion;
                                catMaqDep.TipoDelMovimiento = infoDep.TipoDelMovimiento;
                                catMaqDep.IdPolizaReferenciaAlta = null;
                                catMaqDep.Estatus = true;
                                catMaqDep.FechaCreacion = DateTime.Now;
                                catMaqDep.IdUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                catMaqDep.FechaModificacion = catMaqDep.FechaCreacion;
                                catMaqDep.IdUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;

                                _context.tblM_CatMaquinaDepreciacion.Add(catMaqDep);
                                _context.SaveChanges();
                            }

                            int __idMaquina = depMaquina.First().IdCatMaquina;
                            var catMaq = ctx.tblM_CatMaquina.FirstOrDefault(x => x.id == __idMaquina);

                            catMaq.DepreciacionCapturada = true;

                            ctx.tblM_CatMaquina.Attach(catMaq);
                            ctx.Entry(catMaq).Property(x => x.DepreciacionCapturada).IsModified = true;
                            ctx.SaveChanges();

                            foreach (ActivoFijoRegInfoDepDTO depBaja in depMaquina.Where(x => x.TM == 2 || x.TM == 3 || x.TipoActivo == 3))
                            {
                                if (string.IsNullOrEmpty(depBaja.PolizaRefAlta) || depBaja.TipoActivo == 4)
                                {
                                    continue;
                                }

                                int año = Convert.ToInt32(depBaja.PolizaRefAlta.Split('-')[0]);
                                int mes = Convert.ToInt32(depBaja.PolizaRefAlta.Split('-')[1]);
                                int poliza = Convert.ToInt32(depBaja.PolizaRefAlta.Split('-')[2]);
                                string tp = depBaja.PolizaRefAlta.Split('-')[3];
                                int linea = Convert.ToInt32(depBaja.PolizaRefAlta.Split('-')[4]);

                                tblM_CatMaquinaDepreciacion cmdAlta = _context.tblM_CatMaquinaDepreciacion.FirstOrDefault
                                    (
                                        x => x.Poliza.Año == año && x.Poliza.Mes == mes && x.Poliza.Poliza == poliza &&
                                        x.Poliza.TP == tp && x.Poliza.Linea == linea && x.Estatus
                                    );
                                tblM_CatMaquinaDepreciacion cmdBaja = _context.tblM_CatMaquinaDepreciacion.FirstOrDefault
                                    (
                                        x => x.Poliza.Año == depBaja.Año && x.Poliza.Mes == depBaja.Mes &&
                                        x.Poliza.Poliza == depBaja.Poliza && x.Poliza.TP == depBaja.TP && x.Poliza.Linea == depBaja.Linea && x.Estatus
                                    );

                                cmdBaja.IdPolizaReferenciaAlta = cmdAlta.Poliza.Id;
                                if (depBaja.TipoActivo == 3)
                                {
                                    cmdBaja.Poliza.TipoActivo = 3;
                                }
                                else
                                {
                                    cmdBaja.Poliza.TipoActivo = cmdAlta.Poliza.TipoActivo;
                                }

                                if (_context.tblM_CatMaquinaDepreciacion.Any(x => x.IdPolizaReferenciaAlta == cmdBaja.IdPolizaReferenciaAlta && x.Estatus))
                                {
                                    transaction.Rollback();
                                    transactionArre.Rollback();

                                    resultado.Add(SUCCESS, false);
                                    resultado.Add(MESSAGE, "Error: una poliza de baja/alta ya se encuentra relacionada con la poliza de alta " + año + "-" + mes + "-" + poliza + "-" + tp + "-" + linea);

                                    return resultado;
                                }

                                //_context.tblM_CatMaquinaDepreciacion.Attach(cmdBaja);
                                //_context.Entry(cmdBaja).Property(x => x.IdPolizaReferenciaAlta).IsModified = true;
                                //_context.Entry(cmdBaja).Property(x => x.Poliza.TipoActivo).IsModified = true;
                                _context.SaveChanges();
                            }

                            QuitarAlerta(depMaquina.First().IdCatMaquina);

                            transaction.Commit();
                            transactionArre.Commit();

                            resultado.Add(SUCCESS, true);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            transactionArre.Rollback();

                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Ocurrió un error al actulizar la BD. " + ex.ToString());
                        }
                    }
                }
            }

            return resultado;
        }

        public Dictionary<string, object> AgregarPoliza(ActivoFijoAgregarPolizaDTO infoPoliza)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var polizaRegistrada = _context.tblC_AF_PolizaAltaBaja.FirstOrDefault(x => x.Año == infoPoliza.Año && x.Mes == infoPoliza.Mes && x.TP == infoPoliza.TP && x.Poliza == infoPoliza.Poliza && x.Linea == infoPoliza.Linea && x.Estatus);

                if (polizaRegistrada != null)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ya se encuentra capturada");
                }
                else
                {
                    var resultado_poliza = ObtenerPoliza(infoPoliza);

                    if ((bool)resultado_poliza[SUCCESS])
                    {
                        var poliza = (List<ActivoFijoRegInfoDepDTO>)resultado_poliza[ITEMS];

                        if (poliza.Count > 0)
                        {
                            resultado.Add(SUCCESS, true);
                            resultado.Add(ITEMS, poliza);
                        }
                        else
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "No se encontró póliza");
                        }
                    }
                    else
                    {
                        resultado = resultado_poliza;
                    }
                }
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al obtener información de la póliza: " + ex.Message);
            }

            return resultado;
        }

        private List<ActivoFijoRegInfoDepDTO> EncontrarPolizasSinRelacionar(string noEconomico, int cuenta)
        {
            var relacionCuentas = _context.tblC_AF_RelacionesCuentaAño.Where
                (w =>
                    w.Estatus &&
                    w.CuentaMovimientoId == null &&
                    w.Subcuenta.Cuenta.Cuenta == cuenta &&
                    w.Subcuenta.Cuenta.TipoCuentaId == (int)AFTipoCuentaEnum.Movimiento
                ).ToList();

            var infoDepreciacion = _context.tblC_AF_Cuentas.First(f => f.Cuenta == cuenta);

            var fechaActual = DateTime.Now;
            var fechaInicio = fechaActual.AddMonths(-4);

            var query_sc_movpol = new OdbcConsultaDTO();
            var where_sc_movpol = "";
            var where_sc_movpol2 = "";

            foreach (var ctas in relacionCuentas)
            {
                where_sc_movpol += "(MOV.scta = ? AND MOV.sscta = ?)";

                if (ctas != relacionCuentas.Last())
                {
                    where_sc_movpol += " OR ";
                }
            }

            if (fechaInicio.Year != fechaActual.Year)
            {
                where_sc_movpol2 = " OR (MOV.year = " + fechaInicio.Year + " AND MOV.mes >= " + fechaInicio.Month + ")";
            }

            query_sc_movpol.consulta = string.Format
                (
                    @"SELECT
                        MOV.year AS año,
                        MOV.mes,
                        MOV.poliza,
                        MOV.tp,
                        MOV.linea,
                        MOV.tm,
                        MOV.cta AS cuenta,
                        MOV.scta AS subcuenta,
                        MOV.sscta AS sububcuenta,
                        MOV.monto,
                        MOV.concepto,
                        POL.fechapol,
                        FAC.factura,
                        FAC.fecha AS FechaFactura,
                        CC.descripcion AS AreaCuenta
                    FROM
                        sc_movpol AS MOV
                    INNER JOIN
                        sc_polizas AS POL
                        ON
                            POL.year = MOV.year AND
                            POL.mes = MOV.mes AND
                            POL.poliza = MOV.poliza AND
                            POL.tp = MOV.tp
                    INNER JOIN
                        cc AS CC
                        ON
                            CC.cc = MOV.cc
                    LEFT JOIN
                        sp_movprov AS FAC
                        ON
                            FAC.year = MOV.year AND
                            FAC.mes = MOV.mes AND
                            FAC.poliza = MOV.poliza AND
                            FAC.es_factura = 'S'
                    WHERE
                        MOV.cta = ? AND
                        (
                            {0}
                        ) AND
                        {1} LIKE ? AND
                        (
                            (POL.fechapol >= ?){2}
                        ) AND
                        MOV.tm in (1, 2, 3)
                        ",
                        where_sc_movpol,
                        vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora ? "MOV.referencia" : "CC.descripcion",
                        where_sc_movpol2
                );

            query_sc_movpol.parametros.Add(new OdbcParameterDTO
            {
                nombre = "cta",
                tipo = OdbcType.Int,
                valor = cuenta
            });
            foreach (var ctas in relacionCuentas)
            {
                query_sc_movpol.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "scta",
                    tipo = OdbcType.Int,
                    valor = ctas.Subcuenta.Subcuenta
                });
                query_sc_movpol.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "sscta",
                    tipo = OdbcType.Int,
                    valor = ctas.Subcuenta.SubSubcuenta
                });
            }
            query_sc_movpol.parametros.Add(new OdbcParameterDTO
            {
                nombre = "referencia",
                tipo = OdbcType.NVarChar,
                valor = "%" + noEconomico + "%"
            });
            query_sc_movpol.parametros.Add(new OdbcParameterDTO
            {
                nombre = "fechapol",
                tipo = OdbcType.Date,
                valor = fechaInicio
            });

            var sc_movpol = _contextEnkontrol.Select<ActivoFijoRegInfoDepDTO>(_productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, query_sc_movpol);

            foreach (var registro in sc_movpol.Where(w => w.TM == 1))
            {
                registro.PorcentajeDepreciacion = infoDepreciacion.PorcentajeDepreciacion;
                registro.MesesTotalesDepreciacion = infoDepreciacion.MesesDeDepreciacion;
            }

            var polizasQueNoSeRelacionaran = _context.tblC_AF_PolizasExcluidasParaCapturaAutomatica.Where(x => x.Estatus).ToList();

            foreach (var item in polizasQueNoSeRelacionaran)
            {
                sc_movpol.RemoveAll(x => x.Año == item.Año && x.Mes == item.Mes && x.Poliza == item.Poliza && x.TP == item.TipoPoliza && x.Linea == item.Linea);
            }

            return sc_movpol;
        }

        public Dictionary<string, object> AgregarPolizas(int idMaquina)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

            try
            {
                var idsACosto = new List<int>();

                tblM_CatMaquina catMaq = null;
                using (var ctx = new MainContext(EmpresaEnum.Arrendadora))
                {
                    catMaq = ctx.tblM_CatMaquina.FirstOrDefault(f => f.id == idMaquina);
                }

                List<tblM_CatMaquinaDepreciacion> depMaquina = _context.tblM_CatMaquinaDepreciacion.Where(x => x.IdCatMaquina == idMaquina && x.Estatus).ToList();

                foreach (var item in depMaquina)
                {
                    item.Maquina = catMaq;
                }
                
                var excluir = _context.tblC_AF_RelSubCuentas.Where(x => x.Estatus && x.Excluir).ToList();

                var polizasCC = new Dictionary<string, object>();

                var resultConstruplan = EncontrarPolizasSinRelacionar(catMaq.noEconomico, depMaquina.First().Poliza.Cuenta);
                polizasCC.Add(SUCCESS, true);
                polizasCC.Add(ITEMS, resultConstruplan);

                //if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                //{
                    
                //}
                //else
                //{
                //    polizasCC = ObtenerPolizasCC(depMaquina[0].Maquina.noEconomico);
                //}

                var cuentasMaquina = _context.tblC_AF_CatalogoMaquina.Select(x => x.Cuenta).ToList();

                var infoDep = _context.tblC_AF_Cuentas.Where(x => cuentasMaquina.Contains(x.Cuenta)).ToList();

                if ((bool)polizasCC[SUCCESS])
                {
                    List<ActivoFijoRegInfoDepDTO> polizasEncontradas = (List<ActivoFijoRegInfoDepDTO>)polizasCC[ITEMS];

                    var polizasCC_referencia = new Dictionary<string, object>();

                    polizasCC_referencia.Add(SUCCESS, true);
                    //polizasCC_referencia = ObtenerPolizasCCCargarExcel(depMaquina[0].Maquina.noEconomico);

                    if (/*(bool)polizasCC_referencia[SUCCESS]*/ 1 == 2)
                    {
                        //var polizasEncontradas_referencia = (List<ActivoFijoRegInfoDepDTO>)polizasCC_referencia[ITEMS];

                        //foreach (var item in polizasEncontradas_referencia)
                        //{
                        //    if (polizasEncontradas.FirstOrDefault(x => x.Año == item.Año && x.Mes == item.Mes && x.Poliza == item.Poliza && x.TP == item.TP && x.Linea == item.Linea) == null)
                        //    {
                        //        if (item.TM == 1)
                        //        {
                        //            item.PorcentajeDepreciacion = infoDep.FirstOrDefault(x => x.Cuenta == item.Cuenta).PorcentajeDepreciacion;
                        //            item.MesesTotalesDepreciacion = infoDep.FirstOrDefault(x => x.Cuenta == item.Cuenta).MesesDeDepreciacion;
                        //        }
                        //        polizasEncontradas.Add(item);
                        //    }
                            
                        //}

                        //List<ActivoFijoRegInfoDepDTO> polizasNuevas = new List<ActivoFijoRegInfoDepDTO>();

                        //foreach (ActivoFijoRegInfoDepDTO poliza in polizasEncontradas)
                        //{
                        //    poliza.CapturaPorSistema = true;

                        //    tblM_CatMaquinaDepreciacion polizaRegistrada = depMaquina.FirstOrDefault
                        //        (x =>
                        //            x.Poliza.Año == poliza.Año && x.Poliza.Mes == poliza.Mes &&
                        //            x.Poliza.Poliza == poliza.Poliza && x.Poliza.TP == poliza.TP &&
                        //            x.Poliza.Linea == poliza.Linea
                        //        );
                        //    if (excluir.Any(x => x.Cuenta.Cuenta == poliza.Cuenta && x.Subcuenta == poliza.Subcuenta && x.SubSubcuenta == poliza.SubSubcuenta && x.Año == poliza.Año))
                        //    {
                        //        continue;
                        //    }
                        //    if (polizaRegistrada == null)
                        //    {
                        //        polizasNuevas.Add(poliza);
                        //    }
                        //}

                        //resultado.Add(SUCCESS, true);
                        //resultado.Add(ITEMS, polizasNuevas);
                    }
                    else
                    {
                        List<ActivoFijoRegInfoDepDTO> polizasNuevas = new List<ActivoFijoRegInfoDepDTO>();

                        foreach (ActivoFijoRegInfoDepDTO poliza in polizasEncontradas)
                        {
                            tblM_CatMaquinaDepreciacion polizaRegistrada = depMaquina.FirstOrDefault
                                (x =>
                                    x.Poliza.Año == poliza.Año && x.Poliza.Mes == poliza.Mes &&
                                    x.Poliza.Poliza == poliza.Poliza && x.Poliza.TP == poliza.TP &&
                                    x.Poliza.Linea == poliza.Linea
                                );
                            poliza.CapturaPorSistema = true;
                            if (polizaRegistrada == null)
                            {
                                polizasNuevas.Add(poliza);
                            }
                        }

                        if (polizasNuevas.Count == 0)
                        {
                            //var nuevasPol = resultado[ITEMS] as List<ActivoFijoRegInfoDepDTO>;
                            //polizasCC_referencia.Add()
                            resultado = polizasCC_referencia;
                        }
                        else
                        {
                            resultado.Add(SUCCESS, true);
                            resultado.Add(ITEMS, polizasNuevas);
                        }
                    }
                }
                else
                {
                    resultado = polizasCC;
                }

                //PROCESO PARA DETECTAR INSUMOS DUPLICADOS
//                if ((bool)resultado[SUCCESS] && vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
//                {
//                    foreach (var insumo in depMaquina.Where(w => w.TipoDelMovimiento == (int)TipoDelMovimiento.O && w.PolizaAlta == null))
//                    {
//                        OdbcConsultaDTO odbcPolizasCC = new OdbcConsultaDTO();

//                        odbcPolizasCC.consulta = string.Format(@"
//                                SELECT TOP 1
//                                    MOV.scta AS Subcuenta,
//                                    MOV.sscta AS SubSubcuenta,
//                                    MOV.monto,
//                                    POL.fechapol,
//                                    MOV.concepto,
//                                    (SELECT DISTINCT TOP 1 (CAST(area AS varchar(4)) + '-' + CAST(cuenta AS varchar(4)) + ' - ' + descripcion) FROM si_area_cuenta WHERE area = MOV.area and cuenta = MOV.cuenta_oc) AS AreaCuenta
//                                FROM sc_movpol AS MOV
//                                INNER JOIN sc_polizas AS POL ON MOV.year = POL.year AND MOV.mes = POL.mes AND MOV.poliza = POL.poliza AND MOV.tp = POL.tp
//                                WHERE MOV.year = ? AND MOV.mes = ? AND MOV.poliza = ? AND MOV.tp = ? AND MOV.linea = ?");

//                        odbcPolizasCC.parametros.Add(new OdbcParameterDTO
//                        {
//                            nombre = "year",
//                            tipo = OdbcType.Int,
//                            valor = insumo.Poliza.Año
//                        });

//                        odbcPolizasCC.parametros.Add(new OdbcParameterDTO
//                        {
//                            nombre = "mes",
//                            tipo = OdbcType.Int,
//                            valor = insumo.Poliza.Mes
//                        });

//                        odbcPolizasCC.parametros.Add(new OdbcParameterDTO
//                        {
//                            nombre = "poliza",
//                            tipo = OdbcType.Int,
//                            valor = insumo.Poliza.Poliza
//                        });

//                        odbcPolizasCC.parametros.Add(new OdbcParameterDTO
//                        {
//                            nombre = "tp",
//                            tipo = OdbcType.VarChar,
//                            valor = insumo.Poliza.TP
//                        });

//                        odbcPolizasCC.parametros.Add(new OdbcParameterDTO
//                        {
//                            nombre = "linea",
//                            tipo = OdbcType.Int,
//                            valor = insumo.Poliza.Linea
//                        });

//                        ActivoFijoModPolDTO polizas_cc = _contextEnkontrol.Select<ActivoFijoModPolDTO>(EnkontrolAmbienteEnum.Prod, odbcPolizasCC).FirstOrDefault();

//                        if (polizas_cc != null)
//                        {
//                            insumo.Poliza.Concepto = polizas_cc.Concepto;
//                        }

//                        if (!string.IsNullOrEmpty(insumo.Poliza.Concepto) && insumo.Poliza.Concepto.ToUpper().Contains("INSUMO: "))
//                        {
//                            var arreglo_insumo = insumo.Poliza.Concepto.Trim().Split(new char[] { ' ' });

//                            if (arreglo_insumo.Length > 0)
//                            {
//                                insumo.Poliza.insumo = arreglo_insumo[1];
//                            }
//                        }
//                    }

//                    var nuevasPol = resultado[ITEMS] as List<ActivoFijoRegInfoDepDTO>;

//                    foreach (var item in nuevasPol)
//                    {
//                        if (!string.IsNullOrEmpty(item.Concepto) && item.Concepto.ToUpper().Contains("INSUMO: "))
//                        {
//                            var arreglo_insumo = item.Concepto.Trim().Split(new char[] { ' ' });

//                            if (arreglo_insumo.Length > 0)
//                            {
//                                item.insumo = arreglo_insumo[1];
//                            }
//                        }
//                    }

//                    var operacionRelacionEquipoInsumo = RelacionEquipoInsumo(depMaquina[0].IdCatMaquina);

//                    var depMaquinaTemporal = depMaquina.Where(w => !string.IsNullOrEmpty(w.Poliza.insumo) && w.PolizaAlta == null).ToList();

//                    if (operacionRelacionEquipoInsumo.Success)
//                    {
//                        var partesMaquina = operacionRelacionEquipoInsumo.Value as List<ActivoFijoRelacionEquipoInsumoDTO>;

//                        foreach (var insumo in depMaquina.Where(w => !string.IsNullOrEmpty(w.Poliza.insumo) && w.PolizaAlta == null).GroupBy(g => g.Poliza.insumo))
//                        {
//                            var repetido = nuevasPol.Where(w => w.insumo == insumo.Key).ToList();

//                            if (repetido.Count > 0)
//                            {
//                                var lstInsumosMismoSubconjunto = new List<tblM_CatMaquinaDepreciacion>();

//                                if (partesMaquina != null)
//                                {
//                                    var subconjuntoDelInsumo = partesMaquina.FirstOrDefault(w => w.insumo.Contains(insumo.Key));

//                                    if (subconjuntoDelInsumo != null)
//                                    {
//                                        lstInsumosMismoSubconjunto.AddRange(insumo);

//                                        var quitarDeTemporal = new List<tblM_CatMaquinaDepreciacion>();
//                                        foreach (var otroInsumo in depMaquinaTemporal.GroupBy(g => g.Poliza.insumo))
//                                        {
//                                            if (otroInsumo.Key != insumo.Key)
//                                            {
//                                                var siHayOtraParteDelMismoSubconjunto = partesMaquina.FirstOrDefault(f => f.insumo.Contains(otroInsumo.Key) && f.subconjunto == subconjuntoDelInsumo.subconjunto);

//                                                if (siHayOtraParteDelMismoSubconjunto != null)
//                                                {
//                                                    lstInsumosMismoSubconjunto.AddRange(otroInsumo);
//                                                    quitarDeTemporal.AddRange(otroInsumo);
//                                                }
//                                            }
//                                        }

//                                        foreach (var _itemTemp in quitarDeTemporal)
//                                        {
//                                            depMaquinaTemporal.Remove(_itemTemp);
//                                        }

//                                        //Se quitan los que ya tienen baja/cancelacion
//                                        var lstInsumosConBaja = new List<tblM_CatMaquinaDepreciacion>();
//                                        foreach (var insSubconjunto in lstInsumosMismoSubconjunto)
//                                        {
//                                            var tieneBaja = depMaquina.FirstOrDefault(f => f.IdPolizaReferenciaAlta != null && f.IdPolizaReferenciaAlta.Value == insSubconjunto.IdPoliza);
//                                            if (tieneBaja != null)
//                                            {
//                                                lstInsumosConBaja.Add(tieneBaja);
//                                            }
//                                        }

//                                        foreach (var conBaja in lstInsumosConBaja)
//                                        {
//                                            lstInsumosMismoSubconjunto.RemoveAll(r => r.IdPoliza == conBaja.IdPolizaReferenciaAlta);
//                                        }

//                                        if (subconjuntoDelInsumo.maximo < lstInsumosMismoSubconjunto.Count + repetido.Count)
//                                        {
//                                            var diferencia = (lstInsumosMismoSubconjunto.Count + repetido.Count) - subconjuntoDelInsumo.maximo;

//                                            var lstCostos = new List<tblC_AF_EnviarCosto>();

//                                            foreach (var _insACosto in lstInsumosMismoSubconjunto.OrderBy(o => o.Poliza.FechaMovimiento))
//                                            {
//                                                if (diferencia == 0)
//                                                {
//                                                    break;
//                                                }
//                                                diferencia--;

//                                                idsACosto.Add(_insACosto.Id);
//                                            }

//                                            _context.tblC_AF_EnviarCosto.AddRange(lstCostos);
//                                            _context.SaveChanges();
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }

                if (idsACosto.Count > 0)
                {
                    resultado.Add("idsACosto", idsACosto);
                }
                //
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al actualizar la BD. " + ex.ToString());
            }

            return resultado;
        }

        public Dictionary<string, object> ModificarDepMaquina(List<ActivoFijoRegInfoDepDTO> depMaquina)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

            try
            {

                int idCatMaquina = depMaquina[0].IdCatMaquina;

                List<tblM_CatMaquinaDepreciacion> registrosDeDepreciacionActuales = _context.tblM_CatMaquinaDepreciacion.Where(x => x.IdCatMaquina == idCatMaquina && x.Estatus).ToList();

                List<ActivoFijoRegInfoDepDTO> registrosNuevos = new List<ActivoFijoRegInfoDepDTO>();

                foreach (ActivoFijoRegInfoDepDTO item in depMaquina)
                {
                    item.CapturaPorSistema = false;
                    var coincidencia = registrosDeDepreciacionActuales.FirstOrDefault
                        (x =>
                            x.Poliza.Año == item.Año && x.Poliza.Mes == item.Mes && x.Poliza.Poliza == item.Poliza &&
                            x.Poliza.TP == item.TP && x.Poliza.Linea == item.Linea
                        );

                    if (coincidencia == null)
                    {
                        registrosNuevos.Add(item);
                    }
                }

                if (registrosNuevos.Count > 0)
                {
                    Dictionary<string, object> operacionRegistro = RegistrarDepMaquina(registrosNuevos);
                    if (!(bool)operacionRegistro[SUCCESS])
                    {
                        return operacionRegistro;
                    }
                }
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al actualizar la BD. " + ex.ToString());
            }

            using (DbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    int idCatMaquina = depMaquina[0].IdCatMaquina;

                    foreach (ActivoFijoRegInfoDepDTO depMaq in depMaquina.Where(x => x.IdCatMaquinaDepreciacion > 0))
                    {
                        tblM_CatMaquinaDepreciacion catMaqDep = _context.tblM_CatMaquinaDepreciacion.First(x => x.Id == depMaq.IdCatMaquinaDepreciacion && x.Estatus);

                        catMaqDep.Poliza.FechaMovimiento = depMaq.FechaMovimiento;
                        catMaqDep.Poliza.TipoActivo = depMaq.TipoActivo;
                        catMaqDep.Poliza.Factura = depMaq.Factura;
                        catMaqDep.FechaInicioDepreciacion = depMaq.FechaInicioDepreciacion;
                        catMaqDep.PorcentajeDepreciacion = depMaq.PorcentajeDepreciacion / 100;
                        catMaqDep.MesesTotalesDepreciacion = depMaq.MesesTotalesDepreciacion;
                        catMaqDep.TipoDelMovimiento = depMaq.TipoDelMovimiento;

                        if (depMaq.TM == 2 || depMaq.TM == 3 || depMaq.TipoActivo == 3)
                        {
                            if (!string.IsNullOrEmpty(depMaq.PolizaRefAlta))
                            {
                                int año = Convert.ToInt32(depMaq.PolizaRefAlta.Split('-')[0]);
                                int mes = Convert.ToInt32(depMaq.PolizaRefAlta.Split('-')[1]);
                                int poliza = Convert.ToInt32(depMaq.PolizaRefAlta.Split('-')[2]);
                                string tp = depMaq.PolizaRefAlta.Split('-')[3];
                                int linea = Convert.ToInt32(depMaq.PolizaRefAlta.Split('-')[4]);

                                var polizaAlta = _context.tblC_AF_PolizaAltaBaja.First
                                    (m =>
                                        m.Año == año && m.Mes == mes && m.Poliza == poliza &&
                                        m.TP == tp && m.Linea == linea && m.Estatus
                                    );

                                catMaqDep.IdPolizaReferenciaAlta = polizaAlta.Id;
                                if (depMaq.TipoActivo == 3)
                                {
                                    catMaqDep.Poliza.TipoActivo = 3;
                                }
                                else
                                {
                                    catMaqDep.Poliza.TipoActivo = polizaAlta.TipoActivo;
                                }
                            }
                            else
                            {
                                catMaqDep.IdPolizaReferenciaAlta = null;
                            }
                        }
                        else
                        {
                            catMaqDep.IdPolizaReferenciaAlta = null;
                        }

                        catMaqDep.IdUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                        catMaqDep.FechaModificacion = DateTime.Now;
                        catMaqDep.Poliza.IdUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                        catMaqDep.Poliza.FechaModificacion = DateTime.Now;
                        catMaqDep.CapturaAutomatica = false;

                        _context.SaveChanges();
                    }

                    List<tblM_CatMaquinaDepreciacion> registrosDepCatMaquina = _context.tblM_CatMaquinaDepreciacion.Where(x => x.IdCatMaquina == idCatMaquina && x.Estatus).ToList();

                    foreach (tblM_CatMaquinaDepreciacion item in registrosDepCatMaquina)
                    {
                        if (depMaquina.FirstOrDefault
                                (x =>
                                    x.Año == item.Poliza.Año && x.Mes == item.Poliza.Mes &&
                                    x.Poliza == item.Poliza.Poliza && x.TP == item.Poliza.TP &&
                                    x.Linea == item.Poliza.Linea) == null
                           )
                        {
                            item.Estatus = false;
                            item.Poliza.Estatus = false;
                            item.IdUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                            item.FechaModificacion = DateTime.Now;
                            item.Poliza.IdUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                            item.Poliza.FechaModificacion = DateTime.Now;

                            SaveChanges();
                        }
                    }

                    QuitarAlerta(depMaquina.First().IdCatMaquina);

                    transaction.Commit();

                    resultado.Add(SUCCESS, true);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al actualizar la BD. " + ex.ToString());
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarDepMaquina(int idDepMaquina)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

            using (var ctx = new MainContext(EmpresaEnum.Arrendadora))
            {
                using (var transactionArre = ctx.Database.BeginTransaction())
                {
                    using (DbContextTransaction transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            tblM_CatMaquina catMaq = ctx.tblM_CatMaquina.FirstOrDefault(x => x.id == idDepMaquina);

                            List<tblM_CatMaquinaDepreciacion> polizasCatMaq = _context.tblM_CatMaquinaDepreciacion.Where(x => x.IdCatMaquina == idDepMaquina && x.Estatus).ToList();

                            var MaqDepArre = ctx.tblM_CatMaquinaDepreciacion.FirstOrDefault(f => f.IdCatMaquina == idDepMaquina && f.Estatus);

                            catMaq.DepreciacionCapturada = MaqDepArre != null && vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora ? true : false;

                            ctx.tblM_CatMaquina.Attach(catMaq);
                            ctx.Entry(catMaq).Property(x => x.DepreciacionCapturada).IsModified = true;
                            ctx.SaveChanges();

                            foreach (tblM_CatMaquinaDepreciacion poliza in polizasCatMaq)
                            {
                                poliza.Poliza.Estatus = false;
                                poliza.Estatus = false;

                                poliza.Poliza.IdUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                poliza.Poliza.FechaModificacion = DateTime.Now;

                                poliza.IdUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                poliza.FechaModificacion = DateTime.Now;
                            }

                            _context.SaveChanges();

                            transaction.Commit();
                            transactionArre.Commit();

                            resultado.Add(SUCCESS, true);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            transaction.Rollback();

                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Ocurrió un error al actualizar la BD. " + ex.ToString());
                        }
                    }
                }
            }

            return resultado;
        }

        public Dictionary<string, object> ObtenerDepMaquina(int idDepMaq)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

            try
            {
                var aCEnkontrol = AreaCuentaEnkontrol();

                List<ActivoFijoRegInfoDepDTO> polizasCC = _context.tblM_CatMaquinaDepreciacion.Where(x => x.IdCatMaquina == idDepMaq && x.Estatus).Select
                    (x => new ActivoFijoRegInfoDepDTO
                    {
                        IdCatMaquinaDepreciacion = x.Id,
                        Año = x.Poliza.Año,
                        Mes = x.Poliza.Mes,
                        Poliza = x.Poliza.Poliza,
                        TP = x.Poliza.TP,
                        Linea = x.Poliza.Linea,
                        TM = x.Poliza.TM,
                        Cuenta = x.Poliza.Cuenta,
                        Subcuenta = x.Poliza.Subcuenta != null ? x.Poliza.Subcuenta.Value : 0,
                        SubSubcuenta = x.Poliza.SubSubcuenta != null ? x.Poliza.SubSubcuenta.Value : 0,
                        Monto = x.Poliza.Monto != null ? x.Poliza.Monto.Value : 0,
                        Concepto = x.Poliza.Concepto,
                        Factura = x.Poliza.Factura,
                        FechaFactura = x.Poliza.FechaMovimiento,
                        FechaMovimiento = x.Poliza.FechaMovimiento,
                        TipoActivo = x.Poliza.TipoActivo,
                        IdCatMaquina = x.IdCatMaquina,
                        FechaInicioDepreciacion = x.FechaInicioDepreciacion,
                        PorcentajeDepreciacion = x.PorcentajeDepreciacion,
                        MesesTotalesDepreciacion = x.MesesTotalesDepreciacion,
                        TipoDelMovimiento = x.TipoDelMovimiento,
                        CapturaPorSistema = x.CapturaAutomatica,
                        PolizaRefAlta = x.Poliza.TM == 2 || x.Poliza.TM == 3 ?
                            _context.tblC_AF_PolizaAltaBaja.Select(m => new {
                                id = m.Id,
                                polizaRef = m.Año + "-" + m.Mes + "-" + m.Poliza + "-" + m.TP + "-" + m.Linea
                            }).FirstOrDefault(p => p.id == x.IdPolizaReferenciaAlta && x.Estatus).polizaRef : "",
                    }).ToList();

                foreach (ActivoFijoRegInfoDepDTO infoPol in polizasCC)
                {
                    OdbcConsultaDTO odbcPolizasCC = new OdbcConsultaDTO();

                    odbcPolizasCC.consulta = string.Format(@"
                        SELECT TOP 1
                            MOV.scta AS Subcuenta,
                            MOV.sscta AS SubSubcuenta,
                            MOV.monto,
                            POL.fechapol,
                            MOV.concepto,
                            (SELECT DISTINCT TOP 1 (CAST(area AS varchar(4)) + '-' + CAST(cuenta AS varchar(4)) + ' - ' + descripcion) FROM si_area_cuenta WHERE area = MOV.area and cuenta = MOV.cuenta_oc) AS AreaCuenta
                        FROM sc_movpol AS MOV
                        INNER JOIN sc_polizas AS POL ON MOV.year = POL.year AND MOV.mes = POL.mes AND MOV.poliza = POL.poliza AND MOV.tp = POL.tp
                        WHERE MOV.year = ? AND MOV.mes = ? AND MOV.poliza = ? AND MOV.tp = ? AND MOV.linea = ?");

                    odbcPolizasCC.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "year",
                        tipo = OdbcType.Int,
                        valor = infoPol.Año
                    });

                    odbcPolizasCC.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "mes",
                        tipo = OdbcType.Int,
                        valor = infoPol.Mes
                    });

                    odbcPolizasCC.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "poliza",
                        tipo = OdbcType.Int,
                        valor = infoPol.Poliza
                    });

                    odbcPolizasCC.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "tp",
                        tipo = OdbcType.VarChar,
                        valor = infoPol.TP
                    });

                    odbcPolizasCC.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "linea",
                        tipo = OdbcType.Int,
                        valor = infoPol.Linea
                    });

                    ActivoFijoModPolDTO polizas_cc = _contextEnkontrol.Select<ActivoFijoModPolDTO>(EnkontrolAmbienteEnum.Prod, odbcPolizasCC).FirstOrDefault();

                    if (polizasExceptions.Contains(infoPol.Poliza))
                    {
                        infoPol.AreaCuenta = "0-0";
                    }
                    else
                    {
                        if (polizas_cc == null)
                        {
                            infoPol.Concepto = "POLIZA ELIMINADA";
                        }
                        else
                        {
                            infoPol.Subcuenta = polizas_cc.Subcuenta;
                            infoPol.SubSubcuenta = polizas_cc.SubSubcuenta;
                            infoPol.Monto = polizas_cc.Monto;
                            infoPol.FechaPol = polizas_cc.FechaPol;
                            infoPol.Concepto = polizas_cc.Concepto;
                            infoPol.AreaCuenta = string.IsNullOrEmpty(polizas_cc.AreaCuenta) ? "0-0" : polizas_cc.AreaCuenta;
                        }
                    }
                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, polizasCC);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al consultar la BD. " + ex.ToString());
            }

            return resultado;
        }

        private Dictionary<string, object> ObtenerPoliza(ActivoFijoAgregarPolizaDTO infoPoliza)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                OdbcConsultaDTO odbcPoliza = new OdbcConsultaDTO();
                odbcPoliza.consulta = string.Format(@"
                    SELECT TOP 1
                        MOV.year AS Año, MOV.mes, MOV.poliza, MOV.tp, MOV.linea, MOV.tm, MOV.cta AS Cuenta, MOV.scta AS Subcuenta,
                        MOV.sscta AS SubSubcuenta, FAC.factura, MOV.monto, FAC.fecha AS FechaFactura, POL.fechapol, MOV.concepto
                    FROM
                        sc_movpol AS MOV
                    INNER JOIN cc AS CC ON MOV.cc = CC.cc
                    INNER JOIN sc_polizas AS POL ON MOV.year = POL.year AND MOV.mes = POL.mes AND MOV.poliza = POL.poliza AND MOV.tp = POL.tp
                    LEFT JOIN sp_movprov AS FAC ON MOV.year = FAC.year AND MOV.mes = FAC.mes AND MOV.poliza = FAC.poliza AND FAC.es_factura = 'S'
                    WHERE
                        MOV.year = ? AND MOV.mes = ? AND MOV.poliza = ? AND MOV.tp = ? AND MOV.linea = ?");

                odbcPoliza.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "year",
                    tipo = OdbcType.Int,
                    valor = infoPoliza.Año
                });

                odbcPoliza.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "mes",
                    tipo = OdbcType.Int,
                    valor = infoPoliza.Mes
                });

                odbcPoliza.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "poliza",
                    tipo = OdbcType.Int,
                    valor = infoPoliza.Poliza
                });

                odbcPoliza.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "tp",
                    tipo = OdbcType.VarChar,
                    valor = infoPoliza.TP
                });

                odbcPoliza.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "linea",
                    tipo = OdbcType.Int,
                    valor = infoPoliza.Linea
                });

                var poliza = _contextEnkontrol.Select<ActivoFijoRegInfoDepDTO>(EnkontrolAmbienteEnum.Prod, odbcPoliza);

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, poliza);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrión un error al obtener la información de enkontrol: " + ex.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> ObtenerPolizasCC(string Cc)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

            try
            {
                var fechaActual = DateTime.Now;
                var añoActual = fechaActual.Year;
                var fechaTemp = fechaActual.AddMonths(-3);
                var mesActual = fechaTemp.Year == añoActual ? fechaTemp.Month : 1;

                var añoAnterior = fechaTemp.Year;
                var mesAnterior = fechaTemp.Month;

                OdbcConsultaDTO odbcPolizasCC = new OdbcConsultaDTO();

                var cuentasMaquina = _context.tblC_AF_CatalogoMaquina.Select(x => x.Cuenta).ToList();

                var infoDep = _context.tblC_AF_Cuentas.Where(x => cuentasMaquina.Contains(x.Cuenta)).ToList();

                var relacionSubCuentas = _context.tblC_AF_RelSubCuentas.Where(x => x.Estatus && cuentasMaquina.Contains(x.Cuenta.Cuenta) && !x.Excluir && !x.EsCuentaDepreciacion).ToList();

                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                {
                    odbcPolizasCC.consulta = string.Format(@"
                    SELECT
                        MOV.year AS Año, MOV.mes, MOV.poliza, MOV.tp, MOV.linea, MOV.tm, MOV.cta AS Cuenta, MOV.scta AS Subcuenta,
                        MOV.sscta AS SubSubcuenta, FAC.factura, MOV.monto, FAC.fecha AS FechaFactura, POL.fechapol, MOV.concepto,
                        (SELECT DISTINCT TOP 1 (CAST(area AS varchar(4)) + '-' + CAST(cuenta AS varchar(4)) + ' - ' + descripcion) FROM si_area_cuenta WHERE area = MOV.area and cuenta = MOV.cuenta_oc) AS AreaCuenta
                    FROM
                        sc_movpol AS MOV
                    INNER JOIN cc AS CC ON MOV.cc = CC.cc
                    INNER JOIN sc_polizas AS POL ON MOV.year = POL.year AND MOV.mes = POL.mes AND MOV.poliza = POL.poliza AND MOV.tp = POL.tp
                    LEFT JOIN sp_movprov AS FAC ON MOV.year = FAC.year AND MOV.mes = FAC.mes AND MOV.poliza = FAC.poliza AND FAC.es_factura = 'S'
                    WHERE
                        MOV.cta in {0} AND
                        (
                            (
                                MOV.year = {1} AND
                                MOV.mes >= {2}
                            ) OR
                            (
                                MOV.year = {3} AND
                                MOV.mes >= {4}
                            )
                        ) AND
                        MOV.tm IN (1, 2) AND
                        CC.descripcion = ?
                    ORDER BY POL.fechapol", cuentasMaquina.ToParamInValue(), añoAnterior, mesAnterior, añoActual, mesActual);

                    odbcPolizasCC.parametros.AddRange(cuentasMaquina.Select(cta => new OdbcParameterDTO
                    {
                        nombre = "cta",
                        tipo = OdbcType.Int,
                        valor = cta
                    }));

                    odbcPolizasCC.parametros.Add(new OdbcParameterDTO()
                    {
                        nombre = "descripcion",
                        tipo = System.Data.Odbc.OdbcType.VarChar,
                        valor = Cc
                    });
                }
                else
                {
                    odbcPolizasCC.consulta = string.Format(@"
                    SELECT
                        MOV.year AS Año, MOV.mes, MOV.poliza, MOV.tp, MOV.linea, MOV.tm, MOV.cta AS Cuenta, MOV.scta AS Subcuenta,
                        MOV.sscta AS SubSubcuenta, FAC.factura, MOV.monto, FAC.fecha AS FechaFactura, POL.fechapol, MOV.concepto,
                        (SELECT DISTINCT TOP 1 (CAST(area AS varchar(4)) + '-' + CAST(cuenta AS varchar(4)) + ' - ' + descripcion) FROM si_area_cuenta WHERE area = MOV.area and cuenta = MOV.cuenta_oc) AS AreaCuenta
                    FROM
                        sc_movpol AS MOV
                    INNER JOIN cc AS CC ON MOV.cc = CC.cc
                    INNER JOIN sc_polizas AS POL ON MOV.year = POL.year AND MOV.mes = POL.mes AND MOV.poliza = POL.poliza AND MOV.tp = POL.tp
                    LEFT JOIN sp_movprov AS FAC ON MOV.year = FAC.year AND MOV.mes = FAC.mes AND MOV.poliza = FAC.poliza AND FAC.es_factura = 'S'
                    WHERE
                        MOV.cta in {0} AND
                        MOV.year >= 2020 AND
                        MOV.tm IN (1, 2) AND
                        MOV.referencia like ?
                    ORDER BY POL.fechapol", cuentasMaquina.ToParamInValue());

                    odbcPolizasCC.parametros.AddRange(cuentasMaquina.Select(cta => new OdbcParameterDTO
                    {
                        nombre = "cta",
                        tipo = OdbcType.Int,
                        valor = cta
                    }));

                    odbcPolizasCC.parametros.Add(new OdbcParameterDTO()
                    {
                        nombre = "referencia",
                        tipo = System.Data.Odbc.OdbcType.VarChar,
                        valor = "%" + Cc + "%"
                    });
                }

                

                List<ActivoFijoRegInfoDepDTO> polizas_cc = _contextEnkontrol.Select<ActivoFijoRegInfoDepDTO>(EnkontrolAmbienteEnum.Prod, odbcPolizasCC);

                if (1 == 2/*polizas_cc.Count == 0*/)
                {
                    
                }
                else
                {
                    List<ActivoFijoRegInfoDepDTO> polCancelaciones = new List<ActivoFijoRegInfoDepDTO>();
                    List<ActivoFijoRegInfoDepDTO> polizas_cc_temp = new List<ActivoFijoRegInfoDepDTO>();

                    foreach (var poliza in polizas_cc)
                    {
                        if (relacionSubCuentas.Where(r => r.Cuenta.Cuenta == poliza.Cuenta && r.Subcuenta == poliza.Subcuenta && r.SubSubcuenta == poliza.SubSubcuenta).Count() > 0)
                        {
                            OdbcConsultaDTO odbcPolizasCCCancelaciones = new OdbcConsultaDTO();

                            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                            {
                                odbcPolizasCCCancelaciones.consulta = string.Format(@"
                            SELECT
                                MOV.year AS Año, MOV.mes, MOV.poliza, MOV.tp, MOV.linea, MOV.tm, MOV.cta AS Cuenta, MOV.scta AS Subcuenta, MOV.sscta AS SubSubcuenta,
                                FAC.factura, MOV.monto, FAC.fecha AS FechaFactura, POL.fechapol, MOV.concepto,
                                (SELECT DISTINCT TOP 1 (CAST(area AS varchar(4)) + '-' + CAST(cuenta AS varchar(4)) + ' - ' + descripcion) FROM si_area_cuenta WHERE area = MOV.area and cuenta = MOV.cuenta_oc) AS AreaCuenta
                            FROM
                                sc_movpol AS MOV
                            INNER JOIN cc AS CC ON MOV.cc = CC.cc
                            INNER JOIN sc_polizas AS POL ON MOV.year = POL.year AND MOV.mes = POL.mes AND MOV.poliza = POL.poliza AND MOV.tp = POL.tp
                            LEFT JOIN sp_movprov AS FAC ON MOV.year = FAC.year AND MOV.mes = FAC.mes AND MOV.poliza = FAC.poliza AND FAC.es_factura = 'S'
                            WHERE
                                MOV.cta = ? AND
                                (
                                    (
                                        MOV.year = {0} AND
                                        MOV.mes >= {1}
                                    ) OR
                                    (
                                         MOV.year = {2} AND
                                        MOV.mes >= {3}
                                     )
                                ) AND
                                MOV.tm = 3 AND
                                MOV.monto >= ? AND
                                MOV.monto <= ?
                            ORDER BY POL.fechapol", añoAnterior, mesAnterior, añoActual, mesActual);

                                odbcPolizasCCCancelaciones.parametros.Add(new OdbcParameterDTO()
                                {
                                    nombre = "cta",
                                    tipo = OdbcType.Int,
                                    valor = poliza.Cuenta
                                });

                                odbcPolizasCCCancelaciones.parametros.Add(new OdbcParameterDTO()
                                {
                                    nombre = "year",
                                    tipo = OdbcType.Int,
                                    valor = poliza.Año
                                });

                                odbcPolizasCCCancelaciones.parametros.Add(new OdbcParameterDTO()
                                {
                                    nombre = "monto",
                                    tipo = OdbcType.Decimal,
                                    valor = (poliza.Monto * -1) - .10M
                                });

                                odbcPolizasCCCancelaciones.parametros.Add(new OdbcParameterDTO()
                                {
                                    nombre = "monto",
                                    tipo = OdbcType.Decimal,
                                    valor = (poliza.Monto * -1) + .10M
                                });
                            }
                            else
                            {
                                odbcPolizasCCCancelaciones.consulta = string.Format(@"
                            SELECT
                                MOV.year AS Año, MOV.mes, MOV.poliza, MOV.tp, MOV.linea, MOV.tm, MOV.cta AS Cuenta, MOV.scta AS Subcuenta, MOV.sscta AS SubSubcuenta,
                                FAC.factura, MOV.monto, FAC.fecha AS FechaFactura, POL.fechapol, MOV.concepto,
                                (SELECT DISTINCT TOP 1 (CAST(area AS varchar(4)) + '-' + CAST(cuenta AS varchar(4)) + ' - ' + descripcion) FROM si_area_cuenta WHERE area = MOV.area and cuenta = MOV.cuenta_oc) AS AreaCuenta
                            FROM
                                sc_movpol AS MOV
                            INNER JOIN cc AS CC ON MOV.cc = CC.cc
                            INNER JOIN sc_polizas AS POL ON MOV.year = POL.year AND MOV.mes = POL.mes AND MOV.poliza = POL.poliza AND MOV.tp = POL.tp
                            LEFT JOIN sp_movprov AS FAC ON MOV.year = FAC.year AND MOV.mes = FAC.mes AND MOV.poliza = FAC.poliza AND FAC.es_factura = 'S'
                            WHERE
                                MOV.cta = ? AND
                                (
                                    (
                                        MOV.year = {0} AND
                                        MOV.mes >= {1}
                                    ) OR
                                    (
                                        MOV.year = {2} AND
                                        MOV.mes >= {3}
                                    )
                                ) AND
                                MOV.tm = 3 AND
                                MOV.monto >= ? AND
                                MOV.monto <= ? AND
                                MOV.referencia = ?
                            ORDER BY POL.fechapol", añoAnterior, mesAnterior, añoActual, mesActual);

                                odbcPolizasCCCancelaciones.parametros.Add(new OdbcParameterDTO()
                                {
                                    nombre = "cta",
                                    tipo = OdbcType.Int,
                                    valor = poliza.Cuenta
                                });

                                odbcPolizasCCCancelaciones.parametros.Add(new OdbcParameterDTO()
                                {
                                    nombre = "year",
                                    tipo = OdbcType.Int,
                                    valor = poliza.Año
                                });

                                odbcPolizasCCCancelaciones.parametros.Add(new OdbcParameterDTO()
                                {
                                    nombre = "monto",
                                    tipo = OdbcType.Decimal,
                                    valor = (poliza.Monto * -1) - .10M
                                });

                                odbcPolizasCCCancelaciones.parametros.Add(new OdbcParameterDTO()
                                {
                                    nombre = "monto",
                                    tipo = OdbcType.Decimal,
                                    valor = (poliza.Monto * -1) + .10M
                                });
                                odbcPolizasCCCancelaciones.parametros.Add(new OdbcParameterDTO()
                                {
                                    nombre = "referencia",
                                    tipo = OdbcType.NVarChar,
                                    valor = Cc
                                });
                            }

                            List<ActivoFijoRegInfoDepDTO> polizas_ccCancelaciones = _contextEnkontrol.Select<ActivoFijoRegInfoDepDTO>(EnkontrolAmbienteEnum.Prod, odbcPolizasCCCancelaciones);

                            if (polizas_ccCancelaciones != null && polizas_ccCancelaciones.Count > 0)
                            {
                                polCancelaciones.AddRange(polizas_ccCancelaciones);
                            }

                            if (poliza.TM == 1)
                            {
                                poliza.PorcentajeDepreciacion = infoDep.FirstOrDefault(x => x.Cuenta == poliza.Cuenta).PorcentajeDepreciacion;
                                poliza.MesesTotalesDepreciacion = infoDep.FirstOrDefault(x => x.Cuenta == poliza.Cuenta).MesesDeDepreciacion;
                            }
                        }
                        else
                        {
                            polizas_cc_temp.Add(poliza);
                        }
                    }

                    foreach (var item in polizas_cc_temp)
                    {
                        polizas_cc.Remove(item);
                    }

                    polizas_cc.AddRange(polCancelaciones);

                    //
                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                    {
                        //var buscarMas = ObtenerPolizasCCCargarExcel(Cc);

                        //if ((bool)buscarMas[SUCCESS])
                        //{
                        //    var buscarMas_encontradas = (List<ActivoFijoRegInfoDepDTO>)buscarMas[ITEMS];

                        //    foreach (var poliza in buscarMas_encontradas)
                        //    {
                        //        if (poliza.TM == 1)
                        //        {
                        //            poliza.PorcentajeDepreciacion = infoDep.FirstOrDefault(x => x.Cuenta == poliza.Cuenta).PorcentajeDepreciacion;
                        //            poliza.MesesTotalesDepreciacion = infoDep.FirstOrDefault(x => x.Cuenta == poliza.Cuenta).MesesDeDepreciacion;
                        //        }
                        //    }

                        //    if (polizas_cc.Count != 0)
                        //    {
                        //        foreach (var item in buscarMas_encontradas)
                        //        {
                        //            if (polizas_cc.FirstOrDefault(x => x.Año == item.Año && x.Mes == item.Mes && x.Poliza == item.Poliza && x.TP == item.TP && x.Linea == item.Linea) == null)
                        //            {
                        //                polizas_cc.Add(item);
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        polizas_cc.AddRange(buscarMas_encontradas);
                        //    }
                        //}
                    }
                    //

                    if (polizas_cc.Count == 0)
                    {
                        tblM_CatMaquina otroNumEconomico = null;
                        using (var ctx = new MainContext(EmpresaEnum.Arrendadora))
                        {
                            otroNumEconomico = ctx.tblM_CatMaquina.FirstOrDefault(f => f.noEconomico == Cc);
                        }
                        if (otroNumEconomico != null && otroNumEconomico.EconomicoCC != null)
                        {
                            return ObtenerPolizasCC(otroNumEconomico.EconomicoCC);
                            
                        }
                        else
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "No se encontró una poliza para el CC: " + Cc);
                        }
                    }
                    else
                    {
                        resultado.Add(SUCCESS, true);
                        resultado.Add(ITEMS, polizas_cc);
                    }
                }
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al consultar la base de datos de enkontrol. " + ex.ToString());
            }

            return resultado;
        }

        private void QuitarAlerta(int idMaquina)
        {
            var alertas = _context.tblP_Alerta.Where(x => x.userRecibeID == vSesiones.sesionUsuarioDTO.id && !x.visto && x.objID == idMaquina && x.sistemaID == 11).ToList();
            foreach (var alerta in alertas)
            {
                alerta.visto = true;
            }
            _context.SaveChanges();
        }
        #endregion

        #region PolizaDepreciacion
        public Respuesta FechaCaptura(int cuenta, bool esOverhaul)
        {
            var r = new Respuesta();

            try
            {
                var fechaInicioCapturaEnModulo = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? new DateTime(2020, 6, 1) : new DateTime(2021, 02, 1);
                if (cuenta == 1220 || cuenta == 1225 || cuenta == 1230)
                {
                    fechaInicioCapturaEnModulo = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? new DateTime(2020, 8, 1) : new DateTime(2021, 02, 1);
                }
                if ((cuenta == 1204 || cuenta == 1230 || cuenta == 1225 || cuenta == 1220) && vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                {
                    fechaInicioCapturaEnModulo = new DateTime(2021, 7, 1);
                }
                var fechaActual = DateTime.Now;
                var mesesDiferencia = ((fechaActual.Year - fechaInicioCapturaEnModulo.Year) * 12) + (fechaActual.Month - fechaInicioCapturaEnModulo.Month) + 1;

                var polizas = _context.tblC_AF_PolizasDetalle.Where(x => x.RelacionCuentaAño.Cuenta.Cuenta == cuenta && x.Estatus && x.RelacionCuentaAño.Subcuenta.EsOverhaul == esOverhaul && x.RelacionCuentaAño.Subcuenta.Cuenta.TipoCuentaId == (int)AFTipoCuentaEnum.Depreciacion).ToList();
                var esMaquinaria = _context.tblC_AF_Subcuentas.FirstOrDefault(x => x.EsMaquinaria && x.Cuenta.Cuenta == cuenta && x.Estatus && x.Cuenta.Estatus);

                var fechaSemana = new FechaSemanaDTO();

                for (int i = 1; i <= mesesDiferencia; i++)
                {
                    var semanasCapturadas = polizas.Where(x => x.Poliza.Año == fechaInicioCapturaEnModulo.Year && x.Poliza.Mes == fechaInicioCapturaEnModulo.Month && x.Estatus).ToList();
                    //var capturasMensuales = polizas.Where(x => x.Poliza.Año == fechaInicioCapturaEnModulo.Year && x.Poliza.Mes == fechaInicioCapturaEnModulo.Month && x.Estatus).GroupBy(g => g.Poliza.Poliza).Count();
                    var capturasMensuales = semanasCapturadas.Count > 0 ? semanasCapturadas.GroupBy(g => g.Poliza.Semana).OrderBy(o => o.Key).Last().First().Poliza.Semana : 0;
                    if (esMaquinaria != null)
                    {
                        if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan && fechaInicioCapturaEnModulo.Year == 2021 && fechaInicioCapturaEnModulo.Month == 2)
                        {
                            capturasMensuales++;
                        }

                        if (capturasMensuales >= (esOverhaul && fechaInicioCapturaEnModulo >= new DateTime(2021, 3, 1) ? 4 : 4))
                        {
                            if (esOverhaul && semanasCapturadas.GroupBy(x => x.Poliza.Semana).Count() >= 3)
                            {
                                fechaInicioCapturaEnModulo = fechaInicioCapturaEnModulo.AddMonths(1);
                                continue;
                            }
                            else
                            {
                                fechaInicioCapturaEnModulo = fechaInicioCapturaEnModulo.AddMonths(1);
                                continue;
                            }
                        }
                        else
                        {
                            //var ultimos4MiercolesDelMes = Ultimos4MiercolesDelMes(new DateTime(fechaInicioCapturaEnModulo.Year, fechaInicioCapturaEnModulo.Month, 1));
                            var ultimos4MiercolesDelMes = Ultimos4MartesDelMes(new DateTime(fechaInicioCapturaEnModulo.Year, fechaInicioCapturaEnModulo.Month, 1));
                            if (ultimos4MiercolesDelMes.Count == 5)
                            {
                                ultimos4MiercolesDelMes.RemoveAt(0);
                            }


                            if (esOverhaul)
                            {
                                var polizasCapturadasEnLaSemana = semanasCapturadas.Where(w => w.Poliza.Semana == capturasMensuales).GroupBy(g => g.PolizaId).Count();
                                if (capturasMensuales == 0)
                                {
                                    capturasMensuales = 1;
                                }
                                fechaSemana.Semana = polizasCapturadasEnLaSemana == 3 ? capturasMensuales + 1 : capturasMensuales;
                                capturasMensuales = fechaSemana.Semana - 1;
                            }
                            else
                            {
                                fechaSemana.Semana = capturasMensuales + 1;
                            }
                            
                            fechaSemana.Fecha = new DateTime(fechaInicioCapturaEnModulo.Year, fechaInicioCapturaEnModulo.Month, ultimos4MiercolesDelMes[capturasMensuales]);

                            r.Success = true;
                            r.Message = "Ok";
                            r.Value = fechaSemana;
                            return r;
                        }
                    }
                    else
                    {
                        if (capturasMensuales == 4)
                        {
                            fechaInicioCapturaEnModulo = fechaInicioCapturaEnModulo.AddMonths(1);
                            continue;
                        }
                        else
                        {
                            //var ultimos4MiercolesDelMes = Ultimos4MiercolesDelMes(new DateTime(fechaInicioCapturaEnModulo.Year, fechaInicioCapturaEnModulo.Month, 1));
                            var ultimos4MiercolesDelMes = Ultimos4MartesDelMes(new DateTime(fechaInicioCapturaEnModulo.Year, fechaInicioCapturaEnModulo.Month, 1));
                            if (ultimos4MiercolesDelMes.Count == 5)
                            {
                                ultimos4MiercolesDelMes.RemoveAt(0);
                            }

                            fechaSemana.Semana = capturasMensuales + 4;
                            fechaSemana.Fecha = new DateTime(fechaInicioCapturaEnModulo.Year, fechaInicioCapturaEnModulo.Month, ultimos4MiercolesDelMes[ultimos4MiercolesDelMes.Count - 1]);

                            r.Success = true;
                            r.Message = "Ok";
                            r.Value = fechaSemana;
                            return r;
                        }
                    }
                }

                if (!r.Success)
                {
                    r.Message = "La depreciación mensual de la cuenta: " + cuenta + " ya se encuentra capturada";
                }
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }

        public Respuesta EliminarPoliza(int polizaId)
        {
            var r = new Respuesta();

            using (var transaction = _context.Database.BeginTransaction())
            {
                using (var connEK = ConexionEnkontrol())
                {
                    using (var transactionEK = connEK.BeginTransaction())
                    {
                        try
                        {
                            var poliza = _context.tblC_AF_Polizas.First(x => x.Id == polizaId);
                            poliza.Estatus = false;
                            poliza.FechaModificacion = DateTime.Now;
                            poliza.UsuarioModificacionId = vSesiones.sesionUsuarioDTO.id;

                            foreach (var mov in poliza.PolizaDetalle)
                            {
                                mov.Estatus = false;
                            }

                            _context.SaveChanges();

                            var eliminarMovPoliza = "DELETE sc_movpol WHERE year = ? AND mes = ? AND poliza = ? AND tp = ?";

                            using (var cmd = new OdbcCommand(eliminarMovPoliza))
                            {
                                OdbcParameterCollection parameters = cmd.Parameters;
                                parameters.Clear();

                                parameters.Add("@year", OdbcType.Numeric).Value = poliza.Año;
                                parameters.Add("@mes", OdbcType.Numeric).Value = poliza.Mes;
                                parameters.Add("@poliza", OdbcType.Numeric).Value = poliza.Poliza;
                                parameters.Add("@tp", OdbcType.Char).Value = poliza.TipoPoliza;

                                cmd.Connection = transactionEK.Connection;
                                cmd.Transaction = transactionEK;
                                cmd.ExecuteNonQuery();
                            }

                            var eliminarPoliza = "DELETE sc_polizas WHERE year = ? AND mes = ? AND poliza = ? AND tp = ?";

                            using (var cmd = new OdbcCommand(eliminarPoliza))
                            {
                                OdbcParameterCollection parameters = cmd.Parameters;
                                parameters.Clear();

                                parameters.Add("@year", OdbcType.Numeric).Value = poliza.Año;
                                parameters.Add("@mes", OdbcType.Numeric).Value = poliza.Mes;
                                parameters.Add("@poliza", OdbcType.Numeric).Value = poliza.Poliza;
                                parameters.Add("@tp", OdbcType.Char).Value = poliza.TipoPoliza;

                                cmd.Connection = transactionEK.Connection;
                                cmd.Transaction = transactionEK;
                                cmd.ExecuteNonQuery();
                            }

                            transactionEK.Commit();
                            transaction.Commit();

                            r.Success = true;
                            r.Message = "Ok";
                        }
                        catch (Exception ex)
                        {
                            transactionEK.Rollback();
                            transaction.Rollback();

                            r.Message += ex.Message;
                        }
                    }
                }
            }

            return r;
        }

        public Respuesta RegistrarPoliza(List<PolizaGeneradaDTO> polizaGenerada)
        {
            var r = new Respuesta();

            using (var ctx = new MainContext(EmpresaEnum.Arrendadora))
            {
                using (var transactionArre = ctx.Database.BeginTransaction())
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        using (var connEK = ConexionEnkontrol())
                        {
                            using (var transactionEK = connEK.BeginTransaction())
                            {
                                try
                                {
                                    var areasCuentaEK = AreaCuentaEnkontrol();
                                    var areasCuentaEKAgregadas = new List<AreaCuentaDTO>();

                                    var polizaCaptura = new PolizaCapturaEnkontrolDTO();

                                    var sc_poliza = new PolizaEnkontrolDTO();
                                    sc_poliza.Year = polizaGenerada.First().Año;
                                    sc_poliza.Mes = polizaGenerada.First().Mes;
                                    sc_poliza.Poliza = polizaGenerada.First().Poliza;
                                    sc_poliza.TP = polizaGenerada.First().TipoPoliza;
                                    sc_poliza.FechaPol = new DateTime(sc_poliza.Year, sc_poliza.Mes, polizaGenerada.First().Dia);
                                    sc_poliza.Cargos = polizaGenerada.Where(x => x.TipoMovimiento == (int)AFTipoMovimientoEnum.Cargo).Sum(s => s.Monto);
                                    sc_poliza.Abonos = polizaGenerada.Where(x => x.TipoMovimiento == (int)AFTipoMovimientoEnum.Abono).Sum(s => s.Monto);
                                    sc_poliza.Generada = Convert.ToChar(Enum.GetName(typeof(AFModuloEnkontrolEnum), (int)AFModuloEnkontrolEnum.C));
                                    sc_poliza.Status = Convert.ToChar(Enum.GetName(typeof(AFEstatusPolizaEnum), (int)AFEstatusPolizaEnum.C));
                                    sc_poliza.Fecha_hora_crea = DateTime.Now;
                                    sc_poliza.Usuario_Crea = 13;

                                    polizaCaptura.Poliza = sc_poliza;

                                    foreach (var mov in polizaGenerada)
                                    {
                                        var sc_movpol = new PolizaDetalleEnkontrolDTO();
                                        sc_movpol.Year = mov.Año;
                                        sc_movpol.Mes = mov.Mes;
                                        sc_movpol.Poliza = mov.Poliza;
                                        sc_movpol.TP = mov.TipoPoliza;
                                        sc_movpol.Linea = mov.Linea;
                                        sc_movpol.Cta = mov.Cuenta;
                                        sc_movpol.Scta = mov.Subcuenta;
                                        sc_movpol.Sscta = mov.SubSubcuenta;
                                        sc_movpol.Digito = mov.Digito;
                                        sc_movpol.TM = mov.TipoMovimiento;
                                        sc_movpol.Referencia = mov.Referencia;
                                        sc_movpol.CC = mov.CC;
                                        sc_movpol.Concepto = mov.Concepto;
                                        sc_movpol.Monto = mov.Monto;
                                        sc_movpol.IClave = mov.IClave;
                                        sc_movpol.ITM = mov.ITM;
                                        sc_movpol.Area = mov.Area;
                                        sc_movpol.Cuenta_OC = mov.Cuenta_OC;
                                        sc_movpol.AreaCuentaDescripcion = mov.AreaCuentaDescripcion;

                                        polizaCaptura.Detalle.Add(sc_movpol);
                                    }

                                    OdbcConsultaDTO odbcUltimaPoliza = new OdbcConsultaDTO();
                                    odbcUltimaPoliza.consulta = "SELECT poliza FROM sc_polizas WHERE year = ? AND mes = ? AND tp = ?";
                                    odbcUltimaPoliza.parametros.Add(new OdbcParameterDTO()
                                    {
                                        nombre = "year",
                                        tipo = OdbcType.Int,
                                        valor = polizaCaptura.Poliza.Year
                                    });
                                    odbcUltimaPoliza.parametros.Add(new OdbcParameterDTO()
                                    {
                                        nombre = "mes",
                                        tipo = OdbcType.Int,
                                        valor = polizaCaptura.Poliza.Mes
                                    });
                                    odbcUltimaPoliza.parametros.Add(new OdbcParameterDTO()
                                    {
                                        nombre = "tp",
                                        tipo = OdbcType.Char,
                                        valor = polizaCaptura.Poliza.TP
                                    });
                                    List<PolizaDTO> polizas = _contextEnkontrol.Select<PolizaDTO>(EnkontrolAmbienteEnum.Prod, odbcUltimaPoliza);
                                    if (polizas.Count > 0)
                                    {
                                        polizaCaptura.Poliza.Poliza = polizas.OrderBy(o => o.Poliza).Last().Poliza + 1;
                                    }
                                    else
                                    {
                                        polizaCaptura.Poliza.Poliza = 1;
                                    }

                                    var insertPoliza = @"
                                                INSERT INTO
                                                    sc_polizas
                                                        (
                                                            year, mes, poliza, tp, fechapol, cargos, abonos, generada,
                                                            status, error, status_lock, fec_hora_movto, usuario_movto,
                                                            fecha_hora_crea, usuario_crea, status_carga_pol, concepto
                                                        )
                                                    VALUES
                                                        (
                                                            ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?
                                                        )
                                                ";
                                    using (var cmd = new OdbcCommand(insertPoliza))
                                    {
                                        OdbcParameterCollection parameters = cmd.Parameters;
                                        parameters.Clear();

                                        parameters.Add("@year", OdbcType.Numeric).Value = polizaCaptura.Poliza.Year;
                                        parameters.Add("@mes", OdbcType.Numeric).Value = polizaCaptura.Poliza.Mes;
                                        parameters.Add("@poliza", OdbcType.Numeric).Value = polizaCaptura.Poliza.Poliza;
                                        parameters.Add("@tp", OdbcType.Char).Value = polizaCaptura.Poliza.TP;
                                        parameters.Add("@fechapol", OdbcType.Date).Value = polizaCaptura.Poliza.FechaPol;
                                        parameters.Add("@cargos", OdbcType.Numeric).Value = polizaCaptura.Poliza.Cargos;
                                        parameters.Add("@abonos", OdbcType.Numeric).Value = polizaCaptura.Poliza.Abonos;
                                        parameters.Add("@generada", OdbcType.Char).Value = polizaCaptura.Poliza.Generada;
                                        parameters.Add("@status", OdbcType.Char).Value = polizaCaptura.Poliza.Status;
                                        parameters.Add("@error", OdbcType.VarChar).Value = string.Empty;
                                        parameters.Add("@status_lock", OdbcType.Char).Value = 'N';
                                        parameters.Add("@fec_hora_movto", OdbcType.DateTime).Value = polizaCaptura.Poliza.Fecha_hora_crea;
                                        parameters.Add("@usuario_movto", OdbcType.Char).Value = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? "151" : "290";
                                        parameters.Add("@fecha_hora_crea", OdbcType.DateTime).Value = polizaCaptura.Poliza.Fecha_hora_crea;
                                        parameters.Add("@usuario_crea", OdbcType.Char).Value = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? "151" : "290";
                                        parameters.Add("@status_carga_pol", OdbcType.VarChar).Value = DBNull.Value;
                                        parameters.Add("@concepto", OdbcType.VarChar).Value = "Póliza de Diario";

                                        cmd.Connection = transactionEK.Connection;
                                        cmd.Transaction = transactionEK;
                                        cmd.ExecuteNonQuery();
                                    }

                                    foreach (var mov in polizaCaptura.Detalle)
                                    {
                                        var insertMovPol = @"
                                                    INSERT INTO
                                                        sc_movpol 
                                                            (
                                                                year, mes, poliza, tp, linea, cta, scta, sscta, digito, tm, referencia, cc, concepto,
                                                                monto, iclave, itm, st_par, orden_compra, numpro, area, cuenta_oc
                                                            )
                                                        VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
                                                    ";
                                        using (var cmd = new OdbcCommand(insertMovPol))
                                        {
                                            OdbcParameterCollection parameters = cmd.Parameters;
                                            parameters.Clear();

                                            parameters.Add("@year", OdbcType.Numeric).Value = mov.Year;
                                            parameters.Add("@mes", OdbcType.Numeric).Value = mov.Mes;
                                            parameters.Add("@poliza", OdbcType.Numeric).Value = polizaCaptura.Poliza.Poliza;
                                            parameters.Add("@tp", OdbcType.Char).Value = mov.TP;
                                            parameters.Add("@linea", OdbcType.Numeric).Value = mov.Linea;
                                            parameters.Add("@cta", OdbcType.Numeric).Value = mov.Cta;
                                            parameters.Add("@scta", OdbcType.Numeric).Value = mov.Scta;
                                            parameters.Add("@sscta", OdbcType.Numeric).Value = mov.Sscta;
                                            parameters.Add("@digito", OdbcType.Numeric).Value = mov.Digito;
                                            parameters.Add("@tm", OdbcType.Numeric).Value = mov.TM;
                                            parameters.Add("@referencia", OdbcType.Char).Value = mov.Referencia;
                                            parameters.Add("@cc", OdbcType.Char).Value = mov.CC;
                                            parameters.Add("@concepto", OdbcType.Char).Value = mov.Concepto;
                                            parameters.Add("@monto", OdbcType.Numeric).Value = mov.Monto;
                                            parameters.Add("@iclave", OdbcType.Numeric).Value = mov.IClave;
                                            parameters.Add("@itm", OdbcType.Numeric).Value = mov.ITM;
                                            parameters.Add("@st_par", OdbcType.Char).Value = string.Empty;
                                            parameters.Add("@orden_compra", OdbcType.Numeric).Value = 0;
                                            parameters.Add("@numpro", OdbcType.Numeric).Value = 0;
                                            if (mov.Area != null && vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                                            {
                                                parameters.Add("@area", OdbcType.Numeric).Value = mov.Area.Value;
                                            }
                                            else
                                            {
                                                parameters.Add("@area", OdbcType.Numeric).Value = DBNull.Value;
                                            }
                                            if (mov.Area != null && vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                                            {
                                                parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = mov.Cuenta_OC.Value;
                                            }
                                            else
                                            {
                                                parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = DBNull.Value;
                                            }

                                            cmd.Connection = transactionEK.Connection;
                                            cmd.Transaction = transactionEK;
                                            cmd.ExecuteNonQuery();
                                        }

                                        if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                                        {
                                            var acEKEncontrada = areasCuentaEK.FirstOrDefault(x => x.Area == mov.Area.Value && x.Cuenta == mov.Cuenta_OC.Value && x.CC.ToUpper() == mov.CC.ToUpper());
                                            var acEKAgregada = areasCuentaEKAgregadas.FirstOrDefault(x => x.Area == mov.Area.Value && x.Cuenta == mov.Cuenta_OC.Value && x.CC == mov.CC.ToUpper());
                                            if (acEKEncontrada == null && acEKAgregada == null)
                                            {
                                                var insertAC = @"INSERT INTO si_area_cuenta (centro_costo, area, cuenta, descripcion, maquinaria, cc_activo, ac_cancelada) VALUES (?, ?, ?, ?, ?, ?, ?)";

                                                using (var cmd = new OdbcCommand(insertAC))
                                                {
                                                    OdbcParameterCollection parameters = cmd.Parameters;
                                                    parameters.Clear();

                                                    parameters.Add("@centro_costo", OdbcType.Char).Value = mov.CC.ToUpper();
                                                    parameters.Add("@area", OdbcType.Numeric).Value = mov.Area;
                                                    parameters.Add("@cuenta", OdbcType.Numeric).Value = mov.Cuenta_OC;
                                                    parameters.Add("@descripcion", OdbcType.Char).Value = mov.AreaCuentaDescripcion;
                                                    parameters.Add("@maquinaria", OdbcType.Bit).Value = 1;
                                                    parameters.Add("@cc_activo", OdbcType.Bit).Value = 1;
                                                    parameters.Add("@ac_cancelada", OdbcType.Bit).Value = 0;

                                                    cmd.Connection = transactionEK.Connection;
                                                    cmd.Transaction = transactionEK;
                                                    cmd.ExecuteNonQuery();
                                                }

                                                areasCuentaEKAgregadas.Add(new AreaCuentaDTO { Area = mov.Area.Value, Cuenta = mov.Cuenta_OC.Value, CC = mov.CC.ToUpper() });
                                            }
                                            else
                                            {
                                                if (acEKAgregada != null || (acEKEncontrada != null && acEKEncontrada.EsMaquinaria && acEKEncontrada.CcActivo && !acEKEncontrada.AcCancelada))
                                                {

                                                }
                                                else
                                                {
                                                    var updateAC = @"UPDATE si_area_cuenta SET maquinaria = 1, cc_activo = 1, ac_cancelada = 0 WHERE centro_costo = ? AND area = ? AND cuenta = ?";

                                                    using (var cmd = new OdbcCommand(updateAC))
                                                    {
                                                        OdbcParameterCollection parameters = cmd.Parameters;
                                                        parameters.Clear();

                                                        parameters.Add("@centro_costo", OdbcType.Bit).Value = true;
                                                        parameters.Add("@area", OdbcType.Bit).Value = true;
                                                        parameters.Add("@cuenta", OdbcType.Bit).Value = false;

                                                        cmd.Connection = transactionEK.Connection;
                                                        cmd.Transaction = transactionEK;
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (!polizaGenerada.Any(x => x.esOH141 == true))
                                    {
                                        var polizaSigoplan = new tblC_AF_Poliza();
                                        polizaSigoplan.Año = polizaCaptura.Poliza.Year;
                                        polizaSigoplan.Mes = polizaCaptura.Poliza.Mes;
                                        polizaSigoplan.Semana = polizaGenerada.First().Semana;
                                        polizaSigoplan.Poliza = polizaCaptura.Poliza.Poliza;
                                        polizaSigoplan.TipoPoliza = polizaCaptura.Poliza.TP;
                                        polizaSigoplan.FechaPoliza = polizaCaptura.Poliza.FechaPol;
                                        polizaSigoplan.Cargos = polizaCaptura.Poliza.Cargos;
                                        polizaSigoplan.Abonos = polizaCaptura.Poliza.Abonos;
                                        polizaSigoplan.ModuloEnkontrolId = (int)AFModuloEnkontrolEnum.C;
                                        polizaSigoplan.EstatusPolizaId = (int)AFEstatusPolizaEnum.C;
                                        polizaSigoplan.Estatus = true;
                                        polizaSigoplan.UsuarioCreacionId = vSesiones.sesionUsuarioDTO.id;
                                        polizaSigoplan.FechaCreacion = polizaCaptura.Poliza.Fecha_hora_crea;
                                        polizaSigoplan.UsuarioModificacionId = polizaSigoplan.UsuarioCreacionId;
                                        polizaSigoplan.FechaModificacion = polizaSigoplan.FechaCreacion;

                                        _context.tblC_AF_Polizas.Add(polizaSigoplan);
                                        _context.SaveChanges();

                                        var polizaDetalles = new List<tblC_AF_PolizaDetalle>();
                                        foreach (var mov in polizaCaptura.Detalle)
                                        {
                                            var polizaDetalleSigoplan = new tblC_AF_PolizaDetalle();
                                            polizaDetalleSigoplan.PolizaId = polizaSigoplan.Id;
                                            polizaDetalleSigoplan.Linea = mov.Linea;
                                            polizaDetalleSigoplan.RelacionCuentaAñoId = polizaGenerada.First(x => x.Linea == mov.Linea).RelacionCuentaAñoId;
                                            polizaDetalleSigoplan.TipoMovimientoId = mov.TM;
                                            polizaDetalleSigoplan.Referencia = mov.Referencia;
                                            polizaDetalleSigoplan.CC = mov.CC;
                                            polizaDetalleSigoplan.CatMaquinaId = polizaGenerada.First(x => x.Linea == mov.Linea).CatMaquinaId;
                                            polizaDetalleSigoplan.NumeroEconomico = polizaGenerada.First(x => x.Linea == mov.Linea).NumeroEconomico;
                                            polizaDetalleSigoplan.Concepto = mov.Concepto;
                                            polizaDetalleSigoplan.Monto = mov.Monto;
                                            polizaDetalleSigoplan.IClave = mov.IClave;
                                            polizaDetalleSigoplan.ITM = mov.ITM;
                                            polizaDetalleSigoplan.CcId = polizaGenerada.First(x => x.Linea == mov.Linea).CcId;
                                            polizaDetalleSigoplan.Area = mov.Area;
                                            polizaDetalleSigoplan.Cuenta_OC = mov.Cuenta_OC;
                                            polizaDetalleSigoplan.AreaCuenta = polizaGenerada.First(x => x.Linea == mov.Linea).AreaCuenta;
                                            polizaDetalleSigoplan.Estatus = true;

                                            polizaDetalles.Add(polizaDetalleSigoplan);
                                        }

                                        _context.tblC_AF_PolizasDetalle.AddRange(polizaDetalles);
                                        _context.SaveChanges();

                                        //
                                        // var polizasCapturadas = _context.tblC_AF_Polizas.Where(x => x.Estatus && x.FechaPoliza == polizaSigoplan.FechaPoliza);
                                        var polizasCapturadas = _context.tblC_AF_Polizas.Where
                                            (x =>
                                                x.Estatus &&
                                                x.Año == polizaSigoplan.Año &&
                                                x.Mes == polizaSigoplan.Mes &&
                                                x.Semana == polizaSigoplan.Semana
                                            );
                                        var polizasMaq = polizasCapturadas.Where(x => x.PolizaDetalle.Where(w => w.RelacionCuentaAño.Subcuenta.EsMaquinaria && w.TipoMovimientoId == 2 && w.Estatus).Count() > 0).Distinct();
                                        //
                                        //var polizasCapturadas = _context.tblC_AF_Polizas.Where(x => x.Estatus && x.FechaPoliza == polizaSigoplan.FechaPoliza).Count() == 4 ? true : false;
                                        //if (polizasCapturadas)
                                        if (polizasMaq.Count() == (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? 6 : 3))
                                        {
                                            var fechaCorte = polizaSigoplan.FechaPoliza;
                                            var corteInventario = ctx.tblM_CorteInventarioMaq.FirstOrDefault
                                                (x =>
                                                    x.Estatus &&
                                                    x.FechaCorte == fechaCorte &&
                                                    (
                                                        x.Bloqueado ||
                                                        x.BloqueadoConstruplan
                                                    )
                                                );
                                            if (corteInventario != null)
                                            {
                                                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                                                {
                                                    corteInventario.Bloqueado = false;
                                                }
                                                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                                                {
                                                    corteInventario.BloqueadoConstruplan = false;
                                                }

                                                ctx.SaveChanges();
                                            }
                                        }
                                    }

                                    transactionEK.Commit();
                                    transaction.Commit();
                                    transactionArre.Commit();

                                    var CCs = CCEnkontrol();

                                    var reporte = new ReportePolizaDTO();
                                    reporte.Poliza = sc_poliza.Year + "-" + sc_poliza.Mes + "-" + sc_poliza.Poliza + "-" + sc_poliza.TP;
                                    reporte.EsCC = true;
                                    reporte.CCInicial = CCs.First().CC;
                                    reporte.CCFinal = CCs.Last().CC;
                                    reporte.PolizaInicial = sc_poliza.Poliza;
                                    reporte.PolizaFinal = sc_poliza.Poliza;
                                    reporte.TipoPolizaInicial = sc_poliza.TP;
                                    reporte.TipoPolizaFinal = sc_poliza.TP;
                                    //reporte.PeriodoInicial = new DateTime(sc_poliza.Year, sc_poliza.Mes, 1);
                                    reporte.PeriodoInicial = sc_poliza.FechaPol;
                                    //reporte.PeriodoFinal = new DateTime(sc_poliza.Year, sc_poliza.Mes, DateTime.DaysInMonth(sc_poliza.Year, sc_poliza.Mes));
                                    reporte.PeriodoFinal = sc_poliza.FechaPol;
                                    reporte.ReporteResumido = false;
                                    reporte.PolizaPorHoja = true;
                                    reporte.IncluirFirmas = true;
                                    reporte.Estatus = Enum.GetName(typeof(AFEstatusPolizaEnum), AFEstatusPolizaEnum.C);
                                    reporte.Reviso = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? "CP. Jessica Galdean" : "CP. Liliana Lavandera Torres";
                                    reporte.Autorizo = "CP. Arturo Sánchez";

                                    r.Success = true;
                                    r.Message = "Ok";
                                    r.Value = reporte;
                                }
                                catch (Exception ex)
                                {
                                    transactionEK.Rollback();
                                    transaction.Rollback();

                                    r.Message += ex.Message;
                                }
                            }
                        }
                    }
                }
            }

            return r;
        }

        public Respuesta GenerarPoliza(int cuenta, int año, int mes, int semana, int dia, bool esOverhaul, int? idCuentaDepOverhaul)
        {
            var r = new Respuesta();
            var sesionEmpresaActual = vSesiones.sesionEmpresaActual;

            using (var ctx = new MainContext(EmpresaEnum.Arrendadora))
            {
                try
                {
                    var calcularDepreciacion = construirDetalles(new DateTime(año, mes, dia), new List<int>() { cuenta }, false);
                    var catMaq = ctx.tblM_CatMaquina.ToList();
                    var catCC = ctx.tblP_CC.ToList();
                    var areasCuentaEK = AreaCuentaEnkontrol();
                    var ccEK = CCEnkontrol();

                    if ((bool)calcularDepreciacion[SUCCESS])
                    {
                        calcularDepreciacion.Remove("ResumenSaldos");
                        calcularDepreciacion.Remove("ResumenDepreciacion");
                        calcularDepreciacion.Remove("DiferenciasContables");
                        calcularDepreciacion.Remove("DiferenciasContablesDep");
                        calcularDepreciacion.Remove("Totalizadores");
                        calcularDepreciacion.Remove("FechaHasta");

                        var depreciacionCalculada = (List<ActivoFijoDetalleCuentaDTO>)calcularDepreciacion[ITEMS];
                        var informacionCuenta = _context.tblC_AF_Cuenta.First(x => x.Cuenta == cuenta);
                        var informacionCuentasDepSegunCC = _context.tblC_AF_CuentasCostosSegunCC.Where(w => w.Estatus);
                        var informacionCuentasDep = _context.tblC_AF_RelacionesCuentaAño.Where
                            (x =>
                                x.Estatus && x.Año == año && x.Subcuenta.EsOverhaul == esOverhaul &&
                                (
                                    (x.Subcuenta.Cuenta.TipoCuenta.Id == (int)AFTipoCuentaEnum.Cargo && x.Cuenta.Cuenta == cuenta) ||
                                    (x.Subcuenta.Cuenta.TipoCuenta.Id == (int)AFTipoCuentaEnum.Depreciacion && x.Cuenta.Cuenta == cuenta)
                                )
                            ).ToList();

                        var fecha = new DateTime(año, mes, 1);
                        var cambioCuentaDepreciacion = _context.tblC_AF_CambioCuentaDepreciacion.Where(x => x.registroActivo && x.fechaAplica >= fecha).ToList();

                        //
                        var esOH141 = false;
                        var informacionCuentasDepTemporal = informacionCuentasDep.Select(x => x).ToList();
                        var depreciacionCalculadaTemporal = depreciacionCalculada.First().Detalles.Select(x => x).ToList();
                        if ((EmpresaEnum)vSesiones.sesionEmpresaActual == EmpresaEnum.Arrendadora)
                        {
                            if (idCuentaDepOverhaul == -141)
                            {
                                esOH141 = true;
                                idCuentaDepOverhaul = 1132;
                            }
                        }
                        var detallesPolizaGenerada = new List<PolizaGeneradaDTO>();
                        var detallesPolizaGeneradaAgrupados = new List<PolizaGeneradaDTO>();
                        do
                        {
                            if (esOverhaul)
                            {
                                informacionCuentasDep = informacionCuentasDep.Where
                                    (w =>
                                        (
                                            w.Subcuenta.Cuenta.TipoCuenta.Id == (int)AFTipoCuentaEnum.Depreciacion
                                        ) ||
                                        (
                                            w.Subcuenta.Cuenta.TipoCuenta.Id == (int)AFTipoCuentaEnum.Cargo &&
                                            w.Id == idCuentaDepOverhaul
                                        )
                                    ).ToList();

                                var __meses = informacionCuentasDep.Where(w => w.Subcuenta.Cuenta.TipoCuenta.Id == (int)AFTipoCuentaEnum.Cargo).Select(m => m.Subcuenta.meses).First();

                                informacionCuentasDep.RemoveAll
                                    (re =>
                                        re.Subcuenta.Cuenta.TipoCuenta.Id == (int)AFTipoCuentaEnum.Depreciacion &&
                                        re.Subcuenta.meses != __meses
                                    );

                                depreciacionCalculada.First().Detalles.RemoveAll(re => re.MesesMaximoDepreciacion != __meses);
                            }
                            //

                            List<tblC_AF_RelacionCuentaAño> cuentaSegunCC = new List<tblC_AF_RelacionCuentaAño>();
                            if (informacionCuentasDep.Count > 2)
                            {
                                cuentaSegunCC = informacionCuentasDep.Where(w => informacionCuentasDepSegunCC.Select(m => m.Cuenta.Id).Contains(w.Subcuenta.Cuenta.Id)).ToList();
                            }
                            foreach (var item in cuentaSegunCC)
                            {
                                informacionCuentasDep.Remove(item);
                            }

                            var cuentaDepreciacion = informacionCuentasDep.Where(x => x.CuentaMovimientoId != null && x.Cuenta.Cuenta == cuenta).ToList();

                            var linea = 0;

                            foreach (var detalle in depreciacionCalculada.First().Detalles)
                            {
                                if ((detalle.FechaBaja != null && detalle.FechaBaja.Value < new DateTime(año, mes, 1)) || (detalle.FechaCancelacion != null && !detalle.EsOverhaul)) { continue; }
                                if (detalle.MesesDepreciadosAñoAnterior + detalle.MesesDepreciadosAñoActual > detalle.MesesMaximoDepreciacion) { continue; }
                                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                                {
                                    if (detalle.EsOverhaul != esOverhaul) { continue; }
                                }
                                if (detalle.FechaInicioDepreciacion > new DateTime(año, mes, DateTime.DaysInMonth(año, mes))) { continue; }
                                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                                {
                                    if (detalle.EsOverhaul && esOverhaul && (detalle.MesesDepreciadosAñoAnterior + detalle.MesesDepreciadosAñoActual) == 0) { continue; }
                                }
                                else
                                {
                                    if (detalle.EsOverhaul && (detalle.MesesDepreciadosAñoAnterior + detalle.MesesDepreciadosAñoActual) == 0) { continue; }
                                }

                                if (detalle.DepreciacionTerminadaPorMeses)
                                {
                                    if (esOH141 && detalle.esOverhaul14_1Herradura)
                                    {

                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                //
                                if (new DateTime(detalle.FechaInicioDepreciacion.Year, detalle.FechaInicioDepreciacion.Month, 1) == new DateTime(año, mes, 1)) { continue; }
                                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                                {
                                    if (esOverhaul && detalle.FechaCancelacion != null && new DateTime(detalle.FechaCancelacion.Value.Year, detalle.FechaCancelacion.Value.Month, 1) < new DateTime(año, mes, 1) && detalle.DepreciacionMensual >= 0) { continue; }
                                }
                                else
                                {
                                    if (detalle.EsOverhaul && detalle.FechaCancelacion != null && new DateTime(detalle.FechaCancelacion.Value.Year, detalle.FechaCancelacion.Value.Month, 1) < new DateTime(año, mes, 1) && detalle.DepreciacionMensual >= 0) { continue; }
                                }
                                if (detalle.MesesDepreciadosAñoAnterior + detalle.MesesDepreciadosAñoActual == 0)
                                {
                                    continue;
                                }
                                if (detalle.DepreciacionMensual == 0)
                                {
                                    continue;
                                }
                                if (detalle.esOverhaul14_1 || (detalle.EsOverhaul && detalle.Area.HasValue && detalle.Area.Value == 14 && detalle.Cuenta_OC.HasValue && detalle.Cuenta_OC.Value == 1))
                                {
                                    continue;
                                }
                                if (esOH141 && !detalle.esOverhaul14_1Herradura)
                                {
                                    continue;
                                }

                                var aplicaCambioCuentaDepreciacion = cambioCuentaDepreciacion.FirstOrDefault(x => x.maquinaId == detalle.IdCatMaquina && x.poliza == detalle.Poliza);

                                //
                                foreach (var tipoCuenta in informacionCuentasDep.GroupBy(g => g.Subcuenta.Cuenta.TipoCuenta.Id).OrderBy(o => o.Key))
                                {
                                    linea++;

                                    var registroPolizaGenerada = new PolizaGeneradaDTO();
                                    registroPolizaGenerada.Año = año;
                                    registroPolizaGenerada.Mes = mes;
                                    registroPolizaGenerada.Semana = semana;
                                    registroPolizaGenerada.Dia = dia;
                                    registroPolizaGenerada.TipoPoliza = "03";
                                    registroPolizaGenerada.Linea = linea;
                                    registroPolizaGenerada.RelacionCuentaAñoId = tipoCuenta.First().Id;
                                    registroPolizaGenerada.Cuenta = tipoCuenta.First().Subcuenta.Cuenta.Cuenta;
                                    registroPolizaGenerada.Subcuenta = tipoCuenta.First().Subcuenta.Subcuenta;
                                    registroPolizaGenerada.SubSubcuenta = tipoCuenta.First().Subcuenta.SubSubcuenta;

                                    registroPolizaGenerada.Digito = 0;
                                    registroPolizaGenerada.TipoMovimiento = tipoCuenta.First().Subcuenta.Cuenta.TipoCuenta.Id == (int)AFTipoCuentaEnum.Cargo ? (int)AFTipoMovimientoEnum.Cargo : (int)AFTipoMovimientoEnum.Abono;

                                    if (aplicaCambioCuentaDepreciacion != null)
                                    {
                                        if (registroPolizaGenerada.TipoMovimiento == (int)AFTipoCuentaEnum.Cargo)
                                        {
                                            registroPolizaGenerada.Cuenta = aplicaCambioCuentaDepreciacion.cuentaCargo;
                                            registroPolizaGenerada.Subcuenta = aplicaCambioCuentaDepreciacion.subcuentaCargo;
                                            registroPolizaGenerada.SubSubcuenta = aplicaCambioCuentaDepreciacion.subsubcuentaCargo;
                                        }
                                        else
                                        {
                                            registroPolizaGenerada.Cuenta = aplicaCambioCuentaDepreciacion.cuentaDep;
                                            registroPolizaGenerada.Subcuenta = aplicaCambioCuentaDepreciacion.subcuentaDep;
                                            registroPolizaGenerada.SubSubcuenta = aplicaCambioCuentaDepreciacion.subsubcuentaDep;
                                        }
                                    }


                                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                                    {
                                        if (informacionCuenta.Subcuentas.FirstOrDefault(x => x.Estatus && x.EsMaquinaria && x.Cuenta.Cuenta != 1211) != null)
                                        {
                                            registroPolizaGenerada.CC = detalle.Cc;
                                            registroPolizaGenerada.Referencia = detalle.Clave;
                                        }
                                        else
                                        {
                                            if (informacionCuenta.Subcuentas.FirstOrDefault(x => x.Estatus && !x.EsMaquinaria && x.Cuenta.Cuenta == 1230) != null)
                                            {
                                                registroPolizaGenerada.CC = detalle.Cc;
                                                registroPolizaGenerada.Referencia = "DEP";
                                            }
                                            else
                                            {
                                                if (informacionCuenta.Subcuentas.FirstOrDefault(x => x.Estatus && x.EsMaquinaria && x.Cuenta.Cuenta == 1211) != null)
                                                {
                                                    var ac190 = ccEK.FirstOrDefault(x => x.CC == "190");

                                                    registroPolizaGenerada.CC = ac190.CC;
                                                    registroPolizaGenerada.Referencia = detalle.Clave;
                                                }
                                                else
                                                {
                                                    var ac001 = ccEK.FirstOrDefault(x => x.CC == "001");
                                                    registroPolizaGenerada.CC = ac001.CC;
                                                    registroPolizaGenerada.Referencia = "DEP";
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (registroPolizaGenerada.TipoMovimiento == (int)AFTipoCuentaEnum.Cargo)
                                        {
                                            var ctaCargo = cuentaSegunCC.FirstOrDefault(f => f.Subcuenta.Cuenta.TipoCuentaId == (int)AFTipoCuentaEnum.Cargo);
                                            if (ctaCargo != null && informacionCuentasDepSegunCC.Any(a => a.CcActivoFijo == detalle.Cc))
                                            {
                                                registroPolizaGenerada.RelacionCuentaAñoId = ctaCargo.Id;
                                                registroPolizaGenerada.Cuenta = ctaCargo.Subcuenta.Cuenta.Cuenta;
                                                registroPolizaGenerada.Subcuenta = ctaCargo.Subcuenta.Subcuenta;
                                                registroPolizaGenerada.SubSubcuenta = ctaCargo.Subcuenta.SubSubcuenta;
                                            }
                                            else
                                            {
                                                registroPolizaGenerada.RelacionCuentaAñoId = tipoCuenta.First().Id;
                                                registroPolizaGenerada.Cuenta = tipoCuenta.First().Subcuenta.Cuenta.Cuenta;
                                                registroPolizaGenerada.Subcuenta = tipoCuenta.First().Subcuenta.Subcuenta;
                                                registroPolizaGenerada.SubSubcuenta = tipoCuenta.First().Subcuenta.SubSubcuenta;
                                            }
                                        }


                                        registroPolizaGenerada.CC = detalle.Cc;
                                        registroPolizaGenerada.Referencia = detalle.Clave;

                                        //DEPRECIACION ESPECIAL -53662133.32
                                        if (detalle.esDepreciacionEspecialFija)
                                        {
                                            registroPolizaGenerada.Referencia = "0";
                                            if (registroPolizaGenerada.TipoMovimiento == (int)AFTipoMovimientoEnum.Abono)
                                            {
                                                registroPolizaGenerada.Cuenta = 1250;
                                                registroPolizaGenerada.Subcuenta = 2;
                                                registroPolizaGenerada.SubSubcuenta = 1;
                                            }
                                        }
                                        //
                                    }


                                    registroPolizaGenerada.CatMaquinaId = catMaq.Any(x => x.noEconomico == detalle.Clave) ? (int?)catMaq.First(x => x.noEconomico == detalle.Clave).id : null;
                                    registroPolizaGenerada.NumeroEconomico = registroPolizaGenerada.CatMaquinaId != null ? catMaq.First(x => x.id == registroPolizaGenerada.CatMaquinaId).noEconomico : null;
                                    registroPolizaGenerada.Concepto = cuentaDepreciacion.First(x => x.Subcuenta.Cuenta.TipoCuentaId == (int)AFTipoCuentaEnum.Depreciacion).Subcuenta.ConceptoDepreciacion;
                                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                                    {
                                        if (registroPolizaGenerada.NumeroEconomico == "CMP-32")
                                        {
                                            registroPolizaGenerada.CC = "988";
                                        }
                                    }
                                    if (informacionCuenta.Subcuentas.FirstOrDefault(x => x.Estatus && x.EsMaquinaria) != null)
                                    {
                                        registroPolizaGenerada.Monto = registroPolizaGenerada.TipoMovimiento == (int)AFTipoMovimientoEnum.Cargo ? (detalle.DepreciacionMensual / 4) : (detalle.DepreciacionMensual / 4) * -1;
                                    }
                                    else
                                    {
                                        registroPolizaGenerada.Monto = registroPolizaGenerada.TipoMovimiento == (int)AFTipoMovimientoEnum.Cargo ? (detalle.DepreciacionMensual) : (detalle.DepreciacionMensual) * -1;
                                    }

                                    registroPolizaGenerada.IClave = 0;
                                    registroPolizaGenerada.ITM = 0;

                                    if (registroPolizaGenerada.CatMaquinaId != null)
                                    {
                                        var cc = catCC.FirstOrDefault(x => x.areaCuenta == catMaq.First(y => y.id == registroPolizaGenerada.CatMaquinaId.Value).centro_costos);
                                        registroPolizaGenerada.CcId = cc != null ? (int?)cc.id : null;
                                    }

                                    if (informacionCuenta.Subcuentas.FirstOrDefault(x => x.Estatus && x.EsMaquinaria) != null)
                                    {
                                        registroPolizaGenerada.Area = detalle.Area;
                                        registroPolizaGenerada.Cuenta_OC = detalle.Cuenta_OC;
                                        registroPolizaGenerada.AreaCuenta = detalle.AreaCuenta;
                                        registroPolizaGenerada.AreaCuentaDescripcion = detalle.AreaCuentaDescripcion;
                                    }
                                    else
                                    {
                                        //ESTE IF NUNCA SERA VERDADERO.. NO LO BORRO PORQUE NO RECUERDO PARA QUE LO DEJE.. SALUDOS
                                        if (informacionCuenta.Subcuentas.FirstOrDefault(x => x.Estatus && x.EsMaquinaria && x.Cuenta.Cuenta == 1210) != null)
                                        {
                                            var depVarias = areasCuentaEK.First(x => x.Area == 16 && x.Cuenta == 1);

                                            registroPolizaGenerada.Area = depVarias.Area;
                                            registroPolizaGenerada.Cuenta_OC = depVarias.Cuenta;
                                            registroPolizaGenerada.AreaCuenta = depVarias.Area + "-" + depVarias.Cuenta + " - " + depVarias.Descripcion;
                                            registroPolizaGenerada.AreaCuentaDescripcion = depVarias.Descripcion;
                                        }
                                        else
                                        {
                                            registroPolizaGenerada.Area = detalle.Area;
                                            registroPolizaGenerada.Cuenta_OC = detalle.Cuenta_OC;
                                            registroPolizaGenerada.AreaCuenta = detalle.AreaCuenta;
                                            registroPolizaGenerada.AreaCuentaDescripcion = detalle.AreaCuentaDescripcion;
                                        }
                                    }

                                    detallesPolizaGenerada.Add(registroPolizaGenerada);
                                }
                            }

                            //
                            if ((esOverhaul || (sesionEmpresaActual == (int)EmpresaEnum.Construplan && !informacionCuenta.Subcuentas.Any(a => a.EsMaquinaria))) && !esOH141)
                            {
                                int cont = 1;
                                foreach (var cc in detallesPolizaGenerada.GroupBy(g => g.CC.ToUpper()))
                                {
                                    foreach (var cta in cc.GroupBy(g => g.Cuenta))
                                    {
                                        var registroPolizaGenerada = new PolizaGeneradaDTO();
                                        registroPolizaGenerada.Año = cta.First().Año;
                                        registroPolizaGenerada.Area = cta.First().Area;
                                        registroPolizaGenerada.AreaCuenta = cta.First().AreaCuenta;
                                        registroPolizaGenerada.AreaCuentaDescripcion = cta.First().AreaCuentaDescripcion;
                                        registroPolizaGenerada.CatMaquinaId = cta.First().CatMaquinaId;
                                        registroPolizaGenerada.CC = cc.Key;
                                        registroPolizaGenerada.CcId = cta.First().CcId;
                                        registroPolizaGenerada.Concepto = cta.First().Concepto;
                                        registroPolizaGenerada.Cuenta = cta.Key;
                                        registroPolizaGenerada.Cuenta_OC = cta.First().Cuenta_OC;
                                        registroPolizaGenerada.Dia = cta.First().Dia;
                                        registroPolizaGenerada.Digito = cta.First().Digito;
                                        registroPolizaGenerada.IClave = cta.First().IClave;
                                        registroPolizaGenerada.ITM = cta.First().ITM;
                                        registroPolizaGenerada.Linea = cont;
                                        registroPolizaGenerada.Mes = cta.First().Mes;
                                        registroPolizaGenerada.Monto = cta.Sum(s => s.Monto);
                                        registroPolizaGenerada.NumeroEconomico = cta.First().NumeroEconomico;
                                        registroPolizaGenerada.Poliza = cta.First().Poliza;
                                        registroPolizaGenerada.Referencia = cta.First().Referencia;
                                        registroPolizaGenerada.RelacionCuentaAñoId = cta.First().RelacionCuentaAñoId;
                                        registroPolizaGenerada.Semana = cta.First().Semana;
                                        registroPolizaGenerada.Subcuenta = cta.First().Subcuenta;
                                        registroPolizaGenerada.SubSubcuenta = cta.First().SubSubcuenta;
                                        registroPolizaGenerada.TipoMovimiento = cta.First().TipoMovimiento;
                                        registroPolizaGenerada.TipoPoliza = cta.First().TipoPoliza;

                                        detallesPolizaGeneradaAgrupados.Add(registroPolizaGenerada);
                                        cont++;
                                    }
                                }
                            }
                            //
                            if ((EmpresaEnum)vSesiones.sesionEmpresaActual == EmpresaEnum.Arrendadora && esOH141)
                            {
                                informacionCuentasDep = informacionCuentasDepTemporal.Select(x => x).ToList();
                                depreciacionCalculada.First().Detalles = depreciacionCalculadaTemporal.Select(x => x).ToList();

                                switch (idCuentaDepOverhaul)
                                {
                                    case 1132:
                                        idCuentaDepOverhaul = 1144;
                                        break;
                                    case 1144:
                                        idCuentaDepOverhaul = 1145;
                                        break;
                                    default:
                                        idCuentaDepOverhaul = -1;
                                        break;
                                }
                            }
                        } while ((EmpresaEnum)vSesiones.sesionEmpresaActual == EmpresaEnum.Arrendadora && esOH141 && idCuentaDepOverhaul != -1);

                        if (esOH141)
                        {
                            int cont = 1;
                            foreach (var cc in detallesPolizaGenerada.GroupBy(g => g.CC.ToUpper()))
                            {
                                foreach (var cta in cc.GroupBy(g => new { g.Cuenta, g.Subcuenta, g.SubSubcuenta }))
                                {
                                    var registroPolizaGenerada = new PolizaGeneradaDTO();
                                    registroPolizaGenerada.Año = cta.First().Año;
                                    registroPolizaGenerada.Area = cta.First().Area;
                                    registroPolizaGenerada.AreaCuenta = cta.First().AreaCuenta;
                                    registroPolizaGenerada.AreaCuentaDescripcion = cta.First().AreaCuentaDescripcion;
                                    registroPolizaGenerada.CatMaquinaId = cta.First().CatMaquinaId;
                                    registroPolizaGenerada.CC = cc.Key;
                                    registroPolizaGenerada.CcId = cta.First().CcId;
                                    registroPolizaGenerada.Concepto = cta.First().Concepto;
                                    registroPolizaGenerada.Cuenta = cta.First().Cuenta;
                                    registroPolizaGenerada.Cuenta_OC = cta.First().Cuenta_OC;
                                    registroPolizaGenerada.Dia = cta.First().Dia;
                                    registroPolizaGenerada.Digito = cta.First().Digito;
                                    registroPolizaGenerada.IClave = cta.First().IClave;
                                    registroPolizaGenerada.ITM = cta.First().ITM;
                                    registroPolizaGenerada.Linea = cont;
                                    registroPolizaGenerada.Mes = cta.First().Mes;
                                    registroPolizaGenerada.Monto = cta.Sum(s => s.Monto);
                                    registroPolizaGenerada.NumeroEconomico = cta.First().NumeroEconomico;
                                    registroPolizaGenerada.Poliza = cta.First().Poliza;
                                    registroPolizaGenerada.Referencia = cta.First().Referencia;
                                    registroPolizaGenerada.RelacionCuentaAñoId = cta.First().RelacionCuentaAñoId;
                                    registroPolizaGenerada.Semana = cta.First().Semana;
                                    registroPolizaGenerada.Subcuenta = cta.First().Subcuenta;
                                    registroPolizaGenerada.SubSubcuenta = cta.First().SubSubcuenta;
                                    registroPolizaGenerada.TipoMovimiento = cta.First().TipoMovimiento;
                                    registroPolizaGenerada.TipoPoliza = cta.First().TipoPoliza;
                                    registroPolizaGenerada.esOH141 = true;

                                    detallesPolizaGeneradaAgrupados.Add(registroPolizaGenerada);
                                    cont++;
                                }
                            }
                        }

                        r.Success = true;
                        r.Message = "Ok";
                        r.Value = esOverhaul || (sesionEmpresaActual == (int)EmpresaEnum.Construplan && !informacionCuenta.Subcuentas.Any(a => a.EsMaquinaria)) ? detallesPolizaGeneradaAgrupados : detallesPolizaGenerada;
                    }
                    else
                    {
                        r.Message += (string)calcularDepreciacion[MESSAGE];
                    }
                }
                catch (Exception ex)
                {
                    vSesiones.sesionEmpresaActual = sesionEmpresaActual;
                    r.Message += ex.Message;
                }
            }

            return r;
        }

        public Respuesta ObtenerPolizaDetalle(int año, int mes, int poliza)
        {
            var r = new Respuesta();

            try
            {
                OdbcConsultaDTO odbcPoliza = new OdbcConsultaDTO();
                odbcPoliza.consulta = string.Format(@"
                    SELECT
                        POL.fechapol AS FechaPoliza,
                        POL.year AS Año,
                        POL.mes AS Mes,
                        POL.tp AS TipoPoliza,
                        POL.poliza AS Poliza,
                        CTA.descripcion AS DescripcionCuenta,
                        MOV.linea AS Linea,
                        MOV.cta As Cuenta,
                        MOV.scta AS Subcuenta,
                        MOV.sscta AS SubSubcuenta,
                        MOV.tm AS TipoMovimiento,
                        MOV.referencia AS Referencia,
                        MOV.cc AS CC,
                        CC.descripcion AS DescripcionCC,
                        MOV.Concepto AS Concepto,
                        MOV.monto AS Monto,
                        MOV.area AS Area,
                        MOV.cuenta_oc AS Cuenta_OC,
                        (SELECT TOP 1 descripcion FROM si_area_cuenta WHERE area = MOV.area AND cuenta = MOV.cuenta_oc) AS AreaCuenta
                    FROM
                        sc_movpol AS MOV
                    INNER JOIN
                        sc_polizas AS POL ON POL.year = MOV.year AND POL.mes = MOV.mes AND POL.tp = MOV.tp AND POL.poliza = MOV.poliza
                    INNER JOIN
                        catcta AS CTA ON CTA.cta = MOV.cta AND CTA.scta = MOV.scta AND CTA.sscta = MOV.sscta
                    INNER JOIN
                        cc AS CC ON CC.cc = MOV.cc
                    WHERE
                        MOV.year = ? AND
                        MOV.mes = ? AND
                        MOV.tp = '03' AND
                        MOV.poliza = ?");
                odbcPoliza.parametros.Add(new OdbcParameterDTO()
                {
                    nombre = "year",
                    tipo = OdbcType.Int,
                    valor = año
                });
                odbcPoliza.parametros.Add(new OdbcParameterDTO()
                {
                    nombre = "mes",
                    tipo = OdbcType.Int,
                    valor = mes
                });
                odbcPoliza.parametros.Add(new OdbcParameterDTO()
                {
                    nombre = "poliza",
                    tipo = OdbcType.Int,
                    valor = poliza
                });
                List<PolizaDetalleDTO> polizaDetalle = _contextEnkontrol.Select<PolizaDetalleDTO>(EnkontrolAmbienteEnum.Prod, odbcPoliza);

                r.Success = true;
                r.Message = "Ok";
                r.Value = polizaDetalle;
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }

        public Respuesta ObtenerPolizasDepreciacion(int año, int mes, int cuenta)
        {
            var r = new Respuesta();

            try
            {
                var infoCuenta = _context.tblC_AF_Cuentas.FirstOrDefault(x => x.Cuenta == cuenta && x.Estatus);
                var relSubcuentas = infoCuenta.SubCuentas.Where(x => x.EsCuentaDepreciacion && x.Estatus && x.Año == año).ToList();

                List<PolizaDTO> polizas = new List<PolizaDTO>();
                

                //foreach (var pol in polizas)
                //{
                //    var polSigoplan = _context.tblC_AF_Polizas.FirstOrDefault(x => x.Estatus && x.Año == pol.Año && x.Mes == pol.Mes && x.Poliza == pol.Poliza && x.TipoPoliza == pol.TipoPoliza);
                //    pol.Sigoplan = polSigoplan != null ? true : false;
                //    pol.IdPolSigoplan = polSigoplan != null ? (int?)polSigoplan.Id : null;
                //}

                var polizasSigoplan = _context.tblC_AF_Polizas.Where(x => x.Estatus && x.Año == año && x.Mes == mes && x.PolizaDetalle.FirstOrDefault(y => y.RelacionCuentaAño.Cuenta.Cuenta == cuenta && y.Estatus) != null).ToList();
                var ultimaPolizaCapturada = _context.tblC_AF_Polizas.Where(x => x.Estatus && x.PolizaDetalle.FirstOrDefault(y => y.RelacionCuentaAño.Cuenta.Cuenta == cuenta && y.Estatus) != null).OrderByDescending(o => o.Id).FirstOrDefault();

                foreach (var cuentaDep in relSubcuentas.GroupBy(g => g.CuentaDepreciacion))
                {
                    foreach (var subcuenta in cuentaDep.GroupBy(g => g.Subcuenta))
                    {
                        OdbcConsultaDTO odbcPolizas = new OdbcConsultaDTO();
                        odbcPolizas.consulta = string.Format(@"
                            SELECT DISTINCT
                                POL.year AS Año, POL.mes AS Mes, POL.tp AS TipoPoliza, POL.poliza AS Poliza, MOV.cta AS Cuenta,
                                POL.cargos AS Cargo, POL.abonos AS Abono, CTA.descripcion AS Descripcion, POL.fechapol AS FechaPoliza,
                                POL.generada AS Generada, POL.status AS Estatus
                            FROM
                                sc_movpol AS MOV
                            INNER JOIN
                                sc_polizas AS POL ON POL.year = MOV.year AND POL.mes = MOV.mes AND POL.tp = MOV.tp AND POL.poliza = MOV.poliza
                            INNER JOIN
                                catcta AS CTA ON CTA.cta = MOV.cta AND CTA.scta = MOV.scta AND CTA.sscta = MOV.sscta
                            WHERE
                                POL.year = ? AND
                                POL.mes = ? AND
                                POL.tp = '03' AND
                                MOV.cta = ? AND
                                MOV.scta = ? AND
                                MOV.sscta IN {0}", subcuenta.Select(m => m.SubSubcuenta).ToParamInValue());
                        odbcPolizas.parametros.Add(new OdbcParameterDTO()
                        {
                            nombre = "year",
                            tipo = OdbcType.Int,
                            valor = año
                        });
                        odbcPolizas.parametros.Add(new OdbcParameterDTO()
                        {
                            nombre = "mes",
                            tipo = OdbcType.Int,
                            valor = mes
                        });
                        odbcPolizas.parametros.Add(new OdbcParameterDTO()
                        {
                            nombre = "cta",
                            tipo = OdbcType.Int,
                            valor = cuentaDep.Key
                        });
                        odbcPolizas.parametros.Add(new OdbcParameterDTO()
                        {
                            nombre = "scta",
                            tipo = OdbcType.Int,
                            valor = subcuenta.Key
                        });
                        odbcPolizas.parametros.AddRange(subcuenta.Select(m => new OdbcParameterDTO()
                        {
                            nombre = "sscta",
                            tipo = OdbcType.Int,
                            valor = m.SubSubcuenta
                        }).ToList());

                        List<PolizaDTO> polizasTemporal = _contextEnkontrol.Select<PolizaDTO>(EnkontrolAmbienteEnum.Prod, odbcPolizas);
                        polizas.AddRange(polizasTemporal);
                    }
                }

                foreach (var pol in polizas)
                {
                    //var polSigoplan = _context.tblC_AF_Polizas.FirstOrDefault(x => x.Estatus && x.Año == pol.Año && x.Mes == pol.Mes && x.Poliza == pol.Poliza && x.TipoPoliza == pol.TipoPoliza);
                    var polSigoplan = polizasSigoplan.FirstOrDefault(x => x.Poliza == pol.Poliza && x.TipoPoliza == pol.TipoPoliza);
                    pol.Sigoplan = polSigoplan != null ? true : false;
                    pol.IdPolSigoplan = polSigoplan != null ? (int?)polSigoplan.Id : null;
                    pol.UltimaSigoplan = polSigoplan == ultimaPolizaCapturada ? true : false;
                }

                var polizasU = new List<PolizaDTO>();
                

                foreach (var item in polizas)
                {
                    var polizaU = polizasU.FirstOrDefault(e => e.Año == item.Año && e.Mes == item.Mes && e.TipoPoliza == item.TipoPoliza && e.Poliza == item.Poliza);

                    if (polizaU == null)
                    {
                        polizasU.Add(item);
                    }
                }

                r.Success = true;
                r.Message = "Ok";
                r.Value = polizasU;
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }

        public Dictionary<string, object> CuentasDepOverhaul()
        {
            var r = new Dictionary<string, object>();

            try
            {
                var cuentasDepOverhaul = _context.tblC_AF_RelacionesCuentaAño.Where
                    (w =>
                        w.Subcuenta.EsMaquinaria &&
                        w.Subcuenta.EsOverhaul &&
                        w.Estatus &&
                        w.Subcuenta.Estatus &&
                        w.Año == DateTime.Now.Year &&
                        w.Subcuenta.Cuenta.TipoCuentaId == (int)AFTipoCuentaEnum.Cargo
                    ).Select
                    (m => new ComboDTO {
                        Text = "(" + m.Subcuenta.meses + " meses) " + m.Subcuenta.Cuenta.Cuenta + "-" + m.Subcuenta.Subcuenta + "-" + m.Subcuenta.SubSubcuenta,
                        Prefijo = m.Subcuenta.meses.ToString(),
                        Value = m.Id.ToString()
                    }).ToList();

                r.Add(SUCCESS, true);
                r.Add(ITEMS, cuentasDepOverhaul);
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, ex.Message);
            }

            return r;
        }

        private OdbcConnection ConexionEnkontrol()
        {
            //return new Conexion().ConnectArrendaroraPrueba();
            //return new Conexion().ConnectPrueba();
            return new Conexion().Connect();
        }
        #endregion

        #region TabuladorDepreciacion
        public Dictionary<string, object> GetCentrosCostos()
        {
            var resultado = new Dictionary<string, object>();

            using (var ctx = new MainContext(EmpresaEnum.Arrendadora))
            {
                try
                {
                    var centrosCostos = ctx.tblM_CatMaquina.Where(x => x.DepreciacionCapturada).Select(m => new ComboDTO
                    {
                        Value = m.id.ToString(),
                        Text = m.noEconomico,
                        Prefijo = m.id.ToString()
                    }).ToList();

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, centrosCostos);
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Error al intentar obtener los centros de costo: " + ex.ToString());
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetPeriodosDepreciacion(int IdCatMaquina)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var periodosDep = _context.tblM_CatMaquinaDepreciacion.Where(x => x.IdCatMaquina == IdCatMaquina && x.Estatus && x.FechaInicioDepreciacion <= DateTime.Now).OrderBy(x => x.Poliza.FechaMovimiento).ToList();

                foreach (var periodo in periodosDep.Where(x => x.Poliza.TM == 3))
                {
                    var eliminarPeriodo = periodosDep.First(x => x.IdPoliza == periodo.IdPolizaReferenciaAlta);

                    periodosDep.Remove(eliminarPeriodo);
                }

                periodosDep.RemoveAll(x => x.Poliza.TM == 3 || x.Poliza.TipoActivo != 1);

                var periodos = periodosDep.Select(m => new ComboDTO
                {
                    Value = m.Id.ToString(),
                    Text = m.FechaInicioDepreciacion.Value.ToShortDateString(),
                    Prefijo = m.Id.ToString()
                }).ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, periodos);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Error al intentar obtener los periodos de depreciación: " + ex.ToString());
            }

            return resultado;
        }

        public Dictionary<string, object> GetDepMaquinas(int? maquinaActiva, int? cuenta, string noEconomico, List<int> tipoMovimiento, List<string> fAreasCuenta, DateTime? fechaHasta, int? cuentaOverhaul, DateTime? fecha, bool todosLosEconomicosMaquinaria = false)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                tipoMovimiento = tipoMovimiento == null ? new List<int>() : tipoMovimiento;

                var cuentasMaquinaria = _context.tblC_AF_Cuentas.Where(x => x.EsMaquinaria && x.Estatus).ToList();

                var ctasMaq = cuentasMaquinaria.Select(x => x.Cuenta);

                if ((cuenta != null && ctasMaq.Contains(cuenta.Value)) || !string.IsNullOrEmpty(noEconomico) || todosLosEconomicosMaquinaria)
                {
                    var listaCuentas = new List<int>();
                    if (cuenta != null)
                    {
                        listaCuentas = new List<int> { cuenta.Value };
                    }
                    else
                    {
                        listaCuentas = ctasMaq.ToList();
                    }
                    var resultadoConstruccionCedula = construirDetalles(fecha.HasValue ? fecha.Value : fechaHasta != null ? fechaHasta.Value : DateTime.Now, listaCuentas, false, noEconomico);
                    if ((bool)resultadoConstruccionCedula[SUCCESS])
                    {
                        resultadoConstruccionCedula.Remove("ResumenSaldos");
                        resultadoConstruccionCedula.Remove("ResumenDepreciacion");
                        resultadoConstruccionCedula.Remove("DiferenciasContables");
                        resultadoConstruccionCedula.Remove("DiferenciasContablesDep");
                        resultadoConstruccionCedula.Remove("Totalizadores");
                        resultadoConstruccionCedula.Remove("FechaHasta");

                        var depreciacionCalculada = (List<ActivoFijoDetalleCuentaDTO>)resultadoConstruccionCedula[ITEMS];
                        var depMaquinas = new List<ActivoFijoDetalleDTO>();
                        var lista_dep_maquinas = new List<ActivoFijoDepMaquinaDTO>();
                        var fechaActual = DateTime.Now;

                        if (string.IsNullOrEmpty(noEconomico))
                        {
                            if (todosLosEconomicosMaquinaria)
                            {
                                foreach (var item in depreciacionCalculada)
                                {
                                    depMaquinas.AddRange(item.Detalles);
                                }
                            }
                            else
                            {
                                depMaquinas = depreciacionCalculada.First().Detalles;
                            }
                        }
                        else
                        {
                            foreach (var item in depreciacionCalculada)
                            {
                                if (item.Detalles != null)
                                {
                                    var datos = item.Detalles.Where(x => x.Clave != null && x.Clave.ToUpper() == noEconomico.ToUpper()).ToList();
                                    if (datos != null && datos.Count() > 0)
                                    {
                                        depMaquinas.AddRange(datos);
                                    }
                                }
                            }
                        }

                        foreach (var detalle in depMaquinas)
                        {
                            var dep_maq = new ActivoFijoDepMaquinaDTO();

                            dep_maq.IdDepMaquina = detalle.IdDepMaquina != null ? detalle.IdDepMaquina.Value : 0;
                            dep_maq.EsExtraCatMaqDep = detalle.EsExtraCatMaqDep;
                            dep_maq.IdCatMaquina = detalle.IdCatMaquina != null ? detalle.IdCatMaquina.Value : 0;
                            dep_maq.CC = detalle.Cc;
                            dep_maq.NoEconomico = detalle.Clave;
                            dep_maq.TipoMovimiento = detalle.TipoMovimiento;
                            dep_maq.TipoActivo = detalle.TipoActivo;
                            dep_maq.AreaCuenta = detalle.AreaCuenta;
                            dep_maq.Descripcion = detalle.Descripcion;
                            dep_maq.Factura = detalle.Factura;
                            dep_maq.Poliza = detalle.Poliza;
                            dep_maq.MesesTotalesDepreciacion = detalle.MesesMaximoDepreciacion;
                            dep_maq.FechaBaja = detalle.FechaBaja != null ? (DateTime?)detalle.FechaBaja.Value : null;
                            if (detalle.EsOverhaul && detalle.FechaCancelacion != null)
                            {
                                dep_maq.FechaBaja = detalle.FechaCancelacion;
                            }
                            dep_maq.FechaInicioDepreciacion = detalle.FechaInicioDepreciacion;
                            dep_maq.PorcentajeDepreciacion = detalle.PorcentajeDepreciacion;
                            dep_maq.MesesFaltantes = detalle.FechaBaja != null || detalle.FechaCancelacion != null || detalle.DepreciacionTerminadaPorMeses ? 0 : detalle.MesesMaximoDepreciacion - (detalle.MesesDepreciadosAñoAnterior + detalle.MesesDepreciadosAñoActual);
                            dep_maq.Monto = detalle.MOI + detalle.Altas + detalle.Overhaul;
                            dep_maq.DepreciacionAcumulada = detalle.DepreciacionContableAcumulada;
                            dep_maq.DepreciacionSemanal = detalle.DepreciacionMensual / 4;
                            dep_maq.DepreciacionMensual = detalle.DepreciacionMensual;
                            dep_maq.PorcentajeDepreciacion = detalle.PorcentajeDepreciacion * 100;
                            dep_maq.ValorLibro = detalle.ValorEnLibros;
                            dep_maq.semanasDepreciacionOH_14_1 = detalle.semanasDepreciacionOverhaul14_1;
                            dep_maq.depreciacionOH_14_1 = detalle.depreciacionOverhaul14_1;

                            if (maquinaActiva == 1)
                            {
                                if ((detalle.FechaBaja != null && detalle.FechaBaja.Value < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)) || (detalle.FechaCancelacion != null && !detalle.EsOverhaul)) { continue; }
                                if (detalle.MesesDepreciadosAñoAnterior + detalle.MesesDepreciadosAñoActual > detalle.MesesMaximoDepreciacion) { continue; }
                                if (detalle.FechaInicioDepreciacion > new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))) { continue; }
                                if (detalle.EsOverhaul && detalle.EsOverhaul && (detalle.MesesDepreciadosAñoAnterior + detalle.MesesDepreciadosAñoActual) == 0) { continue; }

                                if (detalle.DepreciacionTerminadaPorMeses)
                                {
                                    if (
                                        fecha.HasValue &&
                                        fecha.Value >= new DateTime(2023, 7, 1) &&
                                        fecha.Value <= new DateTime(2023, 7, 31) &&
                                        detalle.DepreciacionTerminadaPorMeses &&
                                        detalle.semanasDepreciacionOverhaul14_1 > 0
                                       )
                                    {

                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }

                                if (new DateTime(detalle.FechaInicioDepreciacion.Year, detalle.FechaInicioDepreciacion.Month, 1) == new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)) { continue; }
                                if (detalle.EsOverhaul && detalle.FechaCancelacion != null && new DateTime(detalle.FechaCancelacion.Value.Year, detalle.FechaCancelacion.Value.Month, 1) < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) && detalle.DepreciacionMensual >= 0) { continue; }
                            }

                            if (tipoMovimiento.Count > 0 && !tipoMovimiento.Contains(detalle.IdTipoMovimiento))
                            {
                                continue;
                            }

                            if (fAreasCuenta != null)
                            {
                                if (!fAreasCuenta.Contains(detalle.Area + "-" + detalle.Cuenta_OC))
                                {
                                    continue;
                                }
                            }

                            if (cuentaOverhaul != null)
                            {
                                if (detalle.EsOverhaul)
                                {
                                    if (detalle.MesesMaximoDepreciacion != cuentaOverhaul)
                                    {
                                        continue;
                                    }
                                }
                            }

                            if (maquinaActiva == 1 && (detalle.FechaBaja != null || (detalle.FechaCancelacion != null && !detalle.EsOverhaul) || detalle.DepreciacionTerminadaPorMeses || detalle.FechaInicioDepreciacion >= fechaActual))
                            {
                                if (
                                        fecha.HasValue &&
                                        fecha.Value >= new DateTime(2023, 7, 1) &&
                                        fecha.Value <= new DateTime(2023, 7, 31) &&
                                        detalle.DepreciacionTerminadaPorMeses &&
                                        detalle.semanasDepreciacionOverhaul14_1 > 0
                                       )
                                {

                                }
                                else
                                {
                                    continue;
                                }
                            }

                            if (maquinaActiva == 0 && (detalle.FechaBaja != null || detalle.FechaCancelacion != null || !detalle.DepreciacionTerminadaPorMeses))
                            {
                                continue;
                            }

                            var baja = detalle.FechaBaja != null ? true : false;
                            baja = detalle.FechaCancelacion != null ? true : baja;

                            if (maquinaActiva == 2 && !baja)
                            {
                                continue;
                            }

                            if (detalle.FechaCancelacion != null && !detalle.EsOverhaul)
                            {
                                continue;
                            }

                            lista_dep_maquinas.Add(dep_maq);
                        }

                        var resumenTabulador = new ActivoFijoDepMaquinaResumenDTO(lista_dep_maquinas);

                        resultado.Add(SUCCESS, true);
                        resultado.Add(ITEMS, resumenTabulador);

                        return resultado;
                    }
                    else
                    {
                        resultado = resultadoConstruccionCedula;
                    }
                }
                else
                {
                    var fechaActual = DateTime.Now;

                    var getDetalleCuenta = GetDetalleCuenta(DateTime.Now, cuenta.Value);

                    if ((bool)getDetalleCuenta[SUCCESS])
                    {
                        var detalleCuenta = (List<ActivoFijoDetalleDTO>)getDetalleCuenta[ITEMS];

                        var lista_dep_maquinas = new List<ActivoFijoDepMaquinaDTO>();

                        foreach (var item in detalleCuenta)
                        {
                            var dep = new ActivoFijoDepMaquinaDTO();

                            if (item.FechaCancelacion != null)
                            {
                                continue;
                            }

                            dep.Descripcion = item.Descripcion;
                            dep.CC = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? item.Cuenta == 1230 ? item.Cc : "001" : item.Cc;
                            dep.AreaCuenta = item.AreaCuenta;
                            dep.Monto = item.MOI > 0.0M ? item.MOI : item.Altas > 0.0M ? item.Altas : item.Overhaul > 0.0M ? item.Overhaul : 0.0M;
                            dep.FechaInicioDepreciacion = item.FechaInicioDepreciacion;
                            dep.MesesTotalesDepreciacion = item.MesesMaximoDepreciacion;
                            dep.Factura = item.Factura;
                            dep.Poliza = item.Poliza;
                            dep.FechaBaja = item.FechaBaja != null ? item.FechaBaja : (DateTime?)null;
                            dep.DepreciacionAcumulada = item.DepreciacionAñoActual + item.DepreciacionAcumuladaAñoAnterior;
                            dep.MesesFaltantes = dep.MesesTotalesDepreciacion - (item.MesesDepreciadosAñoAnterior + item.MesesDepreciadosAñoActual);
                            dep.PorcentajeDepreciacion = item.PorcentajeDepreciacion * 100;
                            dep.DepreciacionSemanal = item.DepreciacionMensual / 4;
                            dep.DepreciacionMensual = item.DepreciacionMensual;
                            dep.ValorLibro = item.ValorEnLibros;

                            if (maquinaActiva == 1)
                            {
                                if ((item.FechaBaja != null && item.FechaBaja.Value < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)) || (item.FechaCancelacion != null && !item.EsOverhaul)) { continue; }
                                if (item.MesesDepreciadosAñoAnterior + item.MesesDepreciadosAñoActual > item.MesesMaximoDepreciacion) { continue; }
                                if (item.FechaInicioDepreciacion > new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))) { continue; }
                                if (item.EsOverhaul && item.EsOverhaul && (item.MesesDepreciadosAñoAnterior + item.MesesDepreciadosAñoActual) == 0) { continue; }
                                if (item.DepreciacionTerminadaPorMeses) { continue; }

                                if (new DateTime(item.FechaInicioDepreciacion.Year, item.FechaInicioDepreciacion.Month, 1) == new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)) { continue; }
                                if (item.EsOverhaul && item.FechaCancelacion != null && new DateTime(item.FechaCancelacion.Value.Year, item.FechaCancelacion.Value.Month, 1) < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) && item.DepreciacionMensual >= 0) { continue; }
                            }

                            if (tipoMovimiento.Count > 0 && !tipoMovimiento.Contains(item.IdTipoMovimiento))
                            {
                                continue;
                            }

                            if (maquinaActiva == 1 && (item.FechaBaja != null || (item.FechaCancelacion != null && !item.EsOverhaul) || item.DepreciacionTerminadaPorMeses || item.FechaInicioDepreciacion >= fechaActual))
                            {
                                continue;
                            }

                            if (maquinaActiva == 0 && (item.FechaBaja != null || item.FechaCancelacion != null || !item.DepreciacionTerminadaPorMeses))
                            {
                                continue;
                            }

                            var baja = item.FechaBaja != null ? true : false;
                            baja = item.FechaCancelacion != null ? true : baja;

                            if (maquinaActiva == 2 && !baja)
                            {
                                continue;
                            }

                            if (item.FechaCancelacion != null && !item.EsOverhaul)
                            {
                                continue;
                            }

                            lista_dep_maquinas.Add(dep);
                        }

                        var resumenTabulador = new ActivoFijoDepMaquinaResumenDTO(lista_dep_maquinas);

                        resultado.Add(SUCCESS, true);
                        resultado.Add(ITEMS, resumenTabulador);
                    }
                    else
                    {
                        resultado = getDetalleCuenta;
                    }
                }
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al obtener la información de la BD. " + ex.ToString());
            }

            return resultado;
        }

        public Dictionary<string, object> GetTabulador(int idDepMaquina, bool EsExtraCatMaqDep)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var depMaquina = new List<tblM_CatMaquinaDepreciacion>();
                depMaquina = _context.tblM_CatMaquinaDepreciacion.Where(x => ((x.Id == idDepMaquina) || (x.IdPolizaReferenciaAlta == idDepMaquina && x.Poliza.TM == 2)) && x.Estatus).OrderBy(x => x.Poliza.FechaMovimiento).ToList();

                if (EsExtraCatMaqDep)
                {
                    depMaquina = new List<tblM_CatMaquinaDepreciacion>();

                    var depMaquinaExtra = _context.tblM_CatMaquinaDepreciacion_Extras.Where(x => ((x.Id == idDepMaquina) || (x.IdPolizaReferenciaAlta == idDepMaquina && x.Poliza.TM == 2)) && x.Estatus).OrderBy(x => x.Poliza.FechaMovimiento).ToList();

                    foreach (var item in depMaquinaExtra)
                    {
                        var depMq = new tblM_CatMaquinaDepreciacion();
                        var depPol = new tblC_AF_PolizaAltaBaja();
                        tblC_AF_PolizaAltaBaja depPolAlta = null;

                        depPol.Año = item.Poliza.Año;
                        depPol.Concepto = item.Poliza.Concepto;
                        depPol.Cuenta = item.Poliza.Cuenta;
                        depPol.DescripcionTipoActivo = item.Poliza.DescripcionTipoActivo;
                        depPol.Estatus = item.Poliza.Estatus;
                        depPol.Excepcion = item.Poliza.Excepcion;
                        depPol.Factura = item.Poliza.Factura;
                        depPol.FechaCreacion = item.Poliza.FechaCreacion;
                        depPol.FechaModificacion = item.Poliza.FechaModificacion;
                        depPol.FechaMovimiento = item.Poliza.FechaMovimiento;
                        depPol.Id = item.Poliza.Id;
                        depPol.IdUsuarioCreacion = item.Poliza.IdUsuarioCreacion;
                        depPol.IdUsuarioModificacion = item.Poliza.IdUsuarioModificacion;
                        depPol.Linea = item.Poliza.Linea;
                        depPol.Mes = item.Poliza.Mes;
                        depPol.Monto = item.Poliza.Monto;
                        depPol.Poliza = item.Poliza.Poliza;
                        depPol.Subcuenta = item.Poliza.Subcuenta;
                        depPol.SubSubcuenta = item.Poliza.SubSubcuenta;
                        depPol.TipoActivo = item.Poliza.TipoActivo;
                        depPol.TM = item.Poliza.TM;
                        depPol.TP = item.Poliza.TP;
                        depPol.Usuario = item.Poliza.Usuario;
                        depPol.UsuarioModificacion = item.Poliza.UsuarioModificacion;

                        if (item.PolizaAlta != null)
                        {
                            depPolAlta = new tblC_AF_PolizaAltaBaja();

                            depPolAlta.Año = item.PolizaAlta.Año;
                            depPolAlta.Concepto = item.PolizaAlta.Concepto;
                            depPolAlta.Cuenta = item.PolizaAlta.Cuenta;
                            depPolAlta.DescripcionTipoActivo = item.PolizaAlta.DescripcionTipoActivo;
                            depPolAlta.Estatus = item.PolizaAlta.Estatus;
                            depPolAlta.Excepcion = item.PolizaAlta.Excepcion;
                            depPolAlta.Factura = item.PolizaAlta.Factura;
                            depPolAlta.FechaCreacion = item.PolizaAlta.FechaCreacion;
                            depPolAlta.FechaModificacion = item.PolizaAlta.FechaModificacion;
                            depPolAlta.FechaMovimiento = item.PolizaAlta.FechaMovimiento;
                            depPolAlta.Id = item.PolizaAlta.Id;
                            depPolAlta.IdUsuarioCreacion = item.PolizaAlta.IdUsuarioCreacion;
                            depPolAlta.IdUsuarioModificacion = item.PolizaAlta.IdUsuarioModificacion;
                            depPolAlta.Linea = item.PolizaAlta.Linea;
                            depPolAlta.Mes = item.PolizaAlta.Mes;
                            depPolAlta.Monto = item.PolizaAlta.Monto;
                            depPolAlta.Poliza = item.PolizaAlta.Poliza;
                            depPolAlta.Subcuenta = item.PolizaAlta.Subcuenta;
                            depPolAlta.SubSubcuenta = item.PolizaAlta.SubSubcuenta;
                            depPolAlta.TipoActivo = item.PolizaAlta.TipoActivo;
                            depPolAlta.TM = item.PolizaAlta.TM;
                            depPolAlta.TP = item.PolizaAlta.TP;
                            depPolAlta.Usuario = item.PolizaAlta.Usuario;
                            depPolAlta.UsuarioModificacion = item.PolizaAlta.UsuarioModificacion;
                        }

                        depMq.CapturaAutomatica = item.CapturaAutomatica;
                        depMq.Estatus = item.Estatus;
                        depMq.FechaCreacion = item.FechaCreacion;
                        depMq.FechaInicioDepreciacion = item.FechaInicioDepreciacion;
                        depMq.FechaModificacion = item.FechaModificacion;
                        depMq.Id = item.Id;
                        depMq.IdCatMaquina = 0;
                        depMq.IdPoliza = item.IdPoliza;
                        depMq.IdPolizaReferenciaAlta = item.IdPolizaReferenciaAlta;
                        depMq.IdUsuarioCreacion = item.IdUsuarioCreacion;
                        depMq.IdUsuarioModificacion = item.IdUsuarioModificacion;
                        depMq.Maquina = null;
                        depMq.MesesTotalesDepreciacion = item.MesesTotalesDepreciacion;
                        depMq.Movimiento = item.Movimiento;
                        depMq.Poliza = depPol;
                        depMq.PolizaAlta = depPolAlta;
                        depMq.PorcentajeDepreciacion = item.PorcentajeDepreciacion;
                        depMq.TipoDelMovimiento = item.TipoDelMovimiento;
                        depMq.Usuario = item.Usuario;
                        depMq.UsuarioModificacion = item.UsuarioModificacion;

                        depMaquina.Add(depMq);
                    }
                }
                

                if (depMaquina.Count > 0)
                {
                    var fechaInicio = depMaquina.First(x => x.Poliza.TM == 1).FechaInicioDepreciacion;
                    var fechaActual = DateTime.Now;
                    DateTime? fechaBaja = depMaquina.FirstOrDefault(x => x.Poliza.TM == 2) != null ? (DateTime?)depMaquina.FirstOrDefault(x => x.Poliza.TM == 2).Poliza.FechaMovimiento : null;
                    var mesesDeDepreciacion = 0;
                    var mesesTotalesDepreciacion = depMaquina.First(x => x.Poliza.TM == 1).MesesTotalesDepreciacion;
                    var porcentajeDepreciacion = depMaquina.First(x => x.Poliza.TM == 1).PorcentajeDepreciacion;
                    var contador = 0;
                    decimal depAcum = 0.0M;

                    List<ActivoFijoTabuladorDTO> afTb = new List<ActivoFijoTabuladorDTO>();

                    OdbcConsultaDTO odbcPolizas = new OdbcConsultaDTO();

                    odbcPolizas.consulta = string.Format
                        (
                            @"SELECT
                                    MOV.monto
                                  FROM
                                    sc_movpol AS MOV
                                  WHERE
                                    MOV.year = ? AND MOV.mes = ? AND MOV.poliza = ? AND MOV.tp = ? AND
                                    MOV.linea = ?"
                        );

                    odbcPolizas.parametros.Add(new OdbcParameterDTO()
                    {
                        nombre = "year",
                        tipo = OdbcType.Int,
                        valor = depMaquina.First(x => x.Poliza.TM == 1).Poliza.Año
                    });
                    odbcPolizas.parametros.Add(new OdbcParameterDTO()
                    {
                        nombre = "mes",
                        tipo = OdbcType.Int,
                        valor = depMaquina.First(x => x.Poliza.TM == 1).Poliza.Mes
                    });
                    odbcPolizas.parametros.Add(new OdbcParameterDTO()
                    {
                        nombre = "poliza",
                        tipo = OdbcType.Int,
                        valor = depMaquina.First(x => x.Poliza.TM == 1).Poliza.Poliza
                    });
                    odbcPolizas.parametros.Add(new OdbcParameterDTO()
                    {
                        nombre = "tp",
                        tipo = OdbcType.NVarChar,
                        valor = depMaquina.First(x => x.Poliza.TM == 1).Poliza.TP
                    });
                    odbcPolizas.parametros.Add(new OdbcParameterDTO()
                    {
                        nombre = "linea",
                        tipo = OdbcType.Int,
                        valor = depMaquina.First(x => x.Poliza.TM == 1).Poliza.Linea
                    });

                    ActivoFijoRegInfoDepDTO polizas_cc = _contextEnkontrol.Select<ActivoFijoRegInfoDepDTO>(EnkontrolAmbienteEnum.Prod, odbcPolizas).FirstOrDefault();

                    if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora && polizasExceptions.Contains(depMaquina.First(f => f.Poliza.TM == 1).Poliza.Poliza))
                    {
                        polizas_cc = new ActivoFijoRegInfoDepDTO();
                        polizas_cc.Monto = depMaquina.First(f => f.Poliza.TM == 1).Poliza.Monto.Value;
                    }

                    if (fechaBaja != null)
                    {
                        mesesDeDepreciacion = ((fechaBaja.Value.Year - fechaInicio.Value.Year) * 12) + fechaBaja.Value.Month - fechaInicio.Value.Month;
                    }
                    else
                    {
                        mesesDeDepreciacion = ((fechaActual.Year - fechaInicio.Value.Year) * 12) + fechaActual.Month - fechaInicio.Value.Month;
                    }

                    var catControlCalidad = GetAreaCuenta_ControlCalidad(depMaquina.First().IdCatMaquina);
                    List<ActivoFijoCatControlCalidadDTO> areaCuentasMaquina = null;
                    if ((bool)catControlCalidad[SUCCESS])
                    {
                        areaCuentasMaquina = (List<ActivoFijoCatControlCalidadDTO>)catControlCalidad[ITEMS];
                    }
                    else
                    {
                        return catControlCalidad;
                    }

                    for (int x = 1; x <= mesesDeDepreciacion; x++)
                    {
                        if (afTb.Count <= mesesTotalesDepreciacion)
                        {
                            contador++;
                            var tab = new ActivoFijoTabuladorDTO();
                            //var subTabList = new List<ActivoFijoTabuladorDTO>();

                            tab.Año = depMaquina.First(m => m.Poliza.TM == 1).FechaInicioDepreciacion.Value.AddMonths(x).Year;
                            tab.Mensualidad = contador;
                            tab.Mes = new DateTime(2020, (depMaquina.First(m => m.Poliza.TM == 1).FechaInicioDepreciacion.Value.AddMonths(x).Month), 1).ToString("MMMM");
                            var numMes = new DateTime(2020, (depMaquina.First(m => m.Poliza.TM == 1).FechaInicioDepreciacion.Value.AddMonths(x).Month), 1).Month;
                            tab.DepreciacionMensual = (decimal)((porcentajeDepreciacion * polizas_cc.Monto) / 12);
                            tab.DepreciacionSemanal = tab.DepreciacionMensual / 4;
                            tab.DepreciacionAcumulada = tab.DepreciacionMensual + depAcum; depAcum += tab.DepreciacionMensual;
                            tab.ValorEnLibros += polizas_cc.Monto - tab.DepreciacionAcumulada;

                            //if (areaCuentasMaquina.Count == 0)
                            //{
                            //    tab.AreaCuenta = "N/A";
                            //}
                            //else
                            //{
                            //    if (areaCuentasMaquina.Where(w => w.AñoMes == tab.Año + "-" + numMes).Count() == 0)
                            //    {
                            //        if (areaCuentasMaquina.Where(w => w.FechaCaptura < new DateTime(tab.Año, numMes, 1)).LastOrDefault() == null)
                            //        {
                            //            tab.AreaCuenta = "N/A";
                            //        }
                            //        else
                            //        {
                            //            tab.AreaCuenta = areaCuentasMaquina.Where(w => w.FechaCaptura < new DateTime(tab.Año, numMes, 1)).Last().Obra;
                            //        }
                            //    }
                            //    if (areaCuentasMaquina.Where(w => w.AñoMes == tab.Año + "-" + numMes).Count() == 1)
                            //    {
                            //        tab.AreaCuenta = areaCuentasMaquina.First(f => f.AñoMes == tab.Año + "-" + numMes).Obra;
                            //    }
                            //    if (areaCuentasMaquina.Where(w => w.AñoMes == tab.Año + "-" + numMes).GroupBy(g => g.Obra).Count() == 1)
                            //    {
                            //        tab.AreaCuenta = areaCuentasMaquina.First(f => f.AñoMes == tab.Año + "-" + numMes).Obra;
                            //    }
                            //    if (areaCuentasMaquina.Where(w => w.AñoMes == tab.Año + "-" + numMes).GroupBy(g => g.Obra).Count() > 1)
                            //    {
                            //        var diasEnElMes = DateTime.DaysInMonth(tab.Año, numMes);
                            //        var semana = 1;
                            //        var contadorDias = 0;

                            //        foreach (var obrasMes in areaCuentasMaquina.Where(w => w.AñoMes == tab.Año + "-" + numMes).OrderByDescending(o => o.FechaCaptura))
                            //        {
                            //            var subTab = new ActivoFijoTabuladorDTO();
                            //            subTab.Año = tab.Año;
                            //            subTab.Mensualidad = tab.Mensualidad;
                            //            subTab.Mes = tab.Mes;
                            //            subTab.DepreciacionMensual = 0;
                            //            subTab.DepreciacionSemanal = 0;
                            //            subTab.DepreciacionAcumulada = 0;
                            //            subTab.ValorEnLibros = 0;


                            //            for (int i = 1; i <= diasEnElMes; i++)
                            //            {
                            //                if (contadorDias == 7)
                            //                {
                            //                    semana += 1;
                            //                    contadorDias = 0;

                            //                    if (obrasMes.Dia <= i)
                            //                    {
                            //                        subTab.AreaCuenta = obrasMes.Obra;
                            //                        break;
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    contadorDias++;
                            //                }
                            //            }

                            //            subTabList.Add(subTab);
                            //        }
                            //    }
                            //}

                            afTb.Add(tab);
                            //afTb.AddRange(subTabList);
                            //subTabList.RemoveRange(0, subTabList.Count);
                        }
                    }

                    if (fechaBaja != null)
                    {
                        var tab = new ActivoFijoTabuladorDTO();

                        tab.Año = fechaBaja.Value.Year;
                        tab.Mensualidad = 0;
                        tab.Mes = fechaBaja.Value.ToString("MMMM");
                        tab.DepreciacionMensual = 0;
                        tab.DepreciacionSemanal = tab.DepreciacionMensual / 4;
                        tab.DepreciacionAcumulada = depAcum * -1;
                        tab.ValorEnLibros += 0;
                        afTb.Add(tab);
                    }

                    ActivoFijoInfoTabuladorDTO infoTabulador = new ActivoFijoInfoTabuladorDTO();
                    infoTabulador.Tabulador = afTb;
                    infoTabulador.MesesTotalesDepreciacion = mesesTotalesDepreciacion.Value;
                    infoTabulador.Monto = polizas_cc.Monto;
                    infoTabulador.DepreciacionTotal = afTb.Count > 0 ? afTb.Last().DepreciacionAcumulada : 0.0M;
                    infoTabulador.MesesFaltantesDepreciacion = fechaBaja != null ? 0 : mesesTotalesDepreciacion.Value - afTb.Count;
                    infoTabulador.PorcentajeDepreciacion = porcentajeDepreciacion.Value * 100;
                    infoTabulador.FechaBaja = fechaBaja;

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, infoTabulador);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "La maquina seleccionada no se encuentra en el inventario de maquinaria");
                }
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Error al crear tabuldaor: " + ex.ToString());
            }

            return resultado;
        }

        public Dictionary<string, object> GetTabuladorExcel(ActivoFijoInfoTabuladorDTO tabulador)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    var excelDetalles = excel.Workbook.Worksheets.Add("Tabulador");

                    var header = new List<string> { "Año", "Mensualidad", "Mes", "Depreciación semanal", "Depreciación mensual", "Depreciación acumulada", "Valor en libros"};

                    for (int i = 1; i <= header.Count; i++)
                    {
                        excelDetalles.Cells[1, i].Value = header[i - 1];
                    }

                    var cellData = new List<object[]>();
                    int contador = 1;
                    foreach (var item in tabulador.Tabulador)
                    {
                        cellData.Add(new object[]{
                            item.Año,
                            item.Mensualidad,
                            item.Mes,
                            item.DepreciacionSemanal,
                            item.DepreciacionMensual,
                            item.DepreciacionAcumulada,
                            item.ValorEnLibros
                        });
                    }

                    excelDetalles.Cells[2, 1].LoadFromArrays(cellData);

                    ExcelRange range = excelDetalles.Cells[1, 1, excelDetalles.Dimension.End.Row, excelDetalles.Dimension.End.Column];

                    ExcelTable tab = excelDetalles.Tables.Add(range, "Tabla");

                    excelDetalles.Cells[1, 4, excelDetalles.Dimension.End.Row, 7].Style.Numberformat.Format = "$#,##0.00";

                    tab.TableStyle = TableStyles.Medium17;

                    excelDetalles.Cells[excelDetalles.Dimension.Address].AutoFitColumns();

                    var bytes = new MemoryStream();

                    using (var stream = new MemoryStream())
                    {
                        excel.SaveAs(stream);
                        bytes = stream;
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, bytes);
                }
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "ERROR AL DESCARGAR EXCEL. " + ex.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GetConsultaEnExcel(ActivoFijoDepMaquinaResumenDTO resumen, int? cuenta, string equipo, int? estado)
        {
            var r = new Dictionary<string, object>();

            try
            {
                var cuentaMov = cuenta != null ? GetCuentasPolizas().First(x => x.Cuenta == cuenta) : null;

                using (ExcelPackage excel = new ExcelPackage())
                {
                    //Hoja
                    var excelDetalles = excel.Workbook.Worksheets.Add(
                        !string.IsNullOrEmpty(equipo) ? cuenta != null ? cuentaMov.Cuenta + " - " + equipo.ToUpper() : equipo.ToUpper() : cuenta != null ? cuentaMov.Cuenta + " - " + cuentaMov.Descripcion : "Sin información"
                        );

                    //Header tabla
                    var header = new List<string>
                    {
                        //CC, #Eco, AC, TM, TA
                        "Número", "CC", "Número económico", "Descripción", "Área cuenta", "Tipo movimiento", "Tipo activo", "Factura",
                        "Póliza", "MOI", "Depreciación semanal", "Depreciación mensual", "Fecha inicio depreciación", "Depreciación acumulada",
                        "Meses a depreciar", "Meses faltantes", "Falta por depreciar", "% depreciación", "Fecha baja", "Semanas OH StandBy", "Dep OH StandBy"
                    };

                    for (int i = 0; i < header.Count; i++)
                    {
                        excelDetalles.Cells[3, i + 2].Value = header[i];
                    }

                    excelDetalles.Cells[3, 2, 3, excelDetalles.Dimension.End.Column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    //Titulo
                    excelDetalles.Cells[1, 2, 1, header.Count + 1].Merge = true;
                    var cellMerged = excelDetalles.MergedCells[0];
                    excelDetalles.Cells[cellMerged].Value = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? "ARRENDADORA CONSTRUPLAN S.A. DE C.V." : vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? "CONSTRUPLAN S.A. DE C.V." : "";
                    excelDetalles.Cells[cellMerged].Style.Font.Bold = true;
                    excelDetalles.Cells[cellMerged].Style.Font.Size = 14;
                    excelDetalles.Cells[cellMerged].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    //Subtitulo
                    excelDetalles.Cells[2, 2, 2, header.Count + 1].Merge = true;
                    cellMerged = excelDetalles.MergedCells[1];
                    var textoSubtitulo = "";
                    if (!string.IsNullOrEmpty(equipo))
                    {
                        if (cuenta != null)
                        {
                            textoSubtitulo = "DEPRECIACIÓN DEL EQUIPO [" + equipo.ToUpper() + "] EN LA CUENTA [" + cuentaMov.Cuenta + "] " + cuentaMov.Descripcion + " AL " + DateTime.Now.ToShortDateString();
                        }
                        else
                        {
                            textoSubtitulo = "DEPRECIACIÓN DEL EQUIPO [" + equipo.ToUpper() + "] AL " + DateTime.Now.ToShortDateString();
                        }
                    }
                    else
                    {
                        if (cuenta != null)
                        {
                            textoSubtitulo = "DEPRECIACIÓN DE " + cuentaMov.Descripcion + " [" + cuenta + "] AL " + DateTime.Now.ToShortDateString();
                        }
                        else
                        {
                            textoSubtitulo = "Sin información";
                        }
                    }
                    var enumEstatusDep = estado != null ? estado.Value : (int?)null;
                    excelDetalles.Cells[cellMerged].Value = estado == null ?
                        (textoSubtitulo + " - ESTADO: DEPRECIANDO, DEPRECIADOS Y BAJAS") :
                        (textoSubtitulo + " - ESTADO: " + EnumExtensions.GetDescription((AFEstatusDepreciacionEnum)enumEstatusDep.Value));
                    excelDetalles.Cells[cellMerged].Style.Font.Bold = true;
                    excelDetalles.Cells[cellMerged].Style.Font.Size = 14;
                    excelDetalles.Cells[cellMerged].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    //Datos
                    var cellData = new List<object[]>();
                    int contador = 1;
                    foreach (var item in resumen.Depreciaciones)
                    {
                        cellData.Add(new object[] {
                            contador,
                            item.CC, item.NoEconomico, item.Descripcion, item.AreaCuenta, item.TipoMovimiento, item.TipoActivo, item.Factura, item.Poliza,
                            item.Monto, item.DepreciacionSemanal, item.DepreciacionMensual, item.FechaInicioDepreciacion, item.DepreciacionAcumulada, item.MesesTotalesDepreciacion,
                            item.MesesFaltantes, item.ValorLibro, item.PorcentajeDepreciacion, item.FechaBaja != null ? item.FechaBaja.Value : (DateTime?)null,
                            item.semanasDepreciacionOH_14_1, item.depreciacionOH_14_1
                        });
                        contador++;
                    }

                    excelDetalles.Cells[4, 2].LoadFromArrays(cellData);

                    //Formato de tabla
                    ExcelRange range = excelDetalles.Cells[3, 2, excelDetalles.Dimension.End.Row, excelDetalles.Dimension.End.Column];
                    ExcelTable tab = excelDetalles.Tables.Add(range, "Tabla" + cuenta);
                    
                    tab.TableStyle = TableStyles.Medium17;
                    tab.ShowRowStripes = true;

                    //Estilo y formato de datos
                    //Centrados
                    excelDetalles.Cells[4, 2, range.End.Row, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    excelDetalles.Cells[4, 7, range.End.Row, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    excelDetalles.Cells[4, 16, range.End.Row, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    excelDetalles.Cells[4, 19, range.End.Row, 20].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    //Textos
                    
                    excelDetalles.Cells[4, 2, range.End.Row, 10].Style.Numberformat.Format = "@";
                    excelDetalles.Cells[4, 16, range.End.Row, 17].Style.Numberformat.Format = "@";
                    excelDetalles.Cells[4, 19, range.End.Row, 19].Style.Numberformat.Format = "@";

                    //Fechas
                    excelDetalles.Cells[4, 14, range.End.Row, 14].Style.Numberformat.Format = "dd/mm/yyyy";
                    excelDetalles.Cells[4, 20, range.End.Row, 20].Style.Numberformat.Format = "dd/mm/yyyy";

                    //Contabilidad
                    excelDetalles.Cells[4, 11, range.End.Row, 13].Style.Numberformat.Format = "$#,##0.00";
                    excelDetalles.Cells[4, 15, range.End.Row, 15].Style.Numberformat.Format = "$#,##0.00";
                    excelDetalles.Cells[4, 18, range.End.Row, 18].Style.Numberformat.Format = "$#,##0.00";
                    excelDetalles.Cells[4, 22, range.End.Row, 22].Style.Numberformat.Format = "$#,##0.00";

                    //Totales
                    excelDetalles.Cells[range.End.Row + 2, 2, range.End.Row + 2, 10].Merge = true;
                    excelDetalles.Cells[range.End.Row + 2, 2].Value = "TOTAL";
                    excelDetalles.Cells[range.End.Row + 2, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    excelDetalles.Cells[range.End.Row + 2, 2].Style.Font.Bold = true;
                    excelDetalles.Cells[range.End.Row + 2, 2].Style.Font.Size = 13;

                    var columnasTotales = new Dictionary<string, int>()
                    {
                        { "moi", 11 }, { "depSemanal", 12 }, { "depMensual", 13 }, { "depAcumulada", 15 }, { "valorLibro", 18 },
                        { "depOH14_1", 22 }
                    };

                    foreach (var columna in columnasTotales)
                    {
                        var letraCol = OfficeOpenXml.ExcelCellAddress.GetColumnLetter(columna.Value);
                        excelDetalles.Cells[range.End.Row + 2, columna.Value].Formula = "SUM(" + letraCol + "4:" + letraCol + range.End.Row + ")";
                        excelDetalles.Cells[range.End.Row + 2, columna.Value].Style.Numberformat.Format = "$#,##0.00";
                        excelDetalles.Cells[range.End.Row + 2, columna.Value].Style.Font.Bold = true;
                        excelDetalles.Cells[range.End.Row + 2, columna.Value].Style.Font.Size = 13;
                    }

                    ExcelRange rangeComplete = excelDetalles.Cells[3, 2, excelDetalles.Dimension.End.Row, excelDetalles.Dimension.End.Column];

                    rangeComplete.AutoFitColumns();

                    //Si no es maquinaria eliminamos columnas CC, noEconomico, AC, TM, TA, Factura
                    if (cuentaMov != null && cuentaMov.Subcuentas.FirstOrDefault(x => x.Estatus && x.EsMaquinaria) == null)
                    {
                        excelDetalles.DeleteColumn(7, 3);
                        excelDetalles.DeleteColumn(3, 2);
                    }

                    var bytes = new MemoryStream();

                    using (var stream = new MemoryStream())
                    {
                        excel.SaveAs(stream);
                        bytes = stream;
                    }

                    r.Add(SUCCESS, true);
                    r.Add(ITEMS, bytes);
                }
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, ex.Message);
            }

            return r;
        }

        public Dictionary<string, object> GetCuentasCBO()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var cuentas = _context.tblC_AF_Cuentas.Where(x => x.Estatus).Select
                    (x => new ComboDTO
                        {
                            Value = x.Cuenta.ToString(),
                            Prefijo = x.EsMaquinaria ? "EsMaquinaria" : "",
                            Text = "[" + x.Cuenta + "] " + x.Descripcion
                        }
                    ).ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, cuentas);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Error al obtener las cuentas. " + ex.ToString());
            }

            return resultado;
        }

        public Dictionary<string, object> GetTiposMovimiento()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var movimientos = _context.tblC_AF_CatTipoMovimiento.Where(x => x.Id != 0).Select(m => new ComboDTO
                    {
                        Value = m.Id.ToString(),
                        Prefijo = m.Descripcion.ToString(),
                        Text = m.TipoDelMovimiento
                    }).ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, movimientos);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Error al obtener los tipos de movimientos capturados: " + ex.ToString());
            }

            return resultado;
        }

        private Dictionary<string, object> GetAreaCuenta_ControlCalidad(int idNumEconomico)
        {
            var r = new Dictionary<string, object>();

            try
            {
                var areasCuentasTrabajadas = _context.tblM_CatControlCalidad.Where(x => x.IdEconomico == idNumEconomico && x.TipoControl == 2).Select(m => new ActivoFijoCatControlCalidadDTO
                {
                    FechaCaptura = m.FechaCaptura,
                    Año = m.FechaCaptura.Year,
                    Mes = m.FechaCaptura.Month,
                    AñoMes = m.FechaCaptura.Year + "-" + m.FechaCaptura.Month,
                    Dia = m.FechaCaptura.Day,
                    Obra = m.Obra
                }).OrderBy(o => o.AñoMes).ToList();
                
                    //var areasCuentasTrabajadas = _context.tblM_CatControlCalidad.Where(x => x.IdEconomico == idNumEconomico).OrderBy(o => o.FechaCaptura).ToList();

                r.Add(SUCCESS, true);
                r.Add(ITEMS, areasCuentasTrabajadas);
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, ex.Message);
            }

            return r;
        }

        public Dictionary<string, object> GetAreasCuenta()
        {
            var resultado = new Dictionary<string, object>();

            using (var ctx = new MainContext(EmpresaEnum.Arrendadora))
            {
                try
                {
                    var areasCuenta = ctx.tblP_CC.Where(x => x.estatus && x.area != 0 && x.cuenta != 0).Select(m => new ComboDTO()
                    {
                        Text = m.areaCuenta + " - " + m.descripcion,
                        Prefijo = m.areaCuenta,
                        Value = m.areaCuenta
                    }).OrderBy(o => o.Value).ToList();

                    areasCuenta.Add(new ComboDTO()
                    {
                        Text = "14-1 - MAQUINARIA NO ASIGNADA A OBRA",
                        Prefijo = "14-1",
                        Value = "14-1"
                    });

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, areasCuenta);
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Respuesta DepreciacionNumEconomico(string noEconomico, DateTime fechaHasta)
        {
            var r = new Respuesta();

            try
            {
                var resultado = GetDepMaquinas(1, null, noEconomico, new List<int>(), null, fechaHasta, null, null);

                if ((bool)resultado[SUCCESS])
                {
                    var depMaquina = resultado[ITEMS] as ActivoFijoDepMaquinaResumenDTO;

                    var infoDep = new DepreciacionMaquinaConOverhaulDTO();
                    
                    var equipo = depMaquina.Depreciaciones.FirstOrDefault(x => x.IdCatMaquina != 0);
                    
                    infoDep.IdMaquina = equipo != null ? equipo.IdCatMaquina : 0;
                    infoDep.NoEconomico = equipo != null ? equipo.NoEconomico : noEconomico;

                    foreach (var item in depMaquina.Depreciaciones)
                    {
                        if (item.TipoMovimiento == "O")
                        {
                            infoDep.DepreciacionOverhaul += item.DepreciacionAcumulada;
                            infoDep.DepreciacionOverhaulSemanal += item.DepreciacionSemanal;
                            infoDep.MoiOverhaul += item.Monto;
                            infoDep.DepreciacionMensualOH += item.DepreciacionMensual;
                        }
                        else
                        {
                            infoDep.DepreciacionEquipo += item.DepreciacionAcumulada;
                            infoDep.DepreciacionEquipoSemanal += item.DepreciacionSemanal;
                            infoDep.MoiEquipo += item.Monto;
                            infoDep.DepreciacionMensualEquipo += item.DepreciacionMensual;
                        }
                    }

                    r.Success = true;
                    r.Message = "Ok";
                    r.Value = infoDep;
                }
                else
                {
                    r.Message = resultado[MESSAGE].ToString();
                }
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }
        public List<DepreciacionMaquinaConOverhaulDTO> DepreciacionNumEconomicoDTO(List<StandByEcosDTO> noEconomico, DateTime fechaInicio, DateTime fechaHasta)
        {

            var data = new DepreciacionMaquinaConOverhaulDTO();
            var dataLst = new List<DepreciacionMaquinaConOverhaulDTO>();
            try
            {
                var resultado = GetDepMaquinas(null, null, null, new List<int>(), null, fechaHasta, null, null, true);

                if ((bool)resultado[SUCCESS])
                {
                    var depMaquina = resultado[ITEMS] as ActivoFijoDepMaquinaResumenDTO;

                    //
                    //var noEconomicos = depMaquina.Depreciaciones.Where(x => noEconomico.Select(m => m.NumEconomico).Contains(x.NoEconomico)).GroupBy(g => g.NoEconomico).Select(m => m.Key);
                    var noEcos = depMaquina.Depreciaciones.Where(x => noEconomico.Select(m => m.NumEconomico).Contains(x.NoEconomico)).ToList();
                    foreach (var _Economicos in noEcos)
                    {
                        foreach (var item in noEconomico.Where(x => x.NumEconomico == _Economicos.NoEconomico))
                        {
                            var depreciaciones = depMaquina.Depreciaciones.Where(x => x.NoEconomico != null && x.NoEconomico.ToUpper() == _Economicos.NoEconomico.ToUpper()).ToList();

                            var infoDep = new DepreciacionMaquinaConOverhaulDTO();
                            infoDep.IdMaquina = depreciaciones.First().IdCatMaquina;
                            infoDep.NoEconomico = depreciaciones.FirstOrDefault().NoEconomico;
                            infoDep.FechaInicio = fechaInicio;
                            infoDep.FechaFin = item.Fecha;

                            foreach (var deps in depreciaciones)
                            {
                                var depDate = deps.FechaInicioDepreciacion.AddMonths(1);
                                var dD = new DateTime(depDate.Year, depDate.Month, 1);

                                var depDateBaja = deps.FechaBaja;
                                var dDBaja = depDateBaja != null ? new DateTime(depDateBaja.Value.Year, depDateBaja.Value.Month, DateTime.DaysInMonth(depDateBaja.Value.Year, depDateBaja.Value.Month)) : (DateTime?)null;

                                var diasMartes = getDiasMartes(fechaInicio >= dD ? fechaInicio : dD, dDBaja != null && dDBaja.Value < item.Fecha ? dDBaja.Value : item.Fecha);

                                if (deps.TipoMovimiento == "O")
                                {
                                    infoDep.DepreciacionOverhaul += deps.DepreciacionAcumulada;
                                    infoDep.DepreciacionOverhaulSemanal += deps.DepreciacionSemanal;
                                    infoDep.DepreciacionOverhaulPeriodo += deps.DepreciacionSemanal * diasMartes.Count;
                                }
                                else
                                {
                                    infoDep.DepreciacionEquipo += deps.DepreciacionAcumulada;
                                    infoDep.DepreciacionEquipoSemanal += deps.DepreciacionSemanal;
                                    infoDep.DepreciacionEquipoPeriodo += deps.DepreciacionSemanal * diasMartes.Count;
                                }
                            }
                            dataLst.Add(infoDep);
                        }
                    }
                    //

                    //var infoDep = new DepreciacionMaquinaConOverhaulDTO();

                    //var equipo = depMaquina.Depreciaciones.FirstOrDefault(x => x.IdCatMaquina != 0);

                    //infoDep.IdMaquina = equipo != null ? equipo.IdCatMaquina : 0;
                    //infoDep.NoEconomico = equipo != null ? equipo.NoEconomico : noEconomico;

                    //foreach (var item in depMaquina.Depreciaciones)
                    //{
                    //    if (item.TipoMovimiento == "O")
                    //    {
                    //        infoDep.DepreciacionOverhaul += item.DepreciacionAcumulada;
                    //        infoDep.DepreciacionOverhaulSemanal += item.DepreciacionSemanal;
                    //    }
                    //    else
                    //    {
                    //        infoDep.DepreciacionEquipo += item.DepreciacionAcumulada;
                    //        infoDep.DepreciacionEquipoSemanal += item.DepreciacionSemanal;
                    //    }
                    //}

                    //data = infoDep;
                }
                else
                {
                    dataLst = new List<DepreciacionMaquinaConOverhaulDTO>();
                }
            }
            catch (Exception ex)
            {
                dataLst = new List<DepreciacionMaquinaConOverhaulDTO>();
            }

            return dataLst;
        }
        #endregion

        #region CargarExcel
        public Dictionary<string, object> ObtenerPolizasCCCargarExcel(string noEconomico)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

            try
            {
                var fechaActual = DateTime.Now;
                var añoActual = fechaActual.Year;
                var fechaTemp = fechaActual.AddMonths(-3);
                var mesActual = fechaTemp.Year == añoActual ? fechaTemp.Month : 1;

                var añoAnterior = fechaTemp.Year;
                var mesAnterior = fechaTemp.Month;

                OdbcConsultaDTO odbcPolizasCC = new OdbcConsultaDTO();

                var cuentasMaquinaria = _context.tblC_AF_CatalogoMaquina.Select(m => m.Cuenta);

                var relacionSubCuentas = _context.tblC_AF_RelSubCuentas.Where(x => x.Estatus && cuentasMaquinaria.Contains(x.Cuenta.Cuenta) && !x.Excluir && !x.EsCuentaDepreciacion).ToList();

                odbcPolizasCC.consulta = string.Format(@"
                    SELECT
                        MOV.year AS Año, MOV.mes, MOV.poliza, MOV.tp, MOV.linea, MOV.tm, MOV.cta AS Cuenta, MOV.scta AS Subcuenta,
                        MOV.sscta AS SubSubcuenta, FAC.factura, MOV.monto, FAC.fecha AS FechaFactura, POL.fechapol, MOV.concepto,
                        (SELECT DISTINCT TOP 1 (CAST(area AS varchar(4)) + '-' + CAST(cuenta AS varchar(4)) + ' - ' + descripcion) FROM si_area_cuenta WHERE area = MOV.area and cuenta = MOV.cuenta_oc) AS AreaCuenta
                    FROM
                        sc_movpol AS MOV
                    INNER JOIN cc AS CC ON MOV.cc = CC.cc
                    INNER JOIN sc_polizas AS POL ON MOV.year = POL.year AND MOV.mes = POL.mes AND MOV.poliza = POL.poliza AND MOV.tp = POL.tp
                    LEFT JOIN sp_movprov AS FAC ON MOV.year = FAC.year AND MOV.mes = FAC.mes AND MOV.poliza = FAC.poliza AND FAC.es_factura = 'S'
                    WHERE
                        MOV.cta in {0} AND
                        (
                            (
                                MOV.year = {1} AND
                                MOV.mes >= {2}
                            ) OR
                            (
                                MOV.year = {3} AND
                                MOV.mes >= {4}
                            )
                        ) AND
                        MOV.tm IN (1, 2) AND
                        MOV.referencia = ?
                    ORDER BY POL.fechapol", cuentasMaquinaria.ToParamInValue(), añoAnterior, mesAnterior, añoActual, mesActual);

                foreach (var item in cuentasMaquinaria)
                {
                    odbcPolizasCC.parametros.Add(new OdbcParameterDTO()
                    {
                        nombre = "cuenta",
                        tipo = System.Data.Odbc.OdbcType.Int,
                        valor = item
                    });
                }

                odbcPolizasCC.parametros.Add(new OdbcParameterDTO()
                {
                    nombre = "referencia",
                    tipo = System.Data.Odbc.OdbcType.VarChar,
                    valor = noEconomico
                });

                List<ActivoFijoRegInfoDepDTO> polizas_cc = _contextEnkontrol.Select<ActivoFijoRegInfoDepDTO>(EnkontrolAmbienteEnum.Prod, odbcPolizasCC);

                if (polizas_cc.Count == 0)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "No se encontró una poliza para el CC: " + noEconomico);
                }
                else
                {
                    List<ActivoFijoRegInfoDepDTO> polCancelaciones = new List<ActivoFijoRegInfoDepDTO>();
                    List<ActivoFijoRegInfoDepDTO> polizas_cc_temp = new List<ActivoFijoRegInfoDepDTO>();

                    foreach (var poliza in polizas_cc)
                    {
                        if (relacionSubCuentas.Where(r => r.Cuenta.Cuenta == poliza.Cuenta && r.Subcuenta == poliza.Subcuenta && r.SubSubcuenta == poliza.SubSubcuenta).Count() > 0)
                        {
                            OdbcConsultaDTO odbcPolizasCCCancelaciones = new OdbcConsultaDTO();

                            odbcPolizasCCCancelaciones.consulta = string.Format(@"
                            SELECT
                                MOV.year AS Año, MOV.mes, MOV.poliza, MOV.tp, MOV.linea, MOV.tm, MOV.cta AS Cuenta, MOV.scta AS Subcuenta, MOV.sscta AS SubSubcuenta,
                                FAC.factura, MOV.monto, FAC.fecha AS FechaFactura, POL.fechapol, MOV.concepto,
                                (SELECT DISTINCT TOP 1 (CAST(area AS varchar(4)) + '-' + CAST(cuenta AS varchar(4)) + ' - ' + descripcion) FROM si_area_cuenta WHERE area = MOV.area and cuenta = MOV.cuenta_oc) AS AreaCuenta
                            FROM
                                sc_movpol AS MOV
                            INNER JOIN cc AS CC ON MOV.cc = CC.cc
                            INNER JOIN sc_polizas AS POL ON MOV.year = POL.year AND MOV.mes = POL.mes AND MOV.poliza = POL.poliza AND MOV.tp = POL.tp
                            LEFT JOIN sp_movprov AS FAC ON MOV.year = FAC.year AND MOV.mes = FAC.mes AND MOV.poliza = FAC.poliza AND FAC.es_factura = 'S'
                            WHERE
                                MOV.cta = ? AND
                                MOV.year >= ? AND
                                MOV.tm = 3 AND
                                MOV.monto = ?
                            ORDER BY POL.fechapol");

                            odbcPolizasCCCancelaciones.parametros.Add(new OdbcParameterDTO()
                            {
                                nombre = "cta",
                                tipo = OdbcType.Int,
                                valor = poliza.Cuenta
                            });

                            odbcPolizasCCCancelaciones.parametros.Add(new OdbcParameterDTO()
                            {
                                nombre = "year",
                                tipo = OdbcType.Int,
                                valor = poliza.Año
                            });

                            odbcPolizasCCCancelaciones.parametros.Add(new OdbcParameterDTO()
                            {
                                nombre = "monto",
                                tipo = OdbcType.Decimal,
                                valor = (poliza.Monto * -1)
                            });

                            List<ActivoFijoRegInfoDepDTO> polizas_ccCancelaciones = _contextEnkontrol.Select<ActivoFijoRegInfoDepDTO>(EnkontrolAmbienteEnum.Prod, odbcPolizasCCCancelaciones);

                            if (polizas_ccCancelaciones != null && polizas_ccCancelaciones.Count > 0)
                            {
                                polCancelaciones.AddRange(polizas_ccCancelaciones);
                            }
                        }
                        else
                        {
                            polizas_cc_temp.Add(poliza);
                        }
                    }

                    foreach (var item in polizas_cc_temp)
                    {
                        polizas_cc.Remove(item);
                    }

                    polizas_cc.AddRange(polCancelaciones);

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, polizas_cc);
                }
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al consultar la base de datos de enkontrol. " + ex.ToString());
            }

            return resultado;
        }

        public Respuesta RelacionarInfoExcel_NoMaquinas()
        {
            var r = new Respuesta();
            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    var infoCargada = _context.tblC_AF_ExcelNoMaquinas.ToList();

                    var ccs = CCEnkontrol();

                    foreach (var info in infoCargada)
                    {
                        var odbcPoliza = new OdbcConsultaDTO();

                        odbcPoliza.consulta = string.Format
                        (
                            @"SELECT
                                POL.year,
                                POL.mes,
                                POL.poliza,
                                POL.tp,
                                POL.fechaPol,
                                MOV.linea,
                                MOV.cta,
                                MOV.scta,
                                MOV.sscta,
                                MOV.tm,
                                MOV.referencia,
                                MOV.cc,
                                MOV.concepto,
                                MOV.monto
                            FROM
                                sc_movpol AS MOV
                            INNER JOIN
                                sc_polizas AS POL
                                ON
                                    MOV.year = POL.year AND
                                    MOV.mes = POL.mes AND
                                    MOV.poliza = POL.poliza AND
                                    MOV.tp = POL.tp
                            WHERE
                                MOV.cta = ? AND
                                MOV.poliza = ? AND
                                MOV.tp = ? AND
                                MOV.monto = ? AND
                                MOV.tm = 1 AND
                                MOV.year = ? AND
                                MOV.mes = ?"
                        );

                        odbcPoliza.parametros.Add(new OdbcParameterDTO
                        {
                            nombre = "cta",
                            tipo = OdbcType.Int,
                            valor = info.cuenta
                        });
                        odbcPoliza.parametros.Add(new OdbcParameterDTO
                        {
                            nombre = "poliza",
                            tipo = OdbcType.Int,
                            valor = info.poliza
                        });
                        odbcPoliza.parametros.Add(new OdbcParameterDTO
                        {
                            nombre = "tp",
                            tipo = OdbcType.VarChar,
                            valor = info.tp
                        });
                        odbcPoliza.parametros.Add(new OdbcParameterDTO
                        {
                            nombre = "monto",
                            tipo = OdbcType.Decimal,
                            valor = info.moi + info.altas + info.componentes
                        });
                        odbcPoliza.parametros.Add(new OdbcParameterDTO
                        {
                            nombre = "year",
                            tipo = OdbcType.Decimal,
                            valor = info.fechaAlta.Year
                        });
                        odbcPoliza.parametros.Add(new OdbcParameterDTO
                        {
                            nombre = "mes",
                            tipo = OdbcType.Decimal,
                            valor = info.fechaAlta.Month
                        });

                        var coincidencia = _contextEnkontrol.Select<sc_movpolDTO>(_productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, odbcPoliza);

                        var faltante = new tblC_AF_ExcelNoMaquinasFaltantes();

                        faltante.cuenta = info.cuenta;
                        faltante.descripcion = info.descripcion;
                        faltante.monto = info.moi + info.altas + info.componentes;
                        faltante.motivo = null;
                        faltante.numero = info.numero;
                        faltante.poliza = info.poliza;
                        faltante.tp = info.tp;

                        if (coincidencia.Count > 0)
                        {
                            foreach (var poliza in coincidencia)
                            {
                                if (/*poliza.FechaPol.Date == info.fechaAlta.Date*/true)
                                {
                                    var yaExiste = _context.tblC_AF_PolizaAltaBaja_NoMaquina.Any
                                        (a =>
                                            a.yearPoliza == poliza.Year &&
                                            a.mesPoliza == poliza.Mes &&
                                            a.polizaPoliza == poliza.Poliza &&
                                            a.tpPoliza == poliza.TP &&
                                            a.lineaPoliza == poliza.Linea
                                        );

                                    if (!yaExiste)
                                    {
                                        var ccDescripcion = ccs.FirstOrDefault(f => f.CC == info.cc);

                                        var registroBaja = new tblC_AF_PolizaAltaBaja_NoMaquina();

                                        registroBaja.capturaAutomatica = false;
                                        registroBaja.cc = "990";
                                        registroBaja.ccDescripcion = "GASTOS DE OFICINAS CENTRALES";
                                        registroBaja.ccPoliza = "990";
                                        registroBaja.conceptoPoliza = "BAJAS EQUIPO DE COMPUTO";
                                        registroBaja.ctaPoliza = 1225;
                                        registroBaja.estatus = true;
                                        registroBaja.factura = null;
                                        registroBaja.fechaCreacion = DateTime.Now;
                                        registroBaja.fechaModificacion = registroBaja.fechaCreacion;
                                        registroBaja.fechaMovimiento = new DateTime(2021, 4, 1);
                                        registroBaja.fechaPoliza = registroBaja.fechaMovimiento;
                                        registroBaja.inicioDepreciacion = null;
                                        registroBaja.lineaPoliza = 4;
                                        registroBaja.mesesDepreciacion = null;
                                        registroBaja.mesPoliza = 4;
                                        registroBaja.montoPoliza = (info.moi + info.altas + info.componentes) * -1;
                                        registroBaja.polizaPoliza = 384;
                                        registroBaja.porcentajeDepreciacion = null;
                                        registroBaja.referenciaPoliza = "BAJA";
                                        registroBaja.sctaPoliza = 1;
                                        registroBaja.ssctaPoliza = 217;
                                        registroBaja.tipoMovimientoId = null;
                                        registroBaja.tmPoliza = (int)AFTipoMovimientoEnum.Abono;
                                        registroBaja.tpPoliza = "03";
                                        registroBaja.usuarioCreacionId = 13; //ADMIN
                                        registroBaja.usuarioModificacionId = registroBaja.usuarioCreacionId;
                                        registroBaja.yearPoliza = 2021;
                                        registroBaja.polizaBajaId = null;

                                        _context.tblC_AF_PolizaAltaBaja_NoMaquina.Add(registroBaja);
                                        _context.SaveChanges();

                                        var registro = new tblC_AF_PolizaAltaBaja_NoMaquina();

                                        registro.capturaAutomatica = false;
                                        registro.cc = info.cc;
                                        registro.ccDescripcion = ccDescripcion != null ? ccDescripcion.Descripcion : null;
                                        registro.ccPoliza = poliza.Cc;
                                        registro.conceptoPoliza = info.descripcion;
                                        registro.ctaPoliza = info.cuenta;
                                        registro.estatus = true;
                                        registro.factura = null;
                                        registro.fechaCreacion = DateTime.Now;
                                        registro.fechaModificacion = registro.fechaCreacion;
                                        registro.fechaMovimiento = info.fechaAlta;
                                        registro.fechaPoliza = poliza.FechaPol;
                                        registro.inicioDepreciacion = info.fechaAlta;
                                        registro.lineaPoliza = poliza.Linea;
                                        registro.mesesDepreciacion = info.mesesDep;
                                        registro.mesPoliza = poliza.Mes;
                                        registro.montoPoliza = info.moi + info.altas + info.componentes;
                                        registro.polizaPoliza = poliza.Poliza;
                                        registro.porcentajeDepreciacion = info.porcentaje;
                                        registro.referenciaPoliza = poliza.Referencia;
                                        registro.sctaPoliza = info.subcuenta;
                                        registro.ssctaPoliza = info.subsubcuenta;
                                        registro.tipoMovimientoId = info.tipo;
                                        registro.tmPoliza = (int)AFTipoMovimientoEnum.Cargo;
                                        registro.tpPoliza = info.tp;
                                        registro.usuarioCreacionId = 13; //ADMIN
                                        registro.usuarioModificacionId = registro.usuarioCreacionId;
                                        registro.yearPoliza = poliza.Year;
                                        registro.polizaBajaId = registroBaja.id;

                                        _context.tblC_AF_PolizaAltaBaja_NoMaquina.Add(registro);
                                        _context.SaveChanges();

                                        var baja = new tblC_AF_PolizaDeAjuste();

                                        baja.ccPoliza = "990";
                                        baja.conceptoPoliza = "BAJAS EQUIPO DE COMPUTO";
                                        baja.ctaPoliza = 1225;
                                        baja.estatus = true;
                                        baja.fechaCreacion = DateTime.Now;
                                        baja.fechaModificacion = baja.fechaCreacion;
                                        baja.fechaPoliza = new DateTime(2021, 4, 1);
                                        baja.lineaPoliza = 4;
                                        baja.mesPoliza = 4;
                                        baja.montoPoliza = -42867.72M;
                                        baja.polizaAltaBajaId = registroBaja.id;
                                        baja.polizaPoliza = 384;
                                        baja.referenciaPoliza = "BAJA";
                                        baja.tipoPolizaDeAjusteId = 3;
                                        baja.tmPoliza = 2;
                                        baja.tpPoliza = "03";
                                        baja.usuarioCreacionId = 6571;
                                        baja.usuarioModificacionId = 6571;
                                        baja.yearPoliza = 2021;

                                        _context.tblC_AF_PolizaDeAjuste.Add(baja);
                                        _context.SaveChanges();

                                        faltante.motivo = null;

                                        break;
                                    }
                                    else
                                    {
                                        faltante.motivo = "La póliza encontrada ya se encuentra relacionada: " +
                                                          poliza.Year + "-" +
                                                          poliza.Mes + "-" +
                                                          poliza.Poliza + "-" +
                                                          poliza.TP + "-" +
                                                          poliza.Linea;
                                    }
                                }
                                else if (poliza == coincidencia.Last())
                                {
                                    faltante.motivo = "Se encontraron coincidencias pero no con la misma fechaPol";
                                }
                            }
                        }
                        else
                        {
                            faltante.motivo = "No se encontraron coincidencias";
                        }

                        if (!string.IsNullOrEmpty(faltante.motivo))
                        {
                            _context.tblC_AF_ExcelNoMaquinasFaltantes.Add(faltante);
                        }
                    }

                    _context.SaveChanges();
                    transaccion.Commit();

                    r.Success = true;
                    r.Message = "Ok";
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();

                    r.Message = ex.Message;
                }
            }

            return r;
        }
        
        public Dictionary<string, object> JalarExcel()
        {
            var resultado = new Dictionary<string, object>();
            using (var ctx = new MainContext(EmpresaEnum.Arrendadora))
            {
                using (var transactionArre = ctx.Database.BeginTransaction())
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            var _1210Excel = _context.tblC_AF_1210Excel.GroupBy(x => x.Clave).ToList();

                            var añosAnteriores = new List<int>() { 2018 };
                            var añoActual = 2019;
                            var cuentas = new List<int>() { 1210, 1211, 1215 };
                            Dictionary<string, object> resultadoRelSubcuentas = getRelacionSubcuentas(añosAnteriores, cuentas);
                            List<tblC_AF_RelSubCuentas> relacionSubcuentas = (List<tblC_AF_RelSubCuentas>)resultadoRelSubcuentas[ITEMS];

                            var contador = 0;
                            foreach (var noEconomico in _1210Excel)
                            {
                                var obtenerPolizasCC = new Dictionary<string, object>();
                                var polizasCC = new List<ActivoFijoRegInfoDepDTO>();
                                var catMaq = new tblM_CatMaquina();
                                var idCatMaq = 0;

                                catMaq = ctx.tblM_CatMaquina.FirstOrDefault(x => x.noEconomico == noEconomico.Key);

                                if (catMaq != null)
                                {
                                    catMaq.DepreciacionCapturada = true;
                                    ctx.SaveChanges();
                                }

                                foreach (var renglon in noEconomico)
                                {
                                    //CONSTRUPLAN
                                    contador++;
                                    if (catMaq == null)
                                    {
                                        var faltantes = new tblC_AF_1210ExcelFaltantes();

                                        faltantes.Cuenta = renglon.Cuenta;
                                        faltantes.Motivo = "No se encontró maquina en sigoplan";
                                        faltantes.NumeroEconomico = renglon.Clave;
                                        faltantes.NumeroRenglonExcel = renglon.Id;

                                        _context.tblC_AF_1210ExcelFaltantes.Add(faltantes);
                                        _context.SaveChanges();

                                        break;
                                    }
                                    else
                                    {
                                        tblC_AF_PolizaAltaBaja polizaAlta = new tblC_AF_PolizaAltaBaja();
                                        tblM_CatMaquinaDepreciacion catMaqDep = new tblM_CatMaquinaDepreciacion();

                                        polizaAlta.Año = Convert.ToDateTime(renglon.FechaAlta).Year;
                                        polizaAlta.Mes = Convert.ToDateTime(renglon.FechaAlta).Month;
                                        polizaAlta.Poliza = 12111211;
                                        polizaAlta.TP = "03";
                                        polizaAlta.Linea = contador;
                                        polizaAlta.TM = 1;
                                        polizaAlta.Cuenta = renglon.Cuenta;
                                        polizaAlta.Subcuenta = renglon.Subcuenta;
                                        polizaAlta.SubSubcuenta = renglon.SubSubcuenta;
                                        polizaAlta.Concepto = renglon.Descripcion;
                                        polizaAlta.Monto = renglon.MOI.Value > 0 ? renglon.MOI : renglon.AltasEquipo.Value > 0 ? renglon.AltasEquipo : renglon.Componentes.Value > 0 ? renglon.Componentes : 0;
                                        polizaAlta.Factura = renglon.Factura;
                                        polizaAlta.FechaMovimiento = Convert.ToDateTime(renglon.FechaAlta);
                                        polizaAlta.TipoActivo = !string.IsNullOrEmpty(renglon.TipoActivo) ? Convert.ToInt32(renglon.TipoActivo) : renglon.Tipo != "5" ? 1 : 2;
                                        polizaAlta.Estatus = true;
                                        polizaAlta.FechaCreacion = DateTime.Now;
                                        polizaAlta.IdUsuarioCreacion = 13;
                                        polizaAlta.FechaModificacion = polizaAlta.FechaCreacion;
                                        polizaAlta.IdUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;

                                        _context.tblC_AF_PolizaAltaBaja.Add(polizaAlta);
                                        _context.SaveChanges();

                                        catMaqDep.IdCatMaquina = catMaq.id;
                                        catMaqDep.IdPoliza = polizaAlta.Id;
                                        catMaqDep.FechaInicioDepreciacion = Convert.ToDateTime(renglon.FechaInicioDep);
                                        catMaqDep.PorcentajeDepreciacion = renglon.PorcentajeDep;
                                        catMaqDep.MesesTotalesDepreciacion = renglon.MesesTotalesDep;
                                        catMaqDep.TipoDelMovimiento = Convert.ToInt32(renglon.Tipo);
                                        catMaqDep.IdPolizaReferenciaAlta = null;
                                        catMaqDep.Estatus = true;
                                        catMaqDep.FechaCreacion = DateTime.Now;
                                        catMaqDep.IdUsuarioCreacion = 13;
                                        catMaqDep.FechaModificacion = catMaqDep.FechaCreacion;
                                        catMaqDep.IdUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;

                                        _context.tblM_CatMaquinaDepreciacion.Add(catMaqDep);
                                        _context.SaveChanges();
                                    }
                                    //CONSTRUPLNA FIN

                                    //ARRENDADORA
                                    //if (contador == 0)
                                    //{
                                    //    catMaq = _context.tblM_CatMaquina.FirstOrDefault(x => x.noEconomico == renglon.Clave);

                                    //    if (catMaq == null)
                                    //    {
                                    //        var faltantes = new tblC_AF_1210ExcelFaltantes();

                                    //        faltantes.Cuenta = renglon.Cuenta;
                                    //        faltantes.Motivo = "No se encontró maquina en sigoplan";
                                    //        faltantes.NumeroEconomico = renglon.Clave;
                                    //        faltantes.NumeroRenglonExcel = renglon.Id;

                                    //        _context.tblC_AF_1210ExcelFaltantes.Add(faltantes);
                                    //        _context.SaveChanges();

                                    //        break;
                                    //    }
                                    //    else
                                    //    {
                                    //        obtenerPolizasCC = ObtenerPolizasCC(catMaq.noEconomico);
                                    //        idCatMaq = catMaq.id;

                                    //        if ((bool)obtenerPolizasCC[SUCCESS])
                                    //        {
                                    //            polizasCC = (List<ActivoFijoRegInfoDepDTO>)obtenerPolizasCC[ITEMS];
                                    //        }
                                    //        else
                                    //        {
                                    //            if (renglon.Cuenta == 1211)
                                    //            {
                                    //                obtenerPolizasCC = ObtenerPolizasCCCargarExcel(catMaq.noEconomico);

                                    //                if ((bool)obtenerPolizasCC[SUCCESS])
                                    //                {
                                    //                    polizasCC = (List<ActivoFijoRegInfoDepDTO>)obtenerPolizasCC[ITEMS];
                                    //                }
                                    //                else
                                    //                {
                                    //                    var faltantes = new tblC_AF_1210ExcelFaltantes();

                                    //                    faltantes.Cuenta = renglon.Cuenta;
                                    //                    faltantes.Motivo = "No se encontraron polizas";
                                    //                    faltantes.NumeroEconomico = renglon.Clave;
                                    //                    faltantes.NumeroRenglonExcel = renglon.Id;

                                    //                    _context.tblC_AF_1210ExcelFaltantes.Add(faltantes);
                                    //                    _context.SaveChanges();

                                    //                    break;
                                    //                }
                                    //            }
                                    //            else
                                    //            {
                                    //                var faltantes = new tblC_AF_1210ExcelFaltantes();

                                    //                faltantes.Cuenta = renglon.Cuenta;
                                    //                faltantes.Motivo = "No se encontraron polizas";
                                    //                faltantes.NumeroEconomico = renglon.Clave;
                                    //                faltantes.NumeroRenglonExcel = renglon.Id;

                                    //                _context.tblC_AF_1210ExcelFaltantes.Add(faltantes);
                                    //                _context.SaveChanges();

                                    //                break;
                                    //            }
                                    //        }
                                    //    }

                                    //    contador = 1;
                                    //}

                                    //var coincidencia = polizasCC.FirstOrDefault
                                    //    (x =>
                                    //        x.Cuenta == renglon.Cuenta &&
                                    //        x.Año == Convert.ToDateTime(renglon.FechaAlta).Year &&
                                    //        (
                                    //            (Convert.ToDateTime(renglon.FechaAlta).Year == 2018 && renglon.FechaBaja != "" && Convert.ToDateTime(renglon.FechaBaja).Year == 2018 && x.Monto == renglon.MontoBaja) ||
                                    //            (x.Monto == renglon.MOI || x.Monto == renglon.AltasEquipo || x.Monto == renglon.Componentes)
                                    //        ) &&
                                    //        relacionSubcuentas.Where(r => !r.EsCuentaDepreciacion && r.Cuenta.Cuenta == renglon.Cuenta && r.Subcuenta == x.Subcuenta && r.SubSubcuenta == x.SubSubcuenta && r.Año == x.Año).Count() > 0 &&
                                    //        relacionSubcuentas.Where(r => !r.EsCuentaDepreciacion && r.Cuenta.Cuenta == renglon.Cuenta && r.Excluir && r.Subcuenta == x.Subcuenta && r.SubSubcuenta == x.SubSubcuenta && r.Año == x.Año).Count() == 0
                                    //    );

                                    //using (DbContextTransaction transaction = _context.Database.BeginTransaction())
                                    //{
                                    //    try
                                    //    {
                                    //        if (coincidencia != null)
                                    //        {
                                    //            if (!_context.tblC_AF_PolizaAltaBaja.Any(x => x.Año == coincidencia.Año && x.Mes == coincidencia.Mes && x.Poliza == coincidencia.Poliza && x.TP == coincidencia.TP && x.Linea == coincidencia.Linea))
                                    //            {
                                    //                tblC_AF_PolizaAltaBaja polizaAlta = new tblC_AF_PolizaAltaBaja();
                                    //                tblM_CatMaquinaDepreciacion catMaqDep = new tblM_CatMaquinaDepreciacion();

                                    //                polizaAlta.Año = coincidencia.Año;
                                    //                polizaAlta.Mes = coincidencia.Mes;
                                    //                polizaAlta.Poliza = coincidencia.Poliza;
                                    //                polizaAlta.TP = coincidencia.TP;
                                    //                polizaAlta.Linea = coincidencia.Linea;
                                    //                polizaAlta.TM = coincidencia.TM;
                                    //                polizaAlta.Cuenta = coincidencia.Cuenta;
                                    //                polizaAlta.Factura = renglon.Factura;
                                    //                polizaAlta.FechaMovimiento = Convert.ToDateTime(renglon.FechaAlta);
                                    //                polizaAlta.TipoActivo = renglon.Tipo != "5" ? 1 : 2;
                                    //                polizaAlta.Estatus = true;
                                    //                polizaAlta.FechaCreacion = DateTime.Now;
                                    //                polizaAlta.IdUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                    //                polizaAlta.FechaModificacion = polizaAlta.FechaCreacion;
                                    //                polizaAlta.IdUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;

                                    //                _context.tblC_AF_PolizaAltaBaja.Add(polizaAlta);
                                    //                _context.SaveChanges();

                                    //                catMaqDep.IdCatMaquina = idCatMaq;
                                    //                catMaqDep.IdPoliza = polizaAlta.Id;
                                    //                catMaqDep.FechaInicioDepreciacion = Convert.ToDateTime(renglon.FechaInicioDep);
                                    //                catMaqDep.PorcentajeDepreciacion = renglon.PorcentajeDep;
                                    //                catMaqDep.MesesTotalesDepreciacion = renglon.MesesTotalesDep;
                                    //                catMaqDep.TipoDelMovimiento = Convert.ToInt32(renglon.Tipo);
                                    //                catMaqDep.IdPolizaReferenciaAlta = null;
                                    //                catMaqDep.Estatus = true;
                                    //                catMaqDep.FechaCreacion = DateTime.Now;
                                    //                catMaqDep.IdUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                    //                catMaqDep.FechaModificacion = catMaqDep.FechaCreacion;
                                    //                catMaqDep.IdUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;

                                    //                _context.tblM_CatMaquinaDepreciacion.Add(catMaqDep);
                                    //                _context.SaveChanges();

                                    //                //catMaq.DepreciacionCapturada = true;

                                    //                //_context.tblM_CatMaquina.Attach(catMaq);
                                    //                //_context.Entry(catMaq).Property(x => x.DepreciacionCapturada).IsModified = true;
                                    //                //_context.SaveChanges();

                                    //                polizasCC.RemoveAll(x => x.Año == coincidencia.Año && x.Mes == coincidencia.Mes && x.Poliza == coincidencia.Poliza && x.TP == coincidencia.TP && x.Linea == coincidencia.Linea);

                                    //                if (renglon.FechaBaja != "")
                                    //                {
                                    //                    var coincidenciaBaja = polizasCC.FirstOrDefault(x => x.Poliza.ToString() == renglon.PolizaBaja.Substring(3) && x.TP == renglon.PolizaBaja.Substring(0, 2) && x.TM == 2 && x.Monto == (renglon.MontoBaja * -1) && x.Cuenta == polizaAlta.Cuenta);

                                    //                    if (coincidenciaBaja != null)
                                    //                    {
                                    //                        if (!_context.tblC_AF_PolizaAltaBaja.Any(x => x.Año == coincidenciaBaja.Año && x.Mes == coincidenciaBaja.Mes && x.Poliza == coincidenciaBaja.Poliza && x.TP == coincidenciaBaja.TP && x.Linea == coincidenciaBaja.Linea))
                                    //                        {
                                    //                            tblC_AF_PolizaAltaBaja polizaBaja = new tblC_AF_PolizaAltaBaja();
                                    //                            tblM_CatMaquinaDepreciacion catMaqDepBaja = new tblM_CatMaquinaDepreciacion();

                                    //                            polizaBaja.Año = coincidenciaBaja.Año;
                                    //                            polizaBaja.Mes = coincidenciaBaja.Mes;
                                    //                            polizaBaja.Poliza = coincidenciaBaja.Poliza;
                                    //                            polizaBaja.TP = coincidenciaBaja.TP;
                                    //                            polizaBaja.Linea = coincidenciaBaja.Linea;
                                    //                            polizaBaja.TM = coincidenciaBaja.TM;
                                    //                            polizaBaja.Cuenta = coincidenciaBaja.Cuenta;
                                    //                            polizaBaja.Factura = renglon.Factura;
                                    //                            polizaBaja.FechaMovimiento = Convert.ToDateTime(renglon.FechaBaja);
                                    //                            polizaBaja.TipoActivo = renglon.Tipo != "5" ? 1 : 2;
                                    //                            polizaBaja.Estatus = true;
                                    //                            polizaBaja.FechaCreacion = DateTime.Now;
                                    //                            polizaBaja.IdUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                    //                            polizaBaja.FechaModificacion = polizaBaja.FechaCreacion;
                                    //                            polizaBaja.IdUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;

                                    //                            _context.tblC_AF_PolizaAltaBaja.Add(polizaBaja);
                                    //                            _context.SaveChanges();

                                    //                            catMaqDepBaja.IdCatMaquina = idCatMaq;
                                    //                            catMaqDepBaja.IdPoliza = polizaBaja.Id;
                                    //                            catMaqDepBaja.FechaInicioDepreciacion = null;
                                    //                            catMaqDepBaja.PorcentajeDepreciacion = null;
                                    //                            catMaqDepBaja.MesesTotalesDepreciacion = null;
                                    //                            catMaqDepBaja.TipoDelMovimiento = catMaqDep.TipoDelMovimiento;
                                    //                            catMaqDepBaja.IdPolizaReferenciaAlta = polizaAlta.Id;
                                    //                            catMaqDepBaja.Estatus = true;
                                    //                            catMaqDepBaja.FechaCreacion = DateTime.Now;
                                    //                            catMaqDepBaja.IdUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                    //                            catMaqDepBaja.FechaModificacion = catMaqDepBaja.FechaCreacion;
                                    //                            catMaqDepBaja.IdUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;

                                    //                            _context.tblM_CatMaquinaDepreciacion.Add(catMaqDepBaja);
                                    //                            _context.SaveChanges();

                                    //                            polizasCC.RemoveAll(x => x.Año == coincidenciaBaja.Año && x.Mes == coincidenciaBaja.Mes && x.Poliza == coincidenciaBaja.Poliza && x.TP == coincidenciaBaja.TP && x.Linea == coincidenciaBaja.Linea);
                                    //                        }
                                    //                        else
                                    //                        {
                                    //                            var faltantes = new tblC_AF_1210ExcelFaltantes();

                                    //                            faltantes.Cuenta = renglon.Cuenta;
                                    //                            faltantes.Motivo = "Ya existe una poliza capturada (baja)";
                                    //                            faltantes.NumeroEconomico = renglon.Clave;
                                    //                            faltantes.NumeroRenglonExcel = renglon.Id;

                                    //                            _context.tblC_AF_1210ExcelFaltantes.Add(faltantes);
                                    //                            _context.SaveChanges();
                                    //                        }
                                    //                    }
                                    //                    else
                                    //                    {
                                    //                        var faltantes = new tblC_AF_1210ExcelFaltantes();

                                    //                        faltantes.Cuenta = renglon.Cuenta;
                                    //                        faltantes.Motivo = "No se encontró coicidencia en bajas";
                                    //                        faltantes.NumeroEconomico = renglon.Clave;
                                    //                        faltantes.NumeroRenglonExcel = renglon.Id;

                                    //                        _context.tblC_AF_1210ExcelFaltantes.Add(faltantes);
                                    //                        _context.SaveChanges();
                                    //                    }
                                    //                }
                                    //            }
                                    //            else
                                    //            {
                                    //                var faltantes = new tblC_AF_1210ExcelFaltantes();

                                    //                faltantes.Cuenta = renglon.Cuenta;
                                    //                faltantes.Motivo = "Ya existe una poliza capturada (alta)";
                                    //                faltantes.NumeroEconomico = renglon.Clave;
                                    //                faltantes.NumeroRenglonExcel = renglon.Id;

                                    //                _context.tblC_AF_1210ExcelFaltantes.Add(faltantes);
                                    //                _context.SaveChanges();
                                    //            }
                                    //        }
                                    //        else
                                    //        {
                                    //            var faltantes = new tblC_AF_1210ExcelFaltantes();

                                    //            faltantes.Cuenta = renglon.Cuenta;
                                    //            faltantes.Motivo = "No se encontró coicidencia en altas";
                                    //            faltantes.NumeroEconomico = renglon.Clave;
                                    //            faltantes.NumeroRenglonExcel = renglon.Id;

                                    //            _context.tblC_AF_1210ExcelFaltantes.Add(faltantes);
                                    //            _context.SaveChanges();
                                    //        }

                                    //        transaction.Commit();
                                    //    }
                                    //    catch (Exception ex)
                                    //    {
                                    //        transaction.Rollback();
                                    //    }
                                    //}
                                    //ARRENDADORA FIN
                                }
                            }

                            transaction.Commit();
                            transactionArre.Commit();

                            resultado.Add(SUCCESS, true);
                        }
                        catch (Exception ex)
                        {
                            transactionArre.Rollback();
                            transaction.Rollback();

                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Error: " + ex.ToString());
                        }
                    }
                }
            }

            return resultado;
        }
        #endregion

        #region CRUD subcuentas
        public Dictionary<string, object> GetCuentas()
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

                try
                {
                    var cuentas = _context.tblC_AF_Cuentas.Where(x => x.Estatus).Select(x => new {
                        Id = x.Id,
                        Cuenta = x.Cuenta,
                        Descripcion = x.Descripcion,
                        MesesDeDepreciacion = x.MesesDeDepreciacion,
                        PorcentajeDepreciacion = x.PorcentajeDepreciacion,
                        FechaCreacion = x.FechaCreacion,
                        IdUsuarioCreacion = x.IdUsuarioCreacion,
                        Estatus = x.Estatus
                    }).ToList();

                    resultado.Add("data", cuentas);
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al actualizar la BD. " + ex.ToString());
                }

            return resultado;
        }

        public Dictionary<string, object> GetSubCuentas(int cuenta)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

            try
            {
                var cuentaSIGOPLAN = _context.tblC_AF_Cuentas.FirstOrDefault(x => x.Estatus && x.Cuenta == cuenta);

                if (cuentaSIGOPLAN != null)
                {
                    var subCuentas = _context.tblC_AF_RelSubCuentas.Where(x => x.Estatus && !x.Excluir && x.IdCuenta == cuentaSIGOPLAN.Id).Select(x => new
                    {
                        Id = x.Id,
                        IdCuenta = x.IdCuenta,
                        Anio = x.Año,
                        Subcuenta = x.Subcuenta,
                        SubSubcuenta = x.SubSubcuenta,
                        EsOverhaul = x.EsOverhaul,
                        EsCuentaDepreciacion = x.EsCuentaDepreciacion,
                        CuentaDepreciacion = x.CuentaDepreciacion,
                        PorcentajeDepreciacion = x.PorcentajeDepreciacion,
                        MesesMaximoDepreciacion = x.MesesMaximoDepreciacion,
                        FechaCreacion = x.FechaCreacion
                    }).ToList();

                    resultado.Add("data", subCuentas);
                    resultado.Add(SUCCESS, true);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "No se encontró la cuenta.");
                }
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al obtener la información de la BD. " + ex.ToString());
            }

            return resultado;
        }
        #endregion

        #region OperacionesCompartidas
        public Dictionary<string, object> GetCuentas(int tipoResultado)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var consulta = new Object();

                switch (tipoResultado)
                {
                    case (int)TipoResultadoCuentasMaquinaria.CboxMaquinaria:
                        consulta = _context.tblM_CatTipoMaquinaria.Where(x => x.estatus).Select(s => new ComboDTO
                        {
                            Value = s.id.ToString(),
                            Prefijo = s.id == 1 ? "1210" : s.id == 2 ? "1211" : "1215",
                            Text = s.descripcion
                        }).ToList();
                        break;
                    case (int)TipoResultadoCuentasMaquinaria.CuentasMaquinaria:
                        consulta = _context.tblC_AF_Cuentas.Where(x => x.EsMaquinaria && x.Estatus).Select(m => m.Cuenta).ToList();
                        break;
                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, consulta);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Error al obtener las cuentas de maquinaria: " + ex.ToString());
            }

            return resultado;
        }

        private List<CentroCostoDTO> CCEnkontrol()
        {
            OdbcConsultaDTO odbcCCs = new OdbcConsultaDTO();
            odbcCCs.consulta = "SELECT cc, descripcion, corto FROM cc ORDER BY cc";
            List<CentroCostoDTO> CCs = _contextEnkontrol.Select<CentroCostoDTO>(_productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, odbcCCs);

            return CCs;
        }

        private List<AreaCuentaDTO> AreaCuentaEnkontrol()
        {
            OdbcConsultaDTO odbcACs = new OdbcConsultaDTO();
            //odbcACs.consulta = "SELECT DISTINCT (CAST(area  AS varchar(4)) + '-' + CAST(cuenta AS varchar(4))) AS areaCuenta, descripcion, area, cuenta FROM si_area_cuenta ORDER BY area, cuenta";
            odbcACs.consulta = "SELECT centro_costo AS CC, area, cuenta, descripcion, maquinaria AS EsMaquinaria, cc_activo As CcActivo, ac_cancelada AS AcCancelada FROM si_area_cuenta";
            List<AreaCuentaDTO> ACs = _contextEnkontrol.Select<AreaCuentaDTO>(EnkontrolAmbienteEnum.Prod, odbcACs);

            return ACs;
        }

        private List<int> Ultimos4MiercolesDelMes(DateTime fecha)
        {
            var dias = new List<int>();

            int año = fecha.Year;
            int mes = fecha.Month;
            int diasDelMes = DateTime.DaysInMonth(año, mes);
            DateTime comienzoDelMes = new DateTime(año, mes, 1);
            for (int i = 0; i < diasDelMes; i++)
            {
                if (comienzoDelMes.AddDays(i).DayOfWeek == DayOfWeek.Wednesday)
                {
                    dias.Add(i + 1);
                }
            }

            return dias;
        }

        private List<int> Ultimos4MartesDelMes(DateTime fecha)
        {
            var dias = new List<int>();

            int año = fecha.Year;
            int mes = fecha.Month;
            int diasDelMes = DateTime.DaysInMonth(año, mes);
            DateTime comienzoDelMes = new DateTime(año, mes, 1);
            for (int i = 0; i < diasDelMes; i++)
            {
                if (comienzoDelMes.AddDays(i).DayOfWeek == DayOfWeek.Tuesday)
                {
                    dias.Add(i + 1);
                }
            }

            return dias;
        }

        private List<DateTime> getDiasMartes(DateTime inicio, DateTime fin)
        {
            List<DateTime> lst = new List<DateTime>();

            while (inicio <= fin)
            {
                if (inicio.DayOfWeek == DayOfWeek.Tuesday)
                {
                    lst.Add(inicio);
                }
                inicio = inicio.AddDays(1);
            }

            var diasAEliminar = new List<DateTime>();
            foreach (var año in lst.GroupBy(g => g.Year))
            {
                foreach (var mes in año.GroupBy(g => g.Month))
                {
                    var ultimosMartes = Ultimos4MartesDelMes(new DateTime(año.Key, mes.Key, 1));
                    if (ultimosMartes.Count == 5)
                    {
                        if (mes.OrderBy(o => o.Day).First().Day == ultimosMartes.First())
                        {
                            diasAEliminar.Add(mes.OrderBy(o => o.Day).First());
                        }
                    }
                }
            }

            lst.RemoveAll(x => diasAEliminar.Contains(x));

            return lst;
        }

        public bool PolizasCapturadas()
        {

            var ultimoCorteInventario = _context.tblM_CorteInventarioMaq.ToList().LastOrDefault(x => x.Estatus && (x.Bloqueado || x.BloqueadoConstruplan));
            if (ultimoCorteInventario == null)
            {
                return true;
            }
            else
            {
                //var fecha = ultimoCorteInventario.FechaCorte.AddDays(1);
                //// 4 == mayor, overhaul, menor y transporte arrendadora.
                //return _context.tblC_AF_Polizas.Where(x => x.Estatus && x.FechaPoliza == fecha).Count() == 4 ? true : false;
                return false;
            }
        }
        #endregion

        #region OperacionesGenerales
        private List<tblC_AF_Cuenta> GetCuentasPolizas()
        {
            return _context.tblC_AF_Cuenta.Where(x => x.Estatus).ToList();
        }
        #endregion

        private tblC_AF_InfoDepreciacion GetInfoDepB(int cuenta)
        {
            return _context.tblC_AF_InfoDepreciacion.FirstOrDefault(f => f.TipoMovimientoId == 2 && f.Estatus && f.SubCuenta.Cuenta.Cuenta == cuenta);
        }

        #region InsumosOverhaul
        public Respuesta AddInsumo(tblC_AF_InsumosOverhaul insumo)
        {
            var r = new Respuesta();

            try
            {
                insumo.Estatus = true;
                insumo.FechaCreacion = DateTime.Now;
                insumo.IdUsuario = vSesiones.sesionUsuarioDTO.id;

                _context.tblC_AF_InsumosOverhaul.Add(insumo);
                _context.SaveChanges();

                r.Success = true;
                r.Message = "Ok";
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }

        public Respuesta GetInsumos()
        {
            var r = new Respuesta();

            try
            {
                var insumos = _context.tblC_AF_InsumosOverhaul.Select(m => new
                {
                    id = m.Id,
                    insumo = m.Insumo,
                    descripcion = m.Descripcion,
                    porcentaje = m.Porcentaje * 100,
                    meses = m.Meses,
                    estatus = m.Estatus
                }).ToList();

                r.Success = true;
                r.Message = "Ok";
                r.Value = insumos;
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }

        public Respuesta UpdateInsumos(List<tblC_AF_InsumosOverhaul> insumos)
        {
            var r = new Respuesta();

            try
            {
                foreach (var item in insumos)
                {
                    item.FechaCreacion = DateTime.Now;
                    item.IdUsuario = vSesiones.sesionUsuarioDTO.id;

                    _context.tblC_AF_InsumosOverhaul.AddOrUpdate(item);
                }

                _context.SaveChanges();

                r.Success = true;
                r.Message = "Ok";
                r.Value = 1;
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }

        public Respuesta AgregarInsumosAutomaticamente()
        {
            var r = new Respuesta();

            try
            {
                var insumosActuales = _context.tblC_AF_InsumosOverhaul.ToList();

                OdbcConsultaDTO odbcInsumos = new OdbcConsultaDTO();

                odbcInsumos.consulta = string.Format(@"SELECT
                                                        ins.insumo,
                                                        ins.descripcion,
                                                        ins.tipo,
                                                        ins.grupo
                                                      FROM
                                                        insumos AS ins
                                                      WHERE
                                                        ins.cancelado = 'A' AND
                                                        ins.tipo = 5 AND
                                                        ins.grupo in (50, 51)
                                                      ORDER BY ins.insumo");

                var result_insumos = _contextEnkontrol.Select<InsumosOverhaulEKDTO>(EnkontrolAmbienteEnum.Prod, odbcInsumos);

                var nuevosInsumos = new List<tblC_AF_InsumosOverhaul>();

                result_insumos.RemoveAll(rem => insumosActuales.Select(m => m.Insumo).Contains(rem.insumo));

                foreach (var insOve in result_insumos)
                {
                    var newInsumo = new tblC_AF_InsumosOverhaul();

                    var insumoDescription =  insOve.descripcion.TrimStart().Split(new char[] { ' ', '-' })[0];
                    if (insumoDescription.Where(w => Char.IsDigit(w)).Count() >= 3)
                    {
                        newInsumo.Descripcion = insumoDescription;
                        newInsumo.Estatus = true;
                        newInsumo.FechaCreacion = DateTime.Now;
                        newInsumo.IdUsuario = vSesiones.sesionUsuarioDTO.id;
                        newInsumo.Insumo = insOve.insumo;

                        var contador = int.Parse(insOve.insumo.Substring(3, 4));
                        if (contador <= 7999)
                        {
                            newInsumo.Porcentaje = .50M;
                            newInsumo.Meses = 24;
                        }
                        if (contador >= 8000 && contador <= 8999)
                        {
                            newInsumo.Porcentaje = .6666M;
                            newInsumo.Meses = 18;
                        }
                        if (contador >= 9000 && contador <= 9999)
                        {
                            newInsumo.Porcentaje = 1;
                            newInsumo.Meses = 12;
                        }

                        nuevosInsumos.Add(newInsumo);
                    }
                }

                _context.tblC_AF_InsumosOverhaul.AddRange(nuevosInsumos);
                _context.SaveChanges();

                var insumos = _context.tblC_AF_InsumosOverhaul.Select(m => new
                {
                    id = m.Id,
                    insumo = m.Insumo,
                    descripcion = m.Descripcion,
                    porcentaje = m.Porcentaje * 100,
                    meses = m.Meses,
                    estatus = m.Estatus
                }).ToList();

                r.Success = true;
                r.Message = "Ok";
                r.Value = insumos;
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }


        public Respuesta RelacionEquipoInsumo(int maquinaID)
        {
            var r = new Respuesta();

            try
            {
                List<ActivoFijoRelacionEquipoInsumoDTO> data = new List<ActivoFijoRelacionEquipoInsumoDTO>();
                var maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == maquinaID);
                var modeloMaquina = maquina.modeloEquipoID;
                var relacionModeloInsumos = _context.tblM_CatModeloEquipotblM_CatSubConjunto.Where(x => x.modeloID == modeloMaquina).ToList();
                var trackingMaquina = _context.tblM_trackComponentes.Where(x => x.locacionID == maquinaID).Select(x => x.id).ToList();
                var componentesInstalados = _context.tblM_CatComponente.Where(x => trackingMaquina.Contains(x.trackComponenteID ?? default(int))).ToList();
                var remociones = _context.tblM_ReporteRemocionComponente.Where(x => x.maquinaID == maquinaID && x.estatus < 5).Select(x => new { componentesRemovidoID = x.componenteRemovidoID }).ToList();
                var casosEspeciales = _context.tblC_AF_AjusteSubConjuntos.Where(x => x.economicoID == maquinaID).ToList();

                foreach (var item in relacionModeloInsumos) 
                {
                    List<string> auxInsumos = item.numParte.Split('/').ToList();

                    //
                    var subconjuntosIDs = new List<int>();
                    foreach (var itemInsumo in auxInsumos)
                    {
                        subconjuntosIDs.AddRange(relacionModeloInsumos.Where(x => x.subconjuntoID != item.subconjuntoID && x.numParte.Contains(itemInsumo)).Select(x => x.subconjuntoID).ToList());
                    }
                    //

                    int auxMaximo = componentesInstalados.Where(x => x.subConjuntoID.HasValue && (x.subConjuntoID == item.subconjuntoID || subconjuntosIDs.Contains(x.subConjuntoID.Value))).Count();
                    var idComponentesSubConjunto = componentesInstalados.Where(x => x.subConjuntoID.HasValue && (x.subConjuntoID == item.subconjuntoID || subconjuntosIDs.Contains(x.subConjuntoID.Value))).Select(x => x.id).ToList();
                    auxMaximo += remociones.Where(x => idComponentesSubConjunto.Contains(x.componentesRemovidoID)).Count();
                    auxMaximo += casosEspeciales.Where(x => item.subconjuntoID == x.subConjuntoID).Count();

                    ActivoFijoRelacionEquipoInsumoDTO auxData = new ActivoFijoRelacionEquipoInsumoDTO {
                        subconjuntoID = item.subconjuntoID,
                        subconjunto = item.subconjunto == null ? "" : item.subconjunto.descripcion,
                        insumo = auxInsumos,
                        maximo = auxMaximo,
                    };
                    data.Add(auxData);
                }

                r.Success = true;
                r.Message = "Ok";
                r.Value = data;
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }

        public Dictionary<string, object> CargarTablaEnvioCosto(bool enviado) 
        {
            var r = new Dictionary<string, object>();
            try
            {
                var data = _context.tblC_AF_EnviarCosto.Where(x => x.enviaACosto && x.estatus).ToList();

                foreach (var item in data)
                {
                    item.numEconomico = item.economico.noEconomico;
                    item.year = item.enviaACosto ? item.polizaCosto.Split(new char[] { '-' })[0] : null;
                    item.mes = item.enviaACosto ? item.polizaCosto.Split(new char[] { '-' })[1] : null;
                    item.poliza = item.enviaACosto ? item.polizaCosto.Split(new char[] { '-' })[2] : null;
                }

                foreach (var year in data.Where(w => !string.IsNullOrEmpty(w.year)).GroupBy(g => g.year))
                {
                    foreach (var mes in year.GroupBy(g => g.mes))
                    {
                        OdbcConsultaDTO odbcPolizas = new OdbcConsultaDTO();
                        odbcPolizas.consulta = string.Format(@"
                            SELECT DISTINCT
                                POL.year AS Año, POL.mes AS Mes, POL.tp AS TipoPoliza, POL.poliza AS Poliza,
                                POL.cargos AS Cargo, POL.abonos AS Abono, POL.fechapol AS FechaPoliza,
                                POL.generada AS Generada, POL.status AS Estatus
                            FROM
                                sc_polizas AS POL
                            WHERE
                                POL.year = ? AND
                                POL.mes = ? AND
                                POL.poliza = ? AND
                                POL.tp = '03'");
                        odbcPolizas.parametros.Add(new OdbcParameterDTO()
                        {
                            nombre = "year",
                            tipo = OdbcType.Int,
                            valor = Convert.ToInt32(year.Key)
                        });
                        odbcPolizas.parametros.Add(new OdbcParameterDTO()
                        {
                            nombre = "mes",
                            tipo = OdbcType.Int,
                            valor = Convert.ToInt32(mes.Key)
                        });
                        odbcPolizas.parametros.Add(new OdbcParameterDTO()
                        {
                            nombre = "poliza",
                            tipo = OdbcType.Int,
                            valor = Convert.ToInt32(mes.First().poliza)
                        });

                        var poliza = _contextEnkontrol.Select<PolizaDTO>(EnkontrolAmbienteEnum.Prod, odbcPolizas).FirstOrDefault();

                        if (poliza != null)
                        {
                            foreach (var item in mes)
                            {
                                item.status = poliza.Estatus;
                                item.fechaPolizaCosto = poliza.FechaPoliza;
                            }
                        }
                    }
                }

                var info = data.Select(x => new
                {
                    id = x.id,
                    enviaACosto = x.enviaACosto,
                    numEconomico = x.economico == null ? "" : x.economico.noEconomico,
                    depActual = x.depActual,
                    depFaltante = x.depFaltante,
                    descripcion = x.descripcion,
                    polizaBaja = x.polizaBaja,
                    polizaCosto = x.polizaCosto,
                    year = x.year,
                    mes = x.mes,
                    poliza = x.poliza,
                    status = x.status,
                    fechaPolizaCosto = x.fechaPolizaCosto
                });

                r.Add(SUCCESS, true);
                r.Add(ITEMS, info);
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, ex.Message);
            }
            return r;
        }

        #endregion

        #region ConstruplanColombia
        public Dictionary<string, object> TipoActivoColombiaCBox()
        {
            var resultado = new Dictionary<string, object>();


            try
            {
                var cBox = new List<ComboDTO>();

                var cuentas = _context.tblC_AF_SubcuentaColombia
                    .Where(w => w.estatus && w.cuentaColombia.tipoCuentaId == (int)AFTipoCuentaEnum.Movimiento && !w.cuentaColombia.colombiaMexico)
                    .ToList();

                foreach (var gbSubcta in cuentas.GroupBy(g => g.esMaquinaria).OrderBy(o => o.Key))
                {
                    if (gbSubcta.Key)
                    {
                        foreach (var gbCta in gbSubcta.GroupBy(g => g.tipoMaquinaId).OrderBy(o => o.Key))
                        {
                            var cb = new ComboDTO();
                            cb.Text = gbCta.First().tipoMaquina.descripcion;
                            cb.Value = gbCta.First().tipoMaquinaId.ToString();
                            cb.Prefijo = "true";

                            cBox.Add(cb);
                        }
                    }
                    else
                    {
                        foreach (var gbCta in gbSubcta.GroupBy(g => g.cuentaColombia.cuenta).OrderBy(o => o.Key))
                        {
                            var cb = new ComboDTO();
                            cb.Text = gbCta.First().cuentaColombia.descripcion;
                            cb.Value = gbCta.First().cuentaColombia.cuenta.ToString();
                            cb.Prefijo = "false";

                            cBox.Add(cb);
                        }
                    }
                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, cBox);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, ex.Message);

                LogError(11, (int)BitacoraEnum.ActivoFijo, "ActivoFijoController", "TipoActivoColombiaCBox", ex, AccionEnum.CONSULTA, 0, null);
            }

            return resultado;
        }

        public Dictionary<string, object> GetActivosColombia(int tipoActivo, bool esMaquina)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var activos = new List<ActivoDTO>();

                if (esMaquina)
                {
                    var catalogoMaquina = _context.tblM_CatMaquina
                        .Where(w => w.estatus > 0 && w.grupoMaquinaria.tipoEquipoID == tipoActivo && !string.IsNullOrEmpty(w.noEconomico))
                        .ToList();

                    foreach (var catMaq in catalogoMaquina)
                    {
                        var activo = new ActivoDTO();
                        activo.idActivo = catMaq.id;
                        activo.descripcion = catMaq.descripcion + "(" + catMaq.anio + ", " + catMaq.noSerie + ", " + catMaq.marca.descripcion + ", " + catMaq.modeloEquipo.descripcion + ", " + catMaq.proveedor + ")";
                        activo.esMaquina = esMaquina;
                        activo.numeroEconomico = catMaq.noEconomico;

                        activos.Add(activo);
                    }
                }
                else
                {
                    var catalogoNoMaquina = _context.tblC_AF_RelacionPolizaColombia
                        .Where(w => w.estatus && w.esMaquina == esMaquina && w.tmPolizaId == (int)TipoMovimientoEnum.Cargo)
                        .ToList();

                    foreach (var noMaquina in catalogoNoMaquina)
                    {
                        var activo = new ActivoDTO();
                        activo.idActivo = noMaquina.idActivo;
                        activo.descripcion = noMaquina.concepto;
                        activo.esMaquina = esMaquina;
                        activo.numeroEconomico = "";

                        activos.Add(activo);
                    }
                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, activos);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, ex.Message);

                LogError(11, (int)BitacoraEnum.ActivoFijo, "ActivoFijoController", "GetActivosColombia", ex, AccionEnum.CONSULTA, 0, new { tipoActivo, esMaquina });
            }

            return resultado;
        }

        public Dictionary<string, object> GetRelacionPoliza(int idActivo, bool esMaquina)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var polizasRelacioandas = new List<PolizaRelacionadaDTO>();

                var polizas = _context.tblC_AF_RelacionPolizaColombia
                    .Where(w => w.estatus && w.esMaquina == esMaquina && w.idActivo == idActivo)
                    .ToList();

                foreach (var pol in polizas)
                {
                    var polizaRelacionada = new PolizaRelacionadaDTO();
                    polizaRelacionada.concepto = pol.concepto;
                    polizaRelacionada.cuenta = pol.ctaPoliza + "-" + pol.sctaPoliza + "-" + pol.ssctaPoliza;
                    polizaRelacionada.factura = pol.factura ?? "";
                    polizaRelacionada.fechaInicioDep = pol.fechaInicioDep;
                    polizaRelacionada.fechaMovimiento = pol.fechaMovimiento;
                    polizaRelacionada.id = pol.id;
                    polizaRelacionada.mesesDep = pol.mesesDep;
                    polizaRelacionada.monto = pol.montoPoliza;
                    polizaRelacionada.poliza = pol.yearPoliza + "-" + pol.mesPoliza + "-" + pol.polizaPoliza + "-" + pol.tpPoliza + "-" + pol.lineaPoliza;
                    polizaRelacionada.polizaRelacion = pol.relacionPolizaColombiaId_baja != null ? pol.relacionPolizaColombia_baja.yearPoliza + "-" + pol.relacionPolizaColombia_baja.mesPoliza + "-" + pol.relacionPolizaColombia_baja.polizaPoliza + "-" + pol.relacionPolizaColombia_baja.tpPoliza + "-" + pol.relacionPolizaColombia_baja.lineaPoliza : "";
                    polizaRelacionada.porcentajeDep = pol.porcentajeDep;
                    polizaRelacionada.tm = pol.tmPolizaId;

                    polizasRelacioandas.Add(polizaRelacionada);
                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, polizasRelacioandas);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, ex.Message);

                LogError(11, (int)BitacoraEnum.ActivoFijo, "ActivoFijoController", "GetRelacionPoliza", ex, AccionEnum.CONSULTA, 0, new { idActivo, esMaquina });
            }

            return resultado;
        }

        private List<CedulaColombiaDTO> construirDetallesColombia(DateTime fechaFin, List<int> cuentasActivos)
        {
            var yearActual = fechaFin.Year;
            var mesActual = fechaFin.Month;

            var infoCedula = new List<CedulaColombiaDTO>();

            using (var ctx = new MainContext(EmpresaEnum.Construplan))
            {
                var cuentas = ctx.tblC_AF_CuentaColombia
                    .Where(w =>
                        w.estatus &&
                        !w.colombiaMexico &&
                        w.tipoCuentaId == (int)AFTipoCuentaEnum.Movimiento &&
                        cuentasActivos.Contains(w.cuenta))
                    .ToList();

                foreach (var cuenta in cuentas)
                {
                    var movimientosCargos = ctx.tblC_AF_RelacionPolizaColombia
                        .Where(w =>
                            w.ctaPoliza == cuenta.cuenta &&
                            w.tmPolizaId == (int)AFTipoMovimientoEnum.Cargo)
                        .ToList();

                    #region detalles
                    var ajusteDepreciacion = ctx.tblC_AF_AjusteDepreciacionColombia.Where(w => w.relacionPoliza.ctaPoliza == cuenta.cuenta && w.fechaAjuste <= fechaFin && w.estatus).ToList();

                    var detalles = new List<CedulaDetalleColombiaDTO>();

                    foreach (var mov in movimientosCargos)
                    {
                        if (mov.relacionPolizaColombiaAjuste.Count > 0)
                        {
                            foreach (var infoAjuste in mov.relacionPolizaColombiaAjuste.GroupBy(g => g.tipoPolizaDeAjusteId))
                            {
                                switch (infoAjuste.Key)
                                {
                                    case (int)AFTipoPolizaAjusteEnum.AjusteDeMonto:
                                        mov.montoPoliza = mov.montoPoliza + infoAjuste.Sum(s => s.montoPoliza);
                                        break;
                                }
                            }
                        }

                        if (mov.relacionPolizaColombiaId_baja.HasValue && mov.relacionPolizaColombia_baja.relacionPolizaColombiaAjuste.Count > 0)
                        {
                            foreach (var infoAjuste in mov.relacionPolizaColombia_baja.relacionPolizaColombiaAjuste.GroupBy(g => g.tipoPolizaDeAjusteId))
                            {
                                switch (infoAjuste.Key)
                                {
                                    case (int)AFTipoPolizaAjusteEnum.AjusteDeMonto:
                                        mov.relacionPolizaColombia_baja.montoPoliza = mov.relacionPolizaColombia_baja.montoPoliza + infoAjuste.Sum(s => s.montoPoliza);
                                        break;
                                }
                            }
                        }

                        var detalle = new CedulaDetalleColombiaDTO();
                        detalle.id = mov.id;
                        detalle.esMaquina = mov.esMaquina;
                        detalle.idActivo = mov.idActivo;
                        detalle.factura = mov.factura;
                        detalle.noEconomico = mov.numEconomico;
                        detalle.descripcion = mov.concepto;
                        detalle.fechaMovimiento = mov.fechaMovimiento;
                        detalle.fechaInicioDep = mov.fechaInicioDep ?? mov.fechaMovimiento;
                        detalle.mesesDepreciacion = mov.mesesDep.Value;
                        detalle.porcentajeDepreciacion = mov.porcentajeDep.Value * 100;
                        detalle.capturaAutomatica = mov.capturaAutomatica;
                        detalle.cc = mov.cc = mov.cc;
                        detalle.ccDescripcion = mov.ccDescripcion;
                        detalle.ccCompleto = "[" + mov.cc + "] " + mov.ccDescripcion;
                        detalle.polYear = mov.yearPoliza;
                        detalle.polMes = mov.mesPoliza;
                        detalle.polPoliza = mov.polizaPoliza;
                        detalle.polTp = mov.tpPoliza;
                        detalle.polLinea = mov.lineaPoliza;
                        detalle.polizaCompleta = mov.yearPoliza + "-" + mov.mesPoliza + "-" + mov.polizaPoliza + "-" + mov.tpPoliza + "-" + mov.lineaPoliza;
                        detalle.tmPolizaId = mov.tmPolizaId;
                        detalle.cta = mov.ctaPoliza;
                        detalle.scta = mov.sctaPoliza;
                        detalle.sscta = mov.ssctaPoliza;
                        detalle.cuentaCompleta = mov.ctaPoliza + "-" + mov.sctaPoliza + "-" + mov.ssctaPoliza;
                        detalle.moi = mov.fechaMovimiento.Year < yearActual ? mov.montoPoliza : 0M;
                        detalle.alta = mov.fechaMovimiento.Year == yearActual ? mov.montoPoliza : 0M;
                        detalle.fechaPoliza = mov.fechaPoliza;
                        detalle.baja = mov.relacionPolizaColombiaId_baja.HasValue ? mov.relacionPolizaColombia_baja.montoPoliza : 0M;
                        detalle.fechaBaja = mov.relacionPolizaColombiaId_baja.HasValue ? (DateTime?)mov.relacionPolizaColombia_baja.fechaMovimiento : null;
                        detalle.tmPolizaId_Baja = mov.relacionPolizaColombiaId_baja.HasValue ? (int?)mov.relacionPolizaColombia_baja.tmPolizaId : null;
                        detalle.depMensual = (detalle.moi + detalle.alta) / detalle.mesesDepreciacion;

                        if (detalle.fechaInicioDep <= fechaFin)
                        {
                            var diferenciaDeMeses = !detalle.fechaBaja.HasValue ? DatetimeUtils.MesesDiferencia(detalle.fechaInicioDep, fechaFin) : DatetimeUtils.MesesDiferencia(detalle.fechaInicioDep, detalle.fechaBaja.Value);
                            diferenciaDeMeses = diferenciaDeMeses > detalle.mesesDepreciacion ? detalle.mesesDepreciacion : diferenciaDeMeses;
                            var fechaFinDep = detalle.fechaInicioDep.AddMonths(diferenciaDeMeses);

                            detalle.diasDepPrimerMes = 30 - (detalle.fechaInicioDep.Day <= 30 ? detalle.fechaInicioDep.Day : 30);

                            if (fechaFinDep.Year == yearActual)
                            {
                                if (detalle.fechaInicioDep.Year == yearActual)
                                {
                                    detalle.mesesDepreciadosActualmente = fechaFinDep.Month - 1;
                                }
                                else
                                {
                                    detalle.mesesDepreciadosActualmente = fechaFin.Month;
                                    detalle.mesesDepreciadosAnteriormente = diferenciaDeMeses - fechaFin.Month;
                                }
                            }
                            else
                            {
                                detalle.mesesDepreciadosAnteriormente = diferenciaDeMeses;
                            }

                            #region ajustesDepreciación
                            var ajustesDepActivo = ajusteDepreciacion.Where(x => x.relacionPolizaID == mov.id).ToList();

                            foreach (var ajusteDep in ajustesDepActivo)
                            {
                                if (fechaFinDep >= ajusteDep.fechaAjuste)
                                {
                                    switch (ajusteDep.tipoAjusteDepID)
                                    {
                                        case (int)TipoAjusteDepColombiaEnum.MONTO:
                                            if (fechaFinDep.Year == ajusteDep.fechaAjuste.Year)
                                            {
                                                detalle.depreciacionActual += ajusteDep.montoAjuste;
                                            }
                                            else
                                            {
                                                detalle.depreciacionAnterior += ajusteDep.montoAjuste;
                                            }
                                            break;
                                        case (int)TipoAjusteDepColombiaEnum.MESES:
                                            if (fechaFinDep.Year == ajusteDep.fechaAjuste.Year)
                                            {
                                                detalle.mesesDepreciadosActualmente += (int)ajusteDep.montoAjuste;
                                            }
                                            else
                                            {
                                                detalle.mesesDepreciadosAnteriormente += (int)ajusteDep.montoAjuste;
                                            }
                                            break;
                                        case (int)TipoAjusteDepColombiaEnum.DIAS:
                                            detalle.diasDepPrimerMes += (int)ajusteDep.montoAjuste;
                                            break;
                                    }
                                }
                            }
                            #endregion

                            #region Depreciación por días del primer mes
                            if (detalle.diasDepPrimerMes > 0)
                            {
                                if (detalle.fechaInicioDep.Year == fechaFinDep.Year)
                                {
                                    detalle.depreciacionActual += ((detalle.depMensual / 30) * detalle.diasDepPrimerMes);
                                }
                                else
                                {
                                    detalle.depreciacionAnterior += ((detalle.depMensual / 30) * detalle.diasDepPrimerMes);
                                }
                            }
                            #endregion
                        }

                        #region Depreciación del ultimo mes la cual es por días
                        if (detalle.mesesDepreciacion == detalle.mesesDepreciadosAnteriormente + detalle.mesesDepreciadosActualmente)
                        {
                            if (detalle.mesesDepreciadosActualmente > 0)
                            {
                                detalle.depreciacionActual -= detalle.depMensual;
                                detalle.depreciacionActual += ((detalle.depMensual / 30) * (30 - detalle.diasDepPrimerMes));
                            }
                            else
                            {
                                detalle.depreciacionAnterior -= detalle.depMensual;
                                detalle.depreciacionAnterior += ((detalle.depMensual / 30) * (30 - detalle.diasDepPrimerMes));
                            }
                        }
                        #endregion

                        detalle.depreciacionAnterior += detalle.mesesDepreciadosAnteriormente * detalle.depMensual;
                        detalle.depreciacionActual += detalle.mesesDepreciadosActualmente * detalle.depMensual;
                        detalle.bajaDepreciacion += detalle.fechaBaja.HasValue ? detalle.depreciacionAnterior + detalle.depreciacionActual : 0M;
                        detalle.depreciacionAcumulada += detalle.depreciacionAnterior + detalle.depreciacionActual - detalle.bajaDepreciacion;
                        detalle.saldoLibro += detalle.fechaBaja.HasValue ? 0M : detalle.moi + detalle.alta - detalle.depreciacionAcumulada;
                        detalle.depTerminadaPorTiempo = detalle.mesesDepreciadosAnteriormente + detalle.mesesDepreciadosActualmente == detalle.mesesDepreciacion;

                        detalles.Add(detalle);
                    }
                    #endregion

                    #region saldo
                    var saldo = new CedulaSaldoColombiaDTO();

                    var saldosMovimientosMN = getInfoCuentaMovimientoColombia(yearActual, mesActual, cuenta.cuentaMexico.cuenta, false, true);
                    var saldosContabilidadMN = getInfoCuentaSaldoColombia(yearActual, cuenta.cuentaMexico.cuenta, false, true);

                    var saldosContabilidadColombia = getInfoCuentaSaldoColombia(yearActual, cuenta.cuenta, false, false);

                    saldo.esMaquina = cuenta.subcuentas.Any(a => a.esMaquinaria);
                    saldo.cuenta = cuenta.cuenta;
                    saldo.concepto = cuenta.descripcion;
                    saldo.saldoAnteriorMN = saldosMovimientosMN.Where(w => w.Year < yearActual).Sum(s => s.Monto);
                    saldo.saldoAnteriorCOP = detalles.Sum(s => s.moi);
                    saldo.saldoAnteriorCOP += detalles.Where(w => w.fechaBaja.HasValue && w.fechaBaja.Value.Year < yearActual).Sum(s => s.baja);
                    saldo.altaActualMN = saldosMovimientosMN.Where(w => w.Year == yearActual && (w.TM == (int)AFTipoMovimientoEnum.Cargo || w.TM == (int)AFTipoMovimientoEnum.CargoRojo)).Sum(s => s.Monto);
                    saldo.altaActualCOP = detalles.Sum(s => s.alta);
                    saldo.altaActualCOP += detalles.Where(w => w.fechaBaja.HasValue && w.fechaBaja.Value.Year == yearActual && w.tmPolizaId_Baja == (int)AFTipoMovimientoEnum.CargoRojo).Sum(s => s.baja);
                    saldo.bajaActualMN = saldosMovimientosMN.Where(w => w.Year == yearActual && w.TM == (int)AFTipoMovimientoEnum.Abono).Sum(s => s.Monto);
                    saldo.bajaActualCOP = detalles.Where(w => w.fechaBaja.HasValue && w.fechaBaja.Value.Year == yearActual && w.tmPolizaId_Baja == (int)AFTipoMovimientoEnum.Abono).Sum(s => s.baja);
                    saldo.saldoActualMN = saldo.saldoAnteriorMN + saldo.altaActualMN + saldo.bajaActualMN;
                    saldo.saldoActualCOP = saldo.saldoAnteriorCOP + saldo.altaActualCOP + saldo.bajaActualCOP;

                    saldo.contabilidadCOP = saldosContabilidadColombia
                        .Where(w =>
                            w.Year < yearActual
                        ).Sum(s =>
                            s.Enecargos + s.Eneabonos + s.Febcargos + s.Febabonos + s.Marcargos + s.Marabonos + s.Abrcargos + s.Abrabonos +
                            s.Maycargos + s.Mayabonos + s.Juncargos + s.Junabonos + s.Julcargos + s.Julabonos + s.Agocargos + s.Agoabonos +
                            s.Sepcargos + s.Sepabonos + s.Octcargos + s.Octabonos + s.Novcargos + s.Novabonos + s.Diccargos + s.Dicabonos
                        );

                    for (int mes = 1; mes <= mesActual; mes++)
                    {
                        var mesCargo = Enum.GetName(typeof(MesesCargoTablaContabilidad), mes);
                        var mesAbono = Enum.GetName(typeof(MesesAbonosTablaContabilidad), mes);

                        saldo.contabilidadCOP += saldosContabilidadColombia
                            .Where(w =>
                                w.Year == yearActual
                            ).Sum(s => Convert.ToDecimal(s.GetType().GetProperty(mesCargo).GetValue(s, null)) + Convert.ToDecimal(s.GetType().GetProperty(mesAbono).GetValue(s, null)));
                    }

                    saldo.contabilidadMN = saldosContabilidadMN
                        .Where(w =>
                            w.Year < yearActual
                        ).Sum(s =>
                            s.Enecargos + s.Eneabonos + s.Febcargos + s.Febabonos + s.Marcargos + s.Marabonos + s.Abrcargos + s.Abrabonos +
                            s.Maycargos + s.Mayabonos + s.Juncargos + s.Junabonos + s.Julcargos + s.Julabonos + s.Agocargos + s.Agoabonos +
                            s.Sepcargos + s.Sepabonos + s.Octcargos + s.Octabonos + s.Novcargos + s.Novabonos + s.Diccargos + s.Dicabonos
                        );

                    for (int mes = 1; mes <= mesActual; mes++)
                    {
                        var mesCargo = Enum.GetName(typeof(MesesCargoTablaContabilidad), mes);
                        var mesAbono = Enum.GetName(typeof(MesesAbonosTablaContabilidad), mes);

                        saldo.contabilidadMN += saldosContabilidadMN
                            .Where(w =>
                                w.Year == yearActual
                            ).Sum(s => Convert.ToDecimal(s.GetType().GetProperty(mesCargo).GetValue(s, null)) + Convert.ToDecimal(s.GetType().GetProperty(mesAbono).GetValue(s, null)));
                    }

                    saldo.diferenciaMN = saldo.saldoActualMN - saldo.contabilidadMN;
                    saldo.diferenciaCOP = saldo.saldoActualCOP - saldo.contabilidadCOP;
                    #endregion

                    #region cedula depreciacion
                    var dep = new CedulaDepColombiaDTO();

                    var depMovimientosMN = getInfoCuentaMovimientoColombia(yearActual, mesActual, cuenta.cuentaMexico.cuenta, true, true);
                    var depContabilidadMN = getInfoCuentaSaldoColombia(yearActual, cuenta.cuentaMexico.cuenta, true, true);

                    var depContabilidadColombia = getInfoCuentaSaldoColombia(yearActual, cuenta.cuenta, true, false);

                    dep.esMaquina = cuenta.subcuentas.Any(a => a.esMaquinaria);
                    dep.cuenta = cuenta.cuenta;
                    dep.concepto = cuenta.descripcion;
                    dep.depActualMN = Math.Abs(depMovimientosMN.Where(w => w.Year == yearActual).Sum(s => s.Monto));
                    dep.depActualCOP = detalles.Sum(s => s.depreciacionActual + s.bajaDepreciacion);
                    dep.depAcumuladaMN = Math.Abs(depMovimientosMN.Sum(s => s.Monto));
                    dep.depAcumuladaCOP = detalles.Sum(s => s.depreciacionAcumulada);

                    dep.depContabilidadCOP = depContabilidadColombia
                        .Where(w =>
                            w.Year < yearActual
                        ).Sum(s =>
                            s.Enecargos + s.Eneabonos + s.Febcargos + s.Febabonos + s.Marcargos + s.Marabonos + s.Abrcargos + s.Abrabonos +
                            s.Maycargos + s.Mayabonos + s.Juncargos + s.Junabonos + s.Julcargos + s.Julabonos + s.Agocargos + s.Agoabonos +
                            s.Sepcargos + s.Sepabonos + s.Octcargos + s.Octabonos + s.Novcargos + s.Novabonos + s.Diccargos + s.Dicabonos
                        );

                    for (int mes = 1; mes <= mesActual; mes++)
                    {
                        var mesCargo = Enum.GetName(typeof(MesesCargoTablaContabilidad), mes);
                        var mesAbono = Enum.GetName(typeof(MesesAbonosTablaContabilidad), mes);

                        dep.depContabilidadCOP += depContabilidadColombia
                            .Where(w =>
                                w.Year == yearActual
                            ).Sum(s => Convert.ToDecimal(s.GetType().GetProperty(mesCargo).GetValue(s, null)) + Convert.ToDecimal(s.GetType().GetProperty(mesAbono).GetValue(s, null)));
                    }

                    dep.depContabilidadCOP = Math.Abs(dep.depContabilidadCOP);

                    dep.depContabilidadMN = depContabilidadMN
                        .Where(w =>
                            w.Year < yearActual
                        ).Sum(s =>
                            s.Enecargos + s.Eneabonos + s.Febcargos + s.Febabonos + s.Marcargos + s.Marabonos + s.Abrcargos + s.Abrabonos +
                            s.Maycargos + s.Mayabonos + s.Juncargos + s.Junabonos + s.Julcargos + s.Julabonos + s.Agocargos + s.Agoabonos +
                            s.Sepcargos + s.Sepabonos + s.Octcargos + s.Octabonos + s.Novcargos + s.Novabonos + s.Diccargos + s.Dicabonos
                        );

                    for (int mes = 1; mes <= mesActual; mes++)
                    {
                        var mesCargo = Enum.GetName(typeof(MesesCargoTablaContabilidad), mes);
                        var mesAbono = Enum.GetName(typeof(MesesAbonosTablaContabilidad), mes);

                        dep.depContabilidadMN += depContabilidadMN
                            .Where(w =>
                                w.Year == yearActual
                            ).Sum(s => Convert.ToDecimal(s.GetType().GetProperty(mesCargo).GetValue(s, null)) + Convert.ToDecimal(s.GetType().GetProperty(mesAbono).GetValue(s, null)));
                    }

                    dep.depContabilidadMN = Math.Abs(dep.depContabilidadMN);

                    dep.diferenciaMN = dep.depAcumuladaMN - Math.Abs(dep.depContabilidadMN);
                    dep.diferenciaCOP = dep.depAcumuladaCOP - Math.Abs(dep.depContabilidadCOP);
                    #endregion

                    var cedulaCuenta = new CedulaColombiaDTO();
                    cedulaCuenta.saldo = saldo;
                    cedulaCuenta.dep = dep;
                    cedulaCuenta.detalle = detalles;

                    infoCedula.Add(cedulaCuenta);
                }
            }

            return infoCedula;
        }

        private List<ActivoFijoSaldosContablesDTO> getInfoCuentaSaldoColombia(int yearFin, int cuenta, bool esDepreciacion, bool colombiaMexico)
        {
            List<tblC_AF_RelacionCuentaYearColombia> relacionCuenta = null;

            using (var ctx = new MainContext(EmpresaEnum.Construplan))
            {
                if (!esDepreciacion)
                {
                    relacionCuenta = ctx.tblC_AF_RelacionCuentaYearColombia
                        .Where(w =>
                            w.estatus &&
                            w.year <= yearFin &&
                            w.cuentaMovimientoId == null &&
                            w.subcuentaColombia.estatus &&
                            w.subcuentaColombia.cuentaColombia.estatus &&
                            w.subcuentaColombia.cuentaColombia.cuenta == cuenta &&
                            w.subcuentaColombia.cuentaColombia.colombiaMexico == colombiaMexico
                    ).ToList();
                }
                else
                {
                    relacionCuenta = ctx.tblC_AF_RelacionCuentaYearColombia
                        .Where(w =>
                            w.estatus &&
                            w.year <= yearFin &&
                            w.cuentaMovimientoId != null &&
                            w.cuentaColombia.estatus &&
                            w.cuentaColombia.cuenta == cuenta &&
                            w.subcuentaColombia.estatus &&
                            w.subcuentaColombia.cuentaColombia.colombiaMexico == colombiaMexico
                    ).ToList();
                }

                var where_sc_salcont_cc_subcuenta = "";

                foreach (var relCta in relacionCuenta)
                {
                    where_sc_salcont_cc_subcuenta += "(scta = ? AND sscta = ?)";

                    if (relCta != relacionCuenta.Last())
                    {
                        where_sc_salcont_cc_subcuenta += " OR ";
                    }
                }

                var query_sc_salcont_cc = new OdbcConsultaDTO();

                query_sc_salcont_cc.consulta = string.Format
                    (
                        @"SELECT
                        *
                    FROM
                        DBA.sc_salcont_cc
                    WHERE
                        year <= ? AND
                        cta = ? AND
                        (
                            {0}
                        )",
                              where_sc_salcont_cc_subcuenta
                    );

                query_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "year",
                    tipo = OdbcType.Int,
                    valor = yearFin
                });
                query_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "cta",
                    tipo = OdbcType.Int,
                    valor = relacionCuenta.First().subcuentaColombia.cuentaColombia.cuenta
                });

                foreach (var relCuenta in relacionCuenta)
                {
                    query_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "scta",
                        tipo = OdbcType.Int,
                        valor = relCuenta.subcuentaColombia.scta
                    });
                    query_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "sscta",
                        tipo = OdbcType.Int,
                        valor = relCuenta.subcuentaColombia.sscta
                    });
                }

                var sc_salcont_cc = _contextEnkontrol.Select<ActivoFijoSaldosContablesDTO>(colombiaMexico ? vSesiones.sesionAmbienteEnkontrolAdm : EnkontrolAmbienteEnum.Colombia, query_sc_salcont_cc);

                return sc_salcont_cc;
            }
        }

        private List<sc_movpolDTO> getInfoCuentaMovimientoColombia(int yearFin, int mesFin, int cuenta, bool esDepreciacion, bool colombiaMexico)
        {
            List<tblC_AF_RelacionCuentaYearColombia> relacionCuenta = null;

            using (var ctx = new MainContext(EmpresaEnum.Construplan))
            {
                if (!esDepreciacion)
                {
                    relacionCuenta = ctx.tblC_AF_RelacionCuentaYearColombia
                        .Where(w =>
                            w.estatus &&
                            w.year <= yearFin &&
                            w.cuentaMovimientoId == null &&
                            w.subcuentaColombia.estatus &&
                            w.subcuentaColombia.cuentaColombia.estatus &&
                            w.subcuentaColombia.cuentaColombia.cuenta == cuenta &&
                            w.subcuentaColombia.cuentaColombia.colombiaMexico == colombiaMexico
                        ).ToList();
                }
                else
                {
                    relacionCuenta = ctx.tblC_AF_RelacionCuentaYearColombia
                        .Where(w =>
                            w.estatus &&
                            w.year <= yearFin &&
                            w.cuentaMovimientoId != null &&
                            w.subcuentaColombia.estatus &&
                            w.cuentaColombia.estatus &&
                            w.cuentaColombia.cuenta == cuenta &&
                            w.subcuentaColombia.cuentaColombia.colombiaMexico == colombiaMexico
                        ).ToList();
                }

                if (relacionCuenta.Count == 0)
                {
                    return new List<sc_movpolDTO>();
                }

                var where_sc_movpol_subcuenta = "";

                foreach (var relCta in relacionCuenta)
                {
                    where_sc_movpol_subcuenta += "(MOV.scta = ? AND MOV.sscta = ?)";

                    if (relCta != relacionCuenta.Last())
                    {
                        where_sc_movpol_subcuenta += " OR ";
                    }
                }

                var query_sc_movpol = new OdbcConsultaDTO();

                query_sc_movpol.consulta = string.Format
                    (
                        @"SELECT
                        MOV.year,
                        MOV.mes,
                        MOV.poliza,
                        MOV.tp,
                        MOV.linea,
                        MOV.cta,
                        MOV.scta,
                        MOV.sscta,
                        MOV.tm,
                        MOV.referencia,
                        MOV.cc,
                        MOV.concepto,
                        MOV.monto,
                        POL.fechapol
                    FROM
                        DBA.sc_movpol AS MOV
                    INNER JOIN
                        DBA.sc_polizas AS POL
                        ON
                            POL.year = MOV.year AND
                            POL.mes = MOV.mes AND
                            POL.tp = MOV.tp AND
                            POL.poliza = MOV.poliza
                    WHERE
                        MOV.cta = ? AND
                        (
                            {0}
                        ) AND
                        (
                            (MOV.year < ?) OR
                            (MOV.year = ? AND MOV.mes <= ?)
                        ) AND MOV.tm IN (2, 4)",
                              where_sc_movpol_subcuenta
                    );

                query_sc_movpol.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "cta",
                    tipo = OdbcType.Int,
                    valor = relacionCuenta.First().subcuentaColombia.cuentaColombia.cuenta
                });
                foreach (var relCuenta in relacionCuenta)
                {
                    query_sc_movpol.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "scta",
                        tipo = OdbcType.Int,
                        valor = relCuenta.subcuentaColombia.scta
                    });
                    query_sc_movpol.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "sscta",
                        tipo = OdbcType.Int,
                        valor = relCuenta.subcuentaColombia.sscta
                    });
                }
                query_sc_movpol.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "year",
                    tipo = OdbcType.Int,
                    valor = yearFin
                });
                query_sc_movpol.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "year",
                    tipo = OdbcType.Int,
                    valor = yearFin
                });
                query_sc_movpol.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "mes",
                    tipo = OdbcType.Int,
                    valor = mesFin
                });

                var sc_movpol = _contextEnkontrol.Select<sc_movpolDTO>(colombiaMexico ? vSesiones.sesionAmbienteEnkontrolAdm : EnkontrolAmbienteEnum.Colombia, query_sc_movpol);

                return sc_movpol;
            }
        }
        #endregion

        #region ConstruplanPeru
        public Dictionary<string, object> GetAnios()
        {
            int anioInicial = 2022;
            int anioActual = DateTime.Now.Year;

            var anios = new List<ComboBoxDTO>();

            for (int i = anioInicial; i <= anioActual; i++)
            {
                var anio = new ComboBoxDTO();
                anio.texto = i.ToString();
                anio.valor = anio.texto;
                anios.Add(anio);
            }

            var resultado = new Dictionary<string, object>();
            resultado.Add(SUCCESS, true);
            resultado.Add(ITEMS, anios);

            return resultado;
        }

        public Dictionary<string, object> GetCCs()
        {
            var resultado = new Dictionary<string, object>();

            using (var ctx = new MainContext(EmpresaEnum.Peru))
            {
                try
                {
                    var ccs = ctx.tblP_CC.Where(x => x.cc.Length > 3).Select(x => new ComboBoxDTO
                    {
                        valor = x.cc.ToString(),
                        texto = "[" + x.cc + "] " + x.descripcion.Trim()
                    }).ToList();

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, ccs);
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetCuentasPeru()
        {
            var resultado = new Dictionary<string, object>();

            using (var ctx = new MainContext(EmpresaEnum.Construplan))
            {
                try
                {
                    var cuentas = ctx.tblC_AF_CuentaPeru.
                        Where(x =>
                            x.estatus &&
                            !x.peruMexico &&
                            x.tipoCuentaId == (int)AFTipoCuentaEnum.Movimiento)
                        .Select(x => new ComboBoxDTO
                        {
                            valor = x.cuenta.ToString(),
                            texto = "[" + x.cuenta.ToString() + "] " + x.descripcion
                        }).ToList();

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, cuentas);
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetActivos(int? anio, string cc, int? cuenta)
        {
            var resultado = new Dictionary<string, object>();

            using (var ctx = new MainContext(EmpresaEnum.Construplan))
            {
                try
                {
                    var datos = ctx.tblC_AF_RelacionPolizaPeru
                        .Where(x =>
                            x.estatus &&
                            (anio.HasValue ? x.fechaMovimiento.Year == anio : true) &&
                            (string.IsNullOrEmpty(cc) ? true : x.cc == cc) &&
                            (cuenta.HasValue ? x.ctaPoliza == cuenta : true) &&
                            x.tmPolizaId == (int)AFTipoMovimientoEnum.Cargo)
                        .Select(x => new RelacionPolizaDTO
                        {
                            id = x.id,
                            tipoActivo = x.esMaquina ? "MAQUINARIA" : "OFICINA",
                            nombreActivo = x.esMaquina ? x.numEconomico : x.concepto,
                            fechaInicioDepDT = x.fechaInicioDep,
                            porcentajeDep = x.porcentajeDep.HasValue ? x.porcentajeDep.Value : 0,
                            mesesDep = x.mesesDep.HasValue ? x.mesesDep.Value : 0,
                            cc = "[" + x.cc + "] " + x.ccDescripcion,
                            polizaPoliza = x.polizaPoliza,
                            poliza = x.yearPoliza + "-" + x.mesPoliza + "-" + x.tpPoliza + "-",
                            cuenta = x.ctaPoliza,
                            monto = x.montoPoliza
                        }).ToList();

                    foreach (var item in datos)
                    {
                        item.fechaInicioDep = item.fechaInicioDepDT.HasValue ? item.fechaInicioDepDT.Value.ToShortDateString() : "";
                        item.poliza += item.polizaPoliza.ToString("D4");
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, datos);
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetEconomicosPeru()
        {
            var resultado = new Dictionary<string, object>();

            using (var ctx = new MainContext(EmpresaEnum.Peru))
            {
                try
                {
                    var economicos = ctx.tblM_CatMaquina.Select(x => new ComboBoxDTO
                    {
                        valor = x.id.ToString(),
                        texto = x.noEconomico
                    }).ToList();

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, economicos);
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarRelacionActivo(tblC_AF_RelacionPolizaPeru obj)
        {
            var resultado = new Dictionary<string, object>();

            using (var ctx = new MainContext(EmpresaEnum.Construplan))
            {
                using(var ctxPeru = new MainContext(EmpresaEnum.Peru))
	            {
	                try
                    {
                        if (ctx.tblC_AF_RelacionPolizaPeru
                            .Any(x =>
                                x.yearPoliza == obj.fechaMovimiento.Year &&
                                x.mesPoliza == obj.fechaMovimiento.Month &&
                                x.polizaPoliza == obj.polizaPoliza &&
                                x.lineaPoliza == obj.lineaPoliza &&
                                x.ctaPoliza == obj.ctaPoliza &&
                                x.estatus))
                        {
                            throw new Exception("Ya se encuentra registrada una póliza con esta información");
                        }



                        var sqlQuery = string.Format(@"
                            SELECT TOP 1
                                SUBDIAR_CODIGO
                            FROM
                                [003BDCONT{0}]..DETMOV{1}
                            WHERE
                                SUBDIAR_CODIGO = {2} AND
                                DMOV_C_COMPR = {3} AND
                                DMOV_SECUE = {4} AND
                                DMOV_CUENT = {5} AND
                                DMOV_DEBE = {6}",
                            obj.fechaMovimiento.Year,
                            obj.fechaMovimiento.Month.ToString("D2"),
                            obj.tpPoliza,
                            obj.polizaPoliza.ToString("D4"),
                            obj.lineaPoliza.ToString("D5"),
                            obj.ctaPoliza,
                            obj.montoPoliza);

                        using (var conexion = new SqlConnection(ConextSigoDapper.conexionStarsoftBancos()))
                        {
                            DynamicParameters lstParametros = new DynamicParameters();

                            conexion.Open();
                            var datos = conexion.Query<dynamic>(sqlQuery, lstParametros, null, true, 300, CommandType.Text).ToList();
                            conexion.Close();

                            if (datos.Count == 0)
                            {
                                throw new Exception("No se encontró información de esta póliza en Starsoft");
                            }
                        }

                        if (obj.esMaquina)
                        {
                            var economico = ctxPeru.tblM_CatMaquina.FirstOrDefault(x => x.id == obj.idActivo);
                                if (economico != null)
                                {
                                    obj.numEconomico = economico.noEconomico.Trim();
                                }
                        }
                        else
                        {
                            var ultimoIdActivo = ctx.tblC_AF_RelacionPolizaPeru.Where(x => !x.esMaquina).Select(x => x.idActivo).OrderByDescending(x => x).FirstOrDefault() + 1;
                            obj.idActivo = ultimoIdActivo;
                        }

                        obj.capturaAutomatica = false;

                        var cc = ctxPeru.tblP_CC.FirstOrDefault(x => x.cc == obj.cc);
                        if (cc != null)
                        {
                            obj.ccDescripcion = cc.descripcion.Trim();
                        }

                        obj.tmPolizaId = 1;
                        obj.yearPoliza = obj.fechaMovimiento.Year;
                        obj.mesPoliza = obj.fechaMovimiento.Month;
                        obj.ccPoliza = obj.cc;
                        obj.referenciaPoliza = "";
                        obj.estatus = true;
                        obj.fechaCreacion = DateTime.Now;
                        obj.fechaModificacion = DateTime.Now;
                        obj.usuarioCreacionId = vSesiones.sesionUsuarioDTO.id;
                        obj.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;

                        ctx.tblC_AF_RelacionPolizaPeru.Add(obj);
                        ctx.SaveChanges();

                        resultado.Add(SUCCESS, true);
                    }
                    catch (Exception ex)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, ex.Message);
                    }
	            } 
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarRelacionActivo(int id)
        {
            var resultado = new Dictionary<string, object>();

            using (var ctx = new MainContext(EmpresaEnum.Construplan))
            {
                try
                {
                    var registro = ctx.tblC_AF_RelacionPolizaPeru.FirstOrDefault(x => x.id == id);
                    if (registro != null)
                    {
                        registro.estatus = false;
                        registro.fechaModificacion = DateTime.Now;
                        registro.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                        ctx.SaveChanges();

                        resultado.Add(SUCCESS, true);
                    }
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        private List<CedulaColombiaDTO> construirDetallesPeru(DateTime fechaFin, List<int> cuentasActivos)
        {
            var yearActual = fechaFin.Year;
            var mesActual = fechaFin.Month;

            //
            var infoCedula = new List<CedulaColombiaDTO>();

            using (var ctx = new MainContext(EmpresaEnum.Construplan))
            {
                var cuentas = ctx.tblC_AF_CuentaPeru
                .Where(w =>
                    w.estatus &&
                    !w.peruMexico &&
                    w.tipoCuentaId == (int)AFTipoCuentaEnum.Movimiento &&
                    cuentasActivos.Contains(w.cuenta))
                .ToList();

                foreach (var cuenta in cuentas)
                {
                    var movimientosCargos = ctx.tblC_AF_RelacionPolizaPeru
                        .Where(w =>
                            w.ctaPoliza == cuenta.cuenta &&
                            w.tmPolizaId == (int)AFTipoMovimientoEnum.Cargo)
                        .ToList();

                    #region detalles
                    var ajusteDepreciacion = ctx.tblC_AF_AjusteDepreciacionPeru.Where(w => w.relacionPoliza.ctaPoliza == cuenta.cuenta && w.fechaAjuste <= fechaFin && w.estatus).ToList();

                    var detalles = new List<CedulaDetalleColombiaDTO>();

                    foreach (var mov in movimientosCargos)
                    {
                        if (mov.relacionPolizaPeruAjuste.Count > 0)
                        {
                            foreach (var infoAjuste in mov.relacionPolizaPeruAjuste.GroupBy(g => g.tipoPolizaDeAjusteId))
                            {
                                switch (infoAjuste.Key)
                                {
                                    case (int)AFTipoPolizaAjusteEnum.AjusteDeMonto:
                                        mov.montoPoliza = mov.montoPoliza + infoAjuste.Sum(s => s.montoPoliza);
                                        break;
                                }
                            }
                        }

                        if (mov.relacionPolizaPeruId_baja.HasValue && mov.relacionPolizaPeru_baja.relacionPolizaPeruAjuste.Count > 0)
                        {
                            foreach (var infoAjuste in mov.relacionPolizaPeru_baja.relacionPolizaPeruAjuste.GroupBy(g => g.tipoPolizaDeAjusteId))
                            {
                                switch (infoAjuste.Key)
                                {
                                    case (int)AFTipoPolizaAjusteEnum.AjusteDeMonto:
                                        mov.relacionPolizaPeru_baja.montoPoliza = mov.relacionPolizaPeru_baja.montoPoliza + infoAjuste.Sum(s => s.montoPoliza);
                                        break;
                                }
                            }
                        }

                        var detalle = new CedulaDetalleColombiaDTO();
                        detalle.id = mov.id;
                        detalle.esMaquina = mov.esMaquina;
                        detalle.idActivo = mov.idActivo;
                        detalle.factura = mov.factura;
                        detalle.noEconomico = mov.numEconomico;
                        detalle.descripcion = mov.concepto;
                        detalle.fechaMovimiento = mov.fechaMovimiento;
                        detalle.fechaInicioDep = mov.fechaInicioDep ?? mov.fechaMovimiento;
                        detalle.mesesDepreciacion = mov.mesesDep.Value;
                        detalle.porcentajeDepreciacion = mov.porcentajeDep.Value * 100;
                        detalle.capturaAutomatica = mov.capturaAutomatica;
                        detalle.cc = mov.cc = mov.cc;
                        detalle.ccDescripcion = mov.ccDescripcion;
                        detalle.ccCompleto = "[" + mov.cc + "] " + mov.ccDescripcion;
                        detalle.polYear = mov.yearPoliza;
                        detalle.polMes = mov.mesPoliza;
                        detalle.polPoliza = mov.polizaPoliza;
                        detalle.polTp = mov.tpPoliza;
                        detalle.polLinea = mov.lineaPoliza;
                        detalle.polizaCompleta = mov.yearPoliza + "-" + mov.mesPoliza + "-" + mov.polizaPoliza + "-" + mov.tpPoliza + "-" + mov.lineaPoliza;
                        detalle.tmPolizaId = mov.tmPolizaId;
                        detalle.cta = mov.ctaPoliza;
                        detalle.scta = mov.sctaPoliza;
                        detalle.sscta = mov.ssctaPoliza;
                        detalle.cuentaCompleta = mov.ctaPoliza + "-" + mov.sctaPoliza + "-" + mov.ssctaPoliza;
                        detalle.moi = mov.fechaMovimiento.Year < yearActual ? mov.montoPoliza : 0M;
                        detalle.alta = mov.fechaMovimiento.Year == yearActual ? mov.montoPoliza : 0M;
                        detalle.fechaPoliza = mov.fechaPoliza;
                        detalle.baja = mov.relacionPolizaPeruId_baja.HasValue ? mov.relacionPolizaPeru_baja.montoPoliza : 0M;
                        detalle.fechaBaja = mov.relacionPolizaPeruId_baja.HasValue ? (DateTime?)mov.relacionPolizaPeru_baja.fechaMovimiento : null;
                        detalle.tmPolizaId_Baja = mov.relacionPolizaPeruId_baja.HasValue ? (int?)mov.relacionPolizaPeru_baja.tmPolizaId : null;
                        detalle.depMensual = (detalle.moi + detalle.alta) / detalle.mesesDepreciacion;

                        if (detalle.fechaInicioDep <= fechaFin)
                        {
                            var diferenciaDeMeses = !detalle.fechaBaja.HasValue ? DatetimeUtils.MesesDiferencia(detalle.fechaInicioDep, fechaFin) : DatetimeUtils.MesesDiferencia(detalle.fechaInicioDep, detalle.fechaBaja.Value);
                            diferenciaDeMeses = diferenciaDeMeses > detalle.mesesDepreciacion ? detalle.mesesDepreciacion : diferenciaDeMeses;
                            var fechaFinDep = detalle.fechaInicioDep.AddMonths(diferenciaDeMeses);

                            //De momento la depreciación por días del primer mes solo aplica a colombia
                            //detalle.diasDepPrimerMes = 30 - (detalle.fechaInicioDep.Day <= 30 ? detalle.fechaInicioDep.Day : 30);

                            if (fechaFinDep.Year == yearActual)
                            {
                                if (detalle.fechaInicioDep.Year == yearActual)
                                {
                                    detalle.mesesDepreciadosActualmente = fechaFinDep.Month - 1;
                                }
                                else
                                {
                                    detalle.mesesDepreciadosActualmente = fechaFin.Month;
                                    detalle.mesesDepreciadosAnteriormente = diferenciaDeMeses - fechaFin.Month;
                                }
                            }
                            else
                            {
                                detalle.mesesDepreciadosAnteriormente = diferenciaDeMeses;
                            }

                            #region ajustesDepreciación
                            var ajustesDepActivo = ajusteDepreciacion.Where(x => x.relacionPolizaID == mov.id).ToList();

                            foreach (var ajusteDep in ajustesDepActivo)
                            {
                                if (fechaFinDep >= ajusteDep.fechaAjuste)
                                {
                                    switch (ajusteDep.tipoAjusteDepID)
                                    {
                                        case (int)TipoAjusteDepColombiaEnum.MONTO:
                                            if (fechaFinDep.Year == ajusteDep.fechaAjuste.Year)
                                            {
                                                detalle.depreciacionActual += ajusteDep.montoAjuste;
                                            }
                                            else
                                            {
                                                detalle.depreciacionAnterior += ajusteDep.montoAjuste;
                                            }
                                            break;
                                        case (int)TipoAjusteDepColombiaEnum.MESES:
                                            if (fechaFinDep.Year == ajusteDep.fechaAjuste.Year)
                                            {
                                                detalle.mesesDepreciadosActualmente += (int)ajusteDep.montoAjuste;
                                            }
                                            else
                                            {
                                                detalle.mesesDepreciadosAnteriormente += (int)ajusteDep.montoAjuste;
                                            }
                                            break;
                                        case (int)TipoAjusteDepColombiaEnum.DIAS:
                                            detalle.diasDepPrimerMes += (int)ajusteDep.montoAjuste;
                                            break;
                                    }
                                }
                            }
                            #endregion

                            #region Depreciación por días del primer mes
                            //De momento la depreciación por días del primer mes solo aplica a colombia
                            //if (detalle.diasDepPrimerMes > 0)
                            //{
                            //    if (detalle.fechaInicioDep.Year == fechaFinDep.Year)
                            //    {
                            //        detalle.depreciacionActual += ((detalle.depMensual / 30) * detalle.diasDepPrimerMes);
                            //    }
                            //    else
                            //    {
                            //        detalle.depreciacionAnterior += ((detalle.depMensual / 30) * detalle.diasDepPrimerMes);
                            //    }
                            //}
                            #endregion
                        }

                        #region Depreciación del ultimo mes la cual es por días
                        //De momento la depreciación por días del primer mes solo aplica a colombia
                        //if (detalle.mesesDepreciacion == detalle.mesesDepreciadosAnteriormente + detalle.mesesDepreciadosActualmente)
                        //{
                        //    if (detalle.mesesDepreciadosActualmente > 0)
                        //    {
                        //        detalle.depreciacionActual -= detalle.depMensual;
                        //        detalle.depreciacionActual += ((detalle.depMensual / 30) * (30 - detalle.diasDepPrimerMes));
                        //    }
                        //    else
                        //    {
                        //        detalle.depreciacionAnterior -= detalle.depMensual;
                        //        detalle.depreciacionAnterior += ((detalle.depMensual / 30) * (30 - detalle.diasDepPrimerMes));
                        //    }
                        //}
                        #endregion

                        detalle.depreciacionAnterior += detalle.mesesDepreciadosAnteriormente * detalle.depMensual;
                        detalle.depreciacionActual += detalle.mesesDepreciadosActualmente * detalle.depMensual;
                        detalle.bajaDepreciacion += detalle.fechaBaja.HasValue ? detalle.depreciacionAnterior + detalle.depreciacionActual : 0M;
                        detalle.depreciacionAcumulada += detalle.depreciacionAnterior + detalle.depreciacionActual - detalle.bajaDepreciacion;
                        detalle.saldoLibro += detalle.fechaBaja.HasValue ? 0M : detalle.moi + detalle.alta - detalle.depreciacionAcumulada;
                        detalle.depTerminadaPorTiempo = detalle.mesesDepreciadosAnteriormente + detalle.mesesDepreciadosActualmente == detalle.mesesDepreciacion;

                        detalles.Add(detalle);
                    }
                    #endregion

                    #region saldo
                    var saldo = new CedulaSaldoColombiaDTO();

                    var saldosMovimientosMN = getInfoCuentaMovimientoPeru(yearActual, mesActual, cuenta.cuentaMexico.cuenta, false, true);
                    var saldosContabilidadMN = getInfoCuentaSaldoPeru(yearActual, cuenta.cuentaMexico.cuenta, false, true);

                    var saldosContabilidadPeru = getInfoCuentaSaldoPeru(yearActual, cuenta.cuenta, false, false);

                    saldo.esMaquina = cuenta.subcuentas.Any(a => a.esMaquinaria);
                    saldo.cuenta = cuenta.cuenta;
                    saldo.concepto = cuenta.descripcion;
                    saldo.saldoAnteriorMN = saldosMovimientosMN.Where(w => w.Year < yearActual).Sum(s => s.Monto);
                    saldo.saldoAnteriorCOP = detalles.Sum(s => s.moi);
                    saldo.saldoAnteriorCOP += detalles.Where(w => w.fechaBaja.HasValue && w.fechaBaja.Value.Year < yearActual).Sum(s => s.baja);
                    saldo.altaActualMN = saldosMovimientosMN.Where(w => w.Year == yearActual && (w.TM == (int)AFTipoMovimientoEnum.Cargo || w.TM == (int)AFTipoMovimientoEnum.CargoRojo)).Sum(s => s.Monto);
                    saldo.altaActualCOP = detalles.Sum(s => s.alta);
                    saldo.altaActualCOP += detalles.Where(w => w.fechaBaja.HasValue && w.fechaBaja.Value.Year == yearActual && w.tmPolizaId_Baja == (int)AFTipoMovimientoEnum.CargoRojo).Sum(s => s.baja);
                    saldo.bajaActualMN = saldosMovimientosMN.Where(w => w.Year == yearActual && w.TM == (int)AFTipoMovimientoEnum.Abono).Sum(s => s.Monto);
                    saldo.bajaActualCOP = detalles.Where(w => w.fechaBaja.HasValue && w.fechaBaja.Value.Year == yearActual && w.tmPolizaId_Baja == (int)AFTipoMovimientoEnum.Abono).Sum(s => s.baja);
                    saldo.saldoActualMN = saldo.saldoAnteriorMN + saldo.altaActualMN + saldo.bajaActualMN;
                    saldo.saldoActualCOP = saldo.saldoAnteriorCOP + saldo.altaActualCOP + saldo.bajaActualCOP;

                    saldo.contabilidadCOP = saldosContabilidadPeru
                        .Where(w =>
                            w.Year < yearActual
                        ).Sum(s =>
                            s.Enecargos + s.Eneabonos + s.Febcargos + s.Febabonos + s.Marcargos + s.Marabonos + s.Abrcargos + s.Abrabonos +
                            s.Maycargos + s.Mayabonos + s.Juncargos + s.Junabonos + s.Julcargos + s.Julabonos + s.Agocargos + s.Agoabonos +
                            s.Sepcargos + s.Sepabonos + s.Octcargos + s.Octabonos + s.Novcargos + s.Novabonos + s.Diccargos + s.Dicabonos
                        );

                    for (int mes = 1; mes <= mesActual; mes++)
                    {
                        var mesCargo = Enum.GetName(typeof(MesesCargoTablaContabilidad), mes);
                        var mesAbono = Enum.GetName(typeof(MesesAbonosTablaContabilidad), mes);

                        saldo.contabilidadCOP += saldosContabilidadPeru
                            .Where(w =>
                                w.Year == yearActual
                            ).Sum(s => Convert.ToDecimal(s.GetType().GetProperty(mesCargo).GetValue(s, null)) + Convert.ToDecimal(s.GetType().GetProperty(mesAbono).GetValue(s, null)));
                    }

                    saldo.contabilidadMN = saldosContabilidadMN
                        .Where(w =>
                            w.Year < yearActual
                        ).Sum(s =>
                            s.Enecargos + s.Eneabonos + s.Febcargos + s.Febabonos + s.Marcargos + s.Marabonos + s.Abrcargos + s.Abrabonos +
                            s.Maycargos + s.Mayabonos + s.Juncargos + s.Junabonos + s.Julcargos + s.Julabonos + s.Agocargos + s.Agoabonos +
                            s.Sepcargos + s.Sepabonos + s.Octcargos + s.Octabonos + s.Novcargos + s.Novabonos + s.Diccargos + s.Dicabonos
                        );

                    for (int mes = 1; mes <= mesActual; mes++)
                    {
                        var mesCargo = Enum.GetName(typeof(MesesCargoTablaContabilidad), mes);
                        var mesAbono = Enum.GetName(typeof(MesesAbonosTablaContabilidad), mes);

                        saldo.contabilidadMN += saldosContabilidadMN
                            .Where(w =>
                                w.Year == yearActual
                            ).Sum(s => Convert.ToDecimal(s.GetType().GetProperty(mesCargo).GetValue(s, null)) + Convert.ToDecimal(s.GetType().GetProperty(mesAbono).GetValue(s, null)));
                    }

                    saldo.diferenciaMN = saldo.saldoActualMN - saldo.contabilidadMN;
                    saldo.diferenciaCOP = saldo.saldoActualCOP - saldo.contabilidadCOP;
                    #endregion

                    #region cedula depreciacion
                    var dep = new CedulaDepColombiaDTO();

                    var depMovimientosMN = getInfoCuentaMovimientoPeru(yearActual, mesActual, cuenta.cuentaMexico.cuenta, true, true);
                    var depContabilidadMN = getInfoCuentaSaldoPeru(yearActual, cuenta.cuentaMexico.cuenta, true, true);

                    var depContabilidadPeru = getInfoCuentaSaldoPeru(yearActual, cuenta.cuenta, true, false);

                    dep.esMaquina = cuenta.subcuentas.Any(a => a.esMaquinaria);
                    dep.cuenta = cuenta.cuenta;
                    dep.concepto = cuenta.descripcion;
                    dep.depActualMN = Math.Abs(depMovimientosMN.Where(w => w.Year == yearActual).Sum(s => s.Monto));
                    dep.depActualCOP = detalles.Sum(s => s.depreciacionActual + s.bajaDepreciacion);
                    dep.depAcumuladaMN = Math.Abs(depMovimientosMN.Sum(s => s.Monto));
                    dep.depAcumuladaCOP = detalles.Sum(s => s.depreciacionAcumulada);

                    dep.depContabilidadCOP = depContabilidadPeru
                        .Where(w =>
                            w.Year < yearActual
                        ).Sum(s =>
                            s.Enecargos + s.Eneabonos + s.Febcargos + s.Febabonos + s.Marcargos + s.Marabonos + s.Abrcargos + s.Abrabonos +
                            s.Maycargos + s.Mayabonos + s.Juncargos + s.Junabonos + s.Julcargos + s.Julabonos + s.Agocargos + s.Agoabonos +
                            s.Sepcargos + s.Sepabonos + s.Octcargos + s.Octabonos + s.Novcargos + s.Novabonos + s.Diccargos + s.Dicabonos
                        );

                    for (int mes = 1; mes <= mesActual; mes++)
                    {
                        var mesCargo = Enum.GetName(typeof(MesesCargoTablaContabilidad), mes);
                        var mesAbono = Enum.GetName(typeof(MesesAbonosTablaContabilidad), mes);

                        dep.depContabilidadCOP += depContabilidadPeru
                            .Where(w =>
                                w.Year == yearActual
                            ).Sum(s => Convert.ToDecimal(s.GetType().GetProperty(mesCargo).GetValue(s, null)) + Convert.ToDecimal(s.GetType().GetProperty(mesAbono).GetValue(s, null)));
                    }

                    dep.depContabilidadCOP = Math.Abs(dep.depContabilidadCOP);

                    dep.depContabilidadMN = depContabilidadMN
                        .Where(w =>
                            w.Year < yearActual
                        ).Sum(s =>
                            s.Enecargos + s.Eneabonos + s.Febcargos + s.Febabonos + s.Marcargos + s.Marabonos + s.Abrcargos + s.Abrabonos +
                            s.Maycargos + s.Mayabonos + s.Juncargos + s.Junabonos + s.Julcargos + s.Julabonos + s.Agocargos + s.Agoabonos +
                            s.Sepcargos + s.Sepabonos + s.Octcargos + s.Octabonos + s.Novcargos + s.Novabonos + s.Diccargos + s.Dicabonos
                        );

                    for (int mes = 1; mes <= mesActual; mes++)
                    {
                        var mesCargo = Enum.GetName(typeof(MesesCargoTablaContabilidad), mes);
                        var mesAbono = Enum.GetName(typeof(MesesAbonosTablaContabilidad), mes);

                        dep.depContabilidadMN += depContabilidadMN
                            .Where(w =>
                                w.Year == yearActual
                            ).Sum(s => Convert.ToDecimal(s.GetType().GetProperty(mesCargo).GetValue(s, null)) + Convert.ToDecimal(s.GetType().GetProperty(mesAbono).GetValue(s, null)));
                    }

                    dep.depContabilidadMN = Math.Abs(dep.depContabilidadMN);

                    dep.diferenciaMN = dep.depAcumuladaMN - Math.Abs(dep.depContabilidadMN);
                    dep.diferenciaCOP = dep.depAcumuladaCOP - Math.Abs(dep.depContabilidadCOP);
                    #endregion

                    var cedulaCuenta = new CedulaColombiaDTO();
                    cedulaCuenta.saldo = saldo;
                    cedulaCuenta.dep = dep;
                    cedulaCuenta.detalle = detalles;

                    infoCedula.Add(cedulaCuenta);
                }
            }

            return infoCedula;
        }

        private List<sc_movpolDTO> getInfoCuentaMovimientoPeru(int yearFin, int mesFin, int cuenta, bool esDepreciacion, bool peruMexico)
        {
            List<tblC_AF_RelacionCuentaYearPeru> relacionCuenta = null;

            using (var ctx = new MainContext(EmpresaEnum.Construplan))
            {
                if (!esDepreciacion)
                {
                    relacionCuenta = ctx.tblC_AF_RelacionCuentaYearPeru
                        .Where(w =>
                            w.estatus &&
                            w.year <= yearFin &&
                            w.cuentaMovimientoId == null &&
                            w.subcuentaPeru.estatus &&
                            w.subcuentaPeru.cuentaPeru.estatus &&
                            w.subcuentaPeru.cuentaPeru.cuenta == cuenta &&
                            w.subcuentaPeru.cuentaPeru.peruMexico == peruMexico
                        ).ToList();
                }
                else
                {
                    relacionCuenta = ctx.tblC_AF_RelacionCuentaYearPeru
                        .Where(w =>
                            w.estatus &&
                            w.year <= yearFin &&
                            w.cuentaMovimientoId != null &&
                            w.subcuentaPeru.estatus &&
                            w.cuentaPeru.estatus &&
                            w.cuentaPeru.cuenta == cuenta &&
                            w.subcuentaPeru.cuentaPeru.peruMexico == peruMexico
                        ).ToList();
                }

                if (relacionCuenta.Count == 0)
                {
                    return new List<sc_movpolDTO>();
                }

                var where_sc_movpol_subcuenta = "";

                foreach (var relCta in relacionCuenta)
                {
                    where_sc_movpol_subcuenta += "(MOV.scta = ? AND MOV.sscta = ?)";

                    if (relCta != relacionCuenta.Last())
                    {
                        where_sc_movpol_subcuenta += " OR ";
                    }
                }

                var query_sc_movpol = new OdbcConsultaDTO();

                query_sc_movpol.consulta = string.Format
                    (
                        @"SELECT
                        MOV.year,
                        MOV.mes,
                        MOV.poliza,
                        MOV.tp,
                        MOV.linea,
                        MOV.cta,
                        MOV.scta,
                        MOV.sscta,
                        MOV.tm,
                        MOV.referencia,
                        MOV.cc,
                        MOV.concepto,
                        MOV.monto,
                        POL.fechapol
                    FROM
                        DBA.sc_movpol AS MOV
                    INNER JOIN
                        DBA.sc_polizas AS POL
                        ON
                            POL.year = MOV.year AND
                            POL.mes = MOV.mes AND
                            POL.tp = MOV.tp AND
                            POL.poliza = MOV.poliza
                    WHERE
                        MOV.cta = ? AND
                        (
                            {0}
                        ) AND
                        (
                            (MOV.year < ?) OR
                            (MOV.year = ? AND MOV.mes <= ?)
                        ) AND MOV.tm IN ({1}, {2})",
                        where_sc_movpol_subcuenta, (esDepreciacion ? 2 : 1), (esDepreciacion ? 4 : 3)
                    );

                query_sc_movpol.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "cta",
                    tipo = OdbcType.Int,
                    valor = relacionCuenta.First().subcuentaPeru.cuentaPeru.cuenta
                });
                foreach (var relCuenta in relacionCuenta)
                {
                    query_sc_movpol.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "scta",
                        tipo = OdbcType.Int,
                        valor = relCuenta.subcuentaPeru.scta
                    });
                    query_sc_movpol.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "sscta",
                        tipo = OdbcType.Int,
                        valor = relCuenta.subcuentaPeru.sscta
                    });
                }
                query_sc_movpol.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "year",
                    tipo = OdbcType.Int,
                    valor = yearFin
                });
                query_sc_movpol.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "year",
                    tipo = OdbcType.Int,
                    valor = yearFin
                });
                query_sc_movpol.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "mes",
                    tipo = OdbcType.Int,
                    valor = mesFin
                });

                var sc_movpol = _contextEnkontrol.Select<sc_movpolDTO>(vSesiones.sesionAmbienteEnkontrolAdm, query_sc_movpol);

                return sc_movpol;
            }
        }

        private List<ActivoFijoSaldosContablesDTO> getInfoCuentaSaldoPeru(int yearFin, int cuenta, bool esDepreciacion, bool peruMexico)
        {
            List<tblC_AF_RelacionCuentaYearPeru> relacionCuenta = null;

            using (var ctx = new MainContext(EmpresaEnum.Construplan))
            {
                if (!esDepreciacion)
                {
                    relacionCuenta = ctx.tblC_AF_RelacionCuentaYearPeru
                        .Where(w =>
                            w.estatus &&
                            w.year <= yearFin &&
                            w.cuentaMovimientoId == null &&
                            w.subcuentaPeru.estatus &&
                            w.subcuentaPeru.cuentaPeru.estatus &&
                            w.subcuentaPeru.cuentaPeru.cuenta == cuenta &&
                            w.subcuentaPeru.cuentaPeru.peruMexico == peruMexico
                    ).ToList();
                }
                else
                {
                    relacionCuenta = ctx.tblC_AF_RelacionCuentaYearPeru
                        .Where(w =>
                            w.estatus &&
                            w.year <= yearFin &&
                            w.cuentaMovimientoId != null &&
                            w.cuentaPeru.estatus &&
                            w.cuentaPeru.cuenta == cuenta &&
                            w.subcuentaPeru.estatus &&
                            w.subcuentaPeru.cuentaPeru.peruMexico == peruMexico
                    ).ToList();
                }

                if (peruMexico)
                {
                    var where_sc_salcont_cc_subcuenta = "";
                    var years = "";

                    foreach (var item in relacionCuenta.GroupBy(x => x.year).OrderBy(x => x.Key))
                    {
                        years += item.Key;

                        if (item.Key != relacionCuenta.GroupBy(x => x.year).OrderBy(x => x.Key).Last().Key)
                        {
                            years += ", ";
                        }
                    }

                    foreach (var relCta in relacionCuenta)
                    {
                        where_sc_salcont_cc_subcuenta += "(scta = ? AND sscta = ?)";

                        if (relCta != relacionCuenta.Last())
                        {
                            where_sc_salcont_cc_subcuenta += " OR ";
                        }
                    }

                    var query_sc_salcont_cc = new OdbcConsultaDTO();

                    query_sc_salcont_cc.consulta = string.Format
                        (
                            @"SELECT
                                *
                            FROM
                                DBA.sc_salcont_cc
                            WHERE
                                year in ({1}) AND
                                cta = ? AND
                                (
                                    {0}
                                )",
                                          where_sc_salcont_cc_subcuenta, years
                                );

                    query_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "year",
                        tipo = OdbcType.Int,
                        valor = yearFin
                    });
                    query_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "cta",
                        tipo = OdbcType.Int,
                        valor = relacionCuenta.First().subcuentaPeru.cuentaPeru.cuenta
                    });

                    foreach (var relCuenta in relacionCuenta)
                    {
                        query_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                        {
                            nombre = "scta",
                            tipo = OdbcType.Int,
                            valor = relCuenta.subcuentaPeru.scta
                        });
                        query_sc_salcont_cc.parametros.Add(new OdbcParameterDTO
                        {
                            nombre = "sscta",
                            tipo = OdbcType.Int,
                            valor = relCuenta.subcuentaPeru.sscta
                        });
                    }

                    var sc_salcont_cc = _contextEnkontrol.Select<ActivoFijoSaldosContablesDTO>(vSesiones.sesionAmbienteEnkontrolAdm, query_sc_salcont_cc);

                    return sc_salcont_cc;
                }
                else
                {
                    var where_sc_salcont_cc_subcuenta = "";
                    var yearInit = relacionCuenta.OrderBy(x => x.year).First();

                    foreach (var relCta in relacionCuenta)
                    {
                        where_sc_salcont_cc_subcuenta += "(PLANCTA_CODIGO = " + relCta.subcuentaPeru.cuentaPeru.cuenta + ")";

                        if (relCta != relacionCuenta.Last())
                        {
                            where_sc_salcont_cc_subcuenta += " OR ";
                        }
                    }

                    var datos = new List<ActivoFijoSaldosContablesDTO>();

                    for (int i = yearInit.year; i <= yearFin; i++)
                    {
                        var sql = string.Format(@"
                            SELECT
                                {0} AS Year,
                                PLANCTA_CODIGO AS Cta,
                                0 AS Scta,
                                0 AS Sscta,
                                NULL AS Cc,
                                0 AS SalIni,
                                SAL_DEB01 AS Enecargos,
                                SAL_HAB01 AS Eneabonos,
                                SAL_DEB02 AS Febcargos,
                                SAL_HAB02 AS Febabonos,
                                SAL_DEB03 AS Marcargos,
                                SAL_HAB03 AS Marabonos,
                                SAL_DEB04 AS Abrcargos,
                                SAL_HAB04 AS Abrabonos,
                                SAL_DEB05 AS Maycargos,
                                SAL_HAB05 AS Mayabonos,
                                SAL_DEB06 AS Juncargos,
                                SAL_HAB06 AS Junabonos,
                                SAL_DEB07 AS Julcargos,
                                SAL_HAB07 AS Julabonos,
                                SAL_DEB08 AS Agocargos,
                                SAL_HAB08 AS Agoabonos,
                                SAL_DEB09 AS Sepcargos,
                                SAL_HAB09 AS Sepabonos,
                                SAL_DEB10 AS Octcargos,
                                SAL_HAB10 AS Octabonos,
                                SAL_DEB11 AS Novcargos,
                                SAL_HAB11 AS Novabonos,
                                SAL_DEB12 AS Diccargos,
                                SAL_HAB12 AS Dicabonos
                            FROM
                                [003BDCONT{0}]..SALDOS
                            WHERE
                                {1}
                            ", i, where_sc_salcont_cc_subcuenta);

                        using (var conexion = new SqlConnection(ConextSigoDapper.conexionStarsoftBancos()))
                        {
                            DynamicParameters lstParametros = new DynamicParameters();

                            conexion.Open();
                            datos.AddRange(conexion.Query<ActivoFijoSaldosContablesDTO>(sql, lstParametros, null, true, 300, CommandType.Text).ToList());
                            conexion.Close();
                        }
                    }

                    return datos;
                }
            }
        }
        #endregion
    }
}