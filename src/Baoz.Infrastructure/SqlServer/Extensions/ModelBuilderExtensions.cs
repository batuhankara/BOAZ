using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace Baoz.Infrastructure.SqlServer.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyAllTypeConfigurations<TContext>(this ModelBuilder modelBuilder, string nameSpace)
             where TContext : DbContext
        {
            var applyConfigurationMethodInfo = modelBuilder
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .First(m => m.Name.Equals("ApplyConfiguration", StringComparison.OrdinalIgnoreCase));

            var ret = typeof(TContext).Assembly
                .GetTypes()
                .Where(t => t.Namespace == nameSpace)
                .Select(t =>
                        (t, i: t.GetInterfaces().FirstOrDefault(i =>
                         i.Name.Equals(typeof(IEntityTypeConfiguration<>).Name, StringComparison.Ordinal))))
                .Where(it => it.i != null)
                .Select(it => (et: it.i.GetGenericArguments()[0], cfgObj: Activator.CreateInstance(it.t)))
                .Select(it => applyConfigurationMethodInfo.MakeGenericMethod(it.et)
                .Invoke(modelBuilder, new[] { it.cfgObj }))
                .ToList();
        }

        public static ModelBuilder ModifyAllTypeConfigurations(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                }
            }
            return modelBuilder;
        }

      
    }

}
