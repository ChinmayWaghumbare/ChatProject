using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace WebServerEntityFramework.Controllers
{
    public class FileController : ApiController
    {

        public int Upload()
        {
            string sPath = "";
            sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/UserFiles/");

            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            HttpPostedFile hpf = hfc[0];
            hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));
            return 1;
        }
         

        [HttpGet]
        public HttpResponseMessage download()
        {
            string sPath = "";
            sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/UserFiles/BOM.png");

            HttpResponseMessage res = new HttpResponseMessage(HttpStatusCode.OK);
            var fileStream = new FileStream(sPath, FileMode.Open);
            res.Content = new StreamContent(fileStream);
            res.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            res.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            res.Content.Headers.ContentDisposition.FileName = "DemoDownloadedFile.png";
            return res;
        }
    }
}
