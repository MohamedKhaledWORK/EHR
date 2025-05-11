using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Repository;
using DataAccess.Context;
using DataAccess.Models;
using EHR.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EHR.Controllers
{
    public class DoctorController : Controller
    {
        private readonly EHRdbContext _dbContext;
        private readonly IAvailableTimeRepositry _timeRepositry;
        private readonly ILabRepository _labRepository;
        private readonly IMedicalPrescriptionRepository _medicalPrescription;
        private readonly IVisitRepository _visitRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public DoctorController(EHRdbContext dbContext,IAvailableTimeRepositry timeRepositry,ILabRepository labRepository , IMedicalPrescriptionRepository medicalPrescription, IVisitRepository visitRepository, IPatientRepository patientRepository, IDoctorRepository doctorRepository, IUserRepository userRepository, IMapper mapper)
        {
            _dbContext = dbContext;
            _timeRepositry = timeRepositry;
            _labRepository = labRepository;
            _medicalPrescription = medicalPrescription;
            _visitRepository = visitRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string? message = null)
        {
            var Staffid = HttpContext.Session.GetInt32("StaffId");
            ViewBag.Message = message;
            var Doctor = await _doctorRepository.GetDoctorByStaffID(Staffid.Value);
            var mapped = _mapper.Map<Doctors, DoctorViewModel>(Doctor);
            return View(mapped);
        }
        //available time
        public async Task<IActionResult> ShowAvailableTime(int id,string? message = null)
        {
            var doctor =await _doctorRepository.GetByIdAsync(id);
            ViewBag.doctor = doctor;
            ViewBag.Message = message;
            var DoctorTime = await _timeRepositry.GetAllById(id);
            return View(DoctorTime);
        }
        public async Task<IActionResult> AvailableTime(int id,string? message = null)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            ViewBag.doctor = doctor;
            ViewBag.Message = message;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AvailableTime(TimeViewModel model)
        {
            var staff = await _userRepository.GetByUserNameAsync(model.DoctorUserName);
            var doc = await _doctorRepository.GetDoctorByStaffID(staff.Id);
            if (ModelState.IsValid)
            {
                var map = _mapper.Map<TimeViewModel, DoctorAvailability>(model);
                map.Doctor = doc;
                map.DoctorId = doc.Id;
                var availableTime = await _timeRepositry.GetAllById(doc.Id);
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
                await _timeRepositry.AddAsync(map);
                int Result = await _timeRepositry.CompleteAsync();
                if (Result > 0)
                {
                    return RedirectToAction("ShowAvailableTime", new { id = doc.Id, message = "new time added successfuly" });
                }
                else
                {
                    RedirectToAction("AvailableTime", new { id = doc.Id, message = "Field update your data" });
                }
                var doctor = await _doctorRepository.GetByIdAsync(doc.Id);
                ViewBag.doctor = doctor;
            }

            return RedirectToAction("AvailableTime",new { id = doc.Id , message = "Fill the form please"});
        }
        public async Task<IActionResult> Edit(string? message = null)
        {
            ViewBag.Message = message;
            var Staffid = HttpContext.Session.GetInt32("StaffId");
            var doctor = await _doctorRepository.GetDoctorByStaffID(Staffid.Value);
            return View(doctor);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Doctors model)
        {
            if (Regex.IsMatch(model.Staff.Name, @"^[a-zA-Z\s]+$") && Regex.IsMatch(model.Staff.UserName, @"^[a-zA-Z0-9\s]+$"))
            {
                var ExistingDoctor = _dbContext.Doctors.Include(D => D.Staff).FirstOrDefault(D => D.Id == model.Id);
                if (ExistingDoctor is not null)
                {
                    ExistingDoctor.Staff.Name = model.Staff.Name;
                    ExistingDoctor.Staff.UserName = model.Staff.UserName;
                    ExistingDoctor.Staff.Email = model.Staff.Email;
                    ExistingDoctor.Staff.Password = model.Staff.Password;
                    ExistingDoctor.Staff.Role = model.Staff.Role;
                    ExistingDoctor.Specialty = model.Specialty;
                    ExistingDoctor.Phone = model.Phone;
                    ExistingDoctor.OfficeAddress = model.OfficeAddress;
                    int Result = await _doctorRepository.CompleteAsync();
                    if (Result > 0)
                    {
                        return RedirectToAction(nameof(Index),new { message = "your data edit successfuly"});
                    }
                    else
                    {
                        return RedirectToAction("Edit", new { message = "Field update your data" });
                    }

                }
                else
                {
                    return RedirectToAction("Edit", new { message = "this Doctor not found" });
                }

            }
            else
            {
                return RedirectToAction("Edit", new { message = "don't use special chars in user name and user just letters in your name" });
            }
        }

        public async Task<IActionResult> Visits(string Search)
        {
            if (Search is not null)
            {
                var visits= await _visitRepository.GetAlltoPatientName(Search);
                var mapped = _mapper.Map<IEnumerable<Visit>, IEnumerable<VisitViewModel>>(visits);
                return View(mapped);
            }
            else
            {
                var Staffid = HttpContext.Session.GetInt32("StaffId");
                var Doctor = await _doctorRepository.GetDoctorByStaffID(Staffid.Value);
                var Visits = await _visitRepository.GetAllToDoctor(Doctor.Id);
                var mapped = _mapper.Map<IEnumerable<Visit>, IEnumerable<VisitViewModel>>(Visits);
                foreach (var visit in mapped)
                {
                    var patient = await _patientRepository.GetByPatientNameAsync(visit.Patient.Username);
                    visit.Patient = patient;  // Attach patient info directly to the visit
                }
                return View(mapped);
            }
           
        }
        ///EditV
        public async Task<IActionResult> EditV(int id)
        {
            var Visit = await _visitRepository.GetByIdAsync(id);
            if (Visit == null) { return NotFound(); }
            var map = new SimpleViewModel()
            {
                Id = Visit.Id,
                PatientUsername = Visit.Patient.Username,
                DoctorUserName = Visit.Doctor.Staff.UserName,
                Date = Visit.Date,
                Diagnosis = Visit.Diagnosis,
                FollowUpDate = Visit.FollowUpDate,
                TreatmentPlan = Visit.TreatmentPlan,
                ReasonForVisit = Visit.ReasonForVisit,
                VisitType = Visit.VisitType,
            };
            return View(map);
        }
        [HttpPost]
        public async Task<IActionResult> EditV(SimpleViewModel visit)
        {
            if (ModelState.IsValid)
            {
                var patient = await _patientRepository.GetByPatientNameAsync(visit.PatientUsername);
                var staff = await _userRepository.GetByUserNameAsync(visit.DoctorUserName);
                var doctor = await _doctorRepository.GetDoctorByStaffID(staff.Id);
                var rvisit = await _visitRepository.GetByIdAsync(visit.Id);
                if (rvisit is not null)
                {
                    rvisit.PatientId = patient.Id;
                    rvisit.Patient = patient;
                    rvisit.Doctor = doctor;
                    rvisit.Date = visit.Date;
                    rvisit.Diagnosis = visit.Diagnosis;
                    rvisit.FollowUpDate = visit.FollowUpDate;
                    rvisit.TreatmentPlan = visit.TreatmentPlan;
                    rvisit.ReasonForVisit = visit.ReasonForVisit;
                    rvisit.VisitType = visit.VisitType;
                }
                int Result = await _doctorRepository.CompleteAsync();
                if (Result > 0)
                {
                    return RedirectToAction(nameof(Visits));
                }

            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage); // Or use a logger
                }
            }
            return View(visit);
        }

        //Services
        public async Task<IActionResult> Services()
        {
           
            return View();
        }
        //LabTests
        public async Task<IActionResult> LabTests(string Search)
        {
            if (Search is not null)
            {
                var labTests = await _labRepository.GetLabByPatientNameAsync(Search);
                if (labTests.Count() > 0)
                {
                    var map = _mapper.Map<IEnumerable<Lab>,IEnumerable<LabViewModel>>(labTests);
                    return View(map);
                } 
                else
                {
                    TempData["Message"] = "No LabTest For This Patient";
                }
            }
            return View(new List<LabViewModel>());
        }

        //RequestTest
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
                var patient = await _patientRepository.GetByPatientNameAsync(model.PatientUserName);
                if (patient is not null)
                {
                    var map = _mapper.Map<SimpleLabViewModel, Lab>(model);
                    map.Patient = patient;
                    await _labRepository.AddAsync(map);
                    int Result = await _patientRepository.CompleteAsync();
                    if (Result > 0)
                    {
                        return RedirectToAction("LabTests","Doctor", new { search = patient.Username });
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

        //Prescriptions
        public async Task<IActionResult> Prescriptions(string Search)
        {
            if (Search is not null)
            {
                var Prescriptions = await _medicalPrescription.GetPrescriptionByPatientName(Search);
                if (Prescriptions.Count() > 0)
                {
                    var map = _mapper.Map<IEnumerable<MedicationPrescription>, IEnumerable<PrescriptionViewModel>>(Prescriptions);
                    foreach (var m in map)
                    {
                        var prescription = Prescriptions.FirstOrDefault(p => p.Id == m.Id); 
                        if (prescription != null)
                        {
                            m.DoctorUserName = prescription.Doctor.Staff.UserName; 
                        }
                    }
                    return View(map);
                }
                else
                {
                    TempData["Message"] = "No Prescriptions For This Patient";
                }
            }
            return View(new List<PrescriptionViewModel>());
        }
        public IActionResult AddPrescription()
        {
            return View(new PrescriptionViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> AddPrescription(PrescriptionViewModel model)
        {
            if (ModelState.IsValid) 
            {
                var Staffid = HttpContext.Session.GetInt32("StaffId");
                var doctor = await _doctorRepository.GetDoctorByStaffID(Staffid.Value);
                var patient = await _patientRepository.GetByPatientNameAsync(model.PatientUserName);
                if (doctor != null && patient !=null) 
                {
                    var map = _mapper.Map<PrescriptionViewModel,MedicationPrescription>(model);
                    map.Patient = patient;
                    map.Doctor = doctor;
                    await _medicalPrescription.AddAsync(map);
                    int Result =await _medicalPrescription.CompleteAsync();
                    if (Result > 0) 
                    {
                        return RedirectToAction("Prescriptions", "Doctor", new { search = patient.Username });
                    }
                }
            }

            return View(model);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
