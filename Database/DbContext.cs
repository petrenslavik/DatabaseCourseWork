using Database.Mongo;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Database
{
    public class DbContext
    {
        public DbContext(string connectionString)
        {
            var pack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("camelCase", pack, t => true);

            mongoClient = new MongoClient(connectionString);
            Db = mongoClient.GetDatabase("messenger");
        }

        private MongoClient mongoClient;
        public IMongoDatabase Db { get; }
    }
}
