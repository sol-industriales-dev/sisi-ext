using Core.DAO.RecursosHumanos.Captura;
using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos.Plantilla;
using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.RecursosHumanos.Captura
{
    public class PlantillaPersonalService : IPlantillaPersonalDAO
    {
        #region Atributos
        private IPlantillaPersonalDAO m_DAO;
        #endregion
        #region Propiedades
        public IPlantillaPersonalDAO mDAO
        {
            get { return m_DAO; }
            set { m_DAO = value; }
        }
        #endregion
        #region Constructor
        public PlantillaPersonalService(IPlantillaPersonalDAO mDAO)
        {
            this.mDAO = mDAO;
        }
        #endregion

        public int GuardarPlantilla(tblRH_PP_PlantillaPersonal plantilla, List<tblRH_PP_PlantillaPersonal_Det> Dets, List<tblRH_PP_PlantillaPersonal_Aut> Auts)
        {
            return mDAO.GuardarPlantilla(plantilla, Dets, Auts);
        }
        public tblRH_PP_PlantillaPersonal GetPlantillaSP(int id)
        {
            return mDAO.GetPlantillaSP(id);
        }
        public List<PlantillaPersonal2DTO> GetPlantillaEK(string cc)
        {
            return mDAO.GetPlantillaEK(cc);
        }
        public bool AutorizarPlantilla(int plantillaID, int autorizacion, int estatus)
        {
            return mDAO.AutorizarPlantilla(plantillaID, autorizacion, estatus);
        }
        public List<tblRH_PP_PlantillaPersonal_Aut> GetAutorizadores(int plantillaID)
        {
            return mDAO.GetAutorizadores(plantillaID);
        }
        public List<tblRH_PP_PlantillaPersonal> GetPlantillas(string cc, int estatus)
        {
            return mDAO.GetPlantillas(cc, estatus);
        }
        public bool EnviarCorreo(int plantillaID, int autorizacion, int estatus)
        {
            return mDAO.EnviarCorreo(plantillaID, autorizacion,estatus);
        }
        public List<ComboDTO> GetDepartamentos(string cc)
        {
            return mDAO.GetDepartamentos(cc);
        }
        public List<ComboDTO> GetPuestos()
        {
            return mDAO.GetPuestos();
        }
        public List<ComboDTO> GetTipoNomina()
        {
            return mDAO.GetTipoNomina();
        }
        public PlantillaReporteDTO GetReporte(int id, int empresa)
        {
            return mDAO.GetReporte(id, empresa);
        }
        public PlantillaReporteDTO GetReportePlantilla(string cc, int empresa = 0)
        {
            return mDAO.GetReportePlantilla(cc, empresa);
        }
        public List<ComboDTO> FillComboCC(bool plantilla)
        {
            return mDAO.FillComboCC(plantilla);
        }
    }
}
