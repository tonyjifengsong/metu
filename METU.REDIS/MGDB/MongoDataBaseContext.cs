﻿using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace METU.MGDB
{
    public class MongoDataBaseContext
    {
        public IMongoClient _client = null;
        public IMongoDatabase _database = null;
        public MongoDataBaseContext(MongodbHost host)
        {
            _client = new MongoClient(host.Connection);
            _database = _client.GetDatabase(host.DataBase);
        }
    }

}
