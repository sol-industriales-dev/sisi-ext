using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DAO.Administracion.AgendarJunta;
using Core.Entity.Administrativo.AgendarJunta;
using Core.DTO.Principal.Generales;
using Core.DAO.Administracion.SalaJuntas;
using Core.Entity.Administrativo.SalaJuntas;
using System.Collections.Generic;
using Core.DTO.Administracion.AgendarJunta;
using Core.DTO.Administracion.SalaJuntas;

namespace Core.Service.Administracion.AgendarJunta
{
    public class AgendarJuntaService : IAgendarJuntaDAO
    {
        #region INIT
        public IAgendarJuntaDAO r_SalaJuntas { get; set; }
        public IAgendarJuntaDAO SalaJuntas
        {
            get { return r_SalaJuntas; }
            set { r_SalaJuntas = value; }
        }
        public AgendarJuntaService(IAgendarJuntaDAO SalaJuntas)
        {
            this.SalaJuntas = SalaJuntas;
        }
        #endregion

        #region SALA JUNTAS
        public Dictionary<string, object> GetSalaJuntas(SalaJuntasDTO objParamsDTO)
        {
            return r_SalaJuntas.GetSalaJuntas(objParamsDTO);
        }

        public Dictionary<string, object> CESalaJuntas(SalaJuntasDTO objParamsDTO)
        {
            return r_SalaJuntas.CESalaJuntas(objParamsDTO);
        }

        public Dictionary<string, object> GetDatosActualizarSalaJuntas(SalaJuntasDTO objParamsDTO)
        {
            return r_SalaJuntas.GetDatosActualizarSalaJuntas(objParamsDTO);
        }

        public Dictionary<string, object> EliminarSalaJuntas(SalaJuntasDTO objParamsDTO)
        {
            return r_SalaJuntas.EliminarSalaJuntas(objParamsDTO);
        }

        public Dictionary<string, object> FillCboCatEdificios()
        {
            return r_SalaJuntas.FillCboCatEdificios();
        }

        public Dictionary<string, object> FillCboCatSalas(SalaJuntasDTO objParamsDTO)
        {
            return r_SalaJuntas.FillCboCatSalas(objParamsDTO);
        }

        public Dictionary<string, object> FillSalas(SalaJuntasDTO objParamsDTO)
        {
            return r_SalaJuntas.FillSalas(objParamsDTO);
        }
        #endregion

        #region CATALOGO FACULTAMIENTOS
        public Dictionary<string, object> GetFacultamientos()
        {
            return r_SalaJuntas.GetFacultamientos();
        }

        public Dictionary<string, object> CrearFacultamiento(FacultamientoDTO objParamsDTO)
        {
            return r_SalaJuntas.CrearFacultamiento(objParamsDTO);
        }

        public Dictionary<string, object> ActualizarFacultamiento(FacultamientoDTO objParamsDTO)
        {
            return r_SalaJuntas.ActualizarFacultamiento(objParamsDTO);
        }

        public Dictionary<string, object> EliminarFacultamiento(FacultamientoDTO objParamsDTO)
        {
            return r_SalaJuntas.EliminarFacultamiento(objParamsDTO); 
        }

        public Dictionary<string, object> FillCboUsuarios()
        {
            return r_SalaJuntas.FillCboUsuarios(); 
        }

        public Dictionary<string, object> FillCboTipoFacultamientos()
        {
            return r_SalaJuntas.FillCboTipoFacultamientos(); 
        }

        public Dictionary<string, object> GetDatosActualizarFacultamiento(FacultamientoDTO objParamsDTO)
        {
            return r_SalaJuntas.GetDatosActualizarFacultamiento(objParamsDTO); 
        }
        #endregion

        #region GENERAL
        public bool VerificarAcceso()
        {
            return r_SalaJuntas.VerificarAcceso();
        }
        #endregion
    }
}