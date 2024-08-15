using Core.DAO.Encuestas;
using Core.DTO.Encuestas;
using Core.DTO.Principal.Usuarios;
using Core.Entity.Encuestas;
using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Encuestas
{
    public class EncuestaService : IEncuestasDAO
    {
        #region Atributos
        private IEncuestasDAO m_encuestasDAO;
        #endregion

        #region Propiedades
        public IEncuestasDAO EncuestasDAO
        {
            get { return m_encuestasDAO; }
            set { m_encuestasDAO = value; }
        }
        #endregion

        #region Constructores
        public EncuestaService(IEncuestasDAO encuestasDAO)
        {
            this.EncuestasDAO = encuestasDAO;
        }
        #endregion
        public int saveEncuesta(tblEN_Encuesta obj)
        {
            return EncuestasDAO.saveEncuesta(obj);
        }
        public bool GuardarEncAsigUsuario(List<tblEN_EncuestaAsignaUsuario> lst)
        {
            return EncuestasDAO.GuardarEncAsigUsuario(lst);
        }
        public bool eliminaEncAsigUsuario(tblEN_EncuestaAsignaUsuario obj)
        {
            return EncuestasDAO.eliminaEncAsigUsuario(obj);
        }
        public EncuestaDTO getEncuesta(int id)
        {
            return EncuestasDAO.getEncuesta(id);
        }
        public EncuestaDTO getEncuestaByID(int id)
        {
            return EncuestasDAO.getEncuestaByID(id);
        }
        public void saveEncuestaResult(List<tblEN_Resultado> obj, int encuestaID, string comentario)
        {
            EncuestasDAO.saveEncuestaResult(obj, encuestaID, comentario);
        }

        public List<muestroEncuestaDTO> getEncuestaCerteza(List<int> lstEncuesta, DateTime fechaInicio, DateTime fechaFinal)
        {
            return EncuestasDAO.getEncuestaCerteza(lstEncuesta, fechaInicio, fechaFinal);
        }

        public EncuestaDTO getEncuestaResult(int id)
        {
            return EncuestasDAO.getEncuestaResult(id);
        }
        public EncuestaDTO getEncuestaValidar(int id)
        {
            return EncuestasDAO.getEncuestaValidar(id);
        }
        public EncuestaDTO getEncuestaValidarUpdate(int id)
        {
            return EncuestasDAO.getEncuestaValidarUpdate(id);
        }
        public List<tblEN_Encuesta> getEncuestasByOwner(int id)
        {
            return EncuestasDAO.getEncuestasByOwner(id);
        }

        public MemoryStream exportData(List<int> listaEncuestas, DateTime fechaInicio, DateTime fechaFinal)
        {
            return EncuestasDAO.exportData(listaEncuestas, fechaInicio, fechaFinal);
        }

        public List<tblEN_Encuesta> getEncuestasByDepto(int id)
        {
            return EncuestasDAO.getEncuestasByDepto(id);
        }
        public List<EncuestaDTO> getEncuestasTodasByDepto(int id)
        {
            return EncuestasDAO.getEncuestasTodasByDepto(id);
        }
        public List<EncuestaDTO> getEncuestasTodasByDeptoConEncuestaID(int id)
        {
            return EncuestasDAO.getEncuestasTodasByDeptoConEncuestaID(id);
        }
        public List<tblP_Departamento> getDepartamentos()
        {
            return EncuestasDAO.getDepartamentos();
        }
        public int savePregunta(tblEN_Preguntas obj)
        {
            return EncuestasDAO.savePregunta(obj);
        }
        public void delPregunta(int id)
        {
            EncuestasDAO.delPregunta(id);
        }

        public List<EncuestaResultsDTO> getEncuestaResults(int id, DateTime fechaInicio, DateTime fechaFin)
        {
            return EncuestasDAO.getEncuestaResults(id, fechaInicio, fechaFin);
        }
        public void setAceptarEncuesta(int id)
        {
            EncuestasDAO.setAceptarEncuesta(id);
        }
        public void setRechazarEncuesta(int id)
        {
            EncuestasDAO.setRechazarEncuesta(id);
        }
        public void setAceptarEncuestaUpdate(int id)
        {
            EncuestasDAO.setAceptarEncuestaUpdate(id);
        }
        public void setRechazarEncuestaUpdate(int id)
        {
            EncuestasDAO.setRechazarEncuestaUpdate(id);
        }
        public List<EncuestaAsignaUsuarioDTO> getEncuestaAsignaUsuario()
        {
            return EncuestasDAO.getEncuestaAsignaUsuario();
        }
        public List<EncuestaEstatusDTO> getEncuestaPendiente()
        {
            return EncuestasDAO.getEncuestaPendiente();
        }
        public List<EncuestaEstatusDTO> getEncuestaAceptada()
        {
            return EncuestasDAO.getEncuestaAceptada();
        }
        public List<EncuestaEstatusDTO> getEncuestaRechazada()
        {
            return EncuestasDAO.getEncuestaRechazada();
        }
        public List<EncuestaResults2DTO> getEncuestaResultsResumen(int id, DateTime fechaInicio, DateTime fechaFin)
        {
            return EncuestasDAO.getEncuestaResultsResumen(id, fechaInicio, fechaFin);
        }
        public List<EncuestaResults2DTO> getEncuestaResultsResumenPorPregunta(int id, DateTime fechaInicio, DateTime fechaFin)
        {
            return EncuestasDAO.getEncuestaResultsResumenPorPregunta(id, fechaInicio, fechaFin);
        }
        public List<EncuestaResults2DTO> getEncuestaResultsResumenPorDepartamento(int tipoResumen, int areaID, int area2ID, DateTime fechaInicio, DateTime fechaFin)
        {
            return EncuestasDAO.getEncuestaResultsResumenPorDepartamento(tipoResumen, areaID, area2ID, fechaInicio, fechaFin);
        }
        public List<EncuestaResults2DTO> getEncuestaResultsResumenPorDepartamentoYear(int id, int areaID, int area2ID, DateTime fechaInicio, DateTime fechaFin)
        {
            return EncuestasDAO.getEncuestaResultsResumenPorDepartamentoYear(id, areaID, area2ID, fechaInicio, fechaFin);
        }
        public List<EncuestaResults2DTO> getEncuestaResultsResumenPorPreguntaYear(int id, DateTime fechaInicio, DateTime fechaFin)
        {
            return EncuestasDAO.getEncuestaResultsResumenPorPreguntaYear(id, fechaInicio, fechaFin);
        }
        public List<EncuestaResults2DTO> getEncuestaResultsResumenNumero(int id, DateTime fechaInicio, DateTime fechaFin)
        {
            return EncuestasDAO.getEncuestaResultsResumenNumero(id, fechaInicio, fechaFin);
        }
        public List<EncuestaResultsDTO> getEncuestaResultsNoContestadas(int id, DateTime fechaInicio, DateTime fechaFin)
        {
            return EncuestasDAO.getEncuestaResultsNoContestadas(id, fechaInicio, fechaFin);
        }
        public List<GraficaEncuestaDTO> getGraficaByEncuesta(int id, DateTime fechaInicio, DateTime fechaFin)
        {
            return EncuestasDAO.getGraficaByEncuesta(id, fechaInicio, fechaFin);
        }
        public void sendEncuesta(string asunto, SendEncuestaDTO obj)
        {
            EncuestasDAO.sendEncuesta(asunto, obj);
        }

        public bool validarCantidadEncuestas(int id)
        {

            return EncuestasDAO.validarCantidadEncuestas(id);
        }

        public void correoEncuesta(SendEncuestaDTO obj)
        {
            EncuestasDAO.correoEncuesta(obj);
        }

        public List<tblEN_Encuesta> getEncuestasTodosDepto()
        {
            return EncuestasDAO.getEncuestasTodosDepto();
        }
        public List<Preguntas3EstrellasDTO> getPreguntas3Estrellas(int id, DateTime fechaInicio, DateTime fechaFin)
        {
            return EncuestasDAO.getPreguntas3Estrellas(id, fechaInicio, fechaFin);
        }
        public List<ClienteEmpresaDTO> getClienteEmpresas(DateTime fechaInicio, DateTime fechaFin)
        {
            return EncuestasDAO.getClienteEmpresas(fechaInicio, fechaFin);
        }
        public List<EncuestaResults2DTO> getEncuestaResultsResumenPorEmpresa(int tipoResumen, int areaID, DateTime fechaInicio, DateTime fechaFin, string empresa)
        {
            return EncuestasDAO.getEncuestaResultsResumenPorEmpresa(tipoResumen, areaID, fechaInicio, fechaFin, empresa);
        }
        public List<EncuestaResults2DTO> getEncuestaResultsResumenPorEmpresaYear(int tipoResumen, int areaID, DateTime fechaInicio, DateTime fechaFin, string empresa)
        {
            return EncuestasDAO.getEncuestaResultsResumenPorEmpresaYear(tipoResumen, areaID, fechaInicio, fechaFin, empresa);
        }

        public List<EncuestaDTO> getEncuestas()
        {
            return EncuestasDAO.getEncuestas();
        }

        public void UpdateTelefonica(int encuestaID, bool telefonica)
        {
            EncuestasDAO.UpdateTelefonica(encuestaID, telefonica);
        }

        public void UpdateNotificacion(int encuestaID, bool notificacion)
        {
            EncuestasDAO.UpdateNotificacion(encuestaID, notificacion);
        }

        public void UpdatePapel(int encuestaID, bool papel)
        {
            EncuestasDAO.UpdatePapel(encuestaID, papel);
        }

        public List<string> getEmpresas()
        {
            return EncuestasDAO.getEmpresas();
        }

        public List<tblP_Usuario> getClientes(string empresa)
        {
            return EncuestasDAO.getClientes(empresa);
        }

        public EncuestaDTO getEncuestaTelefonica(int id)
        {
            return EncuestasDAO.getEncuestaTelefonica(id);
        }

        public tblP_Usuario nuevoCliente(UsuarioDTO nuevo)
        {
            return EncuestasDAO.nuevoCliente(nuevo);
        }

        public tblEN_Encuesta_Usuario nuevoEncuestaUsuario(int encuestaID, int usuarioResponderID, string asunto, int usuarioTelefonoID)
        {
            return EncuestasDAO.nuevoEncuestaUsuario(encuestaID, usuarioResponderID, asunto, usuarioTelefonoID);
        }

        public tblEN_Encuesta_Usuario nuevoEncuestaUsuarioPapel(int encuestaID, int usuarioResponderID, string asunto, int usuarioTelefonoID, string rutaArchivo)
        {
            return EncuestasDAO.nuevoEncuestaUsuarioPapel(encuestaID, usuarioResponderID, asunto, usuarioTelefonoID, rutaArchivo);
        }

        public List<EncuestaCheckUsuarioDTO> getUsuariosCheck(int encuestaID)
        {
            return EncuestasDAO.getUsuariosCheck(encuestaID);
        }

        public UsuarioDTO checkUsuario(string nombre)
        {
            return EncuestasDAO.checkUsuario(nombre);
        }

        public void GuardarUsuariosCheck(int encuestaID, List<EncuestaCheckUsuarioDTO> usuarios)
        {
            EncuestasDAO.GuardarUsuariosCheck(encuestaID, usuarios);
        }

        public bool checkEncuestaTelefonica(int encuestaID)
        {
            return EncuestasDAO.checkEncuestaTelefonica(encuestaID);
        }

        public EncuestaDTO getEncuestaCheck(int encuestaID)
        {
            return EncuestasDAO.getEncuestaCheck(encuestaID);
        }

        public bool checkTelefonica(int encuestaID)
        {
            return EncuestasDAO.checkTelefonica(encuestaID);
        }

        public List<tblP_Usuario> getUsuarios(string term)
        {
            return EncuestasDAO.getUsuarios(term);
        }

        public List<tblEN_Estrellas> getEstrellas()
        {
            return EncuestasDAO.getEstrellas();
        }
        public void setCrearEncuesta(int empID, int crearID, bool crear)
        {
            EncuestasDAO.setCrearEncuesta(empID, crearID, crear);
        }
        public List<EncuestaCheckUsuarioDTO> getUsuarioPermisosCheck(int usuarioID)
        {
            return EncuestasDAO.getUsuarioPermisosCheck(usuarioID);
        }
        public EncuestaCheckUsuarioDTO getUsuarioPermisosCheckPorEncuesta(int usuarioID, int encuestaID)
        {
            return EncuestasDAO.getUsuarioPermisosCheckPorEncuesta(usuarioID, encuestaID);
        }
        public void guardarPermisosCheck(List<EncuestaCheckUsuarioDTO> lstPermisos)
        {
            EncuestasDAO.guardarPermisosCheck(lstPermisos);
        }
        public List<tblEN_Encuesta> getEncuestasPorPermisosCheck(int idDepartamento)
        {
            return EncuestasDAO.getEncuestasPorPermisosCheck(idDepartamento);
        }
        public List<EncuestaCheckUsuarioDTO> getRptPermisosEncuestas(List<int> listaEncuestas, List<int> usuarios, List<int> Departamentos)
        {
            return EncuestasDAO.getRptPermisosEncuestas(listaEncuestas, usuarios, Departamentos);
        }

        public tblEN_Encuesta_Usuario getEncuestaClienteByID(int id)
        {
            return EncuestasDAO.getEncuestaClienteByID(id);
        }

        public void enviarCorreoUsuariosAsignados(List<SendEncuestaDTO> listado)
        {
            EncuestasDAO.enviarCorreoUsuariosAsignados(listado);
        }
    }
}
