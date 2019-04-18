KuApp.controller('ShortMemoController', function ($window, $scope, ShortMemoService) {

    $scope.lstSems = [{ Sem: "11", Semister: "1" }, { Sem: "21", Semister: "3" }, { Sem: "31", Semister: "5" }]
    //BindCourses();

    $scope.lstCourses = [{ Id: "B.A", CourseName: "B.A" }, { Id: "B.Com", CourseName: "B.Com" }, { Id: "B.Sc(BZC)", CourseName: "B.Sc(BZC)" }, { Id: "B.Sc(MAT)", CourseName: "B.Sc(MAT)" }]
    $scope.Course = "";

    function BindCourses() {

        var getAllCourse = ShortMemoService.getCourses();
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
        var Gettabreport = ShortMemoService.GenerateTabulation($scope.Course, $scope.Semister);
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
});