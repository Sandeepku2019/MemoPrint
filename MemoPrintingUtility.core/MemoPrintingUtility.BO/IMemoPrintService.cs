using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoPrintingUtility.BO
{
    public interface IMemoPrintService
    {
        ICourseService GetCourseInstance();

        ITabularReportService getTabularReportInstance();


        ISubjectRangeGenerateBO getSubjectRangeInstance();


        ISDLCTablarBO GetSDLCInstance();
    }

}
