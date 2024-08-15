using Core.Enum.FileManager;
using System;
using System.Collections.Generic;

namespace Core.DTO.FileManager
{
    public class DirectorioDTO
    {
        public long id { get; set; }
        public long pId { get; set; }
        public string parent { get; set; }
        public int userID { get; set; }
        public int level { get; set; }
        public string value { get; set; }
        public string type { get; set; }
        public string tipoArchivo { get; set; }
        public string date { get; set; }
        public List<DirectorioDTO> data { get; set; }
        public int index { get; set; }
        public int version { get; set; }
        public string usuario { get; set; }
        public bool open { get; set; }
        public List<int> listaTiposArchivosID { get; set; }
        public PermisosDTO permisos { get; set; }
        public bool cargaDinamica { get; set; }
        public int año { get; set; }
        public int divisionID { get; set; }
        public int subdivisionID { get; set; }
        public int ccID { get; set; }
        public string tipoCarpeta { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaEdicion { get; set; }
        public bool puedeSubir { get; set; }
        public bool puedeEliminar { get; set; }
        public bool puedeDescargarArchivo { get; set; }
        public bool puedeDescargarCarpeta { get; set; }
        public bool puedeActualizar { get; set; }
        public bool puedeCrear { get; set; }
        public int estatusVista { get; set; }
        public int obraCerrada { get; set; }
    }
}
