using Core.DTO;
using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utils
{
    public class PersonalUtilities
    {
        public static string NombreCompleto(string nombre, string apellidoPaterno, string apellidoMaterno)
        {
            string nombreCompleto = "";

            nombreCompleto = nombre ?? "";
            nombreCompleto += string.IsNullOrEmpty(apellidoPaterno) ? "" : string.IsNullOrEmpty(nombreCompleto) ? apellidoPaterno : " " + apellidoPaterno;
            nombreCompleto += string.IsNullOrEmpty(apellidoMaterno) ? "" : string.IsNullOrEmpty(nombreCompleto) ? apellidoMaterno : " " + apellidoMaterno;

            return nombreCompleto;
        }

        public static string NombreCompletoMayusculas(string nombre, string apellidoPaterno, string apellidoMaterno)
        {
            return NombreCompleto(nombre, apellidoPaterno, apellidoMaterno).ToUpper();
        }

        public static string NombreCompletoMinusculas(string nombre, string apellidoPaterno, string apellidoMaterno)
        {
            return NombreCompleto(nombre, apellidoPaterno, apellidoMaterno).ToLower();
        }

        public static string NombreCompletoPrimerLetraMayuscula(string nombreCompleto)
        {
            string nombreCompletoNuevoFormato = string.Empty;
            if (!string.IsNullOrEmpty(nombreCompleto))
            {
                string[] arrNombreCompleto = nombreCompleto.Split(' ');
                foreach (var item in arrNombreCompleto)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        if (string.IsNullOrEmpty(nombreCompletoNuevoFormato))
                            nombreCompletoNuevoFormato = string.Format("{0}{1}", item.Substring(0, 1).ToUpper(), item.Substring(1, (item.Length - 1)).ToLower());
                        else
                            nombreCompletoNuevoFormato += string.Format(" {0}{1}", item.Substring(0, 1).ToUpper(), item.Substring(1, (item.Length - 1)).ToLower());
                    }
                }
            }
            return nombreCompletoNuevoFormato;
        }

        public static string PrimerLetraMayuscula(string palabras)
        {
            string palabraNuevoFormato = string.Empty;
            if (!string.IsNullOrEmpty(palabras))
            {
                string[] arrPalabras = palabras.Split(' ');
                foreach (var item in arrPalabras)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        if (string.IsNullOrEmpty(palabraNuevoFormato))
                            palabraNuevoFormato = string.Format("{0}{1}", item.Substring(0, 1).ToUpper(), item.Substring(1, (item.Length - 1)).ToLower());
                        else
                            palabraNuevoFormato += string.Format(" {0}", item.ToLower());
                    }
                }
            }
            return palabraNuevoFormato;
        }

        /// <summary>
        /// Se obtiene nombre corto de la empresa.
        /// </summary>
        /// <returns>Retorna nombre corto de la empresa, ejemplo: "Construplan".</returns>
        public static string GetNombreEmpresa()
        {
            string nombreEmpresa = string.Empty;
            switch (vSesiones.sesionEmpresaActual)
            {
                case (int)EmpresaEnum.Construplan: nombreEmpresa = "Construplan"; break;
                case (int)EmpresaEnum.Arrendadora: nombreEmpresa = "Arrendadora"; break;
                case (int)EmpresaEnum.Colombia: nombreEmpresa = "Colombia"; break;
                case (int)EmpresaEnum.Peru: nombreEmpresa = "Perú"; break;
                case (int)EmpresaEnum.GCPLAN: nombreEmpresa = "GCPLAN"; break;
            }
            return nombreEmpresa;
        }
    }
}
