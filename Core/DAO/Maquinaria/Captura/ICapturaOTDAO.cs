using Core.Entity.Maquinaria.Captura;
using Core.Entity.RecursosHumanos.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Captura
{
    public interface ICapturaOTDAO
    {
        void Guardar(tblM_CapOrdenTrabajo obj, int idBL);

        List<tblRH_CatEmpleados> getCatEmpleados(string term, List<string> CentroCostos);
        List<tblM_CapOrdenTrabajo> getListaOT(string cc, List<string> listcc);
        List<tblM_DetOrdenTrabajo> getListaOTDet(string cc, List<string> listcc,DateTime fechaInicio,DateTime fechaFin);
        tblM_CapOrdenTrabajo GetOTbyID(int id );
        tblM_CapOrdenTrabajo GetOTByEconomico(int idEconomico);
    }
}
