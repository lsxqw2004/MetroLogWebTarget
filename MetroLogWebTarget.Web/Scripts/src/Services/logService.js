(function () {
    'use strict';

    var logService = angular.module('logService', ['ngResource']);
    logService.factory('Logs', ['$resource',
        function($resource) {
            return $resource('/api/logs', {}, {
                query:{method:'GET', params : {}, isArray:true }
            });
        }]);



    //function logService($http) {
    //    var service = {
    //        getData: getData
    //    };

    //    return service;

    //    function getData() { }
    //}
})();