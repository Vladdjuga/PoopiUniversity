using poopi.Entities.Models;
using poopi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace poopi.Entities.Initializers
{
    public class EntityInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            context.Groups.Add(new Group() { Name = "A1", Course = 4 });
            context.Groups.Add(new Group() { Name = "A2", Course = 2 });
            context.Groups.Add(new Group() { Name = "C4", Course = 3 });
            context.Groups.Add(new Group() { Name = "D5", Course = 2 });
        }
    }
}