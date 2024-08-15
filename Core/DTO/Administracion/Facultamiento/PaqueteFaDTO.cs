using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Facultamiento
{
    public class PaqueteFaDTO
    {
        public int PaqueteID { get; set; }
        public string Comentario { get; set; }
        public string Estado { get; set; }
        public string CentroCostos { get; set; }
        public string Obra { get; set; }
        public string Fecha { get; set; }
        public string Departamento { get; set; }
        public bool? EsActivo { get; set; }
        public List<FacultamientoDTO> listaFacultamientos { get; set; }
        public List<AutorizanteFaDTO> ListaAutorizantes { get; set; }
    }
}
