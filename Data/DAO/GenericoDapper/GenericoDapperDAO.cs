using Core.DAO.GenericoDapper;
using Core.DTO.Utils.Data;
using Core.Entity.GestorArchivos;
using Core.Enum.Principal;
using Dapper;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.GenericoDapper
{
    public class GenericoDapperDAO : GenericDAO<tblGA_Directorio>, IGenericoDapper
    {
        Dictionary<string, object> resultado;
        public Dictionary<string, object> ActivarDesactivar(string tabla, bool ActDesc, int id)
        {
            resultado = new Dictionary<string, object>();
            DynamicParameters lstParametros = new DynamicParameters();
            var sql = "spObtenerUltimoCorte";
            try
            {
                using (var conexion = new SqlConnection(ConextSigoDapper.conexionSIGOPLAN()))
                {
                    conexion.Open();
                    var Response = conexion.Query<dynamic>(sql, lstParametros, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    resultado.Add(ITEMS, Response);
                    resultado.Add(SUCCESS, true);
                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                resultado.Add(ITEMS, "Algo ocurrio mal reportelo con TI." + ex.Message.ToString());
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> obtenerConsultaEnkontrol()
        {
            resultado = new Dictionary<string, object>();
            var sql = "SELECT TOP(500000) * FROM DBA.sc_movpol";
            try
            {
                var Response = ConextSigoDapper.Select<dynamic>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = sql,
                    parametros = "",
                });
                    resultado.Add(ITEMS, Response);
                    resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                resultado.Add(ITEMS, "Algo ocurrio mal reportelo con TI." + ex.Message.ToString());
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

    }
}
