using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.Analisis
{
    public class BusqAnalisisDTO
    {
        public string obra { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int tipo { get; set; }
        public List<int> lstGrupo { get; set; }
        public List<int> lstModelo { get; set; }
        public List<string> lstMaquina { get; set; }
        public DateTime fecha { get; set; }
    }
}


