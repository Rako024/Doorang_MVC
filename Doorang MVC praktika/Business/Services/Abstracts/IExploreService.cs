using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Abstracts
{
    public interface IExploreService
    {
        void CreateExplore(Explore explore);
        void DeleteExplore(int id);
        void UpdateExplore(int id, Explore explore);
        Explore GetExplore(Func<Explore,bool>? func = null);
        List<Explore> GetAllExplores(Func<Explore,bool>? func = null);

    }
}
