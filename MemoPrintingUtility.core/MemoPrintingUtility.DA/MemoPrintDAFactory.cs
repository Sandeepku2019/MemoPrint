using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoPrintingUtility.DA
{
    public class MemoPrintDAFactory : IMemoPrintDAFactory
    {
        public ICourseDAService GetCourseInstance()
        {
            return new CourseDAService();
        }

        public ILongMemoDAService GetLongMemoService()
        {
            return new LongMemoDAService();
        }

        public ISDLTablarDA GetSDLCInstance()
        {
            return new SDLTablarDA();
        }

        public ISubjectRangeGenerateDA GetSubjectRangeService()
        {
            return new SubjectRangeGenerateDA();
        }

        public ITabularReportDAService GetTabularDAService()
        {
           return new TabularReportDAService();
        }
    }
}
