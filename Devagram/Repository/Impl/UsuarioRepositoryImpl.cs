﻿using Devagram.Models;

namespace Devagram.Repository.Impl
{
    public class UsuarioRepositoryImpl : IUsuarioRepository 
    {
        private readonly DevagramContext _context; //usa o contexto para conectar no banco

        public UsuarioRepositoryImpl (DevagramContext context)
        {
            _context = context;
        }

        public void AtualizarUsuario(Usuario usuario)
        {
            _context.Update(usuario);
            _context.SaveChanges();
        }

        public Usuario GetUsuarioPorId(int id)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Id == id);
        }

        public Usuario getUsuarioPorLoginSenha(string email, string senha)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Email == email && u.Senha == senha);
           

        }

        public void Salvar(Usuario usuario)
        {
            _context.Add(usuario);   
            _context.SaveChanges();
        }

        public bool VerificarEmail(string email)
        {
           return _context.Usuarios.Any(u => u.Email == email);
        }
    }
}
