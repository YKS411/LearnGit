using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace VueMVCDemo.Models
{
    public class VueMVCDemoContext : DbContext
    {
        public VueMVCDemoContext (DbContextOptions<VueMVCDemoContext> options)
            : base(options)
        {
        }

        public DbSet<VueMVCDemo.Models.StudentModel> StudentModel { get; set; }
    }
}
