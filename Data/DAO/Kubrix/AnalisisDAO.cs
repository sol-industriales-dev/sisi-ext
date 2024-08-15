using Core.DAO.Kubrix;
using Core.DTO.Facturacion;
using Core.Entity.Kubrix.Analisis;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Kubrix
{
    public class AnalisisDAO : GenericDAO<tblK_catCcDiv>, IAnalisisDAO
    {
        public List<ComboDTO> getCboDivision()
        {
            return _context.tblK_catDivision.Select(x => new ComboDTO
            { 
                Value = x.id.ToString(),
                Text = x.id.ToString(),
                Prefijo = x.Divsion
            }).ToList();
        }
    }
}
