namespace Devagram.Dtos
{
    public class ErrorRespostaDto
    {
        public int status { get; set; }
        public string descricao { get; set; }

        public List<string> Erros { get; set; }
    }
}
