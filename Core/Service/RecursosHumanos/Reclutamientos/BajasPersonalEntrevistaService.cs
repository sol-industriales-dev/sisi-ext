using Core.DAO.RecursosHumanos.BajasPersonal;
using Core.DTO.RecursosHumanos.BajasPersonal;
using Core.Entity.RecursosHumanos.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.RecursosHumanos.Reclutamientos
{
    public class BajasPersonalEntrevistaService : IBajasPersonalEntrevistaDAO
    {
        public IBajasPersonalEntrevistaDAO r_bajasPersonalEntrevista { get; set; }
        public BajasPersonalEntrevistaService(IBajasPersonalEntrevistaDAO BajasPersonal)
        {
            this.r_bajasPersonalEntrevista = BajasPersonal;
        }

        #region ENTREVISTA

        public Dictionary<string, object> GetCapturada(int idRegistro, int empresa)
        {
            return r_bajasPersonalEntrevista.GetCapturada(idRegistro, empresa);
        }

        public Dictionary<string, object> GetBaja(int id, int empresa)
        {
            return r_bajasPersonalEntrevista.GetBaja(id, empresa);
        }
        public Dictionary<string, object> CrearEditarEntrevista(BajaPersonalDTO objDTO)
        {
            return r_bajasPersonalEntrevista.CrearEditarEntrevista(objDTO);
        }

        public Dictionary<string, object> GetDatosPersona(int claveEmpleado, string nombre, int empresa)
        {
            return r_bajasPersonalEntrevista.GetDatosPersona(claveEmpleado,nombre,empresa);
        }

        public Dictionary<string, object> FillCboPreguntas(int idPregunta)
        {
            return r_bajasPersonalEntrevista.FillCboPreguntas(idPregunta);
        }

        #endregion

        #region FILL COMBOS
        public Dictionary<string, object> FillCboEstados()
        {
            return r_bajasPersonalEntrevista.FillCboEstados();
        }

        public Dictionary<string, object> FillCboMunicipios(int idEstado)
        {
            return r_bajasPersonalEntrevista.FillCboMunicipios(idEstado);
        }

        public Dictionary<string, object> FillCboEstadosCiviles()
        {
            return r_bajasPersonalEntrevista.FillCboEstadosCiviles();
        }

        public Dictionary<string, object> FillCboEscolaridades()
        {
            return r_bajasPersonalEntrevista.FillCboEscolaridades();
        }
        #endregion

        #region GRALES
        public List<tblRH_CatEmpleados> getCatEmpleadosGeneral(string term, int empresa)
        {
            return r_bajasPersonalEntrevista.getCatEmpleadosGeneral(term, empresa);
        }
        #endregion
    }
}
