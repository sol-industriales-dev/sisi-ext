using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class FechasPresupuestoObraDTO
    {
        public int estado { get; set; }
        public string obraID { get; set; }
        public string obra { get; set; }
        public DateTime fechaEnvio { get; set; }
        public DateTime fechaVoBo1 { get; set; }
        public DateTime fechaVoBo2 { get; set; }
        public DateTime fechaVoBo3 { get; set; }
        public DateTime fechaAutorizacion { get; set; }
    }
}
