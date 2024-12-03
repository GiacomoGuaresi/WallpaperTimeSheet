using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WallpaperTimeSheet.Models;

namespace WallpaperTimeSheet.Data
{
    public static class ConfigData
    {
        public static string GetData(string key)
        {
            using (var db = new AppDbContext())
            {
                var config = db.Configs.FirstOrDefault(c => c.key == key);
                
                if(config == null)
                    return null;
                 
                return config.value;
            }
        }

        public static void UpsertData(string key, object value)
        {
            using (var db = new AppDbContext())
            {
                var config = db.Configs.FirstOrDefault(c => c.key == key);
                if (config == null)
                {
                    db.Configs.Add(new Config { key = key, value = value.ToString() });
                }
                else
                {
                    config.value = value.ToString();
                }
                db.SaveChanges();
            }
        }

        public static void DeleteData(string key)
        {
            using (var db = new AppDbContext())
            {
                var config = db.Configs.FirstOrDefault(c => c.key == key);
                if (config != null)
                {
                    db.Configs.Remove(config);
                    db.SaveChanges();
                }
            }
        }
    }
}
