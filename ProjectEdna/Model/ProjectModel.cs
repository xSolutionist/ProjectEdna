using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectEdna.Model
{
    public class ProjectModel
    {
        public int Id { get; set; }

        [Display(Name = "Projekt Namn")]
        public string ProjectName { get; set; }

        [Display(Name = "Antal")]
        public int ProjectDataSize { get; set; }

        

        public string ProjectCreator { get; set; }

        public string UserId { get; set; }




        public DateTime ProjectCreated { get; set; }

       

    }
}
