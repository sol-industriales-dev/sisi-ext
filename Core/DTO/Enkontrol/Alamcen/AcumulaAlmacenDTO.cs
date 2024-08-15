using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Alamcen
{
    public class AcumulaAlmacenDTO
    {
        public int almacen { get; set; }
        public int insumo { get; set; }
        public int tipo { get; set; }
        public int grupo { get; set; }
        public string cc { get; set; }
        public int ano { get; set; }
        #region inicia año
        public decimal? existencia_ent_ini { get; set; }
        public decimal? importe_ent_ini { get; set; }
        public decimal? existencia_sal_ini { get; set; }
        public decimal? importe_sal_ini { get; set; }
        #endregion
        #region enero
        public decimal existencia_ent_ene { get; set; }
        public decimal importe_ent_ene { get; set; }
        public decimal existencia_sal_ene { get; set; }
        public decimal importe_sal_ene { get; set; }
        #endregion
        #region febrero
        public decimal existencia_ent_feb { get; set; }
        public decimal importe_ent_feb { get; set; }
        public decimal existencia_sal_feb { get; set; }
        public decimal importe_sal_feb { get; set; }
        #endregion
        #region marzo
        public decimal existencia_ent_mar { get; set; }
        public decimal importe_ent_mar { get; set; }
        public decimal existencia_sal_mar { get; set; }
        public decimal importe_sal_mar { get; set; }
        #endregion
        #region abril
        public decimal existencia_ent_abr { get; set; }
        public decimal importe_ent_abr { get; set; }
        public decimal existencia_sal_abr { get; set; }
        public decimal importe_sal_abr { get; set; }
        #endregion
        #region mayo
        public decimal existencia_ent_may { get; set; }
        public decimal importe_ent_may { get; set; }
        public decimal existencia_sal_may { get; set; }
        public decimal importe_sal_may { get; set; }
        #endregion
        #region junio
        public decimal existencia_ent_jun { get; set; }
        public decimal importe_ent_jun { get; set; }
        public decimal existencia_sal_jun { get; set; }
        public decimal importe_sal_jun { get; set; }
        #endregion
        #region julio
        public decimal existencia_ent_jul { get; set; }
        public decimal importe_ent_jul { get; set; }
        public decimal existencia_sal_jul { get; set; }
        public decimal importe_sal_jul { get; set; }
        #endregion
        #region Agosto
        public decimal existencia_ent_ago { get; set; }
        public decimal importe_ent_ago { get; set; }
        public decimal existencia_sal_ago { get; set; }
        public decimal importe_sal_ago { get; set; }
        #endregion
        #region Septiembre
        public decimal existencia_ent_sep { get; set; }
        public decimal importe_ent_sep { get; set; }
        public decimal existencia_sal_sep { get; set; }
        public decimal importe_sal_sep { get; set; }
        #endregion
        #region octubre
        public decimal existencia_ent_oct { get; set; }
        public decimal importe_ent_oct { get; set; }
        public decimal existencia_sal_oct { get; set; }
        public decimal importe_sal_oct { get; set; }
        #endregion
        #region Noviembre
        public decimal existencia_ent_nov { get; set; }
        public decimal importe_ent_nov { get; set; }
        public decimal existencia_sal_nov { get; set; }
        public decimal importe_sal_nov { get; set; }
        #endregion
        #region Diciembre
        public decimal existencia_ent_dic { get; set; }
        public decimal importe_ent_dic { get; set; }
        public decimal existencia_sal_dic { get; set; }
        public decimal importe_sal_dic { get; set; }
        #endregion
        
    }
}
