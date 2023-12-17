using DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System.Data;
using WebApplication.Models;

namespace WebApplication.Controllers
{
	//[Authorize(Roles = "Admin")]
	public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
		private readonly UserManager<AppUser> userManager;
        private readonly DentistRepository dentistRepository;
        private readonly EmployeeRepository employeeRepository;

        public AdministrationController(
            RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager,
            DentistRepository dentistRepository,
            EmployeeRepository employeeRepository)
        {
            this.roleManager = roleManager;
			this.userManager = userManager;
            this.dentistRepository = dentistRepository;
            this.employeeRepository = employeeRepository;
        }

        #region Features concerning user
        public async Task<IActionResult> ListUsers()
        {
            var users = await userManager.Users.ToListAsync();
            List<AppUser> userList = new List<AppUser>();
            foreach (var user in users)
            {
                if (!(await userManager.IsInRoleAsync(user, "Customer"))
                    && !(await userManager.IsInRoleAsync(user, "Admin")))
                {
                    userList.Add(user);
                }
            }
            return View(userList);
        }
        public IActionResult CreateUser()
        {
            ViewData["UserTypeList"] = new List<SelectListItem>()
            {
                new SelectListItem("Dentist", "Dentist"),
                new SelectListItem("Employee", "Employee")
            };
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.UserName);
                if (user != null)
                {
                    ModelState.AddModelError(string.Empty, $"User with user name {user.UserName} already exists");
                    return View(model);
                }
                var newUser = new AppUser()
                {
                    UserName = model.UserName
                };
                var result = await userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    if (model.UserType == "Dentist")
                    {
                        result = await userManager.AddToRoleAsync(newUser, "Dentist");
                        if (result.Succeeded)
                        {
                            var dentist = new Dentist()
                            {
                                AccountId = newUser.Id,
                                PhoneNumber = model.PhoneNumber,
                                FullName = model.FullName,
                                Account = newUser
                            };
                            await dentistRepository.AddDentistAsync(dentist);

                            return RedirectToAction("ListUsers", "Administration");
                        }
                    }
                    else if (model.UserType == "Employee")
                    {
                        result = await userManager.AddToRoleAsync(newUser, "Employee");
                        if (result.Succeeded)
                        {
                            var employee = new Employee()
                            {
                                AccountId = newUser.Id,
                                PhoneNumber = model.PhoneNumber,
                                FullName = model.FullName,
                                Account = newUser
                            };
                            await employeeRepository.AddEmployeeAsync(employee);

                            return RedirectToAction("ListUsers", "Administration");
                        }
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return View();
            }
            var roles = await userManager.GetRolesAsync(user);
            EditUserModel model = null;
            if (await userManager.IsInRoleAsync(user, "Dentist"))
            {
                Dentist dentist = await dentistRepository.GetDentistByAccountAsync(user);
                model = new EditUserModel()
                {
                    Id = dentist.Id,
                    UserName = user.UserName,
                    PhoneNumber = dentist.PhoneNumber,
                    Roles = roles
                };
            }
            else if (await userManager.IsInRoleAsync(user, "Employee"))
            {
                Employee employee = await employeeRepository.GetEmployeeByAccountAsync(user);
                model = new EditUserModel()
                {
                    Id = employee.Id,
                    UserName = user.UserName,
                    PhoneNumber = employee.PhoneNumber,
                    Roles = roles
                };
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, $"Role with Id = {model.Id} is not exist");
                    return View("Error");
                }

                // Update phone number
                if (await userManager.IsInRoleAsync(user, "Dentist"))
                {
                    Dentist dentist = await dentistRepository.GetDentistByAccountAsync(user);
                    dentist.PhoneNumber = model.PhoneNumber;
                    await dentistRepository.UpdateDentistAsync(dentist);
                }
                else if (await userManager.IsInRoleAsync(user, "Employee"))
                {
                    Employee employee = await employeeRepository.GetEmployeeByAccountAsync(user);
                    employee.PhoneNumber = model.PhoneNumber;
                    await employeeRepository.UpdateEmployeeAsync(employee);
                }


                user.UserName = model.UserName;
                
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers", "Administration");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
			var user = await userManager.FindByIdAsync(id);
			if (user == null)
			{
				return NotFound("Some errors occur");
			}
            try
            {
				var result = await userManager.DeleteAsync(user);
				if (result.Succeeded)
				{
					return Ok("Deleted successfully");
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
				return BadRequest("Some errors occur");
			}
            catch (DbUpdateException ex)
            {
				return BadRequest("User cannot be deleted because this user has some roles");
			}
		}

		public async Task<IActionResult> EditRolesInUser(string userId)
		{
			var user = await userManager.FindByIdAsync(userId);
			if (user == null)
			{
				ViewData["ErrorMessage"] = $"User with Id {userId} is not found";
				return View();
			}

			ViewData["UserId"] = user.Id;
			ViewData["UserName"] = user.UserName;

			var model = new List<RoleUserModel>();

			foreach (var role in await roleManager.Roles.ToListAsync())
			{
				var roleUserModel = new RoleUserModel()
				{
					RoleId = role.Id,
					RoleName = role.Name
				};

				if (await userManager.IsInRoleAsync(user, role.Name))
				{
					roleUserModel.IsSelected = true;
				}
				else
				{
					roleUserModel.IsSelected = false;
				}

				model.Add(roleUserModel);
			}
			return View(model);

		}

		[HttpPost]
		public async Task<IActionResult> EditRolesInUser(List<RoleUserModel> model, string userId)
		{
			if (ModelState.IsValid)
			{
				var user = await userManager.FindByIdAsync(userId);
				if (user == null)
				{
					ViewData["ErrorMessage"] = $"User with Id {userId} is not found";
					return View();
				}

                var roles = await userManager.GetRolesAsync(user);
                var result = await userManager.RemoveFromRolesAsync(user, roles);
                if (result.Succeeded)
                {
                    var roleList = model.Where(x => x.IsSelected).Select(r => r.RoleName).ToList();
                    if (roleList.Any())
                    {
                        result = await userManager.AddToRolesAsync(user, roleList);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("EditUser", new { id = userId });
                        }
                    }
                    
                }
                ModelState.AddModelError("", "Cannot add or remove existing roles");
            }
			return View(model);

		}
		#endregion
	}
}
