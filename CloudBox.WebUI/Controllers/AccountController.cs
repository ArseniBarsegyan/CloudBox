using System.IO;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using CloudBox.WebUI.Filters;
using CloudBox.WebUI.Helpers;
using CloudBox.WebUI.Models;
using CloudBox.WebUI.ServiceReference1;

namespace CloudBox.WebUI.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            // Disable partial view call from Url
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Home");
            }
            return PartialView();
        }        

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
            ModelState.AddModelError(string.Empty, ConstantHelper.UserNameOrPasswordIncorrect);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
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
            return View(model);
        }

        [HttpGet]
        public ActionResult Manage()
        {
            var username = User.Identity.Name;
            using (CloudServiceClient serviceClient = new CloudServiceClient())
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
            using (CloudServiceClient serviceClient = new CloudServiceClient())
            {
                ViewBag.FileLink = serviceClient.GetFileLink(path);
                ViewBag.Directories = serviceClient.GetAllDirectoriesByPath(path);
                ViewBag.Files = serviceClient.GetAllFilesByPath(path);
            }
            return PartialView();
        }

        //Download file
        public FileResult DownloadFile(string path)
        {
            var fileBytes = System.IO.File.ReadAllBytes(path);
            var fileName = path.Substring(path.LastIndexOf('/') + 1);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        //AJAX upload file
        [HttpPost]
        public JsonResult Upload()
        {
            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];
                if (upload != null)
                {
                    var fileName = Path.GetFileName(upload.FileName);
                    var savePath = Request.Params["path"] + fileName;
                    using (MemoryStream target = new MemoryStream())
                    {
                        upload.InputStream.CopyTo(target);
                        byte[] data = target.ToArray();

                        using (CloudServiceClient serviceClient = new CloudServiceClient())
                        {
                            return Json(serviceClient.Upload(data, savePath));
                        }
                    }
                }
            }            
            return Json(ConstantHelper.ServerError);
        }

        //Create folder
        [HttpPost]
        public JsonResult CreateFolder()
        {            
            var result = string.Empty;
            if (Request.Params["path"] != null)
            {
                using (CloudServiceClient serviceClient = new CloudServiceClient())
                {
                    result = serviceClient.CreateDirectoryIfNotExists(Request.Params["path"]);
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
                using (CloudServiceClient serviceClient = new CloudServiceClient())
                {
                    serviceClient.RemoveElement(path);
                }
            }            

            return Json(ConstantHelper.DeletedSuccessful, JsonRequestBehavior.AllowGet);
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
