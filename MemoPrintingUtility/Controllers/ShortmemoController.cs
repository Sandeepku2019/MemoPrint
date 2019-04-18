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


                    List<string> HallticketNumbers = lstStudents.OrderBy(x => x.HallTicketNumber).Select(x => x.HallTicketNumber).Distinct().Take(4).ToList<string>();

                    for (int i = 0; i < HallticketNumbers.Count; i++)
                    {

                        if (i == 0)
                        {
                            sw.WriteLine("");
                            sw.WriteLine("");
                        }
                        var lstStunsfirst = lstStudents.OrderBy(y=>y.Order).Where(x => x.HallTicketNumber == HallticketNumbers[i]).ToList<StudentInformation>();
                        var lstStubScon = lstStudents.OrderBy(y => y.Order).Where(x => x.HallTicketNumber == HallticketNumbers[i + 1]).ToList<StudentInformation>();


                        if (lstStunsfirst == null)
                        { lstStunsfirst = new List<StudentInformation>(); }


                        string gap = GetSpaces(5);
                        string HallTicket_1 = lstStunsfirst[0].HallTicketNumber;
                        string FN_1 = lstStunsfirst[0].FatherName == null ? "" : lstStunsfirst[0].FatherName;
                        string SN_1 = lstStunsfirst[0].StudentName == null ? "" : lstStunsfirst[0].StudentName;
                        string CC_1 = lstStunsfirst[0].collegecode == null ? "" : lstStunsfirst[0].collegecode;

                        string CourseDetails = GetSpaces(10) + course + " " + Syear + "yr/" + Ssem + "Sem " + DateTime.Now.Year.ToString();
                        CourseDetails = CourseDetails + GetSpaces(55 - CourseDetails.Length) + DateTime.Now.ToString("dd-MM-yyyy");


                        string HallTicket_2 = lstStubScon[0].HallTicketNumber;
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

                        bool flag1 = false;
                        bool flag2 = false;
                        string space=" ";
                        string dotedrow = "- -  - -  - -  - -  - -  - -  - -";
                        for (i = 0; i < 19; i++)
                        {
                            string SubjectAndGrades = string.Empty;
                            if (i < lstStunsfirst.Count)
                            {
                                
                                SubjectAndGrades = SubjectAndGrades+lstStunsfirst[i].SubjectName + GetSpaces(55 - lstStunsfirst[i].SubjectName.Length);
                                SubjectAndGrades = SubjectAndGrades+lstStunsfirst[i].Credits + GetSpaces(6 - lstStunsfirst[i].Credits.Length);
                                SubjectAndGrades = SubjectAndGrades+lstStunsfirst[i].Status + GetSpaces(4 - lstStunsfirst[i].Status.Length);

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
                                else {
                                    SubjectAndGrades = GetSpaces(66);
                                }
                            }
                            SubjectAndGrades = SubjectAndGrades + gap;
                            if (i < lstStubScon.Count) 
                            {
                                
                                SubjectAndGrades = SubjectAndGrades+lstStubScon[i].SubjectName + GetSpaces(55 - lstStubScon[i].SubjectName.Length);
                                SubjectAndGrades = SubjectAndGrades+lstStubScon[i].Credits + GetSpaces(6 - lstStubScon[i].Credits.Length);
                                SubjectAndGrades = SubjectAndGrades+lstStubScon[i].Status + GetSpaces(4 - lstStubScon[i].Status.Length);
                            }
                            else
                            {
                                if (flag2== false)
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


                        i++;

                        //#region  Pre
                        //string subjectstringPRE = string.Empty;
                        //string SubjectMarks_U = string.Empty;
                        //string subjectsMarksPRE_U = string.Empty;
                        //string MarkformatPRE = GetSpaces(10);
                        //string Aademicstatus = string.Empty;



                        //string SubjectMarks_S = string.Empty;
                        //string subjectsMarksPRE_S = string.Empty;


                        //if (lstStuns.Count > 0)
                        //{
                        //    string subjectformt = "";
                        //    subjectstringPRE = "PRES." + GetSpaces(5 - "PRES.".Length) + GetSpaces(7);
                        //    subjectsMarksPRE_U = "    U" + GetSpaces(5 - "PRES.".Length) + GetSpaces(7);
                        //    subjectsMarksPRE_S = "    S" + GetSpaces(5 - "PRES.".Length) + GetSpaces(7);
                        //    int subord = 1;

                        //    int count = 0;

                        //    foreach (StudentInformation studentM in lstStuns.OrderBy(X => X.Order))
                        //    {
                        //        string spaces = "";
                        //        string umakspace = "";
                        //        string intmarkspae = "";
                        //        int ordr = studentM.Order;
                        //        spaces = GetSpaces((ordr - subord) * 10);
                        //        umakspace = GetSpaces((ordr - subord) * 10);
                        //        intmarkspae = GetSpaces((ordr - subord) * 10);
                        //        string intmark = Convert.ToString(studentM.InternalMarks == null ? "" : studentM.InternalMarks) + GetSpaces(3 - Convert.ToString(studentM.InternalMarks == null ? "" : studentM.InternalMarks).Length) + " " + studentM.Status;
                        //        if (count > 0)
                        //        {
                        //            spaces = "";

                        //            spaces = GetSpaces(7);
                        //            umakspace = GetSpaces(10 - studentM.ExernalMarks.ToString().Length);
                        //            intmarkspae = GetSpaces(9 - intmark.Length);
                        //        }

                        //        subjectformt = spaces + studentM.SubjectCode;
                        //        subjectstringPRE = subjectstringPRE + subjectformt;

                        //        SubjectMarks_U = umakspace + studentM.ExernalMarks.ToString();
                        //        subjectsMarksPRE_U = subjectsMarksPRE_U + SubjectMarks_U;


                        //        intmark = spaces + intmark;// + GetSpaces(7 - intmark.Length); ;
                        //        SubjectMarks_S = intmark;
                        //        subjectsMarksPRE_S = subjectsMarksPRE_S + SubjectMarks_S;
                        //        count++;
                        //    }

                        //    int TotalMark = lstStuns.Sum(x => x.ExernalMarks.ChangeINT());
                        //    TotalMark = TotalMark + lstStuns.Sum(x => x.InternalMarks.ChangeINT());
                        //    sw.WriteLine(subjectstringPRE);
                        //    sw.WriteLine(subjectsMarksPRE_U + GetSpaces(113 - subjectsMarksPRE_U.Length) + lstStuns[0].FinalResult + "    " + TotalMark);
                        //    sw.WriteLine(subjectsMarksPRE_S);


                        //}

                        //#endregion

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

    }
}