using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.CatNotificantes
{
    public class UsuariosNotificantesDTO
    {
        public int idRelNoti { get; set; }
        public int idUsuario { get; set; }
        public int idConcepto { get; set; }
        public string cc { get; set; }
        public string usrNomCompleto { get; set; }
    }
}
