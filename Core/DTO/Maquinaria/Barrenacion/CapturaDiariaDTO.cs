using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Barrenacion
{
    public class CapturaDiariaDTO
    {

        public int id { get; set; }
        public string fechaCaptura { get; set; }
        public string fechaCreacion { get; set; }
        public decimal horometroFinal { get; set; }
        public decimal horasTrabajadas { get; set; }
        public int tipoCaptura { get; set; }
        public int turno { get; set; }
        public int claveOperador { get; set; }
        public decimal precioOperador { get; set; }
        public decimal fsrOperador { get; set; }
        public decimal totalOperador { get; set; }
        public int claveAyudante { get; set; }
        public decimal precioAyudante { get; set; }
        public decimal fsrAyudante { get; set; }
        public decimal totalAyudante { get; set; }
        public int brocaID { get; set; }
        public int martilloID { get; set; }
        public int barraID { get; set; }
        public int barraSegundaID { get; set; }
        public int culataID { get; set; }
        public int portabitID { get; set; }
        public int cilindroID { get; set; }
        public int zancoID { get; set; }
        public int barrenadoraID { get; set; }
        public string brocaSerie { get; set; }
        public string martilloSerie { get; set; }
        public string barraSerie { get; set; }
        public string barraSegundaSerie { get; set; }
        public string culataSerie { get; set; }
        public string portabitSerie { get; set; }
        public string cilindroSerie { get; set; }
        public string zancoSerie { get; set; }
        public decimal bordo { get; set; }
        public decimal espaciamiento { get; set; }
        public int barrenos { get; set; }
        public decimal profundidad { get; set; }
        public string banco { get; set; }
        public decimal densidadMaterial { get; set; }
        public int tipoBarreno { get; set; }
        public decimal subbarreno { get; set; }
        public string areaCuenta { get; set; }
        public int rehabilitacion { get; set; }
        public bool edit { get; set; }

        /*Comentario para subir porque no me aparecia */
    }
}
