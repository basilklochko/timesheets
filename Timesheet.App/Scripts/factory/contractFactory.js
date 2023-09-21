app.factory('contractFactory', function ($http, userFactory) {
    var factory = {};

    factory.getContracts = function () {
        return $http.get('../timesheets.api/api/clientconsultant/' + userFactory.getUser().id);
    };

    factory.addContract = function (clientId, consultantId) {
        var clientConsultant = {
            client: {
                id: clientId
            },
            consultant: {
                id: consultantId
            },
        };

        return $http.post('../timesheets.api/api/clientconsultant/', clientConsultant);
    };

    factory.delete = function (id) {
        return $http.delete('../timesheets.api/api/clientconsultant/' + id);
    };

    return factory;
});