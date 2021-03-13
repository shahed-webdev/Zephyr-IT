using System.Collections.Generic;


namespace JqueryDataTables.LoopsIT
{
    public class DataResult<T> where T : class
    {
        public int draw { get; set; }
        public long recordsTotal { get; set; }
        public long recordsFiltered { get; set; }
        public List<T> data { get; set; }
    }

    public class CustomDataResult<T> : DataResult<T> where T : class
    {
        public decimal GrandTotal { get; set; }
    }
}
