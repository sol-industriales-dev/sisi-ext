using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Principal.Generales;
using System.IO;
using Core.Entity.Administrativo.AgendarJunta;
using Core.DTO.Administracion.AgendarJunta;
using Core.DTO.Administracion.SalaJuntas;

namespace Core.DAO.Administracion.AgendarJunta
{
    public interface IAgendarJuntaDAO
    {
        #region SALA JUNTAS
        Dictionary<string, object> GetSalaJuntas(SalaJuntasDTO objParamsDTO);

        Dictionary<string, object> CESalaJuntas(SalaJuntasDTO objParamsDTO);

        Dictionary<string, object> GetDatosActualizarSalaJuntas(SalaJuntasDTO objParamsDTO);

        Dictionary<string, object> EliminarSalaJuntas(SalaJuntasDTO objParamsDTO);

        Dictionary<string, object> FillCboCatEdificios();

        Dictionary<string, object> FillCboCatSalas(SalaJuntasDTO objParamsDTO);

        Dictionary<string, object> FillSalas(SalaJuntasDTO objParamsDTO);
        #endregion

        #region CATALOGO FACULTAMIENTOS
        Dictionary<string, object> GetFacultamientos();

        Dictionary<string, object> CrearFacultamiento(FacultamientoDTO objParamsDTO);

        Dictionary<string, object> ActualizarFacultamiento(FacultamientoDTO objParamsDTO);

        Dictionary<string, object> EliminarFacultamiento(FacultamientoDTO objParamsDTO);

        Dictionary<string, object> FillCboUsuarios();

        Dictionary<string, object> FillCboTipoFacultamientos();

        Dictionary<string, object> GetDatosActualizarFacultamiento(FacultamientoDTO objParamsDTO);
        #endregion

        #region GENERAL
        bool VerificarAcceso();
        #endregion
    }
}