using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class ComponenteDTO
    {
        public int id { get; set; }
        public string noComponente { get; set; }
        public int? trackComponenteID { get; set; }
        public int? centroCostos { get; set; }
        public string descripcion { get; set; }
        public string noSerie { get; set; }
        public string numParte { get; set; }
        public decimal costo { get; set; }
        public decimal cicloVidaHoras { get; set; }
        public decimal horaCicloActual { get; set; }
        public decimal horasAcumuladas { get; set; }
        public string conjunto { get; set; }
        public string subConjunto { get; set; }
        public int? conjuntoID { get; set; }
        public int? subConjuntoID { get; set; }
        public int? marcaComponenteID { get; set; }
        public bool estatus { get; set; }
        public int? modeloEquipoID { get; set; }
        public int posicionID { get; set; }
        public int? grupoID { get; set; }
        public string nombre_Corto { get; set; }
        public DateTime? fecha { get; set; }
        public int? proveedorID { get; set; }
        public int garantia { get; set; }
        public bool? falla { get; set; }
        public string locacion { get; set; }
        public int locacionID { get; set; }
        public int vidaInicio { get; set; }
        public int tipoLocacion { get; set; }
        public bool intercambio { get; set; }
        public string ordenCompra { get; set; }
    }
}

