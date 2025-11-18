using EntityFrameworkCore.UseRowNumberForPaging;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Loader;

using Vodace.Core.Configuration;
using Vodace.Core.Const;
using Vodace.Core.DBManager;
using Vodace.Core.Enums;
using Vodace.Core.Extensions;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Core.ManageUser;
using Vodace.Entity.DomainModels;
using Vodace.Entity.SystemModels;

using static Dapper.SqlMapper;

namespace Vodace.Core.EFDbContext
{
    public class VOLContext : DbContext, IDependency
    {
        /// <summary>
        /// 数据库连接名称 
        /// </summary>
        public string DataBaseName = null;
        public VOLContext()
                : base()
        {
        }
        public VOLContext(string connction)
            : base()
        {
            DataBaseName = connction;
        }

        public VOLContext(DbContextOptions<VOLContext> options)
            : base(options)
        {

        }
        public override void Dispose()
        {
            base.Dispose();
        }
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (Exception ex)//DbUpdateException 
            {
                throw (ex.InnerException as Exception ?? ex);
            }
        }
        public override DbSet<TEntity> Set<TEntity>()
        {
            return base.Set<TEntity>();
        }
        //public DbSet<TEntity> Set<TEntity>(bool trackAll = false) where TEntity : class
        //{
        //    return base.Set<TEntity>();
        //}
        /// <summary>
        /// 设置跟踪状态
        /// </summary>
        public bool QueryTracking
        {
            set
            {
                this.ChangeTracker.QueryTrackingBehavior =
                       value ? QueryTrackingBehavior.TrackAll
                       : QueryTrackingBehavior.NoTracking;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }
            string connectionString = DBServerProvider.GetConnectionString(null);
            if (Const.DBType.Name == Enums.DbCurrentType.MySql.ToString())
            {
                optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 11)));
            }
            else if (Const.DBType.Name == Enums.DbCurrentType.PgSql.ToString())
            {
                optionsBuilder.UseNpgsql(connectionString);
            }
            else if (Const.DBType.Name == Enums.DbCurrentType.DM.ToString())
            {
                optionsBuilder.UseDm(connectionString);
            }
            else if (Const.DBType.Name == Enums.DbCurrentType.Oracle.ToString())
            {
                optionsBuilder.UseOracle(connectionString,x=>x.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion19));
               // optionsBuilder.UseOracle(connectionString, b => b.UseOracleSQLCompatibility("11"));
            }
            else
            {
                if (AppSetting.GetSettingString("UseSqlserver2008") =="1")
                {
                   optionsBuilder.UseSqlServer(connectionString, x => x.UseRowNumberForPaging());
                }
                optionsBuilder.UseSqlServer(connectionString, o => o.UseCompatibilityLevel(120));
            }
            //默认禁用实体跟踪
            optionsBuilder = optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
           // optionsBuilder.AddInterceptors(new SqlCommandInterceptor());
            base.OnConfiguring(optionsBuilder);
        }

        private void ApplySoftDeleteFilter(ModelBuilder modelBuilder, Type entityType)
        {
            var method = typeof(ModelBuilder).GetMethods()
                .First(m => m.Name == nameof(ModelBuilder.Entity) && m.IsGenericMethod)
                .MakeGenericMethod(entityType);

            var entityTypeBuilder = method.Invoke(modelBuilder, null);

            var parameter = Expression.Parameter(entityType, "e");
            var body = Expression.Equal(
                Expression.Property(parameter, "delete_status"),
                Expression.Constant(0,typeof(int?))
            );
            var lambda = Expression.Lambda(body, parameter);

            // 明确获取 HasQueryFilter 方法，指定参数类型
            var hasQueryFilterMethod = entityTypeBuilder.GetType()
                .GetMethods()
                .First(m => m.Name == nameof(EntityTypeBuilder.HasQueryFilter)
                         && m.GetParameters().Length == 1
                         && m.GetParameters()[0].ParameterType == typeof(LambdaExpression));

            hasQueryFilterMethod.Invoke(entityTypeBuilder, new object[] { lambda });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Type type = null;
            try
            {
                //获取所有类库
                var compilationLibrary = DependencyContext
                    .Default
                    .RuntimeLibraries
                    .Where(x => !x.Serviceable && x.Type != "package" && x.Type == "project");
                foreach (var _compilation in compilationLibrary)
                {
                    //加载指定类
                    AssemblyLoadContext.Default
                    .LoadFromAssemblyName(new AssemblyName(_compilation.Name))
                    .GetTypes()
                    .Where(x =>
                        x.GetTypeInfo().BaseType != null
                        && x.BaseType == (typeof(BaseEntity)))
                        .ToList().ForEach(t =>
                        {
                            modelBuilder.Entity(t);

                            var isDeleted = t.GetProperties().FirstOrDefault(n => n.Name == "delete_status") is not null;
                            if (isDeleted)
                            {
                                ApplySoftDeleteFilter(modelBuilder,t);
                            }
                            //  modelBuilder.Model.AddEntityType(t);
                        });
                }

                //Oracle数据库指定表名与列名全部大写
                if (DBType.Name == DbCurrentType.Oracle.ToString())
                {
                    foreach (var entity in modelBuilder.Model.GetEntityTypes())
                    {
                        string tableName = entity.GetTableName().ToUpper();
                       // if (tableName.StartsWith("SYS_") || tableName.StartsWith("DEMO_"))
                        {
                            entity.SetTableName(entity.GetTableName().ToUpper());
                            foreach (var property in entity.GetProperties())
                            {
                                property.SetColumnName(property.Name.ToUpper());
                                if (property.ClrType == typeof(Guid))
                                {
                                    property.SetValueConverter(new ValueConverter<Guid, string>(v => v.ToString(), v => new Guid(v)));
                                }
                                else if (property.ClrType == typeof(Guid?))
                                {
                                    property.SetValueConverter(new ValueConverter<Guid?, string>(v => v.ToString(), v => new Guid(v)));
                                }
                            }
                        }
                    }
                }
                base.OnModelCreating(modelBuilder);
            }
            catch (Exception ex)
            {
                string mapPath = ($"Log/").MapPath();
                Utilities.FileHelper.WriteFile(mapPath,
                    $"syslog_{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt",
                    type?.Name + "--------" + ex.Message + ex.StackTrace + ex.Source);
            }

        }
    }
}
