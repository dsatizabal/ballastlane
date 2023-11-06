using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BallastLane.Data.Models
{
    public class ClientModel : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string FiscalNumber { get; set; }
        public string Phone { get; set; }
        public string WebSite { get; set; }
        public string Status { get; set; }

    }
}
