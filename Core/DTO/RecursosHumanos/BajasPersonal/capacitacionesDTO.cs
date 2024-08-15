using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.BajasPersonal
{
    public class capacitacionesDTO
    {
        public int id { get; set; }
        public int cursoID { get; set; }
        public string curso { get; set; }
        public string cc { get; set; }
        public int examenID { get; set; }
        public decimal calificacion { get; set; }
        public int divison { get; set; }
        public string fecha { get; set; }

    }
}
