
using DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly CustomerRepository customerRepository;
		private readonly DentistRepository dentistRepository;
		private readonly EmployeeRepository employeeRepository;

		public AccountController(
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager,
            CustomerRepository customerRepository,
            DentistRepository dentistRepository,
            EmployeeRepository employeeRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.customerRepository = customerRepository;
			this.dentistRepository = dentistRepository;
			this.employeeRepository = employeeRepository;
		}
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser()
                {
                    UserName = model.PhoneNumber
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
					result = await userManager.AddToRoleAsync(user, "Customer");
					if (result.Succeeded)
                    {
						await signInManager.SignInAsync(user, isPersistent: false);
                        var customer = new Customer()
                        {
                            AccountId = user.Id,
                            PhoneNumber = model.PhoneNumber,
                            FullName = model.FullName,
                            Address = model.Address,
                            DayOfBirth = model.DayOfBirth
                        };
                        await customerRepository.AddCustomerAsync(customer);

                        return RedirectToAction("Index", "Home");
					}
					
					
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
        public IActionResult Login(string? ReturnUrl = null)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
			return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string? ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.UserName, 
                    model.Password, 
                    model.RememberMe, 
                    lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                if (result.RequiresTwoFactor)
                {

                }
                if (result.IsLockedOut)
                {

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> IsAccountAvailable(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return Json(true);
            return Json($"This Phone Number is already taken");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> EditProfile(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            Customer customer;
            Dentist dentist;
            Employee employee;
            EditProfileModel editProfile;

			if (await userManager.IsInRoleAsync(user, "Customer"))
            {
                customer = await customerRepository.GetCustomerByAccountAsync(user);
                editProfile = new EditProfileModel()
				{
					PhoneNumber = customer.PhoneNumber,
                    FullName = customer.FullName,
                    DayOfBirth = customer.DayOfBirth,
                    Address = customer.Address
				};
				return View(editProfile);
			}
			else if (await userManager.IsInRoleAsync(user, "Dentist"))
			{
				dentist = await dentistRepository.GetDentistByAccountAsync(user);
				editProfile = new EditProfileModel()
				{
					PhoneNumber = dentist.PhoneNumber,
					FullName = dentist.FullName
				};
				return View(editProfile);
			}
            else if (await userManager.IsInRoleAsync(user, "Employee"))
            {
                employee = await employeeRepository.GetEmployeeByAccountAsync(user);
                editProfile = new EditProfileModel()
                {
                    PhoneNumber = employee.PhoneNumber,
                    FullName = employee.FullName
                };
                return View(editProfile);
            }

            return View();
        }
        [HttpPost]
        public IActionResult EditProfile(EditProfileModel model)
        {

            return View();
        }
    }
}
