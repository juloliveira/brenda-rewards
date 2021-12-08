using System;

namespace Brenda.Utils
{
    public class BrendaSettings
    {
        public string Secret { get; set; }
        public DatabaseSettings ConnectionStrings { get; set; }
    }

    public class DatabaseSettings
    {
        public string DefaultConnection { get; set; }
    }
}
