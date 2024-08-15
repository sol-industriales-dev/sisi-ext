using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Caratulas
{
    public class CaratulasDTO
    {
         public int id { get; set; }
        public int idGrupo { get; set; }
        public string grupo { get; set; }
        public int idModelo { get; set; }
        public string modelo { get; set; }
        public decimal depreciacion { get; set; }
        public decimal inversion { get; set; }
        public decimal seguro { get; set; }
        public decimal filtros { get; set; }
        public decimal mantenimientoCo { get; set; }
        public decimal manoObra { get; set; }
        public decimal auxiliar { get; set; }
        public decimal indirectosMatriz { get; set; }
        public decimal depreciacionOH { get; set; }
        public decimal aceite { get; set; }
        public decimal carilleria { get; set; }
        public decimal ansul { get; set; }
        public decimal utilidad { get; set; }
        public decimal costoTotal { get; set; }
        public int idCC { get; set; }
        public bool esActivo { get; set; }
        public string grupoTexto { get; set; }
        public string modeloTexto { get; set; }

        public string tipoHoraDia { get; set; }
        public bool IndicadorTipoMoneda { get; set; }
        public bool IndicadorManoObra { get; set; }
        public decimal IndicadorAxuliar { get; set; }
        public decimal IndicadorIndirectos { get; set; }
        public int esHoraDia { get; set; }
    }
    
}
