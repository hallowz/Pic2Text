using System;
using SQLite;

namespace Pic2Text.Models
{
    public class P2T
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string imageLocation { get; set; }
    }
}
