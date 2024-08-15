using Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Configuraciones
{
    public class RegistroDTO
    {
        public int idUsuarioRegistro { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
        public void registrar()
        {
            esActivo = true;
            if(fechaRegistro == default(DateTime))
            {
                fechaRegistro = DateTime.Now;
            }
            if(idUsuarioRegistro == 0)
            {
                idUsuarioRegistro = vSesiones.sesionUsuarioDTO.id;
            }
        }
    }
}
