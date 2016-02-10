(function () {
    'use strict';

    angular
        .module('logApp')
        .controller('logController', logController);

    logController.$inject = ['$scope','Logs'];

    function logController($scope, Logs) {
        $scope.title = 'logController';

        $scope.Logs = Logs.query();

        //activate();

        //function activate() { }
    }
})();
