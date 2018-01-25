using LabluzPro.Domain.Diversos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabluzPro.Domain.Entities
{
    public class Tipo
    {
        public int ID { get; set; }

        string Nome;
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

        public int IdSolicitacao { get; set; }

        public int iCodUsuarioMovimentacao { get; set; }
        public DateTime dCadastro { get; set; }

        [Display(Name = "Certificado")]
        public virtual ICollection<Certificado> Certificado { get; set; }

    }
}
