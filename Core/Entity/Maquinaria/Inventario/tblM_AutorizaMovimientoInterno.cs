using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_AutorizaMovimientoInterno
    {
        public int id { get; set; }
        public int usuarioEnvio { get; set; }
        public int usuarioRecibe { get; set; }
        public int usuarioValida { get; set; }
        public string cadenafirmaEnvia { get; set; }
        public string cadenafirmaRecibe { get; set; }
        public string cadenafirmaEnterado { get; set; }
        public bool firmaEnvio { get; set; }
        public bool firmaRecibe { get; set; }
        public bool firmaEnterado { get; set; }
        public DateTime FechaCaptura { get; set; }
        public int ControMovimientoInternoID { get; set; }
        public int Autoriza3 { get; set; }
        public int Autoriza2 { get; set; }
        public int Autoriza1 { get; set; }

        public virtual tblM_ControMovimientoInterno ControMovimientoInterno { get; set; }
    }
}
