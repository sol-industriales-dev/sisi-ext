using Core.DTO.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Kubrix
{
    public interface IAnalisisDAO
    {
        List<ComboDTO> getCboDivision();
    }
}
