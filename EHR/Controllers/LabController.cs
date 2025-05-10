using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Repository;
using DataAccess.Models;
using EHR.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Text.RegularExpressions;

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
        public async Task<IActionResult> Index(string? message = null)
        {
            var Staffid= HttpContext.Session.GetInt32("StaffId");
            var Laboratory=await _staffRepository.GetByIdAsync(Staffid.Value);
            var mapped= _mapper.Map<Staff,StaffViewModel>(Laboratory);
            ViewBag.Message = message;
            return View(mapped);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id = 0 ,string? message = null)
        {
            if (Id == 0) 
            {
                var Staffid = HttpContext.Session.GetInt32("StaffId");
                var Laboratory = await _staffRepository.GetByIdAsync(Staffid.Value);
                ViewBag.Message = message;
                return View(Laboratory);
            }
            else
            {
                var Laboratory = await _staffRepository.GetByIdAsync(Id);
                ViewBag.Message = message;
                return View(Laboratory);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Staff model)
        {
            if (ModelState.IsValid) 
            {
                var Id = HttpContext.Session.GetInt32("StaffId");
                if (Regex.IsMatch(model.UserName, @"^[a-zA-Z0-9\s]+$"))
                {
                    _staffRepository.Update(model);
                    int Result = await _staffRepository.CompleteAsync();
                    if (Result > 0)
                    {
                        return RedirectToAction(nameof(Index), new { message = "your data is Updated successfuly" });
                    }
                }
                else
                {
                    return RedirectToAction(nameof(Edit), new { Id = model.Id, message = "user name should be only letters" });
                }

            }
            return RedirectToAction(nameof(Edit), new { Id = model.Id, message = "Update Failed,please fill the from" });
        }
        public async Task<IActionResult> LabTests(string? message = null)
        {
            var labtests=await _labtestRepository.GetAllAsync();
            var mapped = _mapper.Map<IEnumerable<Lab>, IEnumerable<LabViewModel>>(labtests);
            ViewBag.Message = message;
            return View(mapped);
        }
        public async Task<IActionResult> EditTest(int id,string? message = null) 
        {
            var labtest =await _labtestRepository.GetByIdAsync(id);
            var mapped = _mapper.Map<Lab, SimpleLabViewModel>(labtest);
            mapped.PatientUserName =labtest.Patient.Username;
            ViewBag.Message = message;
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
                        return RedirectToAction(nameof(LabTests), new { message = "Updated successfuly"});
                    }
                }
            }
            else
            {
                return RedirectToAction(nameof(EditTest), new { id = model.Id, message = "Update Failed,please fill the from" });
            }
            return RedirectToAction(nameof(EditTest), new { id = model.Id, message = "Update Failed,please fill the from" });
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
