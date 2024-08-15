using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Context
{
    public class ContextEnKontrolNominaPrueba
    {
        public static dynamic Where(string consulta)
        {
            var c = new Conexion();
            string jsonResult = "";
            var con = c.ConnectPruebaRH();

            if (con != null)
            {
                OdbcCommand command = con.CreateCommand();
                command.CommandTimeout = 300;
                command.CommandText = consulta;

                OdbcDataReader reader = command.ExecuteReader();
                DataSet dsEntity = new DataSet();
                dsEntity.Tables.Add("");
                dsEntity.Tables[0].Load(reader);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                if (dsEntity.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        jsonResult = JsonConvert.SerializeObject(dsEntity.Tables[0]);
                        reader.Close();
                        command.Dispose();
                        c.Close(con);
                        return JsonConvert.DeserializeObject<dynamic>(jsonResult, settings);
                    }
                    catch (Exception e)
                    {
                        return JsonConvert.DeserializeObject<dynamic>(jsonResult, settings);
                    }
                }

                reader.Close();
                command.Dispose();
            }
            c.Close(con);
            return null;
        }
    }
}
