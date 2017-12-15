using System;
using System.Collections.Generic;

namespace BeltExam1.Models
{
    public class Activity : BaseEntity
    {
        public int ActivityId { get; set; }
        public User Creator { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
        public decimal Duration { get; set; }
        public string Description { get; set; }
        public List<Signup> Players { get; set; }

    }
}