using Intelemark.Entities;
using Intelemark.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Intelemark.Services
{
    public class Service<T> where T : Entity
    {

        public static async Task<bool> Delete(T Entity)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            try
            {
                var entity = await db.Set<T>().FindAsync(Entity.Id);
                if (entity == null)
                    return false;

                entity.IsDeleted = true;
                entity.LastUpdate = DateTime.Now;
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return false;
            }
        }


        public static async Task<bool> Save(T Entity)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            try
            {
                var entity = await db.Set<T>().FindAsync(Entity.Id);
                if (entity == null)
                    return false;

                entity.IsDeleted = true;
                entity.LastUpdate = DateTime.Now;
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return false;
            }
        }




    }
}