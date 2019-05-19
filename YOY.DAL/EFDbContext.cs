using System;
using System.Collections.Generic;
using System.Data.Entity;
using YOY.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YOY.DAL
{
    /// <summary>
    /// 通过DbContext与数据库连接
    /// </summary>
    public class EFDbContext : DbContext
    {
        public EFDbContext(): base("name=Cloud")
        {
            //不检查一致性
            Database.SetInitializer<EFDbContext>(null);
        }

        public DbSet<User> Users { get; set; } 
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User2Order> User2Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<Notice> Notices { get; set; }
    }
}
