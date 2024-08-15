using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Alamcen
{
    public class AcumulaEnkontrolDTO
    {
        public int almacen { get; set; }
        public int insumo { get; set; }
        public int tipo { get; set; }
        public int grupo { get; set; }
        public string cc { get; set; }
        public int ano { get; set; }

        public decimal? ultimo_cp { get; set; }
        public DateTime? fecha_cp { get; set; }

        #region Inicio de Año
        public decimal? existencia_ent_ini { get; set; }
        public decimal? importe_ent_ini { get; set; }
        public decimal? existencia_sal_ini { get; set; }
        public decimal? importe_sal_ini { get; set; }
        #endregion

        #region Enero
        public decimal existencia_ent_ene { get; set; }
        public decimal importe_ent_ene { get; set; }
        public decimal existencia_sal_ene { get; set; }
        public decimal importe_sal_ene { get; set; }
        #endregion

        #region Febrero
        public decimal existencia_ent_feb { get; set; }
        public decimal importe_ent_feb { get; set; }
        public decimal existencia_sal_feb { get; set; }
        public decimal importe_sal_feb { get; set; }
        #endregion

        #region Marzo
        public decimal existencia_ent_mar { get; set; }
        public decimal importe_ent_mar { get; set; }
        public decimal existencia_sal_mar { get; set; }
        public decimal importe_sal_mar { get; set; }
        #endregion

        #region Abril
        public decimal existencia_ent_abr { get; set; }
        public decimal importe_ent_abr { get; set; }
        public decimal existencia_sal_abr { get; set; }
        public decimal importe_sal_abr { get; set; }
        #endregion

        #region Mayo
        public decimal existencia_ent_may { get; set; }
        public decimal importe_ent_may { get; set; }
        public decimal existencia_sal_may { get; set; }
        public decimal importe_sal_may { get; set; }
        #endregion

        #region Junio
        public decimal existencia_ent_jun { get; set; }
        public decimal importe_ent_jun { get; set; }
        public decimal existencia_sal_jun { get; set; }
        public decimal importe_sal_jun { get; set; }
        #endregion

        #region Julio
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

        #region Octubre
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
