using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_CapturaDatosDiariosMaquinaria
    {
        public int id { get; set; }
        public DateTime fechaCapturaMaquinaria { get; set; }
        public int idCatMaquina { get; set; }
        //public virtual tblM_CatMaquina Maquina { get; set; }
        public DateTime? FechaPatioMaquinaria { get; set; }
        public DateTime? FechaTMC { get; set; }
        public DateTime? FechaMaquinaria { get; set; }
        public int idEstatus { get; set; }
        public string Observaciones { get; set; }
        public bool Enviado { get; set; }
          
    }
}
