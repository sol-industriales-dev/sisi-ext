using Core.DAO.Maquinaria.Captura;
using Core.DTO;
using Core.DTO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Captura.DatosDiarios;
using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Captura
{
    public class DatosDiariosDAO : GenericDAO<tblM_CapHorometro>, IDatosDiariosDAO
    {
        public List<ComboDTO> ObtenerAreaCuenta()
        {
            List<ComboDTO> lstCombo = new List<ComboDTO>();
            try
            {
                var virtuales = new List<string>();
                virtuales.Add("1010");
                virtuales.Add("1015");
                virtuales.Add("1018");
                lstCombo = _context.tblP_CC.Where(x => x.estatus == true && !virtuales.Contains(x.areaCuenta)).OrderBy(x=>x.area).ThenBy(x=>x.cuenta).Select(y => new ComboDTO
                {
                    Value = y.areaCuenta,
                    Text = y.areaCuenta + " - " + y.descripcion
                }).ToList();
            }
            catch (Exception e)
            {
                LogError(2, 0, "CapturaDatosController", "ObtenerAreaCuenta", e, AccionEnum.CONSULTA, 0, lstCombo);
            }
            return lstCombo;
        }
        public List<resultDatosDiariosDTO> ObtenerCatMaquinas(datosDiariosDTO parametros, int idEmpresa)
        {
            List<resultDatosDiariosDTO> lstResultDatos = new List<resultDatosDiariosDTO>();
            List<resultDatosDiariosDTO> lstDatos = new List<resultDatosDiariosDTO>();
            int status = 0;
            if (parametros.status != null)
            {
                status = Convert.ToInt32(parametros.status);
            }
            try
            {


                List<tblM_ControlEnvioMaquinaria> lstEnvios = _context.tblM_ControlEnvioMaquinaria.Where(r => r.tipoControl == 4).ToList();
                var lstResultD = _context.tblM_CatMaquina.Where(x => x.estatus != 0).ToList();
                lstResultDatos = _context.tblM_CapturaDatosDiariosMaquinaria.Where(x => x.fechaCapturaMaquinaria == parametros.fecha
                                                                                            ).ToList().Select(y => new resultDatosDiariosDTO
                                                                                            {
                                                                                                id = y.id,
                                                                                                idCatMaquina = y.idCatMaquina,
                                                                                                Economico = lstResultD.Where(x => x.id == y.idCatMaquina).FirstOrDefault().id,
                                                                                                ModeloEQUIPo = lstResultD.Where(x => x.id == y.idCatMaquina).FirstOrDefault() == null ? 0 : lstResultD.Where(x => x.id == y.idCatMaquina).FirstOrDefault().modeloEquipoID,
                                                                                                economicoDescripcion = lstResultD.Where(x => x.id == y.idCatMaquina).FirstOrDefault() == null ? "NA" : lstResultD.Where(x => x.id == y.idCatMaquina).FirstOrDefault().noEconomico,
                                                                                                descripcion = lstResultD.Where(x => x.id == y.idCatMaquina).FirstOrDefault() == null ? "NA" : lstResultD.Where(x => x.id == y.idCatMaquina).FirstOrDefault().descripcion,
                                                                                                Marca = lstResultD.Where(x => x.id == y.idCatMaquina).FirstOrDefault().marca == null ? "NA" : lstResultD.Where(x => x.id == y.idCatMaquina).FirstOrDefault().marca.descripcion,
                                                                                                Modelo = lstResultD.Where(x => x.id == y.idCatMaquina).FirstOrDefault().modeloEquipo == null ? "NA" : lstResultD.Where(x => x.id == y.idCatMaquina).FirstOrDefault().modeloEquipo.descripcion,
                                                                                                Serie = lstResultD.Where(x => x.id == y.idCatMaquina).FirstOrDefault() == null ? "NA" : lstResultD.Where(x => x.id == y.idCatMaquina).FirstOrDefault().noSerie,
                                                                                                Horometro = 0,
                                                                                                centro_costos = lstResultD.Where(x => x.id == y.idCatMaquina).FirstOrDefault() == null ? "" : lstResultD.Where(x => x.id == y.idCatMaquina).FirstOrDefault().centro_costos,
                                                                                                FechaPatioMaquinaria = y.FechaPatioMaquinaria,
                                                                                                FechaTMC = y.FechaTMC,
                                                                                                FechaMaquinaria = y.FechaMaquinaria,
                                                                                                Status = y.idEstatus,
                                                                                                opcionStatus = y.idEstatus == 1 ? "Overhaul" : y.idEstatus == 2 ? "Equipo en espera de rehabilitación" : y.idEstatus == 3 ? "Equipo en rehabilitación en TMC" : y.idEstatus == 4 ? "Equipo disponible para obra" : y.idEstatus == 5 ? "Equipo disponible para venta" : "",
                                                                                                tipoEquipoID = lstResultD.Where(x => x.id == y.idCatMaquina).FirstOrDefault() == null ? 0 : lstResultD.Where(x => x.id == y.idCatMaquina).FirstOrDefault().grupoMaquinaria.tipoEquipoID
                                                                                            }).ToList();
                if (lstResultDatos.Count == 0)
                {
                    if (parametros.fecha.ToShortDateString() == DateTime.Now.ToShortDateString())
                    {
                        lstResultDatos = new List<resultDatosDiariosDTO>();
                        var lstcc = new string[] { "1010", "1015", "1018" };
                        lstResultDatos = _context.tblM_CatMaquina.Where(x => x.estatus != 0
                                                                          && lstcc.Contains(x.centro_costos)
                                                                    ).ToList().Select(y => new resultDatosDiariosDTO
                                                                    {
                                                                        id = y.id,
                                                                        idCatMaquina = y.id,
                                                                        Economico = y.id,
                                                                        economicoDescripcion = y.noEconomico,
                                                                        descripcion = y.descripcion,
                                                                        ModeloEQUIPo = y.modeloEquipo.id,
                                                                        Modelo = y.modeloEquipo == null ? "" : y.modeloEquipo.descripcion,
                                                                        Marca = y.marca == null ? "" : y.marca.descripcion,
                                                                        centro_costos = y.centro_costos,
                                                                        Serie = y.noSerie,
                                                                        Horometro = _context.tblM_CapHorometro.Where(x => x.Economico == y.noEconomico).FirstOrDefault() != null ? _context.tblM_CapHorometro.Where(x => x.Economico == y.noEconomico).OrderByDescending(n => n.Fecha).FirstOrDefault().Horometro : 0,
                                                                        FechaPatioMaquinaria = obtenerFecha(lstEnvios, y.centro_costos, y.id, 1),
                                                                        FechaTMC = obtenerFecha(lstEnvios, y.centro_costos, y.id, 2),
                                                                        FechaMaquinaria = obtenerFecha(lstEnvios, y.centro_costos, y.id, 3),
                                                                        Status = ChecarMovimientoInterno(lstEnvios, y.centro_costos, y.id, 3) == 5 ? 5 : ChecarMovimientoInterno(lstEnvios, y.centro_costos, y.id, 3) == 4 ? 4 : y.centro_costos == "1018" ? 1 : y.centro_costos == "1015" ? 2 : y.centro_costos == "1010" ? 3 : 0,
                                                                        opcionStatus = ChecarMovimientoInterno(lstEnvios, y.centro_costos, y.id, 3) == 5 ? "Equipo disponible para venta" : ChecarMovimientoInterno(lstEnvios, y.centro_costos, y.id, 3) == 4 ? "Equipo disponible para obra" : y.centro_costos == "1018" ? "Overhaul" : y.centro_costos == "1015" ? "Equipo en espera de rehabilitación" : y.centro_costos == "1010" ? "Equipo en rehabilitación en TMC" : "",
                                                                        tipoEquipoID = y.grupoMaquinaria.tipoEquipoID
                                                                    }).ToList();
                    }

                }
                else
                {
                    lstResultDatos = lstResultDatos.Select(y => new resultDatosDiariosDTO
                    {
                        id = y.id,
                        idCatMaquina = y.idCatMaquina,
                        Economico = y.Economico,
                        ModeloEQUIPo = y.ModeloEQUIPo,
                        economicoDescripcion = y.economicoDescripcion,
                        descripcion = y.descripcion,
                        Marca = y.Marca,
                        Modelo = y.Modelo,
                        Serie = y.Serie,
                        Horometro = Horometros(y.economicoDescripcion),
                        centro_costos = y.centro_costos,
                        FechaPatioMaquinaria = y.FechaPatioMaquinaria,
                        FechaTMC = y.FechaTMC,
                        FechaMaquinaria = y.FechaMaquinaria,
                        Status = y.Status,
                        opcionStatus = y.opcionStatus,
                        tipoEquipoID = y.tipoEquipoID,
                    }).ToList();
                }



                lstDatos = lstResultDatos.Where(
                                r => (r.Status == (parametros.Estado == 0 ? r.Status : parametros.Estado))
                                && (r.ModeloEQUIPo == (parametros.ModeloEquipo == 0 ? r.ModeloEQUIPo : parametros.ModeloEquipo))
                                && (parametros.Economico != "" ? r.economicoDescripcion.Contains(parametros.Economico) : r.economicoDescripcion == r.economicoDescripcion)
                    ).ToList();



            }
            catch (Exception e)
            {
                LogError(2, 0, "CapturaDatosController", "ObtenerCatMaquinas", e, AccionEnum.CONSULTA, 0, lstResultDatos);
            }
            return lstDatos;
        }
        public decimal Horometros(string economico)
        {
            decimal horo = 0;
            if (_context.tblM_CapHorometro.Where(s => s.Economico == economico).FirstOrDefault() != null)
            {
                horo = _context.tblM_CapHorometro.Where(s => s.Economico == economico).OrderByDescending(n => n.Fecha).FirstOrDefault().Horometro;
            }
            else
            {
                horo = 0;
            }
            return horo;
        }

        public DateTime? obtenerFecha(List<tblM_ControlEnvioMaquinaria> lstEnvios, string lugar, int noEconomico, int tipoFecha)
        {
            DateTime? fechaRetorno;
            DateTime? f = new DateTime();
            try
            {
                fechaRetorno = f;
                if (lugar == "1010")
                {
                    switch (tipoFecha)
                    {
                        case 1:
                            if (lstEnvios.Where(r => r.lugar == "1015" && r.noEconomico == noEconomico).FirstOrDefault() != null)
                            {
                                fechaRetorno = lstEnvios.Where(r => r.lugar == "1015" && r.noEconomico == noEconomico).OrderByDescending(x => x.fechaRecepcionEmbarque).FirstOrDefault().fechaRecepcionEmbarque;
                            }
                            break;
                        case 2:
                            if (lstEnvios.Where(r => r.lugar == lugar && r.noEconomico == noEconomico).FirstOrDefault() != null)
                            {
                                fechaRetorno = lstEnvios.Where(r => r.lugar == lugar && r.noEconomico == noEconomico).OrderByDescending(x => x.fechaRecepcionEmbarque).FirstOrDefault().fechaRecepcionEmbarque;
                            }
                            break;
                        case 3:
                            if (_context.tblM_ControInterno.Where(r => r.Destino == "1015" && r.EconomicoID == noEconomico).FirstOrDefault() != null)
                            {
                                fechaRetorno = _context.tblM_ControInterno.Where(r => r.Destino == "1015" && r.EconomicoID == noEconomico).OrderByDescending(l => l.FechaCaptura).FirstOrDefault().FechaCaptura;
                            }
                            break;
                    }
                }
                else
                {
                    switch (tipoFecha)
                    {
                        case 1:
                            if (lstEnvios.Where(r => r.lugar == "1010" && r.noEconomico == noEconomico).FirstOrDefault() != null)
                            {
                                fechaRetorno = lstEnvios.Where(r => r.lugar == "1010" && r.noEconomico == noEconomico).OrderByDescending(x => x.fechaRecepcionEmbarque).FirstOrDefault().fechaRecepcionEmbarque;
                            }
                            break;
                        case 2:
                            if (_context.tblM_ControInterno.Where(r => r.Destino == "1010" && r.EconomicoID == noEconomico).FirstOrDefault() != null)
                            {
                                fechaRetorno = _context.tblM_ControInterno.Where(r => r.Destino == "1010" && r.EconomicoID == noEconomico).OrderByDescending(l => l.FechaCaptura).FirstOrDefault().FechaCaptura;
                            }
                            break;
                        case 3:
                            if (_context.tblM_ControInterno.Where(r => r.Destino == "1015" && r.EconomicoID == noEconomico).FirstOrDefault() != null)
                            {
                                fechaRetorno = _context.tblM_ControInterno.Where(r => r.Destino == "1015" && r.EconomicoID == noEconomico).OrderByDescending(l => l.FechaCaptura).FirstOrDefault().FechaCaptura;
                            }
                            break;
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }

            return fechaRetorno;
        }

        public int ChecarMovimientoInterno(List<tblM_ControlEnvioMaquinaria> lstEnvios, string lugar, int noEconomico, int tipoFecha)
        {
            int variable = 0;
            var obj = _context.tblM_ControInterno.Where(r => r.Destino == "1015" && r.EconomicoID == noEconomico && r.Comentario.Contains("VENTA")).FirstOrDefault();
            if (obj != null)
            {
                var d = _context.tblM_ControInterno.Where(r => r.Destino == "1015" && r.EconomicoID == noEconomico && r.Comentario.Contains("VENTA")).OrderByDescending(l => l.FechaCaptura).FirstOrDefault().FechaCaptura;
                if (d != null)
                {
                    variable = 5;
                }
                else
                {
                    variable = 4;
                }
            }
            else
            {
                var obj3 = _context.tblM_ControInterno.Where(r => r.Destino == "1015" && r.EconomicoID == noEconomico && r.Comentario.Contains("OBRA")).FirstOrDefault();
                if (obj3 != null)
                {
                    variable = 4;
                }
                else
                {
                    variable = 0;
                }
            }
            return variable;
        }

        public List<tblM_CapturaDatosDiariosMaquinaria> ObtenerCapturaDeDatosDiario(datosDiariosDTO parametros, int idEmpresa)
        {
            List<tblM_CapturaDatosDiariosMaquinaria> lstResultDatos = new List<tblM_CapturaDatosDiariosMaquinaria>();
            try
            {
                lstResultDatos = _context.tblM_CapturaDatosDiariosMaquinaria.Where(x => x.fechaCapturaMaquinaria == parametros.fecha).ToList();
            }
            catch (Exception e)
            {
                LogError(2, 0, "CapturaDatosController", "ObtenerCatMaquinas", e, AccionEnum.CONSULTA, 0, lstResultDatos);
            }
            return lstResultDatos;
        }

        public bool CapturarDatosDiaros(List<tblM_CapturaDatosDiariosMaquinaria> parametros)
        {
            bool lstResultDatos = false;
            try
            {
                foreach (var item in parametros)
                {
                    var fech = item.fechaCapturaMaquinaria.ToShortDateString();
                    DateTime a = Convert.ToDateTime(fech);
                    var obj = _context.tblM_CapturaDatosDiariosMaquinaria.Where(x => x.fechaCapturaMaquinaria == a && x.idCatMaquina == item.idCatMaquina).FirstOrDefault();
                    if (obj == null)
                    {
                        obj = new tblM_CapturaDatosDiariosMaquinaria();
                        obj.Enviado = false;
                        _context.tblM_CapturaDatosDiariosMaquinaria.Add(item);
                        _context.SaveChanges();
                    }
                    else
                    {
                        obj.fechaCapturaMaquinaria = item.fechaCapturaMaquinaria;
                        obj.idCatMaquina = item.idCatMaquina;
                        obj.FechaPatioMaquinaria = item.FechaPatioMaquinaria;
                        obj.FechaTMC = item.FechaTMC;
                        obj.FechaMaquinaria = item.FechaMaquinaria;
                        obj.idEstatus = item.idEstatus;
                        obj.Observaciones = item.Observaciones;
                        _context.SaveChanges();
                    }
                }

            }
            catch (Exception e)
            {
                LogError(2, 0, "CapturaDatosController", "ObtenerCatMaquinas", e, AccionEnum.AGREGAR, 0, lstResultDatos);
            }
            return lstResultDatos;
        }

        public MemoryStream GenerarExcelDatosDiarios(datosDiariosDTO parametros, int idEmpresa)
        {
            List<resultDatosDiariosDTO> lstMaqui = new List<resultDatosDiariosDTO>();
            List<resultDatosDiariosDTO> lstMaquinas = new List<resultDatosDiariosDTO>();
            lstMaqui = ObtenerCatMaquinas(parametros, idEmpresa);
            List<tblM_CatTipoMaquinaria> lstTipoGrupos = new List<tblM_CatTipoMaquinaria>();
            lstTipoGrupos = _context.tblM_CatTipoMaquinaria.Where(r => r.estatus == true).ToList();
            resultDatosDiariosDTO objMaquinas = new resultDatosDiariosDTO();



            Color colorDeCelda = ColorTranslator.FromHtml("#fff");
            Color colorDeCeldaNegro = ColorTranslator.FromHtml("#000");
            using (ExcelPackage excel = new ExcelPackage())
            {
                var hoja1 = excel.Workbook.Worksheets.Add("CapturaDatosDiarios");
                int num = 11;
                foreach (var item in lstTipoGrupos)
                {
                    objMaquinas = new resultDatosDiariosDTO();
                    objMaquinas.descripcion = item.descripcion;
                    lstMaquinas.Add(objMaquinas);

                    string titulo = "B" + num + ":" + "L" + num;
                    hoja1.Cells[titulo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[titulo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[titulo].Style.Fill.BackgroundColor.SetColor(colorDeCeldaNegro);
                    hoja1.Cells[titulo].Style.Font.Color.SetColor(Color.White);
                    var lst = lstMaqui.Where(r => r.tipoEquipoID == item.id).ToList();
                    lstMaquinas.AddRange(lst);
                    num = (num + lst.Count() + 1);
                }


                Bitmap image = new Bitmap(@"\\10.1.0.112\Proyecto\SIGOPLAN\IMAGENES\logo.png");
                ExcelPicture excelImage = null;
                if (image != null)
                {
                    excelImage = hoja1.Drawings.AddPicture("Debopam Pal", image);
                    excelImage.From.Column = 1;
                    excelImage.From.Row = 1;
                    excelImage.SetSize(240, 140);
                    // 2x2 px space for better alignment
                    excelImage.From.ColumnOff = 1;
                    excelImage.From.RowOff = 1;
                }
                string TituloRan = "B1:" + "L8";
                hoja1.Cells[TituloRan].Style.Border.Top.Style = ExcelBorderStyle.Thick;
                hoja1.Cells[TituloRan].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                hoja1.Cells[TituloRan].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                hoja1.Cells[TituloRan].Style.Border.Left.Style = ExcelBorderStyle.Thick;
                hoja1.Cells[TituloRan].Style.Border.Top.Color.SetColor(Color.Black);

                string TituloRange1 = "B2:" + "L3";
                List<string[]> headerRow1 = new List<string[]>() { new string[] { 
                    "Dirección de Maquinaria y Equipo"
                } };
                hoja1.Cells[TituloRange1].LoadFromArrays(headerRow1);
                hoja1.Cells[TituloRange1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja1.Cells[TituloRange1].Merge = true;
                hoja1.Cells[TituloRange1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                hoja1.Cells[TituloRange1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja1.Cells[TituloRange1].Style.Fill.BackgroundColor.SetColor(1, 255, 255, 255);
                hoja1.Cells[TituloRange1].Style.Border.Top.Style = ExcelBorderStyle.Thick;
                hoja1.Cells[TituloRange1].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                hoja1.Cells[TituloRange1].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                hoja1.Cells[TituloRange1].Style.Border.Left.Style = ExcelBorderStyle.Thick;
                hoja1.Cells[TituloRange1].Style.Border.Top.Color.SetColor(Color.Black);
                hoja1.Cells[TituloRange1].Style.Font.Size = 16;

                string TituloRange2 = "B5:" + "L6";
                List<string[]> headerRow2 = new List<string[]>() { new string[] { 
                    "Reporte de estatus de maquinaria"
                } };
                hoja1.Cells[TituloRange2].LoadFromArrays(headerRow2);
                hoja1.Cells[TituloRange2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja1.Cells[TituloRange2].Merge = true;
                hoja1.Cells[TituloRange2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                hoja1.Cells[TituloRange2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja1.Cells[TituloRange2].Style.Fill.BackgroundColor.SetColor(1, 255, 255, 255);
                hoja1.Cells[TituloRange2].Style.Border.Top.Style = ExcelBorderStyle.Thick;
                hoja1.Cells[TituloRange2].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                hoja1.Cells[TituloRange2].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                hoja1.Cells[TituloRange2].Style.Border.Left.Style = ExcelBorderStyle.Thick;
                hoja1.Cells[TituloRange2].Style.Border.Top.Color.SetColor(Color.Black);
                hoja1.Cells[TituloRange2].Style.Font.Size = 16;

                string TituloRange = "B10:" + "L10";
                List<string[]> headerRow = new List<string[]>() { new string[] { 
                    "No.", 
                    "Económico", 
                    "Descripción",
                    "Marca", 
                    "Modelo", 
                    "Serie", 
                    "Horómetro", 
                    "Fecha de Ingreso al patio de maquinaria", 
                    "Fecha de Ingreso al TMC", 
                    "Fecha de reingreso al patio de maquinaria(Disponible)",
                    "Estado", 
 
                } };
                hoja1.Cells[TituloRange].LoadFromArrays(headerRow);
                hoja1.Cells[TituloRange].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja1.Cells[TituloRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                hoja1.Cells[TituloRange].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja1.Cells[TituloRange].Style.Font.Color.SetColor(colorDeCeldaNegro);


                string headerRange = "B10:" + "L10";
                hoja1.Cells[headerRange].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja1.Cells[headerRange].Style.Font.Color.SetColor(colorDeCelda);
                hoja1.Cells[headerRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                hoja1.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(1, 237, 125, 49);
                hoja1.Cells[headerRange].Style.Border.Top.Style = ExcelBorderStyle.Thick;
                hoja1.Cells[headerRange].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                hoja1.Cells[headerRange].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                hoja1.Cells[headerRange].Style.Border.Left.Style = ExcelBorderStyle.Thick;
                hoja1.Cells[headerRange].Style.Border.Top.Color.SetColor(Color.Black);
                hoja1.Cells[headerRange].Style.Font.Size = 12;

                hoja1.Cells[headerRange].Style.Font.Bold = true;
                hoja1.Cells[headerRange].Style.WrapText = true;
                hoja1.Column(1).Width = 1;
                hoja1.Column(2).Width = 6;
                hoja1.Column(3).Width = 10.50;
                hoja1.Column(4).Width = 36;
                hoja1.Column(5).Width = 21;
                hoja1.Column(6).Width = 21;
                hoja1.Column(7).Width = 22;
                hoja1.Column(8).Width = 10;
                hoja1.Column(9).Width = 13;
                hoja1.Column(10).Width = 13;
                hoja1.Column(11).Width = 13;
                hoja1.Column(12).Width = 30;


                var cellData = new List<object[]>();
                int contadorLista = 0;
                foreach (var ins in lstMaquinas)
                {

                    cellData.Add(new object[]{
                                ins.Economico,
                                ins.economicoDescripcion,
                                ins.descripcion,
                                ins.Marca,
                                ins.Modelo,
                                ins.Serie,
                                ins.Horometro,
                                 Convert.ToDateTime(ins.FechaPatioMaquinaria).ToString("dd/MM/yyyy") == "01/01/0001" ? "" : Convert.ToDateTime(ins.FechaPatioMaquinaria).ToString("dd/MM/yyyy"),
                                 Convert.ToDateTime(ins.FechaTMC).ToString("dd/MM/yyyy") == "01/01/0001" ? "" : Convert.ToDateTime(ins.FechaTMC).ToString("dd/MM/yyyy"),
                                 Convert.ToDateTime(ins.FechaMaquinaria).ToString("dd/MM/yyyy") == "01/01/0001" ? "" : Convert.ToDateTime(ins.FechaMaquinaria).ToString("dd/MM/yyyy"),
                                ins.opcionStatus, 
                            });
                    contadorLista++;
                }

                hoja1.Cells[11, 2].LoadFromArrays(cellData);
                int cont = 10;
                foreach (var item in lstMaquinas)
                {
                    cont++;
                    string headAling = "B" + cont + ":" + "L" + cont;
                    hoja1.Cells[headAling].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[headAling].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja1.Cells[headAling].Style.Font.Size = 8;
                    //hoja1.Cells[headAling].AutoFitColumns();
                    //hoja1.Column(cont).BestFit = true;
                }
                excel.Compression = OfficeOpenXml.CompressionLevel.BestSpeed;
                var bytes = new MemoryStream();
                using (var exportData = new MemoryStream())
                {
                    excel.SaveAs(exportData);
                    bytes = exportData;
                }

                return bytes;
            }
        }

        //Dictionary<string, object> param = new Dictionary<string, object>();
        //      param.Add("VALUE", "200");
        //      obte(param);

        //public string obte(Dictionary<string, object> param)
        //{
        //    string o = "";

        //    o = param["VALUE"].ToString();

        //    return o;
        //}
        //(string subject, string msg, List<string> emails, List<Byte[]> ListaArchivos, string nombreArchivo
        public MemoryStream GenerarExcelDatosDiariosEnviandocorreo(datosDiariosDTO parametros, int idEmpresa)
        {
            var result = GenerarExcelDatosDiarios(parametros, idEmpresa);
            var Arreglo = result.ToArray();
            string subject = "Captura de datos diarios";
            string msg = "se genero una captura de datos diarios";
            List<string> emails = new List<string>();
            List<Byte[]> ListaArchivos = new List<byte[]>();

            ListaArchivos.Add(Arreglo);
            emails.Add("f.artalejo@construplan.com.mx");
            emails.Add("martin.zayas@construplan.com.mx");
            emails.Add("rene.olea@construplan.com.mx");
            string nombreArchivo = "CapturadeDatosDiarios.xlsx";

            GlobalUtils.sendEmailAdjuntoInMemorySendExcel(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), msg, emails, ListaArchivos, nombreArchivo);
            string fecha = DateTime.Now.ToShortDateString();
            DateTime FechaActual = Convert.ToDateTime(fecha);
            var lstOb = _context.tblM_CapturaDatosDiariosMaquinaria.Where(r => r.fechaCapturaMaquinaria == FechaActual).ToList();
            if (lstOb.Count != 0)
            {
                foreach (var item in lstOb)
                {
                    item.Enviado = true;
                }
                _context.SaveChanges();
            }


            return result;
        }

        public int ObtenerBotonEnviarExcel(DateTime Fecha)
        {
            int retorn = 0;
            DateTime? f = new DateTime();
            string fe = Fecha.ToShortDateString();
            f = Convert.ToDateTime(fe);

            var obj = _context.tblM_CapturaDatosDiariosMaquinaria.Where(z => z.fechaCapturaMaquinaria == f && z.Enviado == false).FirstOrDefault();
            if (obj != null)
            {
                if (obj.Enviado == false)
                {
                    return retorn = 1;
                }
                else
                {
                    return retorn = 2;
                }
            }
            else
            {
                var ob2j = _context.tblM_CapturaDatosDiariosMaquinaria.Where(z => z.fechaCapturaMaquinaria == f && z.Enviado == true).FirstOrDefault();
                if (ob2j != null)
                {
                    return retorn = 3;
                }
                else
                {
                    return retorn = 4;
                }
            }
        }

        public bool PermisoBoton(int idUsuario)
        {
            bool retornarValor = false;

            tblP_AccionesVistatblP_Usuario obj = _context.tblP_AccionesVistatblP_Usuario.Where(r => r.tblP_Usuario_id == idUsuario && r.sistema == 1 && r.tblP_AccionesVista_id == 4027).FirstOrDefault();
            if (obj != null)
            {
                retornarValor = true;
            }

            return retornarValor;
        }

        public List<ComboDTO> ObtenerGrupo()
        {
            return _context.tblM_CatGrupoMaquinaria.Where(r => r.estatus).ToList().Select(y => new ComboDTO
            {
                Value = y.id.ToString(),
                Text = y.descripcion
            }).ToList();
        }
        public List<ComboDTO> ObtenerModelo(int idGrupo)
        {
            return _context.tblM_CatModeloEquipo.Where(r => r.estatus && r.idGrupo == idGrupo).ToList().Select(y => new ComboDTO
            {
                Value = y.id.ToString(),
                Text = y.descripcion
            }).ToList();
        }
        public bool guardar_Estatus_Diario(tblM_CatMaquina_EstatusDiario obj, List<tblM_CatMaquina_EstatusDiario_Det> det)
        {
            bool result = true;
            try {
                obj.fecha = DateTime.Now;
                obj.usuario = vSesiones.sesionUsuarioDTO.id;
                _context.tblM_CatMaquina_EstatusDiario.Add(obj);
                _context.SaveChanges();

                det.ForEach(x=> x.estatusDiarioID = obj.id);
                _context.tblM_CatMaquina_EstatusDiario_Det.AddRange(det);
                _context.SaveChanges();
            }
            catch(Exception e){
                result = false;
            }
            return result;
        }
        public EstatusDiarioDTO getEstatus_Diario(DateTime fecha, string cc)
        {
            var result = new EstatusDiarioDTO();

            var existe = _context.tblM_CatMaquina_EstatusDiario.FirstOrDefault(x=>x.cc.Equals(cc) && x.fecha == fecha);
            if (existe != null)
            {
                result.obj = existe;
                var det = _context.tblM_CatMaquina_EstatusDiario_Det.Where(x => x.estatusDiarioID == existe.id).ToList();
                foreach (var i in det)
                {
                    var mqq = _context.tblM_CatMaquina.FirstOrDefault(x=>x.id == i.noEconomicoID);
                    i.modelo = mqq.modeloEquipo.descripcion;
                }
                result.det = det;
            }
            else {
                var economicosEspecial = _context.tblM_CatMaquina_EstatusDiario_Economico_Especial.Select(x => x.noEconomicoID).ToList();
                var data = _context.tblM_CatMaquina.Where(x => x.centro_costos.Equals(cc) && (x.grupoMaquinaria.tipoEquipoID == 1 || economicosEspecial.Contains(x.id)) && x.estatus != 0).OrderBy(x => x.noEconomico).ToList();
                tblM_CatMaquina_EstatusDiario obj = new tblM_CatMaquina_EstatusDiario();
                List<tblM_CatMaquina_EstatusDiario_Det> det = new List<tblM_CatMaquina_EstatusDiario_Det>();
                obj.cc = cc;
                obj.id = 0;
                obj.estatus = 0;
                obj.cantActivos = 0;
                obj.cantInactivos = 0;
                obj.porActivos = 0;
                obj.porInactivos = 0;
                result.obj = obj;
                var diaAnterior = fecha.AddDays(-1);
                foreach (var i in data)
                {
                    var ultimoParo = _context.tblM_CatMaquina_EstatusDiario_Det.Where(x => x.noEconomico == i.noEconomico).OrderByDescending(x => x.id).FirstOrDefault();
                    if (ultimoParo != null)
                    {
                        if (ultimoParo.activo)
                        {
                            var o = new tblM_CatMaquina_EstatusDiario_Det();
                            o.noEconomico = i.noEconomico;
                            o.noEconomicoID = i.id;
                            o.descripcion = i.grupoMaquinaria.descripcion;
                            o.modelo = _context.tblM_CatModeloEquipo.FirstOrDefault(x => x.id == i.modeloEquipoID).descripcion;
                            o.activo = true;
                            o.causa = "";
                            o.acciones = "";
                            o.tiempo_respuesta_str = "--";

                            det.Add(o);
                        }
                        else {
                            var o = new tblM_CatMaquina_EstatusDiario_Det();
                            o.noEconomico = i.noEconomico;
                            o.noEconomicoID = i.id;
                            o.descripcion = i.grupoMaquinaria.descripcion;
                            o.modelo = _context.tblM_CatModeloEquipo.FirstOrDefault(x => x.id == i.modeloEquipoID).descripcion;
                            o.activo = false;
                            o.causa = ultimoParo.causa;
                            o.fecha_inicial = ultimoParo.fecha_inicial;
                            o.fecha_proyectada = ultimoParo.fecha_proyectada;
                            o.acciones = ultimoParo.acciones;
                            o.tiempo_respuesta = ultimoParo.tiempo_respuesta;
                            o.tiempo_respuesta_str = ultimoParo.tiempo_respuesta_str;

                            det.Add(o);
                        }
                    }
                    else {
                        var o = new tblM_CatMaquina_EstatusDiario_Det();
                        o.noEconomico = i.noEconomico;
                        o.noEconomicoID = i.id;
                        o.modelo = _context.tblM_CatModeloEquipo.FirstOrDefault(x=>x.id == i.modeloEquipoID).descripcion;
                        o.descripcion = i.grupoMaquinaria.descripcion;
                        o.activo = true;
                        o.causa = "";
                        o.acciones = "";
                        o.tiempo_respuesta_str = "--";

                        det.Add(o);
                    }
                    
                }
                result.det = det;
            }

            return result;
        }
        public void saveCapturarDatosDiaros(tblM_CatMaquina_EstatusDiario obj, List<tblM_CatMaquina_EstatusDiario_Det> det)
        {
            if(obj.id == 0)
            {
                obj.usuario = vSesiones.sesionUsuarioDTO.id;
                obj.estatus = 1;
                _context.tblM_CatMaquina_EstatusDiario.Add(obj);
                _context.SaveChanges();

                det.ForEach(x => x.estatusDiarioID = obj.id);
                _context.tblM_CatMaquina_EstatusDiario_Det.AddRange(det);
                _context.SaveChanges();
            }
            else
            {
                var objData = _context.tblM_CatMaquina_EstatusDiario.FirstOrDefault(x => x.id == obj.id);
                objData.usuario = vSesiones.sesionUsuarioDTO.id;

                var detData = _context.tblM_CatMaquina_EstatusDiario_Det.Where(x => x.estatusDiarioID == obj.id);
                foreach (var i in detData)
                {
                    var o = det.FirstOrDefault(x => x.noEconomicoID == i.noEconomicoID);
                    i.activo = o.activo;
                    i.causa = o.causa;
                    i.fecha_inicial = o.fecha_inicial;
                    i.fecha_proyectada = o.fecha_proyectada;
                    i.fecha_real = o.fecha_real;
                    i.tiempo_respuesta_str = o.tiempo_respuesta_str;
                    i.tiempo_respuesta = o.tiempo_respuesta;
                    i.acciones = o.acciones;
                }
                _context.SaveChanges();
            }
            var ccData = _context.tblP_CC.FirstOrDefault(x=>x.areaCuenta.Equals(obj.cc));
            string subject = "SIGOPLAN: REPORTE ESTATUS DIARIO MAQUINARIA "+obj.fecha.ToShortDateString()+" AC "+obj.cc+" - "+ccData;
            string msg = @"Buen dia.<br/>
                            <br/>
                            Se genero un nuevo reporte de estatus Diario de Maquinaria <br/>
                            <br/>
                            FECHA: "+obj.fecha.ToShortDateString()+@"<br/>
                            OBRA:  AC "+obj.cc+" - "+ccData+@"<br/>
                            USUARIO GENERO: "+vSesiones.sesionUsuarioDTO.nombre+@".<br/>
                            <br/>
                            Este es un correo autogenerado por SIGOPLAN favor de no responder.
                            ";
            List<string> emails = new List<string>();
            emails.Add(vSesiones.sesionUsuarioDTO.correo);
            emails.Add("luis.fortino@construplan.com.mx");
            emails.Add("g.reina@construplan.com.mx");
            List<Byte[]> ListaArchivos = new List<byte[]>();
            string nombreArchivo = "ESTATUS DIARIO AC "+obj.cc;
            GlobalUtils.sendEmailAdjuntoInMemorySend(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), msg, emails, ListaArchivos, nombreArchivo);
        }
        public void sendCapturarDatosDiaros(string cc, DateTime fecha,List<byte[]> archivos)
        {
            var empresaActual = vSesiones.sesionEmpresaActual;            
            tblM_CatMaquina_EstatusDiario obj = _context.tblM_CatMaquina_EstatusDiario.FirstOrDefault(x=> x.cc==cc && x.fecha == fecha);
            List<tblM_CatMaquina_EstatusDiario_Det> det = _context.tblM_CatMaquina_EstatusDiario_Det.Where(x=>x.estatusDiarioID == obj.id).ToList();
            var ccData = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta.Equals(obj.cc));
            string subject = "";
            string msg = "";            
            switch (empresaActual)
            {
                case 2:
                    subject = "SIGOPLAN ARRENDADORA: REPORTE ESTATUS DIARIO MAQUINARIA " + obj.fecha.ToShortDateString() + " AC " + obj.cc + " - " + ccData.descripcion;
                    msg = @"Buen dia.<br/>
                            <br/>
                            Se genero un nuevo reporte de estatus Diario de Maquinaria <br/>
                            <br/>
                            FECHA: " + obj.fecha.ToShortDateString() + @"<br/>
                            OBRA:  AC " + obj.cc + " - " + ccData.descripcion + @"<br/>
                            USUARIO GENERO: " + vSesiones.sesionUsuarioDTO.nombre + @".<br/>
                            <br/>
                            Este es un correo autogenerado por SIGOPLAN favor de no responder.
                            ";
                    break;
                case 6:
                    subject = "SIGOPLAN PERU: REPORTE ESTATUS DIARIO MAQUINARIA " + obj.fecha.ToShortDateString() + " AC " + obj.cc + " - " + ccData.descripcion;
                    msg = @"Buen dia.<br/>
                            <br/>
                            Se genero un nuevo reporte de estatus Diario de Maquinaria <br/>
                            <br/>
                            FECHA: " + obj.fecha.ToShortDateString() + @"<br/>
                            OBRA:  AC " + obj.cc + " - " + ccData.descripcion + @"<br/>
                            USUARIO GENERO: " + vSesiones.sesionUsuarioDTO.nombre + @".<br/>
                            <br/>
                            Este es un correo autogenerado por SIGOPLAN favor de no responder.
                            ";
                    break;
                default:
                    subject = "SIGOPLAN: REPORTE ESTATUS DIARIO MAQUINARIA " + obj.fecha.ToShortDateString() + " AC " + obj.cc + " - " + ccData.descripcion;
                    msg = @"Buen dia.<br/>
                            <br/>
                            Se genero un nuevo reporte de estatus Diario de Maquinaria <br/>
                            <br/>
                            FECHA: " + obj.fecha.ToShortDateString() + @"<br/>
                            OBRA:  AC " + obj.cc + " - " + ccData.descripcion + @"<br/>
                            USUARIO GENERO: " + vSesiones.sesionUsuarioDTO.nombre + @".<br/>
                            <br/>
                            Este es un correo autogenerado por SIGOPLAN favor de no responder.
                            ";
                    break;
            }
            List<string> emails = new List<string>();
            emails.Add(vSesiones.sesionUsuarioDTO.correo);
            var usuarios = _context.tblM_CatMaquina_EstatusDiario_Usuario_CC.Where(x => x.cc == cc).Select(x => x.usuarioID).ToList();
            var usuariosTodosLosCC = _context.tblM_CatMaquina_EstatusDiario_Usuario_CC.Where(x => x.cc == "*").Select(x => x.usuarioID).ToList();
            var correos = _context.tblP_Usuario.Where(x => usuarios.Contains(x.id) && x.correo!=null).Select(x=>x.correo).ToList();
            var correosTodosLosCC = _context.tblP_Usuario.Where(x => usuariosTodosLosCC.Contains(x.id) && x.correo != null).Select(x => x.correo).ToList();
            emails.AddRange(correos);
            emails.AddRange(correosTodosLosCC);
            string nombreArchivo = "ESTATUS DIARIO AC " + obj.cc;
            GlobalUtils.sendEmailAdjuntoInMemorySend(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), msg, emails.Distinct().ToList(), archivos, nombreArchivo);
        }

        public Dictionary<string, object> CargarGraficasDashboard(List<string> listaAreaCuenta)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var listaGraficas = new List<DashboardEstatusDiarioDTO>();

                if (listaAreaCuenta.Contains("Todos"))
                {
                    var virtuales = new List<string>();
                    virtuales.Add("1010");
                    virtuales.Add("1015");
                    virtuales.Add("1018");
                    listaAreaCuenta = _context.tblP_CC.Where(x => x.estatus == true && !virtuales.Contains(x.areaCuenta)).OrderBy(x=>x.area).ThenBy(x=>x.cuenta).Select(y => y.areaCuenta).ToList();
                }

                foreach (var areaCuenta in listaAreaCuenta)
                {
                    var registroAreaCuenta = _context.tblP_CC.FirstOrDefault(x => x.estatus && x.areaCuenta == areaCuenta);
                    var registroEstatusDiario = _context.tblM_CatMaquina_EstatusDiario.Where(x => x.cc == areaCuenta).OrderByDescending(x => x.fecha).FirstOrDefault();

                    if (registroEstatusDiario != null)
                    {
                        var grafica = new GraficaDTO();
                        var listaDetalleEstatusDiario = _context.tblM_CatMaquina_EstatusDiario_Det.Where(x => !x.activo && x.estatusDiarioID == registroEstatusDiario.id).ToList();

                        grafica.serie1Descripcion = "Días Inactividad";

                        foreach (var det in listaDetalleEstatusDiario)
                        {
                            grafica.categorias.Add(det.noEconomico);
                            grafica.serie1.Add(det.fecha_inicial != null ? (decimal)(registroEstatusDiario.fecha.Date - ((DateTime)det.fecha_inicial).Date).TotalDays : 0m);

                            grafica.listaCausa.Add(det.causa);
                            grafica.listaFechaInicial.Add(det.fecha_inicial != null ? ((DateTime)det.fecha_inicial).ToShortDateString() : "");
                            grafica.listaFechaProyectada.Add(det.fecha_proyectada != null ? ((DateTime)det.fecha_proyectada).ToShortDateString() : "");
                            grafica.listaAcciones.Add(det.acciones ?? "");
                        }

                        listaGraficas.Add(new DashboardEstatusDiarioDTO
                        {
                            tituloAreaCuenta = registroAreaCuenta.descripcion.ToUpper(),
                            fechaUltimaActualizacionString = registroEstatusDiario.fecha.ToShortDateString(),
                            datosGrafica = grafica
                        });
                    }
                }

                resultado.Add("listaGraficas", listaGraficas);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "CapturaDatosController", "CargarGraficasDashboard", e, AccionEnum.CONSULTA, 0, listaAreaCuenta);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }
    }
}
