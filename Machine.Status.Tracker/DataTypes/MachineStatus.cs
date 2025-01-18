using Machine.Status.Tracker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.Status.Tracker.DataTypes
{
    public class MachineStatus
    {
        public string MachineName { get; set; }
        public string Description { get; set; }
        public MachineStatusEnum Status { get; set; }
        public string Notes { get; set; }
        public string Icon { get; set; }

        // Constructor
        public MachineStatus(string machineName, string description, MachineStatusEnum status, string notes, string icon)
        {
            MachineName = machineName;
            Description = description;
            Status = status;
            Notes = notes;
            Icon = icon;
        }

        public void SetIcon()
        {
            switch (Status)
            {
                case MachineStatusEnum.Running:
                    Icon = "pack://application:,,,/running_icon.png";  
                    break;
                case MachineStatusEnum.Idle:
                    Icon = "pack://application:,,,/idle_icon.png";  
                    break;
                case MachineStatusEnum.Offline:
                    Icon = "pack://application:,,,/offline_icon.png";  
                    break;
            }
        }
    }
}
