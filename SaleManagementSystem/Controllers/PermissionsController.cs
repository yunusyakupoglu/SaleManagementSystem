using Data.IServices;
using Data.Models;
using System;
using System.Web.Mvc;

namespace SaleManagementSystem.Controllers
{
    public class PermissionsController : Controller
    {
        private readonly IPermissionService _service;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;

        public PermissionsController(IPermissionService service, IRoleService roleService, IUserService userService)
        {
            _service = service;
            _roleService = roleService;
            _userService = userService;
        }

        // GET: Permissions
        public ActionResult Index()
        {
            ViewBag.Users = _userService.Get();
            var permissions = _service.Get();
            return View(permissions);
        }

        public ActionResult Refresh()
        {
            ViewBag.Users = _userService.Get();
            try
            {

                var permissions = _service.Get();
                return Json(new { data = permissions }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Insert(bool view, bool insert, bool edit, bool delete, Guid userGuid)
        {
            try
            {
                Permission permission = new Permission
                {
                    CanView = view,
                    CanInsert = insert,
                    CanEdit = edit,
                    CanDelete = delete,
                    UserGuid = userGuid
                };

                var user = _userService.GetByGuid(userGuid);
                _service.Insert(permission);
                return Json(new { success = true, message = $"{user.FirstName} {user.LastName} kullanıcısına yetki verme işlemi başarılı." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}