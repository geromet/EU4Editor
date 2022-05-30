using DataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class Context : DbContext
    {
        #region DbSets
        public DbSet<Idea> Ideas { get; set; }
        public DbSet<IdeaGroup> IdeaGroups { get; set; }
        public DbSet<Religion> Religions { get; set; }
        public DbSet<ReligionGroup> ReligionGroups { get; set; }
        public DbSet<Modifier> Modifiers { get; set; }
        #endregion
        public string DbPath { get; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
    }
}
