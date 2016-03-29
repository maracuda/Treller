using System.Collections.Generic;
using System.Linq;
using SKBKontur.Treller.WebApplication.Implementation.VirtualMachines.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Implementation.VirtualMachines.Storages
{
    public class VirtualMachinesStorage : IVirtualMachinesStorage
    {
        private readonly Dictionary<VirtualMachineProfile, VirtualMachine[]> machinesByProfile;
        private static readonly VirtualMachine[] AvailableMachines =
        {
            new VirtualMachine { Name = "vm-billy-3n1", Profile = VirtualMachineProfile.ThreeNode },
            new VirtualMachine { Name = "vm-billy-3n2", Profile = VirtualMachineProfile.ThreeNode },
            new VirtualMachine { Name = "vm-billy-3n3", Profile = VirtualMachineProfile.ThreeNode },

            new VirtualMachine { Name = "vm-billy-test7", Profile = VirtualMachineProfile.Stand },
            new VirtualMachine { Name = "vm-billy-test11", Profile = VirtualMachineProfile.Stand },
            new VirtualMachine { Name = "vm-billy-test12", Profile = VirtualMachineProfile.Stand },
            new VirtualMachine { Name = "vm-billy-test14", Profile = VirtualMachineProfile.Stand },
            new VirtualMachine { Name = "vm-billy-test15", Profile = VirtualMachineProfile.Stand },
            new VirtualMachine { Name = "vm-billy-test17", Profile = VirtualMachineProfile.Stand },
            new VirtualMachine { Name = "vm-billy-test20", Profile = VirtualMachineProfile.Stand },

            new VirtualMachine { Name = "vm-billy-test1", Profile = VirtualMachineProfile.Ci },
            new VirtualMachine { Name = "vm-billy-test2", Profile = VirtualMachineProfile.Ci },
            new VirtualMachine { Name = "vm-billy-test3", Profile = VirtualMachineProfile.Ci },
            new VirtualMachine { Name = "vm-billy-test4", Profile = VirtualMachineProfile.Ci },
            new VirtualMachine { Name = "vm-billy-test6", Profile = VirtualMachineProfile.Ci },

            new VirtualMachine { Name = "vm-billy-test5", Profile = VirtualMachineProfile.FunctionalCi },

            new VirtualMachine { Name = "vm-billy-build", Profile = VirtualMachineProfile.BuildServer },

            new VirtualMachine { Name = "vm-bjava-3n1", Profile = VirtualMachineProfile.ThreeNodeJava },
            new VirtualMachine { Name = "vm-bjava-3n2", Profile = VirtualMachineProfile.ThreeNodeJava },
            new VirtualMachine { Name = "vm-bjava-3n3", Profile = VirtualMachineProfile.ThreeNodeJava },
        };

        public VirtualMachinesStorage()
        {
            machinesByProfile = AvailableMachines.GroupBy(x => x.Profile).ToDictionary(x => x.Key, x => x.ToArray());
        }

        public VirtualMachine[] GetVirtualMachines(VirtualMachineProfile profile)
        {
            return machinesByProfile[profile];
        }

        public Dictionary<VirtualMachineProfile, VirtualMachine[]> GetAllVirtualMachines()
        {
            return machinesByProfile;
        }
    }
}