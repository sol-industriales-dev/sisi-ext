using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.Requisiciones
{
    [Table("COMOVC")]
    public class COMOVC
    {
        [Key]
        public string OC_CNUMORD { get; set; }
        public DateTime OC_DFECDOC { get; set; }
        public string OC_CCODPRO { get; set; }
        public string OC_CRAZSOC { get; set; }
        public string OC_CDIRPRO { get; set; }
        public string OC_CCOTIZA { get; set; }
        public string OC_CCODMON { get; set; }
        public string OC_CFORPAG { get; set; }
        public decimal OC_NTIPCAM { get; set; }
        public DateTime OC_DFECENT { get; set; }
        public string OC_COBSERV { get; set; }
        public string OC_CSOLICT { get; set; }
        public string OC_CTIPENV { get; set; }
        public string OC_CENTREG { get; set; }
        public string OC_CSITORD { get; set; }
        public decimal OC_NIMPORT { get; set; }
        public decimal OC_NDESCUE { get; set; }
        public decimal OC_NIGV { get; set; }
        public decimal OC_NVENTA { get; set; }
        public DateTime OC_DFECACT { get; set; }
        public string OC_CHORA { get; set; }
        public string OC_CUSUARI { get; set; }
        public string OC_CFECDOC { get; set; }
        public string OC_CCONVER { get; set; }
        public string OC_CFACNOMBRE { get; set; }
        public string OC_CFACRUC { get; set; }
        public string OC_CFACDIREC { get; set; }
        public string OC_CDOCREF { get; set; }
        public string OC_CNRODOCREF { get; set; }
        public string OC_CTACTE_PROV { get; set; }
        public string BCO_CTACTE_PROV { get; set; }
        public string BCO_DESCRIPCION { get; set; }
        public string OC_ORDFAB { get; set; }
        public string OC_DOCORIGEN { get; set; }
        public decimal OC_NIMPPERC { get; set; }
        public string OC_NOMTRAN { get; set; }
        public string OC_RUCTRAN { get; set; }
        public string OC_DIRTRAN { get; set; }
        public string OC_TELFTRAN { get; set; }
        public string OC_CONTTRAN { get; set; }
        public string OC_DESTTRAN { get; set; }
        public string OC_SOLICITA { get; set; }
        public string OC_CTIPOC { get; set; }
        public decimal OC_NFLETE { get; set; }
        public decimal OC_NSEGURO { get; set; }
        public string OC_DESPACHO { get; set; }
        public string COD_AUDITORIA { get; set; }
        public string TIPO_USUARIO { get; set; }
        public string NOMBRE_USUARIO { get; set; }
        public string CARGO_USUARIO { get; set; }
        //public int ARCHIVO_FIRMA { get; set; }
        public string ESTADO_PROVEEDOR { get; set; }
        public string GLOSA_PROVEEDOR { get; set; }
        public string RESPUESTA_PROVEEDOR { get; set; }
        public string FIRMA_PROVEEDOR { get; set; }
        public DateTime? FECHA_HORA_PROVEEDOR { get; set; }
        public DateTime FECHAHORA_CAMBIOESTADO { get; set; }
        public string COD_FP { get; set; }
        public string TipoDocumento { get; set; } 
    }
}
