﻿using Common.Models.Integration.Nodes;
using Common.Models.ViewModels.Nodes;
using Common.Utilities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstracts
{
    public interface INodeService
    {
        Task<IDataResult<List<NodeIndexViewModel>>> GetAllAsync();
        Task<IDataResult<List<NodeIndexViewModel>>> GetActiveNodesAsync();
        Task<IDataResult<List<NodeIndexViewModel>>> GetDoneNodesAsync();
        Task<IDataResult<GetNodeResponseModel>> GetById(Guid id);
    }
}
