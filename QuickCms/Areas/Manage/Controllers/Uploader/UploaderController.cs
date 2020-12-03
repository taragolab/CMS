using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QuickCms.Areas.Manage.Controllers.Uploader
{
    [AllowAnonymous]
    public class UploaderController : Controller
    {
        [Area("Manage")]
        [HttpPost]
        public async Task<IActionResult> Image()
        {
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];

                if (file != null && file.Length > 0)
                {
                    try
                    {
                        var extension = Path.GetExtension(file.FileName);
                        var fileName = Guid.NewGuid() + extension;
                        // var path = Path.Combine( Server.MapPath("~/Images/"), fileName);
                        // file.SaveAs(path);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Images", fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        return Json(new
                        {
                            status = 200,
                            name = fileName,
                            message = "تصویر با موفقیت بارگزاری گردید",
                            error = 0
                        });
                    }
                    catch (Exception e)
                    {
                        return Json(new
                        {
                            status = 500,
                            name = 0,
                            message = "هنگام بارگزاری عکس خطایی رخ داد",
                            error = e.Message
                        });
                    }

                }
            }
            return Json(new
            {
                status = 400,
                name = 0,
                message = "فایلی وجود ندارد",
                error = 0
            });
        }
    }
}