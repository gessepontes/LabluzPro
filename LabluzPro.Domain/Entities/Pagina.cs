using LabluzPro.Domain.Diversos;
using System.ComponentModel.DataAnnotations;

namespace LabluzPro.Domain.Entities
{
    public class Pagina
    {

        public int ID { get; set; }

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

        public string sIcons { get; set; }
        public string sNomeView { get; set; }
        public string sNomeController { get; set; }
    }

}
