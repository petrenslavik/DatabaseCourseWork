using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Interfaces;
using Database.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Database.Mongo
{
    public class Repository<T>:IRepository<T> where T:BaseModel
    {
        protected readonly IMongoCollection<T> collection;

        public Repository(DbContext context,string name)
        {
            collection = context.Db.GetCollection<T>(name);
        }

        public Task<List<T>> GetAll()
        {
            return collection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<T> Create(T entity)
        {
            await collection.InsertOneAsync(entity);
            return entity;
        }

        public Task Delete(string id)
        {
            return collection.DeleteOneAsync(m => m.Id == id);
        }

        public async Task<T> Update(T entity)
        {
            await collection.ReplaceOneAsync(p => p.Id == entity.Id, entity);
            return entity;
        }

        public Task<T> GetById(string id)
        {
            return collection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }
    }
}