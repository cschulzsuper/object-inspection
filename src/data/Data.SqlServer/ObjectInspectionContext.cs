using System.Linq;
using Microsoft.EntityFrameworkCore;
using ChristianSchulz.ObjectInspection.Shared;

namespace ChristianSchulz.ObjectInspection.Data;

public class ObjectInspectionContext : DbContext
{
    public ObjectInspectionContext(DbContextOptions options, ObjectInspectionContextState state)
        : base(options)
    {
        State = state;
    }

    public ObjectInspectionContextState State { get; }
}