using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
    public class EncabezadoTotalesDTO
    {
        public string noEconomico { get; set; }
        public List<TotalesDTO> lstAceites { get; set; }
        public decimal TotalFila;
        public decimal totalColumna; 
    }
}
