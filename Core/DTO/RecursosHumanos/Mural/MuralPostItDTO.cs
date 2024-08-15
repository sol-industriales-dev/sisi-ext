using Core.Entity.Administrativo.RecursosHumanos.Mural;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Mural
{
    public class MuralPostItDTO
    {
        public MuralDTO Mural { get; set; }
        public List<PostItDTO> PostIt { get; set; }
        public List<SeccionDTO> Seccion { get; set; }
    }
}
