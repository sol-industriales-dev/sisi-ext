using Core.Entity.Maquinaria.Captura;
using Core.Enum.Maquinaria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Captura
{
    public interface IAceitesLubricantesDAO
    {
        List<tblM_CatAceitesLubricantes> GetAllAceitesLubricantes(int tipoId, string economico);

        object ExistenciaLubricante(string almacen);

        #region Catálogo Lubricantes
        Dictionary<string, object> CargarCatalogoLubricantes();
        Dictionary<string, object> GetComboModelos();
        Dictionary<string, object> GuardarNuevoLubricante(tblM_CatAceitesLubricantes lubricante);
        Dictionary<string, object> EditarLubricante(tblM_CatAceitesLubricantes lubricante, AceiteLubricanteEnum subConjuntoID_Anterior);
        Dictionary<string, object> EliminarLubricante(tblM_CatAceitesLubricantes lubricante);
        #endregion
    }
}
