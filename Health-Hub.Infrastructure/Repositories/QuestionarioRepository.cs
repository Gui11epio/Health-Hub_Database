using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Health_Hub.Domain.Entities;
using Health_Hub.Domain.Interfaces;
using Health_Hub.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;

namespace Health_Hub.Infrastructure.Repositories
{
    public class QuestionarioRepository : IQuestionarioRepository
    {

        private readonly AppDbContext _context;
        public QuestionarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Questionario>> GetAllAsync()
        {
            return await _context.Questionario.ToListAsync();
        }

        public async Task<(List<Questionario> Itens, int Total)> GetAllByPageAsync(int pageNumber, int pageSize)
        {
            var query = _context.Questionario.AsQueryable();
            var total = await query.CountAsync();
            var itens = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return (itens, total);
        }

        public async Task<Questionario?> GetByIdAsync(int id)
        {
            return await _context.Questionario.FindAsync(id);
        }

        public async Task<Questionario> AddAsync(Questionario questionario)
        {
            _context.Questionario.Add(questionario);
            await _context.SaveChangesAsync();
            return questionario;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var questionario = await _context.Questionario.FindAsync(id);
            if (questionario == null)
                return false;
            _context.Questionario.Remove(questionario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task InserirQuestionarioViaProcedureAsync(int usuarioId, int estresse, int sono, int ansiedade, int sobrecarga, string avaliacao)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "BEGIN SP_INSERIR_QUESTIONARIO(:P_USUARIO_ID, :P_ESTRESSE, :P_SONO, :P_ANSIEDADE, :P_SOBRECARGA, :P_AVALIACAO); END;",
                new OracleParameter("P_USUARIO_ID", OracleDbType.Int32) { Value = usuarioId },
                new OracleParameter("P_ESTRESSE", OracleDbType.Int32) { Value = estresse },
                new OracleParameter("P_SONO", OracleDbType.Int32) { Value = sono },
                new OracleParameter("P_ANSIEDADE", OracleDbType.Int32) { Value = ansiedade },
                new OracleParameter("P_SOBRECARGA", OracleDbType.Int32) { Value = sobrecarga },
                new OracleParameter("P_AVALIACAO", OracleDbType.Varchar2) { Value = avaliacao }
            );
        }
    }

}
