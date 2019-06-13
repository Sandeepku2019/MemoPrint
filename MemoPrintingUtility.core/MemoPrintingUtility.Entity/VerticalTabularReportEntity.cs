using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoPrintingUtility.Entity
{
   public class VerticalTabularReportEntity
    {
        public string SubCode { get; set; }


        /// <summary>
        /// Pres values..
        /// </summary>
        public string MaxMarks { get; set; }

        public string MinMarks { get; set; }

        public string Marks { get; set; }

        public string Grade { get; set; }

        public string Moderation { get; set; }

        public string FinalMarks { get; set; }

        public string InternalMarks { get; set; }

        public string AggrigationMarks { get; set; }

        public string Credits { get; set; }


        public bool isPress { get; set; }

        /// <summary>
        /// CON data
        /// </summary>
        public string CONR { get; set; }

        public string GP { get; set; }

        public string CONMarks { get; set; }

        public string ConIntMarks { get; set; }

        public string ConExtMarks { get; set; }

        public string ConResult { get; set; }

        public string ConAcademi{ get; set; }



    }
}
