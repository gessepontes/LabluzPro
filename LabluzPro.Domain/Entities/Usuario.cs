using LabluzPro.Domain.Diversos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabluzPro.Domain.Entities
{
    public class Usuario
    {

        public Usuario()
        {
            dCadastro = DateTime.Now;
        }

        public int ID { get; set; }

        [Display(Name = "Foto")]
        public string sImagem { get; set; }

        string Nome;

        [Required(ErrorMessage = "{0} é um campo obrigatório.")]
        [Display(Name = "Usuário")]
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

        [Display(Name = "Telefone")]
        [Required(ErrorMessage = "{0} é um campo obrigatório.")]
        [StringLength(15)]
        public string sTelefone { get; set; }

        [Display(Name = "Ativo")]
        public bool bAtivo { get; set; }

        public DateTime dCadastro { get; set; }
        public int iCodUsuarioMovimentacao { get; set; }

        [Display(Name = "Páginas")]
        public virtual ICollection<UsuarioPagina> UsuarioPagina { get; set; }
        public List<Paginas> PaginaSelecionado { get; set; }
    }

    public enum Paginas : int
    {
        [Display(Name = "Responsável")]
        Responsavel = 1,
        [Display(Name = "Usuário")]
        Usuario = 2,
        [Display(Name = "Certificado")]
        Certificado = 3,
        [Display(Name = "Contrato")]
        Contrato = 4,
        [Display(Name = "Documentos")]
        Documentos = 5
    }
}
