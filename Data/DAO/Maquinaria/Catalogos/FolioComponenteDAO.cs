using Core.DAO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Catalogos
{
    public class FolioComponenteDAO : GenericDAO<tblM_FolioComponente>, IFolioComponenteDAO
    {
        public void Guardar(tblM_FolioComponente obj)
        {
            if (!Exists(obj))
            {
                if (obj.id == 0)
                    SaveEntity(obj, (int)BitacoraEnum.COMPONENTE);
                else
                    Update(obj, obj.id, (int)BitacoraEnum.COMPONENTE);
            }
            else
            {
                Update(obj, obj.id, (int)BitacoraEnum.TIPOMAQUINARIA);
            }
        }
        public tblM_FolioComponente getFolio(tblM_FolioComponente obj)
        {
            return _context.tblM_FolioComponente.Where(x => x.modeloID == obj.modeloID &&
                                                          // x.posicionID == obj.posicionID &&
                                                           x.conjuntoID == obj.conjuntoID &&
                                                           x.subConjuntoID == obj.subConjuntoID &&
                                                           x.cc == obj.cc && x.prefijo == obj.prefijo).FirstOrDefault();
        }
        public bool Exists(tblM_FolioComponente obj)
        {
            var aux = _context.tblM_FolioComponente.Where(x => x.modeloID == obj.modeloID &&
                // x.posicionID == obj.posicionID &&
                                                            x.conjuntoID == obj.conjuntoID &&
                                                            x.subConjuntoID == obj.subConjuntoID &&
                                                            x.cc == obj.cc && x.prefijo == obj.prefijo).ToList();
            return aux.Count > 0 ? true : false;
        }

    }
}
