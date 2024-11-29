using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Repository;
using DataAccess.Models;
using EHR.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace EHR.Controllers
{
    public class LabController : Controller
    {
        private readonly IGenericRepository<Staff> _staffRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IGenericRepository<Lab> _labtestRepository;
        private readonly IMapper _mapper;

        public LabController(IGenericRepository<Staff> staffRepository,IPatientRepository patientRepository,IGenericRepository<Lab> labtestRepository, IMapper mapper)
        {
            _staffRepository = staffRepository;
            _patientRepository = patientRepository;
            _labtestRepository = labtestRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var Staffid= HttpContext.Session.GetInt32("StaffId");
            var Laboratory=await _staffRepository.GetByIdAsync(Staffid.Value);
            var mapped= _mapper.Map<Staff,StaffViewModel>(Laboratory);
            return View(mapped);
        }
        public async Task<IActionResult> Edit()
        {
            var Staffid = HttpContext.Session.GetInt32("StaffId");
            var Laboratory = await _staffRepository.GetByIdAsync(Staffid.Value);
            return View(Laboratory);
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
        public async Task<IActionResult> LabTests()
        {
            var labtests=await _labtestRepository.GetAllAsync();
            var mapped = _mapper.Map<IEnumerable<Lab>, IEnumerable<LabViewModel>>(labtests);
            return View(mapped);
        }
        public async Task<IActionResult> EditTest(int id) 
        {
            var labtest =await _labtestRepository.GetByIdAsync(id);
            var mapped = _mapper.Map<Lab, SimpleLabViewModel>(labtest);
            mapped.PatientUserName =labtest.Patient.Username;
            return View(mapped);
        }
        [HttpPost]
        public async Task<IActionResult> EditTest(SimpleLabViewModel model)
        {
             
            if (ModelState.IsValid)
            {
                var patient = await _patientRepository.GetByPatientNameAsync(model.PatientUserName);
                if (patient is not null)
                {
                    var mapped = _mapper.Map<SimpleLabViewModel, Lab>(model);
                    mapped.Patient=patient;
                    _labtestRepository.Update(mapped);
                    int Result = await _labtestRepository.CompleteAsync();
                    if (Result > 0)
                    {
                        return RedirectToAction(nameof(LabTests));
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
            return View(model);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
