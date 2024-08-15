using Core.DTO.RecursosHumanos.BajasPersonal;
using Core.Entity.RecursosHumanos.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.RecursosHumanos.BajasPersonal
{
    public interface IBajasPersonalEntrevistaDAO
    {

        #region ENTREVISTA
        Dictionary<string, object> GetCapturada(int idRegistro, int empresa);
        Dictionary<string, object> GetBaja(int id, int empresa);
        Dictionary<string, object> CrearEditarEntrevista(BajaPersonalDTO objDTO);
        Dictionary<string, object> GetDatosPersona(int claveEmpleado, string nombre, int empresa);
        Dictionary<string, object> FillCboPreguntas(int idPregunta);

        #endregion

        #region FILL COMBOS
        Dictionary<string, object> FillCboEstados();

        Dictionary<string, object> FillCboMunicipios(int idEstado);

        Dictionary<string, object> FillCboEstadosCiviles();

        Dictionary<string, object> FillCboEscolaridades();
        #endregion

        #region GRALES
        List<tblRH_CatEmpleados> getCatEmpleadosGeneral(string term, int empresa);
        
        #endregion
    }
}
