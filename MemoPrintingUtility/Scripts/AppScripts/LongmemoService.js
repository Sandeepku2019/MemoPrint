KuApp.service('LongService', function ($http) {

    var GenerateLongmemoUrl = "/LongMemo/GenerateLongmemo"
    this.GenerateLongmemo = function (Course) {

        return result = $http({
            method: "Post",
            url: GenerateLongmemoUrl,
            params: { course: Course }

        });
    };

});