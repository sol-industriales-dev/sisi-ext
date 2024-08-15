using Core.Entity.RecursosHumanos.Catalogo;
using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Principal.Generales;//clase combo
using Core.DTO.RecursosHumanos;//tomar aditivapersonal dto por ser odbc
namespace Core.DAO.RecursosHumanos.Captura
{       //raguilar 23/11/17
    public  interface IAditivaDeductivaDAO
    {
        List<tblRH_CatPuestos> getCatPuestos(string cC);//aprovechamos la clase 
        List<tblRH_CatPuestos> getAllCatPuestos(string cC);//aprovechamos la clase 
        List<ComboDTO> getListaCC();
        AditivaPersonal getInfoAditiva(string puestoDescripcion,string CentroCostos);
        tblRH_AditivaDeductiva GuardarAditivaDeduc(tblRH_AditivaDeductiva objAditivaDEductiva);
        List<tblRH_AditivaDeductiva> GetListAditivaDeducPersonal();
        List<tblRH_AditivaDeductiva> getListAditivaDeductivaPendientes(int id, string cc, string folio, int estado);
        void eliminarFormato(int formatoID);
        tblRH_AditivaDeductiva getFormatoAditivaDeductivaByID(int idFormatoAditiva);
        Dictionary<string, object> AutorizarPlantilla(int plantillaID, int autorizacion, int estatus, string comentario);
        bool EnviarCorreo(int plantillaID, int autorizacion, int estatus);
        void GuardarSolicitudEvidencia(int solicitudID, string dir);
    }
}
