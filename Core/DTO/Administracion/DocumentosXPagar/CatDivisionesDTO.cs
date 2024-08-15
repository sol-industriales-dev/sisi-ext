using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.DocumentosXPagar
{
    public class CatDivisionesDTO
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string abreviacion {get;set;}

        public int estatus { get; set; }
        public string mensaje { get; set; }
    }
}
