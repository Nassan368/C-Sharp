using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpdeskDAL
{
    public class Problem : HelpdeskEntity
    {
       // public int Id { get; set; }
        public string Description { get; set; } = string.Empty; // Initialize with default value
        public int AssignedEmployeeId { get; set; }
        public virtual Employee AssignedEmployee { get; set; } = new Employee(); // Initialize with new Employee
    }

}
