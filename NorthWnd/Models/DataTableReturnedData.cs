using System.Collections.Generic;

namespace NorthWnd.Models
{
    public class DataTableReturnedData<T>
    {
        public int draw { get; set; }

        public int recordsTotal { get; set; }

        public int recordsFiltered { get; set; }

        public IEnumerable<T> data { get; set; }

        public string error { get; set; }
    }
}