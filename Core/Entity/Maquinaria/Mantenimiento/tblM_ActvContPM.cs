using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Mantenimiento
{
    public class tblM_ActvContPM
    {
        public int id { get; set; }
        public int idActividad { get; set; }//nombre actividad
        public decimal Contador { get; set; }//horas del componente
        public bool Actual { get; set; }//si es el horometro mas reciente del componente
        public int idMaquina { get; set; }//id maquina a quien se le ralizo el mantenimiento
        //public decimal VidaUtil { get; set; }//vida util del componente o suministro
        public int idMantenimientoPm { get; set; }//el id del mantenimiento 
        public int idParteVidaUtil { get; set; }//el id parte vida util
    }
}
