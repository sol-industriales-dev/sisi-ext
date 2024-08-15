using Core.DTO.Maquinaria.Reporte;
using Core.Entity.RecursosHumanos.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public interface ICapturaOTDetDAO
    {
        void Guardar(List<tblM_DetOrdenTrabajo> obj, int idOT);

        List<tblM_DetOrdenTrabajo> getListaOTDet(int obj);
        tblRH_CatEmpleados getCatEmpleados(string term);
        void delete(int id);
        List<RepGastosMaquinariaGrid> GetCostosHoraHombre(int EconomicoID, DateTime FI, DateTime FF);
        List<RepGastosMaquinariaGrid> FillMotivosParo(RepGastosFiltrosDTO obj);
        List<RepGastosMaquinariaGrid> FillUsuario(RepGastosFiltrosDTO obj);

        byte[] obtenerImagen(int id, int tipoEvidencia);
        List<byte[]> obtenerImagenLista(int id, int tipoEvidencia);
    }
}
