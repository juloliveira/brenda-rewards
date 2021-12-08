//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Brenda.Data
//{
//    public class ApplicationContextDesignFactory : DesignTimeDbContextFactoryBase<BrendaContext>
//    {
//        public ApplicationContextDesignFactory() :
//            //base("DefaultConnection", typeof(Startup).GetTypeInfo().Assembly.GetName().Name)
//            base("DefaultConnection", "Brenda.Data")
//        { }
//        protected override BrendaContext CreateNewInstance(DbContextOptions<BrendaContext> options)
//        {
//            return new BrendaContext(options);
//        }
//    }
//}
