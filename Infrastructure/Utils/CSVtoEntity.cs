using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Utils
{
    public class CSVtoEntity
    {
        public static dynamic ConvertCSVTABtoDataTable(byte[] file)
        {
            Stream stream = new MemoryStream(file);
            StreamReader sr = new StreamReader(stream);
            string[] headers = sr.ReadLine().Split('|');
            for (int i = 0; i < headers.Length; i++)
            
            {
                headers[i]=headers[i].Replace(" ","_");
                headers[i] = headers[i].Replace("/", "_");
                headers[i] = headers[i].Replace("(", "");
                headers[i] = headers[i].Replace(")", "");
                headers[i] = headers[i].Replace("%", "_P");
                headers[i] = headers[i].Replace("-", "");
                headers[i] = headers[i].Replace(",", "");
                headers[i] = headers[i].Replace("@", "_a");
                headers[i] = headers[i].Replace(">", "");
                headers[i] = headers[i].Replace(".", "");
                if(headers[i].Equals("Status"))
                {
                    headers[i] = headers[i - 1] + "_" + headers[i];
                }
            }


            DataTable dt = new DataTable();
            string jsonResult = "";
            int counter = 1;
            foreach (string header in headers)
            {
                if (dt.Columns.Contains(header))
                {
                    dt.Columns.Add(header + "-"+counter);
                    counter++;
                }
                else
                {
                    dt.Columns.Add(header);
                }

            }
            while (!sr.EndOfStream)
            {
                string[] rows = sr.ReadLine().Split('|');
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    if (headers[i] == "Comments")
                    {
                        dr[i] = "comentario";
                    }
                    else
                    {
                        dr[i] = rows[i];
                    }
                   
                }
                dt.Rows.Add(dr);
            }
            jsonResult = JsonConvert.SerializeObject(dt);
            return JsonConvert.DeserializeObject<dynamic>(jsonResult);
        } 
    }
}
