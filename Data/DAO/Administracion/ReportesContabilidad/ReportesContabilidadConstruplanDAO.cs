using Core.DAO.ReportesContabilidad;
using Core.DTO.Principal.Generales;
using System.Linq;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using Core.DTO.Administracion.Facultamiento;
using Data.EntityFramework.Context;
using System.Data.Entity;
using Core.DTO;
using Core.Enum.Principal.Bitacoras;
using Infrastructure.Utils;
using Data.DAO.Principal.Menus;
using Core.Enum.Administracion.Facultamiento;
using Core.Entity.Principal.Alertas;
using Core.Enum.Principal.Alertas;
using Core.Entity.Principal.Multiempresa;
using Core.Enum.Principal;
using Core.Entity.Principal.Usuarios;
using Core.DTO.Utils.Data;
using Core.Enum.Multiempresa;
using Core.Entity.Administrativo.Contabilidad.Nomina;
using Core.DTO.ReportesContabilidad;
using Core.DTO.Enkontrol.Tablas.Cuenta;

namespace Data.DAO.ReportesContabilidad
{
    public class ReportesContabilidadConstruplanDAO : GenericDAO<tblP_CC>, IReportesContabilidadDAO
    {
        #region variables
        // Variables a utilizar en los diccionarios de resultados.
        public readonly string SUCCESS = "success";
        public readonly string ERROR = "error";

        private readonly string NOMBRE_CONTROLADOR = "ReportesContabilidadController";
        Dictionary<string, object> resultado = new Dictionary<string, object>();
        private string NombreControlador = "ReportesContabilidadController";
        #endregion


        public Dictionary<string, object> cargarAuxiliarEnkontrol(DateTime fechaInicio, DateTime fechaFin, string ctaInicio, string ctaFin, List<string> areaCuenta)
        {
            resultado = new Dictionary<string, object>();
            var anioInicio = fechaInicio.Year;
            var anioFin = fechaFin.Year;
            var mesInicio = fechaInicio.Month;
            var mesFin = fechaFin.Month;

            List<AuxiliarEnkontrolDTO> data = new List<AuxiliarEnkontrolDTO>();
            try
            {
                var consulta = string.Format(@"
                    SELECT 
                        A.year, 
                        A.mes, 
                        A.poliza, 
                        A.tp, 
                        A.linea, 
                        A.cta, 
                        A.scta, 
                        A.sscta, 
                        A.referencia, 
                        A.cc, 
                        A.concepto, 
                        CASE WHEN (A.tm = 1 or A.tm = 3) THEN A.monto ELSE 0 END as cargo, 
                        CASE WHEN (A.tm = 2 or A.tm = 4) THEN A.monto ELSE 0 END as abono, 
                        A.itm, 
                        CAST(A.area AS VARCHAR) + '-' + CAST(A.cuenta_oc AS VARCHAR) as areaCuenta, 
                        B.fechapol
                    FROM sc_movpol A 
                    LEFT JOIN sc_polizas B 
                    ON A.year = B.year AND A.mes = B.mes AND A.poliza = B.poliza AND A.tp = B.tp 
                    WHERE 
                        A.year BETWEEN {0} AND {1} 
                        AND A.mes BETWEEN {2} AND {3} 
                        AND A.cta  BETWEEN {4} AND {5} 
                        AND A.scta  BETWEEN {6} AND {7} 
                        AND A.sscta  BETWEEN {8} AND {9}
                        AND CAST(A.area AS VARCHAR) + '-' + CAST(A.cuenta_oc AS VARCHAR) = '{10}'

                ", anioInicio, anioFin, mesInicio, mesFin, ctaInicio, ctaFin);

                data = (List<AuxiliarEnkontrolDTO>)_contextEnkontrol.Where(consulta).ToObject<List<AuxiliarEnkontrolDTO>>();
                data.ForEach(x => x.fechapolStr = x.fechapol.ToShortDateString());

                resultado.Add("items", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }


        public Dictionary<string, object> getListaAC()
        {
            resultado = new Dictionary<string, object>();

            List<ComboDTO> data = new List<ComboDTO>();

            try
            {
                data = _context.tblP_CC.Where(x => x.estatus == true && x.areaCuenta != "0" && x.areaCuenta != "0-0").Select(x => new ComboDTO
                {
                    Value = x.areaCuenta,
                    Text = "[" + x.areaCuenta + "] - " + x.descripcion
                }).ToList();
                resultado.Add("ITEMS", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(ERROR, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public Dictionary<string, object> GetCuentas(string term)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                var query_catcta = new OdbcConsultaDTO();
                query_catcta.consulta = string.Format(
                    @"SELECT
                        cta, scta, sscta, descripcion, digito
                    FROM
                        dba.catcta"
                );
                var data = _contextEnkontrol.Select<catctaDTO>(vSesiones.sesionAmbienteEnkontrolAdm, query_catcta);
                var combo = data.Select(m => new Core.DTO.Principal.Generales.ComboDTO
                {
                    Value = m.cta + "-" + m.scta + "-" + m.sscta,
                    Text = "[" + m.cta + "-" + m.scta + "-" + m.sscta + "] " + m.descripcion
                });

                resultado.Add("items", combo);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(ERROR, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }


    }
}