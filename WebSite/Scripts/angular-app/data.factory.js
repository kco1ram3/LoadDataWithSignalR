myApp.factory('dataFactory', ['$http', function ($http) {

    var urlBase = 'http://localhost:4184/api';
    var dataFactory = {};

    dataFactory.search = function (request, callback) {
        $http.post(urlBase + '/Data/Search', request).then(function (response) {
            callback(response.data);
        });;
    };

    return dataFactory;

}]);