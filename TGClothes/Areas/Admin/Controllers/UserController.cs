using Data.EF;
using Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;

namespace TGClothes.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IAccountService _user;

        public UserController(IAccountService user)
        {
            _user = user;
        }

        // GET: Admin/User
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var model = _user.GetAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Account user)
        {
            if (ModelState.IsValid)
            {
                var encryptedMd5Password = Encryptor.MD5Hash(user.Password);
                user.Password = encryptedMd5Password;

                long id = _user.Insert(user);
                if (id > 0)
                {
                    SetAlert("Thêm mới người dùng thành công", "success");
                    return RedirectToAction("Index", "User");
                } 
                else
                {
                    ModelState.AddModelError("", "Thêm mới người dùng không thành công.");
                }
            }
            return View("Index");
        }

        public ActionResult Edit(long id)
        {
            var user = _user.GetUserById(id);
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(Account user)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(user.Password))
                {
                    var encryptedMd5Password = Encryptor.MD5Hash(user.Password);
                    user.Password = encryptedMd5Password;
                }

                var result = _user.Update(user);
                if (result)
                {
                    SetAlert("Cập nhật người dùng thành công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật người dùng không thành công");
                }
            }
            return View("Index");
        }

        [HttpDelete]
        public ActionResult Delete(long id)
        {
            _user.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = _user.ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}