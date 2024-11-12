using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpdeskDAL
{
    public class Department : HelpdeskEntity
    {
        //public int Id { get; set; }
        public string? DepartmentName { get; set; } // Initialize with default value
        //public byte[] Timer { get; set; } = null!;
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>(); // Initialize with empty list
    }

}
