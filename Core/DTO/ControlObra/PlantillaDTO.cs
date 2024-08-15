using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.ControlObra;

namespace Core.DTO.ControlObra
{
    public class PlantillaDTO
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public int colaborador_id { get; set; }
        public string colaboradorNombre { get; set; }
        public DateTime fecha { get; set; }
        public string fechaString { get; set; }
        public TipoPlantillaEnum tipo { get; set; }
        public string tipoDesc { get; set; }
        public bool plantillaBase { get; set; }

        public int usuarioCreacion_id { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificacion_id { get; set; }
        public DateTime? fechaModificacion { get; set; }

        public List<RequerimientoDTO> requerimientos { get; set; }

        public List<int> contratos { get; set; }
    }
}
