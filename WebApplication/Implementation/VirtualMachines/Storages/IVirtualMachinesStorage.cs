using System.Collections.Generic;
using SKBKontur.Treller.WebApplication.Implementation.VirtualMachines.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Implementation.VirtualMachines.Storages
{
    public interface IVirtualMachinesStorage
    {
        VirtualMachine[] GetVirtualMachines(VirtualMachineProfile profile);
        Dictionary<VirtualMachineProfile, VirtualMachine[]> GetAllVirtualMachines();
    }
}