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

                List<SubJectInformation> lstSubJectList = GetAllSubjects();
                List<Courses> lstCourses = GetAllCourse();

                MemoPrintDBDataContext SubjectContext = new MemoPrintDBDataContext();
                SubjectContext.CommandTimeout = 260;
                var result = SubjectContext.Get_subjectRangeDetails().AsQueryable();
                var SubjectRangedetails = (from stu in result
                                           select new SubjectRangeEntity
                                           {
                                               SubjectCode = stu.SubjectCode,
                                               Count = Convert.ToInt32(stu.StudentCount),
                                               Sem = Convert.ToInt32(stu.FK_SEM),
                                               Year = Convert.ToInt32(stu.FK_YEAR),
                                               CourseID = Convert.ToInt32(stu.FK_COURSEID),
                                              
                                           }).ToList();





                foreach (var Sub in SubjectRangedetails)
                {

                    var Course = lstCourses.Where(x => x.Id == Sub.CourseID).ToList();
                    if (Course.Count > 0)
                    {

                        Sub.CourseName = Course[0].CourseName;
                    }


                    var subject = lstSubJectList.Where(x => x.Year == Convert.ToString(Sub.Year) 
                                                        && Convert.ToInt32(x.Sem) == Sub.Sem 
                                                        && x.ShortCode == Sub.SubjectCode 
                                                        && x.CourseID == Sub.CourseID.ToString()
                                                        ).ToList();
                    if (subject.Count > 0)
                    {
                        if (subject[0].IsPractical != "1")
                        {

                            Sub.SubjectName = subject[0].SubjectName;

                        }
                        else
                        {
                            Sub.SubjectName = subject[0].SubjectName;
                            Sub.IPractical = "1";

                        }
                    }


                }
                return SubjectRangedetails;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void InsertSubjectCode(SubjectRangeEntity LstsubjectCode)
        {

            KUPostDBDataContext KPOContext = new KUPostDBDataContext();
            KPOContext.CommandTimeout = 260;
            var result = KPOContext.SP_insertSubjectRangeCode(
                LstsubjectCode.SubjectCode,
                LstsubjectCode.Count, LstsubjectCode.Year, LstsubjectCode.Sem,
                LstsubjectCode.CourseID, LstsubjectCode.CourseName,
                LstsubjectCode.SubjectName == null ?"": LstsubjectCode.SubjectName, LstsubjectCode.RangeStart, LstsubjectCode.RangeEnd);

              

        }



        public List<SubJectInformation> GetAllSubjects()
        {

            KUPostDBDataContext KPOContext = new KUPostDBDataContext();
            KPOContext.CommandTimeout = 260;
            var result = KPOContext.GetAllSubjectDeails().AsQueryable();
            var Subjectdetails = (from stu in result
                                  select new SubJectInformation
                                  {
                                      CourseID = stu.FK_COURSEID,
                                      ShortCode = stu.SHORTCODE,
                                      Year = stu.FK_YEAR,
                                      Sem = stu.FK_SEM,
                                      SubjectName = stu.SUBJECTNAME,
                                      IsPractical = stu.ISPRACTICAL,
                                     

                                  }).ToList();

            return Subjectdetails;

        }


        public List<Courses> GetAllCourse()
        {
            KUPostDBDataContext KPOContext = new KUPostDBDataContext();


            KPOContext.CommandTimeout = 260;
            var result = KPOContext.GetAllCourses().AsQueryable();
            var CourseDetails = (from stu in result
                                 select new Courses
                                 {
                                     Id = stu.Pk_CourseID,
                                     CourseName = stu.CourseName

                                 }).ToList();

            return CourseDetails;

        }
    }
}
