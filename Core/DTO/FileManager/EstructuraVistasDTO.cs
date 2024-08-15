using System.Collections.Generic;

namespace Core.DTO.FileManager
{
    public class EstructuraVistasDTO
    {

        public string title { get; set; }
        public bool selected { get; set; }
        public bool partsel { get; set; }
        public bool expanded { get; set; }
        public long id { get; set; }
        public long key { get; set; }
        public bool folder { get; set; }
        public List<EstructuraVistasDTO> children { get; set; }
        public bool checkbox { get; set; }
        public bool unselectable { get; set; }
        public PermisosDTO permisos { get; set; }
        public bool lazy { get; set; }

        public bool puedeSubir { get; set; }
        public bool puedeEliminar { get; set; }
        public bool puedeDescargarArchivo { get; set; }
        public bool puedeDescargarCarpeta { get; set; }
        public bool puedeActualizar { get; set; }
        public bool puedeCrear { get; set; }
    }

}
