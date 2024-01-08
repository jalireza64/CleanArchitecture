using Microsoft.AspNetCore.Mvc;
using Sepid.Robot.Application.Managers.Device.Abstracts;
using Sepid.Robot.Application.Managers.Device.Model;
using Sepid.Robot.Domain.Entities;
using Sepid.Robot.ManagerWebApi.Base;

namespace Sepid.Robot.ManagerWebApi.Controllers
{
    [ApiVersion(SepidApiVersion.Version1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceManager _manager;
        private readonly ILogger<DeviceController> _logger;

        public DeviceController(ILogger<DeviceController> logger, IDeviceManager manager)
        {
            _logger = logger;
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
        }

        [HttpGet(nameof(GetList), Name = nameof(DeviceController) + nameof(GetList))]
        public async Task<IActionResult> GetList()
        {
            var data = await _manager.GetListAsync();
            return Ok(data);
        }

        [HttpPost(nameof(Create), Name = nameof(DeviceController) + nameof(Create))]
        public async Task<IActionResult> Create([FromBody] CreateDeviceModel model)
        {
            (bool success, Device data) = await _manager.CreateAsync(model);
            if (success) return Created(string.Empty, data);
            return BadRequest();
        }
    }
}