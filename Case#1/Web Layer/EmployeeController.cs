using HelpdeskViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;

namespace HelpdeskWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        // GET all employees
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                EmployeeViewModel viewmodel = new();
                List<EmployeeViewModel> allEmployees = await viewmodel.GetAll();
                return Ok(allEmployees);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }

        // PUT request to update employee information
        [HttpPut]
        public async Task<ActionResult> Put(EmployeeViewModel viewmodel)
        {
            try
            {
                int retVal = await viewmodel.Update();

                return retVal switch
                {
                    1 => Ok(new { msg = "Employee " + viewmodel.Lastname + " updated!" }),
                  
                    -2 => Ok(new { msg = "Data is stale for " + viewmodel.Lastname + ", Employee not updated!" }),
                    _ => Ok(new { msg = "Employee " + viewmodel.Lastname + " not updated!" })
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }

        // POST request to add a new employee
        [HttpPost]
        public async Task<ActionResult> Post(EmployeeViewModel viewmodel)
        {
            try
            {
                await viewmodel.Add();
                return viewmodel.Id > 1
                    ? Ok(new { msg = "Employee " + viewmodel.Lastname + " added!" })
                    : Ok(new { msg = "Employee " + viewmodel.Lastname + " not added!" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }

        // DELETE request to delete an employee by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                EmployeeViewModel viewmodel = new() { Id = id };
                return await viewmodel.Delete() == 1
                    ? Ok(new { msg = "Employee " + id + " deleted!" })
                    : Ok(new { msg = "Employee " + id + " not deleted!" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }
    }
}
