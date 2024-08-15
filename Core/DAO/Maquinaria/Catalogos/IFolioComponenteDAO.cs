using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Catalogos
{
    public interface IFolioComponenteDAO
    {
        void Guardar(tblM_FolioComponente obj);
        tblM_FolioComponente getFolio(tblM_FolioComponente obj);
        bool Exists(tblM_FolioComponente obj);
    }
}
