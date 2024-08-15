using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Catalogos
{
    public interface IArchivosModelosDAO
    {
        void GuardarArchivos(tblM_ArchivosModelos obj);

        List<tblM_ArchivosModelos> getlistaByModelo(int obj);
        tblM_ArchivosModelos getlistaByID(int obj);


    }
}
