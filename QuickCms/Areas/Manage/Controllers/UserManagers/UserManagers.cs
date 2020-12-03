using Entities.Entities.UserAndSecurity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.UserManagers;

namespace QuickCms.Areas.Manage.Controllers.UserManagers
{

    [Area("Manage")]
    public class UserManagers : Controller
    {
        private readonly UserManager<Users> userManager;

        public UserManagers(UserManager<Users> userManager)
        {
            this.userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CheckUserName(string UserName)
        {
            if ((await userManager.FindByNameAsync(UserName) != null))
                return Json($"این نام کاربری قبلا در سیستم ثبت شده");

            return Json(true);

        }

        public IActionResult Index()
        {
            var model = userManager.Users.Select(T =>
                new ListUserViewModel
                {
                    Id = T.Id,
                    UserName = T.UserName,
                    FirstName = T.FirstName,
                    Photo = T.Photo
                    


                }
                );
            return View(model);
        }

        public IActionResult CreateUser()
        {

            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new 
                {
                    status = 100, //you can see the datails of status code in ~/Global/statusCodes
                    errors = ModelState.Values.Where(e => e.Errors.Count > 0).ToList(),
                    message = "لطفا در وارد کردن اطلاعات دقت کنید"
                });
            }
            var user = new Users
            {
                PhoneNumber = model.PhoneNumber,
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Photo = model.Photo,
                Availability = true,
                Gender = model.Gender


            };
            //todo sms verification
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
                return Json(new
                {
                    status = 200, //you can see the datails of status code in Global/statusCode 
                    error = 0,
                    message = "کاربر با موفقیت اضافه شد"

                });

            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return new JsonResult(new
                {
                    status = 600, //you can see the datails of status code in ~/Global/statusCodes
                    errors = ModelState.Values.Where(e => e.Errors.Count > 0).ToList(),
                    message = "هنگام افزودن کاربر مشکلی رخ داد لطفا بعدا تلاش کنید"
                });
            }
        }

        [HttpPost]
        public IActionResult Getstirng(string str)
        {
            if (!string.IsNullOrEmpty(str))
                return Json(new
                {
                    status = 200,
                    message = "اطلاعات با موفقیت دریافت گردید"
                });
            return Json(new
            {
                status = 400,
                message = "اطلاعاتی یافت نشد"
            });
        }

        [HttpPost]
        public IActionResult GetTime()
        {
            var Date = DateTime.Now;

            return Json(Date.ToString());
        }


    }
}
