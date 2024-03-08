using System;

namespace ProjectManager.Domain.Utils.Expressions
{
    public class Pagination
    {
        public static uint DefaultLimit = 20;
        public static uint MaxLimit = 1000;

        private int _limit = (int)DefaultLimit;
        private int _page;

        public int Offset
        {
            get
            {
                return this.Limit * (this.Page - 1);
            }
        }

        public int Page
        {
            get
            {
                return (_page <= 0) ? 1 : _page;
            }
            set
            {
                _page = value < 1 ? 1 : value;
            }
        }

        public int Limit
        {
            get
            {
                return (_limit <= 0) ? (int)DefaultLimit : _limit;
            }
            set
            {
                _limit = value <= 0 ? (int)DefaultLimit : value > MaxLimit ? (int)MaxLimit : value;
            }
        }

        public int PageSize
        {
            get => this.Limit;
            set => this.Limit = value;
        }

        public int CalculateTotalPages(int totalRecords)
        {
            return (int)Math.Ceiling(totalRecords / (double)PageSize);
        }
    }
}
