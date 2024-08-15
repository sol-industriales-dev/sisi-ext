using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Mantenimiento
{
    public class tblM_BitacoraControlAEMantProy
    {
        public int id { get; set; }
        public decimal Hrsaplico { get; set; }
        public int idAct { get; set; }
        public decimal Vigencia { get; set; }
        public bool programado { get; set; }
        public int idMant { get; set; }
        public string Observaciones { get; set; }
        public DateTime  FechaServicio { get; set; }
        public int UsuarioCap { get; set; }
        public DateTime fechaCaptura { get; set; }
        public bool estatus { get; set; }

        public bool aplicado { get; set; }
    }
}
