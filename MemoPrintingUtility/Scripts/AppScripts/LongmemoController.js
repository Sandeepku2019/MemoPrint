KuApp.controller('LongMemoController', function ($window, $scope, LongService) {

    $scope.lstCourses = [{ Id: "BCA(P)", CourseName: "BCA(P)" }, { Id: "B.A", CourseName: "B.A" }, { Id: "B.Com", CourseName: "B.Com" }, { Id: "B.Sc(BZC)", CourseName: "B.Sc(BZC)" }, { Id: "BAL", CourseName: "BA (L)" }, { Id: "B.Sc(MAT)", CourseName: "B.Sc(MAT)" }]
    $scope.Course = "";
   


    $scope.GenerateLongmemo = function () {
        $scope.loading = true;
        $scope.WarningMessage = "Generating Tabulation Report.It takes few minutes . please wait..";
        angular.element(document.getElementById('btnGnerate'))[0].disabled = true;
        var Gettabreport = LongService.GenerateLongmemo($scope.Course, $scope.Semister);
        Gettabreport.then(successCallback, errorCallback);

        function successCallback(response) {


            var result = respond.data;
            alert('Report generated ..!\n Path:' + respond);
            angular.element(document.getElementById('btnGnerate'))[0].disabled = false;
            $scope.loading = false;
        }

        function errorCallback(response) {
            alert('Data not found');
            angular.element(document.getElementById('btnGnerate'))[0].disabled = false;
            $scope.loading = false;
        };

    }


    $scope.CourseChange = function () {
        if ($scope.Course == "BA (L)") {

            $scope.Semister = 0;
            $("#ddlSem").prop('disabled', 'disabled');
            // $("#ddlSem")[0].Disable = true;

        } else {
            $("#ddlSem").prop('disabled', '');
        }

    }
});
