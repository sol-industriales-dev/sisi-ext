using Core.DTO.Facturacion;
using Core.Entity.Kubrix;
using Core.Entity.Kubrix.Analisis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Kubrix
{
    public interface ICatalogoDAO
    {
        List<ComboDTO> getCboDivision();
        List<tblK_catCcDiv> getlstCcDiv(string cc, int idDiv);
        List<tblK_catDivision> getLstDiv();
        List<tblK_Bal12> getlstBal12(string cc);
    }
}
