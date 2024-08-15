using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario.Comparativos
{
    public class CuadroAutorizacionDTO
    {
        public int idAsignacion { get; set; }
        public int idSolicitud { get; set; }
        public string CentroCostos { get; set; }
        public string CCDescripcion { get; set; }
        public string noSolicitud { get; set; }
        public string TipoSolicitud { get; set; }
        public string GrupoEquipo { get; set; }
        public string Modelo { get; set; }
        public string Comentario { get; set; }
        public DateTime FechaPromesaDate { get; set; }
        public string FechaPromesa { get; set; }
        public int lstFinanciero { get; set; }
        public int lstComparativo { get; set; }
        public int estado { get; set; }
        public int botonFin { get; set; }
        public int botonAdq { get; set; }
        public int idCuadro { get; set; }
        public bool registroCuadro { get; set; }
        public string obra { get; set; }
        public DateTime? fechaElaboracionCuadro { get; set; }
        public string fechaElaboracionCuadroString { get; set; }
        public DateTime? fechaUltimaAutorizacionCuadro { get; set; }
        public string fechaUltimaAutorizacionCuadroString { get; set; }
        public int estatusCuadro { get; set; }
        public bool cuadroEditable { get; set; }
    }
}
