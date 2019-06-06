using MemoPrintingUtility.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoPrintingUtility.DA
{
   public interface ISubjectRangeGenerateDA
    {
         List<SubjectRangeEntity> GetSubjectDetailsForRange();

        void InsertSubjectCode(SubjectRangeEntity LstsubjectCode);


        List<SubJectInformation> GetAllSubjects();

        List<Courses> GetAllCourse();

        List<SubJectInformation> GetAllSubjectByYr();

        List<SubjectRangeEntity> GetSubjectDetailsForRangeYr();

        void InsertSubjectCodeYear(SubjectRangeEntity LstsubjectCode);
    }
}
