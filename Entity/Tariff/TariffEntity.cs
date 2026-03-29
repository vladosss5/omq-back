using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace AskAgainApi.Entity.Tariff
{
    public class TariffEntity
    {
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<string> Features { get; set; } = new List<string>();
        public int PeriodOfDays { get; set; }
    }
}
