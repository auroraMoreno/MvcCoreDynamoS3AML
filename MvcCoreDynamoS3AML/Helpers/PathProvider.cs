using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreDynamoS3AML.Helpers
{
    public enum Folders
    {
        Images=0,
    }
    public class PathProvider
    {

        IWebHostEnvironment environment;
        public PathProvider(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public String MapPath(String filename, Folders folders)
        {
            String carpeta = "";
            if (folders == Folders.Images)
            {
                carpeta = "images";
            }
            String path = Path.Combine(this.environment.WebRootPath, carpeta, filename);
            return path;
        }

    }
}
