using System;

namespace ApplicationManagementSystem
{
    public class Examination
    {
        public int Id {  get; set; }
        public string SpecialtyName { get; set; }
        public string FullName {  get; set; }
        public DateTime ExamDate { get; set; }
        public string Observer { get; set; }
        public int Results { get; set; }
    }
}
