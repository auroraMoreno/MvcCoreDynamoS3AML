using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreDynamoS3AML.Helpers
{
    public class UploadHelper
    {
        PathProvider pathProvider;
        public UploadHelper(PathProvider pathProvider)
        {
            this.pathProvider = pathProvider;
        }

        public async Task<String> UploadFileAsync(IFormFile formFile, Folders folders)
        {
            String filename = formFile.FileName;
            String path = this.pathProvider.MapPath(filename, Folders.Images);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            };
            return path;
        }
    }
}
