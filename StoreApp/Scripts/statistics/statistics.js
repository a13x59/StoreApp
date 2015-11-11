'use strict';

var myApp = angular.module('myApp', []);
myApp.controller('ProductsCtrl', ['$scope', function ($scope) {
    window.qwe = $scope.model = {
        selectedProduct: null,
        isLoading: false,
        products: []
    };

        $scope.selectProduct = function (product) {
            var model = $scope.model;

            model.selectedProduct = product;
            //model.operations.items = [];
            //angular.copy(model.selectedUser, model.tempUserObj);
        };

        $scope.updateProductsList = function () {
            //var model = $scope.model;
            //model.products = [];
            //$scope.model.products = [];
            $scope.model.isLoading = true;

            $.ajax({
                url: 'Statistics/GetProductsMaterialsSum',
                method: 'POST',
                success: function (res) {
                    $scope.model.products = res;
                    $scope.model.isLoading = false;
                },
                error: function (res) {
                    alert('error');
                    console.log(res);
                }
            });
        };

        $scope.makeProduct = function () {
            var model = $scope.model;

            $.ajax({
                url: 'MakeProduct',
                method: 'POST',
                data: JSON.stringify({ id: model.selectedProduct.id }),
                success: function (res) {
                    //$scope.products = res.products;
                    model.isLoading = false;
                },
                error: function (res) {
                    alert('error');
                    console.log(res);
                }
            });
        };

        $scope.updateProductsList();
}]);