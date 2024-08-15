using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft
{
    public class MAECLI
    {
        [Key]
        public string CCODCLI { get; set; }
        public string CNOMCLI { get; set; }
        public string CDIRCLI { get; set; }
        public string CTELEFO { get; set; }
        public string CNUMRUC { get; set; }
        public string CDOCIDEN { get; set; }
        public string CFLADES { get; set; }
        public decimal? NPORDES { get; set; }
        public string CTIPVTA { get; set; }
        public string CESTADO { get; set; }
        public DateTime? DFECINS { get; set; }
        public string CNOMREP { get; set; }
        public string CDISTRI { get; set; }
        public string CUSUARI { get; set; }
        public DateTime? DFECCRE { get; set; }
        public DateTime? DFECMOD { get; set; }
        public string CTIPPRE { get; set; }
        public string CVENDE { get; set; }
        public string CZONVTA { get; set; }
        public string CPAIS { get; set; }
        public string CDEPT { get; set; }
        public string CPROV { get; set; }
        public string DIRENT { get; set; }
        public string LOCENT { get; set; }
        public string LOCCLI { get; set; }
        public string LOCEST { get; set; }
        public string ZONVTA { get; set; }
        public string DIAATE { get; set; }
        public string SITCLI { get; set; }
        public string TIPIDE { get; set; }
        public string REGVTA { get; set; }
        public string MONCRE { get; set; }
        public decimal? LMCRUS { get; set; }
        public decimal? LMCRMN { get; set; }
        public decimal? SALDMN { get; set; }
        public decimal? SALDUS { get; set; }
        public decimal? LIMFAC { get; set; }
        public decimal? TOTFAC { get; set; }
        public decimal? TOTLEP { get; set; }
        public decimal? TOTCHD { get; set; }
        public string FULABO { get; set; }
        public string FECUFA { get; set; }
        public string NROSOL { get; set; }
        public string OBSERV { get; set; }
        public decimal? DOCVEN { get; set; }
        public decimal? DIAVEN { get; set; }
        public string NROORD { get; set; }
        public decimal? MORLET { get; set; }
        public decimal? MORFAC { get; set; }
        public decimal? CHEDEV { get; set; }
        public decimal? LETPRO { get; set; }
        public string CTIPCLI { get; set; }
        public string CGIRNEG { get; set; }
        public string CTERRIT { get; set; }
        public string CRUTA { get; set; }
        public string CSEGMEN { get; set; }
        public string CUBISEG { get; set; }
        public string CCODBAN { get; set; }
        public string CNUMCTA { get; set; }
        public string CFREVIS { get; set; }
        public string CHORATE { get; set; }
        public string CTIPATE { get; set; }
        public string CNUMFAX { get; set; }
        public string CEMAIL { get; set; }
        public string CHOST { get; set; }
        public string CZONPOS { get; set; }
        public string COMENTA { get; set; }
        public bool? RETEN { get; set; }
        public bool CFLAGPRIN { get; set; }
        public string CCODTRANS { get; set; }
        public bool? SIN_CONTROL_LIMCREDITO { get; set; }
        public string SUBCLA01 { get; set; }
        public string SUBCLA02 { get; set; }
        public string ZON_CODIGO { get; set; }
        public string CTIPO_DOCUMENTO { get; set; }
        public string CAPELLIDO_PATERNO { get; set; }
        public string CAPELLIDO_MATERNO { get; set; }
        public string CPRIMER_NOMBRE { get; set; }
        public string CSEGUNDO_NOMBRE { get; set; }
        public string TCL_CODIGO { get; set; }
        public string UBIGEO { get; set; }
        public DateTime? FEC_INACTIVO_BLOQUEADO { get; set; }
        public string COD_AUDITORIA { get; set; }
        public string NRO_AUTORIZACION_DIGEMID { get; set; }
        public bool? FLGPORTAL_CLIENTES { get; set; }
        public decimal? ULTIMO_TC_VTA { get; set; }
        public decimal? DESCUENTO_ESP { get; set; }
        public string CCODCLAS { get; set; }
        public string CONTACTO_COBRANZA { get; set; }
        public string CEMAIL_CONTACTO { get; set; }
    }
}
