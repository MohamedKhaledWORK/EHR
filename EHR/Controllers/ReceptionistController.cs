using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Repository;
using DataAccess.Models;
using EHR.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace EHR.Controllers
{
    public class ReceptionistController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IGenericRepository<Visit> _visitRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<Staff> _staffRepository;
        private readonly IAuthService _authService;
        private readonly IPatientRepository _patientRepository;
        private readonly ILabRepository _labtestRepository;
        private readonly IMapper _mapper;
        public ReceptionistController(IGenericRepository<Visit> visitRepository,IUserRepository userRepository, IDoctorRepository doctorRepository, IGenericRepository<Staff> staffRepository,IAuthService authService,IPatientRepository patientRepository, ILabRepository labtestRepository, IMapper mapper)

        {

            _visitRepository = visitRepository;
            _userRepository = userRepository;
             _doctorRepository = doctorRepository;
            _staffRepository = staffRepository;
            _authService = authService;
            _patientRepository = patientRepository;
            _labtestRepository = labtestRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var Staffid = HttpContext.Session.GetInt32("StaffId");
            var Receptionist = await _staffRepository.GetByIdAsync(Staffid.Value);
            var mapped = _mapper.Map<Staff, StaffViewModel>(Receptionist);
            return View(mapped);
        }
        public async Task<IActionResult> Edit()
        {
            var Staffid = HttpContext.Session.GetInt32("StaffId");
            var Receptionist = await _staffRepository.GetByIdAsync(Staffid.Value);
            return View(Receptionist);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Staff model)
        {
            if (ModelState.IsValid)
            {
                _staffRepository.Update(model);
                int Result = await _staffRepository.CompleteAsync();
                if (Result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
        public async Task<IActionResult> LabTest()
        {
            return View();
        }
        public async Task<IActionResult> RequestTest()
        {
            var labTestTypes = new List<LabTestTypeViewModel>
            {
                new LabTestTypeViewModel { LabTestType = "Complete Blood Count (CBC)", NormalRange = "4.5-6.0 million/µL" },
                new LabTestTypeViewModel { LabTestType = "Blood Glucose (Fasting)", NormalRange = "70-100 mg/dL" },
                new LabTestTypeViewModel { LabTestType = "Lipid Profile", NormalRange = "LDL < 100 mg/dL" },
                new LabTestTypeViewModel { LabTestType = "Liver Function Test (LFT)", NormalRange = "ALT: 7-56 U/L, AST: 10-40 U/L" },
                new LabTestTypeViewModel { LabTestType = "Thyroid Function Test (TSH)", NormalRange = "0.4-4.0 mIU/L" },
                new LabTestTypeViewModel { LabTestType = "Urine Culture", NormalRange = "No Growth" },
                new LabTestTypeViewModel { LabTestType = "HIV Test", NormalRange = "Negative" },
                new LabTestTypeViewModel { LabTestType = "C-Reactive Protein (CRP)", NormalRange = "< 10 mg/L" }
            };

            ViewBag.LabTestTypes = labTestTypes;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RequestTest(SimpleLabViewModel model)
        {
            if (ModelState.IsValid)
            {
                var patient =await _patientRepository.GetByPatientNameAsync(model.PatientUserName);
                if (patient is not null)
                {
                    var map = _mapper.Map<SimpleLabViewModel, Lab>(model);
                    map.Patient=patient;
                    await _labtestRepository.AddAsync(map);
                    int Result = await _patientRepository.CompleteAsync();
                    if (Result > 0)
                    {
                        return RedirectToAction(nameof(LabTest));
                    }
                }

            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
            }
            var labTestTypes = new List<LabTestTypeViewModel>
            {
                new LabTestTypeViewModel { LabTestType = "Complete Blood Count (CBC)", NormalRange = "4.5-6.0 million/µL" },
                new LabTestTypeViewModel { LabTestType = "Blood Glucose (Fasting)", NormalRange = "70-100 mg/dL" },
                new LabTestTypeViewModel { LabTestType = "Lipid Profile", NormalRange = "LDL < 100 mg/dL" },
                new LabTestTypeViewModel { LabTestType = "Liver Function Test (LFT)", NormalRange = "ALT: 7-56 U/L, AST: 10-40 U/L" },
                new LabTestTypeViewModel { LabTestType = "Thyroid Function Test (TSH)", NormalRange = "0.4-4.0 mIU/L" },
                new LabTestTypeViewModel { LabTestType = "Urine Culture", NormalRange = "No Growth" },
                new LabTestTypeViewModel { LabTestType = "HIV Test", NormalRange = "Negative" },
                new LabTestTypeViewModel { LabTestType = "C-Reactive Protein (CRP)", NormalRange = "< 10 mg/L" }
            };

            ViewBag.LabTestTypes = labTestTypes;

            return View(model);
        }
        public async Task<IActionResult> ResultTest(string Search)
        {
            if (Search is not null) 
            {
                var patient= await _patientRepository.GetByPatientNameAsync(Search);
                if (patient is not null)
                {
                    var labtest = await _labtestRepository.GetLabByPatientNameAsync(patient.Username);
                    var map = _mapper.Map<IEnumerable<Lab>,IEnumerable<LabViewModel>>(labtest);
                    return View(map);
                }
                else 
                {
                 TempData["Message"] = "No LabTest For This Patient";   
                    return RedirectToAction(nameof(LabTest));
                }
            }
            return RedirectToAction(nameof(LabTest));
        }

        public IActionResult AddPatient()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddPatient(PatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                var map = _mapper.Map<PatientViewModel, Patient>(model);
                var Flag =await _authService.RegisterPatient(map);
                if (Flag) 
                {
                    TempData["Message"] = "Patient is Created";
                    return RedirectToAction("AddPatient","Receptionist");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Visit()
        {
            var Visits= await _visitRepository.GetAllAsync();
            var map = _mapper.Map<IEnumerable<Visit>, IEnumerable<VisitViewModel>> (Visits);
            return View(map);
        }
  
        public async Task<IActionResult> AddVisit()
        {
            var availableDoctors = await _doctorRepository.GetAllAsync();

            // Assuming that each doctor has a list of available time slots
            ViewBag.Doctors = availableDoctors.Select(d => new
            {
                DoctorName = d.Staff.UserName,
                TimeSlots = d.Availabilities.Select(ts => $"{ts.DayOfWeek} - {ts.TimeSlot}").ToList()
            }).ToList();

            return View();
        }
        //public async Task<IActionResult> GetAvailableTimeSlots(string doctorUserName)
        //{
        //    var staff= await _userRepository.GetByUserNameAsync(doctorUserName);
        //    var doctor = await _doctorRepository.GetDoctorByStaffID(staff.Id);
        //    var timeSlots = doctor.Availabilities.Select(ts => new
        //    {
        //        Value =$"{ts.DayOfWeek} - {ts.TimeSlot}",  // or a formatted string like "Monday - 3:00 PM"
        //        Text = $"{ts.DayOfWeek} - {ts.TimeSlot}"  // or just the formatted text
        //    }).ToList();

        //    return Json(timeSlots);
        //}
        [HttpPost]
        public async Task<IActionResult> AddVisit(SimpleViewModel model)
        {
            if (ModelState.IsValid)
            {
            var patient = await _patientRepository.GetByPatientNameAsync(model.PatientUsername);
            var staff = await _userRepository.GetByUserNameAsync(model.DoctorUserName);
            var doctor = await _doctorRepository.GetDoctorByStaffID(staff.Id);
                var Visit = new Visit()
                {
                    PatientId = patient.Id,
                    Patient = patient,
                    Doctor = doctor,
                    Date = model.Date,
                    ReasonForVisit = model.ReasonForVisit,
                    VisitType = model.VisitType,
                };
                await _visitRepository.AddAsync(Visit);
                var Result = await _visitRepository.CompleteAsync();
                if (Result > 0)
                {
                    return RedirectToAction("Visit");
                }
            }
            else 
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage); // Or use a logger
                }
            }

            var availableDoctors = await _doctorRepository.GetAllAsync();

            // Assuming that each doctor has a list of available time slots
            ViewBag.Doctors = availableDoctors.Select(d => new
            {
                DoctorName = d.Staff.UserName,
                TimeSlots = d.Availabilities.Select(ts => $"{ts.DayOfWeek} - {ts.TimeSlot}").ToList()
            }).ToList();
            return View(model);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
