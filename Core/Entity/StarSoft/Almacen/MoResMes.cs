using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.Almacen
{
    [Table("MoResMes")]
    public class MoResMes
    {
        [Key, Column(Order = 0)]
        public string SMALMA { get; set; }
        [Key, Column(Order = 1)]
        public string SMCODIGO { get; set; }
        [Key, Column(Order = 2)]
        public string SMMESPRO { get; set; }
        public decimal? SMUSPREUNI { get; set; }
        public decimal? SMMNPREUNI { get; set; }
        public decimal? SMUSPREANT { get; set; }
        public decimal? SMMNPREANT { get; set; }
        public string SMULTMOV { get; set; }
        public decimal? SMCANENT { get; set; }
        public decimal? SMCANSAL { get; set; }
        public decimal? SMANTCAN { get; set; }
        public decimal? SMACTCAN { get; set; }
        public decimal? SMMNANTVAL { get; set; }
        public decimal? SMMNACTVAL { get; set; }
        public decimal? SMUSANTVAL { get; set; }
        public decimal? SMUSACTVAL { get; set; }
        public decimal? SMUSENT { get; set; }
        public decimal? SMMNENT { get; set; }
        public int? SMUSSAL { get; set; }
        public decimal? SMMNSAL { get; set; }
        public string SMCUENTA { get; set; }
        public string SMGRUPO { get; set; }
        public string SMFAMILIA { get; set; }
        public string SMLINEA { get; set; }
        public string SMTIPO { get; set; }
        public decimal? SMSALDOINI { get; set; }
        public string COD_MODULO { get; set; }
        public string COD_OPCION { get; set; }

    }
}
