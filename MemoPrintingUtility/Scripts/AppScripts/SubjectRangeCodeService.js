KuApp.service('SubjctRangeService', function ($http) {


    var getSubjectdetailsurl = "/SubjectRangeCode/GetData"
    this.getSubjectdetail = function () {

        return result = $http({
            method: "Post",
            url: getSubjectdetailsurl,


        });
    };



    var getAllGeneratedSubjeectsurl = "/SubjectRangeCode/GenerateRanges"
    this.Generate = function (lstSubjects, From, gap) {
                return result =$http({
                method: "POST",
                url: getAllGeneratedSubjeectsurl,
                data: JSON.stringify({ RangeFrom: From, Gap: gap }),
                headers: {
                    'Content-Type': 'application/json'
                }
            });
    };


    var SaveSubjectCode = "/SubjectRangeCode/SaveCodes"
    this.SaveCodes = function (lstSubjects, From, gap) {
        return result = $http({
            method: "POST",
            url: SaveSubjectCode,
            data: JSON.stringify({ lstSubjects: lstSubjects,RangeFrom: From, Gap: gap  }),
            headers: {
                'Content-Type': 'application/json'
            }
        });
    };



    // Year Subject range code

    var getSubjectdetailYrssurl = "/SubjectRangeCode/GetCodeDataYrs"
    this.getSubjectdetailYr = function () {

        return result = $http({
            method: "Post",
            url: getSubjectdetailYrssurl,


        });
    };



});