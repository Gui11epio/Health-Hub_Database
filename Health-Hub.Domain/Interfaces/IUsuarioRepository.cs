using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using @enum;
using Health_Hub.Domain.Entities;

namespace Health_Hub.Domain.IRepositories
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario?> GetByEmailAsync(string email);
        Task<(List<Usuario> Itens, int Total)> GetAllByPageAsync(int pageNumber, int pageSize);
        Task<Usuario> AddAsync(Usuario usuario);
        Task<bool> UpdateAsync(Usuario usuario);
        Task<bool> DeleteAsync(int id);
        Task<string> ExportarJsonAsync();
        Task InserirUsuarioViaProcedureAsync(string emailCorporativo, string nome, string senha, TipoUsuario tipo);
    }
}
