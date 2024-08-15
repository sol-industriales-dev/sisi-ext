using Core.Enum.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.FileManager
{
    public class CarpetaBaseDTO
    {
        public long padreID { get; set; }
        public string nombreCarpetaBase { get; set; }

        public string nombreCarpeta { get; set; }
        public List<int> listaTiposArchivosID { get; set; }
        public bool considerarse { get; set; }
        public string abreviacion { get; set; }
        public int FK_Usuario_Logueado { get; set; }
        public MainContextEnum objEmpresa { get; set; }
    }
}