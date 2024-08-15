using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class rptRemocionComponentesDTO
    {

        public string noEconomico { get; set; }
        public string fecha { get; set; }
        public string descripcion { get; set; }
        public string noComponente { get; set; }
        public string concepto { get; set; }
        public string horasCicloActual { get; set; }
        public string cicloVidaHoras { get; set; }
        public string obra { get; set; }
        public string destino { get; set; }
        public string porcentajeRemocion { get; set; }
   
    }
}
