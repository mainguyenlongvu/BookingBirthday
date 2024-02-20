using Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TGClothes.Areas.Admin.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        // GET: Admin/Feedback
        public ActionResult Index()
        {
            var model = _feedbackService.GetAll();
            return View(model);
        }

        [HttpDelete]
        public ActionResult Delete(long id)
        {
            _feedbackService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}