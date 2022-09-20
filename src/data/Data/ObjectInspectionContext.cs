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

    protected void ApplyCamelCaseJsonPropertyNames(ModelBuilder modelBuilder)
    {
        var model = modelBuilder.Model;
        var entityTypes = model
            .GetEntityTypes()
            .ToArray();

        foreach (var entityType in entityTypes)
        {
            var properties = entityType
                .GetProperties()
                .ToArray();
            
            foreach (var property in properties)
            {
                var jsonPropertyName = property.GetJsonPropertyName();

                var needsCamelCase =
                    !string.IsNullOrEmpty(jsonPropertyName) &&
                    char.IsUpper(jsonPropertyName[0]);

                if (needsCamelCase)
                {
                    var camelCaseJsonPropertyName = CaseStyleConverter.FromPascalCaseToCamelCase(jsonPropertyName!);
                    property.SetJsonPropertyName(camelCaseJsonPropertyName);
                }
            }

            var isOwned = entityType.IsOwned();
            if (isOwned)
            {
                var entityTypeName = entityType.GetContainingPropertyName();

                var needsCamelCase =
                    !string.IsNullOrEmpty(entityTypeName) &&
                    char.IsUpper(entityTypeName[0]);

                if (needsCamelCase)
                {
                    var camelCaseJsonPropertyName = CaseStyleConverter.FromPascalCaseToCamelCase(entityTypeName!);
                    entityType.SetContainingPropertyName(camelCaseJsonPropertyName);
                }
            }
        }
    }
}