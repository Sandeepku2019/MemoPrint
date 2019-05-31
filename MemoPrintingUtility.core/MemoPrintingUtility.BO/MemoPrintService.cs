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

        public ILongMemoService GetLongMemoInstance()
        {
            return new LongMemoService();
        }

        public ISDLCTablarBO GetSDLCInstance()
        {
            return  new SDLCTablarBO();
        }

        public ISubjectRangeGenerateBO getSubjectRangeInstance()
        {
            return new SubjectRangeGenerateBO();
        }

        public ITabularReportService getTabularReportInstance()
        {
            return new TabularReportService();
        }
    }
}
