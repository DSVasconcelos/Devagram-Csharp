using Devagram.Models;

namespace Devagram.Repository
{
    public interface IUsuarioRepository
    {
        Usuario getUsuarioPorLoginSenha(string email, string senha);

        Usuario GetUsuarioPorId(int id);
        
        public void Salvar(Usuario usuario);

        public void AtualizarUsuario(Usuario usuario);

        public bool VerificarEmail(string email);
       
    }
}
