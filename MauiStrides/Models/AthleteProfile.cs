using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MauiStrides.Models
{
    public class AthleteProfile 
    {
     
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Bio { get; set; }
        public string City { get; set; }
        
        public string State { get; set; }
    
        public string Country { get; set; }

        public string Sex { get; set; }

       
        public decimal Weight { get; set; }

        public string ProfileMedium { get; set; } 

        public string Profile { get; set; } 
        public string Club { get; set; }

    }
}
