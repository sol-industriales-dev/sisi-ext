using Core.DTO.Principal.Bitacoras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Principal.Bitacoras
{
    public interface IBitacoraDAO
    {
        IList<BitacoraDTO> getBitacora(int Modulo, int RegistroID);
    }
}
