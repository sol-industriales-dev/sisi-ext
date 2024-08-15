using Core.DTO.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Principal.Usuarios
{
    public interface IOrganigramaDAO
    {
        List<OrganigramaDTO> getByUserID(int id);
    }
}
