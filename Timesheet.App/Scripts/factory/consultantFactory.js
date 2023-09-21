app.factory('consultantFactory', function ($http, userFactory) {
    var factory = {};

    factory.getConsultants = function () {
        return $http.get('../timesheets.api/api/vendorconsultant/' + userFactory.getUser().id);
    };

    factory.save = function (vendorConsultant) {
        return $http.post('../timesheets.api/api/vendorconsultant/', vendorConsultant);
    };

    factory.delete = function (id) {
        return $http.delete('../timesheets.api/api/vendorconsultant/' + id);
    };

    return factory;
});