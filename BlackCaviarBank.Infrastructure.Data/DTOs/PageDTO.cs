using System;

namespace BlackCaviarBank.Infrastructure.Data.DTOs
{
    public class PageDTO
    {
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }
        public bool HasPreviosPage => PageNumber > 1;
        public bool HasNextPage => TotalPages > PageNumber;

        public PageDTO(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
    }
}
