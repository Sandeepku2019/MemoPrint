KuApp.service('ShortMemoService', function ($http) {

    var getCoursedetailsurl = "/ShortMemo/GetCourseDetials"
    this.getCourses = function () {

        return result = $http({
            method: "Post",
            url: getCoursedetailsurl,


        });
    };



    var GenerateTabulationUrl = "/ShortMemo/GenerateTabularSemReport"
    this.GenerateTabulation = function (Course, Semister) {

        return result = $http({
            method: "Post",
            url: GenerateTabulationUrl,
            params: { course: Course, Psem: Semister }

        });
    };

});