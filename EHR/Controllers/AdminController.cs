using AutoMapper;
using BusinessLogic.Interfaces;
using DataAccess.Models;
using EHR.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace EHR.Controllers
{
    public class AdminController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAvailableTimeRepositry _timeRepository;
        private readonly IGenericRepository<Visit> _visitRepository;
        private readonly IUserRepository _staffRepository;
        private readonly IGenericRepository<Patient> _patientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AdminController(IDoctorRepository genericRepository,IAvailableTimeRepositry timeRepository,IGenericRepository<Visit> visitRepository, IUserRepository StaffRepository, IGenericRepository<Patient> patientRepository,IUserRepository userRepository ,IMapper mapper)
        {
            _doctorRepository = genericRepository;
            _timeRepository = timeRepository;
            _visitRepository = visitRepository;
            _staffRepository = StaffRepository;
            _patientRepository = patientRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string? message = null)
        {
            ViewBag.Message = message;
            var Doctors= await _doctorRepository.GetAllAsync();
            var mappedDoctors = _mapper.Map<IEnumerable<Doctors>, IEnumerable<DoctorViewModel>>(Doctors);
            return View(mappedDoctors);
        }

        public IActionResult AddDoctors(string? message = null)
        {
            ViewBag.Message = message;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddDoctors(Doctors doctor)
        {
            
            var User = await _userRepository.GetByUserNameAsync(doctor.Staff.UserName);
            if (User != null)
            {
                return RedirectToAction("AddDoctors" , new { message = "user name already exist be human okkkkay?"});
            }
            if (Regex.IsMatch(doctor.Staff.Name, @"^[a-zA-Z\s]+$") && Regex.IsMatch(doctor.Staff.UserName, @"^[a-zA-Z0-9\s]+$"))
            {
                await _doctorRepository.AddAsync(doctor);
                int Result = await _doctorRepository.CompleteAsync();
                if (Result > 0)
                {
                    return RedirectToAction(nameof(Index), new { message = $"Dr.{doctor.Staff.Name} added successfuly"});
                }
                else
                {
                    return RedirectToAction("AddDoctors", new { message = "There is a problem in adding doctor" });
                }
            }
            else
            {
                return RedirectToAction("AddDoctors", new { message = "There is a special character in user name or name of staff" });
            }
        }


        //AvailableTime

        public async Task<IActionResult> ShowAvailableTime(int id) 
        {
            var doctor =await _doctorRepository.GetByIdAsync(id);
            ViewBag.doctor = doctor;
            var DoctorTime =await _timeRepository.GetAllById(id);
            return View(DoctorTime);
        }
        public async Task<IActionResult> AvailableTime(int id, string? message = null)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            ViewBag.doctor = doctor;
            ViewBag.Message = message;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AvailableTime(TimeViewModel model)
        {
            var staff = await _staffRepository.GetByUserNameAsync(model.DoctorUserName);
            var doc = await _doctorRepository.GetDoctorByStaffID(staff.Id);
            if (ModelState.IsValid) 
            {
                var map =_mapper.Map<TimeViewModel,DoctorAvailability>(model);
                map.Doctor = doc;
                map.DoctorId = doc.Id;
                var availableTime = await _timeRepository.GetAllById(doc.Id);
                if (availableTime != null)
                {
                    foreach (var time in availableTime)
                    {
                        if (time.TimeSlot == model.TimeSlot && time.DayOfWeek == model.DayOfWeek)
                        {
                            return RedirectToAction("AvailableTime", new { id = doc.Id, message = "This time is already exist" });
                        }
                    }
                }
                await _timeRepository.AddAsync(map);
                int Result = await _timeRepository.CompleteAsync();
                if (Result > 0) 
                {
                   return RedirectToAction("Index", new { message = $"new time added successfuly for dr.{doc.Staff.Name}"});
                }
                var doctor = await _doctorRepository.GetByIdAsync(doc.Id);
                ViewBag.doctor = doctor;
            }
           
            return RedirectToAction("AvailableTime", new { id = doc.Id , message = "please be a human and fill the form"});
        }


        //[HttpPost]
        public async Task<IActionResult> Staff()
        {
            var Staff = await _staffRepository.GetAllAsync();
            return View(Staff);
        }
        public IActionResult AddStaff(string? message = null)
        {
            ViewBag.Message = message;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddStaff(Staff staff)
        {
            
            if (ModelState.IsValid)
            {
                var User = await _userRepository.GetByUserNameAsync(staff.UserName);
                if (User is null)
                {
                    if (Regex.IsMatch(staff.Name, @"^[a-zA-Z\s]+$")&& Regex.IsMatch(staff.UserName, @"^[a-zA-Z0-9\s]+$"))
                    {
                        await _staffRepository.AddAsync(staff);
                        int Result = await _staffRepository.CompleteAsync();
                        if (Result > 0)
                        {
                            return RedirectToAction(nameof(Staff));
                        }
                    }
                    else
                    {
                        return RedirectToAction("AddStaff", new { message = "There is a special character in user name or name of staff" });
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Username Already Exist");
                }
            }
            return View(staff);
        }
        public async Task<IActionResult> Patient()
        {
            var Patients= await _patientRepository.GetAllAsync();
            var mappedpatient = _mapper.Map<IEnumerable<Patient>,IEnumerable<PatientViewModel>>(Patients);
            return View(mappedpatient);
        }
        public async Task<IActionResult> Visits()
        {
            var Visits = await _visitRepository.GetAllAsync();
            var mapped = _mapper.Map<IEnumerable<Visit>,IEnumerable<VisitViewModel>>(Visits);
            return View(mapped);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Login", "Account"); 
        }
    }
}
