using Core.DAO.Kubrix;
using Core.DTO.Facturacion;
using Core.Entity.Kubrix;
using Core.Entity.Kubrix.Analisis;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Kubrix
{
    public class CatalogoDAO : GenericDAO<tblK_catCcDiv>, ICatalogoDAO
    {
        public List<ComboDTO> getCboDivision()
        {
            return _context.tblK_catDivision.Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.Divsion,
                Prefijo = String.Empty
            }).ToList();
        }
        public List<tblK_catDivision> getLstDiv()
        {
            return _context.tblK_catDivision.ToList();
        }
        public List<tblK_catCcDiv> getlstCcDiv(string cc, int idDiv)
        {
            var lst = _context.tblK_catCcDiv
                .Where(w => string.IsNullOrEmpty(cc) ? true : w.cc.Equals(cc))
                .Where(w => idDiv == 0 ? true : w.idDivision == idDiv)
                .ToList();
            return lst;
        }
        public List<tblK_Bal12> getlstBal12(string cc)
        {
            var lst = _context.tblK_Bal12
                .Where(w => string.IsNullOrEmpty(cc) ? false : w.cc.Equals(cc))
                .ToList();
            return lst;
        }
    }
}
