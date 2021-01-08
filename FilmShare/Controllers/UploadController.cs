using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FilmShare.Classes.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace FilmShare.Controllers{
    public class UploadController : Controller{
        private readonly ILogger<HomeController> _logger;
        
        public UploadController(ILogger<HomeController> logger){
            _logger = logger;
        }
        
        [HttpPost] 
        [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<IActionResult> UploadPhysical()
                {
                    Console.WriteLine("ARTYOM");
                    if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
                    {
                        ModelState.AddModelError("File", 
                            $"The request couldn't be processed (Error 1).");
                        // Log error
                        Console.WriteLine("Error ARTYOM");
                        return BadRequest(ModelState);
                    }
        
                    // Into class properties
                    var _defaultFormOptions = new FormOptions();
                    _defaultFormOptions.MultipartBodyLengthLimit = Int64.MaxValue;
                    long _fileSizeLimit = Int64.MaxValue;
                    string[] _permittedExtensions = {"mp4"};
                    string _targetFilePath = "wwwroot/files/";
                    
                    var boundary = MultipartRequestHelper.GetBoundary(
                        MediaTypeHeaderValue.Parse(Request.ContentType),
                        _defaultFormOptions.MultipartBoundaryLengthLimit);
                    var reader = new MultipartReader(boundary, HttpContext.Request.Body);
                    var section = await reader.ReadNextSectionAsync();
        
                    while (section != null)
                    {
                        var hasContentDispositionHeader = 
                            ContentDispositionHeaderValue.TryParse(
                                section.ContentDisposition, out var contentDisposition);
        
                        if (hasContentDispositionHeader)
                        {
                            // This check assumes that there's a file
                            // present without form data. If form data
                            // is present, this method immediately fails
                            // and returns the model error.
                            // if (!MultipartRequestHelper
                            //     .HasFileContentDisposition(contentDisposition))
                            // {
                            //     ModelState.AddModelError("File", 
                            //         $"The request couldn't be processed (Error 2).");
                            //     // Log error
                            //     Console.WriteLine("Error 2 ARTYOM");
                            //     return BadRequest(ModelState);
                            // }
                            // else
                            // {
                                // Don't trust the file name sent by the client. To display
                                // the file name, HTML-encode the value.
                                var trustedFileNameForDisplay = WebUtility.HtmlEncode(
                                        contentDisposition.FileName.Value);
                                var trustedFileNameForFileStorage = "film.mp4";
        
                                // **WARNING!**
                                // In the following example, the file is saved without
                                // scanning the file's contents. In most production
                                // scenarios, an anti-virus/anti-malware scanner API
                                // is used on the file before making the file available
                                // for download or for use by other systems. 
                                // For more information, see the topic that accompanies 
                                // this sample.
        
                                var streamedFileContent = await FileHelpers.ProcessStreamedFile(
                                    section, contentDisposition, ModelState, 
                                    _permittedExtensions, _fileSizeLimit);
        
                                if (!ModelState.IsValid)
                                {
                                    return BadRequest(ModelState);
                                }
        
                                using (var targetStream = System.IO.File.Create(
                                    Path.Combine(_targetFilePath, trustedFileNameForFileStorage)))
                                {
                                    await targetStream.WriteAsync(streamedFileContent);
        
                                    _logger.LogInformation(
                                        "Uploaded file '{TrustedFileNameForDisplay}' saved to " +
                                        "'{TargetFilePath}' as {TrustedFileNameForFileStorage}", 
                                        trustedFileNameForDisplay, _targetFilePath, 
                                        trustedFileNameForFileStorage);
                                    return Redirect("/Home/Film");
                                }
                            // }
                        }
        
                        // Drain any remaining section body that hasn't been consumed and
                        // read the headers for the next section.
                        section = await reader.ReadNextSectionAsync();
                    }
        
                    return Redirect("/Home/Film");
                }
    }
}