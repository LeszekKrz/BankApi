﻿using BankAPI.Models.GovernmentId;
using BankAPI.Models.JobTypes;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers;

[ApiController]
public sealed class DictionaryController : ControllerBase
{
    [HttpGet]
    [Route("job-types")]
    [ProducesResponseType(typeof(IReadOnlyList<JobTypeResponse>), StatusCodes.Status200OK)]
    public IReadOnlyList<JobTypeResponse> GetJobTypes()
    {
        return JobType.GetAll().Select(t => t.ToResponse()).ToList();
    }

    [HttpGet]
    [Route("government-id-types")]
    [ProducesResponseType(typeof(IReadOnlyList<GovernmentIdTypeResponse>), StatusCodes.Status200OK)]
    public IReadOnlyList<GovernmentIdTypeResponse> GetGovernmentIdTypes()
    {
        return GovernmentId.GetAllIdTypesAsResponses();
    }
}