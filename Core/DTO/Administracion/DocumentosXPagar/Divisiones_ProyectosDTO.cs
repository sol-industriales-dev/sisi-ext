using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.DocumentosXPagar
{
    public class Divisiones_ProyectosDTO
    {
        public int id { get; set; }
        public int divisionId { get; set; }
        public string cc { get; set; }
        public string descripcionCC { get; set; }
        public string nombreDivision { get; set; }
        public string abreviacion { get; set; }

        public bool isadmin { get; set; }
        public int estatus { get; set; }
        public string mensaje { get; set; }

        public string esAdmin { get; set; }
    }
}
