using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.DAL
{
    public class ReMinderInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ReMinderContext>
    {
        protected override void Seed(ReMinderContext context)
        {
            //FYI: DROP DB
            //sqllocaldb.exe stop v11.0
            //sqllocaldb.exe delete v11.0
            //sqllocaldb.exe start v11.0

            //TODO
            //Test Questions initialization



            context.SaveChanges(); 
        }
    }
}