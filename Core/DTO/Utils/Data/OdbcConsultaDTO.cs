using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Utils.Data
{
    public class OdbcConsultaDTO
    {
        public OdbcConsultaDTO()
        {
            parametros = new List<OdbcParameterDTO>();
        }
        public string consulta { get; set; }
        public List<OdbcParameterDTO> parametros { get; set; }
    }
}
