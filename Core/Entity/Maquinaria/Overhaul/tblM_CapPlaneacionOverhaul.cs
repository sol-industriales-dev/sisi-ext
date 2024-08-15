using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Overhaul;
using System.ComponentModel.DataAnnotations.Schema;
using Core.DTO.Maquinaria.Overhaul;
using Newtonsoft.Json;

namespace Core.Entity.Maquinaria.Overhaul
{
    public class tblM_CapPlaneacionOverhaul
    {
        public int id { get; set; }
        public string idComponentes { get; set; }
        public int maquinaID { get; set; }
        public DateTime fecha { get; set; }       
        public int tipo { get; set; }
        public int estatus { get; set; }
        public int calendarioID { get; set; }
        public string indexCal { get; set; }
        public string actividades { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }
        public decimal ? diasDuracionP { get; set; }
        public string diasTrabajados { get; set; }
        public DateTime? fechaFinP { get; set; }
        public decimal ritmo { get; set; }
        public bool terminado { get; set; }
        public string indexCalOriginal { get; set; }

        public virtual tblM_CalendarioPlaneacionOverhaul calendario { get; set; }
        //public virtual tblM_CatComponente componente { get; set; }

        [NotMapped]
        public List<ComponentePlaneacionDTO> ComponentePlaneacionDTO
        {
            get
            {
                return string.IsNullOrEmpty(idComponentes) ? null : JsonConvert.DeserializeObject<List<ComponentePlaneacionDTO>>(idComponentes);
            }

            set
            {
                idComponentes = JsonConvert.SerializeObject(value);
            }
        }
    }
}

