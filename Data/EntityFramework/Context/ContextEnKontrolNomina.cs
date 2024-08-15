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
    public class ContextEnKontrolNomina
    {
        public static dynamic Where(string consulta)
        {
            var c = new Conexion();
            string jsonResult = "";
            var con = c.ConnectRH();

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
                    catch (Exception e) {
                        return JsonConvert.DeserializeObject<dynamic>(jsonResult, settings);
                    }
                }

                reader.Close();
                command.Dispose();
            }
            c.Close(con);
            return null;
        }
        public static List<dynamic> Where(List<string> lst)
        {
            var lstRes = new List<dynamic>();
            var c = new Conexion();
            var dsEntity = new DataSet();
            var con = c.ConnectRH();
            var jsonResult = "";
            var i = 0;
            if (con != null)
            {
                var command = con.CreateCommand();
                command.CommandTimeout = 300;
                lst.ForEach(consulta =>
                {
                    command.CommandText = consulta;
                    var reader = command.ExecuteReader();
                    dsEntity.Tables.Add("");
                    dsEntity.Tables[i].Load(reader);
                    if (dsEntity.Tables[i].Rows.Count > 0)
                    {
                        reader.Close();
                        jsonResult = JsonConvert.SerializeObject(dsEntity.Tables[i]);
                        lstRes.Add(JsonConvert.DeserializeObject<dynamic>(jsonResult));
                    }
                    i++;
                });
                command.Dispose();
            }
            c.Close(con);
            return lstRes;
        }
        public static dynamic exec(string consulta)
        {
            var c = new Conexion();
            string jsonResult = "";
            var con = c.ConnectRH();

            if (con != null)
            {


                // OdbcCommand command = con.CreateCommand();
                OdbcCommand command = new OdbcCommand();

                command.Connection = con;
                // con.Open();

                command.CommandText = "spm_repgastosmaquinaria";
                /// command.Connection.Database = "";

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@maquina", "OF-03");//company number
                // command.Parameters.AddWithValue("@InventoryId", InventoryNumberHere);//inventory id from query results
                // command.Parameters.AddWithValue("@EndDate", EndDateHere);//end date from query results
                OdbcDataReader itemAveReader = command.ExecuteReader();

                //  OdbcCommand command = new OdbcCommand("{call ek_adm01_9.spM_RepGastosMaquinaria(?)}", con);
                ////  OdbcCommand command = new OdbcCommand("spm_repgastosmaquinaria", con);

                //  command.CommandType = CommandType.StoredProcedure;
                //  OdbcParameter oParam = new OdbcParameter("maquina", OdbcType.VarChar, 50);
                //  oParam.Direction = ParameterDirection.Input;
                //  oParam.Value = "OF-03";
                //  command.Parameters.Add(oParam);
                //  var d = command.ExecuteReader();
                // command.Parameters.AddWithValue("maquina",SqlDbType.VarChar).Value = "OF-03";
                // command.ExecuteNonQuery();

                //OdbcDataReader reader = command.ExecuteReader();

                //DataSet dsEntity = new DataSet();
                //dsEntity.Tables.Add("");
                //dsEntity.Tables[0].Load(reader);
                //jsonResult = JsonConvert.SerializeObject(dsEntity.Tables[0]);
                //reader.Close();
                //command.Dispose();
            }
            c.Close(con);
            return JsonConvert.DeserializeObject<dynamic>(jsonResult);
        }
    }
}



//ContextEnKontrolNominaArrendadora