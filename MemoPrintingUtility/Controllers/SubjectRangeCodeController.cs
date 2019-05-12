using MemoPrintingUtility.BO;
using MemoPrintingUtility.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MemoPrintingUtility.Controllers
{
    public class SubjectRangeCodeController : Controller
    {
        // GET: SubjectRangeCode
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetData()
        {
            try
            {

                MemoPrintService BoMemoService = new MemoPrintService();
                var lstSubjectDetails = BoMemoService.getSubjectRangeInstance().GetSubjectDetailsForRange();
                TempData["SubjectList"] = lstSubjectDetails;
                TempData.Keep();

                return Json(lstSubjectDetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }



        [HttpPost]
        public JsonResult GenerateRanges( Int32 RangeFrom, Int32 Gap)
        {

            try
            {
                int RF = RangeFrom;

                MemoPrintService BoMemoService = new MemoPrintService();
                var lstSubjectDetails = BoMemoService.getSubjectRangeInstance().GetSubjectDetailsForRange();
                TempData["SubjectList"] = lstSubjectDetails;
                TempData.Keep();


                foreach (var subCode in lstSubjectDetails)
                {

                    subCode.RangeStart = RF;
                    subCode.RangeEnd = RF + subCode.Count;

                    RF = subCode.RangeEnd + Gap;
                }
                return Json(lstSubjectDetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ImportData()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveCodes(List<SubjectRangeEntity> lstSubjects, Int32 RangeFrom, Int32 Gap)
        {
            bool status = false;
            int RF = RangeFrom;
            MemoPrintService BoMemoService = new MemoPrintService();
          


         
            var lstSubjectDetails = BoMemoService.getSubjectRangeInstance().GetSubjectDetailsForRange();
            TempData["SubjectList"] = lstSubjectDetails;
            TempData.Keep();


            foreach (var subCode in lstSubjectDetails)
            {

                subCode.RangeStart = RF;
                subCode.RangeEnd = RF + subCode.Count;
                RF = subCode.RangeEnd + Gap;

                BoMemoService.getSubjectRangeInstance().InsertSubjectCode(subCode);
            }
            
            return new JsonResult { Data = new { status = status } };
        }


    }

    
}