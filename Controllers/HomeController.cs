using Microsoft.AspNetCore.StaticFiles;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace СмарТимСервисСайт.Controllers
{
    public class HomeController : Controller
    {
        private static string connectionString = "Server=194.36.88.201;Port=5432;Database=files;User Id=danilf;Password=890lpu";
        private static NpgsqlConnection connection = new NpgsqlConnection(connectionString);

        public ActionResult Upl()
        {
            return View("Upload");
        }
        public ActionResult Dwl()
        {
            return View("Download");
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            if (upload != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(upload.FileName);
                string path = "~/Files/" + fileName;
                // сохраняем файл в папку Files в проекте
                upload.SaveAs(Server.MapPath(path));
                Int64 id = AddFile(path);
                Console.WriteLine(id);
            }
            return RedirectToAction("Upl");
        }
        [HttpPost]
        public FileResult Download(string id)
        {
            string path = GetPathByFileId(Convert.ToInt64(id));
            if(path != null)
            {
                string fileName = path.Substring(path.LastIndexOf('/'));
                FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();
                string contentType;
                if (!provider.TryGetContentType(fileName, out contentType))
                {
                    contentType = "application/octet-stream";
                }
                path = Server.MapPath(path);
                return File(path, contentType, fileName);
            }
            return null;
        }
        private Int64 GetFileIdByPath(string path) 
        {
            connection.Open();
            NpgsqlCommand select = new NpgsqlCommand(
                "select f.id from danilf.files as f where f.path = '" + path + "'",
                connection);
            NpgsqlDataReader reader = select.ExecuteReader();
            if (reader.HasRows)
            {
                foreach (DbDataRecord record in reader)
                {
                    object id = record["id"];
                    connection.Close();
                    return Convert.ToInt64(id);
                }
            }
            connection.Close();
            return 0;
        }
        private Int64 AddFile(string path)
        {
            connection.Open();
            NpgsqlCommand sql = new NpgsqlCommand(
                "insert into danilf.files(path) values('" + path + "')",
                connection);
            sql.ExecuteNonQuery();
            connection.Close();
            return GetFileIdByPath(path);
        }
        private string GetPathByFileId(Int64 id)
        {
            connection.Open();
            NpgsqlCommand select = new NpgsqlCommand(
                "select f.path from danilf.files f where f.id = " + id,
                connection);
            NpgsqlDataReader reader = select.ExecuteReader();
            if (reader.HasRows)
            {
                foreach (DbDataRecord record in reader)
                {
                    object path = record["path"];
                    connection.Close();
                    return Convert.ToString(path);
                }
            }
            connection.Close();
            return null;
        }
    }
}