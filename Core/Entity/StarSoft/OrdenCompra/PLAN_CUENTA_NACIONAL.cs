using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Core.Entity.StarSoft.OrdenCompra
{
    [Table("PLAN_CUENTA_NACIONAL")]
    public class PLAN_CUENTA_NACIONAL
    {
        [Key]
        public string PLANCTA_CODIGO { get; set; }
        public string PLANCTA_DESCRIPCION { get; set; }
        public Int16 PLANCTA_NIVEL { get; set; }
        public string TIPOANEX_CODIGO { get; set; }
        public bool PLANCTA_CENTCOST { get; set; }
        public string TIPOCTA_CODIGO { get; set; }
        public bool PLANCTA_AUTO { get; set; }
        public string PLANCTA_CARGO1 { get; set; }
        public string PLANCTA_CARGO2 { get; set; }
        public string PLANCTA_CARGO3 { get; set; }
        public string PLANCTA_ABONO1 { get; set; }
        public string PLANCTA_ABONO2 { get; set; }
        public string PLANCTA_ABONO3 { get; set; }
        public int PLANCTA_PORCENT1 { get; set; }
        public int PLANCTA_PORCENT2 { get; set; }
        public int PLANCTA_PORCENT3 { get; set; }
        public bool PLANCTA_AJUSTE { get; set; }
        public string PLANCTA_PARTIDA { get; set; }
        public string PLANCTA_DIF_CAMBIO { get; set; }
        public bool PLANCTA_MONETARIA { get; set; }
        public string PLANCTA_CON_COSTO { get; set; }
        public string PLANCTA_PLAN_EXTERIOR { get; set; }
        public string PLANCTA_ESTADO { get; set; }
        public string PLANCTA_COSTO_OBRA { get; set; }
        public string ESituacion { get; set; }
        public string EResultado { get; set; }
        public string EPatrimonio { get; set; }
        public string TSituacion { get; set; }
        public string TResultado { get; set; }
        public string TPatrimonio { get; set; }
        public string CSituacion { get; set; }
        public string CResultado { get; set; }
        public string CPatrimonio { get; set; }
        public string PLANCTA_CODIGO_DES { get; set; }
        public bool PLANCTA_FILE { get; set; }
        public string PLANCTA_CCONTROL { get; set; }
        public string PLANCTA_BIENSERV { get; set; }
        public string PLANCTA_MONEDA { get; set; }
        public string PLANCTA_DESCRIPCION_2020 { get; set; }
        public string PLANCTA_DESCRIPCION_ORI { get; set; }
        public bool PLANCTA_2019 { get; set; }
        public bool PLANCTA_2020 { get; set; }
    }
}
