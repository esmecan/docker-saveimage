﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using saveimage1.Models;

namespace saveimage1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IFileProvider _fileProvider;

        public HomeController(ILogger<HomeController> logger,IFileProvider fileProvider)
        {
            _logger = logger;
            _fileProvider = fileProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Imageshow()
        {
            var images = _fileProvider.GetDirectoryContents("wwwroot/images").ToList().Select(x => x.Name);
            return View(images);
        }
        [HttpPost]
        public IActionResult Imageshow(string name)
        {
            var file=_fileProvider.GetDirectoryContents("wwwroot/images").ToList().First(x => x.Name==name);
            System.IO.File.Delete(file.PhysicalPath);
            return RedirectToAction("Imageshow");

        }

        public IActionResult ImageSave()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>ImageSave(IFormFile imageFile)
        {
            if (imageFile!=null && imageFile.Length>0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension
                    (imageFile.FileName);


                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using(var stream=new FileStream(path,FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }


            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
