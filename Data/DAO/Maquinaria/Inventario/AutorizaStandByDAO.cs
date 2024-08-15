using Core.DAO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Inventario
{
    public class AutorizaStandByDAO : GenericDAO<tblM_AutorizaStandby>, IAutorizaStandbyDAO
    {

        public void Guardar(tblM_AutorizaStandby obj)
        {
            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.ModeloArchivos);
            else
                Update(obj, obj.id, (int)BitacoraEnum.ModeloArchivos);
        }

        public int GetUsuarioValida(int id, string CC)
        {
            return _context.tblP_Autoriza.Join(
                    _context.tblP_CC_Usuario.Where(i => i.cc == CC), 
                    a => a.cc_usuario_ID, 
                    c => c.id, 
                    (a, c) => a)
                .FirstOrDefault(x => x.perfilAutorizaID == 1).usuarioID;
        }

        public tblM_AutorizaStandby getAutorizacionesbyStandbyID(int id)
        {
            return _context.tblM_AutorizaStandby.FirstOrDefault(x => x.standByID == id);
        }


    }
}
