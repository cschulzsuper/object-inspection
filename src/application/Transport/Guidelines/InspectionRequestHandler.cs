﻿using ChristianSchulz.ObjectInspection.Application.Guidelines.Requests;
using ChristianSchulz.ObjectInspection.Application.Guidelines.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Guidelines;

public class InspectionRequestHandler : IInspectionRequestHandler
{
    private readonly IInspectionManager _inspectionManager;
    private readonly IInspectionEventService _inspectionEventService;

    public InspectionRequestHandler(
        IInspectionManager inspectionManager,
        IInspectionEventService inspectionEventService)
    {
        _inspectionManager = inspectionManager;
        _inspectionEventService = inspectionEventService;
    }

    public IAsyncEnumerable<InspectionResponse> GetAll()
        => _inspectionManager
            .GetAsyncEnumerable(query => query
            .Select(entity => new InspectionResponse
            {
                Activated = entity.Activated,
                DisplayName = entity.DisplayName,
                Text = entity.Text,
                UniqueName = entity.UniqueName,
                ETag = entity.ETag
            }));

    public async ValueTask<InspectionResponse> GetAsync(string inspection)
    {
        var entity = await _inspectionManager.GetAsync(inspection);

        return new InspectionResponse
        {
            Activated = entity.Activated,
            DisplayName = entity.DisplayName,
            Text = entity.Text,
            UniqueName = entity.UniqueName,
            ETag = entity.ETag
        };
    }

    public async ValueTask<InspectionResponse> CreateAsync(InspectionRequest request)
    {
        var entity = new Inspection
        {
            Activated = request.Activated,
            DisplayName = request.DisplayName,
            Text = request.Text,
            UniqueName = request.UniqueName
        };

        await _inspectionManager.InsertAsync(entity);

        return new InspectionResponse
        {
            Activated = entity.Activated,
            DisplayName = entity.DisplayName,
            Text = entity.Text,
            UniqueName = entity.UniqueName,
            ETag = entity.ETag
        };
    }

    public async ValueTask ReplaceAsync(string inspection, InspectionRequest request)
    {
        var entity = await _inspectionManager.GetAsync(inspection);

        entity.Activated = request.Activated;
        entity.DisplayName = request.DisplayName;
        entity.Text = request.Text;
        entity.UniqueName = request.UniqueName;
        entity.ETag = request.ETag;

        await _inspectionManager.UpdateAsync(entity);
        await _inspectionEventService.CreateInspectionUpdateEventAsync(entity);
    }

    public async ValueTask DeleteAsync(string inspection, string etag)
    {
        var entity = await _inspectionManager.GetAsync(inspection);

        entity.ETag = etag;

        await _inspectionManager.DeleteAsync(entity);
        await _inspectionEventService.CreateInspectionDeletionEventAsync(entity.UniqueName);
    }

    public async ValueTask<ActivateInspectionResponse> ActivateAsync(string inspection, string etag)
    {
        var entity = await _inspectionManager.GetAsync(inspection);

        entity.Activated = true;
        entity.ETag = etag;

        await _inspectionManager.UpdateAsync(entity);
        await _inspectionEventService.CreateInspectionUpdateEventAsync(entity);

        return new ActivateInspectionResponse
        {
            ETag = entity.ETag
        };
    }

    public async ValueTask<DeactivateInspectionResponse> DeactivateAsync(string inspection, string etag)
    {
        var entity = await _inspectionManager.GetAsync(inspection);

        entity.Activated = false;
        entity.ETag = etag;

        await _inspectionManager.UpdateAsync(entity);
        await _inspectionEventService.CreateInspectionUpdateEventAsync(entity);

        return new DeactivateInspectionResponse
        {
            ETag = entity.ETag
        };

    }
}