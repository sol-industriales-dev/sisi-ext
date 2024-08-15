
using Core.DAO.Maquinaria.Caratulas;
using Core.DTO.Maquinaria._Caratulas;
using Core.DTO.Maquinaria.Caratulas;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Auth;
using Core.Entity.Maquinaria._Caratulas;
using Core.Entity.Maquinaria.Caratulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service.Maquinaria.Caratulas
{
    public class CaratulasService : ICaratulasDAO
    {
        private ICaratulasDAO m_ICaratulaDAO;

        private ICaratulasDAO CaratulaDAO
        {
            get { return m_ICaratulaDAO; }
            set { m_ICaratulaDAO = value; }
        }
        public CaratulasService(ICaratulasDAO CaratulaDAO)
        {
            this.CaratulaDAO = CaratulaDAO;
        }

        public List<ComboDTO> FillAreasCuentas()
        {
            return CaratulaDAO.FillAreasCuentas();
        }

        public List<ComboDTO> FillCboModelo()
        {
            return CaratulaDAO.FillCboModelo();
        }

        public List<ComboDTO> FillCboGrupo()
        {
            return CaratulaDAO.FillCboGrupo();
        }

        public List<ComboDTO> FillCaratulas()
        {
            return CaratulaDAO.FillCaratulas();
        }

        public List<CaratulaGuardadoDTO> GetCaratula()
        {
            return CaratulaDAO.GetCaratula();
        }

        public List<CaratulaGuardadoDTO> MostrarArchivo(HttpPostedFileBase archivo, decimal tipoCambio)
        {
            return CaratulaDAO.MostrarArchivo(archivo, tipoCambio);
        }

        public bool GuardarModelo(tblM_Caratulas parametros)
        {
            return CaratulaDAO.GuardarModelo(parametros);
        }

        public bool GuardarCaratula(List<CaratulaGuardadoDTO> listaCaratula, decimal tipoCambio, int idTecnico, int idSubdireccionMaquinaria)
        {
            return CaratulaDAO.GuardarCaratula(listaCaratula, tipoCambio, idTecnico, idSubdireccionMaquinaria);
        }


        public List<IndicadoresCaratulaDTO> GetIndicadores()
        {
            return CaratulaDAO.GetIndicadores();
        }

        public bool GuardarIndicadores(tblM_IndicadoresCaratula parametros)
        {
            return CaratulaDAO.GuardarIndicadores(parametros);
        }

        public bool ActualizarIndicadoresNuevos(List<tblM_IndicadoresCaratula> lstNuevoIndicadores)
        {
            return CaratulaDAO.ActualizarIndicadoresNuevos(lstNuevoIndicadores);
        }

        public List<ReporteCaratulaDTO> GetReporte(int idCaratula)
        {
            return CaratulaDAO.GetReporte(idCaratula);
        }

        public Dictionary<string, object> ListaAutorizantes(int idCaratula)
        {
            return CaratulaDAO.ListaAutorizantes(idCaratula);
        }

        public Dictionary<string, object> Autorizar(authDTO Autorizar, int id)
        {
            return CaratulaDAO.Autorizar(Autorizar, id);
        }

        public Dictionary<string, object> Rechazar(authDTO Rechazar)
        {
            return CaratulaDAO.Rechazar(Rechazar);
        }

        public Dictionary<string, object> EnviarCorreo(List<Byte[]> downloadPDF)
        {
            return this.CaratulaDAO.EnviarCorreo(downloadPDF);
        }
        public Dictionary<string, object> EnviarCorreoGuardarCaratula(List<Byte[]> downloadPDF)
        {
            return this.CaratulaDAO.EnviarCorreoGuardarCaratula(downloadPDF);
        }
        public Dictionary<string, object> CargarCaratulaActiva(List<int> lstTipoHoraDia)
        {
            return this.CaratulaDAO.CargarCaratulaActiva(lstTipoHoraDia);
        }
        public bool ObtenerAutorizante(int id)
        {
            return this.CaratulaDAO.ObtenerAutorizante(id);
        }


        public List<ComboDTO> obtenerComboCaratulras()
        {
            return this.CaratulaDAO.obtenerComboCaratulras();
        }

        public List<ComboDTO> obtenerCC()
        {
            return this.CaratulaDAO.obtenerCC();
        }
        public Dictionary<string, object> obtenerCaratula(int idCaratula, int idCC, int status, int esHoraDia)
        {
            return this.CaratulaDAO.obtenerCaratula(idCaratula, idCC, status, esHoraDia);
        }
        public Dictionary<string, object> obtenerHistorialCaratulas(int estatus)
        {
            return this.CaratulaDAO.obtenerHistorialCaratulas(estatus);
        }


        public Dictionary<string, object> obtenerAgrupacionCaratulas()
        {
            return this.CaratulaDAO.obtenerAgrupacionCaratulas();
        }
        public List<ComboDTO> obtenerGrupos()
        {
            return this.CaratulaDAO.obtenerGrupos();
        }
        public List<ComboDTO> obtenerModelos(int idGrupo, int Editar, int Agrupacion)
        {
            return this.CaratulaDAO.obtenerModelos(idGrupo,Editar,Agrupacion);
        }
        public bool EliminarAgrupacion(int id)
        {
            return this.CaratulaDAO.EliminarAgrupacion(id);
        }
        public bool EliminarModeloAgrupacion(int id)
        {
            return this.CaratulaDAO.EliminarModeloAgrupacion(id);
        }
        public Dictionary<string, object> GuardarEditar(CaratulaEncDTO parametros)
        {
            return this.CaratulaDAO.GuardarEditar(parametros);
        }
        public List<ComboDTO> ObtenerAgrupaciones()
        {
            return this.CaratulaDAO.ObtenerAgrupaciones();
        }
        public List<tblM_CaratulaConceptos> conceptosMoneda()
        {
            return this.CaratulaDAO.conceptosMoneda();
        }
    }
}
