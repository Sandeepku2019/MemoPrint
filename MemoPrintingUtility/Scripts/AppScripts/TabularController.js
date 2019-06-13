KuApp.controller('TabularController', function ($window, $scope, TabularService) {

    $scope.lstSems = [{ Sem: "11", Semister: "1" }, { Sem: "12", Semister: "2" }, { Sem: "21", Semister: "3" }, { Sem: "22", Semister: "4" }, { Sem: "31", Semister: "5" }, { Sem: "32", Semister: "6" }]
    //BindCourses();

    $scope.lstCourses = [{ Id: "BCA(P)", CourseName: "BCA(P)" }, { Id: "B.A", CourseName: "B.A" }, { Id: "B.Com", CourseName: "B.Com" }, { Id: "B.Sc(BZC)", CourseName: "B.Sc(BZC)" }, { Id: "BAL", CourseName: "BA (L)" }, { Id: "B.Sc(MAT)", CourseName: "B.Sc(MAT)" }]

    $scope.lstTRTypes = [{ Id: "1", TrName: "Hor.Tabular " }, { Id: "2", TrName: "Ver.Tabular" }]
    $scope.Course = "";
    $scope.TrType = "";
    function BindCourses() {

        var getAllCourse = TabularService.getCourses();
        getAllCourse.then(successCallback, errorCallback);

        function successCallback(response) {
            //$scope.lstCourses = response.data;
        }

        function errorCallback(response) {
            alert('Data not found');
        };

    }


    $scope.GenerateTabularReport = function () {
        $scope.loading = true;
        $scope.WarningMessage = "Generating Tabulation Report.It takes few minutes . please wait..";
        angular.element(document.getElementById('btnGnerate'))[0].disabled = true;


        if ($scope.TrType == "1") {
            var Gettabreport = TabularService.GenerateTabulation($scope.Course, $scope.Semister);
            Gettabreport.then(successCallback, errorCallback);

            function successCallback(response) {


                var result = respond.data;
                alert('Report generated ..!\n Path:' + respond.data);
                angular.element(document.getElementById('btnGnerate'))[0].disabled = false;
                $scope.loading = false;
            }

            function errorCallback(response) {
                alert('Data not found');
                angular.element(document.getElementById('btnGnerate'))[0].disabled = false;
                $scope.loading = false;
            };
        }

        if ($scope.TrType == "2") {
            var GettabreportVR = TabularService.GenerateTabulationVR($scope.Course);
            GettabreportVR.then(successCallback, errorCallback);

            function successCallback(response) {


                var result = respond.data;
                alert('Report generated ..!\n Path:' + respond.data);
                angular.element(document.getElementById('btnGnerate'))[0].disabled = false;
                $scope.loading = false;
            }

            function errorCallback(response) {
                alert('Data not found');
                angular.element(document.getElementById('btnGnerate'))[0].disabled = false;
                $scope.loading = false;
            };
        }
    }


    $scope.CourseChange = function () {
        if ($scope.TrType == "1") {
            if ($scope.Course == "BA (L)") {

                $scope.Semister = 0;
                $("#ddlSem").prop('disabled', 'disabled');
                // $("#ddlSem")[0].Disable = true;

            } else {
                $("#ddlSem").prop('disabled', '');
            }
        } else {
            $scope.Semister = 0;
            $("#ddlSem").prop('disabled', 'disabled');
        }

    }


    $scope.TrChange = function () {
        if ($scope.TrType == "2") {

            $scope.Semister = 0;
            $("#ddlSem").prop('disabled', 'disabled');


        } else {
            $("#ddlSem").prop('disabled', '');
        }

    }
});
