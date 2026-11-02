using System;
using System.Collections.Generic;
using System.Text;

namespace cashback_infrastructure.DB
{
    internal interface IDBRepository
    {
        public IEnumerable<T> GetAllEntries<T>();

        public T FindByID<T, TID>(TID id);

        public void Save<T>(T entity);

        public void Delete<T>(T entity);

        public void DeleteByID<TID>(TID id);

    }
}
