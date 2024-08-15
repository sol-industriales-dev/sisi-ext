using Core.Enum.GestorCorporativo;
using System.Collections.Generic;

namespace Core.DTO.GestorCorporativo
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
        public string date { get; set; }
        public List<DirectorioDTO> data { get; set; }
        public int index { get; set; }
        public bool open { get; set; }
        public PermisosDTO permisos { get; set; }
        public GrupoCarpetaEnum grupoCarpeta { get; set; }
        public SubGrupoCarpetaEnum subGrupoCarpeta { get; set; }
    }
}
