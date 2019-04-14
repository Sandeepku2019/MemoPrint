using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoPrintingUtility.BO
{
    public class MemoPrintService : IMemoPrintService
    {
        public ICourseService GetCourseInstance()
        {
            return new CourseService();
        }

        public ITabularReportService getTabularReportInstance()
        {
            return new TabularReportService();
        }
    }
}
