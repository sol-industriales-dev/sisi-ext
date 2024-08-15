using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Overhaul
{
    public interface IArchivosNotasCreditoDAO
    {
        void Guardar(tblM_ArchivosNotasCredito archivos);
        List<tblM_ArchivosNotasCredito> getlistaByNota(int obj);
        tblM_ArchivosNotasCredito getlistaByID(int obj);

        List<string> getlistaArchivosAdjuntos(int obj);
    }
}
