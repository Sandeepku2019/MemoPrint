


KuApp.controller('MyController', function ($window, $scope, SubjctRangeService) {

   
    $("#dvYearWise").hide();
    $("#dvSemWise").hide();
    function BindSubjects() {

        var getAllSubjeects = SubjctRangeService.getSubjectdetail();
        getAllSubjeects.then(successCallback, errorCallback);

        function successCallback(response) {
            $scope.lstSubjectDetails = response.data;


        }

        function errorCallback(response) {
            alert('Data not found');
        };

    }
    $scope.GetaData = function () {

        if ($scope.SubjectRangeType == "Sem") {
            BindSubjects();

            $("#dvYearWise").hide();
            $("#dvSemWise").show();

        } else {

            BindSubjectsYrWise();

            $("#dvYearWise").show();
            $("#dvSemWise").hide();
        }
        
    }



    function BindSubjectsYrWise()
    {
        var getSubjectdetailYr = SubjctRangeService.getSubjectdetailYr();
        getSubjectdetailYr.then(successCallback, errorCallback);

        function successCallback(response) {
            $scope.lstSubjectDetails = response.data;


        }

        function errorCallback(response) {
            alert('Data not found');
        };

    }


    //// Save excel data to our database  
    $scope.GenerateRange = function () {


        var getAllGeneratedSubjeects = SubjctRangeService.Generate($scope.lstSubjectDetail, $scope.RangesStart, $scope.RangesGap);
        getAllGeneratedSubjeects.then(successCallback, errorCallback);

        function successCallback(response) {
            $scope.lstSubjectDetails = response.data;


        }

        function errorCallback(response) {
            alert('Data not found');
        };

    }




    $scope.Save = function () {


        var saveCodes = SubjctRangeService.SaveCodes($scope.lstSubjectDetail, $scope.RangesStart, $scope.RangesGap);
        saveCodes.then(successCallback, errorCallback);

        function successCallback(response) {
            alert('Data Saved Successfully..!');
        }

        function errorCallback(response) {
            alert('Data not found');
        };
    }






});