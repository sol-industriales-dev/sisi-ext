using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Alamcen
{
    public class gestionProveedoresDTO
    {

        public int id { get; set; }
        public string PRVCCODIGO { get; set; }
        public string PRVCNOMBRE { get; set; }
        public string PRVCDIRECC { get; set; }


        public bool statusAutorizacion { get; set; }
    }
}
