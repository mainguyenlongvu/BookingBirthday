using Data.EF;
using Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TGClothes.Controllers
{
    public class ContactController : Controller
    {
        private readonly IFeedbackService _feedbackService;

        public ContactController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Send(string name, string email, string phone, string address, string content)
        {
            var feedback = new Feedback();
            feedback.Name = name;
            feedback.Email = email;
            feedback.Phone = phone;
            feedback.Address = address;
            feedback.Content = content;
            feedback.CreatedDate = DateTime.Now;

            var id = _feedbackService.InsertFeedback(feedback);
            if (id > 0)
                return Json(new
                {
                    status = true
                });
            else
                return Json(new
                {
                    status = false
                });
        }
    }
}