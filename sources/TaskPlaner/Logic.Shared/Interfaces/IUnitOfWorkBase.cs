using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Shared.Interfaces
{
    public interface IUnitOfWorkBase
    {
       
        Task SaveChangesAsync(string userName = "System");
    }
}
