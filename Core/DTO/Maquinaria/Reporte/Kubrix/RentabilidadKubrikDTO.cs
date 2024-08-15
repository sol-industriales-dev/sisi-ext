using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Maquinaria.Reporte.Rentabilidad;
using Core.DTO.Maquinaria.Rentabilidad;

namespace Core.DTO.Maquinaria.Reporte.Kubrix
{
    public class RentabilidadKubrikDTO
    {
        public int tipo_mov { get; set; }
        public string descripcion { get; set; }
        public decimal mayor { get; set; }
        public decimal menor { get; set; }
        public decimal transporteConstruplan { get; set; }
        public decimal transporteArrendadora { get; set; }
        public decimal administrativoCentral { get; set; }
        public decimal administrativoProyectos { get; set; }
        public decimal fletes { get; set; }
        public decimal neumaticos { get; set; }
        public decimal otros { get; set; }
        public decimal total { get; set; }
        public decimal actual { get; set; }
        public decimal semana2 { get; set; }
        public decimal semana3 { get; set; }
        public decimal semana4 { get; set; }
        public decimal semana5 { get; set; }
        public List<decimal> divisiones { get; set; }
        public List<CorteDetDTO> detalles { get; set; }
    }
}
