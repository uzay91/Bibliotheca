using Business.Abstract;
using Core.Concrete.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bibliotheca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationClaimsController : ControllerBase
    {
        private readonly IOperationClaimService _operationClaimService;
        public OperationClaimsController(IOperationClaimService operationClaimService)
        {
            _operationClaimService = operationClaimService;
        }

        [HttpGet()]
        public async Task<IActionResult> List()
        {
            var result = await _operationClaimService.List();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("Role")]
        public async Task<IActionResult> GetByRole(string role)
        {
            var result = await _operationClaimService.GetByRole(role);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPost()]
        public async Task<IActionResult> Add(OperationClaim operationClaim)
        {
            var result = await _operationClaimService.Add(operationClaim);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.Status, result);
        }

        [HttpDelete()]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _operationClaimService.Delete(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

    }
}
