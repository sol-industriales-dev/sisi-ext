using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class ComponentePlaneacionDTO
    {
        public string Value { get; set; }
        public int componenteID { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string posicion { get; set; }
        public int Tipo { get; set; }
        public decimal horasCiclo { get; set; }
        public decimal target { get; set; }
        public int tipoOverhaul { get; set; }
        public bool falla { get; set; }
        public string fechaRemocion { get; set; }
    }
}