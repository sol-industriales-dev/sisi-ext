using Core.DAO.Enkontrol.General.CC;
using Core.DTO;
using Core.DTO.Enkontrol.Tablas.CC;
using Core.DTO.Utils.Data;
using Core.Enum.Multiempresa;
using Data.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Utils;

namespace Data.DAO.Enkontrol.General.CC
{
    public class CCDAO : ICCDAO
    {
        public List<ccDTO> GetCCs()
        {
            var query_cc = new OdbcConsultaDTO();

            query_cc.consulta =
                @"SELECT
                    *
                FROM
                    dba.cc";

            return _contextEnkontrol.Select<ccDTO>(vSesiones.sesionAmbienteEnkontrolAdm, query_cc);
        }

        public List<ccDTO> GetCCs(List<string> ccs)
        {
            var query_cc = new OdbcConsultaDTO();

            query_cc.consulta = string.Format(
                @"SELECT
                    *
                FROM
                    dba.cc
                WHERE
                    cc in {0}", ccs.ToParamInValue());

            query_cc.parametros.AddRange(ccs.Select(x => new OdbcParameterDTO
            {
                nombre = "cc",
                tipo = OdbcType.NVarChar,
                valor = x
            }).ToList());

            return _contextEnkontrol.Select<ccDTO>(vSesiones.sesionAmbienteEnkontrolAdm, query_cc);
        }

        public ccDTO GetCC(string cc)
        {
            var query_cc = new OdbcConsultaDTO();

            query_cc.consulta =
                @"SELECT TOP 1
                    *
                FROM
                    dba.cc
                WHERE
                    cc = ?";

            query_cc.parametros.Add(new OdbcParameterDTO
            {
                nombre = "cc",
                tipo = OdbcType.Char,
                valor = cc
            });

            return _contextEnkontrol.Select<ccDTO>(vSesiones.sesionAmbienteEnkontrolAdm, query_cc).FirstOrDefault();
        }

        public ccDTO GetCCNomina(string cc)
        {
            return null;
        }

        public List<ccDTO> GetCCsNomina(bool? activos)
        {
            return null;
        }

        public List<ccDTO> GetCCsNominaFiltrados(List<string> ccs)
        {
            return null;
        }

        public List<ccDTO> GetCCsNominaInactivos()
        {
            return null;
        }

        public List<ccDTO> GetCCsNominaInactivos(List<string> ccs)
        {
            return null;
        }
    }
}
