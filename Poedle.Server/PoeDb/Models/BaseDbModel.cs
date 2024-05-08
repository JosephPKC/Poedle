using LiteDB;

namespace Poedle.PoeDb.Models
{
    public abstract class BaseDbModel
    {
        [BsonId]
        public int Id { get; set; } = 0;

        public bool _MarkForReview { get; set; }
    }
}
