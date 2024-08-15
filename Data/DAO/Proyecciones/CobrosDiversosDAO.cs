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
using Newtonsoft.Json;
namespace Data.DAO.Proyecciones
{
    public class CobrosDiversosDAO : GenericDAO<tblPro_CobrosDiversos>, ICobrosDiversosDAO
    {
        public tblPro_CobrosDiversos GetJsonData(FiltrosGeneralDTO filtro)
        {
            var m= new MesDTO();
            m.Mes1=0;
            m.Mes2=0;
            m.Mes3=0;
            m.Mes4=0;
            m.Mes5=0;
            m.Mes6=0;
            m.Mes7=0;
            m.Mes8=0;
            m.Mes9=0;
            m.Mes10=0;
            m.Mes11=0;
            m.Mes12=0;
            m.MesT=0;


            var res = new tblPro_CobrosDiversos();
            if (filtro.mes == (DateTime.Now.Month - 1))
            {
                
                var temp = _context.tblPro_CobrosDiversos.FirstOrDefault(x => x.Anio == filtro.anio && x.Mes == filtro.mes);
                if (temp != null) {
                    res = temp;
                }
                else
                {
                    CxCFactoryServices cxCFactoryServices = new CxCFactoryServices();
                    var cxc= cxCFactoryServices.GetCxC().GetJsonData(filtro.escenario, filtro.mes, filtro.anio);
                    SaldosInicialesFactoryServices saldosInicialesFactoryServices=new SaldosInicialesFactoryServices();
                    var si=saldosInicialesFactoryServices.GetSaldosIniciales().GetJsonData(filtro.mes, filtro.anio, 0);;

                    var cd = new CobrosDivDTO();
                    cd.ln1CliPorcentajeSaldoAmortizar = new MesDTO();
                    cd.ln1CliPorcentajeSaldoAmortizar=m;

                    cd.ln2ImporteAmortizar1 = new MesDTO();
                    #region CXC
                    if(cxc!=null){
                        var objCXC = JsonConvert.DeserializeObject<List<CxCDTO>>(cxc.CadenaJson);
                        cd.ln2ImporteAmortizar1.Mes1 = objCXC.Sum(x=>x.MESP1);
                        cd.ln2ImporteAmortizar1.Mes2 = objCXC.Sum(x => x.MESP2);
                        cd.ln2ImporteAmortizar1.Mes3 = objCXC.Sum(x => x.MESP3);
                        cd.ln2ImporteAmortizar1.Mes4 = objCXC.Sum(x => x.MESP4);
                        cd.ln2ImporteAmortizar1.Mes5 = objCXC.Sum(x => x.MESP5);
                        cd.ln2ImporteAmortizar1.Mes6 = objCXC.Sum(x => x.MESP6);
                        cd.ln2ImporteAmortizar1.Mes7 = objCXC.Sum(x => x.MESP7);
                        cd.ln2ImporteAmortizar1.Mes8 = objCXC.Sum(x => x.MESP8);
                        cd.ln2ImporteAmortizar1.Mes9 = objCXC.Sum(x => x.MESP9);
                        cd.ln2ImporteAmortizar1.Mes10 = objCXC.Sum(x => x.MESP10);
                        cd.ln2ImporteAmortizar1.Mes11 = objCXC.Sum(x => x.MESP11);
                        cd.ln2ImporteAmortizar1.Mes12 = objCXC.Sum(x => x.MESP12);
                        cd.ln2ImporteAmortizar1.MesT = objCXC.Sum(x => x.MESP1+x.MESP2+x.MESP3+x.MESP4+x.MESP5+x.MESP6+x.MESP7+x.MESP8+x.MESP9+x.MESP10+x.MESP11+x.MESP12);
                    }
                    else{
                        cd.ln2ImporteAmortizar1=m;
                    }
                    #endregion
                    cd.ln3ImporteAmortizar2 = new MesDTO();
                    #region CXC
                    if(cxc!=null){
                        var objCXC = JsonConvert.DeserializeObject<List<CxCDTO>>(cxc.CadenaJson);
                        cd.ln2ImporteAmortizar1.Mes1 = objCXC.Sum(x=>x.MESP1);
                        cd.ln2ImporteAmortizar1.Mes2 = objCXC.Sum(x => x.MESP2);
                        cd.ln2ImporteAmortizar1.Mes3 = objCXC.Sum(x => x.MESP3);
                        cd.ln2ImporteAmortizar1.Mes4 = objCXC.Sum(x => x.MESP4);
                        cd.ln2ImporteAmortizar1.Mes5 = objCXC.Sum(x => x.MESP5);
                        cd.ln2ImporteAmortizar1.Mes6 = objCXC.Sum(x => x.MESP6);
                        cd.ln2ImporteAmortizar1.Mes7 = objCXC.Sum(x => x.MESP7);
                        cd.ln2ImporteAmortizar1.Mes8 = objCXC.Sum(x => x.MESP8);
                        cd.ln2ImporteAmortizar1.Mes9 = objCXC.Sum(x => x.MESP9);
                        cd.ln2ImporteAmortizar1.Mes10 = objCXC.Sum(x => x.MESP10);
                        cd.ln2ImporteAmortizar1.Mes11 = objCXC.Sum(x => x.MESP11);
                        cd.ln2ImporteAmortizar1.Mes12 = objCXC.Sum(x => x.MESP12);
                        cd.ln2ImporteAmortizar1.MesT = objCXC.Sum(x => x.MESP1+x.MESP2+x.MESP3+x.MESP4+x.MESP5+x.MESP6+x.MESP7+x.MESP8+x.MESP9+x.MESP10+x.MESP11+x.MESP12);
                    }
                    else{
                        cd.ln2ImporteAmortizar1=m;
                    }
                    #endregion
                    cd.ln4CxCPorcentajeSaldoAmortizar = new MesDTO();
                    cd.ln4CxCPorcentajeSaldoAmortizar=m;

                    cd.ln5CxCImporteAmortizar = new MesDTO();
                    //capturadeObrasFactoryServices.GetCapturaObras().getinfoCapturaObras(pEscenario, pDivisor, pMes, pAnio);
                    //var JSaldoFinalFlujoEfectivo = Newtonsoft.Json.JsonConvert.SerializeObject(dtCObras.FirstOrDefault(x => x.Key.Equals("SaldoFinalFlujoEfectivo")).Value);
                    //var objSaldoFinalFlujoEfectivo = Newtonsoft.Json.JsonConvert.DeserializeObject<ConjuntoDatosDTO1>(JSaldoFinalFlujoEfectivo);


                    cd.ln6AporteCapital = new MesDTO();
                    cd.ln6AporteCapital=m;

                }
                
            }
            else
            {
                res = _context.tblPro_CobrosDiversos.FirstOrDefault(x => x.Anio == filtro.anio && x.Mes == filtro.mes);
            }
            if (res.CadenaJson == null)
            {
                var cxc = _context.tblPro_CxC.FirstOrDefault(x => x.Anio == filtro.anio && x.Mes == filtro.mes);
                if (cxc != null)
                {
                    var cxcData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CxCDTO>>(cxc.CadenaJson);
                    var temp = new CobrosDivDTO();
                    temp.ln1CliPorcentajeSaldoAmortizar = new MesDTO();
                    temp.ln1CliPorcentajeSaldoAmortizar = m;

                    temp.ln2ImporteAmortizar1 = new MesDTO();

                    var m2 = new MesDTO();
                    m2.Mes1 = cxcData.Sum(x=>x.MESP1);
                    m2.Mes2 = cxcData.Sum(x => x.MESP2);
                    m2.Mes3 = cxcData.Sum(x => x.MESP3);
                    m2.Mes4 = cxcData.Sum(x => x.MESP4);
                    m2.Mes5 = cxcData.Sum(x => x.MESP5);
                    m2.Mes6 = cxcData.Sum(x => x.MESP6);
                    m2.Mes7 = cxcData.Sum(x => x.MESP7);
                    m2.Mes8 = cxcData.Sum(x => x.MESP8);
                    m2.Mes9 = cxcData.Sum(x => x.MESP9);
                    m2.Mes10 = cxcData.Sum(x => x.MESP10);
                    m2.Mes11 = cxcData.Sum(x => x.MESP11);
                    m2.Mes12 = cxcData.Sum(x => x.MESP12);
                    m2.MesT = m2.Mes1 + m2.Mes2 + m2.Mes3 + m2.Mes4 + m2.Mes5 + m2.Mes6 + m2.Mes7 + m2.Mes8 + m2.Mes9 + m2.Mes10 + m2.Mes11 + m2.Mes12;
                    temp.ln2ImporteAmortizar1 = m2;

                    temp.ln3ImporteAmortizar2 = new MesDTO();
                    temp.ln3ImporteAmortizar2 = m2;


                    temp.ln4CxCPorcentajeSaldoAmortizar = new MesDTO();
                    temp.ln4CxCPorcentajeSaldoAmortizar = m;
                    temp.ln5CxCImporteAmortizar = new MesDTO();
                    temp.ln5CxCImporteAmortizar = m;

                    temp.ln6AporteCapital = new MesDTO();
                    temp.ln6AporteCapital = m;
                    var nuevo = new tblPro_CobrosDiversos();
                    nuevo.Estatus = true;
                    nuevo.Mes = filtro.mes;
                    nuevo.Anio = filtro.anio;
                    nuevo.CadenaJson = Newtonsoft.Json.JsonConvert.SerializeObject(nuevo);
                    GuardarActualizarCobrosDiversos(filtro, temp);
                    var nres = _context.tblPro_CobrosDiversos.FirstOrDefault(x => x.Anio == filtro.anio && x.Mes == filtro.mes);
                    return nres;
                }
                else 
                    return new tblPro_CobrosDiversos();
            }
            else
            {
                
                return res;
            }
        }
        public void GuardarActualizarCobrosDiversos(FiltrosGeneralDTO objFiltro, CobrosDivDTO obj)
        {
            var temp = _context.tblPro_CobrosDiversos.FirstOrDefault(x => x.Mes == objFiltro.mes && x.Anio == objFiltro.anio && x.Estatus);
            if (temp != null)
            {
                temp.CadenaJson = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                _context.SaveChanges();
            }
            else
            {
                var o = new tblPro_CobrosDiversos();
                o.Mes = objFiltro.mes;
                o.Anio = objFiltro.anio;
                o.Estatus = true;
                o.CadenaJson = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                _context.tblPro_CobrosDiversos.Add(o);
                _context.SaveChanges();
            }
        }

    }
}
