using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using combineFiles.Controllers;
using combineFiles.Models;
using Spire.Doc;
namespace combineFiles.Functions
{
    public class Combine : Controller
    {
        private readonly ILogger<mainController> _logger;
        public Combine(ILogger<mainController> logger)
        {
            _logger = logger;
        }
        public Combine()
        {
        }

        public requestModel combineFiles(requestModel[] request)
        {
            requestModel file = new requestModel();

            file.name = request[0].name ?? "file";
            List<Document> docLists = new List<Document>();

            foreach (var el in request)
            {

                if (el.name.EndsWith(".docx"))
                {
                    MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(el.base64File));
                    Document tmp = new Document();
                    tmp.LoadFromStream(memoryStream, Spire.Doc.FileFormat.Docx);
                    //MemoryStream outstream = new MemoryStream();
                    //tmp.SaveToStream(outstream, Spire.Doc.FileFormat.PDF);
                    docLists.Add(tmp);
                }
                else if (el.name.EndsWith(".pdf"))
                {
                    MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(el.base64File));
                    Document tmp = new Document();
                    tmp.LoadFromStream(memoryStream, Spire.Doc.FileFormat.PDF);
                    MemoryStream outstream = new MemoryStream();
                    tmp.SaveToStream(outstream, Spire.Doc.FileFormat.Docx);
                    docLists.Add(tmp);
                }
            }

            Document result = docLists[0];

            foreach (var el in docLists)
            {
                result.Compare(el,"");
            }
            string path = Guid.NewGuid() + ".docx";
            result.SaveToFile(path);
            byte[] docBytes;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                docBytes = System.IO.File.ReadAllBytes(path);
                fs.Read(docBytes, 0, System.Convert.ToInt32(fs.Length));
                fs.Close();
                System.IO.File.Delete(path);
            }
            file.base64File = Convert.ToBase64String(docBytes.ToArray());
            return file;
        }

    }
}
