using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
    public class EstatusDiarioDTO
    {
        public tblM_CatMaquina_EstatusDiario obj { get; set; }
        public List<tblM_CatMaquina_EstatusDiario_Det> det { get; set; }
    }
}
