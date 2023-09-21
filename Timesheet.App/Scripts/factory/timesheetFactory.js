app.factory('timesheetFactory', function ($http, $window, userFactory) {
    var factory = {};

    factory.getTimesheets = function (clientId) {
        if (clientId == undefined) {
            clientId = '';
        }

        return $http.get('../timesheets.api/api/timesheet/?userId=' + userFactory.getUser().id + "&clientId=" + clientId);
    };

    factory.getTimesheet = function (id) {
        return $http.get('../timesheets.api/api/timesheet/' + id);
    };

    factory.save = function (timesheet) {
        return $http.post('../timesheets.api/api/timesheet/', timesheet);
    };

    factory.delete = function (id) {
        return $http.delete('../timesheets.api/api/timesheet/' + id);
    };

    factory.changeStatus = function (id, status) {
        return $http.post('../timesheets.api/api/timesheet/' + id + "?status=" + status);
    };

    return factory;
});