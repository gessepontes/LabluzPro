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
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data do vencimento")]
        public DateTime dVencimento { get; set; }

        [Display(Name = "Imagem")]
        public string sImagem { get; set; }

        string Nome;

        [Required(ErrorMessage = "{0} é um campo obrigatório.")]
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

        [Required(ErrorMessage = "{0} é um campo obrigatório.")]
        [Display(Name = "Número")]
        public string sNumero { get; set; }

        public virtual Tipo Tipo { get; set; }

    }
}
