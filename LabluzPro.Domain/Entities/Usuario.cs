using LabluzPro.Domain.Diversos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabluzPro.Domain.Entities
{
    public class Usuario
    {
        public int ID { get; set; }

        [Display(Name = "Imagem")]
        public string sImagem { get; set; }

        [Required(ErrorMessage = "{0} é um campo obrigatório.")]
        string Nome;
        [Display(Name = "Certificado")]
        public string sNome
        {
            get
            {
                if (string.IsNullOrEmpty(Nome))
                {
                    return Nome;
                }
                return Diverso.FirstCharToUpper(Nome);
            }
            set
            {
                Nome = value;
            }

        }

        string Senha;
        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        public string sSenha
        {
            get
            {
                if (string.IsNullOrEmpty(Senha))
                {
                    return Senha;
                }
                return Diverso.GenerateMD5(Senha);
            }
            set
            {
                Senha = value;
            }

        }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "{0} é um campo obrigatório.")]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string sEmail { get; set; }

        [Display(Name = "Ativo")]
        public bool bAtivo { get; set; }

        public DateTime dCadastro { get; set; }
        public int iCodUsuarioMovimentacao { get; set; }

        public virtual ICollection<UsuarioPagina> UsuarioPerfil { get; set; }

    }
}
