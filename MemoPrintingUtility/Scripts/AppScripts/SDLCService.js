KuApp.service('SDLCService', function ($http) {




    var GenerateTabulationUrl = "/SDLC/GenerateTabularSemReport"
    this.GenerateTabulation = function (Course, Semister) {

        return result = $http({
            method: "Post",
            url: GenerateTabulationUrl,
            params: { course: Course, ReportType: Semister }

        });
    };
});