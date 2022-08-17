using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeUploadPhotoFunctionality.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeUploadPhotoFunctionality.Controllers
{
	[ApiController]
	public class PhotoController : ControllerBase
	{
		private readonly PhotoContext _context;

		public PhotoController(PhotoContext context)
		{
			_context = context;
		}

		[HttpGet]
		[Route("[controller]")]
		public async Task<IActionResult> GetPhotos()
		{
			var photos = await  _context.Photos?.ToListAsync();

			if(photos.Count == 0) 
			{
				return Ok("no photos");
			}

			return Ok(photos);
		}

		//[HttpPost]
		//[Route("[controller]/{id:int}")]
		//public async Task<IActionResult> UploadPhoto([FromRoute] int id, IFormFile file)
		//{
		//	var fileName = new Random().Next(1, 5) + Path.GetExtension(file.FileName);
		//	string filePathImage = await Upload(file, fileName);

		//	if(await UpdateProfileImage(id, filePathImage))
		//	{
		//		return Ok(filePathImage);
		//	}

		//	return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while uploading the image");
		//}

		private async Task<string> Upload(IFormFile file,string fileName)
		{
			string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"Resources/Images", fileName);
			using Stream fileStream = new FileStream(filePath, FileMode.Create);
			await file.CopyToAsync(fileStream);
			return GetRelativePath(fileName);
		}

		private string GetRelativePath(string fileName)
		{
			return Path.Combine(@"Resources/Images", fileName);
		}

		private async Task<bool> UpdateProfileImage(int photoId, string imageUrl)
		{
			var photo = await _context.Photos.FindAsync(photoId);
			if(photo != null)
			{
				photo.PhotoUrl = imageUrl;
				await _context.SaveChangesAsync();

				return true;
			}
			return false;
		}
		
		// =========================================================================

		[HttpPost]
		[Route("[controller]/{id:int}")]
		public async Task<IActionResult> UploadMyPhoto([FromRoute] int id, IFormFile file)
		{
			string fileName = new Random().Next(1, 10) + Path.GetExtension(file.FileName);

			var filePath = await MyUpload(file, fileName);

			if(await UploadToMyDateBase(id, filePath))
			{
				return Ok(filePath);
			}

			return StatusCode(StatusCodes.Status500InternalServerError, "error");
		}

		private async Task<string> MyUpload(IFormFile file, string fileName)
		{
			string imagePath = Path.Combine(Directory.GetCurrentDirectory(), @"Resources/Images", fileName);
			using Stream fileStream = new FileStream(imagePath, FileMode.Create);
			await file.CopyToAsync(fileStream);
			return MyGetRelativePath(fileName);
		}

		private async Task<bool> UploadToMyDateBase(int id, string filePath)
		{
			var photo = await _context.Photos.FindAsync(id);
			if(photo  != null)
			{
				photo.PhotoUrl = filePath;
				await _context.SaveChangesAsync();
				return true;
			}
			return false;

		}

		private string MyGetRelativePath(string fileName)
		{
			return Path.Combine(@"Resources/Images", fileName);
		}
	}
}
