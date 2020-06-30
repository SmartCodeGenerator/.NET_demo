using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackCaviarBank.Domain.Core.QueryParams
{
    public class PagedList<TEntity> : List<TEntity>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PagedList(List<TEntity> items)
        {
            AddRange(items);
        }

        public PagedList(List<TEntity> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public async static Task<PagedList<TEntity>> ToPagedList(IQueryable<TEntity> source, int pageNumber, int pageSize)
        {
            int count = 0;
            List<TEntity> items = new List<TEntity>();
            if (source != null)
            {
                List<TEntity> list = await source.ToListAsync();
                count = list.Count();
                items = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }

            return new PagedList<TEntity>(items, count, pageNumber, pageSize);
        }

        public async static Task<PagedList<TEntity>> AsSimpleData(IQueryable<TEntity> source)
        {
            return new PagedList<TEntity>(await source.ToListAsync());
        }
    }
}
