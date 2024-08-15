using Core.DTO.Utils.Excel;
using Infrastructure.DTO;
using NPOI.HSSF.UserModel;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Infrastructure.Utils
{
    public class ExcelUtilities
    {

        public MemoryStream CreateExcelFile(Controller controller, List<excelSheetDTO> Sheets, string fileName)
        {
            try
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                foreach (var i in Sheets)
                {
                    CreateExcelSheet(workbook, i.Sheet, i.name);
                }

                using (var exportData = new MemoryStream())
                {
                    controller.Response.Clear();
                    workbook.Write(exportData);

                    controller.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    controller.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName + ".xlsx"));
                    controller.Response.BinaryWrite(exportData.ToArray());

                    controller.Response.End();
                    return exportData;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public ISheet CreateExcelSheet(IWorkbook workbook, List<excelRowDTO> data, string name)
        {
            #region TAB
            ISheet sheet = workbook.CreateSheet(name);
            //make a header row
            IRow rowHeader = sheet.CreateRow(0);
            var maxCol = data.Select(x => x.cells.Count).Max();
            for (int j = 0; j < maxCol; j++)
            {
                sheet.SetColumnWidth(j, 7500);
                ICell cell = rowHeader.CreateCell(j);
                cell.SetCellValue(FillEmpty(j + 1));
            }
            //Crear tipo de letra
            XSSFFont font = (XSSFFont)workbook.CreateFont();
            XSSFColor colorToFillGray = new XSSFColor(Color.FromArgb(220, 118, 51));
            XSSFColor colorToFillEmpty = new XSSFColor(Color.Empty);
            XSSFColor colorFontWhite = new XSSFColor(Color.White);
            XSSFColor colorFontBlack = new XSSFColor(Color.Black);
            //loops through data
            for (int i = 0; i < data.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                row.Height = -1;
                for (int j = 0; j < data[i].cells.Count; j++)
                {
                    //Create Style
                    XSSFCellStyle styleHeader = (XSSFCellStyle)workbook.CreateCellStyle();

                    styleHeader.WrapText = true;
                    styleHeader.VerticalAlignment = VerticalAlignment.Center;
                    styleHeader.Alignment = HorizontalAlignment.Center;
                    ICell cell = row.CreateCell(j);
                    cell.CellStyle.WrapText = true;


                    //Crea background-color
                    if (data[i].cells[j].fill == true)
                    {
                        styleHeader.SetFillForegroundColor(colorToFillGray);
                        styleHeader.FillPattern = FillPattern.SolidForeground;

                        font.SetColor(colorFontWhite);
                        font.Boldweight = (short)FontBoldWeight.Bold;
                        styleHeader.SetFont(font);

                        //Set border
                        if (data[i].cells[j].border == true)
                        {
                            styleHeader.BorderTop = BorderStyle.Thin;
                            styleHeader.BorderRight = BorderStyle.Thin;
                            styleHeader.BorderLeft = BorderStyle.Thin;
                            styleHeader.BorderBottom = BorderStyle.Thin;
                        }
                        else
                        {
                            styleHeader.BorderTop = BorderStyle.None;
                            styleHeader.BorderRight = BorderStyle.None;
                            styleHeader.BorderLeft = BorderStyle.None;
                            styleHeader.BorderBottom = BorderStyle.None;
                        }
                        //Set defined style
                        cell.CellStyle = styleHeader;
                    }
                    else
                    {
                        //Create Style
                        XSSFCellStyle style = (XSSFCellStyle)workbook.CreateCellStyle();

                        style.WrapText = true;
                        style.VerticalAlignment = VerticalAlignment.Center;
                        style.Alignment = HorizontalAlignment.Center;
                        style.FillPattern = FillPattern.NoFill;
                        font.SetColor(colorFontBlack);
                        style.FillForegroundColorColor = colorToFillEmpty;

                        //Set border
                        if (data[i].cells[j].border == true)
                        {
                            style.BorderTop = BorderStyle.Thin;
                            style.BorderRight = BorderStyle.Thin;
                            style.BorderLeft = BorderStyle.Thin;
                            style.BorderBottom = BorderStyle.Thin;
                        }
                        else
                        {
                            style.BorderTop = BorderStyle.None;
                            style.BorderRight = BorderStyle.None;
                            style.BorderLeft = BorderStyle.None;
                            style.BorderBottom = BorderStyle.None;
                        }
                        //Set defined style
                        cell.CellStyle = style;
                    }

                    //Set text
                    cell.SetCellValue(data[i].cells[j].text);
                    var colSpan = data[i].cells[j].colSpan;
                    var rowSpan = data[i].cells[j].rowSpan;
                    if (data[i].cells[j].colSpan > 0)
                    {
                        NPOI.SS.Util.CellRangeAddress mr = new NPOI.SS.Util.CellRangeAddress(i + 1, i + 1, j, j + (colSpan - 1));
                        sheet.AddMergedRegion(mr);
                    }
                    if (data[i].cells[j].rowSpan > 0)
                    {

                        NPOI.SS.Util.CellRangeAddress mr = new NPOI.SS.Util.CellRangeAddress(i + 1, (i + 1) + (rowSpan - 1), j, j);
                        sheet.AddMergedRegion(mr);
                    }
                    cell.Row.Height = (short)(row.Height * (rowSpan * 2));
                    //---------------------------
                }
            }
            var dataLast = data.LastOrDefault();
            for (int i = 0; i < data.LastOrDefault().cells.Count; i++)
            {
                if (dataLast.cells[i].autoWidthFit)
                {
                    sheet.AutoSizeColumn(i);
                }
            }

            return sheet;
            #endregion
        }

        public MemoryStream CreateExcelFileEstiloPredefinido(Controller controller, List<excelSheetDTO> Sheets, string fileName)
        {

            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            foreach (var i in Sheets)
            {
                CreateExcelSheetEstiloPredefinido(workbook, i.Sheet, i.name);
            }

            using (var exportData = new MemoryStream())
            {
                controller.Response.Clear();
                workbook.Write(exportData);

                controller.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                controller.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName + ".xlsx"));
                controller.Response.BinaryWrite(exportData.ToArray());

                controller.Response.End();
                return exportData;
            }
        }

        public ISheet CreateExcelSheetEstiloPredefinido(IWorkbook workbook, List<excelRowDTO> data, string name)
        {
            #region TAB
            ISheet sheet = workbook.CreateSheet(name);

            IRow rowHeader = sheet.CreateRow(0);

            var maxCol = data.Select(x => x.cells.Count).Max();

            for (int j = 0; j < maxCol; j++)
            {
                sheet.SetColumnWidth(j, 7500);
                ICell cell = rowHeader.CreateCell(j);
                cell.SetCellValue(FillEmpty(j + 1));
            }

            XSSFColor colorToFillGray = new XSSFColor(Color.FromArgb(220, 118, 51));
            XSSFColor colorToFillEmpty = new XSSFColor(Color.Empty);
            XSSFColor colorFontWhite = new XSSFColor(Color.White);
            XSSFColor colorFontBlack = new XSSFColor(Color.Black);

            #region Estilo_Encabezado_Fill_BordeTrue
            XSSFCellStyle estiloEncabezadoFillBordeTrue = (XSSFCellStyle)workbook.CreateCellStyle();

            estiloEncabezadoFillBordeTrue.WrapText = true;
            estiloEncabezadoFillBordeTrue.VerticalAlignment = VerticalAlignment.Center;
            estiloEncabezadoFillBordeTrue.Alignment = HorizontalAlignment.Center;

            estiloEncabezadoFillBordeTrue.SetFillForegroundColor(colorToFillGray);
            estiloEncabezadoFillBordeTrue.FillPattern = FillPattern.SolidForeground;

            XSSFFont fontEncabezadoFillBordeTrue = (XSSFFont)workbook.CreateFont();
            fontEncabezadoFillBordeTrue.SetColor(colorFontWhite);
            fontEncabezadoFillBordeTrue.Boldweight = (short)FontBoldWeight.Bold;
            estiloEncabezadoFillBordeTrue.SetFont(fontEncabezadoFillBordeTrue);

            estiloEncabezadoFillBordeTrue.BorderTop = BorderStyle.Thin;
            estiloEncabezadoFillBordeTrue.BorderRight = BorderStyle.Thin;
            estiloEncabezadoFillBordeTrue.BorderLeft = BorderStyle.Thin;
            estiloEncabezadoFillBordeTrue.BorderBottom = BorderStyle.Thin;
            #endregion
            #region Estilo_Encabezado_Fill_BordeFalse
            XSSFCellStyle estiloEncabezadoFillBordeFalse = (XSSFCellStyle)workbook.CreateCellStyle();

            estiloEncabezadoFillBordeFalse.WrapText = true;
            estiloEncabezadoFillBordeFalse.VerticalAlignment = VerticalAlignment.Center;
            estiloEncabezadoFillBordeFalse.Alignment = HorizontalAlignment.Center;

            estiloEncabezadoFillBordeFalse.SetFillForegroundColor(colorToFillGray);
            estiloEncabezadoFillBordeFalse.FillPattern = FillPattern.SolidForeground;

            XSSFFont fontEncabezadoFillBordeFalse = (XSSFFont)workbook.CreateFont();
            fontEncabezadoFillBordeFalse.SetColor(colorFontWhite);
            fontEncabezadoFillBordeFalse.Boldweight = (short)FontBoldWeight.Bold;
            estiloEncabezadoFillBordeFalse.SetFont(fontEncabezadoFillBordeFalse);

            estiloEncabezadoFillBordeFalse.BorderTop = BorderStyle.None;
            estiloEncabezadoFillBordeFalse.BorderRight = BorderStyle.None;
            estiloEncabezadoFillBordeFalse.BorderLeft = BorderStyle.None;
            estiloEncabezadoFillBordeFalse.BorderBottom = BorderStyle.None;
            #endregion
            #region Estilo_NoFill_BordeTrue
            XSSFCellStyle estiloNoFillBordeTrue = (XSSFCellStyle)workbook.CreateCellStyle();

            estiloNoFillBordeTrue.WrapText = true;
            estiloNoFillBordeTrue.VerticalAlignment = VerticalAlignment.Center;
            estiloNoFillBordeTrue.Alignment = HorizontalAlignment.Center;
            estiloNoFillBordeTrue.FillPattern = FillPattern.NoFill;

            XSSFFont fontNoFillBordeTrue = (XSSFFont)workbook.CreateFont();
            fontNoFillBordeTrue.SetColor(colorFontBlack);

            estiloNoFillBordeTrue.FillForegroundColorColor = colorToFillEmpty;
            estiloNoFillBordeTrue.BorderTop = BorderStyle.Thin;
            estiloNoFillBordeTrue.BorderRight = BorderStyle.Thin;
            estiloNoFillBordeTrue.BorderLeft = BorderStyle.Thin;
            estiloNoFillBordeTrue.BorderBottom = BorderStyle.Thin;
            #endregion
            #region Estilo_NoFill_BordeFalse
            XSSFCellStyle estiloNoFillBordeFalse = (XSSFCellStyle)workbook.CreateCellStyle();

            estiloNoFillBordeFalse.WrapText = true;
            estiloNoFillBordeFalse.VerticalAlignment = VerticalAlignment.Center;
            estiloNoFillBordeFalse.Alignment = HorizontalAlignment.Center;
            estiloNoFillBordeFalse.FillPattern = FillPattern.NoFill;

            XSSFFont fontNoFillBordeFalse = (XSSFFont)workbook.CreateFont();
            fontNoFillBordeFalse.SetColor(colorFontBlack);

            estiloNoFillBordeFalse.FillForegroundColorColor = colorToFillEmpty;
            estiloNoFillBordeFalse.BorderTop = BorderStyle.None;
            estiloNoFillBordeFalse.BorderRight = BorderStyle.None;
            estiloNoFillBordeFalse.BorderLeft = BorderStyle.None;
            estiloNoFillBordeFalse.BorderBottom = BorderStyle.None;
            #endregion
            #region Estilo_Porcentaje_BordeTrue
            XSSFCellStyle estiloPorcentajeBordeTrue = (XSSFCellStyle)workbook.CreateCellStyle();

            estiloPorcentajeBordeTrue.WrapText = true;
            estiloPorcentajeBordeTrue.VerticalAlignment = VerticalAlignment.Center;
            estiloPorcentajeBordeTrue.Alignment = HorizontalAlignment.Center;
            estiloPorcentajeBordeTrue.FillPattern = FillPattern.NoFill;

            XSSFFont fontPorcentajeBordeTrue = (XSSFFont)workbook.CreateFont();
            fontPorcentajeBordeTrue.SetColor(colorFontBlack);

            estiloPorcentajeBordeTrue.FillForegroundColorColor = colorToFillEmpty;
            estiloPorcentajeBordeTrue.BorderTop = BorderStyle.Thin;
            estiloPorcentajeBordeTrue.BorderRight = BorderStyle.Thin;
            estiloPorcentajeBordeTrue.BorderLeft = BorderStyle.Thin;
            estiloPorcentajeBordeTrue.BorderBottom = BorderStyle.Thin;

            estiloPorcentajeBordeTrue.SetDataFormat(workbook.CreateDataFormat().GetFormat("0.00%"));
            #endregion

            for (int i = 0; i < data.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                row.Height = -1;
                for (int j = 0; j < data[i].cells.Count; j++)
                {
                    ICell cell = row.CreateCell(j);
                    cell.CellStyle.WrapText = true;

                    if (data[i].cells[j].fill == true)
                    {
                        if (data[i].cells[j].border == true)
                        {
                            cell.CellStyle = estiloEncabezadoFillBordeTrue;
                        }
                        else
                        {
                            cell.CellStyle = estiloEncabezadoFillBordeFalse;
                        }
                    }
                    else
                    {
                        if (data[i].cells[j].border == true)
                        {
                            cell.CellStyle = estiloNoFillBordeTrue;
                        }
                        else
                        {
                            cell.CellStyle = estiloNoFillBordeFalse;
                        }
                    }

                    cell.SetCellValue(data[i].cells[j].text);

                    if (data[i].cells[j].text.Contains("%"))
                    {
                        double number;
                        if (Double.TryParse(data[i].cells[j].text.Split('%')[0], out number))
                        {
                            cell.SetCellValue((Convert.ToDouble(data[i].cells[j].text.Split('%')[0])) / 100);
                            cell.CellStyle = estiloPorcentajeBordeTrue;
                        }
                    }

                    var colSpan = data[i].cells[j].colSpan;
                    var rowSpan = data[i].cells[j].rowSpan;

                    if (data[i].cells[j].colSpan > 0)
                    {
                        NPOI.SS.Util.CellRangeAddress mr = new NPOI.SS.Util.CellRangeAddress(i + 1, i + 1, j, j + (colSpan - 1));
                        sheet.AddMergedRegion(mr);
                    }
                    if (data[i].cells[j].rowSpan > 0)
                    {
                        NPOI.SS.Util.CellRangeAddress mr = new NPOI.SS.Util.CellRangeAddress(i + 1, (i + 1) + (rowSpan - 1), j, j);
                        sheet.AddMergedRegion(mr);
                    }

                    cell.Row.Height = (short)(row.Height * (rowSpan * 2));
                }
            }

            var dataLast = data.LastOrDefault();

            for (int i = 0; i < data.LastOrDefault().cells.Count; i++)
            {
                if (dataLast.cells[i].autoWidthFit)
                {
                    sheet.AutoSizeColumn(i);
                }
            }

            return sheet;
            #endregion
        }

        public MemoryStream CreateExcelFileCadenaProductiva(Controller controller, List<excelSheetDTO> Sheets, string fileName)
        {

            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            foreach (var i in Sheets)
            {
                CreateExcelSheetCadenaProductiva(workbook, i.Sheet, i.name);
            }

            using (var exportData = new MemoryStream())
            {
                controller.Response.Clear();
                workbook.Write(exportData);

                controller.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                controller.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName + ".xlsx"));
                controller.Response.BinaryWrite(exportData.ToArray());

                controller.Response.End();
                return exportData;
            }
        }
        public ISheet CreateExcelSheetCadenaProductiva(IWorkbook workbook, List<excelRowDTO> data, string name)
        {
            var arrData = data.ToArray();
            #region TAB
            var sheet = workbook.CreateSheet(name);
            //Crear tipo de letra
            var font = (XSSFFont)workbook.CreateFont();
            var colorToFillEmpty = new XSSFColor(Color.Empty);
            var colorFontWhite = new XSSFColor(Color.White);
            var colorFontBlack = new XSSFColor(Color.Black);
            var colorToFillOrange = new XSSFColor(Color.FromArgb(255, 192, 0));
            var colorToFillGreen = new XSSFColor(Color.FromArgb(59, 226, 61));
            var style = (XSSFCellStyle)workbook.CreateCellStyle();
            var swEstilo = new SwitchClass<XSSFCellStyle>
                    {
                       { 0,  () => style },
                       { 1,  () => caso1(style) },
                       { 2,  () => caso2(style) },
                       { 3 , () => caso3(style) },
                       { 4 , () => caso4(style) },
                       { 5 , () => caso5(style) },
                       { 6 , () => caso6(style) },
                       { 7 , () => caso7(style) },
                       { 8 , () => caso8(style) },
                       { 9 , () => caso9(style) },
                       { 10, () => caso10(style) },
                       { 11, () => caso11(style, colorToFillGreen) },
                       { 12, () => caso12(style, colorToFillOrange) },
                       { 13, () => caso13(style, colorToFillOrange) },
                       { 14, () => caso14(style, colorToFillGreen) },
                    };
            //make a header row
            var rowHeader = sheet.CreateRow(0);
            var cell = rowHeader.CreateCell(0);
            var row = sheet.CreateRow(0);
            var maxCol = arrData.Select(x => x.cells.Count).Max();
            for (int j = 0; j < maxCol; j++)
            {
                sheet.SetColumnWidth(j, 7500);
                cell = rowHeader.CreateCell(j);
                cell.SetCellValue(FillEmpty(j + 1));
            }
            //loops through data
            for (int i = 0; i < arrData.Length; i++)
            {
                row = sheet.CreateRow(i + 1);
                row.Height = -1;
                for (int j = 0; j < arrData[i].cells.Count; j++)
                {
                    cell = row.CreateCell(j);
                    cell.CellStyle.WrapText = true;
                    font = (XSSFFont)workbook.CreateFont();
                    style = (XSSFCellStyle)workbook.CreateCellStyle();
                    style.WrapText = true;
                    style.VerticalAlignment = VerticalAlignment.Center;
                    style.Alignment = HorizontalAlignment.Center;
                    if (arrData[i].cells[j].fill)
                    {
                        font.Boldweight = (short)FontBoldWeight.Bold;
                        font.SetColor(colorFontBlack);
                        style.SetFont(font);
                    }
                    else
                    {
                        style.FillPattern = FillPattern.NoFill;
                        font.SetColor(colorFontBlack);
                        style.FillForegroundColorColor = colorToFillEmpty;
                    }
                    if (arrData[i].cells[j].border)
                    {
                        style.BorderTop = BorderStyle.Thin;
                        style.BorderRight = BorderStyle.Thin;
                        style.BorderLeft = BorderStyle.Thin;
                        style.BorderBottom = BorderStyle.Thin;
                    }
                    style = swEstilo.Execute(arrData[i].cells[j].borderType);
                    //Set defined style
                    cell.CellStyle = style;
                    switch (arrData[i].cells[j].formatType)
                    {
                        case 1:
                            {
                                cell.SetCellType(CellType.Numeric);
                                cell.CellStyle.Alignment = HorizontalAlignment.Right;
                                cell.SetCellValue(double.Parse(data[i].cells[j].text));
                                break;
                            }
                        default: { cell.SetCellValue(data[i].cells[j].text); break; }
                    }
                    var rowSpan = arrData[i].cells[j].rowSpan;
                    if (arrData[i].cells[j].colSpan > 0)
                    {
                        NPOI.SS.Util.CellRangeAddress mr = new NPOI.SS.Util.CellRangeAddress(i + 1, i + 1, j, j + (arrData[i].cells[j].colSpan - 1));
                        sheet.AddMergedRegion(mr);
                    }
                    if (arrData[i].cells[j].rowSpan > 0)
                    {
                        NPOI.SS.Util.CellRangeAddress mr = new NPOI.SS.Util.CellRangeAddress(i + 1, (i + 1) + (rowSpan - 1), j, j);
                        sheet.AddMergedRegion(mr);
                    }
                    cell.Row.Height = (short)(row.Height * (rowSpan * 2));
                    //---------------------------
                }
            }
            var dataLast = arrData.LastOrDefault();
            for (int i = 0; i < data.LastOrDefault().cells.Count; i++)
                if (dataLast.cells[i].autoWidthFit)
                    sheet.AutoSizeColumn(i);
            //sheet.ProtectSheet("Liberación");
            return sheet;
            #endregion
        }

        public MemoryStream CreateExcelFileEncuestas(Controller controller, List<excelSheetDTO> Sheets, string fileName)
        {

            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            foreach (var i in Sheets)
            {
                CreateExcelSheetEncuestas(workbook, i.Sheet, i.name);
            }

            using (var exportData = new MemoryStream())
            {
                controller.Response.Clear();
                workbook.Write(exportData);

                controller.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                controller.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName + ".xlsx"));
                controller.Response.BinaryWrite(exportData.ToArray());

                controller.Response.End();
                return exportData;
            }
        }
        public ISheet CreateExcelSheetEncuestas(IWorkbook workbook, List<excelRowDTO> data, string name)
        {
            #region TAB
            ISheet sheet = workbook.CreateSheet(name);
            //make a header row
            IRow rowHeader = sheet.CreateRow(0);
            var maxCol = data.Select(x => x.cells.Count).Max();
            for (int j = 0; j < maxCol; j++)
            {
                sheet.SetColumnWidth(j, 7500);
                ICell cell = rowHeader.CreateCell(j);
                cell.SetCellValue(FillEmpty(j + 1));
            }
            //Crear tipo de letra
            XSSFFont font = (XSSFFont)workbook.CreateFont();
            XSSFColor colorToFillGray = new XSSFColor(Color.LightGray);
            XSSFColor colorToFillEmpty = new XSSFColor(Color.Empty);
            XSSFColor colorFontWhite = new XSSFColor(Color.White);
            XSSFColor colorFontBlack = new XSSFColor(Color.Black);
            //loops through data
            for (int i = 0; i < data.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                row.Height = -1;
                for (int j = 0; j < data[i].cells.Count; j++)
                {
                    //Create Style
                    XSSFCellStyle styleHeader = (XSSFCellStyle)workbook.CreateCellStyle();

                    styleHeader.WrapText = true;
                    styleHeader.VerticalAlignment = VerticalAlignment.Center;
                    styleHeader.Alignment = HorizontalAlignment.Center;
                    ICell cell = row.CreateCell(j);
                    cell.CellStyle.WrapText = true;


                    //Crea background-color
                    if (data[i].cells[j].fill == true)
                    {
                        styleHeader.SetFillForegroundColor(colorToFillGray);
                        styleHeader.FillPattern = FillPattern.SolidForeground;

                        font.SetColor(colorFontBlack);
                        font.Boldweight = (short)FontBoldWeight.Bold;
                        styleHeader.SetFont(font);

                        //Set border
                        if (data[i].cells[j].border == true)
                        {
                            styleHeader.BorderTop = BorderStyle.Thin;
                            styleHeader.BorderRight = BorderStyle.Thin;
                            styleHeader.BorderLeft = BorderStyle.Thin;
                            styleHeader.BorderBottom = BorderStyle.Thin;
                        }
                        else
                        {
                            styleHeader.BorderTop = BorderStyle.None;
                            styleHeader.BorderRight = BorderStyle.None;
                            styleHeader.BorderLeft = BorderStyle.None;
                            styleHeader.BorderBottom = BorderStyle.None;
                        }
                        //Set defined style
                        cell.CellStyle = styleHeader;
                    }
                    else
                    {
                        //Create Style
                        XSSFCellStyle style = (XSSFCellStyle)workbook.CreateCellStyle();

                        style.WrapText = true;
                        style.VerticalAlignment = VerticalAlignment.Center;
                        style.Alignment = HorizontalAlignment.Center;
                        if (data[i].cells[j].textAlignLeft == true)
                        {
                            style.Alignment = HorizontalAlignment.Left;
                        }
                        style.FillPattern = FillPattern.NoFill;
                        font.SetColor(colorFontBlack);
                        style.FillForegroundColorColor = colorToFillEmpty;

                        //Set border
                        if (data[i].cells[j].border == true)
                        {
                            style.BorderTop = BorderStyle.Thin;
                            style.BorderRight = BorderStyle.Thin;
                            style.BorderLeft = BorderStyle.Thin;
                            style.BorderBottom = BorderStyle.Thin;
                        }
                        else
                        {
                            style.BorderTop = BorderStyle.None;
                            style.BorderRight = BorderStyle.None;
                            style.BorderLeft = BorderStyle.None;
                            style.BorderBottom = BorderStyle.None;
                        }
                        //Set defined style
                        cell.CellStyle = style;
                    }
                    switch (data[i].cells[j].formatType)
                    {
                        case 1:
                            {
                                cell.SetCellValue(double.Parse(data[i].cells[j].text));
                                cell.SetCellType(CellType.Numeric);
                                break;
                            }
                        default: { cell.SetCellValue(data[i].cells[j].text); break; }
                    }
                    //Set text
                    //cell.SetCellValue(data[i].cells[j].text);
                    var colSpan = data[i].cells[j].colSpan;
                    var rowSpan = data[i].cells[j].rowSpan;
                    if (data[i].cells[j].colSpan > 0)
                    {
                        NPOI.SS.Util.CellRangeAddress mr = new NPOI.SS.Util.CellRangeAddress(i + 1, i + 1, j, j + (colSpan - 1));
                        sheet.AddMergedRegion(mr);
                    }
                    if (data[i].cells[j].rowSpan > 0)
                    {

                        NPOI.SS.Util.CellRangeAddress mr = new NPOI.SS.Util.CellRangeAddress(i + 1, (i + 1) + (rowSpan - 1), j, j);
                        sheet.AddMergedRegion(mr);
                    }
                    cell.Row.Height = (short)(row.Height * (rowSpan * 2));
                    //---------------------------
                }
            }
            var dataLast = data.LastOrDefault();
            for (int i = 0; i < data.LastOrDefault().cells.Count; i++)
            {
                if (dataLast.cells[i].autoWidthFit)
                {
                    sheet.AutoSizeColumn(i);
                }
            }

            return sheet;
            #endregion
        }

        public MemoryStream FillExcelFileEncuestasMes(Controller controller, List<excelSheetDTO> Sheets, string fileName, int mes, string year, string ruta)
        {
            //FileStream fs = new FileStream(@"C:\Users\Oscar Valencia\Desktop\Plantilla Encuestas Mensual.xlsx", FileMode.Open, FileAccess.Read);
            //FileStream fs = new FileStream(@"C:\Proyecto\SIGOPLAN\ENCUESTAS\Plantilla Encuestas Mensual.xlsx", FileMode.Open, FileAccess.Read);
            FileStream fs = new FileStream(ruta + "Plantilla Encuestas Mensual.xlsx", FileMode.Open, FileAccess.Read);
            IWorkbook workbook;
            workbook = new XSSFWorkbook(fs);
            int ind = 0;
            foreach (var i in Sheets)
            {
                FillExcelSheetEncuestasMes(workbook, i.Sheet, i.name, ind, mes, year);
                ind++;
            }

            using (var exportData = new MemoryStream())
            {
                controller.Response.Clear();
                workbook.Write(exportData);

                controller.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                controller.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName + ".xlsx"));
                controller.Response.BinaryWrite(exportData.ToArray());

                controller.Response.End();

                return exportData;
            }
        }
        public ISheet FillExcelSheetEncuestasMes(IWorkbook workbook, List<excelRowDTO> data, string name, int ind, int mes, string year)
        {
            ISheet sheet = workbook.GetSheetAt(ind);
            IRow headerRow = sheet.GetRow(5);
            IEnumerator rows = sheet.GetRowEnumerator();

            int colCount = headerRow.LastCellNum;
            int rowCount = sheet.LastRowNum;

            #region Header Trimestre
            switch (mes)
            {
                case 1:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Enero " + year);
                    break;
                case 2:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Febrero " + year);
                    break;
                case 3:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Marzo " + year);
                    break;
                case 4:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Abril " + year);
                    break;
                case 5:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Mayo " + year);
                    break;
                case 6:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Junio " + year);
                    break;
                case 7:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Julio " + year);
                    break;
                case 8:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Agosto " + year);
                    break;
                case 9:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Septiembre " + year);
                    break;
                case 10:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Octubre " + year);
                    break;
                case 11:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Noviembre " + year);
                    break;
                case 12:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Diciembre " + year);
                    break;
                default:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Enero " + year);
                    break;
            }
            #endregion

            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data[i].cells.Count; j++)
                {
                    if (j <= 6)
                    {
                        if (data[i].cells[j].text != "")
                        {
                            double n;
                            bool isNumeric = Double.TryParse(data[i].cells[j].text, out n);
                            if (isNumeric)
                            {
                                if (j > 4)
                                {
                                    sheet.GetRow(i + 6).GetCell(j).SetCellType(CellType.Numeric);
                                    sheet.GetRow(i + 6).GetCell(j).SetCellValue(Double.Parse(data[i].cells[j].text));
                                }
                                else
                                {
                                    sheet.GetRow(i + 6).GetCell(j).SetCellType(CellType.Numeric);
                                    sheet.GetRow(i + 6).GetCell(j).SetCellValue((Double.Parse(data[i].cells[j].text)) / 100);
                                    sheet.GetRow(i + 6).GetCell(j).CellStyle.DataFormat = 9;
                                }
                            }
                            else
                            {
                                sheet.GetRow(i + 6).GetCell(j).SetCellType(CellType.String);
                                sheet.GetRow(i + 6).GetCell(j).SetCellValue(data[i].cells[j].text);
                            }
                        }
                        else
                        {
                            sheet.GetRow(i + 6).GetCell(j).SetCellValue(0);
                        }
                    }
                }
            }

            for (var i = 6; i < rowCount; i++)
            {
                if (sheet.GetRow(i).GetCell(0).StringCellValue == "")
                {
                    sheet.GetRow(i).ZeroHeight = true;
                }
            }

            sheet.ForceFormulaRecalculation = true;

            return sheet;
        }

        public MemoryStream FillExcelFileEncuestas(Controller controller, List<excelSheetDTO> Sheets, string fileName, int tri, string year, string ruta)
        {
            //FileStream fs = new FileStream(@"C:\Users\Oscar Valencia\Desktop\Plantilla Encuestas Trimestre.xlsx", FileMode.Open, FileAccess.Read);
            //FileStream fs = new FileStream(@"C:\Proyecto\SIGOPLAN\ENCUESTAS\Plantilla Encuestas Trimestre.xlsx", FileMode.Open, FileAccess.Read);
            FileStream fs = new FileStream(ruta + "Plantilla Encuestas Trimestre.xlsx", FileMode.Open, FileAccess.Read);
            IWorkbook workbook;
            workbook = new XSSFWorkbook(fs);
            int ind = 0;
            foreach (var i in Sheets)
            {
                FillExcelSheetEncuestas(workbook, i.Sheet, i.name, ind, tri, year);
                ind++;
            }

            using (var exportData = new MemoryStream())
            {
                controller.Response.Clear();
                workbook.Write(exportData);

                controller.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                controller.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName + ".xlsx"));
                controller.Response.BinaryWrite(exportData.ToArray());

                controller.Response.End();

                return exportData;
            }
        }
        public ISheet FillExcelSheetEncuestas(IWorkbook workbook, List<excelRowDTO> data, string name, int ind, int tri, string year)
        {
            ISheet sheet = workbook.GetSheetAt(ind);
            IRow headerRow = sheet.GetRow(5);
            IEnumerator rows = sheet.GetRowEnumerator();

            int colCount = headerRow.LastCellNum;
            int rowCount = sheet.LastRowNum;

            //for (var i = 6; i < rowCount; i++)
            //{
            //    IRow filaNueva = sheet.CreateRow(i);
            //    for (var j = 0; j < 15; j++)
            //    {
            //        ICell celdaNueva = filaNueva.CreateCell(j);
            //        if (j > 1 && j <= 13)
            //        {
            //            celdaNueva.SetCellType(CellType.Numeric);
            //            celdaNueva.CellStyle.DataFormat = 9;
            //        }

            //        if (j == 14)
            //        {
            //            celdaNueva.SetCellType(CellType.Numeric);

            //        }

            //        if (j == 15)
            //        {
            //            celdaNueva.SetCellType(CellType.String);
            //        }
            //    }
            //}

            #region Header Trimestre
            switch (tri)
            {
                case 1:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Enero-Marzo " + year);
                    sheet.GetRow(5).GetCell(5).SetCellValue("Ene");
                    sheet.GetRow(5).GetCell(6).SetCellValue("Feb");
                    sheet.GetRow(5).GetCell(7).SetCellValue("Mar");
                    sheet.GetRow(5).GetCell(8).SetCellValue("Ene");
                    sheet.GetRow(5).GetCell(9).SetCellValue("Feb");
                    sheet.GetRow(5).GetCell(10).SetCellValue("Mar");
                    sheet.GetRow(5).GetCell(11).SetCellValue("Ene");
                    sheet.GetRow(5).GetCell(12).SetCellValue("Feb");
                    sheet.GetRow(5).GetCell(13).SetCellValue("Mar");
                    break;
                case 2:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Abril-Junio " + year);
                    sheet.GetRow(5).GetCell(5).SetCellValue("Abr");
                    sheet.GetRow(5).GetCell(6).SetCellValue("May");
                    sheet.GetRow(5).GetCell(7).SetCellValue("Jun");
                    sheet.GetRow(5).GetCell(8).SetCellValue("Abr");
                    sheet.GetRow(5).GetCell(9).SetCellValue("May");
                    sheet.GetRow(5).GetCell(10).SetCellValue("Jun");
                    sheet.GetRow(5).GetCell(11).SetCellValue("Abr");
                    sheet.GetRow(5).GetCell(12).SetCellValue("May");
                    sheet.GetRow(5).GetCell(13).SetCellValue("Jun");
                    break;
                case 3:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Julio-Septiembre " + year);
                    sheet.GetRow(5).GetCell(5).SetCellValue("Jul");
                    sheet.GetRow(5).GetCell(6).SetCellValue("Ago");
                    sheet.GetRow(5).GetCell(7).SetCellValue("Sep");
                    sheet.GetRow(5).GetCell(8).SetCellValue("Jul");
                    sheet.GetRow(5).GetCell(9).SetCellValue("Ago");
                    sheet.GetRow(5).GetCell(10).SetCellValue("Sep");
                    sheet.GetRow(5).GetCell(11).SetCellValue("Jul");
                    sheet.GetRow(5).GetCell(12).SetCellValue("Ago");
                    sheet.GetRow(5).GetCell(13).SetCellValue("Sep");
                    break;
                case 4:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Octubre-Diciembre " + year);
                    sheet.GetRow(5).GetCell(5).SetCellValue("Oct");
                    sheet.GetRow(5).GetCell(6).SetCellValue("Nov");
                    sheet.GetRow(5).GetCell(7).SetCellValue("Dic");
                    sheet.GetRow(5).GetCell(8).SetCellValue("Oct");
                    sheet.GetRow(5).GetCell(9).SetCellValue("Nov");
                    sheet.GetRow(5).GetCell(10).SetCellValue("Dic");
                    sheet.GetRow(5).GetCell(11).SetCellValue("Oct");
                    sheet.GetRow(5).GetCell(12).SetCellValue("Nov");
                    sheet.GetRow(5).GetCell(13).SetCellValue("Dic");
                    break;
                default:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Enero-Marzo " + year);
                    sheet.GetRow(5).GetCell(5).SetCellValue("Ene");
                    sheet.GetRow(5).GetCell(6).SetCellValue("Feb");
                    sheet.GetRow(5).GetCell(7).SetCellValue("Mar");
                    sheet.GetRow(5).GetCell(8).SetCellValue("Ene");
                    sheet.GetRow(5).GetCell(9).SetCellValue("Feb");
                    sheet.GetRow(5).GetCell(10).SetCellValue("Mar");
                    sheet.GetRow(5).GetCell(11).SetCellValue("Ene");
                    sheet.GetRow(5).GetCell(12).SetCellValue("Feb");
                    sheet.GetRow(5).GetCell(13).SetCellValue("Mar");
                    break;
            }
            #endregion

            //if (data.Count > 15)
            //{
            //    //var rowPromedio = sheet.GetRow(21).ToList();
            //    //var rowFinal = sheet.GetRow(22).ToList();
            //    IRow filaMolde = sheet.GetRow(5);

            //    var dif = data.Count - 15;
            //    var maxCol = data.Select(x => x.cells.Count).Max();
            //    for (int i = 0; i < dif; i++)
            //    {
            //        IRow filaNueva = sheet.CreateRow(21 + (i));

            //        for (int j = 0; j < maxCol; j++)
            //        {
            //            ICell cell = filaNueva.CreateCell(j);

            //            //if (j > 1 && j <= 13)
            //            //{
            //            //    cell.SetCellType(CellType.Numeric);
            //            //    cell.CellStyle.DataFormat = workbook.CreateDataFormat().GetFormat("0%");
            //            //}

            //            //if (j == 14)
            //            //{
            //            //    cell.SetCellType(CellType.Numeric);

            //            //}

            //            //if (j == 15)
            //            //{
            //            //    cell.SetCellType(CellType.String);
            //            //}

            //            //ICell cellMolde = filaMolde.Cells[j];
            //            //cell.CellStyle.CloneStyleFrom(cellMolde.CellStyle);
            //        }

            //        //int sour = 6;
            //        //int dest = 7;
            //        //copyRow(workbook, sheet, sour, dest);
            //    }
            //    //int rowCount = sheet.LastRowNum;
            //    //var ultimaFila = sheet.GetRow(rowCount - 1);
            //    //ultimaFila.RowStyle.BorderBottom = BorderStyle.Thin;
            //}
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data[i].cells.Count; j++)
                {
                    if (j <= 15 && j != 2 && j != 3 && j != 4)
                    {
                        if (data[i].cells[j].text != "")
                        {
                            double n;
                            bool isNumeric = Double.TryParse(data[i].cells[j].text, out n);
                            if (isNumeric)
                            {
                                if (j > 13)
                                {
                                    sheet.GetRow(i + 6).GetCell(j).SetCellType(CellType.Numeric);
                                    sheet.GetRow(i + 6).GetCell(j).SetCellValue(Double.Parse(data[i].cells[j].text));
                                }
                                else
                                {
                                    sheet.GetRow(i + 6).GetCell(j).SetCellType(CellType.Numeric);
                                    sheet.GetRow(i + 6).GetCell(j).SetCellValue((Double.Parse(data[i].cells[j].text)) / 100);
                                    sheet.GetRow(i + 6).GetCell(j).CellStyle.DataFormat = 9;
                                }
                            }
                            else
                            {
                                sheet.GetRow(i + 6).GetCell(j).SetCellType(CellType.String);
                                sheet.GetRow(i + 6).GetCell(j).SetCellValue(data[i].cells[j].text);
                            }
                        }
                        else
                        {
                            sheet.GetRow(i + 6).GetCell(j).SetCellValue(0);
                        }
                    }
                }
            }

            for (var i = 6; i < rowCount; i++)
            {
                if (sheet.GetRow(i).GetCell(0).StringCellValue == "")
                {
                    sheet.GetRow(i).ZeroHeight = true;
                }
            }

            sheet.ForceFormulaRecalculation = true;

            return sheet;
        }

        public MemoryStream FillExcelFileEncuestasSem(Controller controller, List<excelSheetDTO> Sheets, string fileName, int sem, string year, string ruta)
        {
            //FileStream fs = new FileStream(@"C:\Users\Oscar Valencia\Desktop\Plantilla Encuestas Semestre.xlsx", FileMode.Open, FileAccess.Read);
            //FileStream fs = new FileStream(@"C:\Proyecto\SIGOPLAN\ENCUESTAS\Plantilla Encuestas Semestre.xlsx", FileMode.Open, FileAccess.Read);
            FileStream fs = new FileStream(ruta + "Plantilla Encuestas Semestre.xlsx", FileMode.Open, FileAccess.Read);
            IWorkbook workbook;
            workbook = new XSSFWorkbook(fs);
            int ind = 0;
            foreach (var i in Sheets)
            {
                FillExcelSheetEncuestasSem(workbook, i.Sheet, i.name, ind, sem, year);
                ind++;
            }

            using (var exportData = new MemoryStream())
            {
                controller.Response.Clear();
                workbook.Write(exportData);

                controller.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                controller.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName + ".xlsx"));
                controller.Response.BinaryWrite(exportData.ToArray());

                controller.Response.End();

                return exportData;
            }
        }
        public ISheet FillExcelSheetEncuestasSem(IWorkbook workbook, List<excelRowDTO> data, string name, int ind, int sem, string year)
        {
            ISheet sheet = workbook.GetSheetAt(ind);
            IRow headerRow = sheet.GetRow(5);
            IEnumerator rows = sheet.GetRowEnumerator();

            int colCount = headerRow.LastCellNum;
            int rowCount = sheet.LastRowNum;

            #region Header Trimestre
            switch (sem)
            {
                case 1:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Enero-Junio " + year);

                    sheet.GetRow(5).GetCell(5).SetCellValue("Ene");
                    sheet.GetRow(5).GetCell(6).SetCellValue("Feb");
                    sheet.GetRow(5).GetCell(7).SetCellValue("Mar");
                    sheet.GetRow(5).GetCell(8).SetCellValue("Abr");
                    sheet.GetRow(5).GetCell(9).SetCellValue("May");
                    sheet.GetRow(5).GetCell(10).SetCellValue("Junio");

                    sheet.GetRow(5).GetCell(11).SetCellValue("Ene");
                    sheet.GetRow(5).GetCell(12).SetCellValue("Feb");
                    sheet.GetRow(5).GetCell(13).SetCellValue("Mar");
                    sheet.GetRow(5).GetCell(14).SetCellValue("Abr");
                    sheet.GetRow(5).GetCell(15).SetCellValue("May");
                    sheet.GetRow(5).GetCell(16).SetCellValue("Junio");

                    sheet.GetRow(5).GetCell(17).SetCellValue("Ene");
                    sheet.GetRow(5).GetCell(18).SetCellValue("Feb");
                    sheet.GetRow(5).GetCell(19).SetCellValue("Mar");
                    sheet.GetRow(5).GetCell(20).SetCellValue("Abr");
                    sheet.GetRow(5).GetCell(21).SetCellValue("May");
                    sheet.GetRow(5).GetCell(22).SetCellValue("Junio");
                    break;
                case 2:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Julio-Diciembre " + year);

                    sheet.GetRow(5).GetCell(5).SetCellValue("Jul");
                    sheet.GetRow(5).GetCell(6).SetCellValue("Ago");
                    sheet.GetRow(5).GetCell(7).SetCellValue("Sep");
                    sheet.GetRow(5).GetCell(8).SetCellValue("Oct");
                    sheet.GetRow(5).GetCell(9).SetCellValue("Nov");
                    sheet.GetRow(5).GetCell(10).SetCellValue("Dic");

                    sheet.GetRow(5).GetCell(11).SetCellValue("Jul");
                    sheet.GetRow(5).GetCell(12).SetCellValue("Ago");
                    sheet.GetRow(5).GetCell(13).SetCellValue("Sep");
                    sheet.GetRow(5).GetCell(14).SetCellValue("Oct");
                    sheet.GetRow(5).GetCell(15).SetCellValue("Nov");
                    sheet.GetRow(5).GetCell(16).SetCellValue("Dic");

                    sheet.GetRow(5).GetCell(17).SetCellValue("Jul");
                    sheet.GetRow(5).GetCell(18).SetCellValue("Ago");
                    sheet.GetRow(5).GetCell(19).SetCellValue("Sep");
                    sheet.GetRow(5).GetCell(20).SetCellValue("Oct");
                    sheet.GetRow(5).GetCell(21).SetCellValue("Nov");
                    sheet.GetRow(5).GetCell(22).SetCellValue("Dic");
                    break;
                default:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Enero-Junio " + year);

                    sheet.GetRow(5).GetCell(5).SetCellValue("Ene");
                    sheet.GetRow(5).GetCell(6).SetCellValue("Feb");
                    sheet.GetRow(5).GetCell(7).SetCellValue("Mar");
                    sheet.GetRow(5).GetCell(8).SetCellValue("Abr");
                    sheet.GetRow(5).GetCell(9).SetCellValue("May");
                    sheet.GetRow(5).GetCell(10).SetCellValue("Junio");

                    sheet.GetRow(5).GetCell(11).SetCellValue("Ene");
                    sheet.GetRow(5).GetCell(12).SetCellValue("Feb");
                    sheet.GetRow(5).GetCell(13).SetCellValue("Mar");
                    sheet.GetRow(5).GetCell(14).SetCellValue("Abr");
                    sheet.GetRow(5).GetCell(15).SetCellValue("May");
                    sheet.GetRow(5).GetCell(16).SetCellValue("Junio");

                    sheet.GetRow(5).GetCell(17).SetCellValue("Ene");
                    sheet.GetRow(5).GetCell(18).SetCellValue("Feb");
                    sheet.GetRow(5).GetCell(19).SetCellValue("Mar");
                    sheet.GetRow(5).GetCell(20).SetCellValue("Abr");
                    sheet.GetRow(5).GetCell(21).SetCellValue("May");
                    sheet.GetRow(5).GetCell(22).SetCellValue("Junio");
                    break;
            }
            #endregion

            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data[i].cells.Count; j++)
                {
                    if (j <= 24 && j != 2 && j != 3 && j != 4)
                    {
                        if (data[i].cells[j].text != "")
                        {
                            double n;
                            bool isNumeric = Double.TryParse(data[i].cells[j].text, out n);
                            if (isNumeric)
                            {
                                if (j > 22)
                                {
                                    sheet.GetRow(i + 6).GetCell(j).SetCellType(CellType.Numeric);
                                    sheet.GetRow(i + 6).GetCell(j).SetCellValue(Double.Parse(data[i].cells[j].text));
                                }
                                else
                                {
                                    sheet.GetRow(i + 6).GetCell(j).SetCellType(CellType.Numeric);
                                    sheet.GetRow(i + 6).GetCell(j).SetCellValue((Double.Parse(data[i].cells[j].text)) / 100);
                                    sheet.GetRow(i + 6).GetCell(j).CellStyle.DataFormat = 9;
                                }
                            }
                            else
                            {
                                sheet.GetRow(i + 6).GetCell(j).SetCellType(CellType.String);
                                sheet.GetRow(i + 6).GetCell(j).SetCellValue(data[i].cells[j].text);
                            }
                        }
                        else
                        {
                            sheet.GetRow(i + 6).GetCell(j).SetCellValue(0);
                        }
                    }
                }
            }

            for (var i = 6; i < rowCount; i++)
            {
                if (sheet.GetRow(i).GetCell(0).StringCellValue == "")
                {
                    sheet.GetRow(i).ZeroHeight = true;
                }
            }

            sheet.ForceFormulaRecalculation = true;

            return sheet;
        }

        public MemoryStream FillExcelFileEncuestasYear(Controller controller, List<excelSheetDTO> Sheets, string fileName, string year, string ruta)
        {
            //FileStream fs = new FileStream(@"C:\Users\Oscar Valencia\Desktop\Plantilla Encuestas Año.xlsx", FileMode.Open, FileAccess.Read);
            //FileStream fs = new FileStream(@"C:\Proyecto\SIGOPLAN\ENCUESTAS\Plantilla Encuestas Año.xlsx", FileMode.Open, FileAccess.Read);
            FileStream fs = new FileStream(ruta + "Plantilla Encuestas Año.xlsx", FileMode.Open, FileAccess.Read);

            IWorkbook workbook;
            workbook = new XSSFWorkbook(fs);
            int ind = 0;
            foreach (var i in Sheets)
            {
                FillExcelSheetEncuestasYear(workbook, i.Sheet, i.name, ind, year);
                ind++;
            }

            using (var exportData = new MemoryStream())
            {
                controller.Response.Clear();
                workbook.Write(exportData);

                controller.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                controller.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName + ".xlsx"));
                controller.Response.BinaryWrite(exportData.ToArray());

                controller.Response.End();

                return exportData;
            }
        }
        public ISheet FillExcelSheetEncuestasYear(IWorkbook workbook, List<excelRowDTO> data, string name, int ind, string year)
        {
            ISheet sheet = workbook.GetSheetAt(ind);
            IRow headerRow = sheet.GetRow(5);
            IEnumerator rows = sheet.GetRowEnumerator();

            int colCount = headerRow.LastCellNum;
            int rowCount = sheet.LastRowNum;

            var maxCol = data.Select(x => x.cells.Count).Max();

            sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: " + year);

            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data[i].cells.Count; j++)
                {
                    if (j <= 19 && j != 2 && j != 3 && j != 4 && j != 5)
                    {
                        if (data[i].cells[j].text != "")
                        {
                            double n;
                            bool isNumeric = Double.TryParse(data[i].cells[j].text, out n);
                            if (isNumeric)
                            {
                                if (j > 17)
                                {
                                    sheet.GetRow(i + 6).GetCell(j).SetCellValue(Double.Parse(data[i].cells[j].text));
                                }
                                else
                                {
                                    sheet.GetRow(i + 6).GetCell(j).SetCellValue((Double.Parse(data[i].cells[j].text)) / 100);
                                }
                            }
                            else
                            {
                                sheet.GetRow(i + 6).GetCell(j).SetCellValue(data[i].cells[j].text);
                            }
                        }
                        else
                        {
                            sheet.GetRow(i + 6).GetCell(j).SetCellValue(0);
                        }
                    }
                }
            }

            for (var i = 6; i < rowCount; i++)
            {
                if (sheet.GetRow(i).GetCell(0).StringCellValue == "")
                {
                    sheet.GetRow(i).ZeroHeight = true;
                }
            }

            sheet.ForceFormulaRecalculation = true;

            return sheet;
        }

        public MemoryStream FillExcelFileEncuestaIndividualMes(Controller controller, List<excelSheetDTO> Sheets, string fileName, int mes, string year, string departamento, string ruta)
        {
            //FileStream fs = new FileStream(@"C:\Users\Oscar Valencia\Desktop\Plantilla Encuesta Individual Mensual.xlsx", FileMode.Open, FileAccess.Read);
            //FileStream fs = new FileStream(@"C:\Proyecto\SIGOPLAN\ENCUESTAS\Plantilla Encuesta Individual Mensual.xlsx", FileMode.Open, FileAccess.Read);
            FileStream fs = new FileStream(ruta + "Plantilla Encuesta Individual Mensual.xlsx", FileMode.Open, FileAccess.Read);

            IWorkbook workbook;
            workbook = new XSSFWorkbook(fs);
            int ind = 0;
            int indMax = workbook.NumberOfSheets - 1;
            foreach (var i in Sheets)
            {
                if (i.name == null || i.name == "")
                {
                    workbook.RemoveSheetAt(ind);
                    indMax--;
                }
                else
                {
                    FillExcelSheetEncuestaIndividualMes(workbook, i.Sheet, i.name, ind, mes, year, departamento);
                    ind++;
                }
            }
            var hojasTotal = workbook.NumberOfSheets;

            for (int i = indMax; i > (ind - 1); i--)
            {
                workbook.RemoveSheetAt(i);
            }

            using (var exportData = new MemoryStream())
            {
                controller.Response.Clear();
                workbook.Write(exportData);

                controller.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                controller.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName + ".xlsx"));
                controller.Response.BinaryWrite(exportData.ToArray());

                controller.Response.End();

                return exportData;
            }
        }
        public ISheet FillExcelSheetEncuestaIndividualMes(IWorkbook workbook, List<excelRowDTO> data, string name, int ind, int mes, string year, string departamento)
        {
            ISheet sheet = workbook.GetSheetAt(ind);
            IRow headerRow = sheet.GetRow(5);
            IEnumerator rows = sheet.GetRowEnumerator();

            int colCount = headerRow.LastCellNum;
            int rowCount = sheet.LastRowNum;
            sheet.GetRow(4).GetCell(0).SetCellValue("Departamento: " + departamento);

            var encuesta = (name != null && name != "") ? "Encuesta: " + name : "";
            sheet.GetRow(4).GetCell(2).SetCellValue(encuesta);

            #region Header Trimestre
            switch (mes)
            {
                case 1:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Enero " + year);
                    break;
                case 2:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Febrero " + year);
                    break;
                case 3:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Marzo " + year);
                    break;
                case 4:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Abril " + year);
                    break;
                case 5:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Mayo " + year);
                    break;
                case 6:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Junio " + year);
                    break;
                case 7:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Julio " + year);
                    break;
                case 8:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Agosto " + year);
                    break;
                case 9:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Septiembre " + year);
                    break;
                case 10:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Octubre " + year);
                    break;
                case 11:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Noviembre " + year);
                    break;
                case 12:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Diciembre " + year);
                    break;
                default:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Enero " + year);
                    break;
            }
            #endregion

            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data[i].cells.Count; j++)
                {
                    if (j <= 5)
                    {
                        if (data[i].cells[j].text != "")
                        {
                            double n;
                            bool isNumeric = Double.TryParse(data[i].cells[j].text, out n);
                            if (isNumeric)
                            {
                                if (j > 3)
                                {
                                    sheet.GetRow(i + 7).GetCell(j).SetCellType(CellType.Numeric);
                                    sheet.GetRow(i + 7).GetCell(j).SetCellValue(Double.Parse(data[i].cells[j].text));
                                }
                                else
                                {
                                    sheet.GetRow(i + 7).GetCell(j).SetCellType(CellType.Numeric);
                                    sheet.GetRow(i + 7).GetCell(j).SetCellValue((Double.Parse(data[i].cells[j].text)) / 100);
                                    sheet.GetRow(i + 7).GetCell(j).CellStyle.DataFormat = 9;
                                }
                            }
                            else
                            {
                                sheet.GetRow(i + 7).GetCell(j).SetCellType(CellType.String);
                                sheet.GetRow(i + 7).GetCell(j).SetCellValue(data[i].cells[j].text);
                            }
                        }
                        else
                        {
                            sheet.GetRow(i + 7).GetCell(j).SetCellValue(0);
                        }
                    }
                }
            }

            for (var i = 7; i < rowCount; i++)
            {
                if (sheet.GetRow(i).GetCell(0).StringCellValue == "")
                {
                    sheet.GetRow(i).ZeroHeight = true;
                }
            }

            sheet.ForceFormulaRecalculation = true;

            return sheet;
        }

        public MemoryStream FillExcelFileEncuestaIndividualTri(Controller controller, List<excelSheetDTO> Sheets, string fileName, int tri, string year, string departamento, string ruta)
        {
            //FileStream fs = new FileStream(@"C:\Users\Oscar Valencia\Desktop\Plantilla Encuesta Individual Trimestre.xlsx", FileMode.Open, FileAccess.Read);
            //FileStream fs = new FileStream(@"C:\Proyecto\SIGOPLAN\ENCUESTAS\Plantilla Encuesta Individual Trimestre.xlsx", FileMode.Open, FileAccess.Read);
            FileStream fs = new FileStream(ruta + "Plantilla Encuesta Individual Trimestre.xlsx", FileMode.Open, FileAccess.Read);

            IWorkbook workbook;
            workbook = new XSSFWorkbook(fs);
            int ind = 0;
            int indMax = workbook.NumberOfSheets - 1;
            foreach (var i in Sheets)
            {
                if (i.name == null || i.name == "")
                {
                    workbook.RemoveSheetAt(ind);
                    indMax--;
                }
                else
                {
                    FillExcelSheetEncuestaIndividualTri(workbook, i.Sheet, i.name, ind, tri, year, departamento);
                    ind++;
                }
            }

            for (int i = indMax; i > (ind - 1); i--)
            {
                workbook.RemoveSheetAt(i);
            }

            using (var exportData = new MemoryStream())
            {
                controller.Response.Clear();
                workbook.Write(exportData);

                controller.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                controller.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName + ".xlsx"));
                controller.Response.BinaryWrite(exportData.ToArray());

                controller.Response.End();

                return exportData;
            }
        }
        public ISheet FillExcelSheetEncuestaIndividualTri(IWorkbook workbook, List<excelRowDTO> data, string name, int ind, int tri, string year, string departamento)
        {
            ISheet sheet = workbook.GetSheetAt(ind);
            IRow headerRow = sheet.GetRow(5);
            IEnumerator rows = sheet.GetRowEnumerator();

            int colCount = headerRow.LastCellNum;
            int rowCount = sheet.LastRowNum;

            sheet.GetRow(4).GetCell(0).SetCellValue("Departamento: " + departamento);

            var encuesta = (name != null && name != "") ? "Encuesta: " + name : "";
            sheet.GetRow(4).GetCell(2).SetCellValue(encuesta);

            #region Header Trimestre
            switch (tri)
            {
                case 1:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Enero-Marzo " + year);
                    sheet.GetRow(6).GetCell(4).SetCellValue("Ene");
                    sheet.GetRow(6).GetCell(5).SetCellValue("Feb");
                    sheet.GetRow(6).GetCell(6).SetCellValue("Mar");
                    sheet.GetRow(6).GetCell(7).SetCellValue("Ene");
                    sheet.GetRow(6).GetCell(8).SetCellValue("Feb");
                    sheet.GetRow(6).GetCell(9).SetCellValue("Mar");
                    sheet.GetRow(6).GetCell(10).SetCellValue("Ene");
                    sheet.GetRow(6).GetCell(11).SetCellValue("Feb");
                    sheet.GetRow(6).GetCell(12).SetCellValue("Mar");
                    break;
                case 2:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Abril-Junio " + year);
                    sheet.GetRow(6).GetCell(4).SetCellValue("Abr");
                    sheet.GetRow(6).GetCell(5).SetCellValue("May");
                    sheet.GetRow(6).GetCell(6).SetCellValue("Jun");
                    sheet.GetRow(6).GetCell(7).SetCellValue("Abr");
                    sheet.GetRow(6).GetCell(8).SetCellValue("May");
                    sheet.GetRow(6).GetCell(9).SetCellValue("Jun");
                    sheet.GetRow(6).GetCell(10).SetCellValue("Abr");
                    sheet.GetRow(6).GetCell(11).SetCellValue("May");
                    sheet.GetRow(6).GetCell(12).SetCellValue("Jun");
                    break;
                case 3:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Julio-Septiembre " + year);
                    sheet.GetRow(6).GetCell(4).SetCellValue("Jul");
                    sheet.GetRow(6).GetCell(5).SetCellValue("Ago");
                    sheet.GetRow(6).GetCell(6).SetCellValue("Sep");
                    sheet.GetRow(6).GetCell(7).SetCellValue("Jul");
                    sheet.GetRow(6).GetCell(8).SetCellValue("Ago");
                    sheet.GetRow(6).GetCell(9).SetCellValue("Sep");
                    sheet.GetRow(6).GetCell(10).SetCellValue("Jul");
                    sheet.GetRow(6).GetCell(11).SetCellValue("Ago");
                    sheet.GetRow(6).GetCell(12).SetCellValue("Sep");
                    break;
                case 4:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Octubre-Diciembre " + year);
                    sheet.GetRow(6).GetCell(4).SetCellValue("Oct");
                    sheet.GetRow(6).GetCell(5).SetCellValue("Nov");
                    sheet.GetRow(6).GetCell(6).SetCellValue("Dic");
                    sheet.GetRow(6).GetCell(7).SetCellValue("Oct");
                    sheet.GetRow(6).GetCell(8).SetCellValue("Nov");
                    sheet.GetRow(6).GetCell(9).SetCellValue("Dic");
                    sheet.GetRow(6).GetCell(10).SetCellValue("Oct");
                    sheet.GetRow(6).GetCell(11).SetCellValue("Nov");
                    sheet.GetRow(6).GetCell(12).SetCellValue("Dic");
                    break;
                default:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Enero-Marzo " + year);
                    sheet.GetRow(6).GetCell(4).SetCellValue("Ene");
                    sheet.GetRow(6).GetCell(5).SetCellValue("Feb");
                    sheet.GetRow(6).GetCell(6).SetCellValue("Mar");
                    sheet.GetRow(6).GetCell(7).SetCellValue("Ene");
                    sheet.GetRow(6).GetCell(8).SetCellValue("Feb");
                    sheet.GetRow(6).GetCell(9).SetCellValue("Mar");
                    sheet.GetRow(6).GetCell(10).SetCellValue("Ene");
                    sheet.GetRow(6).GetCell(11).SetCellValue("Feb");
                    sheet.GetRow(6).GetCell(12).SetCellValue("Mar");
                    break;
            }
            #endregion

            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data[i].cells.Count; j++)
                {
                    if (j <= 14 && j != 1 && j != 2 && j != 3)
                    {
                        if (data[i].cells[j].text != "")
                        {
                            double n;
                            bool isNumeric = Double.TryParse(data[i].cells[j].text, out n);
                            if (isNumeric)
                            {
                                if (j > 12)
                                {
                                    sheet.GetRow(i + 7).GetCell(j).SetCellType(CellType.Numeric);
                                    sheet.GetRow(i + 7).GetCell(j).SetCellValue(Double.Parse(data[i].cells[j].text));
                                }
                                else
                                {
                                    sheet.GetRow(i + 7).GetCell(j).SetCellType(CellType.Numeric);
                                    sheet.GetRow(i + 7).GetCell(j).SetCellValue((Double.Parse(data[i].cells[j].text)) / 100);
                                    sheet.GetRow(i + 7).GetCell(j).CellStyle.DataFormat = 9;
                                }
                            }
                            else
                            {
                                sheet.GetRow(i + 7).GetCell(j).SetCellType(CellType.String);
                                sheet.GetRow(i + 7).GetCell(j).SetCellValue(data[i].cells[j].text);
                            }
                        }
                        else
                        {
                            sheet.GetRow(i + 7).GetCell(j).SetCellValue(0);
                        }
                    }
                }
            }

            for (var i = 7; i < rowCount; i++)
            {
                if (sheet.GetRow(i).GetCell(0).StringCellValue == "")
                {
                    sheet.GetRow(i).ZeroHeight = true;
                }
            }

            sheet.ForceFormulaRecalculation = true;

            return sheet;
        }

        public MemoryStream FillExcelFileEncuestaIndividualSem(Controller controller, List<excelSheetDTO> Sheets, string fileName, int sem, string year, string departamento, string ruta)
        {
            //FileStream fs = new FileStream(@"C:\Users\Oscar Valencia\Desktop\Plantilla Encuesta Individual Semestre.xlsx", FileMode.Open, FileAccess.Read);
            //FileStream fs = new FileStream(@"C:\Proyecto\SIGOPLAN\ENCUESTAS\Plantilla Encuesta Individual Semestre.xlsx", FileMode.Open, FileAccess.Read);
            FileStream fs = new FileStream(ruta + "Plantilla Encuesta Individual Semestre.xlsx", FileMode.Open, FileAccess.Read);

            IWorkbook workbook;
            workbook = new XSSFWorkbook(fs);
            int ind = 0;
            int indMax = workbook.NumberOfSheets - 1;
            foreach (var i in Sheets)
            {
                if (i.name == null || i.name == "")
                {
                    workbook.RemoveSheetAt(ind);
                    indMax--;
                }
                else
                {
                    FillExcelSheetEncuestaIndividualSem(workbook, i.Sheet, i.name, ind, sem, year, departamento);
                    ind++;
                }
            }

            for (int i = indMax; i > (ind - 1); i--)
            {
                workbook.RemoveSheetAt(i);
            }

            using (var exportData = new MemoryStream())
            {
                controller.Response.Clear();
                workbook.Write(exportData);

                controller.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                controller.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName + ".xlsx"));
                controller.Response.BinaryWrite(exportData.ToArray());

                controller.Response.End();

                return exportData;
            }
        }
        public ISheet FillExcelSheetEncuestaIndividualSem(IWorkbook workbook, List<excelRowDTO> data, string name, int ind, int sem, string year, string departamento)
        {
            ISheet sheet = workbook.GetSheetAt(ind);
            IRow headerRow = sheet.GetRow(5);
            IEnumerator rows = sheet.GetRowEnumerator();

            int colCount = headerRow.LastCellNum;
            int rowCount = sheet.LastRowNum;

            sheet.GetRow(4).GetCell(0).SetCellValue("Departamento: " + departamento);

            var encuesta = (name != null && name != "") ? "Encuesta: " + name : "";
            sheet.GetRow(4).GetCell(2).SetCellValue(encuesta);

            #region Header Trimestre
            switch (sem)
            {
                case 1:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Enero-Junio " + year);

                    sheet.GetRow(6).GetCell(4).SetCellValue("Ene");
                    sheet.GetRow(6).GetCell(5).SetCellValue("Feb");
                    sheet.GetRow(6).GetCell(6).SetCellValue("Mar");
                    sheet.GetRow(6).GetCell(7).SetCellValue("Abr");
                    sheet.GetRow(6).GetCell(8).SetCellValue("May");
                    sheet.GetRow(6).GetCell(9).SetCellValue("Junio");

                    sheet.GetRow(6).GetCell(10).SetCellValue("Ene");
                    sheet.GetRow(6).GetCell(11).SetCellValue("Feb");
                    sheet.GetRow(6).GetCell(12).SetCellValue("Mar");
                    sheet.GetRow(6).GetCell(13).SetCellValue("Abr");
                    sheet.GetRow(6).GetCell(14).SetCellValue("May");
                    sheet.GetRow(6).GetCell(15).SetCellValue("Junio");

                    sheet.GetRow(6).GetCell(16).SetCellValue("Ene");
                    sheet.GetRow(6).GetCell(17).SetCellValue("Feb");
                    sheet.GetRow(6).GetCell(18).SetCellValue("Mar");
                    sheet.GetRow(6).GetCell(19).SetCellValue("Abr");
                    sheet.GetRow(6).GetCell(20).SetCellValue("May");
                    sheet.GetRow(6).GetCell(21).SetCellValue("Junio");
                    break;
                case 2:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Julio-Diciembre " + year);

                    sheet.GetRow(6).GetCell(4).SetCellValue("Jul");
                    sheet.GetRow(6).GetCell(5).SetCellValue("Ago");
                    sheet.GetRow(6).GetCell(6).SetCellValue("Sep");
                    sheet.GetRow(6).GetCell(7).SetCellValue("Oct");
                    sheet.GetRow(6).GetCell(8).SetCellValue("Nov");
                    sheet.GetRow(6).GetCell(9).SetCellValue("Dic");

                    sheet.GetRow(6).GetCell(10).SetCellValue("Jul");
                    sheet.GetRow(6).GetCell(11).SetCellValue("Ago");
                    sheet.GetRow(6).GetCell(12).SetCellValue("Sep");
                    sheet.GetRow(6).GetCell(13).SetCellValue("Oct");
                    sheet.GetRow(6).GetCell(14).SetCellValue("Nov");
                    sheet.GetRow(6).GetCell(15).SetCellValue("Dic");

                    sheet.GetRow(6).GetCell(16).SetCellValue("Jul");
                    sheet.GetRow(6).GetCell(17).SetCellValue("Ago");
                    sheet.GetRow(6).GetCell(18).SetCellValue("Sep");
                    sheet.GetRow(6).GetCell(19).SetCellValue("Oct");
                    sheet.GetRow(6).GetCell(20).SetCellValue("Nov");
                    sheet.GetRow(6).GetCell(21).SetCellValue("Dic");
                    break;
                default:
                    sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: Enero-Junio " + year);

                    sheet.GetRow(6).GetCell(4).SetCellValue("Ene");
                    sheet.GetRow(6).GetCell(5).SetCellValue("Feb");
                    sheet.GetRow(6).GetCell(6).SetCellValue("Mar");
                    sheet.GetRow(6).GetCell(7).SetCellValue("Abr");
                    sheet.GetRow(6).GetCell(8).SetCellValue("May");
                    sheet.GetRow(6).GetCell(9).SetCellValue("Junio");

                    sheet.GetRow(6).GetCell(10).SetCellValue("Ene");
                    sheet.GetRow(6).GetCell(11).SetCellValue("Feb");
                    sheet.GetRow(6).GetCell(12).SetCellValue("Mar");
                    sheet.GetRow(6).GetCell(13).SetCellValue("Abr");
                    sheet.GetRow(6).GetCell(14).SetCellValue("May");
                    sheet.GetRow(6).GetCell(15).SetCellValue("Junio");

                    sheet.GetRow(6).GetCell(16).SetCellValue("Ene");
                    sheet.GetRow(6).GetCell(17).SetCellValue("Feb");
                    sheet.GetRow(6).GetCell(18).SetCellValue("Mar");
                    sheet.GetRow(6).GetCell(19).SetCellValue("Abr");
                    sheet.GetRow(6).GetCell(20).SetCellValue("May");
                    sheet.GetRow(6).GetCell(21).SetCellValue("Junio");
                    break;
            }
            #endregion

            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data[i].cells.Count; j++)
                {
                    if (j <= 23 && j != 1 && j != 2 && j != 3)
                    {
                        if (data[i].cells[j].text != "")
                        {
                            double n;
                            bool isNumeric = Double.TryParse(data[i].cells[j].text, out n);
                            if (isNumeric)
                            {
                                if (j > 21)
                                {
                                    sheet.GetRow(i + 7).GetCell(j).SetCellType(CellType.Numeric);
                                    sheet.GetRow(i + 7).GetCell(j).SetCellValue(Double.Parse(data[i].cells[j].text));
                                }
                                else
                                {
                                    sheet.GetRow(i + 7).GetCell(j).SetCellType(CellType.Numeric);
                                    sheet.GetRow(i + 7).GetCell(j).SetCellValue((Double.Parse(data[i].cells[j].text)) / 100);
                                    sheet.GetRow(i + 7).GetCell(j).CellStyle.DataFormat = 9;
                                }
                            }
                            else
                            {
                                sheet.GetRow(i + 7).GetCell(j).SetCellType(CellType.String);
                                sheet.GetRow(i + 7).GetCell(j).SetCellValue(data[i].cells[j].text);
                            }
                        }
                        else
                        {
                            sheet.GetRow(i + 7).GetCell(j).SetCellValue(0);
                        }
                    }
                }
            }

            for (var i = 7; i < rowCount; i++)
            {
                if (sheet.GetRow(i).GetCell(0).StringCellValue == "")
                {
                    sheet.GetRow(i).ZeroHeight = true;
                }
            }

            sheet.ForceFormulaRecalculation = true;

            return sheet;
        }

        public MemoryStream FillExcelFileEncuestaIndividualYear(Controller controller, List<excelSheetDTO> Sheets, string fileName, string year, string departamento, string ruta)
        {
            //FileStream fs = new FileStream(@"C:\Users\Oscar Valencia\Desktop\Plantilla Encuesta Individual Año.xlsx", FileMode.Open, FileAccess.Read);
            //FileStream fs = new FileStream(@"C:\Proyecto\SIGOPLAN\ENCUESTAS\Plantilla Encuesta Individual Año.xlsx", FileMode.Open, FileAccess.Read);
            FileStream fs = new FileStream(ruta + "Plantilla Encuesta Individual Año.xlsx", FileMode.Open, FileAccess.Read);
            IWorkbook workbook;
            workbook = new XSSFWorkbook(fs);
            int ind = 0;
            int indMax = workbook.NumberOfSheets - 1;
            foreach (var i in Sheets)
            {
                if (i.name == null || i.name == "")
                {
                    workbook.RemoveSheetAt(ind);
                    indMax--;
                }
                else
                {
                    FillExcelSheetEncuestaIndividualYear(workbook, i.Sheet, i.name, ind, year, departamento);
                    ind++;
                }
            }

            for (int i = indMax; i > (ind - 1); i--)
            {
                workbook.RemoveSheetAt(i);
            }

            using (var exportData = new MemoryStream())
            {
                controller.Response.Clear();
                workbook.Write(exportData);

                controller.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                controller.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName + ".xlsx"));
                controller.Response.BinaryWrite(exportData.ToArray());

                controller.Response.End();

                return exportData;
            }
        }
        public ISheet FillExcelSheetEncuestaIndividualYear(IWorkbook workbook, List<excelRowDTO> data, string name, int ind, string year, string departamento)
        {
            ISheet sheet = workbook.GetSheetAt(ind);
            IRow headerRow = sheet.GetRow(5);
            IEnumerator rows = sheet.GetRowEnumerator();

            int colCount = headerRow.LastCellNum;
            int rowCount = sheet.LastRowNum;

            var maxCol = data.Select(x => x.cells.Count).Max();

            sheet.GetRow(3).GetCell(0).SetCellValue("Periodo: " + year);
            sheet.GetRow(4).GetCell(0).SetCellValue("Departamento: " + departamento);

            var encuesta = (name != null && name != "") ? "Encuesta: " + name : "";
            sheet.GetRow(4).GetCell(2).SetCellValue(encuesta);

            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data[i].cells.Count; j++)
                {
                    if (j <= 18 && j != 1 && j != 2 && j != 3 && j != 4)
                    {
                        if (data[i].cells[j].text != "")
                        {
                            double n;
                            bool isNumeric = Double.TryParse(data[i].cells[j].text, out n);
                            if (isNumeric)
                            {
                                if (j > 16)
                                {
                                    sheet.GetRow(i + 7).GetCell(j).SetCellValue(Double.Parse(data[i].cells[j].text));
                                }
                                else
                                {
                                    sheet.GetRow(i + 7).GetCell(j).SetCellValue((Double.Parse(data[i].cells[j].text)) / 100);
                                }
                            }
                            else
                            {
                                sheet.GetRow(i + 7).GetCell(j).SetCellValue(data[i].cells[j].text);
                            }
                        }
                        else
                        {
                            sheet.GetRow(i + 7).GetCell(j).SetCellValue(0);
                        }
                    }
                }
            }

            for (var i = 7; i < rowCount; i++)
            {
                if (sheet.GetRow(i).GetCell(0).StringCellValue == "")
                {
                    sheet.GetRow(i).ZeroHeight = true;
                }
            }

            sheet.ForceFormulaRecalculation = true;

            return sheet;
        }

        public string FillEmpty(int no)
        {
            var newe = "";
            for (int i = 1; i <= no; i++)
            {
                newe += " ";
            }
            return newe;
        }

        private static void copyRow(IWorkbook workbook, ISheet worksheet, int sourceRowNum, int destinationRowNum)
        {
            // Get the source / new row
            IRow newRow = worksheet.GetRow(destinationRowNum);
            IRow sourceRow = worksheet.GetRow(sourceRowNum);

            // If the row exist in destination, push down all rows by 1 else create a new row
            if (newRow != null)
            {
                worksheet.ShiftRows(destinationRowNum, worksheet.LastRowNum, 1);
            }
            else
            {
                newRow = worksheet.CreateRow(destinationRowNum);
            }

            // Loop through source columns to add to new row
            for (int i = 0; i < sourceRow.LastCellNum; i++)
            {
                // Grab a copy of the old/new cell
                ICell oldCell = sourceRow.GetCell(i);
                ICell newCell = newRow.CreateCell(i);

                // If the old cell is null jump to next cell
                if (oldCell == null)
                {
                    newCell = null;
                    continue;
                }

                // Copy style from old cell and apply to new cell
                XSSFCellStyle newCellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                newCellStyle.CloneStyleFrom(oldCell.CellStyle);
                ;
                newCell.CellStyle = newCellStyle;

                // If there is a cell comment, copy
                if (oldCell.CellComment != null)
                {
                    newCell.CellComment = oldCell.CellComment;
                }

                // If there is a cell hyperlink, copy
                if (oldCell.Hyperlink != null)
                {
                    newCell.Hyperlink = oldCell.Hyperlink;
                }

                // Set the cell data type
                newCell.SetCellType(oldCell.CellType);

                // Set the cell data value
                switch (oldCell.CellType)
                {
                    case CellType.Blank:
                        newCell.SetCellValue(oldCell.StringCellValue);
                        break;
                    case CellType.Boolean:
                        newCell.SetCellValue(oldCell.BooleanCellValue);
                        break;
                    case CellType.Error:
                        newCell.SetCellErrorValue(oldCell.ErrorCellValue);
                        break;
                    case CellType.Formula:
                        newCell.SetCellFormula(oldCell.CellFormula);
                        break;
                    case CellType.Numeric:
                        newCell.SetCellValue(oldCell.NumericCellValue);
                        break;
                    case CellType.String:
                        newCell.SetCellValue(oldCell.RichStringCellValue);
                        break;
                }
            }

            //// If there are are any merged regions in the source row, copy to new row
            //for (int i = 0; i < worksheet.NumMergedRegions; i++)
            //{
            //    CellRangeAddress cellRangeAddress = worksheet.GetMergedRegion(i);
            //    if (cellRangeAddress.getFirstRow() == sourceRow.RowNum)
            //    {
            //        CellRangeAddress newCellRangeAddress = new CellRangeAddress(newRow.RowNum,
            //                (newRow.RowNum +
            //                        (cellRangeAddress.getLastRow() - cellRangeAddress.getFirstRow()
            //                                )),
            //                cellRangeAddress.getFirstColumn(),
            //                cellRangeAddress.getLastColumn());
            //        worksheet.AddMergedRegion(newCellRangeAddress);
            //    }
            //}
        }
        #region Estilos por propiedad borderType
        XSSFCellStyle caso1(XSSFCellStyle style)
        {
            style.BorderTop = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            return style;
        }
        XSSFCellStyle caso2(XSSFCellStyle style)
        {
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            return style;
        }
        XSSFCellStyle caso3(XSSFCellStyle style)
        {
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.None;
            style.BorderBottom = BorderStyle.None;
            return style;
        }
        XSSFCellStyle caso4(XSSFCellStyle style)
        {
            style.BorderTop = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.None;
            style.BorderRight = BorderStyle.None;
            return style;
        }
        XSSFCellStyle caso5(XSSFCellStyle style)
        {
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.None;
            style.BorderRight = BorderStyle.None;
            return style;
        }
        XSSFCellStyle caso6(XSSFCellStyle style)
        {
            style.BorderTop = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.None;
            style.BorderLeft = BorderStyle.None;
            return style;
        }
        XSSFCellStyle caso7(XSSFCellStyle style)
        {
            style.BorderBottom = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            return style;
        }
        XSSFCellStyle caso8(XSSFCellStyle style)
        {
            style.BorderRight = BorderStyle.Thin;
            return style;
        }
        XSSFCellStyle caso9(XSSFCellStyle style)
        {
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            return style;
        }
        XSSFCellStyle caso10(XSSFCellStyle style)
        {
            style.BorderTop = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            return style;
        }
        XSSFCellStyle caso11(XSSFCellStyle style, XSSFColor colorToFillGreen)
        {
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.SetFillForegroundColor(colorToFillGreen);
            style.FillPattern = FillPattern.SolidForeground;
            return style;
        }
        XSSFCellStyle caso12(XSSFCellStyle style, XSSFColor colorToFillOrange)
        {
            style.SetFillForegroundColor(colorToFillOrange);
            style.FillPattern = FillPattern.SolidForeground;
            return style;
        }
        XSSFCellStyle caso13(XSSFCellStyle style, XSSFColor colorToFillOrange)
        {
            style.BorderTop = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.SetFillForegroundColor(colorToFillOrange);
            style.FillPattern = FillPattern.SolidForeground;
            return style;
        }
        XSSFCellStyle caso14(XSSFCellStyle style, XSSFColor colorToFillGreen)
        {
            style.BorderTop = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.None;
            style.BorderLeft = BorderStyle.None;
            style.SetFillForegroundColor(colorToFillGreen);
            style.FillPattern = FillPattern.SolidForeground;
            return style;
        }
        #endregion
        public static string NombreValidoArchivo(string name)
        {
            string invalidChars = Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidReStr = string.Format(@"[{0}]+", invalidChars);
            string replace = Regex.Replace(name, invalidReStr, "_").Replace(";", "").Replace(",", "");
            return replace;
        }


        /// <summary>
        /// Retorna el nombre de la columna dependiente del valor proporcionado.
        /// </summary>
        /// <param name="columnNumber"></param>
        /// <returns></returns>
        public static string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }
    }
}
