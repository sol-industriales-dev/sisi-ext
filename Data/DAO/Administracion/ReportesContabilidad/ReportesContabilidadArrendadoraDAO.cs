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
using System.Data.Odbc;
using Core.DTO.Enkontrol.Tablas.Cuenta;

namespace Data.DAO.ReportesContabilidad
{
    class ReportesContabilidadArrendadoraDAO : GenericDAO<tblP_CC>, IReportesContabilidadDAO
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

            List<string> _ctaInicioSplit = new List<string>() { "0", "0", "0" };
            List<string> _ctaFinSplit = new List<string>() { "9999", "9999", "9999" };

            if (ctaInicio != "") { _ctaInicioSplit = ctaInicio.Split('-').ToList(); }
            if (ctaFin != "") { _ctaFinSplit = ctaFin.Split('-').ToList(); }

            if (areaCuenta == null) areaCuenta = new List<string>();
            

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
                        B.fechapol BETWEEN ? AND ?
                        AND 
                        (
                            ({0} = {1} AND {2} = {2} AND {4} = {4} AND (A.cta = {0} AND A.scta = {2} AND A.sscta = {4}))
                            OR ({0} = {1} AND {2} = {3} AND {4} != {5} AND (A.cta = {0} AND A.scta = {2} AND A.sscta BETWEEN {4} AND {5}))
                            OR ({0} = {1} AND {2} != {3} AND ((A.cta = {0} AND A.scta = {2} AND A.sscta >= {4}) OR (A.cta = {0} AND A.scta > {2} AND A.scta < {3}) OR (A.cta = {0} AND A.scta = {3} AND A.sscta <= {5})))                
                            OR({0} != {1} AND ((A.cta = {0} AND A.scta = {2} AND A.sscta >= {4}) OR (A.cta = {0} AND A.scta > {2}) OR (A.cta > {0} AND A.cta < {1}) OR (A.cta = {1} AND A.scta < {3}) OR (A.cta = {1} AND A.scta = {3} AND A.sscta <= {5})))
                        )
                        {6}

                ", _ctaInicioSplit[0], _ctaFinSplit[0], _ctaInicioSplit[1], _ctaFinSplit[1], _ctaInicioSplit[2], _ctaFinSplit[2], (areaCuenta.Count() > 0 ? "AND CAST(A.area AS VARCHAR) + '-' + CAST(A.cuenta_oc AS VARCHAR) IN " + areaCuenta.ToParamInValue() : ""));

                List<OdbcParameterDTO> parametros = new List<OdbcParameterDTO>();

                parametros.Add(new OdbcParameterDTO { nombre = "fechapol", tipo = OdbcType.Date, valor = fechaInicio });
                parametros.Add(new OdbcParameterDTO { nombre = "fechapol", tipo = OdbcType.Date, valor = fechaFin });
                parametros.AddRange(areaCuenta.Select(x => new OdbcParameterDTO { nombre = "areaCuenta", tipo = OdbcType.NVarChar, valor = x }).ToList());

                OdbcConsultaDTO odbc = new OdbcConsultaDTO() { 
                    consulta = consulta,
                    parametros = parametros
                };

                data = _contextEnkontrol.Select<AuxiliarEnkontrolDTO>(EnkontrolAmbienteEnum.ProdARREND, odbc);
                data.ForEach(x => x.fechapolStr = x.fechapol.ToShortDateString());
            
                resultado.Add(ITEMS, data);
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
                data = _context.tblP_CC.Where(x => x.estatus == true && x.areaCuenta != "0" && x.areaCuenta != "0-0").Select(x => new ComboDTO { 
                    Value = x.areaCuenta,
                    Text = "[" + x.areaCuenta + "] - " + x.descripcion
                }).ToList();
                resultado.Add("items", data);
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
                        dba.catcta
                    WHERE 
                        ('[' + CAST(cta as varchar) + '-' + CAST(scta as varchar) + '-' + CAST(sscta as varchar) + '] ' + descripcion) like '%{0}%'"
                    , term
                );
                var data = _contextEnkontrol.Select<catctaDTO>(vSesiones.sesionAmbienteEnkontrolAdm, query_catcta);
                var combo = data.Select(m => new Core.DTO.Principal.Generales.ComboDTO
                {
                    Value = m.cta + "-" + m.scta + "-" + m.sscta,
                    Text = "[" + m.cta + "-" + m.scta + "-" + m.sscta + "] " + m.descripcion
                });

                resultado.Add(ITEMS, combo);
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
