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

    }
}
