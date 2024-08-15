using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Mantenimiento
{
    public class tblM_MatenimientoPm
    {

        public int id { get; set; }
        public string economicoID { get; set; }
        public decimal horometroUltCapturado { get; set; }
        public DateTime fechaUltCapturado { get; set; }
        public int tipoPM { get; set; }
        public DateTime fechaPM { get; set; }
        public decimal horometroPM { get; set; }
        public int personalRealizo { get; set; }
        public string observaciones { get; set; }
        public decimal horometroProy { get; set; }
        public DateTime fechaProy { get; set; }
        public int tipoMantenimientoProy { get; set; }
        public DateTime fechaProyFin { get; set; }
        public bool actual { get; set; }
        public DateTime fechaCaptura { get; set; }
        public int idMaquina { get; set; }
        public bool estatus { get; set; }
        public int planeador { get; set; }
        public int UsuarioCap { get; set; }

        public int estadoMantenimiento { get; set; }
        public decimal horometroPMEjecutado { get; set; }

        //public int id { get; set; }
        //public string economicoID { get; set; }
        //public DateTime fechaMantenimientoActual { get; set; }
        //public DateTime fechaCaptura { get; set; }
        //public int tipoMantenimientoActual { get; set; }
        //public decimal horometroServicio { get; set; }
        //public string observaciones {get; set;}
        //public bool actual { get; set; }
        //public int idMaquina { get; set; }
        //public DateTime fechaProyectadaProximo { get; set; }
        //public int personalRealizo { get; set; }
        //public DateTime fechaFinProyectadaProximo { get; set; }



        
    }
}
