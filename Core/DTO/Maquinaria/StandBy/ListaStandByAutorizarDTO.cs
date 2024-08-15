using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.StandBy
{
    public class ListaStandByAutorizarDTO
    {
        public int id { get; set; }
        public string Economico { get; set; }
        public int noEconomicoID { get; set; }
        public string modelo { get; set; }
        public string estatus { get; set; }
        public int usuarioCapturaID { get; set; }
        public string usuarioCapturaNombre { get; set; }
        public string fechaCaptura { get; set; }
        public int? usuarioAutorizaID { get; set; }
        public string usuarioAutorizaNombre { get; set; }
        public string fechaAutoriza { get; set; }
        public int? usuarioLiberaID { get; set; }
        public string usuarioLiberaNombre { get; set; }
        public string fechaLibera { get; set; }
        public string ccActual { get; set; }
        public string comentarioJustificacion { get; set; }
        public string comentarioValidacion { get; set; }
        public string comentarioLiberacion { get; set; }
        public string evidenciaJustificacion { get; set; }
        public decimal moiEquipo { get; set; }
        public decimal valorEnLibroEquipo { get; set; }
        public decimal depreciacionMensualEquipo { get; set; }
        public decimal valorEnLibroOverhaul { get; set; }
        public decimal depreciacionMensualOverhaul { get; set; }
        public bool esVoBo { get; set; }
    }
}
