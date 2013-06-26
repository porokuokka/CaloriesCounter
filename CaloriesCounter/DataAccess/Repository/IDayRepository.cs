using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaloriesCounter.Models;

namespace CaloriesCounter.DataAccess.Repository
{
    public interface IDayRepository
    {
        Task SaveAsync(Day day);
        Task DeleteAsync(Day day);
        Task<List<Day>> GetAllAsync();
    }
}
