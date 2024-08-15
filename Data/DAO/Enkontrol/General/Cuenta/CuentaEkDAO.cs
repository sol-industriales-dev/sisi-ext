using Core.DAO.Contabilidad.Cuenta;
using Core.DTO;
using Core.DTO.Enkontrol.Tablas.Cuenta;
using Core.DTO.Utils.Data;
using Data.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Utils;

namespace Data.DAO.Enkontrol.General.Cuenta
{
    public class CuentaEkDAO : ICuentaDAO
    {
        public object BuscarCuenta(string term)
        {
            var query_catcta = new OdbcConsultaDTO();

            query_catcta.consulta =
                @"SELECT
                    cta,
                    scta,
                    sscta,
                    descripcion,
                    digito
                FROM
                    dba.catcta
                WHERE
                    CAST(cta AS VARCHAR(4)) + '-' + CAST(scta AS VARCHAR(4)) + '-' + CAST(sscta AS VARCHAR(4)) + '-' + CAST(digito AS VARCHAR(4)) LIKE ?";

            query_catcta.parametros.Add(new OdbcParameterDTO
            {
                nombre = "relCuenta",
                tipo = OdbcType.VarChar,
                valor = "%" + term + "%"
            });

            return _contextEnkontrol.Select<catctaDTO>(vSesiones.sesionAmbienteEnkontrolAdm, query_catcta);
        }

        public object GetCuenta(int cta, int scta, int sscta)
        {
            var query_catcta = new OdbcConsultaDTO();

            query_catcta.consulta =
                @"SELECT
                    cta,
                    scta,
                    sscta,
                    descripcion,
                    digito
                FROM
                    dba.catcta
                WHERE
                    cta = ? AND
                    scta = ? AND
                    sscta = ?";

            query_catcta.parametros.Add(new OdbcParameterDTO
            {
                nombre = "cta",
                tipo = OdbcType.VarChar,
                valor = cta
            });
            query_catcta.parametros.Add(new OdbcParameterDTO
            {
                nombre = "scta",
                tipo = OdbcType.VarChar,
                valor = scta
            });
            query_catcta.parametros.Add(new OdbcParameterDTO
            {
                nombre = "sscta",
                tipo = OdbcType.VarChar,
                valor = sscta
            });

            return _contextEnkontrol.Select<catctaDTO>(vSesiones.sesionAmbienteEnkontrolAdm, query_catcta).FirstOrDefault();
        }

        public object GetCuenta(int cta, int scta, string descripcion)
        {
            var query_catcta = new OdbcConsultaDTO();

            query_catcta.consulta =
                @"SELECT
                    cta,
                    scta,
                    sscta,
                    descripcion,
                    digito
                FROM
                    dba.catcta
                WHERE
                    cta = ? AND
                    scta = ? AND
                    descripcion LIKE ?";

            query_catcta.parametros.Add(new OdbcParameterDTO
            {
                nombre = "cta",
                tipo = OdbcType.Int,
                valor = cta
            });
            query_catcta.parametros.Add(new OdbcParameterDTO
            {
                nombre = "scta",
                tipo = OdbcType.Int,
                valor = scta
            });
            query_catcta.parametros.Add(new OdbcParameterDTO
            {
                nombre = "descripcion",
                tipo = OdbcType.VarChar,
                valor = "%" + descripcion + "%"
            });

            return _contextEnkontrol.Select<catctaDTO>(vSesiones.sesionAmbienteEnkontrolAdm, query_catcta).FirstOrDefault();
        }

        public object GetCuentas(List<int> ctas)
        {
            var query_catcta = new OdbcConsultaDTO();

            query_catcta.consulta =
                string.Format(
                @"SELECT
                    cta,
                    scta,
                    sscta,
                    descripcion,
                    digito
                FROM
                    dba.catcta
                WHERE
                    cta in {0}", ctas.ToParamInValue());

            foreach (var cta in ctas)
            {
                query_catcta.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "cta",
                    tipo = OdbcType.Int,
                    valor = cta
                });
            }

            return _contextEnkontrol.Select<catctaDTO>(vSesiones.sesionAmbienteEnkontrolAdm, query_catcta);
        }
    }
}
