using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace PetsAlone.Core
{
    public class PetsService
    {
        public List<My_Pet_Class> GetAll(PetsDbContext petsDbContext)
        {
            var connection = new SqliteConnection(petsDbContext.Database.GetConnectionString());
            connection.Open();
            var x = new SqliteCommand("SELECT COUNT(*) FROM Pets", connection).ExecuteScalar();

            var y = new List<My_Pet_Class>();
            for (var i = 0; i < (long)x; i++)
            {
                var z = petsDbContext.Pets.FindAsync(i + 1).Result;
                y.Add(z);
            }

            // seems to be the best way to order by newest first
            var yy = new List<My_Pet_Class>();
            foreach (var xy in y)
            {
                if (yy.Count == 0)
                {
                    yy.Add(xy);
                }
                else if (yy[0].MissingSince < xy.MissingSince)
                {
                    var temp = yy;
                    yy = new List<My_Pet_Class> { xy };
                    foreach (var tempitem in temp)
                    {
                        yy.Add(tempitem);
                    }
                }
                else
                {
                    yy.Add(xy);
                }
            }

            return yy;
        }
    }
}