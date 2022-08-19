using Microsoft.EntityFrameworkCore;

namespace Super.Paula.Data
{
    public class PaulaContext : DbContext
    {
        public PaulaContext(DbContextOptions options, PaulaContextState state)
            : base(options)
        {
            State = state;
        }

        public PaulaContextState State { get; }
    }
}