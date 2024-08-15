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
    public class ArchivosModelosCreditoDAO : GenericDAO<tblM_ArchivosModelos>, IArchivosModelosDAO
    {
        public void GuardarArchivos(tblM_ArchivosModelos obj)
        {
            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.ModeloArchivos);
            else
                Update(obj, obj.id, (int)BitacoraEnum.ModeloArchivos);
        }

        public List<tblM_ArchivosModelos> getlistaByModelo(int obj)
        {
            return _context.tblM_ArchivosModelos.Where(x => x.modeloID.Equals(obj)).ToList();
        }

        public tblM_ArchivosModelos getlistaByID(int obj)
        {
            return _context.tblM_ArchivosModelos.FirstOrDefault(x => x.id.Equals(obj));
        }

    }
}
