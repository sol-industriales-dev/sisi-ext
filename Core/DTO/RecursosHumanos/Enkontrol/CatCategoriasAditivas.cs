using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Enkontrol
{
    public class CatCategoriasAditivas
    {
        public int? altas { get; set; }//cantidad de personal actual en el puesto
        public int? cantidad { get; set; }//cantidad de personal necesario para el puesto
        public string categoria { get; set; }// categoria
        public string puesto { get; set; }
        public int id_puesto { get; set; }
        public string id_plantilla { get; set; }
        public string condicionInicial { get; set; }
        public string condicionActual { get; set; }
        public string soporte { get; set; }
        public string link { get; set; }
        public int? cambios { get; set; }
        public bool? esNuevo { get; set; }
        public int totalXContratar { get; set; }
    }
}
