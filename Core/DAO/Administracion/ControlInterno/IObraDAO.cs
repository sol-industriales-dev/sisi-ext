using Core.DTO.Administracion.ControlInterno.Obra;
using Core.Entity.Administrativo.ControlInterno.Obra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Administracion.ControlInterno
{
    public interface IObraDAO
    {
        #region Gestion
        /// <summary>
        /// Obras de Sigoplan
        /// </summary>
        List<tblM_O_CatCCAC> getAllObra();
        List<tblM_O_CatCCAC> getLstObra(BusqObraGestionDTO busq);
        #endregion
        #region _formObra
        bool GuardarObra(List<tblM_O_CatCCAC> lst);
        tblM_O_CatCCAC getFormDesdeClave(string clave);
        List<tblM_O_CatCCAC> getAutocompleteClave(string term);
        #endregion
        #region combobox

        #endregion
    }
}
