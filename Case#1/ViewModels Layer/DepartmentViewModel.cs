
using HelpdeskDAL;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace ExercisesViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        public string? DepartmentName { get; set; }
        //public string? Timer { get; set; }

         readonly DepartmentDAO _dao;

        public DepartmentViewModel()
        {
            _dao = new DepartmentDAO();
        }

        public async Task<List<DepartmentViewModel>> GetAll()
        {
            List<DepartmentViewModel> allDepartments = new();
            try
            {
                List<Department> departments = await _dao.GetAll();
                foreach (Department department in departments)
                {
                    allDepartments.Add(new DepartmentViewModel
                    {
                        Id = department.Id,
                        DepartmentName = department.DepartmentName,
                        //Timer = Convert.ToBase64String(department.Timer!)
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name} {ex.Message}");
                throw;
            }
            return allDepartments;
        }
    }
}
