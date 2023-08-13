using cmdAPI.Models;

namespace cmdAPI.Data
{
    public interface ICommandRepo
    {
        Task SaveChanges();
        Task<Command?> GetCommandById(int id);
        Task<IEnumerable<Command>> GetallCommands();
        Task CreateCommand(Command cmd);
        
        void DeleteCommand(Command cmd);
    }
}