namespace Devagram.Dtos
{
    public class NotificacaoRequisicaoDto
    {
        public int Id { get; set; }
        public bool visualizado { get; set; }
        public string tipo { get; set; }
        public int? IdPublicacao { get; set; }
        public int? IdUsuario { get; set; }
    }
}
