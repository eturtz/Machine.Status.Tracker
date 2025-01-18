using Machine.Status.Tracker.Commands;
using Machine.Status.Tracker.DataTypes;
using Machine.Status.Tracker.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Machine.Status.Tracker.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly MachineStatusManager _statusManager;

        private MachineStatusEnum _newMachineStatus = MachineStatusEnum.Running;
        public ObservableCollection<MachineStatus> MachineStatuses { get; set; }
        public ObservableCollection<MachineStatusEnum> StatusOptions { get; set; }

        public string NewMachineName { get; set; }
        public string NewMachineDescription { get; set; }
        public string NewMachineNotes { get; set; }
        public MachineStatusEnum NewMachineStatus
        {
            get { return _newMachineStatus; }
            set
            {
                if (_newMachineStatus != value)
                {
                    _newMachineStatus = value;
                    OnPropertyChanged(nameof(NewMachineStatus));
                }
            }
        }

        public ICommand AddMachineCommand { get; set; }
        public ICommand EditMachineCommand { get; set; }
        public ICommand DeleteMachineCommand { get; set; }
        public ICommand FilterMachineCommand { get; set; }
        public ICommand ClearFilterCommand { get; set; }

        public MainViewModel()
        {
            _statusManager = new MachineStatusManager();
            MachineStatuses = new ObservableCollection<MachineStatus>(_statusManager.GetAllMachineStatuses());
            StatusOptions = new ObservableCollection<MachineStatusEnum>
            {
                MachineStatusEnum.Running,
                MachineStatusEnum.Idle,
                MachineStatusEnum.Offline
            };

            NewMachineStatus = MachineStatusEnum.Running;

            // Commands
            AddMachineCommand = new DelegateCommand(AddMachine);
            EditMachineCommand = new DelegateCommand(EditMachine, CanEditMachine);
            DeleteMachineCommand = new DelegateCommand(DeleteMachine, CanDeleteMachine);
            FilterMachineCommand = new DelegateCommand(FilterMachineStatus);
            ClearFilterCommand = new DelegateCommand(UpdateMachineList);
        }

        private void AddMachine()
        {
            var machine = new MachineStatus(NewMachineName, NewMachineDescription, NewMachineStatus, NewMachineNotes, null);
            machine.SetIcon();
            bool success = _statusManager.AddMachineStatus(machine.MachineName, machine.Description, machine.Status, machine.Notes);
            if (success)
            {
                UpdateMachineList();
                MessageBox.Show("Machine added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to add machine. Please check your inputs.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void EditMachine()
        {
            var machineToEdit = MachineStatuses.FirstOrDefault();
            bool success = _statusManager.UpdateMachineStatus(machineToEdit, NewMachineName, NewMachineDescription, NewMachineStatus, NewMachineNotes);
            if (success)
            {
                UpdateMachineList();
                MessageBox.Show("Machine updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to update machine. Please check your inputs.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void DeleteMachine()
        {
            var machineToDelete = MachineStatuses.FirstOrDefault();
            bool success = _statusManager.DeleteMachineStatus(machineToDelete);
            if (success)
            {
                UpdateMachineList(); 
                MessageBox.Show("Machine deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to delete machine. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterMachineStatus()
        {
            var filteredMachines = _statusManager.FilterMachinesByStatus(NewMachineStatus);
            MachineStatuses.Clear();
            foreach (var machine in filteredMachines)
            {
                MachineStatuses.Add(machine);
            }
        }

        private void UpdateMachineList()
        {
            var allMachines = _statusManager.GetAllMachineStatuses();
            MachineStatuses.Clear();
            foreach (var machine in allMachines)
            {
                MachineStatuses.Add(machine);
            }
        }

        // PropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Validation for editing machine (example, you can add more validation logic)
        private bool CanEditMachine()
        {
            return MachineStatuses.Any(); // Only allow edit if there's at least one machine
        }

        // Validation for deleting machine
        private bool CanDeleteMachine()
        {
            return MachineStatuses.Any(); // Only allow delete if there's at least one machine
        }
    }

}
