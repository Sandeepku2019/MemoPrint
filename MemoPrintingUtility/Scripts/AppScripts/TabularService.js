KuApp.service('TabularService', function ($http) {

    var getCoursedetailsurl = "/TabularReport/GetCourseDetials"
    this.getCourses = function () {

        return result = $http({
            method: "Post",
            url: getCoursedetailsurl,
           

        });
    };



    var GenerateTabulationUrl = "/TabularReport/GenerateTabularSemReport"
    this.GenerateTabulation = function (Course, Semister) {

        return result = $http({
            method: "Post",
            url:GenerateTabulationUrl,
            params: { course: Course, Psem: Semister }

        });
    };



    var GenerateTabulationVRUrl = "/TabularReport/GenerateTabularVerticalReport"
    this.GenerateTabulationVR = function (Course) {

        return result = $http({
            method: "Post",
            url: GenerateTabulationVRUrl,
            params: { course: Course }

        });
    };

});