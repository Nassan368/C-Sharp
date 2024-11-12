using HelpdeskDAL;
using System.Diagnostics;
using System.Reflection;

namespace HelpdeskViewModels
{
    public class EmployeeViewModel
    {
        readonly private EmployeeDAO _dao;

        public string? Title { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? Phoneno { get; set; }
        public string? Timer { get; set; }
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? Id { get; set; }

        public string? StaffPicture64 { get; set; }

        // Constructor
        public EmployeeViewModel()
        {
            _dao = new EmployeeDAO();
        }

        // Method to get an employee by email
        public async Task GetByEmail()
        {
            try
            {
                Employee? emp = await _dao.GetByEmail(Email!);
                if (emp == null)
                {
                    Debug.WriteLine("Employee not found.");
                    return;
                }

                // Populate ViewModel properties
                Title = emp.Title;
                Firstname = emp.FirstName;
                Lastname = emp.LastName;
                Email = emp.Email;
                Phoneno = emp.PhoneNo;
                DepartmentId = emp.DepartmentId;
                DepartmentName = emp.Department?.DepartmentName ?? "Unknown";
                Id = emp.Id;
                if (emp.StaffPicture != null)
                {
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                }
                Timer = emp.Timer != null ? Convert.ToBase64String(emp.Timer) : null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

        // Method to get an employee by ID
        public async Task GetById()
        {
            try
            {
                Employee? emp = await _dao.GetById(Id!.Value);
                if (emp == null)
                {
                    Debug.WriteLine("Employee not found.");
                    return;
                }

                // Populate ViewModel properties
                Title = emp.Title;
                Firstname = emp.FirstName;
                Lastname = emp.LastName;
                Email = emp.Email;
                Phoneno = emp.PhoneNo;
                DepartmentId = emp.DepartmentId;
                DepartmentName = emp.Department?.DepartmentName ?? "Unknown";
                Id = emp.Id;
                if (emp.StaffPicture != null)
                {
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                }
                Timer = emp.Timer != null ? Convert.ToBase64String(emp.Timer) : null;

            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Email = "not found";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

        // Method to get all employees
        public async Task<List<EmployeeViewModel>> GetAll()
        {
            List<EmployeeViewModel> allVms = new();

            try
            {
                List<Employee> allEmployees = await _dao.GetAll();

                foreach (Employee emp in allEmployees)
                {
                    EmployeeViewModel empVm = new()
                    {
                        Title = emp.Title,
                        Firstname = emp.FirstName,
                        Lastname = emp.LastName,
                        Phoneno = emp.PhoneNo,
                        Email = emp.Email,
                        Id = emp.Id,
                        DepartmentId = emp.DepartmentId,
                        DepartmentName = emp.Department?.DepartmentName ?? "Unknown",
                        Timer = Convert.ToBase64String(emp.Timer!)
                    };

                    allVms.Add(empVm);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name} {ex.Message}");
                throw;
            }

            return allVms;
        }




        // Add a new employee
        public async Task Add()
        {
            Id = -1;
            try
            {
                Employee emp = new()
                {
                    Title = Title,
                    FirstName = Firstname,
                    LastName = Lastname,
                    Email = Email,
                    PhoneNo = Phoneno,
                    DepartmentId = DepartmentId,
                    StaffPicture = StaffPicture64 != null ? Convert.FromBase64String(StaffPicture64!) : null,
                    //Timer = Timer != null ? Convert.FromBase64String(Timer!) : null
                };

                Id = await _dao.Add(emp);
            }
            catch (Exception ex)
            {
                // Log any errors encountered during the process
                Debug.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name} {ex.Message}");
                throw;
            }
        }


        // Update an employee's data
        public async Task<int> Update()
        {
            int updateStatus = -1;


            // Check if the Id is null or invalid before proceeding
            if (Id == null || Id <= 0)
            {
                Debug.WriteLine("Invalid employee Id for update.");
                return updateStatus;  // Return failure
            }
            try
            {
                Employee emp = new()
                {
                    Title = Title,
                    Id = (int)Id!,
                    FirstName = Firstname,
                    LastName = Lastname,
                    Email = Email,
                    PhoneNo = Phoneno,
                    DepartmentId = DepartmentId,
                    StaffPicture = StaffPicture64 != null ? Convert.FromBase64String(StaffPicture64!) : null,
                    Timer = Timer != null ? Convert.FromBase64String(Timer!) : throw new Exception("Timer is null")
                };

                //updateStatus = -1; // Start with a failure state
                updateStatus = Convert.ToInt16(await _dao.Update(emp));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }

            return updateStatus;
        }

        // Delete an employee
        public async Task<int> Delete()
        {
            try
            {
                return await _dao.Delete(Id!.Value);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }



        // Get an employee by phone number
        public async Task GetByPhoneNumber()
        {
            try
            {
                Employee? emp = await _dao.GetByPhoneNumber(Phoneno!);
                if (emp == null)
                {
                    Debug.WriteLine("Employee not found.");
                    return;
                }

                // Populate ViewModel properties
                Title = emp.Title;
                Firstname = emp.FirstName;
                Lastname = emp.LastName;
                Email = emp.Email;
                Phoneno = emp.PhoneNo;
                DepartmentId = emp.DepartmentId;
                DepartmentName = emp.Department?.DepartmentName ?? "Unknown";
                Id = emp.Id;

                // Convert StaffPicture to base64 string
                if (emp.StaffPicture != null)
                {
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                }

                // Convert Timer to base64 string
                Timer = Convert.ToBase64String(emp.Timer!);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Problem in {GetType().Name} {MethodBase.GetCurrentMethod()!.Name} {ex.Message}");
                throw;
            }
        }
    }
}

