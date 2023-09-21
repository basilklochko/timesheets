app.factory('securityFactory', function ($location, userFactory) {
    var factory = {};

    factory.auth = function (controller, type) {
        var path = $location.path();
        var authorized = true;

        switch (type) {
            case "Vendor":
                if (path == "/addtimesheet" || path.indexOf("/confirmdeletetimesheet/") > -1) {
                    authorized = false;
                }
                break;

            case "Client":
                if (controller == "clientController" || controller == "consultantController" || controller == "contractController") {
                    authorized = false;
                }
                if (path == "/addtimesheet" || path.indexOf("/confirmdeletetimesheet/") > -1) {
                    authorized = false;
                }
                break;

            case "Consultant":
                if (controller == "clientController" || controller == "consultantController" || controller == "contractController") {
                    authorized = false;
                }
                break;

            default:
        }

        if (!authorized) {
            $location.path('/');
        }
    };

    return factory;
});