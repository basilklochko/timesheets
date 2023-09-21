app.controller('consultantController', function ($scope, $routeParams, $location, uiFactory, userFactory, consultantFactory, securityFactory) {
    function init() {
        userFactory.init();

        $scope.user = userFactory.getUser();

        securityFactory.auth('consultantController', $scope.user.type);

        if ($location.path() == "/consultants") {
            uiFactory.block();

            consultantFactory.getConsultants().success(function (data) {
                if (data.length == 0) {
                    noConsultantsAlert.classList.remove('hidden');
                }
                else {
                    angular.forEach(data, function (value) {
                        value.Consultant.CreatedDTS = new Date(value.Consultant.CreatedDTS);
                    });

                    $scope.consultants = data;
                }

                uiFactory.unblock();
            });
        }

        if ($location.path() == "/consultant/" + $scope.id) {
            uiFactory.block();

            userFactory.getUserById($scope.id).success(function (data) {
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

    if ($routeParams.id != undefined) {
        $scope.id = $routeParams.id;
    }

    init();

    $scope.gohome = function () {
        $location.path('consultants');
    }

    $scope.save = function () {
        if ($location.path() == '/addconsultant') {
            userAlert.classList.add('hidden');

            uiFactory.block();

            var vendorConsultant = {
                vendorid: userFactory.getUser().id,
                consultant: $scope.user
            };

            consultantFactory.save(vendorConsultant).success(function (data) {
                uiFactory.unblock();

                if (data > 0) {
                    $location.path('consultants');
                }
                else {
                    $scope.userAlertMessage = "Consultant changes were not saved, please try again";
                    userAlert.classList.remove('hidden');
                }
            });
        }
        else {
            userAlert.classList.add('hidden');

            uiFactory.block();

            $scope.user.id = $scope.id;

            userFactory.save($scope.user).success(function (data) {
                uiFactory.unblock();

                if (data > 0) {
                    $location.path('consultants');
                }
                else {
                    $scope.userAlertMessage = "Consultant changes were not saved (this email might be already registered in the system), please try again";
                    userAlert.classList.remove('hidden');
                }
            });
        }
    };

    $scope.modify = function (id) {
        $location.path('consultant/' + id);
    };

    $scope.confirmdelete = function (id) {
        $location.path('confirmdeleteconsultant/' + id);
    };

    $scope.delete = function (id) {
        userAlert.classList.add('hidden');

        uiFactory.block();

        consultantFactory.delete(id).success(function (data) {
            uiFactory.unblock();

            if (data > 0) {
                $location.path('consultants');
            }
            else {
                $scope.userAlertMessage = "Consultant was not deleted, please try again";
                userAlert.classList.remove('hidden');
            }
        });
    };
});