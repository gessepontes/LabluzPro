namespace LabluzPro.Domain.Entities
{
    public class UsuarioPagina
    {
        public int ID { get; set; }

        public int idUsuario { get; set; }
        public int idPagina { get; set; }

        public virtual Usuario Usuario { get; set; }

    }
}
