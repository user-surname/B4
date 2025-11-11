using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace B4.Models.Interfaces
{

    /// <summary>
    /// Interface para IMemoryCacheService
    /// </summary>
    public interface IMemoryCacheService
    {
        void InvalidateCache();
        void InvalidateCache(string cacheKey);
    }
}
