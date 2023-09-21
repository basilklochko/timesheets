app.controller('clientController', function ($scope, $routeParams, $location, uiFactory, userFactory, clientFactory, securityFactory) {
    function init() {
        userFactory.init();

        $scope.user = userFactory.getUser();

        securityFactory.auth('clientController', $scope.user.type);

        if ($location.path() == "/clients") {
            uiFactory.block();

            clientFactory.getClients().success(function (data) {
                if (data.length == 0) {
                    noClientsAlert.classList.remove('hidden');
                }
                else {
                    angular.forEach(data, function (value) {
                        value.Client.CreatedDTS = new Date(value.Client.CreatedDTS);
                    });

                    $scope.clients = data;
                }

                uiFactory.unblock();
            });
        }

        if ($location.path() == "/client/" + $scope.id) {
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
        $location.path('clients');
    }

    $scope.save = function () {
        if ($location.path() == '/addclient') {
            userAlert.classList.add('hidden');

            uiFactory.block();

            var vendorClient = {
                vendorid: userFactory.getUser().id,
                client: $scope.user
            };

            clientFactory.save(vendorClient).success(function (data) {
                uiFactory.unblock();

                if (data > 0) {
                    $location.path('/clients');
                }
                else {
                    $scope.userAlertMessage = "Client changes were not saved, please try again";
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
                    $location.path('clients');
                }
                else {
                    $scope.userAlertMessage = "Client changes were not saved (this email might be already registered in the system), please try again";
                    userAlert.classList.remove('hidden');
                }
            });
        }
    };

    $scope.modify = function (id) {
        $location.path('client/' + id);
    };

    $scope.confirmdelete = function (id) {
        $location.path('confirmdeleteclient/' + id);
    };

    $scope.delete = function (id) {
        userAlert.classList.add('hidden');

        uiFactory.block();

        clientFactory.delete(id).success(function (data) {
            uiFactory.unblock();

            if (data > 0) {
                $location.path('clients');
            }
            else {
                $scope.userAlertMessage = "Client was not deleted, please try again";
                userAlert.classList.remove('hidden');
            }
        });
    };   
});