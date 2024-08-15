using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos
{
    public class ComparacionPlantillaBonoDTO
    {
        public int puestoID { get; set; }
        public string puesto { get; set; }
        public int periocidad { get; set; }
        public string periocidadDesc { get; set; }
        public decimal monto { get; set; }
        public string clase { get; set; }
        public bool comun { get; set; }
    }
}
