using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

public static class Encriptado
{
    private static readonly byte[] Key = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["Clave"]);

    public static string Encriptar(string textoPlano)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.GenerateIV();

            using (MemoryStream ms = new MemoryStream())
            {
                // Guardamos el IV primero
                ms.Write(aes.IV, 0, aes.IV.Length);

                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(textoPlano);
                }

                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    public static string Desencriptar(string textoCifrado)
    {
        byte[] datos = Convert.FromBase64String(textoCifrado);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;

            byte[] iv = new byte[16];
            Array.Copy(datos, 0, iv, 0, 16);
            aes.IV = iv;

            using (MemoryStream ms = new MemoryStream(datos, 16, datos.Length - 16))
            using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
            using (StreamReader sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
