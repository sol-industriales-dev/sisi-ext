using Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Infrastructure.Utils
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Retorna el tag DescriptionAttribute del enum
        /// </summary>
        /// <param name="eLement">Elemento enum</param>
        /// <returns>Descripcioón del tag DescriptionAttribute del enum</returns>
        public static string GetDescription(this Enum eLement)
        {
            Type type = eLement.GetType();
            MemberInfo[] memberInfo = type.GetMember(eLement.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] atributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (atributes != null && atributes.Length > 0)
                {
                    return ((DescriptionAttribute)atributes[0]).Description;
                }
            }
            return eLement.ToString();
        }
        /// <summary>
        /// Convierte un enumerable en una lista de tipo combo
        /// </summary>
        /// <typeparam name="T">Enumerable</typeparam>
        /// <returns>Combobox list del enunerable</returns>
        public static List<ComboDTO> ToCombo<T>() where T : IConvertible
        {
            var lst = Enum.GetValues(typeof(T)).Cast<T>().ToList().Select(x => new ComboDTO
            {
                Text = Regex.Replace(x.ToString(), "([a-z])([A-Z])", "$1 $2"),
                Value = x.GetHashCode()
            }).ToList();
            return lst;
        }
        /// <summary>
        /// Convierte los valores del enumerable en una cadena
        /// </summary>
        /// <typeparam name="T">Enumerable</typeparam>
        /// <returns>Cadena con valores</returns>
        public static string ToLineValue<T>() where T : IConvertible
        {
            var cadena = string.Empty;
            Enum.GetValues(typeof(T)).Cast<T>().ToList().ForEach(e =>
            {
                var hash = e.GetHashCode().ToString();
                cadena += hash + " ";
            });
            return cadena;
        }
        /// <summary>
        /// Convierte los valores del enumerable en una cadena
        /// </summary>
        /// <typeparam name="T">Enumerable</typeparam>
        /// <param name="separador">Separador de valores entre la cadena</param>
        /// <returns>Cadena con valores</returns>
        public static string ToLineValue<T>(string separador) where T : IConvertible
        {
            var cadena = string.Empty;
            Enum.GetValues(typeof(T)).Cast<T>().ToList().ForEach(e =>
            {
                var hash = e.GetHashCode().ToString();
                cadena += hash + separador;
            });
            return cadena.Remove(cadena.Length - 1);
        }
        /// <summary>
        /// Convierte los valores del enumerable en una cadena para parametros de consulta IN
        /// </summary>
        /// <typeparam name="T">Enumerable</typeparam>
        /// <returns>Cadena con valores</returns>
        public static string ToParamInValue<T>() where T : IConvertible
        {
            var cadena = "(";
            Enum.GetValues(typeof(T)).Cast<T>().ToList().ForEach(e =>
            {
                cadena += "?,";
            });
            cadena = cadena.Remove(cadena.Length - 1) + ")";
            return cadena;
        }
        /// <summary>
        /// Convierte los elementos de un enum en una arreglo iterable
        /// </summary>
        /// <typeparam name="T">Enumerable</typeparam>
        /// <returns>Combobox list del enunerable</returns>
        public static T[] ToArray<T>() where T : IConvertible
        {
            return (T[])Enum.GetValues(typeof(T));
        }
        public static T ToEnumerable<T>(this int enumerable)
        {
            return (T)Enum.Parse(typeof(T), enumerable.ToString());
        }
        /// <summary>
        /// Verifica si el valor existe en enumerable
        /// </summary>
        /// <typeparam name="T">Enumerable</typeparam>
        /// <param name="comparador">Valor a comparar</param>
        public static bool Any<T>(T comparador) where T : IConvertible
        {
            return Enum.IsDefined(typeof(T), comparador);
        }
        /// <summary>
        /// Verifica si el valor existe en enumerable
        /// </summary>
        /// <typeparam name="T">Enumerable</typeparam>
        /// <param name="comparador">Valor a comparar</param>
        public static bool Any<T>(int comparador) where T : IConvertible
        {
            return Enum.IsDefined(typeof(T), comparador);
        }
        /// <summary>
        /// Verifica si el valor existe en enumerable
        /// </summary>
        /// <typeparam name="T">Enumerable</typeparam>
        /// <param name="comparador">Valor a comparar</param>
        public static bool Any<T>(string comparador) where T : IConvertible
        {
            return Enum.IsDefined(typeof(T), comparador);
        }
    }
}
