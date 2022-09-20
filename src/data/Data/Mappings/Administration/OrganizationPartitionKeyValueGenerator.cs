using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using ChristianSchulz.ObjectInspection.Application.Administration;
using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Administration;

public class OrganizationPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<Organization>
{
    public override bool GeneratesTemporaryValues => false;

    public override string Next(EntityEntry entry)
        => Value(
            (entry.Context as ObjectInspectionContext)!.State,
            (entry.Entity as Organization)!);

    public string Value(ObjectInspectionContextState state, Organization entity)
        => "organization";

    public string Value(ObjectInspectionContextState state, Queue<object> partitionKeyComponents)
        => "organization";
}