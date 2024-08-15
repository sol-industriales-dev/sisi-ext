using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Facultamiento
{
    public class CatalogoPaquetesDTO
    {
        public int ID { get; set; }
        public string CentroCostos { get; set; }
        public int ccID { get; set; }
        public string Descripcion { get; set; }
        public string Fecha { get; set; }
        public string Estatus { get; set; }
        public string Departamento { get; set; }
        public bool Editable { get; set; }
        public int Version { get; set; }
    }
}
