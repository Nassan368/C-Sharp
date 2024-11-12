using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HelpdeskDAL
{

    public partial class Employee : HelpdeskEntity
    {
       // public int Id { get; set; }
        public string? Title { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNo { get; set; }
        public int DepartmentId { get; set; }

        public byte[]? StaffPicture { get; set; }  // Store employee picture as byte array
       // public byte[] Timer { get; set; } = null!;         // Store a timestamp or binary data
        public virtual Department? Department { get; set; }

        // Add these properties if needed
       
    }


}
