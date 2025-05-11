using BusinessLogic.Interfaces;
using DataAccess.Models;
using EHR.Helper;
using EHR.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EHR.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IPatientRepository _patientRepository;
        private readonly IUserRepository _userRepository;

        public AccountController(IAuthService authService,IPatientRepository patientRepository, IUserRepository userRepository)
        {
            _authService = authService;
            _patientRepository = patientRepository;
            _userRepository = userRepository;
        }



        public IActionResult Intro()
        {
            return View();
        }
        public IActionResult NewUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NewUser(LoginViewModel model)
        {
            if (ModelState.IsValid) 
            {
               var User = await _userRepository.GetByUserNameAsync(model.UserName);
                if (User is not null)
                {
                    if (User.Password is null)
                    {
                        User.Password = model.Password;
                        _userRepository.Update(User);
                        int Result = await _userRepository.CompleteAsync();
                        if (Result > 0)
                        {
                            TempData["Message"] = "Welcome! Use ur New Password ";
                          return  RedirectToAction(nameof(Login));
                        }
                    }
                    else 
                    {
                        TempData["Message"] = "You Already have Password Created"; 
                        return RedirectToAction(nameof(Login));
                    }
                }
            }
            return View(model);
        }
        public IActionResult NewUserP()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NewUserP(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = await _patientRepository.GetByPatientNameAsync(model.UserName);
                if (User is not null)
                {
                    if (User.Password is null)
                    {
                        User.Password = model.Password;
                        _patientRepository.Update(User);
                        int Result = await _patientRepository.CompleteAsync();
                        if (Result > 0)
                        {
                            TempData["Message"] = "Welcome! Use ur New Password ";
                            return RedirectToAction(nameof(LoginP));
                        }
                    }
                    else
                    {
                        TempData["Message"] = "You Already have Password Created";
                        return RedirectToAction(nameof(LoginP));
                    }
                }
            }


            return View(model);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Patient patient)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterPatient(patient);
                if (result) 
                {
                    return RedirectToAction("LoginP");
                }
            }
            return View(patient);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.ValidateStaff(model.UserName, model.Password);
                if (result)
                {
                    var userType = await _userRepository.GetByUserNameAsync(model.UserName);
                    HttpContext.Session.SetInt32("StaffId", userType.Id); 
                    if (userType is not null)
                    {
                        var Role = userType.Role;
                        if (Role == "Doctor")
                            return RedirectToAction("Index", "Doctor");
                        else if (Role == "Laboratory")
                            return RedirectToAction($"Index", "Lab");
                        else if (Role == "Receptionist")
                            return RedirectToAction("Index", "Receptionist");
                        else if (Role == "Admin")
                            return RedirectToAction("Index", "Admin");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Username or Password");
                }
            }
            return View(model);
        }
        public IActionResult LoginP()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginP(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userType = await _patientRepository.GetByPatientNameAsync(model.UserName);
                if (userType == null)
                {
                    TempData["ErrorMessage"] = "Username doesn't exist.";
                }
                else
                {
                    var result = await _authService.ValidatePatient(model.UserName, model.Password);
                    if (result)
                    {
                        HttpContext.Session.SetInt32("PatientId", userType.Id);
                        return RedirectToAction("Index", "Patient");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid Username or Password.";
                    }
                }
            }
            return RedirectToAction(nameof(LoginP));
        }
    
    }
}
