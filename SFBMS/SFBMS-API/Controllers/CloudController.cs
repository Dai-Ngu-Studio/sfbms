using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SFBMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CloudController : ControllerBase
    {
        private const string UnreadableBody = "Request body is not readable.";
        private const string FaultyBody = "Request body is faulty.";
        private const string GoogleStorage = "https://storage.googleapis.com/";

        /// <summary>
        /// Uploads an image represented by a Base-64 String.
        /// Format: data:image/{imageType};base64,{base-64-string}
        /// </summary>
        /// <returns>A JSON response with the key "imageUrl" that contains the URL of the image.</returns>
        [HttpPost("image")]
        [Consumes("text/plain")]
        //[Authorize]
        public ActionResult UploadImage([FromBody] string body)
        {
            string? dataType = null;
            string? bodyData = null;
            string? fileExtension = null;
            try
            {
                string[] bodyParts = body.Split(',');
                if (bodyParts.Length == 2)
                {
                    bodyData = bodyParts[1];
                    string[] metadata = bodyParts[0].Split(';');
                    if (metadata.Length == 2)
                    {
                        string[] dataIdentifier = metadata[0].Split(':');
                        if (dataIdentifier.Length == 2)
                        {
                            dataType = dataIdentifier[1];
                            string[] type = dataType.Split('/');
                            if (type.Length == 2)
                            {
                                fileExtension = $".{type[1]}";
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return BadRequest(FaultyBody);
            }
            if (bodyData == null)
            {
                return BadRequest(FaultyBody);
            }
            try
            {
                string bucketName = "sfbms-48a15.appspot.com";

                byte[] bodyByte = Convert.FromBase64String(bodyData);

                StorageClient storageClient = StorageClient.Create();
                MemoryStream stream = new MemoryStream(bodyByte);
                Google.Apis.Storage.v1.Data.Object gObject = storageClient.UploadObject(bucketName, $"{Guid.NewGuid()}{fileExtension}", dataType, stream);

                Dictionary<string, string> jsonResponse = new Dictionary<string, string>();
                jsonResponse.Add("imageUrl", $"{GoogleStorage}{gObject.Bucket}/{gObject.Name}");
                return Ok(jsonResponse);
            }
            catch (Exception)
            {
                return BadRequest(FaultyBody);
            }
        }
    }
}
