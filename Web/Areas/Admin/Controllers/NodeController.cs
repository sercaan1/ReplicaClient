﻿using AutoMapper;
using Business.Abstracts;
using Common.Models.ViewModels.Nodes;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Web.Attributes;
using Web.Extensions;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [RedirectToLoginIfNotAuthorized]
    public class NodeController : Controller
    {
        private readonly INodeService _nodeService;
        private readonly IMapper _mapper;

        public NodeController(INodeService nodeService, IMapper mapper)
        {
            _nodeService = nodeService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NodeCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.ImageFile != null)
                {
                    vm.Image = await vm.ImageFile.GetAsBase64String();
                }
                var createNodeResponse = await _nodeService.AddAsync(vm);

                if (createNodeResponse.IsSuccess)
                {
                    return RedirectToAction("Index", "Home");
                }
                return BadRequest(createNodeResponse.Message);
            }
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> ActiveNodes()
        {
            var getNodesResponse = await _nodeService.GetAllAsync();

            if (getNodesResponse.IsSuccess)
            {
                var nodesList = _mapper.Map<List<NodeListViewModel>>(getNodesResponse.Data);
                return View(nodesList.Where(x => x.EndDate > DateTime.Now && x.StartDate < DateTime.Now).ToList());
            }

            return NotFound(getNodesResponse.Message);
        }

        [HttpGet]
        public async Task<IActionResult> DoneNodes()
        {
            var getNodesResponse = await _nodeService.GetAllAsync();

            if (getNodesResponse.IsSuccess)
            {
                var nodesList = _mapper.Map<List<NodeListViewModel>>(getNodesResponse.Data);
                return View(nodesList.Where(x => x.EndDate < DateTime.Now).ToList());
            }

            return NotFound(getNodesResponse.Message);
        }

        [HttpGet]
        public async Task<IActionResult> UpcomingNodes()
        {
            var getNodesResponse = await _nodeService.GetAllAsync();

            if (getNodesResponse.IsSuccess)
            {
                var nodesList = _mapper.Map<List<NodeListViewModel>>(getNodesResponse.Data);
                return View(nodesList.Where(x => x.StartDate > DateTime.Now).ToList());
            }

            return NotFound(getNodesResponse.Message);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var getNodeDetailsResponse = await _nodeService.GetByIdAsync(new Guid(id));
            if (getNodeDetailsResponse.IsSuccess)
            {
                return View(getNodeDetailsResponse.Data);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var deleteNodeResponse = await _nodeService.DeleteAsync(new Guid(id));

            if (deleteNodeResponse.IsSuccess)
            {
                return RedirectToAction("Index", "Home");
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var getNodeResponse = await _nodeService.GetByIdAsync(new Guid(id));

            if (getNodeResponse.IsSuccess)
            {
                return View(_mapper.Map<NodeUpdateViewModel>(getNodeResponse.Data));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Update(NodeUpdateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.ImageFile != null)
                {
                    vm.Image = await vm.ImageFile.GetAsBase64String();
                }
                var updateNodeResponse = await _nodeService.UpdateAsync(vm);

                if (updateNodeResponse.IsSuccess)
                {
                    return RedirectToAction("Details", "Node", new { id = vm.Id });
                }

                return NotFound(updateNodeResponse.Message);
            }
            return View(vm);
        }
    }
}
