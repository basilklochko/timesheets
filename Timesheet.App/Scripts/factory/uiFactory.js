app.factory('uiFactory', function () {
    var factory = {};

    factory.block = function () {
        $('#mainplaceholder').fadeTo('slow', .4);
        $('#ajax').removeClass("hidden");
    }

    factory.unblock = function () {
        $('#mainplaceholder').fadeTo('slow', 1);
        $('#ajax').addClass("hidden");
    }

    return factory;
});