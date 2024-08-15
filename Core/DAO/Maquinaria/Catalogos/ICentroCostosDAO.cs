using Core.DTO.Maquinaria.Catalogos;
using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Principal.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Catalogos
{
    public interface ICentroCostosDAO
    {

        string getNombreAreaCuent(string areaCuenta);
        IList<tblM_CentroCostos> FillGridCC(tblM_CentroCostos cc);
        IList<InventarioDTO> fillGridMaquinaria(int cc, int idGrupo);
        string getNombreCC(int cc);
        string getNombreCcFromSIGOPLAN(string centroCosto);
        string getNombreCC(string cc);
        IList<InventarioDTO> fillListaMaquinaria(string grupo, string tipo, string modelo);
        string getNombreCCFix(string centroCostos);
        List<ComboDTO> getListaCC();
        List<ComboDTO> getListaCC_Rep_Costos();
        
        List<ComboDTO> ListCC();
        string getNombreCCArrendadoraRH(string centroCostos);
        tblP_CC getEntityCCConstruplan(int ccID);
        List<ComboDTO> getListaCCConstruplan();
        List<ComboDTO> getListaCCSIGOPLAN();
        List<ComboDTO> getLstCcArrendadoraProd();

    }
}
