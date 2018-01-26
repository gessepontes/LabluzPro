using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.IO;

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
            string path = "";

            switch (destino)
            {
                case "CERTIFICADO":
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\certificado", sFoto);
                    break;
                case "CONTRATO":
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\contrato", sFoto);
                    break;
                case "DOCUMENTO":
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\documento", sFoto);
                    break;
                case "USUARIO":
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\usuario", sFoto);
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

        public static string ReadSetting(string key)
        {
            string result = "";

            try
            {
                result = ConfigurationManager.AppSettings[key].ToString();
            }
            catch (ConfigurationErrorsException)
            {
            }

            return result;

        }

        public static string BodyEmail(int iTipo, string texto = "")
        {
            string body = "";

            switch (iTipo)
            {
                case 1:
                    body += "<table style='border - collapse:collapse; border - spacing:0; Margin - left:auto; Margin - right:auto; width: 600px; background - color:#ffffff;font-size:14px;table-layout:fixed'><tbody><tr><td style='padding:0;vertical-align:top;text-align:left'><div><div style='font-size:32px;line-height:32px'>&nbsp;</div></div>";
                    body += "<table style='border - collapse:collapse; border - spacing:0; table - layout:fixed; width: 100 % '><tbody><tr><td style='padding: 0; vertical - align:top; padding - left:32px; padding - right:32px; word -break:break-word; word - wrap:break-word'><h1 style='font - style:normal; font - weight:700; Margin - bottom:18px; Margin - top:0; font - size:36px; line - height:44px; font - family:Ubuntu,sans - serif; color:#565656;text-align:center'><strong style='font-weight:bold'>Mudança de Senha</strong></h1>";
                    body += "</td></tr></tbody></table><table style='border - collapse:collapse; border - spacing:0; table - layout:fixed; width: 100 % '><tbody><tr><td style='padding: 0; vertical - align:top; padding - left:32px; padding - right:32px; word -break:break-word; word - wrap:break-word'><div style='min - height:20px'>&nbsp;</div></td></tr></tbody></table><table style='border - collapse:collapse; border - spacing:0; table - layout:fixed; width: 100 % '><tbody><tr><td style='padding: 0; vertical - align:top; padding - left:32px; padding - right:32px; word -break:break-word; word - wrap:break-word'><p style='font - style:normal; font - weight:400; Margin - bottom:24px; Margin - top:0; line - height:24px; font - family:Ubuntu,sans - serif; color:#787778;font-size:16px'>Obrigado por participar desta emoção. Segue o link para mudança de sua senha.</p></td></tr></tbody></table><table style='border-collapse:collapse;border-spacing:0;table-layout:fixed;width:100%'><tbody><tr><td style='padding:0;vertical-align:top;padding-left:1px;padding-right:32px;word-break:break-word;word-wrap:break-word'><div><u></u><a style='border-radius:3px;display:inline-block;font-size:14px;font-weight:700;line-height:24px;padding:13px 35px 12px 35px;text-align:center;text-decoration:none!important;color:#fff;font-family:Ubuntu,sans-serif;background-color:#4169E1' href=\"{1}\" target='_blank'>Clique aqui</a><u></u></div></td></tr></tbody></table><table style='border-collapse:collapse;border-spacing:0;table-layout:fixed;width:100%'><tbody><tr><td style='padding:0;vertical-align:top;padding-left:1px;padding-right:32px;word-break:break-word;word-wrap:break-word'><div style='min-height:14px'>&nbsp;</div></td></tr></tbody></table><table style='border-collapse:collapse;border-spacing:0;table-layout:fixed;width:100%'><tbody><tr><td style='padding:0;vertical-align:top;padding-left:1px;padding-right:32px;word-break:break-word;word-wrap:break-word'><p style='font-style:normal;font-weight:400;Margin-bottom:0;Margin-top:0;line-height:24px;font-family:Ubuntu,sans-serif;color:#787778;font-size:16px'><em>Equipe </em>Society PRO agradece sua preferência.</p></td></tr></tbody></table><div style='font-size:32px;line-height:32px'>&nbsp;</div></td></tr></tbody></table>";

                    break;
                case 2:
                    body += "<table style='border - collapse:collapse; border - spacing:0; Margin - left:auto; Margin - right:auto; width: 600px; background - color:#ffffff;font-size:14px;table-layout:fixed'><tbody><tr><td style='padding:0;vertical-align:top;text-align:left'><div><div style='font-size:32px;line-height:32px'>&nbsp;</div></div>";
                    body += "<table style='border - collapse:collapse; border - spacing:0; table - layout:fixed; width: 100 % '><tbody><tr><td style='padding: 0; vertical - align:top; padding - left:32px; padding - right:32px; word -break:break-word; word - wrap:break-word'><h1 style='font - style:normal; font - weight:700; Margin - bottom:18px; Margin - top:0; font - size:36px; line - height:44px; font - family:Ubuntu,sans - serif; color:#565656;text-align:center'><strong style='font-weight:bold'>Confirmação de Conta</strong></h1>";
                    body += "</td></tr></tbody></table><table style='border - collapse:collapse; border - spacing:0; table - layout:fixed; width: 100 % '><tbody><tr><td style='padding: 0; vertical - align:top; padding - left:32px; padding - right:32px; word -break:break-word; word - wrap:break-word'><div style='min - height:20px'>&nbsp;</div></td></tr></tbody></table><table style='border - collapse:collapse; border - spacing:0; table - layout:fixed; width: 100 % '><tbody><tr><td style='padding: 0; vertical - align:top; padding - left:32px; padding - right:32px; word -break:break-word; word - wrap:break-word'><p style='font - style:normal; font - weight:400; Margin - bottom:24px; Margin - top:0; line - height:24px; font - family:Ubuntu,sans - serif; color:#787778;font-size:16px'>Obrigado por participar desta emoção. Segue o link para confirmação de sua conta.</p></td></tr></tbody></table><table style='border-collapse:collapse;border-spacing:0;table-layout:fixed;width:100%'><tbody><tr><td style='padding:0;vertical-align:top;padding-left:1px;padding-right:32px;word-break:break-word;word-wrap:break-word'><div><u></u><a style='border-radius:3px;display:inline-block;font-size:14px;font-weight:700;line-height:24px;padding:13px 35px 12px 35px;text-align:center;text-decoration:none!important;color:#fff;font-family:Ubuntu,sans-serif;background-color:#4169E1' href=\"{1}\" target='_blank'>Clique aqui</a><u></u></div></td></tr></tbody></table><table style='border-collapse:collapse;border-spacing:0;table-layout:fixed;width:100%'><tbody><tr><td style='padding:0;vertical-align:top;padding-left:1px;padding-right:32px;word-break:break-word;word-wrap:break-word'><div style='min-height:14px'>&nbsp;</div></td></tr></tbody></table><table style='border-collapse:collapse;border-spacing:0;table-layout:fixed;width:100%'><tbody><tr><td style='padding:0;vertical-align:top;padding-left:1px;padding-right:32px;word-break:break-word;word-wrap:break-word'><p style='font-style:normal;font-weight:400;Margin-bottom:0;Margin-top:0;line-height:24px;font-family:Ubuntu,sans-serif;color:#787778;font-size:16px'><em>Equipe </em>Society PRO agradece sua preferência.</p></td></tr></tbody></table><div style='font-size:32px;line-height:32px'>&nbsp;</div></td></tr></tbody></table>";

                    break;
                case 3:
                    body += "<table style='border - collapse:collapse; border - spacing:0; Margin - left:auto; Margin - right:auto; width: 600px; background - color:#ffffff;font-size:14px;table-layout:fixed'><tbody><tr><td style='padding:0;vertical-align:top;text-align:left'><div><div style='font-size:32px;line-height:32px'>&nbsp;</div></div>";
                    body += "<table style='border - collapse:collapse; border - spacing:0; table - layout:fixed; width: 100 % '><tbody><tr><td style='padding: 0; vertical - align:top; padding - left:32px; padding - right:32px; word -break:break-word; word - wrap:break-word'><h1 style='font - style:normal; font - weight:700; Margin - bottom:18px; Margin - top:0; font - size:36px; line - height:44px; font - family:Ubuntu,sans - serif; color:#565656;text-align:center'><strong style='font-weight:bold'>Convite de Partida</strong></h1>";
                    body += "</td></tr></tbody></table><table style='border - collapse:collapse; border - spacing:0; table - layout:fixed; width: 100 % '><tbody><tr><td style='padding: 0; vertical - align:top; padding - left:32px; padding - right:32px; word -break:break-word; word - wrap:break-word'><div style='min - height:20px'>&nbsp;</div></td></tr></tbody></table>" + texto + "<table style='border - collapse:collapse; border - spacing:0; table - layout:fixed; width: 100 % '><tbody><tr><td style='padding: 0; vertical - align:top; padding - left:32px; padding - right:32px; word -break:break-word; word - wrap:break-word'><p style='font - style:normal; font - weight:400; Margin - bottom:24px; Margin - top:0; line - height:24px; font - family:Ubuntu,sans - serif; color:#787778;font-size:16px'>Obrigado por participar desta emoção. Segue o link para confirmação da sua partida.</p></td></tr></tbody></table><table style='border-collapse:collapse;border-spacing:0;table-layout:fixed;width:100%'><tbody><tr><td style='padding:0;vertical-align:top;padding-left:1px;padding-right:32px;word-break:break-word;word-wrap:break-word'><div><u></u><a style='border-radius:3px;display:inline-block;font-size:14px;font-weight:700;line-height:24px;padding:13px 35px 12px 35px;text-align:center;text-decoration:none!important;color:#fff;font-family:Ubuntu,sans-serif;background-color:#4169E1' href=\"{1}\" target='_blank'>Aceitar Partida</a><u></u></div></td></tr></tbody></table><table style='border-collapse:collapse;border-spacing:0;table-layout:fixed;width:100%'><tbody><tr><td style='padding:0;vertical-align:top;padding-left:1px;padding-right:32px;word-break:break-word;word-wrap:break-word'><div style='min-height:14px'>&nbsp;</div></td></tr></tbody></table><table style='border-collapse:collapse;border-spacing:0;table-layout:fixed;width:100%'><tbody><tr><td style='padding:0;vertical-align:top;padding-left:1px;padding-right:32px;word-break:break-word;word-wrap:break-word'><p style='font-style:normal;font-weight:400;Margin-bottom:0;Margin-top:0;line-height:24px;font-family:Ubuntu,sans-serif;color:#787778;font-size:16px'><em>Equipe </em>Society PRO agradece sua preferência.</p></td></tr></tbody></table><div style='font-size:32px;line-height:32px'>&nbsp;</div></td></tr></tbody></table>";

                    break;
                case 4:
                    body += "<table style='border - collapse:collapse; border - spacing:0; Margin - left:auto; Margin - right:auto; width: 600px; background - color:#ffffff;font-size:14px;table-layout:fixed'><tbody><tr><td style='padding:0;vertical-align:top;text-align:left'><div><div style='font-size:32px;line-height:32px'>&nbsp;</div></div>";
                    body += "<table style='border - collapse:collapse; border - spacing:0; table - layout:fixed; width: 100 % '><tbody><tr><td style='padding: 0; vertical - align:top; padding - left:32px; padding - right:32px; word -break:break-word; word - wrap:break-word'><h1 style='font - style:normal; font - weight:700; Margin - bottom:18px; Margin - top:0; font - size:36px; line - height:44px; font - family:Ubuntu,sans - serif; color:#565656;text-align:center'><strong style='font-weight:bold'>Convite de Partida</strong></h1>";
                    body += "</td></tr></tbody></table><table style='border - collapse:collapse; border - spacing:0; table - layout:fixed; width: 100 % '><tbody><tr><td style='padding: 0; vertical - align:top; padding - left:32px; padding - right:32px; word -break:break-word; word - wrap:break-word'><div style='min - height:20px'>&nbsp;</div></td></tr></tbody></table>" + texto + "<table style='border - collapse:collapse; border - spacing:0; table - layout:fixed; width: 100 % '><tbody><tr><td style='padding: 0; vertical - align:top; padding - left:32px; padding - right:32px; word -break:break-word; word - wrap:break-word'><p style='font - style:normal; font - weight:400; Margin - bottom:24px; Margin - top:0; line - height:24px; font - family:Ubuntu,sans - serif; color:#787778;font-size:16px'>Obrigado por participar desta emoção. Segue o link para confirmação da sua partida.</p></td></tr></tbody></table><table style='border-collapse:collapse;border-spacing:0;table-layout:fixed;width:100%'><tbody><tr><td style='padding:0;vertical-align:top;padding-left:1px;padding-right:32px;word-break:break-word;word-wrap:break-word'><div style='min-height:14px'>&nbsp;</div></td></tr></tbody></table><table style='border-collapse:collapse;border-spacing:0;table-layout:fixed;width:100%'><tbody><tr><td style='padding:0;vertical-align:top;padding-left:1px;padding-right:32px;word-break:break-word;word-wrap:break-word'><p style='font-style:normal;font-weight:400;Margin-bottom:0;Margin-top:0;line-height:24px;font-family:Ubuntu,sans-serif;color:#787778;font-size:16px'><em>Equipe </em>Society PRO agradece sua preferência.</p></td></tr></tbody></table><div style='font-size:32px;line-height:32px'>&nbsp;</div></td></tr></tbody></table>";

                    break;
                case 5:
                    body += "<table style='border - collapse:collapse; border - spacing:0; Margin - left:auto; Margin - right:auto; width: 600px; background - color:#ffffff;font-size:14px;table-layout:fixed'><tbody><tr><td style='padding:0;vertical-align:top;text-align:left'><div><div style='font-size:32px;line-height:32px'>&nbsp;</div></div>";
                    body += "<table style='border - collapse:collapse; border - spacing:0; table - layout:fixed; width: 100 % '><tbody><tr><td style='padding: 0; vertical - align:top; padding - left:32px; padding - right:32px; word -break:break-word; word - wrap:break-word'><h1 style='font - style:normal; font - weight:700; Margin - bottom:18px; Margin - top:0; font - size:36px; line - height:44px; font - family:Ubuntu,sans - serif; color:#565656;text-align:center'><strong style='font-weight:bold'>Lista de Relacionados</strong></h1>";
                    body += "</td></tr></tbody></table><table style='border - collapse:collapse; border - spacing:0; table - layout:fixed; width: 100 % '><tbody><tr><td style='padding: 0; vertical - align:top; padding - left:32px; padding - right:32px; word -break:break-word; word - wrap:break-word'><div style='min - height:20px'>&nbsp;</div></td></tr></tbody></table>" + texto + "<table style='border - collapse:collapse; border - spacing:0; table - layout:fixed; width: 100 % '><tbody><tr><td style='padding: 0; vertical - align:top; padding - left:32px; padding - right:32px; word -break:break-word; word - wrap:break-word'><p style='font - style:normal; font - weight:400; Margin - bottom:24px; Margin - top:0; line - height:24px; font - family:Ubuntu,sans - serif; color:#787778;font-size:16px'>Obrigado por participar desta emoção. Segue em anexo a lista de relacionados.</p></td></tr></tbody></table><table style='border-collapse:collapse;border-spacing:0;table-layout:fixed;width:100%'><tbody><tr><td style='padding:0;vertical-align:top;padding-left:1px;padding-right:32px;word-break:break-word;word-wrap:break-word'><div style='min-height:14px'>&nbsp;</div></td></tr></tbody></table><table style='border-collapse:collapse;border-spacing:0;table-layout:fixed;width:100%'><tbody><tr><td style='padding:0;vertical-align:top;padding-left:1px;padding-right:32px;word-break:break-word;word-wrap:break-word'><p style='font-style:normal;font-weight:400;Margin-bottom:0;Margin-top:0;line-height:24px;font-family:Ubuntu,sans-serif;color:#787778;font-size:16px'><em>Equipe </em>Society PRO agradece sua preferência.</p></td></tr></tbody></table><div style='font-size:32px;line-height:32px'>&nbsp;</div></td></tr></tbody></table>";

                    break;
                case 6:
                    body += "<table style='border - collapse:collapse; border - spacing:0; Margin - left:auto; Margin - right:auto; width: 600px; background - color:#ffffff;font-size:14px;table-layout:fixed'><tbody><tr><td style='padding:0;vertical-align:top;text-align:left'><div><div style='font-size:32px;line-height:32px'>&nbsp;</div></div>";
                    body += "<table style='border - collapse:collapse; border - spacing:0; table - layout:fixed; width: 100 % '><tbody><tr><td style='padding: 0; vertical - align:top; padding - left:32px; padding - right:32px; word -break:break-word; word - wrap:break-word'><h1 style='font - style:normal; font - weight:700; Margin - bottom:18px; Margin - top:0; font - size:36px; line - height:44px; font - family:Ubuntu,sans - serif; color:#565656;text-align:center'><strong style='font-weight:bold'>Contato</strong></h1>";
                    body += "</td></tr></tbody></table><table style='border - collapse:collapse; border - spacing:0; table - layout:fixed; width: 100 % '><tbody><tr><td style='padding: 0; vertical - align:top; padding - left:32px; padding - right:32px; word -break:break-word; word - wrap:break-word'><div style='min - height:20px'>&nbsp;</div></td></tr></tbody></table>" + texto + "<table style='border - collapse:collapse; border - spacing:0; table - layout:fixed; width: 100 % '><tbody><tr><td style='padding: 0; vertical - align:top; padding - left:32px; padding - right:32px; word -break:break-word; word - wrap:break-word'><p style='font - style:normal; font - weight:400; Margin - bottom:24px; Margin - top:0; line - height:24px; font - family:Ubuntu,sans - serif; color:#787778;font-size:16px'>Obrigado por participar desta emoção. Segue em anexo a lista de relacionados.</p></td></tr></tbody></table><table style='border-collapse:collapse;border-spacing:0;table-layout:fixed;width:100%'><tbody><tr><td style='padding:0;vertical-align:top;padding-left:1px;padding-right:32px;word-break:break-word;word-wrap:break-word'><div style='min-height:14px'>&nbsp;</div></td></tr></tbody></table><table style='border-collapse:collapse;border-spacing:0;table-layout:fixed;width:100%'><tbody><tr><td style='padding:0;vertical-align:top;padding-left:1px;padding-right:32px;word-break:break-word;word-wrap:break-word'><p style='font-style:normal;font-weight:400;Margin-bottom:0;Margin-top:0;line-height:24px;font-family:Ubuntu,sans-serif;color:#787778;font-size:16px'><em>Equipe </em>Society PRO agradece sua preferência.</p></td></tr></tbody></table><div style='font-size:32px;line-height:32px'>&nbsp;</div></td></tr></tbody></table>";

                    break;
                default:
                    break;
            }

            return body;
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

        public static SelectList listaRodada(string IRODADA = "")
        {
            return new SelectList(new List<SelectListItem>
        {
            new SelectListItem { Text = "Selecione a rodada", Value = ""},
            new SelectListItem { Text = "1", Value = "1"},
            new SelectListItem { Text = "2", Value = "2"},
            new SelectListItem { Text = "3", Value = "3"},
            new SelectListItem { Text = "4", Value = "4"},
            new SelectListItem { Text = "5", Value = "5"},
            new SelectListItem { Text = "6", Value = "6"},
            new SelectListItem { Text = "7", Value = "7"},
            new SelectListItem { Text = "8", Value = "8"},
            new SelectListItem { Text = "9", Value = "9"},
            new SelectListItem { Text = "10", Value = "10"},
            new SelectListItem { Text = "11", Value = "11"},
            new SelectListItem { Text = "12", Value = "12"},
            new SelectListItem { Text = "13", Value = "13"},
            new SelectListItem { Text = "14", Value = "14"},
            new SelectListItem { Text = "15", Value = "15"},
            new SelectListItem { Text = "16", Value = "16"},
            new SelectListItem { Text = "17", Value = "17"},
            new SelectListItem { Text = "18", Value = "18"},
            new SelectListItem { Text = "19", Value = "19"},
            new SelectListItem { Text = "20", Value = "20"},
            new SelectListItem { Text = "21", Value = "21"},
            new SelectListItem { Text = "22", Value = "22"},
            new SelectListItem { Text = "23", Value = "23"},
            new SelectListItem { Text = "Oitavas de Final", Value = "Oitavas de Final"},
            new SelectListItem { Text = "Quartas de Final", Value = "Quartas de Final"},
            new SelectListItem { Text = "Semifinal", Value = "Semifinal"},
            new SelectListItem { Text = "Final", Value = "Final"},
        }, "Value", "Text", IRODADA);

        }
    }
}
