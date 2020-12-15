using AutoMapper;
using Leav_management.Contracts;
using Leav_management.Data;
using Leav_management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leav_management.Controllers
{
    [Authorize]
    public class LeaveHistoryController : Controller
    {
        private readonly ILeaveHistoryRepository _historyRepository;
        private readonly ILeaveTypeRepository _typeRepository;
        private readonly ILeaveAllocationRepository _allocationRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;

        public LeaveHistoryController(
            ILeaveHistoryRepository historyRepository,
            ILeaveTypeRepository typeRepository,
            ILeaveAllocationRepository allocationRepository,
            IMapper mapper,
            UserManager<Employee> userManager)
        {
            _historyRepository = historyRepository;
            _typeRepository = typeRepository;
            _allocationRepository = allocationRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        [Authorize (Roles = "Administrator")]
        // GET: LeaveHistoryController
        public async Task<ActionResult> Index()
        {
            var leaveHistory = await _historyRepository.FindAll();
            var leaveHistoryModels = _mapper.Map<List<LeaveHistoryVM>>(leaveHistory);
            var model = new AdminLeaveHistoryViewVM
            {
                TotalRequests = leaveHistoryModels.Count,
                ApprovedRequests = leaveHistoryModels.Count(q => q.Approved == true),
                PendingRequests = leaveHistoryModels.Count(q => q.Approved == null),
                RejectedRequests = leaveHistoryModels.Count(q => q.Approved == false),
                leaveHistories = leaveHistoryModels
            };

            return View(model);
        }

        // GET: LeaveHistoryController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var leaveRequest = await _historyRepository.FindById(id);
            var model = _mapper.Map<LeaveHistoryVM>(leaveRequest);

            return View(model);
        }

        // GET: LeaveHistoryController/Create
        public async Task<ActionResult> Create()
        {
            var leaveTypes = await _typeRepository.FindAll();
            var leaveTypeItoms = leaveTypes.Select(q => new SelectListItem { 
                Text = q.Name,
                Value = q.Id.ToString()
            });

            var model = new CreateLeaveHistoryVM
            {
                LeaveTypes = leaveTypeItoms
            };

            return View(model);
        }

        // POST: LeaveHistoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateLeaveHistoryVM model)
        {
            
            try
            {
                var leaveTypes = await _typeRepository.FindAll();
                var leaveTypeItoms = leaveTypes.Select(q => new SelectListItem
                {
                    Text = q.Name,
                    Value = q.Id.ToString()
                });
                model.LeaveTypes = leaveTypeItoms;

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (DateTime.Compare(model.StartDate, model.EndDate) > 1)
                {
                    ModelState.AddModelError("", "Start date is a later date than the end date");
                    return View(model);
                }

                var employee = await _userManager.GetUserAsync(User);
                var allocation = await _allocationRepository.GetLeaveAllocationsByEmployeeAndType(employee.Id, model.LeaveTypeId);
                int dayesRequested = (int)(model.EndDate.Date - model.StartDate.Date).TotalDays;
                
                if (dayesRequested > allocation.NumberOfDayes)
                {
                    ModelState.AddModelError("", "You dont have enough dayes for this request");
                    return View(model);
                }

                var leaveRequestModel = new LeaveHistoryVM
                {
                    RequestingEmployeeId = employee.Id,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Approved = null,
                    LeaveTypeId = model.LeaveTypeId,
                    DateRequested = DateTime.Now,
                    DateActiond = DateTime.Now
                };

                var leaveRequest = _mapper.Map<LeaveHistory>(leaveRequestModel);
                var isSuccess = await _historyRepository.Create(leaveRequest);

                if (!isSuccess)
                {
                    ModelState.AddModelError("", "Something went wrong submitting your requst");
                    return View(model);
                }

                return RedirectToAction("MyLeave");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something fucked up");
                return View();
            }
        }

        // GET: LeaveHistoryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LeaveHistoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaveHistoryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveHistoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> ApproveRequest(int id)
        {
            try
            {
                var leaveRequest = await _historyRepository.FindById(id);
                var allocation = await _allocationRepository.GetLeaveAllocationsByEmployeeAndType(leaveRequest.RequestingEmployeeId, leaveRequest.LeaveTypeId);

                int dayesRequested = (int)(leaveRequest.EndDate.Date - leaveRequest.StartDate.Date).TotalDays;

                allocation.NumberOfDayes -= dayesRequested;

                leaveRequest.Approved = true;
                leaveRequest.ApprovedById = _userManager.GetUserAsync(User).Result.Id;
                leaveRequest.DateActiond = DateTime.Now;

                await _historyRepository.Update(leaveRequest);
                await _allocationRepository.Update(allocation);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<ActionResult> RejectRequest(int id)
        {
            try
            {
                var leaveRequest = await _historyRepository.FindById(id);
                leaveRequest.Approved = false;
                leaveRequest.ApprovedById = _userManager.GetUserAsync(User).Result.Id;
                leaveRequest.DateActiond = DateTime.Now;

                await _historyRepository.Update(leaveRequest);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<ActionResult> MyLeave()
        {
            var employee = await _userManager.GetUserAsync(User);
            var employeeId = employee.Id;
            var employeeAllocations = await _allocationRepository.GetLeaveAllocationsByEmployee(employeeId);
            var employeeHistorys = await _historyRepository.GetLeaveHistoriesByEmployee(employeeId);

            var employeeAllocationsModel = _mapper.Map<List<LeaveAllocationVM>>(employeeAllocations);
            var employeeHistorysModel = _mapper.Map<List<LeaveHistoryVM>>(employeeHistorys);

            var model = new EmployeeLeaveRequestViewVM
            {
                LeaveAllocations = employeeAllocationsModel,
                LeaveHistories = employeeHistorysModel
            };

            return View(model);
        }
    }
}
