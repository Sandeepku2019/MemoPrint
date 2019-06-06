using MemoPrintingUtility.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoPrintingUtility.BO
{
    public interface ISubjectRangeGenerateBO
    {
        List<SubjectRangeEntity> GetSubjectDetailsForRange();

        void InsertSubjectCode(SubjectRangeEntity LstsubjectCode);


        List<SubjectRangeEntity> GetSubjectDetailsForRangeYr();


        void InsertSubjectCodeYear(SubjectRangeEntity LstsubjectCode);

    }
}
