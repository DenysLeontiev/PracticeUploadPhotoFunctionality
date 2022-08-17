using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeUploadPhotoFunctionality.DataModels
{
	public class PhotoContext : DbContext
	{
		public PhotoContext(DbContextOptions<PhotoContext> options): base(options)
		{
			Database.EnsureCreated();
		}

		public DbSet<Photos> Photos { get; set; }
	}
}
