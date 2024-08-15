using Core.DTO;
using Core.DTO.Utils.Auth;
using Core.Enum.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utils
{
    public static class ListUtilities
    {
        /// <summary>
        /// Convertidor de lista en una cadena.
        /// </summary>
        /// <param name="myList">Lista del llamando</param>
        /// <param name="separador">Caracter separador</param>
        /// <returns>El contenido de la lista en una cadena</returns>
        public static string ToLine(this List<string> myList, string separador)
        {
            separador = separador ?? string.Empty;
            var sb = new System.Text.StringBuilder();
            myList.ForEach(s =>
            {
                sb.Append("'" + s + "'").Append(separador);
            });
            sb = sb.Length == 0 ? sb : sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
        /// <summary>
        /// Retorta una cadena de ? para usar como parámetros en un query parametrizado.
        /// </summary>
        /// <param name="myList">Lista de strings</param>
        /// <returns>Retorna una lista de (?,?,?) donde ? es cada elemento de la lista.</returns>
        public static string ToParamInValue(this IEnumerable<string> myList)
        {
            var sb = new StringBuilder().Append("(");

            string parametros = String.Join(",", myList.Select(x => "?"));

            sb.Append(parametros).Append(")");

            return sb.ToString();
        }
        /// <summary>
        /// Retorta una cadena de ? para usar como parámetros en un query parametrizado.
        /// </summary>
        /// <param name="myList">Lista genérica</param>
        /// <returns>Retorna una lista de (?,?,?) donde ? es cada elemento de la lista.</returns>
        public static string ToParamInValue<T>(this IEnumerable<T> myList) where T : struct
        {
            var sb = new StringBuilder().Append("(");

            string parametros = String.Join(",", myList.Select(x => "?"));

            sb.Append(parametros).Append(")");

            return sb.ToString();
        }
        /// <summary>
        /// Asigna al siguiente autorizante
        /// </summary>
        /// <param name="lst">Autorizantes</param>
        /// <returns>Autorizantes</returns>
        public static List<authDTO> setSigAutorizante(this List<authDTO> lst)
        {
            var idAuth = vSesiones.sesionUsuarioDTO.id;
            var orden = lst.Where(a => a.authEstado.Equals(authEstadoEnum.EnEspera)).Min(m => m.orden);
            if (lst.Any(a => a.authEstado.Equals(authEstadoEnum.EnEspera) && a.idAuth.Equals(idAuth) && a.orden.Equals(orden)))
            {
                lst.FirstOrDefault(a => a.orden.Equals(orden)).authEstado = authEstadoEnum.EnTurno;
                lst.FirstOrDefault(a => a.orden.Equals(orden)).clase = "AutorizanteEnTurno";
            }
            return lst.OrderBy(o => o.orden).ToList();
        }
        /// <summary>
        /// Separa una lista en N cantidad de listas del mismo tipo dependiendo del tamaño indicado.
        /// </summary>
        /// <param name="source">Lista inicial</param>
        /// <param name="chunkSize">Tamaño en el que se va a separar la lista</param>
        /// <returns>Lista de listas separadas.</returns>
        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}
