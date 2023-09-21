app.controller('homeController', function ($scope, $location, userFactory, uiFactory, clientFactory) {
    $scope.user = null;
    var timesheetsVisible = true;

    function init() {
        userFactory.init();

        $scope.user = userFactory.getUser();

        if ($scope.user.type == 'Consultant') {
            uiFactory.block();

            clientFactory.getClientsByConsultant().success(function (data) {
                if (data.length == 0) {
                    timesheetsVisible = false;
                }
            
                uiFactory.unblock();
            });
        }
    }

    $scope.timesheetsVisible = function () {
        return timesheetsVisible;
    }

    $scope.clients = function () {
        $location.path('clients');
    }

    $scope.consultants = function () {
        $location.path('consultants');
    }

    $scope.contracts = function () {
        $location.path('contracts');
    }

    $scope.timesheets = function () {
        $location.path('timesheets');
    }

    $scope.account = function () {
        $location.path('account');
    }

    $scope.password = function () {
        $location.path('password');
    }

    $scope.logout = function () {
        $location.path('logout');
    }

    init();
});