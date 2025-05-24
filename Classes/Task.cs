using donely_Inspilab.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace donely_Inspilab.Classes
{
    public class Task
    //Deze dient voor de definitie van een task, wat het juist is
    {
        // Core props
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Reward {  get; set; }
        public TaskFrequency Frequency { get; set; }
        public bool RequiresValidation { get; set; }
        public bool IsActive { get; set; }
        public int GroupId { get; private set; }

        
        public string NameDisplay => Frequency != TaskFrequency.Once ? $"[{Frequency.ToString().ToUpper()}] {Name}" : Name; //Dient voor Task Listviews --> [DAILY] [WEEKLY] [MONTHLY]
        public string ActiveDisplay => Frequency == TaskFrequency.Once ? "" : (IsActive ? "Active" : "Inactive"); //Dient voor formatting in kolom TaskLibrary --> onetime tasks hebben geen (in)active nodig
        


        //Creation Constructor (zonder id)
        public Task(string _name, string _description, int _reward, TaskFrequency _frequency, bool _requiresValidation, int _groupId, bool _IsActive = true)
        {
            Name = _name;
            Description = _description;
            Reward = _reward;
            Frequency = _frequency;
            RequiresValidation = _requiresValidation;
            IsActive = _IsActive;
            GroupId = _groupId;
        }
        // Full Constructor
        public Task(int _id, string _name, string _description, int _reward, TaskFrequency _frequency, bool _requiresValidation, bool _IsActive, int _groupId)
        : this(_name, _description, _reward, _frequency, _requiresValidation, _groupId, _IsActive)
        {
            Id = _id;
        }

        public void ActivateTask()
        {
            this.IsActive = true;
        }
        public void DeactivateTask()
        {
            this.IsActive = false;
        }
        
    }
}
