using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos.Plantilla;
using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.RecursosHumanos.Captura
{
    public interface IPlantillaPersonalDAO
    {
        int GuardarPlantilla(tblRH_PP_PlantillaPersonal plantilla, List<tblRH_PP_PlantillaPersonal_Det> Dets, List<tblRH_PP_PlantillaPersonal_Aut> Auts);
        tblRH_PP_PlantillaPersonal GetPlantillaSP(int id);
        List<PlantillaPersonal2DTO> GetPlantillaEK(string cc);
        bool AutorizarPlantilla(int plantillaID, int autorizacion,int estatus);
        List<tblRH_PP_PlantillaPersonal_Aut> GetAutorizadores(int plantillaID);
        List<tblRH_PP_PlantillaPersonal> GetPlantillas(string cc,int estatus);
        bool EnviarCorreo(int plantillaID, int autorizacion, int estatus);
        List<ComboDTO> GetDepartamentos(string cc);
        List<ComboDTO> GetPuestos();
        List<ComboDTO> GetTipoNomina();
        PlantillaReporteDTO GetReporte(int id, int empresa);
        PlantillaReporteDTO GetReportePlantilla(string cc, int empresa = 0);
        List<ComboDTO> FillComboCC(bool plantilla);
    }
}
