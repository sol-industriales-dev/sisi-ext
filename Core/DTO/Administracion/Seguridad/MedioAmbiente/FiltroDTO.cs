using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.MedioAmbiente
{
    public class FiltroDTO
    {
        public List<int> listaDivisiones { get; set; }
        public List<int> listaLineasNegocio { get; set; }
        public int idAgrupacion { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
    }
}