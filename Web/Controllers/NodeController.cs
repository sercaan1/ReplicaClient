﻿using Business.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class NodeController : Controller
    {
        private readonly INodeService _nodeService;

        public NodeController(INodeService nodeService)
        {
            _nodeService = nodeService;
        }
        public async Task<IActionResult> Details(string id)
        {
            var getNodeDetailsResponse = await _nodeService.GetByIdAsync(new Guid(id));
            if (getNodeDetailsResponse.IsSuccess)
            {
                return View(getNodeDetailsResponse.Data);
            }

            return NotFound();
        }
    }
}
