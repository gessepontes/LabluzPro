using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using LabluzPro.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.Net.Mime;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace LabluzPro.Domain.Diversos
{
    public static class Diverso
    {
        public static string GenerateMD5(string yourString)
        {
            if (yourString == "" || yourString == null)
            {
                yourString = "123456";
            }
            return string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(yourString)).Select(s => s.ToString("x2")));
        }

        public static async void SaveImage(IFormFile foto, string destino, string sFoto)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            string path = "";

            switch (destino)
            {
                case "CERTIFICADO":
                    path = Path.Combine(Directory.GetCurrentDirectory(), builder.GetSection(key: "AppConfiguration")["ResourcesPathFisical:Certificado"], sFoto);
                    break;
                case "CONTRATO":
                    path = Path.Combine(Directory.GetCurrentDirectory(), builder.GetSection(key: "AppConfiguration")["ResourcesPathFisical:Contrato"], sFoto);
                    break;
                case "DOCUMENTO":
                    path = Path.Combine(Directory.GetCurrentDirectory(), builder.GetSection(key: "AppConfiguration")["ResourcesPathFisical:Documento"], sFoto);
                    break;
                case "USUARIO":
                    path = Path.Combine(Directory.GetCurrentDirectory(), builder.GetSection(key: "AppConfiguration")["ResourcesPathFisical:Usuario"], sFoto);
                    break;
            }

            if (foto == null || foto.Length == 0) { }
            else
            {

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await foto.CopyToAsync(stream);
                }

            }
        }

        public static string Download(string sFoto, string origem)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            string path = "";

            switch (origem)
            {
                case "CERTIFICADO":
                    path = Path.Combine(Directory.GetCurrentDirectory(), builder.GetSection(key: "AppConfiguration")["ResourcesPathFisical:Certificado"], sFoto);
                    break;
                case "CONTRATO":
                    path = Path.Combine(Directory.GetCurrentDirectory(), builder.GetSection(key: "AppConfiguration")["ResourcesPathFisical:Contrato"], sFoto);
                    break;
                case "DOCUMENTO":
                    path = Path.Combine(Directory.GetCurrentDirectory(), builder.GetSection(key: "AppConfiguration")["ResourcesPathFisical:Documento"], sFoto);
                    break;
                case "USUARIO":
                    path = Path.Combine(Directory.GetCurrentDirectory(), builder.GetSection(key: "AppConfiguration")["ResourcesPathFisical:Usuario"], sFoto);
                    break;
            }


            return path;
        }


        public static string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        // o metodo isCPFCNPJ recebe dois parâmetros:
        // uma string contendo o cpf ou cnpj a ser validado
        // e um valor do tipo boolean, indicando se o método irá
        // considerar como válido um cpf ou cnpj em branco.
        // o retorno do método também é do tipo boolean:
        // (true = cpf ou cnpj válido; false = cpf ou cnpj inválido)
        public static bool IsCPFCNPJ(string cpfcnpj, bool vazio)
        {
            if (string.IsNullOrEmpty(cpfcnpj))
                return vazio;
            else
            {
                int[] d = new int[14];
                int[] v = new int[2];
                int j, i, soma;
                string Sequencia, SoNumero;

                SoNumero = Regex.Replace(cpfcnpj, "[^0-9]", string.Empty);

                //verificando se todos os numeros são iguais
                if (new string(SoNumero[0], SoNumero.Length) == SoNumero) return false;

                // se a quantidade de dígitos numérios for igual a 11
                // iremos verificar como CPF
                if (SoNumero.Length == 11)
                {
                    for (i = 0; i <= 10; i++) d[i] = Convert.ToInt32(SoNumero.Substring(i, 1));
                    for (i = 0; i <= 1; i++)
                    {
                        soma = 0;
                        for (j = 0; j <= 8 + i; j++) soma += d[j] * (10 + i - j);

                        v[i] = (soma * 10) % 11;
                        if (v[i] == 10) v[i] = 0;
                    }
                    return (v[0] == d[9] & v[1] == d[10]);
                }
                // se a quantidade de dígitos numérios for igual a 14
                // iremos verificar como CNPJ
                else if (SoNumero.Length == 14)
                {
                    Sequencia = "6543298765432";
                    for (i = 0; i <= 13; i++) d[i] = Convert.ToInt32(SoNumero.Substring(i, 1));
                    for (i = 0; i <= 1; i++)
                    {
                        soma = 0;
                        for (j = 0; j <= 11 + i; j++)
                            soma += d[j] * Convert.ToInt32(Sequencia.Substring(j + 1 - i, 1));

                        v[i] = (soma * 10) % 11;
                        if (v[i] == 10) v[i] = 0;
                    }
                    return (v[0] == d[12] & v[1] == d[13]);
                }
                // CPF ou CNPJ inválido se
                // a quantidade de dígitos numérios for diferente de 11 e 14
                else return false;
            }
        }

        public static bool SendEmail(string _para, string _subject, string _body, List<string> _anexos)
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json").Build();

                AlternateView view = AlternateView.CreateAlternateViewFromString(_body, null, MediaTypeNames.Text.Html);
                LinkedResource resource = new LinkedResource(Path.Combine(Directory.GetCurrentDirectory(), builder.GetSection(key: "AppConfiguration")["ResourcesPathFisical:Logo"]))
                {
                    ContentId = "Imagem1"
                };

                view.LinkedResources.Add(resource);


                MailMessage mailMessage = new MailMessage(builder.GetSection(key: "Email")["User"], _para, _subject, _body)
                {
                    IsBodyHtml = true
                };


                if (_anexos != null)
                {
                    foreach (string item in _anexos)
                    {
                        mailMessage.Attachments.Add(new Attachment(item));
                    }
                }

                mailMessage.AlternateViews.Add(view);
                mailMessage.Priority = MailPriority.Normal;

                SmtpClient client = new SmtpClient(builder.GetSection(key: "Email")["Host"])
                {
                    Port = Convert.ToInt16(builder.GetSection(key: "Email")["Port"]),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(builder.GetSection(key: "Email")["User"], builder.GetSection(key: "Email")["Password"])
                };

                client.Send(mailMessage);

                return true;
            }
            catch (Exception e)
            {
                e.Message.ToString();
                return false;
            }

        }


        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return input;
            }
            else
            {
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
            }

        }
       
        public static bool Acesso(Usuario user, int idPagina)
        {
            foreach (var item in user.UsuarioPagina)
            {
                if (item.idPagina == idPagina) {
                    return true;
                }
            }
           
            return false;
        }

    }
}
