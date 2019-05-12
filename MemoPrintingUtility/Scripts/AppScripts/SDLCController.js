KuApp.controller('SDLCController', function ($window, $scope, SDLCService) {

    $scope.lstCourses = [{ Id: "ART", CourseName: "ART" }, { Id: "BBM", CourseName: "BBM" }, { Id: "BCM", CourseName: "BCM" }, { Id: "MPC", CourseName: "MPC" }];


    $scope.GenerateTabularReport = function () {
        $scope.loading = true;
        $scope.WarningMessage = "Generating Tabulation Report.It takes few minutes . please wait..";
        angular.element(document.getElementById('btnGnerate'))[0].disabled = true;

        var Gettabreport = SDLCService.GenerateTabulation($scope.Course, $scope.ReportType);
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