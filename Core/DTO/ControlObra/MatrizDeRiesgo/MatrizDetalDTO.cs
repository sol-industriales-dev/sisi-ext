using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra.MatrizDeRiesgo
{
    public class MatrizDetalDTO
    {
        public int id { get; set; }
        public int idMatrizDeRiesgo { get; set; }
        public int Historial { get; set; }
        public int No { get; set; }
        public string amenazaOportunidad { get; set; }
        public int categoriaDelRiesgo { get; set; }
        public string descategoriaDelRiesgo { get; set; }
        public string causaBasica { get; set; }
        public string descausaBasica { get; set; }
        public string areaDelProyecto { get; set; }
        public string costoTiempoCalidad { get; set; }
        public int probabilidad { get; set; }
        public int impacto { get; set; }
        public int severidadInicial { get; set; }
        public int severidadActual { get; set; }
        public int tipoDeRespuesta { get; set; }
        public string desctipoDeRespuesta { get; set; }
        public string medidasATomar { get; set; }
        public string dueñoDelRiesgo { get; set; }
        public DateTime fechaDeCompromiso { get; set; }
        public string abiertoProcesoCerrado { get; set; }
    }
}
