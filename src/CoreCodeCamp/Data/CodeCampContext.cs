using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CoreCodeCamp.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using CoreCodeCamp.Data.Entities;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Azure.Services.AppAuthentication;

namespace CoreCodeCamp.Data
{
    public class CodeCampContext : IdentityDbContext<CodeCampUser>
    {
        private IConfiguration _config;


        public CodeCampContext(IConfiguration config, DbContextOptions<CodeCampContext> options)
            : base(options)
        {
            _config = config;

            var conn = (SqlConnection)this.Database.GetDbConnection();
            conn.AccessToken = (new AzureServiceTokenProvider()).GetAccessTokenAsync("https://database.windows.net/").Result;
        }

        public DbSet<Talk> Talks { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<EventInfo> CodeCampEvents { get; set; }
        public DbSet<FavoriteTalk> FavoriteTalks { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);

        //    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        //    builder["Data Source"] = "tcp:bostongab-svr.database.windows.net,1433"; // replace with your server name
        //    builder["Initial Catalog"] = "bostongab"; // replace with your database name
        //    //builder["Authentication"] = "Active Directory Integrated";
        //    builder["Connect Timeout"] = 30;
            
        //    //string connectionString = @"Data Source=bostongab-svr.database.windows.net;Initial Catalog=bostongab;Authentication=Active Directory Integrated;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //    var conn = new SqlConnection(builder.ConnectionString);
            
        //    //var azureServiceTokenProvider = new AzureServiceTokenProvider();
        //    //var accessToken = azureServiceTokenProvider.GetAccessTokenAsync("https://database.windows.net/").Result;

        //    //conn.AccessToken = accessToken;

        //    //optionsBuilder.UseSqlServer(conn);
                        
        //    //optionsBuilder.UseSqlServer(_config["Data-DbCodeCamp"]);
        //}
    }
}
