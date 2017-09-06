using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using CloudBox.DAL.Interfaces;
using CloudBox.DAL.Repositories;
using WebMatrix.WebData;
using CloudBox.WebUI.Filters;
using CloudBox.WebUI.Models;

namespace CloudBox.WebUI.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        private IRepository _repository;

        public AccountController()
        {
            _repository = new Repository(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
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
            CheckIfDirectoryWithUserNameExists(username);

            //Send into view current path, it's directories and files
            ViewBag.CurrentPath = username;
            return View();
        }

        //Returns partial view with all files and directories by sending path as param
        public ActionResult DirectoriesAndFilesSummary(string path)
        {
            ViewBag.DirectoriesAndFilesRelativePath = path;

            ViewBag.Directories = GetAllDirectoriesByPath(path);
            ViewBag.Files = GetAllFilesByPath(path);

            return PartialView();
        }

        //Get all directories from path
        private IEnumerable<string> GetAllDirectoriesByPath(string path)
        {
            var directoriesPaths = Directory.GetDirectories(Server.MapPath("~/Files/" + path));
            for (var i = 0; i < directoriesPaths.Length; i++)
            {
                directoriesPaths[i] = new FileInfo(directoriesPaths[i]).Name;
            }
            return directoriesPaths;
        }

        //Get all files from path
        private IEnumerable<string> GetAllFilesByPath(string path)
        {
            var filesPaths = Directory.GetFiles(Server.MapPath("~/Files/" + path));
            for (var i = 0; i < filesPaths.Length; i++)
            {
                filesPaths[i] = new FileInfo(filesPaths[i]).Name;
            }
            return filesPaths;
        }

        //Check if Server contains user folder. If not, create it
        private void CheckIfDirectoryWithUserNameExists(string username)
        {
            var usersDirectories = Directory.GetDirectories(Server.MapPath("~/Files/"));
            var directoriesNames = usersDirectories.Select(directoryFullName => new FileInfo(directoryFullName).Name);
            if (!directoriesNames.Contains(username))
            {
                Directory.CreateDirectory(Server.MapPath("~/Files/" + username));
            }
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
