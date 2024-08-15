using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Cursos;
using Core.DTO.Principal.Generales;
namespace Core.DAO.Cursos
{
    public interface ICursoDAO
    {
         tblCU_Curso GuardarCurso(tblCU_Curso objCurso);
         tblCU_Modulo GuardarModulo(tblCU_Modulo objModulo);
         tblCU_ModuloDet GuardarModuloDet(tblCU_ModuloDet objModuloDet);
         List<tblCU_Curso> GetListCursos(int id, string folio, string nombre, int combo);
         List<tblCU_Modulo> getModuloid(int id);
         List<tblCU_ModuloDet> getModuloDetid(int id);
         List<ComboDTO> FillComboCurso();
         tblCU_Examen GuardarExamen(tblCU_Examen objExamen);
         tblCU_ExamenPregunta GuardarPregunta(tblCU_ExamenPregunta objPregunta);
         tblCU_ExamenRespuesta GuardarRespuesta(tblCU_ExamenRespuesta objRespuesta);
         void EliminaModulo(int idModulo);
         void EliminaModuloDet(int idModulo);
        //prueba raaguilar 1/218
        bool ComparaPagina(int pagina, int modulo);
        void EliminaModuloDetbyId(int id);
        int EliminaCurso(int IdCurso);
        List<object> GetListDeptos();
        void AsignarCurso(List<tblCU_Asignacion> lstAsignacion);

    }
}
