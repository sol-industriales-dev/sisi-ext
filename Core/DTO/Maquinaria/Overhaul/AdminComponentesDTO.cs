using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class AdminComponentesDTO
    {
        public int id { get; set; }
        public string cc { get; set; }
        public string CCName { get; set; }
        public string economico { get; set; }
        public int locacion { get; set; }
        public List<lstCompDTO> listaComponentes { get; set; }
        public List<int> listaComponentesid { get; set; }
        public decimal minRestaEstatus { get; set; }
    }
}

