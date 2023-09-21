app.factory('userFactory', function ($http, $location, cookieFactory) {
    var factory = {};

    factory.remind = function (email) {
        return $http.get('../timesheets.api/api/password/?email=' + email);
    };

    factory.login = function (email, password) {
        return $http.get('../timesheets.api/api/login/?email=' + email + "&password=" + password);
    };

    factory.getUser = function () {
        var result = null;
        var cookie = cookieFactory.get('timesheet.auth');

        if (cookie != undefined) {
            var arr = cookie.split('|');

            result = {
                id: Number(arr[0]),
                name: arr[1],
                email: arr[2],
                type: arr[3]
            };
        }

        return result;
    };

    factory.init = function () {
        if ($location.path() == "/login" || $location.path() == "/remind" || $location.path() == '/register') {
            return;
        }

        var user = this.getUser();

        if (user == null) {
            $location.path('/login');
        }
    };

    factory.getUserById = function (id) {
        return $http.get('../timesheets.api/api/user/' + id);
    };

    factory.getAccount = function () {
        return $http.get('../timesheets.api/api/user/' + this.getUser().id);
    };

    factory.save = function (user) {
        if (user.id == undefined || user.id == null || user.id == 0) {
            var authUser = this.getUser();
            user.id = authUser == null ? 0 : authUser.id;
        }

        return $http.post('../timesheets.api/api/user/', user);
    };

    factory.changePassword = function (password) {
        var user = this.getUser();

        var data = {
            id: user.id,
            password: password
        };

        return $http.post('../timesheets.api/api/password/', data);
    };

    factory.loginWithToken = function (token) {
        return $http.get('../timesheets.api/api/login/?token=' + token);
    };

    factory.emailValidity = function(email) {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    };

    return factory;
});