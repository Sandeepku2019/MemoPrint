using MemoPrintingUtility.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace MemoPrintingUtility.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AuthenticateUser(string UserName, string Password)
        {
            string xmlString = Server.MapPath("/xml/UserInformation.xml");
            List<UserAuthenticate> items = null;
            XElement doc = XElement.Load(xmlString);
            items = (from p in doc.Descendants("User")
                         //.Elements("User")                     
                     select new UserAuthenticate
                     {
                         UserName = (string)p.Element("UserName"),
                         Password = (string)p.Element("Password")
                     }).ToList<UserAuthenticate>();


            bool output = false;
            if (items != null && items.Where(x => x.UserName.ToLower().Trim() == UserName.ToLower().Trim() && x.Password.Trim() == Password.Trim()).Count() > 0)
            {
                output = true;
            }
            return Json(output, JsonRequestBehavior.AllowGet); 
        }


        [HttpGet]
        public ActionResult RedirectHome()
        {
            return View("TabularReport","index");
        }



    }


}