app.factory('cookieFactory', function ($cookies) {
    var factory = {};

    factory.set = function (name, value) {
        $cookies[name] = value;
    };

    factory.get = function (name) {
        return $cookies[name];
    };

    factory.remove = function (name) {
        delete $cookies[name];
    };

    return factory;
});