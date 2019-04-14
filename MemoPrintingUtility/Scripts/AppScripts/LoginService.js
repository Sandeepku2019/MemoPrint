KuApp.service('LoginService', function ($http) {
  

    // Autheticate
    var getUserdetailsurl = "/Login/AuthenticateUser"
    this.AuthenticateUser = function (UserName,Password) {
       
        return result = $http({
            method: "Post",
            url: getUserdetailsurl,
            params: { UserName: UserName, Password: Password }

        });
    };


    ///Redect
    var rediecturl = "/Login/RedirectHome"
    this.RedirectReportHome = function () {

        debugger;
        return result = $http({
            method: "GET",
            url: rediecturl,
           // params: { UserName: UserName, Password: Password }

        });
    };

});
