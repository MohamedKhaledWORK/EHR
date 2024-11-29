using AutoMapper;
using AutoMapper.Configuration;
using BusinessLogic.Interfaces;
using BusinessLogic.Repository;
using DataAccess.Models;
using EHR.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EHR.Controllers
{
    public class PatientController : Controller
    {
        private readonly IMedicalPrescriptionRepository _medicalPrescription;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IVisitRepository _visitRepository;
        private readonly ILabRepository _labRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public PatientController(ILabRepository labRepository, IUserRepository userRepository,IMedicalPrescriptionRepository medicalPrescription,IPatientRepository patientRepository, IDoctorRepository doctorRepository, IVisitRepository visitRepository,IMapper mapper) 
        {
            _medicalPrescription = medicalPrescription;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _visitRepository = visitRepository;
            _mapper = mapper;
            _labRepository = labRepository;
            _userRepository = userRepository;
        }
        public async Task<IActionResult> Index()
        {
            var Patientid = HttpContext.Session.GetInt32("PatientId");
            var patient = await _patientRepository.GetByIdAsync(Patientid.Value);
            var mapped = _mapper.Map<Patient, PatientViewModel>(patient);
            return View(mapped);
        }
        public async Task<IActionResult> Edit()
        {
            var PatientId = HttpContext.Session.GetInt32("PatientId");
            var patient = await _patientRepository.GetByIdAsync(PatientId.Value);
            return View(patient);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Patient model)
        {
            if (ModelState.IsValid)
            {
                _patientRepository.Update(model);
                int Result = await _patientRepository.CompleteAsync();
                if (Result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Visit()
        {
            var PatientId = HttpContext.Session.GetInt32("PatientId");
            var Visits = PatientId != null ? await _visitRepository.GetAlltoPatient(PatientId.Value) : await _visitRepository.GetAllAsync() ;
            var map = _mapper.Map<IEnumerable<Visit>, IEnumerable<VisitViewModel>>(Visits);
            return View(map);
        }
        
        public async Task<IActionResult> AddVisit()
        {
            var availableDoctors = await _doctorRepository.GetAllAsync();
            var Patientid = HttpContext.Session.GetInt32("PatientId");
            var patient = await _patientRepository.GetByIdAsync(Patientid.Value);
            ViewBag.PatientName=patient.Username;
            // Assuming that each doctor has a list of available time slots
            ViewBag.Doctors = availableDoctors.Select(d => new
            {
                DoctorName = d.Staff.UserName,
                TimeSlots = d.Availabilities.Select(ts => $"{ts.DayOfWeek} - {ts.TimeSlot}").ToList()
            }).ToList();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddVisit(SimpleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Patient = await _patientRepository.GetByPatientNameAsync(model.PatientUsername);
                var staff = await _userRepository.GetByUserNameAsync(model.DoctorUserName);
                var doctor = await _doctorRepository.GetDoctorByStaffID(staff.Id);
                var Visit = new Visit()
                {
                    PatientId = Patient.Id,
                    Patient = Patient,
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
            var availableDoctors = await _doctorRepository.GetAllAsync();
            var Patientid = HttpContext.Session.GetInt32("PatientId");
            var patient = await _patientRepository.GetByIdAsync(Patientid.Value);
            ViewBag.PatientName = patient.Username;
            // Assuming that each doctor has a list of available time slots
            ViewBag.Doctors = availableDoctors.Select(d => new
            {
                DoctorName = d.Staff.UserName,
                TimeSlots = d.Availabilities.Select(ts => $"{ts.DayOfWeek} - {ts.TimeSlot}").ToList()
            }).ToList();

            return View(model);
        }
        public async Task<IActionResult> Prescription()
        {
            var PatientId = HttpContext.Session.GetInt32("PatientId");
            var Prescription = PatientId != null ?  await _medicalPrescription.GetAllPatientById(PatientId.Value) : await _medicalPrescription.GetAllAsync();
            var map = _mapper.Map<IEnumerable<MedicationPrescription>, IEnumerable<PrescriptionViewModel>>(Prescription);
            foreach (var m in map)
            {
                // Find the corresponding prescription (you need to define how to match them)
                var prescription = Prescription.FirstOrDefault(p => p.Id == m.Id); // assuming 'PrescriptionId' exists in both
                if (prescription != null)
                {
                    m.DoctorUserName = prescription.Doctor.Staff.UserName; // Assign the username from Doctor in Prescription
                }
            }
            return View(map);
        }
        public async Task<IActionResult> LabTests()
        {
            var PatientId = HttpContext.Session.GetInt32("PatientId");
            var patient =await _patientRepository.GetByIdAsync(PatientId.Value);
            var labTests = await _labRepository.GetLabByPatientNameAsync(patient.Username);
                if (labTests.Count() > 0)
                {
                    var map = _mapper.Map<IEnumerable<Lab>, IEnumerable<LabViewModel>>(labTests);
                    return View(map);
                }
                else
                {
                    TempData["Message"] = "No LabTest For This Patient";
                }
            return View(new LabViewModel());
        }
        public IActionResult LogoutP()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LoginP", "Account");
        }
    }
}
