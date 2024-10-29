using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrimeBidAPI.Models;
using PrimeBidAPI.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PrimeBidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionpictureController : ControllerBase
    {
        [HttpPost]
        [Route("UploadFile")]
        public IActionResult UploadFile([FromForm] Auctionpicture auctionPic) // Assuming you have this model defined
        {
            Response response = new Response(); // Ensure Response class is defined
            try
            {
                if (auctionPic.file == null || auctionPic.file.Length == 0)
                {
                    return BadRequest(new { ErrorMessage = "No file uploaded." });
                }

                var directoryPath = @"C:\Users\DELL\Documents\GitHub\PrimeBidV2\PrimeBidFrontend\view\Resources";
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string fileExtension = Path.GetExtension(auctionPic.file.FileName);
                string fileName = auctionPic.FileName; // Ensure FileName is set appropriately
                string path = Path.Combine(directoryPath, fileName);

                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    auctionPic.file.CopyTo(stream); // Copy the uploaded file to the stream
                }

                response.statuscode = 200;
                response.ErrorMessage = "Image Created Successfully";
                return Ok(response); // Return a successful response
            }
            catch (Exception ex)
            {
                response.statuscode = 500;
                response.ErrorMessage = "Unable to add file: " + ex.Message;
                return StatusCode(500, response); // Return a 500 error response
            }
        }
    }
}
