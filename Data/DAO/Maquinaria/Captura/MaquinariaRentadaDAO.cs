using Core.DAO.Maquinaria.Captura;
using Core.DTO.Contabilidad;
using Core.DTO.Maquinaria.Reporte;
using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Principal.Multiempresa;
using Core.Enum.Multiempresa;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Maquinaria.Captura;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Reporte;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Captura
{
    public class MaquinariaRentadaDAO : GenericDAO<tblM_MaquinariaRentada>, IMaquinariaRentada
    {
        private MaquinaFactoryServices MaquinaFactory = new MaquinaFactoryServices();
        private CapturaHorometroFactoryServices HorometroFactory = new CapturaHorometroFactoryServices();

        private string ObtenerCcPorNoEconomico(string noEconomico)
        {
            var cc = _context.tblM_CatMaquina.Where(x => x.noEconomico == noEconomico).FirstOrDefault().centro_costos;
            return cc;
        }

        public tblM_MaquinariaRentada SaveMaquinaRentada(tblM_MaquinariaRentada objMaquina)
        {
            try
            {
                // Obtener CentroDeCostos
                if (string.IsNullOrEmpty(objMaquina.CC))
                {
                    objMaquina.CC = ObtenerCcPorNoEconomico(objMaquina.NoEconomico);
                    if (string.IsNullOrEmpty(objMaquina.CC))
                    {
                        return new tblM_MaquinariaRentada();
                    }
                }
                //
                if (string.IsNullOrEmpty(objMaquina.Folio))
                {
                    if (objMaquina.NoEconomico != null && objMaquina.Obra != null && !string.IsNullOrEmpty(objMaquina.CC))
                    {
                        objMaquina.Folio = getFolioMaquinariaRentada(objMaquina.NoEconomico, objMaquina.Obra, objMaquina.CC);
                    }
                }
                else
                {
                    objMaquina.Folio = getNextFolioMaquinariaRentada(objMaquina.Folio);
                }


                SaveEntity(objMaquina, (int)BitacoraEnum.MAQUINARIARENTADA);
            }
            catch (Exception e)
            {
                return new tblM_MaquinariaRentada();
            }
            return objMaquina;
        }

        public tblM_MaquinariaRentada UpdateMaquinaRentada(tblM_MaquinariaRentada objMaquina)
        {
            try
            {
                // Obtener CentroDeCostos
                if (string.IsNullOrEmpty(objMaquina.CC))
                {
                    objMaquina.CC = ObtenerCcPorNoEconomico(objMaquina.NoEconomico);
                    if (string.IsNullOrEmpty(objMaquina.CC))
                    {
                        return new tblM_MaquinariaRentada();
                    }
                }
                //
                Update(objMaquina, objMaquina.id, (int)BitacoraEnum.MAQUINARIARENTADA);
            }
            catch (Exception e)
            {
                return new tblM_MaquinariaRentada();
            }
            return objMaquina;
        }



        public List<tblM_MaquinariaRentada> getMaquinariaRentada(List<string> ccs)
        {
            List<tblM_MaquinariaRentada> objResult = new List<tblM_MaquinariaRentada>();
            ccs = ccs ?? new List<string>();
            var ccList = new List<string>();
            foreach (var i in ccs)
            {
                try
                {
                    ccList.Add(i);
                }
                catch (Exception ex) { }
            }
            objResult = _context.tblM_MaquinariaRentada.Where(x => ccList.Contains(x.CC)).ToList();
            return builtMaquinaRentada(objResult);
        }

        public List<tblM_MaquinariaRentada> getCboMaquinariaFiltro(string obj)
        {
            var result = _context.tblM_MaquinariaRentada
                .Where(x => x.CC == obj)
                .GroupBy(x => x.NoEconomico)
                .Select(group => group.FirstOrDefault()).ToList();
            return builtMaquinaRentada(result);
        }



        public List<tblM_MaquinariaRentada> getMaquinariaRentadaPorFacturacion(List<string> ccs, DateTime PeriodoInicio, DateTime PeriodoFin)
        {
            var lstResult = new List<tblM_MaquinariaRentada>();
            var lstCompleta = new List<tblM_MaquinariaRentada>();
            ccs = ccs ?? new List<string>();
            var ccList = new List<string>();
            foreach (var i in ccs)
            {
                try
                {
                    ccList.Add(i);
                }
                catch (Exception ex) { }
            }
            if (ccList.Count == 0)
            {
                lstResult = _context.tblM_MaquinariaRentada.Where(x =>
                    x.FechaFacturacion >= PeriodoInicio
                    && x.FechaFacturacion <= PeriodoFin).ToList();
            }
            else
            {
                var objResult = _context.tblM_MaquinariaRentada.Where(x =>
                ccList.Count == 0 ? x.CC == x.CC : (ccList.Contains(x.CC))
                && x.PeriodoDel >= PeriodoInicio
                && x.PeriodoA <= PeriodoFin).ToList();
                lstResult = objResult.Where(x =>
                    ccList.Count == 0 ? x.CC == x.CC : (ccList.Contains(x.CC))
                    && x.FechaFacturacion >= PeriodoInicio
                    && x.FechaFacturacion <= PeriodoFin).ToList();
            }
            return builtMaquinaRentada(lstResult);
        }

        public List<tblM_MaquinariaRentada> getMaquinariaRentada(List<string> ccs, string NoEconomico, DateTime PeriodoInicio, DateTime PeriodoFin)
        {
            var lstResult = new List<tblM_MaquinariaRentada>();
            var lstCompleta = new List<tblM_MaquinariaRentada>();
            ccs = ccs ?? new List<string>();
            var ccList = new List<string>();
            var lstDivisa = FillCboProveedor();
            foreach (var i in ccs)
            {
                try
                {
                    ccList.Add(i);
                }
                catch (Exception ex) { }
            }
            if (ccList.Count == 0)
            {
                lstResult = _context.tblM_MaquinariaRentada.Where(x =>
                    x.PeriodoDel >= PeriodoInicio
                    && x.PeriodoA <= PeriodoFin).ToList();
            }
            else
            {
                var objResult = _context.tblM_MaquinariaRentada.Where(x =>
                ccList.Count == 0 ? x.CC == x.CC : (ccList.Contains(x.CC))
                && string.IsNullOrEmpty(NoEconomico) == true ? x.NoEconomico == x.NoEconomico : x.NoEconomico.Contains(NoEconomico)
                && x.PeriodoDel >= PeriodoInicio
                && x.PeriodoA <= PeriodoFin).ToList();
                lstResult = objResult.Where(x =>
                    ccList.Count == 0 ? x.CC == x.CC : (ccList.Contains(x.CC))
                    && x.PeriodoDel >= PeriodoInicio
                    && x.PeriodoA <= PeriodoFin).ToList();
            }
            return builtMaquinaRentada(lstResult);
        }

        public List<tblM_MaquinariaRentada> getMaquinariaRentadaPorId(int id)
        {
            var lstResult = _context.tblM_MaquinariaRentada.Where(x => x.id.Equals(id)).ToList();
            return builtMaquinaRentada(lstResult);
        }

        public List<tblM_MaquinariaRentada> getMaquinariaRentada(tblM_MaquinariaRentada ccs)
        {
            List<tblM_MaquinariaRentada> objResult = new List<tblM_MaquinariaRentada>();
            if (ccs.NoEconomico == null)
            {
                objResult = _context.tblM_MaquinariaRentada.Where(x => x.CC.Equals(ccs.CC)).ToList();
            }
            else
            {
                objResult = _context.tblM_MaquinariaRentada.Where(x => x.NoEconomico.Equals(ccs.NoEconomico)).ToList();
            }
            return builtMaquinaRentada(objResult);
        }

        public List<tblM_MaquinariaRentada> getMaquinariaRentada(int cc, DateTime fecha)
        {
            List<tblM_MaquinariaRentada> objResult = new List<tblM_MaquinariaRentada>();
            DateTime fechaIncio = new DateTime(fecha.Year, fecha.AddMonths(-1).Month, fecha.AddDays(-1).Day);
            objResult = _context.tblM_MaquinariaRentada.Where(x => x.CC == cc.ToString() && x.PeriodoDel.Month >= fechaIncio.Month && x.PeriodoA.Month <= fecha.Month).ToList();
            return builtMaquinaRentada(objResult);
        }

        List<tblM_MaquinariaRentada> builtMaquinaRentada(List<tblM_MaquinariaRentada> lstResult)
        {
            var lstCompleta = new List<tblM_MaquinariaRentada>();
            //var lstProveedor = FillCboProveedor().ToList();
            foreach (var item in lstResult)
            {
                var obj = new tblM_MaquinariaRentada();
                //var lstThisProveedor = lstProveedor.Where(y => y.NOMBRE2.Contains(item.Proveedor)).FirstOrDefault();
                obj.id = item.id;
                obj.Folio = item.Folio;
                obj.NoEconomico = item.NoEconomico;
                obj.Equipo = item.Equipo;
                obj.NoSerie = item.Modelo;
                obj.Proveedor = item.Proveedor;
                obj.Moneda = item.Moneda;
                //if (lstThisProveedor == null)
                //{
                //    obj.Proveedor = "";
                //    obj.Moneda = true;
                //}
                //else
                //{
                //    obj.Proveedor = lstThisProveedor.NOMBRE2;
                //    if (item.Moneda)
                //    {
                //        obj.Moneda = lstThisProveedor.MONEDA.Equals("1") ? true : false;
                //    }
                //    else
                //    {
                //        obj.Moneda = false;
                //    }
                //}
                obj.LlegadaObra = item.LlegadaObra;
                obj.CC = item.CC;
                obj.Obra = item.Obra;
                obj.FechaFacturacion = item.FechaFacturacion;
                obj.RecepcionFactura = item.RecepcionFactura;
                obj.NoFactura = item.NoFactura;
                obj.DepGarantia = item.DepGarantia;
                obj.TramiteDG = item.TramiteDG;
                obj.NotaCredito = item.NotaCredito;
                obj.AplicaNC = item.AplicaNC;
                obj.BaseHoraMensual = item.BaseHoraMensual;
                obj.PeriodoDel = item.PeriodoDel;
                obj.PeriodoA = item.PeriodoA;
                obj.HorometroInicial = item.HorometroInicial;
                obj.HorometroFinal = item.HorometroFinal;
                obj.HorasTrabajadas = item.HorasTrabajadas;
                obj.HorasExtras = item.HorasExtras;
                obj.CostoHorasExtras = item.CostoHorasExtras;
                obj.TotalHorasExtras = item.TotalHorasExtras;
                obj.PrecioMes = item.PrecioMes;
                obj.SeguroMes = item.SeguroMes;
                obj.IVA = item.IVA;
                obj.TotalRenta = item.TotalRenta;
                obj.REQ = item.REQ;
                obj.FechaReq = item.FechaReq;
                obj.OrdenCompra = item.OrdenCompra;
                obj.FechaOrdenCompra = item.FechaOrdenCompra;
                obj.ContraRecibo = item.ContraRecibo;
                obj.FechaContraRecibo = item.FechaContraRecibo;
                obj.Anotaciones = item.Anotaciones;
                obj.DifHora = item.DifHora;
                obj.DifHoraHoraExtra = item.DifHoraHoraExtra;
                obj.DifHoraContraRecibo = item.DifHoraContraRecibo;
                obj.DifHoraFactura = item.DifHoraFactura;
                obj.DifHoraOrdenCompra = item.DifHoraOrdenCompra;
                obj.DifHoraFecha = item.DifHoraFecha;
                obj.CargoDaño = item.CargoDaño;
                obj.CargoDañoHoraExtra = item.CargoDañoHoraExtra;
                obj.CargoDañoContraRecibo = item.CargoDañoContraRecibo;
                obj.CargoDañoFactura = item.CargoDañoFactura;
                obj.CargoDañoOrdenCompra = item.CargoDañoOrdenCompra;
                obj.CargoDañoFecha = item.CargoDañoFecha;
                obj.Fletes = item.Fletes;
                obj.FletesHoraExtra = item.FletesHoraExtra;
                obj.FletesContraRecibo = item.FletesContraRecibo;
                obj.FletesNoFactura = item.FletesNoFactura;
                obj.FletesOrdenCompra = item.FletesOrdenCompra;
                obj.FletesFecha = item.FletesFecha;
                lstCompleta.Add(obj);
            }
            return lstCompleta;
        }
        private string getFolioMaquinariaRentada(string noEconomico, string strNombre, string cc)
        {
            var objResult = _context.tblM_MaquinariaRentada.Where(x => x.CC == cc && x.NoEconomico == noEconomico).ToList().Count + 1;
            var result = strNombre.Substring(0, 3) + "-" + noEconomico + "-" + objResult;
            return result;
        }
        private string getNextFolioMaquinariaRentada(string Folio)
        {
            var objResult = _context.tblM_MaquinariaRentada.Where(x => x.Folio.Contains(Folio.Split('-')[0] + "-" + Folio.Split('-')[1] + "-" + Folio.Split('-')[2] + "-")).ToList().Count + 1;
            var result = Folio.Split('-')[0] + "-" + Folio.Split('-')[1] + "-" + Folio.Split('-')[2] + "-" + objResult;
            return result;
        }

        public List<ProveedorDTO> FillCboProveedor()
        {
            string consulta = "SELECT numpro AS NUMPROVEEDOR, nomcorto AS NOMBRE1, nombre AS NOMBRE2, moneda AS MONEDA FROM DBA.sp_proveedores";
            //var res = (List<ProveedorDTO>)_contextEnkontrol.Where(consulta).ToObject<List<ProveedorDTO>>();
            var res = _contextEnkontrol.Select<ProveedorDTO>(EnkontrolAmbienteEnum.Prod, consulta);
            return res.ToList();
        }

        public bool getProveedorMoneda(int idProveedor)
        {
            string consulta = "SELECT moneda AS MONEDA FROM DBA.sp_proveedores where numpro =" + idProveedor;
            var res1 = (List<ProveedorDTO>)_contextEnkontrol.Where(consulta).ToObject<List<ProveedorDTO>>();
            return res1[0].MONEDA.Equals("1") ? true : false;
        }

        public Dictionary<string, object> getRptProvisionalInfo(int cc, DateTime fechaCorte, decimal TC, bool TodoReporte)
        {
            var result = new Dictionary<string, object>();
            //        try 
            //{	        
            var maquinas = getMaquinariaRentada(cc, fechaCorte);
            var lstEquipos = new List<RepProvisionDTO>();
            var lstTotal = new totalProvisionDTO();
            var lstTotalExtra = new totalProvisionDTO();
            var lstResumen = new totalProvisionDTO();
            foreach (var Equipo in maquinas)
            {
                var obj = new RepProvisionDTO();
                obj.id = Equipo.id;
                obj.NoEconomico = Equipo.NoEconomico;
                obj.Moneda = Equipo.Moneda ? "PESOS" : "DLLS";
                obj.strAnotacion = Equipo.Anotaciones;
                obj.Equipo = Equipo.NoFactura;
                obj.Inicio = Equipo.PeriodoDel.ToShortDateString();
                obj.Termino = Equipo.PeriodoA.ToShortDateString();
                obj.CostoRenta = Equipo.BaseHoraMensual;

                obj.HrsFac = Equipo.HorasTrabajadas;
                obj.PU = obj.HrsFac == 0 ? 0 : decimal.Round(obj.CostoRenta / obj.HrsFac, 2);
                obj.HrsCons = decimal.Round((decimal)CalcularDiasDeDiferencia(fechaCorte, Equipo.PeriodoDel) * (obj.HrsFac / 30), 2) > obj.HrsFac ? obj.HrsFac : decimal.Round((decimal)CalcularDiasDeDiferencia(fechaCorte, Equipo.PeriodoDel) * (obj.HrsFac / 27), 2);
                obj.ImporteConsumido = decimal.Round(obj.PU * obj.HrsCons, 2);
                obj.ImporteTotal = obj.Equipo.Equals("") ? obj.ImporteConsumido : obj.ImporteConsumido - obj.CostoRenta;
                obj.FacturaExtra = Equipo.DifHoraFactura.Equals(string.Empty) ? "-" : Equipo.DifHoraFactura;
                obj.HrsExtra = Equipo.DifHoraHoraExtra;
                obj.PUHrsExtra = obj.HrsExtra.Equals(0) ? obj.HrsFac : decimal.Round(obj.PU, 2);
                obj.ImporteConsumidoExtra = obj.HrsExtra * obj.PUHrsExtra;
                obj.ImporteTotalExtra = obj.FacturaExtra.Equals("-") ? obj.ImporteConsumidoExtra : obj.PUHrsExtra - obj.PUHrsExtra;

                if (Equipo.DifHora == false)
                {
                    obj.HrsExtra = 0;
                    obj.PUHrsExtra = 0;
                    obj.strPUHrsExtra = "";
                    obj.strImporteConsumidoExtra = "";
                    obj.strImporteTotalExtra = "";
                }

                obj.strCostoRenta = String.Format("{0:C2}", obj.CostoRenta);
                obj.strPU = String.Format("{0:C2}", obj.PU);
                obj.strImporteConsumido = String.Format("{0:C2}", obj.ImporteConsumido);
                obj.strImporteTotal = String.Format("{0:C2}", obj.ImporteTotal);
                obj.strPUHrsExtra = String.Format("{0:C2}", obj.PUHrsExtra);
                obj.strImporteConsumidoExtra = String.Format("{0:C2}", obj.ImporteConsumidoExtra);
                obj.strImporteTotalExtra = String.Format("{0:C2}", obj.ImporteTotalExtra);

                if (TodoReporte)
                {
                    if (obj.HrsFac != obj.HrsCons && !obj.Equipo.Equals(string.Empty))
                        lstEquipos.Add(obj);
                }
                else
                {
                    lstEquipos.Add(obj);
                }
                if (Equipo.Moneda)
                {
                    //PESOS
                    lstTotal.TotalPesos += obj.ImporteTotal;
                    lstTotalExtra.TotalPesos += obj.ImporteTotalExtra;
                }
                else
                {
                    //DDLS
                    lstTotal.TotalDlls += obj.ImporteTotal;
                    lstTotalExtra.TotalDlls += obj.ImporteTotalExtra;
                }
            }

            //TC
            lstTotal.TC = lstTotalExtra.TC = lstResumen.TC = TC;
            lstTotal.TotalMN = (lstTotal.TotalDlls * lstTotal.TC) + lstTotal.TotalPesos;
            lstTotalExtra.TotalMN = (lstTotalExtra.TotalDlls * lstTotalExtra.TC) + lstTotalExtra.TotalPesos;

            //Resume renta
            lstResumen.TotalDlls = lstTotal.TotalDlls + lstTotalExtra.TotalDlls;
            lstResumen.TotalPesos = lstTotal.TotalPesos + lstTotalExtra.TotalPesos;
            lstResumen.TotalMN = (lstResumen.TotalDlls * lstResumen.TC) + lstResumen.TotalPesos;


            lstTotal.strTotalPesos = String.Format("{0:C2}", lstTotal.TotalPesos);
            lstTotalExtra.strTotalPesos = String.Format("{0:C2}", lstTotalExtra.TotalPesos);

            lstTotal.strTotalDlls = String.Format("{0:C2}", lstTotal.TotalDlls);
            lstTotalExtra.strTotalPesos = String.Format("{0:C2}", lstTotalExtra.TotalPesos);
            lstTotalExtra.strTotalDlls = String.Format("{0:C2}", lstTotalExtra.TotalDlls);

            lstTotal.strTC = lstTotalExtra.strTC = lstResumen.strTC = String.Format("{0:C2}", TC);

            lstTotal.strTotalMN = String.Format("{0:C2}", lstTotal.TotalMN);
            lstTotalExtra.strTotalMN = String.Format("{0:C2}", lstTotalExtra.TotalMN);

            lstResumen.strTotalDlls = String.Format("{0:C2}", lstResumen.TotalDlls);
            lstResumen.strTotalPesos = String.Format("{0:C2}", lstResumen.TotalPesos);
            lstResumen.strTotalMN = String.Format("{0:C2}", lstResumen.TotalMN);

            result.Add("lstEquipos", lstEquipos);
            result.Add("lstTotal", lstTotal);
            result.Add("lstTotalExtra", lstTotalExtra);
            result.Add("lstResumen", lstResumen);
            //}
            //catch (Exception)
            //{

            //    throw;
            //}

            return result;
        }

        public double CalcularDiasDeDiferencia(DateTime primerFecha, DateTime segundaFecha)
        {
            TimeSpan diferencia;
            diferencia = primerFecha - segundaFecha;
            return diferencia.Days;
        }

        public void guardarExcel()
        {
            string file = "C:\\Proyectos\\SIGOPLAN\\CONTROL GENERAL DE RENTAS.xlsx";
            OleDbConnection con = new System.Data.OleDb.OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + file + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1;';");
            con.Open();
            DataSet dset = new DataSet();
            OleDbDataAdapter dadp = new OleDbDataAdapter("SELECT * FROM [CONTROL GENERAL DE RENTAS$]", con);
            dadp.TableMappings.Add("tbl", "Table");
            dadp.Fill(dset);
            DataTable table = dset.Tables[0];

            var lst = new List<tblM_MaquinariaRentada>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                var obj = new tblM_MaquinariaRentada();
                obj.NoEconomico = table.Rows[i][1] == DBNull.Value ? string.Empty : (string)table.Rows[i][1];
                obj.Equipo = table.Rows[i][2] == DBNull.Value ? string.Empty : (string)table.Rows[i][2];
                obj.NoSerie = table.Rows[i][3] == DBNull.Value ? string.Empty : (string)table.Rows[i][3];
                obj.Modelo = table.Rows[i][4] == DBNull.Value ? string.Empty : (string)table.Rows[i][4];
                obj.Proveedor = table.Rows[i][5] == DBNull.Value ? string.Empty : (string)table.Rows[i][5];
                obj.LlegadaObra = table.Rows[i][6] == DBNull.Value ? new DateTime() : (DateTime)table.Rows[i][6];
                obj.CC = table.Rows[i][7] == DBNull.Value ? "" : table.Rows[i][7].ToString();
                obj.Obra = table.Rows[i][8] == DBNull.Value ? string.Empty : (string)table.Rows[i][8];
                obj.FechaFacturacion = table.Rows[i][9] == DBNull.Value ? new DateTime() : (DateTime)table.Rows[i][9];
                obj.RecepcionFactura = table.Rows[i][10] == DBNull.Value ? new DateTime() : (DateTime)table.Rows[i][10];
                obj.NoFactura = table.Rows[i][11] == DBNull.Value ? string.Empty : Convert.ToString(table.Rows[i][11]);
                obj.DepGarantia = DBNull.Value.Equals(table.Rows[i][12]) ? string.Empty : (string)table.Rows[i][12];
                obj.TramiteDG = table.Rows[i][13] == DBNull.Value ? false : true;
                obj.NotaCredito = table.Rows[i][14] == DBNull.Value ? string.Empty : (string)table.Rows[i][14];
                obj.AplicaNC = table.Rows[i][15] == DBNull.Value ? false : true;
                obj.BaseHoraMensual = DBNull.Value.Equals(table.Rows[i][16]) ? 0 : Convert.ToInt32(table.Rows[i][16]);
                obj.PeriodoDel = table.Rows[i][17] == DBNull.Value ? new DateTime() : (DateTime)table.Rows[i][17];
                obj.PeriodoA = table.Rows[i][18] == DBNull.Value ? new DateTime() : (DateTime)table.Rows[i][18];
                obj.HorometroInicial = table.Rows[i][20] == DBNull.Value ? decimal.Zero : Convert.ToDecimal(table.Rows[i][20]);
                obj.HorometroFinal = table.Rows[i][21] == DBNull.Value ? decimal.Zero : Convert.ToDecimal(table.Rows[i][21]);
                obj.HorasTrabajadas = table.Rows[i][22] == DBNull.Value ? decimal.Zero : Convert.ToDecimal(table.Rows[i][22]);
                obj.HorasExtras = table.Rows[i][23] == DBNull.Value ? decimal.Zero : Convert.ToDecimal(table.Rows[i][23]);
                obj.CostoHorasExtras = DBNull.Value.Equals(table.Rows[i][24]) ? decimal.Zero : Convert.ToDecimal(table.Rows[i][24]);
                obj.TotalHorasExtras = table.Rows[i][25] == DBNull.Value ? decimal.Zero : Convert.ToDecimal(table.Rows[i][25]);
                obj.PrecioMes = table.Rows[i][26] == DBNull.Value ? decimal.Zero : Convert.ToDecimal(table.Rows[i][26]);
                obj.SeguroMes = table.Rows[i][27] == DBNull.Value ? decimal.Zero : Convert.ToDecimal(table.Rows[i][27]);
                obj.IVA = table.Rows[i][28] == DBNull.Value ? decimal.Zero : Convert.ToDecimal(table.Rows[i][28]);
                obj.TotalRenta = table.Rows[i][29] == DBNull.Value ? decimal.Zero : Convert.ToDecimal(table.Rows[i][29]);
                obj.REQ = DBNull.Value.Equals(table.Rows[i][30]) ? string.Empty : Convert.ToString(table.Rows[i][30]);
                obj.FechaReq = table.Rows[i][31] == DBNull.Value ? new DateTime() : (DateTime)table.Rows[i][31];
                obj.OrdenCompra = table.Rows[i][32] == DBNull.Value ? string.Empty : (string)table.Rows[i][32];
                obj.FechaOrdenCompra = table.Rows[i][33] == DBNull.Value ? new DateTime() : (DateTime)table.Rows[i][33];
                obj.ContraRecibo = table.Rows[i][34] == DBNull.Value ? string.Empty : (string)table.Rows[i][34];
                obj.FechaContraRecibo = table.Rows[i][35] == DBNull.Value ? new DateTime() : (DateTime)table.Rows[i][35];
                obj.Anotaciones = table.Rows[i][36] == DBNull.Value ? string.Empty : (string)table.Rows[i][36];
                //obj.Folio = obj.NoEconomico != null && obj.Obra != null && obj.CC != 0 ? getFolioMaquinariaRentada(obj.NoEconomico, obj.Obra, obj.CC) : (string)table.Rows[i][0];
                obj.Folio = string.Empty;
                obj.DifHora = false;
                obj.DifHoraContraRecibo = string.Empty;
                obj.DifHoraFactura = string.Empty;
                obj.DifHoraFecha = new DateTime();
                obj.DifHoraHoraExtra = 0;
                obj.DifHoraOrdenCompra = string.Empty;
                obj.CargoDaño = false;
                obj.CargoDañoContraRecibo = string.Empty;
                obj.CargoDañoFactura = string.Empty;
                obj.CargoDañoFecha = new DateTime();
                obj.CargoDañoHoraExtra = 0;
                obj.CargoDañoOrdenCompra = string.Empty;
                obj.Fletes = false;
                obj.FletesContraRecibo = string.Empty;
                obj.FletesNoFactura = string.Empty;
                obj.FletesFecha = new DateTime();
                obj.FletesHoraExtra = 0;
                obj.FletesOrdenCompra = string.Empty;
                lst.Add(obj);
            }

            foreach (var item in lst)
            {
                SaveMaquinaRentada(item);
            }
        }
    }
}
