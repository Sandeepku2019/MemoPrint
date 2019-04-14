KuApp.controller('LoginController', function ($window,$scope, LoginService) {

    $scope.message = "Infrgistics";


    $scope.CheckLogin = function ()
    {
        debugger;
        GetAllEmployee();
    }
   

    function GetAllEmployee() {

        if ($scope.UserName == undefined || $scope.UserName == "") {

            alert("Please enter User Name");
            return;
        }

        if ($scope.Password == undefined || $scope.Password == "") {

            alert("Please enter Password");
            return;
        }

        if ($scope.UserName != "" && $scope.Password != "")
        {
            var getAllEmployee = LoginService.AuthenticateUser($scope.UserName, $scope.Password);
            getAllEmployee.then(successCallback,errorCallback);
            
            function successCallback(response) {
                if (response.data == true) {
                    $window.location.href = '/TabularReport/index';
                } else {
                    $scope.ErrorMessage = "Invalid User name/Password.";
                }
            }
            
            function errorCallback(response) {
                alert('Data not found');
            };

        }



        function RedirectHome()
        {

            var redirecthome = LoginService.RedirectReportHome();
            redirecthome.then(successCallback, errorCallback);

            function successCallback(response) {
                
            }

            function errorCallback() {
                alert('Data not found');
            };
        }
       

       
    }

});