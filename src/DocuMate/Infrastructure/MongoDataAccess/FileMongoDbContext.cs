﻿using DocuMate.Data.Models;
using MongoDB.Driver;

namespace DocuMate.Infrastructure.MongoDataAccess;

public class FileMongoDbContext (IMongoClient mongoClient)
{
    private readonly IMongoDatabase _database = mongoClient.GetDatabase("docu_mate");

    public IMongoCollection<FileData> Files => _database.GetCollection<FileData>("files");
}