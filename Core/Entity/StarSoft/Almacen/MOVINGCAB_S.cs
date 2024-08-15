using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.Almacen
{
    [Table("MOVINGCAB_S")]
    public class MOVINGCAB_S
    {
        public string CAALMA { get; set; }
        [Key, Column(Order = 0)]
        public string CATD { get; set; }
        [Key, Column(Order = 1)]
        public string CANUMDOC { get; set; }
        public DateTime CAFECDOC { get; set; }
        public string CATIPMOV { get; set; }
        public string CACODMOV { get; set; }
        public string CASITUA { get; set; }
        public string CARFTDOC { get; set; }
        public string CARFNDOC { get; set; }
        public string CASOLI { get; set; }
        public string CAFECDEV { get; set; }
        public string CACODPRO { get; set; }
        public string CACENCOS { get; set; }
        public string CARFALMA { get; set; }
        public string CAGLOSA { get; set; }
        public DateTime CAFECACT { get; set; }
        public string CAHORA { get; set; }
        public string CAUSUARI { get; set; }
        public string CACODCLI { get; set; }
        public string CARUC { get; set; }
        public string CANOMCLI { get; set; }
        public string CAFORVEN { get; set; }
        public string CACODMON { get; set; }
        public string CAVENDE { get; set; }
        public decimal CATIPCAM { get; set; }
        public string CATIPGUI { get; set; }
        public string CASITGUI { get; set; }
        public string CAGUIFAC { get; set; }
        public string CADIRENV { get; set; }
        public string CACODTRAN { get; set; }
        public string CANUMORD { get; set; }
        public string CAGUIDEV { get; set; }
        public string CANOMPRO { get; set; }
        public string CANROPED { get; set; }
        public string CACOTIZA { get; set; }
        public decimal CAPORDESCL { get; set; }
        public decimal CAPORDESES { get; set; }
        public decimal CAIMPORTE { get; set; }
        public string CANOMTRA { get; set; }
        public string CADIRTRA { get; set; }
        public string CARUCTRA { get; set; }
        public string CAPLATRA { get; set; }
        public string CANROIMP { get; set; }
        public string CACODLIQ { get; set; }
        public string CAESTIMP { get; set; }
        public string COD_AUDITORIA { get; set; }
    }
}
