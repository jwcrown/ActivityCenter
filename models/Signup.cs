using System;
using System.Collections.Generic;

namespace BeltExam1.Models
{
    public class Signup : BaseEntity
    {
        public int SignupId { get; set; }
        public User Players { get; set; }
        public int UserId { get; set; }
        public int ActivityId { get; set; }
    }
}