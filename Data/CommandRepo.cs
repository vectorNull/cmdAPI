using cmdAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace cmdAPI.Data
{
    public class SQLCommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context;

        public SQLCommandRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateCommand(Command cmd)
        {
            if (cmd is null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }

            await _context.AddAsync(cmd);
        }

        public void DeleteCommand(Command cmd)
        {
            if (cmd is null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }

            _context.Commands.Remove(cmd);
        }

        public async Task<IEnumerable<Command>> GetallCommands()
        {
            return await _context.Commands.ToListAsync();
        }

        public async Task<Command?> GetCommandById(int id)
        {
           return await _context.Commands.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}