using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.GestorArchivos
{
    public class DirectorioDTO
    {

        public int id { get; set; }
        public int pId { get; set; }
        public string parent { get; set; }
        public int userID { get; set; }
        public int level { get; set; }
        public string value { get; set; }
        public string type { get; set; }
        public string date { get; set; }
        public List<DirectorioDTO> data { get; set; }
        public int index { get; set; }
        public int version { get; set; }
        public string usuario { get; set; }
        public bool open { get; set; }
        public PermisosDTO permisos { get; set; }

    }
}
