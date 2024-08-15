using Core.DAO.RecursosHumanos.Captura;
using Core.Entity.RecursosHumanos.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Principal.Generales;//clase combo
using Core.DTO.RecursosHumanos;//tomar aditivapersonal dto por ser odbc
using Core.Entity.RecursosHumanos.Captura; //altas cantidad

namespace Core.Service.RecursosHumanos.Captura
{

    //reaguilar 23/11/17
    public class AditivaDeductivaService: IAditivaDeductivaDAO
    {
        public IAditivaDeductivaDAO AditivaDeductivaDAO { get; set; }
        public AditivaDeductivaService(IAditivaDeductivaDAO AditivaDeductivaDAO)
        {
            this.AditivaDeductivaDAO = AditivaDeductivaDAO;
        }
        public List<tblRH_CatPuestos> getCatPuestos(string term)
        {
            return AditivaDeductivaDAO.getCatPuestos(term);
        }
        public List<tblRH_CatPuestos> getAllCatPuestos(string cC)
        {
            return AditivaDeductivaDAO.getAllCatPuestos(cC);
        }
        public List<ComboDTO> getListaCC()
        {
            return AditivaDeductivaDAO.getListaCC();
        }
        public AditivaPersonal getInfoAditiva(string puestoDescripcion, string CentroCostos)
        {
            return AditivaDeductivaDAO.getInfoAditiva(puestoDescripcion, CentroCostos);
        }
        //pruebas guardado 4/12/17
        public tblRH_AditivaDeductiva GuardarAditivaDeduc(tblRH_AditivaDeductiva objAditivaDe)
        {
            return AditivaDeductivaDAO.GuardarAditivaDeduc(objAditivaDe);
        }
        //todo el listado de a ditivas deductivas para mostrar en la bandeja de gestion
        public List<tblRH_AditivaDeductiva> GetListAditivaDeducPersonal()
        {
            return AditivaDeductivaDAO.GetListAditivaDeducPersonal();
        }

        //obtener aditivadeductiva por id "usar para ver el detalle"
        public List<tblRH_AditivaDeductiva> getListAditivaDeductivaPendientes(int id, string cc, string folio, int estado)
        {
            return AditivaDeductivaDAO.getListAditivaDeductivaPendientes(id,cc,folio,estado);
        }
    
        //eliminar aditivadeductiva formato raguilar 08/12/17
        public void eliminarFormato(int formatoID)
        {
            AditivaDeductivaDAO.eliminarFormato(formatoID);
        }

        //obtener aditivadeductiva por id raguilar 12/12/17
        public tblRH_AditivaDeductiva getFormatoAditivaDeductivaByID(int idFormatoAditiva)
        {
           return AditivaDeductivaDAO.getFormatoAditivaDeductivaByID(idFormatoAditiva);
        }
        public Dictionary<string, object> AutorizarPlantilla(int plantillaID, int autorizacion, int estatus, string comentario)
        {
            return AditivaDeductivaDAO.AutorizarPlantilla(plantillaID, autorizacion, estatus, comentario);
        }
        public bool EnviarCorreo(int plantillaID, int autorizacion, int estatus)
        {
           return AditivaDeductivaDAO.EnviarCorreo(plantillaID,autorizacion,estatus);
        }

        public void GuardarSolicitudEvidencia(int solicitudID, string dir)
        {
            AditivaDeductivaDAO.GuardarSolicitudEvidencia(solicitudID, dir);
        }
    }
}
