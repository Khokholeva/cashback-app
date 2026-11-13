using System;
using System.Collections.Generic;
using System.Text;
using cashback_domain.Entities;

namespace cashback_application
{
    public static class SaveExtensions
    {
        public async static void SaveToDB<TEntity, TID>(this TEntity entity, IDBRepository<TEntity, TID> dBRepository)
            where TEntity : IEntity<TID>
        {
            await dBRepository.SaveAsync(entity);
        }
    }
}
