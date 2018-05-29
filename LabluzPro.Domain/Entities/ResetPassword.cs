namespace LabluzPro.Domain.Entities
{
   public class ResetPassword
    {
        public ResetPassword()
        {
        }

        public string sSenha { get; set; }
        public string sConfirmaSenha { get; set; }
        public string sEmail { get; set; }
        public string sToken { get; set; }
    }
}
