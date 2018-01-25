using LabluzPro.Domain.Diversos;
using System;
using System.ComponentModel.DataAnnotations;

namespace LabluzPro.Domain.Entities
{
    public class Documento
    {
        public Documento() {
            dCadastro = DateTime.Now;
        }

        public int ID { get; set; }

        [Required(ErrorMessage = "{0} é um campo obrigatório.")]
        [Display(Name = "Vencimento")]
        public DateTime dVencimento { get; set; }

        [Display(Name = "Imagem")]
        public string sImagem { get; set; }

        [Required(ErrorMessage = "{0} é um campo obrigatório.")]
        string Nome;
        [Display(Name = "Documento")]
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

        [Required(ErrorMessage = "{0} é um campo obrigatório.")]
        [Display(Name = "Tipo")]
        public int IdTipo { get; set; }

        public int iCodUsuarioMovimentacao { get; set; }
        public DateTime dCadastro { get; set; }
    }
}
