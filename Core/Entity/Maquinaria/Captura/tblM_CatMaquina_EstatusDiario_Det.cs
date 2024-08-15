using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_CatMaquina_EstatusDiario_Det
    {
        public int id { get; set; }
        public int estatusDiarioID { get; set; }
        public int noEconomicoID { get; set; }
        public string noEconomico { get; set; }
        public string descripcion { get; set; }
        [NotMapped]
        public string modelo { get; set; }
        public bool activo { get; set; }
        public string causa { get; set; }
        public DateTime? fecha_inicial { get; set; }
        public DateTime? fecha_proyectada { get; set; }
        public DateTime? fecha_real { get; set; }
        public string tiempo_respuesta_str { get; set; }
        public decimal tiempo_respuesta { get; set; }
        public string acciones { get; set; }
    }
}
