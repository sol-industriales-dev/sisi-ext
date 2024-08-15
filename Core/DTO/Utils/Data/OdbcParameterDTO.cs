using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Utils.Data
{
    public class OdbcParameterDTO
    {
        public string nombre { get; set; }
        public OdbcType tipo { get; set; }
        public SqlDbType  tipoSql { get; set; }
        public object valor { get; set; }
    }
}
