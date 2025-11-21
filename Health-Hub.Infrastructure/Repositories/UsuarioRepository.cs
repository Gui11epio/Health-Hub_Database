using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using @enum;
using Health_Hub.Domain.Entities;
using Health_Hub.Domain.IRepositories;
using Health_Hub.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;

namespace Health_Hub.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {

        private readonly AppDbContext _context;
        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        } 
            
        public async Task<List<Usuario>> GetAllAsync()
        {
            return await _context.Usuario.ToListAsync();
        }

        public async Task<(List<Usuario> Itens, int Total)> GetAllByPageAsync(int pageNumber, int pageSize)
        {
            var query = _context.Usuario.AsQueryable();
            var total = await query.CountAsync();
            var itens = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return (itens, total);
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _context.Usuario.FirstOrDefaultAsync(u => u.EmailCorporativo == email);
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuario.FindAsync(id);
        }


        public async Task<Usuario> AddAsync(Usuario usuario)
        {
            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;

        }

        public async Task<bool> UpdateAsync(Usuario usuario)
        {
            _context.Usuario.Update(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
                return false;
            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        // ============================
        // CHAMAR SP_INSERIR_USUARIO
        // ============================
        public async Task InserirUsuarioViaProcedureAsync(string emailCorporativo, string nome, string senha, TipoUsuario tipo)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "BEGIN SP_INSERIR_USUARIO(:P_EMAIL, :P_NOME, :P_SENHA, :P_TIPO); END;",
                new OracleParameter("P_EMAIL", OracleDbType.Varchar2) { Value = emailCorporativo },
                new OracleParameter("P_NOME", OracleDbType.Varchar2) { Value = nome },
                new OracleParameter("P_SENHA", OracleDbType.Varchar2) { Value = senha },
                new OracleParameter("P_TIPO", OracleDbType.Int32) { Value = tipo }
            );
        }

        // ============================
        // CHAMAR SP_EXPORTAR_JSON
        // ============================
        public async Task<string> ExportarJsonAsync()
        {
            using var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "SP_EXPORTAR_JSON";
            cmd.CommandType = CommandType.StoredProcedure;

            var output = new OracleParameter("P_OUT_JSON", OracleDbType.Clob)
            {
                Direction = ParameterDirection.Output
            };

            cmd.Parameters.Add(output);

            await _context.Database.OpenConnectionAsync();
            await cmd.ExecuteNonQueryAsync();

            return output.Value?.ToString() ?? "[]";
        }

        
    }
}
