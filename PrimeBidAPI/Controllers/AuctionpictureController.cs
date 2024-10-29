using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrimeBidAPI.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PrimeBidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionpictureController : ControllerBase
    {
        [HttpPost]
        [Route("UploadFile")]
        public IActionResult UploadFile([FromForm] Auctionpicture auctionPic)
        {
            var response = new Response();
            try
            {
                if (auctionPic.File == null || auctionPic.File.Length == 0)
                {
                    return BadRequest(new { ErrorMessage = "No file uploaded." });
                }

                // Set the file name in the model
                auctionPic.FileName = auctionPic.File.FileName;

                // Define the directory to store the file
                var directoryPath = @"C:\Users\DELL\Documents\GitHub\PrimeBidV2\PrimeBidFrontend\view\Resources";
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Determine file path and save file
                string path = Path.Combine(directoryPath, auctionPic.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    auctionPic.File.CopyTo(stream);
                }

                // Return a successful response
                response.statuscode = 200;
                response.ErrorMessage = "Image uploaded successfully.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.statuscode = 500;
                response.ErrorMessage = "Unable to add file: " + ex.Message;
                return StatusCode(500, response);
            }
        }
    }

    // Ensure the Response class is defined somewhere in your project.
    public class Response
    {
        public int statuscode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
