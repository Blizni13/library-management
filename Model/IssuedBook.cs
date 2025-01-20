using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library_management.Model
{
    class IssuedBook
    {
        public int ID { get; set; }
        public string ReaderInfo { get; set; }
        public string BookInfo { get; set; }
        public DateTime IssueDate { get; set; }
        public IssuedBook(int id, string readerInfo, string bookInfo, DateTime issueDate)
        {
            ID = id;
            ReaderInfo = readerInfo;
            BookInfo = bookInfo;
            IssueDate = issueDate;
        }
    }
}
