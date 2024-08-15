using Core.DTO.Contabilidad.Poliza;
using Core.Entity.Administrativo.Contabilidad.FlujoEfectivo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.FlujoEfectivo
{
    public class AuxAnualDTO
    {
        public int year { get; set; }
        public int mes { get; set; }
        public int poliza { get; set; }
        public string tp { get; set; }
        public int linea { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public int digito { get; set; }
        public int tm { get; set; }
        public string referencia { get; set; }
        public string cc { get; set; }
        public string concepto { get; set; }
        public decimal monto { get; set; }
        public decimal cargo { get; set; }
        public decimal abono { get; set; }
        public int iclave { get; set; }
        public int itm { get; set; }
        public string orden_compra { get; set; }
        public int? numpro { get; set; }
        public decimal salini { get; set; }
        public decimal enecargos { get; set; }
        public decimal eneabonos { get; set; }
        public decimal febcargos { get; set; }
        public decimal febabonos { get; set; }
        public decimal marcargos { get; set; }
        public decimal marabonos { get; set; }
        public decimal abrcargos { get; set; }
        public decimal abrabonos { get; set; }
        public decimal maycargos { get; set; }
        public decimal mayabonos { get; set; }
        public decimal juncargos { get; set; }
        public decimal junabonos { get; set; }
        public decimal julcargos { get; set; }
        public decimal julabonos { get; set; }
        public decimal agocargos { get; set; }
        public decimal agoabonos { get; set; }
        public decimal sepcargos { get; set; }
        public decimal sepabonos { get; set; }
        public decimal octcargos { get; set; }
        public decimal octabonos { get; set; }
        public decimal novcargos { get; set; }
        public decimal novabonos { get; set; }
        public decimal diccargos { get; set; }
        public decimal dicabonos { get; set; }
        public decimal cierrecargos { get; set; }
        public decimal cierreabonos { get; set; }
        public string ccDesc { get; set; }
        public string ctaDesc { get; set; }
        public string sctaDesc { get; set; }
        public string ssctaDesc { get; set; }
        public int idConcepto { get; set; }
        public int Concepto { get; set; }
        public AuxAnualDTO()
        {

        }
        public AuxAnualDTO(string ccKey, CatctaDTO cat, SalContCcDTO sal)
        {
            cc = ccKey;
            #region cta
            cta = cat.cta;
            scta = cat.scta;
            sscta = cat.sscta;
            ctaDesc = cat.descripcion;
            #endregion
            #region sal
            year = sal.year;
            salini = sal.salini;
            enecargos = sal.enecargos;
            eneabonos = sal.eneabonos;
            febcargos = sal.febcargos;
            febabonos = sal.febabonos;
            marcargos = sal.marcargos;
            marabonos = sal.marabonos;
            abrcargos = sal.abrcargos;
            abrabonos = sal.abrabonos;
            maycargos = sal.maycargos;
            mayabonos = sal.mayabonos;
            juncargos = sal.juncargos;
            junabonos = sal.junabonos;
            julcargos = sal.julcargos;
            julabonos = sal.julabonos;
            agocargos = sal.agocargos;
            agoabonos = sal.agoabonos;
            sepcargos = sal.sepcargos;
            sepabonos = sal.sepabonos;
            octcargos = sal.octcargos;
            octabonos = sal.octabonos;
            novcargos = sal.novcargos;
            novabonos = sal.novabonos;
            diccargos = sal.diccargos;
            dicabonos = sal.dicabonos;
            cierrecargos = sal.cierrecargos;
            cierreabonos = sal.cierreabonos;
            #endregion
        }
        public AuxAnualDTO(CatctaDTO cat, SalContCcDTO sal, tblC_FE_MovPol mov)
        {
            #region cta
            cta = cat.cta;
            scta = cat.scta;
            sscta = cat.sscta;
            ctaDesc = cat.descripcion;
            #endregion
            #region sal
            year = sal.year;
            salini = sal.salini;
            enecargos = sal.enecargos;
            eneabonos = sal.eneabonos;
            febcargos = sal.febcargos;
            febabonos = sal.febabonos;
            marcargos = sal.marcargos;
            marabonos = sal.marabonos;
            abrcargos = sal.abrcargos;
            abrabonos = sal.abrabonos;
            maycargos = sal.maycargos;
            mayabonos = sal.mayabonos;
            juncargos = sal.juncargos;
            junabonos = sal.junabonos;
            julcargos = sal.julcargos;
            julabonos = sal.julabonos;
            agocargos = sal.agocargos;
            agoabonos = sal.agoabonos;
            sepcargos = sal.sepcargos;
            sepabonos = sal.sepabonos;
            octcargos = sal.octcargos;
            octabonos = sal.octabonos;
            novcargos = sal.novcargos;
            novabonos = sal.novabonos;
            diccargos = sal.diccargos;
            dicabonos = sal.dicabonos;
            cierrecargos = sal.cierrecargos;
            cierreabonos = sal.cierreabonos;
            #endregion
            #region mov
            mes = mov.mes;
            cc = mov.cc;
            poliza = mov.poliza;
            linea = mov.linea;
            monto = mov.monto;
            tm = mov.tm;
            itm = mov.itm;
            idConcepto = mov.idConcepto;
            cargo = mov.tm == 1 || mov.tm ==  3 ? mov.monto : 0;
            abono = mov.tm == 2 || mov.tm == 4 ? mov.monto : 0;
            #endregion
        }
    }
}
