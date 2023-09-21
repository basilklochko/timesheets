app.controller('userController', function ($scope, $location, uiFactory, cookieFactory, userFactory) {
    function init() {
        if ($location.path() == "/login" || $location.path() == "/remind" || $location.path() == "/register") {
            cookieFactory.remove('timesheet.auth');
            mymenu.classList.add("hidden");
        }

        if ($location.path() == "/logout") {
            cookieFactory.remove('timesheet.auth');
            $location.path('login');
        }

        userFactory.init();

        $scope.user = {};

        if ($location.path() == "/account") {
            uiFactory.block();

            userFactory.getAccount().success(function (data) {
                $scope.user = {
                    userName: data.UserName,
                    email: data.Email,
                    address: data.Address,
                    phone: data.Phone,
                    fax: data.Fax,
                    url: data.Url,
                    taxCode: data.TaxCode,
                    contact: data.Contact,
                    logo: data.Logo
                };

                uiFactory.unblock();
            });
        }
    }

    $scope.loginFormValid = function() {
        return !loginForm.checkValidity() || !userFactory.emailValidity(email.value);
    }

    $scope.remindFormValid = function () {
        return !userFactory.emailValidity(email.value);
    }

    $scope.userFormValid = function () {
        return !userForm.checkValidity() || !userFactory.emailValidity(email.value);
    }

    $scope.gologin = function () {
        $location.path('login');
    };

    $scope.gohome = function () {
        $location.path('/');
    };

    $scope.remind = function () {
        remindAlert.classList.add('hidden');
        remindAlert.classList.remove('alert-danger');
        remindAlert.classList.remove('alert-info');

        uiFactory.block();

        userFactory.remind($scope.email).success(function (data) {
            uiFactory.unblock();

            if (data == 0) {
                $scope.remindAlertMessage = "Email was not found";
                remindAlert.classList.add('alert-danger');
            }
            else {
                $scope.remindAlertMessage = "Email with password was sent";
                remindAlert.classList.add('alert-info');
            }

            remindAlert.classList.remove('hidden');
        });
    };

    $scope.login = function () {
        loginAlert.classList.add('hidden');

        uiFactory.block();

        userFactory.login($scope.email, $scope.password).success(function (data) {
            uiFactory.unblock();

            if (data == "") {
                $scope.loginAlertMessage = "Email was not found or incorrect password";
                loginAlert.classList.remove('hidden');

                return;
            }

            cookieFactory.set('timesheet.auth', data);

            $('#userName').text(unescape(data.split('|')[1]));

            mymenu.classList.remove("hidden");

            $location.path('/');
        });
    };

    $scope.save = function () {
        userAlert.classList.add('hidden');

        uiFactory.block();

        userFactory.save($scope.user).success(function (data) {
            uiFactory.unblock();

            if (data > 0) {
                $location.path('/');
            }
            else {
                $scope.userAlertMessage = "User changes were not saved (this email might be already registered in the system), please try again";
                userAlert.classList.remove('hidden');
            }
        });
    };

    $scope.changepassword = function () {
        passwordAlert.classList.add('hidden');

        uiFactory.block();

        userFactory.changePassword($scope.password).success(function (data) {
            uiFactory.unblock();

            if (data > 0) {
                $location.path('/');
            }
            else {
                $scope.passwordAlertMessage = "Password was not changed, please try again";
                passwordAlert.classList.remove('hidden');
            }
        });
    };

    init();
});