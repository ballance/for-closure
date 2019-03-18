using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using ballance.it.for_closure.common.models;

namespace ballance.it.for_closure.common.repositories
 {
     public class RepositoryBase : IDisposable
     {
         private string _connectionString;
         private readonly IDbConnection _db;
        public RepositoryBase(string connectionString)
        {
            _db = new MySqlConnection(connectionString);
        }

        public bool PersistProperty(PropertyModel propertyModel)
        {
            try
            {
                if (propertyModel == null) 
                {
                    System.Console.WriteLine($"ERROR persisting propertymodel - model was null (perhaps mapping failed)");
                    return false;
                }
                var exists = _db.ExecuteScalar<bool>("SELECT COUNT(1) FROM PropertyModels WHERE PropertyId=@PropertyId", 
                    new {propertyModel.PropertyId});

                if (!exists)
                {
                    _db.Insert(propertyModel);
                    System.Console.WriteLine($">>>INSERTED the model # {propertyModel.PropertyId}");
                }
                else
                {
                    _db.Update(propertyModel);
                    System.Console.WriteLine($">>>UPDATED the model # {propertyModel.PropertyId}");
                }
                return true;
            }
            catch(Exception ex)
            {
                System.Console.WriteLine($"ERROR persisting propertymodel {ex}");
                return false;
            }
        }

        public PropertyModel PersistProperty(string Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PropertyModel> RetrieveProperties()
        {
            var properties = _db.Query<PropertyModel>("SELECT * FROM PropertyModels");
            return properties;
        }

        public PropertyModel GetPropertyById(string propertyId)
        {
            var property = _db.QuerySingle<PropertyModel>(
                $"SELECT * FROM PropertyModels WHERE PropertyId = {propertyId}");
            return property;
        }

        public void Dispose()
        {
            _db.Close();
        }
     }
 }