using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Catalogos
{
    public class ModeloArchivoDTO
    {
        [JsonProperty("nombre")]
        public string nombre { get; set; }
        [JsonProperty("ruta")]
        public string ruta { get; set; }
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("FechaCreacion")]
        public string FechaCreacion { get; set; }
        public string FechaCreacionSinFormato { get; set; }
    }
}
