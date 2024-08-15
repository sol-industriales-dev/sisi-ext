using Core.DTO.RecursosHumanos;
using Core.Entity.RecursosHumanos.Reportes;
using Data.Factory.RecursosHumanos.ReportesRH;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.RecursosHumanos.Layout
{
    public class LayoutRHController : BaseController
    {
        ReportesRHFactoryServices reportesRHFactoryServices;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            reportesRHFactoryServices = new ReportesRHFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        // GET: Administrativo/LayaoutRH
        public ActionResult AltasRH()
        {
            return View();
        }
        public ActionResult IncidenciasRH()
        {
            return View();
        }

        public ActionResult fillTableLayoutAltasRH(List<string> cc, DateTime fechaInicio, DateTime fechaFin)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var listResult = reportesRHFactoryServices.getReportesRHService().getListaEmpleados(cc, fechaInicio, fechaFin);
                Session["ListaLayoutAltasRHDTO"] = listResult.ToList();
                result.Add("current", 1);
                result.Add("rowCount", 1);
                result.Add("total", listResult.Count());
                var temp = listResult.OrderBy(x => x.EMP_CC).ThenBy(x => x.EMP_ALTA);
                result.Add("rows", temp);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult fillTableLayoutIncidenciasRH(List<string> cc, DateTime fechaInicio, DateTime fechaFin)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var listResult = reportesRHFactoryServices.getReportesRHService().getListaEmpleadosIncidencias(cc, fechaInicio, fechaFin);
                Session["ListaLayoutIncidenciasRHDTO"] = listResult.ToList();
                result.Add("current", 1);
                result.Add("rowCount", 1);
                result.Add("total", listResult.Count());
                var temp = listResult.Select(x => new
                {
                    EMP_CC = x.EMP_CC,
                    EMP_CLAVE = x.EMP_CLAVE,
                    EMP_FALTAS = x.EMP_FALTAS,
                    EMP_NOM = x.EMP_NOM.Replace("/", " ")
                })
                .OrderBy(x => x.EMP_CC)
                .ThenBy(x => x.EMP_CLAVE);
                result.Add("rows", temp);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportInformacion(List<string> empleados)
        {
            var result = new Dictionary<string, object>();
            try
            {

                Session["setListEmpleados"] = empleados.ToList();

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public FileResult getFileDownloadAltas()
        {
            var memStream = new MemoryStream();
            var streamWriter = new StreamWriter(memStream);
            //string Cadena = "EMP_TRAB|EMP_ALTA|EMP_NOM|EMP_RFC|EMP_CURP|EMP_CC|EMP_DP|EMP_SM|EMP_SEXO|EMP_NAC_EF|EMP_NAC_FECHA|EMP_ULTIMO_REINGRESO|EMP_ULTIMA_BAJA|EMP_AGUINALDO|EMP_VACACIONES|EMP_NOMINA|EMP_IMSS_TIPO|EMP_SUCURSAL|EMP_CL|EMP_PUESTO|EMP_PUESTO_DESCRIPCION|EMP_IMSS|EMP_NSS|EMP_FSUELDO|EMP_SUELDO|EMP_SDI|EMP_FP1|EMP_FP1_TARJETA|EMP_MAIL|EMP_ESTADO_CIVIL|EMP_DIR_VIA|EMP_DIR_CALLE|EMP_DIR_NO|EMP_DIR_INTERIOR|EMP_DIR_COLONIA|EMP_DIR_CP|EMP_DIR_MUNICIPIO|EMP_DIR_POBLACION|EMP_DIR_ESTADO|EMP_DIR_TELEFONO|EMP_DIR_CELULAR|EMP_DIR_RECADOS|EMP_CAMISA|EMP_ZAPATOS|EMP_PANTALON\n";
            string Cadena = "EMP_TRAB|EMP_ALTA|EMP_NOM|EMP_RFC|EMP_CURP|EMP_CC|EMP_DP|EMP_SM|EMP_SEXO|EMP_NAC_EF|EMP_NAC_FECHA|EMP_ULTIMO_REINGRESO|EMP_AGUINALDO|EMP_VACACIONES|EMP_NOMINA|EMP_IMSS_TIPO|EMP_SUCURSAL|EMP_CL|EMP_TURNO|EMP_PUESTO|EMP_PUESTO_DESCRIPCION|EMP_NSS|EMP_UMF|EMP_FSUELDO|EMP_SUELDO|EMP_SUELDO1|EMP_SUELDO2|EMP_SDI|EMP_DIR_CALLE|EMP_DIR_NO|EMP_DIR_COLONIA|EMP_DIR_CP|EMP_DIR_MUNICIPIO|EMP_DIR_POBLACION|EMP_DIR_ESTADO|EMP_DIR_TELEFONO|EMP_DIR_CELULAR|NOM_BENEF|PARENT_BENEF|BENEF_NAC_FECHA|EMP_MAIL|EMP_CFDI_CP";
            streamWriter.WriteLine(Cadena);
            List<LayoutAltasRHDTO> listaIF = (List<LayoutAltasRHDTO>)Session["ListaLayoutAltasRHDTO"];

            var EmpleadosSEleccionados = (List<string>)Session["setListEmpleados"];
            DataTable dtLayout = new DataTable();

            var EncabezadoTabla = Cadena.Split('|');

            for (int i = 0; i < EncabezadoTabla.Length; i++)
            {
                dtLayout.Columns.Add(EncabezadoTabla[i]);
            }

            foreach (var listaRow in listaIF.Where(x => EmpleadosSEleccionados.Contains(x.EMP_TRAB)))
            {
                int dias = listaRow.dias;
                int diasSueldo = 0;
                switch (dias)
                {

                    case 1:
                        diasSueldo = 7;
                        break;
                    case 2:
                        diasSueldo = 10;
                        break;
                    case 3:
                        diasSueldo = 14;
                        break;
                    case 4:
                        diasSueldo = 15;
                        break;
                    case 5:
                        diasSueldo = 30;
                        break;
                    default:
                        break;
                }
                if (diasSueldo != 0)
                {


                    double saldodiario = Math.Round((listaRow.EMP_SUELDO == "0" ? 0 : Convert.ToDouble(listaRow.EMP_SUELDO) / diasSueldo), 2);
                    double SDI = Math.Round(saldodiario * 1.0493, 2);// getDatosTabal(1);

                    dtLayout.Rows.Add(

                        listaRow.EMP_TRAB,
                        listaRow.EMP_ALTA,
                        listaRow.EMP_NOM,
                        listaRow.EMP_RFC,
                        listaRow.EMP_CURP,
                        listaRow.EMP_CC,
                        listaRow.EMP_DP,
                        listaRow.EMP_SM,
                        listaRow.EMP_SEXO,
                        listaRow.EMP_NAC_EF,
                        listaRow.EMP_NAC_FECHA,
                        listaRow.EMP_ULTIMO_REINGRESO,
                        listaRow.EMP_AGUINALDO,
                        listaRow.EMP_VACACIONES,
                        listaRow.EMP_NOMINA,
                        listaRow.EMP_IMSS_TIPO,
                        listaRow.EMP_SUCURSAL,
                        listaRow.EMP_CL,
                        listaRow.EMP_TURNO,
                        listaRow.EMP_PUESTO,
                        listaRow.EMP_PUESTO_DESCRIPCION,
                        listaRow.EMP_NSS,
                        listaRow.EMP_UMF,
                        listaRow.EMP_FSUELDO,
                        saldodiario,
                        listaRow.EMP_SUELDO1,
                        listaRow.EMP_SUELDO2,
                        SDI,
                        listaRow.EMP_DIR_CALLE,
                        listaRow.EMP_DIR_NO,
                        listaRow.EMP_DIR_COLONIA,
                        listaRow.EMP_DIR_CP,
                        listaRow.EMP_DIR_MUNICIPIO,
                        listaRow.EMP_DIR_POBLACION,
                        listaRow.EMP_DIR_ESTADO,
                        listaRow.EMP_DIR_TELEFONO,
                        listaRow.EMP_DIR_CELULAR,
                        listaRow.NOM_BENEF,
                        listaRow.PARENT_BENEF,
                        listaRow.BENEF_NAC_FECHA,
                        listaRow.EMP_MAIL,
                        listaRow.AP

                   );
                }
            }
            WriteExcelWithNPOI(dtLayout, "xls");

            return null;
        }
        public FileResult getFileDownloadBajas()
        {
            var memStream = new MemoryStream();
            var streamWriter = new StreamWriter(memStream);

            // string Cadena = "EMP_TRAB|EMP_NOM|EMP_NSS|EMP_BAJA_FECHA|CB_CLAVE|CB_DESCRIPCION";
            // string Cadena = "EMP_TRAB|EMP_ULTIMA_BAJA|EMP_ANTIGUEDAD|EMP_PUESTO|EMP_PUESTO_DESCRIPCION";

            string Cadena = "EMP_TRAB|EMP_NOM|EMP_DP|EMP_NSS|EMP_ALTA|EMP_ULTIMA_BAJA|EMP_PUESTO|EMP_PUESTO_DESCRIPCION|CB_CLAVE|CB_DESCRIPCION|EMPRESA";
            streamWriter.WriteLine(Cadena);
            List<RepBajasDTO> listaIF = (List<RepBajasDTO>)Session["ListaLayoutBajasRHDTO"];

            var EmpleadosSEleccionados = (List<string>)Session["setListEmpleados"];
            DataTable dtLayout = new DataTable();

            var EncabezadoTabla = Cadena.Split('|');

            for (int i = 0; i < EncabezadoTabla.Length; i++)
            {
                dtLayout.Columns.Add(EncabezadoTabla[i]);
            }

            foreach (var listaRow in listaIF.Where(x => EmpleadosSEleccionados.Contains(x.empleadoID)))
            {
                tblRH_LayautBajaEmpleados LayautBajaEmpleadosObj = new tblRH_LayautBajaEmpleados();

                LayautBajaEmpleadosObj.empleadoID = listaRow.empleadoID;
                LayautBajaEmpleadosObj.fechaCaptura = DateTime.Now;
                LayautBajaEmpleadosObj.usuarioCaptura = getUsuario().id;


                reportesRHFactoryServices.getReportesRHService().setUsuariosBaja(LayautBajaEmpleadosObj);

                dtLayout.Rows.Add(

                    listaRow.empleadoID,
                    listaRow.empleado,
                    listaRow.cC,
                    listaRow.nss,
                    listaRow.fechaAltaStr,
                    listaRow.fechaBajaStr,
                    listaRow.puestoID,
                    listaRow.puestosDes,
                    GetCBClave(listaRow.concepto),
                    listaRow.concepto,
                    listaRow.regPatronal
               );

            }
            WriteExcelWithNPOIBajas(dtLayout, "xls");



            return null;
        }

        public int GetCBClave(string concepto)
        {
            var list = reportesRHFactoryServices.getReportesRHService().getListaConceptosBaja().Where(x => x.Text.Equals(concepto)).FirstOrDefault().Value;


            return Convert.ToInt32(list);
        }

        public string GetCBDescripcion(int concepto)
        {
            switch (concepto)
            {
                case 1:
                    return "Terminación de contrato";
                case 2:
                    return "Separación voluntaria";
                case 3:
                    return "Abandono de empleo";
                case 4:
                    return "Defunción";
                case 5:
                    return "Despido";
                case 6:
                    return "Rescisión/Invalidez/Vejez";
                case 7:
                    return "Rescisión/Invalidez/Vejez";
                case 8:
                    return "Cambio reg. patronal";

                default:


                    break;
            }

            return "";
        }
        public FileResult getFileDownloadIncidencias()
        {
            var memStream = new MemoryStream();
            var streamWriter = new StreamWriter(memStream);

            string Cadena = "EMP_CLAVE|EMP_NOM|EMP_FALTAS";
            streamWriter.WriteLine(Cadena);
            List<LayoutIncidenciasRHDTO> listaIF = (List<LayoutIncidenciasRHDTO>)Session["ListaLayoutIncidenciasRHDTO"];

            var EmpleadosSEleccionados = (List<string>)Session["setListEmpleados"];
            DataTable dtLayout = new DataTable();

            var EncabezadoTabla = Cadena.Split('|');

            for (int i = 0; i < EncabezadoTabla.Length; i++)
            {
                dtLayout.Columns.Add(EncabezadoTabla[i]);
            }

            foreach (var listaRow in listaIF.Where(x => EmpleadosSEleccionados.Contains(x.EMP_CLAVE)))
            {
                dtLayout.Rows.Add(

                    listaRow.EMP_CLAVE,
                    listaRow.EMP_NOM,
                    listaRow.EMP_FALTAS
               );

            }
            WriteExcelWithNPOIIncidencias(dtLayout, "xls");

            return null;
        }
        private MemoryStream WriteExcelWithNPOI(DataTable dt, String extension)
        {

            IWorkbook workbook;

            if (extension == "xlsx")
            {
                workbook = new XSSFWorkbook();
            }
            else if (extension == "xls")
            {
                workbook = new HSSFWorkbook();
            }
            else
            {
                throw new Exception("This format is not supported");
            }

            ISheet sheet1 = workbook.CreateSheet("CONSTRUPLAN");

            //make a header row
            IRow row1 = sheet1.CreateRow(0);

            for (int j = 0; j < dt.Columns.Count; j++)
            {

                ICell cell = row1.CreateCell(j);
                String columnName = dt.Columns[j].ToString();
                cell.SetCellValue(columnName);
            }

            //loops through data
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet1.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    ICell cell = row.CreateCell(j);
                    String columnName = dt.Columns[j].ToString();
                    cell.SetCellValue(dt.Rows[i][columnName].ToString());
                }
            }

            using (var exportData = new MemoryStream())
            {
                Response.Clear();
                workbook.Write(exportData);
                if (extension == "xlsx") //xlsx file format
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "LayoutALTARH - CONSTRUPLAN.xlsx"));
                    Response.BinaryWrite(exportData.ToArray());
                }
                else if (extension == "xls")  //xls file format
                {
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "LayoutALTARH - CONSTRUPLAN.xls"));
                    Response.BinaryWrite(exportData.GetBuffer());
                }
                Response.End();
                return exportData;
            }
        }
        private MemoryStream WriteExcelWithNPOIBajas(DataTable dt, String extension)
        {

            IWorkbook workbook;

            if (extension == "xlsx")
            {
                workbook = new XSSFWorkbook();
            }
            else if (extension == "xls")
            {
                workbook = new HSSFWorkbook();
            }
            else
            {
                throw new Exception("This format is not supported");
            }

            ISheet sheet1 = workbook.CreateSheet("CONSTRUPLAN");

            //make a header row
            IRow row1 = sheet1.CreateRow(0);

            for (int j = 0; j < dt.Columns.Count; j++)
            {

                ICell cell = row1.CreateCell(j);
                String columnName = dt.Columns[j].ToString();
                cell.SetCellValue(columnName);
            }

            //loops through data
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet1.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    ICell cell = row.CreateCell(j);
                    String columnName = dt.Columns[j].ToString();
                    cell.SetCellValue(dt.Rows[i][columnName].ToString());
                }
            }

            using (var exportData = new MemoryStream())
            {
                Response.Clear();
                workbook.Write(exportData);
                if (extension == "xlsx") //xlsx file format
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "LayoutBAJASRH - CONSTRUPLAN.xlsx"));
                    Response.BinaryWrite(exportData.ToArray());
                }
                else if (extension == "xls")  //xls file format
                {
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "LayoutBAJASRH - CONSTRUPLAN.xls"));
                    Response.BinaryWrite(exportData.GetBuffer());
                }
                Response.End();
                return exportData;
            }
        }
        private MemoryStream WriteExcelWithNPOIIncidencias(DataTable dt, String extension)
        {

            IWorkbook workbook;

            if (extension == "xlsx")
            {
                workbook = new XSSFWorkbook();
            }
            else if (extension == "xls")
            {
                workbook = new HSSFWorkbook();
            }
            else
            {
                throw new Exception("This format is not supported");
            }

            ISheet sheet1 = workbook.CreateSheet("CONSTRUPLAN");

            //make a header row
            IRow row1 = sheet1.CreateRow(0);

            for (int j = 0; j < dt.Columns.Count; j++)
            {

                ICell cell = row1.CreateCell(j);
                String columnName = dt.Columns[j].ToString();
                cell.SetCellValue(columnName);
            }

            //loops through data
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet1.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    ICell cell = row.CreateCell(j);
                    String columnName = dt.Columns[j].ToString();
                    cell.SetCellValue(dt.Rows[i][columnName].ToString());
                }
            }

            using (var exportData = new MemoryStream())
            {
                Response.Clear();
                workbook.Write(exportData);
                if (extension == "xlsx") //xlsx file format
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "LayoutIncidenciasRH - CONSTRUPLAN.xlsx"));
                    Response.BinaryWrite(exportData.ToArray());
                }
                else if (extension == "xls")  //xls file format
                {
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "LayoutIncidenciasRH - CONSTRUPLAN.xls"));
                    Response.BinaryWrite(exportData.GetBuffer());
                }
                Response.End();
                return exportData;
            }
        }


    }
}