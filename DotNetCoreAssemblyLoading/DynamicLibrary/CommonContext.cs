using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbstractionsLibrary;
using Microsoft.EntityFrameworkCore;

namespace DynamicLibrary
{
    public class CommonContext: DbContext
    {
        public DbSet<User> Users;
    }
}
