using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;

namespace ProjectManager.Db.Migrations
{
    public static class MigrationsExtentions
    {
        public static void QueryExecute(this Migration connectionString, string query, Action<List<dynamic>, IDbConnection, IDbTransaction> eachResult)
        {

            connectionString.Execute.WithConnection((db, trans) =>
           {
               var listaResults = new List<dynamic>();


               using (var dbCommand = db.CreateCommand())
               {
                   dbCommand.CommandText = query;
                   dbCommand.Connection = db;
                   dbCommand.Transaction = trans;

                   var reader = dbCommand.ExecuteReader();

                   while (reader.Read())
                   {
                       var newExpando = new ExpandoObject() as IDictionary<string, Object>;

                       for (var i = 0; i < reader.FieldCount; i++)
                       {
                           newExpando.Add(reader.GetName(i), reader.GetValue(i));
                       }

                       listaResults.Add(newExpando);
                   }

                   reader.Close();
               }

               eachResult.Invoke(listaResults, db, trans);


           });
        }

        public static void ExecutSql(this IDbConnection db, IDbTransaction trans, string query)
        {
            using (var dbCommand = db.CreateCommand())
            {
                dbCommand.CommandText = query;
                dbCommand.Connection = db;
                dbCommand.Transaction = trans;

                dbCommand.ExecuteNonQuery();
            }
        }
    }
}