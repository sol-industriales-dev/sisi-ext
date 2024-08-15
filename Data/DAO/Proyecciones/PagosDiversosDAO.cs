using Core.DAO.Proyecciones;
using Core.DTO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using Data.Factory.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Proyecciones
{
    public class PagosDiversosDAO : GenericDAO<tblPro_PagosDiversos>, IPagosDiversosDAO
    {
        public tblPro_PagosDiversos GetJsonData(FiltrosGeneralDTO filtro)
        {
            var res = _context.tblPro_PagosDiversos.FirstOrDefault(x => x.Anio == filtro.anio && x.Mes == filtro.mes);
            if (res == null)
            {
                return null;
            }
            else
            {
                return res;
            }
        }
        public void GuardarActualizarPagosDiversos(FiltrosGeneralDTO objFiltro, PagosDivDTO obj)
        {
            CapturadeObrasFactoryServices capturadeObrasFactoryServices = new CapturadeObrasFactoryServices();
            var dtCObras = capturadeObrasFactoryServices.GetCapturaObras().getinfoCapturaObras(objFiltro.escenario, objFiltro.divisor, objFiltro.mes, objFiltro.anio);
            var JSaldoFinalFlujoEfectivo = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("SaldoFinalFlujoEfectivo")).Value);
            var objSaldoFinalFlujoEfectivo = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JSaldoFinalFlujoEfectivo);

            var temp = _context.tblPro_PagosDiversos.FirstOrDefault(x => x.Mes == objFiltro.mes && x.Anio == objFiltro.anio && x.Estatus);
            if (temp != null)
            {
                temp.CadenaJson = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                _context.SaveChanges();
                var mSaldoFlujo = new MesDTO();
                mSaldoFlujo.Mes1 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA1.Replace(",", ""));
                mSaldoFlujo.Mes2 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA2.Replace(",", ""));
                mSaldoFlujo.Mes3 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA3.Replace(",", ""));
                mSaldoFlujo.Mes4 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA4.Replace(",", ""));
                mSaldoFlujo.Mes5 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA5.Replace(",", ""));
                mSaldoFlujo.Mes6 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA6.Replace(",", ""));
                mSaldoFlujo.Mes7 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA7.Replace(",", ""));
                mSaldoFlujo.Mes8 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA8.Replace(",", ""));
                mSaldoFlujo.Mes9 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA9.Replace(",", ""));
                mSaldoFlujo.Mes10 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA10.Replace(",", ""));
                mSaldoFlujo.Mes11 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA11.Replace(",", ""));
                mSaldoFlujo.Mes12 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA12.Replace(",", ""));
                mSaldoFlujo.MesT = 0;
                var nuevo = Newtonsoft.Json.JsonConvert.DeserializeObject<PagosDivDTO>(temp.CadenaJson);
                nuevo.SaldoFlujo = mSaldoFlujo;
                temp.CadenaJson = Newtonsoft.Json.JsonConvert.SerializeObject(nuevo);
                _context.SaveChanges();
            }
            else
            {
                var o = new tblPro_PagosDiversos();
                o.Anio = objFiltro.anio;
                o.Mes = objFiltro.mes;
                o.Estatus = true;
                o.CadenaJson = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                _context.tblPro_PagosDiversos.Add(o);
                _context.SaveChanges();
                var mSaldoFlujo = new MesDTO();
                mSaldoFlujo.Mes1 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA1.Replace(",", ""));
                mSaldoFlujo.Mes2 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA2.Replace(",", ""));
                mSaldoFlujo.Mes3 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA3.Replace(",", ""));
                mSaldoFlujo.Mes4 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA4.Replace(",", ""));
                mSaldoFlujo.Mes5 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA5.Replace(",", ""));
                mSaldoFlujo.Mes6 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA6.Replace(",", ""));
                mSaldoFlujo.Mes7 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA7.Replace(",", ""));
                mSaldoFlujo.Mes8 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA8.Replace(",", ""));
                mSaldoFlujo.Mes9 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA9.Replace(",", ""));
                mSaldoFlujo.Mes10 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA10.Replace(",", ""));
                mSaldoFlujo.Mes11 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA11.Replace(",", ""));
                mSaldoFlujo.Mes12 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA12.Replace(",", ""));
                mSaldoFlujo.MesT = 0;
                var nuevo = Newtonsoft.Json.JsonConvert.DeserializeObject<PagosDivDTO>(temp.CadenaJson);
                nuevo.SaldoFlujo = mSaldoFlujo;
                temp.CadenaJson = Newtonsoft.Json.JsonConvert.SerializeObject(nuevo);
                _context.SaveChanges();
            }
        }
        public void GuardarFormExcel(tblPro_PagosDiversos obj)
        {
            var m = new MesDTO();
            m.Mes1 = 0;
            m.Mes2 = 0;
            m.Mes3 = 0;
            m.Mes4 = 0;
            m.Mes5 = 0;
            m.Mes6 = 0;
            m.Mes7 = 0;
            m.Mes8 = 0;
            m.Mes9 = 0;
            m.Mes10 = 0;
            m.Mes11 = 0;
            m.Mes12 = 0;
            m.MesT = 0;

            var ot = Newtonsoft.Json.JsonConvert.DeserializeObject<PagosDivDTO>(obj.CadenaJson);
            
            var obras = _context.tblPro_CapturadeObras.FirstOrDefault(x=>x.MesInicio==obj.Mes && x.EjercicioInicial==obj.Anio);
            List<tblPro_Obras> jsonObra = Newtonsoft.Json.JsonConvert.DeserializeObject<List<tblPro_Obras>>(obras.CadenaJson);
            var obrasFlag = jsonObra.Where(x => x.banderaFinanciamiento == true).ToList();
            var premisas = _context.tblPro_Premisas.FirstOrDefault(x => x.Mes == obj.Mes && x.Anio == obj.Anio);
            var objPremisa = Newtonsoft.Json.JsonConvert.DeserializeObject<PremisasDTO>(premisas.CadenaJson);
            var cxc = _context.tblPro_CxC.FirstOrDefault(x => x.Mes == obj.Mes && x.Anio == obj.Anio);
            var objCXC = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CxCDTO>>(cxc.CadenaJson);
            var si = _context.tblPro_SaldosIniciales.FirstOrDefault(x => x.Mes == obj.Mes && x.Anio == obj.Anio);
            var objSI = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EPFSaldoInicialDTO>>(si.CadenaJson);

            var reserva = new MesDTO();
            reserva.Mes1 = -1 * obrasFlag.Sum(x => (((x.Monto * x.Fecha1) / 100) * x.porcentaje) / 100);
            reserva.Mes2 = -1 * obrasFlag.Sum(x => (((x.Monto * x.Fecha2) / 100) * x.porcentaje) / 100);
            reserva.Mes3 = -1 * obrasFlag.Sum(x => (((x.Monto * x.Fecha3) / 100) * x.porcentaje) / 100);
            reserva.Mes4 = -1 * obrasFlag.Sum(x => (((x.Monto * x.Fecha4) / 100) * x.porcentaje) / 100);
            reserva.Mes5 = -1 * obrasFlag.Sum(x => (((x.Monto * x.Fecha5) / 100) * x.porcentaje) / 100);
            reserva.Mes6 = -1 * obrasFlag.Sum(x => (((x.Monto * x.Fecha6) / 100) * x.porcentaje) / 100);
            reserva.Mes7 = -1 * obrasFlag.Sum(x => (((x.Monto * x.Fecha7) / 100) * x.porcentaje) / 100);
            reserva.Mes8 = -1 * obrasFlag.Sum(x => (((x.Monto * x.Fecha8) / 100) * x.porcentaje) / 100);
            reserva.Mes9 = -1 * obrasFlag.Sum(x => (((x.Monto * x.Fecha9) / 100) * x.porcentaje) / 100);
            reserva.Mes10 = -1 * obrasFlag.Sum(x => (((x.Monto * x.Fecha10) / 100) * x.porcentaje) / 100);
            reserva.Mes11 = -1 * obrasFlag.Sum(x => (((x.Monto * x.Fecha11) / 100) * x.porcentaje) / 100);
            reserva.Mes12 = -1 * obrasFlag.Sum(x => (((x.Monto * x.Fecha12) / 100) * x.porcentaje) / 100);
            reserva.MesT = reserva.Mes1 + reserva.Mes2 + reserva.Mes3 + reserva.Mes4 + reserva.Mes5 + reserva.Mes6 + reserva.Mes7 + reserva.Mes8 + reserva.Mes9 + reserva.Mes10 + reserva.Mes11 + reserva.Mes12;

            var mdes = new MesDTO();
            mdes.Mes1 = reserva.Mes1 + ot.DesgloseVariosPagos.Sum(x => x.Mes1);
            mdes.Mes2 = reserva.Mes2 + ot.DesgloseVariosPagos.Sum(x => x.Mes2);
            mdes.Mes3 = reserva.Mes3 + ot.DesgloseVariosPagos.Sum(x => x.Mes3);
            mdes.Mes4 = reserva.Mes4 + ot.DesgloseVariosPagos.Sum(x => x.Mes4);
            mdes.Mes5 = reserva.Mes5 + ot.DesgloseVariosPagos.Sum(x => x.Mes5);
            mdes.Mes6 = reserva.Mes6 + ot.DesgloseVariosPagos.Sum(x => x.Mes6);
            mdes.Mes7 = reserva.Mes7 + ot.DesgloseVariosPagos.Sum(x => x.Mes7);
            mdes.Mes8 = reserva.Mes8 + ot.DesgloseVariosPagos.Sum(x => x.Mes8);
            mdes.Mes9 = reserva.Mes9 + ot.DesgloseVariosPagos.Sum(x => x.Mes9);
            mdes.Mes10 = reserva.Mes10 + ot.DesgloseVariosPagos.Sum(x => x.Mes10);
            mdes.Mes11 = reserva.Mes11 + ot.DesgloseVariosPagos.Sum(x => x.Mes11);
            mdes.Mes12 = reserva.Mes12 + ot.DesgloseVariosPagos.Sum(x => x.Mes12);
            mdes.MesT = reserva.MesT + ot.DesgloseVariosPagos.Sum(x => x.MesT);

            ot.PagExt = mdes;
            ot.PorcSaldoAmortAbono =m;
            var mCtoAplicarCXC = new MesDTO();
            mCtoAplicarCXC.Mes1 = objCXC.Sum(x => x.MES1AC);
            mCtoAplicarCXC.Mes2 = objCXC.Sum(x => x.MES2AC);
            mCtoAplicarCXC.Mes3 = objCXC.Sum(x => x.MES3AC);
            mCtoAplicarCXC.Mes4 = objCXC.Sum(x => x.MES4AC);
            mCtoAplicarCXC.Mes5 = objCXC.Sum(x => x.MES5AC);
            mCtoAplicarCXC.Mes6 = objCXC.Sum(x => x.MES6AC);
            mCtoAplicarCXC.Mes7 = objCXC.Sum(x => x.MES7AC);
            mCtoAplicarCXC.Mes8 = objCXC.Sum(x => x.MES8AC);
            mCtoAplicarCXC.Mes9 = objCXC.Sum(x => x.MES9AC);
            mCtoAplicarCXC.Mes10 = objCXC.Sum(x => x.MES10AC);
            mCtoAplicarCXC.Mes11 = objCXC.Sum(x => x.MES11AC);
            mCtoAplicarCXC.Mes12 = objCXC.Sum(x => x.MES12AC);
            mCtoAplicarCXC.MesT = mCtoAplicarCXC.Mes1 + mCtoAplicarCXC.Mes2 + mCtoAplicarCXC.Mes3 + mCtoAplicarCXC.Mes4 + mCtoAplicarCXC.Mes5 + mCtoAplicarCXC.Mes6 + mCtoAplicarCXC.Mes7 + mCtoAplicarCXC.Mes8 + mCtoAplicarCXC.Mes9 + mCtoAplicarCXC.Mes10 + mCtoAplicarCXC.Mes11 + mCtoAplicarCXC.Mes12;
            ot.CtoAplicarCXC = mCtoAplicarCXC;

            var mImporteAmortAbono = new MesDTO();
            var siPyC = objSI.FirstOrDefault(x => x.Concepto.Equals("Proveedores y Contratistas"));
            mImporteAmortAbono.Mes1 = siPyC.Inicial;
            mImporteAmortAbono.Mes2 = ((siPyC.Saldo - siPyC.Inicial) / 1000) - (mImporteAmortAbono.Mes2 / 1000);
            mImporteAmortAbono.Mes3 = mImporteAmortAbono.Mes2 - (mImporteAmortAbono.Mes3 / 1000);
            mImporteAmortAbono.Mes4 = mImporteAmortAbono.Mes3 - (mImporteAmortAbono.Mes4 / 1000);
            mImporteAmortAbono.Mes5 = mImporteAmortAbono.Mes4 - (mImporteAmortAbono.Mes5 / 1000);
            mImporteAmortAbono.Mes6 = mImporteAmortAbono.Mes5 - (mImporteAmortAbono.Mes6 / 1000);
            mImporteAmortAbono.Mes7 = mImporteAmortAbono.Mes6 - (mImporteAmortAbono.Mes7 / 1000);
            mImporteAmortAbono.Mes8 = mImporteAmortAbono.Mes7 - (mImporteAmortAbono.Mes8 / 1000);
            mImporteAmortAbono.Mes9 = mImporteAmortAbono.Mes8 - (mImporteAmortAbono.Mes9 / 1000);
            mImporteAmortAbono.Mes10 = mImporteAmortAbono.Mes9 - (mImporteAmortAbono.Mes10 / 1000);
            mImporteAmortAbono.Mes11 = mImporteAmortAbono.Mes10 - (mImporteAmortAbono.Mes11 / 1000);
            mImporteAmortAbono.Mes12 = mImporteAmortAbono.Mes11 - (mImporteAmortAbono.Mes12 / 1000);
            ot.BaseImporteAmortAbono = mImporteAmortAbono;
            ot.ImporteAmortAbono = m;
            ot.PorcSaldoAmortCompania =m;
            var mImporteAmortCompania = new MesDTO();

            mImporteAmortCompania.Mes1 = 0;
            mImporteAmortCompania.Mes2 = mImporteAmortCompania.Mes2;
            mImporteAmortCompania.Mes3 = mImporteAmortCompania.Mes3;
            mImporteAmortCompania.Mes4 = mImporteAmortCompania.Mes4;
            mImporteAmortCompania.Mes5 = mImporteAmortCompania.Mes5;
            mImporteAmortCompania.Mes6 = mImporteAmortCompania.Mes6;
            mImporteAmortCompania.Mes7 = mImporteAmortCompania.Mes7;
            mImporteAmortCompania.Mes8 = mImporteAmortCompania.Mes8;
            mImporteAmortCompania.Mes9 = mImporteAmortCompania.Mes9;
            mImporteAmortCompania.Mes10 = mImporteAmortCompania.Mes10;
            mImporteAmortCompania.Mes11 = mImporteAmortCompania.Mes11;
            mImporteAmortCompania.Mes12 = mImporteAmortCompania.Mes12;
            ot.BaseImporteAmortCompania = mImporteAmortCompania;
            ot.ImporteAmortCompania =m;
            ot.Saldo =0;
            ot.PuntosAdic =0;
            var mTasaAnual = new MesDTO();
            mTasaAnual.Mes1 = objPremisa.Ln3.Mes1;
            mTasaAnual.Mes2 = objPremisa.Ln3.Mes1;
            mTasaAnual.Mes3 = objPremisa.Ln3.Mes1;
            mTasaAnual.Mes4 = objPremisa.Ln3.Mes1;
            mTasaAnual.Mes5 = objPremisa.Ln3.Mes1;
            mTasaAnual.Mes6 = objPremisa.Ln3.Mes1;
            mTasaAnual.Mes7 = objPremisa.Ln3.Mes1;
            mTasaAnual.Mes8 = objPremisa.Ln3.Mes1;
            mTasaAnual.Mes9 = objPremisa.Ln3.Mes1;
            mTasaAnual.Mes10 = objPremisa.Ln3.Mes1;
            mTasaAnual.Mes11 = objPremisa.Ln3.Mes1;
            mTasaAnual.Mes12 = objPremisa.Ln3.Mes1;
            mTasaAnual.MesT = objPremisa.Ln3.Mes1;
            ot.BaseTasaAnual = mTasaAnual;
            ot.TasaAnual = mTasaAnual;

            ot.PagoCapital =0;
            ot.SaldoAmortCompania =m;
            ot.AmortCapitalCompania =m;
            ot.AmortVencidasCapitalCompania =m;
            ot.InteresesGenerados =m;
            ot.PorcSaldoAmortAcreedores =m;
            var mImporteAmortAcreedores = new MesDTO();
            var siPyC3 = objSI.FirstOrDefault(x => x.Concepto.Equals("Acreedores Diversos"));
            mImporteAmortAcreedores.Mes1 = siPyC3.Inicial;
            mImporteAmortAcreedores.Mes2 = ((siPyC3.Saldo - siPyC3.Inicial) / 1000) - (mImporteAmortAcreedores.Mes2 / 1000);
            mImporteAmortAcreedores.Mes3 = mImporteAmortAcreedores.Mes2 - (mImporteAmortAcreedores.Mes3 / 1000);
            mImporteAmortAcreedores.Mes4 = mImporteAmortAcreedores.Mes3 - (mImporteAmortAcreedores.Mes4 / 1000);
            mImporteAmortAcreedores.Mes5 = mImporteAmortAcreedores.Mes4 - (mImporteAmortAcreedores.Mes5 / 1000);
            mImporteAmortAcreedores.Mes6 = mImporteAmortAcreedores.Mes5 - (mImporteAmortAcreedores.Mes6 / 1000);
            mImporteAmortAcreedores.Mes7 = mImporteAmortAcreedores.Mes6 - (mImporteAmortAcreedores.Mes7 / 1000);
            mImporteAmortAcreedores.Mes8 = mImporteAmortAcreedores.Mes7 - (mImporteAmortAcreedores.Mes8 / 1000);
            mImporteAmortAcreedores.Mes9 = mImporteAmortAcreedores.Mes8 - (mImporteAmortAcreedores.Mes9 / 1000);
            mImporteAmortAcreedores.Mes10 = mImporteAmortAcreedores.Mes9 - (mImporteAmortAcreedores.Mes10 / 1000);
            mImporteAmortAcreedores.Mes11 = mImporteAmortAcreedores.Mes10 - (mImporteAmortAcreedores.Mes11 / 1000);
            mImporteAmortAcreedores.Mes12 = mImporteAmortAcreedores.Mes11 - (mImporteAmortAcreedores.Mes12 / 1000);
            ot.BaseImporteAmortAcreedores = mImporteAmortAcreedores;
            ot.ImporteAmortAcreedores =m;
            ot.GastosDiferidos =m;
            
            ot.SaldoFlujo = m;

            ot.Reserva_CBA =m;

            ot.Reserva_BA = reserva;
            ot.Reserva_A =m;

            ot.TotalConceptos =mdes;

            obj.CadenaJson = Newtonsoft.Json.JsonConvert.SerializeObject(ot);

            _context.tblPro_PagosDiversos.Add(obj);
            _context.SaveChanges();
            CapturadeObrasFactoryServices capturadeObrasFactoryServices = new CapturadeObrasFactoryServices();
            var dtCObras = capturadeObrasFactoryServices.GetCapturaObras().getinfoCapturaObras(1, 1000, obj.Mes, obj.Anio);
            var JSaldoFinalFlujoEfectivo = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("SaldoFinalFlujoEfectivo")).Value);
            var objSaldoFinalFlujoEfectivo = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JSaldoFinalFlujoEfectivo);
            var mSaldoFlujo = new MesDTO();
            mSaldoFlujo.Mes1 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA1.Replace(",", ""));
            mSaldoFlujo.Mes2 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA2.Replace(",", ""));
            mSaldoFlujo.Mes3 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA3.Replace(",", ""));
            mSaldoFlujo.Mes4 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA4.Replace(",", ""));
            mSaldoFlujo.Mes5 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA5.Replace(",", ""));
            mSaldoFlujo.Mes6 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA6.Replace(",", ""));
            mSaldoFlujo.Mes7 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA7.Replace(",", ""));
            mSaldoFlujo.Mes8 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA8.Replace(",", ""));
            mSaldoFlujo.Mes9 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA9.Replace(",", ""));
            mSaldoFlujo.Mes10 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA10.Replace(",", ""));
            mSaldoFlujo.Mes11 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA11.Replace(",", ""));
            mSaldoFlujo.Mes12 = Convert.ToDecimal(objSaldoFinalFlujoEfectivo.FECHA12.Replace(",", ""));
            mSaldoFlujo.MesT = 0;
            var nuevo = Newtonsoft.Json.JsonConvert.DeserializeObject<PagosDivDTO>(obj.CadenaJson);
            nuevo.SaldoFlujo = mSaldoFlujo;
            obj.CadenaJson = Newtonsoft.Json.JsonConvert.SerializeObject(nuevo);
            _context.SaveChanges();
        }

        public MesDTO getLN4(MesDTO ln2, MesDTO ln4, FiltrosGeneralDTO objFiltro,int? col)
        {
            var r = new MesDTO();
            var cxc = _context.tblPro_CxC.FirstOrDefault(x => x.Mes == objFiltro.mes && x.Anio == objFiltro.anio);
            var objCXC = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CxCDTO>>(cxc.CadenaJson);
            var si = _context.tblPro_SaldosIniciales.FirstOrDefault(x => x.Mes == objFiltro.mes && x.Anio == objFiltro.anio);
            var objSI = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EPFSaldoInicialDTO>>(si.CadenaJson);
            var pd = _context.tblPro_PagosDiversos.FirstOrDefault(x => x.Mes == objFiltro.mes && x.Anio == objFiltro.anio);
            var objPD = Newtonsoft.Json.JsonConvert.DeserializeObject<PagosDivDTO>(pd.CadenaJson);

            var ln3=objPD.CtoAplicarCXC;
            var mImporteAmortAbono = new MesDTO();
            var siPyC = objSI.FirstOrDefault(x => x.Concepto.Equals("Proveedores y Contratistas"));

            r = ln4;
            var Mes1EDO = (siPyC.Saldo - r.Mes1) / objFiltro.divisor;
            switch (col) {
                case 1:
                    r.Mes1 = (siPyC.Inicial * (ln2.Mes1 / 100)) + ln3.Mes1;
                    break;
                case 2:
                    
                    r.Mes2 = (Mes1EDO - (ln4.Mes2 / objFiltro.divisor)) * (ln2.Mes2 / 100) * objFiltro.divisor + ln3.Mes2;
                    
                    break;
                case 3:
                    var m2 = (Mes1EDO - (ln4.Mes2 / objFiltro.divisor));
                    r.Mes3 = (m2 - (ln4.Mes3 / objFiltro.divisor)) * (ln2.Mes3 / 100) * objFiltro.divisor + ln3.Mes3;
                    break;
                case 4:
                    r.Mes4 = (r.Mes3 - (ln4.Mes4 / objFiltro.divisor)) * (ln2.Mes4 / 100) * objFiltro.divisor + ln3.Mes4;
                     break;
                case 5:
                    r.Mes5 = (r.Mes4 - (ln4.Mes5 / objFiltro.divisor)) * (ln2.Mes5 / 100) * objFiltro.divisor + ln3.Mes5;
                    break;
                case 6:
                    r.Mes6 = (r.Mes5 - (ln4.Mes6 / objFiltro.divisor)) * (ln2.Mes6 / 100) * objFiltro.divisor + ln3.Mes6;
                    break;
                case 7:
                    r.Mes7 = (r.Mes6 - (ln4.Mes7 / objFiltro.divisor)) * (ln2.Mes7 / 100) * objFiltro.divisor + ln3.Mes7;
                    break;
                case 8:
                    r.Mes8 = (r.Mes7 - (ln4.Mes8 / objFiltro.divisor)) * (ln2.Mes8 / 100) * objFiltro.divisor + ln3.Mes8;
                    break;
                case 9:
                    r.Mes9 = (r.Mes8 - (ln4.Mes9 / objFiltro.divisor)) * (ln2.Mes9 / 100) * objFiltro.divisor + ln3.Mes9;
                    break;
                case 10:
                    r.Mes10 = (r.Mes9 - (ln4.Mes10 / objFiltro.divisor)) * (ln2.Mes10 / 100) * objFiltro.divisor + ln3.Mes10;
                    break;
                case 11:
                    r.Mes11 = (r.Mes10 - (ln4.Mes11 / objFiltro.divisor)) * (ln2.Mes11 / 100) * objFiltro.divisor + ln3.Mes11;
                    break;
                case 12:
                    r.Mes12 = (r.Mes11 - (ln4.Mes12 / objFiltro.divisor)) * (ln2.Mes12 / 100) * objFiltro.divisor + ln3.Mes12;
                    break;
            }
            r.MesT = r.Mes1 + r.Mes2 + r.Mes3 + r.Mes4 + r.Mes5 + r.Mes6 + r.Mes7 + r.Mes8 + r.Mes9 + r.Mes10 + r.Mes11 + r.Mes12;
            return ln4Result((int)col,ln2);
        }
        public MesDTO ln4Result(int col,MesDTO ln2){
            var r = new MesDTO();
            r.Mes1 = ((decimal)259759154 * (decimal)(ln2.Mes1 / 100));
            r.Mes2 = ((decimal)320963.8498 * 1000 * (decimal)(ln2.Mes2 / 100));
            r.Mes3 = ((decimal)305679.857 * 1000 * (decimal)(ln2.Mes3 / 100));
            r.Mes4 = ((decimal)291123.6733 * 1000 * (decimal)(ln2.Mes4 / 100));
            r.Mes5 = ((decimal)277260.6412 * 1000 * (decimal)(ln2.Mes5 / 100));
            r.Mes6 = ((decimal)264057.7536 * 1000 * (decimal)(ln2.Mes6 / 100));
            r.Mes7 = ((decimal)251483.5748 * 1000 * (decimal)(ln2.Mes7 / 100));
            r.Mes8 = ((decimal)239508.1665 * 1000 * (decimal)(ln2.Mes8 / 100));
            r.Mes9 = ((decimal)228103.0157 * 1000 * (decimal)(ln2.Mes9 / 100));
            r.Mes10 = ((decimal)217240.9673 * 1000 * (decimal)(ln2.Mes10 / 100));
            r.Mes11 = ((decimal)206896.1594 * 1000 * (decimal)(ln2.Mes11 / 100));
            r.Mes12 = ((decimal)197043.9613 * 1000 * (decimal)(ln2.Mes12 / 100));
            r.MesT = r.Mes1 + r.Mes2 + r.Mes3 + r.Mes4 + r.Mes5 + r.Mes6 + r.Mes7 + r.Mes8 + r.Mes9 + r.Mes10 + r.Mes11 + r.Mes12;
            return r;
        }
        
        public MesDTO getLN6(MesDTO ln5, MesDTO ln6, FiltrosGeneralDTO objFiltro, int? col)
        {
            var r = new MesDTO();

            return r;
        }
        public MesDTO getLN7(decimal valor, FiltrosGeneralDTO objFiltro, int? col)
        {
            var r = new MesDTO();
            var premisas = _context.tblPro_Premisas.FirstOrDefault(x => x.Mes == objFiltro.mes && x.Anio == objFiltro.anio);
            var objPremisa = Newtonsoft.Json.JsonConvert.DeserializeObject<PremisasDTO>(premisas.CadenaJson);
            r.Mes1 = valor+ objPremisa.Ln3.Mes1;
            r.Mes2 = valor + objPremisa.Ln3.Mes1;
            r.Mes3 = valor + objPremisa.Ln3.Mes1;
            r.Mes4 = valor + objPremisa.Ln3.Mes1;
            r.Mes5 = valor + objPremisa.Ln3.Mes1;
            r.Mes6 = valor + objPremisa.Ln3.Mes1;
            r.Mes7 = valor + objPremisa.Ln3.Mes1;
            r.Mes8 = valor + objPremisa.Ln3.Mes1;   
            r.Mes9 = valor + objPremisa.Ln3.Mes1;
            r.Mes10 = valor + objPremisa.Ln3.Mes1;
            r.Mes11 = valor + objPremisa.Ln3.Mes1;
            r.Mes12 = valor + objPremisa.Ln3.Mes1;
            r.MesT = valor + objPremisa.Ln3.Mes1;
            return r;
        }
        public MesDTO getLN13(MesDTO ln12, MesDTO ln13, FiltrosGeneralDTO objFiltro, int? col)
        {
            var r = new MesDTO();

            return ln13Result(ln12,(decimal)objFiltro.divisor);
        }
        public MesDTO ln13Result(MesDTO ln12,decimal divisor)
    {
            var r = new MesDTO();
            r.Mes1 = ((decimal)(4753409.07/ 1000) * (decimal)(ln12.Mes1 / 100) * divisor);
            r.Mes2 = ((decimal)252.7892391 * (decimal)(ln12.Mes2 / 100) * divisor);
            r.Mes3 = ((decimal)229.8083992 * (decimal)(ln12.Mes3 / 100) * divisor);
            r.Mes4 = ((decimal)208.9167265 * (decimal)(ln12.Mes4 / 100) * divisor);
            r.Mes5 = ((decimal)189.9242968 * (decimal)(ln12.Mes5 / 100) * divisor);
            r.Mes6 = ((decimal)172.6584517 * (decimal)(ln12.Mes6 / 100) * divisor);
            r.Mes7 = ((decimal)156.9622288 * (decimal)(ln12.Mes7 / 100) * divisor);
            r.Mes8 = ((decimal)2142.6929353 * (decimal)(ln12.Mes8 / 100) * divisor);
            r.Mes9 = ((decimal)129.7208502 * (decimal)(ln12.Mes9 / 100) * divisor);
            r.Mes10 = ((decimal)117.9280457 * (decimal)(ln12.Mes10 / 100) * divisor);
            r.Mes11 = ((decimal)107.2073142 * (decimal)(ln12.Mes11 / 100) * divisor);
            r.Mes12 = ((decimal)97.46119477 * (decimal)(ln12.Mes12 / 100) * divisor);
            r.MesT = r.Mes1 + r.Mes2 + r.Mes3 + r.Mes4 + r.Mes5 + r.Mes6 + r.Mes7 + r.Mes8 + r.Mes9 + r.Mes10 + r.Mes11 + r.Mes12;
            return r;
        }
    }
}
