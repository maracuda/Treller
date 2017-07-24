using System.Collections.Generic;
using WebApplication.Implementation.VirtualMachines.BusinessObjects;

namespace WebApplication.Implementation.VirtualMachines
{
    public interface IVirtualMachinesStorage
    {
        VirtualMachine[] GetVirtualMachines(VirtualMachineProfile profile);
        Dictionary<VirtualMachineProfile, VirtualMachine[]> GetAllVirtualMachines();
    }
}