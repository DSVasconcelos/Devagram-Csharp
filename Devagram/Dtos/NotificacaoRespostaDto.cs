namespace Devagram.Dtos
{
    public class NotificacaoRespostaDto
    {
        public int IdInteracao { get; set; }
        
        public string Tipo { get; set; }

        public string Nome { get; set; }

        public int idUsuario { get; set; }
        public int idPublicacao { get; set; }
    }
}
