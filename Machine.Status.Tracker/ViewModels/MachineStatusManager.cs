using Machine.Status.Tracker.DataTypes;
using Machine.Status.Tracker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.Status.Tracker.ViewModels
{
    public class MachineStatusManager
    {
        private List<MachineStatus> _machineStatuses;

        public MachineStatusManager()
        {
            _machineStatuses = new List<MachineStatus>();
        }

        // Add a new machine status
        public bool AddMachineStatus(string machineName, string description, MachineStatusEnum status, string notes)
        {
            if (string.IsNullOrEmpty(machineName))
                return false; // Validation failure (machine name is required)

            var machine = new MachineStatus(machineName, description, status, notes, GetIconForStatus(status));
            _machineStatuses.Add(machine);
            return true;
        }

        // Update an existing machine status
        public bool UpdateMachineStatus(MachineStatus machine, string machineName, string description, MachineStatusEnum status, string notes)
        {
            if (string.IsNullOrEmpty(machineName))
                return false; // Validation failure (machine name is required)

            machine.MachineName = machineName;
            machine.Description = description;
            machine.Status = status;
            machine.Notes = notes;
            machine.Icon = GetIconForStatus(status);
            return true;
        }

        // Delete a machine status
        public bool DeleteMachineStatus(MachineStatus machine)
        {
            return _machineStatuses.Remove(machine);
        }

        // Filter machines based on status
        public List<MachineStatus> FilterMachinesByStatus(MachineStatusEnum status)
        {
            return _machineStatuses.Where(m => m.Status == status).ToList();
        }

        // Helper to get icon based on status
        private string GetIconForStatus(MachineStatusEnum status)
        {
            return status switch
            {
                MachineStatusEnum.Running => "running_icon.png",
                MachineStatusEnum.Idle => "idle_icon.png",
                MachineStatusEnum.Offline => "offline_icon.png",
                _ => "default_icon.png"
            };
        }

        // Get all machines
        public List<MachineStatus> GetAllMachineStatuses()
        {
            return _machineStatuses;
        }
    }
}
