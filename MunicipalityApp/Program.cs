using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MunicipalityApp
{
    static class Program
    {
        public static List<IssueReport> IssueReports = new List<IssueReport>();

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    // IssueReport class definition moved here to ensure it's available everywhere
    
    public class IssueReport
    {
        public static int NextId = 1001;
        public int Id { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string AttachmentPath { get; set; }
        public DateTime ReportDate { get; set; }
        public string Status { get; set; }
        public DateTime? LastUpdated { get; set; }     
        public string StatusNotes { get; set; }           
        public int ProgressPercent { get; set; }         
    }
}
