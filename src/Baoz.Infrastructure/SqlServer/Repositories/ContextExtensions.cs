using Baoz.Infrastructure.SqlServer.Contracts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baoz.Infrastructure.SqlServer.Repositories
{
    public static class ContextExtensions
    {
        public static string GetTableName<T>(this IDbContext context) where T : class
        {
            // We need dbcontext to access the models
            var models = context.ContextDatabase.Model;

            // Get all the entity types information
            var entityTypes = models.GetEntityTypes();

            // T is Name of class
            var entityTypeOfT = entityTypes.First(t => t.ClrType == typeof(T));

            var tableNameAnnotation = entityTypeOfT.GetAnnotation("Relational:TableName");

            return tableNameAnnotation.Value.ToString();
        }
    }
}
