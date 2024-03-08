using System;
using System.IO;
using System.Linq;

namespace ProjectManager.Domain.Funcoes
{
    public class FuncoesGeral
    {
        public T ToEnum<T>(string value, bool ignoreCase, T defaultValue) where T : struct
        {
            if (string.IsNullOrEmpty(value))
                return defaultValue;

            if (Enum.TryParse(value, ignoreCase, out T result))
                return result;

            return defaultValue;
        }
        public decimal Truncar(decimal value, int dec = 2)
        {
            int fator = 1;
            string strDec = "";
            for (var i = 1; i <= dec; i++)
            {
                strDec += "0";
            }
            fator = int.Parse("1" + strDec);
            return Math.Truncate(value * fator) / fator;
        }

        public bool contemLetras(string texto)
        {
            return texto.Where(c => char.IsLetter(c)).Count() > 0;
        }

        public string ApenasNumeros(string str)
        {
            return new string(str.Where(char.IsDigit).ToArray());
        }

        public string ApenasTextos(string str)
        {
            return new string(str.Where(char.IsLetter).ToArray());
        }

    }

    public class FuncoesFile
    {
        public static void SalvarStringParaArquivo(string xml, string arquivo)
        {
            var dir = Path.GetDirectoryName(arquivo);
            if (dir != null && !Directory.Exists(dir))
            {
                try
                {
                    Directory.CreateDirectory(dir);
                }
                catch
                {
                    throw new DirectoryNotFoundException("Diretório " + dir + " não encontrado!");
                }
            }

            try
            {
                var stw = new StreamWriter(arquivo);
                stw.WriteLine(xml);
                stw.Close();
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível criar o arquivo " + arquivo + "!");
            }
        }
    }
}
