using Database.Mongo;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Database
{
    public class DbContext
    {
        public DbContext()
        {
            var pack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("camelCase", pack, t => true);

            mongoClient = new MongoClient(
                "mongodb+srv://admin:CgIoA422gafbmXJQ@petrenko-wuqft.mongodb.net/messenger?retryWrites=true&w=majority");
            Db = mongoClient.GetDatabase("messenger");
        }

        private MongoClient mongoClient;
        public IMongoDatabase Db { get; }
    }
}
