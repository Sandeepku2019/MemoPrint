


KuApp.controller('MyController', function ($window, $scope, SubjctRangeService) {

   
    $("#dvYearWise").hide();
    $("#dvSemWise").hide();

    $scope.SubjectHeadCount = 0;
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
            $scope.lstSubjectDetails = response.data.lstSubjectDetails;
            $scope.SubjectHeadCount = response.data.count;

        }

        function errorCallback(response) {
            alert('Data not found');
        };

    }



    function GenerateRandeCodeSemWise()
    {
        var getAllGeneratedSubjeects = SubjctRangeService.Generate($scope.lstSubjectDetail, $scope.RangesStart, $scope.RangesGap);
        getAllGeneratedSubjeects.then(successCallback, errorCallback);

        function successCallback(response) {
            $scope.lstSubjectDetails = response.data;


        }

        function errorCallback(response) {
            alert('Data not found');
        };
    }



    function GenerateRandeCodeYearWise() {
        var GenerateYRWise = SubjctRangeService.GenerateYRWise($scope.RangesStart, $scope.RangesGap);
        GenerateYRWise.then(successCallback, errorCallback);

        function successCallback(response) {
            $scope.lstSubjectDetails = response.data.lstSubjectDetails;
            $scope.SubjectHeadCount = response.data.count;

        }

        function errorCallback(response) {
            alert('Data not found');
        };
    }


    //// Save excel data to our database  
    $scope.GenerateRange = function () {


        if ($scope.SubjectRangeType == "Sem") {
            GenerateRandeCodeSemWise();

            $("#dvYearWise").hide();
            $("#dvSemWise").show();

        } else {

            GenerateRandeCodeYearWise();

            $("#dvYearWise").show();
            $("#dvSemWise").hide();
        }
    }




    $scope.Save = function () {

        if ($scope.SubjectRangeType == "Sem") {
            
            SaveSubjectRangeCodeSem();
            $("#dvYearWise").hide();
            $("#dvSemWise").show();

        } else {

          
SaveSubjectRangeCodeYear();
        $("#dvYearWise").show();
        $("#dvSemWise").hide();
    }


     
    }

    function SaveSubjectRangeCodeSem()
    {
        var saveCodes = SubjctRangeService.SaveCodes($scope.lstSubjectDetail, $scope.RangesStart, $scope.RangesGap);
        saveCodes.then(successCallback, errorCallback);

        function successCallback(response) {
            alert('Data Saved Successfully..!');
        }

        function errorCallback(response) {
            alert('Data not found');
        };
    }



    function SaveSubjectRangeCodeYear() {
        var SaveYrsCodes = SubjctRangeService.SaveYrsCodes($scope.RangesStart, $scope.RangesGap);
        SaveYrsCodes.then(successCallback, errorCallback);

        function successCallback(response) {
            alert('Data Saved Successfully..!');
        }

        function errorCallback(response) {
            alert('Data not found');
        };
    }




});