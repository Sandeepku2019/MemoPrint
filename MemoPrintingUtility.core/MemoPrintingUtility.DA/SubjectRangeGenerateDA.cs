using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoPrintingUtility.Entity;

namespace MemoPrintingUtility.DA
{
    public class SubjectRangeGenerateDA : ISubjectRangeGenerateDA
    {
        public List<SubjectRangeEntity> GetSubjectDetailsForRange()
        {
            try
            {

                MemoPrintDBDataContext SubjectContext = new MemoPrintDBDataContext();
                SubjectContext.CommandTimeout = 260;
                var result = SubjectContext.Get_subjectRangeDetails().AsQueryable();
                var Subjectdetails = (from stu in result
                                      select new SubjectRangeEntity
                                      {
                                          SubjectCode = stu.SubjectCode,
                                          Count = Convert.ToInt32(stu.StudentCount)
                                         
                                      }).ToList();

                return Subjectdetails;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void InsertSubjectCode(SubjectRangeEntity LstsubjectCode)
        {
            KUPostDBDataContext KPOContext = new KUPostDBDataContext();

            
              var result =  KPOContext.SP_insertSubjectRangeCode(LstsubjectCode.SubjectCode, LstsubjectCode.Count, LstsubjectCode.RangeStart, LstsubjectCode.RangeEnd);

            

        }
    }
}
