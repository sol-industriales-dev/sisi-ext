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
    public class ContextArrendadora
    {
        public static dynamic Where(string consulta)
        {
            var c = new Conexion();
            string jsonResult = "";
            var con = c.ConnectArrendarora();
            DataSet dsEntity = new DataSet();
            if (con != null)
            {
                OdbcCommand command = con.CreateCommand();
                command.CommandTimeout = 300;
                command.CommandText = consulta;

                OdbcDataReader reader = command.ExecuteReader();

                dsEntity.Tables.Add("");
                dsEntity.Tables[0].Load(reader);
                if (dsEntity.Tables[0].Rows.Count > 0)
                {
                    jsonResult = JsonConvert.SerializeObject(dsEntity.Tables[0]);
                    reader.Close();
                    command.Dispose();
                    c.Close(con);
                    return JsonConvert.DeserializeObject<dynamic>(jsonResult);
                }

                reader.Close();
                command.Dispose();
            }
            c.Close(con);

            jsonResult = JsonConvert.SerializeObject(dsEntity.Tables[0]);
            return JsonConvert.DeserializeObject<dynamic>(jsonResult);
        }
        public static dynamic WherePrueba(string consulta)
        {
            var c = new Conexion();
            string jsonResult = "";
            var con = c.ConnectArrendaroraPrueba();
            DataSet dsEntity = new DataSet();
            if (con != null)
            {
                OdbcCommand command = con.CreateCommand();
                command.CommandTimeout = 300;
                command.CommandText = consulta;

                OdbcDataReader reader = command.ExecuteReader();

                dsEntity.Tables.Add("");
                dsEntity.Tables[0].Load(reader);
                if (dsEntity.Tables[0].Rows.Count > 0)
                {
                    jsonResult = JsonConvert.SerializeObject(dsEntity.Tables[0]);
                    reader.Close();
                    command.Dispose();
                    c.Close(con);
                    return JsonConvert.DeserializeObject<dynamic>(jsonResult);
                }

                reader.Close();
                command.Dispose();
            }
            c.Close(con);

            jsonResult = JsonConvert.SerializeObject(dsEntity.Tables[0]);
            return JsonConvert.DeserializeObject<dynamic>(jsonResult);
        }
    }
}
