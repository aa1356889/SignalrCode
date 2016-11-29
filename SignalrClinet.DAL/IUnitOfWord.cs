using System;

namespace SignalrClinet.DAL
{
   public interface IUnitOfWord:IDisposable
    {
        int SaveChanges();
    }
}
