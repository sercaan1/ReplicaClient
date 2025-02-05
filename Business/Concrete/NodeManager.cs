﻿using AutoMapper;
using Business.Abstracts;
using Common.Constants;
using Common.Models.Integration.Nodes;
using Common.Models.ViewModels.Nodes;
using Common.Utilities.Abstracts;
using Common.Utilities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class NodeManager : BaseManager, INodeService
    {
        public NodeManager(IHttpClientFactory httpClientFactory, IMapper mapper) : base(httpClientFactory, mapper)
        {

        }

        public async Task<IDataResult<NodeDetailsViewModel>> AddAsync(NodeCreateViewModel vm)
        {
            var postNodeRequestModel = _mapper.Map<PostNodeRequestModel>(vm);
            postNodeRequestModel.Hardware.NodeId = DummyNode.NodeId;
            postNodeRequestModel.SocialMedia.NodeId = DummyNode.NodeId;
            postNodeRequestModel.Review.NodeId = DummyNode.NodeId;
            postNodeRequestModel.Guide.NodeId = DummyNode.NodeId;

            var jsonContent = JsonSerializer.Serialize(postNodeRequestModel);

            var request = new HttpRequestMessage(HttpMethod.Post, "Node");
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStreamAsync();
                var createdNode = await JsonSerializer.DeserializeAsync<GetNodeDetailsReponseModel>(content, options);

                return new SuccessDataResult<NodeDetailsViewModel>(_mapper.Map<NodeDetailsViewModel>(createdNode), Messages.AddSuccessfully);
            }

            return new ErrorDataResult<NodeDetailsViewModel>(Messages.AddFail);
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, "Node/" + id);
            var deleteResponseMessage = await httpClient.SendAsync(request);

            if (deleteResponseMessage.IsSuccessStatusCode)
                return new SuccessResult(Messages.DeleteSuccessfully);

            return new ErrorResult(Messages.DeleteFail);
        }

        public async Task<IDataResult<List<NodeIndexViewModel>>> GetAllAsync()
        {
            var httpResponseMessage = await httpClient.GetAsync("Node/AllNodes");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                var getNodeResponseModels = await JsonSerializer.DeserializeAsync<List<GetNodeListResponseModel>>(contentStream, options);

                return new SuccessDataResult<List<NodeIndexViewModel>>(_mapper.Map<List<NodeIndexViewModel>>(getNodeResponseModels), Messages.ListedSuccessfully);
            }

            return new ErrorDataResult<List<NodeIndexViewModel>>(Messages.NotFound);
        }

        public async Task<IDataResult<NodeDetailsViewModel>> GetByIdAsync(Guid id)
        {
            var httpResponseMessage = await httpClient.GetAsync("Node/" + id);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                var getNodeDetailsResponseModel = await JsonSerializer.DeserializeAsync<GetNodeDetailsReponseModel>(contentStream, options);

                return new SuccessDataResult<NodeDetailsViewModel>(_mapper.Map<NodeDetailsViewModel>(getNodeDetailsResponseModel), Messages.ListedSuccessfully);
            }

            return new ErrorDataResult<NodeDetailsViewModel>(Messages.NotFound);
        }

        public async Task<IDataResult<NodeDetailsViewModel>> UpdateAsync(NodeUpdateViewModel vm)
        {
            var putNodeRequestModel = _mapper.Map<PutNodeRequestModel>(vm);
            putNodeRequestModel.Hardware.NodeId = vm.Id;
            putNodeRequestModel.SocialMedia.NodeId = vm.Id;
            putNodeRequestModel.Review.NodeId = vm.Id;
            putNodeRequestModel.Guide.NodeId = vm.Id;

            var jsonContent = JsonSerializer.Serialize(putNodeRequestModel);

            var request = new HttpRequestMessage(HttpMethod.Put, "Node");

            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStreamAsync();
                var updatedNode = await JsonSerializer.DeserializeAsync<GetNodeDetailsReponseModel>(content, options);

                return new SuccessDataResult<NodeDetailsViewModel>(_mapper.Map<NodeDetailsViewModel>(updatedNode), Messages.UpdatedSuccessfully);
            }

            return new ErrorDataResult<NodeDetailsViewModel>(Messages.UpdateFail);
        }
    }
}
