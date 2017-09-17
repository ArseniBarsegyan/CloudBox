using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using CloudBox.WebUI.Filters;
using CloudBox.WebUI.Models;
using CloudBox.WebUI.ServiceReference1;

namespace CloudBox.WebUI.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        public AccountController()
        {
        }

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login()
        {
            return PartialView();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToAction("Manage", "Account");
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    WebSecurity.Login(model.UserName, model.Password);
                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError(string.Empty, ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Manage

        [HttpGet]
        public ActionResult Manage()
        {
            var username = User.Identity.Name;
            //CheckIfDirectoryWithUserNameExists(username);

            using (CloudBoxServiceClient serviceClient = new CloudBoxServiceClient())
            {
                serviceClient.CheckIfDirectoryWithUserNameExists(username);
            }

            //Send into view current path, it's directories and files
            ViewBag.CurrentPath = username;
            return View();
        }

        //Returns partial view with all files and directories by sending path as param
        public ActionResult DirectoriesAndFilesSummary(string path)
        {
            using (CloudBoxServiceClient serviceClient = new CloudBoxServiceClient())
            {
                var fileLink = serviceClient.GetFileLink(path);
                ViewBag.FileLink = serviceClient.GetFileLink(path);
                ViewBag.Directories = serviceClient.GetAllDirectoriesByPath(User.Identity.Name, path);
                ViewBag.Files = serviceClient.GetAllFilesByPath(User.Identity.Name, path);
            }

            return PartialView();
        }

        //AJAX upload file
        [HttpPost]
        public JsonResult Upload()
        {
            var files = Request.Files;
            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];
                if (upload != null)
                {
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    string savePath = Request.Params["path"] + fileName;
                    int fileSizeInBytes = upload.ContentLength;
                    using (MemoryStream target = new MemoryStream())
                    {
                        upload.InputStream.CopyTo(target);
                        byte[] data = target.ToArray();

                        using (CloudBoxServiceClient serviceClient = new CloudBoxServiceClient())
                        {
                            return Json(serviceClient.Upload(data, savePath));
                        }
                    }                    
                    //upload.SaveAs(Server.MapPath("~/Files/" + savePath));                    
                }
            }            
            return Json("Server error");
        }

        //Create folder
        [HttpPost]
        public JsonResult CreateFolder()
        {            
            string result = string.Empty;
            if (Request.Params["path"] != null)
            {
                using (CloudBoxServiceClient serviceClient = new CloudBoxServiceClient())
                {
                    result = serviceClient.CreateFolderIfNotExists(Request.Params["path"]);
                }
            }
            return Json(result);
        }

        //Remove file or directory(directory delete is recursive)
        [HttpGet]
        public JsonResult RemoveElement(string path)
        {
            if (path != null)
            {
                using (CloudBoxServiceClient serviceClient = new CloudBoxServiceClient())
                {
                    serviceClient.RemoveElement(path);
                }
            }            

            return Json("Deleted succussfull", JsonRequestBehavior.AllowGet);
        }
        #region Helpers

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
