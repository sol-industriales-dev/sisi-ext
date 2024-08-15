using Core.DAO.Principal.Usuarios;
using Core.Entity.Principal.Usuarios;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Principal.Usuarios
{
    public class envioCorreosDAO : GenericDAO<tblP_EnvioCorreos>, IenvioCorreosDAO
    {

        public List<tblP_EnvioCorreos> GetListaCorreos(int moduloID, string cc)
        {
            List<tblP_EnvioCorreos> result = new List<tblP_EnvioCorreos>();

            var Permiso = _context.tblP_EnvioCorreos.Where(x => x.moduloID == moduloID && x.estatus == true && x.centroCostosPermiso).ToList();
            var sinPermiso = _context.tblP_EnvioCorreos.Where(x => x.moduloID == moduloID && x.estatus == true && !x.centroCostosPermiso).ToList().Select(x => x.usuarioID);

            //var CCValid = (from CC in _context.tblP_CC_Usuario
            //               join correos in _context.tblP_EnvioCorreos
            //               on CC.usuarioID equals correos.usuarioID
            //               where CC.cc == cc && !correos.centroCostosPermiso
            //               select correos).ToList();

            //result.AddRange(CCValid);
            result.AddRange(Permiso);

            return result;
        }
    }
}
