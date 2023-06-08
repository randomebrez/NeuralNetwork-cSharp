using DataBase.Abstraction.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataBase.Abstraction
{
    public interface IDataBaseStorage
    {
        Task<int> AddNewSimulationAsync();

        Task StoreBrainsAsync(List<BrainDb> brains);
    }
}