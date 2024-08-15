using Core.DAO.Maquinaria.Overhaul;
using Core.Entity.Maquinaria.Overhaul;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Overhaul
{
    public class ArchivosNotasCreditoDAO : GenericDAO<tblM_ArchivosNotasCredito>, IArchivosNotasCreditoDAO
    {
        public void Guardar(tblM_ArchivosNotasCredito obj)
        {
            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.NotasCreditoArchivos);
            else
                Update(obj, obj.id, (int)BitacoraEnum.NotasCreditoArchivos);
        }

        public List<tblM_ArchivosNotasCredito> getlistaByNota(int obj)
        {
            return _context.tblM_ArchivosNotasCredito.Where(x => x.NotaCreditoID.Equals(obj)).ToList();
        }

        public tblM_ArchivosNotasCredito getlistaByID(int obj)
        {
            return _context.tblM_ArchivosNotasCredito.FirstOrDefault(x => x.id.Equals(obj));
        }

        public List<string> getlistaArchivosAdjuntos(int obj)
        {
            List<string> archivos = new List<string>();


            archivos = _context.tblM_ArchivosNotasCredito.Where(x => x.NotaCreditoID.Equals(obj) && x.tipoArchivo == 1).Select(a => a.rutaArchivo).ToList();

            return archivos;

        }

    }
}
