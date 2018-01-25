using LabluzPro.Domain.Diversos;
using System;
using System.ComponentModel.DataAnnotations;

namespace LabluzPro.Domain.Entities
{
    public class Contrato
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "{0} é um campo obrigatório.")]
        [Display(Name = "Vencimento")]
        public DateTime dVencimento { get; set; }

        [Display(Name = "Imagem")]
        public string sImagem { get; set; }

        [Required(ErrorMessage = "{0} é um campo obrigatório.")]
        string Nome;
        [Display(Name = "Contrato")]
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

        [Display(Name = "Tipo de equipamento")]
        public int IdTipoEquipamento { get; set; }

        [Display(Name = "Tipo de serviço")]
        public int IdTipoServico { get; set; }

        public int iCodUsuarioMovimentacao { get; set; }
        public DateTime dCadastro { get; set; }

    }
}
