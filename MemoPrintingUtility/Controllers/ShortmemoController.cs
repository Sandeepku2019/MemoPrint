using MemoPrintingUtility.BO;

using MemoPrintingUtility.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MemoPrintingUtility.Controllers
{
    public class ShortmemoController : Controller
    {
        // GET: Shortmemo
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GenerateTabularSemReport(int Psem, string course)
        {
            string hn = "";
            try
            {

                ///// Creting Notepad 
                //string fileNamedirectory = Server.MapPath("/Tab");
                string fileNamedirectory = @"D:\ShortMemo\";
                string filename = course + DateTime.Now.ToString("ddMMyyyy") + "_" + Psem + ".txt";

                // check for Directory
                if (!Directory.Exists(fileNamedirectory))  // if it doesn't exist, create
                {
                    Directory.CreateDirectory(fileNamedirectory);
                }


                // file information 
                FileInfo fi = new FileInfo(fileNamedirectory + filename);
                // Check if file already exists. If yes, delete it.     
                if (System.IO.File.Exists(fileNamedirectory + filename))
                {
                    System.IO.File.Delete(fileNamedirectory + filename);
                }

                MemoPrintService BoMemoService = new MemoPrintService();

                string Syear = string.Empty;
                string Ssem = string.Empty;
                int yr = 0;
                int sm = 0;
                if (Psem == 11)
                {
                    Syear = "I";
                    yr = 1;
                    sm = 1;
                    Ssem = "I";

                }
                if (Psem == 21)
                {
                    Syear = "II";
                    yr = 2;
                    sm = 1;
                    Ssem = "I";

                }

                if (Psem == 31)
                {
                    Syear = "III";
                    yr = 3;
                    Ssem = "I";
                    sm = 1;
                }

                // Create a new file     
                using (StreamWriter sw = fi.CreateText())
                {
                    //
                    var lstStudents = BoMemoService.getTabularReportInstance().GetStudentDetail(course, Psem, sm, yr);
                    //var LstConStudents = BoMemoService.getTabularReportInstance().GetStudentsConsDetails(course, Psem);
                    
                    int TotalSubjectAttemt = 0;
                    int TotalSubjectpass = 0;
                    int rowcount = 0;
                     List<string> HallticketNumbers = lstStudents.OrderBy(x => x.HallTicketNumber).Select(x => x.HallTicketNumber).Distinct().ToList<string>();

                     List<string> MallPactMember =BoMemoService.getTabularReportInstance().GetMallPractHtno(course, Psem, sm, yr).Select(x => x.HallTicketNumber).Distinct().ToList<string>();

                    foreach (var malstuden in MallPactMember)
                    {
                        HallticketNumbers.Remove(malstuden);
                    }


                    // Making even number for  side by side print 
                    if (HallticketNumbers.Count % 2 == 0)
                    {

                    }
                    else
                    {
                        HallticketNumbers.Add("0");
                    }

                    for (int i = 0; i < HallticketNumbers.Count; i++)
                    {

                       
                        hn = HallticketNumbers[i];
                        if (i == 0)
                        {
                            sw.WriteLine("");
                            sw.WriteLine("");
                            sw.WriteLine("");
                        }
                        var lstStunsfirst = lstStudents.OrderBy(y => y.Order).Where(x => x.HallTicketNumber == HallticketNumbers[i]).ToList<StudentInformation>();
                        var lstStubScon = lstStudents.OrderBy(y => y.Order).Where(x => x.HallTicketNumber == HallticketNumbers[i + 1]).ToList<StudentInformation>();


                        if (lstStunsfirst == null || lstStunsfirst.Count == 0)
                        { lstStunsfirst = new List<StudentInformation>(); lstStunsfirst.Add(new StudentInformation() { SubjectName = "", Credits = "", Status = "", SGPA = "", LeterGrade = "" }); }
                        if (lstStubScon == null || lstStubScon.Count == 0)
                        {
                            lstStubScon = new List<StudentInformation>();
                            lstStubScon.Add(new StudentInformation() { SubjectName = "", Credits = "", Status = "", SGPA = "", LeterGrade = "" ,FinalResult=""});
                        }


                        string gap = GetSpaces(5);
                        string HallTicket_1 = lstStunsfirst[0].HallTicketNumber == null ? "" : lstStunsfirst[0].HallTicketNumber;
                        string FN_1 = lstStunsfirst[0].FatherName == null ? "" : lstStunsfirst[0].FatherName;
                        string SN_1 = lstStunsfirst[0].StudentName == null ? "" : lstStunsfirst[0].StudentName;
                        string CC_1 = lstStunsfirst[0].collegecode == null ? "" : lstStunsfirst[0].collegecode;

                        string CourseDetails = GetSpaces(10) + course + " " + Syear + "YR / " + Ssem + "SEM   NOV/DEC ." + (DateTime.Now.Year - 1).ToString();
                        CourseDetails = CourseDetails + GetSpaces(55 - CourseDetails.Length) + DateTime.Now.ToString("dd-MM-yyyy");


                        string HallTicket_2 = lstStubScon[0].HallTicketNumber == null ? "" : lstStubScon[0].HallTicketNumber;
                        string FN_2 = lstStubScon[0].FatherName == null ? "" : lstStubScon[0].FatherName;
                        string SN_2 = lstStubScon[0].StudentName == null ? "" : lstStubScon[0].StudentName;
                        string CC_2 = lstStubScon[0].collegecode == null ? "" : lstStubScon[0].collegecode;

                        string CondidateRow = GetSpaces(10) + SN_1 + GetSpaces(55 - SN_1.Length) + gap + GetSpaces(10) + SN_2 + GetSpaces(55 - SN_2.Length);
                        string FatherRow = GetSpaces(10) + FN_1 + GetSpaces(45 - FN_1.Length) + HallTicket_1 + GetSpaces(10 - HallTicket_1.Length) + gap;
                        FatherRow = FatherRow + GetSpaces(10) + FN_2 + GetSpaces(45 - FN_2.Length) + HallTicket_2 + GetSpaces(10 - HallTicket_2.Length);

                        string ColCodeRow = CC_1 + GetSpaces(65 - CC_1.Length) + gap + CC_1 + GetSpaces(65 - CC_1.Length);




                        sw.WriteLine(ColCodeRow);
                        string CourseRows = CourseDetails + gap + CourseDetails;
                        sw.WriteLine(CourseRows);
                        sw.WriteLine(CondidateRow);
                        sw.WriteLine(FatherRow);


                        sw.WriteLine("");
                        sw.WriteLine("");

                        int rowCount = 0;
                        if (lstStunsfirst.Count > lstStubScon.Count)
                        {
                            rowCount = lstStunsfirst.Count;
                        }
                        else
                        {
                            rowCount = lstStubScon.Count;
                        }

                        int memorows = 0;
                        for (int j = 0; j < 19; j++)
                        {
                            if (memorows < 17)
                            {
                                if (lstStunsfirst.Count - 1 < j)
                                {
                                    lstStunsfirst.Add(new StudentInformation() { SubjectName = "", Credits = "", Status = "", SGPA = "", LeterGrade = "" });
                                }
                                if (lstStubScon.Count - 1 < j)
                                {
                                    lstStubScon.Add(new StudentInformation() { SubjectName = "", Credits = "", Status = "", SGPA = "", LeterGrade = "" }); memorows++;
                                }

                                if (lstStunsfirst[j].SubjectName.Length < 55 && lstStubScon[j].SubjectName.Length < 55)
                                {
                                    SingleLingSubject(sw, j, lstStunsfirst, lstStubScon);
                                }
                                else
                                {
                                    Twolinesubject(sw, j, lstStunsfirst, lstStubScon);
                                    memorows++;
                                    memorows++;

                                }
                            }

                        }

                        string SGPARow = string.Empty;
                        //first record
                        string FirstFinalResult = string.Empty;
                        if (lstStunsfirst[0].Ei == "E" && lstStunsfirst[0].FinalResult== "PROMOTED")
                        {
                            FirstFinalResult = "FAILED";
                        }
                        else
                        {
                            FirstFinalResult = lstStunsfirst[0].FinalResult;
                        }


                        if (FirstFinalResult == "PASSED" || FirstFinalResult == "COMPLETED" || FirstFinalResult == "PROMOTED")
                        { 
                            string sg = lstStunsfirst[0].SGPA == null ? "***" : lstStunsfirst[0].SGPA;
                            SGPARow = GetSpaces(10) + sg + GetSpaces(55 - sg.Length);
                        }
                        else
                        {
                            SGPARow = GetSpaces(10) + "***" + GetSpaces(65 - "***".Length);
                        }
                        SGPARow = SGPARow + gap;


                        //Second recod
                        string SecondFinalResult = string.Empty;
                        if (lstStubScon[0].Ei == "E" && lstStubScon[0].FinalResult == "PROMOTED")
                        {
                            SecondFinalResult = "FAILED";
                        }
                        else
                        {
                            SecondFinalResult = lstStubScon[0].FinalResult;
                        }

                        if (SecondFinalResult == "PASSED" || SecondFinalResult == "COMPLETED" || SecondFinalResult == "PROMOTED")
                        {
                            string sg1 = lstStubScon[0].SGPA == null ? "***" : lstStubScon[0].SGPA;
                            SGPARow = SGPARow+ GetSpaces(10) + sg1 + GetSpaces(55 - sg1.Length);
                        }
                        else
                        {
                            SGPARow = SGPARow + GetSpaces(10) + "***" + GetSpaces(65 - "***".Length);
                        }

                        sw.WriteLine(SGPARow);
                     


                        string statusrow = GetSpaces(10) + FirstFinalResult + GetSpaces(55 - FirstFinalResult.Length) + gap;
                        statusrow = statusrow + GetSpaces(10) + SecondFinalResult + GetSpaces(55 - SecondFinalResult.Length);

                        sw.WriteLine(statusrow);
                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("");


                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("");
                        i++;


                    }
                }

                // Write file contents on console.     
                using (StreamReader sr = System.IO.File.OpenText(fileNamedirectory + filename))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }

                return Json(fileNamedirectory + filename, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                if (hn != "0")
                {
                }
                return Json(Ex.Message, JsonRequestBehavior.AllowGet);
            }


        }


        public void ShowFile(string filename)
        {
            //Clears all content output from Buffer Stream
            Response.ClearContent();
            //Clears all headers from Buffer Stream
            Response.ClearHeaders();
            //Adds an HTTP header to the output stream
            Response.AddHeader("Content-Disposition", "inline;filename=" + filename);
            //Gets or Sets the HTTP MIME type of the output stream
            Response.ContentType = "application/pdf";
            //Writes the content of the specified file directory to an HTTP response output stream as a file block
            Response.WriteFile(filename);
            //sends all currently buffered output to the client
            Response.Flush();
            //Clears all content output from Buffer Stream
            Response.Clear();
        }

        private string GetSpaces(int totalLength)
        {
            string result = string.Empty;
            for (int i = 0; i < totalLength; i++)
            {
                result += " ";
            }
            return result;
        }


        [HttpPost]
        public JsonResult GetCourseDetials()
        {
            //get the course details
            MemoPrintService MemoPrintService = new MemoPrintService();
            var lstCourses = MemoPrintService.GetCourseInstance().GetCourseDetails();
            return Json(lstCourses, JsonRequestBehavior.AllowGet);

        }


        private string ConcatenateSubject(string appenedstring, string subject, string yearPass)
        {
            if (subject != null && subject.Length > 0)
            {
                string sub = subject + " " + yearPass.CheckNumeric(2);
                appenedstring = appenedstring + sub + GetSpaces(10 - sub.Length);
                return appenedstring;
            }
            else
            {
                return appenedstring;
            }
        }

        private string ConcatenateSubjectMarks(string appenedstring, string subjectMarks, string Result = "")
        {
            if (subjectMarks != null && subjectMarks.Length > 0)
            {
                string sub = subjectMarks + " " + Result;
                appenedstring = appenedstring + sub + GetSpaces(10 - sub.Length);
                return appenedstring;
            }
            else
            {
                return appenedstring;
            }

        }

        private int addHeaderFooter(StreamWriter sw, int rowcount)
        {
            if (rowcount == 0)
            {



                sw.WriteLine(" ");
                sw.WriteLine(" ");
                rowcount = 1;


            }
            else if (rowcount == 29)
            {
                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");


                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");

                rowcount = 1;


            }
            else { rowcount++; }
            return rowcount;
        }

        private void SingleLingSubject(StreamWriter sw, int i, List<StudentInformation> lstStunsfirst, List<StudentInformation> lstStubScon)
        {
            bool flag1 = false;
            bool flag2 = false;
            string space = " ";
            string dotedrow = "- -  - -  - -  - -  - -  - -  - -";

            if (lstStunsfirst[i].SubjectName.Length < 55 && lstStubScon[i].SubjectName.Length < 55)
            {

                string SubjectAndGrades = string.Empty;
                if (i < lstStunsfirst.Count)
                {
                    SubjectAndGrades = SubjectAndGrades + lstStunsfirst[i].SubjectName + GetSpaces(55 - lstStunsfirst[i].SubjectName.Length);
                    SubjectAndGrades = SubjectAndGrades + lstStunsfirst[i].Credits + GetSpaces(6 - lstStunsfirst[i].Credits.Length);
                    SubjectAndGrades = SubjectAndGrades + lstStunsfirst[i].LeterGrade + GetSpaces(4 - lstStunsfirst[i].LeterGrade.Length);

                }
                else
                {
                    if (flag1 == false)
                    {
                        SubjectAndGrades = SubjectAndGrades + dotedrow + GetSpaces(55 - dotedrow.Length);
                        SubjectAndGrades = SubjectAndGrades + space + GetSpaces(6 - space.Length);
                        SubjectAndGrades = SubjectAndGrades + space + GetSpaces(4 - space.Length);
                        flag1 = true;
                    }
                    else
                    {
                        SubjectAndGrades = GetSpaces(66);
                    }
                }
                SubjectAndGrades = SubjectAndGrades + GetSpaces(5);
                if (i < lstStubScon.Count)
                {

                    SubjectAndGrades = SubjectAndGrades + lstStubScon[i].SubjectName + GetSpaces(55 - lstStubScon[i].SubjectName.Length);
                    SubjectAndGrades = SubjectAndGrades + lstStubScon[i].Credits + GetSpaces(6 - lstStubScon[i].Credits.Length);
                    SubjectAndGrades = SubjectAndGrades + lstStubScon[i].LeterGrade + GetSpaces(4 - lstStubScon[i].LeterGrade.Length);
                }
                else
                {
                    if (flag2 == false)
                    {
                        SubjectAndGrades = SubjectAndGrades + dotedrow + GetSpaces(55 - dotedrow.Length);
                        SubjectAndGrades = SubjectAndGrades + space + GetSpaces(6 - space.Length);
                        SubjectAndGrades = SubjectAndGrades + space + GetSpaces(4 - space.Length);
                        flag2 = true;
                    }
                    else
                    {
                        SubjectAndGrades = GetSpaces(66);
                    }
                }
                sw.WriteLine(SubjectAndGrades);

            }

        }

        private void Twolinesubject(StreamWriter sw, int i, List<StudentInformation> lstStunsfirst, List<StudentInformation> lstStubScon)
        {

            string subject1 = "";
            subject1 = lstStunsfirst[i].SubjectName;
            string s1sp1 = "";
            string s1sp2 = "";

            if (subject1.Length > 55)
            {
                s1sp1 = subject1.Substring(0, 48);
                s1sp2 = subject1.Substring(48);
            }


            string subject2 = "";
            subject2 = lstStubScon[i].SubjectName;
            string s2sp1 = "";
            string s2sp2 = "";
            if (subject2.Length > 55)
            {
                s2sp1 = subject2.Substring(0, 48);
                s2sp2 = subject2.Substring(48);
            }

            ///scenario 1
            if (subject1.Length > 55 && subject2.Length > 55)
            {
                string s1 = s1sp1 + GetSpaces(55 - s1sp1.Length) + lstStunsfirst[i].Credits + GetSpaces(6 - lstStunsfirst[i].Credits.Length) + lstStunsfirst[i].LeterGrade + GetSpaces(4 - lstStunsfirst[i].LeterGrade.Length) + GetSpaces(5);
                s1 = s1 + s2sp1 + GetSpaces(55 - s2sp1.Length) + lstStubScon[i].Credits + GetSpaces(6 - lstStubScon[i].Credits.Length) + lstStubScon[i].LeterGrade + GetSpaces(4 - lstStubScon[i].LeterGrade.Length);
                sw.WriteLine(s1);

                string sbuid = s1sp2 + GetSpaces(65 - s1sp2.Length) + GetSpaces(5) + s2sp2 + GetSpaces(65 - s2sp2.Length);


                sw.WriteLine(sbuid);
            }

        }



    }
}

