using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace METU.Main.Controllers
{
    [ApiController]
    [EnableCors("cors")]
    [Route("upload")]
 
    public partial class METUUploadCoreController : ControllerBase
    {
       
        [EnableCors("cors")]
        [Route("{id}service")]
        [HttpPost]
        public List<string> UploadFiles([FromRoute] string id, List<IFormFile> files)
        {

            List<string> list = new List<string>();
            long size = files.Sum(f => f.Length);
            string path = AppDomain.CurrentDomain.BaseDirectory + "wwwroot\\";
            string localPath = path + id.ToString().Replace("-", "\\") + "\\";
            if (!Directory.Exists(Path.GetDirectoryName(localPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(localPath));
            }
            foreach (var formFile in files)
            {
                // string str = DateTime.Now.Ticks.ToString();
                string filenamestr = formFile.FileName.Split('.')[1] + DateTime.Now.ToString("yyyyMMddHHmmssffffff") + "." + formFile.FileName.Split('.')[1];

                var filePath = localPath + filenamestr;


                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        formFile.CopyToAsync(stream);
                        stream.Flush();
                        list.Add(id.ToString() + "/" + filenamestr);
                    }
                }
            }

            return list;
        }
        [EnableCors("cors")]
        [Route("{id}services")]
        [HttpPost]
        public List<string> UploadFiles([FromRoute] string id, IFormFileCollection files)
        {

            List<string> list = new List<string>();
            long size = files.Sum(f => f.Length);
            string path = AppDomain.CurrentDomain.BaseDirectory + "wwwroot\\";
            string localPath = path + id.ToString().Replace("-", "\\") + "\\";
            if (!Directory.Exists(Path.GetDirectoryName(localPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(localPath));
            }
            foreach (var formFile in files)
            {
                string filenamestr = formFile.FileName.Split('.')[1] + DateTime.Now.ToString("yyyyMMddHHmmssffffff") + "." + formFile.FileName.Split('.')[1];

                var filePath = localPath + filenamestr;
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        formFile.CopyToAsync(stream);
                        stream.Flush();
                        list.Add(id.ToString() + "/" + filenamestr);
                    }
                }
            }

            return list;
        }

    }
}
