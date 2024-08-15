using Core.Entity.Administrativo.Facultamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Facultamiento
{
    public class CuadroDTO
    {
        public tblFa_CatFacultamiento facultamiento { get; set; }
        public List<MontoDTO> montos { get; set; }
    }
}
