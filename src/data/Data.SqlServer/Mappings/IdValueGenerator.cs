using IdGen;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace ChristianSchulz.ObjectInspection.Data.SqlServer.Mappings
{
    public class IdValueGenerator : ValueGenerator<long>
    {
        private readonly IdGenerator _idGenerator;

        public override bool GeneratesTemporaryValues => false;

        public IdValueGenerator() 
        {
            _idGenerator= new IdGenerator(0);
        }

        public override long Next(EntityEntry _)
        {
            return _idGenerator.CreateId();
        }
    }
}
