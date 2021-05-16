using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaiLapuDrug.Web.Data.Entities
{
    public class ContactRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Body { get; set; }

        public bool IsNew { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
