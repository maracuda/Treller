using System.Collections.Generic;
using SKBKontur.Treller.WebApplication.Services.VirtualMachines.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Services.VirtualMachines.Storages
{
    public interface IVirtualMachinesStorage
    {
        VirtualMachine[] GetVirtualMachines(VirtualMachineProfile profile);
        Dictionary<VirtualMachineProfile, VirtualMachine[]> GetAllVirtualMachines();
    }
}