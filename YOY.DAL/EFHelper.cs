using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YOY.DAL
{
    /// <summary>
    /// 数据库CRUD操作的工具类
    /// </summary>
    public sealed class EFHelper
    {
        /// <summary>
        /// 添加操作
        /// </summary>
        /// <typeparam name="T">实体类型/表</typeparam>
        /// <param name="entity">实体</param>
        /// <returns>成功返回True，失败返回False</returns>
        public static bool Add<T>(T entity) where T : class
        {
            if (entity == null) return false;

            using (var db = new EFDbContext())
            {
                db.Set<T>().Add(entity);
                db.Entry(entity).State = EntityState.Added;

                try
                {
                    return db.SaveChanges() > 0;
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 添加列表实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="list">实体列表</param>
        /// <returns>返回添加记录数</returns>
        public static int AddList<T>(List<T> list) where T : class
        {
            if (list == null || list.Count == 0) return 0;

            using (var db = new EFDbContext())
            {
                foreach( T item in list)
                {
                    db.Set<T>().Add(item);
                    db.Entry(item).State = EntityState.Added;
                }

                try
                {
                    return db.SaveChanges() ;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <typeparam name="T">实体类型/表</typeparam>
        /// <param name="entity">实体</param>
        /// <returns>成功返回True，失败返回False</returns>
        public static bool Delete<T>(T entity) where T : class
        {
            if (entity == null) return false;

            using (var db = new EFDbContext())
            {
                db.Set<T>().Attach(entity);
                db.Set<T>().Remove(entity);

                try
                {
                    return db.SaveChanges() > 0;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 批量删除操作
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="list">实体列表</param>
        /// <returns>影响行数</returns>
        public static int DeleteList<T>(List<T> list) where T :class
        {
            if (list == null || list.Count == 0) return 0;

            using (var db = new EFDbContext())
            {
                db.Set<T>().RemoveRange(list);
                try
                {
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 修改操作
        /// </summary>
        /// <typeparam name="T">实体类型/表</typeparam>
        /// <param name="entity">实体</param>
        /// <returns>成功返回True，失败返回False</returns>
        public static bool Update<T>(T entity) where T : class
        {
            if (entity == null) return false;

            using (var db = new EFDbContext())
            {
                db.Set<T>().Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                try
                {
                    return db.SaveChanges() > 0;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 查询所有记录
        /// </summary>
        /// <typeparam name="T">实体类型/表</typeparam>
        /// <returns>指定表的所有实体列表</returns>
        public static List<T> GetAll<T>() where T : class
        {
            using (var db = new EFDbContext())
            {
                var query = db.Set<T>().ToList();
                return query;
            }
        }
    }
}
