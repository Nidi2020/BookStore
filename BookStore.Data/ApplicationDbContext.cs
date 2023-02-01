using BookStore.Common.Utilities;
using BookStore.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace BookStore.Data;

public class ApplicationDbContext : IdentityDbContext<User, Role, int> //DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    //baraye sakhtan tables be soorat automatic ba komak reflaction ha... fluent configuration 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //firstly
        base.OnModelCreating(modelBuilder);

        var entitiesAssembly = typeof(IEntity).Assembly; //go into assemblies(ddl files) Entities Layer

        modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
        modelBuilder.RegisterEntityTypeConfiguration(entitiesAssembly);
        modelBuilder.AddRestrictDeleteBehaviorConvention();
        modelBuilder.AddPluralizingTableNameConvention();

        SoftDeleteGolobalFilter(modelBuilder);
    }

    private void SoftDeleteGolobalFilter(ModelBuilder modelBuilder)
    {
        Expression<Func<BaseEntity, bool>> filterExpr = bm => !bm.IsDeleted;
        foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes())
        {
            if (mutableEntityType.ClrType.IsAssignableTo(typeof(BaseEntity)))
            {
                var parameter = Expression.Parameter(mutableEntityType.ClrType);
                var body = ReplacingExpressionVisitor.Replace(filterExpr.Parameters.First(), parameter, filterExpr.Body);
                var lambdaExpression = Expression.Lambda(body, parameter);

                // set filter
                mutableEntityType.SetQueryFilter(lambdaExpression);
            }
        }
    }
}



