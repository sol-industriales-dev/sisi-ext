using Core.DAO.Contabilidad.Banco;
using Core.DTO;
using Core.DTO.Enkontrol.Tablas.Banco;
using Core.DTO.Utils.Data;
using Data.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Enkontrol.General.Banco
{
    public class BancoEkDAO : IBancoDAO
    {
        public sb_cuentaDTO GetBanco(int cta, int scta, int sscta)
        {
            var query_sb_banco = new OdbcConsultaDTO();

            query_sb_banco.consulta =
                @"SELECT TOP 1
                    cuenta,
                    descripcion,
                    banco,
                    moneda
                FROM
                    dba.sb_cuenta
                WHERE
                    cta = ? AND
                    scta = ? AND
                    sscta = ?";

            query_sb_banco.parametros.Add(new OdbcParameterDTO
            {
                nombre = "cta",
                tipo = OdbcType.Int,
                valor = cta
            });
            query_sb_banco.parametros.Add(new OdbcParameterDTO
            {
                nombre = "scta",
                tipo = OdbcType.Int,
                valor = scta
            });
            query_sb_banco.parametros.Add(new OdbcParameterDTO
            {
                nombre = "sscta",
                tipo = OdbcType.Int,
                valor = sscta
            });

            return _contextEnkontrol.Select<sb_cuentaDTO>(vSesiones.sesionAmbienteEnkontrolAdm, query_sb_banco).FirstOrDefault();
        }
    }
}
