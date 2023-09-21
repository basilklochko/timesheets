app.controller('contractController', function ($scope, $routeParams, $location, uiFactory, userFactory, clientFactory, consultantFactory, contractFactory, securityFactory) {
    function init() {
        userFactory.init();

        $scope.user = userFactory.getUser();

        securityFactory.auth('contractController', $scope.user.type);

        if ($location.path() == "/contracts") {
            uiFactory.block();

            contractFactory.getContracts().success(function (data) {
                if (data.length == 0) {
                    noContractsAlert.classList.remove('hidden');
                }
                else {
                    //angular.forEach(data, function (value) {
                        //value.Client.CreatedDTS = new Date(value.Client.CreatedDTS);
                    //});

                    $scope.contracts = data;
                }

                uiFactory.unblock();
            });
        }

        if ($location.path() == "/addcontract") {
            uiFactory.block();

            clientFactory.getClients().success(function (data) {
                if (data.length > 0) {
                    $scope.clients = data;
                }

                uiFactory.unblock();
            });

            uiFactory.block();

            consultantFactory.getConsultants().success(function (data) {
                if (data.length > 0) {
                    $scope.consultants = data;
                }

                uiFactory.unblock();
            });
        }
    }

    init();

    if ($routeParams.id != undefined) {
        $scope.id = $routeParams.id;
    }

    $scope.selectClient = function ($event, clientId) {
        $('.row.client').removeClass('selected');
        $($event.target).parents('.row.client').addClass('selected');

        $("#clientId").val(clientId).change();
    }

    $scope.selectConsultant = function ($event, consultantId) {
        $('.row.consultant').removeClass('selected');
        $($event.target).parents('.row.consultant').addClass('selected');

        $("#consultantId").val(consultantId).change();        
    }

    $scope.addcontract = function () {
        uiFactory.block();
        
        contractAlert.classList.add('hidden');

        contractFactory.addContract($scope.clientId, $scope.consultantId).success(function (data) {
            if (data == 0) {
                $scope.contractAlertMessage = "Contract was not added (this association might be already created), please try again";
                contractAlert.classList.remove('hidden');
            }
            else {
                $location.path('contracts');
            }

            uiFactory.unblock();
        });
    }

    $scope.gohome = function () {
        $location.path('contracts');
    }

    $scope.confirmdelete = function (id) {
        $location.path('confirmdeletecontract/' + id);
    };

    $scope.delete = function (id) {
        userAlert.classList.add('hidden');

        uiFactory.block();

        contractFactory.delete(id).success(function (data) {
            uiFactory.unblock();

            if (data > 0) {
                $location.path('contracts');
            }
            else {
                $scope.userAlertMessage = "Contract was not deleted, please try again";
                userAlert.classList.remove('hidden');
            }
        });
    };
});