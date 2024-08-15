using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Core.Entity.Maquinaria.Catalogo
{
    public class tblM_CatGrupoMaquinaria
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public int tipoEquipoID { get; set; }
        public string prefijo { get; set; }
        public bool estatus { get; set; }
        public int noEco { get; set; }
        public bool dn { get; set; }
        public bool sos { get; set; }
        public bool bitacora { get; set; }

        public bool setFotografico { get; set; }
        public bool rehabilitacion { get; set; }

        [JsonIgnore]
        public virtual tblM_CatTipoMaquinaria tipoEquipo { get; set; }
        //[JsonIgnore]
        //public virtual List<tblM_CatMarcaEquipo> marca { get; set; }

    }
}
