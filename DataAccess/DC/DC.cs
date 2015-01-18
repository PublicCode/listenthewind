using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Reflection;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace DataAccess.DC
{
    public class DC : DbContext
    {
        private readonly object _saveLock = new object();
        public DC() : base()
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }
        public DC(string connectionString)
            : base(connectionString)
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }
        public DbSet<camp> camps { get; set; }
        public DbSet<campreservedate> campreservedates { get; set; }
        public DbSet<campcomment> campcomments { get; set; }
        public DbSet<camphost> camphosts { get; set; }
        public DbSet<camphostlanguage> camphostlanguages { get; set; }
        public DbSet<campcollect> campcollects { get; set; }
        public DbSet<campitem> campitems { get; set; }
        public DbSet<camppile> camppiles { get; set; }
        public DbSet<campphoto> campphotos { get; set; }
        public DbSet<campprice> campprices { get; set; }
        public DbSet<campreserve> campreserves { get; set; }
        public DbSet<campreserveatt> campreserveatts { get; set; }
        public DbSet<camptype> camptypes { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<UserOperationLog> UserOperationLogs { get; set; }
        public DbSet<UserOperationDetail> OperationDetail { get; set; }
        public DbSet<TableNumberEnt> TableNumberEnt { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<MailFail> MailFails { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<City> Citys { get; set; }
        public DbSet<CityLocation> CityLocations { get; set; }

        public DbSet<basicdatacollect> basicdatacollects { get; set; }
        public DbSet<UserIntegralHistory> UserIntegralHistorys { get; set; }
        

        public override int SaveChanges()
        {
            UpdateDates();
            return base.SaveChanges();
        }
        private void UpdateDates()
        {
            foreach (var change in ChangeTracker.Entries<ILoggedEntity>())
            {
                if (change.State != EntityState.Deleted)
                {
                    var values = change.CurrentValues;
                    foreach (var name in values.PropertyNames)
                    {
                        var value = values[name];
                        if (value is DateTime)
                        {
                            var date = (DateTime)value;
                            if (date < SqlDateTime.MinValue.Value)
                            {
                                values[name] = SqlDateTime.MinValue.Value;
                            }
                            else if (date > SqlDateTime.MaxValue.Value)
                            {
                                values[name] = SqlDateTime.MaxValue.Value;
                            }
                        }
                    }
                }
            }
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Entity<Quote>().HasOptional(c => c.MyPartner);
            //.WithMany(c=>c.Quotes).HasForeignKey(c=>c.PartnerId);
            modelBuilder.Entity<campreserve>()
                .HasRequired(c => c.ReservePile)              
                .WithMany(t=> t.listOfReserve)
                .HasForeignKey(c => c.CampPileID);

        }

        public DataTable ToDataTable<T>(IEnumerable<T> varlist)
        {
            try
            {
                DataTable dtReturn = new DataTable();

                //Add columns
                PropertyInfo[] oProps = typeof(T).GetProperties();
                foreach (PropertyInfo pi in oProps)
                {
                    Type colType = pi.PropertyType;
                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }
                    dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                }

                //Add rows
                foreach (T rec in varlist)
                {
                    DataRow dr = dtReturn.NewRow();
                    foreach (PropertyInfo pi in oProps)
                    {
                        dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
                    }
                    dtReturn.Rows.Add(dr);
                }
                return dtReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Dispose()
        {
            base.Dispose();
        }
    }

    public static class DCLoader
    {
        public static DC GetNewDC()
        {
            return new DC();
        }

        public static DC GetMyDC()
        {
            var dc = HttpContext.Current == null
                        ? new DC()
                        : (HttpContext.Current.Items["DC"] == null ? new DC() : HttpContext.Current.Items["DC"] as DC);
            return dc;
        }
    }
}
