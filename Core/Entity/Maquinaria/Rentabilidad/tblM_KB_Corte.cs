using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Rentabilidad
{
    public class tblM_KB_Corte
    {
        public int id { get; set; }
        public DateTime fechaCorte { get; set; }
        public int usuarioCaptura { get; set; }
        public DateTime fechaCaptura { get; set; }        
        public int tipoCorte { get; set; }
        public int periodo { get; set; }
        public int anio { get; set; }
        public bool existeCorteOtraEmpresa { get; set; }
        public bool estimadosCerrados { get; set; }
        public bool estatus { get; set; }
        public int corteAnteriorID { get; set; }
    }
}
