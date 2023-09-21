app.factory('clientFactory', function ($http, userFactory) {
    var factory = {};

    factory.getClients = function () {
        return $http.get('../timesheets.api/api/vendorclient/' + userFactory.getUser().id)
    };

    factory.getClientsByConsultant = function () {
        return $http.get('../timesheets.api/api/clientbyconsultant/' + userFactory.getUser().id)
    };

    factory.save = function (vendorClient) {
        return $http.post('../timesheets.api/api/vendorclient/', vendorClient);
    };

    factory.delete = function (id) {
        return $http.delete('../timesheets.api/api/vendorclient/' + id);
    };

    return factory;
});