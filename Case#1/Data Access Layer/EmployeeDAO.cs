using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;

namespace HelpdeskDAL
{
    public class EmployeeDAO
    {
        readonly IRepository<Employee> _repo;

        public EmployeeDAO()
        {
            _repo = new HelpdeskRepository<Employee>();
        }

        // Method to retrieve an employee by email using the repository
        public async Task<Employee?> GetByEmail(string email)
        {
            try
            {
                return await _repo.GetOne(emp => emp.Email == email); // Using repository to get by email
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
                throw;
            }
        }


        public async Task<Employee?> GetById(int id)
        {
            try
            {
                return await _repo.GetOne(em => em.Id == id); // Using repository to fetch by ID
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }


        // Method to retrieve an employee by Id using the repository
        //public async Task<Employee?> GetById(int id)
        //{
        //    try
        //    {
        //        return await _repo.GetOne(id); // Using repository to fetch by ID
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
        //        throw;
        //    }
        //}

        // Method to get all employees using the repository
        public async Task<List<Employee>> GetAll()
        {
            try
            {
                return await _repo.GetAll(); // Using repository to get all employees
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
                throw;
            }
        }

        // Method to add a new employee using the repository
        public async Task<int> Add(Employee newEmployee)
        {
            try
            {
                await _repo.Add(newEmployee); // Using repository to add a new employee
                return newEmployee.Id; // Return the Id of the added employee
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
                throw;
            }
        }

        // Method to update an employee using the repository
        public async Task<UpdateStatus> Update(Employee updatedEmployee)
        {
            UpdateStatus status = UpdateStatus.Failed;
            try
            {
                status = await _repo.Update(updatedEmployee); // Using repository to update the employee
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
                throw;
            }
            return status;
        }

        // Method to delete an employee using the repository
        public async Task<int> Delete(int id)
        {
            try
            {
                return await _repo.Delete(id); // Using repository to delete by Id
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
                throw;
            }
        }

        // Method to retrieve an employee by phone number
        public async Task<Employee?> GetByPhoneNumber(string phone)
        {
            try
            {
                return await _repo.GetOne(emp => emp.PhoneNo == phone); // Using repository to get by phone number
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
                throw;
            }
        }
    }
}
