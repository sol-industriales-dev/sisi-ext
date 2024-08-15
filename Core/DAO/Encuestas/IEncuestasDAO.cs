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

namespace Core.DAO.Encuestas
{
    public interface IEncuestasDAO
    {

        List<EncuestaCheckUsuarioDTO> getRptPermisosEncuestas(List<int> listaEncuestas, List<int> usuarios, List<int> Departamentos);
        MemoryStream exportData(List<int> listaEncuestas, DateTime fechaInicio, DateTime fechaFinal);
        List<muestroEncuestaDTO> getEncuestaCerteza(List<int> lstEncuesta, DateTime fechaInicio, DateTime fechaFinal);
        int saveEncuesta(tblEN_Encuesta encuesta);
        bool GuardarEncAsigUsuario(List<tblEN_EncuestaAsignaUsuario> lst);
        bool eliminaEncAsigUsuario(tblEN_EncuestaAsignaUsuario obj);
        EncuestaDTO getEncuesta(int id);
        EncuestaDTO getEncuestaByID(int id);
        void saveEncuestaResult(List<tblEN_Resultado> obj, int encuestaID, string comentario);
        EncuestaDTO getEncuestaResult(int id);
        EncuestaDTO getEncuestaValidar(int id);
        EncuestaDTO getEncuestaValidarUpdate(int id);
        List<EncuestaResultsDTO> getEncuestaResults(int id, DateTime fechaInicio, DateTime fechaFin);
        void setAceptarEncuesta(int id);
        void setRechazarEncuesta(int id);
        void setAceptarEncuestaUpdate(int id);
        void setRechazarEncuestaUpdate(int id);
        List<EncuestaAsignaUsuarioDTO> getEncuestaAsignaUsuario();
        List<EncuestaEstatusDTO> getEncuestaPendiente();
        List<EncuestaEstatusDTO> getEncuestaAceptada();
        List<EncuestaEstatusDTO> getEncuestaRechazada();
        List<EncuestaResults2DTO> getEncuestaResultsResumen(int id, DateTime fechaInicio, DateTime fechaFin);
        List<EncuestaResults2DTO> getEncuestaResultsResumenPorPregunta(int id, DateTime fechaInicio, DateTime fechaFin);
        List<EncuestaResults2DTO> getEncuestaResultsResumenPorDepartamento(int tipoResumen, int areaID, int area2ID, DateTime fechaInicio, DateTime fechaFin);
        List<EncuestaResults2DTO> getEncuestaResultsResumenPorDepartamentoYear(int id, int areaID, int area2ID, DateTime fechaInicio, DateTime fechaFin);
        List<EncuestaResults2DTO> getEncuestaResultsResumenPorPreguntaYear(int id, DateTime fechaInicio, DateTime fechaFin);
        List<EncuestaResults2DTO> getEncuestaResultsResumenNumero(int id, DateTime fechaInicio, DateTime fechaFin);
        List<EncuestaResultsDTO> getEncuestaResultsNoContestadas(int id, DateTime fechaInicio, DateTime fechaFin);
        List<GraficaEncuestaDTO> getGraficaByEncuesta(int id, DateTime fechaInicio, DateTime fechaFin);
        List<tblEN_Encuesta> getEncuestasByOwner(int id);
        List<tblEN_Encuesta> getEncuestasByDepto(int id);
        List<EncuestaDTO> getEncuestasTodasByDepto(int id);
        List<EncuestaDTO> getEncuestasTodasByDeptoConEncuestaID(int id);
        List<tblP_Departamento> getDepartamentos();
        int savePregunta(tblEN_Preguntas obj);
        void delPregunta(int id);
        void sendEncuesta(string asunto, SendEncuestaDTO obj);
        bool validarCantidadEncuestas(int id);
        void correoEncuesta(SendEncuestaDTO obj);
        List<tblEN_Encuesta> getEncuestasTodosDepto();
        List<Preguntas3EstrellasDTO> getPreguntas3Estrellas(int id, DateTime fechaInicio, DateTime fechaFin);
        List<ClienteEmpresaDTO> getClienteEmpresas(DateTime fechaInicio, DateTime fechaFin);
        List<EncuestaResults2DTO> getEncuestaResultsResumenPorEmpresa(int tipoResumen, int areaID, DateTime fechaInicio, DateTime fechaFin, string empresa);
        List<EncuestaResults2DTO> getEncuestaResultsResumenPorEmpresaYear(int tipoResumen, int areaID, DateTime fechaInicio, DateTime fechaFin, string empresa);
        List<EncuestaDTO> getEncuestas();
        void UpdateTelefonica(int encuestaID, bool telefonica);
        void UpdateNotificacion(int encuestaID, bool notificacion);
        void UpdatePapel(int encuestaID, bool papel);
        List<string> getEmpresas();
        List<tblP_Usuario> getClientes(string empresa);
        EncuestaDTO getEncuestaTelefonica(int id);
        tblP_Usuario nuevoCliente(UsuarioDTO nuevo);
        tblEN_Encuesta_Usuario nuevoEncuestaUsuario(int encuestaID, int usuarioResponderID, string asunto, int usuarioTelefonoID);
        tblEN_Encuesta_Usuario nuevoEncuestaUsuarioPapel(int encuestaID, int usuarioResponderID, string asunto, int usuarioTelefonoID,string rutaArchivo);
        List<EncuestaCheckUsuarioDTO> getUsuariosCheck(int encuestaID);
        UsuarioDTO checkUsuario(string nombre);
        void GuardarUsuariosCheck(int encuestaID, List<EncuestaCheckUsuarioDTO> usuarios);
        bool checkEncuestaTelefonica(int encuestaID);
        EncuestaDTO getEncuestaCheck(int encuestaID);
        bool checkTelefonica(int encuestaID);
        List<tblP_Usuario> getUsuarios(string term);
        List<tblEN_Estrellas> getEstrellas();
        void setCrearEncuesta(int empID, int crearID, bool crear);
        List<EncuestaCheckUsuarioDTO> getUsuarioPermisosCheck(int usuarioID);
        EncuestaCheckUsuarioDTO getUsuarioPermisosCheckPorEncuesta(int usuarioID, int encuestaID);
        void guardarPermisosCheck(List<EncuestaCheckUsuarioDTO> lstPermisos);
        List<tblEN_Encuesta> getEncuestasPorPermisosCheck(int idDepartamento);
        tblEN_Encuesta_Usuario getEncuestaClienteByID(int id);
        void enviarCorreoUsuariosAsignados(List<SendEncuestaDTO> listado);
    }
}
