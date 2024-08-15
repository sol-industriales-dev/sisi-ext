using Core.DTO.Utils.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.GenericoDapper
{
    public interface IGenericoDapper
    {

        Dictionary<string, object> ActivarDesactivar(string tabla, bool ActDesc,int id);
        Dictionary<string, object> obtenerConsultaEnkontrol();

    }
}
