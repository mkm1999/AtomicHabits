using Application.GetUsers;
using Application.UserService.Command.Deleate;
using Application.UserService.Command.Edit;
using Application.UserService.Command.EditPurposeService;
using Application.UserService.Command.Register;
using Application.UserService.Query.GetRoles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ui.Areas.Admin.Models.ViewModels;

namespace Ui.Areas.Admin.Controllers
{
    [Authorize(Roles = "Creator,Admin")]
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IGetUsers _getUsers;
        private readonly IGetRolesService _getRoles;
        private readonly IRegisterUserService _RegisterUser;
        private readonly IDeleateUser _Deleate;
        private readonly IEditUserService _editUserService;
        private readonly IEditPurpose _editPurpose;
        public UserController(IGetUsers getUsers, IGetRolesService getrole, IRegisterUserService registerUser , IDeleateUser Deleate , IEditUserService editUser , IEditPurpose editPurpose)
        {
            _getUsers = getUsers;
            _getRoles = getrole;
            _RegisterUser = registerUser;
            _Deleate = Deleate;
            _editUserService = editUser;
            _editPurpose = editPurpose;
        }
        public IActionResult Index()
        {
            return View(_getUsers.Execute());
        }
        [HttpGet]
        public IActionResult Register()
        {
            var Roles = _getRoles.Execute();
            ViewBag.roles = new SelectList(Roles, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Register([FromBody] RequestRegisterUserDto request)
        {
            var Result = _RegisterUser.Execute(request);
            return Json(Result);
        }
        [HttpPost]
        [Authorize(Roles = "Creator")]
        public IActionResult Deleate([FromBody]DeleateUserVM deleateUserVM)
        {
            int Id = deleateUserVM.UserId;
            var result = _Deleate.Execute(Id);
            return Json(result);
        }
        [HttpGet]
        [Authorize(Roles = "Creator")]
        public IActionResult Edit(int Id)
        {
            var Roles = _getRoles.Execute();
            ViewBag.roles = new SelectList(Roles, "Id", "Name");
            ViewBag.Id = Id;
            return View();
        }
        [HttpPost]
        public IActionResult Edit([FromBody]RequestEditUserDto request)
        {
            var result = _editUserService.Execute(request.Id, request);
            return Json(result);
        }

        public IActionResult Purposeplus(int UserId)
        {
            _editPurpose.Execute(UserId,true);
            return RedirectToAction("Index");
        }
        public IActionResult Purposeminus(int UserId)
        {
            _editPurpose.Execute(UserId, false);

            return RedirectToAction("Index", "User", new {area = "Admin"});
        }
    }
}
