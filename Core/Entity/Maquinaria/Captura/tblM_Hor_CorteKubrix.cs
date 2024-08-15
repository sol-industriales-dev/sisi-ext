using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_Hor_CorteKubrix
    {
        public int id { get; set; }
        public int acID { get; set; }
        public DateTime fechaCorte { get; set; }
        public int usuarioCaptura { get; set; }
        public DateTime fechaCaptura { get; set; }
    }
}